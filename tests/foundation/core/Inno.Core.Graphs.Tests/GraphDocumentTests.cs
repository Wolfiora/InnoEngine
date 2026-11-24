using System;
using System.Collections.Generic;
using Xunit;
using Inno.Core.Graphs;

namespace Inno.Core.Graphs.Tests;

public sealed class GraphDocumentTests
{
    [Fact]
    public void NodeLookupTracksAddRemoveAndReplacementWithoutChangingDocumentOrder()
    {
        var document = new GraphDocument();
        IReadOnlyList<GraphNodeRecord> view = document.nodes;
        var first = new GraphNodeRecord(new GraphNodeID("first"), "test.node");
        var second = new GraphNodeRecord(new GraphNodeID("second"), "test.node");
        document.AddNode(first);
        document.AddNode(second);
        Assert.Same(first, document.FindNode(first.id));
        Assert.Same(second, document.FindNode(second.id));
        Assert.Throws<ArgumentException>(() => document.AddNode(new GraphNodeRecord(first.id, "duplicate")));
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
}
