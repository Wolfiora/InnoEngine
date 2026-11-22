using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Rendering.Shaders;

/// <summary>
/// Contains the single callable interface exported by a source asset.
/// </summary>
public sealed class ShaderSourceFunction
{
    /// <summary>
    /// Captures a source function declaration without retaining parser or provider objects.
    /// </summary>
    /// <param name="name">
    /// Implementation function name selected by import settings.
    /// </param>
    /// <param name="returnType">
    /// Canonical result type; void has no result port.
    /// </param>
    /// <param name="parameters">
    /// Unique named parameters in call order.
    /// </param>
    /// <param name="location">
    /// Original declaration location.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The name is reserved or parameters have duplicate names.
    /// </exception>
    public ShaderSourceFunction(
        string name,
        ShaderSourceType returnType,
        IEnumerable<ShaderSourceParameter> parameters,
        ShaderSourcePosition location
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(returnType);
        ArgumentNullException.ThrowIfNull(parameters);
        if (name == "main")
        {
            throw new ArgumentException("A source module exports a function, not a shader main entry point.", nameof(name));
        }
        ShaderSourceParameter[] snapshot = parameters.ToArray();
        if (snapshot.Any(static parameter => parameter is null)
            || snapshot.Select(static parameter => parameter.name).Distinct(StringComparer.Ordinal).Count() != snapshot.Length
        ) {
            throw new ArgumentException("A function requires unique parameter names.", nameof(parameters));
        }
        this.name = name;
        this.returnType = returnType;
        this.parameters = Array.AsReadOnly(snapshot);
        this.location = location;
    }

    /// <summary>
    /// Gets the implementation function name.
    /// </summary>
    public string name { get; }

    /// <summary>
    /// Gets the canonical result type.
    /// </summary>
    public ShaderSourceType returnType { get; }

    /// <summary>
    /// Gets the immutable function parameters.
    /// </summary>
    public IReadOnlyList<ShaderSourceParameter> parameters { get; }

    /// <summary>
    /// Gets the original declaration position.
    /// </summary>
    public ShaderSourcePosition location { get; }

    /// <summary>
    /// Validates alternative implementations by semantic names, types, direction, and call order.
    /// </summary>
    /// <param name="other">
    /// Candidate interface, whose private implementation function name may differ.
    /// </param>
    /// <returns>
    /// True when both implementations expose the same graph ports and calling interface.
    /// </returns>
    public bool HasSameInterface(ShaderSourceFunction? other)
    {
        if (other is null || !returnType.IsEquivalentTo(other.returnType) || parameters.Count != other.parameters.Count)
        {
            return false;
        }
        for (int i = 0; i < parameters.Count; i++)
        {
            if (parameters[i].name != other.parameters[i].name || parameters[i].direction != other.parameters[i].direction
                || !parameters[i].type.IsEquivalentTo(other.parameters[i].type)
            ) {
                    return false;
            }
        }
        return true;
    }
}
