using EditorUI = Inno.Editor.ImGui.EditorUiSession;
using Inno.Editor.Assets;
using Inno.Core.Identity;
using Inno.Editor.Interactions;
using Inno.Editor.Assets.ImGui;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Editor.Core;
using Inno.Editor.ImGui;
using Inno.Editor.ImGui.ImGuiWidget;
using static Inno.Editor.Panel.FileBrowser.FileBrowserUtility;
using Ui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Panel.FileBrowser;
internal sealed partial class FileBrowserPanel
{
    private int CollectPreservedEntryRows(
        IReadOnlyList<FileBrowserDisplayEntry> entries,
        Span<int> rows,
        int columns = 1
    ) {
        RuntimeIdentity? editing = m_rename.activeTarget;
        RuntimeIdentity? dragging = m_assets.edits.interactions.TryGetActiveDragIdentity(out RuntimeIdentity source) ? source : null;
        if (editing is null && dragging is null && m_drawKeyboardFocus is null)
            return 0;
        int count = 0;
        for (int index = 0; index < entries.Count; index++)
        {
            RuntimeIdentity identity = entries[index].identity;
            if (identity == editing || identity == dragging || identity == m_drawKeyboardFocus)
                rows[count++] = index / columns;
        }
        return count;
    }

    private void DrawToolbar(EditorContext context)
    {
        DrawNavigationBar(context);
        DrawViewAndSearchBar();
    }

    private void DrawNavigationBar(EditorContext context)
    {
        string current = m_assets.browser.currentDirectory;
        Ui.PushStyleVar((ImGuiStyleVar.FramePadding), EditorUI.current.widgets.style.breadcrumbFramePadding);
        bool canGoBack = m_navigation.canGoBack;
        PushButtonColors(canGoBack ? EditorPalette.contentAccent : EditorPalette.contentBorderSoft);
        Ui.BeginDisabled(!canGoBack);
        if (Ui.SmallButton($"{ImGuiIcon.AngleLeft}##Back"))
            m_navigation.GoBack(context);
        Ui.EndDisabled();
        Ui.PopStyleColor(3);
        Ui.SameLine(0f, EditorUI.current.widgets.style.toolbarTightSpacing);
        bool canGoForward = m_navigation.canGoForward;
        PushButtonColors(canGoForward ? EditorPalette.contentAccent : EditorPalette.contentBorderSoft);
        Ui.BeginDisabled(!canGoForward);
        if (Ui.SmallButton($"{ImGuiIcon.AngleRight}##Forward"))
            m_navigation.GoForward(context);
        Ui.EndDisabled();
        Ui.PopStyleColor(3);
        Ui.SameLine(0f, EditorUI.current.widgets.style.toolbarSectionSpacing);
        Ui.PushStyleColor((ImGuiCol.Text), EditorPalette.contentText);
        Ui.Text(GetDirectoryLabel(m_assets.browser.root, current));
        Ui.PopStyleColor();
        Ui.PopStyleVar();
    }

    private void DrawViewAndSearchBar()
    {
        Ui.PushStyleVar((ImGuiStyleVar.FramePadding), EditorUI.current.widgets.style.toolbarFramePadding);
        PushButtonColors(EditorPalette.contentAccent);
        if (Ui.SmallButton($"{m_viewMode}##ViewMode"))
            m_viewMode = m_viewMode == ViewMode.List ? ViewMode.Grid : ViewMode.List;
        Ui.PopStyleColor(3);
        Ui.SameLine(0f, EditorUI.current.widgets.style.toolbarSpacing);
        DrawEntryFilterCombo();
        Ui.SameLine(0f, EditorUI.current.widgets.style.toolbarSpacing);
        Ui.SetNextItemWidth(-1f);
        DrawSearchInput();
        Ui.PopStyleVar();
    }

    private void HandleEntryActivation(
        EditorContext context,
        AssetFileEntry entry,
        FileBrowserPresentation presentation,
        bool doubleClicked
    ) {
        m_rename.MarkInteraction(presentation);
        m_keyboardFocus = entry.identity.runtimeIdentity;
        m_assets.edits.interactions.For(FileBrowserInteractionIds.C_AREA, entry).Focus();
        if (doubleClicked)
        {
            m_pendingActivation = entry.identity.runtimeIdentity;
            return;
        }

        EditorUI.current.widgets.SelectTarget(m_assets.edits.interactions, entry, m_visibleSelection);
    }

