using System;
using Inno.Core.Graphs;
using Inno.Editor.Interactions;

namespace Inno.Editor.Graph;

internal static class GraphHistoryTransition
{
    /// <summary>
    /// Applies a validated change atomically at the caller-controlled commit point.
    /// </summary>
    /// <param name="module">
    /// The module consumed by apply; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="change">
    /// The neutral change payload to query or apply.
    /// </param>
    /// <param name="direction">
    /// The history direction that determines which state is applied.
    /// </param>
    /// <returns>
    /// The validated graph history transition result that represents the completed operation.
    /// </returns>
    public static GraphHistoryTransitionResult Apply(
        GraphEditorModule module,
        EditorHistoryChange change,
        EditorHistoryDirection direction
    ) {
        try
        {
            GraphHistoryData data = GraphHistoryData.Decode(change.payload.ReadBytes(), module.serialization);
            if (!module.TryResolve(data.documentID, out GraphDocumentSession? session) || session is null)
            {
                return new GraphHistoryTransitionResult(
                    EditorHistoryResult.Failure($"Graph document '{data.documentID}' is not open."),
                    false
                );
            }

            GraphDocument original = session.document.Clone();
            try
            {
                session.document.ReplaceContents(
                    GraphHistoryDocumentCodec.Decode(
                        direction == EditorHistoryDirection.Undo ? data.before : data.after,
                        module.serialization
                    )
                );
                session.revision++;
                session.isDirty = true;
                return new GraphHistoryTransitionResult(EditorHistoryResult.Success(), false);
            }
            catch (Exception exception)
            {
                try
                {
                    session.document.ReplaceContents(original);
                }
                catch (Exception rollbackException)
                {
                    return new GraphHistoryTransitionResult(
                        EditorHistoryResult.Failure(
                            $"Graph transition failed: {exception.Message} Rollback failed: {rollbackException.Message}"
                        ),
                        true
                    );
                }

                return new GraphHistoryTransitionResult(EditorHistoryResult.Failure(exception.Message), false);
            }
        }
        catch (Exception exception)
        {
            return new GraphHistoryTransitionResult(
                EditorHistoryResult.Failure($"Graph History payload is invalid: {exception.Message}"),
                false
            );
        }
    }
}

internal readonly record struct GraphHistoryTransitionResult(
    EditorHistoryResult result,
    bool stateIntegrityLost
);
