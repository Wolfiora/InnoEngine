using System;
using System.Buffers;
using System.Collections.Generic;
using Inno.Core.Identity;
using Inno.Core.Serialization.Converters;

namespace Inno.Core.Serialization;

/// <summary>
/// Builds an exact neutral key during one owned synchronous converter capture callback.
/// </summary>
/// <remarks>
/// Only current Core identities, revisions, metadata stamps and neutral values enter the key. Key storage is not
/// serialized or restored. Every failed TryAdd invalidates the whole key. The borrowed builder retires after the callback.
/// </remarks>
public sealed class SerializationCaptureKeyBuilder
{
    private const byte C_OWNER = 1;
    private const byte C_VALUE = 2;
    private const byte C_TEXT = 3;
    private const byte C_GUARD = 4;
    internal const byte C_GUID = 5;

    private readonly int m_threadId = Environment.CurrentManagedThreadId;
    private SerializationCaptureIdentity[] m_identities = [];
    private int m_identityCount;
    private SerializationOperation? m_operation;
    private SerializationCaptureKey? m_previous;
    private List<SerializationCaptureKeyPart>? m_parts;
    private int m_count;
    private int m_capacity;
    private bool m_unavailable;

    internal SerializationCaptureKeyBuilder(
        SerializationOperation operation,
        Type rootType,
        Inno.Core.Identity.Identity source,
        SerializationCaptureKey? previous
    ) : this(operation, previous) {
        if (source.runtimeIdentity is not { } identity)
        {
            m_unavailable = true;
            return;
        }
        Add(new(C_GUARD, identity, 0, operation.GetMetadata(rootType).captureIdentity, null));
        AddIdentity(new(source, identity));
    }

    internal SerializationCaptureKeyBuilder(
        SerializationOperation operation,
        SerializationCaptureKey? previous
    ) {
        m_operation = operation;
        m_previous = previous;
        Add(new(C_GUARD, default, 0, operation.converters.captureIdentity, null));
    }

    /// <summary>
    /// Gets the current immutable owner context while this callback is active.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// The callback has retired or is accessed from another thread.
    /// </exception>
    public SerializationContext context => RequireOperation().context;

    /// <summary>
    /// Reserves capacity for actual frozen input entries without changing a matching key.
    /// </summary>
    /// <param name="capacity">
    /// The expected total entry count, including Core guards.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Capacity is negative.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The callback has retired or is accessed from another thread.
    /// </exception>
    public void EnsureCapacity(int capacity)
    {
        RequireOperation();
        ArgumentOutOfRangeException.ThrowIfNegative(capacity);
        m_capacity = Math.Max(m_capacity, capacity);
        m_parts?.EnsureCapacity(capacity);
        EnsureIdentityCapacity(capacity);
    }

    /// <summary>
    /// Adds an owner revision through its current Core identity; a null revision or detached source invalidates the key.
    /// </summary>
    /// <param name="source">
    /// The current owner, never retained in the completed key.
    /// </param>
    /// <param name="revision">
    /// A complete revision for the producer-owned state, or null when reuse cannot be certified.
    /// </param>
    /// <returns>
    /// True when an identity and revision were available; false invalidates the complete key.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Source is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The callback has retired or is accessed from another thread.
    /// </exception>
    public bool TryAddOwner(
        IdentityObject source,
        ulong? revision
    ) {
        RequireOperation();
        ArgumentNullException.ThrowIfNull(source);
        Inno.Core.Identity.Identity snapshot = source.identity;
        if (!revision.HasValue || snapshot.runtimeIdentity is not { } identity)
            return Decline();
        Add(new(C_OWNER, identity, revision.Value, default, null));
        AddIdentity(new(snapshot, identity));
        return true;
    }

