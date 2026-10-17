using Inno.Rendering.Assets.Authoring;
using Inno.Scripting.Api;

[assembly: ScriptingApiNamespace("InnoEditor.Rendering.Assets", "Inno.Rendering.Assets.Authoring", ScriptingApiScope.Editor)]

[assembly: ScriptingApiExport(typeof(ShaderFunctionAsset), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(ShaderSourceImportSettings), ScriptingApiScope.Editor)]
