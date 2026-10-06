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

    IGenerationChange IEditorReloadParticipant.Capture(AssemblyReloadContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        if (m_playModeSession is not null)
        {
            return new WorkspaceReloadTransaction(
                this,
                Array.Empty<ReloadDocumentState>(),
                preserveDocumentBaselines: true);
        }
        ApplyPendingSourceChanges();
        var documents = new List<ReloadDocumentState>(m_documents.Count);
        foreach ((Guid sceneId, SceneDocument document) in m_documents)
        {
            GameScene scene = document.scene;
            if (scene.isDestroyed)
                continue;
            SynchronizeSource(scene, document);
            bool wasDirty = string.IsNullOrEmpty(document.sourcePath) ||
                            !string.Equals(
                                scene.name,
                                GetAssetName(document.sourcePath),
                                StringComparison.Ordinal) ||
                            HasSerializedChanges(scene, document);
            documents.Add(new ReloadDocumentState(
                sceneId,
                document.savedHash.ToArray(),
                document.isDirty,
                document.nextRefreshTimestamp,
                wasDirty));
        }
        return new WorkspaceReloadTransaction(this, documents, preserveDocumentBaselines: false);
    }

    private sealed class WorkspaceReloadTransaction(
        EditorSceneWorkspace workspace,
        IReadOnlyList<ReloadDocumentState> documents,
        bool preserveDocumentBaselines
    ) : IGenerationChange
    {
        /// <summary>
        /// Prepares candidate state without changing the active generation.
        /// </summary>
        public void PrepareForActivation()
        {
        }

        /// <summary>
        /// Applies the prepared state at the caller-controlled commit point.
        /// </summary>
        public void Apply()
        {
            workspace.SynchronizeReplacedScenes();
            if (preserveDocumentBaselines)
                return;
            foreach (ReloadDocumentState state in documents)
            {
                if (!workspace.m_documents.TryGetValue(state.sceneId, out SceneDocument? document))
                    continue;
                if (state.wasDirty)
                {
                    document.isDirty = true;
                    document.nextRefreshTimestamp = 0;
                    continue;
                }

                document.savedHash = workspace.ComputeSceneHash(document.scene);
                document.isDirty = false;
                document.nextRefreshTimestamp = Stopwatch.GetTimestamp() +
                                                (long)(Stopwatch.Frequency * C_DIRTY_REFRESH_SECONDS);
            }
        }

        /// <summary>
        /// Completes the committed operation and releases temporary state.
        /// </summary>
        public void Complete()
        {
        }

        /// <summary>
        /// Restores the state that existed before candidate activation began.
        /// </summary>
        public void RollbackStructure() => RestoreBaseline();

        /// <summary>
        /// Restores the state that existed before candidate activation began.
        /// </summary>
        public void RestorePreviousState() => RestoreBaseline();

        private void RestoreBaseline()
        {
            workspace.SynchronizeReplacedScenes();
            foreach (ReloadDocumentState state in documents)
            {
                if (!workspace.m_documents.TryGetValue(state.sceneId, out SceneDocument? document))
                    continue;
                document.savedHash = state.savedHash.ToArray();
                document.isDirty = state.isDirty;
                document.nextRefreshTimestamp = state.nextRefreshTimestamp;
            }
        }
    }

    private readonly record struct ReloadDocumentState(
        Guid sceneId,
        byte[] savedHash,
        bool isDirty,
        long nextRefreshTimestamp,
        bool wasDirty
    );

}
