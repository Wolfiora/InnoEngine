using System.Collections.Generic;

namespace Inno.Tooling.Documentation;

/// <summary>
/// Supplies canonical page metadata to navigation and rendered-output checks without a second Markdown parser.
/// </summary>
/// <param name="id">
/// Stable authored page identity.
/// </param>
/// <param name="path">
/// Source path relative to the selected Docs checkout.
/// </param>
/// <param name="contentIdentity">
/// SHA-256 of original UTF-8 Markdown text.
/// </param>
/// <param name="title">
/// Plain-text H1 derived by the canonical parser.
/// </param>
/// <param name="kind">
/// Validated page purpose, such as tutorial, project or api.
/// </param>
/// <param name="owners">
/// Actual project identities from the bound manifest.
/// </param>
/// <param name="surface">
/// Qualified API surface, or null for shared guidance.
/// </param>
/// <param name="sections">
/// Canonical heading anchors mapped to one-based source lines.
/// </param>
/// <param name="symbols">
/// Exact XML symbol identities mapped to canonical anchors, signature identities and visible code.
/// </param>
/// <param name="contracts">
/// Necessary page or section identities that must remain visible to readers.
/// </param>
/// <param name="includes">
/// Source-relative API includes, validated for ownership, existence and cycles.
/// </param>
/// <param name="codeBlocks">
/// Canonical fenced code for example selection and declaration comparison, without reparsing Markdown.
/// Compilation contexts and execution evidence belong to the validation caller.
/// </param>
public sealed record DocumentationPageInfo(
    string id,
    string path,
    string contentIdentity,
    string title,
    string kind,
    IReadOnlyList<string> owners,
    string? surface,
    IReadOnlyDictionary<string, int> sections,
    IReadOnlyDictionary<string, DocumentationSymbolInfo> symbols,
    IReadOnlyList<string> contracts,
    IReadOnlyList<string> includes,
    IReadOnlyList<DocumentationCodeBlockInfo> codeBlocks
);
