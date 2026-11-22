using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Assets.Pipeline.Tests;

[AssetImporter("inno.tests.import-dependency")]
internal sealed class ImportGraphAssetImporter : AssetImporter<ImportGraphAsset>
{
    public override IReadOnlyList<string> supportedExtensions { get; } = [".importgraph"];

    protected override ValueTask ImportAsync(
        AssetImportContext context,
        AssetImportWriter<ImportGraphAsset> output,
        CancellationToken cancellationToken
    ) {
        string dependency = context.ReadUtf8Text().Trim();
        if (!string.IsNullOrWhiteSpace(dependency))
        {
            output.DependsOnSource(AssetPath.Parse(dependency));
        }
        output.SetAsset(new ImportGraphAsset());
        return output.WriteArtifactAsync("runtime", context.sourceBytes, cancellationToken);
    }
}
