using System;
using System.Diagnostics;

namespace Inno.Editor.Annotations;

/// <summary>
/// Hides the annotated Inspector property when a sibling member satisfies a condition.
/// </summary>
[Conditional("INNO_EDITOR")]
public sealed class HideIfAttribute : InspectorPresentationAttribute
{
    /// <summary>
    /// Creates a truthiness condition.
    /// </summary>
    /// <param name="memberName">
    /// Sibling field or property name.
    /// </param>
    public HideIfAttribute(string memberName)
        : this(memberName, InspectorCondition.Truthy, null) {}

    /// <summary>
    /// Creates an equality condition.
    /// </summary>
    /// <param name="memberName">
    /// Sibling field or property name.
    /// </param>
    /// <param name="expectedValue">
    /// Compile-time value that hides the property.
    /// </param>
    public HideIfAttribute(
        string memberName,
        object? expectedValue
    ) : this(memberName, InspectorCondition.Equal, expectedValue) {}

    /// <summary>
    /// Creates a named condition that does not require an expected value.
    /// </summary>
    /// <param name="memberName">
    /// Sibling field or property name.
    /// </param>
    /// <param name="condition">
    /// Comparison applied to the sibling value.
    /// </param>
    public HideIfAttribute(
        string memberName,
        InspectorCondition condition
    ) : this(memberName, condition, null) {}

    private HideIfAttribute(
        string memberName,
        InspectorCondition condition,
        object? expectedValue
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(memberName);
        this.memberName = memberName;
        this.condition = condition;
        this.expectedValue = expectedValue;
    }

    /// <summary>
    /// Gets the sibling member name.
    /// </summary>
    public string memberName { get; }

    /// <summary>
    /// Gets the comparison.
    /// </summary>
    public InspectorCondition condition { get; }

    /// <summary>
    /// Gets the optional expected constant.
    /// </summary>
    public object? expectedValue { get; }
}
