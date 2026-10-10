using System;

namespace Inno.Build.Composition;

/// <summary>
/// Registers a platform contribution with the content implementation selected by a distribution.
/// </summary>
public sealed class GameBuildContribution
{
    /// <summary>
    /// Captures the complete contribution without instantiating authoring services or tools.
    /// </summary>
    /// <param name="platform">
    /// The platform-owned target, packaging, Support Pack and SDK contribution.
    /// </param>
    /// <param name="compilerFactory">
    /// The selected content implementation factory, invoked once per pipeline composition.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// A contribution or factory is null.
    /// </exception>
    public GameBuildContribution(
        BuildPlatformContribution platform,
        GameContentCompilerFactory compilerFactory
    ) {
        ArgumentNullException.ThrowIfNull(platform);
        ArgumentNullException.ThrowIfNull(compilerFactory);
        this.platform = platform;
        this.compilerFactory = compilerFactory;
    }

    /// <summary>
    /// Gets the platform-owned contribution, independent of backend selection.
    /// </summary>
    public BuildPlatformContribution platform { get; }

    /// <summary>
    /// Gets the distribution-selected factory borrowing each pipeline's authoring generation.
    /// </summary>
    public GameContentCompilerFactory compilerFactory { get; }
}
