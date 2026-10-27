using System;
using System.IO;
using System.Linq;
using InnoEditor.Core;
using InnoEditor.Interactions;

namespace EditorUiAcceptance;

/// <summary>
/// Exercises the normal product panel protocol from an ordinary source Editor extension.
/// </summary>
[EditorModule("tests.editor-ui.panel-lifecycle", order: 10_000)]
public sealed class PanelLifecycle(EditorInteractions interactions) : EditorModule
{
    private string[] m_panels = [];
    private bool[] m_originalOpenStates = [];
    private int m_frame;
    private int m_cycles;
    private bool m_completed;

    /// <inheritdoc />
    protected override void OnUpdate(EditorContext context)
    {
        if (m_completed)
            return;
        if (m_panels.Length == 0)
        {
            m_panels = interactions.GetPanelDeclarations().Select(static panel => panel.id).Order(StringComparer.Ordinal).ToArray();
            if (m_panels.Length < 8)
                throw new InvalidOperationException("The complete Editor product must expose its eight standard panels.");
            m_originalOpenStates = new bool[m_panels.Length];
            for (int index = 0; index < m_panels.Length; index++)
            {
                string panel = m_panels[index];
                if (!interactions.TryGetPanelState(panel, out m_originalOpenStates[index]) || !interactions.ClosePanel(panel))
                    throw new InvalidOperationException("The product could not close panel '" + panel + "'.");
            }
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
}
