using System;
using Inno.Adapter;
using Inno.Adapter.Default;
using Inno.Rendering;
using System.IO;
using Inno.Player.Runtime;
using Inno.Shell;

namespace Inno.Player;

internal static class DesktopPlayerComposition
{
    internal static int Run(string[] arguments)
    {
        try
        {
            ParseOptions(arguments, out int? smokeFrameLimit, out GraphicsApi? graphicsApi, out bool windowVisible);
            var adapterCatalog = new DefaultAdapterCatalog();
            string resources = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "Resources", "Content"));
            return PlayerApplication.RunAsync(new PlayerLaunchOptions
            {
                modules = Generated.PlayerMetadataComposition.CreateModules(),
                types = Generated.PlayerMetadataComposition.CreateTypes(),
                serializationMetadata = Generated.PlayerMetadataComposition.CreateSerialization(),
                adapters = adapterCatalog,
                contentDirectory = Directory.Exists(resources) ? resources : Path.Combine(AppContext.BaseDirectory, "Content"),
                persistentDataRoot = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                moduleActivator = Generated.PlayerMetadataComposition.CreateActivator(),
                frameDriver = new PollingShellFrameDriver(),
                consoleColors = true,
                graphicsApi = graphicsApi,
                windowVisible = windowVisible,
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
        out GraphicsApi? graphicsApi,
        out bool windowVisible
    ) {
        smokeFrameLimit = null;
        graphicsApi = null;
        windowVisible = true;
        bool visibilitySpecified = false;
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
            if (string.Equals(arguments[index], "--window-visible", StringComparison.Ordinal))
            {
                if (visibilitySpecified || !bool.TryParse(arguments[index + 1], out windowVisible))
                    throw Usage();
                visibilitySpecified = true;
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

    private static ArgumentException Usage() => new(
        "Usage: Inno.Player [--graphics-api <renderer-id>] [--window-visible <true|false>] [--smoke-frames <positive-count>].");
}
