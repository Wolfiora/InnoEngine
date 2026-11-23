using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Core.Diagnostics;

namespace Inno.Rendering.Shaders;

/// <summary>
/// Returns a recoverable source analysis result and complete original-source dependencies.
/// </summary>
public sealed class ShaderSourceAnalysis
{
    /// <summary>
    /// Captures a completed analysis without retaining compiler objects.
    /// </summary>
    /// <param name="function">
    /// Parsed interface, or null when parsing failed.
    /// </param>
    /// <param name="dependencies">
    /// Resolved included sources, excluding the root.
    /// </param>
    /// <param name="diagnostics">
    /// All problems discovered by the frontend.
    /// </param>
    public ShaderSourceAnalysis(
        ShaderSourceFunction? function,
        IEnumerable<string> dependencies,
        IEnumerable<ShaderSourceDiagnostic> diagnostics
    ) {
        ArgumentNullException.ThrowIfNull(dependencies);
        ArgumentNullException.ThrowIfNull(diagnostics);
        this.function = function;
        this.dependencies = Array.AsReadOnly(dependencies.Distinct(StringComparer.Ordinal).ToArray());
        this.diagnostics = Array.AsReadOnly(diagnostics.ToArray());
    }

    /// <summary>
    /// Gets the parsed interface, or null after an analysis failure.
    /// </summary>
    public ShaderSourceFunction? function { get; }

    /// <summary>
    /// Gets original source dependencies for invalidation.
    /// </summary>
    public IReadOnlyList<string> dependencies { get; }

    /// <summary>
    /// Gets immutable, provider-neutral diagnostics.
    /// </summary>
    public IReadOnlyList<ShaderSourceDiagnostic> diagnostics { get; }

    /// <summary>
    /// Gets whether a usable interface was found without errors.
    /// </summary>
    public bool succeeded => function is not null && !diagnostics.Any(static item => item.severity == DiagnosticSeverity.Error);
}
