using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Core.Diagnostics;

namespace Inno.Rendering.Shaders;

/// <summary>
/// Contains the result of lowering an entire shader graph, before target-native compilation.
/// </summary>
public sealed class ShaderGraphProgramResult
{
    internal ShaderGraphProgramResult(
        IEnumerable<ShaderGraphPass> passes,
        IEnumerable<ShaderGraphDiagnostic> diagnostics
    ) {
        this.passes = Array.AsReadOnly(passes.ToArray());
        this.diagnostics = Array.AsReadOnly(diagnostics.ToArray());
    }

    /// <summary>
    /// Gets the complete stage set; empty if any stage failed.
    /// </summary>
    public IReadOnlyList<ShaderGraphPass> passes { get; }

    /// <summary>
    /// Gets stable graph/node diagnostics without provider references.
    /// </summary>
    public IReadOnlyList<ShaderGraphDiagnostic> diagnostics { get; }

    /// <summary>
    /// Gets whether every declared pass was lowered without errors.
    /// </summary>
    public bool succeeded => passes.Count != 0 && diagnostics.All(static diagnostic => diagnostic.severity != DiagnosticSeverity.Error);
}
