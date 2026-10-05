using System.Threading.Tasks;

namespace Inno.Player.Browser;

internal static class Program
{
    private static Task<int> Main() => BrowserPlayerComposition.RunAsync();
}
