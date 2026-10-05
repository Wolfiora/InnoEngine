using System;

namespace Inno.Core.Serialization;

/// <summary>
/// Captures one ordered serialization key and its generation-owned accessors.
/// </summary>
public sealed class SerializationMemberMetadata
{
    private readonly Func<object, object?>? m_getter;
    private readonly Action<object, object?>? m_setter;

    /// <summary>
    /// Defines a member whose access is implemented by the selected metadata provider.
    /// </summary>
    /// <param name="name">
    /// The exact persistent key.
    /// </param>
    /// <param name="type">
    /// The declared value type.
    /// </param>
    /// <param name="visibility">
    /// Persistent and runtime access permissions.
    /// </param>
    /// <param name="getter">
    /// A read accessor, or null when reading is not permitted.
    /// </param>
    /// <param name="setter">
    /// A write accessor, or null when writing is not permitted.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The name is empty or an accessor required by the visibility is absent.
    /// </exception>
    public SerializationMemberMetadata(
        string name,
        Type type,
        PropertyVisibility visibility,
        Func<object, object?>? getter,
        Action<object, object?>? setter
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(type);
        if ((visibility & (PropertyVisibility.Serialize | PropertyVisibility.RuntimeGet)) != 0 && getter is null)
            throw new ArgumentException("Readable metadata requires a getter.", nameof(getter));
        if ((visibility & (PropertyVisibility.Deserialize | PropertyVisibility.RuntimeSet)) != 0 && setter is null)
            throw new ArgumentException("Writable metadata requires a setter.", nameof(setter));
        this.name = name;
        this.type = type;
        this.visibility = visibility;
        m_getter = getter;
        m_setter = setter;
    }

    /// <summary>
    /// Gets the exact persistent key.
    /// </summary>
    public string name { get; }

    /// <summary>
    /// Gets the declared value type.
    /// </summary>
    public Type type { get; }

    /// <summary>
    /// Gets the member's persistent and runtime access permissions.
    /// </summary>
    public PropertyVisibility visibility { get; }

    /// <summary>
    /// Reads the current value through the selected declaration accessor.
    /// </summary>
    /// <param name="target">
    /// The declaring object; ownership remains with the caller.
    /// </param>
    /// <returns>
    /// The current value, including null when allowed by the declaration.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Reading is not permitted by this metadata.
    /// </exception>
    public object? GetValue(object target)
        => m_getter is not null ? m_getter(target)
            : throw new InvalidOperationException($"Serialization member '{name}' does not permit reads.");

    /// <summary>
    /// Writes a value through the selected declaration accessor.
    /// </summary>
    /// <param name="target">
    /// The object receiving the value.
    /// </param>
    /// <param name="value">
    /// A value compatible with the declared member type.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Writing is not permitted by this metadata.
    /// </exception>
    public void SetValue(
        object target,
        object? value
    ) {
        if (m_setter is null)
            throw new InvalidOperationException($"Serialization member '{name}' does not permit writes.");
        m_setter(target, value);
    }
}
