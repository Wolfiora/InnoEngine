using System;
using System.Collections.Generic;
using Inno.Core.Diagnostics;

namespace Inno.Testing.Diagnostics;

/// <summary>
/// Retains the latest diagnostic report of every source so a test can assert what a real producer published.
/// </summary>
public sealed class TestDiagnosticSink : IDiagnosticSink
{
    /// <summary>
    /// Gets the retained reports keyed by their source identifier.
    /// </summary>
    public Dictionary<string, DiagnosticReport> reports { get; } = new(StringComparer.Ordinal);

    /// <inheritdoc/>
    public void Replace(DiagnosticReport report) => reports[report.source.id] = report;

    /// <inheritdoc/>
    public void Clear(DiagnosticSource source) => reports.Remove(source.id);
}
