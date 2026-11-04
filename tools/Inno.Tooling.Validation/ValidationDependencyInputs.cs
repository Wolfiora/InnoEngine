using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
using Inno.Core.IO;

namespace Inno.Tooling.Validation;

internal sealed class ValidationDependencyInputs(
    ValidationPlan plan,
    CancellationToken cancellation
) {
    private readonly Dictionary<string, string> m_hashes = new(StringComparer.Ordinal);
    private readonly Dictionary<string, string[]> m_packages = new(StringComparer.Ordinal);

    internal static bool Required(ValidationCheck check)
        => check.kind is "build" or "test" or "api" or "wiki" or "products" or "support-pack" or "release-bundle"
            && check.inputs.Any(static input => input.StartsWith('@') && input != "@tooling");

    internal void Reset()
    {
        m_hashes.Clear();
        m_packages.Clear();
    }

    internal bool Matches(
        ValidationCheck check,
        string identity
    ) {
        try { return Capture(check) == identity; }
        catch (Exception failure) when (failure is IOException or UnauthorizedAccessException) { return false; }
    }

    internal string Capture(
        ValidationCheck check,
        bool allowMissing = false
    ) {
        if (!Required(check))
            return string.Empty;
        var inputs = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (string declared in ValidationCompiledGraph.Select(plan, check)
            .Select(static instance => instance.GetProperty("ProjectAssetsFile").GetString()!).Distinct(StringComparer.Ordinal))
        {
            string assets = declared;
            if (string.IsNullOrWhiteSpace(assets))
                throw new InvalidDataException("The SDK did not declare restored inputs for " + check.id);
            assets = Path.GetFullPath(assets);
            inputs[assets] = Hash(assets);
            if (!File.Exists(assets))
            {
                if (!allowMissing)
                    throw new InvalidDataException("Required restored input is unavailable: " + assets);
                continue;
            }
            if (!m_packages.TryGetValue(assets, out string[]? packages))
            {
                using JsonDocument document = JsonDocument.Parse(File.ReadAllBytes(assets));
                string[] folders = document.RootElement.GetProperty("packageFolders").EnumerateObject()
                    .Select(static folder => Path.GetFullPath(folder.Name)).ToArray();
                var selected = new HashSet<string>(StringComparer.Ordinal);
                foreach (JsonProperty library in document.RootElement.GetProperty("libraries").EnumerateObject())
                {
                    if (library.Value.GetProperty("type").GetString() != "package")
                        continue;
                    string relative = library.Value.GetProperty("path").GetString()!;
                    string[] candidates = folders.Select(folder => PathBoundary.Resolve(folder, relative)).ToArray();
                    selected.Add(candidates.FirstOrDefault(Directory.Exists)
                        ?? (allowMissing && candidates.Length != 0 ? candidates[0]
                            : throw new InvalidDataException("A restored package is unavailable: " + library.Name)));
                }
                m_packages.Add(assets, packages = selected.Order(StringComparer.Ordinal).ToArray());
            }
            foreach (string package in packages)
            {
                if (!allowMissing && !Directory.Exists(package))
                    throw new InvalidDataException("A restored package is unavailable: " + package);
                inputs[package] = Hash(package);
            }
        }
        if (inputs.Count == 0)
            throw new InvalidDataException("Required restored dependency discovery was empty: " + check.id);
        return ValidationInputs.Identity(inputs);
    }

    private string Hash(string path)
    {
        if (m_hashes.TryGetValue(path, out string? cached))
            return cached;
        cancellation.ThrowIfCancellationRequested();
        string identity;
        if (Directory.Exists(path))
        {
            var files = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach (string file in PathBoundary.EnumerateFiles(path).Order(StringComparer.Ordinal))
            {
                cancellation.ThrowIfCancellationRequested();
                using FileStream stream = File.OpenRead(file);
                files[Path.GetRelativePath(path, file).Replace('\\', '/')] = Convert.ToHexStringLower(SHA256.HashData(stream));
            }
            identity = ValidationInputs.Identity(files);
        }
        else if (File.Exists(path))
        {
            using FileStream stream = File.OpenRead(path);
            identity = Convert.ToHexStringLower(SHA256.HashData(stream));
        }
        else
            identity = "missing";
        m_hashes.Add(path, identity);
        return identity;
    }
}
