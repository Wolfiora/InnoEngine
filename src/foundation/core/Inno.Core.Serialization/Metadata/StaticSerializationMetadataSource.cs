using System;
using System.Collections.Generic;

namespace Inno.Core.Serialization;

/// <summary>
/// Resolves exact serialization shapes from explicit generated contributions without reflection fallback.
/// </summary>
public sealed class StaticSerializationMetadataSource : ISerializationMetadataSource
{
    private readonly Dictionary<Type, SerializationTypeMetadata> m_metadata = [];

    /// <summary>
    /// Freezes generated declaration contributions supplied by the Player composition.
    /// </summary>
    /// <param name="catalogs">
    /// Assembly-local registration methods; delegates are released after construction.
    /// </param>
    /// <exception cref="ArgumentException">
    /// A registration is null or two assemblies disagree about a closed shape.
    /// </exception>
    public StaticSerializationMetadataSource(IReadOnlyList<Action<Action<SerializationTypeMetadata>>> catalogs)
    {
        ArgumentNullException.ThrowIfNull(catalogs);
        foreach (Action<Action<SerializationTypeMetadata>> catalog in catalogs)
        {
            ArgumentNullException.ThrowIfNull(catalog);
            catalog(Register);
        }
    }

    /// <inheritdoc />
    public SerializationTypeMetadata GetMetadata(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return m_metadata.TryGetValue(type, out SerializationTypeMetadata? metadata) ? metadata
            : throw new InvalidOperationException($"Serialization type '{type}' has no generated closed shape.");
    }

    private void Register(SerializationTypeMetadata metadata)
    {
        ArgumentNullException.ThrowIfNull(metadata);
        if (m_metadata.TryAdd(metadata.type, metadata))
            return;
        SerializationTypeMetadata existing = m_metadata[metadata.type];
        if (existing.requiresConverter != metadata.requiresConverter || existing.members.Count != metadata.members.Count
            || existing.collection?.elementType != metadata.collection?.elementType
            || existing.collection?.keyType != metadata.collection?.keyType)
            throw new ArgumentException($"Generated serialization shape '{metadata.type}' is inconsistent.");
        for (int index = 0; index < existing.members.Count; index++)
        {
            SerializationMemberMetadata left = existing.members[index];
            SerializationMemberMetadata right = metadata.members[index];
            if (left.name != right.name || left.type != right.type || left.visibility != right.visibility)
                throw new ArgumentException($"Generated serialization member '{metadata.type}.{left.name}' is inconsistent.");
        }
    }
}
