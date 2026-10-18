using System;

namespace Inno.Player.Runtime;

/// <summary>
/// Owns the two metadata documents required to identify and verify a Player deployment.
/// </summary>
public sealed class PlayerContentMetadata
{
    /// <summary>
    /// Copies deployment metadata so the source can retire its own buffers immediately.
    /// </summary>
    /// <param name="manifest">
    /// The complete strict runtime manifest envelope.
    /// </param>
    /// <param name="catalog">
    /// The serialized runtime content catalog.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Either document is empty.
    /// </exception>
    public PlayerContentMetadata(
        ReadOnlySpan<byte> manifest,
        ReadOnlySpan<byte> catalog
    ) {
        if (manifest.IsEmpty || catalog.IsEmpty)
            throw new ArgumentException("A Player deployment requires nonempty manifest and catalog documents.");
        this.manifest = manifest.ToArray();
        this.catalog = catalog.ToArray();
    }

    /// <summary>
    /// Gets the owned manifest envelope, valid for this metadata object's lifetime.
    /// </summary>
    public ReadOnlyMemory<byte> manifest { get; }

    /// <summary>
    /// Gets the owned serialized content catalog, valid for this metadata object's lifetime.
    /// </summary>
    public ReadOnlyMemory<byte> catalog { get; }
}
