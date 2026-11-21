using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using Inno.Core.IO;

namespace Inno.Tooling.Validation;

internal sealed class ValidationDependencyInputs(
    ValidationPlan plan,
    CancellationToken cancellation
) {
    private readonly Dictionary<string, string> m_hashes = new(StringComparer.Ordinal);
    private readonly Dictionary<string, KeyValuePair<string, string>[]> m_packages = new(StringComparer.Ordinal);

    internal static bool Required(ValidationCheck check)
        => check.kind is "build"
           or "test"
           or "api"
           or "documentation-api"
           or "documentation-examples"
           or "products"
           or "support-pack"
           or "release-bundle"
           && check.inputs.Any(static input => input.StartsWith('@') && input is not ("@tooling" or "@native-sources"));

    internal void Reset()
    {
        m_hashes.Clear();
        m_packages.Clear();
    }

    internal static string[] RestoredPackageDirectories(
        IEnumerable<ValidationContext> contexts,
        CancellationToken cancellation
    ) {
        var packages = new HashSet<string>(
            OperatingSystem.IsWindows() ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal
        );
        foreach (string assets in contexts.SelectMany(static context => context.sdkGraph.EnumerateArray())
            .SelectMany(static snapshot => snapshot.GetProperty("Instances").EnumerateArray())
            .Select(static instance => instance.GetProperty("ProjectAssetsFile").GetString()!)
            .Where(static path => !string.IsNullOrWhiteSpace(path))
            .Distinct(StringComparer.Ordinal)
        ) {
            cancellation.ThrowIfCancellationRequested();
            if (File.Exists(assets))
            {
                packages.UnionWith(ReadPackages(assets, allowMissing: true).Select(static package => package.Value));
            }
        }
        return packages.Order(StringComparer.Ordinal).ToArray();
    }

    internal bool Matches(
        ValidationCheck check,
        string identity
    ) {
        try
        {
            return Capture(check) == identity;
        }
        catch (Exception failure) when (failure is IOException or UnauthorizedAccessException)
        {
            return false;
        }
    }

    internal string Capture(
        ValidationCheck check,
        bool allowMissing = false
    ) {
        if (!Required(check))
        {
            return string.Empty;
        }
        var inputs = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (string declared in ValidationCompiledGraph.Select(plan, check)
            .Select(static instance => instance.GetProperty("ProjectAssetsFile").GetString()!)
            .Distinct(StringComparer.Ordinal)
        ) {
            string assets = declared;
            if (string.IsNullOrWhiteSpace(assets))
            {
                throw new InvalidDataException("The SDK did not declare restored inputs for " + check.id);
            }
            assets = Path.GetFullPath(assets);
            inputs[ValidationPlanTransport.Logical(plan.root, plan.dotnet, assets)] = HashAssets(assets);
            if (!File.Exists(assets))
            {
                if (!allowMissing)
                {
                    throw new InvalidDataException("Required restored input is unavailable: " + assets);
                }
                continue;
            }
            foreach (string package in Packages(assets, allowMissing).Select(static package => package.Value))
            {
                if (!allowMissing && !Directory.Exists(package))
                {
                    throw new InvalidDataException("A restored package is unavailable: " + package);
                }
                inputs[ValidationPlanTransport.Logical(plan.root, plan.dotnet, package)] = Hash(package);
            }
        }
        if (inputs.Count == 0)
        {
            throw new InvalidDataException("Required restored dependency discovery was empty: " + check.id);
        }
        return ValidationInputs.Identity(inputs);
    }

    private string HashAssets(string path)
    {
        string key = "logical-assets:" + path;
        if (m_hashes.TryGetValue(key, out string? identity))
        {
            return identity;
        }
        cancellation.ThrowIfCancellationRequested();
        identity = File.Exists(path)
                   ? Convert.ToHexStringLower(
                       SHA256.HashData(JsonSerializer.SerializeToUtf8Bytes(LogicalAssets(JsonNode.Parse(File.ReadAllBytes(path)))))
                   )
                   : "missing";
        m_hashes.Add(key, identity);
        return identity;
    }

    private JsonNode? LogicalAssets(JsonNode? node)
    {
        if (node is JsonObject properties)
        {
            var result = new JsonObject();
            foreach (KeyValuePair<string, JsonNode?> property in properties)
            {
                result.Add(ValidationPlanTransport.Logical(plan.root, plan.dotnet, property.Key), LogicalAssets(property.Value));
            }
            return result;
        }
        if (node is JsonArray items)
        {
            return new JsonArray(items.Select(LogicalAssets).ToArray());
        }
        if (node is JsonValue value && value.TryGetValue(out string? text))
        {
            return JsonValue.Create(ValidationPlanTransport.Logical(plan.root, plan.dotnet, text));
        }
        return node?.DeepClone();
    }

    internal Dictionary<string, string> CapturePackages(ValidationCheck check)
    {
        var packages = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (string assets in ValidationCompiledGraph.Select(plan, check)
            .Select(static instance => instance.GetProperty("ProjectAssetsFile").GetString()!)
            .Distinct(StringComparer.Ordinal)
        ) {
            cancellation.ThrowIfCancellationRequested();
            if (string.IsNullOrWhiteSpace(assets) || !File.Exists(assets))
            {
                throw new InvalidDataException("Documentation source provenance requires actual restored dependency inputs.");
            }
            foreach (KeyValuePair<string, string> package in Packages(assets, false))
            {
                if (!Directory.Exists(package.Value))
                {
                    throw new InvalidDataException("A documented source dependency is unavailable: " + package.Key);
                }
                string identity = Hash(package.Value);
                if (packages.TryGetValue(package.Key, out string? previous) && previous != identity)
                {
                    throw new InvalidDataException("The same restored package identity resolves to conflicting contents: " + package.Key);
                }
                packages[package.Key] = identity;
            }
        }
        return packages;
    }

    private KeyValuePair<string, string>[] Packages(
        string assets,
        bool allowMissing
    ) {
        if (m_packages.TryGetValue(assets, out KeyValuePair<string, string>[]? packages))
        {
            return packages;
        }
        packages = ReadPackages(assets, allowMissing);
        m_packages.Add(assets, packages);
        return packages;
    }

    private static KeyValuePair<string, string>[] ReadPackages(
        string assets,
        bool allowMissing
    ) {
        using JsonDocument document = JsonDocument.Parse(File.ReadAllBytes(assets));
        string[] folders = document.RootElement.GetProperty("packageFolders")
            .EnumerateObject()
            .Select(static folder => Path.GetFullPath(folder.Name))
            .ToArray();
        var selected = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (JsonProperty library in document.RootElement.GetProperty("libraries").EnumerateObject())
        {
            if (library.Value.GetProperty("type").GetString() != "package")
            {
                continue;
            }
            string relative = library.Value.GetProperty("path").GetString()!;
            string[] candidates = folders.Select(folder => PathBoundary.Resolve(folder, relative)).ToArray();
            selected.Add(
                relative,
                candidates.FirstOrDefault(Directory.Exists)
                ?? (allowMissing && candidates.Length != 0
                    ? candidates[0]
                    : throw new InvalidDataException("A restored package is unavailable: " + library.Name))
            );
        }
        return selected.OrderBy(static pair => pair.Key, StringComparer.Ordinal).ToArray();
    }

    private string Hash(string path)
    {
        if (m_hashes.TryGetValue(path, out string? cached))
        {
            return cached;
        }
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
        {
            identity = "missing";
        }
        m_hashes.Add(path, identity);
        return identity;
    }
}
