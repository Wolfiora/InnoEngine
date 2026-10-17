using Inno.Rendering;
using System;
using System.Collections.Generic;

namespace Inno.Rendering.Assets;

/// <summary>
/// Identifies one immutable texture source across authoring, deployment, and runtime generations.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("045f13ba-3cc0-5470-b025-08f9febf40e6")]
public readonly record struct RenderTextureArtifactReference
{
    /// <summary>
    /// Creates a reference to one texture slot owned by an imported asset generation.
    /// </summary>
    /// <param name="assetId">
    /// Persistent identity of the asset that owns the named source artifact.
    /// </param>
    /// <param name="contentRevision">
    /// Current committed content revision of the owning asset.
    /// </param>
    /// <param name="slot">
    /// Stable texture slot declaration.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="assetId"/> is empty or <paramref name="contentRevision"/> is negative.
    /// </exception>
    public RenderTextureArtifactReference(
        Guid assetId,
        long contentRevision,
        RenderTextureArtifactSlot slot
    ) {
        if (assetId == Guid.Empty)
            throw new ArgumentException("A texture artifact reference requires a persistent asset identity.", nameof(assetId));
        ArgumentOutOfRangeException.ThrowIfNegative(contentRevision);
        if (string.IsNullOrWhiteSpace(slot.id) || string.IsNullOrWhiteSpace(slot.sourceOutputName))
            throw new ArgumentException("A texture artifact reference requires a valid slot.", nameof(slot));
        this.assetId = assetId;
        this.contentRevision = contentRevision;
        this.slot = slot;
    }

    /// <summary>
    /// Gets the persistent identity of the owning asset.
    /// </summary>
    public Guid assetId { get; }

    /// <summary>
    /// Gets the committed owner content revision used for last-good replacement.
    /// </summary>
    public long contentRevision { get; }

    /// <summary>
    /// Gets the referenced stable texture slot.
    /// </summary>
    public RenderTextureArtifactSlot slot { get; }

}

