using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Inno.Core.Logging;
using Inno.Editor.Core;
using Inno.Editor.Diagnostics;
using Inno.Editor.ImGui;
using Inno.Editor.ImGui.ImGuiWidget;
using Inno.Editor.Interactions;

using EditorUI = Inno.Editor.ImGui.EditorUISession;
using Gui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Panel.Logging;

/// <summary>
/// Displays append-only logs and current diagnostics in the unified editor Console.
/// </summary>
[EditorPanel("diagnostics.console", "Console", order: 400, menuPath: "Diagnostics")]
internal sealed partial class ConsolePanel : EditorPanel
{
#region State
    private readonly ConsolePanelContent m_content = new();
    private readonly HashSet<LogLevel> m_filterLevels = Enum.GetValues<LogLevel>().ToHashSet();
    private readonly LogLevel[] m_levels = Enum.GetValues<LogLevel>();
    private readonly HashSet<string> m_openEntries = new(StringComparer.Ordinal);
    private readonly ConsoleView m_view = new();
    private string m_query = string.Empty;
    private string? m_focusedKey;
    private string? m_selectedKey;
    private string? m_contextKey;
    private bool m_requestContextMenu;
    private bool m_keyboardHandled;
    private object? m_focusedCopyTarget;
    private bool m_collapse = true;
    private long m_lastRevision = -1;
    private bool m_requestScrollToBottom = true;
    private readonly IEditorConsole m_console;
    private readonly EditorInteractions m_interactions;
#endregion
    /// <summary>
    /// Lets the Console entries region own vertical scrolling.
    /// </summary>
    public override bool allowScrolling => false;

#region Lifecycle
    /// <summary>
    /// Creates the panel.
    /// </summary>
    /// <param name="console">
    /// The editor Console contract that owns snapshots and explicit retention operations.
    /// </param>
    /// <param name="interactions">
    /// The editor interaction entry point used by contextual entry commands.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name = "console"/> or <paramref name="interactions"/> is <see langword="null"/>.
    /// </exception>
    internal ConsolePanel(
        IEditorConsole console,
        EditorInteractions interactions
    ) {
        m_console = console ?? throw new ArgumentNullException(nameof(console));
        m_interactions = interactions ?? throw new ArgumentNullException(nameof(interactions));
    }

    /// <summary>
    /// Captures the persistent Console layout preferences without retaining log content.
    /// </summary>
    /// <param name="state">
    /// The project-level panel state that receives stable presentation values.
    /// </param>
    protected override void Capture(EditorState state) => state.Set("collapse", m_collapse);

    /// <summary>
    /// Restores persistent Console layout preferences without restoring historical log content.
    /// </summary>
    /// <param name="state">
    /// The project-level panel state containing stable presentation values.
    /// </param>
    protected override void Restore(EditorState state) => m_collapse = state.Get("collapse", true);

    /// <inheritdoc/>
    protected override void OnDetach(EditorContext context)
    {
        ClearFocus();
        m_view.Clear();
        m_openEntries.Clear();
        ClearLayout();
        m_lastRevision = -1;
        m_requestScrollToBottom = true;
    }

