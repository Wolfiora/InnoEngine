using EditorUI = Inno.Editor.ImGui.EditorUiSession;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Inno.Core.Logging;
using Inno.Editor.Core;
using Inno.Editor.Diagnostics;
using Inno.Editor.Interactions;
using Inno.Editor.ImGui;
using Inno.Editor.ImGui.ImGuiWidget;
using Ui = Inno.Editor.ImGui.ImGui;

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
    protected override void Capture(EditorState state)
    {
        state.Set("collapse", m_collapse);
    }

    /// <summary>
    /// Restores persistent Console layout preferences without restoring historical log content.
    /// </summary>
    /// <param name="state">
    /// The project-level panel state containing stable presentation values.
    /// </param>
    protected override void Restore(EditorState state)
    {
        m_collapse = state.Get("collapse", true);
    }

    /// <inheritdoc />
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
        Ui.BeginChild("ConsoleChild", Vector2.Zero, (ImGuiChildFlags.None), (ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse));
        try
        {
            DrawToolbar();
            Ui.Separator();
            DrawConsoleRegion();
        }
        finally
        {
            Ui.EndChild();
        }
    }

#endregion
#region Toolbar
    private static ConsoleStyle consoleStyle => new(EditorUI.current.widgets.style);

    private void DrawToolbar()
    {
        if (EditorUI.current.widgets.SearchInput("console", "Search messages, sources and files", ref m_query, 1024))
            m_requestScrollToBottom = false;
        bool collapseChanged = Ui.Checkbox("Collapse", ref m_collapse);
        if (collapseChanged)
        {
            m_requestScrollToBottom = true;
        }

        Ui.SameLine(0f, -1f);
        bool filterChanged = DrawFilterCombo();
        if (filterChanged)
        {
            m_requestScrollToBottom = true;
        }

        Ui.SameLine(0f, -1f);
        if (Ui.Button("Clear"))
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
            if (!Ui.Checkbox(level.ToString(), ref selected))
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
        Ui.BeginChild("ConsoleRegion", Vector2.Zero, 0, 0);
        try
        {
            EditorConsoleSnapshot snapshot = m_console.Capture();
            if (m_lastRevision != snapshot.revision)
                RemoveStaleOpenEntries(snapshot);
            bool viewChanged = m_view.Update(snapshot, m_query, m_filterLevels, m_collapse);
            if (viewChanged)
            {
                int focused = m_view.FindIndex(m_focusedKey);
                if (focused < 0)
                {
                    ClearFocus();
                }
                else if (m_interactions.focusedArea == LoggingInteractionIds.C_ENTRY_AREA)
                    FocusRow(m_view.rows[focused], select: false);
            }
            bool hasNewEntries = m_lastRevision >= 0 && snapshot.revision != m_lastRevision;
            m_lastRevision = snapshot.revision;
            bool scrollAtBottom = Ui.GetScrollY() >= Ui.GetScrollMaxY() - consoleStyle.autoScrollTolerance;
            bool shouldScrollToBottom = m_requestScrollToBottom || (hasNewEntries && scrollAtBottom);
            HandleKeyboardNavigation();
            if (m_keyboardHandled)
                m_requestScrollToBottom = false;
            DrawEntries(snapshot);
            DrawEntryContextMenu();
            if (shouldScrollToBottom && !m_keyboardHandled)
            {
                Ui.SetScrollHereY(1f);
                m_requestScrollToBottom = false;
            }
        }
        finally
        {
            Ui.EndChild();
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
        if (m_interactions.focusedArea == LoggingInteractionIds.C_ENTRY_AREA)
            m_interactions.For(LoggingInteractionIds.C_ENTRY_AREA).Focus();
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
        Vector4 cardBorder = row.key == m_selectedKey ? EditorPalette.contentAccent
            : open ? ConsoleStyle.GetExpandedBorder(levelColor) : m_content.GetCollapsedBorderColor();
        Ui.PushStyleColor((ImGuiCol.FrameBg), cardBg);
        Ui.PushStyleColor((ImGuiCol.Border), cardBorder);
        Ui.PushStyleVar((ImGuiStyleVar.FrameRounding), Ui.GetLayout().frameRounding);
        Ui.PushStyleVar((ImGuiStyleVar.FrameBorderSize), EditorUI.current.widgets.style.borderSize);
        Ui.PushStyleVar((ImGuiStyleVar.FramePadding), EditorUI.current.widgets.style.framePadding);
        try
        {
            ImGuiChildFlags childFlags = ImGuiChildFlags.FrameStyle | (open ? ImGuiChildFlags.AutoResizeY : ImGuiChildFlags.None);
            if (open && measure)
                childFlags |= ImGuiChildFlags.AlwaysAutoResize;
            ImGuiWindowFlags windowFlags = ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse | ImGuiWindowFlags.NoSavedSettings;
            float height = open ? 0f : Ui.GetFrameHeight() + Ui.GetLayout().framePadding.Y * 2f;
            bool visible = Ui.BeginChild(AcquireCardWindow(row.key), new Vector2(0f, height), childFlags, windowFlags);
            try
            {
                if (visible)
                {
                    ConsoleEntryImGuiIdentity.Push(row.key);
                    try
                    {
                        DrawHeader(row.latest, row.occurrences.Count, levelColor, levelIcon, open, cardBg, out bool toggled);
                        if (toggled)
                            ToggleEntry(row.key);
                        if (open)
                            DrawOccurrences(row.occurrences, cardBg);
                    }
                    finally
                    {
                        ConsoleEntryImGuiIdentity.Pop();
                    }
                }
            }
            finally
            {
                Ui.EndChild();
            }
            float measuredHeight = Ui.GetItemMaximum().Y - Ui.GetItemMinimum().Y;
            if (Ui.IsItemClicked(ImGuiMouseButton.Left) || Ui.IsItemClicked(ImGuiMouseButton.Right))
                FocusRow(row, select: true);
            if (Ui.IsItemHovered() && Ui.IsMouseReleased(ImGuiMouseButton.Right))
            {
                m_contextKey = row.key;
                m_requestContextMenu = true;
            }
            return measuredHeight;
        }
        finally
        {
            Ui.PopStyleVar(3);
            Ui.PopStyleColor(2);
        }
    }

    private static void DrawOccurrences(
        IReadOnlyList<EditorConsoleOccurrence> occurrences,
        Vector4 cardBackground
    ) {
        Ui.PushStyleColor(ImGuiCol.Separator, ConsoleStyle.GetSeparator(cardBackground));
        try
        {
            Ui.Separator();
            for (int i = 0; i < occurrences.Count; i++)
            {
                if (i > 0)
                    Ui.Separator();
                Ui.PushId(unchecked((int)occurrences[i].sequence));
                try
                {
                    DrawDetail(occurrences[i]);
                }
                finally
                {
                    Ui.PopId();
                }
            }
        }
        finally
        {
            Ui.PopStyleColor();
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
        EditorUiLayout style = Ui.GetLayout();
        string content = isOpen ? entry.displayMessage : m_content.GetFirstLine(entry.displayMessage);
        string repeatText = repeatCount > 1 ? $" (x{repeatCount})" : string.Empty;
        string prefix = $"{levelIcon} [{entry.level}] ";
        string toggleText = isOpen ? "▼" : "▶";
        float toggleW = Ui.MeasureText(toggleText).X + consoleStyle.disclosurePadding.X * 2f;
        float prefixW = Ui.MeasureText(prefix).X + toggleW + style.itemInnerSpacing.X;
        float suffixW = Ui.MeasureText(repeatText).X;
        float suffixColW = MathF.Max(1f, suffixW);
        float tableOuterW = MathF.Max(1f, Ui.GetContentRegionAvailable().X);
        float contentW = MathF.Max(1f, tableOuterW - prefixW - suffixColW);
        if (!isOpen)
        {
            content = m_content.FitTextWithEllipsis(m_content.GetFirstLine(entry.displayMessage), contentW);
        }

        Vector2 tableOuterSize = new(tableOuterW, 0f);
        ImGuiTableFlags flags = ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.NoHostExtendX | ImGuiTableFlags.NoPadOuterX | ImGuiTableFlags.NoPadInnerX | ImGuiTableFlags.NoSavedSettings;
        if (Ui.BeginTable("##HeaderTable", 3, flags, tableOuterSize))
        {
            try
            {
                Ui.TableSetupColumn("##prefix", (ImGuiTableColumnFlags.WidthFixed | ImGuiTableColumnFlags.NoResize), prefixW);
                Ui.TableSetupColumn("##content", (ImGuiTableColumnFlags.WidthFixed | ImGuiTableColumnFlags.NoResize), contentW);
                Ui.TableSetupColumn("##suffix", (ImGuiTableColumnFlags.WidthFixed | ImGuiTableColumnFlags.NoResize), suffixColW);
                Ui.TableNextRow();
                if (isOpen)
                {
                    uint headerBgU32 = Ui.ToPackedColor(headerBgColor);
                    Ui.TableSetBgColor((ImGuiTableBgTarget.RowBg0), headerBgU32);
                }

                _ = Ui.TableSetColumnIndex(0);
                Ui.PushStyleVar((ImGuiStyleVar.FramePadding), consoleStyle.disclosurePadding);
                Ui.PushStyleVar((ImGuiStyleVar.FrameBorderSize), 0f);
                Ui.PushStyleColor((ImGuiCol.Button), ConsoleStyle.toggle);
                Ui.PushStyleColor((ImGuiCol.ButtonHovered), ConsoleStyle.toggleHovered);
                Ui.PushStyleColor((ImGuiCol.ButtonActive), ConsoleStyle.toggleActive);
                try
                {
                    if (Ui.SmallButton($"{toggleText}##HeaderToggle") && !m_keyboardHandled)
                    {
                        toggled = true;
                    }

                }
                finally
                {
                    Ui.PopStyleColor(3);
                    Ui.PopStyleVar(2);
                }
                Ui.SameLine(0f, -1f);
                EditorUI.current.widgets.ColoredText(levelColor, prefix);
                _ = Ui.TableSetColumnIndex(1);
                if (isOpen)
                {
                    EditorUI.current.widgets.WrappedText(content);
                }
                else
                {
                    Ui.Text(content);
                }

                _ = Ui.TableSetColumnIndex(2);
                if (!string.IsNullOrEmpty(repeatText))
                {
                    float remainW = Ui.GetContentRegionAvailable().X;
                    float suffixOffsetX = MathF.Max(0f, remainW - suffixW);
                    Ui.SetCursorPosX(Ui.GetCursorPosX() + suffixOffsetX);
                    Ui.Text(repeatText);
                }

            }
            finally
            {
                Ui.EndTable();
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
        float labelWidth = Ui.MeasureText("Session:").X;
        float availableWidth = Ui.GetContentRegionAvailable().X;
        if (availableWidth < labelWidth + Ui.MeasureText("Diagnostic").X + Ui.GetLayout().cellPadding.X * 4)
        {
            DrawStackedField("Kind:", entry.kind.ToString());
            DrawStackedField("File:", fileWithLineText);
            DrawStackedField("Source:", sourceText);
            if (entry.sessionId.isAssigned)
                DrawStackedField("Session:", entry.sessionId.ToString());
            DrawStackedField("Time:", timeText);
        }
        else if (Ui.BeginTable("##ConsoleEntryDetails", 2, (ImGuiTableFlags.SizingStretchProp | ImGuiTableFlags.NoSavedSettings), new Vector2(availableWidth, 0)))
        {
            try
            {
                Ui.TableSetupColumn("##label", (ImGuiTableColumnFlags.WidthFixed | ImGuiTableColumnFlags.NoResize), labelWidth);
                Ui.TableSetupColumn("##value", (ImGuiTableColumnFlags.WidthStretch | ImGuiTableColumnFlags.NoResize), 1f);
                DrawDetailFieldRow("Kind:", entry.kind.ToString());
                DrawDetailFieldRow("File:", fileWithLineText);
                DrawDetailFieldRow("Source:", sourceText);
                if (entry.sessionId.isAssigned)
                    DrawDetailFieldRow("Session:", entry.sessionId.ToString());
                DrawDetailFieldRow("Time:", timeText);
            }
            finally
            {
                Ui.EndTable();
            }
        }

        if (!string.IsNullOrWhiteSpace(entry.stackTrace))
        {
            Ui.Separator();
            EditorUI.current.widgets.WrappedText(entry.stackTrace);
        }
    }

    private void HandleKeyboardNavigation()
    {
        int current = m_view.FindIndex(m_focusedKey);
        int pageSize = Math.Max(1, (int)(Ui.GetContentRegionAvailable().Y / Math.Max(1f, Ui.GetFrameHeight())));
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
            ClearFocus();
    }

    private void FocusRow(
        ConsoleView.Row row,
        bool select
    ) {
        m_focusedKey = row.key;
        if (select)
            m_selectedKey = row.key;
        // The view owns this neutral boxed payload while Core keeps only a weak action target.
        m_focusedCopyTarget = row.copyTarget;
        m_interactions.For(LoggingInteractionIds.C_ENTRY_AREA, m_focusedCopyTarget).Focus();
    }

    private void DrawEntryContextMenu()
    {
        const string popupId = "##ConsoleEntryContextMenu";
        if (m_contextKey is null)
            return;
        int index = m_view.FindIndex(m_contextKey);
        if (index < 0)
        {
            if (Ui.BeginPopup(popupId))
            {
                try
                {
                    Ui.CloseCurrentPopup();
                }
                finally
                {
                    Ui.EndPopup();
                }
            }
            m_contextKey = null;
            m_requestContextMenu = false;
            return;
        }
        if (m_requestContextMenu)
        {
            Ui.OpenPopup(popupId);
            m_requestContextMenu = false;
        }
        if (!EditorUI.current.menus.ContextMenu(popupId,
                m_interactions.For(LoggingInteractionIds.C_ENTRY_AREA, m_view.rows[index].copyTarget)))
            m_contextKey = null;
    }

    private void RemoveStaleOpenEntries(EditorConsoleSnapshot snapshot)
    {
        if (m_openEntries.Count == 0)
            return;
        var activeIds = new HashSet<string>(StringComparer.Ordinal);
        for (int i = 0; i < snapshot.groups.Count; i++)
            activeIds.Add($"group/{snapshot.groups[i].identity}");
        for (int i = 0; i < snapshot.occurrences.Count; i++)
            activeIds.Add($"occurrence/{snapshot.occurrences[i].sequence}");
        m_openEntries.RemoveWhere(id => !activeIds.Contains(id));
    }

    private static void DrawDetailFieldRow(
        string label,
        string value
    ) {
        Ui.TableNextRow();
        _ = Ui.TableSetColumnIndex(0);
        EditorUI.current.widgets.ColoredText(Inno.Editor.ImGui.EditorPalette.textDisabled, label);
        _ = Ui.TableSetColumnIndex(1);
        EditorUI.current.widgets.WrappedText(value);
    }

    private static void DrawStackedField(
        string label,
        string value
    ) {
        EditorUI.current.widgets.ColoredText(Inno.Editor.ImGui.EditorPalette.textDisabled, label);
        EditorUI.current.widgets.WrappedText(value);
    }
#endregion
}
