using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Extensibility.Types;

namespace Inno.Rendering.Assets.Authoring;

/// <summary>
/// Describes the artifact selected after evaluating a candidate compilation.
/// </summary>
[StableTypeID("2cbb2378-91ef-5c66-866e-e651e80a79a7")]
public sealed class ShaderArtifactSelection
{
    /// <summary>
    /// Creates an artifact selection result.
    /// </summary>
    /// <param name="artifact">
    /// Selected candidate or last-good artifact.
    /// </param>
    /// <param name="candidateSucceeded">
    /// Whether the candidate replaced active state.
    /// </param>
    /// <param name="usingLastGood">
    /// Whether a previous artifact was preserved.
    /// </param>
    /// <param name="diagnostics">
    /// Candidate diagnostics.
    /// </param>
    public ShaderArtifactSelection(
        CompiledShaderArtifact? artifact,
        bool candidateSucceeded,
        bool usingLastGood,
        IReadOnlyList<ShaderDiagnostic> diagnostics
    ) {
        ArgumentNullException.ThrowIfNull(diagnostics);
        this.artifact = artifact;
        this.candidateSucceeded = candidateSucceeded;
        this.usingLastGood = usingLastGood;
        this.diagnostics = Array.AsReadOnly(diagnostics.ToArray());
    }

    /// <summary>
    /// Gets the selected candidate or last-good artifact.
    /// </summary>
    public CompiledShaderArtifact? artifact { get; }

    /// <summary>
    /// Gets whether the candidate replaced active state.
    /// </summary>
    public bool candidateSucceeded { get; }

    /// <summary>
    /// Gets whether a previous artifact was preserved.
    /// </summary>
    public bool usingLastGood { get; }

    /// <summary>
    /// Gets candidate diagnostics.
    /// </summary>
    public IReadOnlyList<ShaderDiagnostic> diagnostics { get; }
}
