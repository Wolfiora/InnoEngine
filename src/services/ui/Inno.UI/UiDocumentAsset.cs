using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Inno.Text;
using Inno.Rendering.Assets;

using Inno.Assets;
using Inno.Extensibility.Types;

namespace Inno.UI;

/// <summary>
/// Represents one imported, language-tagged UI document source.
/// </summary>
[StableTypeId("8b34bafb-f34a-4c19-9853-282485c40b78")]
public sealed class UiDocumentAsset : AssetObject
{
    private const uint C_PAYLOAD_MAGIC = 0x44495549;
    private UiDocumentSource? m_source;
    private IReadOnlyList<UiDocumentFontFace> m_fonts = [];
    private IReadOnlyList<UiDocumentImage> m_images = [];
    private string m_implementationId = string.Empty;

    /// <summary>
    /// Gets the frozen source, or null before runtime content is loaded.
    /// </summary>
    public UiDocumentSource? source => m_source;
    /// <summary>
    /// Gets font dependencies declared by this document.
    /// </summary>
    public IReadOnlyList<UiDocumentFontFace> fonts => m_fonts;

    /// <summary>
    /// Gets imported images declared by the canonical document.
    /// </summary>
    public IReadOnlyList<UiDocumentImage> images => m_images;

    /// <summary>
    /// Gets the exact backend implementation selected when the source was imported.
    /// </summary>
    public string implementationId => m_implementationId;

    /// <summary>
    /// Encodes an importer-owned runtime payload without exposing an implementation-specific format.
    /// </summary>
    /// <param name="implementationId">
    /// Stable backend implementation identity.
    /// </param>
    /// <param name="source">
    /// Validated frozen document source.
    /// </param>
    /// <param name="fonts">
    /// Imported font asset dependencies.
    /// </param>
    /// <param name="images">
    /// Frozen image dependencies recorded by the asset import pipeline.
    /// </param>
    /// <returns>
    /// Complete deterministic runtime payload bytes.
    /// </returns>
    public static byte[] CreateRuntimePayload(
        string implementationId,
        UiDocumentSource source,
        IReadOnlyList<UiDocumentFontFace>? fonts = null,
        IReadOnlyList<UiDocumentImage>? images = null
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(implementationId);
        ArgumentNullException.ThrowIfNull(source);
        if (source.resourceBindings.Count != 0)
            throw new ArgumentException("Runtime resource registrations cannot be persisted.", nameof(source));
        if ((fonts?.Count ?? 0) > 256 || (images?.Count ?? 0) > 4096)
            throw new ArgumentException("The document exceeds its resource declaration budget.");
        using var stream = new MemoryStream();
        using (var writer = new BinaryWriter(stream, new UTF8Encoding(false, true), leaveOpen: true))
        {
            writer.Write(C_PAYLOAD_MAGIC);
            writer.Write(implementationId);
            writer.Write(source.language.value);
            writer.Write(source.sourceUri);
            writer.Write(source.text);
            writer.Write(fonts?.Count ?? 0);
            if (fonts is not null)
                foreach (UiDocumentFontFace face in fonts)
                {
                    writer.Write(face.assetId.ToByteArray());
                    writer.Write(face.family);
                    writer.Write((int)face.style);
                    writer.Write(face.weight);
                }
            writer.Write(images?.Count ?? 0);
            if (images is not null)
                foreach (UiDocumentImage declaration in images)
                {
                    writer.Write(declaration.source);
                    UiImageSource image = declaration.image;
                    writer.Write(image.artifact.assetId.ToByteArray());
                    writer.Write(image.artifact.contentRevision);
                    writer.Write(image.artifact.slot.id);
                    writer.Write(image.artifact.slot.sourceOutputName);
                    writer.Write((int)image.artifact.slot.colorSpace);
                    writer.Write(image.width);
                    writer.Write(image.height);
                }
        }
        return stream.ToArray();
    }

    /// <summary>
    /// Rebuilds runtime-derived state after the serialized asset payload changes.
    /// </summary>
    /// <param name="previousPayload">
    /// The previous payload consumed by on runtime payload changed; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="currentPayload">
    /// The current payload consumed by on runtime payload changed; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    protected override void OnRuntimePayloadChanged(
        ReadOnlyMemory<byte> previousPayload,
        ReadOnlyMemory<byte> currentPayload
    ) {
        if (currentPayload.IsEmpty)
        {
            m_source = null;
            m_fonts = [];
            m_images = [];
            m_implementationId = string.Empty;
            return;
        }
        using var stream = new MemoryStream(currentPayload.ToArray(), writable: false);
        using var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen: false);
        if (reader.ReadUInt32() != C_PAYLOAD_MAGIC)
            throw new InvalidDataException("The UI document runtime payload has an invalid header.");
        string implementation = reader.ReadString();
        string language = reader.ReadString();
        string sourceUri = reader.ReadString();
        string text = reader.ReadString();
        int count = reader.ReadInt32();
        if (count is < 0 or > 256)
            throw new InvalidDataException("The UI document declares an invalid number of font faces.");
        var fonts = new List<UiDocumentFontFace>(count);
        for (int index = 0; index < count; index++)
            fonts.Add(new UiDocumentFontFace(new Guid(reader.ReadBytes(16)), reader.ReadString(),
                (TextFontStyle)reader.ReadInt32(), reader.ReadInt32()));
        int imageCount = reader.ReadInt32();
        if (imageCount is < 0 or > 4096)
            throw new InvalidDataException("The UI document declares an invalid number of images.");
        var images = new List<UiDocumentImage>(imageCount);
        var imageNames = new HashSet<string>(StringComparer.Ordinal);
        for (int index = 0; index < imageCount; index++)
        {
            string name = reader.ReadString();
            Guid assetId = new(reader.ReadBytes(16));
            long revision = reader.ReadInt64();
            string slot = reader.ReadString();
            string output = reader.ReadString();
            var colorSpace = (TextureColorSpace)reader.ReadInt32();
            int width = reader.ReadInt32();
            int height = reader.ReadInt32();
            if (!imageNames.Add(name) || !Enum.IsDefined(colorSpace))
                throw new InvalidDataException("The UI document contains invalid image declarations.");
            images.Add(new(name, new(new(assetId, revision, new(slot, output, colorSpace)), width, height)));
        }
        if (stream.Position != stream.Length)
            throw new InvalidDataException("The UI document runtime payload contains trailing data.");
        ArgumentException.ThrowIfNullOrWhiteSpace(implementation);
        m_implementationId = implementation;
        m_source = new UiDocumentSource(new UiDocumentLanguageId(language), text, sourceUri);
        m_fonts = new ReadOnlyCollection<UiDocumentFontFace>(fonts);
        m_images = new ReadOnlyCollection<UiDocumentImage>(images);
    }
}
