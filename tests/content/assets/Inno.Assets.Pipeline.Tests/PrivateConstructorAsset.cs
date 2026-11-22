using Inno.Core.Serialization;
using Inno.Extensibility.Types;

namespace Inno.Assets.Pipeline.Tests;

[StableTypeID("501988d0-c069-4dcf-97f6-e899b24e5801")]
internal sealed class PrivateConstructorAsset : AssetObject
{
    [SerializableProperty]
    internal string value { get; set; } = string.Empty;
}
