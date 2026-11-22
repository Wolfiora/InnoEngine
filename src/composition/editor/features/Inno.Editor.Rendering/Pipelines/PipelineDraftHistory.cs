using System;
using Inno.Editor.Interactions;

namespace Inno.Editor.Rendering;

[EditorHistoryHandler(PipelineDocuments.C_HISTORY)]
internal sealed class PipelineDraftHistory(PipelineDocuments documents) : EditorHistoryHandler
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
            documents.drafts.ValidateHistory(change, direction);
            return EditorHistoryAvailability.Available();
        }
        catch (Exception error) when (PipelineDocuments.Recoverable(error))
        {
            return EditorHistoryAvailability.Unavailable(error.Message);
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
            documents.drafts.ApplyHistory(change, direction);
            return EditorHistoryResult.Success();
        }
        catch (Exception error) when (PipelineDocuments.Recoverable(error))
        {
            return EditorHistoryResult.Failure(error.Message);
        }
    }

    /// <inheritdoc/>
    protected override bool TryMerge(
        EditorHistoryChange older,
        EditorHistoryChange newer,
        out EditorHistoryChange? merged
    ) => documents.drafts.TryMergeHistory(older, newer, out merged);
}
