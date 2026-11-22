using System;
using Inno.Extensibility.Types;

namespace Inno.Rendering.Assets.Authoring;

/// <summary>
/// Compiles validated Shader IR through one target toolchain.
/// </summary>
[StableTypeID("85c9086c-63d5-51e6-82ec-548ad39702e2")]
public sealed partial class ShaderCompiler
{
    private readonly IShaderCompilerToolchain m_toolchain;

    /// <summary>
    /// Creates a common IR compiler with one backend-owned target toolchain.
    /// </summary>
    /// <param name="toolchain">
    /// Backend compiler implementation.
    /// </param>
    public ShaderCompiler(IShaderCompilerToolchain toolchain)
    {
        ArgumentNullException.ThrowIfNull(toolchain);
        m_toolchain = toolchain;
    }

    /// <summary>
    /// Creates a target supported by the configured backend compiler.
    /// </summary>
    /// <param name="capabilities">
    /// Target renderer capabilities.
    /// </param>
    /// <param name="optimize">
    /// Whether release optimization is enabled.
    /// </param>
    /// <param name="debugInformation">
    /// Whether compiler debug information is emitted.
    /// </param>
    /// <returns>
    /// A stable target description accepted by this compiler.
    /// </returns>
    public ShaderCompileTarget CreateTarget(
        GraphicsCapabilities capabilities,
        bool optimize = true,
        bool debugInformation = false
    ) => m_toolchain.CreateTarget(capabilities, optimize, debugInformation);
}
