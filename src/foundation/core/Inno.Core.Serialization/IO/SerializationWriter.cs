using System;
using System.Collections.Generic;
using Inno.Extensibility.Types;
using Inno.Scripting.Api;

namespace Inno.Core.Serialization;

/// <summary>
/// Writes a structured object during the active serialization operation.
/// </summary>
public sealed class SerializationWriter
{
    private readonly SerializationOperation m_operation;
    private readonly ObjectSerializationNode? m_previous;
    private readonly int m_operationGeneration;
    private ObjectSerializationNode? m_node;
    private HashSet<string>? m_writtenNames;
    private ulong m_writtenMask;
    private int m_writtenCount;
    private bool m_completed;

    internal SerializationWriter(
        SerializationOperation operation,
        ObjectSerializationNode? previous,
        string path,
        Type valueType
    ) {
        m_operation = operation;
        m_operationGeneration = operation.writerGeneration;
        m_previous = previous;
        if (previous is null)
            m_node = new ObjectSerializationNode();
        this.path = path;
        this.valueType = valueType;
    }

    /// <summary>
    /// Gets the immutable context supplied to the current operation.
    /// </summary>
    public SerializationContext context => m_operation.context;

    /// <summary>
    /// Borrows the immutable type snapshot pinned by this operation's converter generation.
    /// </summary>
    /// <remarks>
    /// Use this snapshot for consistent type identities during a complete capture. It must not be retained beyond
    /// the writer's operation; keeping it would retain its collectible types after the converter lease is released.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// The writer no longer belongs to an active capture.
    /// </exception>
    [ScriptingApiIgnore]
    public TypeCacheSnapshot typeSnapshot
    {
        get
        {
            EnsureActive();
            return m_operation.types;
        }
    }

    /// <summary>
    /// Gets the current diagnostic path.
    /// </summary>
    public string path { get; }

    /// <summary>
    /// Gets the declared value type represented by this writer.
    /// </summary>
    public Type valueType { get; }

    /// <summary>
    /// Writes a named value through the unified value pipeline.
    /// </summary>
    /// <typeparam name="TValue">
    /// The declared value type.
    /// </typeparam>
    /// <param name="name">
    /// The unique non-empty member name.
    /// </param>
    /// <param name="value">
    /// The value to write.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="name"/> is empty.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the name is duplicated or the value is unsupported.
    /// </exception>
    public void Write<TValue>(
        string name,
        TValue value
    ) {
        ValidateName(name);
        EnsureActive();
        if (m_previous is not null && m_previous.values.Count <= 64 && m_writtenCount < m_previous.values.Count)
        {
            KeyValuePair<string, SerializationNode> entry = m_previous.values.GetAt(m_writtenCount);
            if (string.Equals(entry.Key, name, StringComparison.Ordinal) && entry.Value is ScalarSerializationNode scalar &&
                scalar.value.GetType() == typeof(TValue) && m_operation.ResolveConverter(typeof(TValue)) is null &&
                SerializationNodeReuse.SameScalar(value, scalar.value))
            {
                MarkWritten(name, m_writtenCount);
                return;
            }
        }
        Write(name, value, typeof(TValue));
    }

    /// <summary>
    /// Writes a value using its declared metadata type without constructing a generic method at runtime.
    /// </summary>
    /// <param name="name">
    /// The unique non-empty member name.
    /// </param>
    /// <param name="value">
    /// The value to encode, including null when permitted by the declared type.
    /// </param>
    /// <param name="declaredType">
    /// The exact declaration whose converter and value semantics apply.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// The declared type is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The member name is empty.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The operation has ended, the name is duplicated, or the value is unsupported.
    /// </exception>
    public void Write(
        string name,
        object? value,
        Type declaredType
    ) {
        ArgumentNullException.ThrowIfNull(declaredType);
        ValidateName(name);
        EnsureActive();
        SerializationNode? previous = Previous(name);
        AddNode(name, ValuePipeline.Write(value, declaredType, m_operation, AppendPath(name),
            allowDefaultObject: false, previous));
    }

