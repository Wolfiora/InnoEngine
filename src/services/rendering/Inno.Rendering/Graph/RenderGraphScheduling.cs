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
        IReadOnlyList<HashSet<int>> dataDependencies
    ) {
        Stack<int> pending = [];
        HashSet<int> live = [];
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

            foreach (int dependency in dataDependencies[passIndex])
            {
                pending.Push(dependency);
            }
        }

        return live;
    }

    private static List<int>? TopologicalSort(
        int passCount,
        IReadOnlySet<int> livePasses,
        IReadOnlyList<HashSet<int>> dependencies
    ) {
        int[] inDegrees = new int[passCount];
        List<List<int>> dependants = new(passCount);
        for (int i = 0; i < passCount; i++)
        {
            dependants.Add([]);
        }

        foreach (int passIndex in livePasses)
        {
            foreach (int dependency in dependencies[passIndex])
            {
                if (!livePasses.Contains(dependency))
                {
                    continue;
                }

                inDegrees[passIndex]++;
                dependants[dependency].Add(passIndex);
            }
        }

        SortedSet<int> ready = [];
        foreach (int passIndex in livePasses)
        {
            if (inDegrees[passIndex] == 0)
            {
                ready.Add(passIndex);
            }
        }

        List<int> result = [];
        while (ready.Count != 0)
        {
            int passIndex = ready.Min;
            ready.Remove(passIndex);
            result.Add(passIndex);
            foreach (int dependant in dependants[passIndex])
            {
                inDegrees[dependant]--;
                if (inDegrees[dependant] == 0)
                {
                    ready.Add(dependant);
                }
            }
        }

        return result.Count == livePasses.Count ? result : null;
    }
}
