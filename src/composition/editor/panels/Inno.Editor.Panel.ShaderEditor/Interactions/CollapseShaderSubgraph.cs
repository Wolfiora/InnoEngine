using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Core.Graphs;
using Inno.Editor.Assets.ImGui;
using Inno.Editor.Graph;
using Inno.Editor.Interactions;
using Inno.Rendering.Assets;
using Inno.Rendering.Shaders;

namespace Inno.Editor.Panel.ShaderEditor;

[EditorAction("shader/collapse-subgraph", ShaderEditorCanvas.C_AREA)]
internal sealed class CollapseShaderSubgraph(
    ShaderEditorDocuments documents,
    AssetBrowserModule browser
) : ShaderSelectionAction(documents)
{
    /// <summary>
    /// Evaluates whether the requested change can be applied to the current generation.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    /// <returns>
    /// The validated editor action state that represents the completed operation.
    /// </returns>
    protected override EditorActionState Query(EditorActionContext<AssetFileEntry> context)
    {
        if (!base.Query(context).isEnabled)
        {
            return EditorActionState.disabled;
        }
        ShaderEditorDocuments.Draft draft = documents.Open(context.target);
        GraphDocument graph = documents.Controller(draft).document;
        GraphNodeRecord[] selected = draft.canvas.selectedNodes.Select(graph.FindNode).OfType<GraphNodeRecord>().ToArray();
        if (selected.Length != draft.canvas.selectedNodes.Count
            || selected.Any(
                static node => (
                    node.definitionID is ShaderGraphDocument.C_OUTPUT_DEFINITION_ID
                    or ShaderGraphNodes.C_INPUT_DEFINITION_ID
                    or ShaderGraphNodes.C_OUTPUT_DEFINITION_ID
                )
            )
        ) {
            return EditorActionState.disabled;
        }
        string[] stages = selected
            .Select(
                node => ShaderGraphDocument.Read(
                    node,
                    ShaderGraphDocument.C_STAGE_KEY,
                    string.Empty,
                    documents.serialization,
                    documents.context
                )
            )
            .Distinct(StringComparer.Ordinal)
            .ToArray();
        if (stages.Length != 1 || stages[0].Length == 0)
        {
            return EditorActionState.disabled;
        }
        HashSet<GraphNodeID> ids = selected.Select(static node => node.id).ToHashSet();
        return graph.edges.Any(edge => ids.Contains(edge.input.nodeID) != ids.Contains(edge.output.nodeID))
               ? EditorActionState.enabled
               : EditorActionState.disabled;
    }

    /// <summary>
    /// Executes the prepared operation and publishes only a completed result.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    protected override void Execute(EditorActionContext<AssetFileEntry> context)
    {
        ShaderEditorDocuments.Draft draft = documents.Open(context.target);
        GraphDocumentController controller = documents.Controller(draft);
        GraphDocument parent = controller.document;
        HashSet<GraphNodeID> selected = [..draft.canvas.selectedNodes];
        GraphNodeRecord[] nodes = parent.nodes.Where(node => selected.Contains(node.id)).ToArray();
        string stage = nodes
            .Select(
                node => ShaderGraphDocument.Read(
                    node,
                    ShaderGraphDocument.C_STAGE_KEY,
                    string.Empty,
                    documents.serialization,
                    documents.context
                )
            )
            .Distinct(StringComparer.Ordinal)
            .Single();
        GraphEdgeRecord[] incomingEdges = parent.edges
            .Where(edge => selected.Contains(edge.input.nodeID) && !selected.Contains(edge.output.nodeID))
            .ToArray();
        GraphEdgeRecord[] outgoingEdges = parent.edges
            .Where(edge => selected.Contains(edge.output.nodeID) && !selected.Contains(edge.input.nodeID))
            .ToArray();
        IGrouping<GraphEndpoint, GraphEdgeRecord>[] inputGroups = incomingEdges.GroupBy(static edge => edge.output).ToArray();
        IGrouping<GraphEndpoint, GraphEdgeRecord>[] outputGroups = outgoingEdges.GroupBy(static edge => edge.output).ToArray();
        var inputNames = new HashSet<string>(StringComparer.Ordinal);
        var outputNames = new HashSet<string>(StringComparer.Ordinal);
        ShaderGraphNodePortDefinition[] inputs = inputGroups
            .Select(
                group => Port(
                    Unique(group.First().input.portID.value, "input", inputNames),
                    draft.ports[group.First().input.nodeID].Single(port => port.id == group.First().input.portID.value).type,
                    required: true
                )
            )
            .ToArray();
        ShaderGraphNodePortDefinition[] outputs = outputGroups
            .Select(
                group => Port(
                    Unique(group.Key.portID.value, "output", outputNames),
                    draft.ports[group.Key.nodeID].Single(port => port.id == group.Key.portID.value).type,
                    required: false
                )
            )
            .ToArray();
        ShaderDefinition parentDefinition = ShaderGraphDocument.ReadDefinition(parent, documents.serialization, documents.context);
        HashSet<string> ownedBindings = nodes
            .Where(static node => node.definitionID == "inno.shader.stage-input")
            .Select(
                node => ShaderGraphDocument.Read(
                    node,
                    ShaderGraphDocument.C_SETTINGS_KEY,
                    new ShaderGraphInputSettings(),
                    documents.serialization,
                    documents.context
                ).id
            )
            .ToHashSet(StringComparer.Ordinal);
        var childDefinition = new ShaderDefinition(
            "Graph Node",
            parentDefinition.properties.Where(property => ownedBindings.Contains(property.id.value)),
            [],
            []
        );
        GraphDocument child = ShaderGraphDocument.Create(childDefinition, documents.serialization, documents.context);
        string displayName = SuggestedName(documents, draft, selected);
        var nodeSettings = new ShaderGraphNodeSettings
        {
            displayName = displayName,
            createPath = "Project",
            kind = ShaderGraphNodeKind.Function,
            effect = outputs.Length == 0 ? ShaderGraphNodeEffect.SideEffect : ShaderGraphNodeEffect.Pure
        };
        ShaderGraphNodes.WriteSettings(child, nodeSettings, documents.serialization, documents.context);
        float minX = nodes.Min(static node => node.position.x);
        float minY = nodes.Min(static node => node.position.y);
        var remap = new Dictionary<GraphNodeID, GraphNodeID>();
        foreach (GraphNodeRecord source in nodes)
        {
            var id = new GraphNodeID("body/" + source.id.value);
            var copy = new GraphNodeRecord(id, source.definitionID)
            {
                position = new(source.position.x - minX + 280, source.position.y - minY + 80)
            };
            foreach ((string key, GraphSerializedValue value)in source.values)
            {
                if (key != ShaderGraphDocument.C_STAGE_KEY)
                {
                    copy.SetValue(key, value.Clone());
                }
            }
            child.AddNode(copy);
            remap.Add(source.id, id);
        }

        foreach (GraphEdgeRecord edge in parent.edges.Where(
            edge => selected.Contains(edge.output.nodeID) && selected.Contains(edge.input.nodeID)
        )) {
            child.AddEdge(
                new GraphEdgeRecord(
                    new GraphEdgeID("body/" + edge.id.value),
                    new GraphEndpoint(remap[edge.output.nodeID], edge.output.portID),
                    new GraphEndpoint(remap[edge.input.nodeID], edge.input.portID)
                )
            );
        }
        if (inputs.Length != 0)
        {
            var boundary = new GraphNodeRecord(new GraphNodeID("function-inputs"), ShaderGraphNodes.C_INPUT_DEFINITION_ID)
            {
                position = new(20, 80)
            };
            boundary.SetValue(
                ShaderGraphDocument.C_SETTINGS_KEY,
                ShaderGraphDocument.Encode(new ShaderGraphNodeInputSettings { ports = inputs }, documents.serialization, documents.context)
            );
            child.AddNode(boundary);
            for (int index = 0; index < inputGroups.Length; index++)
            {
                foreach (GraphEdgeRecord edge in inputGroups[index])
                {
                    child.AddEdge(
                        new GraphEdgeRecord(
                            new GraphEdgeID("input/" + edge.id.value),
                            new GraphEndpoint(boundary.id, new GraphPortID(inputs[index].id)),
                            new GraphEndpoint(remap[edge.input.nodeID], edge.input.portID)
                        )
                    );
                }
            }
        }

        if (outputs.Length != 0)
        {
            var boundary = new GraphNodeRecord(new GraphNodeID("function-outputs"), ShaderGraphNodes.C_OUTPUT_DEFINITION_ID)
            {
                position = new(nodes.Max(static node => node.position.x) - minX + 600, 80)
            };
            boundary.SetValue(
                ShaderGraphDocument.C_SETTINGS_KEY,
                ShaderGraphDocument.Encode(
                    new ShaderGraphNodeOutputSettings { ports = outputs },
                    documents.serialization,
                    documents.context
                )
            );
            child.AddNode(boundary);
            for (int index = 0; index < outputGroups.Length; index++)
            {
                child.AddEdge(
                    new GraphEdgeRecord(
                        new GraphEdgeID("output/" + index),
                        new GraphEndpoint(remap[outputGroups[index].Key.nodeID], outputGroups[index].Key.portID),
                        new GraphEndpoint(boundary.id, new GraphPortID(outputs[index].id))
                    )
                );
            }
        }

        AssetPath path = UniquePath(draft.path, displayName, documents);
        using EditorHistoryTransaction transaction = documents.interactions.history.BeginTransaction("Collapse Shader Subgraph");
        AssetFileEntry created = browser.edits.CreateSource(path, GraphDocumentCodec.Encode(child, documents.serialization));
        Guid sourceID = documents.AssetID(created);
        ShaderGraphNodeInterface nodeInterface = ShaderGraphNodes.ReadInterface(child, documents.serialization, documents.context);
        GraphDocument candidate = parent.Clone();
        foreach (GraphNodeID id in selected)
        {
            candidate.RemoveNode(id);
        }
        var call = new GraphNodeRecord(new GraphNodeID(Guid.NewGuid().ToString("N")), ShaderGraphNodes.C_CALL_DEFINITION_ID)
        {
            position = new(nodes.Average(static node => node.position.x), nodes.Average(static node => node.position.y))
        };
        call.SetValue(ShaderGraphDocument.C_STAGE_KEY, ShaderGraphDocument.Encode(stage, documents.serialization, documents.context));
        call.SetValue("sourceId", ShaderGraphDocument.Encode(sourceID, documents.serialization, documents.context));
        call.SetValue("sourcePath", ShaderGraphDocument.Encode(path.ToString(), documents.serialization, documents.context));
        call.SetValue(
            ShaderGraphNodes.C_INTERFACE_KEY,
            ShaderGraphDocument.Encode(nodeInterface, documents.serialization, documents.context)
        );
        candidate.AddNode(call);
        for (int index = 0; index < inputGroups.Length; index++)
        {
            candidate.AddEdge(
                new GraphEdgeRecord(
                    new GraphEdgeID(Guid.NewGuid().ToString("N")),
                    inputGroups[index].Key,
                    new GraphEndpoint(call.id, new GraphPortID(inputs[index].id))
                )
            );
        }
        for (int index = 0; index < outputGroups.Length; index++)
        {
            foreach (GraphEdgeRecord edge in outputGroups[index])
            {
                candidate.AddEdge(
                    new GraphEdgeRecord(
                        new GraphEdgeID(Guid.NewGuid().ToString("N")),
                        new GraphEndpoint(call.id, new GraphPortID(outputs[index].id)),
                        edge.input
                    )
                );
            }
        }
        ShaderCanvasGroup[] groups = GroupShaderNodes.Read(documents, candidate).Select(group =>
        {
            group.nodes = group.nodes.Where(id => !selected.Contains(new GraphNodeID(id))).ToArray();
            return group;
        }).Where(static group => group.nodes.Length != 0).ToArray();
        candidate.SetMetadata(GroupShaderNodes.C_GROUPS, ShaderGraphDocument.Encode(groups, documents.serialization, documents.context));
        controller.ReplaceDocument(candidate, "Collapse Shader Subgraph");
        transaction.Commit();
        draft.canvas.SelectNodes([call.id]);
        draft.selectedGroupID = string.Empty;
        documents.Changed(draft);
    }

    private static ShaderGraphNodePortDefinition Port(
        string id,
        ShaderSourceType type,
        bool required
    ) => new()
    {
        id = id,
        type = ShaderGraphType.Capture(type),
        required = required
    };

    private static string Unique(
        string candidate,
        string fallback,
        HashSet<string> used
    ) {
        string root = string.IsNullOrWhiteSpace(candidate) ? fallback : candidate;
        string value = root;
        for (int suffix = 2; !used.Add(value); suffix++)
        {
            value = root + suffix;
        }
        return value;
    }

    private static string SuggestedName(
        ShaderEditorDocuments documents,
        ShaderEditorDocuments.Draft draft,
        HashSet<GraphNodeID> selected
    ) {
        ShaderCanvasGroup? group = GroupShaderNodes.Read(
            documents,
            documents.Controller(draft).document
        ).FirstOrDefault(value => value.nodes.Length == selected.Count && value.nodes.All(id => selected.Contains(new GraphNodeID(id))));
        return group is ShaderCanvasGroup exact && !string.IsNullOrWhiteSpace(exact.title) ? exact.title : "Shader Node";
    }

    private static AssetPath UniquePath(
        AssetPath source,
        string name,
        ShaderEditorDocuments documents
    ) {
        AssetSourceMount mount = documents.assets.sourceMounts.Single(value => value.id == source.source);
        string directory = Path.GetDirectoryName(source.localPath)?.Replace('\\', '/') ?? string.Empty;
        string safeName = string.Concat(name.Select(character => Path.GetInvalidFileNameChars().Contains(character) ? '_' : character))
            .Trim();
        if (safeName.Length == 0)
        {
            safeName = "Shader Node";
        }
        string prefix = directory.Length == 0 ? string.Empty : directory + "/";
        var path = new AssetPath(source.source, prefix + safeName + ".ishader");
        for (int suffix = 2; File.Exists(mount.Resolve(path.localPath)) || File.Exists(mount.Resolve(path.localPath) + ".imeta"); suffix++)
        {
            path = new(source.source, prefix + safeName + " " + suffix + ".ishader");
        }
        return path;
    }
}
