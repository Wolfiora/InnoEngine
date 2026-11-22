namespace Inno.Audio;

/// <summary>
/// Selects how encoded clip data is prepared for playback.
/// </summary>
public enum AudioClipLoadMode
{
    /// <summary>
    /// Lets the runtime choose between decoded and streamed storage.
    /// </summary>
    Automatic,
    /// <summary>
    /// Decodes the complete clip before playback.
    /// </summary>
    Decode,
    /// <summary>
    /// Decodes encoded data incrementally while playback advances.
    /// </summary>
    Stream
}
