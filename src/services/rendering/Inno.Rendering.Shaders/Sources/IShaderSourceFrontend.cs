using Inno.Core.Diagnostics;

namespace Inno.Rendering.Shaders;

/// <summary>
/// Parses one source language into canonical function interfaces; it does not create GPU objects.
/// </summary>
public interface IShaderSourceFrontend
{
    /// <summary>
    /// Gets the open source language identity, distinct from a rendering backend or GPU API.
    /// </summary>
    string languageID { get; }

    /// <summary>
    /// Analyzes an immutable source candidate with explicit preprocessing inputs.
    /// </summary>
    /// <param name="request">
    /// Source, selected function, dependencies, and target inputs.
    /// </param>
    /// <returns>
    /// A successful interface or a recoverable set of original-source diagnostics.
    /// </returns>
    ShaderSourceAnalysis Analyze(ShaderSourceRequest request);
}

/// <summary>
/// Identifies an original source position, not a generated temporary compiler filename.
/// </summary>
/// <param name="assetPath">
/// Source path in the current asset mount.
/// </param>
/// <param name="line">
/// One-based source line.
/// </param>
/// <param name="column">
/// One-based source column.
/// </param>
public readonly record struct ShaderSourcePosition(
    string assetPath,
    int line,
    int column
);

/// <summary>
/// Reports a frontend problem without retaining a language-specific exception or parser.
/// </summary>
/// <param name="code">
/// Stable diagnostic code.
/// </param>
/// <param name="severity">
/// Diagnostic severity.
/// </param>
/// <param name="message">
/// Actionable diagnostic description.
/// </param>
/// <param name="location">
/// Original source position.
/// </param>
public sealed record ShaderSourceDiagnostic(
    string code,
    DiagnosticSeverity severity,
    string message,
    ShaderSourcePosition location
);

/// <summary>
/// Contains immutable source text supplied by the asset candidate's controlled source resolver.
/// </summary>
/// <param name="assetPath">
/// Resolved asset mount path.
/// </param>
/// <param name="text">
/// UTF-8 decoded immutable source text.
/// </param>
public sealed record ShaderSourceFile(
    string assetPath,
    string text
);
