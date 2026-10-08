using System;
using Inno.Adapter.Rendering;
using Inno.Rendering.Assets;
using Inno.Rendering.Assets.Authoring;

namespace Inno.Build.Toolchains.Bgfx.Tools;

/// <summary>
/// Registers the BGFX authoring compiler and texture target compiler.
/// </summary>
public sealed class BgfxAuthoringProvider : RenderingAuthoringBackendProvider
{
    private readonly BgfxShaderTargetProfile m_target;
    private readonly ToolRunner? m_tools;

    /// <summary>
    /// Registers the bundled rendering authoring tools without initializing a compiler.
    /// </summary>
    /// <param name="target">
    /// The product's explicit preview shader target.
    /// </param>
    /// <param name="tools">
    /// Frozen executable tools, or null for the product's own deployed tools.
    /// </param>
    public BgfxAuthoringProvider(
        BgfxShaderTargetProfile target,
        ToolRunner? tools = null
    ) : base(RenderingBackendId.bgfx) {
        ArgumentNullException.ThrowIfNull(target);
        m_target = target;
        m_tools = tools;
    }

    /// <inheritdoc />
    public override IShaderCompilerToolchain CreateShaderCompilerToolchain() => new BgfxShadercToolchain(m_target, m_tools);

    /// <inheritdoc />
    public override ITextureTargetCompiler CreateTextureTargetCompiler() => new BgfxTextureTargetCompiler();
}
