using Inno.Assets;
using Inno.Core.Mathematics;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Rendering.Assets;

/// <summary>
/// Represents material state keyed by stable shader property identifiers.
/// </summary>
[StableTypeId("56f1fdc7-dad9-464a-848f-fcae4c33ecf2")]
public class MaterialAsset : AssetObject
{
    [SerializableProperty(PropertyVisibility.Hide)]
    private MaterialPropertyEntry[] m_properties = [];

    [SerializableProperty(PropertyVisibility.Hide)]
    private string[] m_keywords = [];

    [SerializableProperty(PropertyVisibility.Hide)]
    private MaterialMetadataEntry[] m_metadata = [];

    /// <summary>
    /// Gets or sets the referenced shader asset.
    /// </summary>
    [SerializableProperty]
    public ShaderAsset? shader { get; set; }

    /// <summary>
    /// Gets or sets an optional explicitly selected technique.
    /// </summary>
    [SerializableProperty]
    public ShaderTechniqueId techniqueId { get; set; }

    /// <summary>
    /// Gets persistent material values in stable insertion order.
    /// </summary>
    public IReadOnlyList<MaterialPropertyEntry> properties => Array.AsReadOnly(m_properties);

    /// <summary>
    /// Gets enabled stable keyword option identifiers.
    /// </summary>
    public IReadOnlyList<string> keywords => Array.AsReadOnly(m_keywords);

    /// <summary>
    /// Gets open provider-defined metadata.
    /// </summary>
    public IReadOnlyList<MaterialMetadataEntry> metadata => Array.AsReadOnly(m_metadata);

    /// <summary>
    /// Creates or replaces one material property.
    /// </summary>
    /// <param name="id">
    /// Stable shader property identifier.
    /// </param>
    /// <param name="value">
    /// Neutral material value.
    /// </param>
    public void Set(
        ShaderPropertyId id,
        MaterialValue value
    ) {
        if (!id.isValid)
            throw new ArgumentException("A material property ID must be valid.", nameof(id));
        int index = Array.FindIndex(m_properties, candidate => candidate.id == id);
        if (index < 0)
        {
            Array.Resize(ref m_properties, m_properties.Length + 1);
            m_properties[^1] = new MaterialPropertyEntry(id, value);
            return;
        }

        m_properties[index] = new MaterialPropertyEntry(id, value);
    }

    /// <summary>
    /// Atomically replaces all persistent material values with an isolated, deterministically ordered set.
    /// </summary>
    /// <param name="properties">
    /// Complete replacement property set. Duplicate identifiers are rejected.
    /// </param>
    public void ReplaceProperties(IEnumerable<MaterialPropertyEntry> properties)
    {
        ArgumentNullException.ThrowIfNull(properties);
        MaterialPropertyEntry[] candidate = properties.ToArray();
        var ids = new HashSet<ShaderPropertyId>();
        foreach (MaterialPropertyEntry property in candidate)
        {
            if (!property.id.isValid)
                throw new ArgumentException("A material property ID must be valid.", nameof(properties));
            if (!ids.Add(property.id))
            {
                throw new ArgumentException(
                    $"Material property '{property.id}' is duplicated.",
                    nameof(properties));
            }
        }
        m_properties = candidate
            .OrderBy(static property => property.id.value, StringComparer.Ordinal)
            .ToArray();
    }

    /// <summary>
    /// Tries to read one material property.
    /// </summary>
    /// <param name="id">
    /// Stable shader property identifier.
    /// </param>
    /// <param name="value">
    /// Receives the neutral value when present.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the property exists.
    /// </returns>
    public bool TryGet(
        ShaderPropertyId id,
        out MaterialValue value
    ) {
        int index = Array.FindIndex(m_properties, candidate => candidate.id == id);
        value = index < 0 ? default : m_properties[index].value;
        return index >= 0;
    }

    /// <summary>
    /// Enables or disables a declared static keyword option.
    /// </summary>
    /// <param name="keyword">
    /// Stable keyword option identifier.
    /// </param>
    /// <param name="enabled">
    /// Whether the option should be enabled.
    /// </param>
    public void SetKeyword(
        string keyword,
        bool enabled
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(keyword);
        var values = m_keywords.ToHashSet(StringComparer.Ordinal);
        if (enabled)
            values.Add(keyword);
        else
            values.Remove(keyword);
        m_keywords = values.OrderBy(static value => value, StringComparer.Ordinal).ToArray();
    }

    /// <summary>
    /// Creates or replaces one provider-defined metadata value.
    /// </summary>
    /// <param name="key">
    /// Stable metadata key.
    /// </param>
    /// <param name="value">
    /// Provider-defined value.
    /// </param>
    public void SetMetadata(
        string key,
        string value
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        int index = Array.FindIndex(m_metadata, entry => string.Equals(entry.key, key, StringComparison.Ordinal));
        if (index < 0)
        {
            Array.Resize(ref m_metadata, m_metadata.Length + 1);
            m_metadata[^1] = new MaterialMetadataEntry(key, value);
            return;
        }

        m_metadata[index] = new MaterialMetadataEntry(key, value);
    }

    /// <summary>
    /// Tries to read one provider-defined metadata value.
    /// </summary>
    /// <param name="key">
    /// Stable metadata key.
    /// </param>
    /// <param name="value">
    /// Receives the metadata value when present.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the key exists.
    /// </returns>
    public bool TryGetMetadata(
        string key,
        out string? value
    ) {
        int index = Array.FindIndex(m_metadata, entry => string.Equals(entry.key, key, StringComparison.Ordinal));
        value = index < 0 ? null : m_metadata[index].value;
        return index >= 0;
    }

    /// <summary>
    /// Removes one provider-defined metadata value.
    /// </summary>
    /// <param name="key">
    /// Stable metadata key.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when an entry was removed.
    /// </returns>
    public bool RemoveMetadata(string key)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        int index = Array.FindIndex(m_metadata, entry =>
            string.Equals(entry.key, key, StringComparison.Ordinal));
        if (index < 0)
            return false;
        var replacement = new MaterialMetadataEntry[m_metadata.Length - 1];
        if (index > 0)
            Array.Copy(m_metadata, 0, replacement, 0, index);
        if (index < m_metadata.Length - 1)
        {
            Array.Copy(
                m_metadata,
                index + 1,
                replacement,
                index,
                m_metadata.Length - index - 1);
        }
        m_metadata = replacement;
        return true;
    }
}

