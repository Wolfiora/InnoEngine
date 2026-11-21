using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Adapter.Content.FileSystem;
using Inno.Adapter.Modules.DotNet;
using Inno.Adapter.Serialization.DotNet;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Build;
using Inno.Build.MacOSArm64;
using Inno.Build.Managed;
using Inno.Build.WindowsX64;
using Inno.Content;
using Inno.Core.Execution;
using Inno.Core.IO;
using Inno.Core.Identity;
using Inno.Core.Serialization;
using Inno.Core.Settings;
using Inno.Extensibility.Types;
using Inno.Plugins.Authoring;
using Inno.Runtime;
using Inno.Scene;
using Inno.Scripting.Compiler;
using Inno.Testing.SupportPacks;

namespace Inno.Testing.Build;

public abstract class BuildPipelineFixture : PlayerSupportPackFixture
{
    protected readonly AssetPipeline m_assets;
    protected readonly EngineHost m_engine;
    protected readonly IdentityAllocator m_identities = new();
    protected readonly BuildPipeline m_pipeline;
    protected readonly PluginEnvironment m_plugins;
    protected readonly ScriptCompiler m_compiler;
    protected readonly string m_projectRoot;
    protected readonly ProjectSettingsStore m_settings;
    protected readonly SceneWorld m_sceneWorld;
    protected readonly IDisposable m_sceneWorldScope;

    protected BuildPipelineFixture()
        : base(
            Directory.EnumerateFiles(AppContext.BaseDirectory, "Inno.*.dll", SearchOption.TopDirectoryOnly)
                .Where(static path => !IsAuthoringAssembly(Path.GetFileName(path)))
                .ToArray()
        )
    {
        m_projectRoot = Path.Combine(m_root, "Project");
        string assetRoot = Path.Combine(m_projectRoot, "Assets");
        string pluginRoot = Path.Combine(m_projectRoot, "Plugins");
        string libraryRoot = Path.Combine(m_projectRoot, "Library");
        Directory.CreateDirectory(assetRoot);
        Directory.CreateDirectory(pluginRoot);

        m_engine = new EngineHostBuilder()
            .UseMetadataSources(
                new DotNetAssemblyCatalogSource(typeof(BuildPipelineFixture).Assembly, GetType().Assembly),
                new ReflectionTypeCatalogSource(),
                new ReflectionSerializationMetadataSource()
            )
            .Build();
        m_sceneWorld = new SceneWorld(m_identities, m_engine.types);
        m_sceneWorldScope = m_sceneWorld.EnterScope();
        var sources = new PluginSourceService(m_engine.serialization, pluginRoot, libraryRoot);
        PluginScanResult scan = sources.Scan();
        m_assets = new AssetPipeline(
            m_engine.modules,
            m_engine.types,
            m_engine.serialization,
            m_identities,
            m_engine.diagnostics,
            m_engine.logs,
            AssetPipelineOptions.Create(assetRoot, libraryRoot) with
            {
                enableFileSystemWatcher = false
            }
        );
        m_settings = new ProjectSettingsStore(
            new FileByteDocumentStore(Path.GetFullPath(Path.Combine(m_projectRoot, "Settings.Project.inno"))),
            m_engine.types,
            m_engine.serialization,
            new ProjectId("tests.build"),
            AssetSerializationContext.Create(m_assets)
        );
        m_plugins = new PluginEnvironment(
            m_assets,
            m_settings,
            m_engine.serialization,
            pluginRoot,
            libraryRoot,
            scan,
            m_engine.modules.generations
        );
        m_compiler = new ScriptCompiler(
            new ScriptCompilerOptions
            {
                projectRootDirectory = m_projectRoot,
                compilationEnvironment = new Inno.Build.SdkScriptCompilationEnvironment(
                    Path.Combine(
                        Path.GetFullPath("../../..", System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory()),
                        OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet"
                    ),
                    m_projectRoot
                )
            },
            m_assets,
            m_plugins
        );
        m_pipeline = CreatePipeline();
    }

