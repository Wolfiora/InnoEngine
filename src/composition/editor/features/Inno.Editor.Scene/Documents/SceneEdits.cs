using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Linq;

using Inno.Extensibility.Types;
using Inno.Core.Serialization;
using Inno.Editor.Core;
using Inno.Editor.Interactions;
using Inno.Scene;
using Inno.Scene.Components;
using Inno.Scene.Layers;

namespace Inno.Editor.Scene;

/// <summary>
/// Applies scene-document mutations and records compact, reload-safe inverse data in editor history.
/// </summary>
[EditorModule("scene-edits", order: 210)]
public sealed partial class SceneEdits : EditorModule
{
    private readonly EditorSceneWorkspace m_workspace;
    private readonly EditorInteractions m_interactions;

    /// <summary>
    /// Creates the scene editing service used by editor actions and drag handlers.
    /// </summary>
    /// <param name="workspace">
    /// The current scene document workspace.
    /// </param>
    /// <param name="interactions">
    /// The current editor interaction runtime.
    /// </param>
    internal SceneEdits(
        EditorSceneWorkspace workspace,
        EditorInteractions interactions
    ) {
        m_workspace = workspace ?? throw new ArgumentNullException(nameof(workspace));
        m_interactions = interactions ?? throw new ArgumentNullException(nameof(interactions));
    }

    /// <summary>
    /// Gets whether the scene is editable in the current Edit or isolated Play world.
    /// </summary>
    /// <param name="scene">
    /// The loaded scene to inspect.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when scene commands may change this scene. Play changes use
    /// a temporary history branch and are discarded when the Play session ends.
    /// </returns>
    public bool CanEdit(GameScene scene) => m_workspace.CanEdit(scene);

    /// <summary>
    /// Gets whether a scene object belongs to an editable presented scene.
    /// </summary>
    /// <param name="target">
    /// The scene object to inspect.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when its owning scene is writable.
    /// </returns>
    public bool CanEdit(EngineObject target)
    {
        ArgumentNullException.ThrowIfNull(target);
        if (target.isDestroyed)
            return false;
        try
        {
            return m_workspace.CanEdit(ResolveOwnerScene(target));
        }
        catch (InvalidOperationException)
        {
            return false;
        }
    }

    /// <summary>
    /// Creates an additive scene and records a reversible document change.
    /// </summary>
    /// <param name="historyName">
    /// The user-facing history entry name.
    /// </param>
    /// <returns>
    /// The newly created active scene.
    /// </returns>
    public GameScene CreateScene(string historyName = "Create Scene")
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(historyName);
        using IDisposable presentationScope = m_workspace.EnterPresentationScope();
        Guid? activeBefore = GetActiveSceneId();
        Guid? selectedBefore = GetSelectionId();
        GameScene scene = m_workspace.CreateScene();
        RecordWithRollback(
            () =>
            {
                EditorSceneWorkspace.SceneDocumentSnapshot snapshot =
                    m_workspace.CaptureDocumentSnapshot(scene);
                RecordDocument(
                    historyName,
                    existsBefore: false,
                    existsAfter: true,
                    snapshot,
                    activeBefore,
                    GetActiveSceneId(),
                    selectedBefore,
                    scene.identity.persistentId);
            },
            () =>
            {
                if (!m_workspace.CloseScene(scene))
                    throw new InvalidOperationException("The unrecorded scene could not be removed.");
                m_workspace.RestoreActiveScene(activeBefore);
            });
        return scene;
    }

    /// <summary>
    /// Closes one loaded scene without deleting its source asset and records a reversible document change.
    /// </summary>
    /// <param name="scene">
    /// The loaded scene to close.
    /// </param>
    /// <param name="historyName">
    /// The user-facing history entry name.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the scene was closed and recorded.
    /// </returns>
    public bool CloseScene(
        GameScene scene,
        string historyName = "Close Scene"
    ) {
        ArgumentNullException.ThrowIfNull(scene);
        ArgumentException.ThrowIfNullOrWhiteSpace(historyName);
        using IDisposable presentationScope = m_workspace.EnterPresentationScope();
        EditorSceneWorkspace.SceneDocumentSnapshot snapshot = m_workspace.CaptureDocumentSnapshot(scene);
        Guid? activeBefore = GetActiveSceneId();
        Guid? selectedBefore = GetSelectionId();
        if (!m_workspace.CloseScene(scene))
            return false;
        RecordWithRollback(
            () => RecordDocument(
                historyName,
                existsBefore: true,
                existsAfter: false,
                snapshot,
                activeBefore,
                GetActiveSceneId(),
                selectedBefore,
                GetSelectionId()),
            () =>
            {
                _ = m_workspace.RestoreDocumentSnapshot(snapshot);
                m_workspace.RestoreActiveScene(activeBefore);
            });
        return true;
    }
}
