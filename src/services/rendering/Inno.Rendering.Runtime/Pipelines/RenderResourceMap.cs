using Inno.Core.Diagnostics;
using Inno.Core.Execution;
using Inno.Core.Serialization;
using Inno.Rendering;
using Inno.Rendering.Assets;
using System;
using System.Collections.Generic;

namespace Inno.Rendering.Runtime;

/// <summary>
/// Stores open semantic graph resources for one pipeline request.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("f48ab8a3-a1f2-513a-9e4b-396be0f084a8")]
public sealed class RenderResourceMap
{
    private readonly Dictionary<RenderResourceId, RenderTextureHandle> m_textures = [];
    private readonly Dictionary<RenderResourceId, RenderBufferHandle> m_buffers = [];

    /// <summary>
    /// Publishes a texture under a pipeline-defined semantic identifier.
    /// </summary>
    /// <param name="id">
    /// Open semantic resource identifier.
    /// </param>
    /// <param name="texture">
    /// Valid current-graph texture.
    /// </param>
    public void PublishTexture(
        RenderResourceId id,
        RenderTextureHandle texture
    ) {
        if (!id.isValid)
            throw new ArgumentException("A render resource identifier must be valid.", nameof(id));
        if (!texture.isValid)
            throw new ArgumentException("A published texture must be valid.", nameof(texture));
        m_textures[id] = texture;
    }

    /// <summary>
    /// Publishes a buffer under a pipeline-defined semantic identifier.
    /// </summary>
    /// <param name="id">
    /// Open semantic resource identifier.
    /// </param>
    /// <param name="buffer">
    /// Valid current-graph buffer.
    /// </param>
    public void PublishBuffer(
        RenderResourceId id,
        RenderBufferHandle buffer
    ) {
        if (!id.isValid)
            throw new ArgumentException("A render resource identifier must be valid.", nameof(id));
        if (!buffer.isValid)
            throw new ArgumentException("A published buffer must be valid.", nameof(buffer));
        m_buffers[id] = buffer;
    }

    /// <summary>
    /// Tries to get a published texture.
    /// </summary>
    /// <param name="id">
    /// Open semantic resource identifier.
    /// </param>
    /// <param name="texture">
    /// Receives the current-graph texture.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the texture has been published.
    /// </returns>
    public bool TryGetTexture(
        RenderResourceId id,
        out RenderTextureHandle texture
    ) => m_textures.TryGetValue(id, out texture);

    /// <summary>
    /// Tries to get a published buffer.
    /// </summary>
    /// <param name="id">
    /// Open semantic resource identifier.
    /// </param>
    /// <param name="buffer">
    /// Receives the current-graph buffer.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the buffer has been published.
    /// </returns>
    public bool TryGetBuffer(
        RenderResourceId id,
        out RenderBufferHandle buffer
    ) => m_buffers.TryGetValue(id, out buffer);
}

