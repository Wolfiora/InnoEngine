using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Extensibility.Types;

namespace Inno.UI.Assets;

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
        List<IUIDocumentFrontend> providers = [];
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
