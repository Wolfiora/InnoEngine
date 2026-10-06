using Inno.Adapter.Modules.DotNet;
using Inno.Adapter.Serialization.DotNet;
using System;
using System.Threading;
using System.IO;
using System.Linq;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Build.Toolchains.Bgfx.Tools;
using Inno.Core.Diagnostics;
using Inno.Core.Identity;
using Inno.Core.IO;
using Inno.Core.Logging;
using Inno.Core.Serialization;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Types;
using Inno.Rendering;
using Inno.Rendering.Assets;
using Inno.Rendering.Assets.Authoring;

namespace Inno.Build.Toolchains.Bgfx.Shaders;

/// <summary>
/// Compiles authored shader graphs through the standard import, IR and artifact pipeline.
/// </summary>
public static class ShaderArtifactBuilder
{
    /// <summary>
    /// Imports and compiles one shader graph for an explicitly selected graphics target.
    /// </summary>
    /// <param name="assetRoot">
    /// The directory containing shader authoring sources.
    /// </param>
    /// <param name="shaderPath">
    /// The source-local shader graph path.
    /// </param>
    /// <param name="platform">
    /// The native compiler's target platform.
    /// </param>
    /// <param name="backend">
    /// The graphics API for the compiled artifact.
    /// </param>
    /// <param name="outputFile">
    /// The artifact destination, replaced atomically after successful compilation.
    /// </param>
    /// <exception cref="InvalidDataException">
    /// Import or shader compilation fails.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// A required authoring path or output path is blank.
    /// </exception>
    /// <param name="tools">
    /// The host-selected executable distribution, or null for the application's deployed tools.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels import and native compilation before replacing the destination artifact.
    /// </param>
    public static void Compile(
        string assetRoot,
        string shaderPath,
        BgfxShaderTargetPlatform platform,
        GraphicsApi backend,
        string outputFile,
        ToolRunner? tools = null,
        CancellationToken cancellationToken = default
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(outputFile);
        RenderShaderArtifact artifact = CompileArtifact(assetRoot, shaderPath, platform, backend, tools, cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();
        AtomicFile.WriteAllBytes(Path.GetFullPath(outputFile), RenderShaderArtifactCodec.Encode(artifact));
    }

    /// <summary>
    /// Compiles one raster graph into device-only program facts for an embedded adapter distribution.
    /// </summary>
    /// <param name="assetRoot">
    /// The directory containing the graph and its source dependencies.
    /// </param>
    /// <param name="shaderPath">
    /// The source-local graph whose sole raster pass becomes the device program.
    /// </param>
    /// <param name="platform">
    /// The compiler's native target platform.
    /// </param>
    /// <param name="backend">
    /// The graphics API for the program binaries.
    /// </param>
    /// <param name="outputFile">
    /// The device artifact destination, atomically replaced after successful compilation.
    /// </param>
    /// <exception cref="ArgumentException">
    /// A required authoring or output path is blank.
    /// </exception>
    /// <exception cref="InvalidDataException">
    /// Compilation fails or the graph does not define exactly one raster program.
    /// </exception>
    /// <param name="tools">
    /// The host-selected executable distribution, or null for the application's deployed tools.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels import and native compilation before replacing the destination artifact.
    /// </param>
    public static void CompileGraphicsProgram(
        string assetRoot,
        string shaderPath,
        BgfxShaderTargetPlatform platform,
        GraphicsApi backend,
        string outputFile,
        ToolRunner? tools = null,
        CancellationToken cancellationToken = default
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(outputFile);
        RenderShaderArtifact artifact = CompileArtifact(assetRoot, shaderPath, platform, backend, tools, cancellationToken);
        if (artifact.passes.Count != 1 || artifact.passes[0].programKind != ShaderProgramKind.Raster)
            throw new InvalidDataException("A device graphics program requires exactly one raster pass.");
        RenderShaderPassArtifact pass = artifact.passes[0];
        RenderShaderBindingDescriptor[] bindings = pass.shaderInterface.bindings.Select(static binding => new RenderShaderBindingDescriptor(
            new RenderBindingId(binding.id.value),
            binding.bindingKind switch
            {
                ShaderPropertyBindingKind.Uniform => RenderShaderBindingKind.Uniform,
                ShaderPropertyBindingKind.SampledTexture => RenderShaderBindingKind.Texture,
                ShaderPropertyBindingKind.StorageTexture => RenderShaderBindingKind.StorageTexture,
                ShaderPropertyBindingKind.StorageBuffer => RenderShaderBindingKind.StorageBuffer,
                _ => throw new InvalidDataException("A device program contains an unsupported binding domain.")
            },
            slot: binding.location ?? 0,
            uniformType: binding.type == ShaderPropertyType.Matrix4x4 ? RenderUniformType.Matrix4x4 : RenderUniformType.Vector4,
            count: binding.arrayCount,
            storageAccess: binding.storageAccess,
            nativeName: binding.nativeName)).ToArray();
        var descriptor = new GraphicsPipelineDescriptor(
            pass.stages.Single(static stage => stage.stage == ShaderStage.Vertex).bytes.Span,
            pass.stages.Single(static stage => stage.stage == ShaderStage.Fragment).bytes.Span,
            bindings, null, pass.rasterState);
        cancellationToken.ThrowIfCancellationRequested();
        AtomicFile.WriteAllBytes(Path.GetFullPath(outputFile), GraphicsProgramArtifactCodec.Encode(descriptor));
    }

    private static RenderShaderArtifact CompileArtifact(
        string assetRoot,
        string shaderPath,
        BgfxShaderTargetPlatform platform,
        GraphicsApi backend,
        ToolRunner? tools,
        CancellationToken cancellationToken
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(assetRoot);
        ArgumentException.ThrowIfNullOrWhiteSpace(shaderPath);
        string scratch = Path.Combine(Path.GetTempPath(), "InnoShaderBuild", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(scratch);
        try
        {
            var identities = new IdentityAllocator();
            using IDisposable identityScope = identities.EnterScope();
            var diagnostics = new DiagnosticHub();
            using IDisposable diagnosticScope = diagnostics.EnterScope();
            diagnostics.RegisterSink(new DiagnosticOutput());
            using var logs = new LogRouter();
            using var modules = new ModuleHost(new() { catalogSource = new DotNetAssemblyCatalogSource(typeof(ShaderArtifactBuilder).Assembly)});
            using var types = new TypeCatalog(modules, new ReflectionTypeCatalogSource());
            using var serialization = new SerializationRegistry(types, new ReflectionSerializationMetadataSource());
            using var assets = new AssetPipeline(modules, types, serialization, identities, diagnostics, logs,
                new() { assetRoot = Path.GetFullPath(assetRoot), libraryRoot = Path.Combine(scratch, "Library") });
            AssetPath path = AssetPath.Project(shaderPath);
            // A fresh source import must succeed. An earlier build artifact never masks current source errors.
            if (!assets.Import(path))
                throw new InvalidDataException($"Shader source '{path}' failed to import.");
            ShaderAsset shader = assets.Load<ShaderAsset>(path);
            GraphicsCapabilities capabilities = BgfxTargetCapabilities.Create(platform, backend);
            var compiler = new ShaderCompiler(new BgfxShadercToolchain(platform, tools));
            ShaderCompileTarget target = compiler.CreateTarget(capabilities);
            ShaderCompilationResult result = compiler.CompileGraphAsync(shader, target, RenderShaderVariant.empty,
                types, serialization, AssetSerializationContext.Create(assets), assets, cancellationToken).AsTask().GetAwaiter().GetResult();
            foreach (ShaderDiagnostic diagnostic in result.diagnostics)
                Console.Error.WriteLine($"{diagnostic.code}: {diagnostic.message}");
            if (!result.succeeded || result.artifact is null)
                throw new InvalidDataException("Shader compilation failed; the output was not replaced.");
            Console.WriteLine($"Compiled {path}: {result.artifact.passes.Count} passes, {target.key}");
            return result.artifact.CreateRuntimeArtifact();
        }
        finally
        {
            Directory.Delete(scratch, recursive: true);
        }
    }

    private sealed class DiagnosticOutput : IDiagnosticSink
    {
        /// <summary>
        /// Records errors from the current diagnostic report.
        /// </summary>
        /// <param name="report">
        /// The report consumed by replace; ownership remains with the caller unless explicitly stated otherwise.
        /// </param>
        public void Replace(DiagnosticReport report)
        {
            foreach (Diagnostic diagnostic in report.diagnostics)
                if (diagnostic.severity == DiagnosticSeverity.Error)
                    Console.Error.WriteLine($"{diagnostic.code}: {diagnostic.message}");
        }
        /// <summary>
        /// Ignores report removal because console output retains no diagnostic state.
        /// </summary>
        /// <param name="source">
        /// The diagnostic source whose report was removed.
        /// </param>
        public void Clear(DiagnosticSource source) { }
    }
}
