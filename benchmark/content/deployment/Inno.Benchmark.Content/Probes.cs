using Inno.Benchmark.Runner;

namespace Inno.Benchmark.Content;

internal static class Probes
{
    internal static void ContentPack(ProbeReport report)
    {
        long baseline = ProbeReport.Settle();
        var bench = new ContentPackBenchmarks { entryCount = 1024, payloadBytes = 4096 };
        double first = ProbeReport.Time(bench.Setup);
        double cold = ProbeReport.Time(() => bench.OpenAndReadAll());
        double warm = ProbeReport.Median(() => bench.ReadAll());
        bench.Cleanup();
        report.Row("content.1024x4KiB.setup+first open ms", first);
        report.Row("content.1024x4KiB.first open+read ms", cold);
        report.Row("content.1024x4KiB.warm read-all median ms", warm);
        report.Row("content.retained after dispose KiB", (ProbeReport.Settle() - baseline) / 1024.0);
    }
}
