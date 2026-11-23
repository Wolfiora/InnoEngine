using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;

namespace Inno.Tooling.Architecture;

internal static class DocumentationPreparedExports
{
    internal static string Key(
        string project,
        string input,
        string configuration,
        string target
    ) => "prepared/1/" + project + "/" + input + "/" + configuration + "/" + target;

    internal static string ScriptIdentity(IReadOnlyDictionary<string, string> inputs)
        => Convert.ToHexStringLower(
            SHA256.HashData(JsonSerializer.SerializeToUtf8Bytes(inputs.OrderBy(pair => pair.Key, StringComparer.Ordinal)))
        );

    internal static void Record(
        string root,
        string completed,
        CancellationToken cancellationToken
    ) {
        using JsonDocument plan = JsonDocument.Parse(File.ReadAllBytes(Path.Combine(completed, "plan.json")));
        using JsonDocument receipt = JsonDocument.Parse(File.ReadAllBytes(Path.Combine(completed, "producer.json")));
        JsonElement data = receipt.RootElement;
        string configuration = data.GetProperty("configuration").GetString()!;
        if (plan.RootElement.GetProperty("configuration").GetString() != configuration)
        {
            throw new InvalidDataException("Prepared export plan differs from its completed context.");
        }
        using var cache = new DocumentationExportCache(root, cancellationToken);
        foreach (JsonElement capture in data.GetProperty("captures").EnumerateArray())
        {
            string name = capture.GetProperty("path").GetString()!;
            if (name != Path.GetFileName(name)) throw new InvalidDataException("Invalid prepared capture path.");
            byte[] bytes = File.ReadAllBytes(Path.Combine(completed, name));
            if (Convert.ToHexStringLower(SHA256.HashData(bytes)) != capture.GetProperty("sha256").GetString())
            {
                throw new InvalidDataException("Completed capture digest changed.");
            }
            using JsonDocument facts = JsonDocument.Parse(bytes);
            string target = capture.GetProperty("target").GetString()!;
            string model = capture.GetProperty("model").GetString()!;
            if (model == "source-clr")
            {
                foreach (JsonElement project in facts.RootElement.GetProperty("projects").EnumerateArray())
                {
                    string owner = project.GetProperty("project").GetString()!;
                    string input = plan.RootElement.GetProperty("inputIdentities").GetProperty(owner).GetString()!;
                    cache.Capture(owner, Key(owner, input, configuration, target), () => new
                    {
                        fact = project.Clone(),
                        producerRevision = data.GetProperty("revision").GetString(),
                        inputIdentity = input
                    }, cancellationToken);
                }
            }
            else
            {
                cache.Capture(
                    model,
                    Key(model, plan.RootElement.GetProperty("scriptInputIdentity").GetString()!, configuration, target),
                    () => new
                    {
                        fact = facts.RootElement.Clone(),
                        producerRevision = data.GetProperty("revision").GetString(),
                        inputIdentity = plan.RootElement.GetProperty("scriptInputIdentity").GetString()
                    },
                    cancellationToken
                );
            }
        }
        cache.Report();
    }
}