    /// <summary>
    /// Draws this feature using the current editor presentation context.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    protected override void OnDraw(EditorContext context)
    {
        Gui.BeginChild(
            "ConsoleChild",
            Vector2.Zero,
            (ImGuiChildFlags.None),
            (ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
        );
        try
        {
            DrawToolbar();
            Gui.Separator();
            DrawConsoleRegion();
        }
        finally
        {
            Gui.EndChild();
        }
    }

#endregion
#region Toolbar
    private static ConsoleStyle consoleStyle => new(EditorUI.current.widgets.style);

    private void DrawToolbar()
    {
        if (EditorUI.current.widgets.SearchInput("console", "Search messages, sources and files", ref m_query, 1024))
        {
            m_requestScrollToBottom = false;
        }
        bool collapseChanged = Gui.Checkbox("Collapse", ref m_collapse);
        if (collapseChanged)
        {
            m_requestScrollToBottom = true;
        }

        Gui.SameLine(0f, -1f);
        bool filterChanged = DrawFilterCombo();
        if (filterChanged)
        {
            m_requestScrollToBottom = true;
        }

        Gui.SameLine(0f, -1f);
        if (Gui.Button("Clear"))
        {
            m_console.Clear();
            m_openEntries.Clear();
            ClearFocus();
            m_requestScrollToBottom = true;
        }
    }

    private bool DrawFilterCombo()
    {
        if (!EditorUI.current.widgets.BeginBoundedCombo("##ConsoleFilter", "Filter", ImGuiComboFlags.WidthFitPreview))
        {
            return false;
        }

        bool changed = false;
        for (int i = 0; i < m_levels.Length; i++)
        {
            LogLevel level = m_levels[i];
            bool selected = m_filterLevels.Contains(level);
            if (!Gui.Checkbox(level.ToString(), ref selected))
            {
                continue;
            }

            if (selected)
            {
                m_filterLevels.Add(level);
            }
            else
            {
                m_filterLevels.Remove(level);
            }

            changed = true;
        }

        EditorUI.current.widgets.EndBoundedCombo();
        return changed;
    }

#endregion
#region Console Region
    private void DrawConsoleRegion()
    {
        Gui.BeginChild("ConsoleRegion", Vector2.Zero, 0, 0);
        try
        {
            EditorConsoleSnapshot snapshot = m_console.Capture();
            if (m_lastRevision != snapshot.revision)
            {
                RemoveStaleOpenEntries(snapshot);
            }
            bool viewChanged = m_view.Update(snapshot, m_query, m_filterLevels, m_collapse);
            if (viewChanged)
            {
                int focused = m_view.FindIndex(m_focusedKey);
                if (focused < 0)
                {
                    ClearFocus();
                }
                else if (m_interactions.focusedArea == LoggingInteractionIDs.C_ENTRY_AREA)
                {
                    FocusRow(m_view.rows[focused], select: false);
                }
            }
            bool hasNewEntries = m_lastRevision >= 0 && snapshot.revision != m_lastRevision;
            m_lastRevision = snapshot.revision;
            bool scrollAtBottom = Gui.GetScrollY() >= Gui.GetScrollMaxY() - consoleStyle.autoScrollTolerance;
            bool shouldScrollToBottom = m_requestScrollToBottom || (hasNewEntries && scrollAtBottom);
            HandleKeyboardNavigation();
            if (m_keyboardHandled)
            {
                m_requestScrollToBottom = false;
            }
            DrawEntries(snapshot);
            DrawEntryContextMenu();
            if (shouldScrollToBottom && !m_keyboardHandled)
            {
                Gui.SetScrollHereY(1f);
                m_requestScrollToBottom = false;
            }
        }
        finally
        {
            Gui.EndChild();
        }
    }

#endregion
#region Entries
    private void ClearFocus()
    {
        m_focusedKey = null;
        m_selectedKey = null;
        m_focusedCopyTarget = null;
        m_contextKey = null;
        m_requestContextMenu = false;
        if (m_interactions.focusedArea == LoggingInteractionIDs.C_ENTRY_AREA)
        {
            m_interactions.For(LoggingInteractionIDs.C_ENTRY_AREA).Focus();
        }
    }

    private void DrawEntries(EditorConsoleSnapshot snapshot)
    {
        if (snapshot.occurrences.Count == 0)
        {
            ClearLayout();
            m_content.DrawDisabledHint("No logs or diagnostics yet.");
            return;
        }

        if (m_view.rows.Count == 0)
        {
            ClearLayout();
            m_content.DrawDisabledHint("No entries match the current search and filters.");
            return;
        }
        DrawVisibleEntries();
    }

#endregion
#region Entry Card
    private float DrawConsoleEntry(
        ConsoleView.Row row,
        bool measure
    ) {
        (Vector4 levelColor, string levelIcon) = m_content.GetLevelVisual(row.latest.level);
        return DrawEntryCard(row, levelColor, levelIcon, measure);
    }

    private float DrawEntryCard(
        ConsoleView.Row row,
        Vector4 levelColor,
        string levelIcon,
        bool measure
    ) {
        bool open = m_openEntries.Contains(row.key);
        Vector4 cardBg = open ? m_content.GetExpandedBgColor(levelColor) : m_content.GetCollapsedBgColor();
        Vector4 cardBorder = row.key == m_selectedKey
                             ? EditorPalette.contentAccent
                             : open ? ConsoleStyle.GetExpandedBorder(levelColor) : m_content.GetCollapsedBorderColor();
        Gui.PushStyleColor((ImGuiCol.FrameBg), cardBg);
        Gui.PushStyleColor((ImGuiCol.Border), cardBorder);
        Gui.PushStyleVar((ImGuiStyleVar.FrameRounding), Gui.GetLayout().frameRounding);
        Gui.PushStyleVar((ImGuiStyleVar.FrameBorderSize), EditorUI.current.widgets.style.borderSize);
        Gui.PushStyleVar((ImGuiStyleVar.FramePadding), EditorUI.current.widgets.style.framePadding);
        try
        {
            ImGuiChildFlags childFlags = ImGuiChildFlags.FrameStyle | (open ? ImGuiChildFlags.AutoResizeY : ImGuiChildFlags.None);
            if (open && measure)
            {
                childFlags |= ImGuiChildFlags.AlwaysAutoResize;
            }
            ImGuiWindowFlags windowFlags = ImGuiWindowFlags.NoScrollbar
                                           | ImGuiWindowFlags.NoScrollWithMouse
                                           | ImGuiWindowFlags.NoSavedSettings;
            float height = open ? 0f : Gui.GetFrameHeight() + Gui.GetLayout().framePadding.Y * 2f;
            bool visible = Gui.BeginChild(AcquireCardWindow(row.key), new Vector2(0f, height), childFlags, windowFlags);
            try
            {
                if (visible)
                {
                    ConsoleEntryImGuiIdentity.Push(row.key);
                    try
                    {
                        DrawHeader(row.latest, row.occurrences.Count, levelColor, levelIcon, open, cardBg, out bool toggled);
                        if (toggled)
                        {
                            ToggleEntry(row.key);
                        }
                        if (open)
                        {
                            DrawOccurrences(row.occurrences, cardBg);
                        }
                    }
                    finally
                    {
                        ConsoleEntryImGuiIdentity.Pop();
                    }
                }
            }
            finally
            {
                Gui.EndChild();
            }
            float measuredHeight = Gui.GetItemMaximum().Y - Gui.GetItemMinimum().Y;
            if (Gui.IsItemClicked(ImGuiMouseButton.Left) || Gui.IsItemClicked(ImGuiMouseButton.Right))
            {
                FocusRow(row, select: true);
            }
            if (Gui.IsItemHovered() && Gui.IsMouseReleased(ImGuiMouseButton.Right))
            {
                m_contextKey = row.key;
                m_requestContextMenu = true;
            }
            return measuredHeight;
        }
        finally
        {
            Gui.PopStyleVar(3);
            Gui.PopStyleColor(2);
        }
    }

    private static void DrawOccurrences(
        IReadOnlyList<EditorConsoleOccurrence> occurrences,
        Vector4 cardBackground
    ) {
        Gui.PushStyleColor(ImGuiCol.Separator, ConsoleStyle.GetSeparator(cardBackground));
        try
        {
            Gui.Separator();
            for (int i = 0; i < occurrences.Count; i++)
            {
                if (i > 0)
                {
                    Gui.Separator();
                }
                Gui.PushID(unchecked((int)occurrences[i].sequence));
                try
                {
                    DrawDetail(occurrences[i]);
                }
                finally
                {
                    Gui.PopID();
                }
            }
        }
        finally
        {
            Gui.PopStyleColor();
        }
    }

#endregion
#region Entry Header / Detail
    private void DrawHeader(
        EditorConsoleOccurrence entry,
        int repeatCount,
        Vector4 levelColor,
        string levelIcon,
        bool isOpen,
        Vector4 headerBgColor,
        out bool toggled
    ) {
        toggled = false;
        EditorUILayout style = Gui.GetLayout();
        string content = isOpen ? entry.displayMessage : m_content.GetFirstLine(entry.displayMessage);
        string repeatText = repeatCount > 1 ? $" (x{repeatCount})" : string.Empty;
        string prefix = $"{levelIcon} [{entry.level}] ";
        string toggleText = isOpen ? "▼" : "▶";
        float toggleW = Gui.MeasureText(toggleText).X + consoleStyle.disclosurePadding.X * 2f;
        float prefixW = Gui.MeasureText(prefix).X + toggleW + style.itemInnerSpacing.X;
        float suffixW = Gui.MeasureText(repeatText).X;
        float suffixColW = MathF.Max(1f, suffixW);
        float tableOuterW = MathF.Max(1f, Gui.GetContentRegionAvailable().X);
        float contentW = MathF.Max(1f, tableOuterW - prefixW - suffixColW);
        if (!isOpen)
        {
            content = m_content.FitTextWithEllipsis(m_content.GetFirstLine(entry.displayMessage), contentW);
        }

        var tableOuterSize = new Vector2(tableOuterW, 0f);
        ImGuiTableFlags flags = ImGuiTableFlags.SizingFixedFit
                                | ImGuiTableFlags.NoHostExtendX
                                | ImGuiTableFlags.NoPadOuterX
                                | ImGuiTableFlags.NoPadInnerX
                                | ImGuiTableFlags.NoSavedSettings;
        if (Gui.BeginTable("##HeaderTable", 3, flags, tableOuterSize))
        {
            try
            {
                Gui.TableSetupColumn("##prefix", (ImGuiTableColumnFlags.WidthFixed | ImGuiTableColumnFlags.NoResize), prefixW);
                Gui.TableSetupColumn("##content", (ImGuiTableColumnFlags.WidthFixed | ImGuiTableColumnFlags.NoResize), contentW);
                Gui.TableSetupColumn("##suffix", (ImGuiTableColumnFlags.WidthFixed | ImGuiTableColumnFlags.NoResize), suffixColW);
                Gui.TableNextRow();
                if (isOpen)
                {
                    uint headerBgU32 = Gui.ToPackedColor(headerBgColor);
                    Gui.TableSetBgColor((ImGuiTableBgTarget.RowBg0), headerBgU32);
                }

                _ = Gui.TableSetColumnIndex(0);
                Gui.PushStyleVar((ImGuiStyleVar.FramePadding), consoleStyle.disclosurePadding);
                Gui.PushStyleVar((ImGuiStyleVar.FrameBorderSize), 0f);
                Gui.PushStyleColor((ImGuiCol.Button), ConsoleStyle.toggle);
                Gui.PushStyleColor((ImGuiCol.ButtonHovered), ConsoleStyle.toggleHovered);
                Gui.PushStyleColor((ImGuiCol.ButtonActive), ConsoleStyle.toggleActive);
                try
                {
                    if (Gui.SmallButton($"{toggleText}##HeaderToggle") && !m_keyboardHandled)
                    {
                        toggled = true;
                    }
                }
                finally
                {
                    Gui.PopStyleColor(3);
                    Gui.PopStyleVar(2);
                }
                Gui.SameLine(0f, -1f);
                EditorUI.current.widgets.ColoredText(levelColor, prefix);
                _ = Gui.TableSetColumnIndex(1);
                if (isOpen)
                {
                    EditorUI.current.widgets.WrappedText(content);
                }
                else
                {
                    Gui.Text(content);
                }

                _ = Gui.TableSetColumnIndex(2);
                if (!string.IsNullOrEmpty(repeatText))
                {
                    float remainW = Gui.GetContentRegionAvailable().X;
                    float suffixOffsetX = MathF.Max(0f, remainW - suffixW);
                    Gui.SetCursorPosX(Gui.GetCursorPosX() + suffixOffsetX);
                    Gui.Text(repeatText);
                }
            }
            finally
            {
                Gui.EndTable();
            }
        }
    }

    private static void DrawDetail(EditorConsoleOccurrence entry)
    {
        string timeText = entry.time.ToString("HH:mm:ss");
        string sourceText = entry.source;
        if (string.IsNullOrWhiteSpace(sourceText))
        {
            sourceText = "-";
        }

        string fileText = string.IsNullOrWhiteSpace(entry.file) ? "-" : entry.file;
        string fileWithLineText = entry.line > 0 ? entry.column > 0 ? $"{fileText}:{entry.line}:{entry.column}" : $"{fileText}:{entry.line}" : fileText;
        // These cards may be culled between frames; do not rely on a previous frame's auto-fit width.
        float labelWidth = Gui.MeasureText("Session:").X;
        float availableWidth = Gui.GetContentRegionAvailable().X;
        if (availableWidth < labelWidth + Gui.MeasureText("Diagnostic").X + Gui.GetLayout().cellPadding.X * 4)
        {
            DrawStackedField("Kind:", entry.kind.ToString());
            DrawStackedField("File:", fileWithLineText);
            DrawStackedField("Source:", sourceText);
            if (entry.sessionID.isAssigned)
            {
                DrawStackedField("Session:", entry.sessionID.ToString());
            }
            DrawStackedField("Time:", timeText);
        }
        else if (Gui.BeginTable(
            "##ConsoleEntryDetails",
            2,
            (ImGuiTableFlags.SizingStretchProp | ImGuiTableFlags.NoSavedSettings),
            new Vector2(availableWidth, 0)
        )) {
            try
            {
                Gui.TableSetupColumn("##label", (ImGuiTableColumnFlags.WidthFixed | ImGuiTableColumnFlags.NoResize), labelWidth);
                Gui.TableSetupColumn("##value", (ImGuiTableColumnFlags.WidthStretch | ImGuiTableColumnFlags.NoResize), 1f);
                DrawDetailFieldRow("Kind:", entry.kind.ToString());
                DrawDetailFieldRow("File:", fileWithLineText);
                DrawDetailFieldRow("Source:", sourceText);
                if (entry.sessionID.isAssigned)
                {
                    DrawDetailFieldRow("Session:", entry.sessionID.ToString());
                }
                DrawDetailFieldRow("Time:", timeText);
            }
            finally
            {
                Gui.EndTable();
            }
        }

        if (!string.IsNullOrWhiteSpace(entry.stackTrace))
        {
            Gui.Separator();
            EditorUI.current.widgets.WrappedText(entry.stackTrace);
        }
    }

    private void HandleKeyboardNavigation()
    {
        int current = m_view.FindIndex(m_focusedKey);
        int pageSize = Math.Max(1, (int)(Gui.GetContentRegionAvailable().Y / Math.Max(1f, Gui.GetFrameHeight())));
        ListNavigationResult navigation = EditorUI.current.widgets.NavigateList(m_interactions, m_view.rows.Count, current, pageSize);
        m_keyboardHandled = navigation.activate || navigation.selectionRequested || navigation.focusChanged;
        m_scrollFocusedRow = navigation.focusChanged || navigation.activate;
        if (navigation.focusedIndex >= 0 && (navigation.focusChanged || navigation.selectionRequested || navigation.activate))
        {
            ConsoleView.Row row = m_view.rows[navigation.focusedIndex];
            FocusRow(row, navigation.selectionRequested || navigation.activate);
            if (navigation.activate)
            {
                ToggleEntry(row.key);
            }
        }
        if (navigation.cancel)
        {
            ClearFocus();
        }
    }

    private void FocusRow(
        ConsoleView.Row row,
        bool select
    ) {
        m_focusedKey = row.key;
        if (select)
        {
            m_selectedKey = row.key;
        }
        // The view owns this neutral boxed payload while Core keeps only a weak action target.
        m_focusedCopyTarget = row.copyTarget;
        m_interactions.For(LoggingInteractionIDs.C_ENTRY_AREA, m_focusedCopyTarget).Focus();
    }

    private void DrawEntryContextMenu()
    {
        const string C_POPUP_ID = "##ConsoleEntryContextMenu";
        if (m_contextKey is null)
        {
            return;
        }
        int index = m_view.FindIndex(m_contextKey);
        if (index < 0)
        {
            if (Gui.BeginPopup(C_POPUP_ID))
            {
                try
                {
                    Gui.CloseCurrentPopup();
                }
                finally
                {
                    Gui.EndPopup();
                }
            }
            m_contextKey = null;
            m_requestContextMenu = false;
            return;
        }
        if (m_requestContextMenu)
        {
            Gui.OpenPopup(C_POPUP_ID);
            m_requestContextMenu = false;
        }
        if (!EditorUI.current.menus.ContextMenu(
            C_POPUP_ID,
            m_interactions.For(LoggingInteractionIDs.C_ENTRY_AREA, m_view.rows[index].copyTarget)
        )) {
            m_contextKey = null;
        }
    }

    private void RemoveStaleOpenEntries(EditorConsoleSnapshot snapshot)
    {
        if (m_openEntries.Count == 0)
        {
            return;
        }
        var activeIDs = new HashSet<string>(StringComparer.Ordinal);
        for (int i = 0; i < snapshot.groups.Count; i++)
        {
            activeIDs.Add($"group/{snapshot.groups[i].identity}");
        }
        for (int i = 0; i < snapshot.occurrences.Count; i++)
        {
            activeIDs.Add($"occurrence/{snapshot.occurrences[i].sequence}");
        }
        m_openEntries.RemoveWhere(id => !activeIDs.Contains(id));
    }

    private static void DrawDetailFieldRow(
        string label,
        string value
    ) {
        Gui.TableNextRow();
        _ = Gui.TableSetColumnIndex(0);
        EditorUI.current.widgets.ColoredText(EditorPalette.textDisabled, label);
        _ = Gui.TableSetColumnIndex(1);
        EditorUI.current.widgets.WrappedText(value);
    }

    private static void DrawStackedField(
        string label,
        string value
    ) {
        EditorUI.current.widgets.ColoredText(EditorPalette.textDisabled, label);
        EditorUI.current.widgets.WrappedText(value);
    }
#endregion
}
