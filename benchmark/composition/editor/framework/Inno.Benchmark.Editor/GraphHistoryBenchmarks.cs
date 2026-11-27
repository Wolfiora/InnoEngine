using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using Inno.Core.Graphs;
using Inno.Editor.Graph;
using Inno.Editor.Interactions;

namespace Inno.Benchmark.Editor;

/// <remarks>
/// BenchmarkDotNet derives its generated measurement runner from this class.
/// </remarks>
[MemoryDiagnoser]
public class GraphHistoryBenchmarks
{
    private readonly Dictionary<GraphNodeID, GraphPosition> m_move = new() { [new GraphNodeID("0")] = new GraphPosition(10, 10) };
    private EditorHarness m_editor = null!;
    private GraphDocumentController m_graph = null!;

    [Params(64, 512)]
    public int nodeCount;

    [IterationSetup(Target = nameof(Move))]
    public void Prepare()
    {
        m_editor = new EditorHarness();
        var document = new GraphDocument();
        for (int i = 0; i < nodeCount; i++)
        {
            document.AddNode(new GraphNodeRecord(new GraphNodeID(i.ToString()), "bench.node"));
        }
        m_graph = m_editor.graphs.OpenDocument(Guid.NewGuid(), document, m_editor.runtime.interactions.history);
    }

    [IterationSetup(Target = nameof(UndoRedo))]
    public void PrepareHistory()
    {
        Prepare();
        Move();
    }

    [Benchmark]
    public void Move() => m_graph.MoveNodes(m_move);

    [Benchmark]
    public void UndoRedo()
    {
        IEditorHistory history = m_editor.runtime.interactions.history;
        if (!history.Undo().succeeded || !history.Redo().succeeded)
        {
            throw new InvalidOperationException("Move history could not be replayed.");
        }
    }

    [IterationCleanup]
    public void Cleanup() => m_editor.Dispose();
}
