using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Assets.Pipeline.Tests;

[AssetImporter("inno.tests.conflict-b")]
internal sealed class ImporterConflictAssetImporterB : AssetImporter<ImporterConflictAsset>
{
    public override IReadOnlyList<string> supportedExtensions
        => ImporterConflictProbe.duplicateExtension
           ? [".conflict"]
           : [".probeb"];

    protected override ValueTask ImportAsync(
        AssetImportContext context,
        AssetImportWriter<ImporterConflictAsset> output,
        CancellationToken cancellationToken
    ) {
        output.SetAsset(new ImporterConflictAsset());
        return output.WriteArtifactAsync("runtime", context.sourceBytes, cancellationToken: cancellationToken);
    }
}
