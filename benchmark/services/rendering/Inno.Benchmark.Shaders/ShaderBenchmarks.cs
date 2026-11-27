using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Inno.Benchmark.Rendering.Fixtures;
using Inno.Benchmark.Services;
using Inno.Build.Toolchains.Bgfx.Tools;
using Inno.Core.Graphs;
using Inno.Core.Serialization;
using Inno.Rendering;
using Inno.Rendering.Assets.Authoring;
using Inno.Rendering.Shaders;

namespace Inno.Benchmark.Shaders;

/// <summary>
/// Measures shader graph construction, source analysis, graph-to-IR lowering, IR hashing and the stage compiler cache.
/// </summary>
/// <remarks>
/// Native shaderc execution is excluded: the backend toolchain is replaced through the public
/// <see cref="IShaderCompilerToolchain"/> boundary so that only engine-owned cost and cache behavior are measured.
/// BenchmarkDotNet derives a generated runner from this class, so it is not sealed.
/// </remarks>
[MemoryDiagnoser]
public class ShaderBenchmarks
{
    private BenchmarkServices m_services = null!;
    private ShaderNodeCompilerCatalog m_catalog = null!;
    private ShaderSourceModuleAnalysis m_module = null!;
    private GraphDocument m_graph = null!;
    private ShaderGraphLoweringRequest m_request = null!;
    private ShaderIrStage m_stage = null!;
    private CountingShaderToolchain m_toolchain = null!;
    private ShaderCompiler m_warmCompiler = null!;
    private ShaderCompileTarget m_target = null!;
    private byte[] m_encodedGraph = [];

    [Params(8, 64)]
    public int chainLength;

    [GlobalSetup]
    public void Setup()
    {
        m_services = new BenchmarkServices(typeof(ShaderBenchmarks).Assembly);
        m_catalog = CreateCatalog();
        m_module = AnalyzeSource();
        m_graph = BuildGraph(m_services.serialization, chainLength, out GraphNodeRecord source, out GraphNodeRecord color);
        m_request = Request(m_graph, source, color, m_module);
        m_stage = Lower();
        m_toolchain = new CountingShaderToolchain();
        m_warmCompiler = new ShaderCompiler(m_toolchain);
        m_target = m_warmCompiler.CreateTarget(BenchmarkGraphicsCapabilities.Create());
        _ = m_warmCompiler.CompileAsync(m_stage, m_target).AsTask().GetAwaiter().GetResult();
        m_encodedGraph = GraphDocumentCodec.Encode(m_graph, m_services.serialization);
    }

    [GlobalCleanup]
    public void Cleanup() => m_services.Dispose();

    [Benchmark(Description = "Graph: build document")]
    public int BuildGraph() => BuildGraph(m_services.serialization, chainLength, out _, out _).nodes.Count;

    [Benchmark(Description = "Graph: encode+decode document")]
    public int GraphRoundTrip()
        => GraphDocumentCodec.Decode(GraphDocumentCodec.Encode(m_graph, m_services.serialization), m_services.serialization).nodes.Count;

    [Benchmark(Description = "Lowering: graph -> IR + stage hash")]
    public string LowerAndHash() => Lower().contentHash;

    [Benchmark(Description = "Compiler cache: miss (new compiler)")]
    public int CompileMiss()
        => new ShaderCompiler(m_toolchain).CompileAsync(m_stage, m_target).AsTask().GetAwaiter().GetResult().bytes.Length;

    [Benchmark(Description = "Compiler cache: hit (warm compiler)")]
    public int CompileHit() => m_warmCompiler.CompileAsync(m_stage, m_target).AsTask().GetAwaiter().GetResult().bytes.Length;

    [Benchmark(Description = "Decode stored graph -> lower -> cache hit")]
    public int ReopenAndCompile()
    {
        GraphDocument graph = GraphDocumentCodec.Decode(m_encodedGraph, m_services.serialization);
        ShaderGraphLoweringRequest request = Request(
            graph,
            System.Linq.Enumerable.First(graph.nodes, static node => node.id == new GraphNodeID("source")),
            System.Linq.Enumerable.First(graph.nodes, static node => node.id == new GraphNodeID("color")),
            m_module
        );
        ShaderIrStage stage = Stage(m_catalog.Lower(request, m_services.serialization, SerializationContext.empty));
        return m_warmCompiler.CompileAsync(stage, m_target).AsTask().GetAwaiter().GetResult().bytes.Length;
    }

    internal CountingShaderToolchain toolchain => m_toolchain;
    internal ShaderIrStage stage => m_stage;
    internal ShaderCompileTarget target => m_target;

    internal ShaderIrStage Lower() => Stage(m_catalog.Lower(m_request, m_services.serialization, SerializationContext.empty));

