using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
        Dictionary<string, Entry> entries = new(StringComparer.Ordinal);
        foreach (NativeBuildInput input in inputs)
        {
            cancellationToken.ThrowIfCancellationRequested();
            foreach ((string relative, string physical) in cache.Enumerate(input.physicalPath, cancellationToken))
            {
                string logical = relative.Length == 0 ? input.logicalPath : input.logicalPath + "/" + relative;
                if (entries.TryGetValue(logical, out Entry? previous))
                {
                    if (!cache.pathComparer.Equals(previous.physicalPath, physical))
                        throw new ArgumentException($"Native input identity '{logical}' maps to different reading locations.");
                    continue;
                }
                entries.Add(logical, new Entry(logical, physical, cache.Hash(physical, cancellationToken)));
            }
        }
        return new NativeInputSnapshot(entries.Values);
    }
}
