using Inno.Core.Serialization;

namespace Inno.Assets.Pipeline.Tests;

internal sealed class ClipContribution : ISerializable
{
    [SerializableProperty]
    public TextAsset? clip { get; set; }
}
