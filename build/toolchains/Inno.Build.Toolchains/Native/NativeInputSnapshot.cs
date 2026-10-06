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

internal sealed class NativeInputReadCache
{
    private readonly NativeBuildInputState m_state;
    private readonly Dictionary<string, IReadOnlyList<(string relative, string physical)>> m_roots;
    private readonly Dictionary<string, byte[]> m_hashes;

    internal NativeInputReadCache(NativeBuildInputState state)
    {
        m_state = state;
        m_roots = new(pathComparer);
        m_hashes = new(pathComparer);
    }

    internal StringComparer pathComparer => OperatingSystem.IsWindows() ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;

    internal byte[] GetFrozenHash(string path)
        => m_hashes.TryGetValue(path, out byte[]? hash) ? hash
            : throw new InvalidOperationException($"Native input '{path}' was not frozen by this operation's recipe.");

    internal IReadOnlyList<(string relative, string physical)> Enumerate(
        string path,
        CancellationToken cancellationToken
    ) {
        if (m_roots.TryGetValue(path, out IReadOnlyList<(string relative, string physical)>? cached))
            return cached;
        List<(string relative, string physical)> files = [];
        if (File.Exists(path))
            files.Add((string.Empty, path));
        else if (Directory.Exists(path))
            EnumerateDirectory(path, string.Empty, files, new HashSet<string>(pathComparer), cancellationToken);
        else
            throw new FileNotFoundException("A declared native build input is unavailable.", path);
        cached = files.AsReadOnly();
        m_roots.Add(path, cached);
        return cached;
    }

    internal byte[] Hash(
        string path,
        CancellationToken cancellationToken
    ) {
        if (m_hashes.TryGetValue(path, out byte[]? hash))
            return hash;
        using FileStream input = File.OpenRead(path);
        using IncrementalHash digest = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
        byte[] buffer = ArrayPool<byte>.Shared.Rent(64 * 1024);
        long bytes = 0;
        try
        {
            int length;
            while ((length = input.Read(buffer)) != 0)
            {
                cancellationToken.ThrowIfCancellationRequested();
                digest.AppendData(buffer.AsSpan(0, length));
                bytes += length;
            }
            hash = digest.GetHashAndReset();
            m_hashes.Add(path, hash);
            return hash;
        }
        finally
        {
            m_state.RecordHash(bytes);
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    private void EnumerateDirectory(
        string directory,
        string relativeRoot,
        List<(string relative, string physical)> files,
        HashSet<string> ancestors,
        CancellationToken cancellationToken
    ) {
        cancellationToken.ThrowIfCancellationRequested();
        DirectoryInfo info = new(directory);
        string physicalRoot = info.ResolveLinkTarget(returnFinalTarget: true)?.FullName ?? info.FullName;
        if (!ancestors.Add(physicalRoot))
            throw new IOException($"A native input contains a directory link cycle at '{directory}'.");
        try
        {
            foreach (string file in Directory.EnumerateFiles(physicalRoot))
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (Path.GetFileName(file) != ".git")
                    files.Add((relativeRoot + Path.GetFileName(file), Path.Combine(directory, Path.GetFileName(file))));
            }
            foreach (string child in Directory.EnumerateDirectories(physicalRoot))
            {
                string name = Path.GetFileName(child);
                if (name is ".git" or ".build" or "bin" or "obj")
                    continue;
                EnumerateDirectory(Path.Combine(directory, name), relativeRoot + name + "/", files, ancestors, cancellationToken);
            }
        }
        finally
        {
            ancestors.Remove(physicalRoot);
        }
    }
}
