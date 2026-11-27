using Inno.Core.Coroutines;
using Inno.Scripting.Api;

[assembly: ScriptingApiNamespace("InnoEngine.Coroutines", "Inno.Core.Coroutines", ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(CoroutineHandle), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(YieldInstruction), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(WaitForFrames), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(WaitForSeconds), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(WaitForTask), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(WaitUntil), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(WaitWhile), ScriptingApiScope.Runtime)]
