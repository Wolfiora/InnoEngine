using Inno.Core.Identity;
using Inno.Scripting.Api;

[assembly: ScriptingApiNamespace("InnoEngine.Core", "Inno.Core.Identity", ScriptingApiScope.Runtime)]

[assembly: ScriptingApiExport(typeof(IdentityObject), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(Identity), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(IdentityDomainID), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(RuntimeIdentity), ScriptingApiScope.Runtime)]
