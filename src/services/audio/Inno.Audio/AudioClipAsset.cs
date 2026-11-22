using System;
using Inno.Assets;
using Inno.Extensibility.Types;

namespace Inno.Audio;

/// <summary>
/// Represents one imported audio source and its compact runtime metadata.
/// </summary>
[StableTypeID("c64bbcb2-f7ee-48f2-85df-4506dc7b86d5")]
public sealed class AudioClipAsset : AssetObject
{
    private AudioClipMetadata? m_metadata;

    /// <summary>
    /// Gets imported metadata, or <see langword="null"/> before runtime content is loaded.
    /// </summary>
    public AudioClipMetadata? metadata => m_metadata;

    /// <summary>
    /// Gets the imported clip duration, or zero before runtime content is loaded.
    /// </summary>
    public TimeSpan duration => m_metadata?.duration ?? TimeSpan.Zero;

    /// <summary>
    /// Refreshes imported metadata after an artifact commit.
    /// </summary>
    /// <param name="previousPayload">
    /// Previously committed compact header.
    /// </param>
    /// <param name="currentPayload">
    /// Newly committed compact header.
    /// </param>
    protected override void OnRuntimePayloadChanged(
        ReadOnlyMemory<byte> previousPayload,
        ReadOnlyMemory<byte> currentPayload
    ) => m_metadata = currentPayload.IsEmpty
                      ? null
                      : AudioClipMetadataCodec.Decode(currentPayload.Span);
}
