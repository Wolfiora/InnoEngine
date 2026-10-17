using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Rendering;

/// <summary>
/// Describes a graphics program candidate and reflected interface contract.
/// </summary>
public sealed class GraphicsPipelineDescriptor
{
    private readonly byte[] m_vertexShader;
    private readonly byte[] m_fragmentShader;
    private readonly IReadOnlyList<RenderShaderBindingDescriptor> m_bindings;

    /// <summary>
    /// Creates a graphics pipeline descriptor.
    /// </summary>
    /// <param name="vertexShader">
    /// Target backend vertex shader binary.
    /// </param>
    /// <param name="fragmentShader">
    /// Target backend fragment shader binary.
    /// </param>
    /// <param name="bindings">
    /// Manifest-derived interface contract.
    /// </param>
    /// <param name="vertexLayout">
    /// Required mesh vertex layout, or <see langword="null"/> for procedural vertices.
    /// </param>
    /// <param name="rasterState">
    /// Fixed-function raster state.
    /// </param>
    public GraphicsPipelineDescriptor(
        ReadOnlySpan<byte> vertexShader,
        ReadOnlySpan<byte> fragmentShader,
        IReadOnlyList<RenderShaderBindingDescriptor> bindings,
        RenderVertexLayout? vertexLayout,
        RenderRasterState? rasterState = null
    ) {
        if (vertexShader.IsEmpty)
        {
            throw new ArgumentException("Vertex shader binary cannot be empty.", nameof(vertexShader));
        }

        if (fragmentShader.IsEmpty)
        {
            throw new ArgumentException("Fragment shader binary cannot be empty.", nameof(fragmentShader));
        }

        ArgumentNullException.ThrowIfNull(bindings);
        EnsureUniqueBindings(bindings);
        m_vertexShader = vertexShader.ToArray();
        m_fragmentShader = fragmentShader.ToArray();
        m_bindings = Array.AsReadOnly(bindings.ToArray());
        this.vertexLayout = vertexLayout;
        this.rasterState = rasterState ?? RenderRasterState.opaque;
    }

    /// <summary>
    /// Gets the target backend vertex shader binary.
    /// </summary>
    public ReadOnlyMemory<byte> vertexShader => m_vertexShader;

    /// <summary>
    /// Gets the target backend fragment shader binary.
    /// </summary>
    public ReadOnlyMemory<byte> fragmentShader => m_fragmentShader;

    /// <summary>
    /// Gets the manifest-derived interface contract.
    /// </summary>
    public IReadOnlyList<RenderShaderBindingDescriptor> bindings => m_bindings;

    /// <summary>
    /// Gets the required mesh vertex layout, or <see langword="null"/> for procedural vertices.
    /// </summary>
    public RenderVertexLayout? vertexLayout { get; }

    /// <summary>
    /// Gets fixed-function raster state.
    /// </summary>
    public RenderRasterState rasterState { get; }

    private static void EnsureUniqueBindings(IReadOnlyList<RenderShaderBindingDescriptor> bindings)
    {
        if (bindings.Select(static value => value.id).Distinct().Count() != bindings.Count)
        {
            throw new ArgumentException("Pipeline bindings require unique stable IDs.", nameof(bindings));
        }
    }
}

