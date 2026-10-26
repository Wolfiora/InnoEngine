using UiDrawing = Inno.Editor.ImGui.ImGui;
using EditorUI = Inno.Editor.ImGui.EditorUiSession;
using System;
using Inno.Core.Mathematics;
using Inno.Editor.ImGui;
using Inno.Editor.ImGui.ImGuiWidget;
using EditorImGui = Inno.Editor.ImGui.ImGui;
using Ui = Inno.Editor.ImGui.ImGui;
using NumericsVector4 = System.Numerics.Vector4;

namespace Inno.Editor.Inspection;
internal static unsafe class ComponentFieldDrawer
{
    internal static float GetWidth(int count)
    {
        float spacing = UiDrawing.GetLayout().itemSpacing.X;
        return MathF.Max(1f, (Ui.GetContentRegionAvailable().X - spacing * (count - 1)) / count);
    }

    internal static bool Float(
        string path,
        string label,
        ref float value,
        float width
    ) {
        return EditorUI.current.widgets.AxisDragFloat(path, label, ref value, width);
    }

    internal static bool Int(
        string path,
        string label,
        ref int value,
        float width
    ) {
        return EditorUI.current.widgets.AxisDragInt(path, label, ref value, width);
    }

    internal static void Next() => Ui.SameLine(0f, -1f);
}

[PropertyDrawer(typeof(Vector2))]
internal sealed unsafe class Vector2PropertyDrawer : IPropertyDrawer
{
    /// <summary>
    /// Renders the value presentation for the current editor frame.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    public void Draw(PropertyDrawContext context)
    {
        Vector2 value = context.GetValue() is Vector2 current ? current : default;
        float width = ComponentFieldDrawer.GetWidth(2);
        if (ComponentFieldDrawer.Float(context.path, "X", ref value.x, width))
            context.SetValue(value, "x");
        ComponentFieldDrawer.Next();
        if (ComponentFieldDrawer.Float(context.path, "Y", ref value.y, width))
            context.SetValue(value, "y");
    }
}

[PropertyDrawer(typeof(Vector3))]
internal sealed unsafe class Vector3PropertyDrawer : IPropertyDrawer
{
    /// <summary>
    /// Renders the value presentation for the current editor frame.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    public void Draw(PropertyDrawContext context)
    {
        Vector3 value = context.GetValue() is Vector3 current ? current : default;
        float width = ComponentFieldDrawer.GetWidth(3);
        if (ComponentFieldDrawer.Float(context.path, "X", ref value.x, width))
            context.SetValue(value, "x");
        ComponentFieldDrawer.Next();
        if (ComponentFieldDrawer.Float(context.path, "Y", ref value.y, width))
            context.SetValue(value, "y");
        ComponentFieldDrawer.Next();
        if (ComponentFieldDrawer.Float(context.path, "Z", ref value.z, width))
            context.SetValue(value, "z");
    }
}

[PropertyDrawer(typeof(Vector4))]
internal sealed unsafe class Vector4PropertyDrawer : IPropertyDrawer
{
    /// <summary>
    /// Renders the value presentation for the current editor frame.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    public void Draw(PropertyDrawContext context)
    {
        Vector4 value = context.GetValue() is Vector4 current ? current : default;
        float width = ComponentFieldDrawer.GetWidth(4);
        if (ComponentFieldDrawer.Float(context.path, "X", ref value.x, width))
            context.SetValue(value, "x");
        ComponentFieldDrawer.Next();
        if (ComponentFieldDrawer.Float(context.path, "Y", ref value.y, width))
            context.SetValue(value, "y");
        ComponentFieldDrawer.Next();
        if (ComponentFieldDrawer.Float(context.path, "Z", ref value.z, width))
            context.SetValue(value, "z");
        ComponentFieldDrawer.Next();
        if (ComponentFieldDrawer.Float(context.path, "W", ref value.w, width))
            context.SetValue(value, "w");
    }
}

[PropertyDrawer(typeof(Vector2Int))]
internal sealed unsafe class Vector2IntPropertyDrawer : IPropertyDrawer
{
    /// <summary>
    /// Renders the value presentation for the current editor frame.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    public void Draw(PropertyDrawContext context)
    {
        Vector2Int value = context.GetValue() is Vector2Int current ? current : default;
        float width = ComponentFieldDrawer.GetWidth(2);
        if (ComponentFieldDrawer.Int(context.path, "X", ref value.x, width))
            context.SetValue(value, "x");
        ComponentFieldDrawer.Next();
        if (ComponentFieldDrawer.Int(context.path, "Y", ref value.y, width))
            context.SetValue(value, "y");
    }
}

[PropertyDrawer(typeof(Vector3Int))]
internal sealed unsafe class Vector3IntPropertyDrawer : IPropertyDrawer
{
    /// <summary>
    /// Renders the value presentation for the current editor frame.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    public void Draw(PropertyDrawContext context)
    {
        Vector3Int value = context.GetValue() is Vector3Int current ? current : default;
        float width = ComponentFieldDrawer.GetWidth(3);
        if (ComponentFieldDrawer.Int(context.path, "X", ref value.x, width))
            context.SetValue(value, "x");
        ComponentFieldDrawer.Next();
        if (ComponentFieldDrawer.Int(context.path, "Y", ref value.y, width))
            context.SetValue(value, "y");
        ComponentFieldDrawer.Next();
        if (ComponentFieldDrawer.Int(context.path, "Z", ref value.z, width))
            context.SetValue(value, "z");
    }
}

