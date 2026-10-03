using System;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices.JavaScript;
using System.Threading;
using System.Threading.Tasks;
using Inno.Adapter;
using Inno.Adapter.Default;
using Inno.Adapter.Storage.Browser;
using Inno.Core.Logging;
using Inno.Player.Runtime;
using Inno.Runtime;
using Inno.Shell;

namespace Inno.Player.Browser;

internal static partial class Program
{
    private static async Task<int> Main()
    {
        try
        {
            BrowserBridge.SetStatus("Loading game content…");
            await DownloadContentAsync();

            BrowserBridge.SetStatus("Starting game…");
            BrowserBridge.SetStatus(string.Empty);
            return await PlayerApplication.RunAsync(new PlayerLaunchOptions
            {
                adapters = new DefaultAdapterCatalog(storageFactory: new BrowserStorageBackendFactory()),
                contentDirectory = "/Content",
                persistentDataRoot = "/persistent",
                moduleActivator = new LinkedPlayerModuleActivator(),
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

    private static async Task DownloadContentAsync()
    {
        using var client = new HttpClient { BaseAddress = new Uri(BrowserBridge.GetBaseUrl()) };
        string packName = (await client.GetStringAsync("Content/content-pack.txt")).Trim();
        if (!packName.StartsWith("content-", StringComparison.Ordinal)
            || !packName.EndsWith(".pack", StringComparison.Ordinal)
            || !string.Equals(packName, Path.GetFileName(packName), StringComparison.Ordinal))
        {
            throw new InvalidDataException("The browser content index names an invalid pack file.");
        }

        Directory.CreateDirectory("/Content");
        await DownloadFileAsync(client, "runtime.manifest");
        await DownloadFileAsync(client, "catalog.inno");
        await DownloadFileAsync(client, packName);
    }

    private static async Task DownloadFileAsync(
        HttpClient client,
        string fileName
    ) {
        byte[] bytes = await client.GetByteArrayAsync("Content/" + fileName);
        await File.WriteAllBytesAsync(Path.Combine("/Content", fileName), bytes);
    }

    private static ValueTask NextFrameAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return new ValueTask(BrowserBridge.WaitForFrame().WaitAsync(cancellationToken));
    }
}

internal static partial class BrowserBridge
{
    [JSImport("host.baseUrl", "browser-player.js")]
    internal static partial string GetBaseUrl();

    [JSImport("host.nextFrame", "browser-player.js")]
    internal static partial Task WaitForFrame();

    [JSImport("host.status", "browser-player.js")]
    internal static partial void SetStatus(string message);

    [JSImport("host.error", "browser-player.js")]
    internal static partial void SetError(string message);
}
