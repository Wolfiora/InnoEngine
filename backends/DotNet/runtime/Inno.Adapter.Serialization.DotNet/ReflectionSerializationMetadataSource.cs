using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using Inno.Core.Serialization;

namespace Inno.Adapter.Serialization.DotNet;

/// <summary>
/// Supplies desktop authoring member access through weakly cached runtime reflection metadata.
/// </summary>
public sealed class ReflectionSerializationMetadataSource : ISerializationMetadataSource
{
    private readonly ConditionalWeakTable<Type, SerializationTypeMetadata> m_metadata = new();

    /// <inheritdoc />
    public SerializationTypeMetadata GetMetadata(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return m_metadata.GetValue(type, BuildMetadata);
    }

    private static SerializationTypeMetadata BuildMetadata(Type type)
    {
        SerializationCollectionMetadata? collection = null;
        if (ReflectionCollectionMetadata.TryGetMapTypes(type, out Type keyType, out Type valueType))
        {
            collection = new SerializationCollectionMetadata(valueType, keyType, null,
                entries => ReflectionCollectionMetadata.BuildMap(type, keyType, valueType, entries),
                value => ReflectionCollectionMetadata.TryEnumerateMap(value, type, out List<KeyValuePair<object?, object?>> entries)
                    ? entries : throw new InvalidOperationException($"Map '{type}' cannot be enumerated."));
        }
        else if (ReflectionCollectionMetadata.TryGetSequenceElementType(type, out Type elementType))
        {
            collection = new SerializationCollectionMetadata(elementType, null,
                values => ReflectionCollectionMetadata.BuildSequence(type, elementType, values), null, null);
        }
        IReadOnlyList<SerializationMemberMetadata> members = typeof(ISerializable).IsAssignableFrom(type)
            ? ReflectionObjectMetadata.GetSerializableMembers(type)
            : type.IsValueType && !type.IsPrimitive && !type.IsEnum && type != typeof(Guid)
                && type != typeof(decimal) ? ReflectionStructMetadata.GetMembers(type) : [];
        Func<object>? factory = !type.IsAbstract && !type.IsInterface && !type.ContainsGenericParameters
            && (type.IsValueType || type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                binder: null, Type.EmptyTypes, modifiers: null) is not null) ? () => CreateInstance(type) : null;
        return new SerializationTypeMetadata(type, members, factory,
            typeof(ISerializable).IsAssignableFrom(type) ? ReflectionObjectMetadata.CreateRestoreHandler(type) : null,
            collection, type.IsDefined(typeof(RequiresSerializationConverterAttribute), inherit: true));
    }

    private static object CreateInstance(Type type)
    {
        try
        {
            return Activator.CreateInstance(type, nonPublic: true)
                ?? throw new InvalidOperationException($"Serialization type '{type}' did not produce an instance.");
        }
        catch (TargetInvocationException exception) when (exception.InnerException is not null)
        {
            ExceptionDispatchInfo.Capture(exception.InnerException).Throw();
            throw;
        }
    }
}
