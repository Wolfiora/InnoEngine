using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Core.Serialization;

/// <summary>
/// Freezes member access, construction and restoration callbacks for an exact closed declaration.
/// </summary>
public sealed class SerializationTypeMetadata
{
    private readonly Func<object>? m_factory;

    /// <summary>
    /// Copies complete declaration metadata before it participates in a serialization generation.
    /// </summary>
    /// <param name="type">
    /// The exact closed declaration.
    /// </param>
    /// <param name="members">
    /// Ordered persistent and runtime members, without duplicate keys.
    /// </param>
    /// <param name="factory">
    /// A parameterless constructor, or null when an explicit converter owns construction.
    /// </param>
    /// <param name="restored">
    /// An ordered restoration callback, or null when the type declares no hooks.
    /// </param>
    /// <param name="collection">
    /// Optional sequence or map construction and enumeration metadata.
    /// </param>
    /// <param name="requiresConverter">
    /// Whether default object serialization must be rejected.
    /// </param>
    /// <param name="inherited">
    /// Optional metadata contributed by the owner of a base declaration.
    /// </param>
    /// <exception cref="ArgumentException">
    /// A member is null or a persistent key is duplicated.
    /// </exception>
    public SerializationTypeMetadata(
        Type type,
        IReadOnlyList<SerializationMemberMetadata> members,
        Func<object>? factory,
        Action<object, SerializationContext>? restored = null,
        SerializationCollectionMetadata? collection = null,
        bool requiresConverter = false,
        SerializationTypeMetadata? inherited = null
    ) {
        ArgumentNullException.ThrowIfNull(type);
        ArgumentNullException.ThrowIfNull(members);
        SerializationMemberMetadata[] completeMembers = inherited is null ? members.ToArray()
            : inherited.members.Concat(members).ToArray();
        var names = new HashSet<string>(StringComparer.Ordinal);
        foreach (SerializationMemberMetadata member in completeMembers)
            if (member is null || !names.Add(member.name))
                throw new ArgumentException("Serialization metadata requires nonnull members with unique keys.", nameof(members));
        this.type = type;
        this.members = Array.AsReadOnly(completeMembers);
        this.restored = inherited?.restored is not Action<object, SerializationContext> baseRestored ? restored
            : restored is null ? baseRestored : (
                value,
                context
            ) => {
                baseRestored(value, context);
                restored(value, context);
            };
        this.collection = collection;
        this.requiresConverter = requiresConverter || inherited?.requiresConverter == true;
        m_factory = factory;
    }

    /// <summary>
    /// Gets the exact closed declaration.
    /// </summary>
    public Type type { get; }

    /// <summary>
    /// Gets ordered immutable member declarations.
    /// </summary>
    public IReadOnlyList<SerializationMemberMetadata> members { get; }

    /// <summary>
    /// Gets the generation-owned restoration callback, or null when no hooks are declared.
    /// </summary>
    public Action<object, SerializationContext>? restored { get; }

    /// <summary>
    /// Gets collection access metadata, or null for a scalar or object declaration.
    /// </summary>
    public SerializationCollectionMetadata? collection { get; }

    /// <summary>
    /// Gets whether default object serialization requires an explicit converter.
    /// </summary>
    public bool requiresConverter { get; }

    /// <summary>
    /// Creates a new instance through the declaration's selected constructor.
    /// </summary>
    /// <returns>
    /// A new caller-owned instance; a missing factory or failed constructor throws.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// No parameterless constructor is supplied.
    /// </exception>
    public object CreateInstance()
    {
        if (m_factory is null)
            throw new InvalidOperationException($"Serialization type '{type}' requires a parameterless constructor or an explicit converter.");
        try
        {
            return m_factory();
        }
        catch (Exception exception) when (exception is not OutOfMemoryException and not StackOverflowException)
        {
            throw new InvalidOperationException($"The parameterless constructor for serialization type '{type}' failed.", exception);
        }
    }
}
