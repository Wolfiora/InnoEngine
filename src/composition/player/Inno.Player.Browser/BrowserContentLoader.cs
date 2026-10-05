using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Inno.Player.Browser;

internal static class BrowserContentLoader
{
    internal static async Task DownloadAsync()
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

}
