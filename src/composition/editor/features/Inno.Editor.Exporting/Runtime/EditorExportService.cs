using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build;
using Inno.Build.Managed;
using Inno.Core.Logging;
using Inno.Core.Execution;
using Inno.Core.Settings;
using Inno.Editor.Core;

namespace Inno.Editor.Exporting;
/// <summary>
/// Owns asynchronous export, progress, cancellation and retirement independently from presentation.
/// </summary>
[EditorModule("exporting", order: 300)]
public sealed class EditorExportService : EditorModule
{
    private readonly BuildPipeline m_buildPipeline;
    private readonly Logger m_log;
    private readonly ConcurrentQueue<(long operation, BuildProgress value)> m_progress = new();
    private long m_operation;
    private string m_status = string.Empty;
    private string m_error = string.Empty;
    private float? m_fraction;
    private CancellationTokenSource? m_cancellation;
    private Task<PlayerSupportPackLease>? m_gamePreparation;
    private Task<BuildResult>? m_gameExport;
    private Task<BuildResult>? m_pluginExport;
    private GameBuildRequest? m_pendingGameRequest;
    internal EditorExportService(
        BuildPipeline buildPipeline,
        LogRouter logs
    ) {
        m_buildPipeline = buildPipeline ?? throw new ArgumentNullException(nameof(buildPipeline));
        ArgumentNullException.ThrowIfNull(logs);
        m_log = logs.CreateLogger<EditorExportService>();
    }

    /// <summary>
    /// Gets whether a Plugin source export still owns build resources.
    /// </summary>
    public bool isPluginBusy => m_pluginExport is not null;
    /// <summary>
    /// Gets whether a game export or its Support Pack preparation is active.
    /// </summary>
    public bool isGameBusy => m_gamePreparation is not null || m_gameExport is not null;
    /// <summary>
    /// Gets one owner-thread snapshot for the current export, including cancellation and action identity.
    /// </summary>
    public EditorExportProgress progress => new(m_operation, m_fraction, m_status, m_error,
        isGameBusy || isPluginBusy, m_cancellation?.IsCancellationRequested == true);

    /// <summary>
    /// Cancels the observed export only while that operation still owns this service.
    /// </summary>
    /// <param name="operationId">
    /// The operation observed by the caller.
    /// </param>
    public void Cancel(long operationId)
    {
        if (operationId == m_operation)
            CancelCurrent();
    }

    /// <summary>
    /// Gets the targets registered by the active Build distribution.
    /// </summary>
    public IReadOnlyList<BuildTargetId> availableGameTargets => m_buildPipeline.availableGameTargets;

    /// <summary>
    /// Gets deployment implementations registered for a target.
    /// </summary>
    /// <param name="target">
    /// The explicitly chosen build target.
    /// </param>
    /// <returns>
    /// The registered deployment choices; no host fallback is introduced.
    /// </returns>
    public IReadOnlyList<ManagedDeploymentId> GetManagedDeployments(BuildTargetId target) => m_buildPipeline.GetManagedDeployments(target);
    /// <summary>
    /// Gets a registered target label or a diagnostic for an unavailable target.
    /// </summary>
    /// <param name="target">
    /// The target to describe.
    /// </param>
    /// <returns>
    /// A presentation label; never used as a routing identity.
    /// </returns>
    public string GetGameTargetDisplayName(BuildTargetId target) => m_buildPipeline.TryGetGameTargetDisplayName(
        target,
        out string displayName
    ) ? displayName : $"Missing ({target})";
    /// <summary>
    /// Starts a source-package export using the existing Build pipeline.
    /// </summary>
    /// <param name="request">
    /// The complete, explicitly selected package request.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Another export still owns the service.
    /// </exception>
    public void StartPlugin(PluginBuildRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        EnsureIdle();
        StartCancellation();
        m_status = "Capturing the current Plugin source and dependency generation...";
        m_error = string.Empty;
        m_pluginExport = m_buildPipeline.BuildPluginAsync(request, new BuildProgressSink(m_progress, m_operation), m_cancellation!.Token).AsTask();
    }

