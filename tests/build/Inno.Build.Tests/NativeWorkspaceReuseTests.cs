using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Build.Toolchains;
using Inno.Build.WindowsX64;

namespace Inno.Build.Tests;

public sealed class NativeWorkspaceReuseTests : IDisposable
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate int ReadNativeValue();

    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoWorkspaceTests", Guid.NewGuid().ToString("N"));
    private readonly string m_bindingRoot = Path.Combine(Path.GetTempPath(), "InnoWorkspaceBindings", Guid.NewGuid().ToString("N"));
    private readonly NativeComponentDescriptor m_owner = new("fixture", "native/fixture.csproj", "build/fixture.csproj");

    public NativeWorkspaceReuseTests()
    {
        Directory.CreateDirectory(Path.Combine(m_root, "build"));
        File.WriteAllText(Path.Combine(m_root, "InnoEngine.sln"), "fixture");
        File.WriteAllText(Path.Combine(m_root, "build", "fixture.csproj"), "<Project />");
    }

    [Fact]
    public async Task SourceChangesReuseTheWorkspaceButPublishDistinctImmutableProducts()
    {
        Directory.CreateDirectory(m_root);
        string source = Path.Combine(m_root, "input.cpp");
        await File.WriteAllTextAsync(source, "first");
        string? workspace = null;
        int compilations = 0;
        Task Produce(
            NativeBuildContext scoped,
            string output,
            CancellationToken cancellation
        ) {
            string current = scoped.GetNativeBuildRoot(m_owner);
            string physical = new DirectoryInfo(current).LinkTarget ?? current;
            if (workspace is not null)
            {
                Assert.Equal(workspace, physical);
                Assert.True(File.Exists(Path.Combine(current, "retained.object")));
            }
            workspace = physical;
            File.WriteAllText(Path.Combine(current, "retained.object"), "compiler intermediate");
            Interlocked.Increment(ref compilations);
            return File.WriteAllTextAsync(Path.Combine(output, "result.native"), File.ReadAllText(source), cancellation);
        }
        NativeBuildRecipe Recipe() => new(m_owner, "fixture", "fixture-x64",
            [new NativeBuildInput("input.cpp", source)], ["shared", "compiler-flag"]);
        using NativeBuildProduct first = await NativeArtifactPublisher.PublishAsync(new(m_root, "release"), Recipe(), Produce);
        DateTime timestamp = File.GetLastWriteTimeUtc(source);
        await File.WriteAllTextAsync(source, "other");
        File.SetLastWriteTimeUtc(source, timestamp);
        using NativeBuildProduct changed = await NativeArtifactPublisher.PublishAsync(new(m_root, "release"), Recipe(), Produce);
        using NativeBuildProduct hit = await NativeArtifactPublisher.PublishAsync(new(m_root, "release"), Recipe(), Produce);
        Assert.NotEqual(first.fingerprint, changed.fingerprint);
        Assert.Equal(changed.fingerprint, hit.fingerprint);
        Assert.Equal(2, compilations);
        Assert.Equal("first", File.ReadAllText(first.files[0]));
        Assert.Equal("other", File.ReadAllText(changed.files[0]));
    }

    [Fact]
    public async Task IncompatibleCompilerDeclarationsUseSeparateWorkspaces()
    {
        Directory.CreateDirectory(m_root);
        string source = Path.Combine(m_root, "input.cpp");
        await File.WriteAllTextAsync(source, "source");
        string? first = null;
        int calls = 0;
        Task Produce(
            NativeBuildContext scoped,
            string output,
            CancellationToken cancellation
        ) {
            string current = scoped.GetNativeBuildRoot(m_owner);
            if (calls++ == 0)
                first = current;
            else
                Assert.NotEqual(first, current);
            return File.WriteAllTextAsync(Path.Combine(output, "result.native"), "complete", cancellation);
        }
        foreach (string argument in new[] { "-O0", "-O2" })
        {
            using NativeBuildProduct product = await NativeArtifactPublisher.PublishAsync(new(m_root, "release"),
                new(m_owner, "fixture", "fixture-x64", [new NativeBuildInput("input.cpp", source)], [argument]), Produce);
        }
        Assert.Equal(2, calls);
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(true, false)]
    [InlineData(false, true)]
    [InlineData(true, true)]
    public async Task BindingGenerationChangesReuseTheWorkspaceAndCopiedInputLocation(
        bool aggregate,
        bool externalBindings
    ) {
        var owner = new NativeComponentDescriptor("fixture", "native/fixture.csproj", "build/fixture.csproj",
            bindingDefinition: "native/Bindings/bindings.props");
        string? workspace = null;
        string? copiedBridge = null;
        string? previousProduct = null;
        var tools = new NativeToolchainSelection("fixture-x64", new BuildHostDescriptor("Fixture", "x64"),
            new Dictionary<string, string> { ["fixture"] = ToolchainEnvironment.ResolveExecutable("dotnet") },
            new Dictionary<string, string>(), [], [], new NativeLinkPlan(".native"), false);
        foreach (string generation in new[] { new string('a', 64), new string('b', 64) })
        {
            string artifact = Path.Combine(externalBindings ? m_bindingRoot : m_root, "native", "obj", "fixture-x64", generation);
            string bridge = Path.Combine(artifact, "Native");
            string managed = Path.Combine(artifact, "Generated", "Bindings.cs");
            Directory.CreateDirectory(bridge);
            Directory.CreateDirectory(Path.GetDirectoryName(managed)!);
            string header = Path.Combine(bridge, "fixture.h");
            File.WriteAllText(header, generation);
            File.WriteAllText(managed, generation);
            File.WriteAllText(Path.Combine(bridge, "compilation.json"), System.Text.Json.JsonSerializer.Serialize(new NativeBindingCompilation
            {
                roots = [new("generated", "", true, true)], files = [], includeDirectories = [], defines = [], compilerDefines = [],
                cxxStandard = 0, cStandard = 0, requiredExports = [], staticExportSymbols = [], staticExportAbi = "", staticExportNames = []
            }));
            var descriptor = new NativeBindingGenerationDescriptor
            {
                fingerprint = generation,
                bindingsPath = managed,
                nativeDirectory = bridge,
                artifactDirectory = artifact,
                artifactOwnerDirectory = externalBindings ? m_bindingRoot : m_root
            };
            using var bindings = new NativeBindingGenerationResult(
                new Dictionary<string, NativeBindingGenerationDescriptor> { [owner.nativeProject] = descriptor }, []);
            NativeBuildContext context = new NativeBuildContext(m_root, "release").WithToolchain(tools).WithBindings(bindings);
            var recipe = new NativeBuildRecipe(owner, "fixture", "fixture-x64",
                [NativeBuildInput.FromPath(m_root, bridge), NativeBuildInput.FromPath(m_root, managed)],
                [(aggregate ? owner.nativeProject + "=" : "bindings=") + generation,
                    (aggregate ? "componentArgument=" : string.Empty) + "-DINNO_FIXTURE_BRIDGE_ROOT=" + bridge]);
            using NativeBuildProduct product = await NativeArtifactPublisher.PublishAsync(context, recipe,
                async (
                    @scoped,
                    output,
                    cancellation
                ) => {
                    string current = scoped.GetNativeBuildRoot(owner);
                    string physical = new DirectoryInfo(current).LinkTarget ?? current;
                    NativeCMakeSource sources = await NativeCMakeExecutor.PrepareSourceAsync(scoped, owner, cancellation);
                    string input = sources.ResolvePath(header);
                    if (workspace is not null)
                    {
                        Assert.Equal(workspace, physical);
                        Assert.Equal(copiedBridge, input);
                    }
                    workspace = physical;
                    copiedBridge = input;
                    Assert.Equal(generation, File.ReadAllText(input));
                    Assert.Equal("-DINNO_FIXTURE_BRIDGE_ROOT=" + Path.GetDirectoryName(input),
                        sources.ResolveDefinition("-DINNO_FIXTURE_BRIDGE_ROOT=" + bridge));
                    await File.WriteAllTextAsync(Path.Combine(output, "result.native"), generation, cancellation);
                });
            if (previousProduct is not null)
                Assert.NotEqual(previousProduct, product.fingerprint);
            previousProduct = product.fingerprint;
        }
    }

    [Fact]
    public async Task WindowsCMakeReusesUnchangedObjectsAndPublishesChangedSourceBytes()
    {
        if (!OperatingSystem.IsWindows())
            return;
        NativeToolchainSelection tools = await new WindowsX64NativeToolchainProvider(
            ToolchainEnvironment.ResolveExecutable("dotnet")).ResolveAsync(new(m_root, "release"),
            BuildTestProducts.WindowsHost(), "windows-x64", CancellationToken.None);
        string source = Path.Combine(m_root, "Source");
        Directory.CreateDirectory(source);
        File.WriteAllText(Path.Combine(source, "CMakeLists.txt"),
            "cmake_minimum_required(VERSION 3.20)\nproject(WorkspaceFixture C)\nadd_library(fixture SHARED api.c stable.c)\n");
        string changed = Path.Combine(source, "api.c");
        File.WriteAllText(changed, "__declspec(dllexport) int value(void) { return 41; }\n");
        File.WriteAllText(Path.Combine(source, "stable.c"), "int stable(void) { return 7; }\n");
        DateTime originalTime = File.GetLastWriteTimeUtc(changed);
        string? workspace = null;
        string? stableObject = null;
        DateTime stableTime = default;
        int compilations = 0;
        async Task Produce(
            NativeBuildContext scoped,
            string output,
            CancellationToken cancellation
        ) {
            string current = scoped.GetNativeBuildRoot(m_owner);
            string physical = new DirectoryInfo(current).LinkTarget ?? current;
            if (workspace is not null)
                Assert.Equal(workspace, physical);
            workspace = physical;
            string build = await NativeCMakeExecutor.BuildAsync(scoped, m_owner, source, "fixture", [], cancellation);
            File.Copy(NativeCMakeExecutor.FindOutput(scoped, build, "fixture.dll"), Path.Combine(output, "fixture.dll"));
            string objectPath = Directory.EnumerateFiles(physical, "*.obj", SearchOption.AllDirectories)
                .Single(path => Path.GetFileName(path) is "stable.obj" or "stable.c.obj");
            if (compilations++ == 0)
            {
                stableObject = objectPath;
                stableTime = File.GetLastWriteTimeUtc(objectPath);
            }
            else
            {
                Assert.Equal(stableObject, objectPath);
                Assert.Equal(stableTime, File.GetLastWriteTimeUtc(objectPath));
            }
        }
        NativeBuildContext Context() => new NativeBuildContext(m_root, "release").WithToolchain(tools)
            .WithComponentOptions(m_owner, new(NativeLibraryKind.Shared));
        NativeBuildRecipe Recipe() => new(m_owner, "fixture", "windows-x64",
            new[] { source }.Concat(tools.inputPaths).Select(path => NativeBuildInput.FromPath(m_root, path)),
            tools.declarations.Concat(new[] { "shared", "fixture" }));
        using NativeBuildProduct first = await NativeArtifactPublisher.PublishAsync(Context(), Recipe(), Produce);
        Assert.Equal(41, Read(first));
        File.WriteAllText(changed, "__declspec(dllexport) int value(void) { return 42; }\n");
        File.SetLastWriteTimeUtc(changed, originalTime);
        using NativeBuildProduct second = await NativeArtifactPublisher.PublishAsync(Context(), Recipe(), Produce);
        Assert.NotEqual(first.fingerprint, second.fingerprint);
        Assert.Equal(42, Read(second));
        Assert.Equal(41, Read(first));
        NativeBuildContext hitContext = Context();
        using NativeBuildProduct hit = await NativeArtifactPublisher.PublishAsync(hitContext, Recipe(), Produce);
        Assert.Equal(second.fingerprint, hit.fingerprint);
        Assert.Equal(2, compilations);
        Assert.Equal(0, hitContext.statistics.nativeProcesses);
    }

    public void Dispose()
    {
        if (Directory.Exists(m_root))
            Directory.Delete(m_root, recursive: true);
        if (Directory.Exists(m_bindingRoot))
            Directory.Delete(m_bindingRoot, recursive: true);
    }

    private static int Read(NativeBuildProduct product)
    {
        nint library = NativeLibrary.Load(product.files.Single());
        try
        {
            return Marshal.GetDelegateForFunctionPointer<ReadNativeValue>(NativeLibrary.GetExport(library, "value"))();
        }
        finally
        {
            NativeLibrary.Free(library);
        }
    }

}
