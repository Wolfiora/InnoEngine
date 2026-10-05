using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using Inno.Adapter;
using Inno.Adapter.Input;
using Inno.Adapter.Rendering;
using Inno.Core.Events;
using Inno.Core.Execution;
using Inno.Platform;
using Inno.Rendering;
using Inno.Runtime;
using Inno.Runtime.Contracts;

namespace Inno.Shell;

/// <summary>
/// Owns the backend-neutral window, event pump, input source, rendering device, and host frame lifecycle.
/// </summary>
public abstract class Shell : IDisposable
{
    private readonly IAdapterCatalog m_adapterCatalog;
    private readonly AdapterSelection m_adapterSelection;
    private readonly bool m_suspendWhenHidden;
    private readonly RetirementBarrier m_retirement = new("Composition Shell");
    private IPlatformApplication? m_platformApplication;
    private IPlatformWindow? m_primaryWindow;
    private IInputEventSource? m_inputSource;
    private IRenderDevice? m_renderDevice;
    private RuntimeSubsystemPipeline? m_subsystems;
    private bool m_exitRequested;
    private bool m_hasRun;
    private bool m_stoppingNotified;
    private bool m_disposed;
    private bool m_frameActive;
    private Stopwatch? m_timer;
    private double m_previousTime;
    private int m_frameCount;
    private int? m_smokeFrameLimit;
    private bool? m_appliedVerticalSync;
    private bool m_allowBlockingPacing;
    private bool m_applicationSuspended;
    private bool m_primaryWindowHidden;

    /// <summary>
    /// Creates the backend-neutral host resources shared by Player and Editor products.
    /// </summary>
    /// <param name="adapterCatalog">
    /// Catalog that resolves explicit backend selections without leaking implementation types.
    /// </param>
    /// <param name="options">
    /// Primary-window and rendering policy.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="adapterCatalog"/> or <paramref name="options"/> is null.
    /// </exception>
    protected Shell(
        IAdapterCatalog adapterCatalog,
        ShellOptions options
    ) {
        m_adapterCatalog = adapterCatalog ?? throw new ArgumentNullException(nameof(adapterCatalog));
        ArgumentNullException.ThrowIfNull(options);
        m_adapterSelection = options.adapters;
        m_suspendWhenHidden = options.suspendWhenHidden;
        this.options = options;
        framePacing = new FramePacingOptions { verticalSync = options.verticalSync };
    }

    /// <summary>
    /// Gets the mutable presentation cadence applied at the next complete host frame.
    /// Zero maximum frame rate means no software frame limit.
    /// </summary>
    public FramePacingOptions framePacing { get; }

    /// <summary>
    /// Gets the current owner-thread lifecycle state, including suspension and failed retirement.
    /// </summary>
    public ShellState state { get; private set; }

    /// <summary>
    /// Runs one shell lifecycle using an explicitly selected owner-thread frame driver.
    /// </summary>
    /// <param name="driver">
    /// The host's scheduling policy; it must execute frames on their owning thread.
    /// </param>
    /// <param name="smokeFrameLimit">
    /// An optional positive frame count for bounded verification.
    /// </param>
    /// <param name="cancellationToken">
    /// Requests termination before the next frame.
    /// </param>
    /// <returns>
    /// Zero after orderly shutdown; lifecycle and scheduling failures propagate.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The frame driver is null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The smoke frame limit is not positive.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Adapter resources are missing or the shell has already run.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Scheduling was canceled.
    /// </exception>
    public async Task<int> RunAsync(
        IShellFrameDriver driver,
        int? smokeFrameLimit = null,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(driver);
        PrepareRun(smokeFrameLimit, driver.allowsBlockingPacing);
        platformApplication.redrawRequested += Redraw;
        Exception? failure = null;
        try
        {
            state = ShellState.Running;
            OnStarting();
            await driver.RunAsync(AdvanceFrame, cancellationToken);
            CompleteSmokeRun();
        }
        catch (OperationCanceledException canceled) when (cancellationToken.IsCancellationRequested)
        {
            failure = canceled;
        }
        catch (Exception executionFailure)
        {
            state = ShellState.Faulted;
            failure = executionFailure;
        }
        finally
        {
            platformApplication.redrawRequested -= Redraw;
            try
            {
                NotifyStopping();
            }
            catch (Exception stoppingFailure)
            {
                state = ShellState.Faulted;
                failure = failure is null
                    ? stoppingFailure
                    : new AggregateException("Shell execution and stopping both failed.", failure, stoppingFailure);
            }
        }
        if (failure is not null)
            ExceptionDispatchInfo.Capture(failure).Throw();
        return 0;
    }

