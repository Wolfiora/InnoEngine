using System;
using Inno.Rendering;

namespace Inno.Player.Runtime;

/// <summary>
/// Parses the shared Player verification and window preferences independently of publication platform.
/// </summary>
public sealed class PlayerCommandLineOptions
{
    private PlayerCommandLineOptions() { }

    /// <summary>
    /// Gets an optional positive bounded run length.
    /// </summary>
    public int? smokeFrameLimit { get; private init; }

    /// <summary>
    /// Gets an explicit renderer preference, or null to use the selected backend's policy.
    /// </summary>
    public GraphicsApi? graphicsApi { get; private init; }

    /// <summary>
    /// Gets whether the primary window should initially be shown.
    /// </summary>
    public bool windowVisible { get; private init; }

    /// <summary>
    /// Parses the shared Player command line before creating any native resource.
    /// </summary>
    /// <param name="arguments">
    /// Pairs of supported option names and values.
    /// </param>
    /// <returns>
    /// A complete immutable command configuration.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// An option is missing, repeated or invalid.
    /// </exception>
    public static PlayerCommandLineOptions Parse(string[] arguments)
    {
        ArgumentNullException.ThrowIfNull(arguments);
        ParseValues(arguments, out int? smoke, out GraphicsApi? graphics, out bool visible);
        return new PlayerCommandLineOptions
        {
            smokeFrameLimit = smoke,
            graphicsApi = graphics,
            windowVisible = visible
        };
    }
    private static void ParseValues(
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
        "Usage: Player [--graphics-api <renderer-id>] [--window-visible <true|false>] [--smoke-frames <positive-count>].");
}
