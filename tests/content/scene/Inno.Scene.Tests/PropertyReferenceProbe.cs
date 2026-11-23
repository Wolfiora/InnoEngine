using System;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;

namespace Inno.Scene.Tests;

[StableTypeID("486290eb-6cc4-4a96-918a-c8022f51ad85")]
internal sealed class PropertyReferenceProbe : GameComponent
{
    [SerializableProperty(order = -1)]
    public int trigger
    {
        get
        {
            beforeRead?.Invoke();
            return 0;
        }
        set {}
    }

    [SerializableProperty]
    public GameObject? targetObject { get; set; }

    [SerializableProperty]
    public GameComponent? targetComponent { get; set; }

    [SerializableProperty]
    public GameSystem? targetSystem { get; set; }

    internal Action? beforeRead { get; set; }
}
