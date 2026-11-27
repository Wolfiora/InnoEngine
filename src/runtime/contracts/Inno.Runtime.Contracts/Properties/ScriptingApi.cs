using Inno.Runtime.Contracts;
using Inno.Scripting.Api;

[assembly: ScriptingApiNamespace("InnoEngine.Runtime", "Inno.Runtime.Contracts", ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(RuntimeSubsystemExtensionAttribute), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(RuntimeSubsystemLifetime), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(IRuntimeSubsystemActivator), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(IRuntimeSubsystem), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(RuntimeSubsystem), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(RuntimeSubsystemContext), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(RuntimeFrame), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(RuntimeFixedFrame), ScriptingApiScope.Runtime)]
