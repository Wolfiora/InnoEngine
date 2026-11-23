using System;
using System.Collections.Generic;

namespace Inno.Editor.ImGui.ImGuiWidget;

/// <summary>
/// Owns and retires transient controls by session and stable presentation owner.
/// </summary>
public sealed unsafe partial class ImGuiWidget
{
    private readonly EditorUISession m_session;

    internal ImGuiWidget(EditorUISession session)
    {
        ArgumentNullException.ThrowIfNull(session);
        m_session = session;
        this.style = new EditorStyleMetrics(session);
    }

    internal void RetireOwner(string owner)
    {
        if (m_propertyGesture is not null && m_propertyEditKey.owner == owner)
        {
            CancelEdits();
        }
        List<(string owner, uint window)> retired = [];
        foreach ((string owner, uint window) key in m_treeStatesByWindow.Keys)
        {
            if (string.Equals(key.owner, owner, StringComparison.Ordinal))
            {
                retired.Add(key);
            }
        }

        foreach ((string owner, uint window) key in retired)
        {
            m_treeStatesByWindow.Remove(key);
        }
    }

    internal void Retire()
    {
        CancelEdits();
        m_treeStatesByWindow.Clear();
        ClearDialogMeasurements();
    }
}
