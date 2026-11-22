using System;

namespace Inno.Audio;

/// <summary>
/// Reports one backend-detected terminal voice transition.
/// </summary>
public readonly record struct AudioDeviceCompletion
{
    /// <summary>
    /// Creates a backend voice completion record.
    /// </summary>
    /// <param name="voice">
    /// Voice that reached a terminal state.
    /// </param>
    /// <param name="reason">
    /// Reason playback ended.
    /// </param>
    public AudioDeviceCompletion(
        AudioDeviceVoiceHandle voice,
        AudioCompletionReason reason
    ) {
        if (!voice.isValid)
        {
            throw new ArgumentException("A valid voice handle is required.", nameof(voice));
        }
        this.voice = voice;
        this.reason = reason;
    }

    /// <summary>
    /// Gets the completed voice handle.
    /// </summary>
    public AudioDeviceVoiceHandle voice { get; }

    /// <summary>
    /// Gets the terminal playback reason.
    /// </summary>
    public AudioCompletionReason reason { get; }
}
