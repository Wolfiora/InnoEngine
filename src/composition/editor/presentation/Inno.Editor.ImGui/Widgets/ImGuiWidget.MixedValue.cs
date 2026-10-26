using System;
using Inno.Native.ImGui;
using NativeImGui = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.ImGui.ImGuiWidget;
/// <summary>
/// Presents mixed values using the same controls and shared History.
/// </summary>
public sealed partial class ImGuiWidget
{
    private bool m_isMixedValue;

    /// <summary>
    /// Gets whether the currently drawn control group represents differing selected values.
    /// </summary>
    public bool isMixedValue => m_isMixedValue;

    /// <summary>
    /// Draws one control group with the native mixed-value state, restoring the previous item flags on failure.
    /// </summary>
    /// <param name="mixed">
    /// Whether the current selection contains different values.
    /// </param>
    /// <param name="draw">
    /// Synchronous control drawing; never retained.
    /// </param>
    public void MixedValue(
        bool mixed,
        Action draw
    ) {
        m_session.EnsureDrawing();
        ArgumentNullException.ThrowIfNull(draw);
        bool previous = m_isMixedValue;
        m_isMixedValue = mixed || previous;
        NativeImGui.PushItemFlag((int)ImGuiItemFlagsPrivate.MixedValue, mixed);
        try
        {
            draw();
        }
        finally
        {
            NativeImGui.PopItemFlag();
            m_isMixedValue = previous;
        }
    }
}
