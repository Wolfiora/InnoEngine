using Inno.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Inno.Rendering.Assets;

/// <summary>
/// Contains one immutable, source-free shader artifact ready for runtime GPU resource creation.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("43b40c10-c10b-5e97-b662-9bf9b4c9e34e")]
public sealed class RenderShaderArtifact
{
    private readonly byte[] m_definitionData;
    private readonly IReadOnlyList<RenderShaderPassArtifact> m_passes;

    /// <summary>
    /// Creates a complete deployed shader artifact.
    /// </summary>
    /// <param name="shaderName">
    /// The stable shader name expected by the runtime asset definition.
    /// </param>
    /// <param name="targetKey">
    /// The target compiler profile and policy identity.
    /// </param>
    /// <param name="variant">
    /// The canonical static keyword selection.
    /// </param>
    /// <param name="shaderInterface">
    /// The complete validated shader resource binding contract.
    /// </param>
    /// <param name="passes">
    /// Every source-free pass available to the runtime shader definition.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when a required identity is empty, no pass exists, or pass names are duplicated.
    /// </exception>
    /// <param name="definitionData">
    /// Native-serialized material, keyword, technique and pass contract captured with these programs; contains stable asset references only.
    /// </param>
    public RenderShaderArtifact(
        string shaderName,
        string targetKey,
        RenderShaderVariant variant,
        ShaderInterface shaderInterface,
        IReadOnlyList<RenderShaderPassArtifact> passes,
        ReadOnlySpan<byte> definitionData
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(shaderName);
        ArgumentException.ThrowIfNullOrWhiteSpace(targetKey);
        ArgumentNullException.ThrowIfNull(shaderInterface);
        ArgumentNullException.ThrowIfNull(passes);
        RenderShaderPassArtifact[] passSnapshot = passes.ToArray();
        if (passSnapshot.Length == 0)
            throw new ArgumentException("A deployed shader artifact must contain at least one pass.", nameof(passes));
        if (passSnapshot.Select(static pass => pass.name).Distinct(StringComparer.Ordinal).Count() != passSnapshot.Length)
            throw new ArgumentException("A deployed shader artifact cannot repeat a pass name.", nameof(passes));
        this.shaderName = shaderName;
        this.targetKey = targetKey;
        this.variant = variant;
        this.shaderInterface = RenderShaderPassArtifact.CloneInterface(shaderInterface);
        m_passes = Array.AsReadOnly(passSnapshot);
        if (definitionData.IsEmpty)
            throw new ArgumentException("A shader publication requires its captured runtime contract.", nameof(definitionData));
        m_definitionData = definitionData.ToArray();
        contentHash = Convert.ToHexString(SHA256.HashData(RenderShaderArtifactCodec.Encode(this)));
    }

    /// <summary>
    /// Gets the immutable native-serialized runtime contract paired with this exact program publication.
    /// </summary>
    public ReadOnlyMemory<byte> definitionData => m_definitionData;

    /// <summary>
    /// Gets a semantic content identity covering the contract, variant, target, bindings and all compiled stages.
    /// </summary>
    public string contentHash { get; }

    /// <summary>
    /// Gets the stable shader name expected by the runtime asset definition.
    /// </summary>
    public string shaderName { get; }

    /// <summary>
    /// Gets the target compiler profile and policy identity.
    /// </summary>
    public string targetKey { get; }

    /// <summary>
    /// Gets the canonical static keyword selection.
    /// </summary>
    public RenderShaderVariant variant { get; }

    /// <summary>
    /// Gets the complete validated shader resource binding contract.
    /// </summary>
    public ShaderInterface shaderInterface { get; }

    /// <summary>
    /// Gets every source-free pass available to the runtime shader definition.
    /// </summary>
    public IReadOnlyList<RenderShaderPassArtifact> passes => m_passes;
}

