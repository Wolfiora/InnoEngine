namespace Inno.Scripting.Reload;

/// <summary>
/// Atomically published progress owned by one reload request, containing no runtime or extension objects.
/// </summary>
/// <param name="operationId">
/// The host-local reload request sequence; zero precedes the first request.
/// </param>
/// <param name="fraction">
/// The fraction through compilation, activation and verified retirement.
/// </param>
/// <param name="stage">
/// The actual stage or terminal diagnostic.
/// </param>
/// <param name="cancellationRequested">
/// Whether cancellation has been requested and still requires owned work to drain.
/// </param>
/// <param name="canCancel">
/// Whether this exact operation currently admits cancellation.
/// </param>
public sealed record ScriptReloadProgress(
    long operationId,
    float fraction,
    string stage,
    bool cancellationRequested,
    bool canCancel = false
);
