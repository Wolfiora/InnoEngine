using System;
using Inno.Benchmark.Runner;

namespace Inno.Benchmark.Rendering;

internal static class Probes
{
    internal static void Render(ProbeReport report)
    {
        var bench = new RenderResourceBenchmarks();
        bench.Setup();
        try
        {
            Measure("texture.first", bench.FirstTexture);
            Measure("texture.repeat", bench.RepeatTexture);
            Measure("material.first", bench.FirstMaterial);
            Measure("material.repeat", bench.RepeatMaterial);
            Measure("raw.first", () => bench.SameRevision());
            Measure("raw.sameRevision", () => bench.SameRevision());
            Measure("raw.newRevision", () => bench.NextRevision());
        }
        finally
        {
            bench.Cleanup();
        }

        void Measure(
            string name,
            Action action
        ) {
            (long opens, long bytes, int creates, int uploads) before = bench.Counters();
            action();
            (long opens, long bytes, int creates, int uploads) after = bench.Counters();
            report.Row(name + ".opens", after.opens - before.opens);
            report.Row(name + ".bytes read", after.bytes - before.bytes);
            report.Row(name + ".texture creates", after.creates - before.creates);
            report.Row(name + ".device uploads", after.uploads - before.uploads);
        }
    }
}
