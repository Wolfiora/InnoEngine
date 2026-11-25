using Inno.Editor.Core;
using Inno.Editor.Scene;

namespace Inno.Editor.Interactions.Tests;

[EditorModule("tests.scene-history-probe", order: 220)]
public sealed class SceneHistoryProbe : EditorModule
{
    public SceneHistoryProbe(
        SceneEdits sceneEdits,
        IEditorSceneWorkspace workspace
    ) {
        edits = sceneEdits;
        documents = workspace;
    }

    public static SceneEdits? edits { get; private set; }
    public static IEditorSceneWorkspace? documents { get; private set; }

    public static void Reset()
    {
        edits = null;
        documents = null;
    }
}
