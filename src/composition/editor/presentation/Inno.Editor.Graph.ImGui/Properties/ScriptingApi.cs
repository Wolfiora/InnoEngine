using Inno.Scripting.Api;
using Inno.Editor.Graph.ImGui;

[assembly: ScriptingApiNamespace("InnoEditor.Graph", "Inno.Editor.Graph.ImGui", ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(GraphCanvasView), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(GraphCanvasNode), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(GraphCanvasPort), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(GraphCanvasLayout), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(GraphCanvasPointerInput), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(GraphCanvasPointerResult), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(GraphCanvasPointerController), ScriptingApiScope.Editor)]
