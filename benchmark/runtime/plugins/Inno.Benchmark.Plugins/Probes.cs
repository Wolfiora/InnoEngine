using System;
using System.IO;
using System.Linq;
using Inno.Benchmark.Runner;

namespace Inno.Benchmark.Plugins;

internal static class Probes
{
    internal static void Plugins(ProbeReport report)
    {
        long baseline = ProbeReport.Settle();
        using (var fixture = new PluginScanFixture(64, 8))
        {
            double cold = ProbeReport.Time(() => fixture.CreateService().Scan());
            int files = Directory.EnumerateFiles(fixture.library, "*", SearchOption.AllDirectories).Count();
            DateTime stamp = Directory.EnumerateFiles(fixture.library, "*", SearchOption.AllDirectories).Max(File.GetLastWriteTimeUtc);
            double warm = ProbeReport.Median(() => fixture.CreateService().Scan());
            DateTime after = Directory.EnumerateFiles(fixture.library, "*", SearchOption.AllDirectories).Max(File.GetLastWriteTimeUtc);
            report.Row("plugin.64.cold scan (empty Library) ms", cold);
            report.Row("plugin.64.warm scan median ms", warm);
            report.Row("plugin.64.Library files after scan", files);
            report.Row("plugin.64.warm scan rewrote cache (0=reuse)", after > stamp ? 1 : 0);
        }
        report.Row("plugin.retained after dispose KiB", (ProbeReport.Settle() - baseline) / 1024.0);
    }
}
