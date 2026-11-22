using System;
using System.Collections.Generic;

namespace Inno.Core.Settings;

/// <summary>
/// Exposes ownership and dependency information while a protocol-owned composer combines one contribution.
/// </summary>
public sealed class ProjectSettingContributionContext
{
    private readonly IReadOnlySet<string> m_dependencies;
    private readonly IReadOnlySet<string> m_overrides;

    internal ProjectSettingContributionContext(
        string contributorID,
        ProjectSettingContributionSource source,
        IReadOnlySet<string> dependencies,
        IReadOnlySet<string> overrides
    ) {
        this.contributorID = contributorID;
        this.source = source;
        m_dependencies = dependencies;
        m_overrides = overrides;
    }

    /// <summary>
    /// Gets the stable Plugin ID, or <c>project</c> for the project-authored contribution.
    /// </summary>
    public string contributorID { get; }

    /// <summary>
    /// Gets the contribution source.
    /// </summary>
    public ProjectSettingContributionSource source { get; }

    /// <summary>
    /// Gets whether this is the project-authored highest-precedence contribution.
    /// </summary>
    public bool isProject => source == ProjectSettingContributionSource.Project;

    /// <summary>
    /// Gets whether this contribution may explicitly replace data owned by another contributor.
    /// </summary>
    /// <param name="ownerID">
    /// The stable owner whose data would be replaced.
    /// </param>
    /// <returns>
    /// <see langword="true"/> for project data, or when the Plugin both depends on and explicitly overrides
    /// <paramref name="ownerID"/>.
    /// </returns>
    public bool CanOverride(string ownerID)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(ownerID);
        return isProject
               || string.Equals(ownerID, "host", StringComparison.Ordinal)
               || (m_dependencies.Contains(ownerID) && m_overrides.Contains(ownerID));
    }
}
