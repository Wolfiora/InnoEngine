using Inno.Core.Mathematics;
using Inno.Editor.ImGui;

using Gui = Inno.Editor.ImGui.ImGui;
using NumericsVector4 = System.Numerics.Vector4;

namespace Inno.Editor.Inspection;

[PropertyDrawer(typeof(Color))]
internal sealed unsafe class ColorPropertyDrawer : IPropertyDrawer
{
    /// <summary>
    /// Renders the value presentation for the current editor frame.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    public void Draw(PropertyDrawContext context)
    {
        Color value = context.GetValue() is Color current ? current : default;
        var nativeValue = new NumericsVector4(value.r, value.g, value.b, value.a);
        ImGuiColorEditFlags flags = context.hdrColor
                                    ? ImGuiColorEditFlags.Hdr
                                      | ImGuiColorEditFlags.Float
                                      | ImGuiColorEditFlags.InputRgb
                                      | ImGuiColorEditFlags.DisplayRgb
                                    : ImGuiColorEditFlags.None;
        if (Gui.ColorEditLinear4($"##{context.path}", ref nativeValue, flags))
        {
            context.SetValue(new Color(nativeValue.X, nativeValue.Y, nativeValue.Z, nativeValue.W));
        }
    }
}
