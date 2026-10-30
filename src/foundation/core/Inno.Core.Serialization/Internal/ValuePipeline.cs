using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace Inno.Core.Serialization;

internal static class ValuePipeline
{
    internal static SerializationNode WriteRoot(
        ISerializable value,
        Type declaredType,
        SerializationOperation operation,
        SerializationNode? previous = null
    ) {
        operation.EnterCapture(value, "$");
        try
        {
            ConverterInvoker? converter = operation.ResolveConverter(declaredType);
            if (converter is not null)
                return converter.Write(operation, "$", declaredType, value, previous);
            if (operation.GetMetadata(declaredType).requiresConverter)
                throw new InvalidOperationException(
                    $"Serializable type '{declaredType.FullName}' requires an explicit serialization converter.");

            if (value.GetType() != declaredType)
            {
                throw new InvalidOperationException(
                    $"Default root serialization requires exact type '{declaredType.FullName}', but the runtime value is '{value.GetType().FullName}'. Mark the root type with RequiresSerializationConverter and register a converter for polymorphism.");
            }
            EnsureDefaultSerializableType(declaredType, operation);
            var writer = new SerializationWriter(operation, previous as ObjectSerializationNode, "$", declaredType);
            WriteProperties(value, writer, operation);
            return writer.Complete();
        }
        finally
        {
            operation.ExitCapture(value);
        }
    }

    internal static object ReadRoot(
        SerializationNode node,
        Type declaredType,
        SerializationOperation operation
    ) {
        ConverterInvoker? converter = operation.ResolveConverter(declaredType);
        if (converter is not null)
            return converter.Read(operation, "$", declaredType, RequireObject(node, declaredType, "$"));
        if (operation.GetMetadata(declaredType).requiresConverter)
            throw new InvalidOperationException(
                $"Serializable type '{declaredType.FullName}' requires an explicit serialization converter.");

        EnsureDefaultSerializableType(declaredType, operation);
        ISerializable instance = CreateSerializable(declaredType, operation);
        RestoreProperties(instance, RequireObject(node, declaredType, "$"), operation, "$");
        return instance;
    }

    internal static void RestoreRoot(
        ISerializable target,
        SerializationNode node,
        Type targetType,
        SerializationOperation operation
    ) {
        ConverterInvoker? converter = operation.ResolveConverter(targetType);
        if (converter is not null)
        {
            converter.Restore(operation, "$", targetType, RequireObject(node, targetType, "$"), target);
            return;
        }
        if (operation.GetMetadata(targetType).requiresConverter)
            throw new InvalidOperationException(
                $"Serializable type '{targetType.FullName}' requires an explicit serialization converter.");

        EnsureDefaultSerializableType(targetType, operation);
        RestoreProperties(target, RequireObject(node, targetType, "$"), operation, "$" );
    }

    internal static SerializationNode Write(
        object? value,
        Type declaredType,
        SerializationOperation operation,
        string path,
        bool allowDefaultObject,
        SerializationNode? previous = null
    ) {
        operation.EnsureActive();
        if (previous is ScalarSerializationNode scalar && value is not null &&
            scalar.value.GetType() == declaredType && value.GetType() == declaredType &&
            operation.ResolveConverter(declaredType) is null && SerializationNodeReuse.SameScalar(value, scalar.value))
            return scalar;
        SerializationNode node = WriteValue(value, declaredType, operation, path, allowDefaultObject, previous);
        node.RememberPath(path);
        return node;
    }

