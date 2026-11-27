using System;

namespace Inno.Audio;

/// <summary>Accepts independent decoder cursors for incremental playback.</summary>
public interface IAudioStreamClipDevice
{
    /// <summary>Prepares a bounded prefix and retains the cursor factory until clip destruction.</summary>
    /// <param name="descriptor">The stream format and immutable source metadata.</param>
    /// <param name="open">Opens a fresh device-owned cursor on the control thread.</param>
    /// <returns>A clip handle, or an invalid handle when admission fails.</returns>
    AudioClipHandle CreateStreamClip(
        AudioClipDescriptor descriptor,
        Func<AudioStreamReader> open
    );
}
