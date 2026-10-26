using System;
using System.IO;
using System.Threading;
using Inno.Adapter.Authoring.Default;
using Inno.Adapter.Default;
using Inno.Adapter.Modules.DotNet;
using Inno.Adapter.Platform;
using Inno.Adapter.Platform.Sdl3;
using Inno.Adapter.Presentation;
using Inno.Adapter.Presentation.ImGui;
using Inno.Adapter.Rendering;
using Inno.Adapter.Rendering.Bgfx;
using Inno.Adapter.Serialization.DotNet;
using Inno.Adapter.Storage;
using Inno.Adapter.Storage.FileSystem;
using Inno.Build;
using Inno.Build.Distribution.Standard;
using Inno.Build.Toolchains.Bgfx.Tools;
using Inno.Core.Execution;
using Inno.Core.Input;
using Inno.Core.Logging;
using Inno.Editor.Core;
using Inno.Editor.Hosting;
using Inno.Extensibility.Modules;
using Inno.Integration.MacOSArm64.Bgfx;
using Inno.Integration.MacOSArm64.Bgfx.Runtime;
using Inno.Integration.MacOSArm64.Sdl3;
using Inno.Platform;
using Inno.Platform.MacOSArm64;
using Inno.Runtime;
using Inno.Scripting.Compiler;
using Inno.Shell;

namespace Inno.Editor.MacOSArm64;

/// <summary>
/// Composes the MacOS Editor from explicit platform integrations and shared product hosting.
/// </summary>
public static class MacOSArm64EditorComposition
{
    /// <summary>
    /// Runs the platform Editor on the caller's main thread; no directory opens the native folder dialog.
    /// </summary>
    /// <param name="arguments">
    /// An optional project directory and the shared Editor command-line options.
    /// </param>
    /// <returns>
    /// Zero on normal exit or dialog cancellation, two on cancellation or invalid arguments, or one on startup failure.
    /// </returns>
    /// <param name="cancellationToken">
    /// Cancels preparation or requests orderly shutdown of the shared Editor host.
    /// An open native folder dialog retains its callback owner until the user completes or closes it.
    /// </param>
    public static int Run(
        string[] arguments,
        CancellationToken cancellationToken
    ) {
        try
        {
            ArgumentNullException.ThrowIfNull(arguments);
            cancellationToken.ThrowIfCancellationRequested();
            if (arguments.Length == 0)
            {
                using var picker = new Sdl3PlatformApplication(new MacOSArm64Sdl3HostIntegration());
                string? selected = Sdl3FolderDialog.SelectFolder(picker);
                if (selected is null)
                    return 0;
                arguments = [selected];
            }
            EditorCommandLineOptions command = EditorCommandLineOptions.Parse(arguments);
            _ = MacOSArm64ApplicationLocations.userDataRoot;
            string project = Path.GetFullPath(command.projectDirectory);
            string persistentRoot = Path.Combine(project, "Library", "PersistentData");
            var adapters = new DefaultAuthoringAdapterCatalog(new DefaultAdapterCatalogOptions
            {
                platform = new PlatformBackendCatalog([new Sdl3PlatformBackendProvider(new MacOSArm64Sdl3HostIntegration())]),
                rendering = new RenderingBackendCatalog([new BgfxRenderingBackendProvider(new MacOSArm64BgfxSurfaceIntegration())]),
                storage = new StorageBackendCatalog([new FileSystemStorageBackendProvider(persistentRoot)])
            }, [new BgfxAuthoringProvider(MacOSArm64BgfxIntegration.shaderProfile)],
                [new ImGuiPresentationProvider(new ImGuiInteractionOptions(commandKeyBehavior: true, horizontalWheelDirection: -1))]);
            var buildContext = StandardBuildEnvironment.Capture(Path.GetDirectoryName(typeof(MacOSArm64EditorComposition).Assembly.Location)!, Inno.Build.MacOSArm64.MacOSArm64BuildModule.target.id);
            var launch = new EditorLaunchOptions
            {
                defaultBuildTarget = Inno.Build.MacOSArm64.MacOSArm64BuildModule.target.id,
                projectDirectory = project,
                keyboard = new EditorKeyboardPolicy(KeyModifier.Super, "Cmd"),
                adapterCatalog = adapters,
                shell = new ShellOptions
                {
                    adapters = StandardAdapterSelection.Create(StorageBackendId.fileSystem),
                    window = new PlatformWindowOptions
                    {
                        title = "Inno Editor",
                        width = 1600,
                        height = 900,
                        visible = command.windowVisible,
                        resizable = true,
                        highPixelDensity = true
                    },
                    preferredGraphicsApi = command.graphicsApi,
                    sRgbBackbuffer = true
                },
                presentation = PresentationBackendId.imGui,
                frameDriver = new PollingShellFrameDriver(),
                distribution = StandardBuildDistribution.Create(buildContext).build,
                buildContext = buildContext,
                supportPackRoot = Path.Combine(Path.GetDirectoryName(typeof(MacOSArm64EditorComposition).Assembly.Location)!, "SupportPacks"),
                createEngineHost = CreateEngineHost,
                createScriptModuleSource = deployment => CreateScriptModuleSource(project, deployment),
                createSessionLogSink = (
                    kind,
                    session
                ) => new FileLogSink(Path.Combine(persistentRoot,
                    kind == RuntimeSessionKind.Play ? "inno.editor.play" : "inno.editor", "Logs", session.ToString())),
                createHostLogSink = () => new ConsoleLogSink(useColors: true),
                smokeFrameLimit = command.smokeFrameLimit
            };
            return OwnerThreadExecution.Run(() => EditorApplication.RunAsync(launch, cancellationToken));
        }
        catch (OperationCanceledException)
        {
            return 2;
        }
        catch (ArgumentException exception)
        {
            Console.Error.WriteLine(exception.Message);
            return 2;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine($"[{DateTime.Now:O}] Unhandled exception:{Environment.NewLine}{exception}");
            return 1;
        }
    }

    private static EngineHost CreateEngineHost()
        => new EngineHostBuilder().UseMetadataSources(
            new DotNetAssemblyCatalogSource(typeof(MacOSArm64EditorComposition).Assembly),
            new ReflectionTypeCatalogSource(),
            new ReflectionSerializationMetadataSource()).Build();

    private static IModuleSource CreateScriptModuleSource(
        string project,
        ScriptModuleDeployment deployment
    ) => new DotNetModuleSource
    {
        artifactRootDirectory = Path.Combine(project, "Library", "Assemblies"),
        moduleName = deployment.moduleName,
        mainAssemblyPath = deployment.mainAssemblyPath,
        domain = deployment.domain,
        scope = deployment.scope,
        preloadAssemblyPaths = deployment.preloadAssemblyPaths,
        upstreamModuleNames = deployment.upstreamModuleNames,
        assemblyScopes = deployment.assemblyScopes,
        collectible = true
    };
}
