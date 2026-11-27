using System;
using System.Collections.Generic;
using Inno.Benchmark.Runner;

namespace Inno.Benchmark.Shaders;

internal static class Program
{
    private static int Main(string[] args)
        => BenchmarkRunner.Run(typeof(Program).Assembly, args, new Dictionary<string, Action<ProbeReport>>
        {
            ["shaders"] = Probes.Shaders,
            ["shader-entries"] = report => Probes.ShaderCapacity(report, false),
            ["shader-bytes"] = report => Probes.ShaderCapacity(report, true)
        });
}
