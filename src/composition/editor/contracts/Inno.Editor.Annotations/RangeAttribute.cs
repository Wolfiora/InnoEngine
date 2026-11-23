using System;
using System.Diagnostics;

namespace Inno.Editor.Annotations;

/// <summary>
/// Constrains a numeric Inspector control to an inclusive range.
/// </summary>
[Conditional("INNO_EDITOR")]
public sealed class RangeAttribute : InspectorPresentationAttribute
{
    /// <summary>
    /// Creates an inclusive numeric range.
    /// </summary>
    /// <param name="minimum">
    /// Inclusive lower bound.
    /// </param>
    /// <param name="maximum">
    /// Inclusive upper bound.
    /// </param>
    public RangeAttribute(
        double minimum,
        double maximum
    ) {
        if (!double.IsFinite(minimum))
        {
            throw new ArgumentOutOfRangeException(nameof(minimum));
        }
        if (!double.IsFinite(maximum) || maximum < minimum)
        {
            throw new ArgumentOutOfRangeException(nameof(maximum));
        }
        this.minimum = minimum;
        this.maximum = maximum;
    }

    /// <summary>
    /// Gets the inclusive lower bound.
    /// </summary>
    public double minimum { get; }

    /// <summary>
    /// Gets the inclusive upper bound.
    /// </summary>
    public double maximum { get; }
}
