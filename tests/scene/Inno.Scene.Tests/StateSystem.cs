using Inno.Core.Serialization;
using Inno.Extensibility.Types;

namespace Inno.Scene.Tests;

[StableTypeID("b469170b-a1f2-4176-b3ad-252d1cb53b90")]
internal sealed class StateSystem : GameSystem
{
    [SerializableProperty]
    public int value { get; set; }

    [SerializableProperty]
    public SystemReferenceComponent? component { get; set; }

    public int resetCount { get; private set; }

    protected override void Reset() => resetCount++;
}
