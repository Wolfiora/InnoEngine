using Inno.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Inno.Rendering.Assets;

/// <summary>
/// Carries one immutable compiled shader pass and its reflected runtime bindings.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("f22a2116-992d-5c69-bf6c-af05c191123b")]
public sealed class RenderShaderPassArtifact
{
    private readonly IReadOnlyList<RenderShaderStageArtifact> m_stages;

    /// <summary>
    /// Creates a deployed shader pass.
    /// </summary>
    /// <param name="name">
    /// The stable pass name within the owning shader.
    /// </param>
    /// <param name="programKind">
    /// The programmable stage combination used by the pass.
    /// </param>
    /// <param name="rasterState">
    /// The backend-neutral fixed-function state used by raster passes.
    /// </param>
    /// <param name="shaderInterface">
    /// The validated pass-local resource binding contract.
    /// </param>
    /// <param name="stages">
    /// The complete target stage binaries required by the pass.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when the name, stage set, or program kind is invalid.
    /// </exception>
    public RenderShaderPassArtifact(
        string name,
        ShaderProgramKind programKind,
        RenderRasterState rasterState,
        ShaderInterface shaderInterface,
        IReadOnlyList<RenderShaderStageArtifact> stages
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        if (!Enum.IsDefined(programKind))
            throw new ArgumentOutOfRangeException(nameof(programKind));
        ArgumentNullException.ThrowIfNull(rasterState);
        ArgumentNullException.ThrowIfNull(shaderInterface);
        ArgumentNullException.ThrowIfNull(stages);
        RenderShaderStageArtifact[] stageSnapshot = stages.ToArray();
        ValidateStages(programKind, stageSnapshot);
        this.name = name;
        this.programKind = programKind;
        this.rasterState = CloneRasterState(rasterState);
        this.shaderInterface = CloneInterface(shaderInterface);
        m_stages = Array.AsReadOnly(stageSnapshot);
    }

    /// <summary>
    /// Gets the stable pass name within the owning shader.
    /// </summary>
    public string name { get; }

    /// <summary>
    /// Gets the programmable stage combination used by this pass.
    /// </summary>
    public ShaderProgramKind programKind { get; }

    /// <summary>
    /// Gets an immutable copy of the backend-neutral raster state.
    /// </summary>
    public RenderRasterState rasterState { get; }

    /// <summary>
    /// Gets the validated pass-local resource binding contract.
    /// </summary>
    public ShaderInterface shaderInterface { get; }

    /// <summary>
    /// Gets the complete target stage binaries required by this pass.
    /// </summary>
    public IReadOnlyList<RenderShaderStageArtifact> stages => m_stages;

    internal static ShaderInterface CloneInterface(ShaderInterface source)
        => new(source.bindings.Select(static binding => new ShaderInterfaceBinding(
            binding.id,
            binding.type,
            binding.stages,
            binding.arrayCount,
            binding.bindingKind,
            binding.storageAccess,
            binding.nativeName,
            binding.location)).ToArray());

    internal static RenderRasterState CloneRasterState(RenderRasterState source)
        => new()
        {
            topology = source.topology,
            cull = source.cull,
            frontFace = source.frontFace,
            depthCompare = source.depthCompare,
            depthWrite = source.depthWrite,
            blend = source.blend,
            colorWriteMask = source.colorWriteMask,
            multisampling = source.multisampling
        };

    private static void ValidateStages(
        ShaderProgramKind programKind,
        IReadOnlyCollection<RenderShaderStageArtifact> stages
    ) {
        ShaderStage[] actual = stages.Select(static value => value.stage).Order().ToArray();
        ShaderStage[] expected = programKind == ShaderProgramKind.Raster
            ? [ShaderStage.Vertex, ShaderStage.Fragment]
            : [ShaderStage.Compute];
        if (!actual.SequenceEqual(expected))
        {
            throw new ArgumentException(
                $"Program kind '{programKind}' requires stages '{string.Join(", ", expected)}'.",
                nameof(stages));
        }
    }
}

