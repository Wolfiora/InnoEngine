using Inno.Core.Serialization;
using Inno.Extensibility.Types;

namespace Inno.Assets.Pipeline.Tests;

[StableTypeID("8f87f452-203e-4cd4-9c91-c93af4802141")]
internal sealed class TestBuildDefinitionAsset : AssetObject
{
    [SerializableProperty]
    internal string label { get; set; } = string.Empty;
}
