using Inno.Core.Mathematics;

namespace Inno.Editor.Inspection;

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
        {
            context.SetValue(value, "x");
        }
        ComponentFieldDrawer.Next();
        if (ComponentFieldDrawer.Float(context.path, "Y", ref value.y, width))
        {
            context.SetValue(value, "y");
        }
        ComponentFieldDrawer.Next();
        if (ComponentFieldDrawer.Float(context.path, "Z", ref value.z, width))
        {
            context.SetValue(value, "z");
        }
    }
}
