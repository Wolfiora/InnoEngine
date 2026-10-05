using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Core.Serialization;

namespace Inno.Scene;

internal static class SerializedPropertyValueCodec
{
    internal static IReadOnlyList<SerializationMemberMetadata> GetMembers(
        Type componentType,
        SerializationContext context
    ) {
        ArgumentNullException.ThrowIfNull(componentType);
        ArgumentNullException.ThrowIfNull(context);
        const PropertyVisibility persistent = PropertyVisibility.Serialize | PropertyVisibility.Deserialize;
        return context.GetRequired<SerializationRegistry>().GetMetadata(componentType).members
            .Where(static member => (member.visibility & persistent) == persistent).ToArray();
    }

    internal static byte[] Encode(
        SerializationMemberMetadata member,
        GameComponent component,
        SerializationContext context,
        SceneGraphReferenceMap references
    ) {
        ArgumentNullException.ThrowIfNull(member);
        ArgumentNullException.ThrowIfNull(component);
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(references);
        object? value = member.GetValue(component);
        SerializationRegistry serialization = context.GetRequired<SerializationRegistry>();
        using (references.Enter())
            return serialization.Encode(writer => writer.Write("value", value, member.type), context);
    }

    internal static void Decode(
        SerializationMemberMetadata member,
        GameComponent component,
        ReadOnlySpan<byte> bytes,
        SerializationContext context,
        SceneGraphReferenceMap references
    ) {
        ArgumentNullException.ThrowIfNull(member);
        ArgumentNullException.ThrowIfNull(component);
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(references);
        SerializationRegistry serialization = context.GetRequired<SerializationRegistry>();
        using (references.Enter())
        {
            object? value = serialization.Decode(bytes, reader => reader.Read("value", member.type), context);
            member.SetValue(component, value);
        }
    }
}
