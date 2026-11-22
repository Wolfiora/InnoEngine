using System;

namespace Inno.UI;

/// <summary>
/// Contains a language-tagged fragment for a backend-specific document mutation.
/// </summary>
public readonly record struct UIDocumentFragment
{
    /// <summary>
    /// Creates a validated source fragment.
    /// </summary>
    /// <param name="language">
    /// Explicit fragment language.
    /// </param>
    /// <param name="text">
    /// Complete fragment text.
    /// </param>
    public UIDocumentFragment(
        UIDocumentLanguageID language,
        string text
    ) {
        if (!language.isValid)
        {
            throw new ArgumentException("A document fragment requires an assigned language.", nameof(language));
        }
        ArgumentNullException.ThrowIfNull(text);
        this.language = language;
        this.text = text;
    }

    /// <summary>
    /// Gets the explicitly selected fragment language.
    /// </summary>
    public UIDocumentLanguageID language { get; }

    /// <summary>
    /// Gets the complete fragment text.
    /// </summary>
    public string text { get; }
}
