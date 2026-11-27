using Inno.Benchmark.Runner;

namespace Inno.Benchmark.Editor.Search;

internal static class Program
{
    private static int Main(string[] args) => BenchmarkRunner.Run(typeof(Program).Assembly, args);
}
