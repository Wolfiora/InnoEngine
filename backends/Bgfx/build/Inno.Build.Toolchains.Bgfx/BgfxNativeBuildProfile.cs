using System.Collections.Generic;

namespace Inno.Build.Toolchains.Bgfx;

/// <summary>
/// Supplies platform-owned project generation and SDK invocation data to the shared BGFX recipe.
/// </summary>
public abstract class BgfxNativeBuildProfile
{
    /// <summary>
    /// Gets the exact native target accepted by this configuration.
    /// </summary>
    public abstract string targetId { get; }

    /// <summary>
    /// Gets ordered vendor project-generation arguments, excluding shared component options.
    /// </summary>
    public abstract IReadOnlyList<string> generatorArguments { get; }

    /// <summary>
    /// Gets the vendor output directory token used to select this target's artifacts.
    /// </summary>
    public abstract string artifactPathToken { get; }

    /// <summary>
    /// Freezes the selected SDK invocation without executing tools or creating staging.
    /// </summary>
    /// <param name="context">
    /// The frozen target tools and configuration used by this operation.
    /// </param>
    /// <param name="includeTools">
    /// Whether the component must prepare offline compiler executables.
    /// </param>
    /// <returns>
    /// Immutable arguments executed from the owned BGFX source snapshot.
    /// </returns>
    public abstract BgfxBuildInvocation CreateBuildInvocation(
        NativeBuildContext context,
        bool includeTools
    );
}
