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
using Inno.UI.Runtime;
using ShellHost = Inno.Shell.Shell;

namespace Inno.Player.Runtime;

internal sealed class GamePlayerHost : ShellHost
{
    private readonly EngineHost m_engine;
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

    internal static GamePlayerHost Create(PlayerLaunchOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        string packagedContentRoot = Path.GetFullPath(options.contentDirectory);
        byte[] manifestEnvelope = File.ReadAllBytes(Path.Combine(packagedContentRoot, "runtime.manifest"));
        string persistentDataPath = RuntimeManifestEnvelope.ReadPersistentDataPath(manifestEnvelope);
        string persistentRoot = Path.GetFullPath(Path.Combine(options.persistentDataRoot, persistentDataPath));
        string runtimeContentRoot = RuntimeContentDeployment.Materialize(
            packagedContentRoot,
            persistentRoot);
        EngineHost engine = new EngineHostBuilder()
            .UseMetadataCache(Path.Combine(persistentRoot, "Library", "RuntimeMetadata"))
            .UseLogDelivery(options.logDeliveryMode)
            .Build();
        GamePlayerHost? host = null;
        try
        {
            using SerializationGeneration serialization = engine.serialization.CaptureGeneration();
            GameRuntimeManifest manifest = RuntimeManifestEnvelope.Decode(manifestEnvelope, serialization);
            ActivateRuntimeModules(
                options.moduleActivator,
                engine.modules,
                manifest.modules,
                Path.Combine(runtimeContentRoot, "Managed"));
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
                        highPixelDensity = true
                    },
                    preferredGraphicsApi = options.graphicsApi,
                    verticalSync = true,
                    sRgbBackbuffer = true,
                    forceSingleThreadedRendering = options.renderOnCallingThread
                },
                engine, options);
            host.InitializeRuntime(
                manifest,
                runtimeContentRoot,
                persistentRoot, options.jobExecutionMode);
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
                engine.Dispose();
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
    protected override void OnEvent(Event evnt) => session.events.Enqueue(evnt);

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
        string runtimeContentRoot,
        string persistentRoot,
        RuntimeJobExecutionMode jobExecutionMode
    ) {
        InitializeAdapterResources();
        m_diagnosticLogs = new DiagnosticLogSink(m_engine.diagnostics, m_engine.logs);
        m_renderDiagnostics = m_engine.diagnostics.CreateReporter(new DiagnosticSource("inno.player.rendering", "Rendering"));
        m_session = m_engine.CreateSession(new RuntimeSessionOptions
        {
            kind = RuntimeSessionKind.Player,
            jobExecutionMode = jobExecutionMode,
            applicationId = manifest.applicationId,
            runtimeContentDirectory = runtimeContentRoot,
            persistentDataDirectory = persistentRoot,
            createSubsystems = owner =>
            {
                m_settings = new ProjectSettingsStore(
                    Path.Combine(runtimeContentRoot, SettingsFileNames.project),
                    m_engine.types, m_engine.serialization, new ProjectId(manifest.applicationId),
                    AssetSerializationContext.Create(owner.assets));
                settings.SetContributors(manifest.CreateSettingContributors());
                settings.RebuildCurrent();
                if (!string.Equals(settings.projectId.value, manifest.applicationId, StringComparison.Ordinal))
                    throw new InvalidDataException("Runtime manifest and Project Settings identities do not match.");
                return DefaultEngine.CreateSessionSubsystems(new EngineSessionComposition(
                    owner, adapters, adapterSelection, inputSource, owner.assets,
                    () => settings.Get<AudioProjectSettings>(AudioProjectSettings.settingId)));
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
            targetArtifacts: new FileRenderTargetArtifactProvider(runtimeContentRoot, m_engine.serialization,
                AssetSerializationContext.Create(session.assets)),
            contentScopeProvider: () => SceneContentSource.CreateScope(session.scenes),
            primaryPresentationViewportProvider: size => CreatePresentationViewport(presentation, size),
            inputSnapshotProvider: () => inputRuntime.snapshot,
            primaryInputSurfaceSizeProvider: () => new RenderPresentationSize(
                Math.Max(1, primaryWindow.width), Math.Max(1, primaryWindow.height)),
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
        IReadOnlyList<GameRuntimeModule> deployedModules,
        string managedRoot
    ) {
        ArgumentNullException.ThrowIfNull(modules);
        ArgumentNullException.ThrowIfNull(deployedModules);
        string root = Path.GetFullPath(managedRoot);
        if (!Directory.Exists(root))
            throw new DirectoryNotFoundException($"Deployed managed content root '{root}' does not exist.");

        string[] declaredFiles = deployedModules
            .SelectMany(static module => module.preloadAssemblies.Prepend(module.mainAssembly))
            .ToArray();
        string[] actualFiles = Directory.EnumerateFiles(root, "*.dll", SearchOption.TopDirectoryOnly)
            .Select(Path.GetFileName)
            .Order(StringComparer.OrdinalIgnoreCase)
            .ToArray()!;
        if (!declaredFiles.Order(StringComparer.OrdinalIgnoreCase).SequenceEqual(
                actualFiles,
                StringComparer.OrdinalIgnoreCase))
        {
            throw new InvalidDataException(
                "Deployed managed assemblies do not exactly match the frozen runtime module manifest.");
        }

        activator.Activate(modules, deployedModules, root);

        string[] activeNames = modules.modules
            .Select(static module => module.moduleName)
            .Order(StringComparer.Ordinal)
            .ToArray();
        string[] expectedNames = deployedModules
            .Select(static module => module.name)
            .Order(StringComparer.Ordinal)
            .ToArray();
        if (!activeNames.SequenceEqual(expectedNames, StringComparer.Ordinal))
            throw new InvalidOperationException("The frozen runtime module generation was not activated completely.");
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
