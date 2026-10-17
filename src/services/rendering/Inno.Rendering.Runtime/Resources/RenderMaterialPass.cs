using Inno.Core.Diagnostics;
using Inno.Rendering;
using Inno.Rendering.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Rendering.Runtime;

[Inno.Extensibility.Types.StableTypeId("b5677cc4-c195-5f52-a52f-5efecec8314e")]
internal enum RenderMaterialBindingKind
{
    Uniform,
    Texture
}

[Inno.Extensibility.Types.StableTypeId("702a1ec9-219b-5d44-b322-1a433e7e41b5")]
internal sealed record RenderMaterialBinding(
    RenderMaterialBindingKind kind,
    RenderBindingId id,
    byte[]? uniformData,
    PersistentTextureHandle texture,
    RenderSamplerState sampler
);

/// <summary>
/// Represents one frame-resolved material pass and its material-owned bindings.
/// </summary>
/// <remarks>
/// The instance is scoped to the active render device generation. Pipeline-owned graph textures and
/// buffers remain explicit and are bound by the caller after this material state is applied.
/// </remarks>
[Inno.Extensibility.Types.StableTypeId("b1566a8a-dd0a-5dca-8f43-6bb12aa86973")]
public sealed class RenderMaterialPass
{
    private readonly ShaderPassDefinition m_definition;
    private readonly IReadOnlyDictionary<RenderBindingId, RenderShaderBindingKind> m_declaredBindings;
    private readonly IReadOnlySet<RenderBindingId> m_activeBindings;
    private readonly IReadOnlyList<RenderMaterialBinding> m_bindings;

    internal RenderMaterialPass(
        ShaderPassDefinition definition,
        GraphicsPipelineHandle graphicsPipeline,
        ComputePipelineHandle computePipeline,
        IReadOnlyList<ShaderPropertyDefinition> declaredBindings,
        ShaderInterface activeInterface,
        IReadOnlyList<RenderMaterialBinding> bindings
    ) {
        ArgumentNullException.ThrowIfNull(declaredBindings);
        ArgumentNullException.ThrowIfNull(activeInterface);
        m_definition = definition.Copy();
        this.graphicsPipeline = graphicsPipeline;
        this.computePipeline = computePipeline;
        m_declaredBindings = declaredBindings.ToDictionary(
            static property => new RenderBindingId(property.id.value),
            static property => ToRenderBindingKind(property.bindingKind));
        m_activeBindings = activeInterface.bindings
            .Select(static binding => new RenderBindingId(binding.id.value))
            .ToHashSet();
        m_bindings = Array.AsReadOnly(bindings.Select(static binding => binding with
        {
            uniformData = binding.uniformData?.ToArray()
        }).ToArray());
    }

    /// <summary>
    /// Gets the selected provider-defined shader pass with detached metadata storage.
    /// </summary>
    public ShaderPassDefinition definition => m_definition.Copy();

    /// <summary>
    /// Gets the graphics pipeline, or an invalid handle for a compute pass.
    /// </summary>
    public GraphicsPipelineHandle graphicsPipeline { get; }

    /// <summary>
    /// Gets the compute pipeline, or an invalid handle for a raster pass.
    /// </summary>
    public ComputePipelineHandle computePipeline { get; }

    /// <summary>
    /// Gets whether this is a graphics material pass.
    /// </summary>
    public bool isGraphics => graphicsPipeline.isValid;

    /// <summary>
    /// Gets whether this is a compute material pass.
    /// </summary>
    public bool isCompute => computePipeline.isValid;

    /// <summary>
    /// Gets whether the compiled program actively consumes one declared binding.
    /// </summary>
    /// <param name="binding">
    /// Stable binding declared by the shader contract.
    /// </param>
    /// <param name="kind">
    /// Expected command-binding domain.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the final compiled pass reflects the binding; otherwise,
    /// <see langword="false"/> when the compiler proved the declared binding unused and removed it.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The binding is not declared by the shader, or its declared kind differs from <paramref name="kind"/>.
    /// </exception>
    public bool UsesBinding(
        RenderBindingId binding,
        RenderShaderBindingKind kind
    ) {
        if (!binding.isValid)
            throw new ArgumentException("A stable shader binding identifier is required.", nameof(binding));
        if (!Enum.IsDefined(kind))
            throw new ArgumentOutOfRangeException(nameof(kind));
        if (!m_declaredBindings.TryGetValue(binding, out RenderShaderBindingKind declaredKind))
            throw new ArgumentException($"Shader does not declare binding '{binding.value}'.", nameof(binding));
        if (declaredKind != kind)
            throw new ArgumentException(
                $"Shader binding '{binding.value}' is declared as {declaredKind}, not {kind}.",
                nameof(kind));
        return m_activeBindings.Contains(binding);
    }

    /// <summary>
    /// Binds the program and all material-owned values and textures.
    /// </summary>
    /// <param name="commands">
    /// Current pass command encoder.
    /// </param>
    public void Bind(RenderCommandEncoder commands)
    {
        ArgumentNullException.ThrowIfNull(commands);
        if (graphicsPipeline.isValid)
            commands.BindGraphicsPipeline(graphicsPipeline);
        else if (computePipeline.isValid)
            commands.BindComputePipeline(computePipeline);
        else
            throw new InvalidOperationException("A resolved material pass has no active program.");

        foreach (RenderMaterialBinding binding in m_bindings)
        {
            if (binding.kind == RenderMaterialBindingKind.Uniform)
                commands.SetUniform(binding.id, binding.uniformData ?? []);
            else
                commands.BindTexture(binding.id, binding.texture, binding.sampler);
        }
    }

    private static RenderShaderBindingKind ToRenderBindingKind(ShaderPropertyBindingKind kind)
        => kind switch
        {
            ShaderPropertyBindingKind.Uniform => RenderShaderBindingKind.Uniform,
            ShaderPropertyBindingKind.SampledTexture => RenderShaderBindingKind.Texture,
            ShaderPropertyBindingKind.StorageTexture => RenderShaderBindingKind.StorageTexture,
            ShaderPropertyBindingKind.StorageBuffer => RenderShaderBindingKind.StorageBuffer,
            _ => throw new ArgumentOutOfRangeException(nameof(kind))
        };
}

