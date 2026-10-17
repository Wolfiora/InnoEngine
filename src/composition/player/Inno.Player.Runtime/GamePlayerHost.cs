using Inno.Core.IO;
using Inno.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Adapter;
using Inno.Adapter.Audio;
using Inno.Assets;
using Inno.Animation;
using Inno.Animation.Runtime;
using Inno.Audio;
using Inno.Audio.Runtime;
using Inno.Core.Events;
using Inno.Core.Diagnostics;
using Inno.Core.Execution;
using Inno.Core.Logging;
using Inno.Core.Serialization;
using Inno.Core.Settings;
using Inno.Extensibility.Modules;
using Inno.Engine.Default;
using Inno.Input.Runtime;
using Inno.Runtime;
using Inno.Runtime.Contracts;
using Inno.Scene;
using Inno.Platform;
using Inno.Rendering;
using Inno.Rendering.Runtime;
using Inno.Shell;
using Inno.Storage.Runtime;
using Inno.Storage;
using Inno.UI.Runtime;
using ShellHost = Inno.Shell.Shell;

namespace Inno.Player.Runtime;

internal sealed class GamePlayerHost : ShellHost
{
    private readonly EngineHost m_engine;
    private IRuntimeContentStore? m_content;
    private ProjectSettingsStore? m_settings;
    private RuntimeSession? m_session;
    private RenderRuntime? m_rendering;
    private DiagnosticLogSink? m_diagnosticLogs;
    private DiagnosticReporter? m_renderDiagnostics;

    private GamePlayerHost(
        IAdapterCatalog adapterCatalog,
        ShellOptions shellOptions,
        EngineHost engine,
        PlayerLaunchOptions options
    )
        : base(adapterCatalog, shellOptions)
    {
        m_engine = engine;
        m_engine.logs.RegisterSink(new ConsoleLogSink(options.consoleColors));
    }

    internal static async Task<GamePlayerHost> CreateAsync(
        PlayerLaunchOptions options,
        CancellationToken cancellationToken
    ) {
        ArgumentNullException.ThrowIfNull(options);
        EngineHost engine = new EngineHostBuilder()
            .UseMetadataSources(options.modules, options.types, options.serializationMetadata)
            .UseLogDelivery(options.logDeliveryMode)
            .Build();
        GamePlayerHost? host = null;
        IRuntimeContentStore? content = null;
        try
        {
            PlayerContentMetadata metadata = await options.contentSource.ReadMetadataAsync(cancellationToken);
            using SerializationGeneration serialization = engine.serialization.CaptureGeneration();
            GameRuntimeManifest manifest = RuntimeManifestEnvelope.Decode(metadata.manifest.Span, serialization);
            RuntimeContentCatalog catalog = serialization.Deserialize<RuntimeContentCatalog>(metadata.catalog.Span);
            catalog.Validate();
            if (catalog.runtimeAssemblyCount != manifest.modules.Sum(static module => module.assemblies.Length))
                throw new InvalidDataException("The content catalog and runtime manifest have different code closures.");
            ActivateRuntimeModules(
                options.moduleActivator,
                engine.modules,
                GameCodeDeployment.FromManifest(manifest.modules));
            var pack = new ContentPackDescriptor(catalog.contentHash, catalog.packFileName);
            content = await options.contentSource.PrepareAsync(
                pack,
                new StorageScope(manifest.applicationId),
                serialization,
                cancellationToken);
            if (content is null)
                throw new InvalidOperationException("The Player content source returned no prepared store.");
            if (content.descriptor != pack)
                throw new InvalidDataException("The prepared Player content store differs from the selected deployment pack.");
            cancellationToken.ThrowIfCancellationRequested();
            host = new GamePlayerHost(
                options.adapters,
                new ShellOptions
                {
                    adapters = options.adapterSelection,
                    window = new PlatformWindowOptions
                    {
                        title = manifest.productName,
                        width = manifest.windowWidth,
                        height = manifest.windowHeight,
                        resizable = true,
                        highPixelDensity = true,
                        visible = options.windowVisible
                    },
                    preferredGraphicsApi = options.graphicsApi,
                    verticalSync = true,
                    sRgbBackbuffer = true,
                    forceSingleThreadedRendering = options.renderOnCallingThread,
                    suspendWhenHidden = options.windowVisible
                },
                engine, options);
            host.m_content = content;
            content = null;
            host.InitializeRuntime(manifest, options);
            return host;
        }
        catch
        {
            if (host is not null)
            {
                host.Dispose();
            }
            else
            {
                try
                {
                    content?.Dispose();
                }
                finally
                {
                    engine.Dispose();
                }
            }
            throw;
        }
    }