    private void HandleEntryKeyboardNavigation(int pageSize)
    {
        EditorInteractions interactions = m_assets.edits.interactions;
        int focus = -1;
        int selectedIndex = -1;
        RuntimeIdentity? selected = (interactions.selection.selectedTarget as IdentityObject)?.identity.runtimeIdentity;
        for (int i = 0; i < m_visibleSelection.Count; i++)
        {
            RuntimeIdentity candidate = m_visibleSelection[i];
            if (candidate == m_keyboardFocus)
            {
                focus = i;
                break;
            }
            if (candidate == selected)
                selectedIndex = i;
        }
        if (focus < 0)
            focus = selectedIndex;
        ListNavigationResult navigation = m_ui.widgets.NavigateList(interactions, m_visibleSelection.Count, focus, pageSize);
        m_keyboardHandled = navigation.focusChanged || navigation.selectionRequested || navigation.activate || navigation.cancel;
        if (navigation.focusedIndex >= 0 && m_keyboardHandled)
        {
            RuntimeIdentity identity = m_visibleSelection[navigation.focusedIndex];
            if (!interactions.TryResolveIdentity(identity, out IdentityObject? target) || target is null)
                return;
            m_keyboardFocus = identity;
            if (navigation.focusChanged)
            {
                m_requestedKeyboardFocus = m_keyboardFocus;
                m_drawKeyboardFocus = m_keyboardFocus;
            }
            if (navigation.selectionRequested)
            {
                if (navigation.extendRange)
                    interactions.SelectRange(m_visibleSelection, target, navigation.additive);
                else if (navigation.toggle)
                    interactions.ToggleSelection(target);
                else
                    interactions.SetSelection(target);
            }
            interactions.For(FileBrowserInteractionIds.C_AREA, interactions.selection.selectedTarget).Focus();
            if (navigation.activate)
                m_pendingActivation = m_keyboardFocus;
        }
        if (navigation.cancel)
            m_keyboardFocus = null;
    }

    private void RequestEntryKeyboardFocus(AssetFileEntry entry)
    {
        if (entry.identity.runtimeIdentity == m_drawKeyboardFocus && Ui.IsWindowFocused(ImGuiFocusedFlags.ChildWindows))
            Ui.FocusNextItem();
    }

    private void DrawBreadcrumbBar(
        EditorContext context,
        float height
    ) {
        IReadOnlyList<BreadcrumbPart> parts = BuildBreadcrumbParts(m_assets.browser.root, m_assets.browser.currentDirectory);
        Vector2 framePadding = EditorUI.current.widgets.style.breadcrumbFramePadding;
        float contentWidth = CalculateBreadcrumbContentWidth(parts, framePadding);
        float availableWidth = Ui.GetContentRegionAvailable().X;
        bool hasHorizontalOverflow = contentWidth > availableWidth;
        Ui.SetNextWindowContentSize(new Vector2(hasHorizontalOverflow ? contentWidth : 0f, 0f));
        Ui.PushStyleVar((ImGuiStyleVar.WindowPadding), Vector2.Zero);
        if (Ui.BeginChild("##BreadcrumbBar", new Vector2(0f, height), (ImGuiChildFlags.None), (ImGuiWindowFlags.HorizontalScrollbar)))
        {
            DrawBreadcrumbTopSeparator();
            float contentHeight = hasHorizontalOverflow ? height - Ui.GetLayout().scrollbarSize : height;
            float itemHeight = EditorUI.current.widgets.GetClickableTextSize("A", framePadding).Y;
            Ui.SetCursorPosY(MathF.Max(0f, (contentHeight - itemHeight) * 0.5f));
            Ui.PushStyleColor((ImGuiCol.Text), EditorPalette.contextText);
            for (int i = 0; i < parts.Count; i++)
            {
                BreadcrumbPart part = parts[i];
                if (i > 0)
                {
                    Ui.SameLine(0f, CurrentBrowserStyle.breadcrumbSpacing);
                    EditorUI.current.widgets.CenteredText(">", new Vector2(Ui.MeasureText(">").X, itemHeight));
                    Ui.SameLine(0f, CurrentBrowserStyle.breadcrumbSpacing);
                }

                Vector2 itemSize = EditorUI.current.widgets.GetClickableTextSize(part.label, framePadding);
                if (EditorUI.current.widgets.ClickableText($"crumb_{part.root}_{part.directory}", part.label, itemSize))
                {
                    if (string.IsNullOrEmpty(part.directory))
                        m_navigation.NavigateToRoot(context, part.root);
                    else
                        m_navigation.NavigateTo(context, part.directory);
                }
            }

            Ui.PopStyleColor();
        }

        Ui.EndChild();
        Ui.PopStyleVar();
    }

    private static float GetBreadcrumbBarHeight(
        AssetBrowserRoot root,
        string currentDirectory
    ) {
        IReadOnlyList<BreadcrumbPart> parts = BuildBreadcrumbParts(root, currentDirectory);
        EditorUiLayout style = Ui.GetLayout();
        float itemHeight = EditorUI.current.widgets.GetClickableTextSize("A", EditorUI.current.widgets.style.breadcrumbFramePadding).Y;
        float contentHeight = MathF.Max(
            CurrentBrowserStyle.breadcrumbHeight,
            MathF.Ceiling(itemHeight + style.itemSpacing.Y * 2f + EditorUI.current.widgets.style.borderSize)
        );
        float contentWidth = CalculateBreadcrumbContentWidth(parts, EditorUI.current.widgets.style.breadcrumbFramePadding);
        return contentWidth > Ui.GetContentRegionAvailable().X ? contentHeight + style.scrollbarSize : contentHeight;
    }

    private static float CalculateBreadcrumbContentWidth(
        IReadOnlyList<BreadcrumbPart> parts,
        Vector2 framePadding
    ) {
        if (parts.Count == 0)
            return 0f;
        float width = 0f;
        float separatorWidth = Ui.MeasureText(">").X;
        for (int i = 0; i < parts.Count; i++)
        {
            if (i > 0)
                width += separatorWidth + CurrentBrowserStyle.breadcrumbSpacing * 2f;
            width += Ui.MeasureText(parts[i].label).X + framePadding.X * 2f;
        }

        return MathF.Ceiling(width);
    }
}
