namespace Inno.Audio;

/// <summary>
/// Explains why a voice stopped accepting playback commands.
/// </summary>
public enum AudioCompletionReason
{
    /// <summary>
    /// The clip reached its natural end.
    /// </summary>
    NaturalEnd,
    /// <summary>
    /// A caller explicitly stopped the voice.
    /// </summary>
    Stopped,
    /// <summary>
    /// The runtime reclaimed the voice to satisfy its voice budget.
    /// </summary>
    Stolen,
    /// <summary>
    /// Encoded data could not be prepared or decoded.
    /// </summary>
    DecodeFailed,
    /// <summary>
    /// Playback ended because the owning device generation was lost.
    /// </summary>
    DeviceLost
}
