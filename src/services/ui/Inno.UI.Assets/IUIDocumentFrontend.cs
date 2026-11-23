namespace Inno.UI.Assets;

/// <summary>
/// Validates one UI document language without creating runtime UI objects.
/// </summary>
public interface IUIDocumentFrontend
{
    /// <summary>
    /// Gets the open source-language identity.
    /// </summary>
    UIDocumentLanguageID languageID { get; }

    /// <summary>
    /// Analyzes one immutable source snapshot.
    /// </summary>
    /// <param name="source">
    /// Original source snapshot.
    /// </param>
    /// <returns>
    /// Canonical text or neutral diagnostics.
    /// </returns>
    UIDocumentAnalysis Analyze(UIDocumentSourceFile source);
}
