using System;
using System.IO;
using System.Numerics;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Core.Execution;
using Inno.Editor.Core;
using Inno.Editor.Shaders;
using Inno.Rendering.Assets;

using EditorUI = Inno.Editor.ImGui.EditorUISession;
using Gui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Panel.ShaderEditor;

[EditorPanel("rendering.shader-editor", "Shader Editor", order: 230, menuPath: "Authoring")]
internal sealed class ShaderEditorPanel(ShaderEditorDocuments documents) : EditorPanel
{
    /// <summary>
    /// Gets whether use window padding is enabled for this implementation.
    /// </summary>
    public override bool useWindowPadding => true;

    /// <summary>
    /// Gets whether allow scrolling is enabled for this implementation.
    /// </summary>
    public override bool allowScrolling => false;

    /// <summary>
    /// Gets the preferred initial window size in logical editor units.
    /// </summary>
    public override Vector2 initialSize => new(960f, 640f);

    /// <summary>
    /// Draws this feature using the current editor presentation context.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    protected override void OnDraw(EditorContext context)
    {
        AssetFileEntry? entry = documents.interactions.selection.selectedTarget as AssetFileEntry;
        if (documents.interactions.selection.selectedTarget is ShaderAsset shader)
        {
            _ = documents.assets.TryGetFileSystemEntry(shader.assetPath, out entry);
        }
        if (documents.interactions.selection.selectedTarget is ShaderInspectionSelection selection
            && documents.assets.TryGetInfo(selection.assetID, out AssetInfo? info)
            && info is not null
        ) {
            _ = documents.assets.TryGetFileSystemEntry(info.assetPath, out entry);
        }
        if (entry is null || entry.isDirectory || !entry.assetPath.localPath.EndsWith(".ishader", StringComparison.OrdinalIgnoreCase))
        {
            ShaderEditorCanvas.DrawHeader(documents, null);
            EditorUI.current.widgets.CenteredWrappedText(
                "Select a Shader (.ishader) in the File Browser.",
                Vector2.Max(Vector2.One, Gui.GetContentRegionAvailable()),
                new Vector2(40f, 28f)
            );
            return;
        }

        try
        {
            new ShaderEditorCanvas(documents, documents.Open(entry)).Draw();
        }
        catch (Exception failure) when ((failure is IOException or InvalidOperationException or FormatException)
                                        && RetirementPendingException.Find(failure) is null)
        {
            EditorUI.current.widgets.WrappedText(failure.Message);
        }
    }
}
