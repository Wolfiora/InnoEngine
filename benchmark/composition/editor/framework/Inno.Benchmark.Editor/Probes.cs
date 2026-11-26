using Inno.Benchmark.Runner;

namespace Inno.Benchmark.Editor;

internal static class Probes
{
    internal static void EditorGraph(ProbeReport report)
    {
        long baseline = ProbeReport.Settle();
        var bench = new EditorGraphBenchmarks { nodeCount = 512 };
        double setup = ProbeReport.Time(bench.Setup);
        double first = ProbeReport.Time(() => bench.EditDocument());
        long warm = ProbeReport.Settle();
        for (int index = 0; index < 50; index++)
        {
            bench.EditDocument();
        }
        report.Row("editor.runtime start ms", setup);
        report.Row("editor.graph.512 first edit session ms", first);
        report.Row("editor.graph.heap growth over 50 open/edit/close KiB", (ProbeReport.Settle() - warm) / 1024.0);
        bench.Cleanup();
        report.Row("editor.retained after dispose KiB", (ProbeReport.Settle() - baseline) / 1024.0);
    }
}
