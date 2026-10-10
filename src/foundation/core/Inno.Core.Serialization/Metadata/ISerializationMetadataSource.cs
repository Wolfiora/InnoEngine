using System;

namespace Inno.Core.Serialization;

/// <summary>
/// Supplies declaration access and construction without choosing a runtime reflection implementation.
/// </summary>
public interface ISerializationMetadataSource
{
    /// <summary>
    /// Resolves a complete serialization shape for the current declaration.
    /// </summary>
    /// <param name="type">
    /// The exact closed declaration, including its generic arguments.
    /// </param>
    /// <returns>
    /// Immutable access metadata; an unsupported declaration throws instead of probing another provider.
    /// </returns>
    SerializationTypeMetadata GetMetadata(Type type);
}
