using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.RegularExpressions;
using Inno.Core.IO;

namespace Inno.Tooling.Architecture;

internal static class DocumentationValidationReceipts
{
    internal static void Collect(
        string planFile,
        string resultsFile,
        string directory,
        string output
    ) {
        JsonElement plan = Read(planFile);
        JsonElement results = Read(resultsFile);
        JsonElement context = plan.GetProperty("documentation");
        JsonElement[] checks = List(plan, "checks").Where(check => Text(check, "exclusion") == null).ToArray();
        JsonElement catalogCheck = checks.Single(check => Text(check, "id") == "documentation" && Text(check, "kind") == "documentation");
        string? snapshot = Text(context, "snapshot");
        string? tree = Text(context, "sourceTree");
        string? content = Text(catalogCheck.GetProperty("documentation"), "contentIdentity");
        Check(Digest(snapshot) && Digest(content) && Matches(tree, "[a-f0-9]{40}"), "Invalid documentation snapshot identity.");
        string catalogFile = PathBoundary.RequireUnlinkedPath(directory, Path.Combine(directory, "documentation-catalog.json"));
        JsonElement catalog = Read(catalogFile);
        Check(
            Text(catalog, "snapshot") == snapshot && Text(catalog, "sourceTree") == tree && Text(catalog, "contentIdentity") == content
            && Text(catalog, "repository") == "https://github.com/Wolfiora/InnoEngine",
            "Catalog differs from the reconciled documentation pair."
        );
        List<object> sourceApi = [];
        List<object> sourceExamples = [];
        foreach (JsonElement check in checks.Where(check => Text(check, "kind") is "documentation-api" or "documentation-examples"))
        {
            bool examples = Text(check, "kind") == "documentation-examples";
            string id = Text(check, "id")!;
            string? owner = Text(check, "context");
            Check(
                examples
                ? Matches(owner, "[a-zA-Z0-9_-]+") && id == "documentation-examples:" + owner
                : Matches(id, "documentation-api:[a-zA-Z0-9_-]+:[a-zA-Z0-9_-]+"),
                "Invalid documentation check identity."
            );
            string name = examples ? owner + "-documentation-examples.json" : id.Replace(':', '-') + ".json";
            string path = PathBoundary.RequireUnlinkedPath(directory, Path.Combine(directory, name));
            JsonElement report = Read(path);
            JsonElement expected = check.GetProperty("documentation");
            string? configuration = Text(expected, "configuration");
            string? target = Text(expected, "target");
            Check(
                Text(report, "status") == "Passed" && Text(report, "snapshot") == snapshot && Text(report, "configuration") == configuration
                && Text(report, "target") == target
                && (examples
                    ? Text(report, "sourceTree") == tree && Text(report, "contentIdentity") == content
                    : Text(report, "documentationSourceTree") == tree && Text(report, "scope") == "SelectedSourceReferenceComparison"),
                "Report differs from its planned context."
            );
            JsonElement result = List(results, "results").Single(
                item => Text(item, "id") == id && Text(item, "status") is "Passed" or "Reused"
            );
            JsonElement[] proofs = List(result, "documentationInputs");
            Exact(proofs.Select(item => Text(item, "project")!), Strings(check, "projects"), "Documentation owner coverage");
            bool compiled = examples
                || Strings(check, "inputs").Any(input => input.StartsWith('@') && input is not ("@tooling" or "@native-sources"));
            foreach (JsonElement proof in proofs)
            {
                string[] owners = Strings(proof, "owners");
                Check(
                    Digest(Text(proof, "environment"))
                    && Text(proof, "environment") == Text(plan, "environment")
                    && Digest(Text(proof, "sourceIdentity"))
                    && Text(proof, "configuration") == configuration
                    && Text(proof, "target") == target
                    && owners.Contains(Text(proof, "project"))
                    && owners.Distinct(StringComparer.Ordinal).Count() == owners.Length
                    && owners.All(owner => Relative(owner) && owner.EndsWith(".csproj", StringComparison.Ordinal)),
                    "Invalid owner or environment in documentation input provenance."
                );
                JsonProperty[] sources = proof.GetProperty("sources").EnumerateObject().ToArray();
                Check(
                    sources.Length > 0
                    && sources.All(
                        source => Relative(source.Name) && Matches(source.Value.GetString(), "(?:[a-f0-9]{40}|[a-f0-9]{64}|missing)")
                    ),
                    "Invalid repository source identity."
                );
                JsonProperty[] packages = proof.GetProperty("packages").EnumerateObject().ToArray();
                Check(
                    (compiled || packages.Length == 0) && packages.All(package => (
                        Relative(package.Name)
                        && Matches(package.Name, "[a-z0-9_.-]+/[a-z0-9_.+-]+") && Digest(package.Value.GetString())
                    )),
                    "Invalid package content identity."
                );
            }
            string catalogIdentity = Hash(catalogFile);
            string reportIdentity = Hash(path);
            if (examples)
            {
                JsonElement[] pages = VerifyExamples(report, expected, catalog);
                sourceExamples.Add(new
                {
                    check = id,
                    catalogIdentity,
                    reportIdentity,
                    configuration,
                    target,
                    inputs = proofs,
                    pages,
                    scriptReferences = List(report, "scriptReferences")
                });
                continue;
            }
            JsonElement[] projects = List(report, "projects");
            Exact(projects.Select(item => Text(item, "project")!), Strings(check, "projects"), "Source comparison owner coverage");
            JsonElement[] references = List(report, "references");
            Exact(references.Select(item => Text(item, "path")!), Strings(expected, "references"), "Source comparison reference selection");
            foreach (JsonElement reference in references)
            {
                _ = List(catalog, "references").Single(item => (
                    Text(item, "path") == Text(reference, "path")
                    && Text(item, "identity") == Text(reference, "identity")
                    && Text(item, "model") == "source-clr"
                    && Text(item, "configuration") == configuration
                    && Text(item, "target") == target
                ));
            }
            var inputs = proofs.Select(proof =>
            {
                JsonElement project = projects.Single(item => Text(item, "project") == Text(proof, "project"));
                Check(
                    Text(project, "status") == "Passed"
                    && List(project, "differences").Length == 0
                    && !string.IsNullOrWhiteSpace(Text(project, "name"))
                    && Digest(Text(project, "referenceIdentity")),
                    "Source owner did not pass."
                );
                return new
                {
                    project = Text(project, "name"),
                    path = Text(proof, "project"),
                    owners = proof.GetProperty("owners"),
                    sourceIdentity = Text(proof, "sourceIdentity"),
                    environment = Text(proof, "environment"),
                    configuration,
                    target,
                    referenceIdentity = Text(project, "referenceIdentity"),
                    packages = proof.GetProperty("packages"),
                    sources = proof.GetProperty("sources")
                };
            }).ToArray();
            sourceApi.Add(new
            {
                check = id,
                reportIdentity,
                catalogIdentity,
                configuration,
                target,
                references,
                projects = projects.Select(project => Text(project, "name")),
                inputMode = compiled ? "Compiled" : "SourceOnly",
                inputs
            });
        }
        AtomicFile.WriteAllBytes(
            output,
            JsonSerializer.SerializeToUtf8Bytes(
                new { documentation = new { sourceTree = tree, snapshot, contentIdentity = content }, sourceApi, sourceExamples }
            )
        );
    }

