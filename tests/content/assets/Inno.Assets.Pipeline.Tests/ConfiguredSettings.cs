using Inno.Core.Serialization;
using Inno.Extensibility.Types;

namespace Inno.Assets.Pipeline.Tests;

[StableTypeID("f58eaeea-4bd5-4f3a-a7c0-c5a5414a6bb4")]
internal sealed class ConfiguredSettings : ISerializable
{
    [SerializableProperty]
    internal int multiplier { get; set; } = 1;

    [SerializableProperty]
    internal TextAsset? lookup { get; set; }
}
