using System;
using System.IO;
using Inno.Core.IO;
using Microsoft.Build.Framework;
using BuildTask = Microsoft.Build.Utilities.Task;

namespace Inno.Build.TaskHosting;

/// <summary>
/// Registers the lightweight publisher's process owner before any task uses its private load directory.
/// </summary>
public sealed class RegisterTaskPublisherTask : BuildTask
{
    /// <summary>
    /// Gets or sets the registered cache root containing private publisher directories.
    /// </summary>
    [Required]
    public string PublisherRoot { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the private configuration directory whose assembly was loaded for this invocation.
    /// </summary>
    [Required]
    public string PublisherDirectory { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the operation-owned compilation directory shared by publishers in this project invocation.
    /// This directory is never used as a task assembly load location.
    /// </summary>
    [Required]
    public string BootstrapDirectory { get; set; } = string.Empty;

    /// <summary>
    /// Records ownership using the same protocol as full task-runtime publication.
    /// </summary>
    /// <returns>
    /// True after recording the current process, or false after reporting an invalid owner or IO failure.
    /// </returns>
    public override bool Execute()
    {
        try
        {
            string root = TaskRuntimePreparation.RequireAbsolute(PublisherRoot);
            string directory = TaskRuntimePreparation.RequireAbsolute(PublisherDirectory);
            string bootstrap = TaskRuntimePreparation.RequireAbsolute(BootstrapDirectory);
            PathBoundary.RequireUnlinkedPath(root, directory);
            PathBoundary.RequireUnlinkedPath(root, bootstrap);
            string assembly = Path.GetFullPath(typeof(RegisterTaskPublisherTask).Assembly.Location);
            string expected = Path.Combine(directory, "Inno.Build.TaskHosting.dll");
            if (!string.Equals(assembly, expected, OperatingSystem.IsWindows()
                ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
                throw new InvalidOperationException("Publisher ownership must identify this task's actual private assembly.");
            TaskRuntimePreparation.RegisterProcessOwner(directory);
            TaskRuntimePreparation.RegisterProcessOwner(bootstrap);
            return true;
        }
        catch (Exception failure)
        {
            Log.LogErrorFromException(failure, showStackTrace: true);
            return false;
        }
    }
}
