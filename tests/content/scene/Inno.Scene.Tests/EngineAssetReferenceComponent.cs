using Inno.Assets;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;

namespace Inno.Scene.Tests;

[StableTypeID("cc867fe9-866c-4a19-ac69-5e561b20a874")]
internal sealed class EngineAssetReferenceComponent : GameComponent
{
    [SerializableProperty]
    public TextAsset? asset { get; set; }
}
