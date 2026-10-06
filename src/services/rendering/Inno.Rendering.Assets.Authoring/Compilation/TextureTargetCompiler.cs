using Inno.Rendering;
using Inno.Rendering.Assets;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Rendering.Assets.Authoring;

/// <summary>
/// Converts supported artist texture sources into a validated portable runtime container.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("01400c3b-21c2-5193-ae8f-3cf3665f4261")]
public interface ITextureTargetCompiler
{
    /// <summary>
    /// Compiles one source texture into an uncompressed KTX artifact with a complete mip chain.
    /// </summary>
    /// <param name="source">
    /// Borrowed encoded image stream retained by the caller until compilation completes.
    /// </param>
    /// <param name="colorSpace">
    /// Sampling color-space contract.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation for the offline compiler process.
    /// </param>
    /// <returns>
    /// An operation producing complete KTX bytes suitable for a backend-neutral texture container upload.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the target compiler rejects the source.
    /// </exception>
    ValueTask<byte[]> CompileKtxAsync(
        Stream source,
        TextureColorSpace colorSpace,
        CancellationToken cancellationToken = default
    );
}

