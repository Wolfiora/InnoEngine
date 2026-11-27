using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Loader;
using Inno.Benchmark.Runner;
using Inno.Extensibility.Reload;

namespace Inno.Benchmark.Reload;

internal static class Probes
{
    internal static void Reload(ProbeReport report)
    {
        long baseline = ProbeReport.Settle();
        int contextsBefore = AssemblyLoadContext.All.Count();
        IAssemblyUnloadProbe last;
        using (var fixture = new ReloadFixture())
        {
            double first = ProbeReport.Time(() => ReloadFixture.WaitForRetirement(fixture.HotReload()));
            long afterFirst = ProbeReport.Settle();
            List<int> collections = [];
            double activationMs = 0;
            double retirementMs = 0;
            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int index = 0; index < 50; index++)
            {
                IAssemblyUnloadProbe retired = fixture.TimedActivation(out double elapsed);
                activationMs += elapsed;
                long start = Stopwatch.GetTimestamp();
                collections.Add(ReloadFixture.WaitForRetirement(retired));
                retirementMs += Stopwatch.GetElapsedTime(start).TotalMilliseconds;
                if (!retired.isCompleted) throw new InvalidOperationException("Retired generation still live.");
            }
            stopwatch.Stop();
            report.Row("reload.first hot reload+retire ms", first);
            report.Row("reload.50 hot reloads avg ms", stopwatch.Elapsed.TotalMilliseconds / 50);
            report.Row("reload.candidate activation mean ms", activationMs / 50);
            report.Row("reload.wait for retirement mean ms", retirementMs / 50);
            report.Row("reload.completed unload probes", collections.Count);
            report.Row("reload.GC passes to retire avg", collections.Average());
            report.Row("reload.heap growth over 50 reloads KiB", (ProbeReport.Settle() - afterFirst) / 1024.0);
            report.Row("reload.live AssemblyLoadContexts delta while host alive", AssemblyLoadContext.All.Count() - contextsBefore);
            last = fixture.UnloadCurrent();
            report.Row("reload.final unload completed before dispose", last.isCompleted ? 1 : 0);
        }
        report.Row("reload.final unload completed immediately after dispose", last.isCompleted ? 1 : 0);
        report.Row("reload.final unload forced GC passes", ReloadFixture.WaitForRetirement(last));
        report.Row("reload.final unload completed after dispose", last.isCompleted ? 1 : 0);
        ProbeReport.Settle();
        report.Row("reload.live AssemblyLoadContexts delta after dispose", AssemblyLoadContext.All.Count() - contextsBefore);
        report.Row("reload.retained after dispose KiB", (ProbeReport.Settle() - baseline) / 1024.0);
    }
}
