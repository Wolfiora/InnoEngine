using Inno.Editor.Shaders.ImGui;
using Inno.Scripting.Api;

[assembly: ScriptingApiNamespace("InnoEditor.Shaders", "Inno.Editor.Shaders.ImGui", ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(ShaderNodeDrawerAttribute), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(ShaderNodeDrawer), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(ShaderNodeDrawContext), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(ShaderPropertyInspector), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(ShaderPreviews), ScriptingApiScope.Editor)]
