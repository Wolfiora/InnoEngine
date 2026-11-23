using System.Collections.Generic;

namespace Inno.Assets.Pipeline;

internal readonly struct WatcherPollResult(
    IReadOnlyList<AssetChangedEvent> changes,
    bool requiresFullRescan
) {
    internal IReadOnlyList<AssetChangedEvent> changes { get; } = changes ?? [];
    internal bool requiresFullRescan { get; } = requiresFullRescan;
}
