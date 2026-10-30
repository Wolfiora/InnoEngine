using System;
using System.Diagnostics;
using System.Text.Json;
using Inno.Core.Events;
using Inno.Core.Input;
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
        m_runtime.interactions.For("tests/multiple-shortcuts").Focus();
        int beforeDispatch = MultiShortcutAction.executeCount;
        Measure("shortcut-dispatch", () => m_runtime.HandleKeyPressed(new KeyPressedEvent(0, KeyCode.F3, KeyModifier.None)));
        Assert.Equal(beforeDispatch + 10_200, MultiShortcutAction.executeCount);
        Measure("shortcut-unmatched", () => m_runtime.HandleKeyPressed(new KeyPressedEvent(0, KeyCode.F12, KeyModifier.None)));
        Assert.Equal(beforeDispatch + 10_200, MultiShortcutAction.executeCount);
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
        using Process process = Process.GetCurrentProcess();
        for (int warmup = 0; warmup < 200; warmup++)
            operation();
        for (int sample = 0; sample < 5; sample++)
        {
            TimeSpan cpu = process.TotalProcessorTime;
            int gen0 = GC.CollectionCount(0);
            int gen1 = GC.CollectionCount(1);
            int gen2 = GC.CollectionCount(2);
            long before = GC.GetAllocatedBytesForCurrentThread();
            long start = Stopwatch.GetTimestamp();
            for (int iteration = 0; iteration < iterations; iteration++)
                operation();
            double milliseconds = Stopwatch.GetElapsedTime(start).TotalMilliseconds;
            long bytes = GC.GetAllocatedBytesForCurrentThread() - before;
            Console.WriteLine(JsonSerializer.Serialize(new
            {
                workload = name, sample, iterations, milliseconds, bytes,
                cpuMilliseconds = (process.TotalProcessorTime - cpu).TotalMilliseconds,
                gen0 = GC.CollectionCount(0) - gen0,
                gen1 = GC.CollectionCount(1) - gen1,
                gen2 = GC.CollectionCount(2) - gen2
            }));
        }
    }
}
