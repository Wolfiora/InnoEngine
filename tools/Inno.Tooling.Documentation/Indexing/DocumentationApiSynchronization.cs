using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.IO;
using Inno.Build.Toolchains;

namespace Inno.Tooling.Documentation.Indexing;

internal static class DocumentationApiSynchronization
{
    internal static async Task<int> ExecuteAsync(string operation, string[] arguments, CancellationToken cancellation)
    {
        if (arguments.Length != 2 || arguments[0] != "--docs-root")
            throw new ArgumentException("Usage: docs <api-plan|api-check|api-sync> --docs-root CHECKOUT");
        string root = Path.GetFullPath(arguments[1]);
        string output = PathBoundary.RequireUnlinkedPath(root, Path.Combine(root, "artifacts", "api-sync"));
        var publisher = new BuildCachePublisher(output, BuildCacheArtifactRole.ReusableWorkspace);
        BuildCacheOwnerCatalog.Register(root, output, "documentation-api-sync", publisher.role, cancellation);
        using FileLease activity = await publisher.AcquireUseAsync(cancellation).ConfigureAwait(false);
        using FileLease writing = await FileLease.AcquireAsync(Path.Combine(output, "writer.lock"),
            Timeout.InfiniteTimeSpan, cancellation).ConfigureAwait(false);
        string bindingPath = DocumentationApiFacts.Resolve(root, "reference/engine.lock.json");
        byte[] bindingBytes = File.ReadAllBytes(bindingPath);
        DocumentationBinding binding = JsonSerializer.Deserialize<DocumentationBinding>(bindingBytes)
            ?? throw new InvalidDataException("Missing engine API binding.");
        DocumentationApiPlan plan = DocumentationApiContent.Plan(root, binding);
        if (operation == "api-plan")
        {
            Console.WriteLine(JsonSerializer.Serialize(plan));
            return 0;
        }
        if (operation == "api-check")
        {
            Console.WriteLine(JsonSerializer.Serialize(new { schema = "inno-api-editorial/1", status = plan.edits.Count == 0 ? "Passed" : "Failed",
                plan.symbols, conclusion = plan.warnings.Count == 0 ? "success" : "neutral", plan.warnings,
                pendingSynchronization = plan.edits.Select(edit => edit.path).ToArray() }));
            return plan.edits.Count == 0 ? 0 : 1;
        }
        var originals = new Dictionary<string, byte[]?>(StringComparer.Ordinal);
        foreach (DocumentationApiEdit edit in plan.edits)
        {
            string path = DocumentationApiFacts.Resolve(root, edit.path);
            byte[]? bytes = File.Exists(path) ? File.ReadAllBytes(path) : null;
            string? identity = bytes is null ? null : DocumentationMarkdown.Hash(File.ReadAllText(path));
            if (identity != edit.before) throw new IOException("API content changed before synchronization: " + edit.path);
            originals.Add(path, bytes);
        }
        if (!File.ReadAllBytes(bindingPath).SequenceEqual(bindingBytes)) throw new IOException("API input binding changed before synchronization.");
        cancellation.ThrowIfCancellationRequested();
        // This bounded local transaction is committed to Git only by the protected publisher.
        // Cancellation is observed before mutation; I/O failures restore every original byte.
        try
        {
            foreach (DocumentationApiEdit edit in plan.edits)
            {
                string path = DocumentationApiFacts.Resolve(root, edit.path);
                if (edit.text is null) File.Delete(path);
                else AtomicFile.WriteAllBytes(path, Encoding.UTF8.GetBytes(edit.text));
            }
        }
        catch
        {
            foreach (var original in originals)
                if (original.Value is null) File.Delete(original.Key);
                else AtomicFile.WriteAllBytes(original.Key, original.Value);
            throw;
        }
        Console.WriteLine(JsonSerializer.Serialize(new { status = "Synchronized", plan.symbols, changedFiles = plan.edits.Count,
            warnings = plan.warnings.Count, editorialConclusion = plan.warnings.Count == 0 ? "success" : "neutral" }));
        return 0;
    }
}