    internal static GraphDocument BuildGraph(
        SerializationRegistry serialization,
        int chainLength,
        out GraphNodeRecord source,
        out GraphNodeRecord color
    ) {
        var graph = new GraphDocument();
        GraphNodeRecord constant = Add(graph, "constant", "inno.shader.constant");
        constant.SetValue(
            "value",
            new GraphSerializedValue(serialization.Encode(writer => writer.Write("value", 0.25f), SerializationContext.empty))
        );
        GraphNodeRecord previous = constant;
        for (int index = 0; index < chainLength; index++)
        {
            GraphNodeRecord doubled = Add(graph, "double." + index, DoubleNodeCompiler.C_DEFINITION_ID);
            Connect(graph, previous, "value", doubled, "input");
            previous = doubled;
        }
        source = Add(graph, "source", "inno.shader.source");
        color = Add(graph, "color", "inno.shader.construct");
        Connect(graph, previous, "value", source, "input.value");
        for (int index = 0; index < 4; index++)
        {
            Connect(graph, source, "return", color, "component." + index);
        }
        return graph;
    }

    internal static ShaderSourceModuleAnalysis AnalyzeSource()
        => new ShaderSourceFrontendCatalog([new BgfxShaderSourceFrontend()]).AnalyzeModule(
            [new ShaderSourceImplementationRequest(
                "metal",
                "inno.shader-language.bgfx-sc",
                new ShaderSourceRequest(
                    new ShaderSourceFile("module.ishadersource", "float Shade(float value) { return value * 0.75; }"),
                    "Shade",
                    new NoIncludes()
                )
            )]
        );

    private static ShaderIrStage Stage(ShaderGraphLoweringResult result)
    {
        if (!result.succeeded)
        {
            throw new InvalidDataException(
                string.Join("\n", System.Linq.Enumerable.Select(result.diagnostics, static value => value.message))
            );
        }
        return new ShaderIrStage(ShaderStage.Fragment, result.block!, [], [new ShaderIrStageOutput("color", ShaderIrOutputKind.Color)]);
    }

    private static ShaderGraphLoweringRequest Request(
        GraphDocument graph,
        GraphNodeRecord source,
        GraphNodeRecord color,
        ShaderSourceModuleAnalysis module
    ) => new(
        graph,
        new Dictionary<string, GraphEndpoint> { ["color"] = new(color.id, new GraphPortID("value")) },
        "metal",
        new Dictionary<GraphNodeID, ShaderSourceModuleAnalysis> { [source.id] = module }
    );

    private static ShaderNodeCompilerCatalog CreateCatalog()
        => new(
            new IShaderNodeCompiler[]
            {
                new ShaderConstantNodeCompiler(),
                new ShaderBinaryNodeCompiler(),
                new ShaderConstructNodeCompiler(),
                new ShaderStageInputNodeCompiler(),
                new ShaderSelectNodeCompiler(),
                new ShaderSourceNodeCompiler(),
                new DoubleNodeCompiler()
            }
        );

    private static GraphNodeRecord Add(
        GraphDocument graph,
        string id,
        string definition
    ) {
        var node = new GraphNodeRecord(new GraphNodeID(id), definition);
        graph.AddNode(node);
        return node;
    }

    private static void Connect(
        GraphDocument graph,
        GraphNodeRecord source,
        string output,
        GraphNodeRecord target,
        string input
    ) => graph.AddEdge(
        new GraphEdgeRecord(
            new GraphEdgeID("edge." + graph.edges.Count),
            new GraphEndpoint(source.id, new GraphPortID(output)),
            new GraphEndpoint(target.id, new GraphPortID(input))
        )
    );

    private sealed class NoIncludes : IShaderSourceResolver
    {
        public ShaderSourceFile ReadInclude(
            string includingFile,
            string include
        ) => throw new FileNotFoundException(include);
    }

    private sealed class DoubleNodeCompiler : IShaderNodeCompiler
    {
        internal const string C_DEFINITION_ID = "bench.double";

        public string definitionID => C_DEFINITION_ID;

        public IReadOnlyList<ShaderNodePort> GetPorts(ShaderNodeDescriptionContext context)
            => [
                new ShaderNodePort("input", ShaderSourceType.Atomic("float"), GraphPortDirection.Input),
                new ShaderNodePort("value", ShaderSourceType.Atomic("float"), GraphPortDirection.Output)
            ];

        public IReadOnlyDictionary<string, ShaderIrValue> Lower(ShaderNodeLoweringContext context)
            => new Dictionary<string, ShaderIrValue>
        {
            ["value"] = context.builder.Binary(ShaderIrOperation.Multiply, context.Input("input"), context.builder.Constant(2f))
        };
    }
}
