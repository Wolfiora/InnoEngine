using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Inno.Scripting.Compiler;

internal static class FrameworkReferenceResolver
{
    internal static ScriptCompilationEnvironment Resolve(CancellationToken cancellationToken = default)
    {
        string referenceDirectory = FindReferenceDirectory();
        var references = new List<MetadataReference>();
        var names = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "System.Private.CoreLib" };
        using var hash = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
        foreach (string identity in new[]
                 {
                     ScriptCompilationEnvironment.C_TARGET_FRAMEWORK,
                     ScriptCompilationEnvironment.C_LANGUAGE_VERSION,
                     typeof(CSharpCompilation).Module.ModuleVersionId.ToString("N"),
                     typeof(FrameworkReferenceResolver).Module.ModuleVersionId.ToString("N"),
                     RuntimeInformation.FrameworkDescription
                 })
        {
            hash.AppendData(Encoding.UTF8.GetBytes(identity));
            hash.AppendData([0]);
        }
        foreach (string path in Directory.EnumerateFiles(referenceDirectory, "*.dll")
                     .OrderBy(static path => Path.GetFileName(path), StringComparer.Ordinal))
        {
            cancellationToken.ThrowIfCancellationRequested();
            hash.AppendData(Encoding.UTF8.GetBytes(Path.GetFileName(path)));
            hash.AppendData([0]);
            byte[] image = File.ReadAllBytes(path);
            hash.AppendData(SHA256.HashData(image));
            references.Add(MetadataReference.CreateFromImage(
                ImmutableCollectionsMarshal.AsImmutableArray(image), filePath: path));
            names.Add(Path.GetFileNameWithoutExtension(path));
        }
        return new ScriptCompilationEnvironment(
            references.AsReadOnly(), names, Convert.ToHexStringLower(hash.GetHashAndReset()));
    }

    private static string FindReferenceDirectory()
    {
        const string targetFramework = ScriptCompilationEnvironment.C_TARGET_FRAMEWORK;
        foreach (string dotnetRoot in GetDotnetRoots())
        {
            string packRoot = Path.Combine(dotnetRoot, "packs", "Microsoft.NETCore.App.Ref");
            if (!Directory.Exists(packRoot))
                continue;
            string? directory = Directory
                .EnumerateDirectories(packRoot)
                .Select(path => new
                {
                    path,
                    version = ParseVersion(Path.GetFileName(path))
                })
                .Where(candidate => candidate.version is not null &&
                                    candidate.version.Major == 10 && candidate.version.Minor == 0 &&
                                    !Path.GetFileName(candidate.path).Contains('-', StringComparison.Ordinal))
                .OrderByDescending(static candidate => candidate.version)
                .Select(candidate => Path.Combine(candidate.path, "ref", targetFramework))
                .FirstOrDefault(Directory.Exists);
            if (directory is not null)
                return directory;
        }
        throw new InvalidOperationException(
            $"The {targetFramework} reference pack is required to generate script API assemblies. " +
            "Install the matching .NET SDK, publish the Editor with its reference pack, or configure DOTNET_ROOT.");
    }

    private static IEnumerable<string> GetDotnetRoots()
    {
        var roots = new List<string>();
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        Add(AppContext.BaseDirectory);
        Add(Environment.GetEnvironmentVariable("DOTNET_ROOT"));
        Add(Environment.GetEnvironmentVariable("DOTNET_ROOT_X64"));
        Add(Environment.GetEnvironmentVariable("DOTNET_ROOT_ARM64"));
        string runtimeDirectory = Path.GetFullPath(RuntimeEnvironment.GetRuntimeDirectory());
        Add(Path.GetFullPath(Path.Combine(runtimeDirectory, "..", "..", "..")));
        return roots;

        void Add(string? path)
        {
            if (!string.IsNullOrWhiteSpace(path) && Directory.Exists(path))
            {
                string root = Path.TrimEndingDirectorySeparator(Path.GetFullPath(path));
                if (seen.Add(root))
                    roots.Add(root);
            }
        }
    }

    private static Version? ParseVersion(string value)
        => Version.TryParse(value, out Version? result) ? result : null;
}
