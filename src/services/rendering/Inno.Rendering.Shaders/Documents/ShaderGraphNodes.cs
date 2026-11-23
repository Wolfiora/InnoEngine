using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Core.Graphs;
using Inno.Core.Serialization;
using Inno.Rendering.Assets;

namespace Inno.Rendering.Shaders;

/// <summary>
/// Reads and expands reusable node graphs without retaining assets or provider instances.
/// </summary>
public static class ShaderGraphNodes
{
    /// <summary>
    /// Identifies the single multi-port node-input interface record.
    /// </summary>
    public const string C_INPUT_DEFINITION_ID = "inno.shader.node-inputs";

    /// <summary>
    /// Identifies the optional multi-port node-output interface record.
    /// </summary>
    public const string C_OUTPUT_DEFINITION_ID = "inno.shader.node-outputs";

    /// <summary>
    /// Identifies a reference to another Shader graph used as a node.
    /// </summary>
    public const string C_CALL_DEFINITION_ID = "inno.shader.graph-node";

    /// <summary>
    /// Identifies the serialized interface snapshot retained by a graph-node reference.
    /// </summary>
    public const string C_INTERFACE_KEY = "interface";

    /// <summary>
    /// Identifies graph-level reusable-node metadata, independent of either interface direction.
    /// </summary>
    public const string C_SETTINGS_KEY = "inno.shader.node-settings";

    /// <summary>
    /// Determines whether a Shader graph declares a reusable node interface.
    /// </summary>
    /// <param name="graph">
    /// Graph to inspect.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when graph-level node settings are present.
    /// </returns>
    public static bool IsNodeGraph(GraphDocument graph)
    {
        ArgumentNullException.ThrowIfNull(graph);
        return graph.metadata.ContainsKey(C_SETTINGS_KEY);
    }

    /// <summary>
    /// Writes graph-level reusable-node metadata without coupling it to an input or output record.
    /// </summary>
    /// <param name="graph">
    /// Graph whose node identity is being assigned.
    /// </param>
    /// <param name="settings">
    /// Detached node metadata.
    /// </param>
    /// <param name="serialization">
    /// Owner converter registry.
    /// </param>
    /// <param name="context">
    /// Complete owner reference context.
    /// </param>
    public static void WriteSettings(
        GraphDocument graph,
        ShaderGraphNodeSettings settings,
        SerializationRegistry serialization,
        SerializationContext context
    ) {
        ArgumentNullException.ThrowIfNull(graph);
        ArgumentNullException.ThrowIfNull(settings);
        graph.SetMetadata(C_SETTINGS_KEY, ShaderGraphDocument.Encode(settings, serialization, context));
    }

    /// <summary>
    /// Reads required graph-level reusable-node metadata.
    /// </summary>
    /// <param name="graph">
    /// Graph declaring the reusable node.
    /// </param>
    /// <param name="serialization">
    /// Owner converter registry.
    /// </param>
    /// <param name="context">
    /// Complete owner reference context.
    /// </param>
    /// <returns>
    /// Detached node metadata.
    /// </returns>
    public static ShaderGraphNodeSettings ReadSettings(
        GraphDocument graph,
        SerializationRegistry serialization,
        SerializationContext context
    ) {
        ArgumentNullException.ThrowIfNull(graph);
        if (!graph.metadata.TryGetValue(C_SETTINGS_KEY, out GraphSerializedValue? value))
        {
            throw new InvalidOperationException("The Shader graph does not declare reusable-node settings.");
        }
        return ShaderGraphDocument.Decode<ShaderGraphNodeSettings>(value, serialization, context);
    }