    /// <summary>
    /// Adds a complete property revision only when current metadata proves context-free default capture without converters.
    /// </summary>
    /// <param name="source">
    /// The current serializable identity object.
    /// </param>
    /// <returns>
    /// True for certified property state; false invalidates the complete key and requires normal capture.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Source is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Metadata/revision access fails, or the callback has retired or is accessed from another thread.
    /// </exception>
    public bool TryAddProperties(ISerializable source)
    {
        SerializationOperation operation = RequireOperation();
        ArgumentNullException.ThrowIfNull(source);
        if (source is not IdentityObject identified)
            return Decline();
        Inno.Core.Identity.Identity snapshot = identified.identity;
        if (snapshot.runtimeIdentity is not { } identity)
            return Decline();
        ISerializationMetadataSource provider = operation.context.GetRequired<ISerializationMetadataSource>();
        SerializationTypeMetadata metadata = provider.GetMetadata(source.GetType());
        if (metadata.captureRevision is null)
        {
            foreach (SerializationMemberMetadata member in metadata.members)
                if ((member.visibility & PropertyVisibility.Serialize) != 0)
                    return Decline();
        }
        if (!operation.converters.CanReuseRevision(metadata, provider))
            return Decline();
        ulong? revision = metadata.captureRevision is { } readRevision ? readRevision(source) : 0;
        if (!revision.HasValue)
            return Decline();
        Add(new(C_OWNER, identity, revision.Value, metadata.captureIdentity, null));
        AddIdentity(new(snapshot, identity));
        return true;
    }

    /// <summary>
    /// Adds an exact neutral numeric input to the declared key.
    /// </summary>
    /// <param name="value">
    /// The current complete input value.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// The callback has retired or is accessed from another thread.
    /// </exception>
    public void AddValue(ulong value) => Add(new(C_VALUE, default, value, default, null));

    /// <summary>
    /// Adds an exact neutral identifier without hashing or allocating text.
    /// </summary>
    /// <param name="value">
    /// The current identifier input, including an empty identifier when meaningful.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// The callback has retired or is accessed from another thread.
    /// </exception>
    public void AddValue(Guid value) => Add(new(C_GUID, default, 0, value, null));

    /// <summary>
    /// Adds exact ordinal text, preserving the distinction between null and empty.
    /// </summary>
    /// <param name="value">
    /// The immutable current text input.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// The callback has retired or is accessed from another thread.
    /// </exception>
    public void AddText(string? value) => Add(new(C_TEXT, default, 0, default, value));

    internal SerializationCaptureAttempt Complete()
    {
        RequireOperation();
        if (m_unavailable || m_identityCount == 0)
            return SerializationCaptureAttempt.empty;
        SerializationCaptureKey key = m_parts is null && m_previous is not null && m_previous.parts.Length == m_count
            ? m_previous
            : new(m_parts is not null ? m_parts.ToArray() : m_previous!.parts.AsSpan(0, m_count).ToArray());
        ReadOnlySpan<SerializationCaptureIdentity> identities = m_identities.AsSpan(0, m_identityCount);
        foreach (SerializationCaptureIdentity identity in identities)
            identity.Validate();
        return new(key, ReferenceEquals(key, m_previous) ? [] : identities.ToArray());
    }

    internal void Retire()
    {
        m_operation = null;
        m_previous = null;
        m_parts?.Clear();
        m_parts = null;
        SerializationCaptureIdentity[] identities = m_identities;
        m_identities = [];
        m_identityCount = 0;
        if (identities.Length > 0)
            ArrayPool<SerializationCaptureIdentity>.Shared.Return(identities, clearArray: true);
    }

    private SerializationOperation RequireOperation()
    {
        SerializationOperation operation = m_operation ?? throw new InvalidOperationException("The serialization key callback has retired.");
        if (Environment.CurrentManagedThreadId != m_threadId)
            throw new InvalidOperationException("The serialization key callback belongs to another thread.");
        operation.EnsureActive();
        return operation;
    }

    private bool Decline()
    {
        m_unavailable = true;
        return false;
    }

    private void AddIdentity(SerializationCaptureIdentity identity)
    {
        EnsureIdentityCapacity(checked(m_identityCount + 1));
        m_identities[m_identityCount++] = identity;
    }