    private static JsonElement[] VerifyExamples(
        JsonElement report,
        JsonElement expected,
        JsonElement catalog
    ) {
        JsonElement[] results = List(report, "results");
        Exact(results.Select(item => Text(item, "page")!), Strings(expected, "pages"), "Example page discovery");
        List<JsonElement> pages = [];
        var modes = new HashSet<string>(StringComparer.Ordinal);
        foreach (JsonElement result in results)
        {
            string? id = Text(result, "page");
            JsonElement page = List(catalog, "pages").Single(item => Text(item, "id") == id);
            JsonElement example = page.GetProperty("examples");
            string mode = Text(result, "mode")!;
            modes.Add(mode);
            Check(
                Text(result, "status") == "Passed"
                && List(result, "errors").Length == 0
                && Text(result, "contentIdentity") == Text(page, "contentIdentity")
                && mode == Text(example, "mode"),
                "An example page, mode or result changed."
            );
            string?[] blocks = List(page, "codeBlocks")
                .Where(block => Text(block, "language") is "cs" or "csharp" && List(block, "declarationSymbols").Length == 0)
                .Select(block => Text(block, "contentIdentity"))
                .ToArray();
            JsonProperty[] references = result.GetProperty("references").EnumerateObject().ToArray();
            Check(
                blocks.Length > 0
                && blocks.All(Digest)
                && blocks.SequenceEqual(List(result, "blocks").Select(block => Text(block, "contentIdentity")))
                && references.Length > 0 && references.All(reference => Digest(reference.Value.GetString())),
                "Missing executable blocks or compilation references."
            );
            if (mode is "runtime-script" or "editor-script")
            {
                string[] names = references.Select(reference => reference.Name).ToArray();
                Check(
                    names.Contains("Inno.ScriptApi.Runtime") && names.Contains("Inno.ScriptApi.Editor") == (mode == "editor-script"),
                    "Unexpected script surface."
                );
            }
            pages.Add(JsonSerializer.SerializeToElement(new { page = id, mode, projects = example.GetProperty("projects"), blocks }));
        }
        JsonElement[] projected = List(report, "scriptReferences");
        JsonElement[] declared = List(catalog, "references").Where(item => Text(item, "model") switch
        {
            "runtime-script" => modes.Contains("runtime-script") || modes.Contains("editor-script"),
            "editor-script" => modes.Contains("editor-script"),
            _ => false
        }).ToArray();
        Exact(
            projected.Select(item => Text(item, "path")!),
            declared.Select(item => Text(item, "path")!),
            "Projected script reference selection",
            allowEmpty: true
        );
        foreach (JsonElement projection in projected)
        {
            JsonElement bound = declared.Single(item => Text(item, "path") == Text(projection, "path"));
            string? model = Text(bound, "model");
            Check(
                Text(projection, "status") == "Passed"
                && List(projection, "differences").Length == 0
                && Text(projection, "identity") == Text(bound, "identity")
                && Text(projection, "model") == model && Text(projection, "configuration") == Text(report, "configuration")
                && Text(bound, "configuration") == Text(report, "configuration")
                && Text(bound, "target") == null
                && Digest(Text(projection, "documentationIdentity"))
                && Strings(projection, "projects").SequenceEqual(Strings(bound, "projects")),
                "Projected API comparison differs from its declaration."
            );
            JsonProperty[] identities = projection.GetProperty("projectionIdentities").EnumerateObject().ToArray();
            Exact(
                identities.Select(value => value.Name),
                model == "editor-script" ? ["Inno.ScriptApi.Editor", "Inno.ScriptApi.Runtime"] : ["Inno.ScriptApi.Runtime"],
                "Projected assemblies"
            );
            foreach (JsonProperty identity in identities)
            {
                JsonElement[] consumers = results
                    .Where(item => Text(item, "mode") == model || model == "runtime-script" && Text(item, "mode") == "editor-script")
                    .ToArray();
                Check(
                    Digest(identity.Value.GetString())
                    && consumers.Length > 0
                    && consumers.All(item => Text(item.GetProperty("references"), identity.Name) == identity.Value.GetString()),
                    "Projected API evidence differs from the actual script compiler inputs."
                );
            }
        }
        return pages.ToArray();
    }

