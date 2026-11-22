using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.Scene.Layers;

namespace Inno.Scene.Tests;

[StableTypeID("821c7b92-9aeb-40cb-924e-169004199ef0")]
internal sealed class LayerValueComponent : GameComponent
{
    [SerializableProperty]
    public GameLayer layer { get; set; }

    [SerializableProperty]
    public GameLayerMask mask { get; set; }
}
