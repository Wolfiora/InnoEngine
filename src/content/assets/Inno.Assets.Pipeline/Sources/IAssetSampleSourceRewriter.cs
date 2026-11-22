namespace Inno.Assets.Pipeline;

/// <summary>
/// Rewrites a source language in a private sample clone before source assets are published.
/// </summary>
public interface IAssetSampleSourceRewriter
{
    /// <summary>
    /// Rewrites staged source files and registers any changed serialized type identities.
    /// </summary>
    /// <param name="context">
    /// The transaction-owned staged clone.
    /// </param>
    /// <remarks>
    /// Runs on a worker with a pinned extension generation. Access only transaction-owned files
    /// and context data, observe cancellation, and do not access live Assets or Editor state.
    /// </remarks>
    void Transform(AssetSampleTransformContext context);
}
