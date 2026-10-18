using Inno.Core.Diagnostics;
using System;
using System.Collections.Generic;

namespace Inno.Rendering;

internal static partial class RenderGraphCompiler
{
    private static int[] AllocateTextureSlots(
        IReadOnlyList<RenderTextureRecord> textures,
        IReadOnlyList<RenderPassRecord> passes,
        IReadOnlyDictionary<int, int> schedulePositions
    )
        => AllocateSlots(
            textures.Count,
            index => textures[index].imported,
            (
                left,
                right
            ) => textures[left].descriptor.Equals(textures[right].descriptor),
            new RenderResourceKey(true, 0),
            passes,
            schedulePositions);

    private static int[] AllocateBufferSlots(
        IReadOnlyList<RenderBufferRecord> buffers,
        IReadOnlyList<RenderPassRecord> passes,
        IReadOnlyDictionary<int, int> schedulePositions
    )
        => AllocateSlots(
            buffers.Count,
            index => buffers[index].imported,
            (
                left,
                right
            ) => buffers[left].descriptor.Equals(buffers[right].descriptor),
            new RenderResourceKey(false, 0),
            passes,
            schedulePositions);

    private static int[] AllocateSlots(
        int resourceCount,
        Func<int, bool> isImported,
        Func<int, int, bool> descriptorsEqual,
        RenderResourceKey keyTemplate,
        IReadOnlyList<RenderPassRecord> passes,
        IReadOnlyDictionary<int, int> schedulePositions
    ) {
        int[] firstUses = new int[resourceCount];
        int[] lastUses = new int[resourceCount];
        int[] slots = new int[resourceCount];
        Array.Fill(firstUses, int.MaxValue);
        Array.Fill(lastUses, -1);
        Array.Fill(slots, -1);

        foreach ((int passIndex, int position) in schedulePositions)
        {
            foreach (RenderResourceUse use in passes[passIndex].resources)
            {
                if (use.key.isTexture != keyTemplate.isTexture)
                {
                    continue;
                }

                firstUses[use.key.index] = Math.Min(firstUses[use.key.index], position);
                lastUses[use.key.index] = Math.Max(lastUses[use.key.index], position);
            }
        }

        List<int> resources = [];
        for (int index = 0; index < resourceCount; index++)
        {
            if (!isImported(index) && lastUses[index] >= 0)
            {
                resources.Add(index);
            }
        }

        resources.Sort((
            left,
            right
        ) => firstUses[left].CompareTo(firstUses[right]));
        List<(int representative, int lastUse)> allocations = [];
        foreach (int resource in resources)
        {
            int selected = -1;
            for (int slot = 0; slot < allocations.Count; slot++)
            {
                (int representative, int lastUse) allocation = allocations[slot];
                if (allocation.lastUse < firstUses[resource]
                    && descriptorsEqual(allocation.representative, resource))
                {
                    selected = slot;
                    allocations[slot] = (allocation.representative, lastUses[resource]);
                    break;
                }
            }

            if (selected < 0)
            {
                selected = allocations.Count;
                allocations.Add((resource, lastUses[resource]));
            }

            slots[resource] = selected;
        }

        return slots;
    }
}
