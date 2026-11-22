using System;
using Inno.Assets;
using Inno.Extensibility.Types;

namespace Inno.Text;

/// <summary>
/// Represents one imported OpenType font file or collection.
/// </summary>
[StableTypeID("c8c708e2-3fd1-4ee1-9f16-5a47680272af")]
public sealed class FontAsset : AssetObject
{
    private FontMetadata? m_metadata;

    /// <summary>
    /// Gets imported metadata, or null before runtime content is loaded.
    /// </summary>
    public FontMetadata? metadata => m_metadata;

    /// <summary>
    /// Refreshes imported metadata after an artifact commit.
    /// </summary>
    /// <param name="previousPayload">
    /// The previous compact runtime payload.
    /// </param>
    /// <param name="currentPayload">
    /// The current compact runtime payload.
    /// </param>
    protected override void OnRuntimePayloadChanged(
        ReadOnlyMemory<byte> previousPayload,
        ReadOnlyMemory<byte> currentPayload
    ) => m_metadata = currentPayload.IsEmpty ? null : FontMetadataCodec.Decode(currentPayload.Span);
}
