using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.Serialization;

namespace Inno.Assets.Pipeline.Tests;

[AssetImporter("tests.configured")]
internal sealed class ConfiguredImporter : AssetImporter<ConfiguredAsset>
{
    public override IReadOnlyList<string> supportedExtensions { get; } = [".configured"];

    public override ISerializable CreateImportSettings() => new ConfiguredSettings();

    protected override ValueTask ImportAsync(
        AssetImportContext context,
        AssetImportWriter<ConfiguredAsset> output,
        CancellationToken cancellationToken
    ) {
        ConfiguredSettings settings = (ConfiguredSettings)context.importSettings!;
        if (settings.multiplier < 0) throw new InvalidOperationException("A negative multiplier is invalid.");
        string value = $"{settings.multiplier}:{context.ReadUtf8Text()}";
        if (settings.lookup is not null) value += ":" + settings.lookup.content;
        output.SetAsset(new ConfiguredAsset { value = value });
        return output.WriteArtifactAsync("runtime", Encoding.UTF8.GetBytes(value), cancellationToken);
    }
}
