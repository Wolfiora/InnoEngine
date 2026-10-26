using System;
using System.Collections.Generic;

namespace Inno.Editor.ImGui.ImGuiWidget;
/// <summary>
/// Owns and retires transient controls by session and stable presentation owner.
/// </summary>
public sealed unsafe partial class ImGuiWidget
{
    private readonly EditorUiSession m_session;
    internal ImGuiWidget(EditorUiSession session)
    {
        m_session = session ?? throw new ArgumentNullException(nameof(session));
    }

    internal void RetireOwner(string owner)
    {
        if (m_propertyGesture is not null && m_propertyEditKey.Owner == owner)
            CancelEdits();
        var retired = new List<(string Owner, uint Window)>();
        foreach (var key in m_treeStatesByWindow.Keys)
        {
            if (string.Equals(key.Owner, owner, StringComparison.Ordinal))
                retired.Add(key);
        }

        foreach (var key in retired)
            m_treeStatesByWindow.Remove(key);
    }

    internal void Retire()
    {
        CancelEdits();
        m_treeStatesByWindow.Clear();
    }
}
