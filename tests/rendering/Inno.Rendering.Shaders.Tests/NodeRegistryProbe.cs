using System;
using System.Collections.Generic;

namespace Inno.Rendering.Shaders.Tests;

internal sealed class NodeRegistryProbe : IShaderNodeCompiler, IDisposable
{
    internal static int created;
    internal static int disposed;
    internal static bool conflict;
    private readonly string m_id;

    public NodeRegistryProbe()
    {
        created++;
        m_id = conflict ? "inno.shader.constant" : "tests.registry.node";
    }

    public string definitionID => m_id;

    public IReadOnlyList<ShaderNodePort> GetPorts(ShaderNodeDescriptionContext context) => [];

    public IReadOnlyDictionary<string, ShaderIrValue> Lower(ShaderNodeLoweringContext context) => new Dictionary<string, ShaderIrValue>();

    public void Dispose() => disposed++;
}
