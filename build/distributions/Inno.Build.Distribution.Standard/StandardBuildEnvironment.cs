using Inno.Build.Bindings;
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
        BuildTargetId toolsTarget
    ) {
        string system = OperatingSystem.IsWindows() ? "Windows"
            : OperatingSystem.IsMacOS() ? "MacOS"
            : OperatingSystem.IsLinux() ? "Linux"
            : throw new PlatformNotSupportedException("This system cannot execute the standard build distribution.");
        string architecture = RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X64 => "x64",
            Architecture.Arm64 => "arm64",
            Architecture.X86 => "x86",
            _ => throw new PlatformNotSupportedException("This processor has no standard build host integration.")
        };
        string host = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? "dotnet";
        host = ToolchainEnvironment.ResolveExecutable(host);
        return new BuildCompositionContext(host, applicationDirectory, new BuildHostDescriptor(system, architecture), toolsTarget, new NativeBindingGenerator(host));
    }
}
