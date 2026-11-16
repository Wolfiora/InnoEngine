namespace Inno.Tooling.Documentation;

/// <summary>
/// Identifies unread content without silently dropping constraints or splitting their ownership.
/// </summary>
/// <param name="pageId">
/// Stable page identity within the same snapshot.
/// </param>
/// <param name="section">
/// Section anchor, or null for the entire page.
/// </param>
/// <param name="offset">
/// UTF-16 position within the selection for a content continuation; zero begins a related contract.
/// </param>
/// <param name="reason">
/// Whether the target continues original text or supplies a necessary associated contract.
/// </param>
public sealed record DocumentationContinuation(
    string pageId,
    string? section,
    int offset,
    string reason
);
