using System.IO;

namespace Inno.Audio;

/// <summary>
/// Opens independent playback cursors for an encoded audio format.
/// </summary>
public abstract class AudioStreamDecoder
{
    /// <summary>
    /// Initializes one cursor on the calling audio control thread.
    /// </summary>
    /// <param name="encoded">
    /// An engine-owned reader positioned at the start. It remains open until the cursor retires.
    /// </param>
    /// <param name="descriptor">
    /// Imported format, channel count and sample rate for the decoded frames.
    /// </param>
    /// <returns>
    /// A new cursor. Throw InvalidDataException for a corrupt or mismatched header.
    /// </returns>
    public abstract AudioStreamReader Open(
        Stream encoded,
        AudioClipDescriptor descriptor
    );
}
