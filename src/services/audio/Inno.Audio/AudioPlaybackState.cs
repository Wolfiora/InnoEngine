namespace Inno.Audio;

/// <summary>
/// Describes the observable lifecycle of one playback voice.
/// </summary>
public enum AudioPlaybackState
{
    /// <summary>
    /// The voice is waiting for clip preparation.
    /// </summary>
    Preparing,
    /// <summary>
    /// The voice is scheduled against the audio clock.
    /// </summary>
    Scheduled,
    /// <summary>
    /// The voice is actively advancing.
    /// </summary>
    Playing,
    /// <summary>
    /// The voice is paused at its current cursor.
    /// </summary>
    Paused,
    /// <summary>
    /// The voice has completed and no longer accepts control commands.
    /// </summary>
    Completed,
    /// <summary>
    /// The handle is invalid, stale, or unknown to the active runtime.
    /// </summary>
    Invalid
}