    /// <summary>
    /// Writes a named structured object.
    /// </summary>
    /// <param name="name">
    /// The unique non-empty member name.
    /// </param>
    /// <param name="write">
    /// The callback that fills the child object.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="write"/> is null.
    /// </exception>
    public void WriteObject(
        string name,
        Action<SerializationWriter> write
    ) {
        ArgumentNullException.ThrowIfNull(write);
        ValidateName(name);
        EnsureActive();
        SerializationNode? previous = Previous(name);
        var child = new SerializationWriter(m_operation, previous as ObjectSerializationNode, AppendPath(name), typeof(object));
        AddNode(name, previous ?? child.m_node!);
        write(child);
        ReplaceNode(name, child.Complete());
    }

    /// <summary>
    /// Writes an ordered array of structured objects.
    /// </summary>
    /// <typeparam name="TValue">
    /// The source element type.
    /// </typeparam>
    /// <param name="name">
    /// The unique non-empty member name.
    /// </param>
    /// <param name="values">
    /// The elements to write.
    /// </param>
    /// <param name="writeElement">
    /// The callback that fills each element object.
    /// </param>
    /// <param name="captureKey">
    /// An optional pure callback covering every element output and contextual input. It must declare
    /// a current Core identity owner and return true only for a complete proof. Snapshot capture alone
    /// can reuse a matching element; ordinary serialization always executes writeElement.
    /// </param>
    /// <remarks>
    /// The key builder belongs only to its synchronous callback. Uncertified rows and rows that invoke
    /// nested converters execute the ordinary element writer on every capture. Callback failures and
    /// changed declared inputs abort publication without modifying a previous snapshot.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// Values or writeElement is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The member name is null, empty or whitespace.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The writer has retired, a member is duplicated, or declared capture inputs change during writing.
    /// </exception>
    public void WriteObjectArray<TValue>(
        string name,
        IEnumerable<TValue> values,
        Action<SerializationWriter, TValue> writeElement,
        Func<TValue, SerializationCaptureKeyBuilder, bool>? captureKey = null
    ) {
        ArgumentNullException.ThrowIfNull(values);
        ArgumentNullException.ThrowIfNull(writeElement);
        ValidateName(name);

        EnsureActive();
        SerializationNode? previous = Previous(name);
        var array = new SerializationArrayCapture(previous as ArraySerializationNode);
        AddNode(name, previous ?? array.initialNode!);
        int index = 0;
        if (values is TValue[] items)
        {
            for (int itemIndex = 0; itemIndex < items.Length; itemIndex++)
                WriteElement(items[itemIndex]);
        }
        else
            foreach (TValue value in values)
                WriteElement(value);
        ReplaceNode(name, array.Complete());

        void WriteElement(TValue value)
        {
            ObjectSerializationNode? prior = array.Next as ObjectSerializationNode;
            SerializationCaptureAttempt attempt = captureKey is null ? SerializationCaptureAttempt.empty
                : SerializationCaptureAttempt.Capture(m_operation, typeof(TValue), value, prior?.captureKey, captureKey: captureKey);
            SerializationCaptureKey? key = attempt.key;
            if (key is not null && ReferenceEquals(key, prior?.captureKey))
            {
                array.Add(prior!);
                index++;
                return;
            }
            var child = new SerializationWriter(m_operation, prior,
                array.Next?.capturePath ?? $"{AppendPath(name)}[{index}]", typeof(TValue));
            long converterWrites = m_operation.converterWrites;
            writeElement(child, value);
            ObjectSerializationNode result = child.Complete();
            if (key is not null)
            {
                attempt.ValidateIdentities();
                key = attempt.Verify(SerializationCaptureAttempt.Capture(m_operation, typeof(TValue), value, key, captureKey: captureKey));
                if (m_operation.converterWrites != converterWrites)
                    key = null;
            }
            if (ReferenceEquals(result, prior) && prior!.captureKey is null)
                key = null;
            array.Add(SerializationNodeReuse.WithCaptureKey(result, prior, key));
            index++;
        }
    }

    /// <summary>
    /// Writes annotated properties from a serializable object into the current structured object.
    /// </summary>
    /// <param name="value">
    /// The object whose annotated properties should be written.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="value"/> is null.
    /// </exception>
    public void WriteProperties(ISerializable value)
    {
        ArgumentNullException.ThrowIfNull(value);
        EnsureActive();
        m_operation.EnterCapture(value, path);
        try
        {
            ValuePipeline.WriteProperties(value, this, m_operation);
        }
        finally
        {
            m_operation.ExitCapture(value);
        }
    }

