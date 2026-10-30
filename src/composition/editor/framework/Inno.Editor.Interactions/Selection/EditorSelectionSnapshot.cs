using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Core.Identity;

namespace Inno.Editor.Interactions;
/// <summary>
/// Captures ordered selection identities without retaining any domain object or extension instance.
/// </summary>
public sealed class EditorSelectionSnapshot
{
    private readonly EditorTargetReference[] m_targets;
    internal EditorSelectionSnapshot(
        IEnumerable<EditorTargetReference> targets,
        RuntimeIdentity? anchor
    ) {
        m_targets = targets.ToArray();
        this.identities = Array.AsReadOnly(m_targets.Where(value => value.runtimeIdentity is not null).Select(value => value.runtimeIdentity!.Value).ToArray());
        this.anchor = anchor;
    }

    /// <summary>
    /// Gets unique selection identities in order; the final identity is the primary selection.
    /// </summary>
    public IReadOnlyList<RuntimeIdentity> identities { get; }
    /// <summary>
    /// Gets the range anchor identity, or null when there is no registered anchor.
    /// </summary>
    public RuntimeIdentity? anchor { get; }

    /// <summary>
    /// Resolves this immutable selection in its original domains, including same-identity recovery after a generation change.
    /// </summary>
    /// <typeparam name="TTarget">
    /// The target type accepted by the current presenter.
    /// </typeparam>
    /// <param name="interactions">
    /// The active session supplying the original identity domains.
    /// </param>
    /// <param name="destination">
    /// A temporary caller-owned collection to clear and fill.
    /// </param>
    /// <returns>
    /// The number of currently available compatible targets; missing members remain represented by this snapshot.
    /// </returns>
    public int CopyTargets<TTarget>(
        EditorInteractions interactions,
        ICollection<TTarget> destination
    )
        where TTarget : class
    {
        ArgumentNullException.ThrowIfNull(interactions);
        ArgumentNullException.ThrowIfNull(destination);
        destination.Clear();
        foreach (EditorTargetReference reference in m_targets)
            if (reference.Rebind(interactions).Resolve(interactions) is TTarget target)
                destination.Add(target);
        return destination.Count;
    }

    /// <summary>
    /// Gets the primary identity, or null when this snapshot contains no registered targets.
    /// </summary>
    public RuntimeIdentity? primary => identities.Count == 0 ? null : identities[^1];
}
