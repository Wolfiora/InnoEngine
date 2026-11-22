using System;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.Scene;

namespace Inno.Editor.Interactions.Tests;

[StableTypeID("8503e97b-1845-4ab2-a116-1ece04934a4b")]
[AllowMultipleComponent]
internal sealed class SubtreeRestoreFailureComponent : GameBehavior
{
    internal static bool throwOnDestroy;

    [SerializableProperty]
    public int value { get; set; } = 5;

    protected override void OnDestroy()
    {
        if (throwOnDestroy)
        {
            throw new InvalidOperationException("Injected subtree destruction callback failure.");
        }
    }

    [OnSerializableRestored]
    private void OnRestored() => throw new InvalidOperationException("Injected subtree restore callback failure.");
}
