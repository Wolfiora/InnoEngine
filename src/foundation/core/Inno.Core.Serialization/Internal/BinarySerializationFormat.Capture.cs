using System;

namespace Inno.Core.Serialization;

internal static partial class BinarySerializationFormat
{
    private const int C_MAX_CAPTURE_BLOCK_BYTES = 64 * 1024;

    private static byte[]? TryGetEncodedCapture(ObjectSerializationNode node)
    {
        if (node.encodedCapture is { } encoded)
            return encoded;
        if (node.captureKey is null || node.encodingTooLarge)
            return null;
        int length;
        try
        {
            length = GetEncodedSize(node);
        }
        catch (OverflowException)
        {
            // An optional bounded memo must not impose ToArray's contiguous-size limit on streaming.
            node.RejectEncodedCapture();
            return null;
        }
        if (length > C_MAX_CAPTURE_BLOCK_BYTES)
        {
            node.RejectEncodedCapture();
            return null;
        }
        // This is the same canonical encoder. Bypass only this root to avoid recursive memo creation;
        // independently frozen child captures can still share their already encoded blocks.
        return node.PublishEncodedCapture(Encode(node, includeMagic: false, useObjectCache: false));
    }
}
