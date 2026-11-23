using System;
using Inno.Core.Serialization.Converters;

namespace Inno.Core.Serialization;

internal abstract class ConverterInvoker
{
    /// <summary>
    /// Creates a validated converter invoker instance.
    /// </summary>
    /// <param name="targetType">
    /// The target type consumed by converter invoker; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="converterType">
    /// The converter type consumed by converter invoker; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    protected ConverterInvoker(
        Type targetType,
        Type converterType
    ) {
        this.targetType = targetType;
        this.converterType = converterType;
    }

    internal Type targetType { get; }

    internal Type converterType { get; }

    internal abstract SerializationNode Write(
        SerializationOperation operation,
        string path,
        Type valueType,
        object value,
        SerializationNode? previous
    );

    internal abstract object Read(
        SerializationOperation operation,
        string path,
        Type valueType,
        ObjectSerializationNode node
    );

    internal abstract void Restore(
        SerializationOperation operation,
        string path,
        Type valueType,
        ObjectSerializationNode node,
        object target
    );
}

internal sealed class ConverterInvoker<T> : ConverterInvoker
{
    private readonly SerializationConverter<T> m_converter;

    internal ConverterInvoker(
        SerializationConverter<T> converter,
        Type converterType
    ) : base(typeof(T), converterType)
    {
        m_converter = converter;
    }

    internal override SerializationNode Write(
        SerializationOperation operation,
        string path,
        Type valueType,
        object value,
        SerializationNode? previous
    ) {
        if (value is not T typed)
        {
            throw new InvalidOperationException(
                $"Converter '{converterType.FullName}' cannot write runtime value '{value.GetType().FullName}' at '{path}'."
            );
        }

        ObjectSerializationNode? prior = previous as ObjectSerializationNode;
        operation.RecordConverterWrite();
        long converterWrites = operation.converterWrites;
        SerializationCaptureAttempt attempt = SerializationCaptureAttempt.Capture(
            operation,
            valueType,
            typed,
            prior?.captureKey,
            m_converter
        );
        SerializationCaptureKey? key = attempt.key;
        if (key is not null && ReferenceEquals(key, prior?.captureKey))
        {
            return prior!;
        }
        var writer = new SerializationWriter(operation, prior, path, valueType);
        m_converter.Write(writer, typed);
        ObjectSerializationNode result = writer.Complete();
        if (key is not null)
        {
            attempt.ValidateIdentities();
            key = attempt.Verify(SerializationCaptureAttempt.Capture(operation, valueType, typed, key, m_converter));
            if (operation.converterWrites != converterWrites)
            {
                key = null;
            }
        }
        return SerializationNodeReuse.WithCaptureKey(result, prior, key);
    }

    internal override object Read(
        SerializationOperation operation,
        string path,
        Type valueType,
        ObjectSerializationNode node
    ) {
        T result = m_converter.Read(new SerializationReader(operation, node, path, valueType));
        if (result is null)
        {
            throw new InvalidOperationException($"Converter '{converterType.FullName}' returned null at '{path}'.");
        }
        if (!valueType.IsInstanceOfType(result))
        {
            throw new InvalidOperationException(
                $"Converter '{converterType.FullName}' returned '{result.GetType().FullName}', which is incompatible with '{valueType.FullName}' at '{path}'."
            );
        }
        if (result is ISerializable serializable)
        {
            operation.ScheduleRestoredObject(serializable);
        }
        return result;
    }

    internal override void Restore(
        SerializationOperation operation,
        string path,
        Type valueType,
        ObjectSerializationNode node,
        object target
    ) {
        if (target is not T typed)
        {
            throw new InvalidOperationException(
                $"Converter '{converterType.FullName}' cannot restore target '{target.GetType().FullName}' at '{path}'."
            );
        }

        m_converter.Restore(new SerializationReader(operation, node, path, valueType), typed);
        if (target is ISerializable serializable)
        {
            operation.ScheduleRestoredObject(serializable);
        }
    }
}
