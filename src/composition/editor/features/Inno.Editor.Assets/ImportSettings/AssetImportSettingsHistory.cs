using System;
using System.IO;
using Inno.Core.Execution;
using Inno.Editor.Interactions;

namespace Inno.Editor.Assets;

[EditorHistoryHandler(AssetImportSettingsEdits.C_HISTORY)]
internal sealed class AssetImportSettingsHistory(AssetImportSettingsEdits edits) : EditorHistoryHandler
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
            edits.Validate(change, direction);
            return EditorHistoryAvailability.Available();
        }
        catch (Exception failure) when ((failure is IOException or InvalidOperationException or ArgumentException or FormatException)
                                        && RetirementPendingException.Find(failure) is null)
        {
            return EditorHistoryAvailability.Unavailable(failure.Message);
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
        try
        {
            edits.ApplyHistory(change, direction);
            return EditorHistoryResult.Success();
        }
        catch (Exception failure) when ((failure is IOException or InvalidOperationException or ArgumentException or FormatException)
                                        && RetirementPendingException.Find(failure) is null)
        {
            return EditorHistoryResult.Failure(failure.Message);
        }
    }
}
