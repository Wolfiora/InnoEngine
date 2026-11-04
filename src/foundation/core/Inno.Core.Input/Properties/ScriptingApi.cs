using Inno.Core.Input;
using Inno.Scripting.Api;

[assembly: ScriptingApiNamespace(
    "InnoEngine.Input",
    "Inno.Core.Input",
    ScriptingApiScope.Runtime)]

[assembly: ScriptingApiExport(typeof(KeyCode), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(KeyModifier), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(MouseButton), ScriptingApiScope.Runtime)]

[assembly: ScriptingApiExport(typeof(InputChange), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(KeyInputChange), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(TextInputChange), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(PointerInputChange), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(PointerButtonInputChange), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(PointerMoveInputChange), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(PointerScrollInputChange), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(InputSequence), ScriptingApiScope.Runtime)]
