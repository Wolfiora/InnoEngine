using Inno.Core.Diagnostics;
using System;
using System.Collections.Generic;

namespace Inno.Rendering;

internal static partial class RenderGraphValidator
{
    private static HashSet<int> FindLivePasses(
        IReadOnlyList<RenderTextureRecord> textures,
        IReadOnlyList<RenderBufferRecord> buffers,
        IReadOnlyList<RenderPassRecord> passes,
        IReadOnlySet<RenderResourceKey> outputs,
        RenderGraphValidationWorkspace analysis
    ) {
        Stack<int> pending = analysis.pending;
        HashSet<int> live = analysis.live;
        pending.Clear();
        live.Clear();
        for (int passIndex = 0; passIndex < passes.Count; passIndex++)
        {
            RenderPassRecord pass = passes[passIndex];
            bool root = pass.hasSideEffect;
            foreach (RenderResourceUse use in pass.resources)
            {
                bool writes = use.access is RenderResourceAccess.Write or RenderResourceAccess.ReadWrite;
                if (writes && (outputs.Contains(use.key) || IsImported(use.key, textures, buffers)))
                {
                    root = true;
                    break;
                }
            }

            if (root)
            {
                pending.Push(passIndex);
            }
        }

        while (pending.TryPop(out int passIndex))
        {
            if (!live.Add(passIndex))
            {
                continue;
            }

            foreach (int dependency in analysis.dataDependencies[passIndex])
            {
                pending.Push(dependency);
            }
        }

        return live;
    }

    private static List<int>? TopologicalSort(
        int passCount,
        IReadOnlySet<int> livePasses,
        RenderGraphValidationWorkspace analysis
    ) {
        List<int> inDegrees = analysis.inDegrees;
        List<List<int>> dependants = analysis.dependants;
        while (inDegrees.Count < passCount)
        {
            inDegrees.Add(0);
            dependants.Add([]);
        }
        for (int i = 0; i < passCount; i++)
        {
            inDegrees[i] = 0;
            dependants[i].Clear();
        }

        foreach (int passIndex in livePasses)
        {
            foreach (int dependency in analysis.dependencies[passIndex])
            {
                if (!livePasses.Contains(dependency))
                {
                    continue;
                }

                inDegrees[passIndex]++;
                dependants[dependency].Add(passIndex);
            }
        }

        PriorityQueue<int, int> ready = analysis.ready;
        ready.Clear();
        foreach (int passIndex in livePasses)
        {
            if (inDegrees[passIndex] == 0)
            {
                ready.Enqueue(passIndex, passIndex);
            }
        }

        List<int> result = analysis.schedule;
        result.Clear();
        while (ready.Count != 0)
        {
            int passIndex = ready.Dequeue();
            result.Add(passIndex);
            foreach (int dependant in dependants[passIndex])
            {
                inDegrees[dependant]--;
                if (inDegrees[dependant] == 0)
                {
                    ready.Enqueue(dependant, dependant);
                }
            }
        }

        return result.Count == livePasses.Count ? result : null;
    }
}
