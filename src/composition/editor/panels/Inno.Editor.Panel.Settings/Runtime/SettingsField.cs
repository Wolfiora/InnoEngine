using Inno.Editor.Settings;

namespace Inno.Editor.Panel.Settings;

internal sealed class SettingsField
{
    private readonly BuildSettingsField? m_build;
    private readonly EditorSetting? m_editor;
    private readonly ProjectSettingEditor? m_project;

    internal SettingsField(EditorSetting definition)
    {
        m_editor = definition;
        this.scope = SettingsScope.Editor;
        this.path = definition.path;
        this.pagePath = definition.pagePath;
        this.label = definition.label;
        this.order = definition.order;
        this.section = definition.section ?? string.Empty;
        this.description = definition.description;
    }

    internal SettingsField(ProjectSettingEditor definition)
    {
        m_project = definition;
        this.scope = SettingsScope.Project;
        this.path = definition.path;
        this.pagePath = definition.pagePath;
        this.label = definition.label;
        this.order = definition.order;
        this.section = definition.section ?? string.Empty;
        this.description = definition.description;
    }

    internal SettingsField(BuildSettingsField definition)
    {
        m_build = definition;
        this.scope = SettingsScope.Build;
        this.path = definition.path;
        this.pagePath = definition.pagePath;
        this.label = definition.label;
        this.order = 0;
        this.section = definition.section;
        this.description = definition.description;
    }

    internal SettingsScope scope { get; }
    internal string path { get; }
    internal string pagePath { get; }
    internal string label { get; }
    internal int order { get; }
    internal string section { get; }
    internal string description { get; }
    internal EditorSetting? editor => m_editor;
    internal ProjectSettingEditor? project => m_project;
    internal BuildSettingsField? build => m_build;
}
