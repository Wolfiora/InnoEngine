using System;
using System.Collections.Generic;
using Inno.Core.Serialization;
using Inno.Core.Settings;

namespace Inno.Scene.Layers;

/// <summary>
/// Composes sparse project-local layer and interaction contributions into one compact runtime stack.
/// </summary>
[ProjectSettingComposer("inno.scene.layers")]
internal sealed class GameLayerSettingsComposer : ProjectSettingComposer<GameLayerCatalog, GameLayerSettingContribution>
{
    /// <summary>
    /// Captures project-local layer and interaction changes relative to the baseline stack.
    /// </summary>
    /// <param name="baseline">
    /// The baseline stack before the contributor's changes.
    /// </param>
    /// <param name="value">
    /// The authored stack whose changes are captured.
    /// </param>
    /// <returns>
    /// A sparse contribution containing only values changed from the baseline.
    /// </returns>
    protected override GameLayerSettingContribution CaptureContribution(
        GameLayerCatalog baseline,
        GameLayerCatalog value
    ) {
        List<int> removedSlots = [];
        List<string> removedLocalIDs = [];
        List<int> upsertSlots = [];
        List<string> upsertLocalIDs = [];
        List<string> upsertNames = [];
        for (int index = 0; index < GameLayer.C_MAX_COUNT; index++)
        {
            var layer = new GameLayer(index);
            ProjectLocalID? baselineID = baseline.GetLocalID(layer);
            ProjectLocalID? valueID = value.GetLocalID(layer);
            string? baselineName = baseline.GetName(layer);
            string? valueName = value.GetName(layer);
            if (baselineID is ProjectLocalID oldID && oldID != valueID)
            {
                removedSlots.Add(index);
                removedLocalIDs.Add(oldID.value);
            }
            if (valueID is ProjectLocalID newID
                && (baselineID != valueID
                    || !string.Equals(baselineName, valueName, StringComparison.Ordinal))
            ) {
                upsertSlots.Add(index);
                upsertLocalIDs.Add(newID.value);
                upsertNames.Add(valueName!);
            }
        }

        List<int> interactionFirstSlots = [];
        List<int> interactionSecondSlots = [];
        List<bool> interactionValues = [];
        for (int first = 0; first < GameLayer.C_MAX_COUNT; first++)
        {
            for (int second = first; second < GameLayer.C_MAX_COUNT; second++)
            {
                var firstLayer = new GameLayer(first);
                var secondLayer = new GameLayer(second);
                bool baselineValue = baseline.CanInteract(firstLayer, secondLayer);
                bool authoredValue = value.CanInteract(firstLayer, secondLayer);
                if (baselineValue == authoredValue)
                {
                    continue;
                }
                interactionFirstSlots.Add(first);
                interactionSecondSlots.Add(second);
                interactionValues.Add(authoredValue);
            }
        }

        return new GameLayerSettingContribution
        {
            removedSlots = removedSlots.ToArray(),
            removedLocalIds = removedLocalIDs.ToArray(),
            upsertSlots = upsertSlots.ToArray(),
            upsertLocalIds = upsertLocalIDs.ToArray(),
            upsertNames = upsertNames.ToArray(),
            interactionFirstSlots = interactionFirstSlots.ToArray(),
            interactionSecondSlots = interactionSecondSlots.ToArray(),
            interactionValues = interactionValues.ToArray()
        };
    }

    /// <summary>
    /// Determines whether a layer contribution contains no authored changes.
    /// </summary>
    /// <param name="contribution">
    /// The validated contribution to inspect.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the contribution contains no layer or interaction changes; otherwise, <see langword="false"/>.
    /// </returns>
    protected override bool IsEmpty(GameLayerSettingContribution contribution)
    {
        contribution.Validate();
        return contribution.removedSlots.Length == 0
               && contribution.upsertSlots.Length == 0
               && contribution.interactionFirstSlots.Length == 0;
    }

    /// <summary>
    /// Applies ordered project-local layer contributions to the target stack.
    /// </summary>
    /// <param name="target">
    /// The mutable stack that receives the composed result.
    /// </param>
    /// <param name="contributions">
    /// The ordered, validated contributions to apply.
    /// </param>
    protected override void Compose(
        GameLayerCatalog target,
        IReadOnlyList<ProjectSettingContribution<GameLayerSettingContribution>> contributions
    ) {
        var slotOwners = new string?[GameLayer.C_MAX_COUNT];
        var localIDOwners = new Dictionary<ProjectLocalID, string>();
        var interactionOwners = new string[GameLayer.C_MAX_COUNT, GameLayer.C_MAX_COUNT];
        for (int index = 0; index < GameLayer.C_MAX_COUNT; index++)
        {
            var layer = new GameLayer(index);
            if (target.GetLocalID(layer) is ProjectLocalID localID)
            {
                slotOwners[index] = "host";
                localIDOwners.Add(localID, "host");
            }
            for (int second = 0; second < GameLayer.C_MAX_COUNT; second++)
            {
                interactionOwners[index, second] = "host";
            }
        }

        foreach (ProjectSettingContribution<GameLayerSettingContribution> contribution in contributions)
        {
            contribution.value.Validate();
            ApplyRemovals(target, contribution, slotOwners, localIDOwners);
            ApplyUpserts(target, contribution, slotOwners, localIDOwners);
            ApplyInteractions(target, contribution, interactionOwners);
        }
    }

