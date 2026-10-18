using System;
using System.IO;
using System.Threading;
using Inno.Build.Toolchains;
using Inno.Core.IO;
using Microsoft.Build.Framework;
using BuildTask = Microsoft.Build.Utilities.Task;

namespace Inno.Build.TaskHosting;

/// <summary>
/// Copies one protected generated source into the consuming SDK build's own intermediate directory.
/// </summary>
public sealed class MaterializeBindingsTask : BuildTask, ICancelableTask
{
    private readonly object m_lifecycle = new();
    private readonly CancellationTokenSource m_cancellation = new();
    private bool m_completed;

    /// <summary>
    /// Gets or sets the exact selected managed source, without selecting another generation.
    /// </summary>
    [Required]
    public string Source { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the absolute SDK-owned intermediate root.
    /// </summary>
    [Required]
    public string IntermediateDirectory { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the Native project owning mutable host-source publication.
    /// </summary>
    [Required]
    public string ComponentProject { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the immutable artifact root; empty denotes mutable host-source publication.
    /// </summary>
    public string ArtifactDirectory { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the exact generation identity required when consuming an immutable artifact.
    /// </summary>
    public string Fingerprint { get; set; } = string.Empty;

    /// <summary>
    /// Gets the only generated Compile source, owned by this SDK build after the task returns.
    /// </summary>
    [Output]
    public string MaterializedSource { get; private set; } = string.Empty;

    /// <summary>
    /// Cancels ownership acquisition without disturbing an active publisher.
    /// </summary>
    public void Cancel()
    {
        lock (m_lifecycle)
            if (!m_completed)
                m_cancellation.Cancel();
    }

    /// <summary>
    /// Validates and copies the source while holding its publication owner.
    /// </summary>
    /// <returns>
    /// True for a complete private source; false after logging a failed or canceled copy.
    /// </returns>
    public override bool Execute()
    {
        try
        {
            CancellationToken cancellation = m_cancellation.Token;
            cancellation.ThrowIfCancellationRequested();
            if (!Path.IsPathFullyQualified(Source) || !Path.IsPathFullyQualified(IntermediateDirectory)
                || !Path.IsPathFullyQualified(ComponentProject))
                throw new ArgumentException("Binding consumption requires absolute source and owner paths.");
            string source = Path.GetFullPath(Source);
            using IDisposable ownership = Acquire(source, cancellation);
            string destination = Path.Combine(Path.GetFullPath(IntermediateDirectory), "bindings", "Bindings.cs");
            if (SamePath(source, destination))
                throw new ArgumentException("The SDK consumer must not overwrite the publisher's source.");
            byte[] content = File.ReadAllBytes(source);
            cancellation.ThrowIfCancellationRequested();
            if (!File.Exists(destination) || !content.AsSpan().SequenceEqual(File.ReadAllBytes(destination)))
                AtomicFile.WriteAllBytes(destination, content);
            MaterializedSource = destination;
            return true;
        }
        catch (Exception failure)
        {
            Log.LogErrorFromException(failure, showStackTrace: true);
            return false;
        }
        finally
        {
            lock (m_lifecycle)
            {
                m_completed = true;
                m_cancellation.Dispose();
            }
        }
    }

    private IDisposable Acquire(
        string source,
        CancellationToken cancellation
    ) {
        if (ArtifactDirectory.Length != 0)
        {
            if (!Path.IsPathFullyQualified(ArtifactDirectory))
                throw new ArgumentException("Binding artifacts require an absolute publication owner.");
            string directory = Path.GetFullPath(ArtifactDirectory);
            if (!SamePath(source, Path.Combine(directory, "Generated", "Bindings.cs")))
                throw new ArgumentException("The selected source is outside its declared binding artifact.");
            return BuildArtifactReadLease.AcquireAsync(directory, Fingerprint, ["Native", "Generated"], cancellation)
                .AsTask().GetAwaiter().GetResult();
        }
        string owner = Path.GetDirectoryName(Path.GetFullPath(ComponentProject))!;
        if (!SamePath(source, Path.Combine(owner, "Generated", "Bindings.cs")))
            throw new ArgumentException("Host bindings must belong to their Native project's Generated directory.");
        return FileLease.AcquireAsync(Path.Combine(owner, "obj", "bindings", "generation.lock"),
            TimeSpan.FromSeconds(30), cancellation).AsTask().GetAwaiter().GetResult();
    }

    private static bool SamePath(
        string left,
        string right
    ) => string.Equals(left, right, OperatingSystem.IsWindows()
        ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
}
