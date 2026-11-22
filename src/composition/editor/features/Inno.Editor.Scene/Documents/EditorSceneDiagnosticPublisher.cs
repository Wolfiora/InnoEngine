using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Core.Diagnostics;
using Inno.Scene;

namespace Inno.Editor.Scene;

internal sealed class EditorSceneDiagnosticPublisher : IDisposable
{
    private const string C_RESTORE_GROUP = "Scene Workspace Restore";
    private const string C_SYNCHRONIZATION_GROUP = "Scene Synchronization";
    private const string C_DIRTY_CHECK_GROUP = "Scene Dirty Check";

    private readonly Dictionary<Guid, string> m_synchronizationStates = [];
    private readonly Dictionary<Guid, string> m_dirtyCheckStates = [];
    private string m_restoreState = string.Empty;

    /// <summary>
    /// Releases the resources owned by this instance.
    /// </summary>
    public void Dispose()
    {
        ResolveRestore();
        foreach (Guid sceneID in m_synchronizationStates.Keys.ToArray())
        {
            ResolveSynchronization(sceneID);
        }
        foreach (Guid sceneID in m_dirtyCheckStates.Keys.ToArray())
        {
            ResolveDirtyCheck(sceneID);
        }
    }

    internal bool PublishRestoreFailure(
        string code,
        string message
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        string state = $"{code}:{message}";
        if (string.Equals(m_restoreState, state, StringComparison.Ordinal))
        {
            return false;
        }
        Diagnostics.Set(C_RESTORE_GROUP, Diagnostic.Error(code, message));
        m_restoreState = state;
        return true;
    }

    internal void ResolveRestore()
    {
        if (string.IsNullOrEmpty(m_restoreState))
        {
            return;
        }
        Diagnostics.Clear(C_RESTORE_GROUP);
        m_restoreState = string.Empty;
    }

    internal bool PublishSynchronizationFailure(
        GameScene scene,
        Exception exception
    ) {
        ArgumentNullException.ThrowIfNull(scene);
        ArgumentNullException.ThrowIfNull(exception);
        Guid targetID = scene.identity.persistentID;
        string state = exception.ToString();
        if (m_synchronizationStates.TryGetValue(targetID, out string? previous)
            && string.Equals(previous, state, StringComparison.Ordinal)
        ) {
            return false;
        }
        Diagnostics.Set(
            targetID,
            C_SYNCHRONIZATION_GROUP,
            Diagnostic.Error("SCENE-SYNCHRONIZATION", exception.Message),
            scene.name
        );
        m_synchronizationStates[targetID] = state;
        return true;
    }

    internal void PublishSourceUnavailable(
        GameScene scene,
        string sourcePath
    ) {
        Guid sceneID = scene.identity.persistentID;
        string state = "SCENE-SOURCE-MISSING:" + sourcePath;
        if (m_synchronizationStates.TryGetValue(sceneID, out string? previous)
            && string.Equals(previous, state, StringComparison.Ordinal)
        ) {
            return;
        }
        Diagnostics.Set(
            sceneID,
            C_SYNCHRONIZATION_GROUP,
            Diagnostic.Warning(
                "SCENE-SOURCE-MISSING",
                $"Scene source '{sourcePath}' is unavailable; its assignment and current edits are preserved."
            ),
            scene.name
        );
        m_synchronizationStates[sceneID] = state;
    }

    internal void ResolveSynchronization(Guid sceneID)
    {
        if (!m_synchronizationStates.Remove(sceneID))
        {
            return;
        }
        Diagnostics.Clear(sceneID, C_SYNCHRONIZATION_GROUP);
    }

    internal bool PublishDirtyCheckFailure(
        GameScene scene,
        Exception exception
    ) {
        ArgumentNullException.ThrowIfNull(scene);
        ArgumentNullException.ThrowIfNull(exception);
        Guid targetID = scene.identity.persistentID;
        string state = exception.ToString();
        if (m_dirtyCheckStates.TryGetValue(targetID, out string? previous)
            && string.Equals(previous, state, StringComparison.Ordinal)
        ) {
            return false;
        }
        Diagnostics.Set(
            targetID,
            C_DIRTY_CHECK_GROUP,
            Diagnostic.Error("SCENE-DIRTY-CHECK", exception.Message),
            scene.name
        );
        m_dirtyCheckStates[targetID] = state;
        return true;
    }

    internal void ResolveDirtyCheck(Guid sceneID)
    {
        if (!m_dirtyCheckStates.Remove(sceneID))
        {
            return;
        }
        Diagnostics.Clear(sceneID, C_DIRTY_CHECK_GROUP);
    }

    internal void RetainSynchronizationTargets(IReadOnlySet<Guid> sceneIDs)
    {
        ArgumentNullException.ThrowIfNull(sceneIDs);
        Guid[] removed = m_synchronizationStates.Keys
            .Where(id => !sceneIDs.Contains(id))
            .ToArray();
        for (int i = 0; i < removed.Length; i++)
        {
            ResolveSynchronization(removed[i]);
        }
        Guid[] removedDirtyChecks = m_dirtyCheckStates.Keys
            .Where(id => !sceneIDs.Contains(id))
            .ToArray();
        for (int i = 0; i < removedDirtyChecks.Length; i++)
        {
            ResolveDirtyCheck(removedDirtyChecks[i]);
        }
    }
}
