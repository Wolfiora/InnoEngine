using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Assets.Pipeline.Tests;

[AssetImporter("tests.extension-consumer")]
internal sealed class ExtensionConsumerImporter : AssetImporter<ExtensionDependentAsset>
{
    public override IReadOnlyList<string> supportedExtensions { get; } = [".extensiondependent"];

    protected override ValueTask ImportAsync(
        AssetImportContext context,
        AssetImportWriter<ExtensionDependentAsset> output,
        CancellationToken cancellationToken
    ) {
        string source = context.ReadUtf8Text();
        if (Guid.TryParse(source, out Guid id))
        {
            _ = context.references.Resolve(
                id,
                context.services.GetStableTypeID<ExtensionDependentAsset>(),
                "surface.extensionasset",
                typeof(ExtensionDependentAsset),
                "$.shader"
            );
        }
        else
        {
            _ = context.ResolveDependency<ExtensionDependentAsset>(AssetPath.Project(source));
        }
        output.SetAsset(new ExtensionDependentAsset());
        return output.WriteArtifactAsync("runtime", context.sourceBytes, cancellationToken);
    }
}
