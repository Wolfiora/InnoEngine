using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.IO;
using Inno.Build.Toolchains;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace Inno.Build.Tests;

[Collection("Build pipeline serialization")]
public sealed class DevelopmentBuildWorkerTests
{
    [Theory]
    [InlineData(true, false)]
    [InlineData(false, false)]
    [InlineData(false, true)]
    public async Task CrashedTerminatedOrExpiredWorkersPreserveThePreviousPublication(
        bool crash,
        bool deadline
    ) {
        string root = Path.Combine(Path.GetTempPath(), "InnoWorkerFailure", Guid.NewGuid().ToString("N"));
        try
        {
            Directory.CreateDirectory(root);
            File.WriteAllText(Path.Combine(root, "InnoEngine.sln"), "fixture");
            string assembly = Path.Combine(root, "artifacts", "build-tools", "cli", "workspaces", "fixture", "Outputs", "Inno.Build.Cli.dll");
            Directory.CreateDirectory(Path.GetDirectoryName(assembly)!);
            string previous = Path.Combine(Path.GetDirectoryName(assembly)!, "previous-publication");
            File.WriteAllText(previous, "complete");
            string code = "using System; using System.IO; using System.Threading; using System.Threading.Tasks; "
                + "namespace Inno.Build.Toolchains; public static class ManagedToolRuntime { "
                + "public static async Task<string> ReadInputIdentityAsync(string root, string configuration, string dotnet, string project, string cacheName, CancellationToken token) { "
                + "File.WriteAllText(Path.Combine(root, \"worker-started\"), \"ready\"); "
                + (crash ? "Environment.Exit(23); " : "await Task.Delay(Timeout.Infinite, token); ")
                + "return \"unexpected\"; } }";
            string[] framework = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator);
            CSharpCompilation compilation = CSharpCompilation.Create("Inno.Build.Toolchains", [CSharpSyntaxTree.ParseText(code)],
                framework.Select(static path => MetadataReference.CreateFromFile(path)), new(OutputKind.DynamicallyLinkedLibrary));
            using (FileStream output = File.Create(assembly))
                Assert.True(compilation.Emit(output).Success);
            File.Copy(assembly, Path.Combine(Path.GetDirectoryName(assembly)!, "Inno.Build.Toolchains.dll"));
            using (await FileLease.AcquireAsync(Path.GetDirectoryName(Path.GetDirectoryName(assembly))! + ".read.lock", TimeSpan.FromSeconds(5))) { }
            string workers = Path.Combine(root, "artifacts", "entry", "workers");
            using ProcessOwnedDirectory working = await ProcessOwnedDirectory.CreateAsync(workers);
            string request = Path.Combine(working.directory, "request.json");
            string nonce = Guid.NewGuid().ToString("N");
            DotNetSdkDescriptor sdk = await DotNetSdkResolver.ResolveAsync("dotnet",
                Path.Combine(ToolchainEnvironment.FindRepoRoot(), "entry", "InnoEngine.Build", "InnoEngine.Build.csproj"), CancellationToken.None);
            await File.WriteAllBytesAsync(request, JsonSerializer.SerializeToUtf8Bytes(new
            {
                root, configuration = "Debug", dotnet = sdk.hostPath, assembly, operation = "inspect", nonce, tool = "cli"
            }));
            using Process process = new() { StartInfo = new ProcessStartInfo(sdk.hostPath)
            {
                UseShellExecute = false, CreateNoWindow = true, RedirectStandardError = true
            } };
            foreach (string argument in new[]
            {
                Path.Combine(ToolchainEnvironment.FindRepoRoot(), "entry", "InnoEngine.Build", "bin", "Debug", "net9.0", "InnoEngine.Build.dll"),
                "--internal-build-worker", request, nonce
            })
                process.StartInfo.ArgumentList.Add(argument);
            Assert.True(process.Start());
            Task<string> errors = process.StandardError.ReadToEndAsync();
            using var timeout = new CancellationTokenSource(deadline ? TimeSpan.FromMinutes(12) : TimeSpan.FromSeconds(30));
            try
            {
                while (!File.Exists(Path.Combine(root, "worker-started")))
                {
                    Assert.False(process.HasExited, process.HasExited ? await errors : string.Empty);
                    await Task.Delay(10, timeout.Token);
                }
                if (!crash && !deadline)
                {
                    Assert.Equal(ProcessOwnerState.Active, Assert.Single(ProcessOwnedDirectory.ReadClaims(workers)).ownerState);
                    Assert.True(Directory.Exists(working.directory));
                    process.Kill(entireProcessTree: true);
                }
                await process.WaitForExitAsync(timeout.Token);
            }
            finally
            {
                if (!process.HasExited)
                {
                    process.Kill(entireProcessTree: true);
                    await process.WaitForExitAsync();
                }
            }
            Assert.NotEqual(0, process.ExitCode);
            Assert.False(File.Exists(Path.Combine(working.directory, "response.json")));
            Assert.Equal("complete", File.ReadAllText(previous));
            string diagnostic = await errors;
            if (deadline)
                Assert.Contains("CanceledException", diagnostic);
            else
                Assert.Empty(diagnostic);
            working.Dispose();
            Assert.Empty(ProcessOwnedDirectory.ReadClaims(workers));
        }
        finally
        {
            if (Directory.Exists(root))
                Directory.Delete(root, recursive: true);
        }
    }

    [Fact]
    public async Task TenRealInspectionsExitWithoutRetainingTheToolsInTheParent()
    {
        string root = ToolchainEnvironment.FindRepoRoot();
        string receipt = Path.Combine(root, "artifacts", "entry", "build", "cli", "Debug", "ready.json");
        using JsonDocument ready = JsonDocument.Parse(File.ReadAllBytes(receipt));
        string assembly = ready.RootElement.GetProperty("assembly").GetString()!;
        string publication = Path.GetDirectoryName(Path.GetDirectoryName(assembly))!;
        using BuildArtifactReadLease reading = await BuildArtifactReadLease.AcquireAsync(publication, Path.GetFileName(publication), ["Outputs"]);
        DotNetSdkDescriptor sdk = await DotNetSdkResolver.ResolveAsync("dotnet",
            Path.Combine(root, "entry", "InnoEngine.Build", "InnoEngine.Build.csproj"), CancellationToken.None);
        string entry = Path.Combine(root, "entry", "InnoEngine.Build", "bin", "Debug", "net9.0", "InnoEngine.Build.dll");
        using var cancellation = new CancellationTokenSource(TimeSpan.FromMinutes(10));
        string? previous = null;
        for (int index = 0; index < 10; index++)
        {
            using ProcessOwnedDirectory working = await ProcessOwnedDirectory.CreateAsync(Path.Combine(root, "artifacts", "entry", "workers"), cancellation.Token);
            string request = Path.Combine(working.directory, "request.json");
            string nonce = Guid.NewGuid().ToString("N");
            await File.WriteAllBytesAsync(request, JsonSerializer.SerializeToUtf8Bytes(new
            {
                root, configuration = "Debug", dotnet = sdk.hostPath, assembly, operation = "inspect", nonce, tool = "cli"
            }), cancellation.Token);
            using Process process = new() { StartInfo = new ProcessStartInfo(sdk.hostPath)
            {
                UseShellExecute = false, CreateNoWindow = true, RedirectStandardError = true, RedirectStandardOutput = true
            } };
            foreach (string argument in new[] { entry, "--internal-build-worker", request, nonce })
                process.StartInfo.ArgumentList.Add(argument);
            Assert.True(process.Start());
            Task<string> errors = process.StandardError.ReadToEndAsync(cancellation.Token);
            Task<string> output = process.StandardOutput.ReadToEndAsync(cancellation.Token);
            try
            {
                await process.WaitForExitAsync(cancellation.Token);
            }
            catch
            {
                if (!process.HasExited)
                    process.Kill(entireProcessTree: true);
                await process.WaitForExitAsync();
                throw;
            }
            Assert.Equal(0, process.ExitCode);
            Assert.True(string.IsNullOrEmpty(await errors), await output);
            using JsonDocument response = JsonDocument.Parse(File.ReadAllBytes(Path.Combine(working.directory, "response.json")));
            string current = response.RootElement.GetProperty("result").GetString()!;
            Assert.Equal(nonce, response.RootElement.GetProperty("nonce").GetString());
            if (previous is not null)
                Assert.Equal(previous, current);
            previous = current;
        }
        // The fixture calls only the process entry; it never loads the CLI publication into this parent.
        Assert.DoesNotContain(AppDomain.CurrentDomain.GetAssemblies(), loaded => !loaded.IsDynamic && loaded.Location == assembly);
    }

    [Fact]
    public async Task ThirtyDefaultJitWorkerLifetimesReturnResultsAndRejectInvalidNonce()
    {
        string root = Path.Combine(Path.GetTempPath(), "InnoWorkerFixture", Guid.NewGuid().ToString("N"));
        try
        {
            Directory.CreateDirectory(root);
            File.WriteAllText(Path.Combine(root, "InnoEngine.sln"), "fixture");
            string assembly = Path.Combine(root, "artifacts", "build-tools", "cli", "workspaces", "fixture", "Inno.Build.Cli.dll");
            Directory.CreateDirectory(Path.GetDirectoryName(assembly)!);
            string code = "using System.Threading; using System.Threading.Tasks; namespace Inno.Build.Toolchains; public static class ManagedToolRuntime { "
                + "public static Task<string> ReadInputIdentityAsync(string root, string configuration, string dotnet, string project, string cacheName, CancellationToken token) => Task.FromResult(\"fixture-identity\"); "
                + "public static Task<string> PublishRuntimeAsync(string root, string configuration, string dotnet, string assembly, string project, string cacheName, CancellationToken token) => Task.FromResult(assembly); }";
            string[] framework = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator);
            CSharpCompilation compilation = CSharpCompilation.Create("Inno.Build.Toolchains", [CSharpSyntaxTree.ParseText(code)],
                framework.Select(static path => MetadataReference.CreateFromFile(path)), new(OutputKind.DynamicallyLinkedLibrary));
            using (FileStream output = File.Create(assembly))
                Assert.True(compilation.Emit(output).Success);
            File.Copy(assembly, Path.Combine(Path.GetDirectoryName(assembly)!, "Inno.Build.Toolchains.dll"));
            string entry = Path.Combine(ToolchainEnvironment.FindRepoRoot(), "entry", "InnoEngine.Build", "bin", "Debug", "net9.0", "InnoEngine.Build.dll");
            DotNetSdkDescriptor sdk = await DotNetSdkResolver.ResolveAsync("dotnet",
                Path.Combine(ToolchainEnvironment.FindRepoRoot(), "entry", "InnoEngine.Build", "InnoEngine.Build.csproj"), CancellationToken.None);
            string dotnet = sdk.hostPath;
            using (await FileLease.AcquireAsync(Path.GetDirectoryName(Path.GetDirectoryName(assembly))! + ".read.lock", TimeSpan.FromSeconds(10))) { }
            Assert.True(File.Exists(entry));
            string workers = Path.Combine(root, "artifacts", "entry", "workers");
            using var cancellation = new CancellationTokenSource(TimeSpan.FromMinutes(2));
            for (int index = 0; index < 31; index++)
            {
                using ProcessOwnedDirectory working = await ProcessOwnedDirectory.CreateAsync(workers, cancellation.Token);
                string nonce = Guid.NewGuid().ToString("N");
                string operation = index % 2 == 0 ? "inspect" : "publish";
                string request = Path.Combine(working.directory, "request.json");
                await File.WriteAllBytesAsync(request, JsonSerializer.SerializeToUtf8Bytes(new { root, configuration = "Debug", dotnet, assembly, operation, nonce, tool = "cli" }), cancellation.Token);
                using Process process = new() { StartInfo = new ProcessStartInfo(dotnet) { UseShellExecute = false, CreateNoWindow = true, RedirectStandardError = true } };
                process.StartInfo.ArgumentList.Add(entry);
                process.StartInfo.ArgumentList.Add("--internal-build-worker");
                process.StartInfo.ArgumentList.Add(request);
                process.StartInfo.ArgumentList.Add(index == 30 ? new string('a', 32) : nonce);
                Assert.True(process.Start());
                Task<string> errors = process.StandardError.ReadToEndAsync(cancellation.Token);
                try { await process.WaitForExitAsync(cancellation.Token); }
                catch { if (!process.HasExited) process.Kill(entireProcessTree: true); await process.WaitForExitAsync(); throw; }
                string diagnostic = await errors;
                if (index == 30)
                {
                    Assert.NotEqual(0, process.ExitCode);
                    Assert.False(File.Exists(Path.Combine(working.directory, "response.json")));
                }
                else
                {
                    Assert.Equal(0, process.ExitCode);
                    Assert.True(string.IsNullOrEmpty(diagnostic), diagnostic);
                    using JsonDocument response = JsonDocument.Parse(File.ReadAllBytes(Path.Combine(working.directory, "response.json")));
                    Assert.Equal(nonce, response.RootElement.GetProperty("nonce").GetString());
                    Assert.Equal(operation == "inspect" ? "fixture-identity" : assembly, response.RootElement.GetProperty("result").GetString());
                }
            }
            Assert.Empty(ProcessOwnedDirectory.ReadClaims(workers));
        }
        finally
        {
            if (Directory.Exists(root))
                Directory.Delete(root, recursive: true);
        }
    }
}
