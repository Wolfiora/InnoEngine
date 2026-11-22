using System;
using System.Diagnostics;

namespace Inno.Editor.Annotations;

/// <summary>
/// Starts a visually separated Inspector section before the annotated property.
/// </summary>
[Conditional("INNO_EDITOR")]
public sealed class HeaderAttribute : InspectorPresentationAttribute
{
    /// <summary>
    /// Creates a section header with optional explanatory text.
    /// </summary>
    /// <param name="title">
    /// Visible section title.
    /// </param>
    /// <param name="description">
    /// Optional explanatory text displayed when the title is hovered.
    /// </param>
    public HeaderAttribute(
        string title,
        string description = ""
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        this.title = title;
        this.description = description ?? string.Empty;
    }

    /// <summary>
    /// Gets the visible section title.
    /// </summary>
    public string title { get; }

    /// <summary>
    /// Gets the optional explanatory text.
    /// </summary>
    public string description { get; }
}
