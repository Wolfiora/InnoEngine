using System.Collections.Generic;

namespace Inno.Scene.Tests;

internal sealed class PrefabInstantiationBehavior : GameBehavior
{
    public PrefabAsset? prefab { get; set; }

    public List<GameObject> instances { get; } = [];

    protected override void Update()
    {
        if (prefab is not null && instances.Count < 2)
        {
            instances.Add(gameObject.scene.InstantiatePrefab(prefab, gameObject.transform));
        }
    }
}
