using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;
using Inno.Build.Toolchains.Bgfx.Platforms;
using Inno.Build.Toolchains;

namespace Inno.Build.Toolchains.Bgfx;

/// <summary>
/// Builds the pinned graphics component through the shared native workflow.
/// </summary>
public static class BgfxNativeBuild
{
    private static readonly string[] S_LIBRARY_TOKENS =
    {
        BgfxBuildConstants.BGFX_DIR_NAME,
        BgfxBuildConstants.BX_DIR_NAME,
        BgfxBuildConstants.BIMG_DIR_NAME,
    };
    private static readonly string[] S_SHARED_EXTENSIONS = { ".dll", ".dylib", ".so" };

    /// <summary>
    /// Builds and installs the native graphics artifacts for the current host.
    /// </summary>
    /// <param name="context">
    /// The checkout and configuration whose sources and outputs belong to this operation.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The configuration is not debug or release.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// A native build process fails.
    /// </exception>
    /// <param name="cancellationToken">
    /// Cancels child processes and prevents artifact installation after cancellation.
    /// </param>
    /// <returns>
    /// Completion after the component has been built and installed in the selected checkout.
    /// </returns>
    /// <exception cref="OperationCanceledException">
    /// The operation was canceled.
    /// </exception>
    public static async Task BuildAsync(
        NativeBuildContext context,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(context);
        cancellationToken.ThrowIfCancellationRequested();
        string configuration = context.configuration;
        var builder = BgfxBuilderFactory.CreateForCurrentPlatform();
        var repoRoot = context.engineRoot;
        var externDir = Path.Combine(repoRoot, ToolchainLayout.C_EXTERNAL_DIRECTORY_NAME);
        var bgfxDir = Path.Combine(externDir, BgfxBuildConstants.BGFX_DIR_NAME);
        var bxDir = Path.Combine(externDir, BgfxBuildConstants.BX_DIR_NAME);
        var bimgDir = Path.Combine(externDir, BgfxBuildConstants.BIMG_DIR_NAME);
        var outputPlatform = builder.outputPlatform;
        var outputDir = Path.Combine(repoRoot, ToolchainLayout.C_OUTPUT_DIRECTORY_NAME, BgfxBuildConstants.OUTPUT_PRODUCT_DIR_NAME, outputPlatform);

        Directory.CreateDirectory(externDir);
        Directory.CreateDirectory(outputDir);

        BgfxBuildUtils.ValidateSubmodules(bgfxDir, bxDir, bimgDir);

        await builder.BuildAsync(bgfxDir, context, string.Empty, cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();
        CopyArtifacts(bgfxDir, outputDir, builder, false, configuration);
        Console.WriteLine($"bgfx build complete. Output: {outputDir}");
    }

    private static void CopyArtifacts(
        string bgfxDir,
        string outputDir,
        BgfxBuilder builder,
        bool includeStatic,
        string config
    ) {
        var extensions = includeStatic
            ? S_SHARED_EXTENSIONS.Concat(new[] { ".a", ".lib" }).ToArray()
            : S_SHARED_EXTENSIONS;

        DeleteExistingConfigurationArtifacts(outputDir, extensions, config);

        var options = new BuildArtifactOptions(
            BgfxBuildConstants.BUILD_DIR_NAME,
            S_LIBRARY_TOKENS,
            extensions,
            new[] { builder.artifactPathToken },
            ToolchainEnvironment.NormalizeOutputName);

        BuildArtifactCopier.CopyArtifacts(bgfxDir, outputDir, config, options);
    }

    private static void DeleteExistingConfigurationArtifacts(
        string outputDir,
        IReadOnlyCollection<string> extensions,
        string config
    ) {
        if (!Directory.Exists(outputDir))
        {
            return;
        }

        foreach (var path in Directory.EnumerateFiles(outputDir, "*", SearchOption.TopDirectoryOnly))
        {
            if (!extensions.Contains(Path.GetExtension(path), StringComparer.OrdinalIgnoreCase))
            {
                continue;
            }

            if (Path.GetFileNameWithoutExtension(path)
                .EndsWith($"-{config}", StringComparison.OrdinalIgnoreCase))
            {
                File.Delete(path);
            }
        }
    }

}