    /// <summary>
    /// Releases product resources followed by rendering, input, window, and platform resources.
    /// </summary>
    public void Dispose()
    {
        if (m_disposed)
            return;
        List<Exception>? failures = null;
        Release(NotifyStopping, ref failures);
        Release(RetireProductResources, ref failures);
        if (m_renderDevice is not null)
            Release(m_renderDevice.Dispose, ref failures);
        if (m_inputSource is not null)
            Release(m_inputSource.Dispose, ref failures);
        if (m_primaryWindow is not null)
            Release(m_primaryWindow.Dispose, ref failures);
        if (m_platformApplication is not null)
            Release(m_platformApplication.Dispose, ref failures);
        m_disposed = true;
        if (failures is not null)
        {
            state = ShellState.Faulted;
            throw new AggregateException("One or more composition-shell resources could not be released.", failures);
        }
        state = ShellState.Disposed;
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Gets the implementation-neutral adapter catalog used by this product host.
    /// </summary>
    protected IAdapterCatalog adapters => m_adapterCatalog;

    /// <summary>
    /// Gets the immutable backend selection used by this product host.
    /// </summary>
    protected AdapterSelection adapterSelection => m_adapterSelection;

    /// <summary>
    /// Gets the active backend-neutral platform application.
    /// </summary>
    protected IPlatformApplication platformApplication
        => m_platformApplication
           ?? throw new InvalidOperationException("The composition shell adapter resources are not initialized.");

    /// <summary>
    /// Gets the active primary platform window.
    /// </summary>
    protected IPlatformWindow primaryWindow
        => m_primaryWindow
           ?? throw new InvalidOperationException("The composition shell adapter resources are not initialized.");

    /// <summary>
    /// Gets the shared input event source used to create isolated runtime-session backends.
    /// </summary>
    protected IInputEventSource inputSource
        => m_inputSource
           ?? throw new InvalidOperationException("The composition shell adapter resources are not initialized.");

    /// <summary>
    /// Gets the active backend-neutral rendering device.
    /// </summary>
    protected IRenderDevice renderDevice
        => m_renderDevice
           ?? throw new InvalidOperationException("The composition shell adapter resources are not initialized.");

    /// <summary>
    /// Gets the immutable shell creation options.
    /// </summary>
    protected ShellOptions options { get; }

    /// <summary>
    /// Gets whether at least one complete product frame has run.
    /// </summary>
    protected bool hasCompletedFrame { get; private set; }

    /// <summary>
    /// Borrows the EngineHost-owned pipeline used for application-wide output and services.
    /// </summary>
    /// <param name="pipeline">
    /// The started pipeline; its EngineHost remains responsible for retirement.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// A host pipeline is already configured.
    /// </exception>
    protected void UseHostPipeline(RuntimeSubsystemPipeline pipeline)
    {
        ArgumentNullException.ThrowIfNull(pipeline);
        if (m_subsystems is not null)
            throw new InvalidOperationException("The Shell already has a host subsystem pipeline.");
        m_subsystems = pipeline;
    }

    /// <summary>
    /// Creates common adapter resources after a derived product has prepared its non-window bootstrap state.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when common adapter resources are already initialized.
    /// </exception>
    protected void InitializeAdapterResources()
    {
        ObjectDisposedException.ThrowIf(m_disposed, this);
        if (state != ShellState.Created)
            throw new InvalidOperationException("The composition shell cannot initialize adapter resources in its current state.");
        m_adapterSelection.Validate(m_adapterCatalog);

        try
        {
            m_platformApplication = m_adapterCatalog.platform.CreateApplication(m_adapterSelection.platform);
            m_primaryWindow = m_platformApplication.CreateWindow(options.window);
            m_inputSource = m_adapterCatalog.input.CreateEventSource(
                m_adapterSelection.input,
                m_primaryWindow,
                acceptAllWindows: false);
            m_renderDevice = m_adapterCatalog.rendering.CreateDevice(
                m_adapterSelection.rendering,
                new RenderingBackendOptions
                {
                    window = m_primaryWindow,
                    preferredGraphicsApi = options.preferredGraphicsApi,
                    verticalSync = options.verticalSync,
                    sRgbBackbuffer = options.sRgbBackbuffer,
                    forceSingleThreaded = options.forceSingleThreadedRendering
                });
            state = ShellState.Ready;
        }
        catch (Exception failure)
        {
            state = ShellState.Faulted;
            List<Exception>? failures = null;
            if (m_renderDevice is not null)
                Release(m_renderDevice.Dispose, ref failures);
            m_renderDevice = null;
            if (m_inputSource is not null)
                Release(m_inputSource.Dispose, ref failures);
            m_inputSource = null;
            if (m_primaryWindow is not null)
                Release(m_primaryWindow.Dispose, ref failures);
            m_primaryWindow = null;
            if (m_platformApplication is not null)
                Release(m_platformApplication.Dispose, ref failures);
            m_platformApplication = null;
            if (failures is not null)
            {
                failures.Insert(0, failure);
                throw new AggregateException("Adapter initialization and rollback failed.", failures);
            }
            throw;
        }
    }

    /// <summary>
    /// Requests an orderly exit after the current event or frame callback completes.
    /// </summary>
    protected void RequestExit() => m_exitRequested = true;

    /// <summary>
    /// Runs once immediately before the common event and frame loop starts.
    /// </summary>
    protected virtual void OnStarting()
    {
    }

    /// <summary>
    /// Receives one backend-neutral platform event after shared input routing and close evaluation.
    /// </summary>
    /// <param name="evnt">
    /// Event produced by the active platform adapter.
    /// </param>
    protected virtual void OnEvent(Event evnt)
    {
    }

    /// <summary>
    /// Receives an effective application suspension change at an event-pump safety point.
    /// Repeated notifications and changes that leave another suspension reason active do not invoke this hook.
    /// </summary>
    /// <param name="isSuspended">
    /// Whether product frames and their clock are suspended after the change.
    /// </param>
    protected virtual void OnSuspensionChanged(bool isSuspended) { }

    /// <summary>
    /// Determines whether one event requests orderly product shutdown.
    /// </summary>
    /// <param name="evnt">
    /// Event produced by the active platform adapter.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the common loop should stop.
    /// </returns>
    protected virtual bool ShouldExit(Event evnt)
        => evnt is ApplicationQuitEvent
           || evnt is WindowCloseEvent close && close.windowId == primaryWindow.windowId;

    /// <summary>
    /// Advances one product-specific frame after all pending platform events have been dispatched.
    /// </summary>
    /// <param name="frame">
    /// Immutable timing and identity for the current shell frame.
    /// </param>
    protected abstract void OnFrame(ShellFrame frame);

    /// <summary>
    /// Submits product UI requests while the host output pipeline is open.
    /// </summary>
    /// <param name="frame">
    /// The current application frame.
    /// </param>
    protected virtual void OnPresentation(ShellFrame frame) { }

    /// <summary>
    /// Receives the final frame count only when a bounded smoke run reaches its requested frame limit.
    /// Closing a window or requesting an earlier exit does not report smoke completion.
    /// </summary>
    /// <param name="frameCount">
    /// Number of product frames completed by the smoke run.
    /// </param>
    protected virtual void OnSmokeCompleted(int frameCount)
    {
    }

    /// <summary>
    /// Runs once when the main loop is stopping while all product and adapter resources remain alive.
    /// </summary>
    protected virtual void OnStopping()
    {
    }

    /// <summary>
    /// Releases resources owned by the derived product before common adapter resources are destroyed.
    /// </summary>
    protected virtual void DisposeProductResources()
    {
    }

    private bool AdvanceFrame()
    {
        if (m_exitRequested || primaryWindow.isClosed)
            return false;
        PumpEvents();
        if (m_exitRequested || primaryWindow.isClosed)
            return false;
        if (state == ShellState.Running)
            DrawFrame();
        else if (m_allowBlockingPacing)
            Thread.Sleep(10);
        return !m_exitRequested && !primaryWindow.isClosed;
    }

    private void PrepareRun(
        int? smokeFrameLimit,
        bool allowBlockingPacing
    ) {
        ObjectDisposedException.ThrowIf(m_disposed, this);
        if (m_platformApplication is null || m_primaryWindow is null ||
            m_inputSource is null || m_renderDevice is null)
        {
            throw new InvalidOperationException("The composition shell adapter resources are not initialized.");
        }
        if (smokeFrameLimit is <= 0)
            throw new ArgumentOutOfRangeException(nameof(smokeFrameLimit));
        if (m_hasRun || state != ShellState.Ready)
            throw new InvalidOperationException("A composition shell can run only once.");
        m_hasRun = true;
        m_timer = Stopwatch.StartNew();
        m_smokeFrameLimit = smokeFrameLimit;
        m_allowBlockingPacing = allowBlockingPacing;
    }

    private void Redraw(uint windowId)
    {
        if (state != ShellState.Running || !hasCompletedFrame || m_frameActive || m_exitRequested || primaryWindow.isClosed)
            return;
        if (windowId == primaryWindow.windowId && primaryWindow.pixelWidth > 0 && primaryWindow.pixelHeight > 0)
            renderDevice.ResizeBackbuffer(primaryWindow.pixelWidth, primaryWindow.pixelHeight);
        DrawFrame();
    }

    private void DrawFrame()
    {
        m_frameActive = true;
        try
        {
            bool verticalSync = framePacing.verticalSync;
            if (m_appliedVerticalSync != verticalSync)
            {
                renderDevice.SetVerticalSync(verticalSync);
                m_appliedVerticalSync = verticalSync;
            }
            double totalTime = m_timer!.Elapsed.TotalSeconds;
            float deltaTime = Math.Max(0f, (float)(totalTime - m_previousTime));
            var frame = new ShellFrame(m_frameCount, totalTime, deltaTime);
            var runtimeFrame = new RuntimeFrame(m_frameCount, (float)totalTime, (float)totalTime,
                deltaTime, deltaTime, 1f, false);
            try
            {
                m_subsystems?.BeginFrame(runtimeFrame);
                OnFrame(frame);
                m_subsystems?.Update(runtimeFrame);
                m_subsystems?.LateUpdate(runtimeFrame);
                if (m_subsystems is not null)
                    m_subsystems.RenderFrame(runtimeFrame, () => OnPresentation(frame));
                else
                    OnPresentation(frame);
            }
            finally
            {
                m_subsystems?.EndFrame(runtimeFrame);
            }
            m_previousTime = totalTime;
            m_frameCount++;
            hasCompletedFrame = true;
            if (m_smokeFrameLimit.HasValue && m_frameCount >= m_smokeFrameLimit.Value)
                RequestExit();
            int maximumRate = framePacing.maximumFrameRate;
            if (m_allowBlockingPacing && maximumRate > 0)
            {
                double remaining = 1d / maximumRate - (m_timer.Elapsed.TotalSeconds - totalTime);
                if (remaining > 0d)
                    Thread.Sleep(TimeSpan.FromSeconds(remaining));
            }
        }
        finally
        {
            m_frameActive = false;
        }
    }

    private void CompleteSmokeRun()
    {
        if (m_smokeFrameLimit.HasValue && m_frameCount >= m_smokeFrameLimit.Value)
            OnSmokeCompleted(m_frameCount);
    }

    private void PumpEvents()
    {
        while (platformApplication.PollEvent(out Event? evnt))
        {
            if (evnt is null)
                continue;
            ApplySuspension(evnt);
            if (state == ShellState.Suspended && evnt is KeyEvent or MouseEvent or TextInputEvent)
                continue;
            if (evnt is not ApplicationSuspensionChangedEvent)
                inputSource.ProcessEvent(evnt);
            if (evnt is WindowResizeEvent resize && resize.windowId == primaryWindow.windowId)
                renderDevice.ResizeBackbuffer(primaryWindow.pixelWidth, primaryWindow.pixelHeight);
            if (ShouldExit(evnt))
            {
                RequestExit();
                break;
            }
            OnEvent(evnt);
        }
    }

    private void RetireProductResources()
    {
        try
        {
            m_retirement.Wait(DisposeProductResources);
        }
        catch
        {
            state = ShellState.Faulted;
            throw;
        }
    }

    private void ApplySuspension(Event evnt)
    {
        switch (evnt)
        {
            case ApplicationSuspensionChangedEvent application:
                m_applicationSuspended = application.isSuspended;
                break;
            case WindowVisibilityChangedEvent window when
                m_suspendWhenHidden && window.windowId == primaryWindow.windowId:
                m_primaryWindowHidden = !window.isVisible;
                break;
            default:
                return;
        }
        bool suspended = m_applicationSuspended || m_primaryWindowHidden;
        if (suspended == (state == ShellState.Suspended))
            return;
        if (suspended)
        {
            m_timer!.Stop();
            state = ShellState.Suspended;
        }
        else
        {
            m_timer!.Start();
            state = ShellState.Running;
        }
        inputSource.ProcessEvent(new ApplicationSuspensionChangedEvent(suspended));
        OnSuspensionChanged(suspended);
    }

    private void NotifyStopping()
    {
        if (m_stoppingNotified)
            return;
        m_stoppingNotified = true;
        bool faulted = state == ShellState.Faulted;
        state = ShellState.Stopping;
        try
        {
            OnStopping();
            state = faulted ? ShellState.Faulted : ShellState.Stopped;
        }
        catch
        {
            state = ShellState.Faulted;
            throw;
        }
    }

    private static void Release(
        Action release,
        ref List<Exception>? failures
    ) {
        try
        {
            release();
        }
        catch (Exception pendingRetirement) when (RetirementPendingException.Find(pendingRetirement) is not null)
        {
            throw;
        }
        catch (Exception exception)
        {
            failures ??= [];
            failures.Add(exception);
        }
    }
}
