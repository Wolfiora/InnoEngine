using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.IO;
using Inno.Tooling.Documentation;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

/// <summary>
/// Compiles canonical documentation examples against explicit prepared host or script references.
/// This validator does not parse Markdown again, build products, run example code or certify source provenance.
/// </summary>
public static class DocumentationExamples
{
    /// <summary>
    /// Validates complete example groups in a prepared snapshot using one evaluated SDK graph.
    /// </summary>
    /// <param name="arguments">
    /// Required engine-root, cache-root, snapshot, dotnet and output options; optional repeated page identities,
    /// configuration, target, runtime-reference and editor-reference. References must already exist.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels reading, SDK inspection or compilation before atomic report publication.
    /// </param>
    /// <returns>
    /// Zero when every selected example group compiles; one for failed compilation, missing prerequisites,
    /// missing explicit contexts or empty required example discovery. The report preserves each group's status.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Options are missing, duplicated or unknown, or a requested page does not exist.
    /// </exception>
    /// <exception cref="IOException">
    /// Snapshot integrity, repository inventory or report publication fails.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Validation was canceled; no completed report replaces the previous result.
    /// </exception>
    public static async Task<int> ExecuteAsync(
        string[] arguments,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(arguments);
        var options = new Dictionary<string, string>(StringComparer.Ordinal);
        var selected = new HashSet<string>(StringComparer.Ordinal);
        for (int index = 0; index < arguments.Length; index += 2)
        {
            string key = arguments[index];
            if (index + 1 == arguments.Length)
                throw new ArgumentException("Incomplete example validation option: " + key);
            if (key == "--page")
            {
                if (!selected.Add(arguments[index + 1]))
                    throw new ArgumentException("Duplicate example page selection.");
            }
            else if (key is not ("--engine-root" or "--cache-root" or "--snapshot" or "--dotnet" or "--output"
                or "--configuration" or "--target" or "--runtime-reference" or "--editor-reference")
                || !options.TryAdd(key, arguments[index + 1]))
                throw new ArgumentException("Unknown or duplicate example validation option: " + key);
        }
        foreach (string required in new[] { "--engine-root", "--cache-root", "--snapshot", "--dotnet", "--output" })
            if (!options.ContainsKey(required))
                throw new ArgumentException("Missing example validation option: " + required);
        string root = Path.GetFullPath(options["--engine-root"]);
        string configuration = options.GetValueOrDefault("--configuration", "Debug");
        if (configuration is not ("Debug" or "SourceDebug" or "Release"))
            throw new ArgumentException("Unsupported example configuration.");
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(options["--cache-root"],
            options["--snapshot"], cancellationToken: cancellationToken).ConfigureAwait(false);
        DocumentationCatalogReport catalog = await snapshot.ExportCatalogAsync(cancellationToken).ConfigureAwait(false);
        foreach (string page in selected)
            if (!catalog.pages.Any(candidate => candidate.id == page))
                throw new ArgumentException("Selected example page is missing: " + page);
        var projects = RepositorySourceInventory.Projects(root)
            .ToDictionary(static project => Path.GetFileNameWithoutExtension(project), StringComparer.Ordinal);
        var groups = catalog.pages.Where(page => selected.Count == 0 || selected.Contains(page.id))
            .Select(page => (page, blocks: page.codeBlocks.Where(static block => block.language is "csharp" or "cs"
                && block.declarationSymbols.Count == 0).ToArray()))
            .Where(static group => group.blocks.Length != 0).ToArray();
        var requested = groups.Where(static group => group.page.examples is not null)
            .SelectMany(static group => group.page.examples!.projects)
            .Where(projects.ContainsKey).Select(name => projects[name]).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var sdk = new WikiProjectReferences(root, configuration, Path.GetFullPath(options["--dotnet"]),
            options.GetValueOrDefault("--target"), selectedProjects: requested);
        var frozen = new Dictionary<string, FrozenReference>(StringComparer.OrdinalIgnoreCase);
        var results = new List<ExampleGroupResult>();
        foreach (var group in groups)
        {
            cancellationToken.ThrowIfCancellationRequested();
            DocumentationExampleContext? context = group.page.examples;
            var identities = new SortedDictionary<string, string>(StringComparer.Ordinal);
            var errors = new List<string>();
            string status;
            try
            {
                if (context is null)
                    throw new InvalidDataException("Declare exampleMode and exampleProjects; reference inference is not permitted.");
                if (context.projects.Any(name => !projects.ContainsKey(name)))
                    throw new InvalidDataException("An example owner is absent from the actual engine project inventory.");
                CSharpCompilation compilation;
                if (context.mode == "host")
                {
                    CSharpCompilation[] contexts = context.projects.Select(name => sdk.GetCompilation(projects[name], cancellationToken)).ToArray();
                    compilation = contexts[0].WithReferences(contexts.SelectMany(static item => item.References)
                        .OfType<PortableExecutableReference>().Select(Freeze).Distinct());
                }
                else
                {
                    var scripts = new WikiScriptReferences(options.GetValueOrDefault("--runtime-reference"),
                        options.GetValueOrDefault("--editor-reference"));
                    bool editor = context.mode == "editor-script";
                    IReadOnlyList<MetadataReference> references = scripts.GetReferences(editor);
                    identities.Add("script-projection", scripts.GetIdentity(editor));
                    compilation = CSharpCompilation.Create("DocumentationScript", references:
                        sdk.GetFrameworkReferences(cancellationToken).OfType<PortableExecutableReference>().Select(Freeze).Concat(references),
                        options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary,
                            nullableContextOptions: NullableContextOptions.Enable));
                }
                CSharpParseOptions parse = (CSharpParseOptions?)compilation.SyntaxTrees.FirstOrDefault()?.Options
                    ?? new CSharpParseOptions(LanguageVersion.CSharp14);
                SyntaxTree[] trees = group.blocks.Select(block => CSharpSyntaxTree.ParseText(block.text, parse,
                    group.page.path + "#line-" + block.line, cancellationToken: cancellationToken)).ToArray();
                OutputKind output = trees.Any(tree => tree.GetRoot(cancellationToken).DescendantNodes().OfType<GlobalStatementSyntax>().Any())
                    ? OutputKind.ConsoleApplication : OutputKind.DynamicallyLinkedLibrary;
                compilation = compilation.RemoveAllSyntaxTrees().AddSyntaxTrees(trees).WithAssemblyName("DocumentationExample")
                    .WithOptions(compilation.Options.WithOutputKind(output));
                errors.AddRange(compilation.GetDiagnostics(cancellationToken)
                    .Where(static diagnostic => diagnostic.Severity == DiagnosticSeverity.Error).Select(static diagnostic => diagnostic.ToString()));
                status = errors.Count == 0 ? "Passed" : "Failed";
            }
            catch (Exception failure) when (failure is IOException or InvalidDataException or InvalidOperationException or JsonException)
            {
                status = "Blocked";
                errors.Add(failure.Message);
            }
            results.Add(new(group.page.id, group.page.contentIdentity, context?.mode, group.blocks,
                status, identities, errors));

            PortableExecutableReference Freeze(PortableExecutableReference reference)
            {
                string path = reference.FilePath ?? throw new InvalidDataException("A host reference has no explicit source path.");
                if (!frozen.TryGetValue(path, out FrozenReference? value))
                {
                    byte[] bytes = File.ReadAllBytes(path);
                    value = new(MetadataReference.CreateFromImage(bytes, filePath: path, properties: reference.Properties),
                        Convert.ToHexStringLower(SHA256.HashData(bytes)));
                    frozen.Add(path, value);
                }
                identities[path] = value.identity;
                return value.reference.WithProperties(reference.Properties);
            }
        }
        string overall = groups.Length == 0 || results.Any(static result => result.status == "Blocked") ? "Blocked"
            : results.Any(static result => result.status == "Failed") ? "Failed" : "Passed";
        cancellationToken.ThrowIfCancellationRequested();
        AtomicFile.WriteAllBytes(Path.GetFullPath(options["--output"]), JsonSerializer.SerializeToUtf8Bytes(new
        {
            status = overall, catalog.snapshot, catalog.contentIdentity, catalog.revision, catalog.sourceTree,
            sourceApiStatus = "Unverified", configuration, target = options.GetValueOrDefault("--target"),
            reason = groups.Length == 0 ? "Required executable example discovery was empty." : null, results
        }, new JsonSerializerOptions { WriteIndented = true, NewLine = "\n" }));
        Console.WriteLine($"Documentation examples: {overall}; {results.Count} groups. Source and runtime behavior certification remain separate.");
        return overall == "Passed" ? 0 : 1;
    }

    private sealed record FrozenReference(
        PortableExecutableReference reference,
        string identity
    );

    private sealed record ExampleGroupResult(
        string page,
        string contentIdentity,
        string? mode,
        IReadOnlyList<DocumentationCodeBlockInfo> blocks,
        string status,
        IReadOnlyDictionary<string, string> references,
        IReadOnlyList<string> errors
    );
}
