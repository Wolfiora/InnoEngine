using System;
using Inno.Adapter.Serialization.DotNet;

namespace Inno.Core.Serialization.Tests;

internal sealed class FreshCaptureMetadataSource : ISerializationMetadataSource
{
    private readonly ReflectionSerializationMetadataSource m_source = new();

    public SerializationTypeMetadata GetMetadata(Type type)
    {
        SerializationTypeMetadata original = m_source.GetMetadata(type);
        return new(type, original.members, original.CreateInstance, original.restored, original.collection, original.requiresConverter);
    }
}
