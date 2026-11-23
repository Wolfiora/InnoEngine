using Inno.Core.Serialization;

namespace Inno.Assets.Pipeline;

internal struct AssetArtifactOutputData
{
    [SerializableProperty]
    internal string name { get; set; }

    [SerializableProperty]
    internal string fileName { get; set; }

    [SerializableProperty]
    internal string contentHash { get; set; }

    [SerializableProperty]
    internal long length { get; set; }

    [SerializableProperty]
    internal AssetDeploymentScope deploymentScope { get; set; }
}
