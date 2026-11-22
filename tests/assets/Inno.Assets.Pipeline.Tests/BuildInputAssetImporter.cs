using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Assets.Pipeline.Tests;

[AssetImporter("inno.tests.build-input")]
internal sealed class BuildInputAssetImporter : AssetImporter<DependencyAsset>
{
    public override IReadOnlyList<string> supportedExtensions { get; } = [".buildinput"];
    public override AssetDeploymentScope deploymentScope => AssetDeploymentScope.AuthoringOnly;

    protected override ValueTask ImportAsync(
        AssetImportContext context,
        AssetImportWriter<DependencyAsset> output,
        CancellationToken cancellationToken
    ) {
        string text = context.ReadUtf8Text();
        if (text == "failed")
        {
            throw new InvalidDataException("Required authoring input failed.");
        }
        if (Guid.TryParse(text, out Guid dependency))
        {
            context.DependsOnArtifact(dependency);
        }
        output.SetAsset(new DependencyAsset());
        return output.WriteArtifactAsync("authoring", context.sourceBytes, cancellationToken, AssetDeploymentScope.AuthoringOnly);
    }
}
