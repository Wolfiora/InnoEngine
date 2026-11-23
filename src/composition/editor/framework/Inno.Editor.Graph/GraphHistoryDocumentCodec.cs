using System;
using Inno.Core.Graphs;
using Inno.Core.Serialization;

namespace Inno.Editor.Graph;

internal static class GraphHistoryDocumentCodec
{
    /// <summary>
    /// Encodes the supplied value into deterministic structured data.
    /// </summary>
    /// <param name="document">
    /// The document consumed by encode; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="serialization">
    /// The serialization consumed by encode; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <returns>
    /// An immutable snapshot of the values selected by the operation.
    /// </returns>
    public static byte[] Encode(
        GraphDocument document,
        SerializationRegistry serialization
    ) => GraphDocumentCodec.Encode(document, serialization);

    /// <summary>
    /// Decodes deterministic structured data into a validated value.
    /// </summary>
    /// <param name="bytes">
    /// The complete immutable byte payload consumed by this operation.
    /// </param>
    /// <param name="serialization">
    /// The serialization consumed by decode; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <returns>
    /// The validated graph document that represents the completed operation.
    /// </returns>
    public static GraphDocument Decode(
        ReadOnlySpan<byte> bytes,
        SerializationRegistry serialization
    ) => GraphDocumentCodec.Decode(bytes, serialization);
}
