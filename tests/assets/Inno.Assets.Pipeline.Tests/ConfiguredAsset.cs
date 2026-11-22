using Inno.Core.Serialization;
using Inno.Extensibility.Types;

namespace Inno.Assets.Pipeline.Tests;

[StableTypeID("bdb3db71-e9cb-4d46-839b-3cbe302c5555")]
internal sealed class ConfiguredAsset : AssetObject
{
    [SerializableProperty]
    internal string value { get; set; } = string.Empty;
}
