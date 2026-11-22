using Inno.Core.Serialization;
using Inno.Extensibility.Types;

namespace Inno.Scene.Tests;

[StableTypeID("b4ada657-2b58-4929-b1c0-616b54bb0939")]
internal sealed class EngineObjectReferenceComponent : GameComponent
{
    [SerializableProperty]
    public GameObject? targetObject { get; set; }

    [SerializableProperty]
    public int value { get; set; }
}
