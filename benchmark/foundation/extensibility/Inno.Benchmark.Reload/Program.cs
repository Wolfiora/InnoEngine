using System;
using System.Collections.Generic;
using System.IO;
using Inno.Benchmark.Runner;

namespace Inno.Benchmark.Reload;

internal static class Program
{
    private static int Main(string[] args)
    {
        string modules = Path.Combine(AppContext.BaseDirectory, "Modules");
        if (Environment.GetEnvironmentVariable(ReloadFixture.C_MODULE_DIRECTORY_VARIABLE) is null)
        {
            Environment.SetEnvironmentVariable(ReloadFixture.C_MODULE_DIRECTORY_VARIABLE, modules);
        }

        return BenchmarkRunner.Run(typeof(Program).Assembly, args, new Dictionary<string, Action<ProbeReport>>
        {
            ["reload"] = Probes.Reload
        });
    }
}
