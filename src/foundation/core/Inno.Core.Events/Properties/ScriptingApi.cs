using Inno.Core.Events;
using Inno.Scripting.Api;

[assembly: ScriptingApiNamespace("InnoEngine.Events", "Inno.Core.Events", ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(Event), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(EventDispatcher), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(EventHub), ScriptingApiScope.Runtime)]