    private static SerializationNode WriteValue(
        object? value,
        Type declaredType,
        SerializationOperation operation,
        string path,
        bool allowDefaultObject,
        SerializationNode? previous = null
    ) {
        operation.EnsureActive();
        Type? nullableType = Nullable.GetUnderlyingType(declaredType);
        Type valueType = nullableType ?? declaredType;
        if (value is null)
        {
            if (valueType.IsValueType && nullableType is null)
                throw new InvalidOperationException($"Non-nullable value '{path}' of type '{valueType.FullName}' cannot be null.");
            return NullSerializationNode.instance;
        }

        bool trackReference = !value.GetType().IsValueType && value is not string && value is not byte[];
        if (trackReference)
            operation.EnterCapture(value, path);
        try
        {
            ConverterInvoker? converter = operation.ResolveConverter(valueType);
            if (converter is not null)
                return converter.Write(operation, path, valueType, value, previous);

            if (IsPrimitive(valueType))
                return SerializationNodeReuse.Scalar(NormalizePrimitive(value, valueType, path), previous);
            if (valueType.IsEnum)
                return SerializationNodeReuse.Scalar(Convert.ToInt64(value, CultureInfo.InvariantCulture), previous);
            if (valueType == typeof(byte[]))
                return SerializationNodeReuse.Binary((byte[])value, previous);
            if (valueType.IsArray)
                return WriteArray((Array)value, valueType, operation, path, previous as ArraySerializationNode);
            if (operation.GetMetadata(valueType).collection is { keyType: not null } map)
                return WriteMap(value, valueType, map.keyType, map.elementType, operation, path, previous);
            if (operation.GetMetadata(valueType).collection is { keyType: null } sequence)
                return WriteSequence(value, sequence.elementType, operation, path, previous as ArraySerializationNode);
            if (valueType.IsValueType)
                return WriteStruct(value, valueType, operation, path, previous as ObjectSerializationNode);

            if (allowDefaultObject && value is ISerializable serializable)
            {
                if (value.GetType() != valueType)
                {
                    throw new InvalidOperationException(
                        $"Default serialization at '{path}' requires exact type '{valueType.FullName}', but the runtime value is '{value.GetType().FullName}'. Register a converter for polymorphic values.");
                }
                EnsureDefaultSerializableType(valueType, operation);
                var writer = new SerializationWriter(operation, previous as ObjectSerializationNode, path, valueType);
                WriteProperties(serializable, writer, operation);
                return writer.Complete();
            }

            throw new InvalidOperationException(
                $"Class value '{path}' of type '{valueType.FullName}' requires an explicit SerializationConverter<{valueType.Name}>.");
        }
        finally
        {
            if (trackReference)
                operation.ExitCapture(value);
        }
    }

    internal static object? Read(
        SerializationNode node,
        Type declaredType,
        SerializationOperation operation,
        string path,
        bool allowDefaultObject
    ) {
        operation.EnsureActive();
        Type? nullableType = Nullable.GetUnderlyingType(declaredType);
        Type valueType = nullableType ?? declaredType;
        if (node is NullSerializationNode)
        {
            if (!valueType.IsValueType || nullableType is not null)
                return null;
            throw new InvalidOperationException($"Non-nullable value '{path}' of type '{valueType.FullName}' cannot be null.");
        }

        ConverterInvoker? converter = operation.ResolveConverter(valueType);
        if (converter is not null)
            return converter.Read(operation, path, valueType, RequireObject(node, valueType, path));

        if (IsPrimitive(valueType))
            return ReadPrimitive(node, valueType, path);
        if (valueType.IsEnum)
            return Enum.ToObject(valueType, Convert.ToInt64(ReadScalar(node, path), CultureInfo.InvariantCulture));
        if (valueType == typeof(byte[]))
            return node is BinarySerializationNode binary
                ? (byte[])binary.value.Clone()
                : throw TypeMismatch(path, "binary", node);
        if (valueType.IsArray)
            return ReadArray(node, valueType, operation, path);
        if (operation.GetMetadata(valueType).collection is { keyType: not null } map)
            return ReadMap(node, valueType, map.keyType, map.elementType, operation, path);
        if (operation.GetMetadata(valueType).collection is { keyType: null } sequence)
            return ReadSequence(node, valueType, sequence.elementType, operation, path);
        if (valueType.IsValueType)
            return ReadStruct(node, valueType, operation, path);

        if (allowDefaultObject && typeof(ISerializable).IsAssignableFrom(valueType))
        {
            EnsureDefaultSerializableType(valueType, operation);
            ISerializable instance = CreateSerializable(valueType, operation);
            RestoreProperties(instance, RequireObject(node, valueType, path), operation, path);
            return instance;
        }

        throw new InvalidOperationException(
            $"Class value '{path}' of type '{valueType.FullName}' requires an explicit SerializationConverter<{valueType.Name}>.");
    }

