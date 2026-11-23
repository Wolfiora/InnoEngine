using Inno.Core.Serialization;

namespace Inno.Assets.Pipeline;

internal sealed class AssetArtifactManifest : ISerializable
{
    [SerializableProperty]
    internal string key { get; set; } = string.Empty;

    [SerializableProperty]
    internal AssetArtifactOutputData[] outputs { get; set; } = [];
}
