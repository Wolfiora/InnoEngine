using Inno.Editor.Assets;
using Inno.Assets.Pipeline;
using Inno.Scripting.Api;

[assembly: ScriptingApiNamespace("InnoEditor.Assets", "Inno.Editor.Assets", ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(AssetCreationMenuAttribute), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(AssetCreationContext), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(AssetCreationTemplate), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(AssetCreationTemplate<>), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(AssetEditor), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(AssetFileEntry), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(AssetEditorAttribute), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(AssetEditorContext), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(AssetOperationValidation), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(AssetImportSettingsEdits), ScriptingApiScope.Editor)]
