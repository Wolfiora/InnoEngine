using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.IO;

namespace Inno.Build.Toolchains;

internal sealed class NativeInputReadCache
{
    private readonly NativeBuildInputState m_state;
    private readonly string m_phase;
    private readonly Dictionary<string, IReadOnlyList<(string relative, string physical)>> m_roots;
    private readonly Dictionary<string, byte[]> m_hashes;
    private readonly PhysicalPathResolver m_physicalPaths;
    private readonly Dictionary<string, string> m_readingPaths;

    internal NativeInputReadCache(
        NativeBuildInputState state,
        string phase = "initial"
    ) {
        m_state = state;
        m_phase = phase;
        m_roots = new(pathComparer);
        m_hashes = new(pathComparer);
        m_physicalPaths = new(pathComparer);
        m_readingPaths = new(pathComparer);
    }

    internal StringComparer pathComparer => OperatingSystem.IsWindows() ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;

    internal byte[] GetFrozenHash(string path)
        => m_hashes.TryGetValue(m_physicalPaths.Resolve(path), out byte[]? hash) ? hash
            : throw new InvalidOperationException($"Native input '{path}' was not frozen by this operation's recipe.");

    internal NativeInputSnapshot CaptureRecordedInputs(string engineRoot)
        => new(m_readingPaths.Select(entry => new NativeInputSnapshot.Entry(
            NativeBuildInput.FromPath(engineRoot, entry.Key).logicalPath, entry.Key, m_hashes[entry.Value])));

    internal string[][] CaptureRootMembership()
        => m_roots.OrderBy(static pair => pair.Key, pathComparer)
            .Select(pair => new[] { pair.Key }.Concat(pair.Value.Select(static entry => entry.relative).Order(StringComparer.Ordinal)).ToArray()).ToArray();

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

    internal byte[][] Hash(
        IReadOnlyList<string> paths,
        CancellationToken cancellationToken
    ) {
        var physicalPaths = new string[paths.Count];
        var pending = new List<string>();
        var unique = new HashSet<string>(pathComparer);
        for (int index = 0; index < paths.Count; index++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string readingPath = Path.GetFullPath(paths[index]);
            string physical = m_physicalPaths.Resolve(readingPath);
            m_readingPaths[readingPath] = physical;
            physicalPaths[index] = physical;
            if (m_hashes.ContainsKey(physical) || !unique.Add(physical))
                m_state.RecordReuse(m_phase);
            else
                pending.Add(physical);
        }
        var hashes = new byte[pending.Count][];
        var bytes = new long[pending.Count];
        var opened = new bool[pending.Count];
        ExceptionDispatchInfo? failure = null;
        try
        {
            if (pending.Count == 1)
                Read(0);
            else if (pending.Count > 1)
                Parallel.For(0, pending.Count, new ParallelOptions
                {
                    MaxDegreeOfParallelism = Math.Min(Environment.ProcessorCount, 4),
                    CancellationToken = cancellationToken
                }, (
                    index,
                    loop
                ) => {
                    try
                    {
                        Read(index);
                    }
                    catch (Exception exception)
                    {
                        Interlocked.CompareExchange(ref failure, ExceptionDispatchInfo.Capture(exception), null);
                        loop.Stop();
                    }
                });
            failure?.Throw();
            cancellationToken.ThrowIfCancellationRequested();
        }
        finally
        {
            // Initial capture holds the state gate. Workers only read bytes; record after all workers drain.
            for (int index = 0; index < pending.Count; index++)
                if (opened[index])
                    m_state.RecordHash(bytes[index], m_phase);
        }
        // Publish this batch only after every read succeeds; failed/canceled batches never become frozen inputs.
        for (int index = 0; index < pending.Count; index++)
            m_hashes.Add(pending[index], hashes[index]);
        return physicalPaths.Select(path => m_hashes[path]).ToArray();

        void Read(int index)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using FileStream input = File.OpenRead(pending[index]);
            opened[index] = true;
            using IncrementalHash digest = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
            byte[] buffer = ArrayPool<byte>.Shared.Rent(64 * 1024);
            try
            {
                int length;
                while ((length = input.Read(buffer)) != 0)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    digest.AppendData(buffer.AsSpan(0, length));
                    bytes[index] += length;
                }
                hashes[index] = digest.GetHashAndReset();
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
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
        string physicalRoot = m_physicalPaths.Resolve(directory);
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
                if (name is ".git" or ".build" or "bin" or "obj" or "__pycache__")
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
