using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.Scene;

namespace Inno.Editor.PlayMode.Native.Tests;

[StableTypeID("942e794b-1c53-45a6-80e6-a641d76fdd09")]
[AllowMultipleComponent]
internal sealed class DirtyCaptureComponent : GameComponent
{
    [SerializableProperty]
    public int number;

    [SerializableProperty]
    public GameObject? target;

    [SerializableProperty]
    public DirtyCaptureValue state { get; set; } = new();
}
