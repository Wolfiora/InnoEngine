using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
    /// Gets or sets an optional operation-owned selection consumed by the subsequent managed project closure.
    /// </summary>
    public string BindingSelectionOutputPath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the managed build's frozen selection that publication must preserve before deployment.
    /// An empty value denotes initial preparation without a preceding managed compilation.
    /// </summary>
    public string BindingSelectionInputPath { get; set; } = string.Empty;

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
            var context = new NativeBuildContext(EngineRoot, Configuration.ToLowerInvariant())
                .WithBindingGenerator(execution.bindingGenerator);
            NativeToolchainSelection toolchain = distribution.ResolveNativeToolchain(TargetId)
                .ResolveAsync(context, execution.host, TargetId, cancellation.Token).AsTask().GetAwaiter().GetResult();
            context = context.WithToolchain(toolchain);
            var products = plan.BuildAsync(context, cancellation.Token).GetAwaiter().GetResult();
            cancellation.Token.ThrowIfCancellationRequested();
            var generations = products.Select((
                product,
                index
            ) => (product, owner: plan.steps[index].component.nativeProject))
                .Where(static entry => entry.product.bindingGeneration is not null)
                .GroupBy(static entry => entry.owner, StringComparer.Ordinal)
                .ToDictionary(static group => group.Key,
                    static group => RequireSharedGeneration(group.Select(static entry => entry.product.bindingGeneration!)),
                    StringComparer.Ordinal);
            var linkage = plan.steps.Where(step => generations.ContainsKey(step.component.nativeProject))
                .GroupBy(static step => step.component.nativeProject, StringComparer.Ordinal)
                .ToDictionary(static group => group.Key,
                    static group => group.Select(step => step.options.libraryKind).Distinct().Single(), StringComparer.Ordinal);
            if (BindingSelectionInputPath.Length != 0)
                NativeBindingGenerationDescriptor.ValidateSelection(BindingSelectionInputPath, generations, linkage);
            ProductNativeDeployment.InstallAsync(products, plan, OutputDirectory, cancellation.Token).GetAwaiter().GetResult();
            if (BindingSelectionOutputPath.Length != 0)
                NativeBindingGenerationDescriptor.WriteSelection(BindingSelectionOutputPath, generations, linkage);
            NativeBuildStatistics statistics = context.statistics;
            Log.LogMessage(MessageImportance.High,
                $"INNO-NATIVE-PREPARE elapsedMs={Stopwatch.GetElapsedTime(started).TotalMilliseconds:F1} "
                + $"hashedFiles={statistics.hashedFiles} hashedBytes={statistics.hashedBytes} "
                + $"processes={statistics.nativeProcesses} products={products.Count} "
                + $"bindingBatches={statistics.bindingBatches} bindingGenerations={statistics.bindingGenerations} "
                + $"outputFiles={statistics.outputFiles} outputBytes={statistics.outputBytes} "
                + $"materializedBytes={statistics.materializedBytes} managedProcesses={statistics.managedProcesses}");
            foreach (NativeBuildPhaseStatistics phase in statistics.phases)
                Log.LogMessage(MessageImportance.High,
                    $"INNO-NATIVE-PHASE name={phase.phase} files={phase.files} bytes={phase.bytes} reusedReads={phase.reusedReads}");
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

    private static NativeBindingGenerationDescriptor RequireSharedGeneration(IEnumerable<NativeBindingGenerationDescriptor> generations)
    {
        NativeBindingGenerationDescriptor selected = generations.First();
        if (generations.Any(generation => generation.fingerprint != selected.fingerprint
            || generation.bindingsPath != selected.bindingsPath || generation.bridgeDirectory != selected.bridgeDirectory))
            throw new InvalidOperationException("Native products sharing a binding owner must select the same complete generation.");
        return selected;
    }
}
