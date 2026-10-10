using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Inno.Content;

/// <summary>
/// Freezes the complete payload set and rejects ambiguous keys before publication.
/// </summary>
public sealed class ContentPackIndex
{
    /// <summary>
    /// Gets the reserved archive entry that carries this index and never indexes itself.
    /// </summary>
    public const string C_INDEX_KEY = "Content.index";

    private readonly Dictionary<ContentKey, ContentEntry> m_byKey;

    /// <summary>
    /// Copies and sorts a complete payload inventory.
    /// </summary>
    /// <param name="entries">
    /// Immutable payload descriptions; ownership of the input enumeration remains with the caller.
    /// </param>
    /// <exception cref="ArgumentException">
    /// An entry is null, repeats a folded key, or uses the reserved index identity.
    /// </exception>
    public ContentPackIndex(IEnumerable<ContentEntry> entries)
    {
        ArgumentNullException.ThrowIfNull(entries);
        m_byKey = new();
        HashSet<string> folded = new(StringComparer.OrdinalIgnoreCase) { C_INDEX_KEY };
        List<ContentEntry> ordered = [];
        foreach (ContentEntry entry in entries)
        {
            ArgumentNullException.ThrowIfNull(entry);
            if (!folded.Add(entry.key.value!))
                throw new ArgumentException($"Content repeats a reserved or ambiguous key '{entry.key}'.", nameof(entries));
            m_byKey.Add(entry.key, entry);
            ordered.Add(entry);
            totalLength = checked(totalLength + entry.length);
        }
        ordered.Sort(static (
            left,
            right
        ) => StringComparer.Ordinal.Compare(left.key.value, right.key.value));
        this.entries = new ReadOnlyCollection<ContentEntry>(ordered);
        ValidateTree(ordered);
    }

    /// <summary>
    /// Gets the immutable inventory in deterministic logical-key order.
    /// </summary>
    public IReadOnlyList<ContentEntry> entries { get; }

    /// <summary>
    /// Gets the checked sum of all decoded payload lengths, excluding the index.
    /// </summary>
    public long totalLength { get; }

    /// <summary>
    /// Finds the metadata for an exact assigned key.
    /// </summary>
    /// <param name="key">
    /// The case-sensitive logical identity.
    /// </param>
    /// <param name="entry">
    /// The immutable metadata when present; otherwise null.
    /// </param>
    /// <returns>
    /// True only when the exact key is present.
    /// </returns>
    public bool TryGetEntry(
        ContentKey key,
        out ContentEntry? entry
    ) => m_byKey.TryGetValue(key, out entry);

    private static void ValidateTree(List<ContentEntry> entries)
    {
        HashSet<string> files = entries.Select(static entry => entry.key.value!)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        foreach (ContentEntry entry in entries)
        {
            string key = entry.key.value!;
            for (int separator = key.IndexOf('/'); separator >= 0; separator = key.IndexOf('/', separator + 1))
            {
                if (files.Contains(key[..separator]))
                    throw new ArgumentException($"Content uses '{key[..separator]}' as both a file and directory.", nameof(entries));
            }
        }
    }
}
