using System.Numerics;
using Inno.Editor.ImGui;

namespace Inno.Editor.Panel.Inspector;

internal static class InspectorTargetStyle
{
    /// <summary>
    /// Gets the subdued background of Tag labels in Inspector target headers.
    /// </summary>
    internal static Vector4 tagLabel { get; } = new(0.34f, 0.29f, 0.44f, EditorPalette.C_OPACITY_OPAQUE);

    /// <summary>
    /// Gets the subdued background of Layer labels in Inspector target headers.
    /// </summary>
    internal static Vector4 layerLabel { get; } = new(0.24f, 0.34f, 0.4f, EditorPalette.C_OPACITY_OPAQUE);
}
