using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Assets.Pipeline.Tests;

[AssetImporter("inno.tests.build-consumer")]
internal sealed class BuildConsumerAssetImporter : AssetImporter<DependencyAsset>
{
    public override IReadOnlyList<string> supportedExtensions { get; } = [".buildconsumer"];

    protected override ValueTask ImportAsync(
        AssetImportContext context,
        AssetImportWriter<DependencyAsset> output,
        CancellationToken cancellationToken
    ) {
        context.DependsOnArtifact(Guid.Parse(context.ReadUtf8Text()));
        output.SetAsset(new DependencyAsset());
        return output.WriteArtifactAsync("runtime", context.sourceBytes, cancellationToken: cancellationToken);
    }
}
