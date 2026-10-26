using System.Threading.Tasks;

namespace Inno.Player.BrowserWasm32;

internal static class Program
{
    private static Task<int> Main(string[] arguments) => BrowserWasm32PlayerComposition.RunAsync(arguments);
}
