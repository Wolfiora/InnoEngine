using Inno.Input;
using Inno.Scripting.Api;

[assembly: ScriptingApiNamespace("InnoEngine.Input", "Inno.Input", ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(InputSnapshot), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(IInputService), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(Input), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(InputDeviceState), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(InputDeviceChange), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(InputDeviceChangeKind), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(InputDeviceConnection), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(InputDeviceEvent), ScriptingApiScope.Runtime)]
