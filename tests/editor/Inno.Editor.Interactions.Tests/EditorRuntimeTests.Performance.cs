using System;
using System.Diagnostics;
using System.Text.Json;
using Inno.Editor.Core;
using Xunit;

namespace Inno.Editor.Interactions.Tests;

public sealed partial class EditorRuntimeTests
{
    [Fact]
    [Trait("Category", "Performance")]
    public void PublicCommandAndStatisticsPathsProduceRepeatableSamples()
    {
        var interaction = m_runtime.interactions.For("tests/menu");
        Measure("menu-open", () => Assert.NotEmpty(interaction.BuildMenu().items));
        var statistics = new EditorStatistics();
        for (int index = 0; index < 100; index++)
        {
            statistics.Publish(new EditorStatistic(
                new("metric/" + index),
                new("group/" + index / 10),
                "Group " + index / 10,
                "Metric " + index,
                index.ToString(),
                order: index));
        }
        Measure("statistics-unchanged", () => Assert.Equal(100, statistics.GetSnapshot().Count));
    }

    private static void Measure(
        string name,
        Action operation
    ) {
        const int iterations = 2_000;
        for (int warmup = 0; warmup < 200; warmup++)
            operation();
        for (int sample = 0; sample < 5; sample++)
        {
            long before = GC.GetAllocatedBytesForCurrentThread();
            long start = Stopwatch.GetTimestamp();
            for (int iteration = 0; iteration < iterations; iteration++)
                operation();
            double milliseconds = Stopwatch.GetElapsedTime(start).TotalMilliseconds;
            long bytes = GC.GetAllocatedBytesForCurrentThread() - before;
            Console.WriteLine(JsonSerializer.Serialize(new { workload = name, sample, iterations, milliseconds, bytes }));
        }
    }
}
