namespace Inno.Editor.Exporting;

/// <summary>
/// Contains one owner-thread export presentation snapshot.
/// </summary>
/// <param name="operationId">
/// The export identity within this service.
/// </param>
/// <param name="fraction">
/// The last measured completion fraction, or null while the active phase has no measured total.
/// </param>
/// <param name="status">
/// The actual stage or outcome message.
/// </param>
/// <param name="error">
/// The current failure diagnostic, or empty.
/// </param>
/// <param name="isBusy">
/// Whether work still owns resources.
/// </param>
/// <param name="cancellationRequested">
/// Whether cancellation is draining work.
/// </param>
public readonly record struct EditorExportProgress(
    long operationId,
    float? fraction,
    string status,
    string error,
    bool isBusy,
    bool cancellationRequested
);