    public override void Dispose()
    {
        m_plugins.Dispose();
        m_assets.Dispose();
        m_settings.Dispose();
        m_sceneWorldScope.Dispose();
        m_sceneWorld.Dispose();
        m_engine.Dispose();
        base.Dispose();
    }

    protected static BuildProfile CreateProfile(
        BuildTargetId target,
        string productName = "Test Game"
    ) => new()
    {
        applicationId = "tests.game",
        productName = productName,
        startupScene = "Scenes/Startup.iscene",
        target = target
    };

    protected void SaveStartupScene()
    {
        var scene = new GameScene("Startup");
        scene.CreateObject("Player");
        Assert.True(
            m_assets.Save(
                AssetPath.Project("Scenes/Startup.iscene"),
                SceneAsset.Capture(scene, m_engine.serialization, m_assets)
            )
        );
    }

    protected BuildPipeline CreatePipeline(
        IPlayerSupportPackProvisioner? provisioner = null,
        ManagedDeploymentCatalog? managedDeployments = null
    ) => new(
        m_assets,
        m_plugins,
        m_settings,
        m_engine.serialization,
        m_engine.generations,
        m_compiler,
        m_supportPackRoot,
        [
            new GameBuildTargetBinding(
                new MacOSArm64GameBuildTarget(new MacOSArm64SupportPackValidator()),
                new Inno.Build.Toolchains.Bgfx.Tools.BgfxGameContentCompiler(
                    m_assets,
                    m_engine.serialization,
                    m_engine.types,
                    Inno.Integration.MacOSArm64.Bgfx.MacOSArm64BgfxIntegration.shaderProfile,
                    [Inno.Rendering.GraphicsApi.Metal],
                    new Inno.Build.Toolchains.Bgfx.Tools.ToolRunner()
                )
            ),
            new GameBuildTargetBinding(
                new WindowsX64GameBuildTarget(new WindowsX64SupportPackValidator()),
                new Inno.Build.Toolchains.Bgfx.Tools.BgfxGameContentCompiler(
                    m_assets,
                    m_engine.serialization,
                    m_engine.types,
                    Inno.Integration.WindowsX64.Bgfx.WindowsX64BgfxIntegration.shaderProfile,
                    [
                        Inno.Rendering.GraphicsApi.Direct3D11,
                        Inno.Rendering.GraphicsApi.Direct3D12,
                        Inno.Rendering.GraphicsApi.Vulkan,
                        Inno.Rendering.GraphicsApi.OpenGL
                    ],
                    new Inno.Build.Toolchains.Bgfx.Tools.ToolRunner()
                )
            )
        ],
        managedDeployments ?? CreateManagedDeployments(),
        provisioner
    );

    protected static ManagedDeploymentCatalog CreateManagedDeployments()
        => new([new FixtureManagedDeploymentCompiler()]);

    protected sealed class CommitFailingProgress : IProgress<BuildProgress>
    {
        public void Report(BuildProgress value)
        {
            if (value.stage == "commit")
            {
                throw new InvalidOperationException("Fixture commit observer failed after publication.");
            }
        }
    }

    protected sealed class FixtureManagedDeploymentCompiler : IManagedDeploymentCompiler
    {
        public FixtureManagedDeploymentCompiler(IReadOnlyList<string>? runtimeIdentifiers = null)
        {
            capabilities = new ManagedDeploymentCapabilities(
                runtimeIdentifiers ?? ["win-x64", "osx-arm64"],
                dynamicCode: true,
                aheadOfTime: false,
                nativeStaticLinking: false
            );
        }

        public ManagedDeploymentId id => ManagedDeploymentId.coreClr;

        public ManagedDeploymentCapabilities capabilities { get; }

