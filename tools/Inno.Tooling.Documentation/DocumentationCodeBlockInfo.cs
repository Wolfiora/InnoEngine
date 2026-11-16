using System.Collections.Generic;

namespace Inno.Tooling.Documentation;

/// <summary>
/// Describes one canonical fenced code block without executing it or inferring a compilation environment.
/// </summary>
/// <param name="language">
/// The authored fence language. An empty value remains visible for content-policy validation.
/// </param>
/// <param name="text">
/// Original code with normalized LF line endings and no closing-fence newline.
/// </param>
/// <param name="contentIdentity">
/// SHA-256 of the normalized code, independent of its source position.
/// </param>
/// <param name="line">
/// One-based line of the opening fence in the authored Markdown snapshot.
/// </param>
/// <param name="section">
/// The containing canonical heading identity, or null when the block precedes every heading.
/// </param>
/// <param name="declarationSymbols">
/// Symbols whose recorded declaration identity exactly matches this block.
/// Other blocks remain examples or instructions requiring an explicit validation context.
/// A matching declaration is not a source API or executable-example certificate.
/// </param>
public sealed record DocumentationCodeBlockInfo(
    string language,
    string text,
    string contentIdentity,
    int line,
    string? section,
    IReadOnlyList<string> declarationSymbols
);
