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
            TaskRuntimePreparation.RuntimeFile[] files = FreezeFiles();
            string fingerprint = TaskRuntimePreparation.Fingerprint(files);
            string destination = Path.Combine(cache, fingerprint);
            bool prepared = BuildEngine is IBuildEngine4 engine
                && TaskRuntimeBuildScope.Read(engine, "publication:" + cache + "/" + fingerprint, verifyOutputs: false)
                    is ITaskItem[] publication
                && publication.Length == files.Length
                && publication.Zip(files).All(static pair => pair.First.ItemSpec == pair.Second.source
                    && pair.First.GetMetadata("RelativePath") == pair.Second.relative
                    && pair.First.GetMetadata("FileHash") == pair.Second.hash);
            if (!prepared)
            {
                using FileLease ownership = FileLease.AcquireAsync(destination + ".lock",
                    Timeout.InfiniteTimeSpan, cancellation).AsTask().GetAwaiter().GetResult();
                TaskRuntimePreparation.ValidateSources(files, cancellation);
                if (!TaskRuntimePreparation.IsComplete(Path.Combine(destination, "Runtime"), files, cancellation))
                    TaskRuntimePreparation.PublishCandidate(destination, files, cancellation);
                TaskRuntimePreparation.ValidateSources(files, cancellation);
            }
            PublishedAssembly = Path.Combine(destination, "Runtime", "Inno.Build.Tasks.dll");
            RegisterReader(load, PublishedAssembly);
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

    private TaskRuntimePreparation.RuntimeFile[] FreezeFiles() => TaskRuntimePreparation.FreezeFiles(InputFiles);
    private static string RequireAbsolute(string path) => TaskRuntimePreparation.RequireAbsolute(path);
    private static void RegisterReader(
        string load,
        string assembly
    ) => TaskRuntimePreparation.RegisterReader(load, assembly);
    private static void RegisterProcessOwner(string directory) => TaskRuntimePreparation.RegisterProcessOwner(directory);
}
