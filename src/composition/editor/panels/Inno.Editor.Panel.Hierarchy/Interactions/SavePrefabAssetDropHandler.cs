using System;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Editor.Core;
using Inno.Editor.Interactions;
using Inno.Editor.Scene;
using Inno.Scene;

namespace Inno.Editor.Panel.Hierarchy;

[EditorDrop(HierarchyInteractionIDs.C_FILE_BROWSER_AREA)]
internal sealed class SavePrefabAssetDropHandler : EditorDrop<GameObject, string>
{
    private readonly IEditorSceneWorkspace m_workspace;
    private readonly AssetPipeline m_assets;

    internal SavePrefabAssetDropHandler(
        IEditorSceneWorkspace workspace,
        AssetPipeline assets
    ) {
        ArgumentNullException.ThrowIfNull(workspace);
        ArgumentNullException.ThrowIfNull(assets);
        m_workspace = workspace;
        m_assets = assets;
    }

    /// <summary>
    /// Evaluates whether the requested change can be applied to the current generation.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    /// <returns>
    /// The validated editor drop status that represents the completed operation.
    /// </returns>
    protected override EditorDropStatus Query(EditorDropContext<GameObject, string> context)
        => m_workspace.canPersist && context.source.isRuntimeValid
           ? EditorDropStatus.Accept()
           : EditorDropStatus.rejected;

    /// <summary>
    /// Validates and applies the current editor drag-and-drop interaction atomically.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    /// <returns>
    /// The validated editor drop result that represents the completed operation.
    /// </returns>
    protected override EditorDropResult Drop(EditorDropContext<GameObject, string> context)
    {
        string path = m_workspace.SavePrefab(context.source, context.target);
        if (!m_assets.TryGetFileSystemEntry(AssetPath.Parse(path), out AssetFileEntry selection))
        {
            return EditorDropResult.rejected;
        }
        _ = context.interactions.For(context.area, selection).Select();
        return EditorDropResult.Accepted(selection);
    }
}
