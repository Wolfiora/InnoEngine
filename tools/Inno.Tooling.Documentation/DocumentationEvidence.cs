using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.IO;
using Inno.Tooling.Documentation.Indexing;

namespace Inno.Tooling.Documentation;

/// <summary>
/// Holds an immutable source certificate selected through an independently trusted content identity.
/// Authentication belongs to the preparation transport; parsing or hashing a receipt does not authenticate it.
/// </summary>
public sealed class DocumentationEvidence
{
    private readonly Certificate m_certificate;

    /// <summary>
    /// Gets the SHA-256 identity authenticated by the caller's preparation boundary.
    /// </summary>
    public string identity { get; }

    private DocumentationEvidence(
        string identity,
        Certificate certificate
    ) {
        this.identity = identity;
        m_certificate = certificate;
    }

    /// <summary>
    /// Reads one bounded certificate whose digest was obtained independently from a trusted producer.
    /// It performs no network access, source scan, compilation or cache mutation.
    /// </summary>
    /// <param name="path">
    /// Explicit local certificate file; symbolic links are rejected.
    /// </param>
    /// <param name="trustedIdentity">
    /// SHA-256 obtained from an authenticated workflow artifact or locally reconciled producer.
    /// Computing this value from an arbitrary downloaded file does not establish trust.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels reading before an immutable certificate is returned.
    /// </param>
    /// <returns>
    /// A caller-owned immutable value, reusable across matching snapshot sessions.
    /// </returns>
    /// <exception cref="IOException">
    /// The file is missing, oversized, linked, corrupt or structurally invalid.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// The read was canceled.
    /// </exception>
    public static async Task<DocumentationEvidence> ReadAsync(
        string path,
        string trustedIdentity,
        CancellationToken cancellationToken = default
    ) {
        DocumentationStore.RequireHash(trustedIdentity, "trusted source certificate");
        path = Path.GetFullPath(path);
        PathBoundary.RequireUnlinkedPath(Path.GetPathRoot(path)!, path);
        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true);
        if (stream.Length is <= 0 or > 4 * 1024 * 1024)
            throw new InvalidDataException("The source certificate exceeds its bounded metadata size.");
        var bytes = new byte[checked((int)stream.Length)];
        await stream.ReadExactlyAsync(bytes, cancellationToken).ConfigureAwait(false);
        if (stream.ReadByte() != -1 || DocumentationMarkdown.Hash(bytes) != trustedIdentity)
            throw new InvalidDataException("Source certificate integrity does not match the trusted producer identity.");
        Certificate certificate;
        try { certificate = DocumentationStore.Deserialize<Certificate>(bytes); }
        catch (JsonException failure) { throw new InvalidDataException("Invalid source certificate structure.", failure); }
        if (certificate.format != "inno-documentation-source/1" || string.IsNullOrWhiteSpace(certificate.repository)
            || certificate.references is not { Length: > 0 } || certificate.producers is not { Length: > 0 })
            throw new InvalidDataException("Source certification requires explicit source, reference and producer coverage.");
        DocumentationStore.RequireHash(certificate.sourceTree, "certified source tree", allowGitTree: true);
        DocumentationStore.RequireHash(certificate.projectManifestIdentity, "certified project inventory");
        var paths = new HashSet<string>(StringComparer.Ordinal);
        foreach (Reference reference in certificate.references)
        {
            if (reference is null || string.IsNullOrWhiteSpace(reference.path) || !paths.Add(reference.path)
                || reference.model != "source-clr" || string.IsNullOrWhiteSpace(reference.configuration)
                || reference.projects is not { Length: > 0 }
                || reference.projects.Any(string.IsNullOrWhiteSpace)
                || reference.projects.Distinct(StringComparer.Ordinal).Count() != reference.projects.Length)
                throw new InvalidDataException("Source certificate reference coverage is invalid or duplicated.");
            DocumentationStore.RequireHash(reference.identity, "certified reference");
        }
        foreach (Producer producer in certificate.producers)
        {
            if (producer is null || string.IsNullOrWhiteSpace(producer.origin))
                throw new InvalidDataException("Source certificate producer provenance is missing.");
            DocumentationStore.RequireHash(producer.receiptIdentity, "producer receipt");
        }
        return new(trustedIdentity, certificate);
    }

    internal void RequireMatch(DocumentationManifest manifest)
    {
        DocumentationReferenceInfo[] references = manifest.references!.Where(static item => item.model != "project-inventory").ToArray();
        if (m_certificate.repository != manifest.binding.repository || m_certificate.sourceTree != manifest.binding.sourceTree
            || m_certificate.projectManifestIdentity != manifest.binding.projectManifestIdentity
            || references.Length != m_certificate.references.Length)
            throw new InvalidDataException("Source certificate does not cover the selected engine and reference snapshot.");
        foreach (DocumentationReferenceInfo reference in references)
        {
            Reference? proof = m_certificate.references.SingleOrDefault(item => item.path == reference.path);
            if (proof is null || proof.identity != reference.identity || proof.model != reference.model
                || proof.configuration != reference.configuration || proof.target != reference.target
                || !proof.projects.Order(StringComparer.Ordinal).SequenceEqual(reference.projects.Order(StringComparer.Ordinal), StringComparer.Ordinal))
                throw new InvalidDataException("Source certificate coverage differs for reference: " + reference.path);
        }
    }

    private sealed record Certificate(
        string format,
        string repository,
        string sourceTree,
        string projectManifestIdentity,
        Reference[] references,
        Producer[] producers
    );

    private sealed record Reference(
        string path,
        string identity,
        string model,
        string configuration,
        string? target,
        string[] projects
    );

    private sealed record Producer(
        string origin,
        string receiptIdentity
    );
}
