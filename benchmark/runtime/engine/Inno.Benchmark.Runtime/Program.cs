using Inno.Benchmark.Runner;

namespace Inno.Benchmark.Runtime;

internal static class Program
{
    private static int Main(string[] args) => BenchmarkRunner.Run(typeof(Program).Assembly, args);
}
