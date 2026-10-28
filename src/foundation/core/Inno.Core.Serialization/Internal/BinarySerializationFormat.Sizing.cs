using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace Inno.Core.Serialization;

internal static partial class BinarySerializationFormat
{
    private static int GetEncodedSize(SerializationNode node)
    {
        int payload = node switch
        {
            NullSerializationNode => 0,
            ScalarSerializationNode scalar => GetScalarSize(scalar.value),
            BinarySerializationNode binary => checked(sizeof(int) + binary.value.Length),
            ObjectSerializationNode value => GetObjectSize(value),
            ArraySerializationNode value => GetArraySize(value),
            MapSerializationNode value => GetMapSize(value),
            _ => throw new InvalidDataException($"Unsupported serialization node '{node.GetType().FullName}'.")
        };
        return checked(sizeof(byte) + payload);
    }

    private static int GetScalarSize(object value) => value switch
    {
        bool or byte or sbyte => 1,
        short or ushort => 2,
        int or uint or float => 4,
        long or ulong or double => 8,
        decimal or Guid => 16,
        string text => GetEncodedStringSize(text),
        _ => throw new InvalidDataException($"Unsupported scalar runtime type '{value.GetType().FullName}'.")
    };

    private static int GetEncodedStringSize(string value)
    {
        int bytes = Encoding.UTF8.GetByteCount(value);
        int prefix = 1 + BitOperations.Log2((uint)bytes) / 7;
        return checked(prefix + bytes);
    }

    private static int GetObjectSize(ObjectSerializationNode node)
    {
        int size = sizeof(int);
        foreach (KeyValuePair<string, SerializationNode> member in node.values)
            size = checked(size + GetEncodedStringSize(member.Key) + GetEncodedSize(member.Value));
        return size;
    }

    private static int GetArraySize(ArraySerializationNode node)
    {
        int size = sizeof(int);
        foreach (SerializationNode value in node.values)
            size = checked(size + GetEncodedSize(value));
        return size;
    }

    private static int GetMapSize(MapSerializationNode node)
    {
        int size = sizeof(int);
        foreach (KeyValuePair<SerializationNode, SerializationNode> entry in node.values)
            size = checked(size + GetEncodedSize(entry.Key) + GetEncodedSize(entry.Value));
        return size;
    }
}
