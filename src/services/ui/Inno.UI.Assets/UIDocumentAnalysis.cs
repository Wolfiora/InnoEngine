using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Inno.UI.Assets;

/// <summary>
/// Returns validated text without retaining parser or implementation objects.
/// </summary>
public sealed class UIDocumentAnalysis
{
    /// <summary>
    /// Creates a frozen analysis result.
    /// </summary>
    /// <param name="text">
    /// Validated canonical text, or null on failure.
    /// </param>
    /// <param name="diagnostics">
    /// Complete frontend diagnostics.
    /// </param>
    /// <param name="fonts">
    /// Font assets declared by the document.
    /// </param>
    public UIDocumentAnalysis(
        string? text,
        IEnumerable<UIDocumentDiagnostic> diagnostics,
        IEnumerable<UIDocumentFontDeclaration>? fonts = null
    ) {
        this.text = text;
        this.diagnostics = new ReadOnlyCollection<UIDocumentDiagnostic>(
            diagnostics?.ToArray()
            ?? throw new ArgumentNullException(nameof(diagnostics))
        );
        this.fonts = new ReadOnlyCollection<UIDocumentFontDeclaration>(fonts?.ToArray() ?? []);
    }

    /// <summary>
    /// Gets canonical source text, or null after failure.
    /// </summary>
    public string? text { get; }

    /// <summary>
    /// Gets frozen diagnostics.
    /// </summary>
    public IReadOnlyList<UIDocumentDiagnostic> diagnostics { get; }

    /// <summary>
    /// Gets font assets declared by the canonical document.
    /// </summary>
    public IReadOnlyList<UIDocumentFontDeclaration> fonts { get; }

    /// <summary>
    /// Gets whether the frontend accepted the source.
    /// </summary>
    public bool succeeded => text is not null && diagnostics.Count == 0;
}
