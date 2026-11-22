using System;

using HostFont = Inno.Adapter.Presentation.ImGui.ImGuiFont;
using HostScope = Inno.Adapter.Presentation.ImGui.ImGuiFontScope;
using HostStyle = Inno.Adapter.Presentation.ImGui.ImGuiFontStyle;
using NativeImGui = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.ImGui;

/// <summary>
/// Chooses composable font emphasis while leaving actual font ownership with the presentation host.
/// </summary>
[Flags]
public enum ImGuiFontStyle
{
    /// <summary>
    /// Uses the ordinary face.
    /// </summary>
    Regular = 0,
    /// <summary>
    /// Emphasizes text with the bold face.
    /// </summary>
    Bold = 1,
    /// <summary>
    /// Emphasizes text with the italic face.
    /// </summary>
    Italic = 2
}

/// <summary>
/// Uses the host's context-owned font catalog without exposing backend types to Editor features.
/// </summary>
public static unsafe class ImGuiFont
{
    /// <summary>
    /// Checks whether the current host registered an exact face.
    /// </summary>
    /// <param name="style">
    /// The requested emphasis.
    /// </param>
    /// <returns>
    /// True when the exact face is available.
    /// </returns>
    public static bool IsAvailable(ImGuiFontStyle style)
    {
        EditorUISession.current.EnsureDrawing();
        return HostFont.IsAvailable((HostStyle)style);
    }

    /// <summary>
    /// Pushes a host-owned font face for one synchronous control group.
    /// </summary>
    /// <param name="style">
    /// The requested emphasis.
    /// </param>
    /// <returns>
    /// An owner-window/frame scope; the host may use a registered nearest face.
    /// </returns>
    public static ImGuiFontScope PushStyle(ImGuiFontStyle style)
    {
        EditorUISession session = EditorUISession.current;
        session.EnsureDrawing();
        HostScope scope = HostFont.PushStyle((HostStyle)style);
        return new ImGuiFontScope(session, scope, NativeImGui.GetFrameCount(), NativeImGui.Raw.GetCurrentContext()->FontStack.Size);
    }
}

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
