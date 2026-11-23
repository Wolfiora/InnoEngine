using Inno.Editor.Panel.Hierarchy;
using Inno.Scripting.Api;

[assembly: ScriptingApiNamespace("InnoEditor.Hierarchy", "Inno.Editor.Panel.Hierarchy", ScriptingApiScope.Editor)]

[assembly: ScriptingApiExport(typeof(HierarchyObjectDropTarget), ScriptingApiScope.Editor)]
[assembly: ScriptingApiExport(typeof(HierarchySceneDropTarget), ScriptingApiScope.Editor)]
