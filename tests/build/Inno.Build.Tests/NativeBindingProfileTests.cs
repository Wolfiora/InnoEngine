using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BGCS.Configuration;
using BGCS.Core;
using BGCS.Core.Targets;
using BGCS.Extension.BrowserWasm;
using Inno.Build.Bindings;
using Inno.Build.Toolchains;
using Xunit;

namespace Inno.Build.Tests;

/// <summary>
/// Exercises platform-owned SDK extension registration through the public generation boundary.
/// </summary>
[Trait("ExternalDependency", "BGCS")]
public sealed class NativeBindingProfileTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoBindingProfile", Guid.NewGuid().ToString("N"));

    /// <summary>
    /// Requires an explicit Browser extension and publishes aggregate pointer adapters with remappable inputs.
    /// </summary>
    /// <returns>The completion of generation, verification and a repeated cache hit.</returns>
    [Fact]
    public async Task BrowserAggregateBridgeRequiresExplicitExtensionAndReusesVerifiedGeneration()
    {
        Directory.CreateDirectory(Path.Combine(m_root, "Source"));
        File.WriteAllText(Path.Combine(m_root, "InnoEngine.sln"), string.Empty);
        File.WriteAllText(Path.Combine(m_root, "Fixture.csproj"), "<Project />");
        File.WriteAllText(Path.Combine(m_root, "Source", "api.h"),
            "typedef struct Pair { int first; int second; } Pair; Pair swap_pair(Pair value);");
        File.WriteAllText(Path.Combine(m_root, "bindings.props"),
            "<Project><PropertyGroup><InnoNativeComponentId>fixture</InnoNativeComponentId>" +
            "<InnoBindingConfig>bindgen.json</InnoBindingConfig></PropertyGroup></Project>");
        File.WriteAllText(Path.Combine(m_root, "bindgen.json"), """
            {"schemaVersion":2,"library":{"id":"fixture","nativeName":"fixture"},
             "source":{"language":"c","headers":["Source/api.h"]},
             "api":{"namespace":"Fixture.Native","name":"Api"},
             "output":{"layout":"singleFile","singleFileName":"Bindings.cs"}}
            """);
        var component = new NativeComponentDescriptor("fixture", "Fixture.csproj", "Fixture.csproj", bindingDefinition: "bindings.props");
        var request = new NativeBindingGenerationRequest([component], "fixture-target", NativeBindingOutputMode.TargetArtifacts);
        var target = new TargetConfiguration { id = "emscripten-wasm32-emscripten" };
        var runtime = new ManagedRuntimeId("dotnet.mono-wasm");
        var missing = new NativeBindingGenerator(new NativeBindingProfile("fixture-target", target, runtime,
            importMode: BindingImportMode.LibraryImport));
        await Assert.ThrowsAsync<InvalidDataException>(async () =>
        {
            using NativeBindingGenerationResult rejected = await missing.GenerateAsync(
                new NativeBuildContext(m_root, "release"), request, CancellationToken.None);
        });
        Assert.False(Directory.Exists(Path.Combine(m_root, "artifacts", "bindings")));

        var generator = new NativeBindingGenerator(new NativeBindingProfile("fixture-target", target, runtime,
            configureGenerator: static builder => builder.AddPlugin<BrowserWasmPlugin>(),
            importMode: BindingImportMode.LibraryImport));
        NativeBuildContext cold = new(m_root, "release");
        using NativeBindingGenerationResult first = await generator.GenerateAsync(cold, request, CancellationToken.None);
        NativeBindingGenerationDescriptor generation = first[component.nativeProject];
        NativeBindingCompilation compilation = NativeBindingCompilation.Load(generation.nativeDirectory);
        Assert.Contains(compilation.files, static file => file.compile && file.rootId == "generated");
        Assert.True(generation.usesRuntimeImports);
        Assert.NotEmpty(compilation.requiredExports);
        Assert.Empty(compilation.staticExportSymbols);
        Assert.All(compilation.roots.Where(static root => root.frozen), static root => Assert.False(Path.IsPathFullyQualified(root.directory)));
        Assert.Equal(1, cold.statistics.bindingGenerations);

        byte[] published = File.ReadAllBytes(generation.bindingsPath);
        var unsupported = new NativeBindingGenerator(new NativeBindingProfile("fixture-target", target, runtime,
            configureGenerator: static builder => builder.AddPlugin<BrowserWasmPlugin>(),
            importMode: BindingImportMode.FunctionTable));
        InvalidDataException importFailure = await Assert.ThrowsAsync<InvalidDataException>(async () =>
        {
            using NativeBindingGenerationResult rejected = await unsupported.GenerateAsync(
                new NativeBuildContext(m_root, "release"), request, CancellationToken.None);
        });
        Assert.Contains("FunctionTable", importFailure.Message, StringComparison.Ordinal);
        Assert.Equal(published, File.ReadAllBytes(generation.bindingsPath));

        NativeBuildContext hot = new(m_root, "release");
        using NativeBindingGenerationResult repeated = await generator.GenerateAsync(hot, request, CancellationToken.None);
        Assert.Equal(generation.fingerprint, repeated[component.nativeProject].fingerprint);
        Assert.Equal(0, hot.statistics.bindingGenerations);
    }

    /// <summary>
    /// Removes only this fixture's owned temporary workspace after all readers have drained.
    /// </summary>
    public void Dispose()
    {
        if (Directory.Exists(m_root))
            Directory.Delete(m_root, true);
    }
}
