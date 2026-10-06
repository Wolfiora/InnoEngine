using Inno.Core.Diagnostics;
using System;
using System.Collections.Generic;

namespace Inno.Rendering;

internal static partial class RenderGraphValidator
{
    private static void BuildResourceDependencies(
        IReadOnlyList<RenderTextureRecord> textures,
        IReadOnlyList<RenderBufferRecord> buffers,
        IReadOnlyList<RenderPassRecord> passes,
        IReadOnlySet<RenderResourceKey> outputs,
        IReadOnlyList<HashSet<int>> dependencies,
        IReadOnlyList<HashSet<int>> dataDependencies,
        List<RenderGraphDiagnostic> diagnostics
    ) {
        Dictionary<RenderResourceKey, int> lastWriters = [];
        Dictionary<RenderResourceKey, List<int>> readers = [];
        HashSet<RenderResourceKey> initialized = [];
        for (int textureIndex = 0; textureIndex < textures.Count; textureIndex++)
        {
            if (textures[textureIndex].imported)
            {
                initialized.Add(new RenderResourceKey(true, textureIndex));
            }
        }

        for (int bufferIndex = 0; bufferIndex < buffers.Count; bufferIndex++)
        {
            if (buffers[bufferIndex].imported)
            {
                initialized.Add(new RenderResourceKey(false, bufferIndex));
            }
        }

        for (int passIndex = 0; passIndex < passes.Count; passIndex++)
        {
            RenderPassRecord pass = passes[passIndex];
            IReadOnlyList<RenderResourceUse> uniqueUses = [.. UniqueUses(pass.resources)];
            foreach (RenderResourceUse use in uniqueUses)
            {
                bool reads = use.access is RenderResourceAccess.Read or RenderResourceAccess.ReadWrite;
                bool writes = use.access is RenderResourceAccess.Write or RenderResourceAccess.ReadWrite;

                if (reads && !initialized.Contains(use.key))
                {
                    diagnostics.Add(new RenderGraphDiagnostic(
                        "RENDER_GRAPH_UNINITIALIZED_READ",
                        $"Pass '{passes[passIndex].name}' reads '{GetResourceName(use.key, textures, buffers)}' before it is initialized.",
                        DiagnosticSeverity.Error,
                        passes[passIndex].name,
                        GetResourceName(use.key, textures, buffers)));
                }

                if (lastWriters.TryGetValue(use.key, out int writer))
                {
                    AddDependency(writer, passIndex, dependencies, dataDependencies);
                }

                if (writes)
                {
                    if (readers.TryGetValue(use.key, out List<int>? previousReaders))
                    {
                        foreach (int reader in previousReaders)
                        {
                            AddDependency(reader, passIndex, dependencies, dataDependencies);
                        }

                        previousReaders.Clear();
                    }

                    lastWriters[use.key] = passIndex;
                }
                else
                {
                    if (!readers.TryGetValue(use.key, out List<int>? resourceReaders))
                    {
                        resourceReaders = [];
                        readers.Add(use.key, resourceReaders);
                    }

                    resourceReaders.Add(passIndex);
                }
            }

            foreach (RenderResourceUse use in uniqueUses)
            {
                bool writes = use.access is RenderResourceAccess.Write or RenderResourceAccess.ReadWrite;
                if (!writes)
                {
                    continue;
                }

                if (StoresResult(pass, use.key))
                {
                    initialized.Add(use.key);
                }
                else
                {
                    initialized.Remove(use.key);
                }
            }
        }

        foreach (RenderResourceKey output in outputs)
        {
            if (!initialized.Contains(output))
            {
                string resourceName = GetResourceName(output, textures, buffers);
                diagnostics.Add(new RenderGraphDiagnostic(
                    "RENDER_GRAPH_OUTPUT_UNINITIALIZED",
                    $"Graph output '{resourceName}' has no stored contents at the end of the graph.",
                    DiagnosticSeverity.Error,
                    resourceName: resourceName));
            }
        }
    }

    private static bool StoresResult(
        RenderPassRecord pass,
        RenderResourceKey key
    ) {
        foreach (RenderAttachment attachment in pass.attachments)
        {
            if (key.isTexture
                && attachment.texture.index == key.index
                && attachment.storeAction == RenderStoreAction.Discard)
            {
                return false;
            }
        }

        return true;
    }

    private static void BuildPhaseDependencies(
        IReadOnlyList<RenderPassRecord> passes,
        IReadOnlyList<HashSet<int>> dependencies
    ) {
        for (int passIndex = 0; passIndex < passes.Count; passIndex++)
        {
            RenderPassRecord pass = passes[passIndex];
            for (int targetIndex = 0; targetIndex < passes.Count; targetIndex++)
            {
                if (passIndex == targetIndex)
                {
                    continue;
                }

                RenderPassRecord target = passes[targetIndex];
                if (pass.before.Contains(target.phase))
                {
                    dependencies[targetIndex].Add(passIndex);
                }

                if (pass.after.Contains(target.phase))
                {
                    dependencies[passIndex].Add(targetIndex);
                }
            }
        }
    }

    private static IEnumerable<RenderResourceUse> UniqueUses(IReadOnlyList<RenderResourceUse> uses)
    {
        HashSet<RenderResourceKey> emitted = [];
        foreach (RenderResourceUse use in uses)
        {
            if (emitted.Add(use.key))
            {
                yield return use;
            }
        }
    }

    private static bool IsImported(
        RenderResourceKey key,
        IReadOnlyList<RenderTextureRecord> textures,
        IReadOnlyList<RenderBufferRecord> buffers
    )
        => key.isTexture ? textures[key.index].imported : buffers[key.index].imported;

    private static string GetResourceName(
        RenderResourceKey key,
        IReadOnlyList<RenderTextureRecord> textures,
        IReadOnlyList<RenderBufferRecord> buffers
    )
        => key.isTexture ? textures[key.index].name : buffers[key.index].name;

    private static List<HashSet<int>> CreateEdgeSets(int count)
    {
        List<HashSet<int>> result = new(count);
        for (int i = 0; i < count; i++)
        {
            result.Add([]);
        }

        return result;
    }

    private static void AddDependency(
        int dependency,
        int dependant,
        IReadOnlyList<HashSet<int>> dependencies,
        IReadOnlyList<HashSet<int>> dataDependencies
    ) {
        if (dependency == dependant)
        {
            return;
        }

        dependencies[dependant].Add(dependency);
        dataDependencies[dependant].Add(dependency);
    }
}
