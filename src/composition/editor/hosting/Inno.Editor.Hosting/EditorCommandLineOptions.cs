using System;
using System.Diagnostics.CodeAnalysis;
using Inno.Rendering;

namespace Inno.Editor.Hosting;

/// <summary>
/// Parses the shared Editor project and verification workflow before platform composition starts.
/// </summary>
public sealed class EditorCommandLineOptions
{
    private EditorCommandLineOptions(string projectDirectory) => this.projectDirectory = projectDirectory;

    /// <summary>
    /// Gets the caller-selected authoring project.
    /// </summary>
    public string projectDirectory { get; }

    /// <summary>
    /// Gets the optional bounded verification frame count.
    /// </summary>
    public int? smokeFrameLimit { get; private init; }

    /// <summary>
    /// Gets an explicit renderer preference, or null for backend policy.
    /// </summary>
    public GraphicsApi? graphicsApi { get; private init; }

    /// <summary>
    /// Parses the common Editor command line without selecting a platform.
    /// </summary>
    /// <param name="arguments">
    /// The project path followed by optional name/value pairs.
    /// </param>
    /// <returns>
    /// The immutable project startup configuration.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The project or an option is missing, repeated or invalid.
    /// </exception>
    public static EditorCommandLineOptions Parse(string[] arguments)
    {
        if (!TryGetRunOptions(arguments, out string? project, out int? smoke, out GraphicsApi? graphics))
            throw new ArgumentException("Usage: Editor <project-directory> [--graphics-api <renderer-id>] [--smoke-frames <positive-count>].", nameof(arguments));
        return new EditorCommandLineOptions(project) { smokeFrameLimit = smoke, graphicsApi = graphics };
    }
    private static bool TryGetRunOptions(
        string[] args,
        [NotNullWhen(true)] out string? projectDirectory,
        out int? smokeFrameLimit,
        out GraphicsApi? graphicsApi
    ) {
        ArgumentNullException.ThrowIfNull(args);
        smokeFrameLimit = null;
        graphicsApi = null;
        projectDirectory = args.Length > 0 ? args[0] : null;
        if (projectDirectory is null)
            return false;
        for (int index = 1; index < args.Length; index += 2)
        {
            if (index + 1 >= args.Length)
                return false;
            if (string.Equals(args[index], "--smoke-frames", StringComparison.Ordinal))
            {
                if (smokeFrameLimit is not null
                    || !int.TryParse(args[index + 1], out int parsedFrameLimit)
                    || parsedFrameLimit <= 0)
                {
                    return false;
                }
                smokeFrameLimit = parsedFrameLimit;
                continue;
            }
            if (string.Equals(args[index], "--graphics-api", StringComparison.Ordinal))
            {
                if (graphicsApi is not null || !TryParseGraphicsApi(args[index + 1], out GraphicsApi parsedApi))
                    return false;
                graphicsApi = parsedApi;
                continue;
            }
            return false;
        }
        return true;
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
}
