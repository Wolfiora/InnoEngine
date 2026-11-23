using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Assets.Pipeline.Tests;

[AssetImporter("inno.tests.runtime-dependency")]
internal sealed class DependencyAssetImporter : AssetImporter<DependencyAsset>
{
    public override IReadOnlyList<string> supportedExtensions { get; } = [".depgraph"];

    protected override ValueTask ImportAsync(
        AssetImportContext context,
        AssetImportWriter<DependencyAsset> output,
        CancellationToken cancellationToken
    ) {
        string dependency = context.ReadUtf8Text().Trim();
        if (!string.IsNullOrWhiteSpace(dependency))
        {
            output.DependsOnAsset(AssetPath.Parse(dependency));
        }
        output.SetAsset(new DependencyAsset());
        return output.WriteArtifactAsync("runtime", context.sourceBytes, cancellationToken: cancellationToken);
    }
}