    internal async Task<int> RunGameAsync(
        IShellFrameDriver frameDriver,
        int? smokeFrameLimit = null,
        CancellationToken cancellationToken = default
    ) {
        using IDisposable scope = settings.EnterExecutionScope();
        using IDisposable sessionScope = session.EnterExecutionScope();
        using IDisposable uiScope = session.subsystems.GetRequiredSubsystem<UiRuntime>().EnterExecutionScope();
        return await RunAsync(frameDriver, smokeFrameLimit, cancellationToken);
    }

    /// <summary>
    /// Routes one backend-neutral platform event into the active game runtime session.
    /// </summary>
    /// <param name="evnt">
    /// Event produced by the common shell.
    /// </param>
    protected override void OnEvent(Event evnt)
    {
        if (evnt is not ApplicationSuspensionChangedEvent)
            session.events.Enqueue(evnt);
    }

    /// <inheritdoc />
    protected override void OnSuspensionChanged(bool isSuspended)
        => session.events.Emit(new ApplicationSuspensionChangedEvent(isSuspended));

    /// <summary>
    /// Advances the active game runtime session for one common shell frame.
    /// </summary>
    /// <param name="frame">
    /// Immutable timing and identity for the current shell frame.
    /// </param>
    protected override void OnFrame(ShellFrame frame) => session.Tick(frame.deltaTime);

    /// <summary>
    /// Writes deterministic rendering statistics after a bounded smoke run completes.
    /// </summary>
    /// <param name="frameCount">
    /// Number of game frames completed.
    /// </param>
    protected override void OnSmokeCompleted(int frameCount)
    {
        var diagnostics = new SmokeDiagnostics();
        m_engine.diagnostics.RegisterSink(diagnostics);
        m_engine.diagnostics.UnregisterSink(diagnostics);
        if (diagnostics.errors.Count != 0)
            throw new InvalidOperationException("Player smoke completed with active errors:" + Environment.NewLine
                + string.Join(Environment.NewLine, diagnostics.errors));
        RenderFrameStatistics? statistics;
        using (rendering.EnterExecutionScope())
            statistics = GraphicsSettings.frameStatistics;
        Console.WriteLine(
            $"INNO-SMOKE frames={frameCount} "
            + $"views={statistics?.viewCount ?? 0} "
            + $"draws={statistics?.drawCount ?? 0} "
            + $"dispatches={statistics?.dispatchCount ?? 0}");
    }

    /// <summary>
    /// Releases game runtime resources before the common shell destroys shared adapters.
    /// </summary>
    protected override void DisposeProductResources()
    {
        List<Exception> failures = [];
        Attempt(() => m_session?.Dispose());
        m_session = null;
        m_rendering = null;
        Attempt(() => m_settings?.Dispose());
        m_settings = null;
        Attempt(() => m_content?.Dispose());
        m_content = null;
        Attempt(m_engine.Dispose);
        Attempt(() => m_renderDiagnostics?.Dispose());
        m_renderDiagnostics = null;
        Attempt(() => m_diagnosticLogs?.Dispose());
        m_diagnosticLogs = null;
        if (failures.Count > 0)
            throw new AggregateException("Player retirement failed after all available owners were attempted.", failures);

        void Attempt(Action cleanup)
        {
            try
            {
                cleanup();
            }
            catch (Exception pendingRetirement) when (RetirementPendingException.Find(pendingRetirement) is not null)
            {
                throw;
            }
            catch (Exception exception)
            {
                failures.Add(exception);
            }
        }
    }

    private RuntimeSession session => m_session ?? throw new InvalidOperationException("The Player runtime session is not initialized.");

    private ProjectSettingsStore settings
        => m_settings ?? throw new InvalidOperationException("The Player settings owner is not initialized.");

    private RenderRuntime rendering
        => m_rendering ?? throw new InvalidOperationException("The Player rendering runtime is not initialized.");

    private static RenderViewport CreatePresentationViewport(
        GamePresentationSettings presentation,
        RenderPresentationSize size
    ) {
        GamePresentationViewport viewport = presentation.CalculateViewport(size.width, size.height);
        return new RenderViewport(viewport.x, viewport.y, viewport.width, viewport.height);
    }