        public ValueTask<ManagedDeploymentResult> CompileAsync(
            ManagedDeploymentRequest request,
            CancellationToken cancellationToken = default
        ) {
            cancellationToken.ThrowIfCancellationRequested();
            string projectRoot = Path.GetDirectoryName(request.projectPath)!;
            Assert.True(File.Exists(Path.Combine(projectRoot, "PlayerDeploymentDefinition.g.cs")));
            Assert.NotEmpty(Directory.EnumerateFiles(request.codeInputDirectory, "*.dll"));
            Directory.CreateDirectory(request.outputDirectory);
            string executable = request.runtimeIdentifier == "win-x64" ? "Inno.Player.WindowsX64.exe" : "Inno.Player.MacOSArm64";
            File.Copy(Path.Combine(projectRoot, executable), Path.Combine(request.outputDirectory, executable));
            foreach (string native in Directory.EnumerateFiles(Path.Combine(projectRoot, "native")))
            {
                string destination = Path.Combine(request.outputDirectory, "native", Path.GetFileName(native));
                Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
                File.Copy(native, destination);
            }
            return ValueTask.FromResult(
                new ManagedDeploymentResult(
                    id,
                    request.outputDirectory,
                    "fixture-sdk",
                    Directory.EnumerateFiles(request.outputDirectory, "*", SearchOption.AllDirectories)
                        .Select(file => Path.GetRelativePath(request.outputDirectory, file))
                        .ToArray()
                )
            );
        }
    }

    protected sealed class PendingSupportPackProvisioner : IPlayerSupportPackProvisioner
    {
        private readonly TaskCompletionSource m_completion = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public ValueTask ProvisionAsync(
            BuildTargetId target,
            string supportPackRoot,
            CancellationToken cancellationToken = default
        ) => new(m_completion.Task.WaitAsync(cancellationToken));
    }

    protected sealed class DrainingSupportPackProvisioner : IPlayerSupportPackProvisioner
    {
        public TaskCompletionSource canceled { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
        public TaskCompletionSource cleanup { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
        public bool started { get; private set; }
        public bool completed { get; private set; }

        public async ValueTask ProvisionAsync(
            BuildTargetId target,
            string supportPackRoot,
            CancellationToken cancellationToken = default
        ) {
            started = true;
            using CancellationTokenRegistration registration = cancellationToken.Register(() => canceled.TrySetResult());
            await cleanup.Task.ConfigureAwait(false);
            completed = true;
            cancellationToken.ThrowIfCancellationRequested();
        }
    }

    protected sealed class TestSupportPackProvisioner(Action<BuildTargetId> provision) : IPlayerSupportPackProvisioner
    {
        public int callCount { get; private set; }

        public ValueTask ProvisionAsync(
            BuildTargetId target,
            string supportPackRoot,
            CancellationToken cancellationToken = default
        ) {
            _ = supportPackRoot;
            cancellationToken.ThrowIfCancellationRequested();
            callCount++;
            provision(target);
            return ValueTask.CompletedTask;
        }
    }

    protected sealed class BlockingBuildTarget : IGameBuildTarget, IGameContentCompiler
    {
        public TaskCompletionSource started { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public TaskCompletionSource release { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);

        public void Validate(string directory) => new Inno.Build.MacOSArm64.MacOSArm64SupportPackValidator().Validate(directory);

        public BuildTargetId id => new BuildTargetId("macos-arm64");
        public BuildTargetId target => id;

        public ManagedDeploymentId defaultManagedDeployment => ManagedDeploymentId.coreClr;

        public string runtimeIdentifier => "osx-arm64";

        public string displayName => "Blocking test target";

        public async ValueTask CompileAsync(
            GameBuildContentContext context,
            CancellationToken cancellationToken = default
        ) {
            ArgumentNullException.ThrowIfNull(context);
            started.SetResult();
            await release.Task.WaitAsync(cancellationToken);
        }

        public ValueTask<string> PackageAsync(
            GameBuildPackageContext context,
            CancellationToken cancellationToken = default
        ) {
            ArgumentNullException.ThrowIfNull(context);
            cancellationToken.ThrowIfCancellationRequested();
            throw new InvalidOperationException("A changed generation must fail before packaging.");
        }
    }
}
