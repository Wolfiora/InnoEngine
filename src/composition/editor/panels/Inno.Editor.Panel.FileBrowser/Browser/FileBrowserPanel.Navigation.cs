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
using EditorWidget = Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget;
using Inno.Native.ImGui;
using Inno.Adapter.Presentation.ImGui;
using static Inno.Editor.Panel.FileBrowser.FileBrowserUtility;
using NativeImGui = Inno.Native.ImGui.ImGui;

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

        NativeImGui.PushStyleVar(ImGuiStyleVar.FramePadding, EditorWidget.style.breadcrumbFramePadding);
        bool canGoBack = m_navigation.canGoBack;
        PushButtonColors(canGoBack ? EditorPalette.assetAccent : EditorPalette.assetBorderSoft);

        NativeImGui.BeginDisabled(!canGoBack);
        if (NativeImGui.SmallButton($"{ImGuiIcon.AngleLeft}##Back"))
            m_navigation.GoBack(context);

        NativeImGui.EndDisabled();
        NativeImGui.PopStyleColor(3);

        NativeImGui.SameLine(0f, EditorWidget.style.assetToolbarTightSpacing);
        bool canGoForward = m_navigation.canGoForward;
        PushButtonColors(canGoForward ? EditorPalette.assetAccent : EditorPalette.assetBorderSoft);
        NativeImGui.BeginDisabled(!canGoForward);
        if (NativeImGui.SmallButton($"{ImGuiIcon.AngleRight}##Forward"))
            m_navigation.GoForward(context);

        NativeImGui.EndDisabled();
        NativeImGui.PopStyleColor(3);

        NativeImGui.SameLine(0f, EditorWidget.style.assetToolbarSectionSpacing);

        NativeImGui.PushStyleColor(ImGuiCol.Text, EditorPalette.assetText);
        NativeImGui.TextUnformatted(GetDirectoryLabel(m_assets.browser.root, current));
        NativeImGui.PopStyleColor();

        NativeImGui.PopStyleVar();
    }

    private void DrawViewAndSearchBar()
    {
        NativeImGui.PushStyleVar(ImGuiStyleVar.FramePadding, EditorWidget.style.toolbarFramePadding);

        PushButtonColors(EditorPalette.assetAccent);
        if (NativeImGui.SmallButton($"{m_viewMode}##ViewMode"))
            m_viewMode = m_viewMode == ViewMode.List ? ViewMode.Grid : ViewMode.List;
        NativeImGui.PopStyleColor(3);

        NativeImGui.SameLine(0f, EditorWidget.style.assetToolbarSpacing);
        DrawEntryFilterCombo();
        NativeImGui.SameLine(0f, EditorWidget.style.assetToolbarSpacing);

        NativeImGui.SetNextItemWidth(-1f);
        DrawSearchInput();
        NativeImGui.PopStyleVar();
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

        if (NativeImGui.GetIO().KeyCtrl || NativeImGui.GetIO().KeySuper)
            m_assets.browser.ToggleSelection(context, entry);
        else
            m_assets.browser.Select(context, entry.assetPath.ToString());
    }

    private void DrawBreadcrumbBar(
        EditorContext context,
        float height
    ) {
        IReadOnlyList<BreadcrumbPart> parts = BuildBreadcrumbParts(
            m_assets.browser.root,
            m_assets.browser.currentDirectory);
        Vector2 framePadding = EditorWidget.style.breadcrumbFramePadding;
        float contentWidth = CalculateBreadcrumbContentWidth(parts, framePadding);
        float availableWidth = NativeImGui.GetContentRegionAvail().X;
        bool hasHorizontalOverflow = contentWidth > availableWidth;
        NativeImGui.SetNextWindowContentSize(new Vector2(
            hasHorizontalOverflow ? contentWidth : 0f,
            0f));
        NativeImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, Vector2.Zero);
        if (NativeImGui.BeginChild("##BreadcrumbBar", new Vector2(0f, height), ImGuiChildFlags.None, ImGuiWindowFlags.HorizontalScrollbar))
        {
            DrawBreadcrumbTopSeparator();
            float contentHeight = hasHorizontalOverflow
                ? height - NativeImGui.GetStyle().ScrollbarSize
                : height;
            float itemHeight = EditorWidget.GetClickableTextSize("A", framePadding).Y;
            NativeImGui.SetCursorPosY(MathF.Max(0f, (contentHeight - itemHeight) * 0.5f));
            NativeImGui.PushStyleColor(ImGuiCol.Text, EditorPalette.assetBreadcrumbText);

            for (int i = 0; i < parts.Count; i++)
            {
                BreadcrumbPart part = parts[i];
                if (i > 0)
                {
                    NativeImGui.SameLine(0f, EditorWidget.style.assetBreadcrumbSpacing);
                    EditorWidget.CenteredText(">", new Vector2(
                        NativeImGui.CalcTextSize(">").X,
                        itemHeight));
                    NativeImGui.SameLine(0f, EditorWidget.style.assetBreadcrumbSpacing);
                }

                Vector2 itemSize = EditorWidget.GetClickableTextSize(part.label, framePadding);
                if (EditorWidget.ClickableText(
                        $"crumb_{part.root}_{part.directory}",
                        part.label,
                        itemSize))
                {
                    if (string.IsNullOrEmpty(part.directory))
                        m_navigation.NavigateToRoot(context, part.root);
                    else
                        m_navigation.NavigateTo(context, part.directory);
                }
            }

            NativeImGui.PopStyleColor();
        }

        NativeImGui.EndChild();
        NativeImGui.PopStyleVar();
    }

    private static float GetBreadcrumbBarHeight(
        AssetBrowserRoot root,
        string currentDirectory
    ) {
        IReadOnlyList<BreadcrumbPart> parts = BuildBreadcrumbParts(root, currentDirectory);
        ImGuiStylePtr style = NativeImGui.GetStyle();
        float itemHeight = EditorWidget.GetClickableTextSize(
            "A",
            EditorWidget.style.breadcrumbFramePadding).Y;
        float contentHeight = MathF.Max(
            EditorWidget.style.assetBreadcrumbHeight,
            MathF.Ceiling(itemHeight + style.ItemSpacing.Y * 2f + EditorWidget.style.borderSize));
        float contentWidth = CalculateBreadcrumbContentWidth(
            parts,
            EditorWidget.style.breadcrumbFramePadding);
        return contentWidth > NativeImGui.GetContentRegionAvail().X
            ? contentHeight + style.ScrollbarSize
            : contentHeight;
    }

    private static float CalculateBreadcrumbContentWidth(
        IReadOnlyList<BreadcrumbPart> parts,
        Vector2 framePadding
    ) {
        if (parts.Count == 0)
            return 0f;

        float width = 0f;
        float separatorWidth = NativeImGui.CalcTextSize(">").X;
        for (int i = 0; i < parts.Count; i++)
        {
            if (i > 0)
                width += separatorWidth + EditorWidget.style.assetBreadcrumbSpacing * 2f;

            width += NativeImGui.CalcTextSize(parts[i].label).X + framePadding.X * 2f;
        }

        return MathF.Ceiling(width);
    }

}