    /// <summary>
    /// Writes independently restorable property bytes through the current converter generation.
    /// </summary>
    /// <param name="name">
    /// The unique member receiving the binary property snapshot.
    /// </param>
    /// <param name="value">
    /// The object whose persistent properties are captured once.
    /// </param>
    /// <param name="context">
    /// The complete owner context, or null to use this writer's context.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// The source object is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The writer has ended, a key is duplicated, or capture fails.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The member name is empty.
    /// </exception>
    public void WritePropertiesData(
        string name,
        ISerializable value,
        SerializationContext? context = null
    ) {
        ArgumentNullException.ThrowIfNull(value);
        ValidateName(name);
        EnsureActive();
        AddNode(name, m_operation.CapturePropertyData(value, context ?? this.context, Previous(name) as BinarySerializationNode));
    }

    internal ObjectSerializationNode Complete()
    {
        EnsureActive();
        m_completed = true;
        if (m_previous is not null && (m_node is not null || m_writtenCount != m_previous.values.Count))
        {
            EnsureMutable();
            for (int index = 0; index < m_previous.values.Count; index++)
            {
                string name = m_previous.values.GetAt(index).Key;
                if (m_previous.values.Count <= 64 ? (m_writtenMask & (1UL << index)) == 0 : m_writtenNames?.Contains(name) != true)
                    m_node!.values.Remove(name);
            }
        }
        ObjectSerializationNode result = m_node ?? m_previous ?? new ObjectSerializationNode();
        result.RememberPath(path);
        return result;
    }

    private SerializationNode? Previous(string name)
    {
        if (m_previous is not null && m_writtenCount < m_previous.values.Count &&
            string.Equals(m_previous.values.GetAt(m_writtenCount).Key, name, StringComparison.Ordinal))
            return m_previous.values.GetAt(m_writtenCount).Value;
        return m_previous is not null && m_previous.values.TryGetValue(name, out SerializationNode? value) ? value : null;
    }

    private void AddNode(
        string name,
        SerializationNode node
    ) {
        EnsureActive();
        if (m_previous is null)
        {
            m_node ??= new ObjectSerializationNode();
            if (!m_node.values.TryAdd(name, node))
                throw new InvalidOperationException($"Serialization object '{path}' already contains key '{name}'.");
            return;
        }
        int index = -1;
        if (m_previous.values.Count <= 64)
        {
            if (m_writtenCount < m_previous.values.Count &&
                string.Equals(m_previous.values.GetAt(m_writtenCount).Key, name, StringComparison.Ordinal))
                index = m_writtenCount;
            else
                index = m_previous.values.IndexOf(name);
        }
        SerializationNode? previous = index >= 0 ? m_previous.values.GetAt(index).Value : Previous(name);
        MarkWritten(name, index);
        if (!ReferenceEquals(previous, node))
        {
            EnsureMutable();
            m_node!.values[name] = node;
        }
    }

    private void MarkWritten(
        string name,
        int previousIndex
    ) {
        bool duplicate;
        if (previousIndex >= 0)
        {
            ulong bit = 1UL << previousIndex;
            duplicate = (m_writtenMask & bit) != 0;
            m_writtenMask |= bit;
        }
        else
            duplicate = !(m_writtenNames ??= new(StringComparer.Ordinal)).Add(name);
        if (duplicate)
            throw new InvalidOperationException($"Serialization object '{path}' already contains key '{name}'.");
        m_writtenCount++;
    }

    private void EnsureMutable()
    {
        if (m_node is not null)
            return;
        m_node = m_previous is null ? new ObjectSerializationNode() : SerializationNodeReuse.CopyObject(m_previous);
    }

    private void ReplaceNode(
        string name,
        SerializationNode value
    ) {
        EnsureActive();
        if (!ReferenceEquals(Previous(name), value))
        {
            EnsureMutable();
            m_node!.values[name] = value;
        }
    }

    private void EnsureActive()
    {
        m_operation.EnsureActive();
        if (m_operationGeneration != m_operation.writerGeneration)
            throw new InvalidOperationException("The serialization writer belongs to a completed property capture.");
        if (m_completed && m_operation.isSnapshotCapture)
            throw new InvalidOperationException("The serialization writer callback has already completed.");
    }

    private string AppendPath(string name) => Previous(name)?.capturePath ?? (path == "$" ? $"$.{name}" : $"{path}.{name}");

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("A non-empty serialization member name is required.", nameof(name));
    }
}
