using System;

namespace Inno.Editor.Core;

/// <summary>
/// Describes one immutable, presentation-ready statistic without retaining provider instances or types.
/// </summary>
public sealed class EditorStatistic
{
    /// <summary>
    /// Creates one statistic contribution.
    /// </summary>
    /// <param name="id">
    /// Globally stable statistic identity used for replacement within a frame.
    /// </param>
    /// <param name="groupID">
    /// Stable visual group identity.
    /// </param>
    /// <param name="groupName">
    /// User-facing group heading.
    /// </param>
    /// <param name="label">
    /// User-facing metric label.
    /// </param>
    /// <param name="value">
    /// Already formatted user-facing value.
    /// </param>
    /// <param name="groupOrder">
    /// Ascending group presentation order.
    /// </param>
    /// <param name="order">
    /// Ascending metric order within the group.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when an identifier, group name, label, or value is empty.
    /// </exception>
    public EditorStatistic(
        EditorStatisticID id,
        EditorStatisticGroupID groupID,
        string groupName,
        string label,
        string value,
        int groupOrder = 0,
        int order = 0
    ) {
        if (!id.isValid)
        {
            throw new ArgumentException("A statistic ID is required.", nameof(id));
        }
        if (!groupID.isValid)
        {
            throw new ArgumentException("A statistic group ID is required.", nameof(groupID));
        }
        ArgumentException.ThrowIfNullOrWhiteSpace(groupName);
        ArgumentException.ThrowIfNullOrWhiteSpace(label);
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        this.id = id;
        this.groupID = groupID;
        this.groupName = groupName.Trim();
        this.label = label.Trim();
        this.value = value.Trim();
        this.groupOrder = groupOrder;
        this.order = order;
    }

    /// <summary>
    /// Gets the globally stable statistic identity.
    /// </summary>
    public EditorStatisticID id { get; }

    /// <summary>
    /// Gets the stable visual group identity.
    /// </summary>
    public EditorStatisticGroupID groupID { get; }

    /// <summary>
    /// Gets the user-facing group heading.
    /// </summary>
    public string groupName { get; }

    /// <summary>
    /// Gets the user-facing metric label.
    /// </summary>
    public string label { get; }

    /// <summary>
    /// Gets the presentation-ready value.
    /// </summary>
    public string value { get; }

    /// <summary>
    /// Gets ascending group presentation order.
    /// </summary>
    public int groupOrder { get; }

    /// <summary>
    /// Gets ascending metric order within the group.
    /// </summary>
    public int order { get; }
}
