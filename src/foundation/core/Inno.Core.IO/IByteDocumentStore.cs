using System;

namespace Inno.Core.IO;

/// <summary>
/// Reads and replaces one complete document without prescribing its physical location.
/// </summary>
public interface IByteDocumentStore
{
    /// <summary>
    /// Gets a stable diagnostic name for this document, independent of its storage implementation.
    /// </summary>
    string documentName { get; }

    /// <summary>
    /// Gets whether this source permits atomic replacement.
    /// </summary>
    bool canWrite { get; }

    /// <summary>
    /// Gets whether a document currently exists; callers must still handle absence during a later read.
    /// </summary>
    bool exists { get; }

    /// <summary>
    /// Obtains a newly owned complete document snapshot.
    /// </summary>
    /// <returns>
    /// Owned document bytes, or null when no document exists; an empty document remains distinct from absence.
    /// </returns>
    byte[]? Read();

    /// <summary>
    /// Replaces the document atomically after preparation succeeds.
    /// </summary>
    /// <param name="data">
    /// The complete candidate bytes; the implementation cannot retain borrowed caller memory.
    /// </param>
    /// <exception cref="NotSupportedException">
    /// The source is read-only.
    /// </exception>
    void Write(ReadOnlySpan<byte> data);
}
