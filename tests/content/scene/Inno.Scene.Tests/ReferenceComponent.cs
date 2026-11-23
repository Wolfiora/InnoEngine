using Inno.Core.Serialization;
using Inno.Extensibility.Types;

namespace Inno.Scene.Tests;

[StableTypeID("09d35197-7161-4fdf-8d60-95ecf3555c0c")]
internal sealed class ReferenceComponent : GameComponent
{
    [SerializableProperty]
    public int value { get; set; }

    [SerializableProperty]
    public GameObject? targetObject { get; set; }

    [SerializableProperty]
    public ReferenceComponent? targetComponent { get; set; }

    public int resetCount { get; private set; }

    protected override void Reset() => resetCount++;
}
