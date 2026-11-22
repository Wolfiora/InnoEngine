using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Inno.Core.Logging;
using Inno.Editor.Core;
using Inno.Editor.ImGui;
using Inno.Editor.ImGui.ImGuiWidget;
using Inno.Editor.Settings;

using Gui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Panel.Settings;

[EditorModal("editor.settings", "Settings", order: 100)]
internal sealed class SettingsModal(
    EditorSettings editorSettings,
    ProjectSettingsEditor projectSettings,
    SettingsWindowModule window,
    LogRouter logs
) : EditorModal
{
    private readonly SettingsNavigation m_navigation = new();
    private readonly SettingsTree m_tree = new();
    private readonly Logger m_log = logs.CreateLogger<SettingsModal>();
    private long m_navigationRevision = -1;
    private string m_query = string.Empty;
    private float m_treePaneRatio = 0.25f;
    private string m_applyError = string.Empty;

    /// <summary>
    /// Gets whether this implementation is visible.
    /// </summary>
    public override bool isVisible => window.isVisible;

    /// <summary>
    /// Gets whether this implementation can move.
    /// </summary>
    public override bool canMove => true;

    /// <summary>
    /// Gets whether this implementation can resize.
    /// </summary>
    public override bool canResize => true;

    /// <summary>
    /// Lets the navigation tree and current settings page own their scrolling.
    /// </summary>
    public override bool allowScrolling => false;

    /// <summary>
    /// Gets the preferred initial window size in logical editor units.
    /// </summary>
    public override Vector2 initialSize => new(1050f, 700f);

    /// <summary>
    /// Gets the smallest editor window size that keeps every setting control usable.
    /// </summary>
    public override Vector2 minimumSize => new(760f, 520f);

    /// <summary>
    /// Draws this feature using the current editor presentation context.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    protected override void OnDraw(EditorContext context)
    {
        try
        {
            DrawWindow();
        }
        catch (Exception failure)
        {
            try
            {
                window.FailPresentation();
            }
            catch (Exception retirementFailure)
            {
                throw new AggregateException("Settings presentation and its owned retirement failed.", failure, retirementFailure);
            }
            throw;
        }
    }

    private void DrawWindow()
    {
        SettingsEditSession? session = window.session;
        if (session is null)
        {
            return;
        }
        if (session.editorCatalogRevision != editorSettings.catalogRevision
            || session.projectCatalogRevision != projectSettings.catalogRevision
        ) {
            window.Refresh();
            session = window.session;
            if (session is null)
            {
                return;
            }
        }

        IReadOnlyList<SettingsPage> pages = session.pages;
        EnsureNavigationSession(pages);
        SettingsPage? selected = SettingsTree.FindPage(pages, m_navigation.currentPath);
        selected ??= SettingsTree.FindFirstMatch(pages, m_query);
        if (selected is not null)
        {
            m_navigation.Replace(selected.path);
        }
        float footerHeight = Gui.GetFrameHeightWithSpacing() + Gui.GetLayout().itemSpacing.Y;
        if (m_applyError.Length != 0)
        {
            footerHeight += Gui.GetFrameHeightWithSpacing();
        }
        Vector2 available = Gui.GetContentRegionAvailable();
        DrawBody(session, pages, new Vector2(available.X, MathF.Max(1f, available.Y - footerHeight)));
        Gui.Separator();
        if (m_applyError.Length != 0)
        {
            EditorUISession.current.widgets.ColoredText(
                EditorPalette.error,
                session.isHistoryFaulted
                ? "History integrity was lost. Restart the Editor."
                : session.canApply
                  ? "Apply failed. Edits remain staged; retry or Cancel."
                  : "Settings recovery is required. See Console for details."
            );
            EditorUISession.current.widgets.DrawItemTooltip(m_applyError);
        }
        DrawButtons(session);
    }

    private void DrawBody(
        SettingsEditSession session,
        IReadOnlyList<SettingsPage> pages,
        Vector2 size
    ) {
        ImGuiTableFlags flags = ImGuiTableFlags.NoPadOuterX
                                | ImGuiTableFlags.NoKeepColumnsVisible
                                | ImGuiTableFlags.SizingFixedFit
                                | ImGuiTableFlags.NoSavedSettings;
        float splitterWidth = MathF.Max(5f * EditorUISession.current.widgets.style.zoom, Gui.GetLayout().dockingSeparatorSize);
        float treeWidth = ResolveTreeWidth(size.X, splitterWidth);
        Gui.PushStyleVar(ImGuiStyleVar.CellPadding, Vector2.Zero);
        bool tableStarted = false;
        try
        {
            tableStarted = Gui.BeginTable("##settings_split", 3, flags, size);
            if (tableStarted)
            {
                Gui.TableSetupColumn("##settings_tree", (ImGuiTableColumnFlags.WidthFixed | ImGuiTableColumnFlags.NoResize), treeWidth);
                Gui.TableSetupColumn(
                    "##settings_splitter",
                    (ImGuiTableColumnFlags.WidthFixed | ImGuiTableColumnFlags.NoResize),
                    splitterWidth
                );
                Gui.TableSetupColumn("##settings_page", ImGuiTableColumnFlags.WidthStretch);
                Gui.TableNextRow();
                _ = Gui.TableSetColumnIndex(0);
                DrawTreePane(pages, size.Y);
                _ = Gui.TableSetColumnIndex(1);
                DrawSplitter(splitterWidth, size.X, treeWidth, size.Y);
                _ = Gui.TableSetColumnIndex(2);
                DrawPagePane(session, pages, size.Y);
            }
        }
        finally
        {
            if (tableStarted)
            {
                Gui.EndTable();
            }
            Gui.PopStyleVar();
        }
    }

    private void DrawTreePane(
        IReadOnlyList<SettingsPage> pages,
        float height
    ) {
        bool visible = Gui.BeginChild(
            "##settings_tree_pane",
            new Vector2(0f, height),
            (ImGuiChildFlags.Borders | ImGuiChildFlags.AlwaysUseWindowPadding),
            (ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
        );
        try
        {
            if (!visible)
            {
                return;
            }
            DrawNavigationButtons();
            Gui.SameLine(0f, EditorUISession.current.widgets.style.toolbarSectionSpacing);
            bool wasSearching = !string.IsNullOrWhiteSpace(m_query);
            Gui.SetNextItemWidth(-1f);
            bool queryChanged = EditorUISession.current.widgets.SearchInput("settings", "Search settings", ref m_query, width: -1f);
            if (queryChanged)
            {
                SettingsPage? first = SettingsTree.FindFirstMatch(pages, m_query);
                if (first is not null)
                {
                    if (wasSearching)
                    {
                        m_navigation.Replace(first.path);
                    }
                    else
                    {
                        m_navigation.NavigateTo(first.path);
                    }
                }
                else
                {
                    m_navigation.Replace(string.Empty);
                }
            }

            Gui.Separator();
            bool scrollVisible = Gui.BeginChild("##settings_tree_scroll", Vector2.Zero, 0, 0);
            try
            {
                if (scrollVisible)
                {
                    m_tree.Draw(pages, m_query, m_navigation.currentPath, page => m_navigation.NavigateTo(page.path));
                }
            }
            finally
            {
                Gui.EndChild();
            }
        }
        finally
        {
            Gui.EndChild();
        }
    }

    private void DrawPagePane(
        SettingsEditSession session,
        IReadOnlyList<SettingsPage> pages,
        float height
    ) {
        bool visible = Gui.BeginChild(
            "##settings_page_pane",
            new Vector2(0f, height),
            (ImGuiChildFlags.Borders | ImGuiChildFlags.AlwaysUseWindowPadding)
        );
        try
        {
            if (!visible)
            {
                return;
            }
            SettingsPage? page = SettingsTree.FindPage(pages, m_navigation.currentPath);
            if (page is not null)
            {
                var view = new SettingsPageView(session);
                view.Draw(page, child =>
                {
                    m_navigation.NavigateTo(child.path);
                    m_tree.Reveal(child.path);
                });
            }
            else
            {
                Gui.Text("No matching settings");
            }
        }
        finally
        {
            Gui.EndChild();
        }
    }

    private void DrawSplitter(
        float width,
        float availableWidth,
        float treeWidth,
        float height
    ) {
        _ = Gui.InvisibleButton("##settings_tree_splitter_grip", new Vector2(width, MathF.Max(1f, height)));
        bool hovered = Gui.IsItemHovered();
        bool active = Gui.IsItemActive();
        if (hovered || active)
        {
            Gui.SetMouseCursor(ImGuiMouseCursor.ResizeEw);
        }
        if (active)
        {
            Vector2 delta = Gui.GetMouseDragDelta(ImGuiMouseButton.Left);
            if (MathF.Abs(delta.X) > 0f)
            {
                float usable = MathF.Max(0f, availableWidth - width);
                float requested = ClampTreeWidth(treeWidth + delta.X, usable);
                m_treePaneRatio = usable > float.Epsilon ? Math.Clamp(requested / usable, 0f, 1f) : 0.25f;
                Gui.ResetMouseDragDelta(ImGuiMouseButton.Left);
            }
        }

        Vector2 minimum = Gui.GetItemMinimum();
        Vector2 maximum = Gui.GetItemMaximum();
        Vector4 color = active
                        ? EditorPalette.contentAccent
                        : hovered ? EditorPalette.contentBorder : EditorPalette.contentBorderSoft;
        float centerX = (minimum.X + maximum.X) * 0.5f;
        Gui.DrawLine(
            new Vector2(centerX, minimum.Y),
            new Vector2(centerX, maximum.Y),
            Gui.ToPackedColor(color),
            EditorUISession.current.widgets.style.borderSize
        );
    }

    private float ResolveTreeWidth(
        float availableWidth,
        float splitterWidth
    ) {
        float usable = MathF.Max(1f, availableWidth - splitterWidth);
        return ClampTreeWidth(usable * Math.Clamp(m_treePaneRatio, 0f, 1f), usable);
    }

    private void DrawButtons(SettingsEditSession session)
    {
        EditorUILayout style = Gui.GetLayout();
        if (!session.isDirty)
        {
            float okWidth = Gui.MeasureText("OK").X + style.framePadding.X * 2f;
            Gui.SetCursorPosX(Gui.GetCursorPosX() + MathF.Max(0f, Gui.GetContentRegionAvailable().X - okWidth));
            if (Gui.Button("OK"))
            {
                TryClose();
            }
            return;
        }

        float cancelWidth = Gui.MeasureText("Cancel").X + style.framePadding.X * 2f;
        float applyWidth = Gui.MeasureText("Apply").X + style.framePadding.X * 2f;
        float totalWidth = cancelWidth + style.itemSpacing.X + applyWidth;
        Gui.SetCursorPosX(Gui.GetCursorPosX() + MathF.Max(0f, Gui.GetContentRegionAvailable().X - totalWidth));
        if (Gui.Button("Cancel"))
        {
            TryClose();
        }
        Gui.SameLine(0f, -1f);
        Gui.BeginDisabled(!session.canApply);
        try
        {
            if (Gui.Button("Apply"))
            {
                try
                {
                    _ = session.Apply();
                    m_applyError = string.Empty;
                }
                catch (Exception failure) when (IsOperationalFailure(failure))
                {
                    ReportFailure(failure);
                }
            }
        }
        finally
        {
            Gui.EndDisabled();
        }
    }

    private void TryClose()
    {
        try
        {
            window.Close();
            m_applyError = string.Empty;
        }
        catch (Exception failure) when (IsOperationalFailure(failure))
        {
            ReportFailure(failure);
        }
    }

    private void ReportFailure(Exception failure)
    {
        m_applyError = failure.Message;
        m_log.Write(LogLevel.Error, "Settings operation failed; edits and History ownership were retained: {0}", [failure]);
    }

    private void DrawNavigationButtons()
    {
        float frameHeight = Gui.GetFrameHeight();
        var buttonSize = new Vector2(frameHeight, frameHeight);
        Gui.BeginDisabled(!m_navigation.canGoBack);
        if (Gui.Button($"{ImGuiIcon.C_ANGLE_LEFT}##SettingsBack", buttonSize))
        {
            m_navigation.GoBack();
        }

        Gui.EndDisabled();
        Gui.SameLine(0f, EditorUISession.current.widgets.style.toolbarTightSpacing);
        Gui.BeginDisabled(!m_navigation.canGoForward);
        if (Gui.Button($"{ImGuiIcon.C_ANGLE_RIGHT}##SettingsForward", buttonSize))
        {
            m_navigation.GoForward();
        }

        Gui.EndDisabled();
    }

    private void EnsureNavigationSession(IReadOnlyList<SettingsPage> pages)
    {
        if (m_navigationRevision == window.sessionRevision)
        {
            return;
        }
        m_navigationRevision = window.sessionRevision;
        m_applyError = string.Empty;
        SettingsPage? first = SettingsTree.FindFirstMatch(pages, m_query);
        m_navigation.Reset(first?.path ?? string.Empty);
    }

    private static float ClampTreeWidth(
        float requested,
        float usable
    ) {
        float minimum = MathF.Min(EditorUISession.current.widgets.style.paneMinimumVisibleWidth, usable * 0.5f);
        float maximum = MathF.Max(minimum, usable - minimum);
        return Math.Clamp(requested, minimum, maximum);
    }

    private static bool IsOperationalFailure(Exception failure)
        => failure is IOException or UnauthorizedAccessException or ArgumentException or InvalidOperationException or AggregateException;
}
