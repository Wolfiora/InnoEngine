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

sealed partial class SceneEdits
{
    /// <summary>
    /// Renames a loaded scene and records the two display strings.
    /// </summary>
    /// <param name="scene">
    /// The loaded scene to rename.
    /// </param>
    /// <param name="name">
    /// The new display name.
    /// </param>
    /// <param name="historyName">
    /// The user-facing history entry name.
    /// </param>
    public void RenameScene(
        GameScene scene,
        string name,
        string historyName = "Rename Scene"
    ) {
        ArgumentNullException.ThrowIfNull(scene);
        ArgumentNullException.ThrowIfNull(name);
        using IDisposable presentationScope = m_workspace.EnterPresentationScope();
        m_workspace.EnsureEditable(scene);
        ChangeScalar(
            scene,
            SceneScalarKind.SceneName,
            scene.name,
            name,
            value => scene.name = value,
            historyName,
            $"scene-name:{scene.identity.persistentId:N}");
    }

    /// <summary>
    /// Renames a live GameObject and records the two display strings.
    /// </summary>
    /// <param name="gameObject">
    /// The live GameObject to rename.
    /// </param>
    /// <param name="name">
    /// The new display name.
    /// </param>
    /// <param name="historyName">
    /// The user-facing history entry name.
    /// </param>
    public void RenameGameObject(
        GameObject gameObject,
        string name,
        string historyName = "Rename GameObject"
    ) {
        ArgumentNullException.ThrowIfNull(gameObject);
        ArgumentNullException.ThrowIfNull(name);
        using IDisposable presentationScope = m_workspace.EnterPresentationScope();
        m_workspace.EnsureEditable(gameObject.scene);
        ChangeScalar(
            gameObject,
            SceneScalarKind.GameObjectName,
            gameObject.name,
            name,
            value => gameObject.name = value,
            historyName,
            $"game-object-name:{gameObject.identity.persistentId:N}");
    }

    /// <summary>
    /// Changes the explicit active state of a GameObject and records the two Boolean values.
    /// </summary>
    /// <param name="gameObject">
    /// The live GameObject whose active state should change.
    /// </param>
    /// <param name="active">
    /// The requested explicit active state.
    /// </param>
    /// <param name="historyName">
    /// An optional user-facing history entry name.
    /// </param>
    public void SetGameObjectActive(
        GameObject gameObject,
        bool active,
        string? historyName = null
    ) {
        ArgumentNullException.ThrowIfNull(gameObject);
        using IDisposable presentationScope = m_workspace.EnterPresentationScope();
        m_workspace.EnsureEditable(gameObject.scene);
        string before = gameObject.activeSelf ? "1" : "0";
        string after = active ? "1" : "0";
        ChangeScalar(
            gameObject,
            SceneScalarKind.GameObjectActive,
            before,
            after,
            value => gameObject.SetActive(string.Equals(value, "1", StringComparison.Ordinal)),
            historyName ?? (active ? "Activate GameObject" : "Deactivate GameObject"),
            mergeKey: null);
    }

    /// <summary>
    /// Changes the tag of a live GameObject and records the two ordinal tag strings.
    /// </summary>
    /// <param name="gameObject">
    /// The live GameObject whose tag should change.
    /// </param>
    /// <param name="tag">
    /// The requested non-empty tag.
    /// </param>
    /// <param name="historyName">
    /// The user-facing history entry name.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="gameObject"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="tag"/> or <paramref name="historyName"/> is empty.
    /// </exception>
    public void SetGameObjectTag(
        GameObject gameObject,
        string tag,
        string historyName = "Set GameObject Tag"
    ) {
        ArgumentNullException.ThrowIfNull(gameObject);
        ArgumentException.ThrowIfNullOrWhiteSpace(tag);
        ArgumentException.ThrowIfNullOrWhiteSpace(historyName);
        using IDisposable presentationScope = m_workspace.EnterPresentationScope();
        m_workspace.EnsureEditable(gameObject.scene);
        string requestedTag = tag.Trim();
        ChangeScalar(
            gameObject,
            SceneScalarKind.GameObjectTag,
            gameObject.tag,
            requestedTag,
            value => gameObject.tag = value,
            historyName,
            $"game-object-tag:{gameObject.identity.persistentId:N}");
    }

    /// <summary>
    /// Changes the layer of a live GameObject and records the two stable numeric layer slots.
    /// </summary>
    /// <param name="gameObject">
    /// The live GameObject whose layer should change.
    /// </param>
    /// <param name="layer">
    /// The requested project layer slot.
    /// </param>
    /// <param name="historyName">
    /// The user-facing history entry name.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="gameObject"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="historyName"/> is empty.
    /// </exception>
    public void SetGameObjectLayer(
        GameObject gameObject,
        GameLayer layer,
        string historyName = "Set GameObject Layer"
    ) {
        ArgumentNullException.ThrowIfNull(gameObject);
        ArgumentException.ThrowIfNullOrWhiteSpace(historyName);
        using IDisposable presentationScope = m_workspace.EnterPresentationScope();
        m_workspace.EnsureEditable(gameObject.scene);
        ChangeScalar(
            gameObject,
            SceneScalarKind.GameObjectLayer,
            gameObject.layer.index.ToString(System.Globalization.CultureInfo.InvariantCulture),
            layer.index.ToString(System.Globalization.CultureInfo.InvariantCulture),
            value => gameObject.layer = new GameLayer(
                int.Parse(value, System.Globalization.CultureInfo.InvariantCulture)),
            historyName,
            $"game-object-layer:{gameObject.identity.persistentId:N}");
    }

