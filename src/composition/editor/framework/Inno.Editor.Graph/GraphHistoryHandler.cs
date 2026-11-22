using System;
using System.Diagnostics;
using Inno.Editor.Interactions;

namespace Inno.Editor.Graph;

[EditorHistoryHandler(GraphHistoryData.C_KIND)]
internal sealed class GraphHistoryHandler(GraphEditorModule module) : EditorHistoryHandler
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
        _ = context;
        _ = direction;
        try
        {
            GraphHistoryData data = GraphHistoryData.Decode(change.payload.ReadBytes(), module.serialization);
            return module.TryResolve(data.documentID, out _)
                   ? EditorHistoryAvailability.Available()
                   : EditorHistoryAvailability.Unavailable($"Graph document '{data.documentID}' is not open.");
        }
        catch (Exception exception)
        {
            return EditorHistoryAvailability.Unavailable($"Graph History payload is invalid: {exception.Message}");
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
        _ = context;
        GraphHistoryTransitionResult transition = GraphHistoryTransition.Apply(module, change, direction);
        return transition.stateIntegrityLost
               ? StateIntegrityFailure(transition.result.message)
               : transition.result;
    }

    /// <summary>
    /// Attempts to merge without changing state when the operation cannot complete.
    /// </summary>
    /// <param name="older">
    /// The earlier history payload considered for coalescing.
    /// </param>
    /// <param name="newer">
    /// The later history payload considered for coalescing.
    /// </param>
    /// <param name="merged">
    /// Receives the neutral coalesced payload when merging succeeds.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the requested condition is satisfied; otherwise, <see langword="false"/>.
    /// </returns>
    protected override bool TryMerge(
        EditorHistoryChange older,
        EditorHistoryChange newer,
        out EditorHistoryChange? merged
    ) {
        merged = null;
        if (older.mergeKey is null || !StringComparer.Ordinal.Equals(older.mergeKey, newer.mergeKey))
        {
            return false;
        }

        try
        {
            GraphHistoryData previous = GraphHistoryData.Decode(older.payload.ReadBytes(), module.serialization);
            GraphHistoryData current = GraphHistoryData.Decode(newer.payload.ReadBytes(), module.serialization);
            if (previous.documentID != current.documentID
                || Stopwatch.GetElapsedTime(previous.timestamp, current.timestamp).TotalSeconds > 1.0
            ) {
                return false;
            }

            var combined = new GraphHistoryData(previous.documentID, previous.before, current.after, current.timestamp);
            merged = new EditorHistoryChange(
                GraphHistoryData.C_KIND,
                EditorHistoryPayload.FromBytes(combined.Encode()),
                older.mergeKey
            );
            return true;
        }
        catch
        {
            return false;
        }
    }
}
