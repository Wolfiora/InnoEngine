using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Assets.Pipeline.Tests;

[AssetImporter("tests.extension-dependent")]
internal sealed class ExtensionDependentImporter : AssetImporter<ExtensionDependentAsset>
{
    internal static bool available;
    internal static int attempts;
    public override IReadOnlyList<string> supportedExtensions { get; } = [".extensionasset"];

    protected override ValueTask ImportAsync(
        AssetImportContext context,
        AssetImportWriter<ExtensionDependentAsset> output,
        CancellationToken cancellationToken
    ) {
        attempts++;
        if (!available)
        {
            throw new AssetImportExtensionUnavailableException("tests.target", "tests.surface");
        }
        output.SetAsset(new ExtensionDependentAsset());
        return output.WriteArtifactAsync("runtime", context.sourceBytes, cancellationToken: cancellationToken);
    }
}
