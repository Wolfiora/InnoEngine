using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Inno.Core.Mathematics;
using Inno.Core.Serialization;
using Inno.Editor.Interactions;
using Inno.Scene;

namespace Inno.Editor.Scene;
sealed partial class SceneEdits
{
    /// <summary>
    /// Deletes selected subtrees atomically, omitting descendants already covered by a selected ancestor.
    /// </summary>
    /// <param name="targets">
    /// Live objects to delete; every owner is validated before the first mutation.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// An owner is unavailable, read-only, or rollback fails.
    /// </exception>
    public void DeleteGameObjects(IReadOnlyList<GameObject> targets)
    {
        ArgumentNullException.ThrowIfNull(targets);
        GameObject[] unique = targets.Distinct().ToArray();
        foreach (GameObject target in unique)
        {
            ArgumentNullException.ThrowIfNull(target);
            m_workspace.EnsureEditable(target.scene);
            if (!target.isRuntimeValid)
                throw new InvalidOperationException("A selected GameObject is no longer available.");
        }

        var selected = unique.Select(value => value.identity.persistentId).ToHashSet();
        GameObject[] roots = unique.Where(value => !HasSelectedAncestor(value, selected)).ToArray();
        using EditorHistoryTransaction transaction = m_interactions.history.BeginTransaction("Delete GameObjects");
        foreach (GameObject target in roots)
            _ = DeleteGameObject(target);
        transaction.Commit();
    }

    /// <summary>
    /// Applies one primary property edit to a selection using owner-resolved serialized values and one atomic History transaction.
    /// </summary>
    /// <param name="targets">
    /// Ordered compatible scene objects; the first object supplies the edited value.
    /// </param>
    /// <param name="propertyName">
    /// The common root serialized property.
    /// </param>
    /// <param name="mutation">
    /// Changes the primary value synchronously; no callback is retained.
    /// </param>
    /// <param name="historyName">
    /// The user-facing batch edit name.
    /// </param>
    /// <param name="mergeKey">
    /// An optional active gesture identity; null keeps discrete edits separate.
    /// </param>
    /// <param name="componentName">
    /// An optional public scalar value component; Euler axes use euler.x, euler.y or euler.z.
    /// </param>
    /// <returns>
    /// True when at least one object changed; false when all values already matched.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The selection is empty or a required common property is absent.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// An owner is unavailable, read-only, or rollback fails.
    /// </exception>
    public bool ChangeProperties(
        IReadOnlyList<EngineObject> targets,
        string propertyName,
        Action mutation,
        string historyName,
        string? mergeKey = null,
        string? componentName = null
    ) {
        ArgumentNullException.ThrowIfNull(targets);
        ArgumentException.ThrowIfNullOrWhiteSpace(propertyName);
        ArgumentNullException.ThrowIfNull(mutation);
        ArgumentException.ThrowIfNullOrWhiteSpace(historyName);
        if (targets.Count == 0)
            throw new ArgumentException("A property edit requires at least one target.", nameof(targets));
        foreach (EngineObject target in targets)
        {
            ArgumentNullException.ThrowIfNull(target);
            m_workspace.EnsureEditable(ResolveOwnerScene(target));
            if (!ScenePropertySerialization.CapturePropertySnapshots(target, m_workspace.serialization, m_workspace.assets).Any(value => value.name == propertyName))
                throw new ArgumentException($"Selected target has no serialized property '{propertyName}'.", nameof(targets));
        }

        if (componentName is not null)
            foreach (EngineObject target in targets)
                ValidateComponent(GetSelectionProperty(target, propertyName).propertyType, componentName);
        using EditorHistoryTransaction transaction = m_interactions.history.BeginTransaction(historyName);
        bool changed = ChangeProperty(targets[0], propertyName, mutation, historyName, mergeKey);
        SerializationPropertySnapshot snapshot = ScenePropertySerialization.CapturePropertySnapshots(
            targets[0],
            m_workspace.serialization,
            m_workspace.assets
        ).Single(value => value.name == propertyName);
        byte[] value = m_workspace.serialization.EncodePropertySnapshots([snapshot]);
        for (int index = 1; index < targets.Count; index++)
        {
            EngineObject target = targets[index];
            changed |= ChangeProperty(target, propertyName, () =>
            {
                if (componentName is null)
                    RequirePropertyRestore(target, value);
                else
                {
                    SerializedProperty property = GetSelectionProperty(target, propertyName);
                    object primary = GetSelectionProperty(targets[0], propertyName).GetValue()!;
                    object current = property.GetValue() ?? Activator.CreateInstance(Nullable.GetUnderlyingType(property.propertyType) ?? property.propertyType)!;
                    property.SetValue(CopyComponent(primary, current, componentName));
                }
            }, historyName, mergeKey);
        }

        transaction.Commit();
        return changed;
    }

