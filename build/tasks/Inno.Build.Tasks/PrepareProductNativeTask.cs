using System;
using System.Diagnostics;
using System.Threading;
using Inno.Build.Toolchains;
using Inno.Build;
using Inno.Build.Distribution.Standard;
using Microsoft.Build.Framework;
using BuildTask = Microsoft.Build.Utilities.Task;

namespace Inno.Build.Tasks;

/// <summary>
/// Connects explicit product builds and publication to their target-native closure and exact deployment.
/// </summary>
public sealed class PrepareProductNativeTask : BuildTask, ICancelableTask
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
    /// Gets or sets the exact product identity whose component closure must be prepared.
    /// </summary>
    [Required]
    public string ProductId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the explicitly declared product target, independent of the task execution machine.
    /// </summary>
    [Required]
    public string TargetId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the debug or release configuration used by the managed application.
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
            long started = Stopwatch.GetTimestamp();
            var execution = StandardBuildEnvironment.Capture(AppContext.BaseDirectory, new BuildTargetId(TargetId));
            var distribution = StandardBuildDistribution.Create(execution).build;
            ProductNativeBuildPlan plan = distribution.ResolveNativeProduct(TargetId, ProductId);
            var context = new NativeBuildContext(EngineRoot, Configuration.ToLowerInvariant());
            NativeToolchainSelection toolchain = distribution.ResolveNativeToolchain(TargetId)
                .ResolveAsync(context, execution.host, TargetId, cancellation.Token).AsTask().GetAwaiter().GetResult();
            context = context.WithToolchain(toolchain);
            var products = plan.BuildAsync(context, cancellation.Token).GetAwaiter().GetResult();
            cancellation.Token.ThrowIfCancellationRequested();
            ProductNativeDeployment.InstallAsync(products, plan, OutputDirectory, cancellation.Token).GetAwaiter().GetResult();
            NativeBuildStatistics statistics = context.statistics;
            Log.LogMessage(MessageImportance.High,
                $"INNO-NATIVE-PREPARE elapsedMs={Stopwatch.GetElapsedTime(started).TotalMilliseconds:F1} "
                + $"hashedFiles={statistics.hashedFiles} hashedBytes={statistics.hashedBytes} "
                + $"processes={statistics.nativeProcesses} products={products.Count}");
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
