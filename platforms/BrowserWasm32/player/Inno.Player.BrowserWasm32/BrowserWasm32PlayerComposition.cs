using Inno.Adapter.Rendering;
using Inno.Adapter.Rendering.Bgfx;
using Inno.Integration.BrowserWasm32.Bgfx.Runtime;
using Inno.Adapter.Platform;
using Inno.Adapter.Platform.Sdl3;
using Inno.Integration.BrowserWasm32.Sdl3;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using Inno.Adapter;
using Inno.Adapter.Default;
using Inno.Adapter.Storage;
using Inno.Adapter.Storage.BrowserWasm32;
using Inno.Core.Logging;
using Inno.Player.Runtime;
using Inno.Runtime;
using Inno.Shell;
using Inno.Storage;

namespace Inno.Player.BrowserWasm32;

internal static class BrowserWasm32PlayerComposition
{
    internal static async Task<int> RunAsync(string[] arguments)
    {
        try
        {
            PlayerCommandLineOptions command = PlayerCommandLineOptions.Parse(arguments);
            BrowserWasm32Bridge.SetStatus("Loading game content…");
            using var client = new HttpClient { BaseAddress = new Uri(BrowserWasm32Bridge.GetBaseUrl()) };
            var storage = new StorageBackendCatalog([new BrowserWasm32StorageBackendProvider()]);

            BrowserWasm32Bridge.SetStatus("Starting game…");
            BrowserWasm32Bridge.SetStatus(string.Empty);
            return await PlayerApplication.RunAsync(new PlayerLaunchOptions
            {
                modules = Generated.PlayerMetadataComposition.CreateModules(),
                types = Generated.PlayerMetadataComposition.CreateTypes(),
                serializationMetadata = Generated.PlayerMetadataComposition.CreateSerialization(),
                adapters = new DefaultAdapterCatalog(new DefaultAdapterCatalogOptions
                {
                    platform = new PlatformBackendCatalog([
                        new Sdl3PlatformBackendProvider(new BrowserWasm32Sdl3HostIntegration())
                    ]),
                    storage = storage,
                    rendering = new RenderingBackendCatalog([
                        new BgfxRenderingBackendProvider(new BrowserWasm32BgfxSurfaceIntegration())
                    ])
                }),
                adapterSelection = StandardAdapterSelection.Create(StorageBackendId.browser),
                contentSource = new HttpPlayerContentSource(client),
                createStorage = manifest => storage.CreateStorage(StorageBackendId.browser,
                    new StorageScope(manifest.applicationId)),
                moduleActivator = Generated.PlayerMetadataComposition.CreateActivator(),
                frameDriver = new ScheduledShellFrameDriver(NextFrameAsync),
                graphicsApi = command.graphicsApi,
                windowVisible = command.windowVisible,
                smokeFrameLimit = command.smokeFrameLimit,
                jobExecutionMode = RuntimeJobExecutionMode.SingleThread,
                renderOnCallingThread = true,
                logDeliveryMode = LogDeliveryMode.Inline
            });
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
            BrowserWasm32Bridge.SetError(exception.ToString());
            return 1;
        }
    }

    private static async ValueTask NextFrameAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Task opportunity = BrowserWasm32Bridge.WaitForFrame();
        using CancellationTokenRegistration cancellation = cancellationToken.Register(BrowserWasm32Bridge.CancelFrame);
        await opportunity.WaitAsync(cancellationToken);
    }
}
