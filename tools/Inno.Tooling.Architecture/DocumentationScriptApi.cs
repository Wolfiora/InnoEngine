using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Inno.Core.IO;
using Inno.Tooling.Documentation;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Inno.Tooling.Architecture;

internal static class DocumentationScriptApi
{
    internal static int Export(
        string[] arguments,
        CancellationToken cancellationToken
    ) {
        var options = new Dictionary<string, string>(StringComparer.Ordinal);
        var sources = new Dictionary<string, byte[]>(StringComparer.Ordinal);
        for (int index = 0; index < arguments.Length; index += 2)
        {
            string key = arguments[index];
            if (index + 1 == arguments.Length)
                throw new ArgumentException("Incomplete script API export option: " + key);
            string value = arguments[index + 1];
            if (key == "--source-reference")
            {
                string source = Path.GetFullPath(value);
                if (!sources.TryAdd(source, File.ReadAllBytes(source)))
                    throw new ArgumentException("Repeated source reference: " + source);
            }
            else if (key is not ("--engine-root" or "--dotnet" or "--surface" or "--runtime-reference" or "--editor-reference"
                or "--output" or "--configuration" or "--target" or "--sdk-snapshot" or "--artifacts-path" or "--framework-project") || !options.TryAdd(key, value))
                throw new ArgumentException("Unknown or duplicate script API export option: " + key);
        }
        foreach (string required in new[] { "--engine-root", "--dotnet", "--surface", "--runtime-reference", "--output", "--framework-project" })
            if (!options.ContainsKey(required))
                throw new ArgumentException("Missing script API export option: " + required);
        if (sources.Count == 0)
            throw new ArgumentException("Script API origins require the actual source reference manifests.");
        string root = Path.GetFullPath(options["--engine-root"]);
        string configuration = options.GetValueOrDefault("--configuration", "Debug");
        if (configuration is not ("Debug" or "SourceDebug" or "Release"))
            throw new ArgumentException("Unsupported script API extraction configuration.");
        var owners = new HashSet<string>(StringComparer.Ordinal)
        {
            PathBoundary.RequireUnlinkedPath(root, PathBoundary.Resolve(root, options["--framework-project"]))
        };
        var sdk = new DocumentationProjectReferences(root, configuration, options["--dotnet"], selectedProjects: owners,
            artifactsPath: options.GetValueOrDefault("--artifacts-path"),
            sdkSnapshot: options.TryGetValue("--sdk-snapshot", out string? graph) ? File.ReadAllText(graph) : null);
        Capture capture = Read(options["--surface"], configuration, options["--runtime-reference"],
            options.GetValueOrDefault("--editor-reference"), sdk.GetFrameworkReferences(cancellationToken), sources.Values, cancellationToken);
        capture = capture with { target = options.GetValueOrDefault("--target") };
        var inventory = RepositorySourceInventory.Projects(root).ToHashSet(StringComparer.OrdinalIgnoreCase);
        foreach (Project project in capture.projects)
            if (!inventory.Contains(PathBoundary.RequireUnlinkedPath(root, PathBoundary.Resolve(root, project.project))))
                throw new InvalidDataException("Projected owner is absent from the actual project inventory: " + project.project);
        foreach (var source in sources)
            if (!File.ReadAllBytes(source.Key).AsSpan().SequenceEqual(source.Value))
                throw new IOException("Source reference changed during script API preparation: " + source.Key);
        string output = Path.GetFullPath(options["--output"]);
        if (sources.ContainsKey(output))
            throw new ArgumentException("Script API output cannot replace its source reference.");
        cancellationToken.ThrowIfCancellationRequested();
        AtomicFile.WriteAllBytes(output, JsonSerializer.SerializeToUtf8Bytes(capture,
            new JsonSerializerOptions { WriteIndented = true, NewLine = "\n" }));
        Console.WriteLine($"Exported {capture.projects.Sum(static project => project.symbols.Count)} actual {capture.model} declarations; source certification remains separate.");
        return 0;
    }

