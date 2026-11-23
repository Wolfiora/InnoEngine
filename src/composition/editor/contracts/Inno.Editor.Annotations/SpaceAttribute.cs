using System;
using System.Diagnostics;

namespace Inno.Editor.Annotations;

/// <summary>
/// Inserts vertical spacing before the annotated Inspector property.
/// </summary>
[Conditional("INNO_EDITOR")]
public sealed class SpaceAttribute : InspectorPresentationAttribute
{
    /// <summary>
    /// Creates a spacing decorator.
    /// </summary>
    /// <param name="height">
    /// Non-negative logical pixel height.
    /// </param>
    public SpaceAttribute(float height = 8f)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(height);
        this.height = height;
    }

    /// <summary>
    /// Gets the logical pixel height.
    /// </summary>
    public float height { get; }
}
