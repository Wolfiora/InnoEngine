using System;
using System.Numerics;
using Inno.Native.ImGui;
using NativeImGui = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.ImGui.ImGuiWidget;
/// <summary>
/// Provides reusable editor controls and rendering helpers built on the native ImGui API.
/// </summary>
public sealed unsafe partial class ImGuiWidget
{
    private float m_appliedZoom = float.NaN;
    private bool? m_appliedCompactMode;
    /// <summary>
    /// Gets the centralized editor layout metrics shared by every widget and feature panel.
    /// </summary>
    public EditorStyleMetrics style { get; }

    /// <summary>
    /// Applies the centralized editor layout metrics and palette to the current ImGui context.
    /// </summary>
    public void SetupStyle()
    {
        m_session.EnsureDrawing();
        m_session.PrepareStyleInput();
        ImGuiStyle* nativeStyle = NativeImGui.Raw.GetStyle();
        ApplyLayoutStyle(nativeStyle);
        EditorPalette.Apply(nativeStyle);
        m_appliedZoom = style.zoom;
        m_appliedCompactMode = style.isCompact;
    }

    internal void ApplyPendingStyle()
    {
        m_session.PrepareStyleInput();
        if (float.IsNaN(m_appliedZoom) || MathF.Abs(m_appliedZoom - style.zoom) > 0.0001f || m_appliedCompactMode != style.isCompact)
            SetupStyle();
    }

    private void ApplyLayoutStyle(ImGuiStyle* nativeStyle)
    {
        nativeStyle->Alpha = EditorPalette.opacityOpaque;
        nativeStyle->DisabledAlpha = style.disabledAlpha;
        nativeStyle->FontScaleMain = style.fontScale;
        nativeStyle->WindowPadding = ImGui.ToNative(style.windowPadding);
        nativeStyle->WindowRounding = style.windowRounding;
        nativeStyle->WindowBorderSize = style.borderSize;
        nativeStyle->WindowMinSize = ImGui.ToNative(style.windowMinimumSize);
        nativeStyle->WindowTitleAlign = ImGui.ToNative(new Vector2(0.5f, 0.5f));
        nativeStyle->WindowMenuButtonPosition = ImGuiDir.None;
        nativeStyle->ChildRounding = style.windowRounding;
        nativeStyle->ChildBorderSize = style.borderSize;
        nativeStyle->PopupRounding = style.menuRounding;
        nativeStyle->PopupBorderSize = 0f;
        nativeStyle->FramePadding = ImGui.ToNative(style.framePadding);
        nativeStyle->FrameRounding = style.frameRounding;
        nativeStyle->FrameBorderSize = 0f;
        nativeStyle->ItemSpacing = ImGui.ToNative(style.itemSpacing);
        nativeStyle->ItemInnerSpacing = ImGui.ToNative(style.itemInnerSpacing);
        nativeStyle->CellPadding = ImGui.ToNative(style.cellPadding);
        nativeStyle->IndentSpacing = style.indentSpacing;
        nativeStyle->SeparatorTextPadding = ImGui.ToNative(style.sectionHeaderPadding);
        nativeStyle->SeparatorTextBorderSize = style.borderSize;
        nativeStyle->ColumnsMinSpacing = style.columnMinimumSpacing;
        nativeStyle->ScrollbarSize = style.scrollbarSize;
        nativeStyle->ScrollbarPadding = style.scrollbarPadding;
        nativeStyle->ScrollbarRounding = style.frameRounding;
        nativeStyle->GrabMinSize = style.grabMinimumSize;
        nativeStyle->GrabRounding = style.frameRounding;
        nativeStyle->TabRounding = style.windowRounding;
        nativeStyle->TabBorderSize = 0f;
        nativeStyle->TabBarOverlineSize = 0f;
        nativeStyle->ColorButtonPosition = ImGuiDir.Right;
        nativeStyle->ButtonTextAlign = ImGui.ToNative(new Vector2(0.5f, 0.5f));
        nativeStyle->SelectableTextAlign = ImGui.ToNative(Vector2.Zero);
    }
}