    internal static Capture Read(
        string surface,
        string configuration,
        string runtimePath,
        string? editorPath,
        IReadOnlyList<MetadataReference> frameworkReferences,
        IEnumerable<byte[]> sourceReferences,
        CancellationToken cancellationToken,
        IReadOnlySet<string>? selectedOwners = null
    ) {
        bool editor = surface switch
        {
            "runtime-script" => false,
            "editor-script" => true,
            _ => throw new ArgumentException("Expected runtime-script or editor-script surface.")
        };
        var sourceTypes = new Dictionary<string, List<(string path, string assembly, DocumentationApiExtractor.DocumentationApiEntry symbol)>>(StringComparer.Ordinal);
        var sourceMembers = new Dictionary<string, DocumentationApiExtractor.DocumentationApiEntry>(StringComparer.Ordinal);
        var ambiguousMembers = new HashSet<string>(StringComparer.Ordinal);
        var declaredScopes = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (byte[] bytes in sourceReferences)
        {
            using JsonDocument source = JsonDocument.Parse(bytes);
            if (source.RootElement.GetProperty("model").GetString() != "source-clr")
                continue;
            foreach (JsonElement project in source.RootElement.GetProperty("projects").EnumerateArray())
            {
                // Export declarations can live in a feature assembly that does not own the exported type.
                // Collect them before filtering projected owners; compilation context is not a scope.
                if (project.TryGetProperty("scriptExports", out JsonElement exports))
                    foreach (JsonElement export in exports.EnumerateArray())
                    {
                        string key = export.GetProperty("assembly").GetString() + "\0" + export.GetProperty("type").GetString();
                        string scope = export.GetProperty("scope").GetString()!;
                        if (scope is not ("Runtime" or "Editor" or "Authoring")
                            || declaredScopes.TryGetValue(key, out string? earlier) && earlier != scope)
                            throw new InvalidDataException("Conflicting or invalid declared scripting scope: " + key);
                        declaredScopes[key] = scope;
                    }
                if (selectedOwners is not null && !selectedOwners.Contains(project.GetProperty("name").GetString()!))
                    continue;
                foreach (JsonElement element in project.GetProperty("symbols").EnumerateArray())
                {
                    var symbol = element.Deserialize<DocumentationApiExtractor.DocumentationApiEntry>()!;
                    string path = project.GetProperty("project").GetString()!;
                    string memberKey = path + "\0" + symbol.id;
                    if (!sourceMembers.TryAdd(memberKey, symbol)
                        && DocumentationReferences.ComparableSymbol(JsonSerializer.SerializeToElement(sourceMembers[memberKey]))
                            != DocumentationReferences.ComparableSymbol(element))
                        ambiguousMembers.Add(memberKey);
                    if (!symbol.id.StartsWith("T:", StringComparison.Ordinal))
                        continue;
                    if (!sourceTypes.TryGetValue(symbol.id, out var candidates))
                        sourceTypes.Add(symbol.id, candidates = []);
                    if (!candidates.Any(prior => prior.path == path && prior.symbol.signature == symbol.signature))
                        candidates.Add((path, project.GetProperty("assembly").GetString()!, symbol));
                }
            }
        }
        var scripts = new DocumentationScriptReferences(runtimePath, editorPath);
        IReadOnlyList<MetadataReference> references = scripts.GetReferences(editor);
        string selectedPath = editor ? editorPath! : runtimePath;
        string xmlPath = Path.ChangeExtension(selectedPath, ".xml");
        byte[] xmlBytes = File.ReadAllBytes(xmlPath);
        using var xmlStream = new MemoryStream(xmlBytes, writable: false);
        XDocument document = XDocument.Load(xmlStream);
        XElement provenance = document.Root?.Element("provenance")
            ?? throw new InvalidDataException("The script reference lacks compiler-produced origin metadata; prepare current logical references.");
        if ((string?)provenance.Attribute("format") != "inno-script-documentation/2")
            throw new InvalidDataException("Unsupported script documentation provenance.");
        var origins = provenance.Elements("type").ToDictionary(
            static origin => (string)origin.Attribute("id")!, static origin => (string)origin.Attribute("source")!, StringComparer.Ordinal);
        var originAssemblies = provenance.Elements("type").ToDictionary(
            static origin => (string)origin.Attribute("id")!, static origin => (string?)origin.Attribute("assembly")
                ?? throw new InvalidDataException("A script projection origin requires its actual assembly."), StringComparer.Ordinal);
        var mappings = new Dictionary<string, string>(origins, StringComparer.Ordinal);
        if (editor)
        {
            XDocument runtimeDocument = XDocument.Load(Path.ChangeExtension(runtimePath, ".xml"));
            foreach (XElement origin in runtimeDocument.Root?.Element("provenance")?.Elements("type") ?? [])
                mappings.Add((string)origin.Attribute("id")!, (string)origin.Attribute("source")!);
        }
        var toSource = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var mapping in mappings)
        {
            toSource.Add(mapping.Key[2..], mapping.Value[2..]);
            if (mapping.Key.Contains('`'))
            {
                string name = mapping.Key[2..].Split('`')[0];
                string original = mapping.Value[2..].Split('`')[0];
                if (toSource.TryGetValue(name, out string? prior) && prior != original)
                    throw new InvalidDataException("Ambiguous constructed generic script identity: " + name);
                toSource[name] = original;
            }
        }
        var toLogical = toSource.GroupBy(static pair => pair.Value, StringComparer.Ordinal)
            .Where(static group => group.Count() == 1).ToDictionary(static group => group.Key, static group => group.Single().Key, StringComparer.Ordinal);
        Regex reverseNames = IdentityPattern(toSource.Keys);
        Regex forwardNames = IdentityPattern(toLogical.Keys);
        var selected = (PortableExecutableReference)references[editor ? 1 : 0];
        selected = ((AssemblyMetadata)selected.GetMetadata()).GetReference(
            documentation: new AssemblyDocumentationProvider(xmlPath, xmlBytes), filePath: selectedPath);
        var compilation = CSharpCompilation.Create("DocumentationScriptReference",
            references: frameworkReferences.Concat(references.Where(reference => reference != references[editor ? 1 : 0])).Append(selected),
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, nullableContextOptions: NullableContextOptions.Enable,
                metadataImportOptions: MetadataImportOptions.Public));
        var assembly = (IAssemblySymbol?)compilation.GetAssemblyOrModuleSymbol(selected)
            ?? throw new InvalidDataException("The selected logical script assembly cannot be read.");
        var paths = new Dictionary<string, string>(StringComparer.Ordinal);
        IReadOnlyList<DocumentationApiExtractor.DocumentationApiEntry> symbols = DocumentationApiExtractor.CaptureReference(assembly, Resolve, cancellationToken);
        if (symbols.Count == 0)
            throw new InvalidDataException("Required projected declaration discovery is empty.");
        if (!File.ReadAllBytes(xmlPath).AsSpan().SequenceEqual(xmlBytes))
            throw new IOException("Script documentation changed during capture.");
        Project[] projects = symbols.GroupBy(static symbol => symbol.project, StringComparer.Ordinal)
            .OrderBy(static group => group.Key, StringComparer.Ordinal)
            .Select(group => new Project(paths[group.Key], group.Key, group.OrderBy(static symbol => symbol.id, StringComparer.Ordinal).ToArray())).ToArray();
        return new(surface, configuration, null, "Unverified", scripts.GetIdentities(editor),
            Convert.ToHexStringLower(SHA256.HashData(xmlBytes)), projects);

        DocumentationApiExtractor.DocumentationApiEntry Resolve(ISymbol symbol)
        {
            INamedTypeSymbol type = symbol as INamedTypeSymbol ?? symbol.ContainingType;
            string id = type.GetDocumentationCommentId()!;
            if (!origins.TryGetValue(id, out string? original) || !sourceTypes.TryGetValue(original, out var allCandidates))
                throw new InvalidDataException("Projected type has no unambiguous compiler origin and captured engine owner: " + id);
            var candidates = allCandidates.Where(candidate => candidate.assembly == originAssemblies[id]).ToArray();
            if (candidates.Length != 1) throw new InvalidDataException("Projected type origin does not resolve to one captured assembly: " + id);
            var origin = candidates[0];
            if (!declaredScopes.TryGetValue(origin.assembly + "\0" + original, out string? scope))
                throw new InvalidDataException("Projected type lacks its actual export scope declaration: " + id);
            if (paths.TryGetValue(origin.symbol.project, out string? path) && path != origin.path)
                throw new InvalidDataException("A projected owner has inconsistent source paths.");
            paths[origin.symbol.project] = origin.path;
            string originalId = reverseNames.Replace(symbol.GetDocumentationCommentId()!, match => toSource[match.Value]);
            if (ambiguousMembers.Contains(origin.path + "\0" + originalId))
                throw new InvalidDataException("Projected member has conflicting captured source contracts: " + originalId);
            if (!sourceMembers.TryGetValue(origin.path + "\0" + originalId, out var member))
            {
                if (symbol is IMethodSymbol { MethodKind: MethodKind.Constructor, Parameters.Length: 0, ContainingType.IsValueType: true })
                    return origin.symbol with { documentation = string.Empty, implicitDeclaration = true, scriptingScope = scope };
                if (symbol is IMethodSymbol { MethodKind: MethodKind.Constructor, Parameters.Length: 0 })
                    return origin.symbol with
                    {
                        documentation = new XElement("member", new XAttribute("name", symbol.GetDocumentationCommentId()!),
                            new XElement("summary", "Appears in the generated script reference without a matching public or protected engine constructor."),
                            new XElement("remarks", "This is an IDE projection artifact, not a supported construction path. Do not call this constructor from scripts; obtain the instance through the containing type's documented owner or factory.")).ToString(SaveOptions.DisableFormatting),
                        implicitDeclaration = true, projectionOnly = true, scriptingScope = scope
                    };
                throw new InvalidDataException("Projected member has no exact captured source declaration: " + symbol.GetDocumentationCommentId());
            }
            string xml = member.documentation;
            if (!string.IsNullOrWhiteSpace(xml))
            {
                XElement contract = XElement.Parse(xml);
                foreach (XAttribute attribute in contract.DescendantsAndSelf().Attributes()
                    .Where(static attribute => attribute.Name.LocalName is "name" or "cref"))
                    attribute.Value = forwardNames.Replace(attribute.Value, match => toLogical[match.Value]);
                xml = contract.ToString(SaveOptions.DisableFormatting);
            }
            return member with { documentation = xml, scriptingScope = scope };
        }
    }

    private static Regex IdentityPattern(IEnumerable<string> names) => new(
        @"(?<![\w.])(?:" + string.Join('|', names.OrderByDescending(static name => name.Length).Select(Regex.Escape)) + @")(?!\w)",
        RegexOptions.CultureInvariant);

    internal static async Task<IReadOnlyList<Comparison>> CompareAsync(
        DocumentationSnapshot snapshot,
        DocumentationCatalogReport catalog,
        IReadOnlySet<string> modes,
        string configuration,
        string? target,
        string? runtimePath,
        string? editorPath,
        Func<IReadOnlyList<MetadataReference>> frameworkReferences,
        CancellationToken cancellationToken
    ) {
        // A compile-only snapshot can contain no API facts. Its receipt has no projection
        // proof and cannot satisfy publication's separate API evidence requirements.
        if (!catalog.references.Any(reference => reference.model is "runtime-script" or "editor-script")) return [];
        DocumentationReferenceInfo[] selected = catalog.references.Where(reference => reference.configuration == configuration
            && (reference.target is null || reference.target == target)).Where(reference =>
            reference.model == "runtime-script" && (modes.Contains("runtime-script") || modes.Contains("editor-script"))
            || reference.model == "editor-script" && modes.Contains("editor-script")).ToArray();
        if ((modes.Contains("runtime-script") || modes.Contains("editor-script")) && !selected.Any(reference => reference.model == "runtime-script")
            || modes.Contains("editor-script") && !selected.Any(reference => reference.model == "editor-script"))
            throw new InvalidDataException("Required script projection facts are absent for the selected configuration and target.");
        if (selected.Length == 0) return [];
        var sourceFacts = new List<byte[]>();
        var requiredOwners = selected.SelectMany(static reference => reference.projects).ToHashSet(StringComparer.Ordinal);
        foreach (DocumentationReferenceInfo source in catalog.references.Where(reference => reference.model == "source-clr"
            && reference.configuration == configuration && (reference.target is null || reference.target == target)
            && reference.projects.Any(requiredOwners.Contains)))
            sourceFacts.Add(await snapshot.ReadReferenceAsync(source.path, cancellationToken).ConfigureAwait(false));
        var results = new List<Comparison>();
        foreach (DocumentationReferenceInfo reference in selected)
        {
            var differences = new List<string>();
            IReadOnlyDictionary<string, string> identities = new Dictionary<string, string>();
            string? xmlIdentity = null;
            string status;
            try
            {
                if (reference.configuration != configuration || reference.target is not null && reference.target != target)
                    throw new InvalidDataException("The projected reference context differs from the selected script compiler context.");
                Capture actual = Read(reference.model, configuration, runtimePath
                    ?? throw new InvalidDataException("The current logical Runtime reference is unavailable."),
                    editorPath, frameworkReferences(), sourceFacts, cancellationToken, reference.projects.ToHashSet(StringComparer.Ordinal));
                identities = actual.projectionIdentities;
                xmlIdentity = actual.documentationIdentity;
                using JsonDocument expected = JsonDocument.Parse(await snapshot.ReadReferenceAsync(reference.path, cancellationToken).ConfigureAwait(false));
                var previous = expected.RootElement.GetProperty("projects").EnumerateArray()
                    .ToDictionary(static project => project.GetProperty("name").GetString()!, StringComparer.Ordinal);
                foreach (Project project in actual.projects)
                {
                    if (!previous.Remove(project.name, out JsonElement old))
                    {
                        differences.Add("Missing projected owner: " + project.name);
                        continue;
                    }
                    if (old.GetProperty("project").GetString() != project.project)
                        differences.Add("Projected owner path changed: " + project.name);
                    var symbols = old.GetProperty("symbols").EnumerateArray()
                        .ToDictionary(static symbol => symbol.GetProperty("id").GetString()!, StringComparer.Ordinal);
                    foreach (DocumentationApiExtractor.DocumentationApiEntry symbol in project.symbols)
                    {
                        if (!symbols.Remove(symbol.id, out JsonElement previousSymbol))
                            differences.Add("Missing projected declaration: " + symbol.id);
                        else if (DocumentationReferences.ComparableSymbol(previousSymbol)
                            != DocumentationReferences.ComparableSymbol(JsonSerializer.SerializeToElement(symbol)))
                            differences.Add("Projected declaration or contract changed: " + symbol.id);
                    }
                    differences.AddRange(symbols.Keys.Select(static id => "Declaration is no longer projected: " + id));
                }
                differences.AddRange(previous.Keys.Select(static owner => "Owner is no longer projected: " + owner));
                status = differences.Count == 0 ? "Passed" : "Failed";
            }
            catch (Exception failure) when (failure is IOException or InvalidDataException or InvalidOperationException or JsonException)
            {
                status = "Blocked";
                differences.Add(failure.Message);
            }
            results.Add(new(reference.path, reference.identity, reference.model, configuration, reference.target, reference.projects,
                status, identities, xmlIdentity, differences));
        }
        return results;
    }

    internal sealed record Comparison(
        string path,
        string identity,
        string model,
        string configuration,
        string? target,
        IReadOnlyList<string> projects,
        string status,
        IReadOnlyDictionary<string, string> projectionIdentities,
        string? documentationIdentity,
        IReadOnlyList<string> differences
    );

    internal sealed record Capture(
        string model,
        string configuration,
        string? target,
        string sourceApiStatus,
        IReadOnlyDictionary<string, string> projectionIdentities,
        string documentationIdentity,
        IReadOnlyList<Project> projects
    );

    internal sealed record Project(
        string project,
        string name,
        IReadOnlyList<DocumentationApiExtractor.DocumentationApiEntry> symbols
    );
}
