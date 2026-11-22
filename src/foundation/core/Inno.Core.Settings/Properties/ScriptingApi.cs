using Inno.Core.Settings;
using Inno.Scripting.Api;

[assembly: ScriptingApiNamespace("InnoEngine.Settings", "Inno.Core.Settings", ScriptingApiScope.Runtime)]

[assembly: ScriptingApiExport(typeof(ProjectSettingID), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(ProjectID), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(ProjectLocalID), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(ProjectScopedID), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(ProjectIdentitySettings), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(ProjectSettingDefinitionAttribute), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(ProjectSettingComposerAttribute), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(ProjectSettingContributionSource), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(ProjectSettingContributionContext), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(ProjectSettingContribution<>), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(ProjectSettingComposer), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(ProjectSettingComposer<,>), ScriptingApiScope.Runtime)]
[assembly: ScriptingApiExport(typeof(Settings), ScriptingApiScope.Runtime)]
