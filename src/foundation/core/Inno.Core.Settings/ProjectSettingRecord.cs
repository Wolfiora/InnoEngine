using System;

namespace Inno.Core.Settings;

/// <summary>
/// Stores one protocol-owned neutral setting contribution for persistence and composition.
/// </summary>
public struct ProjectSettingRecord
{
    /// <summary>
    /// Creates a setting record.
    /// </summary>
    /// <param name="id">
    /// Stable setting identity.
    /// </param>
    /// <param name="stableTypeID">
    /// Stable settings type identity.
    /// </param>
    /// <param name="propertyData">
    /// Composer-owned contribution bytes, or complete property bytes for replacement settings.
    /// </param>
    public ProjectSettingRecord(
        ProjectSettingID id,
        Guid stableTypeID,
        ReadOnlySpan<byte> propertyData
    ) {
        if (!id.isValid)
        {
            throw new ArgumentException("A project setting ID must be valid.", nameof(id));
        }
        if (stableTypeID == Guid.Empty)
        {
            throw new ArgumentException("A project setting requires a stable type ID.", nameof(stableTypeID));
        }
        this.id = id;
        this.stableTypeId = stableTypeID;
        this.propertyData = propertyData.ToArray();
    }

    /// <summary>
    /// Gets or sets the stable setting identity.
    /// </summary>
    public ProjectSettingID id { get; set; }

    /// <summary>
    /// Gets or sets the stable settings type identity.
    /// </summary>
    public Guid stableTypeId { get; set; }

    /// <summary>
    /// Gets or sets the composer-owned payload or default complete-property payload.
    /// </summary>
    public byte[] propertyData { get; set; }
}
