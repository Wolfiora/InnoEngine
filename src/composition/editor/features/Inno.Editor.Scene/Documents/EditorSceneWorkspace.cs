using Inno.Extensibility.Reload;
using System;
using Inno.References;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Extensibility.Modules;
using Inno.Core.Identity;
using Inno.Core.Logging;
using Inno.Extensibility.Types;
using Inno.Core.Serialization;
using Inno.Editor.Core;
using Inno.Editor.Interactions;
using Inno.Runtime;
using Inno.Scene;
using Inno.Scene.Components;

namespace Inno.Editor.Scene;

/// <summary>
/// Tracks editor scene documents, their source paths, and serialized dirty state.
/// </summary>
[EditorModule("scene-workspace", order: 200)]
internal sealed partial class EditorSceneWorkspace :
    EditorModule,
    IEditorGameScenePresentation,
    IEditorSceneWorkspace,
    IEditorScenePlayMode,
    IEditorReloadParticipant
{
    private const double C_DIRTY_REFRESH_SECONDS = 0.1;
    private const string C_SCENE_EXTENSION = ".iscene";
    private const string C_PREFAB_EXTENSION = ".iprefab";

    private readonly Dictionary<Guid, SceneDocument> m_documents = [];
    private readonly ConcurrentQueue<AssetChange> m_sourceChanges = new();
    private readonly AssetPipeline m_assets;
    private readonly EditorSceneDiagnosticPublisher m_diagnostics = new();
    private readonly EditorReloadCoordinator m_reloads;
    private readonly Logger m_log;
    private readonly RuntimeSession m_runtimeSession;
    private readonly SceneStateDiagnosticTracker m_sceneStateDiagnostics;
    private readonly SerializationRegistry m_serialization;
    private readonly IEditorSelectionCoordinator? m_selection;
    private readonly TypeCatalog m_types;

    private bool m_isAttached;
    private bool m_isPreparingPlayMode;
    private PlayModeLease? m_playModeSession;
    private IDisposable? m_reloadIntegration;
    private IDisposable? m_reloadRegistration;
    private string[]? m_pendingScenePaths;
    private string m_pendingActivePath = string.Empty;
    private long m_nextRestoreAttemptTimestamp;
    private long m_waitingTypeCatalogVersion = -1;

    /// <summary>
    /// Creates a scene workspace and optionally enables editor selection coordination.
    /// </summary>
    /// <param name="runtimeSession">
    /// The Edit session whose runtime-owned coroutines are retired during script generation changes.
    /// </param>
    /// <param name="assets">
    /// The authoring asset pipeline that owns scene and prefab documents.
    /// </param>
    /// <param name="types">
    /// The host-owned type catalog that resolves scene element generations.
    /// </param>
    /// <param name="serialization">
    /// The serialization registry used for scene snapshots and history state.
    /// </param>
    /// <param name="reloads">
    /// The host-owned coordinator for atomic editor generation transitions.
    /// </param>
    /// <param name="logs">
    /// The application log router used for scene workspace diagnostics.
    /// </param>
    /// <param name="selection">
    /// The active editor selection coordinator, or <see langword="null"/> when selection is not hosted.
    /// </param>
    internal EditorSceneWorkspace(
        RuntimeSession runtimeSession,
        AssetPipeline assets,
        TypeCatalog types,
        SerializationRegistry serialization,
        EditorReloadCoordinator reloads,
        LogRouter logs,
        IEditorSelectionCoordinator? selection = null
    ) {
        m_runtimeSession = runtimeSession ?? throw new ArgumentNullException(nameof(runtimeSession));
        m_assets = assets ?? throw new ArgumentNullException(nameof(assets));
        m_types = types ?? throw new ArgumentNullException(nameof(types));
        m_serialization = serialization ?? throw new ArgumentNullException(nameof(serialization));
        m_reloads = reloads ?? throw new ArgumentNullException(nameof(reloads));
        ArgumentNullException.ThrowIfNull(logs);
        m_log = logs.CreateLogger<EditorSceneWorkspace>();
        m_selection = selection;
        m_sceneStateDiagnostics = new SceneStateDiagnosticTracker(runtimeSession.scenes, types);
    }

    internal SceneWorld world => m_playModeSession?.runtimeWorld ?? m_runtimeSession.scenes;

    internal IDisposable EnterPresentationScope() => (m_playModeSession?.runtimeSession ?? m_runtimeSession).EnterExecutionScope();

    internal SerializationRegistry serialization => m_serialization;

    internal IAssetReferenceResolver assets => m_assets;

    internal TypeCatalog types => m_types;

    internal TSceneObject? Find<TSceneObject>(Guid persistentId)
        where TSceneObject : IdentityObject
        => world.Find<TSceneObject>(persistentId);

    /// <summary>
    /// Gets all scenes currently available to editor features.
    /// </summary>
    public IReadOnlyList<GameScene> scenes => world.loadedScenes;

    /// <summary>
    /// Gets the active scene, or <see langword="null"/> when the workspace contains no scenes.
    /// </summary>
    public GameScene? activeScene => world.activeScene;

    /// <summary>
    /// Gets whether the loaded scenes represent editable documents that may be persisted.
    /// </summary>
    public bool canPersist => !m_isPreparingPlayMode && m_playModeSession is null;

    /// <summary>
    /// Checks whether the presented scene can be changed without writing to a read-only source.
    /// </summary>
    /// <param name="scene">
    /// The scene consumed by can edit; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <returns>
    /// <see langword="true"/> for a Project scene or a transient scene in the current Edit or
    /// isolated Play world. Play changes remain in the runtime copy and are discarded on stop.
    /// </returns>
    public bool CanEdit(GameScene scene)
    {
        ArgumentNullException.ThrowIfNull(scene);
        if (m_isPreparingPlayMode || scene.isDestroyed || !scene.isLoaded ||
            !world.loadedScenes.Any(candidate => ReferenceEquals(candidate, scene)))
            return false;
        if (m_playModeSession is PlayModeLease playModeSession)
        {
            if (!playModeSession.TryGetSnapshot(scene.identity.persistentId, out SceneDocumentSnapshot snapshot))
                return true;
            return string.IsNullOrEmpty(snapshot.sourcePath)
                || AssetPath.Parse(snapshot.sourcePath).source == AssetSourceId.project;
        }
        return !m_documents.TryGetValue(scene.identity.persistentId, out SceneDocument? document)
            || string.IsNullOrEmpty(document.sourcePath)
            || AssetPath.Parse(document.sourcePath).source == AssetSourceId.project;
    }

    /// <summary>
    /// Captures the scene set that represents the game for the current Editor frame.
    /// </summary>
    /// <returns>
    /// A coherent snapshot of the Edit world before Play commits, the isolated Play world while it is
    /// active, or the Edit world again after the Play lease has been released.
    /// </returns>
    public ContentReadScope Capture()
    {
        SceneWorld presentedWorld = m_playModeSession?.runtimeWorld ?? m_runtimeSession.scenes;
        GameScene[] scenes = presentedWorld.loadedScenes
            .Where(static scene => !scene.isDestroyed)
            .ToArray();
        GameScene? activeScene = presentedWorld.activeScene;
        if (activeScene is { isDestroyed: true } ||
            activeScene is not null && !scenes.Contains(activeScene))
        {
            activeScene = null;
        }
        return SceneContentSource.CreateScope(scenes, activeScene);
    }

    /// <summary>
    /// Makes one loaded scene the active editor document without changing scene order.
    /// </summary>
    /// <param name="scene">
    /// The loaded scene to activate.
    /// </param>
    public void SetActiveScene(GameScene scene)
    {
        ArgumentNullException.ThrowIfNull(scene);
        using IDisposable scope = EnterPresentationScope();
        world.SetActiveScene(scene);
    }

    internal void DisposeUnattached()
    {
        m_playModeSession?.Dispose();
        ((IDisposable)this).Dispose();
    }

    /// <summary>
    /// Creates and loads a uniquely named unsaved scene alongside the currently loaded scenes.
    /// </summary>
    /// <returns>
    /// The newly created active scene.
    /// </returns>
    internal GameScene CreateScene()
    {
        using IDisposable scope = EnterPresentationScope();
        string name = CreateUniqueSceneName(world.loadedScenes);
        GameScene scene = world.LoadNewSceneAdditive(name);
        if (canPersist)
        {
            m_documents.Add(
                scene.identity.persistentId,
                new SceneDocument(scene, string.Empty, Guid.Empty, []));
        }
        return scene;
    }

    /// <summary>
    /// Applies queued asset path changes to loaded scene documents and prefab instances.
    /// This method must be called from the editor main thread.
    /// </summary>
    internal void Refresh()
    {
        SynchronizeReplacedScenes();
        ApplyPendingSourceChanges();

        var synchronizedScenes = new HashSet<Guid>();
        foreach (SceneDocument document in m_documents.Values)
        {
            if (document.scene.isDestroyed)
                continue;
            Guid sceneId = document.scene.identity.persistentId;
            synchronizedScenes.Add(sceneId);
            try
            {
                SynchronizeSource(document.scene, document);
                m_diagnostics.ResolveSynchronization(sceneId);
            }
            catch (Exception exception)
            {
                document.isDirty = true;
                if (m_diagnostics.PublishSynchronizationFailure(document.scene, exception))
                    m_log.Write(LogLevel.Error, "Scene document synchronization failed: {0}", [exception]);
            }
        }
        m_diagnostics.RetainSynchronizationTargets(synchronizedScenes);
        m_sceneStateDiagnostics.Reconcile();
    }

    /// <summary>
    /// Closes a loaded scene and removes its editor document state without deleting its source asset.
    /// </summary>
    /// <param name="scene">
    /// Loaded scene to close.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the scene was loaded and closed.
    /// </returns>
    internal bool CloseScene(GameScene scene)
    {
        ArgumentNullException.ThrowIfNull(scene);
        if (scene.isDestroyed)
            return false;
        using IDisposable scope = EnterPresentationScope();
        Guid sceneId = scene.identity.persistentId;
        bool closed = world.UnloadScene(scene);
        if (closed && canPersist)
        {
            m_documents.Remove(sceneId);
            m_diagnostics.ResolveSynchronization(sceneId);
            m_diagnostics.ResolveDirtyCheck(sceneId);
        }
        return closed;
    }

    /// <summary>
    /// Removes all tracked document state.
    /// </summary>
    internal void Clear()
    {
        m_documents.Clear();
        m_diagnostics.RetainSynchronizationTargets(new HashSet<Guid>());
        while (m_sourceChanges.TryDequeue(out _))
        {
        }
    }

    /// <summary>
    /// Attaches the workspace to Asset Database changes and ensures an editable scene exists.
    /// </summary>
    /// <param name="context">
    /// The shared editor context for the active runtime.
    /// </param>
    protected override void OnStart(EditorContext context)
    {
        if (m_isAttached)
            return;
        m_assets.Changed += OnAssetDatabaseChanged;
        m_reloadIntegration = SceneReloadIntegration.Acquire(
            m_runtimeSession,
            m_serialization,
            m_assets,
            m_reloads);
        m_reloadRegistration = m_reloads.Register(this);
        m_isAttached = true;
        m_sceneStateDiagnostics.Reconcile(force: true);
    }

    /// <summary>
    /// Refreshes source synchronization for loaded editor documents.
    /// </summary>
    /// <param name="context">
    /// The shared editor context containing current frame state.
    /// </param>
    protected override void OnUpdate(EditorContext context)
    {
        if (m_playModeSession is not null)
        {
            m_sceneStateDiagnostics.Reconcile();
            return;
        }
        TryRestorePendingScenes();
        Refresh();
    }

    /// <summary>
    /// Detaches the workspace from Asset Database changes and releases any scene it created for the editor.
    /// </summary>
    /// <param name="context">
    /// The shared editor context for the runtime being stopped.
    /// </param>
    protected override void OnStop(EditorContext context)
    {
        if (!m_isAttached)
            return;
        try
        {
            m_playModeSession?.Dispose();
        }
        finally
        {
            m_assets.Changed -= OnAssetDatabaseChanged;
            m_reloadRegistration?.Dispose();
            m_reloadRegistration = null;
            m_runtimeSession.scenes.UnloadAllScenes();
            m_sceneStateDiagnostics.Reconcile(force: true);
            m_reloadIntegration?.Dispose();
            m_reloadIntegration = null;
            m_isAttached = false;
            Clear();
        }
    }

    /// <summary>
    /// Releases resources retained by this feature after it has stopped.
    /// </summary>
    protected override void OnDispose()
    {
        m_reloadRegistration?.Dispose();
        m_reloadRegistration = null;
        m_reloadIntegration?.Dispose();
        m_reloadIntegration = null;
        m_diagnostics.Dispose();
    }

    void IEditorReloadParticipant.RefreshDiagnostics()
        => m_sceneStateDiagnostics.Reconcile(force: true);

    internal void EnsureEditable(GameScene scene)
    {
        if (!CanEdit(scene))
            throw new InvalidOperationException(
                "The scene is not editable in the current presentation or belongs to a read-only source.");
    }

    private void EnsurePresentedScene(GameScene scene)
    {
        IReadOnlyList<GameScene> presentedScenes = world.loadedScenes;
        for (int i = 0; i < presentedScenes.Count; i++)
        {
            if (ReferenceEquals(presentedScenes[i], scene))
                return;
        }
        throw new InvalidOperationException(
            "The scene is not owned by the Editor's active Edit or Play presentation session.");
    }
}
