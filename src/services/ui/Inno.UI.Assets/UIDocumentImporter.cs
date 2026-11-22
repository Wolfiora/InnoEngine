using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Inno.Assets.Pipeline;
using Inno.Core.Serialization;

namespace Inno.UI.Assets;

/// <summary>
/// Imports implementation-neutral UTF-8 UI source using explicit sidecar settings.
/// </summary>
[AssetImporter("inno.ui.document")]
public sealed class UIDocumentImporter : AssetImporter<UIDocumentAsset>
{
    /// <summary>
    /// Gets the normalized source extensions accepted by this importer.
    /// </summary>
    public override IReadOnlyList<string> supportedExtensions { get; } = [".iuidocument"];

    /// <summary>
    /// Creates an import settings using this implementation's validated inputs.
    /// </summary>
    /// <returns>
    /// The validated iserializable that represents the completed operation.
    /// </returns>
    public override ISerializable CreateImportSettings() => new UIDocumentImportSettings();

    /// <summary>
    /// Imports source content into a validated runtime asset and artifact set.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    /// <param name="output">
    /// The import output writer that receives runtime data and dependency declarations.
    /// </param>
    /// <param name="cancellationToken">
    /// The token that cancels the operation before it commits.
    /// </param>
    /// <returns>
    /// An asynchronous operation that completes after all requested work has finished.
    /// </returns>
    protected override async ValueTask ImportAsync(
        AssetImportContext context,
        AssetImportWriter<UIDocumentAsset> output,
        CancellationToken cancellationToken
    ) {
        UIDocumentImportSettings settings = context.importSettings as UIDocumentImportSettings
                                            ?? throw new InvalidOperationException("UI source requires its standard import settings.");
        ArgumentException.ThrowIfNullOrWhiteSpace(settings.languageId);
        ArgumentException.ThrowIfNullOrWhiteSpace(settings.implementationId);
        await UIDocumentImportPipeline.ImportAsync(
            context,
            output,
            new UIDocumentLanguageID(settings.languageId),
            settings.implementationId,
            cancellationToken
        ).ConfigureAwait(false);
    }
}
