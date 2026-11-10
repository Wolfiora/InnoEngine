using System;

namespace Inno.Tooling.Documentation;

/// <summary>
/// Selects a read-only query against one already published documentation snapshot.
/// </summary>
/// <param name="operation">
/// Search, read, symbol, resource or status; operation names use lowercase ASCII.
/// </param>
/// <param name="value">
/// Search terms, a stable page identity or relative content path, an XML symbol ID, or a resource URI.
/// </param>
/// <param name="project">
/// An exact project identity from the source project manifest, or null for every project.
/// </param>
/// <param name="surface">
/// An exact API surface identity, or null to return qualified candidates without choosing a surface.
/// </param>
/// <param name="section">
/// A section anchor within the selected page, or null for the page.
/// </param>
/// <param name="offset">
/// Zero-based UTF-16 continuation offset within the selected content; search uses a result offset.
/// </param>
/// <param name="limit">
/// Maximum search candidates, between one and one hundred. Content reads retain complete units or explicit continuations.
/// </param>
public sealed record DocumentationRequest(
    string operation,
    string? value = null,
    string? project = null,
    string? surface = null,
    string? section = null,
    int offset = 0,
    int limit = 5
);
