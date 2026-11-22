using Inno.Core.Serialization;
using Inno.Extensibility.Types;

namespace Inno.Assets.Pipeline.Tests;

[StableTypeID("490def27-4cdd-48a0-b324-9b7438600404")]
internal sealed class SlowAsset : AssetObject
{
    [SerializableProperty]
    internal string value { get; set; } = string.Empty;
}
