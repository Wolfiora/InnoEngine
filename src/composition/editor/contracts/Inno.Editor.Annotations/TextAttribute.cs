using System;
using System.Diagnostics;

namespace Inno.Editor.Annotations;

/// <summary>
/// Draws persistent wrapped explanatory text before the annotated Inspector property.
/// </summary>
[Conditional("INNO_EDITOR")]
public sealed class TextAttribute : InspectorPresentationAttribute
{
    /// <summary>
    /// Creates persistent Inspector text.
    /// </summary>
    /// <param name="text">
    /// Non-empty text displayed in the Inspector layout.
    /// </param>
    public TextAttribute(string text)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);
        this.text = text;
    }

    /// <summary>
    /// Gets the persistent Inspector text.
    /// </summary>
    public string text { get; }
}
