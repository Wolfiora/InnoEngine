using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.Scene;

namespace Inno.Editor.Interactions.Tests;

[StableTypeID("f00d5950-1136-4393-b246-644b69948f90")]
[AllowMultipleSystem]
internal sealed class HistorySystem : GameSystem
{
    [SerializableProperty]
    public int value { get; set; }

    protected override void Reset() => value = 11;
}
