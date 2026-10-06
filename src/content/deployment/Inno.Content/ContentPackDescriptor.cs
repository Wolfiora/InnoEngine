using System;

namespace Inno.Content;

/// <summary>
/// Identifies the complete encoded pack independently of its physical source.
/// </summary>
public sealed record ContentPackDescriptor
{
    /// <summary>
    /// Validates the complete pack identity and its single portable file name.
    /// </summary>
    /// <param name="contentHash">
    /// The SHA-256 of the complete pack bytes.
    /// </param>
    /// <param name="fileName">
    /// The name <c>content-{uppercase SHA-256}.pack</c> relative to the source.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The hash or file name does not identify one current-format pack.
    /// </exception>
    public ContentPackDescriptor(
        string contentHash,
        string fileName
    ) {
        this.contentHash = ContentEntry.NormalizeHash(contentHash);
        if (!string.Equals(fileName, $"content-{this.contentHash}.pack", StringComparison.Ordinal))
            throw new ArgumentException("The pack file name must match its content identity.", nameof(fileName));
        this.fileName = fileName;
    }

    /// <summary>
    /// Gets the SHA-256 of all encoded bytes, including the index.
    /// </summary>
    public string contentHash { get; }

    /// <summary>
    /// Gets the validated single file name used by the selected content source.
    /// </summary>
    public string fileName { get; }
}