    /// <summary>
    /// Applies a mutation to one serializable scene property and records only its before and after values.
    /// </summary>
    /// <param name="target">
    /// The live scene object containing the root serialized property.
    /// </param>
    /// <param name="propertyName">
    /// The exact root serialized member key.
    /// </param>
    /// <param name="mutation">
    /// The mutation that assigns the new value.
    /// </param>
    /// <param name="historyName">
    /// The user-facing history entry name.
    /// </param>
    /// <param name="mergeKey">
    /// An optional stable key for coalescing adjacent continuous edits.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the property value changed and a history entry was recorded.
    /// </returns>
    public bool ChangeProperty(
        EngineObject target,
        string propertyName,
        Action mutation,
        string historyName,
        string? mergeKey = null
    ) {
        ArgumentNullException.ThrowIfNull(target);
        ArgumentException.ThrowIfNullOrWhiteSpace(propertyName);
        ArgumentNullException.ThrowIfNull(mutation);
        ArgumentException.ThrowIfNullOrWhiteSpace(historyName);
        using IDisposable presentationScope = m_workspace.EnterPresentationScope();
        m_workspace.EnsureEditable(ResolveOwnerScene(target));
        IReadOnlyList<SerializationPropertySnapshot> before = OrderPropertySnapshots(
            ScenePropertySerialization.CapturePropertySnapshots(
                target,
                m_workspace.serialization,
                m_workspace.assets),
            propertyName);
        if (!before.Any(snapshot => string.Equals(snapshot.name, propertyName, StringComparison.Ordinal)))
        {
            throw new ArgumentException(
                $"Serializable property '{propertyName}' was not found on '{target.GetType().FullName}'.",
                nameof(propertyName));
        }
        IReadOnlyList<SerializationPropertySnapshot> after;
        try
        {
            mutation();
            after = OrderPropertySnapshots(
                ScenePropertySerialization.CapturePropertySnapshots(
                    target,
                    m_workspace.serialization,
                    m_workspace.assets),
                propertyName);
        }
        catch (Exception exception)
        {
            RollbackAndRethrow(exception, () => RestoreSnapshots(target, before));
            throw;
        }

        IReadOnlyDictionary<string, SerializationPropertySnapshot> afterByName = after.ToDictionary(
            static snapshot => snapshot.name,
            StringComparer.Ordinal);
        var deltas = new List<ScenePropertyValueDelta>();
        for (int index = 0; index < before.Count; index++)
        {
            SerializationPropertySnapshot previous = before[index];
            if (!afterByName.TryGetValue(previous.name, out SerializationPropertySnapshot? current))
                continue;
            if (previous.data.Span.SequenceEqual(current.data.Span))
                continue;
            deltas.Add(new ScenePropertyValueDelta(
                previous.name,
                m_workspace.serialization.EncodePropertySnapshots([previous]),
                m_workspace.serialization.EncodePropertySnapshots([current])));
        }
        if (deltas.Count == 0)
            return false;
        ScenePropertyHistoryData data = ScenePropertyHistoryData.Create(
            target.identity.persistentId,
            propertyName,
            deltas);
        RecordWithRollback(
            () => m_interactions.history.RecordApplied(
                historyName,
                new EditorHistoryChange(
                    SceneHistoryKinds.Property,
                    EditorHistoryPayload.FromBytes(data.Encode()),
                    mergeKey)),
            () => RestorePropertyDeltas(target, deltas, useAfter: false));
        return true;
    }

    private static IReadOnlyList<SerializationPropertySnapshot> OrderPropertySnapshots(
        IReadOnlyList<SerializationPropertySnapshot> snapshots,
        string primaryPropertyName
    )
        => snapshots
            .OrderBy(snapshot => string.Equals(snapshot.name, primaryPropertyName, StringComparison.Ordinal) ? 0 : 1)
            .ToArray();

    private void RestoreSnapshots(
        EngineObject target,
        IReadOnlyList<SerializationPropertySnapshot> snapshots
    ) {
        for (int index = 0; index < snapshots.Count; index++)
        {
            RequirePropertyRestore(
                target,
                m_workspace.serialization.EncodePropertySnapshots([snapshots[index]]));
        }
    }

    private void RestorePropertyDeltas(
        EngineObject target,
        IReadOnlyList<ScenePropertyValueDelta> deltas,
        bool useAfter
    ) {
        for (int index = 0; index < deltas.Count; index++)
            RequirePropertyRestore(target, useAfter ? deltas[index].after : deltas[index].before);
    }

    private void ChangeScalar(
        EngineObject target,
        SceneScalarKind scalarKind,
        string before,
        string after,
        Action<string> setter,
        string historyName,
        string? mergeKey
    ) {
        if (string.Equals(before, after, StringComparison.Ordinal))
            return;
        RecordWithRollback(
            () =>
            {
                setter(after);
                SceneScalarHistoryData data = SceneScalarHistoryData.Create(
                    target.identity.persistentId,
                    scalarKind,
                    before,
                    after);
                Record(historyName, SceneHistoryKinds.Scalar, data.Encode(), mergeKey);
            },
            () => setter(before));
    }

    private void RequirePropertyRestore(
        EngineObject target,
        ReadOnlySpan<byte> data
    ) {
        SerializationPropertyRestoreResult result = ScenePropertySerialization.RestoreProperties(
            target,
            data,
            m_workspace.serialization,
            m_workspace.assets);
        if (!result.success || result.ignoredCount != 0 || result.restoredCount == 0)
            throw new InvalidOperationException("Scene property compensation was incomplete.");
    }

    private static void RequireReferenceRestore(SceneReferenceRestoreResult result)
    {
        if (!result.succeeded)
            throw new InvalidOperationException(result.message);
    }

}
