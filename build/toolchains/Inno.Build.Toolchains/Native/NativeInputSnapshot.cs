using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Inno.Build.Toolchains;

internal sealed class NativeInputSnapshot
{
    internal sealed record Entry(
        string logicalPath,
        string physicalPath,
        byte[] hash
    );

    internal NativeInputSnapshot(IEnumerable<Entry> entries)
        => this.entries = Array.AsReadOnly(entries.OrderBy(static entry => entry.logicalPath, StringComparer.Ordinal).ToArray());

    internal IReadOnlyList<Entry> entries { get; }

    internal static NativeInputSnapshot Capture(
        IEnumerable<NativeBuildInput> inputs,
        NativeInputReadCache cache,
        CancellationToken cancellationToken
    ) {
        Dictionary<string, string> paths = new(StringComparer.Ordinal);
        foreach (NativeBuildInput input in inputs)
        {
            cancellationToken.ThrowIfCancellationRequested();
            foreach ((string relative, string physical) in cache.Enumerate(input.physicalPath, cancellationToken))
            {
                string logical = relative.Length == 0 ? input.logicalPath : input.logicalPath + "/" + relative;
                if (paths.TryGetValue(logical, out string? previous))
                {
                    if (!cache.pathComparer.Equals(previous, physical))
                        throw new ArgumentException($"Native input identity '{logical}' maps to different reading locations.");
                    continue;
                }
                paths.Add(logical, physical);
            }
        }
        var ordered = paths.OrderBy(static pair => pair.Key, StringComparer.Ordinal).ToArray();
        byte[][] hashes = cache.Hash(ordered.Select(static pair => pair.Value).ToArray(), cancellationToken);
        return new NativeInputSnapshot(ordered.Select((
            pair,
            index
        ) => new Entry(pair.Key, pair.Value, hashes[index])));
    }
}
