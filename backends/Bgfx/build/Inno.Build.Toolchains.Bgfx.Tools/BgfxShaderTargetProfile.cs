using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Inno.Rendering;

namespace Inno.Build.Toolchains.Bgfx.Tools;

/// <summary>
/// Binds an open target identity to immutable renderer profiles without a backend-owned platform list.
/// </summary>
public sealed class BgfxShaderTargetProfile
{
    private readonly IReadOnlyDictionary<GraphicsApi, BgfxShaderCompilerProfile> m_renderers;

    /// <summary>
    /// Freezes the renderer configuration contributed by the platform.
    /// </summary>
    /// <param name="id">
    /// The open stable target identity.
    /// </param>
    /// <param name="renderers">
    /// One immutable profile per supported graphics API.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The identity, renderer collection or a renderer entry is empty or duplicated.
    /// </exception>
    public BgfxShaderTargetProfile(
        string id,
        IEnumerable<BgfxShaderCompilerProfile> renderers
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentNullException.ThrowIfNull(renderers);
        var snapshot = new Dictionary<GraphicsApi, BgfxShaderCompilerProfile>();
        foreach (BgfxShaderCompilerProfile renderer in renderers)
            if (renderer is null || !snapshot.TryAdd(renderer.capabilities.backend, renderer))
                throw new ArgumentException("Shader renderers must be non-null and unique.", nameof(renderers));
        if (snapshot.Count == 0)
            throw new ArgumentException("A shader target requires at least one renderer.", nameof(renderers));
        this.id = id;
        m_renderers = new ReadOnlyDictionary<GraphicsApi, BgfxShaderCompilerProfile>(snapshot);
    }

    /// <summary>
    /// Gets the explicit stable target identity.
    /// </summary>
    public string id { get; }

    /// <summary>
    /// Resolves renderer configuration without choosing a platform or executing tools.
    /// </summary>
    /// <param name="backend">
    /// The explicit graphics API.
    /// </param>
    /// <returns>
    /// The borrowed immutable profile; unsupported APIs fail explicitly.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// The target has no configuration for this API.
    /// </exception>
    public BgfxShaderCompilerProfile Resolve(GraphicsApi backend)
        => m_renderers.TryGetValue(backend, out BgfxShaderCompilerProfile? renderer) ? renderer
            : throw new NotSupportedException($"Shader target '{id}' has no renderer profile for '{backend}'.");

    internal BgfxShaderCompilerProfile Resolve(GraphicsCapabilities capabilities)
    {
        ArgumentNullException.ThrowIfNull(capabilities);
        BgfxShaderCompilerProfile renderer = Resolve(capabilities.backend);
        if (capabilities.Supports(GraphicsCapability.Compute) && renderer.computeProfile.Length == 0)
            throw new NotSupportedException($"Shader target '{id}' does not support compute.");
        return renderer;
    }

    internal string GetKey(GraphicsCapabilities capabilities)
        => "bgfx-shaderc:" + id + ":" + Resolve(capabilities).CreateKey(capabilities);
}
