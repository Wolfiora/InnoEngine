using Inno.Text;

namespace Inno.UI.Assets;

/// <summary>
/// One font asset declared by a UI document language frontend.
/// </summary>
/// <param name="assetPath">
/// Isolated font asset path.
/// </param>
/// <param name="family">
/// Document-private physical family used in canonical text.
/// </param>
/// <param name="style">
/// CSS-compatible face style.
/// </param>
/// <param name="weight">
/// CSS-compatible face weight.
/// </param>
public sealed record UIDocumentFontDeclaration(
    string assetPath,
    string family,
    TextFontStyle style,
    int weight
);
