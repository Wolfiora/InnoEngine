using Inno.Editor.Settings;
using Inno.Scripting.Api;

[assembly: ScriptingApiNamespace("InnoEditor.Settings", "Inno.Editor.Settings", ScriptingApiScope.Editor)]

[assembly: ScriptingApiExport(typeof(EditorSetting), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(EditorSettingObject), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(EditorSettingPathAttribute), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(EditorSettings), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(ProjectSettingPathAttribute), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(ProjectSettingEditor), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(ProjectSettingEditor<>), ScriptingApiScope.Editor)]
