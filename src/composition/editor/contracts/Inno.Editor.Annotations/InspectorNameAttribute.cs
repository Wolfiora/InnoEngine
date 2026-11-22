using System;
using System.Diagnostics;

namespace Inno.Editor.Annotations;

/// <summary>
/// Replaces the generated Inspector label for one property.
/// </summary>
[Conditional("INNO_EDITOR")]
public sealed class InspectorNameAttribute : InspectorPresentationAttribute
{
    /// <summary>
    /// Creates a label override.
    /// </summary>
    /// <param name="name">
    /// Non-empty visible label.
    /// </param>
    public InspectorNameAttribute(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        this.name = name;
    }

    /// <summary>
    /// Gets the visible label.
    /// </summary>
    public string name { get; }
}
