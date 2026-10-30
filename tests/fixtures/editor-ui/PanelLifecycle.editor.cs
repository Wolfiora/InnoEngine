using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using InnoEditor.Core;
using InnoEditor.Interactions;
using InnoEditor.Scene;

namespace EditorUiAcceptance;

/// <summary>
/// Exercises the normal product panel protocol from an ordinary source Editor extension.
/// </summary>
/// <param name="interactions">
/// The current Host's public commands, selection, panel and History entry points.
/// </param>
/// <param name="workspace">
/// The current Edit workspace whose loaded scenes must survive window-only operations.
/// </param>
[EditorModule("tests.editor-ui.panel-lifecycle", order: 10_000)]
public sealed class PanelLifecycle(
    EditorInteractions interactions,
    IEditorSceneWorkspace workspace
) : EditorModule
{
    private readonly List<string> m_resources = ["completed-cycles\tmanaged-bytes-after-full-gc\tprivate-bytes\tselection-count\tscene-count\thistory-resident-bytes\thistory-disk-bytes\tpending-events"];
    private readonly List<string> m_preparationResources = ["phase\tmanaged-bytes-after-full-gc\tprivate-bytes\tselection-count\tscene-count\thistory-resident-bytes\thistory-disk-bytes\tpending-events"];
    private string[] m_panels = [];
    private bool[] m_originalOpenStates = [];
    private string? m_undoName;
    private string? m_redoName;
    private long m_historyResidentBytes;
    private long m_historyDiskBytes;
    private int m_selectionCount;
    private int m_sceneCount;
    private int m_warmup;
    private int m_preparationFrames;
    private int m_frame;
    private int m_cycles;
    private bool m_baselineCaptured;
    private bool m_completed;

    /// <inheritdoc />
    protected override void OnUpdate(EditorContext context)
    {
        if (m_completed)
            return;
        if (m_panels.Length == 0 && m_warmup++ < 20)
        {
            return;
        }
        if (m_panels.Length == 0)
        {
            if (workspace.scenes.Count == 0)
                workspace.Open("FlappyBird/FlappyBird.iscene");
            m_panels = interactions.GetPanelDeclarations().Select(static panel => panel.id).Order(StringComparer.Ordinal).ToArray();
            if (m_panels.Length < 8)
                throw new InvalidOperationException("The complete Editor product must expose its eight standard panels.");
            m_originalOpenStates = new bool[m_panels.Length];
            m_preparationResources.Add(CaptureResourceRow("before-first-panel-draw"));
            for (int index = 0; index < m_panels.Length; index++)
            {
                string panel = m_panels[index];
                if (!interactions.TryGetPanelState(panel, out m_originalOpenStates[index]) || !interactions.OpenPanel(panel))
                    throw new InvalidOperationException("The product could not open panel '" + panel + "' for preparation.");
            }
            m_preparationFrames = 120;
            return;
        }
        if (m_preparationFrames > 0)
        {
            if (--m_preparationFrames > 0)
                return;
            foreach (string panel in m_panels)
                if (!interactions.ClosePanel(panel))
                    throw new InvalidOperationException("The product could not close panel '" + panel + "' after preparation.");
            m_preparationResources.Add(CaptureResourceRow("after-120-drawn-frames"));
            return;
        }
        bool expectedOpen = (m_frame++ & 1) != 0;
        foreach (string panel in m_panels)
        {
            if (!interactions.TryGetPanelState(panel, out bool isOpen) || isOpen != expectedOpen)
                throw new InvalidOperationException("Panel state or native drawing failed for '" + panel + "' at cycle " + m_cycles + ".");
        }
        if (expectedOpen)
            m_cycles++;
        else
        {
            VerifyDocumentState();
            if (m_cycles % 10 == 0)
                CaptureResources();
        }
        if (m_cycles == 100 && !expectedOpen)
        {
            for (int index = 0; index < m_panels.Length; index++)
            {
                bool restored = m_originalOpenStates[index] ? interactions.OpenPanel(m_panels[index]) : interactions.ClosePanel(m_panels[index]);
                if (!restored)
                    throw new InvalidOperationException("The product could not restore panel '" + m_panels[index] + "'.");
            }
            string directory = Path.Combine(context.projectDirectory, "Library", "Acceptance");
            Directory.CreateDirectory(directory);
            File.WriteAllLines(Path.Combine(directory, "panel-lifecycle.tsv"),
                new[] { "panel\tcompleted-open-close-cycles", "protocol\tpublic EditorInteractions / actual product drawing" }
                    .Concat(m_panels.Select(static panel => panel + "\t100")));
            File.WriteAllLines(Path.Combine(directory, "panel-lifecycle-resources.tsv"), m_resources);
            File.WriteAllLines(Path.Combine(directory, "panel-lifecycle-preparation.tsv"), m_preparationResources);
            m_completed = true;
            return;
        }
        foreach (string panel in m_panels)
        {
            bool changed = expectedOpen ? interactions.ClosePanel(panel) : interactions.OpenPanel(panel);
            if (!changed)
                throw new InvalidOperationException("The panel protocol rejected '" + panel + "'.");
        }
    }

    private void VerifyDocumentState()
    {
        IEditorHistory history = interactions.history;
        if (!m_baselineCaptured)
        {
            m_undoName = history.undoName;
            m_redoName = history.redoName;
            m_historyResidentBytes = history.residentBytes;
            m_historyDiskBytes = history.diskBytes;
            m_selectionCount = interactions.selection.count;
            m_sceneCount = workspace.scenes.Count;
            m_baselineCaptured = true;
        }
        if (history.isFaulted || history.undoName != m_undoName || history.redoName != m_redoName ||
            history.residentBytes != m_historyResidentBytes || history.diskBytes != m_historyDiskBytes ||
            interactions.selection.count != m_selectionCount || workspace.scenes.Count != m_sceneCount)
            throw new InvalidOperationException("Window-only lifecycle operations changed Edit selection, scenes or committed History state.");
    }

    private void CaptureResources()
        => m_resources.Add(CaptureResourceRow(m_cycles.ToString(System.Globalization.CultureInfo.InvariantCulture)));

    private string CaptureResourceRow(string observation)
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        long managed = GC.GetTotalMemory(forceFullCollection: false);
        using Process process = Process.GetCurrentProcess();
        return FormattableString.Invariant($"{observation}\t{managed}\t{process.PrivateMemorySize64}\t{interactions.selection.count}\t{workspace.scenes.Count}\t{interactions.history.residentBytes}\t{interactions.history.diskBytes}\t{interactions.events.pendingCount}");
    }
}
