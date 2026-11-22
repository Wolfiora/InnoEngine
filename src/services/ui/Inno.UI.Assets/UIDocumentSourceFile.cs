using System;
using Inno.Assets;

namespace Inno.UI.Assets;

/// <summary>
/// Contains immutable source text supplied to a UI language frontend.
/// </summary>
/// <param name="assetPath">
/// Original asset mount path.
/// </param>
/// <param name="text">
/// Strictly decoded UTF-8 text.
/// </param>
/// <param name="readSource">
/// Reads and records an optional isolated source dependency.
/// </param>
public sealed record UIDocumentSourceFile(
    string assetPath,
    string text,
    Func<AssetPath, string>? readSource = null
);
