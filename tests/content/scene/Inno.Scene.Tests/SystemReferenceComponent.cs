using Inno.Core.Serialization;
using Inno.Extensibility.Types;

namespace Inno.Scene.Tests;

[StableTypeID("75344398-ed18-46ae-808b-e0cb45d93850")]
internal sealed class SystemReferenceComponent : GameComponent
{
    [SerializableProperty]
    public StateSystem? system { get; set; }
}
