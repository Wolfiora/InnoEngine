using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Inno.Build.Toolchains;
using Inno.Build.Toolchains.Browser;
using Xunit;

namespace Inno.Build.Tests;

public sealed class BrowserToolchainTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoBrowserToolchainTests", Guid.NewGuid().ToString("N"));

    public void Dispose()
    {
        if (Directory.Exists(m_root))
            Directory.Delete(m_root, recursive: true);
    }

    [Fact]
    public async Task EnvironmentUsesTheProjectSelectedToolsWithoutChangingTheParent()
    {
        string project = CreateProject();
        string? parentCache = Environment.GetEnvironmentVariable("EM_CACHE");
        string? parentRoot = Environment.GetEnvironmentVariable("DOTNET_ROOT");
        string? parentCompiler = Environment.GetEnvironmentVariable("BGCS_CC");
        string? parentCppCompiler = Environment.GetEnvironmentVariable("BGCS_CPP2C_CXX");

        IReadOnlyDictionary<string, string> environment = await BrowserToolchain.ResolveEnvironmentAsync("dotnet", project);

        Assert.Equal(Path.Combine(m_root, "selected-sdk", "bin"), environment["DOTNET_EMSCRIPTEN_LLVM_ROOT"]);
        Assert.Equal(Path.Combine(m_root, "selected-sdk", "bin", OperatingSystem.IsWindows() ? "clang.exe" : "clang"), environment["BGCS_CC"]);
        Assert.Equal(Path.Combine(m_root, "selected-sdk", "bin", OperatingSystem.IsWindows() ? "clang++.exe" : "clang++"), environment["BGCS_CPP2C_CXX"]);
        Assert.Equal(Path.Combine(m_root, "selected-cache"), environment["EM_CACHE"]);
        Assert.Equal(Path.Combine(m_root, "selected-node", "bin", NodeFileName()), environment["EMSDK_NODE"]);
        Assert.False(environment.ContainsKey("EMCC_CORES"));
        Assert.False(environment.ContainsKey("BINARYEN_CORES"));
        Assert.Equal(parentCache, Environment.GetEnvironmentVariable("EM_CACHE"));
        Assert.Equal(parentRoot, Environment.GetEnvironmentVariable("DOTNET_ROOT"));
        Assert.Equal(parentCompiler, Environment.GetEnvironmentVariable("BGCS_CC"));
        Assert.Equal(parentCppCompiler, Environment.GetEnvironmentVariable("BGCS_CPP2C_CXX"));
    }

    [Fact]
    public async Task EnvironmentRejectsAnIncompleteSelectedToolchain()
    {
        string project = CreateProject();
        File.Delete(Path.Combine(m_root, "selected-node", "bin", NodeFileName()));

        FileNotFoundException failure = await Assert.ThrowsAsync<FileNotFoundException>(
            () => BrowserToolchain.ResolveEnvironmentAsync("dotnet", project));

        Assert.EndsWith(NodeFileName(), failure.FileName);
    }

    [Fact]
    public async Task EnvironmentRejectsAProjectWithoutWorkloadSelection()
    {
        Directory.CreateDirectory(m_root);
        string project = Path.Combine(m_root, "NoWorkload.proj");
        new XDocument(new XElement("Project")).Save(project);

        InvalidOperationException failure = await Assert.ThrowsAsync<InvalidOperationException>(
            () => BrowserToolchain.ResolveEnvironmentAsync("dotnet", project));

        Assert.Contains("EmscriptenSdkToolsPath", failure.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task CapturePreservesStructuredArgumentsAndChildOnlyEnvironment()
    {
        const string variable = "INNO_TOOLCHAIN_TEST_VALUE";
        const string value = "spaces and `literal` $characters";
        string? parent = Environment.GetEnvironmentVariable(variable);
        Directory.CreateDirectory(m_root);
        string project = Path.Combine(m_root, "Environment.proj");
        new XDocument(new XElement("Project",
            new XElement("PropertyGroup", new XElement("Result", "$(" + variable + ")")))).Save(project);

        string output = await ToolchainEnvironment.CaptureOutputAsync("dotnet",
            ["msbuild", project, "-nologo", "-nodeReuse:false", "-getProperty:Result"],
            m_root, CancellationToken.None, new Dictionary<string, string> { [variable] = value });

        Assert.Equal(value, output.Trim());
        Assert.Equal(parent, Environment.GetEnvironmentVariable(variable));
    }

    [Fact]
    public async Task CaptureRejectsFailedTools()
    {
        Directory.CreateDirectory(m_root);

        await Assert.ThrowsAsync<InvalidOperationException>(() => ToolchainEnvironment.CaptureOutputAsync(
            "dotnet", ["--inno-invalid-option"], m_root, CancellationToken.None));
    }

    [Fact]
    public async Task CaptureDoesNotStartAnAlreadyCanceledOperation()
    {
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => ToolchainEnvironment.CaptureOutputAsync(
            "inno-tool-that-does-not-exist", [], m_root, cancellation.Token));
    }

    private string CreateProject()
    {
        string sdk = Path.Combine(m_root, "selected-sdk");
        string node = Path.Combine(m_root, "selected-node");
        string python = Path.Combine(m_root, "selected-python");
        string cache = Path.Combine(m_root, "selected-cache");
        Directory.CreateDirectory(Path.Combine(sdk, "bin"));
        File.WriteAllText(Path.Combine(sdk, "bin", OperatingSystem.IsWindows() ? "clang.exe" : "clang"), string.Empty);
        File.WriteAllText(Path.Combine(sdk, "bin", OperatingSystem.IsWindows() ? "clang++.exe" : "clang++"), string.Empty);
        Directory.CreateDirectory(Path.Combine(node, "bin"));
        Directory.CreateDirectory(python);
        Directory.CreateDirectory(Path.Combine(cache, "sysroot"));
        File.WriteAllText(Path.Combine(node, "bin", NodeFileName()), string.Empty);
        File.WriteAllText(Path.Combine(python, "python.exe"), string.Empty);
        string project = Path.Combine(m_root, "SelectedWorkload.proj");
        new XDocument(new XElement("Project", new XElement("PropertyGroup",
            new XElement("EmscriptenSdkToolsPath", sdk),
            new XElement("EmscriptenNodeToolsPath", node),
            new XElement("EmscriptenPythonToolsPath", python),
            new XElement("EmscriptenCacheSdkCacheDir", cache)))).Save(project);
        return project;
    }

    private static string NodeFileName() => OperatingSystem.IsWindows() ? "node.exe" : "node";
}
