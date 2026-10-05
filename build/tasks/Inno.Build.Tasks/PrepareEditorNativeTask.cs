using System;
using System.Threading;
using Inno.Build.Toolchains;
using Inno.Build.Toolchains.Host;
using Microsoft.Build.Framework;
using BuildTask = Microsoft.Build.Utilities.Task;

namespace Inno.Build.Tasks;

/// <summary>
/// Connects Editor publication to host-native preparation and exact product deployment.
/// </summary>
public sealed class PrepareEditorNativeTask : BuildTask, ICancelableTask
{
    private readonly object m_gate = new();
    private CancellationTokenSource? m_cancellation;
    private bool m_canceled;

    /// <summary>
    /// Gets or sets the source checkout containing the unified native toolchains.
    /// </summary>
    [Required]
    public string EngineRoot { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the managed application directory receiving the complete native deployment.
    /// </summary>
    [Required]
    public string OutputDirectory { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the debug or release configuration used by the published managed application.
    /// </summary>
    public string Configuration { get; set; } = "release";

    /// <summary>
    /// Prepares all host-native products and installs their exact closure after successful validation.
    /// </summary>
    /// <returns>
    /// True after deployment, or false after reporting a build failure or cancellation to MSBuild.
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
            var products = HostNativeBuild.BuildEditorAsync(
                new NativeBuildContext(EngineRoot, Configuration.ToLowerInvariant()), cancellation.Token)
                .GetAwaiter().GetResult();
            cancellation.Token.ThrowIfCancellationRequested();
            HostNativeDeployment.InstallAsync(products, OutputDirectory, cancellation.Token).GetAwaiter().GetResult();
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
    /// Requests process-tree retirement before or during task execution without racing token disposal.
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
