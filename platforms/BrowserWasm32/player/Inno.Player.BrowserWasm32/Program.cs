using System.Threading.Tasks;

namespace Inno.Player.BrowserWasm32;

internal static class Program
{
    private static Task<int> Main() => BrowserWasm32PlayerComposition.RunAsync();
}
