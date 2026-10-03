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
public static class BgfxToolsBuild
{
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
        var repoRoot = context.engineRoot;
        var externDir = Path.Combine(repoRoot, ToolchainLayout.C_EXTERNAL_DIRECTORY_NAME);
        var bgfxDir = Path.Combine(externDir, BgfxBuildConstants.BGFX_DIR_NAME);
        var bxDir = Path.Combine(externDir, BgfxBuildConstants.BX_DIR_NAME);
        var bimgDir = Path.Combine(externDir, BgfxBuildConstants.BIMG_DIR_NAME);
        var builder = BgfxBuilderFactory.CreateForCurrentPlatform();
        var outputDir = Path.Combine(repoRoot, ToolchainLayout.C_OUTPUT_DIRECTORY_NAME, BgfxBuildConstants.OUTPUT_PRODUCT_DIR_NAME, builder.outputPlatform);

        Directory.CreateDirectory(externDir);
        Directory.CreateDirectory(outputDir);

        BgfxBuildUtils.ValidateSubmodules(bgfxDir, bxDir, bimgDir);

        EnsureBgfxBuilt(outputDir, configuration);
        await builder.BuildToolsAsync(bgfxDir, context, cancellationToken);
        cancellationToken.ThrowIfCancellationRequested();
        CopyTools(bgfxDir, outputDir, builder, configuration);

        Console.WriteLine($"bgfx tools build complete. Output: {outputDir}");
    }

    private static void CopyTools(
        string bgfxDir,
        string outputDir,
        BgfxBuilder builder,
        string config
    ) {
        var buildDir = Path.Combine(bgfxDir, BgfxBuildConstants.BUILD_DIR_NAME);
        if (!Directory.Exists(buildDir))
        {
            return;
        }

        var toolDir = Path.Combine(outputDir, "tools");
        Directory.CreateDirectory(toolDir);
        DeleteExistingTools(toolDir, config);

        var toolNames = new[]
        {
            "shaderc",
            "geometryc",
            "geometryv",
            "texturec",
            "texturev",
        };

        var candidates = Directory.EnumerateFiles(buildDir, "*", SearchOption.AllDirectories)
            .Where(path =>
            {
                var fileName = Path.GetFileName(path);
                if (!toolNames.Any(name => fileName.StartsWith(name, StringComparison.OrdinalIgnoreCase)
                                           || fileName.StartsWith($"{name}.exe", StringComparison.OrdinalIgnoreCase)))
                {
                    return false;
                }

                var normalized = path.Replace('\\', '/');
                if (!normalized.Contains(builder.artifactPathToken, StringComparison.OrdinalIgnoreCase))
                    return false;
                string stem = Path.GetFileNameWithoutExtension(path);
                if (stem.EndsWith("Debug", StringComparison.OrdinalIgnoreCase))
                    return config == ToolchainLayout.C_DEBUG_CONFIGURATION;
                if (stem.EndsWith("Release", StringComparison.OrdinalIgnoreCase))
                    return config == ToolchainLayout.C_RELEASE_CONFIGURATION;
                return true;
            })
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        foreach (var src in candidates)
        {
            var destName = NormalizeToolName(Path.GetFileName(src), config);
            var dest = Path.Combine(toolDir, destName);
            File.Copy(src, dest, overwrite: true);
        }
    }

    private static void DeleteExistingTools(
        string toolDir,
        string config
    ) {
        foreach (var path in Directory.EnumerateFiles(toolDir, "*", SearchOption.TopDirectoryOnly))
        {
            if (Path.GetFileNameWithoutExtension(path)
                .EndsWith($"-{config}", StringComparison.OrdinalIgnoreCase))
            {
                File.Delete(path);
            }
        }
    }

    private static string NormalizeToolName(
        string fileName,
        string config
    ) {
        var ext = Path.GetExtension(fileName);
        var baseName = Path.GetFileNameWithoutExtension(fileName);
        var normalized = TrimConfigSuffix(baseName);
        return $"{normalized}-{config}{ext}";
    }

    private static string TrimConfigSuffix(string baseName)
    {
        if (baseName.EndsWith("Release", StringComparison.OrdinalIgnoreCase))
        {
            baseName = baseName[..^"Release".Length];
        }
        else if (baseName.EndsWith("Debug", StringComparison.OrdinalIgnoreCase))
        {
            baseName = baseName[..^"Debug".Length];
        }

        return baseName.TrimEnd('-', '_', '.');
    }

    private static void EnsureBgfxBuilt(
        string outputDir,
        string config
    ) {
        if (!Directory.Exists(outputDir))
        {
            throw new InvalidOperationException("bgfx outputs not found. Run Inno.Build.Toolchains.Bgfx first.");
        }

        var candidates = Directory.EnumerateFiles(outputDir, "*", SearchOption.TopDirectoryOnly)
            .Where(path =>
            {
                var ext = Path.GetExtension(path);
                if (ext is not (".dylib" or ".dll" or ".so" or ".a" or ".lib"))
                {
                    return false;
                }

                var name = Path.GetFileNameWithoutExtension(path);
                return name.EndsWith($"-{config}", StringComparison.OrdinalIgnoreCase);
            })
            .ToList();

        if (candidates.Count == 0)
        {
            throw new InvalidOperationException("bgfx outputs not found. Run Inno.Build.Toolchains.Bgfx first.");
        }
    }

}
