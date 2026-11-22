using System.Collections.Generic;
using System.Linq;
using Xunit;
using Inno.Editor.Core;

namespace Inno.Editor.Core.Tests;

public sealed partial class EditorRuntimeTests
{
    [Fact]
    public void UnchangedStatisticsReuseAnImmutableSnapshot()
    {
        var statistics = new EditorStatistics();
        statistics.Publish(
            new EditorStatistic(new EditorStatisticID("frame"), new EditorStatisticGroupID("timing"), "Timing", "Frame", "16 ms")
        );
        IReadOnlyList<EditorStatistic> first = statistics.GetSnapshot();
        Assert.Same(first, statistics.GetSnapshot());
        statistics.Publish(
            new EditorStatistic(new EditorStatisticID("frame"), new EditorStatisticGroupID("timing"), "Timing", "Frame", "16 ms")
        );
        Assert.Same(first, statistics.GetSnapshot());
        statistics.Publish(
            new EditorStatistic(new EditorStatisticID("frame"), new EditorStatisticGroupID("timing"), "Timing", "Frame", "8 ms")
        );
        IReadOnlyList<EditorStatistic> changed = statistics.GetSnapshot();
        Assert.NotSame(first, changed);
        Assert.Equal("16 ms", Assert.Single(first).value);
        Assert.Equal("8 ms", Assert.Single(changed).value);
    }

    [Fact]
    public void StatisticFrameHandoffRetainsLateProvidersAndRemovesExpiredContributions()
    {
        EditorStatistics statistics = m_runtime.context.statistics;
        statistics.Publish(
            new EditorStatistic(new EditorStatisticID("first"), new EditorStatisticGroupID("sampling"), "Sampling", "First", "1")
        );
        IReadOnlyList<EditorStatistic> first = statistics.GetSnapshot();
        m_runtime.Update(new EditorFrame(0.016f, 1f, isFocused: true));
        Assert.Same(first, statistics.GetSnapshot());
        statistics.Publish(
            new EditorStatistic(new EditorStatisticID("second"), new EditorStatisticGroupID("sampling"), "Sampling", "Second", "2")
        );
        IReadOnlyList<EditorStatistic> combined = statistics.GetSnapshot();
        Assert.Equal(2, combined.Count(value => value.groupID == new EditorStatisticGroupID("sampling")));
        m_runtime.Update(new EditorFrame(0.016f, 2f, isFocused: true));
        Assert.Equal(
            "second",
            Assert.Single(statistics.GetSnapshot().Where(value => value.groupID == new EditorStatisticGroupID("sampling"))).id.value
        );
        Assert.Equal(2, combined.Count);
        m_runtime.Update(new EditorFrame(0.016f, 3f, isFocused: true));
        Assert.DoesNotContain(statistics.GetSnapshot(), value => value.groupID == new EditorStatisticGroupID("sampling"));
    }

    [Fact]
    public void StatisticGroupingAndOrderChangesInvalidateTheSnapshot()
    {
        var statistics = new EditorStatistics();
        statistics.Publish(
            new EditorStatistic(new EditorStatisticID("a"), new EditorStatisticGroupID("old"), "Old", "First", "1", groupOrder: 1)
        );
        statistics.Publish(
            new EditorStatistic(new EditorStatisticID("b"), new EditorStatisticGroupID("second"), "Second", "Second", "2", groupOrder: 2)
        );
        IReadOnlyList<EditorStatistic> original = statistics.GetSnapshot();
        statistics.Publish(
            new EditorStatistic(new EditorStatisticID("b"), new EditorStatisticGroupID("new"), "New", "Renamed", "2", groupOrder: 0)
        );
        IReadOnlyList<EditorStatistic> changed = statistics.GetSnapshot();
        Assert.NotSame(original, changed);
        Assert.Equal("b", changed[0].id.value);
        Assert.Equal("New", changed[0].groupName);
        Assert.Equal("Renamed", changed[0].label);
        Assert.Equal("a", original[0].id.value);
    }
}
