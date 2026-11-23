using System;

namespace Inno.Rendering.Shaders;

/// <summary>
/// Identifies one explicitly selected implementation and preprocessing configuration of a source module.
/// </summary>
public sealed class ShaderSourceImplementationRequest
{
    /// <summary>
    /// Creates a candidate without inferring a language or adapter from its filename.
    /// </summary>
    /// <param name="implementationID">
    /// Unique owner-defined key, including the target or variant when applicable.
    /// </param>
    /// <param name="languageID">
    /// Registered source language identity.
    /// </param>
    /// <param name="source">
    /// Source function and candidate-scoped dependency resolver.
    /// </param>
    public ShaderSourceImplementationRequest(
        string implementationID,
        string languageID,
        ShaderSourceRequest source
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(implementationID);
        ArgumentException.ThrowIfNullOrWhiteSpace(languageID);
        ArgumentNullException.ThrowIfNull(source);
        this.implementationID = implementationID;
        this.languageID = languageID;
        this.source = source;
    }

    /// <summary>
    /// Gets the stable owner-defined implementation/configuration key.
    /// </summary>
    public string implementationID { get; }

    /// <summary>
    /// Gets the explicitly selected language identity.
    /// </summary>
    public string languageID { get; }

    /// <summary>
    /// Gets the transient candidate request, which must not be persisted or retained after analysis.
    /// </summary>
    public ShaderSourceRequest source { get; }
}
