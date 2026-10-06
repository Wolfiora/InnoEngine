using System.IO;

namespace Inno.Audio;

/// <summary>
/// Supplies immutable encoded audio independently of any backend's input layout.
/// </summary>
public interface IAudioClipSource
{
    /// <summary>
    /// Gets the SHA-256 identity of the complete encoded bytes.
    /// </summary>
    string contentHash { get; }

    /// <summary>
    /// Gets the exact encoded byte length used to bound preparation.
    /// </summary>
    long length { get; }

    /// <summary>
    /// Opens a reader with an independent content pin and cursor.
    /// </summary>
    /// <returns>
    /// A caller-owned readable stream; missing or retired content fails the operation.
    /// </returns>
    Stream OpenRead();
}
