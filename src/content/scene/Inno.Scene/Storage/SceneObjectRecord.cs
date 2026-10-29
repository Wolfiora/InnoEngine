using System.Collections.Generic;

namespace Inno.Scene;

/// <summary>
/// Stores one scene object's attachment and commit state.
/// </summary>
internal sealed class SceneObjectRecord
{
    internal SceneObjectRecord(
        SceneStore store,
        GameObject gameObject
    ) {
        this.store = store;
        this.gameObject = gameObject;
    }

    internal SceneStore store { get; }
    internal GameObject gameObject { get; }
    internal List<GameComponent> components { get; } = [];
    internal bool isAlive { get; set; } = true;
    internal bool isCommitted { get; set; }
}
