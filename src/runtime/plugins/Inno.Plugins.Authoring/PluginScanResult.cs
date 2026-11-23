using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Plugins.Authoring;

/// <summary>
/// Contains a complete immutable Plugin discovery result.
/// </summary>
public sealed class PluginScanResult
{
    internal PluginScanResult(
        IReadOnlyList<PluginCandidate> candidates,
        IReadOnlyList<PluginDiagnostic> diagnostics
    ) {
        this.candidates = Array.AsReadOnly(candidates.ToArray());
        this.diagnostics = Array.AsReadOnly(diagnostics.ToArray());
    }

    /// <summary>
    /// Gets dependency-ordered valid candidates.
    /// </summary>
    public IReadOnlyList<PluginCandidate> candidates { get; }

    /// <summary>
    /// Gets isolated diagnostics for rejected candidates.
    /// </summary>
    public IReadOnlyList<PluginDiagnostic> diagnostics { get; }
}
