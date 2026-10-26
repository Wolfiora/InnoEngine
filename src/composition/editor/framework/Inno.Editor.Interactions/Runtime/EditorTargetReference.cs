using System;
using Inno.Core.Identity;

namespace Inno.Editor.Interactions;
internal sealed class EditorTargetReference
{
    private readonly object? m_value;
    private readonly WeakReference<object>? m_target;
    private EditorTargetReference(
        RuntimeIdentity? runtimeIdentity,
        Guid persistentId,
        object? value,
        WeakReference<object>? target
    ) {
        this.runtimeIdentity = runtimeIdentity;
        this.persistentId = persistentId;
        m_value = value;
        m_target = target;
    }

    internal RuntimeIdentity? runtimeIdentity { get; }
    internal Guid persistentId { get; }

    internal static EditorTargetReference Capture(object? value)
    {
        if (value is IdentityObject target)
        {
            RuntimeIdentity runtime = target.identity.runtimeIdentity ?? throw new InvalidOperationException("An editor interaction target must belong to a live identity domain.");
            return new EditorTargetReference(runtime, target.identity.persistentId, null, null);
        }

        if (value is null or string || value.GetType().IsValueType && !value.GetType().Assembly.IsCollectible)
            return new EditorTargetReference(null, Guid.Empty, value, null);
        return new EditorTargetReference(null, Guid.Empty, null, new WeakReference<object>(value));
    }

    internal object? Resolve(EditorInteractions interactions)
    {
        if (runtimeIdentity is RuntimeIdentity runtime)
            return interactions.TryResolveIdentity(runtime, out IdentityObject? target) ? target : null;
        return m_target is not null ? m_target.TryGetTarget(out object? value) ? value : null : m_value;
    }

    internal EditorTargetReference Rebind(EditorInteractions interactions)
    {
        if (runtimeIdentity is not RuntimeIdentity runtime || interactions.TryResolveIdentity(runtime, out _))
            return this;
        if (interactions.TryResolveIdentity(runtime.domainId, persistentId, out IdentityObject? replacement) && replacement is not null)
        {
            return Capture(replacement);
        }

        return this;
    }

    internal bool Matches(
        EditorInteractions interactions,
        object? target
    ) {
        if (target is IdentityObject identity && runtimeIdentity is RuntimeIdentity runtime)
            return identity.identity.runtimeIdentity == runtime;
        return Equals(Resolve(interactions), target);
    }
}
