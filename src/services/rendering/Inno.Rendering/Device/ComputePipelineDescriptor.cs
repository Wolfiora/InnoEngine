using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Rendering;

/// <summary>
/// Describes a compute program candidate and reflected interface contract.
/// </summary>
public sealed class ComputePipelineDescriptor
{
    private readonly byte[] m_computeShader;
    private readonly IReadOnlyList<RenderShaderBindingDescriptor> m_bindings;

    /// <summary>
    /// Creates a compute pipeline descriptor.
    /// </summary>
    /// <param name="computeShader">
    /// Target backend compute shader binary.
    /// </param>
    /// <param name="bindings">
    /// Manifest-derived interface contract.
    /// </param>
    public ComputePipelineDescriptor(
        ReadOnlySpan<byte> computeShader,
        IReadOnlyList<RenderShaderBindingDescriptor> bindings
    ) {
        if (computeShader.IsEmpty)
        {
            throw new ArgumentException("Compute shader binary cannot be empty.", nameof(computeShader));
        }

        ArgumentNullException.ThrowIfNull(bindings);
        if (bindings.Select(static value => value.id).Distinct().Count() != bindings.Count)
        {
            throw new ArgumentException("Pipeline bindings require unique stable IDs.", nameof(bindings));
        }

        m_computeShader = computeShader.ToArray();
        m_bindings = Array.AsReadOnly(bindings.ToArray());
    }

    /// <summary>
    /// Gets the target backend compute shader binary.
    /// </summary>
    public ReadOnlyMemory<byte> computeShader => m_computeShader;

    /// <summary>
    /// Gets the manifest-derived interface contract.
    /// </summary>
    public IReadOnlyList<RenderShaderBindingDescriptor> bindings => m_bindings;
}

