using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Inno.Core.IO;
using Microsoft.Build.Framework;

namespace Inno.Build.TaskHosting;

/// <summary>
/// Coordinates SDK restore writes for the checkout's shared restore graph owner.
/// </summary>
public sealed class RestoreOwnedProjectTask : Microsoft.Build.Utilities.Task, ICancelableTask
{
    private readonly object m_gate = new();
    private readonly CancellationTokenSource m_cancellation = new();
    private bool m_finished;

    /// <summary>
    /// Gets or sets projects with their explicit AdditionalProperties metadata.
    /// </summary>
    [Required]
    public ITaskItem[] Projects { get; set; } = [];

    /// <summary>
    /// Gets or sets the absolute lock belonging to all writers of the shared restore graph.
    /// The lock file remains after release.
    /// </summary>
    [Required]
    public string LockPath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets explicit name=value restore properties, independent of product discovery.
    /// </summary>
    public string[] Properties { get; set; } = [];

    /// <summary>
    /// Waits outside the MSBuild node, then restores under exclusive cross-process write ownership.
    /// </summary>
    /// <returns>
    /// True after every requested restore succeeds; false with a logged failure or cancellation.
    /// </returns>
    public override bool Execute()
    {
        FileLease? ownership = null;
        try
        {
            if (BuildEngine is not IBuildEngine3 engine || !Path.IsPathFullyQualified(LockPath))
                throw new ArgumentException("Owned restore requires an MSBuild node and an absolute write-owner lock.");
            if (Projects.Length == 0)
                throw new ArgumentException("Owned restore requires at least one explicitly selected project.");
            CancellationToken token = m_cancellation.Token;
            token.ThrowIfCancellationRequested();
            var projects = new List<string>(Projects.Length);
            var globals = new List<IDictionary>(Projects.Length);
            foreach (ITaskItem project in Projects)
            {
                string path = project.GetMetadata("FullPath");
                if (!File.Exists(path))
                    throw new FileNotFoundException("An owned restore project is missing.", path);
                var properties = new Hashtable(StringComparer.OrdinalIgnoreCase);
                Apply(project.GetMetadata("AdditionalProperties").Split(';', StringSplitOptions.RemoveEmptyEntries), properties);
                Apply(Properties, properties);
                projects.Add(path);
                globals.Add(properties);
            }
            engine.Yield();
            try
            {
                ownership = FileLease.AcquireAsync(LockPath, Timeout.InfiniteTimeSpan, token)
                    .AsTask().GetAwaiter().GetResult();
            }
            finally
            {
                engine.Reacquire();
            }
            token.ThrowIfCancellationRequested();
            var removed = new IList<string>[projects.Count];
            var versions = new string[projects.Count];
            for (int index = 0; index < removed.Length; index++)
                removed[index] = Array.Empty<string>();
            BuildEngineResult result = engine.BuildProjectFilesInParallel(projects.ToArray(), ["Restore"],
                globals.ToArray(), removed, versions, returnTargetOutputs: false);
            token.ThrowIfCancellationRequested();
            return result.Result;
        }
        catch (Exception failure)
        {
            Log.LogErrorFromException(failure, showStackTrace: true);
            return false;
        }
        finally
        {
            ownership?.Dispose();
            lock (m_gate)
            {
                m_finished = true;
                m_cancellation.Dispose();
            }
        }
    }

    /// <summary>
    /// Cancels this writer's ownership wait without canceling another active restore.
    /// </summary>
    public void Cancel()
    {
        lock (m_gate)
            if (!m_finished)
                m_cancellation.Cancel();
    }

    private static void Apply(
        IEnumerable<string> declarations,
        IDictionary properties
    ) {
        foreach (string declaration in declarations)
        {
            int separator = declaration.IndexOf('=');
            if (separator <= 0)
                throw new ArgumentException("Restore properties require explicit name=value declarations.");
            properties[declaration[..separator]] = Microsoft.Build.Evaluation.ProjectCollection.Unescape(declaration[(separator + 1)..]);
        }
    }
}
