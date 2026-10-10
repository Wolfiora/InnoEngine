using Inno.Adapter.Rendering;
using Inno.Adapter.Rendering.Bgfx;
using Inno.Integration.MacOS.Bgfx.Runtime;
using Inno.Adapter.Platform;
using Inno.Adapter.Platform.Sdl3;
using Inno.Integration.MacOS.Sdl3;
using System;
using Inno.Adapter;
using Inno.Adapter.Default;
using Inno.Rendering;
using System.IO;
using Inno.Player.Runtime;
using Inno.Shell;
using Inno.Adapter.Storage;
using Inno.Adapter.Storage.FileSystem;
using Inno.Core.Logging;
using Inno.Core.Execution;
using Inno.Runtime;
using Inno.Storage;
using Inno.Platform.MacOS;

namespace Inno.Player.MacOS;

internal static class MacOSPlayerComposition
{
    internal static int Run(string[] arguments)
    {
        try
        {
            PlayerCommandLineOptions command = PlayerCommandLineOptions.Parse(arguments);
            string dataRoot = MacOSApplicationLocations.userDataRoot;
            var storage = new StorageBackendCatalog([new FileSystemStorageBackendProvider(dataRoot)]);
            var adapterCatalog = new DefaultAdapterCatalog(new DefaultAdapterCatalogOptions { platform = new PlatformBackendCatalog([new Sdl3PlatformBackendProvider(new MacOSSdl3HostIntegration())]), storage = storage, rendering = new RenderingBackendCatalog([new BgfxRenderingBackendProvider(new MacOSBgfxSurfaceIntegration())]) });
            return OwnerThreadExecution.Run(() => PlayerApplication.RunAsync(new PlayerLaunchOptions
            {
                modules = Generated.PlayerMetadataComposition.CreateModules(),
                types = Generated.PlayerMetadataComposition.CreateTypes(),
                serializationMetadata = Generated.PlayerMetadataComposition.CreateSerialization(),
                adapters = adapterCatalog,
                adapterSelection = StandardAdapterSelection.Create(StorageBackendId.fileSystem),
                contentSource = new MacOSPlayerContentSource(AppContext.BaseDirectory, dataRoot),
                createStorage = manifest => new FileSystemApplicationStorage(
                    Path.Combine(ResolveDataRoot(dataRoot, manifest), "Storage")),
                createLogSink = (
                    manifest,
                    sessionId
                ) => new FileLogSink(
                    Path.Combine(ResolveDataRoot(dataRoot, manifest), "Logs", sessionId.ToString())),
                moduleActivator = Generated.PlayerMetadataComposition.CreateActivator(),
                frameDriver = new PollingShellFrameDriver(),
                consoleColors = true,
                graphicsApi = command.graphicsApi,
                windowVisible = command.windowVisible,
                smokeFrameLimit = command.smokeFrameLimit
            }));
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
            return 1;
        }
    }

    private static string ResolveDataRoot(
        string root,
        GameRuntimeManifest manifest
    ) => Path.Combine(root, manifest.persistentDataPath.Length == 0 ? manifest.applicationId : manifest.persistentDataPath);

}
