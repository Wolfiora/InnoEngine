using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.Scene;

namespace Inno.Editor.Interactions.Tests;

[StableTypeID("b269c970-8afe-46a0-a4a0-f44f737d059a")]
internal sealed class HistoryReferenceComponent : GameComponent
{
    [SerializableProperty]
    public GameObject? targetObject { get; set; }

    [SerializableProperty]
    public GameComponent? targetComponent { get; set; }
}
