using Inno.Assets;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.Scene;

namespace Inno.Editor.PlayMode.Tests;

[StableTypeID("9afec1fd-c1ef-45fa-9bcc-0b3e2877d613")]
internal sealed class PlayModeAssetReferenceBehavior : GameBehavior
{
    /// <summary>
    /// Gets or sets the asset reference copied into the isolated Play Mode scene.
    /// </summary>
    [SerializableProperty]
    public TextAsset? asset { get; set; }
}
