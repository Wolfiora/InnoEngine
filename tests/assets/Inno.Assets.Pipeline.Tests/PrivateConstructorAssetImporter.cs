using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Assets.Pipeline.Tests;

[AssetImporter("inno.tests.private-constructor")]
internal sealed class PrivateConstructorAssetImporter : AssetImporter<PrivateConstructorAsset>
{
    private static readonly IReadOnlyList<string> EXTENSIONS = [".privateasset"];

    private PrivateConstructorAssetImporter() {}

    public override IReadOnlyList<string> supportedExtensions => EXTENSIONS;

    protected override ValueTask ImportAsync(
        AssetImportContext context,
        AssetImportWriter<PrivateConstructorAsset> output,
        CancellationToken cancellationToken
    ) {
        output.SetAsset(new PrivateConstructorAsset { value = context.ReadUtf8Text() });
        return output.WriteArtifactAsync("runtime", context.sourceBytes, cancellationToken: cancellationToken);
    }
}
