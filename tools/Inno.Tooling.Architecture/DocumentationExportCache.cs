using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using Inno.Build.Toolchains;
using Inno.Core.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Inno.Tooling.Architecture;

/// <summary>
/// Reuses complete per-owner extraction results under the existing publisher and lease protocol.
/// Its inputs are immutable compiler snapshots, not source timestamps or a previous green check.
/// </summary>
internal sealed class DocumentationExportCache : IDisposable
{
    private static readonly string S_TOOL_IDENTITY = ToolIdentity();
    private static readonly JsonSerializerOptions S_JSON = new() { WriteIndented = true, NewLine = "\n" };
    private readonly string m_root;
    private readonly BuildCachePublisher m_publisher;
    private readonly FileLease m_activity;
    private int m_reused;
    private int m_captured;

    internal DocumentationExportCache(
        string engineRoot,
        CancellationToken cancellationToken
    ) {
        m_root = PathBoundary.RequireUnlinkedPath(engineRoot,
            Path.Combine(engineRoot, "artifacts", "documentation-export", "extraction-cache"));
        m_publisher = new(m_root, BuildCacheArtifactRole.ImmutableArtifact);
        BuildCacheOwnerCatalog.Register(engineRoot, m_root, "documentation-api-extraction", m_publisher.role, cancellationToken);
        m_activity = m_publisher.AcquireUseAsync(cancellationToken).AsTask().GetAwaiter().GetResult();
    }

    /// <inheritdoc />
    public void Dispose() => m_activity.Dispose();

    internal T Capture<T>(
        string owner,
        string inputIdentity,
        Func<T> capture,
        CancellationToken cancellationToken
    ) {
        cancellationToken.ThrowIfCancellationRequested();
        string key = Hash(Encoding.UTF8.GetBytes(S_TOOL_IDENTITY + "\n" + inputIdentity));
        string directory = PathBoundary.RequireUnlinkedPath(m_root, Path.Combine(m_root, "entries", key));
        string leasePath = PathBoundary.RequireUnlinkedPath(m_root, Path.Combine(m_root, "leases", key + ".lock"));
        using FileLease ownership = FileLease.AcquireAsync(leasePath, Timeout.InfiniteTimeSpan,
            cancellationToken).AsTask().GetAwaiter().GetResult();
        cancellationToken.ThrowIfCancellationRequested();
        if (BuildArtifactManifest.IsComplete(directory, key, ["facts"]))
        {
            try
            {
                T result = JsonSerializer.Deserialize<T>(File.ReadAllBytes(Path.Combine(directory, "facts", "project.json")))
                    ?? throw new InvalidDataException("The extraction cache contains an empty owner result.");
                m_publisher.Record(directory, leasePath, [], null, null, cancellationToken);
                m_reused++;
                return result;
            }
            catch (Exception failure) when (failure is JsonException or InvalidDataException)
            {
                // A corrupt cache is replaceable acceleration data, never successful API evidence.
            }
        }
        T captured = capture();
        cancellationToken.ThrowIfCancellationRequested();
        string staging = PathBoundary.RequireUnlinkedPath(m_root,
            Path.Combine(m_root, "staging", Guid.NewGuid().ToString("N")));
        try
        {
            Directory.CreateDirectory(Path.Combine(staging, "facts"));
            File.WriteAllBytes(Path.Combine(staging, "facts", "project.json"), JsonSerializer.SerializeToUtf8Bytes(captured, S_JSON));
            BuildArtifactManifest.Write(staging, key, ["facts"]);
            cancellationToken.ThrowIfCancellationRequested();
            Directory.CreateDirectory(Path.GetDirectoryName(directory)!);
            string? quarantine = null;
            if (Directory.Exists(directory))
            {
                quarantine = PathBoundary.RequireUnlinkedPath(m_root,
                    Path.Combine(m_root, "quarantine", key + "-" + Guid.NewGuid().ToString("N")));
                Directory.CreateDirectory(Path.GetDirectoryName(quarantine)!);
                Directory.Move(directory, quarantine);
            }
            try
            {
                Directory.Move(staging, directory);
            }
            catch
            {
                if (quarantine is not null && !Directory.Exists(directory))
                    Directory.Move(quarantine, directory);
                throw;
            }
            // Exported facts own their returned values. This acceleration artifact remains
            // retireable after the operation; a removed project cannot pin an obsolete root.
            m_publisher.Record(directory, leasePath, [], null, null, cancellationToken);
            if (quarantine is not null)
            {
                m_publisher.Record(quarantine, leasePath, [], null, null, cancellationToken);
                Console.WriteLine("API extraction cache quarantined corrupt input result: " + owner);
            }
            m_captured++;
            return captured;
        }
        finally
        {
            if (Directory.Exists(staging))
                Directory.Delete(staging, recursive: true);
        }
    }

    internal void Report()
        => Console.WriteLine($"API extraction owners: {m_captured} captured; {m_reused} reused from matching compiler inputs.");

    private static string ToolIdentity()
    {
        var assembly = typeof(DocumentationExportCache).Assembly;
        string[] sources = assembly.GetManifestResourceNames().Where(static name =>
            name.StartsWith("ApiExtractionSource/", StringComparison.Ordinal)).Order(StringComparer.Ordinal).ToArray();
        if (sources.Length == 0)
            throw new InvalidDataException("The API extractor has no embedded input identity.");
        var identities = new List<string>();
        foreach (string source in sources)
        {
            using Stream stream = assembly.GetManifestResourceStream(source)
                ?? throw new InvalidDataException("Missing extraction identity input: " + source);
            using var reader = new StreamReader(stream);
            identities.Add(source + "\0" + Hash(Encoding.UTF8.GetBytes(reader.ReadToEnd().ReplaceLineEndings("\n"))));
        }
        foreach (var compiler in new[] { typeof(Compilation).Assembly, typeof(CSharpCompilation).Assembly })
            identities.Add(compiler.GetName().Name + "\0" + Hash(File.ReadAllBytes(compiler.Location)));
        return Hash(Encoding.UTF8.GetBytes(string.Join('\n', identities)));
    }

    private static string Hash(byte[] bytes) => Convert.ToHexStringLower(SHA256.HashData(bytes));
}
