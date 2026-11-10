using System.Collections.Generic;

namespace Inno.Tooling.Documentation;

/// <summary>
/// Identifies an original content unit or an explicitly incomplete search excerpt.
/// </summary>
/// <param name="pageId">
/// Stable page identity independent of its title or current path.
/// </param>
/// <param name="page">
/// Relative path within the documentation checkout.
/// </param>
/// <param name="section">
/// Stable section anchor, or an empty string for a whole page.
/// </param>
/// <param name="line">
/// One-based source line of the selected content.
/// </param>
/// <param name="projects">
/// Actual source project owners declared by the page and validated against the reference manifest.
/// </param>
/// <param name="surface">
/// Qualified API surface, or null for surface-independent guidance.
/// </param>
/// <param name="symbol">
/// Exact XML symbol identity when resolving a symbol, otherwise null.
/// </param>
/// <param name="reason">
/// Why this content was selected.
/// </param>
/// <param name="completeness">
/// CompleteUnit, Excerpt or Partial; complete units may still have necessary related contracts.
/// </param>
/// <param name="text">
/// Original Markdown for reads, or an explicitly marked excerpt for search.
/// </param>
/// <param name="resource">
/// Stable inno-doc resource URI bound to the returned snapshot and page.
/// </param>
public sealed record DocumentationHit(
    string pageId,
    string page,
    string section,
    int line,
    IReadOnlyList<string> projects,
    string? surface,
    string? symbol,
    string reason,
    string completeness,
    string text,
    string resource
);
