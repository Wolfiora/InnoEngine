using System;
using System.Linq;
using Inno.Core.Serialization;

namespace Inno.Scene;

/// <summary>
/// Stores sparse semantic operations for one GameTag setting contribution.
/// </summary>
internal sealed class GameTagSettingContribution : ISerializable
{
    [SerializableProperty]
    internal string[] additions { get; set; } = [];

    [SerializableProperty]
    internal string[] removals { get; set; } = [];

    internal void Validate()
    {
        if (additions is null || removals is null)
        {
            throw new InvalidOperationException("GameTag contribution arrays cannot be null.");
        }
        string[] normalizedAdditions = additions.Select(GameTagCatalog.Normalize).Order(StringComparer.Ordinal).ToArray();
        string[] normalizedRemovals = removals.Select(GameTagCatalog.Normalize).Order(StringComparer.Ordinal).ToArray();
        if (!normalizedAdditions.SequenceEqual(additions, StringComparer.Ordinal)
            || normalizedAdditions.Distinct(StringComparer.Ordinal).Count() != normalizedAdditions.Length
        ) {
            throw new InvalidOperationException("GameTag additions must be unique, normalized, and ordered.");
        }
        if (!normalizedRemovals.SequenceEqual(removals, StringComparer.Ordinal)
            || normalizedRemovals.Distinct(StringComparer.Ordinal).Count() != normalizedRemovals.Length
        ) {
            throw new InvalidOperationException("GameTag removals must be unique, normalized, and ordered.");
        }
        if (normalizedAdditions.Intersect(normalizedRemovals, StringComparer.Ordinal).Any())
        {
            throw new InvalidOperationException("A GameTag contribution cannot add and remove the same tag.");
        }
    }
}
