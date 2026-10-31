namespace Inno.Editor.Scripting;

/// <summary>
/// Captures the progress and cancellation policy of one editor script request.
/// </summary>
/// <param name="operationId">
/// The request identity within the owning script host.
/// </param>
/// <param name="fraction">
/// The completed fraction of the complete reload workflow.
/// </param>
/// <param name="status">
/// The current stage or failure diagnostic.
/// </param>
/// <param name="cancellationRequested">
/// Whether cancellation is draining owned work.
/// </param>
/// <param name="canCancel">
/// Whether this exact operation currently admits cancellation.
/// </param>
public readonly record struct EditorScriptCompilationProgress(
    long operationId,
    float fraction,
    string status,
    bool cancellationRequested,
    bool canCancel = false
);
