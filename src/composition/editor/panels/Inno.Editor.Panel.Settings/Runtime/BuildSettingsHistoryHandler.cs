using System;
using System.IO;
using Inno.Build;
using Inno.Editor.Interactions;

namespace Inno.Editor.Panel.Settings;

[EditorHistoryHandler(BuildSettingsHistory.C_KIND)]
internal sealed class BuildSettingsHistoryHandler(BuildSettingsStore settings) : EditorHistoryHandler
{
    /// <summary>
    /// Evaluates whether the requested change can be applied to the current generation.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    /// <param name="change">
    /// The neutral change payload to query or apply.
    /// </param>
    /// <param name="direction">
    /// The history direction that determines which state is applied.
    /// </param>
    /// <returns>
    /// The validated editor history availability that represents the completed operation.
    /// </returns>
    protected override EditorHistoryAvailability Query(
        EditorHistoryContext context,
        EditorHistoryChange change,
        EditorHistoryDirection direction
    ) {
        try
        {
            (ReadOnlyMemory<byte> before, ReadOnlyMemory<byte> after) = BuildSettingsHistory.Read(change);
            settings.ValidateDocument(before.Span);
            settings.ValidateDocument(after.Span);
            return EditorHistoryAvailability.Available();
        }
        catch (Exception exception) when (
            exception is InvalidDataException or InvalidOperationException or IOException or UnauthorizedAccessException)
        {
            return EditorHistoryAvailability.Unavailable(exception.Message);
        }
    }

    /// <summary>
    /// Applies a validated change atomically at the caller-controlled commit point.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    /// <param name="change">
    /// The neutral change payload to query or apply.
    /// </param>
    /// <param name="direction">
    /// The history direction that determines which state is applied.
    /// </param>
    /// <returns>
    /// The validated editor history result that represents the completed operation.
    /// </returns>
    protected override EditorHistoryResult Apply(
        EditorHistoryContext context,
        EditorHistoryChange change,
        EditorHistoryDirection direction
    ) {
        byte[] original = settings.CaptureDocument();
        try
        {
            (ReadOnlyMemory<byte> before, ReadOnlyMemory<byte> after) = BuildSettingsHistory.Read(change);
            settings.RestoreDocument(direction == EditorHistoryDirection.Undo ? before.Span : after.Span);
            return EditorHistoryResult.Success();
        }
        catch (Exception exception) when (
            exception is InvalidDataException or InvalidOperationException or IOException or UnauthorizedAccessException)
        {
            try
            {
                settings.RestoreDocument(original);
            }
            catch (Exception rollbackException)
            {
                return StateIntegrityFailure(
                    $"Build Settings transition failed: {exception.Message} "
                    + $"Rollback failed: {rollbackException.Message}"
                );
            }
            return EditorHistoryResult.Failure(exception.Message);
        }
    }
}
