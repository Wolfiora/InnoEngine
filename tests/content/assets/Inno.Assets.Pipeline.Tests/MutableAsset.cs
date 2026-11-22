using Inno.Core.Serialization;
using Inno.Extensibility.Types;

namespace Inno.Assets.Pipeline.Tests;

[StableTypeID("fa17dc36-b9cc-4187-b5da-72906ad00505")]
internal sealed class MutableAsset : AssetObject
{
    [SerializableProperty]
    internal string value { get; set; } = string.Empty;
}
