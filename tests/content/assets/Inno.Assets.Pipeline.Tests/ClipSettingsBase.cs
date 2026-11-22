using Inno.Core.Serialization;

namespace Inno.Assets.Pipeline.Tests;

internal abstract class ClipSettingsBase : ISerializable
{
    [SerializableProperty]
    public TextAsset? clip { get; set; }

    [SerializableProperty]
    public string[] labels { get; set; } = [];
}
