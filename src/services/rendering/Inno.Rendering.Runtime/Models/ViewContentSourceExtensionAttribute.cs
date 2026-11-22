using System;
using Inno.Extensibility.Types;

namespace Inno.Rendering.Runtime;

/// <summary>
/// Discovers an independently reloadable world-content source.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
[StableTypeID("9e766605-2b84-5040-858e-ae84b6a5e7c3")]
public sealed class ViewContentSourceExtensionAttribute : Attribute
{
    /// <summary>
    /// Creates a source declaration with a stable identifier.
    /// </summary>
    /// <param name="id">
    /// Globally stable source identifier.
    /// </param>
    public ViewContentSourceExtensionAttribute(string id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        this.id = id;
    }

    /// <summary>
    /// Gets the globally stable source identifier.
    /// </summary>
    public string id { get; }
}