    private void EnsureIdentityCapacity(int capacity)
    {
        if (capacity <= m_identities.Length)
            return;
        SerializationCaptureIdentity[] identities = ArrayPool<SerializationCaptureIdentity>.Shared.Rent(capacity);
        m_identities.AsSpan(0, m_identityCount).CopyTo(identities);
        SerializationCaptureIdentity[] previous = m_identities;
        m_identities = identities;
        if (previous.Length > 0)
            ArrayPool<SerializationCaptureIdentity>.Shared.Return(previous, clearArray: true);
    }

    private void Add(SerializationCaptureKeyPart part)
    {
        RequireOperation();
        if (m_parts is null && m_previous is not null && m_count < m_previous.parts.Length && m_previous.parts[m_count] == part)
        {
            m_count++;
            return;
        }
        if (m_parts is null)
        {
            m_parts = new(Math.Max(m_capacity, m_count + 1));
            if (m_previous is not null)
                for (int index = 0; index < m_count; index++)
                    m_parts.Add(m_previous.parts[index]);
        }
        m_parts.Add(part);
        m_count++;
    }
}

internal sealed record SerializationCaptureKey(SerializationCaptureKeyPart[] parts)
{
    internal bool HasSameInputs(SerializationCaptureKey other)
    {
        if (parts.Length != other.parts.Length)
            return false;
        for (int index = 0; index < parts.Length; index++)
        {
            SerializationCaptureKeyPart before = parts[index];
            SerializationCaptureKeyPart after = other.parts[index];
            if (before.kind != after.kind || before.identity != after.identity ||
                before.revision != after.revision || !string.Equals(before.text, after.text, StringComparison.Ordinal) ||
                before.kind == SerializationCaptureKeyBuilder.C_GUID && before.metadata != after.metadata)
                return false;
        }
        return true;
    }
}

internal sealed record SerializationCaptureAttempt(
    SerializationCaptureKey? key,
    SerializationCaptureIdentity[] identities
) {
    internal static readonly SerializationCaptureAttempt empty = new(null, []);

    internal static SerializationCaptureAttempt Capture<T>(
        SerializationOperation operation,
        Type valueType,
        T value,
        SerializationCaptureKey? previous,
        SerializationConverter<T>? converter = null,
        Func<T, SerializationCaptureKeyBuilder, bool>? captureKey = null
    ) {
        if (!operation.isSnapshotCapture)
            return empty;
        SerializationCaptureKeyBuilder builder;
        if (captureKey is not null)
            builder = new(operation, previous);
        else if (value is IdentityObject identified && identified.identity.runtimeIdentity is not null)
            builder = new(operation, valueType, identified.identity, previous);
        else
            return empty;
        try
        {
            bool complete = captureKey is not null ? captureKey(value, builder) : converter!.CaptureKey(value, builder);
            return complete ? builder.Complete() : empty;
        }
        finally
        {
            builder.Retire();
        }
    }

    internal SerializationCaptureKey? Verify(SerializationCaptureAttempt after)
    {
        if (key is null || after.key is null || !key.HasSameInputs(after.key))
            throw new InvalidOperationException("The declared serialization capture inputs changed during the write.");
        // Fresh equivalent metadata preserves ordinary capture without publishing a reusable hint.
        return ReferenceEquals(after.key, key) ? key : null;
    }

    internal void ValidateIdentities()
    {
        foreach (SerializationCaptureIdentity identity in identities)
            identity.Validate();
    }
}

internal readonly record struct SerializationCaptureIdentity(
    Inno.Core.Identity.Identity identity,
    RuntimeIdentity expected
) {
    internal void Validate()
    {
        if (identity.runtimeIdentity != expected)
            throw new InvalidOperationException($"Core identity '{expected}' changed during serialization capture.");
    }
}

internal readonly record struct SerializationCaptureKeyPart(
    byte kind,
    RuntimeIdentity identity,
    ulong revision,
    Guid metadata,
    string? text
);
