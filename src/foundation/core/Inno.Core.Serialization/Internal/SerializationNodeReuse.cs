using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Inno.Core.Serialization;

internal static class SerializationNodeReuse
{
    internal static bool SameScalar<T>(
        T value,
        object previous
    ) {
        if (typeof(T) == typeof(float))
            return BitConverter.SingleToInt32Bits(Unsafe.As<T, float>(ref value)) == BitConverter.SingleToInt32Bits((float)previous);
        if (typeof(T) == typeof(double))
            return BitConverter.DoubleToInt64Bits(Unsafe.As<T, double>(ref value)) == BitConverter.DoubleToInt64Bits((double)previous);
        if (typeof(T) == typeof(decimal))
            return SameScalar((object)Unsafe.As<T, decimal>(ref value), previous);
        return EqualityComparer<T>.Default.Equals(value, (T)previous);
    }

    internal static ObjectSerializationNode CopyObject(ObjectSerializationNode previous)
    {
        var result = new ObjectSerializationNode();
        result.values.EnsureCapacity(previous.values.Count);
        foreach (var pair in previous.values)
            result.values.Add(pair.Key, pair.Value);
        return result;
    }

    internal static SerializationNode Scalar(
        object value,
        SerializationNode? previous
    ) => previous is ScalarSerializationNode scalar && SameScalar(value, scalar.value)
        ? scalar
        : new ScalarSerializationNode(value);

    internal static SerializationNode Binary(
        byte[] value,
        SerializationNode? previous
    ) => previous is BinarySerializationNode binary && value.AsSpan().SequenceEqual(binary.value)
        ? binary
        : new BinarySerializationNode((byte[])value.Clone());

    internal static bool SameScalar(
        object value,
        object previous
    ) {
        if (value.GetType() != previous.GetType())
            return false;
        if (value is float number)
            return BitConverter.SingleToInt32Bits(number) == BitConverter.SingleToInt32Bits((float)previous);
        if (value is double real)
            return BitConverter.DoubleToInt64Bits(real) == BitConverter.DoubleToInt64Bits((double)previous);
        if (value is decimal precision)
        {
            Span<int> left = stackalloc int[4];
            Span<int> right = stackalloc int[4];
            decimal.GetBits(precision, left);
            decimal.GetBits((decimal)previous, right);
            return left.SequenceEqual(right);
        }
        return value.Equals(previous);
    }
}
