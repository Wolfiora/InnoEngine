using Inno.Core.Mathematics;

namespace Inno.Editor.Inspection;

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
        {
            context.SetValue(Quaternion.FromEulerAnglesXYZDegrees(eulerDegrees).normalized, "euler.x");
        }
        ComponentFieldDrawer.Next();
        if (ComponentFieldDrawer.Float(context.path, "Y", ref eulerDegrees.y, width))
        {
            context.SetValue(Quaternion.FromEulerAnglesXYZDegrees(eulerDegrees).normalized, "euler.y");
        }
        ComponentFieldDrawer.Next();
        if (ComponentFieldDrawer.Float(context.path, "Z", ref eulerDegrees.z, width))
        {
            context.SetValue(Quaternion.FromEulerAnglesXYZDegrees(eulerDegrees).normalized, "euler.z");
        }
    }
}
