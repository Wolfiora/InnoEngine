using System.Numerics;
using Inno.Editor.Annotations;

using Gui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Inspection;

[InspectorAttributeDrawer(typeof(SpaceAttribute))]
internal sealed class SpaceInspectorAttributeDrawer : IInspectorAttributeDrawer
{
    /// <summary>
    /// Renders the requested vertical spacing before its annotated property.
    /// </summary>
    /// <param name="context">
    /// Active attribute drawing context.
    /// </param>
    public void DrawBefore(InspectorAttributeDrawContext context)
    {
        float height = ((SpaceAttribute)context.attribute).height;
        Gui.Dummy(new Vector2(0f, height));
    }
}