    private void InitializeRuntime(
        GameRuntimeManifest manifest,
        PlayerLaunchOptions options
    ) {
        InitializeAdapterResources();
        m_diagnosticLogs = new DiagnosticLogSink(m_engine.diagnostics, m_engine.logs);
        m_renderDiagnostics = m_engine.diagnostics.CreateReporter(new DiagnosticSource("inno.player.rendering", "Rendering"));
        m_session = m_engine.CreateSession(new RuntimeSessionOptions
        {
            kind = RuntimeSessionKind.Player,
            jobExecutionMode = options.jobExecutionMode,
            applicationId = manifest.applicationId,
            contentStore = m_content,
            createLogSink = options.createLogSink is null ? null : id => options.createLogSink(manifest, id),
            createSubsystems = owner =>
            {
                using ContentReadLease document = m_content!.Acquire(new ContentKey(SettingsFileNames.project));
                using Stream documentStream = document.OpenRead();
                byte[] documentBytes = new byte[checked((int)document.entry.length)];
                documentStream.ReadExactly(documentBytes);
                m_settings = new ProjectSettingsStore(
                    new ReadOnlyByteDocumentStore(document.entry.key.value!, documentBytes),
                    m_engine.types, m_engine.serialization, new ProjectId(manifest.applicationId),
                    AssetSerializationContext.Create(owner.assets));
                settings.SetContributors(manifest.CreateSettingContributors());
                settings.RebuildCurrent();
                if (!string.Equals(settings.projectId.value, manifest.applicationId, StringComparison.Ordinal))
                    throw new InvalidDataException("Runtime manifest and Project Settings identities do not match.");
                return DefaultEngine.CreateSessionSubsystems(new EngineSessionComposition(
                    owner, adapters, adapterSelection, inputSource, owner.assets,
                    () => settings.Get<AudioProjectSettings>(AudioProjectSettings.settingId),
                    () => options.createStorage(manifest)));
            }
        });
        GamePresentationSettings presentation = settings.Get<GamePresentationSettings>(GamePresentationSettings.settingId);
        AudioRuntime audio = session.subsystems
            .GetRequiredSubsystem<AudioRuntime>();
        AnimationRuntime animation = session.subsystems
            .GetRequiredSubsystem<AnimationRuntime>();
        InputRuntime inputRuntime = session.subsystems
            .GetRequiredSubsystem<InputRuntime>();
        m_rendering = new RenderRuntime(m_engine.types, renderDevice, m_renderDiagnostics,
            targetArtifacts: new ContentRenderTargetArtifactProvider(m_content!, m_engine.serialization,
                AssetSerializationContext.Create(session.assets)),
            contentScopeProvider: () => SceneContentSource.CreateScope(session.scenes),
            primaryPresentationViewportProvider: size => CreatePresentationViewport(presentation, size),
            inputSnapshotProvider: () => inputRuntime.snapshot,
            primaryInputSurfaceSizeProvider: () => primaryWindow.width > 0 && primaryWindow.height > 0
                ? new RenderPresentationSize(primaryWindow.width, primaryWindow.height) : null,
            compositionProgramProvider: adapters.rendering.CreateCompositionProgramProvider(
                adapterSelection.rendering));
        UseHostPipeline(m_engine.CreateHostPipeline(DefaultEngine.CreateHostSubsystems(m_rendering)));
        using (settings.EnterExecutionScope())
        using (AnimationExecutionContext.EnterScope(animation))
        using (audio.EnterExecutionScope())
        using (rendering.EnterExecutionScope())
        using (session.EnterExecutionScope())
        {
            SceneAsset startupAsset = session.assets.Load<SceneAsset>(
                AssetPath.Parse(manifest.startupScene));
            session.scenes.LoadScene(startupAsset.Instantiate(
                m_engine.serialization,
                session.assets));
        }
    }

    private static void ActivateRuntimeModules(
        IPlayerModuleActivator activator,
        ModuleHost modules,
        GameCodeDeployment deployment
    ) {
        ArgumentNullException.ThrowIfNull(activator);
        ArgumentNullException.ThrowIfNull(modules);
        ArgumentNullException.ThrowIfNull(deployment);
        activator.Activate(modules, deployment);
        string[] activeNames = modules.modules.Select(static module => module.moduleName)
            .Order(StringComparer.Ordinal).ToArray();
        string[] expectedNames = deployment.modules.Select(static module => module.name)
            .Order(StringComparer.Ordinal).ToArray();
        if (!activeNames.SequenceEqual(expectedNames, StringComparer.Ordinal))
            throw new InvalidOperationException("The frozen runtime code generation was not activated completely.");
    }

    private sealed class SmokeDiagnostics : IDiagnosticSink
    {
        internal readonly List<string> errors = [];
        /// <summary>
        /// Records errors from the current diagnostic report.
        /// </summary>
        /// <param name="report">
        /// The report consumed by replace; ownership remains with the caller unless explicitly stated otherwise.
        /// </param>
        public void Replace(DiagnosticReport report)
        {
            foreach (Diagnostic diagnostic in report.diagnostics)
                if (diagnostic.severity == DiagnosticSeverity.Error)
                    errors.Add(report.source.id + "/" + diagnostic.code + ": " + diagnostic.message);
        }
        /// <summary>
        /// Removes all retained entries and returns the instance to an empty reusable state.
        /// </summary>
        /// <param name="source">
        /// The source value or location read by this operation.
        /// </param>
        public void Clear(DiagnosticSource source) { }
    }
}
