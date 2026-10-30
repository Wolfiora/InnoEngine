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
        var metadata = new SerializationTypeMetadata(type, members, factory,
            typeof(ISerializable).IsAssignableFrom(type) ? ReflectionObjectMetadata.CreateRestoreHandler(type) : null,
            collection, type.IsDefined(typeof(RequiresSerializationConverterAttribute), inherit: true));
        if (ReflectionStructMetadata.HasPureFieldAccess(type, metadata))
            metadata = metadata.WithPureFieldAccess();
        SerializationRevisionAttribute? revision = type.GetCustomAttribute<SerializationRevisionAttribute>(inherit: false);
        if (revision is null)
            return metadata;
        foreach (SerializationMemberMetadata member in metadata.members)
            if (string.Equals(member.name, revision.memberName, StringComparison.Ordinal) &&
                (member.visibility & PropertyVisibility.Serialize) != 0)
            {
                throw new InvalidOperationException(
                    $"Capture revision '{type.FullName}.{revision.memberName}' must not be serialized.");
            }
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        FieldInfo? field = type.GetField(revision.memberName, flags);
        if (field is not null && !field.IsStatic && (field.FieldType == typeof(ulong) || field.FieldType == typeof(ulong?)))
        {
            Func<object, object?> readField = field.GetValue;
            return metadata.WithCaptureRevision(value => ReadCaptureRevision(value, readField));
        }
        PropertyInfo? property = type.GetProperty(revision.memberName, flags);
        if (property is not null && property.GetIndexParameters().Length == 0 &&
            property.GetGetMethod(true) is { IsStatic: false }
            && (property.PropertyType == typeof(ulong) || property.PropertyType == typeof(ulong?)))
        {
            Func<object, object?> readProperty = property.GetValue;
            return metadata.WithCaptureRevision(value => ReadCaptureRevision(value, readProperty));
        }
        throw new InvalidOperationException(
            $"Capture revision '{type.FullName}.{revision.memberName}' must be a readable ulong or nullable ulong instance member.");
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

    private static ulong? ReadCaptureRevision(
        object value,
        Func<object, object?> reader
    ) {
        try
        {
            return (ulong?)reader(value);
        }
        catch (TargetInvocationException exception) when (exception.InnerException is not null)
        {
            ExceptionDispatchInfo.Capture(exception.InnerException).Throw();
            throw;
        }
    }
}
