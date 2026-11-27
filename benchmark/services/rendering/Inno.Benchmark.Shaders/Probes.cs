using System.Collections.Generic;
using System.Linq;
using Inno.Benchmark.Runner;
using Inno.Rendering.Assets.Authoring;
using Inno.Rendering.Shaders;

namespace Inno.Benchmark.Shaders;

internal static class Probes
{
    internal static void Shaders(ProbeReport report)
    {
        long baseline = ProbeReport.Settle();
        var bench = new ShaderBenchmarks { chainLength = 8 };
        bench.Setup();
        CountingShaderToolchain counter = bench.toolchain;
        int start = counter.invocations;
        var compiler = new ShaderCompiler(counter);
        for (int index = 0; index < 100; index++)
        {
            _ = compiler.CompileAsync(bench.stage, bench.target).AsTask().GetAwaiter().GetResult();
        }
        report.Row("shader.100 compiles same stage: toolchain calls (misses)", counter.invocations - start);
        ShaderIrStage relowered = bench.Lower();
        int beforeRelower = counter.invocations;
        _ = compiler.CompileAsync(relowered, bench.target).AsTask().GetAwaiter().GetResult();
        report.Row("shader.re-lowered identical graph: misses", counter.invocations - beforeRelower);
        bench.Cleanup();
        report.Row("shader.retained after dispose KiB", (ProbeReport.Settle() - baseline) / 1024.0);
    }

    internal static void ShaderCapacity(
        ProbeReport report,
        bool byteLimit
    ) {
        int count = byteLimit ? 7 : 257;
        var counter = new CountingShaderToolchain(byteLimit ? 5 * 1024 * 1024 : 4096);
        var compiler = new ShaderCompiler(counter);
        var bench = new ShaderBenchmarks { chainLength = 8 };
        bench.Setup();
        try
        {
            var stages = new List<ShaderIrStage>(count);
            for (int length = 1; length <= count; length++)
            {
                var variant = new ShaderBenchmarks { chainLength = length };
                variant.Setup();
                try
                {
                    stages.Add(variant.stage);
                }
                finally
                {
                    variant.Cleanup();
                }
            }
            foreach (var stage in stages)
            {
                _ = compiler.CompileAsync(stage, bench.target).AsTask().GetAwaiter().GetResult();
            }
            report.Row("shader.distinct inserts: toolchain calls", counter.invocations);
            int before = counter.invocations;
            foreach (var stage in stages.Take(byteLimit ? 1 : 255))
            {
                _ = compiler.CompileAsync(stage, bench.target).AsTask().GetAwaiter().GetResult();
            }
            report.Row("shader.revisit: toolchain calls", counter.invocations - before);
        }
        finally
        {
            bench.Cleanup();
        }
    }
}
