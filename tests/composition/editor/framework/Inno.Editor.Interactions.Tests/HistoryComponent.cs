using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.Scene;

namespace Inno.Editor.Interactions.Tests;

[StableTypeID("267e77d8-1112-4cf9-a9f1-01d9a1e59bbc")]
[AllowMultipleComponent]
internal sealed class HistoryComponent : GameBehavior
{
    [SerializableProperty]
    public int value { get; set; }

    protected override void Reset() => value = 7;
}
