using System;
using System.IO;
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
    public static void Compile(
        string assetRoot,
        string shaderPath,
        BgfxShaderTargetPlatform platform,
        GraphicsApi backend,
        string outputFile
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(assetRoot);
        ArgumentException.ThrowIfNullOrWhiteSpace(shaderPath);
        ArgumentException.ThrowIfNullOrWhiteSpace(outputFile);
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
            using var modules = new ModuleHost(new() { cacheDirectory = Path.Combine(scratch, "Modules") });
            using var types = new TypeCatalog(modules);
            using var serialization = new SerializationRegistry(types);
            using var assets = new AssetPipeline(modules, types, serialization, identities, diagnostics, logs,
                new() { assetRoot = Path.GetFullPath(assetRoot), libraryRoot = Path.Combine(scratch, "Library") });
            AssetPath path = AssetPath.Project(shaderPath);
            // A fresh source import must succeed. An earlier build artifact never masks current source errors.
            if (!assets.Import(path))
                throw new InvalidDataException($"Shader source '{path}' failed to import.");
            ShaderAsset shader = assets.Load<ShaderAsset>(path);
            GraphicsCapabilities capabilities = BgfxTargetCapabilities.Create(platform, backend);
            var compiler = new ShaderCompiler(new BgfxShadercToolchain(platform));
            ShaderCompileTarget target = compiler.CreateTarget(capabilities);
            ShaderCompilationResult result = compiler.CompileGraphAsync(shader, target, RenderShaderVariant.empty,
                types, serialization, AssetSerializationContext.Create(assets), assets).AsTask().GetAwaiter().GetResult();
            foreach (ShaderDiagnostic diagnostic in result.diagnostics)
                Console.Error.WriteLine($"{diagnostic.code}: {diagnostic.message}");
            if (!result.succeeded || result.artifact is null)
                throw new InvalidDataException("Shader compilation failed; the output was not replaced.");
            AtomicFile.WriteAllBytes(Path.GetFullPath(outputFile), RenderShaderArtifactCodec.Encode(result.artifact.CreateRuntimeArtifact()));
            Console.WriteLine($"Compiled {path}: {result.artifact.passes.Count} passes, {target.key}");
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
