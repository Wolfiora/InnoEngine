using Inno.Scripting.Api;
using Inno.Editor.Graph.ImGui;

[assembly: ScriptingApiNamespace("InnoEditor.Graph", "Inno.Editor.Graph.ImGui", ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(GraphCanvasView), ScriptingApiScope.Editor)]
