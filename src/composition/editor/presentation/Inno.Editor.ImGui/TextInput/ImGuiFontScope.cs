using System;

using HostScope = Inno.Adapter.Presentation.ImGui.ImGuiFontScope;
using NativeImGui = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.ImGui;

/// <summary>
/// Restores a font push only in its original session, frame and native stack position.
/// </summary>
public unsafe ref struct ImGuiFontScope : IDisposable
{
    private EditorUISession? m_session;
    private HostScope m_scope;
    private readonly int m_frame;
    private readonly int m_depth;

    internal ImGuiFontScope(
        EditorUISession session,
        HostScope scope,
        int frame,
        int depth
    ) {
        m_session = session;
        m_scope = scope;
        m_frame = frame;
        m_depth = depth;
    }

    /// <summary>
    /// Restores the preceding font after validating scope order; default and retired scopes are inert.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// The scope is used in another context, frame or stack position.
    /// </exception>
    public void Dispose()
    {
        if (m_session is null)
        {
            return;
        }
        m_session.EnsureDrawing();
        if (NativeImGui.GetFrameCount() != m_frame || NativeImGui.Raw.GetCurrentContext()->FontStack.Size != m_depth)
        {
            throw new InvalidOperationException("Font scopes must retire in their original frame and reverse stack order.");
        }
        m_scope.Dispose();
        m_session = null;
    }
}
