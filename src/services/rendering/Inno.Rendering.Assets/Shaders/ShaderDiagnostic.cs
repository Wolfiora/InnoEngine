using Inno.Core.Diagnostics;
using Inno.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Rendering.Assets;

/// <summary>
/// Reports one structured shader validation or compilation issue.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("31ce0701-357e-5b45-8364-171f59820d8f")]
public sealed class ShaderDiagnostic
{
    /// <summary>
    /// Creates a shader diagnostic.
    /// </summary>
    /// <param name="code">
    /// Stable diagnostic code.
    /// </param>
    /// <param name="severity">
    /// Diagnostic severity.
    /// </param>
    /// <param name="message">
    /// Artist-facing message.
    /// </param>
    /// <param name="location">
    /// Optional source mapping.
    /// </param>
    public ShaderDiagnostic(
        string code,
        DiagnosticSeverity severity,
        string message,
        ShaderSourceLocation? location = null
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        this.code = code;
        this.severity = severity;
        this.message = message;
        this.location = location;
    }

    /// <summary>
    /// Gets the stable diagnostic code.
    /// </summary>
    public string code { get; }

    /// <summary>
    /// Gets the diagnostic severity.
    /// </summary>
    public DiagnosticSeverity severity { get; }

    /// <summary>
    /// Gets the artist-facing message.
    /// </summary>
    public string message { get; }

    /// <summary>
    /// Gets the optional source mapping.
    /// </summary>
    public ShaderSourceLocation? location { get; }
}

