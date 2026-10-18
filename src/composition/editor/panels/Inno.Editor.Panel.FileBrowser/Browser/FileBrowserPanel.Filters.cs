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

    private void DrawEntryFilterCombo()
    {
        if (!EditorWidget.BeginBoundedCombo(
                "##AssetEntryFilter",
                "Filter",
                ImGuiComboFlags.WidthFitPreview))
            return;

        DrawEntryTypeFilterOption("All", FileBrowserEntryTypeFilter.All);
        DrawEntryTypeFilterOption("Folders", FileBrowserEntryTypeFilter.FoldersOnly);
        DrawEntryTypeFilterOption("Files", FileBrowserEntryTypeFilter.FilesOnly);
        NativeImGui.Separator();
        DrawEntryScopeFilterOption("Current", FileBrowserEntryScopeFilter.CurrentOnly);
        DrawEntryScopeFilterOption("Recursive", FileBrowserEntryScopeFilter.Recursive);

        EditorWidget.EndBoundedCombo();
    }

    private void DrawEntryTypeFilterOption(
        string label,
        FileBrowserEntryTypeFilter filter
    ) {
        bool selected = m_entryTypeFilter == filter;
        if (NativeImGui.Checkbox(label, ref selected))
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
        if (NativeImGui.Checkbox(label, ref selected))
        {
            if (selected)
                m_entryScopeFilter = filter;
            else if (m_entryScopeFilter == filter)
                m_entryScopeFilter = FileBrowserEntryScopeFilter.CurrentOnly;
        }
    }

    private void DrawSearchInput()
    {
        _ = EditorWidget.SearchInput(
            "AssetSearch",
            ImGuiIcon.MagnifyingGlass,
            ref m_filter,
            (nuint)C_SEARCH_BUFFER_SIZE);
    }

}
