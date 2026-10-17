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

/// <summary>
/// Asset browser panel with a tree pane and filtered table view.
/// </summary>
[EditorPanel("asset.file-browser", "File", order: 300, menuPath: "Content")]
internal sealed partial class FileBrowserPanel : EditorPanel
{
    private const int C_SEARCH_BUFFER_SIZE = 256;
    private const int C_GRID_LABEL_LINE_COUNT = 2;
    private const float C_GRID_ICON_HORIZONTAL_OPTICAL_OFFSET = -1f;

    private readonly AssetEditorModule m_assets;
    private readonly FileBrowserData m_data;
    private readonly FileBrowserNavigation m_navigation;
    private readonly FileBrowserDragDrop m_dragDrop;
    private readonly FileBrowserChangeTracker m_changeTracker;
    private readonly FileBrowserRename m_rename;
    private readonly FileBrowserContextMenu m_contextMenu;
    private readonly FileBrowserTree m_tree;

    private float m_treePaneRatio = 0.5f;
    private string m_filter = string.Empty;
    private ViewMode m_viewMode = ViewMode.List;
    private FileBrowserEntryTypeFilter m_entryTypeFilter = FileBrowserEntryTypeFilter.All;
    private FileBrowserEntryScopeFilter m_entryScopeFilter = FileBrowserEntryScopeFilter.CurrentOnly;
    private float m_gridScale = EditorWidget.style.assetGridDefaultScale;
    private float m_listNameSeparatorPosition = EditorWidget.style.assetListNameSeparatorPosition;
    private float m_listTypeSeparatorPosition = EditorWidget.style.assetListTypeSeparatorPosition;

    private enum ViewMode
    {
        List,
        Grid
    }


    /// <summary>
    /// Keeps scrolling inside the tree, entry list, and breadcrumb regions.
    /// </summary>
    public override bool allowScrolling => false;

    /// <summary>
    /// Captures an immutable snapshot of the current observable state.
    /// </summary>
    /// <param name="state">
    /// The lifecycle or domain state applied by this operation.
    /// </param>
    protected override void Capture(EditorState state)
    {
        state.Set("viewMode", m_viewMode.ToString());
        state.Set("filter", m_filter);
        state.Set("entryTypeFilter", m_entryTypeFilter.ToString());
        state.Set("entryScopeFilter", m_entryScopeFilter.ToString());
        state.Set("treePaneRatio", m_treePaneRatio);
        state.Set("gridScale", m_gridScale);
        state.Set("listNameSeparator", m_listNameSeparatorPosition);
        state.Set("listTypeSeparator", m_listTypeSeparatorPosition);
    }

    /// <summary>
    /// Restores the supplied snapshot while preserving current invariants.
    /// </summary>
    /// <param name="state">
    /// The lifecycle or domain state applied by this operation.
    /// </param>
    protected override void Restore(EditorState state)
    {
        if (Enum.TryParse(state.Get("viewMode", string.Empty), out ViewMode viewMode))
            m_viewMode = viewMode;
        if (Enum.TryParse(
                state.Get("entryTypeFilter", string.Empty),
                out FileBrowserEntryTypeFilter typeFilter))
        {
            m_entryTypeFilter = typeFilter;
        }
        if (Enum.TryParse(
                state.Get("entryScopeFilter", string.Empty),
                out FileBrowserEntryScopeFilter scopeFilter))
        {
            m_entryScopeFilter = scopeFilter;
        }
        m_filter = state.Get("filter", string.Empty);
        float restoredTreePaneRatio = state.Get("treePaneRatio", 0.5f);
        m_treePaneRatio = float.IsFinite(restoredTreePaneRatio)
            ? Math.Clamp(restoredTreePaneRatio, 0f, 1f)
            : 0.5f;
        m_gridScale = Math.Clamp(
            state.Get("gridScale", EditorWidget.style.assetGridDefaultScale),
            EditorWidget.style.assetGridMinimumScale,
            EditorWidget.style.assetGridMaximumScale);
        SetListColumnSeparators(
            state.Get("listNameSeparator", EditorWidget.style.assetListNameSeparatorPosition),
            state.Get("listTypeSeparator", EditorWidget.style.assetListTypeSeparatorPosition));
    }

    /// <summary>
    /// Creates the panel.
    /// </summary>
    internal FileBrowserPanel(AssetEditorModule assets)
    {
        m_assets = assets;
        m_data = new FileBrowserData(assets);
        m_navigation = new FileBrowserNavigation(assets);
        m_dragDrop = new FileBrowserDragDrop(assets);
        m_changeTracker = new FileBrowserChangeTracker(assets);
        m_rename = new FileBrowserRename(assets);
        m_contextMenu = new FileBrowserContextMenu(assets, m_rename);
        m_tree = new FileBrowserTree(
            m_data,
            m_navigation,
            m_dragDrop,
            m_rename,
            m_contextMenu,
            assets);
    }

    /// <summary>
    /// Attaches this feature to its owning runtime generation.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    protected override void OnAttach(EditorContext context)
    {
        m_changeTracker.Attach(context);
    }

    /// <summary>
    /// Detaches this feature and releases generation-scoped state.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    protected override void OnDetach(EditorContext context)
    {
        m_changeTracker.Detach();
    }

    /// <summary>
    /// Draws this feature using the current editor presentation context.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    protected override void OnDraw(EditorContext context)
    {
        m_rename.Update(context);
        PushBrowserStyle();
        try
        {
            DrawBrowser(context);
        }
        finally
        {
            PopBrowserStyle();
        }
    }

}
