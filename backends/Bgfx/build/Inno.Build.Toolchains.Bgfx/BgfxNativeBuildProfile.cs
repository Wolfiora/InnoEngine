using System;
using System.Collections.Generic;

namespace Inno.Build.Toolchains.Bgfx;

/// <summary>
/// Supplies platform-owned project generation and SDK invocation data to the shared BGFX recipe.
/// </summary>
public abstract class BgfxNativeBuildProfile
{
    /// <summary>
    /// Adds the integration's explicit generator executable and its configuration inputs before preparation.
    /// </summary>
    /// <param name="context">
    /// The requested target, checkout and already selected platform SDK.
    /// </param>
    /// <param name="includeTools">
    /// Whether this request builds offline tools, which do not compile a runtime binding bridge.
    /// </param>
    /// <returns>
    /// A frozen selection declaring the generator and all existing SDK commands.
    /// </returns>
    public abstract NativeToolchainSelection SelectTools(
        NativeBuildContext context,
        bool includeTools
    );

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
    /// Gets SDK options needed to compile the generated bridge with the producer's runtime linkage.
    /// These options apply only to the independently compiled bridge archive.
    /// </summary>
    public abstract IReadOnlyList<string> bindingCmakeArguments { get; }

    /// <summary>
    /// Formats bridge dependency and retention requirements for the platform's GENie generator.
    /// </summary>
    /// <param name="archivePath">
    /// The complete path of the independently compiled bridge archive.
    /// </param>
    /// <returns>
    /// The dependency reference and ordered linker arguments retaining every bridge translation unit.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The archive path is empty.
    /// </exception>
    public abstract (string reference, IReadOnlyList<string> arguments) CreateBindingLinkSettings(string archivePath);

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
