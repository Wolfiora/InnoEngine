using Inno.Core.Serialization;

namespace Inno.Core.Settings;

/// <summary>
/// Provides one decoded contribution and its immutable composition context.
/// </summary>
/// <typeparam name="TContribution">
/// The protocol-owned neutral contribution type.
/// </typeparam>
public sealed class ProjectSettingContribution<TContribution>
    where TContribution : class, ISerializable
{
    internal ProjectSettingContribution(
        ProjectSettingContributionContext context,
        TContribution value
    ) {
        this.context = context;
        this.value = value;
    }

    /// <summary>
    /// Gets the ownership and dependency context for this contribution.
    /// </summary>
    public ProjectSettingContributionContext context { get; }

    /// <summary>
    /// Gets the decoded protocol-owned contribution value.
    /// </summary>
    public TContribution value { get; }
}

/// <summary>
/// Identifies where one setting contribution originated.
/// </summary>
public enum ProjectSettingContributionSource
{
    /// <summary>
    /// The contribution is a default supplied by an activated Plugin.
    /// </summary>
    Plugin,
    /// <summary>
    /// The contribution is the project-authored delta with highest precedence.
    /// </summary>
    Project
}
