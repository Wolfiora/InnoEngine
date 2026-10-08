using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Inno.Core.IO;
using Microsoft.Build.Framework;

namespace Inno.Build.TaskHosting;

/// <summary>
/// Publishes one immutable task runtime per content identity before a build loads its private closure.
/// This bootstrap depends only on filesystem primitives and never loads the task runtime being built.
/// </summary>
public sealed partial class PublishTaskHostTask : Microsoft.Build.Utilities.Task, ICancelableTask
{
    private readonly object m_cancellationGate = new();
    private readonly CancellationTokenSource m_cancellation = new();
    private bool m_finished;

    /// <summary>
    /// Gets or sets the frozen runtime files, each declaring RelativePath and FileHash metadata.
    /// </summary>
    [Required]
    public ITaskItem[] InputFiles { get; set; } = [];

    /// <summary>
    /// Gets or sets the absolute owner of content-addressed, shared task runtime snapshots.
    /// </summary>
    [Required]
    public string CacheDirectory { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the absolute private load directory whose parent receives this process's ownership marker.
    /// </summary>
    [Required]
    public string LoadDirectory { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets an optional private bootstrap directory to register under the same process ownership.
    /// An empty value leaves the caller's publisher assembly lifetime outside this cache.
    /// </summary>
    public string PublisherDirectory { get; set; } = string.Empty;

    /// <summary>
    /// Gets the verified immutable task assembly path after successful publication.
    /// </summary>
    [Output]
    public string PublishedAssembly { get; private set; } = string.Empty;

    /// <summary>
    /// Verifies sources and cached files, publishes a complete candidate under exclusive ownership,
    /// and registers the private reader before returning its shared runtime.
    /// </summary>
    /// <returns>
    /// True when a complete runtime was published or reused; false with a logged error otherwise.
    /// </returns>
    public override bool Execute()
    {
        try
        {
            CancellationToken cancellation = m_cancellation.Token;
            cancellation.ThrowIfCancellationRequested();
            string cache = RequireAbsolute(CacheDirectory);
            string load = RequireAbsolute(LoadDirectory);
            string owner = Path.GetDirectoryName(cache)!;
            string loads = Path.Combine(owner, "loads");
            PathBoundary.RequireUnlinkedPath(owner, cache);
            PathBoundary.RequireUnlinkedPath(loads, load);
            if (PublisherDirectory.Length > 0)
            {
                string publisher = RequireAbsolute(PublisherDirectory);
                PathBoundary.RequireUnlinkedPath(Path.Combine(owner, "publishers"), publisher);
                RegisterProcessOwner(publisher);
            }
            RuntimeFile[] files = FreezeFiles();
            string fingerprint = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(
                string.Join('\n', files.Select(static file => file.relative + "|" + file.hash)))));
            string destination = Path.Combine(cache, fingerprint);
            using (FileLease ownership = FileLease.AcquireAsync(destination + ".lock",
                Timeout.InfiniteTimeSpan, cancellation).AsTask().GetAwaiter().GetResult())
            {
                ValidateSources(files, cancellation);
                if (!IsComplete(Path.Combine(destination, "Runtime"), files, cancellation))
                    PublishCandidate(destination, files, cancellation);
                ValidateSources(files, cancellation);
                PublishedAssembly = Path.Combine(destination, "Runtime", "Inno.Build.Tasks.dll");
                RegisterReader(load, PublishedAssembly);
            }
            RetireReaders(owner, destination);
            Log.LogMessage(MessageImportance.Normal, "INNO-TASK-HOST verified {0}", fingerprint);
            return true;
        }
        catch (Exception exception)
        {
            Log.LogErrorFromException(exception, showStackTrace: true);
            return false;
        }
        finally
        {
            lock (m_cancellationGate)
            {
                m_finished = true;
                m_cancellation.Dispose();
            }
        }
    }

    /// <summary>
    /// Cancels ownership waits and unpublished copies without racing token retirement.
    /// </summary>
    public void Cancel()
    {
        lock (m_cancellationGate)
            if (!m_finished)
                m_cancellation.Cancel();
    }

