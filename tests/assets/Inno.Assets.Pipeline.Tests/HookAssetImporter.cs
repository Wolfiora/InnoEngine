using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Assets.Pipeline.Tests;

[AssetImporter("inno.tests.hook")]
internal sealed class HookAssetImporter : AssetImporter<HookAsset>
{
    public override IReadOnlyList<string> supportedExtensions { get; } = [".hookasset"];

    protected override ValueTask ImportAsync(
        AssetImportContext context,
        AssetImportWriter<HookAsset> output,
        CancellationToken cancellationToken
    ) {
        output.SetAsset(new HookAsset());
        return output.WriteArtifactAsync("runtime", context.sourceBytes, cancellationToken);
    }
}
