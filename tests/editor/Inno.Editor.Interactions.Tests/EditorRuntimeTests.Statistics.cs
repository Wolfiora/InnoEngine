using System.Linq;
using Inno.Editor.Core;
using Xunit;

namespace Inno.Editor.Interactions.Tests;

public sealed partial class EditorRuntimeTests
{
    [Fact]
    public void UnchangedStatisticsReuseAnImmutableSnapshot()
    {
        var statistics = new EditorStatistics();
        statistics.Publish(new EditorStatistic(new("frame"), new("timing"), "Timing", "Frame", "16 ms"));
        var first = statistics.GetSnapshot();
        Assert.Same(first, statistics.GetSnapshot());
        statistics.Publish(new EditorStatistic(new("frame"), new("timing"), "Timing", "Frame", "16 ms"));
        Assert.Same(first, statistics.GetSnapshot());
        statistics.Publish(new EditorStatistic(new("frame"), new("timing"), "Timing", "Frame", "8 ms"));
        var changed = statistics.GetSnapshot();
        Assert.NotSame(first, changed);
        Assert.Equal("16 ms", Assert.Single(first).value);
        Assert.Equal("8 ms", Assert.Single(changed).value);
    }

    [Fact]
    public void StatisticFrameHandoffRetainsLateProvidersAndRemovesExpiredContributions()
    {
        EditorStatistics statistics = m_runtime.context.statistics;
        statistics.Publish(new EditorStatistic(new("first"), new("sampling"), "Sampling", "First", "1"));
        var first = statistics.GetSnapshot();
        m_runtime.Update(new EditorFrame(0.016f, 1f, isFocused: true));
        Assert.Same(first, statistics.GetSnapshot());
        statistics.Publish(new EditorStatistic(new("second"), new("sampling"), "Sampling", "Second", "2"));
        var combined = statistics.GetSnapshot();
        Assert.Equal(2, combined.Count(value => value.groupId == new EditorStatisticGroupId("sampling")));
        m_runtime.Update(new EditorFrame(0.016f, 2f, isFocused: true));
        Assert.Equal("second", Assert.Single(statistics.GetSnapshot().Where(value => value.groupId == new EditorStatisticGroupId("sampling"))).id.value);
        Assert.Equal(2, combined.Count);
        m_runtime.Update(new EditorFrame(0.016f, 3f, isFocused: true));
        Assert.DoesNotContain(statistics.GetSnapshot(), value => value.groupId == new EditorStatisticGroupId("sampling"));
    }

    [Fact]
    public void StatisticGroupingAndOrderChangesInvalidateTheSnapshot()
    {
        var statistics = new EditorStatistics();
        statistics.Publish(new EditorStatistic(new("a"), new("old"), "Old", "First", "1", groupOrder: 1));
        statistics.Publish(new EditorStatistic(new("b"), new("second"), "Second", "Second", "2", groupOrder: 2));
        var original = statistics.GetSnapshot();
        statistics.Publish(new EditorStatistic(new("b"), new("new"), "New", "Renamed", "2", groupOrder: 0));
        var changed = statistics.GetSnapshot();
        Assert.NotSame(original, changed);
        Assert.Equal("b", changed[0].id.value);
        Assert.Equal("New", changed[0].groupName);
        Assert.Equal("Renamed", changed[0].label);
        Assert.Equal("a", original[0].id.value);
    }
}
