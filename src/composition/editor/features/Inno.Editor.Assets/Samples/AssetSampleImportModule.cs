using Inno.Editor.Assets;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Core.Execution;
using Inno.Core.Logging;
using Inno.Editor.Core;
using Inno.Editor.Interactions;
using Inno.Editor.Scripting;
using Inno.Scripting.Compiler;

namespace Inno.Editor.Assets;
/// <summary>
/// Owns sample import validation, publication and rollback independently of its progress window.
/// </summary>
[EditorModule("asset-sample-import", order: 110)]
public sealed class AssetSampleImportModule : EditorModule
{
    private readonly object m_progressLock = new();
    private readonly AssetEditingModule m_assets;
    private readonly ScriptCompiler m_compiler;
    private readonly IEditorScriptCompilation m_scripting;
    private readonly Logger m_log;
    private AssetSampleImportTransaction? m_import;
    private EditorHistoryPayload? m_historyPayload;
    private Exception? m_failure;
    private bool m_validating;
    private bool m_canceled;
    private bool m_faulted;
    private AssetSampleImportProgress m_progress = new(0, 0f, string.Empty, false);
    private long m_operation;
    private bool m_acceptProgress;
    internal AssetSampleImportModule(
        AssetEditingModule assets,
        ScriptCompiler compiler,
        IEditorScriptCompilation scripting,
        LogRouter logs
    ) {
        m_assets = assets;
        m_compiler = compiler;
        m_scripting = scripting;
        m_log = logs.CreateLogger<AssetSampleImportModule>();
    }

    /// <summary>
    /// Defers later modules while the import pins their generation or requires a Host restart.
    /// </summary>
    public override bool blocksFollowingUpdates => isBusy || m_faulted;
    /// <summary>
    /// Gets whether an import still owns validation or publication work.
    /// </summary>
    public bool isBusy => !m_faulted && m_import is not null;
    /// <summary>
    /// Gets whether the current authoring generation admits a new sample import.
    /// </summary>
    public bool canImport => !m_faulted && !isBusy && m_scripting.state is EditorScriptCompilationState.Ready or EditorScriptCompilationState.Failed;

    /// <summary>
    /// Gets a complete immutable progress snapshot under the existing owner lock.
    /// </summary>
    public AssetSampleImportProgress progress
    {
        get
        {
            lock (m_progressLock)
                return m_progress;
        }
    }

    /// <summary>
    /// Begins validating an installed sample before its source copy can be committed.
    /// </summary>
    /// <param name="source">
    /// The installed sample root to validate and import.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// The authoring generation is busy or faulted.
    /// </exception>
    public void Request(AssetPath source)
    {
        if (!canImport)
            throw new InvalidOperationException("Sample import requires an idle authoring generation.");
        m_import = m_assets.pipeline.PrepareSampleImport(source);
        m_canceled = false;
        lock (m_progressLock)
        {
            m_acceptProgress = true;
            m_progress = new(++m_operation, 0f, "Preparing sample assets...", false);
        }
    }

    /// <summary>
    /// Requests cancellation without releasing work that has not retired.
    /// </summary>
    private void CancelCurrent()
    {
        if (m_import is null)
            return;
        m_canceled = true;
        lock (m_progressLock)
            m_progress = m_progress with { status = "Canceling sample import...", cancellationRequested = true };
        m_import.Cancel();
    }

    /// <summary>
    /// Cancels only the import observed by the presentation.
    /// </summary>
    /// <param name="operationId">
    /// The current import identity captured by the caller.
    /// </param>
    public void Cancel(long operationId)
    {
        if (progress.operationId == operationId)
            CancelCurrent();
    }

