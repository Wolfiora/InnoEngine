using Inno.Core.Serialization;
using Inno.Extensibility.Types;

namespace Inno.Core.Settings;

/// <summary>
/// Stores the editable identity namespace of the current project.
/// </summary>
[StableTypeID("d5f90462-506d-4ff7-aa9f-0f14da28b9c3")]
[ProjectSettingDefinition(ProjectIdentitySettings.C_SETTING_PROTOCOL_ID, allowPluginContributions: false)]
public sealed class ProjectIdentitySettings : ISerializable
{
    /// <summary>
    /// Gets the immutable project-setting protocol value used by discovery metadata.
    /// </summary>
    public const string C_SETTING_PROTOCOL_ID = "inno.project.identity";

    private string m_projectID = "inno.project";

    /// <summary>
    /// Gets the stable project setting protocol identity.
    /// </summary>
    public static ProjectSettingID settingID => new(C_SETTING_PROTOCOL_ID);

    /// <summary>
    /// Gets or sets the editable project namespace.
    /// </summary>
    [SerializableProperty]
    public string projectId
    {
        get => m_projectID;
        set => m_projectID = new ProjectID(value).value;
    }

    /// <summary>
    /// Gets the validated project identifier.
    /// </summary>
    public ProjectID id => new(m_projectID);

    /// <summary>
    /// Qualifies a display name under the current project identifier.
    /// </summary>
    /// <param name="name">
    /// The project-local display name.
    /// </param>
    /// <returns>
    /// The complete project-scoped identity.
    /// </returns>
    public ProjectScopedID Qualify(string name) => id.Qualify(ProjectLocalID.FromName(name));
}
