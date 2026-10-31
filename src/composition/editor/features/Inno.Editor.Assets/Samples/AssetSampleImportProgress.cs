namespace Inno.Editor.Assets;

/// <summary>
/// Contains one atomically captured sample import status.
/// </summary>
/// <param name="operationId">
/// The import identity within its owner.
/// </param>
/// <param name="fraction">
/// The completed validation and publication fraction.
/// </param>
/// <param name="status">
/// The current stage description.
/// </param>
/// <param name="cancellationRequested">
/// Whether cancellation is draining owned work.
/// </param>
public readonly record struct AssetSampleImportProgress(
    long operationId,
    float fraction,
    string status,
    bool cancellationRequested
);
