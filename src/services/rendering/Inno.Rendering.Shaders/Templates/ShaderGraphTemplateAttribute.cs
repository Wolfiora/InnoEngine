using System;

namespace Inno.Rendering.Shaders;

/// <summary>
/// Declares immutable creation metadata for a Shader graph template.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class ShaderGraphTemplateAttribute : Attribute
{
    /// <summary>
    /// Creates Shader graph template discovery metadata.
    /// </summary>
    /// <param name="id">
    /// Stable template identity used by creation commands.
    /// </param>
    /// <param name="displayName">
    /// User-facing creation menu label.
    /// </param>
    public ShaderGraphTemplateAttribute(
        string id,
        string displayName
    ) {
        this.id = string.IsNullOrWhiteSpace(id)
                  ? throw new ArgumentException("Shader graph template identity cannot be empty.", nameof(id))
                  : id;
        this.displayName = string.IsNullOrWhiteSpace(displayName)
                           ? throw new ArgumentException("Shader graph template display name cannot be empty.", nameof(displayName))
                           : displayName;
    }

    /// <summary>
    /// Gets the stable template identity used by creation commands.
    /// </summary>
    public string id { get; }

    /// <summary>
    /// Gets the user-facing creation menu label.
    /// </summary>
    public string displayName { get; }
}
