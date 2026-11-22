namespace Inno.Adapter.Presentation.ImGui;

/// <summary>
/// Restores the previous ImGui font when a scoped style ends.
/// </summary>
public ref struct ImGuiFontScope
{
    private readonly nuint m_contextKey;
    private bool m_isPushed;

    internal ImGuiFontScope(
        nuint contextKey,
        bool isPushed
    ) {
        m_contextKey = contextKey;
        m_isPushed = isPushed;
    }

    /// <summary>
    /// Restores the font that was active before this scope.
    /// </summary>
    public void Dispose()
    {
        if (!m_isPushed)
        {
            return;
        }
        ImGuiFont.PopStyle(m_contextKey);
        m_isPushed = false;
    }
}
