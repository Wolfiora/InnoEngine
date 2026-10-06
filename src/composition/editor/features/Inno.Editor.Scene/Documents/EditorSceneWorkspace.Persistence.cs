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

internal sealed partial class EditorSceneWorkspace

{

    /// <summary>
    /// Gets whether a scene contains unsaved serialized changes.
    /// </summary>
    /// <param name="scene">
    /// Scene to inspect.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the scene has no source path or differs from its saved baseline.
    /// </returns>
    public bool IsDirty(GameScene scene)
    {
        ArgumentNullException.ThrowIfNull(scene);
        if (m_playModeSession is not null)
        {
            EnsurePresentedScene(scene);
            return false;
        }
        ApplyPendingSourceChanges();
        SceneDocument document = GetOrCreateDocument(scene);
        try
        {
            SynchronizeSource(scene, document);
            m_diagnostics.ResolveSynchronization(scene.identity.persistentId);
        }
        catch (Exception exception)
        {
            document.isDirty = true;
            if (m_diagnostics.PublishSynchronizationFailure(scene, exception))
                m_log.Write(LogLevel.Error, "Scene document synchronization failed: {0}", [exception]);
        }
        if (string.IsNullOrEmpty(document.sourcePath))
            return true;
        if (!string.Equals(scene.name, GetAssetName(document.sourcePath), StringComparison.Ordinal))
        {
            document.isDirty = true;
            return true;
        }

        long now = Stopwatch.GetTimestamp();
        if (now < document.nextRefreshTimestamp)
            return document.isDirty;

        document.nextRefreshTimestamp = now + (long)(Stopwatch.Frequency * C_DIRTY_REFRESH_SECONDS);
        try
        {
            document.isDirty = HasSerializedChanges(scene, document);
            m_diagnostics.ResolveDirtyCheck(scene.identity.persistentId);
        }
        catch (Exception exception)
        {
            document.isDirty = true;
            if (m_diagnostics.PublishDirtyCheckFailure(scene, exception))
                m_log.Write(LogLevel.Error, "Scene dirty check failed: {0}", [exception]);
        }
        return document.isDirty;
    }

    /// <summary>
    /// Saves a scene to its existing path or creates a scene asset in the requested directory.
    /// </summary>
    /// <param name="scene">
    /// Scene to save.
    /// </param>
    /// <param name="currentDirectory">
    /// Fallback asset directory for a new scene.
    /// </param>
    /// <returns>
    /// The saved source-relative path.
    /// </returns>
    public string Save(
        GameScene scene,
        string currentDirectory
    ) {
        ArgumentNullException.ThrowIfNull(scene);
        EnsureCanPersist();
        SceneDocument document = GetOrCreateDocument(scene);
        EnsureEditable(scene);
        string relativePath;
        if (string.IsNullOrEmpty(document.sourcePath))
        {
            relativePath = CreateUniquePath(currentDirectory, scene.name, C_SCENE_EXTENSION);
        }
        else
        {
            relativePath = RenameSceneSourceIfNeeded(scene, document);
        }
        SaveSceneAtPath(scene, relativePath);
        return relativePath;
    }

    /// <summary>
    /// Saves a scene as an asset in the requested directory and makes that path its document path.
    /// </summary>
    /// <param name="scene">
    /// Scene to save.
    /// </param>
    /// <param name="currentDirectory">
    /// Target asset directory.
    /// </param>
    /// <returns>
    /// The saved source-relative path.
    /// </returns>
    public string SaveToDirectory(
        GameScene scene,
        string currentDirectory
    ) {
        ArgumentNullException.ThrowIfNull(scene);
        EnsureCanPersist();
        SceneDocument document = GetOrCreateDocument(scene);
        EnsureEditable(scene);
        string currentPath = document.sourcePath;
        string currentParent = NormalizePath(Path.GetDirectoryName(currentPath));
        string targetDirectory = NormalizePath(currentDirectory);
        string relativePath;
        if (!string.IsNullOrEmpty(currentPath) &&
            string.Equals(currentParent, targetDirectory, StringComparison.Ordinal))
        {
            relativePath = RenameSceneSourceIfNeeded(scene, document);
        }
        else
        {
            relativePath = CreateUniquePath(targetDirectory, scene.name, C_SCENE_EXTENSION);
        }
        SaveSceneAtPath(scene, relativePath);
        return relativePath;
    }

