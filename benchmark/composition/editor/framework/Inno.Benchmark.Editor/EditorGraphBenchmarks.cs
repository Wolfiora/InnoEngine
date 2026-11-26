using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using Inno.Core.Graphs;
using Inno.Editor.Graph;

namespace Inno.Benchmark.Editor;

/// <summary>
/// Measures editor graph document editing through the undoable controller.
/// </summary>
/// <remarks>
/// BenchmarkDotNet derives a generated runner from this class, so it is not sealed.
/// </remarks>
[MemoryDiagnoser]
public class EditorGraphBenchmarks
{
    private EditorHarness m_harness = null!;

    [Params(64, 512)]
    public int nodeCount;

    [GlobalSetup]
    public void Setup() => m_harness = new EditorHarness();

    [GlobalCleanup]
    public void Cleanup() => m_harness.Dispose();

    [Benchmark(Description = "Graph: open, add N, connect chain, move all, copy+paste, close")]
    public int EditDocument()
    {
        Guid id = Guid.NewGuid();
        GraphDocumentController controller = m_harness.graphs.OpenDocument(id, new GraphDocument(), m_harness.runtime.interactions.history);
        var nodes = new GraphNodeID[nodeCount];
        var positions = new Dictionary<GraphNodeID, GraphPosition>(nodeCount);
        for (int index = 0; index < nodeCount; index++)
        {
            nodes[index] = controller.AddNode("bench.node", new GraphPosition(index * 10, 0));
            if (index > 0)
            {
                controller.Connect(
                    new GraphEndpoint(nodes[index - 1], new GraphPortID("out")),
                    new GraphEndpoint(nodes[index], new GraphPortID("in"))
                );
            }
            positions[nodes[index]] = new GraphPosition(index * 10, 50);
        }
        controller.MoveNodes(positions);
        GraphClipboardData clipboard = controller.Copy(nodes);
        int pasted = controller.Paste(clipboard, new GraphPosition(0, 200)).Count;
        int total = controller.document.nodes.Count + pasted;
        m_harness.graphs.CloseDocument(id);
        return total;
    }
}
