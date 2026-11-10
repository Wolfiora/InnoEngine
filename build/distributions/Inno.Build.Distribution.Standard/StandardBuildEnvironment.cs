using System;
using System.IO;
using System.Runtime.InteropServices;
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
    /// An explicit SDK executable, or null to use DOTNET_HOST_PATH, the loaded shared runtime's
    /// muxer, then normal executable discovery when this process has no shared runtime installation.
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
        BuildHostDescriptor buildHost = StandardBuildHosts.Capture();
        string host = ResolveManagedHost(dotnetHost);
        return new BuildCompositionContext(host, applicationDirectory, buildHost, toolsTarget);
    }

    /// <summary>
    /// Resolves the managed SDK executable without selecting a native tool target or preparing a product.
    /// </summary>
    /// <param name="dotnetHost">
    /// Explicit executable, or null to use DOTNET_HOST_PATH and then the currently loaded runtime's installation.
    /// Normal executable discovery is used only when there is no shared runtime installation.
    /// </param>
    /// <returns>
    /// The absolute existing managed SDK executable selected for this operation.
    /// </returns>
    /// <exception cref="FileNotFoundException">
    /// The selected SDK executable is unavailable.
    /// </exception>
    /// <exception cref="PlatformNotSupportedException">
    /// No standard execution host is registered for this system and processor.
    /// </exception>
    public static string ResolveManagedHost(string? dotnetHost = null)
    {
        string host = dotnetHost ?? Environment.GetEnvironmentVariable("DOTNET_HOST_PATH")
            ?? CurrentRuntimeHost("dotnet" + StandardBuildHosts.Capture().services.executableExtension);
        return ToolchainEnvironment.ResolveExecutable(host);
    }

    private static string CurrentRuntimeHost(string executable)
    {
        // An IDE can launch an apphost directly without setting DOTNET_HOST_PATH.
        // Resolve the muxer beside the runtime that actually loaded this process before consulting PATH.
        var runtime = new DirectoryInfo(RuntimeEnvironment.GetRuntimeDirectory());
        if (runtime.Parent?.Name == "Microsoft.NETCore.App" && runtime.Parent.Parent is { Name: "shared", Parent: { } installation })
        {
            string runtimeHost = Path.Combine(installation.FullName, executable);
            if (File.Exists(runtimeHost))
                return runtimeHost;
        }
        return executable;
    }
}
