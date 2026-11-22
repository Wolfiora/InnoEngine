using System;
using Inno.Editor.Interactions;
using Inno.Rendering.Assets;

namespace Inno.Editor.Panel.ShaderEditor;

[EditorAction("editor/open", priority: 1000)]
internal sealed class OpenShaderEditorAction : EditorAction<ShaderAsset, string>
{
    /// <summary>
    /// Evaluates the operation's current availability and presentation state.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    /// <returns>
    /// The validated editor action state that represents the completed operation.
    /// </returns>
    protected override EditorActionState Query(EditorActionContext<ShaderAsset, string> context) => EditorActionState.enabled;

    /// <summary>
    /// Applies the editor action to the supplied interaction context.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    protected override void Execute(EditorActionContext<ShaderAsset, string> context)
    {
        context.interactions.SetSelection(context.target);
        if (!context.interactions.OpenPanel("rendering.shader-editor"))
        {
            throw new InvalidOperationException("Shader Editor is unavailable.");
        }
    }
}
