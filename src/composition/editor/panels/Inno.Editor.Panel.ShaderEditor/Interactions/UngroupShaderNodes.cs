using System;
using System.Linq;
using Inno.Assets.Pipeline;
using Inno.Core.Graphs;
using Inno.Editor.Interactions;
using Inno.Rendering.Shaders;

namespace Inno.Editor.Panel.ShaderEditor;

[EditorAction("shader/ungroup", ShaderEditorCanvas.C_AREA)]
internal sealed class UngroupShaderNodes(ShaderEditorDocuments documents) : ShaderSelectionAction(documents)
{
    /// <summary>
    /// Executes the prepared operation and publishes only a completed result.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    protected override void Execute(EditorActionContext<AssetFileEntry> context)
    {
        ShaderEditorDocuments.Draft draft = documents.Open(context.target);
        GraphDocument graph = documents.Controller(draft).document.Clone();
        ShaderCanvasGroup[] groups = GroupShaderNodes
            .Read(documents, graph)
            .Where(
                group => (
                    draft.selectedGroupID.Length != 0
                    ? group.id != draft.selectedGroupID
                    : !group.nodes.All(id => draft.canvas.selectedNodes.Contains(new GraphNodeID(id)))
                )
            )
            .ToArray();
        graph.SetMetadata(GroupShaderNodes.C_GROUPS, ShaderGraphDocument.Encode(groups, documents.serialization, documents.context));
        documents.Controller(draft).ReplaceDocument(graph, "Ungroup Shader Nodes");
        draft.selectedGroupID = string.Empty;
        documents.Changed(draft);
    }
}
