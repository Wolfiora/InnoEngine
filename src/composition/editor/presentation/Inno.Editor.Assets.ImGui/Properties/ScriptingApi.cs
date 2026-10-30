using Inno.Editor.Assets.ImGui;
using Inno.Scripting.Api;

[assembly: ScriptingApiNamespace("InnoEditor.Assets", "Inno.Editor.Assets.ImGui", ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(AssetBrowserRoot), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(AssetBrowserState), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(AssetBrowserStyle), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(AssetIconAttribute), ScriptingApiScope.Editor)]
