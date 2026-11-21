using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Build.Toolchains;

namespace Inno.Build.Toolchains.Tests;

public sealed class NativeWorkspaceReuseTests : IDisposable
{
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
        NativeBuildRecipe Recipe()
            => new(
                m_owner,
                "fixture",
                "fixture-x64",
                [new NativeBuildInput("input.cpp", source)],
                ["shared", "compiler-flag"]
            );
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
            {
                first = current;
            }
            else
            {
                Assert.NotEqual(first, current);
            }
            return File.WriteAllTextAsync(Path.Combine(output, "result.native"), "complete", cancellation);
        }
        foreach (string argument in new[] { "-O0", "-O2" })
        {
            using NativeBuildProduct product = await NativeArtifactPublisher.PublishAsync(
                new(m_root, "release"),
                new(m_owner, "fixture", "fixture-x64", [new NativeBuildInput("input.cpp", source)], [argument]),
                Produce
            );
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
        var owner = new NativeComponentDescriptor(
            "fixture",
            "native/fixture.csproj",
            "build/fixture.csproj",
            bindingDefinition: "native/Bindings/bindings.props"
        );
        string? workspace = null;
        string? copiedBridge = null;
        string? previousProduct = null;
        var tools = new NativeToolchainSelection(
            "fixture-x64",
            new BuildHostDescriptor("Fixture", "x64"),
            new Dictionary<string, string> { ["fixture"] = ToolchainEnvironment.ResolveExecutable("dotnet") },
            new Dictionary<string, string>(),
            [],
            [],
            new NativeLinkPlan(".native"),
            false
        );
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
            File.WriteAllText(
                Path.Combine(bridge, "compilation.json"),
                System.Text.Json.JsonSerializer.Serialize(new NativeBindingCompilation
                {
                    roots = [new("generated", "", true, true)],
                    files = [],
                    includeDirectories = [],
                    defines = [],
                    compilerDefines = [],
                    cxxStandard = 0,
                    cStandard = 0,
                    requiredExports = [],
                    staticExportSymbols = [],
                    staticExportAbi = "",
                    staticExportNames = []
                })
            );
            var descriptor = new NativeBindingGenerationDescriptor
            {
                fingerprint = generation,
                bindingsPath = managed,
                usesRuntimeImports = false,
                nativeDirectory = bridge,
                artifactDirectory = artifact,
                artifactOwnerDirectory = externalBindings ? m_bindingRoot : m_root
            };
            using var bindings = new NativeBindingGenerationResult(
                new Dictionary<string, NativeBindingGenerationDescriptor> { [owner.nativeProject] = descriptor },
                []
            );
            NativeBuildContext context = new NativeBuildContext(m_root, "release").WithToolchain(tools).WithBindings(bindings);
            var recipe = new NativeBuildRecipe(
                owner,
                "fixture",
                "fixture-x64",
                [NativeBuildInput.FromPath(m_root, bridge), NativeBuildInput.FromPath(m_root, managed)],
                [
                    (aggregate ? owner.nativeProject + "=" : "bindings=") + generation,
                    (aggregate ? "componentArgument=" : string.Empty) + "-DINNO_FIXTURE_BRIDGE_ROOT=" + bridge
                ]
            );
            using NativeBuildProduct product = await NativeArtifactPublisher.PublishAsync(
                context,
                recipe,
                async (
                    @scoped,
                    output,
                    cancellation
                ) =>
                {
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
                    Assert.Equal(
                        "-DINNO_FIXTURE_BRIDGE_ROOT=" + Path.GetDirectoryName(input),
                        sources.ResolveDefinition("-DINNO_FIXTURE_BRIDGE_ROOT=" + bridge)
                    );
                    await File.WriteAllTextAsync(Path.Combine(output, "result.native"), generation, cancellation);
                }
            );
            if (previousProduct is not null)
            {
                Assert.NotEqual(previousProduct, product.fingerprint);
            }
            previousProduct = product.fingerprint;
        }
    }

    public void Dispose()
    {
        if (Directory.Exists(m_root))
        {
            Directory.Delete(m_root, recursive: true);
        }
        if (Directory.Exists(m_bindingRoot))
        {
            Directory.Delete(m_bindingRoot, recursive: true);
        }
    }
}
