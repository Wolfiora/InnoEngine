namespace Inno.Audio;

/// <summary>
/// Describes the availability of the active audio output generation.
/// </summary>
public enum AudioDeviceState
{
    /// <summary>
    /// The device has not completed initialization.
    /// </summary>
    Initializing,
    /// <summary>
    /// The device is available for audible playback.
    /// </summary>
    Ready,
    /// <summary>
    /// Playback is advancing without an output device.
    /// </summary>
    Muted,
    /// <summary>
    /// The active output device was lost and recovery is pending.
    /// </summary>
    Lost,
    /// <summary>
    /// The device has been released and cannot accept work.
    /// </summary>
    Disposed
}
