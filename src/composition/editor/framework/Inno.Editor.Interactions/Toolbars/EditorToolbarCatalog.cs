using System;
using System.Collections.Generic;

namespace Inno.Editor.Interactions;

internal sealed class EditorToolbarCatalog(
    EditorExtensionCatalog catalog,
    EditorActionRouter actions
) {
    internal EditorToolbarModel Build(EditorActionContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        var items = new List<(EditorToolbarItem item, string typeName)>();
        EditorExtensionCatalog.Snapshot snapshot = catalog.extensions;
        for (int actionIndex = 0; actionIndex < snapshot.actions.Length; actionIndex++)
        {
            EditorExtensionCatalog.ActionRegistration registration = snapshot.actions[actionIndex];
            for (int toolbarIndex = 0; toolbarIndex < registration.toolbars.Length; toolbarIndex++)
            {
                EditorToolbarItemAttribute placement = registration.toolbars[toolbarIndex];
                if (!string.Equals(placement.area, context.area, StringComparison.Ordinal))
                {
                    continue;
                }

                EditorActionState status = actions.Query(registration.id, context);
                if (!status.isVisible)
                {
                    continue;
                }
                EditorToolbarIcon icon = status.isChecked && placement.activeIcon != EditorToolbarIcon.None
                                         ? placement.activeIcon
                                         : placement.icon;
                items.Add(
                    (new EditorToolbarItem(
                        registration.id,
                        icon,
                        status.displayName ?? placement.tooltip,
                        placement.order,
                        status
                    ), registration.type.FullName ?? registration.type.Name)
                );
            }
        }

        items.Sort(static (
            left,
            right
        ) =>
        {
            int order = left.item.order.CompareTo(right.item.order);
            if (order != 0)
            {
                return order;
            }
            int action = string.Compare(left.item.actionID, right.item.actionID, StringComparison.Ordinal);
            return action != 0
                   ? action
                   : string.Compare(left.typeName, right.typeName, StringComparison.Ordinal);
        });
        return new EditorToolbarModel(items.ConvertAll(static value => value.item));
    }
}
