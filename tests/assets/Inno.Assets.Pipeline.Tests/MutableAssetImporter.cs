using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Assets.Pipeline.Tests;

[AssetImporter("inno.tests.mutable")]
internal sealed class MutableAssetImporter : AssetImporter<MutableAsset>
{
    internal static int attempts;
    public override IReadOnlyList<string> supportedExtensions { get; } = [".mutableasset"];

    protected override ValueTask ImportAsync(
        AssetImportContext context,
        AssetImportWriter<MutableAsset> output,
        CancellationToken cancellationToken
    ) {
        attempts++;
        string value = context.ReadUtf8Text();
        if (value == "!invalid!")
        {
            throw new InvalidDataException("The mutable asset source is invalid.");
        }
        output.SetAsset(new MutableAsset { value = value });
        return output.WriteArtifactAsync("runtime", context.sourceBytes, cancellationToken);
    }

    protected override ValueTask<ReadOnlyMemory<byte>?> ExportAsync(
        AssetExportContext context,
        MutableAsset asset,
        CancellationToken cancellationToken
    ) => ValueTask.FromResult<ReadOnlyMemory<byte>?>(Encoding.UTF8.GetBytes(asset.value));
}