    internal static void WriteProperties(
        ISerializable value,
        SerializationWriter writer,
        SerializationOperation operation
    ) {
        IReadOnlyList<SerializationMemberMetadata> members = operation.GetMetadata(value.GetType()).members;
        for (int i = 0; i < members.Count; i++)
        {
            SerializationMemberMetadata member = members[i];
            if ((member.visibility & PropertyVisibility.Serialize) == 0)
                continue;
            writer.Write(member.name, member.GetValue(value), member.type);
        }
    }

    internal static void RestoreProperties(
        ISerializable target,
        ObjectSerializationNode node,
        SerializationOperation operation,
        string path
    ) {
        IReadOnlyList<SerializationMemberMetadata> members = operation.GetMetadata(target.GetType()).members;
        for (int i = 0; i < members.Count; i++)
        {
            SerializationMemberMetadata member = members[i];
            if ((member.visibility & PropertyVisibility.Deserialize) == 0 ||
                !node.values.TryGetValue(member.name, out SerializationNode? memberNode))
            {
                continue;
            }

            member.SetValue(
                target,
                Read(memberNode, member.type, operation, AppendPath(path, member.name), allowDefaultObject: false));
        }

        operation.ScheduleRestoredObject(target);
    }

    private static SerializationNode WriteArray(
        Array value,
        Type arrayType,
        SerializationOperation operation,
        string path,
        ArraySerializationNode? previous
    ) {
        if (arrayType.GetArrayRank() != 1)
            throw new InvalidOperationException($"Only one-dimensional arrays are supported at '{path}'.");
        Type elementType = arrayType.GetElementType()!;
        var capture = new SerializationArrayCapture(previous);
        for (int i = 0; i < value.Length; i++)
            capture.Add(Write(value.GetValue(i), elementType, operation, capture.Next?.capturePath ?? $"{path}[{i}]", false, capture.Next));
        return capture.Complete();
    }

    private static object ReadArray(
        SerializationNode node,
        Type arrayType,
        SerializationOperation operation,
        string path
    ) {
        if (arrayType.GetArrayRank() != 1)
            throw new InvalidOperationException($"Only one-dimensional arrays are supported at '{path}'.");
        if (node is not ArraySerializationNode array)
            throw TypeMismatch(path, "array", node);
        Type elementType = arrayType.GetElementType()!;
        var values = new object?[array.values.Count];
        for (int i = 0; i < values.Length; i++)
            values[i] = Read(array.values[i], elementType, operation, $"{path}[{i}]", false);
        return operation.GetMetadata(arrayType).collection!.buildSequence!(values);
    }

    private static SerializationNode WriteSequence(
        object value,
        Type elementType,
        SerializationOperation operation,
        string path,
        ArraySerializationNode? previous
    ) {
        if (value is not IEnumerable enumerable)
            throw new InvalidOperationException($"Sequence value '{path}' is not enumerable.");
        var capture = new SerializationArrayCapture(previous);
        int index = 0;
        foreach (object? element in enumerable)
        {
            capture.Add(Write(element, elementType, operation, capture.Next?.capturePath ?? $"{path}[{index}]", false, capture.Next));
            index++;
        }
        return capture.Complete();
    }

    private static object ReadSequence(
        SerializationNode node,
        Type sequenceType,
        Type elementType,
        SerializationOperation operation,
        string path
    ) {
        if (node is not ArraySerializationNode array)
            throw TypeMismatch(path, "array", node);
        var values = new object?[array.values.Count];
        for (int i = 0; i < values.Length; i++)
            values[i] = Read(array.values[i], elementType, operation, $"{path}[{i}]", false);
        return operation.GetMetadata(sequenceType).collection!.buildSequence!(values);
    }

