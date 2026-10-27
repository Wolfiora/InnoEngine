using System.Collections.Generic;
using Inno.Core.Graphs;
using Inno.Editor.Graph;
using Inno.Editor.Shaders;
using Inno.Rendering.Shaders;

namespace Inno.Editor.Panel.ShaderEditor;

internal sealed partial class ShaderEditorDocuments
{
    internal void CompleteCanvasEdit(Draft draft)
    {
        GraphDocumentController controller = Controller(draft);
        // A preview remains presentation-only until this atomic domain edit succeeds.
        // Preserve a released intent when History rejects the operation so it can be retried.
        draft.pendingMove ??= draft.pointer.previewPositions.Count == 0
            ? null
            : new Dictionary<GraphNodeId, GraphPosition>(draft.pointer.previewPositions);
        if (draft.pendingMove is { } positions)
        {
            bool changed = false;
            foreach (var entry in positions)
                changed |= controller.document.FindNode(entry.Key) is not { } current || current.position != entry.Value;
            if (changed)
            {
                controller.MoveNodes(positions);
                Changed(draft);
            }
            draft.pendingMove = null;
        }
        if (draft.resizingNode is GraphNodeId resizing
            && controller.document.FindNode(resizing) is { } node)
        {
            GraphDocument replacement = controller.document.Clone();
            replacement.FindNode(node.id)!.SetValue("editor.width", ShaderGraphDocument.Encode(draft.resizePreviewWidth, serialization, context));
            controller.ReplaceDocument(replacement, "Resize Shader Node");
            Changed(draft);
        }
        CancelCanvasEdit(draft);
    }

    private void CompleteCanvasEdits()
    {
        foreach (Draft draft in m_drafts.Values)
            CompleteCanvasEdit(draft);
    }

    internal static void CancelCanvasEdit(Draft draft)
    {
        draft.pendingMove = null;
        draft.pointer.Cancel();
        draft.resizingNode = null;
    }
}
