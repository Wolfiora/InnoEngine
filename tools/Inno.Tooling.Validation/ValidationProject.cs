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
    string productTarget
) {
    internal static ValidationProject Read(
        string root,
        JsonElement snapshot
    ) {
        JsonElement properties = snapshot.GetProperty("Properties");
        string Value(string name) => properties.GetProperty(name).GetString() ?? string.Empty;
        string Relative(string path) => Path.GetRelativePath(root, path).Replace('\\', '/');
        string path = Relative(snapshot.GetProperty("Path").GetString()!);
        if (Value("IsTestProject").Equals("true", StringComparison.OrdinalIgnoreCase)
            && Value("InnoValidationLayer") == "native" && string.IsNullOrWhiteSpace(Value("InnoTestNativeProduct")))
            throw new InvalidDataException($"Native test owner must declare its product deployment: {path}");
        var inputs = new HashSet<string>(StringComparer.Ordinal) { path };
        var references = new HashSet<string>(StringComparer.Ordinal);
        bool native = false;
        foreach (JsonElement variant in snapshot.GetProperty("Variants").EnumerateArray().Append(snapshot))
        {
            foreach (JsonElement import in variant.GetProperty("Imports").EnumerateArray())
                inputs.Add(Relative(import.GetString()!));
            foreach (JsonProperty group in variant.GetProperty("Items").EnumerateObject())
                foreach (JsonElement item in group.Value.EnumerateArray())
                    if (item.TryGetProperty("FullPath", out JsonElement fullPath) && !string.IsNullOrEmpty(fullPath.GetString()))
                    {
                        inputs.Add(Relative(fullPath.GetString()!));
                        if (group.Name == "ProjectReference")
                            references.Add(Relative(fullPath.GetString()!));
                    }
            JsonElement effective = variant.GetProperty("Properties");
            native |= !string.IsNullOrEmpty(effective.GetProperty("InnoTestNativeProduct").GetString())
                || !string.IsNullOrEmpty(effective.GetProperty("InnoProductId").GetString())
                || effective.GetProperty("BindGenGeneratedBindings").GetString()?.Equals("true", StringComparison.OrdinalIgnoreCase) == true;
        }
        return new(path, references.Order(StringComparer.Ordinal).ToArray(),
            inputs.Order(StringComparer.Ordinal).ToArray(), Value("InnoValidationLayer"), Value("InnoValidationEnvironment"),
            Value("IsTestProject").Equals("true", StringComparison.OrdinalIgnoreCase),
            native,
            !Value("InnoValidationReusable").Equals("false", StringComparison.OrdinalIgnoreCase), Value("InnoProductTarget"));
    }
}
