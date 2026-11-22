using System;

namespace Inno.Editor.Rendering;

/// <summary>
/// Marks one reloadable Editor gizmo provider.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class EditorGizmoProviderExtensionAttribute : Attribute
{
    /// <summary>
    /// Creates a provider declaration with a stable identifier.
    /// </summary>
    /// <param name="id">
    /// The globally unique provider identifier.
    /// </param>
    public EditorGizmoProviderExtensionAttribute(string id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        this.id = id;
    }

    /// <summary>
    /// Gets the stable provider identifier.
    /// </summary>
    public string id { get; }
}
