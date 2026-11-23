namespace Inno.Audio;

/// <summary>
/// Contributes optional buses and processors to a candidate mixer graph.
/// </summary>
public abstract class AudioMixerFeature
{
    /// <summary>
    /// Adds feature-owned graph declarations on the host control thread.
    /// </summary>
    /// <param name="builder">
    /// Candidate graph builder valid only for this invocation.
    /// </param>
    /// <param name="state">
    /// Reload-safe neutral feature state.
    /// </param>
    public abstract void Build(
        AudioMixerBuilder builder,
        SerializedAudioExtensionState state
    );
}
