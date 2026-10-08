using Inno.Adapter.Presentation.ImGui;
using Inno.Integration.MacOS.Bgfx;
using Inno.Adapter.Platform;
using Inno.Adapter.Platform.Sdl3;
using Inno.Integration.MacOS.Sdl3;
using Inno.Build.Toolchains.Bgfx.Tools;
using System;
using System.IO;
using Inno.Adapter.Authoring.Default;
using Inno.Adapter.Default;
using Inno.Adapter.Modules.DotNet;
using Inno.Adapter.Presentation;
using Inno.Adapter.Serialization.DotNet;
using Inno.Adapter.Storage;
using Inno.Adapter.Storage.FileSystem;
using Inno.Build;
using Inno.Build.Distribution.Standard;
using Inno.Core.Execution;
using Inno.Core.Logging;
using Inno.Editor.Hosting;
using Inno.Editor.Core;
using Inno.Core.Input;
using Inno.Extensibility.Modules;
using Inno.Platform;
using Inno.Platform.MacOS;
using Inno.Runtime;
using Inno.Scripting.Compiler;
using Inno.Shell;

namespace Inno.Editor.MacOS;

internal static class MacOSEditorComposition
{
    internal static int Run(string[] arguments)
    {
        try
        {
            EditorCommandLineOptions command = EditorCommandLineOptions.Parse(arguments);
            _ = MacOSApplicationLocations.userDataRoot;
            string project = Path.GetFullPath(command.projectDirectory);
            string persistentRoot = Path.Combine(project, "Library", "PersistentData");
            var adapters = new DefaultAuthoringAdapterCatalog(new DefaultAdapterCatalogOptions
            {
                platform = new PlatformBackendCatalog([new Sdl3PlatformBackendProvider(new MacOSSdl3HostIntegration())]),
                storage = new StorageBackendCatalog([new FileSystemStorageBackendProvider(persistentRoot)])
            }, [new BgfxAuthoringProvider(MacOSBgfxIntegration.shaderProfile)],
                [new ImGuiPresentationProvider(new ImGuiInteractionOptions(commandKeyBehavior: true, horizontalWheelDirection: -1))]);
            var buildContext = StandardBuildEnvironment.Capture(AppContext.BaseDirectory, BuildTargetId.macOSArm64);
            var launch = new EditorLaunchOptions
            {
                defaultBuildTarget = Inno.Build.BuildTargetId.macOSArm64,
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
                supportPackRoot = Path.Combine(AppContext.BaseDirectory, "SupportPacks"),
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
            return OwnerThreadExecution.Run(() => EditorApplication.RunAsync(launch));
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
            new DotNetAssemblyCatalogSource(typeof(MacOSEditorComposition).Assembly),
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
