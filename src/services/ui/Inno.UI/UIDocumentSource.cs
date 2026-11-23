using System;

namespace Inno.UI;

/// <summary>
/// Contains immutable text in one explicitly selected UI document language.
/// </summary>
public sealed class UIDocumentSource
{
    /// <summary>
    /// Creates a validated in-memory document source.
    /// </summary>
    /// <param name="language">
    /// Explicit source language understood by a registered backend.
    /// </param>
    /// <param name="text">
    /// Complete immutable document text.
    /// </param>
    /// <param name="sourceUri">
    /// Virtual source address used only for diagnostics and relative dependency identity.
    /// </param>
    public UIDocumentSource(
        UIDocumentLanguageID language,
        string text,
        string sourceUri = "memory://ui-document"
    ) {
        if (!language.isValid)
        {
            throw new ArgumentException("A document source requires an assigned language.", nameof(language));
        }
        ArgumentException.ThrowIfNullOrWhiteSpace(text);
        ArgumentException.ThrowIfNullOrWhiteSpace(sourceUri);
        this.language = language;
        this.text = text;
        this.sourceUri = sourceUri;
    }

    /// <summary>
    /// Gets the explicitly selected source language.
    /// </summary>
    public UIDocumentLanguageID language { get; }

    /// <summary>
    /// Gets the complete immutable source text.
    /// </summary>
    public string text { get; }

    /// <summary>
    /// Gets the virtual source address.
    /// </summary>
    public string sourceUri { get; }
}
