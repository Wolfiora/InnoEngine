using Inno.Core.Graphs;
using Inno.Core.Serialization;
using Inno.Rendering.Assets;

namespace Inno.Rendering.Shaders;

[ShaderGraphTemplate(ShaderBuiltInIDs.C_NODE_TEMPLATE, "Reusable Node")]
internal sealed class ReusableShaderGraphNodeTemplate : ShaderGraphTemplate
{
    /// <summary>
    /// Creates a value using this implementation's validated inputs.
    /// </summary>
    /// <param name="serialization">
    /// The serialization consumed by create; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    /// <returns>
    /// The validated graph document that represents the completed operation.
    /// </returns>
    public override GraphDocument Create(
        SerializationRegistry serialization,
        SerializationContext context
    ) {
        GraphDocument graph = ShaderGraphDocument.Create(new ShaderDefinition("Graph Node", [], [], []), serialization, context);
        ShaderGraphNodes.WriteSettings(graph, new ShaderGraphNodeSettings(), serialization, context);
        var input = new GraphNodeRecord(new GraphNodeID("node-inputs"), ShaderGraphNodes.C_INPUT_DEFINITION_ID)
        {
            position = new(40, 120)
        };
        input.SetValue(
            ShaderGraphDocument.C_SETTINGS_KEY,
            ShaderGraphDocument.Encode(new ShaderGraphNodeInputSettings
            {
                ports = [new ShaderGraphNodePortDefinition() { id = "value", type = new() { id = "float" } }]
            }, serialization, context)
        );
        var output = new GraphNodeRecord(new GraphNodeID("node-outputs"), ShaderGraphNodes.C_OUTPUT_DEFINITION_ID)
        {
            position = new(620, 120)
        };
        output.SetValue(
            ShaderGraphDocument.C_SETTINGS_KEY,
            ShaderGraphDocument.Encode(new ShaderGraphNodeOutputSettings
            {
                ports = [new ShaderGraphNodePortDefinition() { id = "value", type = new() { id = "float" } }]
            }, serialization, context)
        );
        graph.AddNode(input);
        graph.AddNode(output);
        graph.AddEdge(
            new GraphEdgeRecord(
                new GraphEdgeID("node-value"),
                new GraphEndpoint(input.id, new GraphPortID("value")),
                new GraphEndpoint(output.id, new GraphPortID("value"))
            )
        );
        return graph;
    }
}
