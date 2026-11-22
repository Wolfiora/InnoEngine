using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Adapter.Modules.DotNet;
using Inno.Adapter.Serialization.DotNet;
using Inno.Core.Diagnostics;
using Inno.Core.Graphs;
using Inno.Core.Serialization;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Types;
using Inno.Rendering;
using Inno.Rendering.Assets;
using Inno.Rendering.Assets.Authoring;
using Inno.Rendering.Shaders;

namespace Inno.Rendering.Assets.Authoring.Tests;

[Collection("Rendering assets serialization")]
public sealed class ShaderCompilerTests : IDisposable
{
    private readonly ModuleHost m_modules = new(
        new ModuleHostOptions() { catalogSource = new DotNetAssemblyCatalogSource(typeof(ShaderCompilerTests).Assembly)}
    );

    private readonly TypeCatalog m_types;
    private readonly SerializationRegistry m_serialization;
    private readonly ShaderNodeCompilerRegistry m_nodes;

    public ShaderCompilerTests()
    {
        m_types = new(m_modules, new ReflectionTypeCatalogSource());
        m_serialization = new(m_types, new ReflectionSerializationMetadataSource());
        m_nodes = new(m_types);
    }

    [Fact]
    public void FailedToolResultsCannotCarrySuccessfulBinaryBytes()
        => Assert.Throws<ArgumentException>(() => new ShaderStageToolResult(
            [1],
            [],
            [new ShaderSourceDiagnostic(
                "TEST_ERROR",
                DiagnosticSeverity.Error,
                "The adapter rejected its binary.",
                new ShaderSourcePosition("test", 1, 1)
            )]
        ));

    [Fact]
    public void ToolResultsCopyAdapterOwnedBytesBindingsAndDiagnostics()
    {
        byte[] bytes = [1, 2, 3];
        ShaderSourceDiagnostic[] diagnostics = [new ShaderSourceDiagnostic(
            "TEST",
            DiagnosticSeverity.Warning,
            "A warning",
            new ShaderSourcePosition("test", 1, 1)
        )];
        ShaderStageBinding[] bindings = [new ShaderStageBinding("color", "native_color", 0)];
        var result = new ShaderStageToolResult(bytes, bindings, diagnostics);
        bytes[0] = 9;
        diagnostics[0] = new("CHANGED", DiagnosticSeverity.Error, "Mutated input", new ShaderSourcePosition("test", 1, 1));
        bindings[0] = new("other", "other", 1);
        Assert.Equal(1, result.bytes.Span[0]);
        Assert.Equal("TEST", Assert.Single(result.diagnostics).code);
        Assert.Equal("color", Assert.Single(result.bindings).id);
    }

    [Fact]
    public async Task CompileAsyncUsesGraphLoweringAndTypedStages()
    {
        var toolchain = new FakeToolchain();
        ShaderCompilationResult result = await CompileAsync(new ShaderCompiler(toolchain), Template());
        Assert.True(result.succeeded, Diagnostics(result));
        Assert.Equal(2, toolchain.requests.Count);
        Assert.Equal("color", Assert.Single(result.artifact!.shaderInterface.bindings).id.value);
    }

    [Fact]
    public async Task AdapterFailureRetainsOriginalSourceLocationAndNoPartialProgram()
    {
        ShaderCompilationResult result = await CompileAsync(new ShaderCompiler(new FakeToolchain { reportError = true }), Template());
        Assert.False(result.succeeded);
        Assert.Null(result.artifact);
        ShaderDiagnostic error = Assert.Single(result.diagnostics.Where(static value => value.severity == DiagnosticSeverity.Error));
        Assert.Equal("Shaders/function.ishadersource", error.location!.Value.assetPath);
        Assert.Equal(2, error.location.Value.line);
        Assert.Equal(3, error.location.Value.column);
    }

    [Fact]
    public async Task UnsupportedAlternativePassKeepsSupportedPassAndExactBindingLayout()
    {
        GraphDocument graph = Template();
        ShaderDefinition definition = Definition(graph);
        GraphDocument alternate = graph.Clone();
        foreach (GraphNodeRecord original in alternate.nodes)
        {
            var node = new GraphNodeRecord(new GraphNodeID("alternate." + original.id.value), original.definitionID);
            foreach ((string key, GraphSerializedValue value) in original.values)
            {
                node.SetValue(key, value.Clone());
            }
            if (node.definitionID == ShaderGraphDocument.C_OUTPUT_DEFINITION_ID)
            {
                ShaderGraphStageSettings settings = ShaderGraphDocument.Read<ShaderGraphStageSettings>(
                    node,
                    "settings",
                    null!,
                    m_serialization,
                    SerializationContext.empty
                );
                node.SetValue("settings", ShaderGraphDocument.Encode(settings, m_serialization, SerializationContext.empty));
            }
            else
            {
                node.SetValue(
                    "stage",
                    ShaderGraphDocument.Encode(
                        "alternate." + ShaderGraphDocument.Read(node, "stage", string.Empty, m_serialization, SerializationContext.empty),
                        m_serialization,
                        SerializationContext.empty
                    )
                );
            }
            graph.AddNode(node);
        }
        foreach (GraphEdgeRecord edge in alternate.edges)
        {
            graph.AddEdge(
                new GraphEdgeRecord(
                    new GraphEdgeID("alternate." + edge.id.value),
                    new GraphEndpoint(new GraphNodeID("alternate." + edge.output.nodeID.value), edge.output.portID),
                    new GraphEndpoint(new GraphNodeID("alternate." + edge.input.nodeID.value), edge.input.portID)
                )
            );
        }
        definition.passes =
        [
            new ShaderPassDefinition(
                "Clustered",
                ShaderProgramKind.Raster,
                requiredFeatures: GraphicsCapability.Compute | GraphicsCapability.StorageBuffer
            ),
            definition.passes[0]
        ];
        graph.SetMetadata(
            ShaderGraphDocument.C_DEFINITION_KEY,
            ShaderGraphDocument.Encode(m_serialization.Serialize(definition), m_serialization, SerializationContext.empty)
        );
        graph = ShaderGraphPrograms.Bind(
            graph,
            "Clustered",
            [new GraphNodeID("alternate.vertex"), new GraphNodeID("alternate.fragment")],
            m_serialization,
            SerializationContext.empty
        );
        var toolchain = new FakeToolchain();
        ShaderCompilationResult result = await CompileAsync(new ShaderCompiler(toolchain), graph);
        Assert.True(result.succeeded, Diagnostics(result));
        Assert.Equal("Main", Assert.Single(result.artifact!.passes).definition.name);
        Assert.Equal(2, toolchain.requests.Count);
        Assert.Contains(
            result.diagnostics,
            static value => value.code == "SHADER_CAPABILITY_UNAVAILABLE" && value.severity == DiagnosticSeverity.Warning
        );
    }

