namespace Inno.Editor.Core;

/// <summary>
/// Describes whether an editor owner has released the state required for a generation change.
/// </summary>
public interface IEditorReloadGuard
{
    /// <summary>
    /// Gets whether an explicit user reload request may be accepted in the current owner state.
    /// </summary>
    bool canRequestReload { get; }

    /// <summary>
    /// Gets whether compilation and activation may proceed without a live isolated runtime session.
    /// </summary>
    bool canActivateReload { get; }

    /// <summary>
    /// Gets the reason admission is blocked, or null when the owner is ready for reload.
    /// </summary>
    string? blockingReason { get; }
}
