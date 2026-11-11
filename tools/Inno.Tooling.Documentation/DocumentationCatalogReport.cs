using System.Collections.Generic;

namespace Inno.Tooling.Documentation;

/// <summary>
/// Describes a prepared snapshot for website, coverage and transport adapters without reparsing Markdown.
/// Content identity excludes query implementation details; source verification remains separate.
/// </summary>
/// <param name="snapshot">
/// Exact query generation protected by the exporting session.
/// </param>
/// <param name="contentIdentity">
/// SHA-256 of sorted page paths and content hashes, independent of machine paths and query binaries.
/// </param>
/// <param name="repository">
/// Source repository recorded in the selected binding.
/// </param>
/// <param name="revision">
/// Exact engine source revision recorded in the selected binding.
/// </param>
/// <param name="sourceTree">
/// Engine source-tree identity; this value alone does not certify compatibility.
/// </param>
/// <param name="toolingRevision">
/// Tooling revision requested by the documentation source binding.
/// </param>
/// <param name="sourceApiStatus">
/// Source API evidence state. Unverified must not be treated as a passed API check.
/// </param>
/// <param name="projects">
/// Actual project identities mapped to their authoritative page identity; null identifies missing coverage.
/// </param>
/// <param name="pages">
/// Parsed page facts in stable path order, including complete section and contract identities.
/// </param>
/// <param name="references">
/// Protected project/API payloads available from the same snapshot without returning to a mutable checkout.
/// </param>
public sealed record DocumentationCatalogReport(
    string snapshot,
    string contentIdentity,
    string repository,
    string revision,
    string sourceTree,
    string toolingRevision,
    string sourceApiStatus,
    IReadOnlyDictionary<string, string?> projects,
    IReadOnlyList<DocumentationPageInfo> pages,
    IReadOnlyList<DocumentationReferenceInfo> references
) {
    /// <summary>
    /// Gets the matching trusted source certificate identity, or null for an unverified catalog.
    /// </summary>
    public string? sourceEvidenceIdentity { get; init; }
}
