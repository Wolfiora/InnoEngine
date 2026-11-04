namespace Inno.UI.Assets;

/// <summary>
/// Declares an image dependency without decoding it in the document language frontend.
/// </summary>
/// <param name="assetPath">
/// Source-aware texture asset path.
/// </param>
/// <param name="source">
/// Opaque name embedded in canonical source by the language frontend.
/// </param>
public sealed record UiDocumentImageDeclaration(
    string assetPath,
    string source
);