    /// <summary>
    /// Captures a game object subtree as a prefab in the requested directory.
    /// </summary>
    /// <param name="gameObject">
    /// Prefab root.
    /// </param>
    /// <param name="currentDirectory">
    /// Target asset directory.
    /// </param>
    /// <returns>
    /// The saved source-relative path.
    /// </returns>
    public string SavePrefab(
        GameObject gameObject,
        string currentDirectory
    ) {
        ArgumentNullException.ThrowIfNull(gameObject);
        EnsureCanPersist();
        EnsureEditable(gameObject.scene);
        string relativePath = CreateUniquePath(currentDirectory, gameObject.name, C_PREFAB_EXTENSION);
        if (!m_assets.Save(
                AssetPath.Project(relativePath),
                PrefabAsset.Capture(gameObject, m_serialization, m_assets)))
            throw new InvalidOperationException($"No asset importer could save prefab '{relativePath}'.");
        return relativePath;
    }

    /// <summary>
    /// Opens a scene asset additively as the active editor scene.
    /// </summary>
    /// <param name="relativePath">
    /// Scene asset source-relative path.
    /// </param>
    /// <returns>
    /// The existing loaded instance or the newly loaded scene.
    /// </returns>
    public GameScene Open(string relativePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(relativePath);
        EnsureCanPersist();
        string normalizedPath = NormalizePath(relativePath);
        SceneDocument? existing = m_documents.Values.FirstOrDefault(document =>
            document.scene.isLoaded &&
            !document.scene.isDestroyed &&
            string.Equals(document.sourcePath, normalizedPath, StringComparison.OrdinalIgnoreCase));
        if (existing is not null)
        {
            m_runtimeSession.scenes.SetActiveScene(existing.scene);
            return existing.scene;
        }

        SceneAsset asset = m_assets.Load<SceneAsset>(AssetPath.Parse(normalizedPath));
        GameScene scene = asset.Instantiate(m_serialization, m_assets);
        scene.name = GetAssetName(normalizedPath);
        byte[] savedHash = ComputeSceneHash(scene);
        m_runtimeSession.scenes.LoadSceneAdditive(scene);
        m_documents.Add(
            scene.identity.persistentId,
            new SceneDocument(scene, normalizedPath, asset.identity.persistentId, savedHash));
        return scene;
    }

    /// <summary>
    /// Tries to get the current source-relative asset path of a saved scene.
    /// </summary>
    /// <param name="scene">
    /// Scene whose document path is requested.
    /// </param>
    /// <param name="relativePath">
    /// The saved source path when available.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the scene is backed by a scene asset.
    /// </returns>
    public bool TryGetSourcePath(
        GameScene scene,
        out string relativePath
    ) {
        ArgumentNullException.ThrowIfNull(scene);
        if (m_playModeSession is PlayModeLease playModeSession)
        {
            EnsurePresentedScene(scene);
            relativePath = playModeSession.TryGetSnapshot(
                scene.identity.persistentId,
                out SceneDocumentSnapshot snapshot)
                ? snapshot.sourcePath
                : string.Empty;
            return !string.IsNullOrEmpty(relativePath);
        }
        SceneDocument document = GetOrCreateDocument(scene);
        try
        {
            SynchronizeSource(scene, document);
        }
        catch
        {
            document.isDirty = true;
        }
        relativePath = document.sourcePath;
        return !string.IsNullOrEmpty(relativePath);
    }

    private void SaveSceneAtPath(
        GameScene scene,
        string relativePath
    ) {
        scene.name = GetAssetName(relativePath);
        bool exists = m_assets.TryLoad(AssetPath.Parse(relativePath), out SceneAsset? sceneAsset);
        sceneAsset ??= new SceneAsset();
        sceneAsset.CaptureFrom(scene, m_serialization, m_assets);
        bool saved = exists
            ? m_assets.Save(sceneAsset)
            : m_assets.Save(AssetPath.Parse(relativePath), sceneAsset);
        if (!saved)
            throw new InvalidOperationException($"No asset importer could save scene '{relativePath}'.");

        byte[] savedHash = ComputeSceneHash(scene);
        SceneDocument document = GetOrCreateDocument(scene);
        document.sourcePath = relativePath;
        document.sourceAssetId = sceneAsset.identity.persistentId;
        document.savedHash = savedHash;
        document.isDirty = false;
        document.nextRefreshTimestamp = Stopwatch.GetTimestamp() +
                                        (long)(Stopwatch.Frequency * C_DIRTY_REFRESH_SECONDS);
    }

