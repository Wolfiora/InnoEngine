using System;
using System.Collections.Generic;

namespace Inno.Core.Serialization;

/// <summary>
/// Supplies typed collection construction and map enumeration without runtime generic compilation.
/// </summary>
public sealed class SerializationCollectionMetadata
{
    /// <summary>
    /// Defines the shape and typed operations of a sequence or map.
    /// </summary>
    /// <param name="elementType">
    /// The sequence element or map value type.
    /// </param>
    /// <param name="keyType">
    /// The map key type, or null for a sequence.
    /// </param>
    /// <param name="buildSequence">
    /// A sequence factory, or null for a map.
    /// </param>
    /// <param name="buildMap">
    /// A map factory, or null for a sequence.
    /// </param>
    /// <param name="enumerateMap">
    /// Typed map enumeration, or null for a sequence.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The operations do not describe exactly one complete collection shape.
    /// </exception>
    public SerializationCollectionMetadata(
        Type elementType,
        Type? keyType,
        Func<IReadOnlyList<object?>, object>? buildSequence,
        Func<IReadOnlyList<KeyValuePair<object?, object?>>, object>? buildMap,
        Func<object, IReadOnlyList<KeyValuePair<object?, object?>>>? enumerateMap
    ) {
        ArgumentNullException.ThrowIfNull(elementType);
        if (keyType is null ? buildSequence is null || buildMap is not null || enumerateMap is not null
            : buildSequence is not null || buildMap is null || enumerateMap is null)
            throw new ArgumentException("Collection metadata requires one complete sequence or map shape.");
        this.elementType = elementType;
        this.keyType = keyType;
        this.buildSequence = buildSequence;
        this.buildMap = buildMap;
        this.enumerateMap = enumerateMap;
    }

    /// <summary>
    /// Gets the sequence element or map value type.
    /// </summary>
    public Type elementType { get; }

    /// <summary>
    /// Gets the map key type, or null for a sequence.
    /// </summary>
    public Type? keyType { get; }

    /// <summary>
    /// Gets the typed sequence constructor, or null for a map.
    /// </summary>
    public Func<IReadOnlyList<object?>, object>? buildSequence { get; }

    /// <summary>
    /// Gets the typed map constructor, or null for a sequence.
    /// </summary>
    public Func<IReadOnlyList<KeyValuePair<object?, object?>>, object>? buildMap { get; }

    /// <summary>
    /// Gets typed map enumeration, or null for a sequence.
    /// </summary>
    public Func<object, IReadOnlyList<KeyValuePair<object?, object?>>>? enumerateMap { get; }
}
