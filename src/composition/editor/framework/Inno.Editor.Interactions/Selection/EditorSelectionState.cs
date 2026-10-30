using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Inno.Core.Identity;

namespace Inno.Editor.Interactions;
/// <summary>
/// Owns ordered transient selection through domain-qualified identities instead of retaining live objects.
/// </summary>
public sealed class EditorSelectionState
{
    private readonly EditorInteractions m_interactions;
    private EditorTargetReference[] m_targets = [];
    private HashSet<RuntimeIdentity> m_identityMembership = [];
    private EditorTargetReference? m_anchor;
    private EditorSelectionSnapshot? m_snapshot;
    internal EditorSelectionState(EditorInteractions interactions)
    {
        m_interactions = interactions;
    }

    /// <summary>
    /// Gets the current primary target for this callback, or null when unavailable or unselected.
    /// </summary>
    public object? selectedTarget => m_targets.Length == 0 ? null : m_targets[^1].Resolve(m_interactions);
    /// <summary>
    /// Gets the number of selected identities, including temporarily unavailable members.
    /// </summary>
    public int count => m_targets.Length;
    /// <summary>
    /// Gets the current range anchor, or null when the anchor is unavailable.
    /// </summary>
    public object? anchor => m_anchor?.Resolve(m_interactions);
    /// <summary>
    /// Gets the selection revision, incremented only when membership, order or resolved generation changes.
    /// </summary>
    public ulong version { get; private set; }

    /// <summary>
    /// Checks membership by the target's owning identity domain and generation.
    /// </summary>
    /// <param name="target">
    /// The immediate target to inspect.
    /// </param>
    /// <returns>
    /// True when the target is a member of this selection.
    /// </returns>
    public bool Contains(object? target)
    {
        if (target is null)
            return false;
        if (target is IdentityObject identity)
            return identity.identity.runtimeIdentity is RuntimeIdentity runtime && m_identityMembership.Contains(runtime);
        foreach (EditorTargetReference entry in m_targets)
        {
            if (entry.Matches(m_interactions, target))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Copies currently available selected targets into a caller-owned temporary collection.
    /// </summary>
    /// <typeparam name="TTarget">
    /// The target type accepted by the caller.
    /// </typeparam>
    /// <param name="destination">
    /// The collection to clear and fill; do not retain live entries across callbacks.
    /// </param>
    /// <returns>
    /// The number of targets resolved in this callback.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The destination is null.
    /// </exception>
    public int CopyTargets<TTarget>(ICollection<TTarget> destination)
        where TTarget : class
    {
        ArgumentNullException.ThrowIfNull(destination);
        destination.Clear();
        foreach (EditorTargetReference entry in m_targets)
        {
            if (entry.Resolve(m_interactions) is TTarget target)
                destination.Add(target);
        }

        return destination.Count;
    }

    /// <summary>
    /// Captures the ordered runtime identities without retaining their resolved objects.
    /// </summary>
    /// <returns>
    /// An immutable callback-independent snapshot; resolve identities afresh before using targets.
    /// </returns>
    public EditorSelectionSnapshot Capture()
    {
        if (m_snapshot is not null)
            return m_snapshot;
        return m_snapshot = new EditorSelectionSnapshot(m_targets, m_anchor?.runtimeIdentity);
    }

    /// <summary>
    /// Resolves the primary selected target as the requested type for immediate use.
    /// </summary>
    /// <typeparam name="TTarget">
    /// The expected target type.
    /// </typeparam>
    /// <param name="target">
    /// Receives the available primary target, or null.
    /// </param>
    /// <returns>
    /// True when the current primary target has the requested type.
    /// </returns>
    public bool TryGet<TTarget>([NotNullWhen(true)] out TTarget? target)
    {
        if (selectedTarget is TTarget typed)
        {
            target = typed;
            return true;
        }

        target = default;
        return false;
    }

    internal void Select(object target)
    {
        ArgumentNullException.ThrowIfNull(target);
        if (m_targets.Length == 1 && m_targets[0].Matches(m_interactions, target))
            return;
        EditorTargetReference entry = EditorTargetReference.Capture(target);
        m_targets = [entry];
        m_anchor = entry;
        Changed();
    }

    internal void Replace(
        IEnumerable<IdentityObject> targets,
        IdentityObject? anchor
    ) {
        ArgumentNullException.ThrowIfNull(targets);
        var entries = new List<EditorTargetReference>();
        var seen = new HashSet<RuntimeIdentity>();
        foreach (IdentityObject target in targets)
        {
            ArgumentNullException.ThrowIfNull(target);
            EditorTargetReference entry = EditorTargetReference.Capture(target);
            if (seen.Add(entry.runtimeIdentity!.Value))
                entries.Add(entry);
        }

        EditorTargetReference? nextAnchor = anchor is null ? entries.Count == 0 ? null : entries[0] : EditorTargetReference.Capture(anchor);
        bool equal = entries.Count == m_targets.Length && nextAnchor?.runtimeIdentity == m_anchor?.runtimeIdentity;
        for (int i = 0; equal && i < entries.Count; i++)
            equal = entries[i].runtimeIdentity == m_targets[i].runtimeIdentity;
        if (equal)
            return;
        m_targets = entries.ToArray();
        m_anchor = nextAnchor;
        Changed(seen);
    }

    internal void Clear()
    {
        if (m_targets.Length == 0)
            return;
        m_targets = [];
        m_anchor = null;
        Changed();
    }

    internal EditorTargetReference? anchorReference => m_anchor;

    internal EditorTargetReference[] CaptureReferences() => (EditorTargetReference[])m_targets.Clone();
    internal void Restore(
        EditorTargetReference[] targets,
        EditorTargetReference? anchor
    ) {
        m_targets = targets;
        m_anchor = anchor;
        Changed();
    }

    internal void Rebind()
    {
        bool changed = false;
        for (int i = 0; i < m_targets.Length; i++)
        {
            EditorTargetReference rebound = m_targets[i].Rebind(m_interactions);
            if (rebound.runtimeIdentity == m_targets[i].runtimeIdentity)
                continue;
            m_targets[i] = rebound;
            changed = true;
        }

        if (m_anchor is not null)
            m_anchor = m_anchor.Rebind(m_interactions);
        if (changed)
            Changed();
    }

    private void Changed(HashSet<RuntimeIdentity>? identities = null)
    {
        if (identities is null)
        {
            identities = new HashSet<RuntimeIdentity>(m_targets.Length);
            foreach (EditorTargetReference target in m_targets)
                if (target.runtimeIdentity is RuntimeIdentity identity)
                    identities.Add(identity);
        }
        m_identityMembership = identities;
        m_snapshot = null;
        version++;
    }
}
