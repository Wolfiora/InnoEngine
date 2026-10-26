using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Inno.Tooling.Architecture;

internal static class NativePlatformPolicyValidator
{
    internal static void Validate(
        string root,
        ICollection<string> failures
    ) {
        string backends = Path.Combine(root, "backends");
        if (!Directory.Exists(backends))
            return;
        foreach (string definition in RepositorySourceInventory.Files(backends, "bindings.props"))
        {
            string bindings = Path.GetDirectoryName(definition)!;
            string relative = Path.GetRelativePath(root, bindings).Replace('\\', '/');
            string common = Path.Combine(bindings, "bindgen.json");
            if (!File.Exists(common))
                continue;
            if (Directory.EnumerateFiles(bindings, "bindgen.*.json").Any())
                failures.Add($"{relative}: platform binding presets belong to their platform integration.");
            try
            {
                using JsonDocument document = JsonDocument.Parse(File.ReadAllText(common));
                foreach (string property in new[] { "targets", "extends", "sdk" })
                    if (document.RootElement.TryGetProperty(property, out _))
                        failures.Add($"{relative}/bindgen.json: target or SDK policy '{property}' belongs to a platform Profile.");
                if (document.RootElement.TryGetProperty("source", out JsonElement source))
                    foreach (string property in new[] { "systemIncludeDirectories", "cppSystemIncludeDirectories", "sysRoot", "compiler" })
                        if (source.TryGetProperty(property, out _))
                            failures.Add($"{relative}/bindgen.json: SDK parser input '{property}' must be supplied by the platform provider.");
            }
            catch (JsonException failure)
            {
                failures.Add($"{relative}/bindgen.json: {failure.Message}");
            }
        }
    }
}
