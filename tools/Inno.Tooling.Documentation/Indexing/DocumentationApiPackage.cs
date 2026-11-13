using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.IO;

namespace Inno.Tooling.Documentation.Indexing;

internal sealed record DocumentationApiPackageFile(string path, string sha256, string text);
internal sealed record DocumentationApiPackageData(
    string schema, string repository, string revision, string sourceTree, string toolingRevision,
    string contentIdentity, IReadOnlyList<DocumentationApiPackageFile> files
);

internal static class DocumentationApiPackage
{
    private const string C_BINDING = "reference/engine.lock.json";
    private const string C_RECEIPT = "reference/api/export.json";

    internal static async Task<int> ExecuteAsync(string operation, string[] arguments, CancellationToken cancellation)
    {
        var options = new Dictionary<string, string>(StringComparer.Ordinal);
        for (int index = 0; index < arguments.Length; index += 2)
            if (index + 1 == arguments.Length || arguments[index] is not ("--docs-root" or "--package")
                || !options.TryAdd(arguments[index], arguments[index + 1]))
                throw new ArgumentException("Usage: docs <api-package|api-import> --docs-root CHECKOUT --package FILE");
        if (options.Count != 2) throw new ArgumentException("Both docs-root and package are required.");
        string root = Path.GetFullPath(options["--docs-root"]);
        string packagePath = Path.GetFullPath(options["--package"]);
        return await DocumentationApiSynchronization.WithOwnershipAsync(root,
            () => Task.FromResult(ExecuteOwned(operation, root, packagePath, cancellation)), cancellation).ConfigureAwait(false);
    }

    private static int ExecuteOwned(
        string operation,
        string root,
        string packagePath,
        CancellationToken cancellation
    ) {
        cancellation.ThrowIfCancellationRequested();
        if (operation == "api-package")
        {
            DocumentationBinding binding = ReadBinding(File.ReadAllText(DocumentationApiFacts.Resolve(root, C_BINDING)));
            // This verifies the complete eligible owner set and every capture before making a transferable package.
            _ = DocumentationApiFacts.Read(root, binding);
            string[] paths = RequiredPaths(binding);
            DocumentationApiPackageFile[] files = paths.Select(path =>
            {
                string text = File.ReadAllText(DocumentationApiFacts.Resolve(root, path));
                return new DocumentationApiPackageFile(path, DocumentationMarkdown.Hash(text), text);
            }).ToArray();
            var package = new DocumentationApiPackageData("inno-api-package/1", binding.repository, binding.revision,
                binding.sourceTree, binding.toolingRevision, Identity(files), files);
            Validate(package);
            cancellation.ThrowIfCancellationRequested();
            AtomicFile.WriteAllBytes(packagePath, JsonSerializer.SerializeToUtf8Bytes(package, DocumentationStore.json));
            Console.WriteLine(JsonSerializer.Serialize(new { status = "Packaged", package.contentIdentity, files = files.Length }));
            return 0;
        }
        DocumentationApiPackageData incoming = JsonSerializer.Deserialize<DocumentationApiPackageData>(File.ReadAllBytes(packagePath))
            ?? throw new InvalidDataException("Missing API package.");
        DocumentationBinding candidate = Validate(incoming);
        string receiptPath = DocumentationApiFacts.Resolve(root, C_RECEIPT);
        if (File.Exists(receiptPath))
        {
            using JsonDocument receipt = JsonDocument.Parse(File.ReadAllBytes(receiptPath));
            if (receipt.RootElement.GetProperty("contentIdentity").GetString() == incoming.contentIdentity)
            {
                // Validate the retained facts, too: a digest is not a substitute for checking local data.
                DocumentationBinding retained = ReadBinding(File.ReadAllText(DocumentationApiFacts.Resolve(root, C_BINDING)));
                _ = DocumentationApiFacts.Read(root, retained);
                DocumentationApiPackageFile[] retainedFiles = RequiredPaths(retained).Select(path =>
                {
                    string text = File.ReadAllText(DocumentationApiFacts.Resolve(root, path));
                    return new DocumentationApiPackageFile(path, DocumentationMarkdown.Hash(text), text);
                }).ToArray();
                if (Identity(retainedFiles) != incoming.contentIdentity)
                    throw new InvalidDataException("The retained API package does not match its import receipt.");
                Console.WriteLine("{\"status\":\"Unchanged\",\"changedFiles\":0}");
                return 0;
            }
        }
        var updates = incoming.files.ToDictionary(file => file.path, file => (string?)file.text, StringComparer.Ordinal);
        string oldBindingPath = DocumentationApiFacts.Resolve(root, C_BINDING);
        if (File.Exists(oldBindingPath))
        {
            DocumentationBinding previous = ReadBinding(File.ReadAllText(oldBindingPath));
            foreach (string retired in RequiredPaths(previous).Except(updates.Keys, StringComparer.Ordinal)) updates.Add(retired, null);
        }
        updates.Add(C_RECEIPT, JsonSerializer.Serialize(new { schema = "inno-api-import/1", incoming.revision,
            incoming.sourceTree, incoming.toolingRevision, incoming.contentIdentity }) + "\n");
        var originals = updates.Keys.ToDictionary(path => path, path =>
        {
            string absolute = DocumentationApiFacts.Resolve(root, path);
            return File.Exists(absolute) ? File.ReadAllBytes(absolute) : null;
        }, StringComparer.Ordinal);
        cancellation.ThrowIfCancellationRequested();
        try
        {
            foreach (var update in updates)
            {
                string path = DocumentationApiFacts.Resolve(root, update.Key);
                if (update.Value is null) File.Delete(path);
                else AtomicFile.WriteAllBytes(path, Encoding.UTF8.GetBytes(update.Value));
            }
            _ = DocumentationApiFacts.Read(root, candidate);
            // The synchronization transaction restores content on failure. This outer transaction restores facts.
            _ = DocumentationApiSynchronization.ExecuteOwned("api-sync", root, cancellation);
        }
        catch
        {
            foreach (var original in originals)
            {
                string path = DocumentationApiFacts.Resolve(root, original.Key);
                if (original.Value is null) File.Delete(path);
                else AtomicFile.WriteAllBytes(path, original.Value);
            }
            throw;
        }
        return 0;
    }

