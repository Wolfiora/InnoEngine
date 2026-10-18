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
/// Holds frame-local material overrides without modifying a shared asset.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("aa10e23b-3f29-525e-af70-e48b77746037")]
public sealed class MaterialPropertyBlock
{
    private readonly Dictionary<ShaderPropertyId, MaterialValue> m_values = [];

    /// <summary>
    /// Gets the number of active overrides.
    /// </summary>
    public int count => m_values.Count;

    /// <summary>
    /// Creates or replaces one frame-local override.
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
        m_values[id] = value;
    }

    /// <summary>
    /// Tries to read one frame-local override.
    /// </summary>
    /// <param name="id">
    /// Stable shader property identifier.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the override exists.
    /// </returns>
    /// <param name="value">
    /// The concrete value read or transformed by this operation.
    /// </param>
    public bool TryGet(
        ShaderPropertyId id,
        out MaterialValue value
    ) => m_values.TryGetValue(id, out value);

    /// <summary>
    /// Removes all frame-local overrides.
    /// </summary>
    public void Clear() => m_values.Clear();

    internal MaterialPropertyBlock Snapshot()
    {
        var snapshot = new MaterialPropertyBlock();
        foreach ((ShaderPropertyId id, MaterialValue value) in m_values)
            snapshot.m_values.Add(id, value);
        return snapshot;
    }
}

