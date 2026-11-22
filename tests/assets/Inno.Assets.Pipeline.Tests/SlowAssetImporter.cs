using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Assets.Pipeline.Tests;

[AssetImporter("inno.tests.slow")]
internal sealed class SlowAssetImporter : AssetImporter<SlowAsset>
{
    internal static readonly ManualResetEventSlim IMPORT_STARTED = new(false);
    internal static readonly ManualResetEventSlim ALLOW_IMPORT = new(false);
    internal static int importCount;

    public override IReadOnlyList<string> supportedExtensions { get; } = [".slowasset"];

    protected override ValueTask ImportAsync(
        AssetImportContext context,
        AssetImportWriter<SlowAsset> output,
        CancellationToken cancellationToken
    ) {
        Interlocked.Increment(ref importCount);
        IMPORT_STARTED.Set();
        if (!ALLOW_IMPORT.Wait(TimeSpan.FromSeconds(5)))
        {
            throw new TimeoutException("The slow importer test gate was not released.");
        }
        output.SetAsset(new SlowAsset { value = context.ReadUtf8Text() });
        return output.WriteArtifactAsync("runtime", context.sourceBytes, cancellationToken);
    }

    internal static void Reset()
    {
        importCount = 0;
        IMPORT_STARTED.Reset();
        ALLOW_IMPORT.Reset();
    }
}
