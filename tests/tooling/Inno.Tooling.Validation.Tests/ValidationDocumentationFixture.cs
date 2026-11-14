using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Inno.Tooling.Architecture;
using Inno.Tooling.Documentation;

namespace Inno.Tooling.Validation.Tests;

internal sealed class ValidationDocumentationFixture(ValidationRepository repository)
{
    internal string cache => repository.PathOf("artifacts/documentation");
    private string source => repository.PathOf("artifacts/docs-source");

    internal void Initialize()
    {
        var owners = Directory.EnumerateFiles(repository.PathOf("."), "*.csproj", SearchOption.AllDirectories)
            .Where(path => !path.StartsWith(repository.PathOf("artifacts") + Path.DirectorySeparatorChar, StringComparison.Ordinal))
            .Select(path => new
            {
                name = Path.GetFileNameWithoutExtension(path), path = Path.GetRelativePath(repository.PathOf("."), path).Replace('\\', '/'),
                entry = "project-" + Path.GetFileNameWithoutExtension(path).ToLowerInvariant(), surfaces = new[] { "source-clr" }
            }).ToArray();
        byte[] inventory = JsonSerializer.SerializeToUtf8Bytes(owners);
        byte[] facts = JsonSerializer.SerializeToUtf8Bytes(new
        {
            model = "source-clr", configuration = "Debug", projects = owners.Select(owner => new
            {
                name = owner.name, project = owner.path, symbols = Array.Empty<object>()
            })
        });
        Write("reference/projects/catalog.json", Encoding.UTF8.GetString(inventory));
        Write("reference/api/facts.json", Encoding.UTF8.GetString(facts));
        string revision = new('a', 40);
        string tree = new('b', 40);
        Write("reference/engine.lock.json", JsonSerializer.Serialize(new
        {
            repository = "https://github.com/Wolfiora/InnoEngine", revision, sourceTree = tree, toolingRevision = revision,
            projectManifest = "reference/projects/catalog.json", projectManifestIdentity = Convert.ToHexStringLower(SHA256.HashData(inventory)),
            apiManifests = new[] { new
            {
                path = "reference/api/facts.json", sha256 = Convert.ToHexStringLower(SHA256.HashData(facts)), model = "source-clr",
                configuration = "Debug", sourceRevision = revision, sourceTree = tree
            } }
        }));
        foreach (var owner in owners)
            Write("content/en/" + owner.name + ".md", "---\nuid: " + owner.entry + "\nkind: project\nowners: [" + owner.name
                + "]\napiSurface: source-clr\n---\n# " + owner.name + "\n\nRead this owner's contract.\n");
    }

    internal void Write(
        string path,
        string text
    ) => repository.Write("artifacts/docs-source/" + path, text);

    internal Task<DocumentationResult> IndexAsync() => DocumentationStore.IndexAsync(repository.PathOf("."), source, cache);

    internal async Task CaptureAsync(string project)
    {
        await repository.BuildAsync(project);
        string output = repository.PathOf("artifacts/captured-documentation.json");
        if (DocumentationReferences.ExportApi(["--engine-root", repository.PathOf("."), "--dotnet", repository.dotnet,
            "--project", project, "--output", output]) != 0)
            throw new InvalidOperationException("The fixture's real source contract could not be captured.");
        JsonNode captured = JsonNode.Parse(File.ReadAllBytes(output))!;
        JsonNode facts = JsonNode.Parse(File.ReadAllBytes(Path.Combine(source, "reference/api/facts.json")))!;
        JsonArray projects = facts["projects"]!.AsArray();
        int index = projects.Select((owner, position) => (owner, position)).Single(item => item.owner!["project"]!.GetValue<string>() == project).position;
        projects[index] = captured["projects"]![0]!.DeepClone();
        byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(facts);
        Write("reference/api/facts.json", Encoding.UTF8.GetString(bytes));
        JsonNode binding = JsonNode.Parse(File.ReadAllBytes(Path.Combine(source, "reference/engine.lock.json")))!;
        binding["apiManifests"]![0]!["sha256"] = Convert.ToHexStringLower(SHA256.HashData(bytes));
        Write("reference/engine.lock.json", binding.ToJsonString());
    }

    internal Task<JsonDocument> PlanAsync(
        DocumentationResult current,
        DocumentationResult? previous = null
    ) => repository.PlanAsync(documentationCache: cache, documentationSnapshot: current.snapshot, documentationBaseline: previous?.snapshot);

    internal static string Page(
        string owner,
        string body,
        string? mode = null
    ) => "---\nuid: task-" + owner.ToLowerInvariant() + "\nkind: how-to\nowners: [" + owner + "]\n"
        + (mode is null ? string.Empty : "exampleMode: " + mode + "\nexampleProjects: [" + owner + "]\n")
        + "---\n# Work with " + owner + "\n\n" + body;
}
