using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Inno.UI;

/// <summary>
/// Contains incremental resource changes and ordered draw commands for one UI context.
/// </summary>
public sealed class UIRenderFrame
{
    private readonly IReadOnlyList<UIMeshUpdate> m_meshUpdates;
    private readonly IReadOnlyList<UIMeshHandle> m_releasedMeshes;
    private readonly IReadOnlyList<UITextureUpdate> m_textureUpdates;
    private readonly IReadOnlyList<UITextureHandle> m_releasedTextures;
    private readonly IReadOnlyList<UIDrawCommand> m_commands;

    internal UIRenderFrame(
        UIMeshUpdate[] meshUpdates,
        UIMeshHandle[] releasedMeshes,
        UITextureUpdate[] textureUpdates,
        UITextureHandle[] releasedTextures,
        UIDrawCommand[] commands
    ) {
        m_meshUpdates = new ReadOnlyCollection<UIMeshUpdate>(meshUpdates);
        m_releasedMeshes = new ReadOnlyCollection<UIMeshHandle>(releasedMeshes);
        m_textureUpdates = new ReadOnlyCollection<UITextureUpdate>(textureUpdates);
        m_releasedTextures = new ReadOnlyCollection<UITextureHandle>(releasedTextures);
        m_commands = new ReadOnlyCollection<UIDrawCommand>(commands);
    }

    /// <summary>
    /// Gets an empty resource-delta frame.
    /// </summary>
    public static UIRenderFrame empty { get; } = new([], [], [], [], []);

    /// <summary>
    /// Gets created or replaced mesh generations.
    /// </summary>
    public IReadOnlyList<UIMeshUpdate> meshUpdates => m_meshUpdates;

    /// <summary>
    /// Gets mesh generations that must be retired.
    /// </summary>
    public IReadOnlyList<UIMeshHandle> releasedMeshes => m_releasedMeshes;

    /// <summary>
    /// Gets created or replaced texture generations.
    /// </summary>
    public IReadOnlyList<UITextureUpdate> textureUpdates => m_textureUpdates;

    /// <summary>
    /// Gets texture generations that must be retired.
    /// </summary>
    public IReadOnlyList<UITextureHandle> releasedTextures => m_releasedTextures;

    /// <summary>
    /// Gets ordered draws referencing published resources.
    /// </summary>
    public IReadOnlyList<UIDrawCommand> commands => m_commands;

    /// <summary>
    /// Creates a command-free frame that deterministically retires plugin-owned resources.
    /// </summary>
    /// <param name="meshes">
    /// Assigned mesh handles to retire.
    /// </param>
    /// <param name="textures">
    /// Assigned texture handles to retire.
    /// </param>
    /// <returns>
    /// An immutable retirement frame.
    /// </returns>
    public static UIRenderFrame CreateRetirement(
        UIMeshHandle[] meshes,
        UITextureHandle[] textures
    ) {
        ArgumentNullException.ThrowIfNull(meshes);
        ArgumentNullException.ThrowIfNull(textures);
        if (Array.Exists(meshes, static value => !value.isValid))
        {
            throw new ArgumentException("Retirement meshes must be assigned.", nameof(meshes));
        }
        if (Array.Exists(textures, static value => !value.isValid))
        {
            throw new ArgumentException("Retirement textures must be assigned.", nameof(textures));
        }
        return new([], (UIMeshHandle[])meshes.Clone(), [], (UITextureHandle[])textures.Clone(), []);
    }
}
