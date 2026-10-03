using System;
using System.Threading;
using Inno.Build;
using Inno.Build.SupportPacks;
using Microsoft.Build.Framework;
using BuildTask = Microsoft.Build.Utilities.Task;

namespace Inno.Build.Tasks;

/// <summary>
/// Connects MSBuild publication to the common atomic Support Pack publisher.
/// </summary>
public sealed class PublishSupportPackTask : BuildTask, ICancelableTask
{
    private readonly object m_gate = new();
    private CancellationTokenSource? m_cancellation;
    private bool m_canceled;

    /// <summary>
    /// Gets or sets the engine checkout directory.
    /// </summary>
    [Required]
    public string EngineRoot { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the directory owning installed target packs.
    /// </summary>
    [Required]
    public string OutputRoot { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the requested build target identifier.
    /// </summary>
    [Required]
    public string Target { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the SDK host executable.
    /// </summary>
    public string DotnetHost { get; set; } = "dotnet";

    /// <summary>
    /// Publishes and validates the target pack, reporting failures through MSBuild.
    /// </summary>
    /// <returns>
    /// True after installation; false when publication fails or is canceled.
    /// </returns>
    public override bool Execute()
    {
        using var cancellation = new CancellationTokenSource();
        lock (m_gate)
        {
            m_cancellation = cancellation;
            if (m_canceled)
                cancellation.Cancel();
        }
        try
        {
            string installed = BuiltInPlayerSupportPacks.CreatePublisher().PublishAsync(
                EngineRoot, OutputRoot, new BuildTargetId(Target), DotnetHost, cancellation.Token)
                .AsTask().GetAwaiter().GetResult();
            Log.LogMessage(MessageImportance.Normal, "Installed Player Support Pack: {0}", installed);
            return true;
        }
        catch (Exception failure)
        {
            Log.LogErrorFromException(failure, showStackTrace: true);
            return false;
        }
        finally
        {
            lock (m_gate)
                m_cancellation = null;
        }
    }

    /// <summary>
    /// Requests cancellation before or during execution without racing disposal.
    /// </summary>
    public void Cancel()
    {
        lock (m_gate)
        {
            m_canceled = true;
            m_cancellation?.Cancel();
        }
    }
}
