using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Inno.Assets;
using Inno.Core.Execution;
using Inno.Core.Graphs;
using Inno.Rendering;
using Inno.Rendering.Assets;
using Inno.Rendering.Assets.Authoring;
using Inno.Rendering.Shaders;

namespace Inno.Editor.Panel.ShaderEditor;

internal sealed record ShaderNodeCreation(
    string definitionID,
    Guid sourceID = default,
    string function = ""
);

internal sealed partial class ShaderEditorDocuments
{
    internal GraphNodeRecord PrepareNode(
        Draft draft,
        ShaderNodeCreation creation
    ) {
        GraphDocument graph = Controller(draft).document;
        GraphNodeID? stage = draft.createFromPort is GraphEndpoint endpoint
            ? new(ShaderGraphDocument.Read(graph.FindNode(endpoint.nodeID)!, "stage", string.Empty, serialization, context))
            : draft.activeStage
              ?? graph.nodes.FirstOrDefault(static node => node.definitionID == ShaderGraphDocument.C_OUTPUT_DEFINITION_ID)?.id;
        var node = new GraphNodeRecord(
            new(Guid.NewGuid().ToString("N")),
            creation.definitionID
        ) { position = draft.menuPosition };
        if (stage is GraphNodeID id)
        {
            node.SetValue("stage", ShaderGraphDocument.Encode(id.value, serialization, context));
        }
        if (creation.definitionID == "inno.shader.stage-input")
        {
            HashSet<string> names = graph.nodes.Where(static value => value.definitionID == "inno.shader.stage-input")
                .Select(value => ShaderGraphDocument.Read(value, "settings", new ShaderGraphInputSettings(), serialization, context).id)
                .ToHashSet(StringComparer.Ordinal);
            string name = "parameter";
            for (int suffix = 2; names.Contains(name); suffix++)
            {
                name = "parameter" + suffix;
            }
            node.SetValue(
                "settings",
                ShaderGraphDocument.Encode(new ShaderGraphInputSettings
                {
                    id = name,
                    kind = ShaderIrInputKind.Uniform,
                    type = new() { id = "float4" },
                    semantic = string.Empty
                }, serialization, context)
            );
        }
        else if (creation.definitionID == ShaderGraphNodes.C_INPUT_DEFINITION_ID)
        {
            node.SetValue(
                ShaderGraphDocument.C_SETTINGS_KEY,
                ShaderGraphDocument.Encode(new ShaderGraphNodeInputSettings(), serialization, context)
            );
        }
        else if (creation.definitionID == ShaderGraphNodes.C_OUTPUT_DEFINITION_ID)
        {
            node.SetValue(
                ShaderGraphDocument.C_SETTINGS_KEY,
                ShaderGraphDocument.Encode(new ShaderGraphNodeOutputSettings(), serialization, context)
            );
        }
        if (draft.createFromPort is GraphEndpoint source)
        {
            ShaderNodePort port = draft.ports[source.nodeID].Single(value => value.id == source.portID.value);
            node.SetValue("type", ShaderGraphDocument.Encode(port.type.id, serialization, context));
            if (creation.definitionID == "inno.shader.reroute")
            {
                node.SetValue("valueType", ShaderGraphDocument.Encode(ShaderGraphType.Capture(port.type), serialization, context));
            }
        }
        if (creation.sourceID != Guid.Empty)
        {
            if (!assets.TryGetInfo(creation.sourceID, out AssetInfo? info) || info is null)
            {
                throw new IOException("Referenced Shader authoring asset is unavailable.");
            }
            node.SetValue("sourceId", ShaderGraphDocument.Encode(creation.sourceID, serialization, context));
            node.SetValue("sourcePath", ShaderGraphDocument.Encode(info.assetPath.ToString(), serialization, context));
            if (creation.definitionID == "inno.shader.source")
            {
                ArgumentException.ThrowIfNullOrWhiteSpace(creation.function);
                node.SetValue("function", ShaderGraphDocument.Encode(creation.function, serialization, context));
            }
            else if (creation.definitionID == ShaderGraphNodes.C_CALL_DEFINITION_ID)
            {
                node.SetValue(
                    ShaderGraphNodes.C_INTERFACE_KEY,
                    ShaderGraphDocument.Encode(LoadGraphNodeInterface(creation.sourceID), serialization, context)
                );
            }
        }
        return node;
    }

