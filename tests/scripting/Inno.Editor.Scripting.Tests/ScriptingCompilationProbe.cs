using Inno.Editor.Core;

namespace Inno.Editor.Scripting.Tests;

[EditorModule("tests.scripting-compilation-probe", order: 101)]
public sealed class ScriptingCompilationProbe : EditorModule
{
    public ScriptingCompilationProbe(IEditorScriptCompilation scripting)
    {
        compilation = scripting;
    }

    public static IEditorScriptCompilation? compilation { get; private set; }

    public static void Reset() => compilation = null;
}
