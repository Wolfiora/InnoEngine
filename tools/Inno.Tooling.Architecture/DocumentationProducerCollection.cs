using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.IO;
using Inno.Tooling.Documentation;

namespace Inno.Tooling.Architecture;

internal static class DocumentationProducerCollection
{
    private const string C_REPOSITORY = "https://github.com/Wolfiora/InnoEngine";

    internal static async Task<int> ExecuteAsync(
        string[] arguments,
        CancellationToken cancellationToken
    ) {
        if (arguments is not [string engine, string inputs, string output, string revision, string tree, .. var configurations]
            || configurations.Length == 0 || configurations.Distinct().Count() != configurations.Length
            || configurations.Any(item => item is not ("Debug" or "Release" or "SourceDebug"))
            || !Regex.IsMatch(revision, "^[0-9a-f]{40}$") || !Regex.IsMatch(tree, "^[0-9a-f]{40}$")
        ) {
            throw new ArgumentException(
                "Expected engine root, producer directory, new output directory, source revision, tree and distinct configurations."
            );
        }
        if (Directory.Exists(output) || File.Exists(output))
        {
            throw new IOException("Collection requires a new output directory.");
        }
        Directory.CreateDirectory(output);
        string inventoryPath = Path.Combine(output, "expected-projects.json");
        DocumentationReferences.ExportProjects(["--engine-root", engine, "--output", inventoryPath], cancellationToken);
        string inventoryDigest = Digest(File.ReadAllBytes(inventoryPath));
        JsonArray inventory = JsonNode.Parse(File.ReadAllBytes(inventoryPath))!.AsArray();
        var producers = new Dictionary<(string configuration, string host), Producer>();
        var enumeration = new EnumerationOptions
        {
            RecurseSubdirectories = true,
            AttributesToSkip = FileAttributes.ReparsePoint,
            IgnoreInaccessible = false
        };
        foreach (string receiptPath in Directory.EnumerateFiles(inputs, "producer.json", enumeration))
        {
            cancellationToken.ThrowIfCancellationRequested();
            string directory = Path.GetDirectoryName(receiptPath)!;
            JsonObject receipt = JsonNode.Parse(File.ReadAllBytes(receiptPath))!.AsObject();
            string host = Required(receipt, "hostTarget");
            string configuration = Required(receipt, "configuration");
            if (Required(receipt, "schema") != "inno-api-producer/1" || Required(receipt, "repository") != C_REPOSITORY
                || Required(receipt, "revision") != revision || Required(receipt, "toolingRevision") != revision
                || Required(receipt, "sourceTree") != tree || !configurations.Contains(configuration)
                || !Regex.IsMatch(host, "^[a-z0-9-]+$") || Required(receipt, "projectManifestIdentity") != inventoryDigest
            ) {
                throw new InvalidDataException("Stale or foreign API producer or source inventory: " + receiptPath);
            }
            ReadOwned(directory, "projects.json", inventoryDigest);
            JsonObject plan = JsonNode.Parse(ReadOwned(directory, "plan.json", Required(receipt, "planIdentity")))!.AsObject();
            if (Required(plan, "schema") != "inno-api-export-plan/1" || Required(plan, "hostTarget") != host
                || Required(plan, "configuration") != configuration || Required(plan, "sdkRuntime") != Required(receipt, "sdkRuntime")
                || !producers.TryAdd((configuration, host), new Producer(directory, receipt, plan))
            ) {
                throw new InvalidDataException("Repeated or inconsistent API producer context: " + receiptPath);
            }
        }
        var selected = new Dictionary<(string configuration, string target, string model), Capture>();
        foreach (((string configuration, string host), Producer producer) in producers)
        {
            foreach (JsonObject declaration in producer.receipt["captures"]!.AsArray().Cast<JsonObject>())
            {
                cancellationToken.ThrowIfCancellationRequested();
                string target = Required(declaration, "target");
                string model = Required(declaration, "model");
                string digest = Required(declaration, "sha256");
                byte[] bytes = ReadOwned(producer.directory, Required(declaration, "path"), digest);
                JsonObject facts = JsonNode.Parse(bytes)!.AsObject();
                if (!Regex.IsMatch(target, "^[a-z0-9-]+$") || model is not ("source-clr" or "editor-script" or "runtime-script")
                    || Required(declaration, "configuration") != configuration || Required(facts, "configuration") != configuration
                    || Required(facts, "target") != target || Required(facts, "model") != model
                ) {
                    throw new InvalidDataException("Capture facts differ from the producer context.");
                }
                if (target != host)
                {
                    if (producers.ContainsKey((configuration, target)))
                    {
                        continue;
                    }
                    if (model != "source-clr" || facts["projects"]!.AsArray().Any(item => item!["symbols"]!.AsArray().Count != 0))
                    {
                        throw new InvalidDataException("Foreign producer certified a nonempty API surface.");
                    }
                }
                (string configuration, string target, string model) key = (configuration, target, model);
                if (!selected.TryAdd(key, new Capture(facts, Encoding.UTF8.GetString(bytes), digest)) && selected[key].digest != digest)
                {
                    throw new InvalidDataException("Disagreeing API capture: " + key);
                }
            }
        }
        foreach (string configuration in configurations)
        {
            if (!producers.Keys.Any(key => key.configuration == configuration))
            {
                throw new InvalidDataException("Missing API configuration: " + configuration);
            }
        }
        foreach (((string configuration, string host), Producer producer) in producers)
        {
            foreach (JsonObject owner in producer.plan["projects"]!.AsArray().Cast<JsonObject>())
            {
                string? target = (string?)owner["target"];
                if (!selected.TryGetValue((configuration, string.IsNullOrEmpty(target) ? host : target, "source-clr"), out Capture? capture)
                    || capture.facts["projects"]!.AsArray().Count(item => Required(item!, "project") == Required(owner, "path")) != 1
                ) {
                    throw new InvalidDataException("Missing or repeated API owner: " + Required(owner, "path"));
                }
            }
            if (!selected.ContainsKey((configuration, host, "runtime-script"))
                || !selected.ContainsKey((configuration, host, "editor-script"))
            ) {
                throw new InvalidDataException("Missing script projection for " + configuration + "/" + host);
            }
        }
        var manifests = new JsonArray();
        foreach (var (key, capture) in selected.OrderBy(pair => pair.Key.configuration, StringComparer.Ordinal)
            .ThenBy(pair => pair.Key.target, StringComparer.Ordinal)
            .ThenBy(pair => pair.Key.model, StringComparer.Ordinal)
        ) {
            string path = $"reference/api/{key.configuration}-{key.target}-{key.model}.json";
            byte[] bytes = Encoding.UTF8.GetBytes(capture.text.Replace("\r\n", "\n", StringComparison.Ordinal));
            AtomicFile.WriteAllBytes(Path.Combine(output, path), bytes);
            manifests.Add(new JsonObject
            {
                ["path"] = path,
                ["sha256"] = Digest(bytes),
                ["configuration"] = key.configuration,
                ["model"] = key.model,
                ["target"] = key.target,
                ["sourceRevision"] = revision,
                ["sourceTree"] = tree
            });
        }
        foreach (JsonObject project in inventory.Cast<JsonObject>())
        {
            HashSet<string> surfaces = selected
                .Where(pair => pair.Value.facts["projects"]!.AsArray()
                    .Any(item => Required(item!, "name") == Required(project, "name")))
                .Select(pair => pair.Key.model)
                .ToHashSet(StringComparer.Ordinal);
            if (surfaces.Contains("runtime-script"))
            {
                surfaces.Add("editor-script");
            }
            project["surfaces"] = JsonSerializer.SerializeToNode(surfaces.Order(StringComparer.Ordinal));
        }
        const string C_PROJECT_MANIFEST = "reference/projects/catalog.json";
        byte[] catalog = JsonSerializer.SerializeToUtf8Bytes(inventory);
        AtomicFile.WriteAllBytes(Path.Combine(output, C_PROJECT_MANIFEST), catalog);
        AtomicFile.WriteAllBytes(
            Path.Combine(output, "reference/engine.lock.json"),
            JsonSerializer.SerializeToUtf8Bytes(new
            {
                repository = C_REPOSITORY,
                revision,
                sourceTree = tree,
                toolingRevision = revision,
                projectManifest = C_PROJECT_MANIFEST,
                projectManifestIdentity = Digest(catalog),
                apiManifests = manifests
            })
        );
        return await DocumentationCommand.ExecuteAsync([
            "api-package",
            "--docs-root",
            output,
            "--package",
            Path.Combine(output, "api-package.json")
        ], cancellationToken).ConfigureAwait(false);
    }

    private static byte[] ReadOwned(
        string directory,
        string name,
        string expected
    ) {
        if (!Regex.IsMatch(name, @"^[a-zA-Z0-9-]+\.json\z"))
        {
            throw new InvalidDataException("Invalid API producer file name: " + name);
        }
        string path = Path.Combine(directory, name);
        if ((File.GetAttributes(path) & FileAttributes.ReparsePoint) != 0)
        {
            throw new InvalidDataException("Linked API producer file: " + name);
        }
        byte[] bytes = File.ReadAllBytes(path);
        return Digest(bytes) == expected ? bytes : throw new InvalidDataException("Corrupt API producer file: " + name);
    }

    private static string Required(
        JsonNode source,
        string name
    ) => (string?)source[name] ?? throw new InvalidDataException("Missing API producer field: " + name);

    private static string Digest(byte[] bytes) => Convert.ToHexStringLower(SHA256.HashData(bytes));

    private sealed record Producer(
        string directory,
        JsonObject receipt,
        JsonObject plan
    );

    private sealed record Capture(
        JsonObject facts,
        string text,
        string digest
    );
}
