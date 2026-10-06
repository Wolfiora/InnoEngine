using System;

namespace Inno.Core.IO;

/// <summary>
/// Owns an immutable document snapshot that cannot be replaced through its read boundary.
/// </summary>
public sealed class ReadOnlyByteDocumentStore : IByteDocumentStore
{
    private readonly byte[] m_data;

    /// <summary>
    /// Copies one complete document into this source.
    /// </summary>
    /// <param name="documentName">
    /// The stable logical diagnostic name.
    /// </param>
    /// <param name="data">
    /// Borrowed bytes copied before construction completes.
    /// </param>
    public ReadOnlyByteDocumentStore(
        string documentName,
        ReadOnlySpan<byte> data
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(documentName);
        this.documentName = documentName;
        m_data = data.ToArray();
    }

    /// <inheritdoc />
    public string documentName { get; }

    /// <inheritdoc />
    public bool canWrite => false;

    /// <inheritdoc />
    public bool exists => true;

    /// <inheritdoc />
    public byte[] Read() => (byte[])m_data.Clone();

    /// <inheritdoc />
    public void Write(ReadOnlySpan<byte> data) => throw new NotSupportedException(
        $"Document '{documentName}' is read-only.");
}
