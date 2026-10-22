using System;
using System.IO;
using Inno.Build.Composition;
using Inno.Build.Toolchains;

namespace Inno.Build.Distribution.Standard;

/// <summary>
/// Captures the actual machine running distribution tools without choosing an output target.
/// </summary>
public static class StandardBuildEnvironment
{
    /// <summary>
    /// Resolves the selected managed host and captures execution capabilities at the composition boundary.
    /// </summary>
    /// <param name="applicationDirectory">
    /// The product or tool's installed location used for source provisioning.
    /// </param>
    /// <param name="toolsTarget">
    /// The native target explicitly selected for executable build tools, independently of exported games.
    /// </param>
    /// <param name="dotnetHost">
    /// An explicit managed SDK executable, or null to use the current tool process's selected host.
    /// </param>
    /// <returns>
    /// A frozen execution context; publication targets must be selected independently.
    /// </returns>
    /// <exception cref="PlatformNotSupportedException">
    /// The distribution has no build execution integration for this system or processor.
    /// </exception>
    /// <exception cref="FileNotFoundException">
    /// The selected managed SDK host cannot be located.
    /// </exception>
    public static BuildCompositionContext Capture(
        string applicationDirectory,
        BuildTargetId toolsTarget,
        string? dotnetHost = null
    ) {
        string host = dotnetHost ?? Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? "dotnet";
        host = ToolchainEnvironment.ResolveExecutable(host);
        return new BuildCompositionContext(host, applicationDirectory, StandardBuildHosts.Capture(), toolsTarget);
    }
}
