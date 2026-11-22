using System;
using System.Collections.Generic;
using Inno.Core.Serialization;
using Inno.Core.Settings;

namespace Inno.Scene.Layers;

/// <summary>
/// Stores sparse semantic operations for one GameLayer setting contribution.
/// </summary>
internal sealed class GameLayerSettingContribution : ISerializable
{
    [SerializableProperty]
    internal int[] removedSlots { get; set; } = [];

    [SerializableProperty]
    internal string[] removedLocalIds { get; set; } = [];

    [SerializableProperty]
    internal int[] upsertSlots { get; set; } = [];

    [SerializableProperty]
    internal string[] upsertLocalIds { get; set; } = [];

    [SerializableProperty]
    internal string[] upsertNames { get; set; } = [];

    [SerializableProperty]
    internal int[] interactionFirstSlots { get; set; } = [];

    [SerializableProperty]
    internal int[] interactionSecondSlots { get; set; } = [];

    [SerializableProperty]
    internal bool[] interactionValues { get; set; } = [];

    internal void Validate()
    {
        if (removedSlots is null
            || removedLocalIds is null
            || upsertSlots is null
            || upsertLocalIds is null
            || upsertNames is null
            || interactionFirstSlots is null
            || interactionSecondSlots is null
            || interactionValues is null
        ) {
            throw new InvalidOperationException("GameLayer contribution arrays cannot be null.");
        }
        if (removedSlots.Length != removedLocalIds.Length)
        {
            throw new InvalidOperationException("GameLayer removal contribution arrays must have equal lengths.");
        }
        if (upsertSlots.Length != upsertLocalIds.Length || upsertSlots.Length != upsertNames.Length)
        {
            throw new InvalidOperationException("GameLayer upsert contribution arrays must have equal lengths.");
        }
        if (interactionFirstSlots.Length != interactionSecondSlots.Length
            || interactionFirstSlots.Length != interactionValues.Length
        ) {
            throw new InvalidOperationException("GameLayer interaction contribution arrays must have equal lengths.");
        }

        var slots = new HashSet<int>();
        for (int index = 0; index < removedSlots.Length; index++)
        {
            _ = new GameLayer(removedSlots[index]);
            _ = new ProjectLocalID(removedLocalIds[index]);
            if (!slots.Add(removedSlots[index]))
            {
                throw new InvalidOperationException($"GameLayer slot {removedSlots[index]} is removed more than once.");
            }
        }
        slots.Clear();
        for (int index = 0; index < upsertSlots.Length; index++)
        {
            _ = new GameLayer(upsertSlots[index]);
            _ = new ProjectLocalID(upsertLocalIds[index]);
            _ = GameLayerCatalog.NormalizeName(upsertNames[index]);
            if (!slots.Add(upsertSlots[index]))
            {
                throw new InvalidOperationException($"GameLayer slot {upsertSlots[index]} is upserted more than once.");
            }
        }
        var pairs = new HashSet<(int first, int second)>();
        for (int index = 0; index < interactionFirstSlots.Length; index++)
        {
            int first = interactionFirstSlots[index];
            int second = interactionSecondSlots[index];
            _ = new GameLayer(first);
            _ = new GameLayer(second);
            if (first > second)
            {
                throw new InvalidOperationException("GameLayer interaction pairs must use canonical slot order.");
            }
            if (!pairs.Add((first, second)))
            {
                throw new InvalidOperationException(
                    $"GameLayer interaction ({first}, {second}) is contributed more than once."
                );
            }
        }
    }
}