    [Fact]
    public async Task LastGoodStorePreservesArtifactAfterFailedCandidate()
    {
        Guid shaderID = Guid.NewGuid();
        var store = new ShaderLastGoodStore();
        ShaderCompilationResult success = await CompileAsync(new ShaderCompiler(new FakeToolchain()), Template());
        ShaderCompilationResult failure = await CompileAsync(new ShaderCompiler(new FakeToolchain { reportError = true }), Template());
        ShaderArtifactSelection first = store.Select(shaderID, success.artifact!.targetKey, RenderShaderVariant.empty, success);
        ShaderArtifactSelection second = store.Select(shaderID, success.artifact.targetKey, RenderShaderVariant.empty, failure);
        Assert.True(first.candidateSucceeded);
        Assert.False(second.candidateSucceeded);
        Assert.True(second.usingLastGood);
        Assert.Same(success.artifact, second.artifact);
    }

    public void Dispose()
    {
        m_nodes.Dispose();
        m_serialization.Dispose();
        m_types.Dispose();
        m_modules.Dispose();
    }

    private GraphDocument Template() => ShaderGraphTemplates.CreateRaster(m_serialization, SerializationContext.empty);

    private ShaderDefinition Definition(GraphDocument graph)
        => ShaderGraphDocument.ReadDefinition(graph, m_serialization, SerializationContext.empty);

    private ValueTask<ShaderCompilationResult> CompileAsync(
        ShaderCompiler compiler,
        GraphDocument graph
    ) {
        ShaderGraphProgramResult program = new ShaderGraphProgramCompiler(m_nodes).Lower(
            graph,
            "tests.fake",
            new Dictionary<GraphNodeID, ShaderSourceModuleAnalysis>(),
            m_serialization,
            SerializationContext.empty
        );
        RenderTextureFormat[] formats = Enum.GetValues<RenderTextureFormat>();
        var capabilities = new GraphicsCapabilities(
            GraphicsApi.metal,
            GraphicsCapability.None,
            new GraphicsLimits(256, 8, 8192, 16),
            formats,
            formats,
            formats,
            formats,
            false,
            false
        );
        return compiler.CompileAsync(
            Definition(graph),
            program,
            compiler.CreateTarget(capabilities),
            RenderShaderVariant.empty,
            m_serialization,
            SerializationContext.empty
        );
    }

    private static string Diagnostics(ShaderCompilationResult result)
        => string.Join("\n", result.diagnostics.Select(static value => value.message));

    private sealed class FakeToolchain : IShaderCompilerToolchain
    {
        public string implementationID => "tests.fake";

        public IReadOnlyList<string> supportedSourceLanguages => [];

        public ShaderCompileTarget CreateTarget(
            GraphicsCapabilities capabilities,
            bool optimize = true,
            bool debugInformation = false
        ) => new("tests:fake", capabilities, optimize, debugInformation);

        public ValueTask<ShaderStageToolResult> CompileAsync(
            ShaderStageToolRequest request,
            CancellationToken cancellationToken
        ) {
            cancellationToken.ThrowIfCancellationRequested();
            requests.Add(request);
            return ValueTask.FromResult(
                reportError
                ? new ShaderStageToolResult(
                    [],
                    [],
                    [new ShaderSourceDiagnostic(
                        "TEST_ERROR",
                        DiagnosticSeverity.Error,
                        "Opaque compiler failure",
                        new ShaderSourcePosition("Shaders/function.ishadersource", 2, 3)
                    )]
                )
                : new ShaderStageToolResult(
                    [1, 2, 3],
                    request.stage.inputs
                        .Where(static input => input.kind == ShaderIrInputKind.Uniform)
                        .Select(static input => new ShaderStageBinding(input.id, "native_" + input.id, 0)),
                    []
                )
            );
        }

        internal bool reportError { get; init; }

        internal List<ShaderStageToolRequest> requests { get; } = [];
    }
}
