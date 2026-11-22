using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Assets.Pipeline.Tests;

[AssetImporter("inno.tests.mixed-scope")]
internal sealed class MixedScopeAssetImporter : AssetImporter<DependencyAsset>
{
    public override IReadOnlyList<string> supportedExtensions { get; } = [".mixedscope"];

    protected override ValueTask ImportAsync(
        AssetImportContext context,
        AssetImportWriter<DependencyAsset> output,
        CancellationToken cancellationToken
    ) {
        output.SetAsset(new DependencyAsset());
        if (context.ReadUtf8Text() == "authoring")
        {
            output.SetDeploymentScope(AssetDeploymentScope.AuthoringOnly);
            return output.WriteArtifactAsync(
                "graph",
                context.sourceBytes,
                cancellationToken,
                AssetDeploymentScope.AuthoringOnly
            );
        }
        return output.WriteArtifactAsync("runtime", context.sourceBytes, cancellationToken);
    }
}
