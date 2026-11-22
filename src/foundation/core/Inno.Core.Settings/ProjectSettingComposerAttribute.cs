using System;

namespace Inno.Core.Settings;

/// <summary>
/// Declares the protocol-owned composer used to combine contributions for one project setting.
/// Settings without a composer retain the default whole-value replacement behavior.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class ProjectSettingComposerAttribute : Attribute
{
    /// <summary>
    /// Creates a composer declaration for one stable setting protocol.
    /// </summary>
    /// <param name="settingID">
    /// The setting protocol composed by the attributed type.
    /// </param>
    public ProjectSettingComposerAttribute(string settingID)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(settingID);
        this.settingID = new ProjectSettingID(settingID);
    }

    /// <summary>
    /// Gets the stable setting protocol composed by the attributed type.
    /// </summary>
    public ProjectSettingID settingID { get; }
}
