using System;
using System.Collections.Generic;
using Inno.Core.Graphs;
using Xunit;

namespace Inno.Core.Graphs.Tests;

public sealed class GraphDocumentTests
{
    [Fact]
    public void NodeLookupTracksAddRemoveAndReplacementWithoutChangingDocumentOrder()
    {
        var document = new GraphDocument();
        IReadOnlyList<GraphNodeRecord> view = document.nodes;
        var first = new GraphNodeRecord(new("first"), "test.node");
        var second = new GraphNodeRecord(new("second"), "test.node");
        document.AddNode(first);
        document.AddNode(second);
        Assert.Same(first, document.FindNode(first.id));
        Assert.Same(second, document.FindNode(second.id));
        Assert.Throws<ArgumentException>(() => document.AddNode(new(first.id, "duplicate")));
        Assert.Equal(2, view.Count);
        Assert.True(document.RemoveNode(first.id));
        Assert.Null(document.FindNode(first.id));
        Assert.False(document.RemoveNode(first.id));
        Assert.Same(second, Assert.Single(view));

        var replacement = new GraphDocument();
        replacement.AddNode(first);
        document.ReplaceContents(replacement);
        Assert.Same(view, document.nodes);
        Assert.Null(document.FindNode(second.id));
        GraphNodeRecord copied = Assert.Single(view);
        Assert.NotSame(first, copied);
        Assert.Same(copied, document.FindNode(first.id));
        Assert.Throws<ArgumentException>(() => document.ReplaceContents(document));
        Assert.Same(copied, document.FindNode(first.id));
    }

    [Fact]
    public void RepeatedLookupAndCollectionAccessDoNotAllocate()
    {
        var document = new GraphDocument();
        for (int index = 0; index < 2_000; index++)
            document.AddNode(new(new("node-" + index), "test.node"));
        GraphNodeRecord node = document.nodes[^1];
        GraphNodeId id = node.id;
        for (int index = 0; index < 1_000; index++)
            _ = document.FindNode(id);

        long before = GC.GetAllocatedBytesForCurrentThread();
        for (int index = 0; index < 10_000; index++)
        {
            _ = document.FindNode(id);
            _ = document.nodes;
            _ = document.edges;
            _ = document.metadata;
            _ = node.values;
        }
        long allocated = GC.GetAllocatedBytesForCurrentThread() - before;

        Assert.Equal(0, allocated);
        Assert.Same(node, document.FindNode(id));
    }
}
