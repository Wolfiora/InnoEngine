using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Inno.UI.Assets;

/// <summary>
/// Owns one immutable UI language registration snapshot.
/// </summary>
public sealed class UIDocumentFrontendCatalog
{
    private readonly Dictionary<UIDocumentLanguageID, IUIDocumentFrontend> m_frontends = [];

    /// <summary>
    /// Captures a validated provider set.
    /// </summary>
    /// <param name="frontends">
    /// Providers owned by the authoring generation.
    /// </param>
    public UIDocumentFrontendCatalog(IEnumerable<IUIDocumentFrontend> frontends)
    {
        ArgumentNullException.ThrowIfNull(frontends);
        foreach (IUIDocumentFrontend frontend in frontends)
        {
            if (frontend is null || !frontend.languageID.isValid || !m_frontends.TryAdd(frontend.languageID, frontend))
            {
                throw new ArgumentException("UI document frontends require unique, assigned language IDs.", nameof(frontends));
            }
        }
        this.languageIDs = new ReadOnlyCollection<UIDocumentLanguageID>([.. m_frontends.Keys]);
    }

    /// <summary>
    /// Gets registered language identities.
    /// </summary>
    public IReadOnlyList<UIDocumentLanguageID> languageIDs { get; }

    /// <summary>
    /// Analyzes source using an explicit language.
    /// </summary>
    /// <param name="language">
    /// Registered language identity.
    /// </param>
    /// <param name="source">
    /// Immutable source snapshot.
    /// </param>
    /// <returns>
    /// Neutral analysis result.
    /// </returns>
    public UIDocumentAnalysis Analyze(
        UIDocumentLanguageID language,
        UIDocumentSourceFile source
    ) => m_frontends.TryGetValue(language, out IUIDocumentFrontend? frontend)
         ? frontend.Analyze(source) ?? throw new InvalidOperationException($"UI language '{language}' returned no analysis.")
         : throw new NotSupportedException($"UI document language '{language}' is unavailable.");

    internal IEnumerable<IUIDocumentFrontend> providers => m_frontends.Values;
}
