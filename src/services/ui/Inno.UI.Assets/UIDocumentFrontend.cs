using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Inno.Assets;
using Inno.Extensibility.Types;
using Inno.Text;

namespace Inno.UI.Assets;

/// <summary>
/// Contains immutable source text supplied to a UI language frontend.
/// </summary>
/// <param name="assetPath">
/// Original asset mount path.
/// </param>
/// <param name="text">
/// Strictly decoded UTF-8 text.
/// </param>
/// <param name="readSource">
/// Reads and records an optional isolated source dependency.
/// </param>
public sealed record UIDocumentSourceFile(
    string assetPath,
    string text,
    Func<AssetPath, string>? readSource = null
);

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

/// <summary>
/// One font asset declared by a UI document language frontend.
/// </summary>
/// <param name="assetPath">
/// Isolated font asset path.
/// </param>
/// <param name="family">
/// Document-private physical family used in canonical text.
/// </param>
/// <param name="style">
/// CSS-compatible face style.
/// </param>
/// <param name="weight">
/// CSS-compatible face weight.
/// </param>
public sealed record UIDocumentFontDeclaration(
    string assetPath,
    string family,
    TextFontStyle style,
    int weight
);

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

/// <summary>
/// Validates one UI document language without creating runtime UI objects.
/// </summary>
public interface IUIDocumentFrontend
{
    /// <summary>
    /// Gets the open source-language identity.
    /// </summary>
    UIDocumentLanguageID languageID { get; }

    /// <summary>
    /// Analyzes one immutable source snapshot.
    /// </summary>
    /// <param name="source">
    /// Original source snapshot.
    /// </param>
    /// <returns>
    /// Canonical text or neutral diagnostics.
    /// </returns>
    UIDocumentAnalysis Analyze(UIDocumentSourceFile source);
}

/// <summary>
/// Owns one immutable UI language registration snapshot.
/// </summary>
public sealed class UIDocumentFrontendCatalog
{
    private readonly Dictionary<UIDocumentLanguageID, IUIDocumentFrontend> m_frontends = [];

    /// <summary>
    /// Captures a validated provider set.
    /// </summary>
    /// <param name="frontends">
    /// Providers owned by the authoring generation.
    /// </param>
    public UIDocumentFrontendCatalog(IEnumerable<IUIDocumentFrontend> frontends)
    {
        ArgumentNullException.ThrowIfNull(frontends);
        foreach (IUIDocumentFrontend frontend in frontends)
        {
            if (frontend is null || !frontend.languageID.isValid || !m_frontends.TryAdd(frontend.languageID, frontend))
            {
                throw new ArgumentException("UI document frontends require unique, assigned language IDs.", nameof(frontends));
            }
        }
        this.languageIDs = new ReadOnlyCollection<UIDocumentLanguageID>([.. m_frontends.Keys]);
    }

    /// <summary>
    /// Gets registered language identities.
    /// </summary>
    public IReadOnlyList<UIDocumentLanguageID> languageIDs { get; }

    /// <summary>
    /// Analyzes source using an explicit language.
    /// </summary>
    /// <param name="language">
    /// Registered language identity.
    /// </param>
    /// <param name="source">
    /// Immutable source snapshot.
    /// </param>
    /// <returns>
    /// Neutral analysis result.
    /// </returns>
    public UIDocumentAnalysis Analyze(
        UIDocumentLanguageID language,
        UIDocumentSourceFile source
    ) => m_frontends.TryGetValue(language, out IUIDocumentFrontend? frontend)
         ? frontend.Analyze(source) ?? throw new InvalidOperationException($"UI language '{language}' returned no analysis.")
         : throw new NotSupportedException($"UI document language '{language}' is unavailable.");

    internal IEnumerable<IUIDocumentFrontend> providers => m_frontends.Values;
}

/// <summary>
/// Discovers UI language frontends through the shared type-generation transaction.
/// </summary>
public sealed class UIDocumentFrontendRegistry : TypeRegistry<UIDocumentFrontendCatalog>
{
    private readonly TypeCatalog m_types;

    /// <summary>
    /// Registers with the owner type catalog.
    /// </summary>
    /// <param name="types">
    /// Catalog that outlives this registry.
    /// </param>
    public UIDocumentFrontendRegistry(TypeCatalog types)
        : base(types)
        {
        m_types = types;
    }

    /// <summary>
    /// Analyzes source while preventing provider retirement.
    /// </summary>
    /// <param name="language">
    /// Exact language identity.
    /// </param>
    /// <param name="source">
    /// Immutable source snapshot.
    /// </param>
    /// <returns>
    /// Detached neutral analysis.
    /// </returns>
    public UIDocumentAnalysis Analyze(
        UIDocumentLanguageID language,
        UIDocumentSourceFile source
    ) {
        using IDisposable operation = m_types.AcquireOperation("Analyze UI document source");
        return current.Analyze(language, source);
    }

    /// <summary>
    /// Builds a validated result from the current immutable input snapshot.
    /// </summary>
    /// <param name="types">
    /// The active type catalog generation used for extension resolution.
    /// </param>
    /// <returns>
    /// The validated ui document frontend catalog that represents the completed operation.
    /// </returns>
    protected override UIDocumentFrontendCatalog Build(TypeCacheSnapshot types)
    {
        var providers = new List<IUIDocumentFrontend>();
        foreach (Type type in types.GetTypesImplementing<IUIDocumentFrontend>()
            .Select(reference => reference.Resolve(types))
            .OrderBy(static type => type.FullName, StringComparer.Ordinal)
        ) {
            providers.Add(CreateExtension<IUIDocumentFrontend>(type));
        }
        return new UIDocumentFrontendCatalog(providers);
    }

    /// <summary>
    /// Releases the generation lease retained by an immutable registry snapshot.
    /// </summary>
    /// <param name="snapshot">
    /// The immutable state snapshot consumed by this operation.
    /// </param>
    protected override void DisposeSnapshot(UIDocumentFrontendCatalog snapshot) => DisposeExtensions(snapshot.providers);
}
