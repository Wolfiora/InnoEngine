using System;
using System.Collections.Generic;
using Inno.Extensibility.Types;

namespace Inno.Core.Serialization;

internal sealed class SerializationOperation
{
    private readonly ConverterRegistryLease m_converters;
    private List<Action>? m_completionCallbacks;
    private HashSet<object>? m_scheduledObjects;
    private List<object>? m_scheduledObjectOrder;
    private Dictionary<object, string>? m_capturePaths;
    private bool m_isActive = true;
    private int m_writerGeneration;

    internal SerializationOperation(
        SerializationContext context,
        ConverterRegistryLease converters,
        bool snapshotCapture = false
    ) {
        ArgumentNullException.ThrowIfNull(converters);
        this.context = context;
        m_converters = converters;
        isSnapshotCapture = snapshotCapture;
    }

    internal SerializationContext context { get; }
    internal bool isSnapshotCapture { get; }
    internal int writerGeneration => m_writerGeneration;
    internal TypeCacheSnapshot types => m_converters.types;

    internal void RetireCaptureMember()
    {
        EnsureActive();
        m_writerGeneration = checked(m_writerGeneration + 1);
        m_completionCallbacks?.Clear();
        m_scheduledObjects?.Clear();
        m_scheduledObjectOrder?.Clear();
        m_capturePaths?.Clear();
    }

    internal SerializationTypeMetadata GetMetadata(Type type)
        => context.GetRequired<ISerializationMetadataSource>().GetMetadata(type);

    internal ConverterInvoker? ResolveConverter(Type valueType)
    {
        EnsureActive();
        return m_converters.Resolve(valueType);
    }

    internal void EnsureActive()
    {
        if (!m_isActive)
            throw new InvalidOperationException("The serialization reader or writer is no longer active.");
    }

    internal BinarySerializationNode CapturePropertyData(
        ISerializable value,
        SerializationContext context,
        BinarySerializationNode? previous
    ) {
        EnsureActive();
        return PropertySnapshotPipeline.CaptureData(value, context, m_converters, previous);
    }

    internal void AddCompletionCallback(Action callback)
    {
        ArgumentNullException.ThrowIfNull(callback);
        EnsureActive();
        (m_completionCallbacks ??= []).Add(callback);
    }

    internal void ScheduleRestoredObject(ISerializable value)
    {
        EnsureActive();
        if (!(m_scheduledObjects ??= new(ReferenceEqualityComparer.Instance)).Add(value))
            return;
        (m_scheduledObjectOrder ??= []).Add(value);

        Action<object, SerializationContext>? callback = GetMetadata(value.GetType()).restored;
        if (callback is not null)
            (m_completionCallbacks ??= []).Add(() => callback(value, context));
    }

    internal void EnterCapture(
        object value,
        string path
    ) {
        EnsureActive();
        Dictionary<object, string> captures = m_capturePaths ??= new(ReferenceEqualityComparer.Instance);
        if (captures.TryGetValue(value, out string? existingPath))
        {
            throw new InvalidOperationException(
                $"Serialization cycle detected at '{path}'. The same object is already being written at '{existingPath}'.");
        }

        captures.Add(value, path);
    }

    internal Checkpoint CreateCheckpoint()
    {
        EnsureActive();
        return new Checkpoint(m_completionCallbacks?.Count ?? 0, m_scheduledObjectOrder?.Count ?? 0);
    }

    internal void Rollback(Checkpoint checkpoint)
    {
        EnsureActive();
        if (checkpoint.callbackCount < 0 ||
            checkpoint.callbackCount > (m_completionCallbacks?.Count ?? 0) ||
            checkpoint.scheduledObjectCount < 0 ||
            checkpoint.scheduledObjectCount > (m_scheduledObjectOrder?.Count ?? 0))
        {
            throw new InvalidOperationException("The serialization operation checkpoint is invalid.");
        }

        if (m_completionCallbacks is { } callbacks && callbacks.Count > checkpoint.callbackCount)
            callbacks.RemoveRange(checkpoint.callbackCount, callbacks.Count - checkpoint.callbackCount);
        if (m_scheduledObjectOrder is { } order)
        {
            for (int i = order.Count - 1; i >= checkpoint.scheduledObjectCount; i--)
            {
                m_scheduledObjects!.Remove(order[i]);
                order.RemoveAt(i);
            }
        }
    }

    internal void ExitCapture(object value)
    {
        if (m_isActive)
            m_capturePaths?.Remove(value);
    }

    internal void Complete()
    {
        EnsureActive();
        try
        {
            if (m_completionCallbacks is { } callbacks)
                for (int i = 0; i < callbacks.Count; i++)
                    callbacks[i]();
        }
        finally
        {
            m_isActive = false;
            m_completionCallbacks?.Clear();
            m_scheduledObjects?.Clear();
            m_scheduledObjectOrder?.Clear();
            m_capturePaths?.Clear();
        }
    }

    internal void Fail()
    {
        if (!m_isActive)
            return;

        m_isActive = false;
        m_completionCallbacks?.Clear();
        m_scheduledObjects?.Clear();
        m_scheduledObjectOrder?.Clear();
        m_capturePaths?.Clear();
    }

    internal readonly record struct Checkpoint(
        int callbackCount,
        int scheduledObjectCount
    );

}
