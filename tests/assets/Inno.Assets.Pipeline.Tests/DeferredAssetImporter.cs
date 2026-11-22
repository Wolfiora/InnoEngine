using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Assets.Pipeline.Tests;

[AssetImporter("inno.tests.deferred")]
internal sealed class DeferredAssetImporter : AssetImporter<DeferredAsset>
{
    private static readonly IReadOnlyList<string> EXTENSIONS = [".deferredasset"];

    public override IReadOnlyList<string> supportedExtensions => isAvailable ? EXTENSIONS : [];

    internal static bool isAvailable { get; set; } = true;

    protected override ValueTask ImportAsync(
        AssetImportContext context,
        AssetImportWriter<DeferredAsset> output,
        CancellationToken cancellationToken
    ) {
        output.SetAsset(new DeferredAsset());
        return output.WriteArtifactAsync("runtime", context.sourceBytes, cancellationToken: cancellationToken);
    }
}
