using BenchmarkDotNet.Attributes;
using Inno.Rendering.Shaders;

namespace Inno.Benchmark.Shaders;

/// <remarks>
/// BenchmarkDotNet derives its generated measurement runner from this class.
/// </remarks>
[MemoryDiagnoser]
public class ShaderSourceBenchmarks
{
    [Benchmark]
    public ShaderSourceModuleAnalysis AnalyzeModule() => ShaderBenchmarks.AnalyzeSource();
}
