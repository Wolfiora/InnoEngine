using System;
using System.Linq;
using Inno.Assets.Pipeline;
using Inno.Core.Graphs;
using Inno.Editor.Interactions;
using Inno.Rendering.Shaders;

namespace Inno.Editor.Panel.ShaderEditor;

[EditorAction("shader/group", ShaderEditorCanvas.C_AREA)]
internal sealed class GroupShaderNodes(ShaderEditorDocuments documents) : ShaderSelectionAction(documents)
{
    internal const string C_GROUPS = "inno.editor.shader.groups";

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
        ShaderCanvasGroup[] groups = Read(documents, graph);
        groups =
        [
            ..groups,
            new ShaderCanvasGroup()
            {
                id = Guid.NewGuid().ToString("N"),
                title = "Group " + (groups.Length + 1),
                nodes = draft.canvas.selectedNodes.Select(static id => id.value).ToArray()
            }
        ];
        graph.SetMetadata(C_GROUPS, ShaderGraphDocument.Encode(groups, documents.serialization, documents.context));
        documents.Controller(draft).ReplaceDocument(graph, "Group Shader Nodes");
        draft.selectedGroupID = groups[^1].id;
        documents.Changed(draft);
    }

    internal static ShaderCanvasGroup[] Read(
        ShaderEditorDocuments documents,
        GraphDocument graph
    ) => graph.metadata.TryGetValue(
        C_GROUPS,
        out GraphSerializedValue? value
    ) ? ShaderGraphDocument.Decode<ShaderCanvasGroup[]>(value, documents.serialization, documents.context) : [];
}
