using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

using Inno.Core.Serialization;

namespace Inno.Adapter.Serialization.DotNet;

internal static class ReflectionStructMetadata
{
    private const BindingFlags C_MEMBERS =
        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    private static readonly ConditionalWeakTable<Type, MembersBox> S_CACHE = new();

    internal static SerializationMemberMetadata[] GetMembers(Type structType)
    {
        return S_CACHE.GetValue(structType, static type => new MembersBox(BuildMembers(type))).members;
    }

    private static SerializationMemberMetadata[] BuildMembers(Type structType)
    {
        var members = new List<SerializationMemberMetadata>();
        var names = new HashSet<string>(StringComparer.Ordinal);
        MemberInfo[] candidates = structType
            .GetMembers(C_MEMBERS)
            .OrderBy(GetSerializableOrder)
            .ThenBy(static member => member.MetadataToken)
            .ToArray();
        for (int i = 0; i < candidates.Length; i++)
        {
            SerializationMemberMetadata? member = candidates[i] switch
            {
                FieldInfo field => TryCreateField(structType, field),
                PropertyInfo property => TryCreateProperty(structType, property),
                _ => null
            };
            if (member is null)
                continue;
            if (!names.Add(member.name))
            {
                throw new InvalidOperationException(
                    $"Struct '{structType.FullName}' declares duplicate serialized key '{member.name}'.");
            }
            members.Add(member);
        }

        if (members.Count == 0)
        {
            throw new InvalidOperationException(
                $"Struct '{structType.FullName}' has no public writable data members. Register a SerializationConverter<{structType.Name}>.");
        }

        return [.. members];
    }

    private static int GetSerializableOrder(MemberInfo member)
    {
        return member.GetCustomAttribute<SerializablePropertyAttribute>(inherit: true)?.order
            ?? int.MaxValue;
    }

    private static SerializationMemberMetadata? TryCreateField(
        Type structType,
        FieldInfo field
    ) {
        if (field.IsStatic)
            return null;

        SerializablePropertyAttribute? attribute =
            field.GetCustomAttribute<SerializablePropertyAttribute>(inherit: true);
        bool isDefaultMember = field.IsPublic && !field.IsInitOnly;
        if (attribute is null && !isDefaultMember)
            return null;

        PropertyVisibility visibility = attribute?.propertyVisibility ?? PropertyVisibility.Show;
        bool requiresWrite = (visibility & (PropertyVisibility.Deserialize | PropertyVisibility.RuntimeSet)) != 0;
        if (requiresWrite && field.IsInitOnly)
        {
            throw new InvalidOperationException(
                $"Struct field '{structType.FullName}.{field.Name}' must be writable for visibility '{visibility}'.");
        }

        return new SerializationMemberMetadata(
            field.Name,
            field.FieldType,
            visibility,
            field.GetValue,
            field.IsInitOnly ? null : field.SetValue);
    }

    private static SerializationMemberMetadata? TryCreateProperty(
        Type structType,
        PropertyInfo property
    ) {
        if (property.GetIndexParameters().Length != 0)
            return null;

        SerializablePropertyAttribute? attribute =
            property.GetCustomAttribute<SerializablePropertyAttribute>(inherit: true);
        MethodInfo? getter = property.GetGetMethod(nonPublic: true);
        MethodInfo? setter = property.GetSetMethod(nonPublic: true);
        bool isDefaultMember = getter?.IsPublic == true && setter?.IsPublic == true;
        if (attribute is null && !isDefaultMember)
            return null;

        PropertyVisibility visibility = attribute?.propertyVisibility ?? PropertyVisibility.Show;
        bool requiresRead = (visibility & (PropertyVisibility.Serialize | PropertyVisibility.RuntimeGet)) != 0;
        bool requiresWrite = (visibility & (PropertyVisibility.Deserialize | PropertyVisibility.RuntimeSet)) != 0;
        if (requiresRead && getter is null)
        {
            throw new InvalidOperationException(
                $"Struct property '{structType.FullName}.{property.Name}' must define a getter for visibility '{visibility}'.");
        }
        if (requiresWrite && setter is null)
        {
            throw new InvalidOperationException(
                $"Struct property '{structType.FullName}.{property.Name}' must define a setter for visibility '{visibility}'.");
        }

        return new SerializationMemberMetadata(
            property.Name,
            property.PropertyType,
            visibility,
            getter is null ? null : property.GetValue,
            setter is null ? null : property.SetValue);
    }

    private sealed record MembersBox(SerializationMemberMetadata[] members);
}
