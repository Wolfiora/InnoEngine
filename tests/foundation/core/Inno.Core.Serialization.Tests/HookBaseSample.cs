using System.Collections.Generic;

namespace Inno.Core.Serialization.Tests;

// Open: HookDerivedSample derives from it to inherit its serialization hooks.
internal class HookBaseSample : ISerializable
{
    [SerializableProperty]
    public int value { get; set; }

    public List<string> calls { get; } = [];

    [OnSerializableRestored]
    private void AfterBaseRestore() => calls.Add("base");
}