    private static bool Relative(string name)
        => !string.IsNullOrWhiteSpace(name) && !Path.IsPathRooted(name) && !name.Contains('\\')
           && !name.Contains(':') && !name.Contains('\0') && !name.Split('/').Any(segment => segment is "." or ".." or "");

    private static bool Digest(string? value) => Matches(value, "[a-f0-9]{64}");

    private static bool Matches(
        string? value,
        string pattern
    ) => value != null && Regex.IsMatch(value, @"\A" + pattern + @"\z");

    private static string? Text(
        JsonElement item,
        string key
    ) => item.TryGetProperty(key, out JsonElement value) ? value.GetString() : null;

    private static JsonElement[] List(
        JsonElement item,
        string key
    ) => item.TryGetProperty(key, out JsonElement value) && value.ValueKind != JsonValueKind.Null ? value.EnumerateArray().ToArray() : [];

    private static string[] Strings(
        JsonElement item,
        string key
    ) => List(item, key).Select(value => value.GetString()!).ToArray();

    private static JsonElement Read(string path) => JsonSerializer.Deserialize<JsonElement>(File.ReadAllBytes(path));

    private static string Hash(string path)
    {
        using FileStream input = File.OpenRead(path);
        return Convert.ToHexStringLower(SHA256.HashData(input));
    }

    private static void Exact(
        IEnumerable<string> actual,
        IEnumerable<string> expected,
        string label,
        bool allowEmpty = false
    ) {
        string[] values = actual.Order(StringComparer.Ordinal).ToArray();
        Check(
            (values.Length > 0 || allowEmpty) && values.Distinct(StringComparer.Ordinal).Count() == values.Length
            && values.SequenceEqual(expected.Order(StringComparer.Ordinal)),
            label + " is incomplete or duplicated."
        );
    }

    private static void Check(
        bool valid,
        string message
    ) {
        if (!valid)
        {
            throw new InvalidDataException(message);
        }
    }
}