    /// <summary>
    /// Reads and validates the public node interface declared by a Shader graph.
    /// </summary>
    /// <param name="graph">
    /// Detached graph asset source.
    /// </param>
    /// <param name="serialization">
    /// Owner converter registry.
    /// </param>
    /// <param name="context">
    /// Complete owner reference context.
    /// </param>
    /// <returns>
    /// The detached graph-node interface.
    /// </returns>
    public static ShaderGraphNodeInterface ReadInterface(
        GraphDocument graph,
        SerializationRegistry serialization,
        SerializationContext context
    ) {
        ArgumentNullException.ThrowIfNull(graph);
        if (ShaderGraphDocument.ReadTarget(graph, serialization, context).Length != 0)
        {
            throw new InvalidOperationException("A graph-authored node cannot select a Shader Target.");
        }
        if (graph.nodes.Any(static node => node.definitionID == ShaderGraphDocument.C_OUTPUT_DEFINITION_ID))
        {
            throw new InvalidOperationException("A graph-authored node cannot contain explicit GPU stage outputs.");
        }
        GraphNodeRecord[] inputs = graph.nodes.Where(static node => node.definitionID == C_INPUT_DEFINITION_ID).ToArray();
        GraphNodeRecord[] outputs = graph.nodes.Where(static node => node.definitionID == C_OUTPUT_DEFINITION_ID).ToArray();
        if (inputs.Length > 1)
        {
            throw new InvalidOperationException("A graph-authored node can contain at most one Function Inputs record.");
        }
        if (outputs.Length > 1)
        {
            throw new InvalidOperationException("A graph-authored node can contain at most one Function Outputs record.");
        }
        if (inputs.Length == 0 && outputs.Length == 0)
        {
            throw new InvalidOperationException("A graph-authored node requires Function Inputs, Function Outputs, or both.");
        }
        ShaderGraphNodeSettings settings = ReadSettings(graph, serialization, context);
        ShaderGraphNodeInputSettings input = inputs.Length == 0
                                             ? new ShaderGraphNodeInputSettings()
                                             : ShaderGraphDocument.Read(
                                                 inputs[0],
                                                 ShaderGraphDocument.C_SETTINGS_KEY,
                                                 new ShaderGraphNodeInputSettings(),
                                                 serialization,
                                                 context
                                             );
        ShaderGraphNodeOutputSettings output = outputs.Length == 0
                                               ? new ShaderGraphNodeOutputSettings()
                                               : ShaderGraphDocument.Read(
                                                   outputs[0],
                                                   ShaderGraphDocument.C_SETTINGS_KEY,
                                                   new ShaderGraphNodeOutputSettings(),
                                                   serialization,
                                                   context
                                               );
        var result = new ShaderGraphNodeInterface
        {
            displayName = settings.displayName,
            createPath = settings.createPath,
            createOrder = settings.createOrder,
            kind = settings.kind,
            effect = settings.effect,
            role = settings.role,
            inputs = input.ports,
            outputs = output.ports
        };
        Validate(result, inputs.Length, outputs.Length);
        return result;
    }

    /// <summary>
    /// Reads the current interface snapshot stored on a graph-node reference.
    /// </summary>
    /// <param name="node">
    /// Graph-node reference.
    /// </param>
    /// <param name="serialization">
    /// Owner converter registry.
    /// </param>
    /// <param name="context">
    /// Complete owner reference context.
    /// </param>
    /// <returns>
    /// The detached stored interface.
    /// </returns>
    public static ShaderGraphNodeInterface ReadCallInterface(
        GraphNodeRecord node,
        SerializationRegistry serialization,
        SerializationContext context
    ) {
        ShaderGraphNodeInterface result = ShaderGraphDocument.Read(
            node,
            C_INTERFACE_KEY,
            new ShaderGraphNodeInterface(),
            serialization,
            context
        );
        Validate(result, result.inputs.Length == 0 ? 0 : 1, result.outputs.Length == 0 ? 0 : 1);
        return result;
    }

    /// <summary>
    /// Expands every inline graph-node reference and refreshes domain-output interfaces.
    /// </summary>
    /// <param name="graph">
    /// Authored parent graph.
    /// </param>
    /// <param name="resolve">
    /// Resolves a referenced Shader graph by persistent identity and diagnostic path.
    /// </param>
    /// <param name="serialization">
    /// Owner converter registry.
    /// </param>
    /// <param name="context">
    /// Complete owner reference context.
    /// </param>
    /// <returns>
    /// A detached graph containing no inline graph-node references.
    /// </returns>
    public static GraphDocument Expand(
        GraphDocument graph,
        Func<Guid, string, GraphDocument> resolve,
        SerializationRegistry serialization,
        SerializationContext context
    ) {
        ArgumentNullException.ThrowIfNull(graph);
        ArgumentNullException.ThrowIfNull(resolve);
        GraphDocument expanded = graph.Clone();
        Expand(expanded, resolve, serialization, context, []);
        return expanded;
    }