    /// <summary>
    /// Advances completed background phases and publishes the validated import at a frame safe point.
    /// </summary>
    /// <param name="context">
    /// The current Editor frame and interaction services.
    /// </param>
    protected override void OnUpdate(EditorContext context)
    {
        if (!isBusy)
            return;
        try
        {
            if (m_canceled || m_failure is not null)
            {
                RetireImport();
                return;
            }

            AssetSampleImportTransaction import = m_import!;
            if (!m_validating)
            {
                if (!import.Advance())
                    return;
                m_validating = true;
                SetProgress(0.2f, "Validating sample scripts...");
                string physicalSource = Path.Combine(m_assets.pipeline.assetRoot, import.target.localPath);
                string targetPath = import.target.ToString();
                long operation = progress.operationId;
                import.BeginValidation(async (
                    sources,
                    cancellationToken
                ) =>
                {
                    ScriptCompilationResult result = await m_compiler.CompileAuthoringGenerationAsync(new ImportProgress(this, operation), cancellationToken, sources).ConfigureAwait(false);
                    if (!result.success)
                        throw new InvalidOperationException("Sample script preflight failed:" + Environment.NewLine + string.Join(Environment.NewLine, result.diagnostics.Select(static diagnostic => diagnostic.message)));
                    SetProgress(0.85f, "Preparing sample history...", operation);
                    m_historyPayload = await Task.Run(() =>
                    {
                        byte[] archive = AssetSourceArchive.CapturePhysical(physicalSource, isDirectory: true, cancellationToken);
                        var data = new AssetHistoryData(AssetHistoryOperationKind.CreateAsset, targetPath, string.Empty, isDirectory: true, archive);
                        cancellationToken.ThrowIfCancellationRequested();
                        return EditorHistoryPayload.FromBytes(data.Encode());
                    }, cancellationToken).ConfigureAwait(false);
                });
                return;
            }

            if (!import.isValidationComplete)
                return;
            import.Commit(_ =>
            {
                m_assets.interactions.history.RecordApplied(
                    "Import Sample",
                    new EditorHistoryChange(AssetHistoryKinds.SourceOperation, m_historyPayload ?? throw new InvalidOperationException("Validated sample history is missing."))
                );
            });
            m_log.Write(LogLevel.Info, "Imported sample to '{0}'.", [import.target]);
            m_assets.SelectPath(import.target.ToString());
            ClearImport();
        }
        catch (Exception pending) when (RetirementPendingException.Find(pending) is not null)
        {
            if (m_import?.isFaulted == true)
                ReportFault(pending);
        // The next frame retries retirement while the modal and dependency ownership remain intact.
        }
        catch (Exception failure)
        {
            m_failure = failure;
            CancelCurrent();
            if (m_import?.isFaulted == true)
                ReportFault(failure);
        }
    }

    /// <summary>
    /// Cancels and retires pending work before the scripting and asset dependencies stop.
    /// </summary>
    /// <param name="context">
    /// The stopping Editor's interaction services.
    /// </param>
    protected override void OnStop(EditorContext context)
    {
        CancelCurrent();
        RetireImport();
    }

    /// <summary>
    /// Retries retirement without releasing dependencies still used by background work.
    /// </summary>
    protected override void OnDispose() => RetireImport();
    private void RetireImport()
    {
        m_import?.Rollback();
        if (m_failure is not null)
            m_log.Write(LogLevel.Error, "Sample import failed: {0}", [m_failure]);
        else if (m_import is not null)
            m_log.Write(LogLevel.Info, "Sample import was canceled; no copy was published.");
        ClearImport();
    }

    private void ClearImport()
    {
        m_historyPayload?.Dispose();
        lock (m_progressLock)
        {
            m_acceptProgress = false;
            m_progress = m_progress with
            {
                fraction = m_canceled || m_failure is not null ? m_progress.fraction : 1f,
                status = m_failure?.Message ?? (m_canceled ? "Sample import canceled." : "Sample import completed.")
            };
        }
        m_import = null;
        m_historyPayload = null;
        m_failure = null;
        m_validating = false;
        m_canceled = false;
    }

    private void ReportFault(Exception failure)
    {
        m_faulted = true;
        m_failure = failure;
        m_log.Write(LogLevel.Error, "Sample import publication or retirement faulted the Host; restart is required: {0}", [failure]);
    }

    private void SetProgress(
        float progress,
        string status,
        long? operationId = null
    ) {
        lock (m_progressLock)
        {
            if (!m_acceptProgress || m_progress.cancellationRequested || (operationId is long expected && expected != m_progress.operationId))
                return;
            m_progress = m_progress with { fraction = progress, status = status };
        }
    }

    private sealed class ImportProgress(
        AssetSampleImportModule owner,
        long operation
    ) : IProgress<ScriptCompilationProgress>
    {
        /// <inheritdoc />
        public void Report(ScriptCompilationProgress value) => owner.SetProgress(0.2f + value.fraction * 0.6f, value.stage, operation);
    }
}
