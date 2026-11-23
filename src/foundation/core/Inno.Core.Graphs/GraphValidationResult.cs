using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Core.Diagnostics;

namespace Inno.Core.Graphs;

/// <summary>
/// Contains deterministic graph validation diagnostics.
/// </summary>
public sealed class GraphValidationResult
{
    private readonly IReadOnlyList<GraphDiagnostic> m_diagnostics;

    /// <summary>
    /// Creates a graph validation result.
    /// </summary>
    /// <param name="diagnostics">
    /// Diagnostics in deterministic validation order.
    /// </param>
    public GraphValidationResult(IReadOnlyList<GraphDiagnostic> diagnostics)
    {
        ArgumentNullException.ThrowIfNull(diagnostics);
        m_diagnostics = Array.AsReadOnly(diagnostics.ToArray());
    }

    /// <summary>
    /// Gets diagnostics in deterministic validation order.
    /// </summary>
    public IReadOnlyList<GraphDiagnostic> diagnostics => m_diagnostics;

    /// <summary>
    /// Gets whether no error diagnostic is present.
    /// </summary>
    public bool isValid
    {
        get
        {
            foreach (GraphDiagnostic diagnostic in m_diagnostics)
            {
                if (diagnostic.severity == DiagnosticSeverity.Error)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
