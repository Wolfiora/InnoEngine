using System;
using Inno.Extensibility.Types;

namespace Inno.Rendering.Assets.Authoring;

/// <summary>
/// Selects one renderer profile and compilation policy.
/// </summary>
[StableTypeID("29e9c0f0-cb34-5b98-a6d6-356f744f3049")]
public sealed class ShaderCompileTarget
{
    /// <summary>
    /// Creates a shader compilation target.
    /// </summary>
    /// <param name="profileKey">
    /// Backend compiler-owned stable profile key.
    /// </param>
    /// <param name="capabilities">
    /// Target renderer capabilities.
    /// </param>
    /// <param name="optimize">
    /// Whether release optimization is enabled.
    /// </param>
    /// <param name="debugInformation">
    /// Whether shader debug information is emitted.
    /// </param>
    public ShaderCompileTarget(
        string profileKey,
        GraphicsCapabilities capabilities,
        bool optimize = true,
        bool debugInformation = false
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(profileKey);
        ArgumentNullException.ThrowIfNull(capabilities);
        this.profileKey = profileKey;
        this.capabilities = capabilities;
        this.optimize = optimize;
        this.debugInformation = debugInformation;
    }

    /// <summary>
    /// Gets the backend compiler-owned stable profile key.
    /// </summary>
    public string profileKey { get; }

    /// <summary>
    /// Gets target renderer capabilities.
    /// </summary>
    public GraphicsCapabilities capabilities { get; }

    /// <summary>
    /// Gets whether release optimization is enabled.
    /// </summary>
    public bool optimize { get; }

    /// <summary>
    /// Gets whether shader debug information is emitted.
    /// </summary>
    public bool debugInformation { get; }

    /// <summary>
    /// Gets a stable target cache-key fragment.
    /// </summary>
    public string key => $"{profileKey}:opt={optimize}:debug={debugInformation}";
}