    private RuntimeFile[] FreezeFiles()
    {
        if (InputFiles.Length == 0)
            throw new InvalidDataException("The task runtime closure is empty.");
        var paths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        RuntimeFile[] files = InputFiles.Select(item =>
        {
            string relative = item.GetMetadata("RelativePath").Replace('\\', '/');
            string hash = item.GetMetadata("FileHash").ToUpperInvariant();
            if (relative.Length == 0 || relative.Split('/').Any(static segment => segment is "" or "." or "..")
                || relative.Contains(':') || !paths.Add(relative)
                || hash.Length != 64 || hash.Any(static character => !Uri.IsHexDigit(character)))
                throw new InvalidDataException("A runtime input has an invalid path, hash or duplicate identity.");
            return new RuntimeFile(RequireAbsolute(item.ItemSpec), relative, hash);
        }).OrderBy(static file => file.relative, StringComparer.Ordinal).ToArray();
        if (!paths.Contains("Inno.Build.Tasks.dll"))
            throw new InvalidDataException("The task runtime closure has no task assembly.");
        return files;
    }

    private static string RequireAbsolute(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        if (!Path.IsPathFullyQualified(path))
            throw new ArgumentException("Task host locations must be absolute.", nameof(path));
        return Path.GetFullPath(path);
    }

    private static void ValidateSources(
        IReadOnlyList<RuntimeFile> files,
        CancellationToken cancellation
    ) {
        foreach (RuntimeFile file in files)
        {
            cancellation.ThrowIfCancellationRequested();
            if (Hash(file.source) != file.hash)
                throw new InvalidDataException($"Task runtime input changed after selection: {file.source}");
        }
    }

    private static bool IsComplete(
        string runtime,
        IReadOnlyList<RuntimeFile> files,
        CancellationToken cancellation
    ) {
        if (!Directory.Exists(runtime))
            return false;
        string[] actual = PathBoundary.EnumerateFiles(runtime)
            .Select(path => Path.GetRelativePath(runtime, path).Replace('\\', '/')).ToArray();
        if (!actual.ToHashSet(StringComparer.Ordinal).SetEquals(files.Select(static file => file.relative)))
            return false;
        foreach (RuntimeFile file in files)
        {
            cancellation.ThrowIfCancellationRequested();
            if (Hash(PathBoundary.Resolve(runtime, file.relative)) != file.hash)
                return false;
        }
        return true;
    }

    private static void PublishCandidate(
        string destination,
        IReadOnlyList<RuntimeFile> files,
        CancellationToken cancellation
    ) {
        string staging = destination + ".staging-" + Guid.NewGuid().ToString("N");
        string runtime = Path.Combine(staging, "Runtime");
        try
        {
            foreach (RuntimeFile file in files)
            {
                cancellation.ThrowIfCancellationRequested();
                string output = PathBoundary.Resolve(runtime, file.relative);
                Directory.CreateDirectory(Path.GetDirectoryName(output)!);
                File.Copy(file.source, output);
            }
            if (!IsComplete(runtime, files, cancellation))
                throw new InvalidDataException("The copied task runtime did not match its selected closure.");
            ValidateSources(files, cancellation);
            cancellation.ThrowIfCancellationRequested();
            AtomicDirectory.Install(staging, destination);
        }
        finally
        {
            if (Directory.Exists(staging))
                Directory.Delete(staging, recursive: true);
        }
    }

    private static string Hash(string path)
    {
        using FileStream input = File.OpenRead(path);
        return Convert.ToHexString(SHA256.HashData(input));
    }

    private static void RegisterReader(
        string load,
        string assembly
    ) {
        RegisterProcessOwner(load);
        Directory.CreateDirectory(load);
        AtomicFile.WriteAllBytes(Path.Combine(load, "source-host.txt"), Encoding.UTF8.GetBytes(assembly));
    }

    private static void RegisterProcessOwner(string directory)
    {
        string operation = Path.GetDirectoryName(directory)!;
        string owners = Path.Combine(operation, "owners");
        Directory.CreateDirectory(owners);
        string process = Environment.ProcessId.ToString(System.Globalization.CultureInfo.InvariantCulture);
        AtomicFile.WriteAllBytes(Path.Combine(owners, process + ".pid"), Encoding.UTF8.GetBytes(process));
    }

    private sealed record RuntimeFile(
        string source,
        string relative,
        string hash
    );
}
