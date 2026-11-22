using Inno.Editor.Core;
using Inno.Scripting.Api;

[assembly: ScriptingApiNamespace("InnoEditor.Core", "Inno.Editor.Core", ScriptingApiScope.Editor)]

[assembly: ScriptingApiExport(typeof(EditorKeyboardPolicy), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(EditorContext), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(EditorFrame), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(EditorStatisticID), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(EditorStatisticGroupID), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(EditorStatistic), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(EditorStatistics), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(EditorRuntime), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(EditorModule), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(EditorModuleAttribute), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(EditorState), ScriptingApiScope.Editor)]

[assembly: ScriptingApiExport(typeof(EditorPanel), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(EditorPanelAttribute), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(EditorModal), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(EditorModalAttribute), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(IEditorPanelReloadState), ScriptingApiScope.Editor)]

[assembly: ScriptingApiExport(typeof(EditorDisplayNames), ScriptingApiScope.Editor)]