    private static SerializationNode WriteMap(
        object value,
        Type mapType,
        Type keyType,
        Type mapValueType,
        SerializationOperation operation,
        string path,
        SerializationNode? previous
    ) {
        IReadOnlyList<KeyValuePair<object?, object?>> entries = operation.GetMetadata(mapType).collection!.enumerateMap!(value);
        var node = new MapSerializationNode();
        for (int i = 0; i < entries.Count; i++)
        {
            KeyValuePair<object?, object?> entry = entries[i];
            if (entry.Key is null)
                throw new InvalidOperationException($"Map key '{path}[{i}].key' cannot be null.");
            node.values.Add(new KeyValuePair<SerializationNode, SerializationNode>(
                Write(entry.Key, keyType, operation, $"{path}[{i}].key", false),
                Write(entry.Value, mapValueType, operation, $"{path}[{i}].value", false)));
        }
        return previous is MapSerializationNode &&
               BinarySerializationFormat.Encode(node).AsSpan().SequenceEqual(BinarySerializationFormat.Encode(previous))
            ? previous
            : node;
    }

    private static object ReadMap(
        SerializationNode node,
        Type mapType,
        Type keyType,
        Type mapValueType,
        SerializationOperation operation,
        string path
    ) {
        if (node is not MapSerializationNode map)
            throw TypeMismatch(path, "map", node);
        var entries = new KeyValuePair<object?, object?>[map.values.Count];
        for (int i = 0; i < entries.Length; i++)
        {
            entries[i] = new KeyValuePair<object?, object?>(
                Read(map.values[i].Key, keyType, operation, $"{path}[{i}].key", false),
                Read(map.values[i].Value, mapValueType, operation, $"{path}[{i}].value", false));
        }
        return operation.GetMetadata(mapType).collection!.buildMap!(entries);
    }

    private static SerializationNode WriteStruct(
        object value,
        Type structType,
        SerializationOperation operation,
        string path,
        ObjectSerializationNode? previous
    ) {
        IReadOnlyList<SerializationMemberMetadata> members = operation.GetMetadata(structType).members;
        ObjectSerializationNode? node = previous is null ? new ObjectSerializationNode() : null;
        int count = 0;
        for (int i = 0; i < members.Count; i++)
        {
            SerializationMemberMetadata member = members[i];
            if ((member.visibility & PropertyVisibility.Serialize) == 0)
                continue;
            SerializationNode? prior = previous is not null && previous.values.TryGetValue(member.name, out SerializationNode? found)
                ? found
                : null;
            SerializationNode captured = Write(member.GetValue(value), member.type, operation,
                prior?.capturePath ?? AppendPath(path, member.name), false, prior);
            if (!ReferenceEquals(prior, captured))
            {
                node ??= SerializationNodeReuse.CopyObject(previous!);
                node.values[member.name] = captured;
            }
            count++;
        }
        if (previous is not null && (node is not null || count != previous.values.Count))
        {
            node ??= SerializationNodeReuse.CopyObject(previous);
            foreach (string name in previous.values.Keys)
            {
                bool included = false;
                for (int index = 0; index < members.Count; index++)
                    if (members[index].name == name && (members[index].visibility & PropertyVisibility.Serialize) != 0)
                    {
                        included = true;
                        break;
                    }
                if (!included)
                    node.values.Remove(name);
            }
        }
        return node ?? previous!;
    }

    private static object ReadStruct(
        SerializationNode node,
        Type structType,
        SerializationOperation operation,
        string path
    ) {
        ObjectSerializationNode objectNode = RequireObject(node, structType, path);
        object result = operation.GetMetadata(structType).CreateInstance();
        IReadOnlyList<SerializationMemberMetadata> members = operation.GetMetadata(structType).members;
        for (int i = 0; i < members.Count; i++)
        {
            SerializationMemberMetadata member = members[i];
            if ((member.visibility & PropertyVisibility.Deserialize) == 0 ||
                !objectNode.values.TryGetValue(member.name, out SerializationNode? memberNode))
            {
                continue;
            }
            member.SetValue(result, Read(memberNode, member.type, operation, AppendPath(path, member.name), false));
        }
        return result;
    }

