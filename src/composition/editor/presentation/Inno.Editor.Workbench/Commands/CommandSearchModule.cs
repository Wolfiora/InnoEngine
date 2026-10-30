using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Core.Identity;
using Inno.Editor.Core;
using Inno.Editor.ImGui;
using Inno.Editor.Interactions;

namespace Inno.Editor.Workbench;
[EditorModule(GlobalInteractionIds.C_COMMAND_SEARCH_MODULE, order: 20)]
internal sealed class CommandSearchModule(EditorInteractions interactions) : EditorModule
{
    private string m_area = "editor/global";
    private RuntimeIdentity? m_target;
    internal bool isOpen { get; private set; }
    internal string search { get; set; } = string.Empty;
    internal bool requestsFocus { get; set; }

    internal void Open()
    {
        m_area = interactions.focusedArea;
        m_target = (interactions.focusedTarget as IdentityObject)?.identity.runtimeIdentity;
        search = string.Empty;
        requestsFocus = true;
        isOpen = true;
    }

    internal void Close()
    {
        isOpen = false;
        m_target = null;
        search = string.Empty;
    }

    internal bool Draw(EditorUiSession ui)
    {
        EditorInteraction global = interactions.For(GlobalInteractionIds.C_MAIN_MENU_AREA);
        EditorMenuModel globalMenu = global.BuildMenu();
        var globalIds = new HashSet<string>(StringComparer.Ordinal);
        Collect(globalMenu.items);
        if (ui.menus.DrawSearchItems(global, globalMenu.items, search))
            return true;
        object? target = m_target is RuntimeIdentity id && interactions.TryResolveIdentity(id, out IdentityObject? live) ? live : null;
        EditorInteraction local = interactions.For(m_area, target);
        EditorMenuModel commands = local.BuildCommands();
        var localItems = Leaves(commands.items).Where(item => !globalIds.Contains(item.actionId)).ToArray();
        return ui.menus.DrawSearchItems(local, localItems, search);
        void Collect(IReadOnlyList<EditorMenuItem> items)
        {
            foreach (EditorMenuItem item in items)
            {
                if (item.actionId.Length != 0)
                    _ = globalIds.Add(item.actionId);
                Collect(item.children);
            }
        }
    }

    /// <inheritdoc/>
    protected override void OnStop(EditorContext context) => Close();
    private static IEnumerable<EditorMenuItem> Leaves(IReadOnlyList<EditorMenuItem> items)
    {
        foreach (EditorMenuItem item in items)
        {
            if (item.children.Count == 0)
                yield return item;
            else
            {
                foreach (EditorMenuItem child in Leaves(item.children))
                    yield return child;
            }
        }
    }
}
