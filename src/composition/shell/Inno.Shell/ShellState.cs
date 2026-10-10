namespace Inno.Shell;

/// <summary>
/// Describes the observable lifetime of one owner-thread composition shell.
/// </summary>
public enum ShellState
{
    /// <summary>
    /// Adapter resources have not been initialized.
    /// </summary>
    Created,

    /// <summary>
    /// Adapter resources are ready for the single permitted run.
    /// </summary>
    Ready,

    /// <summary>
    /// Platform events and product frames are advancing.
    /// </summary>
    Running,

    /// <summary>
    /// Platform events continue while product frames and their clock are suspended.
    /// </summary>
    Suspended,

    /// <summary>
    /// The stopping callback is executing while resources remain alive.
    /// </summary>
    Stopping,

    /// <summary>
    /// The run ended or was canceled and the stopping callback completed.
    /// </summary>
    Stopped,

    /// <summary>
    /// Initialization, execution, or retirement failed; another run is prohibited.
    /// </summary>
    Faulted,

    /// <summary>
    /// All owned resources were released successfully.
    /// </summary>
    Disposed
}
