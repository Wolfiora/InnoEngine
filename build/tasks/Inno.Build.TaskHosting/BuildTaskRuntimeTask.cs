using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Inno.Core.IO;
using Inno.Build.Toolchains;
using Microsoft.Build.Framework;

namespace Inno.Build.TaskHosting;

/// <summary>
/// Coordinates shared task-runtime compilation before immutable publication and private loading.
/// </summary>
public sealed class BuildTaskRuntimeTask : Microsoft.Build.Utilities.Task, ICancelableTask
{
    private readonly object m_cancellationGate = new();
    private readonly CancellationTokenSource m_cancellation = new();
    private bool m_finished;

    /// <summary>
    /// Gets or sets the task project whose complete runtime is built by the calling MSBuild engine.
    /// </summary>
    [Required]
    public string ProjectFile { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the absolute shared intermediate owner, also supplied as ArtifactsPath.
    /// </summary>
    [Required]
    public string ArtifactsDirectory { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the private reader location registered before returning an immutable runtime.
    /// </summary>
    [Required]
    public string LoadDirectory { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the ordered targets returning the complete hashed runtime closure.
    /// </summary>
    [Required]
    public string[] Targets { get; set; } = [];

    /// <summary>
    /// Gets or sets explicit name=value properties after product-specific global properties are removed.
    /// Explicit values override removal of inherited properties with the same name.
    /// ArtifactsPath is owned by this task and cannot be overridden.
    /// </summary>
    public string[] Properties { get; set; } = [];

    /// <summary>
    /// Gets or sets parent global properties that must not enter the host tool build.
    /// </summary>
    public string[] RemoveProperties { get; set; } = [];

    /// <summary>
    /// Gets the returned runtime files and their hash metadata after successful compilation.
    /// </summary>
    [Output]
    public ITaskItem[] TargetOutputs { get; private set; } = [];

    /// <summary>
    /// Serializes shared writes across processes while yielding the MSBuild node during ownership waits.
    /// </summary>
    /// <returns>
    /// True after a complete build; false with a logged error after failure or cancellation.
    /// </returns>
    public override bool Execute()
    {
        try
        {
            CancellationToken token = m_cancellation.Token;
            token.ThrowIfCancellationRequested();
            if (BuildEngine is not IBuildEngine4 engine)
                throw new InvalidOperationException("Task-runtime preparation requires the MSBuild Build-lifetime registry.");
            string project = RequireAbsolute(ProjectFile);
            string artifacts = RequireAbsolute(ArtifactsDirectory);
            string load = RequireAbsolute(LoadDirectory);
            string taskRoot = Path.GetDirectoryName(Path.GetDirectoryName(load))!;
            string cache = Path.Combine(Path.GetDirectoryName(taskRoot)!, "hosts");
            PathBoundary.RequireUnlinkedPath(taskRoot, load);
            string[] targets = Targets.ToArray();
            string[] removed = RemoveProperties.ToArray();
            if (!File.Exists(project) || targets.Length == 0 || targets.Any(string.IsNullOrWhiteSpace))
                throw new ArgumentException("A task project and nonempty build targets are required.");
            IDictionary properties = FreezeProperties(artifacts, removed);
            // MSBuild's API applies undefinitions to the final request. Explicit task
            // properties must survive removal of identically named parent globals.
            removed = removed.Where(name => !properties.Contains(name)).ToArray();
            TaskRuntimeRequestSnapshot initial = TaskRuntimeRequestKey.Capture(project, artifacts, targets, properties, token);
            bool restoreRequired = initial.needsRestore;
            bool restoreRequested = targets.Contains("Restore", StringComparer.OrdinalIgnoreCase);
            string? request = restoreRequired ? null : initial.key;
            if (request is not null && TaskRuntimeBuildScope.Read(engine, request, load, cancellation: token) is ITaskItem[] reused)
            {
                TargetOutputs = reused;
                Log.LogMessage(MessageImportance.High, "INNO-TASK-RUNTIME reused {0}", request);
                return true;
            }
            FileLease? ownership = null;
            engine.Yield();
            try
            {
                ownership = FileLease.AcquireAsync(artifacts + ".lock",
                    Timeout.InfiniteTimeSpan, token).AsTask().GetAwaiter().GetResult();
            }
            finally
            {
                try
                {
                    engine.Reacquire();
                }
                catch
                {
                    ownership?.Dispose();
                    throw;
                }
            }
            using (ownership)
            {
                token.ThrowIfCancellationRequested();
                Directory.CreateDirectory(artifacts);
                if (restoreRequired || restoreRequested)
                {
                    BuildEngineResult restored = engine.BuildProjectFilesInParallel([project], ["Restore"],
                        [properties], [removed], [null!], returnTargetOutputs: false);
                    token.ThrowIfCancellationRequested();
                    if (!restored.Result)
                        return false;

                }
                TaskRuntimeRequestSnapshot restoredInputs = restoreRequired || restoreRequested
                    ? TaskRuntimeRequestKey.Capture(project, artifacts, targets, properties, token) : initial;
                foreach (TaskRuntimeRestoreRequest inputRestore in restoredInputs.restoreRequests
                    .DistinctBy(static input => input.project + "|" + string.Join(";", input.properties
                        .OrderBy(static property => property.Key, StringComparer.OrdinalIgnoreCase)
                        .Select(static property => property.Key + "=" + property.Value))))
                {
                    var inputProperties = new Hashtable(StringComparer.OrdinalIgnoreCase);
                    foreach (var property in inputRestore.properties)
                        inputProperties[property.Key] = property.Value;
                    BuildEngineResult restored = engine.BuildProjectFilesInParallel([inputRestore.project], ["Restore"],
                        [inputProperties], [inputRestore.removedProperties.ToArray()], [null!], returnTargetOutputs: false);
                    token.ThrowIfCancellationRequested();
                    if (!restored.Result)
                        return false;
                }
                TaskRuntimeRequestSnapshot locked = TaskRuntimeRequestKey.Capture(project, artifacts, targets, properties, token);
                if (locked.needsRestore)
                    throw new InvalidOperationException("Task-runtime dependencies are not restored.");
                if (!restoreRequested && !restoreRequired && request is not null && locked.key != request)
                    throw new InvalidOperationException("Task-runtime inputs changed while waiting for build ownership.");
                request = locked.key;
                if (TaskRuntimeBuildScope.Read(engine, request, load, cancellation: token) is ITaskItem[] completed)
                {
                    TargetOutputs = completed;
                    Log.LogMessage(MessageImportance.High, "INNO-TASK-RUNTIME reused {0}", request);
                    return true;
                }
                string[] compilationTargets = targets.Where(static target => !target.Equals("Restore", StringComparison.OrdinalIgnoreCase)).ToArray();
                if (compilationTargets.Length == 0)
                    throw new ArgumentException("Task-runtime preparation requires a publication target after restore.", nameof(Targets));
                BuildEngineResult result = engine.BuildProjectFilesInParallel([project], compilationTargets,
                    [properties], [removed], [null!], returnTargetOutputs: true);
                token.ThrowIfCancellationRequested();
                if (!result.Result)
                    return false;
                TargetOutputs = result.TargetOutputsPerProject.SelectMany(static projectOutputs => projectOutputs.Values)
                    .SelectMany(static output => output).ToArray();
                if (TargetOutputs.Length == 0)
                    throw new InvalidDataException("The task project returned no runtime closure.");
                TaskRuntimeRequestSnapshot compiled = TaskRuntimeRequestKey.Capture(project, artifacts, targets, properties, token);
                if (compiled.key != request)
                    throw new InvalidOperationException("Task-runtime inputs changed during compilation; its preparation was not cached. "
                        + locked.DescribeChanges(compiled));
                TargetOutputs = TaskRuntimePreparation.Publish(cache, TargetOutputs, engine, load, token);
                TaskRuntimePreparation.RegisterReader(load, TargetOutputs.Single(static item => item.GetMetadata("RelativePath") == "Inno.Build.Tasks.dll").ItemSpec);
                var workspaces = new BuildCachePublisher(Path.GetDirectoryName(artifacts)!);
                using (workspaces.AcquireUseAsync(token).AsTask().GetAwaiter().GetResult())
                    workspaces.Record(artifacts, artifacts + ".lock", [], null, null, token);
                TaskRuntimeBuildScope.Register(engine, request, TargetOutputs);
                Log.LogMessage(MessageImportance.High, "INNO-TASK-RUNTIME prepared {0}", request);
            }
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
    /// Cancels an ownership wait without racing retirement of the cancellation source.
    /// </summary>
    public void Cancel()
    {
        lock (m_cancellationGate)
            if (!m_finished)
                m_cancellation.Cancel();
    }

    private IDictionary FreezeProperties(
        string artifacts,
        IReadOnlyList<string> removed
    ) {
        Hashtable properties = new(StringComparer.OrdinalIgnoreCase);
        if (BuildEngine is IBuildEngine6 engine)
            foreach (var property in engine.GetGlobalProperties())
                if (!removed.Contains(property.Key, StringComparer.OrdinalIgnoreCase))
                    properties[property.Key] = property.Value;
        var explicitNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (string property in Properties)
        {
            int separator = property.IndexOf('=');
            if (separator <= 0 || !explicitNames.Add(property[..separator]))
                throw new ArgumentException("Task properties require unique name=value declarations.");
            string name = property[..separator];
            if (string.Equals(name, "ArtifactsPath", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("The task artifact owner cannot be overridden.");
            properties[name] = property[(separator + 1)..];
        }
        properties["ArtifactsPath"] = artifacts;
        return properties;
    }

    private static string RequireAbsolute(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        if (!Path.IsPathFullyQualified(value))
            throw new ArgumentException("Task-runtime ownership requires absolute locations.");
        return Path.GetFullPath(value);
    }
}
