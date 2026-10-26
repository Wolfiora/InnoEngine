using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Core.IO;
using Xunit;

namespace Inno.Build.Tests;

public sealed class ManagedBuildInputSnapshotTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoManagedInputTests", Guid.NewGuid().ToString("N"));

    public ManagedBuildInputSnapshotTests()
    {
        Directory.CreateDirectory(m_root);
        File.WriteAllText(Path.Combine(m_root, "InnoEngine.sln"), "fixture");
        Write("Shared/Shared.csproj", "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>net10.0</TargetFramework></PropertyGroup></Project>");
        Write("Shared/Shared.cs", "public sealed class Shared { }");
        Write("App/App.csproj", "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>net10.0</TargetFramework>"
            + "<ImplicitUsings>disable</ImplicitUsings></PropertyGroup><Import Project=\"Options.props\" />"
            + "<ItemGroup><ProjectReference Include=\"../Shared/Shared.csproj\" /><EmbeddedResource Include=\"data.txt\" />"
            + "<AdditionalFiles Include=\"input.txt\" /></ItemGroup></Project>");
        Write("App/Options.props", "<Project><PropertyGroup><DefineConstants>FIXTURE</DefineConstants></PropertyGroup></Project>");
        Write("App/App.cs", "public sealed class App { }");
        Write("App/data.txt", "first");
        Write("App/input.txt", "first");
        Write("Tool/Tool.csproj", "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>net10.0</TargetFramework></PropertyGroup></Project>");
        Write("Tool/Tool.cs", "public sealed class Tool { }");
    }

    [Theory]
    [InlineData("net10.0-browser")]
    [InlineData("netstandard2.0")]
    public async Task FrameworkSelectionUsesTheSdkAndRestoredTargetSemantics(string framework)
    {
        string project = Path.Combine(m_root, "App", "App.csproj");
        File.WriteAllText(project, File.ReadAllText(project).Replace("<TargetFramework>net10.0</TargetFramework>",
            "<TargetFramework>" + framework + "</TargetFramework>", StringComparison.Ordinal));
        string shared = Path.Combine(m_root, "Shared", "Shared.csproj");
        File.WriteAllText(shared, File.ReadAllText(shared).Replace("<TargetFramework>net10.0</TargetFramework>",
            "<TargetFramework>" + framework + "</TargetFramework>", StringComparison.Ordinal));
        string dotnet = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? ToolchainEnvironment.ResolveExecutable("dotnet");
        DotNetSdkDescriptor sdk = await DotNetSdkResolver.ResolveAsync(dotnet, project);
        await ToolchainEnvironment.RunAsync(sdk.hostPath, [sdk.cliPath, "restore", project, "--nologo", "-v:quiet"],
            m_root, CancellationToken.None, DotNetSdkEnvironment.Create(sdk.hostPath));

        ManagedBuildInputSnapshot snapshot = await ManagedBuildInputSnapshot.CaptureAsync(
            new NativeBuildContext(m_root, "release"), sdk, project,
            new Dictionary<string, string> { ["Configuration"] = "Release" });

        Assert.NotEmpty(snapshot.fingerprint);
        await snapshot.VerifyAsync();
        Assert.False(Directory.Exists(Path.Combine(m_root, "App", "bin")));
    }

    /// <summary>
    /// Keeps package declarations in the frozen source and rejects changes after capture.
    /// </summary>
    /// <returns>
    /// Completion after the execution copy is checked and a modified package request is rejected.
    /// </returns>
    [Fact]
    public async Task PackageDeclarationsBelongToFrozenCompilationInputs()
    {
        Write("App/Options.props", """
            <Project>
              <ItemGroup><PackageReference Include="BGCS.Runtime" Version="[4.1.2]" /></ItemGroup>
            </Project>
            """);
        string project = Path.Combine(m_root, "App", "App.csproj");
        string dotnet = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? ToolchainEnvironment.ResolveExecutable("dotnet");
        using var cancellation = new CancellationTokenSource(TimeSpan.FromMinutes(3));
        DotNetSdkDescriptor sdk = await DotNetSdkResolver.ResolveAsync(dotnet, project, cancellation.Token);
        await ToolchainEnvironment.RunAsync(sdk.hostPath,
            [sdk.cliPath, "restore", project, "--disable-build-servers", "-v:quiet"],
            m_root, cancellation.Token, DotNetSdkEnvironment.Create(sdk.hostPath));
        ManagedBuildInputSnapshot snapshot = await ManagedBuildInputSnapshot.CaptureAsync(
            new NativeBuildContext(m_root, "release"), sdk, project,
            new Dictionary<string, string> { ["Configuration"] = "Release" }, cancellation.Token);
        using ManagedBuildExecution execution = await snapshot.CreateExecutionAsync(cancellation.Token);
        string source = Path.Combine(m_root, "App", "Options.props");
        string frozen = Path.Combine(execution.engineRoot, "App", "Options.props");
        Assert.Equal(File.ReadAllText(source), File.ReadAllText(frozen));
        Assert.Empty(Directory.EnumerateFiles(execution.engineRoot, "packages*.lock.json", SearchOption.AllDirectories));
        File.WriteAllText(source, File.ReadAllText(source).Replace("[4.1.2]", "[4.1.1]", StringComparison.Ordinal));
        await Assert.ThrowsAsync<InvalidOperationException>(() => snapshot.VerifyAsync(cancellation.Token).AsTask());
    }

    [Fact]
    public async Task RuntimeInputEvaluationDoesNotShareOrdinaryRestoreOutputs()
    {
        string project = Path.Combine(m_root, "App", "App.csproj");
        File.WriteAllText(project, File.ReadAllText(project).Replace("</Project>",
            "<ItemGroup><InnoBuildInputProject Include=\"../Tool/Tool.csproj\" GlobalPropertiesToRemove=\"RuntimeIdentifier\" /></ItemGroup></Project>", StringComparison.Ordinal));
        Write("Tool/Tool.csproj", "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>net10.0</TargetFramework>"
            + "<ImplicitUsings>disable</ImplicitUsings></PropertyGroup><ItemGroup>"
            + "<ProjectReference Include=\"../Shared/Shared.csproj\" /></ItemGroup></Project>");
        string dotnet = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? ToolchainEnvironment.ResolveExecutable("dotnet");
        DotNetSdkDescriptor sdk = await DotNetSdkResolver.ResolveAsync(dotnet, project);
        await RestoreOrdinary();
        string assets = Path.Combine(m_root, "App", "obj", "project.assets.json");
        byte[] ordinary = File.ReadAllBytes(assets);
        ManagedBuildInputSnapshot snapshot = await ManagedBuildInputSnapshot.CaptureAsync(
            new NativeBuildContext(m_root, "release"), sdk, project,
            new Dictionary<string, string>
            {
                ["Configuration"] = "Release",
                ["RuntimeIdentifier"] = RuntimeInformation.RuntimeIdentifier
            });
        Assert.Equal(ordinary, File.ReadAllBytes(assets));
        await Task.WhenAll(RestoreOrdinary(), snapshot.VerifyAsync().AsTask());
        Assert.Equal(ordinary, File.ReadAllBytes(assets));
        Assert.False(Directory.Exists(Path.Combine(m_root, "App", "bin")));

        Task RestoreOrdinary() => ToolchainEnvironment.RunAsync(sdk.hostPath,
            [sdk.cliPath, "restore", project, "--disable-build-servers", "-m:1", "-nodeReuse:false", "--nologo", "-v:quiet"],
            m_root, CancellationToken.None, DotNetSdkEnvironment.Create(sdk.hostPath));
    }

    [Fact]
    public async Task CheckoutAliasPreservesOwnedSourcesWhileSharingPhysicalInputReads()
    {
        string alias = m_root + "-alias";
        try
        {
            if (OperatingSystem.IsWindows())
                await ToolchainEnvironment.RunAsync("powershell.exe",
                    ["-NoProfile", "-NonInteractive", "-Command",
                        "New-Item -ItemType Junction -Path '" + alias + "' -Target '" + m_root + "' | Out-Null"],
                    m_root, CancellationToken.None);
            else
                Directory.CreateSymbolicLink(alias, m_root);
            string project = Path.Combine(alias, "App", "App.csproj");
            string dotnet = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? ToolchainEnvironment.ResolveExecutable("dotnet");
            DotNetSdkDescriptor sdk = await DotNetSdkResolver.ResolveAsync(dotnet, project);
            await ToolchainEnvironment.RunAsync(sdk.hostPath, [sdk.cliPath, "restore", project, "--nologo", "-v:quiet"],
                alias, CancellationToken.None, DotNetSdkEnvironment.Create(sdk.hostPath));
            var context = new NativeBuildContext(alias, "release");
            ManagedBuildInputSnapshot snapshot = await ManagedBuildInputSnapshot.CaptureAsync(context, sdk, project,
                new Dictionary<string, string> { ["Configuration"] = "Release" });
            using ManagedBuildExecution execution = await snapshot.CreateExecutionAsync();
            Assert.Contains("class App", File.ReadAllText(Path.Combine(execution.engineRoot, "App", "App.cs")), StringComparison.Ordinal);
            Assert.Contains("class Shared", File.ReadAllText(Path.Combine(execution.engineRoot, "Shared", "Shared.cs")), StringComparison.Ordinal);
            await snapshot.VerifyAsync();
            await execution.VerifyAsync();
        }
        finally
        {
            if (Directory.Exists(alias))
                Directory.Delete(alias, recursive: false);
        }
    }

    [Fact]
    public async Task FixedExecutionCompilesPlannedBytesWhenTheOriginalSourceChangesAndReturns()
    {
        Write("Vendor/bin/tool.dat", "owned vendor tool");
        string fixture = Path.Combine(m_root, "App", "App.csproj");
        File.WriteAllText(fixture, File.ReadAllText(fixture).Replace("</Project>",
            "<ItemGroup><AdditionalFiles Include=\"../Vendor/bin/tool.dat\" /></ItemGroup></Project>", StringComparison.Ordinal));
        string project = Path.Combine(m_root, "App", "App.csproj");
        string dotnet = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? ToolchainEnvironment.ResolveExecutable("dotnet");
        DotNetSdkDescriptor sdk = await DotNetSdkResolver.ResolveAsync(dotnet, project);
        await ToolchainEnvironment.RunAsync(sdk.hostPath, [sdk.cliPath, "restore", project, "--nologo", "-v:quiet"],
            m_root, CancellationToken.None, DotNetSdkEnvironment.Create(sdk.hostPath));
        var context = new NativeBuildContext(m_root, "release");
        ManagedBuildInputSnapshot snapshot = await ManagedBuildInputSnapshot.CaptureAsync(context, sdk, project,
            new System.Collections.Generic.Dictionary<string, string> { ["Configuration"] = "Release" });
        using ManagedBuildExecution execution = await snapshot.CreateExecutionAsync();
        Assert.Equal("owned vendor tool", File.ReadAllText(Path.Combine(execution.engineRoot, "Vendor", "bin", "tool.dat")));
        string original = Path.Combine(m_root, "App", "App.cs");
        File.WriteAllText(original, "public sealed class Changed { }");
        Assert.Contains("class App", File.ReadAllText(Path.Combine(Path.GetDirectoryName(execution.project)!, "App.cs")), StringComparison.Ordinal);
        await ToolchainEnvironment.RunAsync(sdk.hostPath,
            [sdk.cliPath, "build", execution.project, "--configuration", "Release", "--disable-build-servers",
                "-m:1", "-nodeReuse:false", "--nologo", "-v:quiet"], execution.engineRoot, CancellationToken.None,
            DotNetSdkEnvironment.Create(sdk.hostPath));
        File.WriteAllText(original, "public sealed class App { }");
        await snapshot.VerifyAsync();
        await execution.VerifyAsync();
        string output = Path.Combine(Path.GetDirectoryName(execution.project)!, "bin", "Release", "net10.0", "App.dll");
        using FileStream stream = File.OpenRead(output);
        using var image = new System.Reflection.PortableExecutable.PEReader(stream);
        System.Reflection.Metadata.MetadataReader metadata = System.Reflection.Metadata.PEReaderExtensions.GetMetadataReader(image);
        string[] names = System.Linq.Enumerable.ToArray(System.Linq.Enumerable.Select(metadata.TypeDefinitions,
            handle => metadata.GetString(metadata.GetTypeDefinition(handle).Name)));
        Assert.Contains("App", names);
        Assert.DoesNotContain("Changed", names);
    }

    [Fact]
    public async Task SelectedSdkGraphDetectsSameLengthSourceResourceImportAndNewFilesWithoutCompiling()
    {
        string project = Path.Combine(m_root, "App", "App.csproj");
        string dotnet = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? ToolchainEnvironment.ResolveExecutable("dotnet");
        DotNetSdkDescriptor sdk = await DotNetSdkResolver.ResolveAsync(dotnet, project);
        await ToolchainEnvironment.RunAsync(sdk.hostPath, [sdk.cliPath, "restore", project, "--nologo", "-v:quiet"],
            m_root, CancellationToken.None, DotNetSdkEnvironment.Create(sdk.hostPath));
        await ToolchainEnvironment.RunAsync(sdk.hostPath, [sdk.cliPath, "restore", Path.Combine(m_root, "Tool", "Tool.csproj"), "--nologo", "-v:quiet"],
            m_root, CancellationToken.None, DotNetSdkEnvironment.Create(sdk.hostPath));
        File.WriteAllText(project, File.ReadAllText(project).Replace("</Project>",
            "<ItemGroup><InnoBuildInputProject Include=\"../Tool/Tool.csproj\" /></ItemGroup></Project>", StringComparison.Ordinal));
        var properties = new Dictionary<string, string> { ["Configuration"] = "Release" };
        ManagedBuildInputSnapshot original = await Capture(sdk, project, properties);
        await original.VerifyAsync();
        ManagedBuildInputSnapshot repeated = await Capture(sdk, project, properties);
        Assert.Equal(original.fingerprint, repeated.fingerprint);
        foreach (string path in new[] { "Shared/Shared.cs", "App/data.txt", "App/Options.props", "Tool/Tool.cs" })
        {
            string file = Path.Combine(m_root, path);
            string previous = File.ReadAllText(file);
            DateTime timestamp = File.GetLastWriteTimeUtc(file);
            File.WriteAllText(file, previous.Replace("first", "other", StringComparison.Ordinal)
                .Replace("Shared", "Change", StringComparison.Ordinal).Replace("FIXTURE", "CHANGED", StringComparison.Ordinal));
            if (path == "Tool/Tool.cs")
                File.WriteAllText(file, previous.Replace("Tool", "Next", StringComparison.Ordinal));
            File.SetLastWriteTimeUtc(file, timestamp);
            await Assert.ThrowsAsync<InvalidOperationException>(() => original.VerifyAsync().AsTask());
            File.WriteAllText(file, previous);
            File.SetLastWriteTimeUtc(file, timestamp);
        }
        Write("App/Added.cs", "public sealed class Added { }");
        await Assert.ThrowsAsync<InvalidOperationException>(() => original.VerifyAsync().AsTask());
        Assert.False(Directory.Exists(Path.Combine(m_root, "App", "bin")));
        Assert.All(Directory.EnumerateDirectories(Path.Combine(m_root, "artifacts", "build-tools", "managed-inputs")),
            static directory => Assert.Contains(Path.GetFileName(directory), new[] { "evaluators", "requests" }));
        string requests = Path.Combine(m_root, "artifacts", "build-tools", "managed-inputs", "requests");
        Assert.Empty(ProcessOwnedDirectory.ReadClaims(requests));
        Assert.Empty(Directory.EnumerateDirectories(requests));
        Assert.Single(Directory.EnumerateDirectories(Path.Combine(m_root, "artifacts", "build-tools", "managed-inputs", "evaluators")));

        Task<ManagedBuildInputSnapshot> Capture(
            DotNetSdkDescriptor selected,
            string entry,
            IReadOnlyDictionary<string, string> globals
        ) => ManagedBuildInputSnapshot.CaptureAsync(new NativeBuildContext(m_root, "release"), selected, entry, globals).AsTask();
    }

    /// <summary>
    /// Captures SDK-generated symbols without invoking compilation or product preparation.
    /// </summary>
    /// <returns>
    /// Completion after a target-derived symbol change invalidates the frozen snapshot.
    /// </returns>
    [Fact]
    [Trait("ProcessIsolation", "Required")]
    public async Task SdkPreparedSymbolsParticipateInTheFrozenInputIdentity()
    {
        string project = Path.Combine(m_root, "App", "App.csproj");
        File.WriteAllText(project, File.ReadAllText(project).Replace("</Project>", "<Import Project=\"Symbols.targets\" /></Project>", StringComparison.Ordinal));
        Write("App/Symbols.targets", """
            <Project>
              <Target Name="AppendObservedSymbol" AfterTargets="GenerateTargetFrameworkDefineConstants">
                <Error Condition="'$(ImplicitFrameworkDefine)' != 'NET10_0'" Text="The SDK framework symbol was not prepared." />
                <PropertyGroup><DefineConstants>$(DefineConstants);$(INNO_TEST_COMPILER_SYMBOL)</DefineConstants></PropertyGroup>
              </Target>
              <Target Name="RejectCompilation" BeforeTargets="PrepareForBuild;CoreCompile">
                <Error Text="Input capture must not compile or prepare a product." />
              </Target>
            </Project>
            """);
        string? previous = Environment.GetEnvironmentVariable("INNO_TEST_COMPILER_SYMBOL");
        try
        {
            Environment.SetEnvironmentVariable("INNO_TEST_COMPILER_SYMBOL", "FIRST_SYMBOL");
            string dotnet = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? ToolchainEnvironment.ResolveExecutable("dotnet");
            DotNetSdkDescriptor sdk = await DotNetSdkResolver.ResolveAsync(dotnet, project);
            await ToolchainEnvironment.RunAsync(sdk.hostPath, [sdk.cliPath, "restore", project, "--nologo", "-v:quiet"],
                m_root, CancellationToken.None, DotNetSdkEnvironment.Create(sdk.hostPath));
            ManagedBuildInputSnapshot snapshot = await ManagedBuildInputSnapshot.CaptureAsync(
                new NativeBuildContext(m_root, "release"), sdk, project,
                new Dictionary<string, string> { ["Configuration"] = "Release" });
            await snapshot.VerifyAsync();
            Environment.SetEnvironmentVariable("INNO_TEST_COMPILER_SYMBOL", "SECOND_SYMBOL");
            InvalidOperationException failure = await Assert.ThrowsAsync<InvalidOperationException>(() => snapshot.VerifyAsync().AsTask());
            Assert.Contains("CompilerDefineConstants", failure.Message, StringComparison.Ordinal);
            Assert.False(Directory.Exists(Path.Combine(m_root, "App", "bin")));
        }
        finally
        {
            Environment.SetEnvironmentVariable("INNO_TEST_COMPILER_SYMBOL", previous);
        }
    }

    public void Dispose() => Directory.Delete(m_root, recursive: true);

    private void Write(
        string path,
        string text
    ) {
        string file = Path.Combine(m_root, path);
        Directory.CreateDirectory(Path.GetDirectoryName(file)!);
        File.WriteAllText(file, text);
    }
}
