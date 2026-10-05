using System;
using Inno.Rendering.Assets;

namespace Inno.Adapter.Rendering;

/// <summary>
/// Supplies authoring tools paired with one runtime rendering implementation.
/// </summary>
public abstract class RenderingAuthoringBackendProvider
{
    /// <summary>
    /// Captures the registration identity assigned by the composition owner.
    /// </summary>
    /// <param name="id">
    /// The assigned implementation identity.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The identity is unassigned.
    /// </exception>
    protected RenderingAuthoringBackendProvider(RenderingBackendId id)
    {
        if (!id.isValid)
            throw new ArgumentException("A provider requires an assigned backend ID.", nameof(id));
        this.id = id;
    }

    /// <summary>
    /// Gets the stable runtime implementation identity served by these tools.
    /// </summary>
    public RenderingBackendId id { get; }

    /// <summary>
    /// Creates the shader toolchain for this implementation.
    /// </summary>
    /// <returns>
    /// A compiler compatible with devices registered under the same identity.
    /// </returns>
    public abstract IShaderCompilerToolchain CreateShaderCompilerToolchain();

    /// <summary>
    /// Creates the texture compiler for this implementation.
    /// </summary>
    /// <returns>
    /// A compiler compatible with devices registered under the same identity.
    /// </returns>
    public abstract ITextureTargetCompiler CreateTextureTargetCompiler();
}
