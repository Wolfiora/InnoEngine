using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using Inno.Build.Toolchains.Bgfx.Tools;
using Inno.Integration.MacOSArm64.Bgfx;
using Inno.Integration.WindowsX64.Bgfx;
using Inno.Rendering;
using Inno.Rendering.Assets.Authoring;
using Inno.Rendering.Shaders;

namespace Inno.Benchmark.Bgfx.Shaderc;

/// <remarks>
/// BenchmarkDotNet derives its generated measurement runner from this class.
/// </remarks>
[MemoryDiagnoser]
public class NativeShaderBenchmarks
{
    private ShaderIrStage m_stage = null!;
    private BgfxShadercToolchain m_toolchain = null!;
    private ShaderCompileTarget m_target = null!;
    private ShaderCompiler m_compiler = null!;

    [GlobalSetup]
    public void Setup()
    {
        var builder = new ShaderIrBuilder();
        ShaderIrValue value = builder.Constant(0.25f);
        ShaderIrValue color = builder.Construct(ShaderSourceType.Atomic("float4"), value, value, value, value);
        m_stage = new ShaderIrStage(
            ShaderStage.Fragment,
            builder.Build(new Dictionary<string, ShaderIrValue> { ["color"] = color }),
            [],
            [new ShaderIrStageOutput("color", ShaderIrOutputKind.Color)]
        );
        GraphicsApi api = OperatingSystem.IsWindows() ? GraphicsApi.direct3D11 : GraphicsApi.metal;
        BgfxShaderTargetProfile profile = OperatingSystem.IsWindows()
                                          ? WindowsX64BgfxShaderProfiles.target
                                          : MacOSArm64BgfxShaderProfiles.target;
        string compiler = Environment.GetEnvironmentVariable("INNO_BENCHMARK_SHADERC")!;
        string includes = Environment.GetEnvironmentVariable("INNO_BENCHMARK_SHADER_INCLUDES")!;
        using (Stream binary = File.OpenRead(compiler))
        {
            Console.WriteLine($"shaderc={compiler} sha256={Convert.ToHexString(SHA256.HashData(binary))} api={api}");
        }
        m_toolchain = new BgfxShadercToolchain(
            profile,
            new ToolRunner(new Dictionary<BgfxTool, string> { [BgfxTool.Shaderc] = compiler }, includes)
        );
        m_target = m_toolchain.CreateTarget(BgfxTargetCapabilities.Create(api));
    }

    [IterationSetup]
    public void ResetCompiler() => m_compiler = new ShaderCompiler(m_toolchain);

    [Benchmark]
    public int CompileFragment()
    {
        ShaderStageToolResult result = m_compiler.CompileAsync(m_stage, m_target).AsTask().GetAwaiter().GetResult();
        if (!result.succeeded || result.bytes.Length == 0)
        {
            throw new InvalidOperationException("shaderc failed: " + string.Join(", ", result.diagnostics));
        }
        return result.bytes.Length;
    }
}
