using Inno.Core.Mathematics;

namespace Inno.Editor.Inspection;

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
        {
            context.SetValue(value, "x");
        }
        ComponentFieldDrawer.Next();
        if (ComponentFieldDrawer.Int(context.path, "Y", ref value.y, width))
        {
            context.SetValue(value, "y");
        }
        ComponentFieldDrawer.Next();
        if (ComponentFieldDrawer.Int(context.path, "W", ref value.width, width))
        {
            context.SetValue(value, "width");
        }
        ComponentFieldDrawer.Next();
        if (ComponentFieldDrawer.Int(context.path, "H", ref value.height, width))
        {
            context.SetValue(value, "height");
        }
    }
}
