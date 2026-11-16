using System.Collections.Generic;

namespace Inno.Tooling.Documentation;

/// <summary>
/// Identifies an immutable source-fact payload protected by a documentation snapshot.
/// Captured facts remain distinct from evidence that they match the current engine candidate.
/// </summary>
/// <param name="path">
/// Original reference path relative to the explicitly selected Docs checkout.
/// </param>
/// <param name="identity">
/// SHA-256 of the complete original reference bytes.
/// </param>
/// <param name="model">
/// Project-inventory or the actual captured API surface model.
/// </param>
/// <param name="configuration">
/// Evaluated API configuration, or null for the project inventory.
/// </param>
/// <param name="target">
/// Explicit captured target, or null for a target-neutral capture.
/// </param>
/// <param name="sourceRevision">
/// Exact source revision recorded for this capture.
/// </param>
/// <param name="sourceTree">
/// Exact source-tree identity recorded for this capture.
/// </param>
/// <param name="projects">
/// Actual project identities present in the payload, including empty public surfaces.
/// </param>
public sealed record DocumentationReferenceInfo(
    string path,
    string identity,
    string model,
    string? configuration,
    string? target,
    string sourceRevision,
    string sourceTree,
    IReadOnlyList<string> projects
);
