using System;
using System.Collections.Generic;

using Inno.Core.Mathematics;
using Inno.Scene.Components;

namespace Inno.Scene;

/// <summary>
/// Maintains parent, child, root, and sibling relationships.
/// </summary>
internal sealed class HierarchyService
{
    private readonly GameScene m_scene;
    private readonly List<Transform> m_roots = [];
    private readonly Dictionary<Transform, int> m_rootIndices = new(ReferenceEqualityComparer.Instance);

    internal HierarchyService(GameScene scene)
    {
        m_scene = scene;
    }

    internal ulong revision { get; private set; }

    internal void Register(Transform transform)
    {
        if (m_rootIndices.TryAdd(transform, m_roots.Count))
        {
            m_roots.Add(transform);
            revision++;
        }
    }

    internal void Unregister(Transform transform)
    {
        Transform? parent = transform.parent;
        if (parent is null)
            RemoveRoot(transform);
        else
            parent.RemoveChildDirect(transform);
        transform.SetParentDirect(null);
        revision++;
    }

    internal void SetParent(
        Transform transform,
        Transform? parent,
        bool worldPositionStays
    ) {
        ArgumentNullException.ThrowIfNull(transform);
        if (ReferenceEquals(transform, parent))
            throw new InvalidOperationException("A transform cannot parent itself.");
        if (parent is not null && parent.IsDescendantOf(transform))
            throw new InvalidOperationException("The requested transform parent would create a hierarchy cycle.");
        if (parent is not null && !ReferenceEquals(parent.gameObject.scene, m_scene))
            throw new InvalidOperationException("Transforms from different scenes cannot share a hierarchy.");
        if (ReferenceEquals(transform.parent, parent))
            return;
        if (worldPositionStays
            && parent is not null
            && MathF.Abs(Matrix.Determinant(parent.localToWorldMatrix)) < MathHelper.C_TOLERANCE)
        {
            throw new InvalidOperationException(
                "A transform cannot preserve world space under a parent hierarchy with zero scale.");
        }

        Vector3 worldPosition = transform.worldPosition;
        Quaternion worldRotation = transform.worldRotation;
        Vector3 worldScale = transform.worldScale;
        Vector3 localPosition = transform.localPosition;
        Quaternion localRotation = transform.localRotation;
        Vector3 localScale = transform.localScale;

        Transform? previousParent = transform.parent;
        if (previousParent is null)
            RemoveRoot(transform);
        else
            previousParent.RemoveChildDirect(transform);

        transform.SetParentDirect(parent);
        if (parent is null)
            Register(transform);
        else
            parent.AddChildDirect(transform);

        if (worldPositionStays)
        {
            transform.ApplyWorld(worldPosition, worldRotation, worldScale);
        }
        else
        {
            transform.localPosition = localPosition;
            transform.localRotation = localRotation;
            transform.localScale = localScale;
            transform.RecomputeWorldFromLocal();
        }

        m_scene.RecomputeActiveSubtree(transform.gameObject);
        revision++;
    }

    internal int GetSiblingIndex(Transform transform)
    {
        if (transform.parent is Transform parent)
            return parent.IndexOfChild(transform);
        return m_rootIndices.GetValueOrDefault(transform, -1);
    }

    internal void SetSiblingIndex(
        Transform transform,
        int siblingIndex
    ) {
        if (transform.parent is Transform parent)
        {
            int current = parent.IndexOfChild(transform);
            if (current < 0)
                throw new InvalidOperationException("Transform is missing from its parent child collection.");
            int target = Math.Clamp(siblingIndex, 0, parent.children.Count - 1);
            if (current != target)
            {
                parent.MoveChild(current, target);
                revision++;
            }
            return;
        }

        int rootIndex = m_rootIndices.GetValueOrDefault(transform, -1);
        if (rootIndex < 0)
            throw new InvalidOperationException("Transform is missing from the scene hierarchy index.");
        int rootTarget = Math.Clamp(siblingIndex, 0, m_roots.Count - 1);
        if (rootIndex == rootTarget)
            return;
        m_roots.RemoveAt(rootIndex);
        m_roots.Insert(rootTarget, transform);
        RefreshRootIndices(Math.Min(rootIndex, rootTarget), Math.Max(rootIndex, rootTarget) + 1);
        revision++;
    }

    internal void Clear()
    {
        m_roots.Clear();
        m_rootIndices.Clear();
        revision++;
    }

    private void RemoveRoot(Transform transform)
    {
        if (!m_rootIndices.Remove(transform, out int index))
            return;
        m_roots.RemoveAt(index);
        RefreshRootIndices(index, m_roots.Count);
    }

    private void RefreshRootIndices(
        int start,
        int end
    ) {
        for (int index = start; index < end; index++)
            m_rootIndices[m_roots[index]] = index;
    }
}
