using System;

namespace Inno.Rendering.Shaders;

/// <summary>
/// Describes one immutable named structure field.
/// </summary>
public sealed class ShaderSourceField
{
    /// <summary>
    /// Creates a structure field.
    /// </summary>
    /// <param name="name">
    /// Stable field name.
    /// </param>
    /// <param name="type">
    /// Complete field type.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The field has void type or is an opaque storage binding.
    /// </exception>
    public ShaderSourceField(
        string name,
        ShaderSourceType type
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(type);
        if (type.id == "void" || type.storage is not null)
        {
            throw new ArgumentException("A structure field cannot have void type or contain an opaque storage binding.", nameof(type));
        }
        this.name = name;
        this.type = type;
    }

    /// <summary>
    /// Gets the public field name.
    /// </summary>
    public string name { get; }

    /// <summary>
    /// Gets the complete field type.
    /// </summary>
    public ShaderSourceType type { get; }
}
