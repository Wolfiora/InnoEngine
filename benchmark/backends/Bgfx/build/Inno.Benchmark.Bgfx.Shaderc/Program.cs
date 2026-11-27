using System;
using System.IO;
using System.Runtime.InteropServices;
using Inno.Benchmark.Runner;

namespace Inno.Benchmark.Bgfx.Shaderc;

internal static class Program
{
    private static int Main(string[] args) => BenchmarkRunner.Run(typeof(Program).Assembly, args, available: Available);

    private static bool Available()
    {
        bool supported = OperatingSystem.IsWindows() && RuntimeInformation.ProcessArchitecture == Architecture.X64
                         || OperatingSystem.IsMacOS() && RuntimeInformation.ProcessArchitecture == Architecture.Arm64;
        if (supported && File.Exists(Environment.GetEnvironmentVariable("INNO_BENCHMARK_SHADERC"))
            && Directory.Exists(Environment.GetEnvironmentVariable("INNO_BENCHMARK_SHADER_INCLUDES"))
        ) {
            return true;
        }
        Console.Error.WriteLine("Unavailable: shaderc needs a supported host, an executable and its include directory.");
        return false;
    }
}
