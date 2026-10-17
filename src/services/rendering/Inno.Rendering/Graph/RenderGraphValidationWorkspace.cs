using System.Collections.Generic;

namespace Inno.Rendering;

// Frame-local analysis of the accepted immutable prefix. No callbacks or extension instances are retained.
internal sealed class RenderGraphValidationWorkspace
{
    internal int textureCount;
    internal int bufferCount;
    internal int passCount;
    internal readonly Dictionary<RenderResourceKey, int> lastWriters = [];
    internal readonly Dictionary<RenderResourceKey, List<int>> readers = [];
    internal readonly HashSet<RenderResourceKey> initialized = [];
    internal readonly List<HashSet<int>> dependencies = [];
    internal readonly List<HashSet<int>> dataDependencies = [];
    internal readonly List<RenderGraphDiagnostic> diagnostics = [];
    internal readonly HashSet<RenderResourceKey> uniqueUses = [];
    internal readonly HashSet<int> live = [];
    internal readonly Stack<int> pending = [];
    internal readonly List<int> inDegrees = [];
    internal readonly List<List<int>> dependants = [];
    internal readonly PriorityQueue<int, int> ready = new();
    internal readonly List<int> schedule = [];

    internal void Reset()
    {
        textureCount = bufferCount = passCount = 0;
        lastWriters.Clear();
        readers.Clear();
        initialized.Clear();
        dependencies.Clear();
        dataDependencies.Clear();
        diagnostics.Clear();
        uniqueUses.Clear();
        live.Clear();
        pending.Clear();
        inDegrees.Clear();
        dependants.Clear();
        ready.Clear();
        schedule.Clear();
    }
}
