using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Xunit;

namespace Inno.Build.Tests;

public sealed class ManagedBuildInputSnapshotTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoManagedInputTests", Guid.NewGuid().ToString("N"));

    public ManagedBuildInputSnapshotTests()
    {
        Directory.CreateDirectory(m_root);
        File.WriteAllText(Path.Combine(m_root, "InnoEngine.sln"), "fixture");
        Write("Shared/Shared.csproj", "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>net9.0</TargetFramework></PropertyGroup></Project>");
        Write("Shared/Shared.cs", "public sealed class Shared { }");
        Write("App/App.csproj", "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>net9.0</TargetFramework>"
            + "<ImplicitUsings>disable</ImplicitUsings></PropertyGroup><Import Project=\"Options.props\" />"
            + "<ItemGroup><ProjectReference Include=\"../Shared/Shared.csproj\" /><EmbeddedResource Include=\"data.txt\" />"
            + "<AdditionalFiles Include=\"input.txt\" /></ItemGroup></Project>");
        Write("App/Options.props", "<Project><PropertyGroup><DefineConstants>FIXTURE</DefineConstants></PropertyGroup></Project>");
        Write("App/App.cs", "public sealed class App { }");
        Write("App/data.txt", "first");
        Write("App/input.txt", "first");
        Write("Tool/Tool.csproj", "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>net9.0</TargetFramework></PropertyGroup></Project>");
        Write("Tool/Tool.cs", "public sealed class Tool { }");
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
        string output = Path.Combine(Path.GetDirectoryName(execution.project)!, "bin", "Release", "net9.0", "App.dll");
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
            static directory => Assert.Equal("evaluators", Path.GetFileName(directory)));
        Assert.Single(Directory.EnumerateDirectories(Path.Combine(m_root, "artifacts", "build-tools", "managed-inputs", "evaluators")));

        Task<ManagedBuildInputSnapshot> Capture(
            DotNetSdkDescriptor selected,
            string entry,
            IReadOnlyDictionary<string, string> globals
        ) => ManagedBuildInputSnapshot.CaptureAsync(new NativeBuildContext(m_root, "release"), selected, entry, globals).AsTask();
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
