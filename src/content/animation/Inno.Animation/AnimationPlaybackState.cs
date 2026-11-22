namespace Inno.Animation;

/// <summary>
/// Describes the observable lifecycle of one animation playback.
/// </summary>
public enum AnimationPlaybackState
{
    /// <summary>
    /// The playback is advancing and producing samples.
    /// </summary>
    Playing,
    /// <summary>
    /// The playback retains its position without advancing.
    /// </summary>
    Paused
}
