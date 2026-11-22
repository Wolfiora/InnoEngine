using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Core.Diagnostics;

namespace Inno.Rendering.Shaders;

/// <summary>
/// Reports whether every supplied implementation and variant has the same callable graph interface.
/// </summary>
public sealed class ShaderSourceModuleAnalysis
{
    internal ShaderSourceModuleAnalysis(
        IEnumerable<ShaderSourceImplementationAnalysis> implementations,
        IEnumerable<ShaderSourceDiagnostic> diagnostics
    ) {
        this.implementations = Array.AsReadOnly(implementations.ToArray());
        this.diagnostics = Array.AsReadOnly(diagnostics.ToArray());
        this.succeeded = this.implementations.Count != 0 && this.implementations.All(static item => item.analysis.succeeded)
                         && this.diagnostics.All(static item => item.severity != DiagnosticSeverity.Error);
        this.function = succeeded ? this.implementations[0].analysis.function : null;
    }

    /// <summary>
    /// Gets all implementation snapshots, including failed or unavailable implementations.
    /// </summary>
    public IReadOnlyList<ShaderSourceImplementationAnalysis> implementations { get; }

    /// <summary>
    /// Gets implementation and cross-implementation diagnostics with original source positions.
    /// </summary>
    public IReadOnlyList<ShaderSourceDiagnostic> diagnostics { get; }

    /// <summary>
    /// Gets the common interface only when all supplied implementations agree and are valid.
    /// </summary>
    public ShaderSourceFunction? function { get; }

    /// <summary>
    /// Gets whether interface analysis succeeded; native compilation is a separate gate.
    /// </summary>
    public bool succeeded { get; }
}

/// <summary>
/// Records an include resolution edge so compilation never reinterprets authoring paths against live files.
/// </summary>
/// <param name="includingFile">
/// Source that requested the include.
/// </param>
/// <param name="include">
/// Original include spelling passed to the source resolver.
/// </param>
/// <param name="resolvedPath">
/// Exact source identity within the frozen file set.
/// </param>
public sealed record ShaderSourceInclude(
    string includingFile,
    string include,
    string resolvedPath
);