    private static void ApplyRemovals(
        GameLayerCatalog target,
        ProjectSettingContribution<GameLayerSettingContribution> contribution,
        string?[] slotOwners,
        Dictionary<ProjectLocalID, string> localIDOwners
    ) {
        for (int index = 0; index < contribution.value.removedSlots.Length; index++)
        {
            var layer = new GameLayer(contribution.value.removedSlots[index]);
            var expected = new ProjectLocalID(contribution.value.removedLocalIds[index]);
            ProjectLocalID? current = target.GetLocalID(layer);
            if (current is null)
            {
                continue;
            }
            if (current != expected)
            {
                throw Conflict(
                    contribution.context,
                    $"cannot remove layer '{expected}' from slot {layer.index} because it contains '{current}'."
                );
            }
            string owner = slotOwners[layer.index] ?? "host";
            if (!contribution.context.CanOverride(owner))
            {
                throw Conflict(contribution.context, $"cannot remove layer '{expected}' owned by '{owner}'.");
            }
            _ = target.Remove(layer);
            slotOwners[layer.index] = null;
            localIDOwners.Remove(expected);
        }
    }

    private static void ApplyUpserts(
        GameLayerCatalog target,
        ProjectSettingContribution<GameLayerSettingContribution> contribution,
        string?[] slotOwners,
        Dictionary<ProjectLocalID, string> localIDOwners
    ) {
        for (int index = 0; index < contribution.value.upsertSlots.Length; index++)
        {
            var layer = new GameLayer(contribution.value.upsertSlots[index]);
            var localID = new ProjectLocalID(contribution.value.upsertLocalIds[index]);
            string name = contribution.value.upsertNames[index];
            ProjectLocalID? currentID = target.GetLocalID(layer);
            string? currentName = target.GetName(layer);
            if (currentID == localID && string.Equals(currentName, name, StringComparison.Ordinal))
            {
                continue;
            }

            if (target.TryGetLayer(localID, out GameLayer existingLayer) && existingLayer != layer)
            {
                string existingOwner = slotOwners[existingLayer.index] ?? localIDOwners[localID];
                if (!contribution.context.CanOverride(existingOwner))
                {
                    throw Conflict(
                        contribution.context,
                        $"cannot move layer '{localID}' from slot {existingLayer.index}; it is owned by '{existingOwner}'."
                    );
                }
                _ = target.Remove(existingLayer);
                slotOwners[existingLayer.index] = null;
                localIDOwners.Remove(localID);
            }

            if (currentID is ProjectLocalID replacedID)
            {
                string owner = slotOwners[layer.index] ?? localIDOwners[replacedID];
                if (!contribution.context.CanOverride(owner))
                {
                    throw Conflict(
                        contribution.context,
                        $"cannot replace slot {layer.index} layer '{replacedID}' owned by '{owner}'."
                    );
                }
                localIDOwners.Remove(replacedID);
            }
            target.DefineLocal(layer, localID, name);
            slotOwners[layer.index] = contribution.context.contributorID;
            localIDOwners[localID] = contribution.context.contributorID;
        }
    }

    private static void ApplyInteractions(
        GameLayerCatalog target,
        ProjectSettingContribution<GameLayerSettingContribution> contribution,
        string[,] interactionOwners
    ) {
        for (int index = 0; index < contribution.value.interactionFirstSlots.Length; index++)
        {
            var first = new GameLayer(contribution.value.interactionFirstSlots[index]);
            var second = new GameLayer(contribution.value.interactionSecondSlots[index]);
            bool value = contribution.value.interactionValues[index];
            if (target.CanInteract(first, second) == value)
            {
                continue;
            }
            string owner = interactionOwners[first.index, second.index];
            if (!contribution.context.CanOverride(owner))
            {
                throw Conflict(
                    contribution.context,
                    $"cannot replace interaction ({first.index}, {second.index}) owned by '{owner}'."
                );
            }
            target.SetInteraction(first, second, value);
            interactionOwners[first.index, second.index] = contribution.context.contributorID;
            interactionOwners[second.index, first.index] = contribution.context.contributorID;
        }
    }

    private static InvalidOperationException Conflict(
        ProjectSettingContributionContext context,
        string message
    ) => new($"GameLayer contribution '{context.contributorID}' {message}");
}

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
