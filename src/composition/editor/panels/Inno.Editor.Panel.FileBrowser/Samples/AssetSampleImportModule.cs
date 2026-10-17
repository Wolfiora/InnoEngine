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

namespace Inno.Editor.Panel.FileBrowser;

[EditorModule("asset-sample-import", order: 110)]
internal sealed class AssetSampleImportModule : EditorModule, IProgress<ScriptCompilationProgress>
{
    private readonly object m_progressLock = new();
    private readonly AssetEditorModule m_assets;
    private readonly ScriptCompiler m_compiler;
    private readonly IEditorScriptCompilation m_scripting;
    private readonly Logger m_log;
    private AssetSampleImportTransaction? m_import;
    private EditorHistoryPayload? m_historyPayload;
    private Exception? m_failure;
    private bool m_validating;
    private bool m_canceled;
    private bool m_faulted;
    private string m_status = string.Empty;
    private float m_progress;

    internal AssetSampleImportModule(
        AssetEditorModule assets,
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

    internal bool isBusy => !m_faulted && m_import is not null;
    internal bool canImport => !m_faulted && !isBusy && m_scripting.state is EditorScriptCompilationState.Ready or EditorScriptCompilationState.Failed;
    internal string status { get { lock (m_progressLock) return m_status; } }
    internal float progress { get { lock (m_progressLock) return m_progress; } }

    internal void Request(AssetPath source)
    {
        if (!canImport)
            throw new InvalidOperationException("Sample import requires an idle authoring generation.");
        m_import = m_assets.pipeline.PrepareSampleImport(source);
        m_canceled = false;
        SetProgress(0f, "Preparing sample assets...");
    }

    internal void Cancel()
    {
        m_canceled = true;
        m_import?.Cancel();
        SetProgress(progress, "Canceling sample import...");
    }

    /// <summary>
    /// Receives asynchronous compilation progress without accessing live Editor state.
    /// </summary>
    /// <param name="value">
    /// The compiler stage and completion fraction.
    /// </param>
    public void Report(ScriptCompilationProgress value)
        => SetProgress(0.2f + value.fraction * 0.6f, value.stage);

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
                import.BeginValidation(async (
                    sources,
                    cancellationToken
                ) =>
                {
                    ScriptCompilationResult result = await m_compiler.CompileAuthoringGenerationAsync(
                        this, cancellationToken, sources).ConfigureAwait(false);
                    if (!result.success)
                        throw new InvalidOperationException("Sample script preflight failed:" + Environment.NewLine
                            + string.Join(Environment.NewLine,
                                result.diagnostics.Select(static diagnostic => diagnostic.message)));
                    SetProgress(0.85f, "Preparing sample history...");
                    m_historyPayload = await Task.Run(() =>
                    {
                        byte[] archive = AssetSourceArchive.CapturePhysical(
                            physicalSource, isDirectory: true, cancellationToken);
                        var data = new AssetHistoryData(
                            AssetHistoryOperationKind.CreateAsset, targetPath, string.Empty,
                            isDirectory: true, archive);
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
                    "Import Sample", new EditorHistoryChange(
                        AssetHistoryKinds.SourceOperation,
                        m_historyPayload ?? throw new InvalidOperationException("Validated sample history is missing.")));
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
            Cancel();
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
        Cancel();
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
        m_import = null;
        m_historyPayload?.Dispose();
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
        string status
    ) {
        lock (m_progressLock)
        {
            m_progress = progress;
            m_status = status;
        }
    }
}
