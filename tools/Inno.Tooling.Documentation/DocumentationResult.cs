using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Inno.Tooling.Documentation;

/// <summary>
/// Returns transport-independent documentation content with explicit completeness and provenance.
/// </summary>
/// <param name="status">
/// Found, Empty, Ambiguous, Missing, Partial, Navigation, Mismatched or Ready. Execution failures throw rather than resemble content.
/// </param>
/// <param name="snapshot">
/// The immutable documentation generation identity.
/// </param>
/// <param name="sourceApiStatus">
/// Source validation state. Unverified never asserts that declarations match current engine source.
/// </param>
/// <param name="message">
/// A concise explanation of the result and any required next action.
/// </param>
/// <param name="hits">
/// Ordered content or navigation candidates; each identifies its completeness independently.
/// </param>
/// <param name="continuations">
/// Remaining content or necessary contracts that must be read before claiming a complete contract.
/// </param>
/// <param name="nextRequest">
/// Structured continuation for a result page or content segment, or null when that selection is exhausted.
/// Associated contracts remain separately listed in continuations.
/// </param>
public sealed record DocumentationResult(
    string status,
    string snapshot,
    string sourceApiStatus,
    string message,
    IReadOnlyList<DocumentationHit> hits,
    IReadOnlyList<DocumentationContinuation> continuations,
    DocumentationRequest? nextRequest = null
) {
    /// <summary>
    /// Gets measured preparation phases in milliseconds, or null for an ordinary read.
    /// Total and PagePreparation are inclusive; MarkdownParsing and OwnerRegistration are subsets
    /// of PagePreparation. These observations are not performance or execution certificates.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyDictionary<string, double>? timingsMilliseconds { get; init; }
}
