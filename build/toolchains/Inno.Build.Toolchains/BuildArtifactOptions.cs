using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Inno.Build.Toolchains;

/// <summary>
/// Defines the deterministic filter and naming policy used to collect one native product.
/// </summary>
/// <param name="buildDirName">
/// The dependency-relative directory searched for native build outputs.
/// </param>
/// <param name="libraryTokens">
/// File-name tokens that identify artifacts belonging to the product.
/// </param>
/// <param name="extensions">
/// The accepted native artifact extensions.
/// </param>
/// <param name="requiredPathTokens">
/// Optional normalized path tokens used to reject unrelated intermediate files.
/// </param>
/// <param name="normalizeOutputName">
/// The deterministic output naming policy.
/// </param>
public sealed record BuildArtifactOptions(
    string buildDirName,
    IReadOnlyCollection<string> libraryTokens,
    IReadOnlyCollection<string> extensions,
    IReadOnlyCollection<string>? requiredPathTokens,
    Func<string, string, string> normalizeOutputName
);

/// <summary>
/// Copies filtered native outputs into the engine's rebuildable dependency store.
/// </summary>
public static class BuildArtifactCopier
{
    private const string DSYM_TOKEN = ".dSYM";

    /// <summary>
    /// Copies all artifacts accepted by one product policy into its output directory.
    /// </summary>
    /// <param name="buildRoot">
    /// The native dependency root containing the configured build directory.
    /// </param>
    /// <param name="outputDir">
    /// The destination directory receiving normalized artifacts.
    /// </param>
    /// <param name="config">
    /// The normalized native build configuration.
    /// </param>
    /// <param name="options">
    /// The immutable filtering and naming policy.
    /// </param>
    public static void CopyArtifacts(
        string buildRoot,
        string outputDir,
        string config,
        BuildArtifactOptions options
    ) {
        var buildDir = Path.Combine(buildRoot, options.buildDirName);
        if (!Directory.Exists(buildDir))
            throw new DirectoryNotFoundException($"Native artifact directory not found: {buildDir}");

        var candidates = Directory.EnumerateFiles(buildDir, "*", SearchOption.AllDirectories)
            .Where(path =>
            {
                if (path.Contains(DSYM_TOKEN, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                var ext = Path.GetExtension(path);
                if (!options.extensions.Contains(ext, StringComparer.OrdinalIgnoreCase))
                {
                    return false;
                }

                var fileName = Path.GetFileName(path);
                if (!ToolchainEnvironment.ContainsAny(fileName, options.libraryTokens.ToArray())
                    || !MatchesConfiguration(Path.GetRelativePath(buildDir, path), config))
                {
                    return false;
                }

                if (options.requiredPathTokens == null || options.requiredPathTokens.Count == 0)
                {
                    return true;
                }

                var normalized = path.Replace('\\', '/');
                return ToolchainEnvironment.ContainsAny(normalized, options.requiredPathTokens.ToArray());
            })
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (candidates.Count == 0)
            throw new FileNotFoundException($"No {config} native artifacts found under {buildDir}.");

        var artifacts = candidates.Select(source => (
            source,
            name: options.normalizeOutputName(Path.GetFileName(source), config))).ToArray();
        var duplicate = artifacts.GroupBy(static artifact => artifact.name, StringComparer.OrdinalIgnoreCase)
            .FirstOrDefault(static group => group.Count() > 1);
        if (duplicate is not null)
            throw new InvalidOperationException($"Multiple native artifacts produce '{duplicate.Key}'.");

        Directory.CreateDirectory(outputDir);
        foreach (var artifact in artifacts)
            File.Copy(artifact.source, Path.Combine(outputDir, artifact.name), overwrite: true);
    }

    private static bool MatchesConfiguration(
        string path,
        string config
    ) {
        string name = Path.GetFileNameWithoutExtension(path);
        if (name.EndsWith("Debug", StringComparison.OrdinalIgnoreCase))
            return config.Equals("debug", StringComparison.OrdinalIgnoreCase);
        if (name.EndsWith("Release", StringComparison.OrdinalIgnoreCase))
            return config.Equals("release", StringComparison.OrdinalIgnoreCase);
        string? directory = Path.GetDirectoryName(path);
        if (string.IsNullOrEmpty(directory))
            return true;
        string[] segments = directory.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        for (int index = segments.Length - 1; index >= 0; index--)
        {
            if (segments[index].Equals("Debug", StringComparison.OrdinalIgnoreCase))
                return config.Equals("debug", StringComparison.OrdinalIgnoreCase);
            if (segments[index].Equals("Release", StringComparison.OrdinalIgnoreCase))
                return config.Equals("release", StringComparison.OrdinalIgnoreCase);
        }
        return true;
    }
}
