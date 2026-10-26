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
    private void DrawEntryFilterCombo()
    {
        if (!EditorUI.current.widgets.BeginBoundedCombo("##AssetEntryFilter", "Filter", ImGuiComboFlags.WidthFitPreview))
            return;
        DrawEntryTypeFilterOption("All", FileBrowserEntryTypeFilter.All);
        DrawEntryTypeFilterOption("Folders", FileBrowserEntryTypeFilter.FoldersOnly);
        DrawEntryTypeFilterOption("Files", FileBrowserEntryTypeFilter.FilesOnly);
        Ui.Separator();
        DrawEntryScopeFilterOption("Current", FileBrowserEntryScopeFilter.CurrentOnly);
        DrawEntryScopeFilterOption("Recursive", FileBrowserEntryScopeFilter.Recursive);
        EditorUI.current.widgets.EndBoundedCombo();
    }

    private void DrawEntryTypeFilterOption(
        string label,
        FileBrowserEntryTypeFilter filter
    ) {
        bool selected = m_entryTypeFilter == filter;
        if (Ui.Checkbox(label, ref selected))
        {
            if (selected)
                m_entryTypeFilter = filter;
            else if (m_entryTypeFilter == filter)
                m_entryTypeFilter = FileBrowserEntryTypeFilter.All;
        }
    }

    private void DrawEntryScopeFilterOption(
        string label,
        FileBrowserEntryScopeFilter filter
    ) {
        bool selected = m_entryScopeFilter == filter;
        if (Ui.Checkbox(label, ref selected))
        {
            if (selected)
                m_entryScopeFilter = filter;
            else if (m_entryScopeFilter == filter)
                m_entryScopeFilter = FileBrowserEntryScopeFilter.CurrentOnly;
        }
    }

    private void DrawSearchInput()
    {
        _ = EditorUI.current.widgets.SearchInput("AssetSearch", ImGuiIcon.MagnifyingGlass, ref m_filter, (nuint)C_SEARCH_BUFFER_SIZE);
    }
}
