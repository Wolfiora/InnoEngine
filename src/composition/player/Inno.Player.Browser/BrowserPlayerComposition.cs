using System;
using System.Threading;
using System.Threading.Tasks;
using Inno.Adapter;
using Inno.Adapter.Default;
using Inno.Adapter.Storage;
using Inno.Adapter.Storage.Browser;
using Inno.Core.Logging;
using Inno.Player.Runtime;
using Inno.Runtime;
using Inno.Shell;

namespace Inno.Player.Browser;

internal static class BrowserPlayerComposition
{
    internal static async Task<int> RunAsync()
    {
        try
        {
            BrowserBridge.SetStatus("Loading game content…");
            await BrowserContentLoader.DownloadAsync();

            BrowserBridge.SetStatus("Starting game…");
            BrowserBridge.SetStatus(string.Empty);
            return await PlayerApplication.RunAsync(new PlayerLaunchOptions
            {
                modules = Generated.PlayerMetadataComposition.CreateModules(),
                types = Generated.PlayerMetadataComposition.CreateTypes(),
                serializationMetadata = Generated.PlayerMetadataComposition.CreateSerialization(),
                adapters = new DefaultAdapterCatalog(storageProviders: [new BrowserStorageBackendProvider()]),
                adapterSelection = new AdapterSelection { storage = StorageBackendId.browser },
                contentDirectory = "/Content",
                persistentDataRoot = "/persistent",
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
