using UiDrawing = Inno.Editor.ImGui.ImGui;
using EditorUI = Inno.Editor.ImGui.EditorUiSession;
using Inno.Editor.Assets;
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
        PushButtonColors(canGoBack ? EditorPalette.assetAccent : EditorPalette.assetBorderSoft);
        Ui.BeginDisabled(!canGoBack);
        if (Ui.SmallButton($"{ImGuiIcon.AngleLeft}##Back"))
            m_navigation.GoBack(context);
        Ui.EndDisabled();
        Ui.PopStyleColor(3);
        Ui.SameLine(0f, EditorUI.current.widgets.style.assetToolbarTightSpacing);
        bool canGoForward = m_navigation.canGoForward;
        PushButtonColors(canGoForward ? EditorPalette.assetAccent : EditorPalette.assetBorderSoft);
        Ui.BeginDisabled(!canGoForward);
        if (Ui.SmallButton($"{ImGuiIcon.AngleRight}##Forward"))
            m_navigation.GoForward(context);
        Ui.EndDisabled();
        Ui.PopStyleColor(3);
        Ui.SameLine(0f, EditorUI.current.widgets.style.assetToolbarSectionSpacing);
        Ui.PushStyleColor((ImGuiCol.Text), EditorPalette.assetText);
        Ui.Text(GetDirectoryLabel(m_assets.browser.root, current));
        Ui.PopStyleColor();
        Ui.PopStyleVar();
    }

    private void DrawViewAndSearchBar()
    {
        Ui.PushStyleVar((ImGuiStyleVar.FramePadding), EditorUI.current.widgets.style.toolbarFramePadding);
        PushButtonColors(EditorPalette.assetAccent);
        if (Ui.SmallButton($"{m_viewMode}##ViewMode"))
            m_viewMode = m_viewMode == ViewMode.List ? ViewMode.Grid : ViewMode.List;
        Ui.PopStyleColor(3);
        Ui.SameLine(0f, EditorUI.current.widgets.style.assetToolbarSpacing);
        DrawEntryFilterCombo();
        Ui.SameLine(0f, EditorUI.current.widgets.style.assetToolbarSpacing);
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
        if (doubleClicked)
        {
            m_navigation.OpenEntry(context, entry, m_tree);
            return;
        }

        EditorUI.current.widgets.SelectTarget(m_assets.edits.interactions, entry, m_visibleSelection);
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
            float contentHeight = hasHorizontalOverflow ? height - UiDrawing.GetLayout().scrollbarSize : height;
            float itemHeight = EditorUI.current.widgets.GetClickableTextSize("A", framePadding).Y;
            Ui.SetCursorPosY(MathF.Max(0f, (contentHeight - itemHeight) * 0.5f));
            Ui.PushStyleColor((ImGuiCol.Text), EditorPalette.assetBreadcrumbText);
            for (int i = 0; i < parts.Count; i++)
            {
                BreadcrumbPart part = parts[i];
                if (i > 0)
                {
                    Ui.SameLine(0f, EditorUI.current.widgets.style.assetBreadcrumbSpacing);
                    EditorUI.current.widgets.CenteredText(">", new Vector2(Ui.MeasureText(">").X, itemHeight));
                    Ui.SameLine(0f, EditorUI.current.widgets.style.assetBreadcrumbSpacing);
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
        EditorUiLayout style = UiDrawing.GetLayout();
        float itemHeight = EditorUI.current.widgets.GetClickableTextSize("A", EditorUI.current.widgets.style.breadcrumbFramePadding).Y;
        float contentHeight = MathF.Max(
            EditorUI.current.widgets.style.assetBreadcrumbHeight,
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
                width += separatorWidth + EditorUI.current.widgets.style.assetBreadcrumbSpacing * 2f;
            width += Ui.MeasureText(parts[i].label).X + framePadding.X * 2f;
        }

        return MathF.Ceiling(width);
    }
}
