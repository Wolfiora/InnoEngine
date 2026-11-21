using Inno.Runtime;
using Inno.Scripting.Api;

[assembly: ScriptingApiNamespace("InnoEngine.Core", "Inno.Runtime", ScriptingApiScope.Runtime)]

[assembly: ScriptingApiExport(typeof(Time), ScriptingApiScope.Runtime)]
