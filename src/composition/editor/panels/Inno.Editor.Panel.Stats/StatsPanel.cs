using EditorUI = Inno.Editor.ImGui.EditorUiSession;
using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Editor.Core;
using Inno.Editor.ImGui;
using Inno.Editor.ImGui.ImGuiWidget;
using Ui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Panel.Stats;
/// <summary>
/// Shows lightweight runtime metrics.
/// </summary>
[EditorPanel("diagnostics.stats", "Stats", order: 500, menuPath: "Diagnostics")]
internal sealed class StatsPanel : EditorPanel
{
    private const float C_ROW_PADDING = 3f;

    private readonly FrameStatisticsSampler m_statistics = new();
    private IReadOnlyList<EditorStatistic>? m_previousStatistics;
    private StatisticGroup[] m_groups = [];
    private (string label, string value)[] m_frameValues = [];
    /// <summary>
    /// Gets whether use window padding is active for the current instance.
    /// </summary>
    public override bool useWindowPadding => false;

    /// <summary>
    /// Creates the panel.
    /// </summary>
    internal StatsPanel()
    {
    }

    /// <summary>
    /// Draws this feature using the current editor presentation context.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    protected override void OnDraw(EditorContext context)
    {
        if (m_statistics.Update(context.frame.totalTime, context.frame.deltaTime))
        {
            m_frameValues =
            [
                ("Time", $"{context.frame.totalTime:F2} s"),
                ("Delta", $"{m_statistics.deltaTime * 1000f:F2} ms"),
                ("FPS", $"{m_statistics.framesPerSecond:F1}")
            ];
        }
        DrawGroup("Frame Timing", m_frameValues);
        EditorUI.current.widgets.Trend("Frame time (ms)", m_statistics.frameTimes);
        EditorUI.current.widgets.Trend("Frames per second", m_statistics.frameRates);
        IReadOnlyList<EditorStatistic> statistics = context.statistics.GetSnapshot();
        if (!ReferenceEquals(statistics, m_previousStatistics))
        {
            m_groups = statistics.GroupBy(static statistic => statistic.groupId)
                .Select(static group => new StatisticGroup(group.First().groupName,
                    group.Select(static value => (value.label, value.value)).ToArray())).ToArray();
            m_previousStatistics = statistics;
        }
        if (m_groups.Length == 0)
        {
            EditorUI.current.widgets.Hint("No feature statistics were published for the current frame.");
            return;
        }
        foreach (StatisticGroup group in m_groups)
            DrawGroup(group.title, group.values);
    }

    /// <inheritdoc />
    protected override void OnDetach(EditorContext context)
    {
        m_statistics.Reset();
        m_previousStatistics = null;
        m_groups = [];
        m_frameValues = [];
    }

    private static void DrawGroup(
        string title,
        ReadOnlySpan<(string label, string value)> values
    ) {
        EditorUI.current.widgets.CollectionSectionHeader(title);
        System.Numerics.Vector2 padding = EditorUI.current.widgets.style.sectionPadding;
        ImGuiTableFlags flags = ImGuiTableFlags.SizingStretchProp | ImGuiTableFlags.RowBg | ImGuiTableFlags.NoSavedSettings | ImGuiTableFlags.PadOuterX;
        Ui.PushStyleVar((ImGuiStyleVar.CellPadding), new System.Numerics.Vector2(padding.X, C_ROW_PADDING * EditorUI.current.widgets.style.zoom));
        Ui.PushStyleColor((ImGuiCol.TableRowBg), EditorPalette.collectionRow);
        Ui.PushStyleColor((ImGuiCol.TableRowBgAlt), EditorPalette.collectionRowAlternate);
        try
        {
            if (!Ui.BeginTable($"##stats_{title}", 2, flags, default(System.Numerics.Vector2), 0f))
                return;
            try
            {
                Ui.TableSetupColumn("##label", (ImGuiTableColumnFlags.WidthStretch), 0.42f);
                Ui.TableSetupColumn("##value", (ImGuiTableColumnFlags.WidthStretch), 0.58f);
                foreach ((string label, string value) in values)
                {
                    Ui.TableNextRow();
                    Ui.TableSetColumnIndex(0);
                    EditorUI.current.widgets.ColoredText(Inno.Editor.ImGui.EditorPalette.textDisabled, label);
                    Ui.TableSetColumnIndex(1);
                    EditorUI.current.widgets.WrappedText(value);
                }
            }
            finally
            {
                Ui.EndTable();
            }
        }
        finally
        {
            Ui.PopStyleColor(2);
            Ui.PopStyleVar();
        }
    }

    private sealed record StatisticGroup(
        string title,
        (string label, string value)[] values
    );
}
