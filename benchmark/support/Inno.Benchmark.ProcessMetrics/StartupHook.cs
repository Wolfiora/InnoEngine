using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;

// The .NET startup hook protocol requires this name in the global namespace.
internal static class StartupHook
{
    public static void Initialize()
    {
        string? report = Environment.GetEnvironmentVariable("INNO_BENCHMARK_PROCESS_METRICS");
        if (string.IsNullOrEmpty(report)) return;
        Environment.SetEnvironmentVariable("INNO_BENCHMARK_PROCESS_METRICS", null);
        long allocated = GC.GetTotalAllocatedBytes(true);
        int gen0 = GC.CollectionCount(0);
        int gen1 = GC.CollectionCount(1);
        int gen2 = GC.CollectionCount(2);
        AppDomain.CurrentDomain.ProcessExit += (
            _,
            _
        ) =>
        {
            long bytes = GC.GetTotalAllocatedBytes(true) - allocated;
            using Process process = Process.GetCurrentProcess();
            File.WriteAllLines(report, [
                "pid=" + Environment.ProcessId,
                "runtime=" + Environment.Version,
                "allocatedBytes=" + bytes,
                "gen0=" + (GC.CollectionCount(0) - gen0),
                "gen1=" + (GC.CollectionCount(1) - gen1),
                "gen2=" + (GC.CollectionCount(2) - gen2),
                "cpuMilliseconds=" + process.TotalProcessorTime.TotalMilliseconds.ToString(CultureInfo.InvariantCulture)
            ]);
        };
    }
}
