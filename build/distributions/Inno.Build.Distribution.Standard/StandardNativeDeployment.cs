using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Inno.Build.Toolchains;
using Inno.Native.LibraryLoading;

namespace Inno.Build.Distribution.Standard;

internal static class StandardNativeDeployment
{
    internal static IReadOnlyDictionary<string, byte[]> CreateMetadata(
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
            if (product.sharedLibraryExtension is not string extension || !name.EndsWith(extension, StringComparison.Ordinal))
                continue;
            string stem = name[..^extension.Length];
            Add(libraries, stem, portable);
            // The standard distribution accepts both declared vendor stems and conventional lib-prefixed stems.
            // A custom distribution can provide its own exact aliases through the same metadata factory.
            if (stem.StartsWith("lib", StringComparison.Ordinal) && stem.Length > 3)
                Add(libraries, stem[3..], portable);
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
}
