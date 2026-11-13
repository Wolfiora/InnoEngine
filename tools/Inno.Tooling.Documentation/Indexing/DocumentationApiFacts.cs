using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Inno.Core.IO;

namespace Inno.Tooling.Documentation.Indexing;

internal sealed record DocumentationApiFact(
    string key, string project, string family, string? scope, string id, string? containingType,
    string kind, string signature, string signatureHash, string documentation, string source, int line,
    string revision, string configuration, string? target, string surface, string? baseType,
    IReadOnlyList<string> interfaces
);

internal static class DocumentationApiFacts
{
    internal static IReadOnlyDictionary<string, DocumentationApiFact[]> Read(string root, DocumentationBinding binding)
    {
        string inventoryPath = Resolve(root, binding.projectManifest);
        byte[] inventoryBytes = File.ReadAllBytes(inventoryPath);
        if (DocumentationMarkdown.Hash(inventoryBytes) != binding.projectManifestIdentity)
            throw new InvalidDataException("API project inventory identity differs from its binding.");
        using JsonDocument inventory = JsonDocument.Parse(inventoryBytes);
        var eligible = inventory.RootElement.EnumerateArray().Where(project =>
        {
            string path = project.GetProperty("path").GetString()!;
            return !path.StartsWith("backends/", StringComparison.Ordinal)
                && !path.StartsWith("extern/", StringComparison.Ordinal)
                && !path.StartsWith("tests/", StringComparison.Ordinal)
                && (!project.TryGetProperty("isTest", out JsonElement test) || !test.GetBoolean())
                && path.EndsWith(".csproj", StringComparison.Ordinal);
        }).ToDictionary(project => project.GetProperty("name").GetString()!, project => project.GetProperty("path").GetString()!, StringComparer.Ordinal);
        var result = new Dictionary<string, List<DocumentationApiFact>>(StringComparer.Ordinal);
        var captured = new HashSet<string>(StringComparer.Ordinal);
        var projections = new Dictionary<(string configuration, string? target, string model), string>();
        bool declaresScriptExports = false;
        var scriptModels = new HashSet<string>(StringComparer.Ordinal);
        foreach (DocumentationApiCapture capture in binding.apiManifests ?? [])
        {
            byte[] bytes = File.ReadAllBytes(Resolve(root, capture.path));
            if (DocumentationMarkdown.Hash(bytes) != capture.sha256)
                throw new InvalidDataException("API capture identity differs: " + capture.path);
            using JsonDocument document = JsonDocument.Parse(bytes);
            string model = document.RootElement.GetProperty("model").GetString()!;
            if (model != capture.model || String(document.RootElement, "configuration") != capture.configuration
                || String(document.RootElement, "target") != capture.target)
                throw new InvalidDataException("API context differs from its binding: " + capture.path);
            DocumentationStore.RequireHash(capture.sourceRevision, "API source revision", allowGitTree: true);
            DocumentationStore.RequireHash(capture.sourceTree, "API source tree", allowGitTree: true);
            string family = model == "source-clr" ? "engine" : model is "runtime-script" or "editor-script"
                ? "scripting" : throw new InvalidDataException("Unsupported API capture model: " + model);
            if (family == "scripting")
            {
                scriptModels.Add(model);
                if (document.RootElement.TryGetProperty("projectionIdentities", out JsonElement identities))
                {
                    string runtime = identities.GetProperty("Inno.ScriptApi.Runtime").GetString()!;
                    DocumentationStore.RequireHash(runtime, "Runtime projection identity");
                    if (model == "editor-script") DocumentationStore.RequireHash(identities.GetProperty("Inno.ScriptApi.Editor").GetString()!, "Editor projection identity");
                    if (!projections.TryAdd((capture.configuration, capture.target, model), runtime))
                        throw new InvalidDataException("Repeated logical projection context.");
                }
            }
            foreach (JsonElement project in document.RootElement.GetProperty("projects").EnumerateArray())
            {
                string name = project.GetProperty("name").GetString()!;
                if (!eligible.TryGetValue(name, out string? path)) continue;
                if (project.GetProperty("project").GetString() != path)
                    throw new InvalidDataException("API owner does not match the project inventory: " + name);
                if (family == "engine") captured.Add(name);
                if (family == "engine" && project.TryGetProperty("scriptExports", out JsonElement exports) && exports.GetArrayLength() != 0)
                    declaresScriptExports = true;
                foreach (JsonElement symbol in project.GetProperty("symbols").EnumerateArray())
                {
                    string source = symbol.GetProperty("source").GetString()!;
                    if (source.Split('/').Any(part => part is "" or "." or "..") || source.Contains('\\')
                        || Path.IsPathRooted(source) || symbol.GetProperty("line").GetInt32() < 1)
                        throw new InvalidDataException("Invalid API source location: " + source);
                    if (!source.EndsWith(".cs", StringComparison.OrdinalIgnoreCase)
                        || source.StartsWith("backends/", StringComparison.Ordinal) || source.StartsWith("extern/", StringComparison.Ordinal)) continue;
                    string id = symbol.GetProperty("id").GetString()!;
                    string? scope = family == "scripting" ? String(symbol, "scriptingScope") : null;
                    if (family == "scripting" && scope is not ("Runtime" or "Editor" or "Authoring"))
                        throw new InvalidDataException("Projected API lacks its declared ScriptingApiScope: " + id);
                    string signature = symbol.GetProperty("signature").GetString()!;
                    string signatureHash = symbol.GetProperty("signatureHash").GetString()!;
                    if (DocumentationMarkdown.Hash(signature) != signatureHash)
                        throw new InvalidDataException("Invalid API declaration identity: " + id);
                    string key = family + ":" + name + ":" + id;
                    var fact = new DocumentationApiFact(key, name, family, scope, id, String(symbol, "containingType"),
                        symbol.GetProperty("kind").GetString()!, signature, signatureHash, String(symbol, "documentation") ?? "",
                        source, symbol.GetProperty("line").GetInt32(), capture.sourceRevision, capture.configuration,
                        capture.target, model, String(symbol, "baseType"), symbol.GetProperty("interfaces").EnumerateArray().Select(item => item.GetString()!).ToArray());
                    if (!result.TryGetValue(key, out List<DocumentationApiFact>? contexts)) result.Add(key, contexts = []);
                    if (contexts.Any(prior => prior.scope != scope))
                        throw new InvalidDataException("One logical API has conflicting declared scopes: " + key);
                    DocumentationApiFact? duplicate = contexts.SingleOrDefault(prior => prior.configuration == fact.configuration
                        && prior.target == fact.target && prior.surface == fact.surface);
                    if (duplicate is not null)
                        throw new InvalidDataException("Repeated API capture in one context: " + key);
                    contexts.Add(fact);
                }
            }
        }
        string[] missing = eligible.Keys.Except(captured, StringComparer.Ordinal).Order(StringComparer.Ordinal).ToArray();
        if (missing.Length != 0) throw new InvalidDataException("Complete API synchronization lacks source captures: " + string.Join(", ", missing));
        if (declaresScriptExports && (!scriptModels.Contains("runtime-script") || !scriptModels.Contains("editor-script")))
            throw new InvalidDataException("Complete API synchronization requires both actual logical script projections.");
        foreach (List<DocumentationApiFact> contexts in result.Values)
            foreach (DocumentationApiFact runtime in contexts.Where(fact => fact.surface == "runtime-script").ToArray())
            {
                if (!projections.TryGetValue((runtime.configuration, runtime.target, "runtime-script"), out string? runtimeIdentity)
                    || !projections.TryGetValue((runtime.configuration, runtime.target, "editor-script"), out string? editorRuntimeIdentity)) continue;
                if (runtimeIdentity != editorRuntimeIdentity)
                    throw new InvalidDataException("Editor and Runtime captures use different Runtime projection inputs.");
                if (!contexts.Any(fact => fact.surface == "editor-script" && fact.configuration == runtime.configuration && fact.target == runtime.target))
                    contexts.Add(runtime with { surface = "editor-script" });
            }
        return result.ToDictionary(pair => pair.Key, pair => pair.Value.OrderBy(fact => fact.configuration, StringComparer.Ordinal)
            .ThenBy(fact => fact.target, StringComparer.Ordinal).ThenBy(fact => fact.surface, StringComparer.Ordinal).ToArray(), StringComparer.Ordinal);
    }

    internal static string Resolve(string root, string relative)
        => PathBoundary.RequireUnlinkedPath(root, PathBoundary.Resolve(root, relative));

    private static string? String(JsonElement value, string name)
        => value.TryGetProperty(name, out JsonElement result) && result.ValueKind != JsonValueKind.Null ? result.GetString() : null;
}
