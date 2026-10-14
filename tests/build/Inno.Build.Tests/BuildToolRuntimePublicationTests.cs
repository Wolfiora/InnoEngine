using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Cli;
using Inno.Build.Toolchains;
using Inno.Core.IO;
using Xunit;

namespace Inno.Build.Tests;

[Collection("Build pipeline serialization")]
public sealed class BuildToolRuntimePublicationTests
{
    [Fact]
    public async Task PublicationRestoresDeclaredAuxiliaryInputsBeforeFreezingTheGraph()
    {
        string root = Path.Combine(Path.GetTempPath(), "InnoToolInputRestore", Guid.NewGuid().ToString("N"));
        try
        {
            Directory.CreateDirectory(Path.Combine(root, "Tool"));
            Directory.CreateDirectory(Path.Combine(root, "Auxiliary"));
            File.WriteAllText(Path.Combine(root, "InnoEngine.sln"), "fixture");
            File.WriteAllText(Path.Combine(root, "Tool", "Tool.csproj"),
                "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>net9.0</TargetFramework></PropertyGroup>"
                + "<ItemGroup><InnoBuildInputProject Include=\"../Auxiliary/Auxiliary.csproj\" /></ItemGroup></Project>");
            File.WriteAllText(Path.Combine(root, "Tool", "Value.cs"), "public class Value {}");
            File.WriteAllText(Path.Combine(root, "Auxiliary", "Auxiliary.csproj"),
                "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>net9.0</TargetFramework></PropertyGroup></Project>");
            File.WriteAllText(Path.Combine(root, "Auxiliary", "Input.cs"), "public class Input {}");
            string host = ToolchainEnvironment.ResolveExecutable("dotnet");
            using ProcessOwnedDirectory bootstrap = await ProcessOwnedDirectory.CreateAsync(Path.Combine(root, "artifacts", "build-tools", "fixture", "workspaces"));
            await ToolchainEnvironment.RunAsync(host,
                ["build", Path.Combine(root, "Tool", "Tool.csproj"), "--output", bootstrap.directory, "--disable-build-servers", "-m:1", "-nodeReuse:false"],
                root, CancellationToken.None, DotNetSdkEnvironment.Create(host));
            string assets = Path.Combine(root, "Auxiliary", "obj", "project.assets.json");
            Assert.False(File.Exists(assets));
            string published = await ManagedToolRuntime.PublishRuntimeAsync(root, "Debug", host,
                Path.Combine(bootstrap.directory, "Tool.dll"), "Tool/Tool.csproj", "fixture", CancellationToken.None);
            Assert.True(File.Exists(assets));
            Assert.True(File.Exists(published));
        }
        finally
        {
            if (Directory.Exists(root))
                Directory.Delete(PathBoundary.RequireUnlinkedPath(Path.GetTempPath(), root), recursive: true);
        }
    }

    [Fact]
    public async Task RealToolInputsRemainStableAcrossFreshGraphEvaluations()
    {
        string root = ToolchainEnvironment.FindRepoRoot();
        string project = Path.Combine(root, "build", "cli", "Inno.Build.Cli", "Inno.Build.Cli.csproj");
        DotNetSdkDescriptor sdk = await DotNetSdkResolver.ResolveAsync("dotnet", project, CancellationToken.None);
        ManagedBuildInputSnapshot snapshot = await ManagedBuildInputSnapshot.CaptureAsync(new NativeBuildContext(root, "debug"),
            sdk, project, new Dictionary<string, string> { ["Configuration"] = "Debug", ["InnoBuildToolProject"] = "Inno.Build.Cli" });
        await snapshot.VerifyAsync();
    }

    [Fact]
    public async Task PreparedToolLoadsAnImmutableDeploymentAndLeavesSdkOutputWritable()
    {
        string root = ToolchainEnvironment.FindRepoRoot();
        string owner = Path.Combine(root, "artifacts", "build-tools", "cli", "workspaces", Guid.NewGuid().ToString("N"));
        try
        {
            string output = Path.Combine(owner, "Runtime");
            string host = ToolchainEnvironment.ResolveExecutable("dotnet");
            using var cancellation = new CancellationTokenSource(TimeSpan.FromMinutes(15));
            await ToolchainEnvironment.RunAsync(host,
                ["build", Path.Combine(root, "build", "cli", "Inno.Build.Cli", "Inno.Build.Cli.csproj"),
                    "-p:BuildProjectReferences=false", "-p:InnoBuildToolProject=Inno.Build.Cli", "-p:InnoBuildToolOutputPath=" + output + Path.DirectorySeparatorChar,
                    "-nodeReuse:false", "-v:quiet"],
                root, cancellation.Token, DotNetSdkEnvironment.Create(host));
            string source = Path.Combine(output, "net9.0", "Inno.Build.Cli.dll");
            string prepared = await ManagedToolRuntime.PublishRuntimeAsync(root, "Debug", host, source, "build/cli/Inno.Build.Cli/Inno.Build.Cli.csproj", "cli", cancellation.Token);
            string publication = Directory.GetParent(Path.GetDirectoryName(prepared)!)!.FullName;
            using FileLease activeReader = await FileLease.AcquireSharedAsync(publication + ".read.lock",
                TimeSpan.FromSeconds(10), cancellation.Token);
            string repeated = await ManagedToolRuntime.PublishRuntimeAsync(root, "Debug", host, source, "build/cli/Inno.Build.Cli/Inno.Build.Cli.csproj", "cli", cancellation.Token);
            Assert.Equal(prepared, repeated);
            Assert.NotEqual(source, prepared);
            Assert.Contains(Path.Combine("cli", "hosts"), prepared);
            Assert.Equal(AssemblyName.GetAssemblyName(source).FullName, AssemblyName.GetAssemblyName(prepared).FullName);
            WeakReference monitor = LoadAndVerifySourceWritable(prepared, source);
            for (int attempt = 0; attempt < 10 && monitor.IsAlive; attempt++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                Thread.Sleep(10);
            }
            Assert.False(monitor.IsAlive);
        }
        finally
        {
            if (Directory.Exists(owner))
                Directory.Delete(PathBoundary.RequireUnlinkedPath(Path.Combine(root, "artifacts", "build-tools", "cli", "workspaces"), owner), recursive: true);
        }
    }

    [Fact]
    public async Task PreCanceledPublicationDoesNotDiscoverSdkOrCreateState()
    {
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => ManagedToolRuntime.PublishRuntimeAsync(
            "unavailable-checkout", "Debug", "unavailable-sdk", "unavailable-assembly", "build/cli/Inno.Build.Cli/Inno.Build.Cli.csproj", "cli", cancellation.Token));
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static WeakReference LoadAndVerifySourceWritable(
        string publication,
        string source
    ) {
        var load = new AssemblyLoadContext("Build-tool publication acceptance", isCollectible: true);
        var monitor = new WeakReference(load);
        try
        {
            _ = load.LoadFromAssemblyPath(publication);
            using FileStream writing = File.Open(source, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            Assert.True(writing.CanWrite);
            return monitor;
        }
        finally
        {
            load.Unload();
        }
    }
}
