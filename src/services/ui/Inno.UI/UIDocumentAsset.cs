using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using Inno.Assets;
using Inno.Extensibility.Types;
using Inno.Text;

namespace Inno.UI;

/// <summary>
/// Represents one imported, language-tagged UI document source.
/// </summary>
[StableTypeID("8b34bafb-f34a-4c19-9853-282485c40b78")]
public sealed class UIDocumentAsset : AssetObject
{
    private const uint C_PAYLOAD_MAGIC = 0x4449_5549;
    private UIDocumentSource? m_source;
    private IReadOnlyList<UIDocumentFontFace> m_fonts = [];
    private string m_implementationID = string.Empty;

    /// <summary>
    /// Gets the frozen source, or null before runtime content is loaded.
    /// </summary>
    public UIDocumentSource? source => m_source;

    /// <summary>
    /// Gets font dependencies declared by this document.
    /// </summary>
    public IReadOnlyList<UIDocumentFontFace> fonts => m_fonts;

    /// <summary>
    /// Gets the exact backend implementation selected when the source was imported.
    /// </summary>
    public string implementationID => m_implementationID;

    /// <summary>
    /// Encodes an importer-owned runtime payload without exposing an implementation-specific format.
    /// </summary>
    /// <param name="implementationID">
    /// Stable backend implementation identity.
    /// </param>
    /// <param name="source">
    /// Validated frozen document source.
    /// </param>
    /// <param name="fonts">
    /// Imported font asset dependencies.
    /// </param>
    /// <returns>
    /// Complete deterministic runtime payload bytes.
    /// </returns>
    public static byte[] CreateRuntimePayload(
        string implementationID,
        UIDocumentSource source,
        IReadOnlyList<UIDocumentFontFace>? fonts = null
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(implementationID);
        ArgumentNullException.ThrowIfNull(source);
        using var stream = new MemoryStream();
        using (var writer = new BinaryWriter(stream, new UTF8Encoding(false, true), leaveOpen: true))
        {
            writer.Write(C_PAYLOAD_MAGIC);
            writer.Write(implementationID);
            writer.Write(source.language.value);
            writer.Write(source.sourceUri);
            writer.Write(source.text);
            writer.Write(fonts?.Count ?? 0);
            if (fonts is not null)
            {
                foreach (UIDocumentFontFace face in fonts)
                {
                    writer.Write(face.assetID.ToByteArray());
                    writer.Write(face.family);
                    writer.Write((int)face.style);
                    writer.Write(face.weight);
                }
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
            m_implementationID = string.Empty;
            return;
        }
        using var stream = new MemoryStream(currentPayload.ToArray(), writable: false);
        using var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen: false);
        if (reader.ReadUInt32() != C_PAYLOAD_MAGIC)
        {
            throw new InvalidDataException("The UI document runtime payload has an invalid header.");
        }
        string implementation = reader.ReadString();
        string language = reader.ReadString();
        string sourceUri = reader.ReadString();
        string text = reader.ReadString();
        int count = reader.ReadInt32();
        if (count is < 0 or > 256)
        {
            throw new InvalidDataException("The UI document declares an invalid number of font faces.");
        }
        var fonts = new List<UIDocumentFontFace>(count);
        for (int index = 0; index < count; index++)
        {
            fonts.Add(
                new UIDocumentFontFace(
                    new Guid(reader.ReadBytes(16)),
                    reader.ReadString(),
                    (TextFontStyle)reader.ReadInt32(),
                    reader.ReadInt32()
                )
            );
        }
        if (stream.Position != stream.Length)
        {
            throw new InvalidDataException("The UI document runtime payload contains trailing data.");
        }
        ArgumentException.ThrowIfNullOrWhiteSpace(implementation);
        m_implementationID = implementation;
        m_source = new UIDocumentSource(new UIDocumentLanguageID(language), text, sourceUri);
        m_fonts = new ReadOnlyCollection<UIDocumentFontFace>(fonts);
    }
}
