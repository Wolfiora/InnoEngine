using System;

namespace Inno.Rendering.Shaders;

/// <summary>
/// Declares the immutable identity used to select a Shader Target from an authored graph.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class ShaderTargetAttribute : Attribute
{
    /// <summary>
    /// Creates target discovery metadata.
    /// </summary>
    /// <param name="id">
    /// Stable target identity persisted by Shader assets.
    /// </param>
    public ShaderTargetAttribute(string id)
    {
        this.id = string.IsNullOrWhiteSpace(id)
                  ? throw new ArgumentException("Shader target identity cannot be empty.", nameof(id))
                  : id;
    }

    /// <summary>
    /// Gets the stable target identity persisted by Shader assets.
    /// </summary>
    public string id { get; }
}