    /// <summary>
    /// Compares common serialized property values using the complete owner serialization context.
    /// </summary>
    /// <param name="targets">
    /// Live compatible scene objects to compare.
    /// </param>
    /// <param name="propertyName">
    /// The required common serialized member.
    /// </param>
    /// <returns>
    /// True when every encoded value matches, including an empty selection.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// A selected object does not expose the requested property.
    /// </exception>
    public bool HaveSamePropertyValues(
        IReadOnlyList<EngineObject> targets,
        string propertyName
    ) {
        ArgumentNullException.ThrowIfNull(targets);
        ArgumentException.ThrowIfNullOrWhiteSpace(propertyName);
        SerializationPropertySnapshot? first = null;
        foreach (EngineObject target in targets)
        {
            SerializationPropertySnapshot current = ScenePropertySerialization.CapturePropertySnapshots(
                target,
                m_workspace.serialization,
                m_workspace.assets
            ).Single(value => value.name == propertyName);
            if (first is not null && !first.data.Span.SequenceEqual(current.data.Span))
                return false;
            first = current;
        }

        return true;
    }

    /// <summary>
    /// Moves minimal selected roots together, preserving descendants and recording only affected placements.
    /// </summary>
    /// <param name="targets">
    /// Selected live objects in their required relative order.
    /// </param>
    /// <param name="destination">
    /// The loaded editable destination scene.
    /// </param>
    /// <param name="parent">
    /// The new parent, or null for scene roots.
    /// </param>
    /// <param name="insertionIndex">
    /// The insertion index after removing the selected roots; the default appends them.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// An owner is read-only, unavailable, or the move would form a cycle.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The parent belongs to a different scene.
    /// </exception>
    public void MoveGameObjects(
        IReadOnlyList<GameObject> targets,
        GameScene destination,
        GameObject? parent = null,
        int insertionIndex = int.MaxValue
    ) {
        ArgumentNullException.ThrowIfNull(targets);
        ArgumentNullException.ThrowIfNull(destination);
        m_workspace.EnsureEditable(destination);
        if (parent is not null && !ReferenceEquals(parent.scene, destination))
            throw new ArgumentException("The parent must belong to the destination scene.", nameof(parent));
        GameObject[] unique = targets.Distinct().ToArray();
        foreach (GameObject target in unique)
        {
            ArgumentNullException.ThrowIfNull(target);
            m_workspace.EnsureEditable(target.scene);
            if (!target.isRuntimeValid)
                throw new InvalidOperationException("A selected GameObject is no longer available.");
        }

        var selected = unique.Select(value => value.identity.persistentId).ToHashSet();
        GameObject[] roots = unique.Where(value => !HasSelectedAncestor(value, selected)).ToArray();
        if (roots.Length == 0)
            return;
        for (var ancestor = parent?.transform; ancestor is not null; ancestor = ancestor.parent)
            if (selected.Contains(ancestor.gameObject.identity.persistentId))
                throw new InvalidOperationException("A selection cannot be moved beneath itself.");
        _ = ChangeHierarchy(roots[0], hierarchy =>
        {
            foreach (GameObject target in roots)
            {
                if (!ReferenceEquals(target.scene, destination))
                    hierarchy.MoveToScene(target, destination);
                target.transform.SetParent(parent?.transform);
                target.transform.SetSiblingIndex(int.MaxValue);
            }

            int available = parent is null ? destination.GetObjects().Count(value => value.transform.parent is null) - roots.Length : parent.transform.children.Count - roots.Length;
            int start = Math.Clamp(insertionIndex, 0, available);
            // Place from the end so intermediate sibling shifts cannot reverse the selection.
            for (int index = roots.Length - 1; index >= 0; index--)
                roots[index].transform.SetSiblingIndex(start);
        }, "Move GameObjects", roots);
    }

    private SerializedProperty GetSelectionProperty(
        EngineObject target,
        string propertyName
    ) => m_workspace.serialization.GetProperties((ISerializable)target).Single(value => value.name == propertyName);

    private static void ValidateComponent(
        Type type,
        string componentName
    ) {
        type = Nullable.GetUnderlyingType(type) ?? type;
        if (type == typeof(Quaternion) && componentName is "euler.x" or "euler.y" or "euler.z")
            return;
        FieldInfo? field = type.GetField(componentName, BindingFlags.Instance | BindingFlags.Public);
        if (!type.IsValueType || field is null || field.IsInitOnly || !field.FieldType.IsPrimitive)
            throw new ArgumentException("A batch component must be a writable public scalar value field.", nameof(componentName));
    }

    private static object CopyComponent(
        object primary,
        object current,
        string componentName
    ) {
        if (primary is Quaternion edited && current is Quaternion existing && componentName.StartsWith("euler.", StringComparison.Ordinal))
        {
            Vector3 next = edited.ToEulerAnglesXYZDegrees();
            Vector3 value = existing.ToEulerAnglesXYZDegrees();
            switch (componentName)
            {
                case "euler.x": value.x = next.x; break;
                case "euler.y": value.y = next.y; break;
                case "euler.z": value.z = next.z; break;
            }
            return Quaternion.FromEulerAnglesXYZDegrees(value).normalized;
        }
        // Only the current public value is boxed and edited; no private state or plugin metadata is retained.
        FieldInfo field = primary.GetType().GetField(componentName, BindingFlags.Instance | BindingFlags.Public)!;
        field.SetValue(current, field.GetValue(primary));
        return current;
    }

    private static bool HasSelectedAncestor(
        GameObject target,
        ISet<Guid> selected
    ) {
        for (var parent = target.transform.parent; parent is not null; parent = parent.parent)
            if (selected.Contains(parent.gameObject.identity.persistentId))
                return true;
        return false;
    }
}
