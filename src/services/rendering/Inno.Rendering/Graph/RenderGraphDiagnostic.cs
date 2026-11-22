using System;
using Inno.Core.Diagnostics;

namespace Inno.Rendering;

/// <summary>
/// Reports one structured render-graph compilation problem.
/// </summary>
public sealed class RenderGraphDiagnostic
{
    /// <summary>
    /// Creates a render-graph diagnostic.
    /// </summary>
    /// <param name="code">
    /// Stable machine-readable code.
    /// </param>
    /// <param name="message">
    /// Actionable diagnostic text.
    /// </param>
    /// <param name="severity">
    /// Diagnostic impact.
    /// </param>
    /// <param name="passName">
    /// Optional related pass name.
    /// </param>
    /// <param name="resourceName">
    /// Optional related resource name.
    /// </param>
    public RenderGraphDiagnostic(
        string code,
        string message,
        DiagnosticSeverity severity,
        string? passName = null,
        string? resourceName = null
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        this.code = code;
        this.message = message;
        this.severity = severity;
        this.passName = passName;
        this.resourceName = resourceName;
    }

    /// <summary>
    /// Gets the stable machine-readable code.
    /// </summary>
    public string code { get; }

    /// <summary>
    /// Gets actionable diagnostic text.
    /// </summary>
    public string message { get; }

    /// <summary>
    /// Gets diagnostic impact.
    /// </summary>
    public DiagnosticSeverity severity { get; }

    /// <summary>
    /// Gets the related pass name, if any.
    /// </summary>
    public string? passName { get; }

    /// <summary>
    /// Gets the related resource name, if any.
    /// </summary>
    public string? resourceName { get; }
}
