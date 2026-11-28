using System;

namespace Inno.Audio;

/// <summary>
/// Processes interleaved floating-point samples for one mixer bus on the audio control thread.
/// </summary>
public abstract class AudioBlockProcessor : IDisposable
{
    /// <summary>
    /// Transforms each frame in place before the backend submits the block for playback.
    /// </summary>
    /// <param name="samples">
    /// Interleaved samples borrowed only for this invocation.
    /// </param>
    /// <param name="channels">
    /// The number of samples per frame.
    /// </param>
    /// <param name="sampleRate">
    /// The number of frames per second.
    /// </param>
    /// <param name="configuration">
    /// The immutable parameters of this mixer contribution.
    /// </param>
    public abstract void Process(
        Span<float> samples,
        int channels,
        int sampleRate,
        AudioProcessorConfiguration configuration
    );

    /// <summary>
    /// Releases persistent effect state after its final block.
    /// </summary>
    public virtual void Dispose() {}
}
