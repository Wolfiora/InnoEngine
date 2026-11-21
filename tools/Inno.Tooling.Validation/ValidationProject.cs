using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Inno.Tooling.Validation;

internal sealed record ValidationProject(
    string path,
    string[] references,
    string[] inputs,
    string layer,
    string environment,
    bool test,
    bool native,
    bool reusable,
    string productTarget,
    bool requiresNativeProducer
) {
    internal static ValidationProject Read(
        string root,
        JsonElement snapshot,
        string? backendTarget = null,
        JsonElement? selectedVariant = null
    ) {
        JsonElement properties = snapshot.GetProperty("Properties");
        string Value(string name) => properties.GetProperty(name).GetString() ?? string.Empty;
        string Relative(string path) => Path.GetRelativePath(root, path).Replace('\\', '/');
        string path = Relative(snapshot.GetProperty("Path").GetString()!);
        if (Value("IsTestProject").Equals("true", StringComparison.OrdinalIgnoreCase)
            && Value("InnoValidationLayer") == "native" && string.IsNullOrWhiteSpace(Value("InnoTestNativeProduct"))
        ) {
            throw new InvalidDataException($"Native test owner must declare its product deployment: {path}");
        }
        var inputs = new HashSet<string>(StringComparer.Ordinal) { path };
        var references = new HashSet<string>(StringComparer.Ordinal);
        bool native = false;
        IEnumerable<JsonElement> variants = selectedVariant is JsonElement selected
                                            ? [selected]
                                            : snapshot.GetProperty("Variants").EnumerateArray().Append(snapshot);
        foreach (JsonElement variant in variants)
        {
            string? variantTarget = variant.GetProperty("Properties").GetProperty("InnoNativeTarget").GetString();
            if (backendTarget is not null && !string.IsNullOrEmpty(variantTarget) && variantTarget != backendTarget)
            {
                continue;
            }
            foreach (JsonElement import in variant.GetProperty("Imports").EnumerateArray())
            {
                inputs.Add(Relative(import.GetString()!));
            }
            foreach (JsonProperty group in variant.GetProperty("Items").EnumerateObject())
            {
                if (group.Name is "PackageReference" or "Using" or "InnoNativeDependency")
                {
                    continue;
                }
                foreach (JsonElement item in group.Value.EnumerateArray())
                {
                    if (item.TryGetProperty("FullPath", out JsonElement fullPath) && !string.IsNullOrEmpty(fullPath.GetString()))
                    {
                        string file = Relative(fullPath.GetString()!);
                        // The SDK lists adjacent build definitions as None even when this variant never imports them.
                        // Copied or otherwise declared items and actual imports retain their own input edges.
                        if ((backendTarget is not null || selectedVariant.HasValue) && group.Name == "None"
                            && (file.EndsWith(".props", StringComparison.OrdinalIgnoreCase)
                                || file.EndsWith(".targets", StringComparison.OrdinalIgnoreCase))
                            && item.EnumerateObject().All(static property => property.Name is "Identity" or "FullPath")
                        ) {
                            continue;
                        }
                        inputs.Add(file);
                        if (group.Name == "ProjectReference")
                        {
                            references.Add(file);
                        }
                    }
                }
            }
            JsonElement effective = variant.GetProperty("Properties");
            native |= !string.IsNullOrEmpty(effective.GetProperty("InnoTestNativeProduct").GetString())
                      || !string.IsNullOrEmpty(effective.GetProperty("InnoProductId").GetString())
                      || variant.GetProperty("Items").GetProperty("InnoNativeDependency").GetArrayLength() != 0;
        }
        return new(
            path,
            references.Order(StringComparer.Ordinal).ToArray(),
            inputs.Order(StringComparer.Ordinal).ToArray(),
            Value("InnoValidationLayer"),
            Value("InnoValidationEnvironment"),
            Value("IsTestProject").Equals("true", StringComparison.OrdinalIgnoreCase),
            native,
            !Value("InnoValidationReusable").Equals("false", StringComparison.OrdinalIgnoreCase),
            Value("InnoProductTarget"),
            Value("InnoValidationRequiresNativeProducer").Equals("true", StringComparison.OrdinalIgnoreCase)
        );
    }
}
