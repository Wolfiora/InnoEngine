using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Core.Settings;

/// <summary>
/// Describes one dependency-ordered provider of default setting values.
/// </summary>
public sealed class ProjectSettingsContributor
{
    private readonly ProjectSettingRecord[] m_settings;

    /// <summary>
    /// Creates one settings contributor.
    /// </summary>
    /// <param name="id">
    /// Stable contributor identity.
    /// </param>
    /// <param name="dependencies">
    /// Contributors that must precede this contributor.
    /// </param>
    /// <param name="overrides">
    /// Dependencies whose defaults may be explicitly replaced.
    /// </param>
    /// <param name="settings">
    /// Default setting contributions.
    /// </param>
    public ProjectSettingsContributor(
        string id,
        IEnumerable<string> dependencies,
        IEnumerable<string> overrides,
        IEnumerable<ProjectSettingRecord> settings
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentNullException.ThrowIfNull(dependencies);
        ArgumentNullException.ThrowIfNull(overrides);
        ArgumentNullException.ThrowIfNull(settings);
        this.id = id;
        this.dependencies = Array.AsReadOnly(dependencies.ToArray());
        this.overrides = Array.AsReadOnly(overrides.ToArray());
        m_settings = settings.Select(Clone).ToArray();
    }

    /// <summary>
    /// Gets the stable contributor identity.
    /// </summary>
    public string id { get; }

    /// <summary>
    /// Gets contributors that must precede this contributor.
    /// </summary>
    public IReadOnlyList<string> dependencies { get; }

    /// <summary>
    /// Gets dependencies whose defaults may be replaced.
    /// </summary>
    public IReadOnlyList<string> overrides { get; }

    /// <summary>
    /// Gets a detached read-only contribution snapshot, including independently owned payload bytes.
    /// </summary>
    public IReadOnlyList<ProjectSettingRecord> settings => Array.AsReadOnly(m_settings.Select(Clone).ToArray());

    private static ProjectSettingRecord Clone(ProjectSettingRecord record)
        => new()
    {
        id = record.id,
        stableTypeId = record.stableTypeId,
        propertyData = record.propertyData?.ToArray()!
    };
}
