using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Core.Diagnostics;
using Inno.Extensibility.Types;

namespace Inno.Rendering.Assets.Authoring;

/// <summary>
/// Returns a candidate artifact and structured diagnostics without mutating active state.
/// </summary>
[StableTypeID("72b304de-8a2b-5c08-b899-b4aef05be149")]
public sealed class ShaderCompilationResult
{
    /// <summary>
    /// Creates a shader compilation result.
    /// </summary>
    /// <param name="artifact">
    /// Candidate artifact, or <see langword="null"/> after failure.
    /// </param>
    /// <param name="diagnostics">
    /// Validation and compiler diagnostics.
    /// </param>
    public ShaderCompilationResult(
        CompiledShaderArtifact? artifact,
        IReadOnlyList<ShaderDiagnostic> diagnostics
    ) {
        ArgumentNullException.ThrowIfNull(diagnostics);
        this.artifact = artifact;
        this.diagnostics = Array.AsReadOnly(diagnostics.ToArray());
    }

    /// <summary>
    /// Gets the candidate artifact, or <see langword="null"/> after failure.
    /// </summary>
    public CompiledShaderArtifact? artifact { get; }

    /// <summary>
    /// Gets validation and compiler diagnostics.
    /// </summary>
    public IReadOnlyList<ShaderDiagnostic> diagnostics { get; }

    /// <summary>
    /// Gets whether a complete candidate artifact was produced.
    /// </summary>
    public bool succeeded
        => artifact is not null
           && diagnostics.All(static value => value.severity != DiagnosticSeverity.Error);
}
