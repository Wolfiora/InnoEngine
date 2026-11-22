using System;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.Scene;

namespace Inno.Editor.Interactions.Tests;

[StableTypeID("71cc27da-2c47-47f2-a037-c9bc29615358")]
[AllowMultipleComponent]
internal sealed class RestoreCleanupFailureComponent : GameBehavior
{
    [SerializableProperty]
    public int value { get; set; } = 5;

    [OnSerializableRestored]
    private void OnRestored() => throw new InvalidOperationException("Injected restore callback failure.");
}