    private SceneDocument GetOrCreateDocument(GameScene scene)
    {
        Guid sceneId = scene.identity.persistentId;
        if (m_documents.TryGetValue(sceneId, out SceneDocument? document))
        {
            document.scene = scene;
            return document;
        }
        document = new SceneDocument(scene, string.Empty, Guid.Empty, []);
        m_documents.Add(sceneId, document);
        return document;
    }

    private void EnsureCanPersist()
    {
        if (!canPersist)
        {
            throw new InvalidOperationException(
                "Scene and prefab persistence is unavailable while Play Mode runtime copies are active.");
        }
    }

    private byte[] ComputeSceneHash(GameScene scene)
    {
        var dependencies = new AssetDependencyCollection(includeLastKnownPaths: false);
        SerializationContext context = AssetSerializationContext.Create(m_assets, dependencies);
        byte[] payload = m_serialization.Serialize(scene, context);
        return SHA256.HashData(payload);
    }

    private bool HasSerializedChanges(
        GameScene scene,
        SceneDocument document
    )
        => !ComputeSceneHash(scene).AsSpan().SequenceEqual(document.savedHash);

    private string CreateUniquePath(
        string directory,
        string name,
        string extension
    ) {
        directory = NormalizePath(directory);
        string fileName = SanitizeFileName(name);
        string candidate = Combine(directory, fileName + extension);
        for (int suffix = 1; m_assets.TryGetFileSystemEntry(AssetPath.Parse(candidate), out _); suffix++)
            candidate = Combine(directory, $"{fileName} {suffix}{extension}");
        return candidate;
    }

    private static string CreateUniqueSceneName(IReadOnlyList<GameScene> scenes)
    {
        const string c_baseName = "Untitled Scene";
        var names = scenes.Select(static scene => scene.name).ToHashSet(StringComparer.Ordinal);
        if (!names.Contains(c_baseName))
            return c_baseName;
        for (int suffix = 1; ; suffix++)
        {
            string candidate = $"{c_baseName} {suffix}";
            if (!names.Contains(candidate))
                return candidate;
        }
    }

    private static string GetAssetName(string relativePath)
    {
        string name = Path.GetFileNameWithoutExtension(relativePath);
        return string.IsNullOrWhiteSpace(name) ? "Untitled" : name;
    }

    private static string SanitizeFileName(string value)
    {
        char[] invalid = Path.GetInvalidFileNameChars();
        string sanitized = new((value ?? string.Empty)
            .Trim()
            .Select(character => invalid.Contains(character) ? '_' : character)
            .ToArray());
        return string.IsNullOrWhiteSpace(sanitized) ? "Untitled" : sanitized;
    }

    private static string Combine(
        string directory,
        string fileName
    )
        => string.IsNullOrEmpty(directory) ? fileName : $"{directory}/{fileName}";

    private static string NormalizePath(string? path)
        => string.IsNullOrWhiteSpace(path)
            ? string.Empty
            : path.Replace('\\', '/').Trim('/');

    private sealed class SceneDocument(
        GameScene scene,
        string sourcePath,
        Guid sourceAssetId,
        byte[] savedHash
    ) {
        /// <summary>
        /// The scene value used as part of this type's public representation.
        /// </summary>
        public GameScene scene = scene;
        /// <summary>
        /// The source path value used as part of this type's public representation.
        /// </summary>
        public string sourcePath = sourcePath;
        /// <summary>
        /// The source asset id value used as part of this type's public representation.
        /// </summary>
        public Guid sourceAssetId = sourceAssetId;
        /// <summary>
        /// The saved hash value used as part of this type's public representation.
        /// </summary>
        public byte[] savedHash = savedHash;
        /// <summary>
        /// The is dirty value used as part of this type's public representation.
        /// </summary>
        public bool isDirty;
        /// <summary>
        /// The next refresh timestamp value used as part of this type's public representation.
        /// </summary>
        public long nextRefreshTimestamp;
    }

}
