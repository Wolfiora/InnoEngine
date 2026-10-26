using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Core.Events;
using Inno.Core.Logging;
using Inno.Editor.Core;

namespace Inno.Editor.Interactions;
internal sealed class EditorActionRouter(
    EditorExtensionCatalog catalog,
    EditorContext editor,
    EditorInteractions interactions,
    Logger log
)
{
    private readonly Queue<PendingAction> m_pending = [];
    private readonly HashSet<string> m_presentationFailures = new(StringComparer.Ordinal);
    private readonly HashSet<string> m_queryFailures = new(StringComparer.Ordinal);
    private IReadOnlyDictionary<string, EditorExtensionCatalog.ActionRegistration[]>? m_byId;
    internal EditorActionState Query(
        string action,
        EditorActionContext context
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(action);
        ArgumentNullException.ThrowIfNull(context);
        EditorExtensionCatalog.ActionRegistration? registration = Resolve(action, context);
        if (registration is null)
            return EditorActionState.hidden;
        try
        {
            EditorActionState state = registration.action.QueryInternal(context);
            m_queryFailures.Remove(action);
            return state;
        }
        catch (Exception exception)
        {
            if (m_queryFailures.Add(action))
                log.Write(LogLevel.Error, "Editor action '{0}' query failed: {1}", [action, exception]);
            return EditorActionState.disabled;
        }
    }

    internal bool Execute(
        string action,
        EditorActionContext context
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(action);
        ArgumentNullException.ThrowIfNull(context);
        EditorExtensionCatalog.ActionRegistration? registration = Resolve(action, context);
        if (registration is null)
            return false;
        try
        {
            EditorActionState state = registration.action.QueryInternal(context);
            if (!state.isVisible || !state.isEnabled)
                return false;
            interactions.PrepareOperation();
            registration.action.ExecuteInternal(context);
            return true;
        }
        catch (Exception exception)
        {
            TryCancel(registration.action, action, "execution cleanup");
            log.Write(LogLevel.Error, "Editor action '{0}' failed: {1}", [action, exception]);
            return false;
        }
    }

    internal bool Present(
        string action,
        EditorActionContext context
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(action);
        ArgumentNullException.ThrowIfNull(context);
        EditorExtensionCatalog.ActionRegistration? registration = Resolve(action, context);
        if (registration is null)
            return false;
        try
        {
            bool presented = registration.action.PresentInternal(context);
            m_presentationFailures.Remove(action);
            return presented;
        }
        catch (Exception exception)
        {
            TryCancel(registration.action, action, "presentation cleanup");
            if (m_presentationFailures.Add(action))
                log.Write(LogLevel.Error, "Editor action '{0}' presentation failed: {1}", [action, exception]);
            return false;
        }
    }

    internal bool IsActive(
        string action,
        EditorActionContext context
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(action);
        ArgumentNullException.ThrowIfNull(context);
        EditorExtensionCatalog.ActionRegistration? registration = Resolve(action, context);
        return registration is not null && registration.action.IsActiveFor(context.target);
    }

    internal void Enqueue(
        string action,
        EditorActionContext context
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(action);
        ArgumentNullException.ThrowIfNull(context);
        m_pending.Enqueue(new PendingAction(action, context.area, EditorTargetReference.Capture(context.target), context.target is not null, EditorTargetReference.Capture(context.argument), context.argument is not null));
    }

    internal void Flush()
    {
        while (m_pending.TryDequeue(out PendingAction? request))
        {
            object? target = request.target.Resolve(interactions);
            object? argument = request.argument.Resolve(interactions);
            if (request.hasTarget && target is null || request.hasArgument && argument is null)
                continue;
            _ = Execute(request.action, new EditorActionContext(editor, interactions, request.area, target, argument));
        }
    }

    internal void Clear(IReadOnlyList<EditorExtensionCatalog.ActionRegistration> registrations)
    {
        ArgumentNullException.ThrowIfNull(registrations);
        m_pending.Clear();
        m_presentationFailures.Clear();
        m_queryFailures.Clear();
        InvalidateDiscovery();
        for (int i = 0; i < registrations.Count; i++)
        {
            EditorExtensionCatalog.ActionRegistration registration = registrations[i];
            TryCancel(registration.action, registration.id, "runtime shutdown");
        }
    }

    internal void ResetTransientState()
    {
        m_pending.Clear();
        m_presentationFailures.Clear();
        m_queryFailures.Clear();
        InvalidateDiscovery();
    }

    internal void InvalidateDiscovery() => m_byId = null;
    internal void LosePresentationExcept(object? target)
    {
        var visited = new HashSet<EditorAction>(ReferenceEqualityComparer.Instance);
        foreach (EditorExtensionCatalog.ActionRegistration registration in catalog.extensions.actions)
        {
            EditorAction action = registration.action;
            if (!visited.Add(action) || !action.isActive || action.IsActiveFor(target))
                continue;
            try
            {
                action.LosePresentationInternal();
            }
            catch (Exception exception)
            {
                log.Write(LogLevel.Error, "Editor action '{0}' failed while losing presentation focus: {1}", [registration.attribute.action, exception]);
            }
        }
    }

    internal bool TryGetShortcut(
        string action,
        string area,
        object? target,
        out HotKeyGesture gesture
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(action);
        ArgumentException.ThrowIfNullOrWhiteSpace(area);
        var context = new EditorActionContext(editor, interactions, area, target);
        EditorExtensionCatalog.ActionRegistration? registration = Resolve(action, context);
        return TryResolveShortcut(registration, area, out gesture);
    }

    internal bool DispatchShortcut(
        KeyPressedEvent keyEvent,
        string area,
        object? target
    ) {
        var actionIds = new HashSet<string>(StringComparer.Ordinal);
        foreach (EditorExtensionCatalog.ActionRegistration registration in catalog.extensions.actions)
            _ = actionIds.Add(registration.id);
        var context = new EditorActionContext(editor, interactions, area, target);
        var candidates = new List<ShortcutCandidate>();
        foreach (string action in actionIds)
        {
            ResolvedAction? resolved = ResolveWithSpecificity(action, context);
            if (resolved is null)
            {
                continue;
            }

            foreach (EditorShortcutAttribute shortcut in GetApplicableShortcuts(resolved.Value.registration, area))
            {
                HotKeyGesture gesture = CreateGesture(shortcut);
                if (!gesture.Matches(keyEvent))
                    continue;
                candidates.Add(new ShortcutCandidate(action, resolved.Value, gesture));
                break;
            }
        }

        candidates.Sort(static (
            left,
            right
        ) =>
        {
            int areaComparison = right.resolved.exactArea.CompareTo(left.resolved.exactArea);
            if (areaComparison != 0)
                return areaComparison;
            int distanceComparison = left.resolved.targetDistance.CompareTo(right.resolved.targetDistance);
            if (distanceComparison != 0)
                return distanceComparison;
            int priorityComparison = right.resolved.registration.attribute.priority.CompareTo(left.resolved.registration.attribute.priority);
            if (priorityComparison != 0)
                return priorityComparison;
            return string.Compare(left.action, right.action, StringComparison.Ordinal);
        });
        for (int i = 0; i < candidates.Count; i++)
        {
            ShortcutCandidate candidate = candidates[i];
            if (Execute(candidate.action, context))
                return true;
        }

        return false;
    }

    private bool TryResolveShortcut(
        EditorExtensionCatalog.ActionRegistration? registration,
        string area,
        out HotKeyGesture gesture
    ) {
        EditorShortcutAttribute? best = registration is null ? null : GetApplicableShortcuts(registration, area).FirstOrDefault();
        if (best is null)
        {
            gesture = default;
            return false;
        }

        gesture = CreateGesture(best);
        return true;
    }

    private EditorExtensionCatalog.ActionRegistration? Resolve(
        string action,
        EditorActionContext context
    ) => ResolveWithSpecificity(action, context)?.registration;
    private ResolvedAction? ResolveWithSpecificity(
        string action,
        EditorActionContext context
    ) {
        EditorExtensionCatalog.ActionRegistration? best = null;
        int bestDistance = int.MaxValue;
        if (!Index.TryGetValue(action, out EditorExtensionCatalog.ActionRegistration[]? registrations))
            return null;
        foreach (EditorExtensionCatalog.ActionRegistration registration in registrations)
        {
            bool exactArea = registration.area is not null;
            if (exactArea && !string.Equals(registration.area, context.area, StringComparison.Ordinal))
            {
                continue;
            }

            int distance = int.MaxValue;
            Type? targetType = registration.targetType;
            if (targetType is not null)
            {
                if (context.target is null || !EditorTypeDistance.TryGet(context.target.GetType(), targetType, out distance))
                {
                    continue;
                }
            }

            Type? argumentType = registration.argumentType;
            if (argumentType is not null && (context.argument is null || !argumentType.IsInstanceOfType(context.argument)))
            {
                continue;
            }

            bool bestExactArea = best?.area is not null;
            if (best is null
                || exactArea
                && !bestExactArea
                || exactArea == bestExactArea
                && (distance < bestDistance
                || distance == bestDistance
                && registration.attribute.priority > best.attribute.priority
                || distance == bestDistance
                && registration.attribute.priority == best.attribute.priority
                && string.Compare(
                registration.type.FullName,
                best.type.FullName,
                StringComparison.Ordinal
            ) < 0))
            {
                best = registration;
                bestDistance = distance;
            }
        }

        return best is null ? null : new ResolvedAction(best, bestDistance, best.area is not null);
    }

    internal IReadOnlyList<EditorMenuItem> BuildUnplacedCommands(
        EditorActionContext context,
        IReadOnlySet<string> placed
    ) {
        var result = new List<EditorMenuItem>();
        foreach (string id in Index.Keys)
        {
            if (placed.Contains(id))
                continue;
            EditorActionState state = Query(id, context);
            if (!state.isVisible)
                continue;
            string label = state.displayName ?? id;
            result.Add(new EditorMenuItem(label, id, 0, false, state));
        }

        result.Sort(static (
            left,
            right
        ) => StringComparer.OrdinalIgnoreCase.Compare(left.label, right.label));
        return result;
    }

    private IReadOnlyDictionary<string, EditorExtensionCatalog.ActionRegistration[]> Index => m_byId ??= catalog.extensions.actions.GroupBy(
        static registration => registration.id,
        StringComparer.Ordinal
    ).ToDictionary(
        static group => group.Key,
        static group => group.ToArray(),
        StringComparer.Ordinal
    );

    private HotKeyGesture CreateGesture(EditorShortcutAttribute shortcut) => shortcut.primary ? HotKeyGesture.Primary(
        shortcut.key,
        editor.keyboard,
        shortcut.modifiers
    ) : new HotKeyGesture(
        shortcut.key,
        shortcut.modifiers,
        editor.keyboard.superModifierLabel
    );
    private static IEnumerable<EditorShortcutAttribute> GetApplicableShortcuts(
        EditorExtensionCatalog.ActionRegistration registration,
        string area
    ) {
        bool hasExact = registration.shortcuts.Any(shortcut => !string.IsNullOrEmpty(shortcut.area) && string.Equals(shortcut.area, area, StringComparison.Ordinal));
        return registration.shortcuts.Where(shortcut => hasExact ? string.Equals(shortcut.area, area, StringComparison.Ordinal) : string.IsNullOrEmpty(shortcut.area));
    }

    private void TryCancel(
        EditorAction action,
        string actionId,
        string phase
    ) {
        try
        {
            action.CancelInternal();
        }
        catch (Exception exception)
        {
            log.Write(LogLevel.Error, "Editor action '{0}' failed during {1}: {2}", [actionId, phase, exception]);
        }
    }

    private sealed record PendingAction(
        string action,
        string area,
        EditorTargetReference target,
        bool hasTarget,
        EditorTargetReference argument,
        bool hasArgument
    );
    private readonly record struct ResolvedAction(
        EditorExtensionCatalog.ActionRegistration registration,
        int targetDistance,
        bool exactArea
    );
    private readonly record struct ShortcutCandidate(
        string action,
        ResolvedAction resolved,
        HotKeyGesture gesture
    );
}
