namespace Inno.Editor.Annotations;

/// <summary>
/// Defines the comparison used by conditional Inspector presentation.
/// </summary>
public enum InspectorCondition
{
    /// <summary>
    /// Shows a value when it evaluates as true or non-empty.
    /// </summary>
    Truthy,
    /// <summary>
    /// Shows a value when it evaluates as false, null, or empty.
    /// </summary>
    Falsy,
    /// <summary>
    /// Compares the value with the supplied expected constant.
    /// </summary>
    Equal,
    /// <summary>
    /// Requires the value to differ from the supplied expected constant.
    /// </summary>
    NotEqual,
    /// <summary>
    /// Requires a null value.
    /// </summary>
    Null,
    /// <summary>
    /// Requires a non-null value.
    /// </summary>
    NotNull,
    /// <summary>
    /// Requires a reference-like value to expose an assigned resource.
    /// </summary>
    Assigned,
    /// <summary>
    /// Requires a reference-like value not to expose an assigned resource.
    /// </summary>
    NotAssigned
}