    private static void Expand(
        GraphDocument graph,
        Func<Guid, string, GraphDocument> resolve,
        SerializationRegistry serialization,
        SerializationContext context,
        HashSet<Guid> stack
    ) {
        var retainedDomainOutputs = new HashSet<GraphNodeID>();
        while (graph.nodes.FirstOrDefault(node => (
            node.definitionID == C_CALL_DEFINITION_ID
            && !retainedDomainOutputs.Contains(node.id)
        )) is { } call
        ) {
            Guid sourceID = ShaderGraphDocument.Read(call, "sourceId", Guid.Empty, serialization, context);
            string sourcePath = ShaderGraphDocument.Read(call, "sourcePath", string.Empty, serialization, context);
            if (sourceID == Guid.Empty)
            {
                throw new InvalidOperationException("A graph-node reference has no Shader asset identity.");
            }
            if (!stack.Add(sourceID))
            {
                throw new InvalidOperationException($"Graph-node references contain a cycle at '{sourcePath}'.");
            }
            GraphDocument child = resolve(sourceID, sourcePath).Clone();
            ShaderGraphNodeInterface nodeInterface = ReadInterface(child, serialization, context);
            if (nodeInterface.kind == ShaderGraphNodeKind.DomainOutput)
            {
                call.SetValue(C_INTERFACE_KEY, ShaderGraphDocument.Encode(nodeInterface, serialization, context));
                retainedDomainOutputs.Add(call.id);
                stack.Remove(sourceID);
                continue;
            }
            Expand(child, resolve, serialization, context, stack);
            stack.Remove(sourceID);
            Inline(graph, call, child, nodeInterface, serialization, context);
        }
    }

    private static void Inline(
        GraphDocument parent,
        GraphNodeRecord call,
        GraphDocument child,
        ShaderGraphNodeInterface nodeInterface,
        SerializationRegistry serialization,
        SerializationContext context
    ) {
        GraphNodeRecord? inputNode = child.nodes.SingleOrDefault(static node => node.definitionID == C_INPUT_DEFINITION_ID);
        GraphNodeRecord? outputNode = child.nodes.SingleOrDefault(static node => node.definitionID == C_OUTPUT_DEFINITION_ID);
        string stage = ShaderGraphDocument.Read(call, ShaderGraphDocument.C_STAGE_KEY, string.Empty, serialization, context);
        MergeDefinition(parent, child, stage, serialization, context);
        var map = new Dictionary<GraphNodeID, GraphNodeID>();
        foreach (GraphNodeRecord node in child.nodes.Where(node => node.id != inputNode?.id && node.id != outputNode?.id))
        {
            var mapped = new GraphNodeID(call.id.value + "/" + node.id.value);
            if (parent.FindNode(mapped) is not null)
            {
                throw new InvalidOperationException(
                    $"Inlining '{nodeInterface.displayName}' produced duplicate node identity '{mapped.value}'."
                );
            }
            var copy = new GraphNodeRecord(mapped, node.definitionID)
            {
                position = new(call.position.x + node.position.x, call.position.y + node.position.y)
            };
            foreach ((string key, GraphSerializedValue value) in node.values)
            {
                copy.SetValue(key, value);
            }
            if (stage.Length != 0)
            {
                copy.SetValue(ShaderGraphDocument.C_STAGE_KEY, ShaderGraphDocument.Encode(stage, serialization, context));
            }
            parent.AddNode(copy);
            map.Add(node.id, mapped);
        }

        Dictionary<string, GraphEdgeRecord[]> incoming = parent.edges.Where(edge => edge.input.nodeID == call.id)
            .GroupBy(static edge => edge.input.portID.value, StringComparer.Ordinal)
            .ToDictionary(static group => group.Key, static group => group.ToArray(), StringComparer.Ordinal);
        Dictionary<string, GraphEdgeRecord[]> outgoing = parent.edges.Where(edge => edge.output.nodeID == call.id)
            .GroupBy(static edge => edge.output.portID.value, StringComparer.Ordinal)
            .ToDictionary(static group => group.Key, static group => group.ToArray(), StringComparer.Ordinal);
        HashSet<string> inputIDs = nodeInterface.inputs.Select(static port => port.id).ToHashSet(StringComparer.Ordinal);
        HashSet<string> outputIDs = nodeInterface.outputs.Select(static port => port.id).ToHashSet(StringComparer.Ordinal);
        string? missingInput = incoming.Keys.FirstOrDefault(id => !inputIDs.Contains(id));
        if (missingInput is not null)
        {
            throw new InvalidOperationException(
                $"Graph-node call '{nodeInterface.displayName}' retains unavailable input port '{missingInput}'."
            );
        }
        string? missingOutput = outgoing.Keys.FirstOrDefault(id => !outputIDs.Contains(id));
        if (missingOutput is not null)
        {
            throw new InvalidOperationException(
                $"Graph-node call '{nodeInterface.displayName}' retains unavailable output port '{missingOutput}'."
            );
        }
        foreach (GraphEdgeRecord[] values in incoming.Values)
        {
            if (values.Length != 1)
            {
                throw new InvalidOperationException("A graph-node input has multiple incoming connections.");
            }
        }

        var resolvedInputs = new Dictionary<string, GraphEndpoint>(StringComparer.Ordinal);
        foreach (ShaderGraphNodePortDefinition port in nodeInterface.inputs)
        {
            if (incoming.TryGetValue(port.id, out GraphEdgeRecord[]? edges))
            {
                resolvedInputs.Add(port.id, edges[0].output);
            }
            else if (!port.required)
            {
                resolvedInputs.Add(port.id, AddOptionalZero(parent, call, port, stage, serialization, context));
            }
            else
            {
                throw new InvalidOperationException(
                    $"Required graph-node input '{port.id}' is not connected on '{nodeInterface.displayName}'."
                );
            }
        }

        var resolvedOutputs = new Dictionary<string, GraphEndpoint>(StringComparer.Ordinal);
        foreach (GraphEdgeRecord edge in child.edges)
        {
            bool fromInput = inputNode is not null && edge.output.nodeID == inputNode.id;
            bool toOutput = outputNode is not null && edge.input.nodeID == outputNode.id;
            GraphEndpoint mappedSource;
            if (fromInput)
            {
                if (!resolvedInputs.TryGetValue(edge.output.portID.value, out GraphEndpoint source))
                {
                    continue;
                }
                mappedSource = source;
            }
            else
            {
                if (!map.TryGetValue(edge.output.nodeID, out GraphNodeID sourceNode))
                {
                    continue;
                }
                mappedSource = new(sourceNode, edge.output.portID);
            }
            if (toOutput)
            {
                if (!resolvedOutputs.TryAdd(edge.input.portID.value, mappedSource))
                {
                    throw new InvalidOperationException($"Graph-node output '{edge.input.portID.value}' is connected more than once.");
                }
            }
            else if (map.TryGetValue(edge.input.nodeID, out GraphNodeID targetNode))
            {
                parent.AddEdge(
                    new GraphEdgeRecord(
                        new GraphEdgeID(call.id.value + "/" + edge.id.value),
                        mappedSource,
                        new GraphEndpoint(targetNode, edge.input.portID)
                    )
                );
            }
        }
        foreach (ShaderGraphNodePortDefinition port in nodeInterface.outputs)
        {
            if (!resolvedOutputs.TryGetValue(port.id, out GraphEndpoint source))
            {
                throw new InvalidOperationException($"Graph-node output '{port.id}' has no value inside '{nodeInterface.displayName}'.");
            }
            if (!outgoing.TryGetValue(port.id, out GraphEdgeRecord[]? edges))
            {
                continue;
            }
            foreach (GraphEdgeRecord edge in edges)
            {
                parent.AddEdge(new GraphEdgeRecord(new GraphEdgeID(call.id.value + "/return/" + edge.id.value), source, edge.input));
            }
        }
        parent.RemoveNode(call.id);
    }

