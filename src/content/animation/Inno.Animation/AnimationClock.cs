namespace Inno.Animation;

/// <summary>
/// Selects the session clock used to advance an animation playback.
/// </summary>
public enum AnimationClock
{
    /// <summary>
    /// Uses scaled simulation time and stops while the session is paused.
    /// </summary>
    Scaled,
    /// <summary>
    /// Uses unscaled session time and continues while simulation is paused.
    /// </summary>
    Unscaled
}
