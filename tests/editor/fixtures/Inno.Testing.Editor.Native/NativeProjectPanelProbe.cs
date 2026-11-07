using Inno.Editor.Core;
using Inno.Editor.Interactions;
using Inno.Editor.Scene;
using Inno.Editor.Graph;

namespace Inno.Testing.Editor.Native;

/// <summary>
/// Resolves the actual workspace and editing services through public Editor module composition.
/// </summary>
/// <param name="workspace">The composed scene workspace.</param>
/// <param name="graphs">The composed graph editor module.</param>
/// <param name="edits">The composed scene editing service.</param>
[EditorModule("tests.native-project-panels", order: 2000)]
public sealed class NativeProjectPanelProbe(
    IEditorSceneWorkspace workspace,
    GraphEditorModule graphs,
    SceneEdits edits
) : EditorModule {
    /// <summary>
    /// Gets the borrowed scene workspace.
    /// </summary>
    public IEditorSceneWorkspace workspace { get; } = workspace;
    /// <summary>
    /// Gets the borrowed graph editor module.
    /// </summary>
    public GraphEditorModule graphs { get; } = graphs;
    /// <summary>
    /// Gets the borrowed scene editing service.
    /// </summary>
    public SceneEdits edits { get; } = edits;
}
