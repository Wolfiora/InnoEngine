using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Extensibility.Types;

namespace Inno.Rendering.Assets.Authoring;

/// <summary>
/// Contains an immutable target shader artifact and expected reflected interface.
/// </summary>
[StableTypeID("25e556fe-a071-58b2-8e70-0e41bd434d83")]
public sealed class CompiledShaderArtifact
{
    private readonly byte[] m_definitionData;

    /// <summary>
    /// Creates a compiled shader artifact.
    /// </summary>
    /// <param name="shaderName">
    /// Artist-facing shader name.
    /// </param>
    /// <param name="targetKey">
    /// Stable target cache key.
    /// </param>
    /// <param name="variant">
    /// Static keyword variant.
    /// </param>
    /// <param name="shaderInterface">
    /// Manifest-derived binding contract.
    /// </param>
    /// <param name="passes">
    /// Compiled pass binaries.
    /// </param>
    /// <param name="definitionData">
    /// Native-serialized runtime definition captured before asynchronous compilation.
    /// </param>
    public CompiledShaderArtifact(
        string shaderName,
        string targetKey,
        RenderShaderVariant variant,
        ShaderInterface shaderInterface,
        IReadOnlyList<CompiledShaderPass> passes,
        ReadOnlySpan<byte> definitionData
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(shaderName);
        ArgumentException.ThrowIfNullOrWhiteSpace(targetKey);
        ArgumentNullException.ThrowIfNull(shaderInterface);
        ArgumentNullException.ThrowIfNull(passes);
        this.shaderName = shaderName;
        this.targetKey = targetKey;
        this.variant = variant;
        this.shaderInterface = shaderInterface;
        this.passes = Array.AsReadOnly(passes.ToArray());
        if (definitionData.IsEmpty)
        {
            throw new ArgumentException("Compiled programs require a captured runtime contract.", nameof(definitionData));
        }
        m_definitionData = definitionData.ToArray();
    }

    /// <summary>
    /// Gets the artist-facing shader name.
    /// </summary>
    public string shaderName { get; }

    /// <summary>
    /// Gets the stable target cache key.
    /// </summary>
    public string targetKey { get; }

    /// <summary>
    /// Gets the static keyword variant.
    /// </summary>
    public RenderShaderVariant variant { get; }

    /// <summary>
    /// Gets the manifest-derived binding contract.
    /// </summary>
    public ShaderInterface shaderInterface { get; }

    /// <summary>
    /// Gets compiled pass binaries.
    /// </summary>
    public IReadOnlyList<CompiledShaderPass> passes { get; }

    /// <summary>
    /// Creates the source-free deployment artifact consumed by Editor preview sessions and Players.
    /// </summary>
    /// <returns>
    /// A deep immutable runtime artifact containing only target binaries, fixed-function state, and binding contracts.
    /// </returns>
    public RenderShaderArtifact CreateRuntimeArtifact()
        => new(
            shaderName,
            targetKey,
            variant,
            shaderInterface,
            passes.Select(static pass => new RenderShaderPassArtifact(
                pass.definition.name,
                pass.definition.programKind,
                ConvertRasterState(pass.definition.renderState),
                pass.shaderInterface,
                pass.stages.Select(static stage => new RenderShaderStageArtifact(
                    stage.stage,
                    stage.bytes.Span
                )).ToArray()
            )).ToArray(),
            m_definitionData
        );

    private static RenderRasterState ConvertRasterState(ShaderRenderState source)
        => new()
    {
        topology = source.topology,
        cull = source.cull switch
        {
            ShaderCullMode.None => RenderCullMode.None,
            ShaderCullMode.Front => RenderCullMode.Front,
            ShaderCullMode.Back => RenderCullMode.Back,
            _ => throw new ArgumentOutOfRangeException(nameof(source))
        },
        frontFace = source.frontFace,
        depthCompare = source.depthCompare switch
        {
            ShaderCompareFunction.Never => RenderDepthCompare.Never,
            ShaderCompareFunction.Less => RenderDepthCompare.Less,
            ShaderCompareFunction.Equal => RenderDepthCompare.Equal,
            ShaderCompareFunction.LessEqual => RenderDepthCompare.LessEqual,
            ShaderCompareFunction.Greater => RenderDepthCompare.Greater,
            ShaderCompareFunction.NotEqual => RenderDepthCompare.NotEqual,
            ShaderCompareFunction.GreaterEqual => RenderDepthCompare.GreaterEqual,
            ShaderCompareFunction.Always => RenderDepthCompare.Always,
            _ => throw new ArgumentOutOfRangeException(nameof(source))
        },
        depthWrite = source.depthWrite,
        blend = source.blend,
        colorWriteMask = source.colorWriteMask,
        multisampling = source.multisampling
    };
}