    internal IReadOnlyList<ShaderNodePort> Describe(GraphNodeRecord node)
    {
        ShaderSourceModuleAnalysis? module = null;
        string implementation = string.Empty;
        ShaderIrStageInput? input = node.definitionID == "inno.shader.stage-input"
            ? ShaderGraphDocument.Read(node, "settings", new ShaderGraphInputSettings(), serialization, context).CreateBinding()
            : null;
        if (node.definitionID == "inno.shader.source")
        {
            Guid id = ShaderGraphDocument.Read(node, "sourceId", Guid.Empty, serialization, context);
            if (id != Guid.Empty
                && assets.TryGetInfo(id, out AssetInfo? info)
                && info is not null
                && info.status != AssetImportStatus.Imported
            ) {
                throw new InvalidOperationException("Source function " + info.status + ": " + string.Join("\n", info.diagnostics));
            }
            if (id != Guid.Empty && assets.TryLoad(id, out ShaderFunctionAsset? function) && function is not null && !function.isMissing)
            {
                string selected = ShaderGraphDocument.Read(node, "function", string.Empty, serialization, context);
                module = frontends.AnalyzeModule(
                    ShaderSourceBundle.Decode(ShaderSourceBundle.Read(function, assets), selected, serialization)
                );
                implementation = function.implementationId;
            }
        }
        GraphNodeRecord described = node;
        if (node.definitionID == ShaderGraphNodes.C_CALL_DEFINITION_ID)
        {
            Guid id = ShaderGraphDocument.Read(node, "sourceId", Guid.Empty, serialization, context);
            if (id != Guid.Empty && TryLoadGraphNodeInterface(id, out ShaderGraphNodeInterface? nodeInterface))
            {
                described = new GraphNodeRecord(node.id, node.definitionID) { position = node.position };
                foreach ((string key, GraphSerializedValue value) in node.values)
                {
                    described.SetValue(key, value);
                }
                described.SetValue(
                    ShaderGraphNodes.C_INTERFACE_KEY,
                    ShaderGraphDocument.Encode(nodeInterface!, serialization, context)
                );
            }
        }
        return nodes.DescribePorts(described, serialization, context, module, implementation, input);
    }

    internal ShaderGraphNodeInterface LoadGraphNodeInterface(Guid id)
        => TryLoadGraphNodeInterface(id, out ShaderGraphNodeInterface? nodeInterface)
           ? nodeInterface!
           : throw new InvalidOperationException("Graph node Shader is unavailable or has import diagnostics.");

    internal ShaderNodePort? CompatibleInput(
        Draft draft,
        GraphNodeRecord node
    ) {
        if (draft.createFromPort is not GraphEndpoint source)
        {
            return null;
        }
        ShaderNodePort output = draft.ports[source.nodeID].Single(value => value.id == source.portID.value);
        return Describe(node).FirstOrDefault(
            value => value.direction == GraphPortDirection.Input && value.type.IsEquivalentTo(output.type)
        );
    }

    internal bool CanCreate(
        Draft draft,
        ShaderNodeCreation creation
    ) {
        if (draft.readOnly)
        {
            return false;
        }
        if (creation.definitionID is ShaderGraphNodes.C_INPUT_DEFINITION_ID or ShaderGraphNodes.C_OUTPUT_DEFINITION_ID
            && Controller(draft).document.nodes.Any(node => node.definitionID == creation.definitionID)
        ) {
                return false;
        }
        if (draft.createFromPort is null)
        {
            return true;
        }
        try
        {
            return CompatibleInput(draft, PrepareNode(draft, creation)) is not null;
        }
        catch (Exception failure) when ((failure is InvalidOperationException or ArgumentException or IOException or FormatException)
                                        && RetirementPendingException.Find(failure) is null)
        {
            return false;
        }
    }

    internal void Create(
        Draft draft,
        ShaderNodeCreation creation
    ) {
        GraphNodeRecord node = PrepareNode(draft, creation);
        ShaderNodePort? input = CompatibleInput(draft, node);
        GraphDocument graph = Controller(draft).document.Clone();
        graph.AddNode(node);
        if (node.definitionID is ShaderGraphNodes.C_INPUT_DEFINITION_ID or ShaderGraphNodes.C_OUTPUT_DEFINITION_ID
            && !ShaderGraphNodes.IsNodeGraph(graph)
        ) {
            ShaderGraphNodes.WriteSettings(graph, new ShaderGraphNodeSettings(), serialization, context);
        }
        if (node.definitionID == "inno.shader.stage-input")
        {
            graph = ShaderGraphBindings.ChangeInput(
                graph,
                node.id,
                ShaderGraphDocument.Read(node, "settings", new ShaderGraphInputSettings(), serialization, context),
                serialization,
                context
            );
        }
        if (input is not null && draft.createFromPort is GraphEndpoint source)
        {
            graph.AddEdge(new(new(Guid.NewGuid().ToString("N")), source, new(node.id, new(input.id))));
        }
        Controller(draft).ReplaceDocument(graph, "Create Shader Node");
        draft.createFromPort = null;
        draft.canvas.SelectNodes([node.id]);
        Changed(draft);
    }

    private bool TryLoadGraphNodeInterface(
        Guid id,
        out ShaderGraphNodeInterface? nodeInterface
    ) {
        nodeInterface = null;
        if (!assets.TryGetInfo(id, out AssetInfo? info) || info is null || info.status != AssetImportStatus.Imported
            || !assets.TryLoad(id, out ShaderAsset? shader) || shader is null || shader.isMissing
        ) {
            return false;
        }
        GraphDocument graph = ShaderGraphArtifact.ReadDocument(ShaderGraphArtifact.Read(shader, assets), serialization);
        nodeInterface = ShaderGraphNodes.ReadInterface(graph, serialization, context);
        return true;
    }
}
