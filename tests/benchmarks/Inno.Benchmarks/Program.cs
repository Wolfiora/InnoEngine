using System;
using System.IO;
using BenchmarkDotNet.Running;

namespace Inno.Benchmarks;

public static class Program
{
    public static int Main(string[] args)
    {
        string modules = Path.Combine(AppContext.BaseDirectory, "Modules");
        if (Directory.Exists(modules) && Environment.GetEnvironmentVariable(ReloadFixture.C_MODULE_DIRECTORY_VARIABLE) is null)
        {
            Environment.SetEnvironmentVariable(ReloadFixture.C_MODULE_DIRECTORY_VARIABLE, modules);
        }
        if (args.Length > 0 && args[0] == "--probes")
        {
            return ProbeRunner.Run(args.Length > 1 ? args[1] : "probes.md");
        }
        _ = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        return 0;
    }
}
