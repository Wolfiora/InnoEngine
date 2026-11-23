using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Inno.UI;

/// <summary>
/// Describes optional behavior implemented by one UI backend generation.
/// </summary>
public sealed class UIBackendCapabilities
{
    private readonly IReadOnlyList<UIDocumentLanguageID> m_documentLanguages;

    /// <summary>
    /// Creates an immutable capability description.
    /// </summary>
    /// <param name="documentLanguages">
    /// Explicit source languages accepted by the backend.
    /// </param>
    /// <param name="supportsFontCollectionFaces">
    /// Whether nonzero font collection face indices can be selected.
    /// </param>
    public UIBackendCapabilities(
        IEnumerable<UIDocumentLanguageID> documentLanguages,
        bool supportsFontCollectionFaces
    ) {
        ArgumentNullException.ThrowIfNull(documentLanguages);
        UIDocumentLanguageID[] languages = documentLanguages.Distinct().ToArray();
        if (languages.Length == 0 || languages.Any(static language => !language.isValid))
        {
            throw new ArgumentException("A UI backend must declare at least one assigned document language.", nameof(documentLanguages));
        }
        m_documentLanguages = new ReadOnlyCollection<UIDocumentLanguageID>(languages);
        this.supportsFontCollectionFaces = supportsFontCollectionFaces;
    }

    /// <summary>
    /// Gets source languages accepted by the backend.
    /// </summary>
    public IReadOnlyList<UIDocumentLanguageID> documentLanguages => m_documentLanguages;

    /// <summary>
    /// Gets whether the backend can select nonzero faces from a font collection.
    /// </summary>
    public bool supportsFontCollectionFaces { get; }

    /// <summary>
    /// Determines whether an exact source language is supported.
    /// </summary>
    /// <param name="language">
    /// Language to test.
    /// </param>
    /// <returns>
    /// True when the language is registered by this backend.
    /// </returns>
    public bool Supports(UIDocumentLanguageID language) => m_documentLanguages.Contains(language);
}

/// <summary>
/// Identifies an optional backend behavior requested by a caller.
/// </summary>
public enum UIBackendCapability
{
    /// <summary>
    /// Selecting an explicit face from a font collection.
    /// </summary>
    FontCollectionFaceSelection
}
