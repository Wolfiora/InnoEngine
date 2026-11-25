using System.Collections.Generic;

namespace Inno.Core.Serialization.Tests;

internal sealed class SnapshotEnvelope : ISerializable
{
    internal bool omitState;
    internal bool fail;
    internal bool duplicate;
    internal int writeCalls;
    internal long observedTypeGeneration;

    internal Dictionary<string, int> fields { get; } = [];
    internal List<int> items { get; } = [];
    internal DefaultSample state { get; } = new();
}