[PropertyDrawer(typeof(Vector4Int))]
internal sealed unsafe class Vector4IntPropertyDrawer : IPropertyDrawer
{
    /// <summary>
    /// Renders the value presentation for the current editor frame.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    public void Draw(PropertyDrawContext context)
    {
        Vector4Int value = context.GetValue() is Vector4Int current ? current : default;
        float width = ComponentFieldDrawer.GetWidth(4);
        if (ComponentFieldDrawer.Int(context.path, "X", ref value.x, width))
            context.SetValue(value, "x");
        ComponentFieldDrawer.Next();
        if (ComponentFieldDrawer.Int(context.path, "Y", ref value.y, width))
            context.SetValue(value, "y");
        ComponentFieldDrawer.Next();
        if (ComponentFieldDrawer.Int(context.path, "Z", ref value.z, width))
            context.SetValue(value, "z");
        ComponentFieldDrawer.Next();
        if (ComponentFieldDrawer.Int(context.path, "W", ref value.w, width))
            context.SetValue(value, "w");
    }
}

[PropertyDrawer(typeof(Rect))]
internal sealed unsafe class RectPropertyDrawer : IPropertyDrawer
{
    /// <summary>
    /// Renders the value presentation for the current editor frame.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    public void Draw(PropertyDrawContext context)
    {
        Rect value = context.GetValue() is Rect current ? current : default;
        float width = ComponentFieldDrawer.GetWidth(4);
        if (ComponentFieldDrawer.Float(context.path, "X", ref value.x, width))
            context.SetValue(value, "x");
        ComponentFieldDrawer.Next();
        if (ComponentFieldDrawer.Float(context.path, "Y", ref value.y, width))
            context.SetValue(value, "y");
        ComponentFieldDrawer.Next();
        if (ComponentFieldDrawer.Float(context.path, "W", ref value.width, width))
            context.SetValue(value, "width");
        ComponentFieldDrawer.Next();
        if (ComponentFieldDrawer.Float(context.path, "H", ref value.height, width))
            context.SetValue(value, "height");
    }
}

[PropertyDrawer(typeof(RectInt))]
internal sealed unsafe class RectIntPropertyDrawer : IPropertyDrawer
{
    /// <summary>
    /// Renders the value presentation for the current editor frame.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    public void Draw(PropertyDrawContext context)
    {
        RectInt value = context.GetValue() is RectInt current ? current : default;
        float width = ComponentFieldDrawer.GetWidth(4);
        if (ComponentFieldDrawer.Int(context.path, "X", ref value.x, width))
            context.SetValue(value, "x");
        ComponentFieldDrawer.Next();
        if (ComponentFieldDrawer.Int(context.path, "Y", ref value.y, width))
            context.SetValue(value, "y");
        ComponentFieldDrawer.Next();
        if (ComponentFieldDrawer.Int(context.path, "W", ref value.width, width))
            context.SetValue(value, "width");
        ComponentFieldDrawer.Next();
        if (ComponentFieldDrawer.Int(context.path, "H", ref value.height, width))
            context.SetValue(value, "height");
    }
}

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
        var flags = context.hdrColor ? Inno.Editor.ImGui.ImGuiColorEditFlags.Hdr | Inno.Editor.ImGui.ImGuiColorEditFlags.Float | Inno.Editor.ImGui.ImGuiColorEditFlags.InputRgb | Inno.Editor.ImGui.ImGuiColorEditFlags.DisplayRgb : Inno.Editor.ImGui.ImGuiColorEditFlags.None;
        if (EditorImGui.ColorEditLinear4($"##{context.path}", ref nativeValue, flags))
        {
            context.SetValue(new Color(nativeValue.X, nativeValue.Y, nativeValue.Z, nativeValue.W));
        }
    }
}

[PropertyDrawer(typeof(Quaternion))]
internal sealed unsafe class QuaternionPropertyDrawer : IPropertyDrawer
{
    /// <summary>
    /// Renders the value presentation for the current editor frame.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    public void Draw(PropertyDrawContext context)
    {
        Quaternion value = context.GetValue() is Quaternion current ? current : Quaternion.identity;
        Vector3 eulerDegrees = value.ToEulerAnglesXYZDegrees();
        float width = ComponentFieldDrawer.GetWidth(3);
        if (ComponentFieldDrawer.Float(context.path, "X", ref eulerDegrees.x, width))
            context.SetValue(Quaternion.FromEulerAnglesXYZDegrees(eulerDegrees).normalized, "euler.x");
        ComponentFieldDrawer.Next();
        if (ComponentFieldDrawer.Float(context.path, "Y", ref eulerDegrees.y, width))
            context.SetValue(Quaternion.FromEulerAnglesXYZDegrees(eulerDegrees).normalized, "euler.y");
        ComponentFieldDrawer.Next();
        if (ComponentFieldDrawer.Float(context.path, "Z", ref eulerDegrees.z, width))
            context.SetValue(Quaternion.FromEulerAnglesXYZDegrees(eulerDegrees).normalized, "euler.z");
    }
}
