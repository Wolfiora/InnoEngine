using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
using Inno.Core.IO;

namespace Inno.Entry.Bootstrap;

internal static class DevelopmentProduct
{
    internal static string? Open(
        string root,
        string target,
        string configuration,
        string assemblyName,
        CancellationToken cancellationToken
    ) {
        string receiptPath = ReceiptPath(root, target, configuration);
        if (!File.Exists(receiptPath))
            return null;
        Receipt? receipt;
        try
        {
            receipt = JsonSerializer.Deserialize<Receipt>(File.ReadAllBytes(receiptPath));
        }
        catch (JsonException)
        {
            return null;
        }
        if (receipt is null || receipt.target != target || receipt.configuration != configuration
            || receipt.files is null || receipt.files.Count == 0 || string.IsNullOrEmpty(receipt.assemblyPath))
            return null;
        string path = Path.GetFullPath(receipt.assemblyPath, root);
        if (!IsChild(root, path) || !File.Exists(path) || Path.GetFileNameWithoutExtension(path) != assemblyName)
            return null;
        PathBoundary.RequireUnlinkedPath(root, path);
        string directory = Path.GetDirectoryName(path)!;
        if (!receipt.files.ContainsKey(Path.GetFileName(path))
            || !receipt.files.ContainsKey(Path.GetFileNameWithoutExtension(path) + ".deps.json")
            || !receipt.files.Keys.Any(static file => file.StartsWith("native/", StringComparison.Ordinal)))
            return null;
        Dictionary<string, string> current = ReadFiles(directory, cancellationToken);
        return SameFiles(receipt.files, current) ? path : null;
    }

    internal static void Record(
        string root,
        string target,
        string configuration,
        string assemblyPath,
        CancellationToken cancellationToken
    ) {
        string path = Path.GetFullPath(assemblyPath);
        if (!IsChild(root, path) || !File.Exists(path))
            throw new InvalidDataException("The prepared Editor must belong to this checkout.");
        PathBoundary.RequireUnlinkedPath(root, path);
        string directory = Path.GetDirectoryName(path)!;
        Dictionary<string, string> files = ReadFiles(directory, cancellationToken);
        if (!files.ContainsKey(Path.GetFileNameWithoutExtension(path) + ".deps.json")
            || !files.Keys.Any(static file => file.StartsWith("native/", StringComparison.Ordinal)))
            throw new InvalidDataException("The prepared Editor has no complete managed and Native deployment.");
        var receipt = new Receipt(target, configuration, Path.GetRelativePath(root, path), files);
        if (!SameFiles(files, ReadFiles(directory, cancellationToken)))
            throw new IOException("The Editor deployment changed while recording its preparation.");
        cancellationToken.ThrowIfCancellationRequested();
        AtomicFile.WriteAllBytes(ReceiptPath(root, target, configuration), JsonSerializer.SerializeToUtf8Bytes(receipt));
    }

    private static Dictionary<string, string> ReadFiles(
        string directory,
        CancellationToken cancellationToken
    ) {
        var files = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (string file in PathBoundary.EnumerateFiles(directory).Order(StringComparer.Ordinal))
        {
            cancellationToken.ThrowIfCancellationRequested();
            // Persistent coordination files are not application payloads.
            if (file.EndsWith(".lock", StringComparison.Ordinal))
                continue;
            using FileStream input = File.OpenRead(file);
            files.Add(Path.GetRelativePath(directory, file).Replace('\\', '/'),
                Convert.ToHexStringLower(SHA256.HashData(input)));
        }
        return files;
    }

    private static bool SameFiles(
        IReadOnlyDictionary<string, string> expected,
        IReadOnlyDictionary<string, string> actual
    ) => expected.Count == actual.Count && actual.All(pair => expected.TryGetValue(pair.Key, out string? hash)
        && hash == pair.Value);

    private static bool IsChild(
        string root,
        string path
    ) {
        string relative = Path.GetRelativePath(root, path);
        return !Path.IsPathRooted(relative) && relative != ".."
            && !relative.StartsWith(".." + Path.DirectorySeparatorChar, StringComparison.Ordinal);
    }

    private static string ReceiptPath(
        string root,
        string target,
        string configuration
    ) => Path.Combine(root, "artifacts", "products", "editor", target, configuration, "ready.json");

    private sealed record Receipt(
        string target,
        string configuration,
        string assemblyPath,
        IReadOnlyDictionary<string, string> files
    );
}
