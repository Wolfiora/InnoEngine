using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;

namespace Inno.Player.Browser;

internal static partial class BrowserBridge
{
    [JSImport("host.baseUrl", "browser-player.js")]
    internal static partial string GetBaseUrl();

    [JSImport("host.nextFrame", "browser-player.js")]
    internal static partial Task WaitForFrame();

    [JSImport("host.cancelFrame", "browser-player.js")]
    internal static partial void CancelFrame();

    [JSImport("host.status", "browser-player.js")]
    internal static partial void SetStatus(string message);

    [JSImport("host.error", "browser-player.js")]
    internal static partial void SetError(string message);
}
