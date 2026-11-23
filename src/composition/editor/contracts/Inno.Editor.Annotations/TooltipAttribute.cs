using System;
using System.Diagnostics;

namespace Inno.Editor.Annotations;

/// <summary>
/// Provides hover help for one Inspector property label.
/// </summary>
[Conditional("INNO_EDITOR")]
public sealed class TooltipAttribute : InspectorPresentationAttribute
{
    /// <summary>
    /// Creates property hover help.
    /// </summary>
    /// <param name="text">
    /// Non-empty tooltip text.
    /// </param>
    public TooltipAttribute(string text)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);
        this.text = text;
    }

    /// <summary>
    /// Gets the tooltip text.
    /// </summary>
    public string text { get; }
}
