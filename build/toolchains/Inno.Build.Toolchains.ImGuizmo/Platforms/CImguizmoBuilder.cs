using System.Threading.Tasks;
using System.Threading;
using Inno.Build.Toolchains;

namespace Inno.Build.Toolchains.ImGuizmo.Platforms;

internal abstract class CImguizmoBuilder
{
    /// <summary>
    /// Gets the native platform identifier produced by this builder.
    /// </summary>
    public abstract string outputPlatform { get; }
    
    /// <summary>
    /// Determines whether the current host can execute this implementation.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> when the requested condition is satisfied; otherwise, <see langword="false"/>.
    /// </returns>
    public abstract bool IsSupported();
    
    /// <summary>
    /// Compiles the component sources using the selected checkout and configuration.
    /// </summary>
    /// <param name="cimguizmoDir">
    /// The cimguizmo dir text validated by the build operation.
    /// </param>
    /// <param name="cimguiDir">
    /// The cimgui dir text validated by the build operation.
    /// </param>
    /// <param name="cimguiLibraryFile">
    /// The exact published import or shared library selected by the parent operation.
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
    public abstract Task BuildAsync(
        string cimguizmoDir,
        string cimguiDir,
        string cimguiLibraryFile,
        NativeBuildContext context,
        CancellationToken cancellationToken
    );

    /// <summary>
    /// Retrieves the requested build type value from current authoritative state.
    /// </summary>
    /// <param name="config">
    /// The normalized native build configuration.
    /// </param>
    /// <returns>
    /// The validated text representation owned by the caller.
    /// </returns>
    protected static string GetBuildType(string config)
    {
        return config == ToolchainLayout.C_DEBUG_CONFIGURATION ? "Debug" : "Release";
    }
}