    private static ISerializable CreateSerializable(
        Type valueType,
        SerializationOperation operation
    ) => (ISerializable)operation.GetMetadata(valueType).CreateInstance();

    private static void EnsureDefaultSerializableType(
        Type valueType,
        SerializationOperation operation
    ) {
        if (!typeof(ISerializable).IsAssignableFrom(valueType))
            throw new InvalidOperationException($"Type '{valueType.FullName}' does not implement ISerializable.");
        if (valueType.IsAbstract || valueType.IsInterface || operation.GetMetadata(valueType).requiresConverter)
            throw new InvalidOperationException($"Serializable type '{valueType.FullName}' requires an explicit converter.");
    }

    private static ObjectSerializationNode RequireObject(
        SerializationNode node,
        Type valueType,
        string path
    )
        => node as ObjectSerializationNode
           ?? throw new InvalidOperationException(
               $"Serialization value '{path}' for '{valueType.FullName}' must be an object, but was '{GetNodeKind(node)}'.");

    private static object ReadPrimitive(
        SerializationNode node,
        Type valueType,
        string path
    ) {
        object scalar = ReadScalar(node, path);
        if (valueType == typeof(string))
            return scalar is string text ? text : throw TypeMismatch(path, "string", node);
        if (valueType == typeof(Guid))
            return scalar is Guid guid ? guid : throw TypeMismatch(path, "Guid", node);
        if (scalar.GetType() == valueType)
            return scalar;
        try
        {
            return Convert.ChangeType(scalar, valueType, CultureInfo.InvariantCulture)!;
        }
        catch (Exception exception)
        {
            throw new InvalidOperationException(
                $"Serialization scalar '{path}' cannot be converted from '{scalar.GetType().FullName}' to '{valueType.FullName}'.",
                exception);
        }
    }

    private static object ReadScalar(
        SerializationNode node,
        string path
    )
        => node is ScalarSerializationNode scalar
            ? scalar.value
            : throw TypeMismatch(path, "scalar", node);

    private static object NormalizePrimitive(
        object value,
        Type valueType,
        string path
    ) {
        if (value.GetType() == valueType)
            return value;
        try
        {
            return Convert.ChangeType(value, valueType, CultureInfo.InvariantCulture)!;
        }
        catch (Exception exception)
        {
            throw new InvalidOperationException(
                $"Primitive value '{path}' cannot be converted to '{valueType.FullName}'.",
                exception);
        }
    }

    internal static bool IsPrimitive(Type type)
        => type == typeof(bool) ||
           type == typeof(byte) ||
           type == typeof(sbyte) ||
           type == typeof(short) ||
           type == typeof(ushort) ||
           type == typeof(int) ||
           type == typeof(uint) ||
           type == typeof(long) ||
           type == typeof(ulong) ||
           type == typeof(float) ||
           type == typeof(double) ||
           type == typeof(decimal) ||
           type == typeof(string) ||
           type == typeof(Guid);

    private static InvalidOperationException TypeMismatch(
        string path,
        string expected,
        SerializationNode actual
    )
        => new($"Serialization value '{path}' must be {expected}, but was '{GetNodeKind(actual)}'.");

    private static string GetNodeKind(SerializationNode node)
        => node switch
        {
            NullSerializationNode => "null",
            ScalarSerializationNode => "scalar",
            BinarySerializationNode => "binary",
            ObjectSerializationNode => "object",
            ArraySerializationNode => "array",
            MapSerializationNode => "map",
            _ => node.GetType().Name
        };

    private static string AppendPath(
        string path,
        string name
    ) => path == "$" ? $"$.{name}" : $"{path}.{name}";
}
