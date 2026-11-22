using Inno.Assets;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.Scene;

namespace Inno.Editor.Interactions.Tests;

[StableTypeID("edbd3940-a94c-499e-8f03-b0e232a39b21")]
[AllowMultipleComponent]
internal sealed class HistoryAssetReferenceComponent : GameComponent
{
    [SerializableProperty]
    public TextAsset? asset { get; set; }
}
