using System;
using System.Runtime.InteropServices;
using Inno.Build.Toolchains;

namespace Inno.Build.WindowsX64;

/// <summary>
/// Contributes Windows process and filesystem execution capabilities.
/// </summary>
public sealed class WindowsBuildHostProvider : IBuildHostProvider
{
    /// <inheritdoc />
    public BuildHostDescriptor? TryCapture() => !OperatingSystem.IsWindows() ? null
        : new("Windows", RuntimeInformation.ProcessArchitecture.ToString().ToLowerInvariant(),
            new BuildHostServices(new WindowsToolchainWorkspaceProvider(), StringComparer.OrdinalIgnoreCase,
                ["platforms/WindowsX64/build/Inno.Build.WindowsX64/Host/WindowsBuildHostProvider.cs",
                    "platforms/WindowsX64/build/Inno.Build.WindowsX64/Host/Workspace/WindowsToolchainWorkspaceProvider.cs",
                    "platforms/WindowsX64/build/Inno.Build.WindowsX64/Host/Workspace/WindowsToolchainDirectory.cs"], ".exe"));
}
