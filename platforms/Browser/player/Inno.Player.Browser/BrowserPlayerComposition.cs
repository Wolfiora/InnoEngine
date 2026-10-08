using Inno.Adapter.Platform;
using Inno.Adapter.Platform.Sdl3;
using Inno.Integration.Browser.Sdl3;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using Inno.Adapter;
using Inno.Adapter.Default;
using Inno.Adapter.Storage;
using Inno.Adapter.Storage.Browser;
using Inno.Core.Logging;
using Inno.Player.Runtime;
using Inno.Runtime;
using Inno.Shell;
using Inno.Storage;

namespace Inno.Player.Browser;

internal static class BrowserPlayerComposition
{
    internal static async Task<int> RunAsync()
    {
        try
        {
            BrowserBridge.SetStatus("Loading game content…");
            using var client = new HttpClient { BaseAddress = new Uri(BrowserBridge.GetBaseUrl()) };
            var storage = new StorageBackendCatalog([new BrowserStorageBackendProvider()]);

            BrowserBridge.SetStatus("Starting game…");
            BrowserBridge.SetStatus(string.Empty);
            return await PlayerApplication.RunAsync(new PlayerLaunchOptions
            {
                modules = Generated.PlayerMetadataComposition.CreateModules(),
                types = Generated.PlayerMetadataComposition.CreateTypes(),
                serializationMetadata = Generated.PlayerMetadataComposition.CreateSerialization(),
                adapters = new DefaultAdapterCatalog(new DefaultAdapterCatalogOptions { platform = new PlatformBackendCatalog([new Sdl3PlatformBackendProvider(new BrowserSdl3HostIntegration())]), storage = storage }),
                adapterSelection = StandardAdapterSelection.Create(StorageBackendId.browser),
                contentSource = new HttpPlayerContentSource(client),
                createStorage = manifest => storage.CreateStorage(StorageBackendId.browser,
                    new StorageScope(manifest.applicationId)),
                moduleActivator = Generated.PlayerMetadataComposition.CreateActivator(),
                frameDriver = new ScheduledShellFrameDriver(NextFrameAsync),
                jobExecutionMode = RuntimeJobExecutionMode.SingleThread,
                renderOnCallingThread = true,
                logDeliveryMode = LogDeliveryMode.Inline
            });
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
            BrowserBridge.SetError(exception.ToString());
            return 1;
        }
    }

    private static async ValueTask NextFrameAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Task opportunity = BrowserBridge.WaitForFrame();
        using CancellationTokenRegistration cancellation = cancellationToken.Register(BrowserBridge.CancelFrame);
        await opportunity.WaitAsync(cancellationToken);
    }
}
