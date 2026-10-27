using System;
using System.Collections.Generic;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Core.Identity;
using Inno.Editor.Assets.ImGui;
using Inno.Editor.Assets;
using Inno.Editor.Core;
using Inno.Editor.ImGui;
using Inno.Editor.ImGui.ImGuiWidget;
using Inno.Editor.Interactions;
using Ui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Panel.FileBrowser;

internal sealed partial class FileBrowserTree
{
    private readonly List<TreeNavigationRow> m_navigationRows = [];
    private readonly List<RuntimeIdentity> m_visibleSelection = [];
    private RuntimeIdentity? m_keyboardFocus;
    private RuntimeIdentity? m_pendingSelection;
    private RuntimeIdentity? m_requestedKeyboardFocus;
    private RuntimeIdentity? m_drawKeyboardFocus;
    private bool m_rootFocused;
    private bool m_requestedRootFocus;
    private bool m_drawRootFocus;
    private bool m_pendingActivation;
    private bool m_pendingRootNavigation;
    private int m_keyboardPageSize;

    internal void CompleteNavigation(EditorContext context)
    {
        EditorInteractions interactions = m_assets.edits.interactions;
        if (m_pendingSelection is RuntimeIdentity pending
            && interactions.TryResolveIdentity(pending, out IdentityObject? resolved) && resolved is AssetFileEntry entry)
        {
            if (m_pendingActivation)
            {
                if (!interactions.selection.Contains(entry))
                    interactions.SetSelection(entry);
                m_navigation.OpenEntry(context, entry, this);
            }
            else
                EditorUiSession.current.widgets.SelectTarget(interactions, entry, m_visibleSelection);
            m_keyboardFocus = pending;
            m_rootFocused = false;
            interactions.For(FileBrowserInteractionIds.C_AREA, entry).Focus();
        }
        if (m_pendingRootNavigation)
        {
            m_navigation.NavigateToRoot(context, m_assets.browser.root);
            m_assets.browser.Select(context, null);
            m_keyboardFocus = null;
            m_rootFocused = true;
            interactions.For(FileBrowserInteractionIds.C_AREA).Focus();
        }
        int focus = -1;
        int selectedIndex = -1;
        RuntimeIdentity? selectedIdentity = (interactions.selection.selectedTarget as IdentityObject)?.identity.runtimeIdentity;
        for (int index = 0; index < m_navigationRows.Count; index++)
        {
            TreeNavigationRow row = m_navigationRows[index];
            if (row.identity is null ? m_rootFocused : row.identity == m_keyboardFocus)
            {
                focus = index;
                break;
            }
            if (row.identity is not null && row.identity == selectedIdentity)
                selectedIndex = index;
        }
        if (focus < 0)
            focus = selectedIndex;
        ListNavigationResult navigation = EditorUiSession.current.widgets.NavigateList(interactions, m_navigationRows.Count, focus, m_keyboardPageSize);
        if (navigation.focusedIndex >= 0 && (navigation.focusChanged || navigation.selectionRequested || navigation.activate))
        {
            TreeNavigationRow row = m_navigationRows[navigation.focusedIndex];
            m_keyboardFocus = row.identity;
            m_rootFocused = row.identity is null;
            if (navigation.focusChanged)
            {
                m_requestedKeyboardFocus = row.identity;
                m_requestedRootFocus = m_rootFocused;
            }
            if (row.identity is RuntimeIdentity identity
                && interactions.TryResolveIdentity(identity, out IdentityObject? target) && target is AssetFileEntry asset)
            {
                if (navigation.selectionRequested)
                {
                    if (navigation.extendRange)
                        interactions.SelectRange(m_visibleSelection, asset, navigation.additive);
                    else if (navigation.toggle)
                        interactions.ToggleSelection(asset);
                    else
                        interactions.SetSelection(asset);
                }
                if (navigation.activate)
                    m_navigation.OpenEntry(context, asset, this);
            }
            else if (navigation.activate)
            {
                m_navigation.NavigateToRoot(context, m_assets.browser.root);
                m_assets.browser.Select(context, null);
            }
            interactions.For(FileBrowserInteractionIds.C_AREA, interactions.selection.selectedTarget).Focus();
        }
        if (navigation.cancel)
        {
            m_keyboardFocus = null;
            m_rootFocused = false;
        }
    }

    private void BeginNavigation()
    {
        ClearNavigationFrame();
        m_drawKeyboardFocus = m_requestedKeyboardFocus;
        m_drawRootFocus = m_requestedRootFocus;
        m_requestedKeyboardFocus = null;
        m_requestedRootFocus = false;
        float rowHeight = Ui.GetFrameHeight() + Ui.GetLayout().itemSpacing.Y;
        m_keyboardPageSize = Math.Max(1, (int)(Ui.GetContentRegionAvailable().Y / rowHeight));
    }

    private bool RequestsKeyboardFocus(
        AssetFileEntry? entry,
        bool isRoot
    ) => isRoot ? m_drawRootFocus : entry is not null && entry.identity.runtimeIdentity == m_drawKeyboardFocus;

    private void ClearNavigationFrame()
    {
        m_pendingSelection = null;
        m_pendingActivation = false;
        m_pendingRootNavigation = false;
    }

    private readonly record struct TreeNavigationRow(RuntimeIdentity? identity);
}