    private static DocumentationBinding Validate(DocumentationApiPackageData package)
    {
        if (package.schema != "inno-api-package/1" || package.repository != "https://github.com/Wolfiora/InnoEngine"
            || package.files is null || package.files.Count == 0)
            throw new InvalidDataException("Unsupported API package provenance.");
        DocumentationStore.RequireHash(package.revision, "API revision", allowGitTree: true);
        DocumentationStore.RequireHash(package.sourceTree, "API source tree", allowGitTree: true);
        DocumentationStore.RequireHash(package.toolingRevision, "API tooling revision", allowGitTree: true);
        var files = new Dictionary<string, DocumentationApiPackageFile>(StringComparer.Ordinal);
        foreach (DocumentationApiPackageFile file in package.files)
        {
            RequireFactPath(file.path);
            if (!files.TryAdd(file.path, file) || DocumentationMarkdown.Hash(file.text) != file.sha256)
                throw new InvalidDataException("Repeated or corrupt API package file: " + file.path);
        }
        if (!files.TryGetValue(C_BINDING, out DocumentationApiPackageFile? bindingFile))
            throw new InvalidDataException("API package has no binding.");
        DocumentationBinding binding = ReadBinding(bindingFile.text);
        if (binding.repository != package.repository || binding.revision != package.revision || binding.sourceTree != package.sourceTree
            || binding.toolingRevision != package.toolingRevision || !RequiredPaths(binding).SequenceEqual(files.Keys.Order(StringComparer.Ordinal)))
            throw new InvalidDataException("API package file set or binding differs from its provenance.");
        if (binding.apiManifests is null || binding.apiManifests.Count == 0
            || binding.apiManifests.Any(capture => capture.sourceRevision != binding.revision || capture.sourceTree != binding.sourceTree))
            throw new InvalidDataException("A complete API package must describe one exact engine input.");
        if (files[binding.projectManifest].sha256 != binding.projectManifestIdentity
            || binding.apiManifests.Any(capture => files[capture.path].sha256 != capture.sha256)
            || Identity(package.files) != package.contentIdentity)
            throw new InvalidDataException("API package content identity differs from its manifests.");
        return binding;
    }

    private static DocumentationBinding ReadBinding(string text)
        => JsonSerializer.Deserialize<DocumentationBinding>(text) ?? throw new InvalidDataException("Missing API input binding.");

    private static string[] RequiredPaths(DocumentationBinding binding)
    {
        string[] paths = new[] { C_BINDING, binding.projectManifest }.Concat((binding.apiManifests ?? []).Select(capture => capture.path))
            .Order(StringComparer.Ordinal).ToArray();
        foreach (string path in paths) RequireFactPath(path);
        if (paths.Distinct(StringComparer.OrdinalIgnoreCase).Count() != paths.Length)
            throw new InvalidDataException("API binding repeats a fact path.");
        return paths;
    }

    private static void RequireFactPath(string path)
    {
        if (path == C_BINDING) return;
        if (!(path.StartsWith("reference/api/", StringComparison.Ordinal) || path.StartsWith("reference/projects/", StringComparison.Ordinal))
            || !path.EndsWith(".json", StringComparison.Ordinal) || path.Contains('\\')
            || path.Split('/').Any(part => part is "" or "." or "..") || path is DocumentationApiContent.C_REGISTRY or C_RECEIPT)
            throw new InvalidDataException("API packages can only replace registered JSON facts: " + path);
    }

    private static string Identity(IEnumerable<DocumentationApiPackageFile> files)
        // The binding revision may advance without any API/XML/projection change. Such exports must not create commits.
        => DocumentationMarkdown.Hash(string.Join('\n', files.Where(file => file.path != C_BINDING).OrderBy(file => file.path, StringComparer.Ordinal)
            .Select(file => file.path + "\0" + file.sha256)));
}
