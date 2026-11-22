using Inno.Core.Serialization;

namespace Inno.Core.Settings;

/// <summary>
/// Stores project-authored setting contributions in one native document.
/// </summary>
public sealed class ProjectSettingsDocument : ISerializable
{
    /// <summary>
    /// Gets or sets project-authored protocol contributions.
    /// </summary>
    [SerializableProperty]
    public ProjectSettingRecord[] overrides { get; set; } = [];
}
