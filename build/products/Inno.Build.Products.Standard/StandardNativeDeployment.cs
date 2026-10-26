using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Inno.Build.Toolchains;
using Inno.Core.IO;
using Inno.Native.LibraryLoading;

namespace Inno.Build.Products.Standard;

/// <summary>
/// Composes validated standard product deployment metadata.
/// </summary>
public static class StandardNativeDeployment
{
    /// <summary>
    /// Creates a validator for the platform-selected native suffix.
    /// </summary>
    /// <param name="target">
    /// The platform target.
    /// </param>
    /// <param name="extension">
    /// The platform runtime-library suffix.
    /// </param>
    /// <returns>
    /// A validator that rejects foreign or incomplete deployment metadata.
    /// </returns>
    public static IPlayerSupportPackValidator CreateValidator(
        BuildTargetId target,
        string extension
    ) => new DeploymentValidator(target, extension);

    /// <summary>
    /// Encodes the exact runtime payload selected by the platform layout.
    /// </summary>
    /// <param name="products">
    /// The validated product closure.
    /// </param>
    /// <param name="payloads">
    /// The exact destination mapping.
    /// </param>
    /// <returns>
    /// Native loading metadata for the complete closure.
    /// </returns>
    public static IReadOnlyDictionary<string, byte[]> CreateMetadata(
        IReadOnlyList<NativeBuildProduct> products,
        IReadOnlyDictionary<string, string> payloads
    ) {
        string target = products.Select(static product => product.targetId).Distinct(StringComparer.Ordinal).Single();
        var libraries = new Dictionary<string, string>(StringComparer.Ordinal);
        var files = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach ((string relative, string source) in payloads.OrderBy(static pair => pair.Key, StringComparer.Ordinal))
        {
            string portable = relative.Replace('\\', '/');
            string name = Path.GetFileName(source);
            Add(files, name, portable);
            Add(files, Path.GetFileNameWithoutExtension(name), portable);
            NativeBuildProduct product = products.Single(product => product.files.Contains(source, StringComparer.Ordinal));
            if (product.artifacts.Single(artifact => artifact.file == source).role != NativeArtifactRole.SharedLibrary
                || product.sharedLibraryExtension is not string extension)
                continue;
            string stem = name[..^extension.Length];
            AddLibraryAliases(libraries, stem, portable);
        }
        var index = new NativeDeploymentIndex(target, libraries, files);
        return new Dictionary<string, byte[]> { [NativeDeploymentIndex.C_FILE_NAME] = index.Encode() };
    }

    private static void Add(
        IDictionary<string, string> mapping,
        string alias,
        string path
    ) {
        if (mapping.TryGetValue(alias, out string? prior) && prior != path)
            throw new InvalidDataException($"Native deployment alias '{alias}' has multiple selected payloads.");
        mapping[alias] = path;
    }

    private static void AddLibraryAliases(
        IDictionary<string, string> libraries,
        string stem,
        string path
    ) {
        Add(libraries, stem, path);
        if (stem.StartsWith("lib", StringComparison.Ordinal) && stem.Length > 3)
            Add(libraries, stem[3..], path);
    }

    private sealed class DeploymentValidator(
        BuildTargetId target,
        string extension
    ) : IPlayerSupportPackValidator
    {
        /// <inheritdoc />
        public void Validate(string directory)
        {
            string root = PathBoundary.Resolve(directory, "native");
            NativeDeploymentIndex index = NativeDeploymentIndex.Decode(File.ReadAllBytes(Path.Combine(root, NativeDeploymentIndex.C_FILE_NAME)));
            if (index.targetId != target.value)
                throw new InvalidDataException("The selected Native deployment metadata has a foreign target.");
            var actual = new HashSet<string>(PathBoundary.EnumerateFiles(root)
                .Where(path => Path.GetFileName(path) != NativeDeploymentIndex.C_FILE_NAME)
                .Select(path => Path.GetRelativePath(root, path).Replace('\\', '/')), StringComparer.Ordinal);
            if (!actual.SetEquals(index.files.Values) || index.libraries.Values.Any(path => !actual.Contains(path)))
                throw new InvalidDataException("Native deployment metadata does not describe the complete selected payload.");
            var expectedLibraries = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach (string path in actual.Where(path => path.EndsWith(extension, StringComparison.Ordinal)))
                AddLibraryAliases(expectedLibraries, Path.GetFileName(path)[..^extension.Length], path);
            if (index.libraries.Count != expectedLibraries.Count
                || expectedLibraries.Any(pair => !index.libraries.TryGetValue(pair.Key, out string? path) || path != pair.Value))
                throw new InvalidDataException("Native deployment metadata does not expose the selected library aliases.");
        }
    }
}
