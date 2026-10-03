using System.Threading.Tasks;
using System.Threading;
using Inno.Build.Toolchains;

namespace Inno.Build.Toolchains.Bgfx.Platforms;

internal abstract class BgfxBuilder
{
    /// <summary>
    /// Gets the native platform identifier produced by this builder.
    /// </summary>
    public abstract string outputPlatform { get; }
    /// <summary>
    /// Gets the normalized build-tree fragment that owns artifacts for this host target.
    /// </summary>
    public abstract string artifactPathToken { get; }
    /// <summary>
    /// Gets the native make target used for debug output.
    /// </summary>
    protected abstract string debugMakeTarget { get; }
    /// <summary>
    /// Gets the native make target used for optimized output.
    /// </summary>
    protected abstract string releaseMakeTarget { get; }

    /// <summary>
    /// Determines whether the current host can execute this implementation.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> when the requested condition is satisfied; otherwise, <see langword="false"/>.
    /// </returns>
    public abstract bool IsSupported();

    /// <summary>
    /// Retrieves the requested make target value from current authoritative state.
    /// </summary>
    /// <param name="config">
    /// The normalized debug or release configuration.
    /// </param>
    /// <returns>
    /// The validated text representation owned by the caller.
    /// </returns>
    public string GetMakeTarget(string config)
    {
        return config == ToolchainLayout.C_DEBUG_CONFIGURATION
            ? debugMakeTarget
            : releaseMakeTarget;
    }

    /// <summary>
    /// Compiles the component sources using the selected checkout and configuration.
    /// </summary>
    /// <param name="bgfxDir">
    /// The bgfx dir text validated by the build operation.
    /// </param>
    /// <param name="context">
    /// The selected checkout and native configuration.
    /// </param>
    /// <param name="makeTargetOverride">
    /// The make target override text validated by the build operation.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels the native process tree.
    /// </param>
    /// <returns>
    /// Completion after native compilation succeeds; failures and cancellation propagate.
    /// </returns>
    public abstract Task BuildAsync(
        string bgfxDir,
        NativeBuildContext context,
        string? makeTargetOverride,
        CancellationToken cancellationToken
    );

    /// <summary>
    /// Builds the native offline tools required by the selected configuration.
    /// </summary>
    /// <param name="bgfxDir">
    /// The bgfx dir text validated by the build tools operation.
    /// </param>
    /// <param name="context">
    /// The selected checkout and native configuration.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels the native process tree.
    /// </param>
    /// <returns>
    /// Completion after native compilation succeeds; failures and cancellation propagate.
    /// </returns>
    public abstract Task BuildToolsAsync(
        string bgfxDir,
        NativeBuildContext context,
        CancellationToken cancellationToken
    );
}
