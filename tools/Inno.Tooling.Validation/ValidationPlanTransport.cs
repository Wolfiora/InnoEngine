using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Inno.Build.Toolchains;

namespace Inno.Tooling.Validation;

internal static class ValidationPlanTransport
{
    internal static byte[] Write(ValidationPlan plan)
    {
        JsonNode document = JsonSerializer.SerializeToNode(plan)!;
        return JsonSerializer.SerializeToUtf8Bytes(
            Rewrite(document, Locations(plan.root, plan.dotnet), bind: false),
            new JsonSerializerOptions { WriteIndented = true }
        );
    }

    internal static ValidationPlan Read(
        string path,
        string root,
        string dotnet
    ) {
        JsonNode document = JsonNode.Parse(File.ReadAllBytes(path))
                            ?? throw new InvalidDataException("The portable validation plan is empty.");
        if (document["root"]?.GetValue<string>() != "${workspace}")
        {
            throw new InvalidDataException("A validation plan must declare a logical workspace binding.");
        }
        return Rewrite(document, Locations(root, dotnet), bind: true).Deserialize<ValidationPlan>()
               ?? throw new InvalidDataException("The portable validation plan could not be bound.");
    }

    internal static string Logical(
        string root,
        string dotnet,
        string value
    ) => Replace(value, Locations(root, dotnet), bind: false);

    private static KeyValuePair<string, string>[] Locations(
        string root,
        string dotnet
    ) {
        string host = Path.IsPathFullyQualified(dotnet) ? dotnet : ToolchainEnvironment.ResolveExecutable(dotnet);
        string nuget = Environment.GetEnvironmentVariable("NUGET_PACKAGES")
                       ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".nuget", "packages");
        return new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["${workspace}"] = Path.GetFullPath(root).TrimEnd(Path.DirectorySeparatorChar),
            ["${dotnet}"] = Path.GetDirectoryName(Path.GetFullPath(host))!,
            ["${nuget}"] = Path.GetFullPath(nuget)
        }.OrderByDescending(static pair => pair.Value.Length).ToArray();
    }

    private static JsonNode Rewrite(
        JsonNode node,
        KeyValuePair<string, string>[] locations,
        bool bind
    ) {
        if (node is JsonObject record)
        {
            return new JsonObject(
                record.Select(
                    pair => new KeyValuePair<string, JsonNode?>(
                        Replace(pair.Key, locations, bind),
                        pair.Value is null ? null : Rewrite(pair.Value, locations, bind)
                    )
                )
            );
        }
        if (node is JsonArray array)
        {
            return new JsonArray(array.Select(value => value is null ? null : Rewrite(value, locations, bind)).ToArray());
        }
        if (node is JsonValue value && value.TryGetValue(out string? text))
        {
            return JsonValue.Create(Replace(text, locations, bind))!;
        }
        return node.DeepClone();
    }

    private static string Replace(
        string value,
        KeyValuePair<string, string>[] locations,
        bool bind
    ) {
        foreach (KeyValuePair<string, string> location in locations)
        {
            if (bind)
            {
                value = value.Replace(location.Key, location.Value, StringComparison.Ordinal);
            }
            else
            {
                foreach (string path in new[]
                {
                    location.Value,
                    location.Value.Replace('\\', '/')
                }.Distinct(StringComparer.Ordinal)
                ) {
                    value = Regex.Replace(
                        value,
                        Regex.Escape(path) + "(?=$|[/\\\\;])",
                        _ => location.Key,
                        OperatingSystem.IsWindows() ? RegexOptions.IgnoreCase : RegexOptions.None
                    );
                }
            }
        }

        return value;
    }
}
