namespace Inno.UI.Assets;

/// <summary>
/// Reports a language-neutral source problem.
/// </summary>
/// <param name="code">
/// Stable diagnostic code.
/// </param>
/// <param name="message">
/// Actionable description.
/// </param>
/// <param name="line">
/// One-based source line.
/// </param>
/// <param name="column">
/// One-based source column.
/// </param>
public sealed record UIDocumentDiagnostic(
    string code,
    string message,
    int line,
    int column
);
