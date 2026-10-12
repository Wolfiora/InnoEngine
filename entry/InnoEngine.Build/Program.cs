using System.Threading.Tasks;
using Inno.Entry.Bootstrap;

namespace InnoEngine.Build;

internal static class Program
{
    private static Task<int> Main(string[] arguments) => DevelopmentEntry.RunBuildAsync(arguments);
}
