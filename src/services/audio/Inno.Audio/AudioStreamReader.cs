using System;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Audio;

/// <summary>
/// Decodes one playback cursor outside the hardware callback. Its owner serializes reads, seeks and disposal.
/// </summary>
public abstract class AudioStreamReader : IDisposable
{
    /// <summary>
    /// Gets whether this cursor supports absolute frame positioning.
    /// </summary>
    public abstract bool canSeek { get; }

    /// <summary>
    /// Reads whole interleaved floating-point frames into a borrowed buffer.
    /// </summary>
    /// <param name="samples">
    /// Writable storage in the declared channel layout and rate, borrowed until the returned operation completes.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation requested when playback stops or the decoder generation retires. Propagate it to pending IO.
    /// </param>
    /// <returns>
    /// The number of complete frames produced. Short reads are valid. Zero signifies end of stream.
    /// </returns>
    public abstract ValueTask<int> ReadAsync(
        Memory<float> samples,
        CancellationToken cancellationToken
    );

    /// <summary>
    /// Moves the next read to an absolute frame at the encoded sample rate.
    /// </summary>
    /// <param name="frame">
    /// A nonnegative zero-based frame index.
    /// </param>
    /// <returns>
    /// Whether positioning succeeded.
    /// </returns>
    public abstract bool Seek(long frame);

    /// <summary>
    /// Releases cursor state before the engine closes the encoded stream.
    /// </summary>
    public abstract void Dispose();
}
