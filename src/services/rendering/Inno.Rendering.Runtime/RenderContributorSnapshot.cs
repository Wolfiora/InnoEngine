using System;
using System.Collections.Generic;
using Inno.Rendering;

namespace Inno.Rendering.Runtime;

internal sealed class RenderContributorSnapshot
{
    internal static readonly RenderContributorSnapshot empty = new([]);

    internal RenderContributorSnapshot(IReadOnlyList<IRenderFrameGraphContributor> contributors)
    {
        var entries = new Entry[contributors.Count];
        for (int index = 0; index < entries.Length; index++)
        {
            IRenderFrameGraphContributor contributor = contributors[index];
            entries[index] = new Entry(contributor, $"Contributor[{index}] {contributor.GetType().Name}");
        }
        this.entries = Array.AsReadOnly(entries);
    }

    internal IReadOnlyList<Entry> entries { get; }

    internal readonly record struct Entry(
        IRenderFrameGraphContributor contributor,
        string scopeName
    );
}
