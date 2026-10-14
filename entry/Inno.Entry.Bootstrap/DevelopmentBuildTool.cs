using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
using Inno.Core.IO;

namespace Inno.Entry.Bootstrap;

internal static class DevelopmentBuildTool
{
    internal static DevelopmentBuildToolLease? Open(
        string root,
        DevelopmentToolDefinition tool,
        string configuration,
        string dotnet,
        CancellationToken cancellation
    ) {
        Receipt? receipt = Read(root, tool, configuration);
        if (receipt is null || !File.Exists(receipt.assembly))
            return null;
        string relative = Path.GetRelativePath(Path.Combine(root, "artifacts", "build-tools", tool.cacheName, "hosts"), receipt.assembly);
        if (Path.IsPathRooted(relative) || relative.StartsWith("..", StringComparison.Ordinal))
            return null;
        DevelopmentBuildToolLease? reading = new(receipt.assembly, cancellation);
        try
        {
            if (!File.Exists(receipt.assembly)
                || !SameFiles(receipt.files, ReadFiles(Path.GetDirectoryName(receipt.assembly)!, cancellation)))
                return null;
            string? current = Probe(receipt.assembly, root, tool, configuration, dotnet, cancellation);
            if (current != receipt.inputs)
                return null;
            DevelopmentBuildToolLease accepted = reading;
            reading = null;
            return accepted;
        }
        finally
        {
            reading?.Dispose();
        }
    }

    internal static string Record(
        string root,
        DevelopmentToolDefinition tool,
        string configuration,
        string dotnet,
        string assembly,
        CancellationToken cancellation
    ) {
        assembly = DevelopmentBuildWorkerProcess.Execute("publish", assembly, root, configuration, dotnet, tool.cacheName, cancellation);
        using var reading = new DevelopmentBuildToolLease(assembly, cancellation);
        string inputs = Probe(assembly, root, tool, configuration, dotnet, cancellation)
            ?? throw new InvalidDataException("The prepared Build tool cannot verify its evaluated input closure.");
        var receipt = new Receipt(assembly, inputs, ReadFiles(Path.GetDirectoryName(assembly)!, cancellation));
        AtomicFile.WriteAllBytes(Path.Combine(root, "artifacts", "entry", "build", tool.cacheName, configuration, "ready.json"),
            JsonSerializer.SerializeToUtf8Bytes(receipt));
        return assembly;
    }

    private static Receipt? Read(
        string root,
        DevelopmentToolDefinition tool,
        string configuration
    ) {
        string path = Path.Combine(root, "artifacts", "entry", "build", tool.cacheName, configuration, "ready.json");
        if (!File.Exists(path))
            return null;
        try
        {
            Receipt? receipt = JsonSerializer.Deserialize<Receipt>(File.ReadAllBytes(path));
            if (receipt is null || receipt.files is null || receipt.files.Count == 0 || string.IsNullOrEmpty(receipt.inputs)
                || !Path.IsPathFullyQualified(receipt.assembly))
                return null;
            PathBoundary.RequireUnlinkedPath(root, receipt.assembly);
            return receipt;
        }
        catch (JsonException) { return null; }
    }

    private static string? Probe(
        string assembly,
        string root,
        DevelopmentToolDefinition tool,
        string configuration,
        string dotnet,
        CancellationToken cancellation
    ) {
        try
        {
            return DevelopmentBuildWorkerProcess.Execute("inspect", assembly, root, configuration, dotnet, tool.cacheName, cancellation);
        }
        catch (Exception failure) when (failure is IOException or InvalidOperationException or ArgumentException)
        {
            Console.WriteLine($"Build preparation is required: {failure.Message}");
            return null;
        }
    }

    private static Dictionary<string, string> ReadFiles(
        string directory,
        CancellationToken cancellation
    ) {
        var result = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (string file in PathBoundary.EnumerateFiles(directory).Order(StringComparer.Ordinal))
        {
            cancellation.ThrowIfCancellationRequested();
            if (file.EndsWith(".lock", StringComparison.Ordinal))
                continue;
            using FileStream input = File.OpenRead(file);
            result.Add(Path.GetRelativePath(directory, file), Convert.ToHexStringLower(SHA256.HashData(input)));
        }
        return result;
    }

    private static bool SameFiles(
        IReadOnlyDictionary<string, string> first,
        IReadOnlyDictionary<string, string> second
    ) => first.Count == second.Count && second.All(pair => first.TryGetValue(pair.Key, out string? hash) && hash == pair.Value);

    private sealed record Receipt(
        string assembly,
        string inputs,
        Dictionary<string, string> files
    );

}
