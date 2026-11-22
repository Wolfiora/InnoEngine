using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Core.Diagnostics;

namespace Inno.Rendering.Shaders;

/// <summary>
/// Contains a detached typed region only after all graph, port and node lowering validation succeeds.
/// </summary>
public sealed class ShaderGraphLoweringResult
{
    internal ShaderGraphLoweringResult(
        ShaderIrBlock? block,
        IEnumerable<ShaderGraphDiagnostic> diagnostics
    ) {
        this.block = block;
        this.diagnostics = Array.AsReadOnly(diagnostics.ToArray());
    }

    /// <summary>
    /// Gets the immutable typed region, or null on failure.
    /// </summary>
    public ShaderIrBlock? block { get; }

    /// <summary>
    /// Gets located graph diagnostics, including missing definitions and stale source ports.
    /// </summary>
    public IReadOnlyList<ShaderGraphDiagnostic> diagnostics { get; }

    /// <summary>
    /// Gets whether lowering produced a complete region without errors; native compilation is a separate gate.
    /// </summary>
    public bool succeeded => block is not null && diagnostics.All(static value => value.severity != DiagnosticSeverity.Error);
}
