using System;
using Inno.Adapter;
using Inno.Adapter.Default;
using Inno.Rendering;
using System.IO;
using Inno.Player.Runtime;
using Inno.Shell;

namespace Inno.Player;

internal static class Program
{
    private static int Main(string[] arguments)
    {
        try
        {
            ParseOptions(arguments, out int? smokeFrameLimit, out GraphicsApi? graphicsApi);
            var adapterCatalog = new DefaultAdapterCatalog();
            string resources = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "Resources", "Content"));
            return PlayerApplication.RunAsync(new PlayerLaunchOptions
            {
                adapters = adapterCatalog,
                contentDirectory = Directory.Exists(resources) ? resources : Path.Combine(AppContext.BaseDirectory, "Content"),
                persistentDataRoot = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                moduleActivator = new CollectiblePlayerModuleActivator(),
                frameDriver = new PollingShellFrameDriver(),
                consoleColors = true,
                graphicsApi = graphicsApi,
                smokeFrameLimit = smokeFrameLimit
            }).GetAwaiter().GetResult();
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
            return 1;
        }
    }

    private static void ParseOptions(
        string[] arguments,
        out int? smokeFrameLimit,
        out GraphicsApi? graphicsApi
    ) {
        smokeFrameLimit = null;
        graphicsApi = null;
        for (int index = 0; index < arguments.Length; index += 2)
        {
            if (index + 1 >= arguments.Length)
                throw Usage();
            if (string.Equals(arguments[index], "--smoke-frames", StringComparison.Ordinal))
            {
                if (smokeFrameLimit is not null
                    || !int.TryParse(arguments[index + 1], out int frameCount)
                    || frameCount <= 0)
                {
                    throw Usage();
                }
                smokeFrameLimit = frameCount;
                continue;
            }
            if (string.Equals(arguments[index], "--graphics-api", StringComparison.Ordinal))
            {
                if (graphicsApi is not null || !TryParseGraphicsApi(arguments[index + 1], out GraphicsApi api))
                    throw Usage();
                graphicsApi = api;
                continue;
            }
            throw Usage();
        }
    }

    private static bool TryParseGraphicsApi(
        string value,
        out GraphicsApi api
    ) {
        if (string.Equals(value, "noop", StringComparison.OrdinalIgnoreCase))
        {
            api = default;
            return false;
        }
        api = value.ToLowerInvariant() switch
        {
            "d3d11" => GraphicsApi.Direct3D11,
            "d3d12" => GraphicsApi.Direct3D12,
            "metal" => GraphicsApi.Metal,
            "vulkan" => GraphicsApi.Vulkan,
            "opengl" => GraphicsApi.OpenGL,
            _ => GraphicsApi.TryParse(value, out GraphicsApi parsed) ? parsed : default
        };
        return api.isValid && api != GraphicsApi.Noop;
    }

    private static ArgumentException Usage() => new("Usage: Inno.Player [--graphics-api <renderer-id>] [--smoke-frames <positive-count>].");
}
