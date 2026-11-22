using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Inno.Adapter.Modules.DotNet;
using Inno.Adapter.Serialization.DotNet;
using Inno.Core.Graphs;
using Inno.Core.Serialization;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Types;

namespace Inno.Core.Graphs.Tests;

public sealed class GraphValidatorTests : IDisposable
{
    private readonly string m_testRoot = Path.Combine(
        Path.GetTempPath(),
        "InnoCoreGraphsTests",
        Guid.NewGuid().ToString("N")
    );

    private readonly ModuleHost m_modules;
    private readonly TypeCatalog m_types;
    private readonly SerializationRegistry m_serialization;

    public GraphValidatorTests()
    {
        m_modules = new ModuleHost(new ModuleHostOptions
        {
            catalogSource = new DotNetAssemblyCatalogSource(typeof(GraphValidatorTests).Assembly)
        });
        m_types = new TypeCatalog(m_modules, new ReflectionTypeCatalogSource());
        m_serialization = new SerializationRegistry(m_types, new ReflectionSerializationMetadataSource());
    }

    public void Dispose()
    {
        m_serialization.Dispose();
        m_types.Dispose();
        m_modules.Dispose();
        if (Directory.Exists(m_testRoot))
        {
            Directory.Delete(m_testRoot, recursive: true);
        }
    }

    [Fact]
    public void Validate_WithCompatibleConnection_IsValid()
    {
        GraphDocument document = CreateConnectedDocument("float", "float");

        GraphValidationResult result = GraphValidator.Validate(document, new Resolver(m_serialization));

        Assert.True(result.isValid);
        Assert.Empty(result.diagnostics);
    }

    [Fact]
    public void Validate_WithDirectedConversion_IsValid()
    {
        GraphDocument document = CreateConnectedDocument("float", "vector3");

        GraphValidationResult result = GraphValidator.Validate(
            document,
            new Resolver(m_serialization),
            new Conversion()
        );

        Assert.True(result.isValid);
    }

    [Fact]
    public void Validate_WithIncompatibleTypes_ReportsError()
    {
        GraphDocument document = CreateConnectedDocument("texture2d", "float");

        GraphValidationResult result = GraphValidator.Validate(document, new Resolver(m_serialization));

        Assert.False(result.isValid);
        Assert.Contains(result.diagnostics, diagnostic => diagnostic.code == "GRAPH_INCOMPATIBLE_TYPES");
    }

    [Fact]
    public void Validate_WithMissingNodeDefinition_PreservesDocumentAsWarning()
    {
        var document = new GraphDocument();
        var node = new GraphNodeRecord(new GraphNodeID("missing"), "plugin.removed");
        document.AddNode(node);

        GraphValidationResult result = GraphValidator.Validate(document, new Resolver(m_serialization));

        Assert.True(result.isValid);
        Assert.Single(document.nodes);
        Assert.Contains(result.diagnostics, diagnostic => diagnostic.code == "GRAPH_MISSING_NODE");
    }

    [Fact]
    public void Validate_WithCycle_ReportsError()
    {
        var document = new GraphDocument();
        GraphNodeRecord left = CreateNode("left", "float", "float");
        GraphNodeRecord right = CreateNode("right", "float", "float");
        document.AddNode(left);
        document.AddNode(right);
        document.AddEdge(CreateEdge("left-right", left.id, right.id));
        document.AddEdge(CreateEdge("right-left", right.id, left.id));

        GraphValidationResult result = GraphValidator.Validate(document, new Resolver(m_serialization));

        Assert.False(result.isValid);
        Assert.Contains(result.diagnostics, diagnostic => diagnostic.code == "GRAPH_CYCLE");
    }

    [Fact]
    public void RemoveNode_RemovesConnectedEdges()
    {
        GraphDocument document = CreateConnectedDocument("float", "float");

        Assert.True(document.RemoveNode(new GraphNodeID("source")));

        Assert.Single(document.nodes);
        Assert.Empty(document.edges);
    }

    private GraphDocument CreateConnectedDocument(
        string outputType,
        string inputType
    ) {
        var document = new GraphDocument();
        GraphNodeRecord source = CreateNode("source", outputType, outputType);
        GraphNodeRecord destination = CreateNode("destination", inputType, inputType);
        document.AddNode(source);
        document.AddNode(destination);
        document.AddEdge(CreateEdge("edge", source.id, destination.id));
        return document;
    }

    private GraphNodeRecord CreateNode(
        string id,
        string inputType,
        string outputType
    ) {
        var node = new GraphNodeRecord(new GraphNodeID(id), $"node.{id}");
        node.SetValue("inputType", GraphSerializedValue.From(inputType, m_serialization));
        node.SetValue("outputType", GraphSerializedValue.From(outputType, m_serialization));
        return node;
    }

    private static GraphEdgeRecord CreateEdge(
        string id,
        GraphNodeID source,
        GraphNodeID destination
    ) => new(
        new GraphEdgeID(id),
        new GraphEndpoint(source, new GraphPortID("out")),
        new GraphEndpoint(destination, new GraphPortID("in"))
    );

    private sealed class Resolver(SerializationRegistry serialization) : IGraphNodeDefinitionResolver
    {
        public bool TryResolve(
            string definitionID,
            out GraphNodeDefinition? definition
        ) {
            if (!definitionID.StartsWith("node.", StringComparison.Ordinal))
            {
                definition = null;
                return false;
            }

            definition = new Definition(definitionID, serialization);
            return true;
        }
    }

    private sealed class Definition(
        string id,
        SerializationRegistry serialization
    ) : GraphNodeDefinition(id, id, "Tests")
    {
        public override IReadOnlyList<GraphPortDefinition> GetPorts(GraphNodeRecord node)
        {
            string inputType = node.TryGetValue("inputType", out GraphSerializedValue? input)
                               ? input!.Deserialize<string>(serialization)!
                               : "float";
            string outputType = node.TryGetValue("outputType", out GraphSerializedValue? output)
                                ? output!.Deserialize<string>(serialization)!
                                : "float";

            return
            [
                new GraphPortDefinition(new GraphPortID("in"), "In", inputType, GraphPortDirection.Input),
                new GraphPortDefinition(new GraphPortID("out"), "Out", outputType, GraphPortDirection.Output)
            ];
        }
    }

    private sealed class Conversion : IGraphTypeConversion
    {
        public bool CanConvert(
            string sourceTypeID,
            string destinationTypeID
        ) => sourceTypeID == "float" && destinationTypeID == "vector3";
    }
}
