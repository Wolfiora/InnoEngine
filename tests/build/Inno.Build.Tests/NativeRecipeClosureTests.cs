using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Build.Toolchains;
using Inno.Build.Toolchains.MiniAudio;
using Inno.Build.Toolchains.Sdl3;
using Inno.Build.Toolchains.Text;
using Inno.Build.Toolchains.UI;

namespace Inno.Build.Tests;

public sealed class NativeRecipeClosureTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoRecipeClosureTests", Guid.NewGuid().ToString("N"));
    private readonly NativeComponentDescriptor m_owner = new("fixture", "native/fixture.csproj", "build/fixture.csproj",
        bindingDefinition: "native/Bindings/bindings.props");

    public NativeRecipeClosureTests()
    {
        Write("InnoEngine.sln", "fixture");
        Write("native/fixture.csproj", "<Project />");
        Write("build/fixture.csproj", "<Project />");
        Write("build/Producer.cs", "producer");
        Write("input.cpp", "source");
        Write("tools/compiler", "fixture executable identity");
    }

    [Fact]
    public void SourceDeclarationDoesNotRequireOrCreateGeneratedOutputs()
    {
        NativeBuildRecipe recipe = Recipe(Context());
        Assert.All(recipe.inputs, input => Assert.DoesNotContain("selected", input.physicalPath, StringComparison.Ordinal));
        Assert.DoesNotContain(recipe.declarations, declaration => declaration.StartsWith("bindings=", StringComparison.Ordinal));
        Assert.NotEmpty(NativeBuildFingerprint.Create(recipe.declarations, recipe.inputs));
        Assert.False(Directory.Exists(Path.Combine(m_root, "artifacts")));
        Assert.False(Directory.Exists(Path.Combine(m_root, "selected")));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void BackendDeclarationUsesItsProducerOptionsWithoutGeneratingBindings(bool audio)
    {
        NativeComponentDescriptor owner = audio ? MiniAudioToolchain.componentDescriptor : Sdl3Toolchain.componentDescriptor;
        Write(owner.toolchainProject, "<Project />");
        NativeBuildContext context = Context().WithComponentOptions(owner, new(NativeLibraryKind.Shared));
        NativeBuildRecipe recipe = audio ? MiniAudioToolchain.CreateRecipe(context) : Sdl3Toolchain.CreateRecipe(context);
        Assert.Equal(owner.id, recipe.component);
        Assert.Equal("fixture-target", recipe.targetId);
        Assert.Contains(recipe.declarations, value => value == (audio ? "-DBUILD_SHARED_LIBS=ON" : "-DSDL_SHARED=ON"));
        Assert.DoesNotContain(recipe.inputs, input => input.physicalPath.Contains("BuildUtils", StringComparison.Ordinal)
            || input.physicalPath.Contains("BuildConstants", StringComparison.Ordinal));
        Assert.False(Directory.Exists(Path.Combine(m_root, "artifacts")));
        NativeBuildContext staticContext = context.WithComponentOptions(owner, new(NativeLibraryKind.Static));
        Assert.Throws<NotSupportedException>(() => audio
            ? MiniAudioToolchain.CreateRecipe(staticContext) : Sdl3Toolchain.CreateRecipe(staticContext));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void BridgeBackendDeclarationExcludesHostGeneratedOutput(bool ui)
    {
        NativeComponentDescriptor owner = ui ? UiToolchain.componentDescriptor : TextToolchain.componentDescriptor;
        Write(owner.toolchainProject, "<Project />");
        Write(Path.GetDirectoryName(owner.nativeProject)! + "/Native/Generated/unused.cpp", "unselected host output");
        NativeBuildContext context = Context().WithComponentOptions(owner, new(NativeLibraryKind.Shared));
        NativeBuildRecipe recipe = ui ? UiToolchain.CreateRecipe(context) : TextToolchain.CreateRecipe(context);
        Assert.Equal(owner.id, recipe.component);
        Assert.DoesNotContain(recipe.declarations, value => value.Contains("BRIDGE_ROOT", StringComparison.Ordinal));
        Assert.DoesNotContain(recipe.inputs, input => input.physicalPath.EndsWith("Native", StringComparison.Ordinal)
            || input.physicalPath.Contains("Generated", StringComparison.Ordinal));
        Assert.Contains(recipe.inputs, input => input.physicalPath.EndsWith("CMakeLists.txt", StringComparison.Ordinal));
        Assert.False(Directory.Exists(Path.Combine(m_root, "artifacts")));
    }

    [Fact]
    public void ProductSourceDeclarationUsesEachSelectedOwnerWithoutExecutingProducers()
    {
        int declarations = 0;
        var steps = new List<ProductNativeBuildStep>();
        foreach (string id in new[] { "first", "second" })
        {
            var owner = new NativeComponentDescriptor(id, m_owner.nativeProject, m_owner.toolchainProject);
            steps.Add(new ProductNativeBuildStep(id, owner, new(NativeLibraryKind.Shared), id == "first" ? [] : ["first"],
                static (
                    context,
                    completed,
                    cancellation
                ) => throw new Xunit.Sdk.XunitException("Input declaration must not compile."),
                context => {
                    declarations++;
                    Assert.Equal(NativeLibraryKind.Shared, context.RequireComponentOptions(owner).libraryKind);
                    return new NativeBuildRecipe(owner, id, context.RequireToolchain().targetId,
                        [NativeBuildInput.FromPath(m_root, "input.cpp")], ["selected=" + id]);
                },
                static (
                    relative,
                    target
                ) => relative));
        }
        var plan = new ProductNativeBuildPlan("fixture", steps);
        IReadOnlyList<NativeBuildRecipe> recipes = plan.CreateRecipes(Context());
        Assert.Equal(2, declarations);
        Assert.Equal(new[] { "first", "second" }, System.Linq.Enumerable.Select(recipes, static recipe => recipe.component));
        Assert.False(Directory.Exists(Path.Combine(m_root, "artifacts")));
    }

    [Fact]
    public void StandaloneStepsRejectMissingDeclarationsAndForeignRecipes()
    {
        Assert.Throws<ArgumentException>(() => new ProductNativeBuildStep("fixture", m_owner, new(NativeLibraryKind.Shared), [],
            static (
                context,
                completed,
                cancellation
            ) => throw new Xunit.Sdk.XunitException("Validation must not compile."), null,
            static (
                relative,
                target
            ) => relative));
        var step = new ProductNativeBuildStep("fixture", m_owner, new(NativeLibraryKind.Shared), [],
            static (
                context,
                completed,
                cancellation
            ) => throw new Xunit.Sdk.XunitException("Validation must not compile."),
            context => new NativeBuildRecipe(m_owner, "foreign", context.RequireToolchain().targetId, [], []),
            static (
                relative,
                target
            ) => relative);
        Assert.Throws<InvalidOperationException>(() => new ProductNativeBuildPlan("fixture", [step]).CreateRecipes(Context()));
    }

    [Fact]
    public async Task MissingGenerationPreventsPublicationBeforeProducerOrStaging()
    {
        NativeBuildContext context = Context();
        bool produced = false;
        await Assert.ThrowsAsync<InvalidOperationException>(() => NativeArtifactPublisher.PublishAsync(context, Recipe(context),
            (
                @scoped,
                output,
                cancellation
            ) => {
                produced = true;
                return Task.CompletedTask;
            }));
        Assert.False(produced);
        Assert.False(Directory.Exists(Path.Combine(m_root, "artifacts")));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task PublisherBindsActualGenerationBytesAndIdentityAfterSourceDeclaration(bool changeBytes)
    {
        string generated = Write("selected/Bindings.cs", "first");
        string bridge = Write("selected/Native/api.cpp", "bridge");
        using NativeBindingGenerationResult firstGeneration = Generation("generation-a", generated, Path.GetDirectoryName(bridge)!);
        NativeBuildContext firstContext = Context().WithBindings(firstGeneration);
        int productions = 0;
        Task Produce(
            NativeBuildContext scoped,
            string output,
            CancellationToken cancellation
        ) {
            productions++;
            return File.WriteAllTextAsync(Path.Combine(output, "fixture.native"), File.ReadAllText(generated), cancellation);
        }
        using NativeBuildProduct first = await NativeArtifactPublisher.PublishAsync(firstContext, Recipe(firstContext), Produce);
        DateTime timestamp = File.GetLastWriteTimeUtc(generated);
        if (changeBytes)
        {
            File.WriteAllText(generated, "other");
            File.SetLastWriteTimeUtc(generated, timestamp);
        }
        using NativeBindingGenerationResult nextGeneration = Generation(changeBytes ? "generation-a" : "generation-b",
            generated, Path.GetDirectoryName(bridge)!);
        NativeBuildContext nextContext = Context().WithBindings(nextGeneration);
        using NativeBuildProduct next = await NativeArtifactPublisher.PublishAsync(nextContext, Recipe(nextContext), Produce);
        Assert.NotEqual(first.fingerprint, next.fingerprint);
        Assert.Equal(2, productions);
        Assert.Equal("first", File.ReadAllText(first.files[0]));
        Assert.Equal(changeBytes ? "other" : "first", File.ReadAllText(next.files[0]));
        using NativeBindingGenerationResult hotGeneration = Generation(changeBytes ? "generation-a" : "generation-b",
            generated, Path.GetDirectoryName(bridge)!);
        NativeBuildContext hotContext = Context().WithBindings(hotGeneration);
        using NativeBuildProduct hot = await NativeArtifactPublisher.PublishAsync(hotContext, Recipe(hotContext), Produce);
        Assert.Equal(next.fingerprint, hot.fingerprint);
        Assert.Equal(2, productions);
    }

    public void Dispose()
    {
        if (Directory.Exists(m_root))
            Directory.Delete(m_root, recursive: true);
    }

    private NativeBuildContext Context() => new NativeBuildContext(m_root, "release")
        .WithToolchain(new NativeToolchainSelection("fixture-target", new BuildHostDescriptor("Windows", "x64"),
            new Dictionary<string, string> { ["compiler"] = Path.Combine(m_root, "tools", "compiler") },
            new Dictionary<string, string>(), [], [], new NativeLinkPlan(".dll"), false))
        .WithComponentOptions(m_owner, new(NativeLibraryKind.Shared));

    private NativeBuildRecipe Recipe(NativeBuildContext context)
    {
        NativeBuildRecipe recipe = NativeBuildRecipe.CreateForComponent(context, m_owner, "fixture", "fixture-target",
            [Path.Combine(m_root, "input.cpp")], ["compiler-options"], [Path.Combine(m_root, "build", "Producer.cs")]);
        foreach (NativeBuildInput input in recipe.inputs)
            if (!File.Exists(input.physicalPath) && !Directory.Exists(input.physicalPath))
                Write(Path.GetRelativePath(m_root, input.physicalPath), "common executor");
        return recipe;
    }

    private NativeBindingGenerationResult Generation(
        string fingerprint,
        string generated,
        string bridge
    ) {
        File.WriteAllText(Path.Combine(bridge, "compilation.json"), JsonSerializer.Serialize(new NativeBindingCompilation
        {
            roots = [new("generated", "", true, true)], files = [], includeDirectories = [], defines = [], compilerDefines = [],
            cxxStandard = 0, cStandard = 0, requiredExports = [], staticExportSymbols = [], staticExportAbi = "", staticExportNames = []
        }));
        return new(new Dictionary<string, NativeBindingGenerationDescriptor>
        {
            [m_owner.nativeProject] = new()
            {
                fingerprint = fingerprint, bindingsPath = generated, nativeDirectory = bridge,
                usesRuntimeImports = false,
                artifactDirectory = string.Empty, artifactOwnerDirectory = string.Empty
            }
        }, []);
    }

    private string Write(
        string relative,
        string content
    ) {
        string file = Path.Combine(m_root, relative);
        Directory.CreateDirectory(Path.GetDirectoryName(file)!);
        File.WriteAllText(file, content);
        return file;
    }
}
