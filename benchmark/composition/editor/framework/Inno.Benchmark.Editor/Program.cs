using System;
using System.Collections.Generic;
using Inno.Benchmark.Runner;

namespace Inno.Benchmark.Editor;

internal static class Program
{
    private static int Main(string[] args)
        => BenchmarkRunner.Run(typeof(Program).Assembly, args, new Dictionary<string, Action<ProbeReport>>
        {
            ["editor"] = Probes.EditorGraph
        });
}
