using Inno.Editor.PlayMode;
using Inno.Scripting.Api;

[assembly: ScriptingApiNamespace("InnoEditor.PlayMode", "Inno.Editor.PlayMode", ScriptingApiScope.Editor)]

[assembly: ScriptingApiExport(typeof(EditorPlayModeState), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(IEditorPlayMode), ScriptingApiScope.Editor)]