    /// <summary>
    /// Prepares the selected Support Pack before capturing and building the game.
    /// </summary>
    /// <param name="request">
    /// The complete game request; no form or window state is retained.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Another export still owns the service.
    /// </exception>
    public void StartGame(GameBuildRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        EnsureIdle();
        StartCancellation();
        m_fraction = null;
        m_status = "Checking or preparing the Player Support Pack...";
        m_error = string.Empty;
        m_pendingGameRequest = request;
        try
        {
            m_gamePreparation = m_buildPipeline.EnsurePlayerSupportPackAsync(request.profile.target, request.profile.managedDeployment, m_cancellation!.Token).AsTask();
        }
        catch (Exception exception)
        {
            m_gamePreparation = Task.FromException<PlayerSupportPackLease>(exception);
        }
    }

    private void EnsureIdle()
    {
        if (isPluginBusy || isGameBusy)
            throw new InvalidOperationException("An export still owns the build service.");
    }

    /// <summary>
    /// Advances compilation tickets, build progress, and completed export tasks.
    /// </summary>
    /// <param name="context">
    /// The active Editor context for the current frame.
    /// </param>
    protected override void OnUpdate(EditorContext context)
    {
        while (m_progress.TryDequeue(out var update))
        {
            if (update.operation != m_operation || m_cancellation is null || m_cancellation.IsCancellationRequested)
                continue;
            BuildProgress progress = update.value;
            m_status = progress.message;
            if (isGameBusy || isPluginBusy)
                m_fraction = MathF.Max(m_fraction ?? 0f, (float)progress.fraction);
        }

        CompletePluginExport();
        CompleteGamePreparation();
        CompleteGameExport();
    }

    /// <summary>
    /// Cancels and drains active export work before the Editor generation is stopped.
    /// </summary>
    /// <param name="context">
    /// The active Editor context being stopped.
    /// </param>
    protected override void OnStop(EditorContext context)
    {
        RetireExports();
    }

    /// <summary>
    /// Drains export cleanup before releasing cancellation resources owned by this module.
    /// </summary>
    protected override void OnDispose()
    {
        RetireExports();
    }

    private void RetireExports()
    {
        m_cancellation?.Cancel();
        if (m_gamePreparation is { IsCompleted: false } || m_gameExport is { IsCompleted: false } || m_pluginExport is { IsCompleted: false })
        {
            throw new RetirementPendingException("Canceled export work still owns build services; retry retirement after it drains.");
        }

        RequireRetiredOutcome(m_gamePreparation);
        RequireRetiredOutcome(m_gameExport);
        RequireRetiredOutcome(m_pluginExport);
        CompletePluginExport();
        CompleteGameExport();
        Task<PlayerSupportPackLease>? preparation = m_gamePreparation;
        m_gamePreparation = null;
        m_pendingGameRequest = null;
        if (preparation is not null)
        {
            try
            {
                using PlayerSupportPackLease prepared = preparation.GetAwaiter().GetResult();
            }
            catch (OperationCanceledException)
            {
            // Cancellation during shutdown is expected; preparation must never start a new build here.
            }
            catch (Exception exception)
            {
                m_log.Write(LogLevel.Error, "Game Support Pack preparation failed during shutdown: {0}", [exception]);
            }
        }

        while (m_progress.TryDequeue(out _))
        {
        }

        ReleaseCancellation();
    }

    private static void RequireRetiredOutcome(Task? task)
    {
        if (task?.Exception is Exception failure && RetirementPendingException.Find(failure) is not null)
            throw failure;
    }

    private void CompletePluginExport()
    {
        Task<BuildResult>? export = m_pluginExport;
        if (export is null || !export.IsCompleted)
            return;
        m_pluginExport = null;
        try
        {
            BuildResult result = export.GetAwaiter().GetResult();
            if (!result.succeeded)
            {
                ReportBuildFailure("Plugin export", result);
                return;
            }

            string outputPath = result.outputPath ?? throw new InvalidOperationException("A successful Plugin build has no output path.");
            string contentHash = result.contentHash ?? throw new InvalidOperationException("A successful Plugin build has no content identity.");
            m_fraction = 1f;
            m_status = $"Exported {result.assetCount} source files to {outputPath}";
            m_error = string.Empty;
            m_log.Write(
                LogLevel.Info,
                "Exported Plugin '{0}' to '{1}' ({2} assets, {3} embedded dependencies, {4}).",
                ["Project", outputPath, result.assetCount, result.embeddedPluginCount, contentHash]
            );
        }
        catch (OperationCanceledException)
        {
            m_status = string.Empty;
            m_error = "Plugin export was canceled; no partial package was installed.";
            m_log.Write(LogLevel.Info, "Plugin export was canceled.");
        }
        catch (Exception exception)
        {
            m_status = string.Empty;
            m_error = exception.Message;
            m_log.Write(LogLevel.Error, "Plugin export failed: {0}", [exception]);
        }
        finally
        {
            ReleaseCancellation();
        }
    }

