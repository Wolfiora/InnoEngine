using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Inno.Build.Toolchains.Bgfx.Tools;
using Inno.Core.Graphs;
using Inno.Core.Serialization;
using Inno.Rendering;
using Inno.Rendering.Assets.Authoring;
using Inno.Rendering.Shaders;

namespace Inno.Benchmark.Shaders;

/// <summary>
/// Backend toolchain replacement that counts invocations (cache misses) and returns fixed-size bytecode.
/// </summary>
public sealed class CountingShaderToolchain(int outputBytes = 4096) : IShaderCompilerToolchain
{
    private readonly byte[] m_bytecode = new byte[outputBytes];
    private int m_invocations;

    public int invocations => Volatile.Read(ref m_invocations);

    public string implementationID => "bench.counting";

    public IReadOnlyList<string> supportedSourceLanguages { get; } = ["inno.shader-language.bgfx-sc"];

    public ValueTask<ShaderStageToolResult> CompileAsync(
        ShaderStageToolRequest request,
        CancellationToken cancellationToken
    ) {
        Interlocked.Increment(ref m_invocations);
        return ValueTask.FromResult(new ShaderStageToolResult(m_bytecode, [], []));
    }

    public ShaderCompileTarget CreateTarget(
        GraphicsCapabilities capabilities,
        bool optimize = true,
        bool debugInformation = false
    ) => new("bench.counting.metal", capabilities, optimize, debugInformation);
}
