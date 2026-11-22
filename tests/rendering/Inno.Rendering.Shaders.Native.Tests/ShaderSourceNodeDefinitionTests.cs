using System.Linq;
using Xunit;
using Inno.Core.Graphs;

namespace Inno.Rendering.Shaders.Native.Tests;

public sealed class ShaderSourceNodeDefinitionTests
{
    private static readonly GraphNodeRecord NODE = new(new GraphNodeID("test-node"), "inno.shader.source");

    [Fact]
    public void FunctionPortsUseSemanticNamesAndInOutCreatesBothDirections()
    {
        var definition = new ShaderSourceNodeDefinition(
            new ShaderSourceFunction(
                "Shade",
                ShaderSourceType.Atomic("float4"),
                [
                    new ShaderSourceParameter("uv", ShaderSourceType.Atomic("float2"), ShaderSourceParameterDirection.Input),
                    new ShaderSourceParameter("color", ShaderSourceType.Atomic("float4"), ShaderSourceParameterDirection.InputOutput)
                ],
                new ShaderSourcePosition("source", 1, 1)
            )
        );
        Assert.Equal(
            ["input.uv", "input.color", "output.color", "return"],
            definition.GetPorts(NODE).Select(static port => port.id.value)
        );
        Assert.Equal(GraphPortCapacity.Multiple, definition.GetPorts(NODE)[2].capacity);
        Assert.Same(definition.GetPorts(NODE), definition.GetPorts(NODE));
    }

    [Fact]
    public void AggregatesExposeMembersWithoutDiscardingTheirWholeValuePorts()
    {
        ShaderSourceType aggregate = ShaderSourceType.Structure(
            "Surface",
            [new ShaderSourceField("normal", ShaderSourceType.Atomic("float3"))]
        );
        var definition = new ShaderSourceNodeDefinition(
            new ShaderSourceFunction(
                "Shade",
                aggregate,
                [new ShaderSourceParameter("surface", aggregate, ShaderSourceParameterDirection.Input)],
                new ShaderSourcePosition("source", 1, 1)
            )
        );
        Assert.Equal(
            ["input.surface", "input.surface.normal", "return", "return.normal"],
            definition.GetPorts(NODE).Select(static port => port.id.value)
        );
        Assert.Equal(definition.GetPorts(NODE)[0].valueTypeID, definition.GetPorts(NODE)[2].valueTypeID);
    }

    [Fact]
    public void ArrayLengthsAndStructureLayoutsCannotAliasGraphValueTypes()
    {
        string FirstPortType(ShaderSourceType type)
            => new ShaderSourceNodeDefinition(new ShaderSourceFunction("Shade", type, [], new ShaderSourcePosition("source", 1, 1)))
                .GetPorts(NODE)[0].valueTypeID;
        Assert.NotEqual(
            FirstPortType(ShaderSourceType.ArrayOf(ShaderSourceType.Atomic("float"), 2)),
            FirstPortType(ShaderSourceType.ArrayOf(ShaderSourceType.Atomic("float"), 3))
        );
        Assert.NotEqual(
            FirstPortType(ShaderSourceType.Structure("Surface", [new ShaderSourceField("a", ShaderSourceType.Atomic("float3"))])),
            FirstPortType(ShaderSourceType.Structure("Surface", [new ShaderSourceField("a", ShaderSourceType.Atomic("float4"))]))
        );
    }
}
