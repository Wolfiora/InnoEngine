using System;
using System.Collections.Generic;

namespace Inno.UI;

/// <summary>
/// Builds one immutable backend-neutral frame while transferring ownership of update arrays.
/// </summary>
/// <remarks>
/// This backend SPI is intentionally not exported to ordinary game scripts.
/// </remarks>
public sealed class UIRenderFrameBuilder
{
    private readonly List<UIMeshUpdate> m_meshUpdates = [];
    private readonly List<UIMeshHandle> m_releasedMeshes = [];
    private readonly List<UITextureUpdate> m_textureUpdates = [];
    private readonly List<UITextureHandle> m_releasedTextures = [];
    private readonly List<UIDrawCommand> m_commands = [];
    private bool m_built;

    /// <summary>
    /// Adds one mesh update and takes exclusive ownership of both arrays.
    /// </summary>
    /// <param name="mesh">
    /// Assigned mesh handle.
    /// </param>
    /// <param name="revision">
    /// Positive content revision.
    /// </param>
    /// <param name="vertices">
    /// Owned vertex array; the caller must not mutate it afterward.
    /// </param>
    /// <param name="indices">
    /// Owned index array; the caller must not mutate it afterward.
    /// </param>
    public void AddMeshUpdate(
        UIMeshHandle mesh,
        ulong revision,
        UIVertex[] vertices,
        uint[] indices
    ) {
        EnsureMutable();
        if (!mesh.isValid)
        {
            throw new ArgumentException("A mesh update requires an assigned handle.", nameof(mesh));
        }
        ArgumentOutOfRangeException.ThrowIfZero(revision);
        ArgumentNullException.ThrowIfNull(vertices);
        ArgumentNullException.ThrowIfNull(indices);
        if (vertices.Length == 0 || indices.Length == 0)
        {
            throw new ArgumentException("A mesh update requires nonempty geometry.");
        }
        foreach (uint index in indices)
        {
            if (index >= vertices.Length)
            {
                throw new ArgumentException("A mesh index references a vertex outside the update.", nameof(indices));
            }
        }
        m_meshUpdates.Add(new UIMeshUpdate(mesh, revision, vertices, indices));
    }

    /// <summary>
    /// Adds one mesh retirement.
    /// </summary>
    /// <param name="mesh">
    /// Assigned mesh handle.
    /// </param>
    public void AddReleasedMesh(UIMeshHandle mesh)
    {
        EnsureMutable();
        if (!mesh.isValid)
        {
            throw new ArgumentException("A released mesh requires an assigned handle.", nameof(mesh));
        }
        m_releasedMeshes.Add(mesh);
    }

    /// <summary>
    /// Adds one texture update and takes exclusive ownership of its pixel array.
    /// </summary>
    /// <param name="texture">
    /// Assigned texture handle.
    /// </param>
    /// <param name="revision">
    /// Positive content revision.
    /// </param>
    /// <param name="width">
    /// Positive pixel width.
    /// </param>
    /// <param name="height">
    /// Positive pixel height.
    /// </param>
    /// <param name="pixels">
    /// Owned tightly packed premultiplied RGBA8 pixels.
    /// </param>
    public void AddTextureUpdate(
        UITextureHandle texture,
        ulong revision,
        int width,
        int height,
        byte[] pixels
    ) {
        EnsureMutable();
        if (!texture.isValid)
        {
            throw new ArgumentException("A texture update requires an assigned handle.", nameof(texture));
        }
        ArgumentOutOfRangeException.ThrowIfZero(revision);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);
        ArgumentNullException.ThrowIfNull(pixels);
        if (pixels.Length != checked(width * height * 4))
        {
            throw new ArgumentException("Texture pixels must be tightly packed RGBA8.", nameof(pixels));
        }
        m_textureUpdates.Add(new UITextureUpdate(texture, revision, width, height, pixels));
    }

    /// <summary>
    /// Adds one texture retirement.
    /// </summary>
    /// <param name="texture">
    /// Assigned texture handle.
    /// </param>
    public void AddReleasedTexture(UITextureHandle texture)
    {
        EnsureMutable();
        if (!texture.isValid)
        {
            throw new ArgumentException("A released texture requires an assigned handle.", nameof(texture));
        }
        m_releasedTextures.Add(texture);
    }

    /// <summary>
    /// Adds one ordered mesh draw.
    /// </summary>
    /// <param name="command">
    /// Draw command referencing an assigned mesh.
    /// </param>
    public void AddCommand(UIDrawCommand command)
    {
        EnsureMutable();
        if (!command.mesh.isValid)
        {
            throw new ArgumentException("A draw command requires an assigned mesh.", nameof(command));
        }
        m_commands.Add(command);
    }

    /// <summary>
    /// Freezes the accumulated frame. The builder cannot be reused.
    /// </summary>
    /// <returns>
    /// An immutable frame owning every supplied array.
    /// </returns>
    public UIRenderFrame Build()
    {
        EnsureMutable();
        m_built = true;
        return new(
            m_meshUpdates.ToArray(),
            m_releasedMeshes.ToArray(),
            m_textureUpdates.ToArray(),
            m_releasedTextures.ToArray(),
            m_commands.ToArray()
        );
    }

    private void EnsureMutable()
    {
        if (m_built)
        {
            throw new InvalidOperationException("A UI render-frame builder can only build one frame.");
        }
    }
}
