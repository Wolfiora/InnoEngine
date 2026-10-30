using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Core.Logging;
using Inno.Editor.Core;

namespace Inno.Editor.Interactions;

internal sealed class EditorMenuCatalog(
    EditorExtensionCatalog catalog,
    EditorActionRouter actions,
    Logger log
)
{
    private readonly HashSet<string> m_sourceFailures = new(StringComparer.Ordinal);
    private Dictionary<string, TemplateNode[]>? m_templates;

    internal EditorMenuModel Build(EditorMenuContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        TemplateNode[] template = GetTemplate(context.area);
        var dynamicPlacements = new List<Placement>();
        foreach (EditorExtensionCatalog.MenuSourceRegistration registration in catalog.extensions.menuSources)
        {
            if (!string.Equals(registration.area, context.area, StringComparison.Ordinal))
                continue;
            try
            {
                var builder = new EditorMenuBuilder();
                registration.source.Build(context, builder);
                string sourceName = registration.type.FullName ?? registration.type.Name;
                m_sourceFailures.Remove(sourceName);
                if (builder.groups.Count == 0 && builder.items.Count == 0)
                    continue;
                var placements = new List<Placement>(builder.groups.Count + builder.items.Count);
                foreach (var group in builder.groups)
                    placements.Add(CreatePlacement(group.path, null, group.order, group.separatorBefore, null));
                foreach (var item in builder.items)
                    placements.Add(CreatePlacement(item.path, item.actionId, item.order, item.separatorBefore, item.argument));
                dynamicPlacements.AddRange(placements);
            }
            catch (Exception exception)
            {
                string sourceName = registration.type.FullName ?? registration.type.Name;
                if (m_sourceFailures.Add(sourceName))
                    log.Write(LogLevel.Error, "Editor menu source '{0}' failed: {1}", [sourceName, exception]);
            }
        }
        if (dynamicPlacements.Count > 0)
        {
            foreach (Placement placement in dynamicPlacements.OrderBy(static value => value.order).ThenBy(static value => value.path, StringComparer.Ordinal))
                template = Merge(template, placement, 0);
        }
        return new EditorMenuModel(Resolve(template, context));
    }

    internal void InvalidateDiscovery()
    {
        m_templates = null;
        m_sourceFailures.Clear();
    }

    private TemplateNode[] GetTemplate(string area)
    {
        if (m_templates is null)
        {
            var placementsByArea = new Dictionary<string, List<Placement>>(StringComparer.Ordinal);
            foreach (EditorExtensionCatalog.ActionRegistration registration in catalog.extensions.actions)
            {
                foreach (EditorMenuAttribute menu in registration.menus)
                {
                    if (!placementsByArea.TryGetValue(menu.area, out List<Placement>? placements))
                        placementsByArea.Add(menu.area, placements = []);
                    placements.Add(CreatePlacement(menu.path, registration.id, menu.order, menu.separatorBefore, null));
                }
            }
            var templates = new Dictionary<string, TemplateNode[]>(StringComparer.Ordinal);
            foreach ((string key, List<Placement> placements) in placementsByArea)
            {
                var root = new MutableNode(string.Empty, 0, false);
                foreach (Placement placement in placements.OrderBy(static value => value.order).ThenBy(static value => value.path, StringComparer.Ordinal))
                    AddPlacement(root, placement);
                templates.Add(key, FreezeTemplate(root.children.Values));
            }
            m_templates = templates;
        }
        return m_templates.TryGetValue(area, out TemplateNode[]? template) ? template : [];
    }

    private IReadOnlyList<EditorMenuItem> Resolve(
        TemplateNode[] nodes,
        EditorMenuContext context
    ) {
        if (nodes.Length == 0)
            return Array.Empty<EditorMenuItem>();
        var result = new List<EditorMenuItem>(nodes.Length);
        foreach (TemplateNode node in nodes)
        {
            IReadOnlyList<EditorMenuItem> children = Resolve(node.children, context);
            Placement? actionPlacement = null;
            EditorActionState state = EditorActionState.hidden;
            int order = children.Count > 0 ? children[0].order : node.order;
            bool separatorBefore = false;
            foreach (Placement placement in node.placements)
            {
                if (placement.actionId is string action)
                {
                    EditorActionState candidate = actions.Query(action, context.CreateActionContext(placement.argument));
                    if (!candidate.isVisible)
                        continue;
                    actionPlacement = placement;
                    state = candidate;
                }
                order = placement.order;
                separatorBefore = placement.separatorBefore;
            }
            if (actionPlacement is null)
            {
                if (children.Count == 0)
                    continue;
                state = EditorActionState.enabled;
            }
            string label = string.IsNullOrWhiteSpace(state.displayName) ? node.label : state.displayName!;
            result.Add(new EditorMenuItem(label, actionPlacement?.actionId ?? string.Empty, order,
                separatorBefore, state, children, actionPlacement?.argument));
        }
        for (int index = 1; index < result.Count; index++)
        {
            if (result[index - 1].order <= result[index].order)
                continue;
            result.Sort(static (
                left,
                right
            ) => left.order != right.order ? left.order.CompareTo(right.order) : StringComparer.Ordinal.Compare(left.label, right.label));
            break;
        }
        return result.AsReadOnly();
    }

    private static Placement CreatePlacement(
        string path,
        string? actionId,
        int order,
        bool separatorBefore,
        object? argument
    ) {
        string[] segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (segments.Length == 0)
            throw new InvalidOperationException("An editor menu path must contain at least one segment.");
        return new Placement(string.Join('/', segments), segments, actionId, order, separatorBefore, argument);
    }

    private static void AddPlacement(
        MutableNode root,
        Placement placement
    ) {
        MutableNode current = root;
        for (int index = 0; index < placement.segments.Length; index++)
        {
            string segment = placement.segments[index];
            if (!current.children.TryGetValue(segment, out MutableNode? child))
            {
                child = new MutableNode(segment, placement.order,
                    index == placement.segments.Length - 1 && placement.separatorBefore);
                current.children.Add(segment, child);
            }
            current = child;
        }
        current.order = placement.order;
        current.separatorBefore = placement.separatorBefore;
        current.placements.Add(placement);
    }

    private static TemplateNode[] FreezeTemplate(IEnumerable<MutableNode> nodes)
        => nodes.OrderBy(static value => value.order).ThenBy(static value => value.label, StringComparer.Ordinal)
            .Select(static node => new TemplateNode(node.label, node.order, node.separatorBefore,
                node.placements.OrderBy(static placement => placement.order).ToArray(), FreezeTemplate(node.children.Values))).ToArray();

    private static TemplateNode[] Merge(
        TemplateNode[] nodes,
        Placement placement,
        int depth
    ) {
        string label = placement.segments[depth];
        int index = -1;
        for (int candidate = 0; candidate < nodes.Length; candidate++)
        {
            if (!string.Equals(nodes[candidate].label, label, StringComparison.Ordinal))
                continue;
            index = candidate;
            break;
        }
        TemplateNode current = index >= 0 ? nodes[index] : new TemplateNode(label, placement.order, false, [], []);
        bool leaf = depth == placement.segments.Length - 1;
        Placement[] placements = current.placements;
        if (leaf)
        {
            int insertion = 0;
            while (insertion < placements.Length && placements[insertion].order <= placement.order)
                insertion++;
            placements = new Placement[current.placements.Length + 1];
            Array.Copy(current.placements, placements, insertion);
            placements[insertion] = placement;
            Array.Copy(current.placements, insertion, placements, insertion + 1, current.placements.Length - insertion);
        }
        TemplateNode replacement = new(label, current.order, current.separatorBefore, placements,
            leaf ? current.children : Merge(current.children, placement, depth + 1));
        var result = new TemplateNode[nodes.Length + (index < 0 ? 1 : 0)];
        Array.Copy(nodes, result, nodes.Length);
        result[index >= 0 ? index : nodes.Length] = replacement;
        Array.Sort(result, static (
            left,
            right
        ) => left.order != right.order ? left.order.CompareTo(right.order) : StringComparer.Ordinal.Compare(left.label, right.label));
        return result;
    }

    private sealed record Placement(
        string path,
        string[] segments,
        string? actionId,
        int order,
        bool separatorBefore,
        object? argument
    );

    private sealed record TemplateNode(
        string label,
        int order,
        bool separatorBefore,
        Placement[] placements,
        TemplateNode[] children
    );

    private sealed class MutableNode(
        string label,
        int order,
        bool separatorBefore
    )
    {
        internal string label = label;
        internal int order = order;
        internal bool separatorBefore = separatorBefore;
        internal readonly List<Placement> placements = [];
        internal readonly Dictionary<string, MutableNode> children = new(StringComparer.Ordinal);
    }
}