    private void CompleteGameExport()
    {
        Task<BuildResult>? export = m_gameExport;
        if (export is null || !export.IsCompleted)
            return;
        m_gameExport = null;
        try
        {
            BuildResult result = export.GetAwaiter().GetResult();
            if (!result.succeeded)
            {
                ReportBuildFailure("Game build", result);
                return;
            }

            string outputPath = result.outputPath ?? throw new InvalidOperationException("A successful game build has no output path.");
            m_status = $"Exported {result.assetCount} assets and {result.runtimeAssemblyCount} runtime assemblies to {outputPath}";
            m_error = string.Empty;
            m_fraction = 1f;
            m_log.Write(
                LogLevel.Info,
                "Exported game to '{0}' ({1}, {2} assets, {3} artifact bundles, {4} runtime assemblies).",
                [outputPath, result.target?.ToString() ?? "unknown", result.assetCount, result.artifactBundleCount, result.runtimeAssemblyCount]
            );
        }
        catch (OperationCanceledException)
        {
            m_status = string.Empty;
            m_error = "Game build was canceled; no partial output was installed.";
            m_log.Write(LogLevel.Info, "Game build was canceled.");
        }
        catch (Exception exception)
        {
            m_status = string.Empty;
            m_error = exception.Message;
            m_log.Write(LogLevel.Error, "Game build failed: {0}", [exception]);
        }
        finally
        {
            ReleaseCancellation();
        }
    }

    private void CompleteGamePreparation()
    {
        Task<PlayerSupportPackLease>? preparation = m_gamePreparation;
        if (preparation is null || !preparation.IsCompleted)
            return;
        m_gamePreparation = null;
        try
        {
            using PlayerSupportPackLease prepared = preparation.GetAwaiter().GetResult();
            m_cancellation!.Token.ThrowIfCancellationRequested();
            GameBuildRequest request = m_pendingGameRequest ?? throw new InvalidOperationException("The prepared game build has no request.");
            m_pendingGameRequest = null;
            m_status = "Capturing the authoring generation and compiling runtime scripts...";
            m_gameExport = m_buildPipeline.BuildGameAsync(request, new BuildProgressSink(m_progress, m_operation), m_cancellation.Token).AsTask();
        }
        catch (OperationCanceledException)
        {
            m_pendingGameRequest = null;
            m_status = string.Empty;
            m_error = "Game build was canceled; no partial output was installed.";
            m_log.Write(LogLevel.Info, "Game build was canceled while preparing its Support Pack.");
            ReleaseCancellation();
        }
        catch (Exception exception)
        {
            m_pendingGameRequest = null;
            m_status = string.Empty;
            m_error = exception.Message;
            m_log.Write(LogLevel.Error, "Game Support Pack preparation failed: {0}", [exception]);
            ReleaseCancellation();
        }
    }

    private void StartCancellation()
    {
        m_operation++;
        m_fraction = null;
        m_cancellation?.Dispose();
        m_cancellation = new CancellationTokenSource();
    }

    private void CancelCurrent()
    {
        if (m_cancellation is null)
            return;
        m_status = "Canceling export...";
        m_cancellation.Cancel();
    }

    private void ReleaseCancellation()
    {
        m_cancellation?.Dispose();
        m_cancellation = null;
    }

    private void ReportBuildFailure(
        string operation,
        BuildResult result
    ) {
        string message = string.Join(Environment.NewLine, result.diagnostics.Select(static diagnostic => $"[{diagnostic.code}] {diagnostic.message}"));
        m_status = string.Empty;
        m_error = message;
        m_log.Write(LogLevel.Error, "{0} failed:{1}{2}", [operation, Environment.NewLine, message]);
    }

    private sealed class BuildProgressSink(
        ConcurrentQueue<(long operation, BuildProgress value)> progress,
        long operation
    ) : IProgress<BuildProgress>
    {
        /// <summary>
        /// Publishes one progress update to the receiving workflow.
        /// </summary>
        /// <param name="value">
        /// The concrete value read or transformed by this operation.
        /// </param>
        public void Report(BuildProgress value) => progress.Enqueue((operation, value));
    }
}
