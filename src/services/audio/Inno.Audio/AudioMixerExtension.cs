namespace Inno.Audio;

/// <summary>
/// Builds a base mixer graph on the host control thread.
/// </summary>
public abstract class AudioMixerExtension
{
    /// <summary>
    /// Adds base buses and processors to a candidate mixer graph.
    /// </summary>
    /// <param name="builder">
    /// Candidate graph builder valid only for this invocation.
    /// </param>
    /// <param name="state">
    /// Reload-safe neutral extension state.
    /// </param>
    public abstract void Build(
        AudioMixerBuilder builder,
        SerializedAudioExtensionState state
    );
}