    private static void MergeDefinition(
        GraphDocument parent,
        GraphDocument child,
        string stageID,
        SerializationRegistry serialization,
        SerializationContext context
    ) {
        ShaderDefinition parentDefinition = ShaderGraphDocument.ReadDefinition(parent, serialization, context);
        ShaderDefinition childDefinition = ShaderGraphDocument.ReadDefinition(child, serialization, context);
        ShaderStage callerStage = ShaderStage.None;
        if (stageID.Length != 0
            && parent.FindNode(new GraphNodeID(stageID)) is { definitionID: ShaderGraphDocument.C_OUTPUT_DEFINITION_ID } stageNode
        ) {
            callerStage = ShaderGraphDocument.Read(
                stageNode,
                ShaderGraphDocument.C_SETTINGS_KEY,
                new ShaderGraphStageSettings(),
                serialization,
                context
            ).stage;
        }
        List<ShaderPropertyDefinition> properties = parentDefinition.properties.ToList();
        foreach (ShaderPropertyDefinition childPropertyValue in childDefinition.properties)
        {
            ShaderPropertyDefinition childProperty = childPropertyValue;
            if (callerStage != ShaderStage.None)
            {
                childProperty.stages |= callerStage;
            }
            int index = properties.FindIndex(value => value.id.value == childProperty.id.value);
            if (index < 0)
            {
                properties.Add(childProperty);
                continue;
            }
            ShaderPropertyDefinition existing = properties[index];
            if (existing.type != childProperty.type || existing.bindingKind != childProperty.bindingKind
                || existing.storageAccess != childProperty.storageAccess || existing.bindingOwner != childProperty.bindingOwner
            ) {
                throw new InvalidOperationException($"Graph node property '{childProperty.id.value}' conflicts with its caller.");
            }
            existing.stages |= childProperty.stages;
            properties[index] = existing;
        }
        parentDefinition.properties = properties.ToArray();
        parent.SetMetadata(
            ShaderGraphDocument.C_DEFINITION_KEY,
            ShaderGraphDocument.Encode(serialization.Serialize(parentDefinition, context), serialization, context)
        );
    }

