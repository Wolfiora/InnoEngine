using System;
using System.IO;
using System.Linq;
using InnoEditor.Core;
using InnoEditor.Interactions;
using InnoEditor.Scene;
using InnoEngine.Scene;

namespace EditorUIAcceptance;

/// <summary>
/// Exercises the normal product panel protocol from an ordinary source Editor extension.
/// </summary>
/// <param name="interactions">
/// The current Host's public commands, selection, panel and History entry points.
/// </param>
/// <param name="workspace">
/// The current Edit workspace whose loaded scenes must survive window-only operations.
/// </param>
/// <param name="edits">
/// The public scene edits that create the scene the panels draw.
/// </param>
[EditorModule("tests.editor-ui.panel-lifecycle", order: 10_000)]
public sealed class PanelLifecycle(
    EditorInteractions interactions,
    IEditorSceneWorkspace workspace,
    SceneEdits edits
) : EditorModule
{
    private string[] m_panels = [];
    private bool[] m_originalOpenStates = [];
    private string? m_undoName;
    private string? m_redoName;
    private long m_historyResidentBytes;
    private long m_historyDiskBytes;
    private int m_selectionCount;
    private int m_sceneCount;
    private bool m_panelsOpened;
    private int m_frame;
    private bool m_reopened;
    private bool m_baselineCaptured;
    private bool m_completed;

    /// <inheritdoc/>
    protected override void OnUpdate(EditorContext context)
    {
        if (m_completed)
        {
            return;
        }
        if (m_panels.Length == 0)
        {
            if (workspace.scenes.Count == 0)
            {
                // The panels draw a saved, clean scene with real objects, as they would for an opened scene asset.
                GameScene scene = edits.CreateScene();
                edits.RenameScene(scene, "Panel Lifecycle Acceptance");
                edits.RenameGameObject(edits.CreateGameObject(scene), "Lifecycle First");
                edits.RenameGameObject(edits.CreateGameObject(scene), "Lifecycle Second");
                _ = workspace.Save(scene, string.Empty);
            }
            m_panels = interactions.GetPanelDeclarations().Select(static panel => panel.id).Order(StringComparer.Ordinal).ToArray();
            if (m_panels.Length < 8)
            {
                throw new InvalidOperationException("The complete Editor product must expose its eight standard panels.");
            }
            m_originalOpenStates = new bool[m_panels.Length];
            for (int index = 0; index < m_panels.Length; index++)
            {
                string panel = m_panels[index];
                if (!interactions.TryGetPanelState(panel, out m_originalOpenStates[index]) || !interactions.OpenPanel(panel))
                {
                    throw new InvalidOperationException("The product could not open panel '" + panel + "' for preparation.");
                }
            }
            m_panelsOpened = true;
            return;
        }
        if (m_panelsOpened)
        {
            m_panelsOpened = false;
            foreach (string panel in m_panels)
            {
                if (!interactions.ClosePanel(panel))
                {
                    throw new InvalidOperationException("The product could not close panel '" + panel + "' after preparation.");
                }
            }
            return;
        }
        bool expectedOpen = (m_frame++ & 1) != 0;
        foreach (string panel in m_panels)
        {
            if (!interactions.TryGetPanelState(panel, out bool isOpen) || isOpen != expectedOpen)
            {
                throw new InvalidOperationException("Panel state or native drawing failed for '" + panel + "'.");
            }
        }
        if (expectedOpen)
        {
            m_reopened = true;
        }
        else
        {
            VerifyDocumentState();
        }
        if (m_reopened && !expectedOpen)
        {
            for (int index = 0; index < m_panels.Length; index++)
            {
                bool restored = m_originalOpenStates[index]
                                ? interactions.OpenPanel(m_panels[index])
                                : interactions.ClosePanel(m_panels[index]);
                if (!restored)
                {
                    throw new InvalidOperationException("The product could not restore panel '" + m_panels[index] + "'.");
                }
            }
            string directory = Path.Combine(context.projectDirectory, "Library", "Acceptance");
            Directory.CreateDirectory(directory);
            File.WriteAllLines(
                Path.Combine(directory, "panel-lifecycle.tsv"),
                new[] { "panel\tcompleted-open-close-cycles", "protocol\tpublic EditorInteractions / actual product drawing" }
                    .Concat(m_panels.Select(static panel => panel + "\t1"))
            );
            string completion = Path.Combine(directory, "journey-completed.txt");
            File.WriteAllText(completion + ".pending", "Lifecycle");
            File.Move(completion + ".pending", completion);
            m_completed = true;
            return;
        }
        foreach (string panel in m_panels)
        {
            bool changed = expectedOpen ? interactions.ClosePanel(panel) : interactions.OpenPanel(panel);
            if (!changed)
            {
                throw new InvalidOperationException("The panel protocol rejected '" + panel + "'.");
            }
        }
    }

    private void VerifyDocumentState()
    {
        if (interactions.events.pendingCount != 0)
        {
            throw new InvalidOperationException("The closed panels retained pending events.");
        }
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
        if (history.isFaulted || history.undoName != m_undoName || history.redoName != m_redoName
            || history.residentBytes != m_historyResidentBytes || history.diskBytes != m_historyDiskBytes
            || interactions.selection.count != m_selectionCount || workspace.scenes.Count != m_sceneCount
        ) {
            throw new InvalidOperationException(
                "Window-only lifecycle operations changed Edit selection, scenes or committed History state."
            );
        }
    }
}
