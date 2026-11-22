using Inno.Editor.Core;
using Inno.Editor.Scene;

namespace Inno.Editor.Scripting.Tests;

[EditorModule("tests.scene-reload-probe", order: 102)]
public sealed class SceneReloadProbe : EditorModule
{
    public SceneReloadProbe(IEditorSceneWorkspace sceneWorkspace)
    {
        workspace = sceneWorkspace;
    }

    public static IEditorSceneWorkspace? workspace { get; private set; }

    public static void Reset() => workspace = null;
}