    private static GraphEndpoint AddOptionalZero(
        GraphDocument graph,
        GraphNodeRecord call,
        ShaderGraphNodePortDefinition port,
        string stage,
        SerializationRegistry serialization,
        SerializationContext context
    ) {
        _ = ShaderGraphLiteral.Zero(port.type.CreateType());
        var id = new GraphNodeID(call.id.value + "/default/" + port.id);
        var node = new GraphNodeRecord(id, "inno.shader.reroute") { position = call.position };
        node.SetValue("valueType", ShaderGraphDocument.Encode(port.type, serialization, context));
        if (stage.Length != 0)
        {
            node.SetValue(ShaderGraphDocument.C_STAGE_KEY, ShaderGraphDocument.Encode(stage, serialization, context));
        }
        graph.AddNode(node);
        return new(id, new GraphPortID("value"));
    }

    private static void Validate(
        ShaderGraphNodeInterface value,
        int inputRecords,
        int outputRecords
    ) {
        if (string.IsNullOrWhiteSpace(value.displayName))
        {
            throw new InvalidOperationException("A graph-authored node requires a display name.");
        }
        if (!Enum.IsDefined(value.kind))
        {
            throw new InvalidOperationException("The graph-authored node kind is invalid.");
        }
        if (!Enum.IsDefined(value.effect))
        {
            throw new InvalidOperationException("The graph-authored node effect is invalid.");
        }
        if (inputRecords is < 0 or > 1 || outputRecords is < 0 or > 1 || inputRecords + outputRecords == 0)
        {
            throw new InvalidOperationException(
                "A graph-authored node requires at most one interface record per direction and at least one direction."
            );
        }
        if (value.inputs.Length + value.outputs.Length == 0)
        {
            throw new InvalidOperationException("A graph-authored node requires at least one public input or output port.");
        }
        if (value.kind == ShaderGraphNodeKind.Function && value.effect == ShaderGraphNodeEffect.Pure && outputRecords == 0)
        {
            throw new InvalidOperationException("A pure graph-authored function requires at least one Function Output.");
        }
        if (value.kind == ShaderGraphNodeKind.DomainOutput && outputRecords != 0)
        {
            throw new InvalidOperationException("A domain output graph node cannot declare returned values.");
        }
        if (value.kind == ShaderGraphNodeKind.DomainOutput && string.IsNullOrWhiteSpace(value.role))
        {
            throw new InvalidOperationException("A domain output graph node requires a stable Target role.");
        }
        ValidatePorts(value.inputs, "input");
        ValidatePorts(value.outputs, "output");
    }

    private static void ValidatePorts(
        IEnumerable<ShaderGraphNodePortDefinition> ports,
        string direction
    ) {
        if (ports is null)
        {
            throw new InvalidOperationException($"The graph-node {direction} port list is missing.");
        }
        var ids = new HashSet<string>(StringComparer.Ordinal);
        foreach (ShaderGraphNodePortDefinition port in ports)
        {
            if (port is null || string.IsNullOrWhiteSpace(port.id) || port.type is null || port.type.CreateType().id == "void")
            {
                throw new InvalidOperationException($"A graph-node {direction} port is invalid.");
            }
            if (!ids.Add(port.id))
            {
                throw new InvalidOperationException($"A graph-node {direction} port repeats '{port.id}'.");
            }
        }
    }
}

/// <summary>
/// Chooses whether a graph-authored node is inlined or consumed by a domain Target.
/// </summary>
public enum ShaderGraphNodeKind
{
    /// <summary>
    /// Inlines the node graph into the caller before Target expansion and typed lowering.
    /// </summary>
    Function,
    /// <summary>
    /// Leaves the node as a typed domain boundary for the selected Shader Target.
    /// </summary>
    DomainOutput
}

/// <summary>
/// Declares whether a graph-authored function is pure or intentionally emits ordered effects.
/// </summary>
public enum ShaderGraphNodeEffect
{
    /// <summary>
    /// The function only computes returned values and therefore requires at least one output.
    /// </summary>
    Pure,
    /// <summary>
    /// The function may contain ordered GPU effects and can intentionally expose no returned values.
    /// </summary>
    SideEffect
}
