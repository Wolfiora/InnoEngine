using System;

namespace Inno.Rendering.Shaders;

/// <summary>
/// Describes one parameter parsed from a public source declaration.
/// </summary>
public sealed class ShaderSourceParameter
{
    /// <summary>
    /// Creates a validated function parameter.
    /// </summary>
    /// <param name="name">
    /// Public parameter name, used for semantic port matching rather than ordinal matching.
    /// </param>
    /// <param name="type">
    /// Canonical value type.
    /// </param>
    /// <param name="direction">
    /// Declared value flow.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The parameter has void type.
    /// </exception>
    public ShaderSourceParameter(
        string name,
        ShaderSourceType type,
        ShaderSourceParameterDirection direction
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(type);
        if (!Enum.IsDefined(direction))
        {
            throw new ArgumentOutOfRangeException(nameof(direction));
        }
        if (type.id == "void")
        {
            throw new ArgumentException("A parameter cannot have void type.", nameof(type));
        }
        this.name = name;
        this.type = type;
        this.direction = direction;
    }

    /// <summary>
    /// Gets the public parameter name.
    /// </summary>
    public string name { get; }

    /// <summary>
    /// Gets the complete canonical value type.
    /// </summary>
    public ShaderSourceType type { get; }

    /// <summary>
    /// Gets the declared value flow.
    /// </summary>
    public ShaderSourceParameterDirection direction { get; }
}

/// <summary>
/// Declares value flow across a source function parameter.
/// </summary>
public enum ShaderSourceParameterDirection
{
    /// <summary>
    /// The caller supplies a value.
    /// </summary>
    Input,
    /// <summary>
    /// The function produces a value.
    /// </summary>
    Output,
    /// <summary>
    /// The caller supplies a value and the function produces its replacement.
    /// </summary>
    InputOutput
}
