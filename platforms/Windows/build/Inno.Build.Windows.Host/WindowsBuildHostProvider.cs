using System;
using System.Runtime.InteropServices;
using Inno.Build.Toolchains;

namespace Inno.Build.Windows.Host;

/// <summary>
/// Contributes Windows process and filesystem execution capabilities.
/// </summary>
public sealed class WindowsBuildHostProvider : IBuildHostProvider
{
    /// <inheritdoc />
    public BuildHostDescriptor? TryCapture() => !OperatingSystem.IsWindows() ? null
        : new("Windows", RuntimeInformation.ProcessArchitecture.ToString().ToLowerInvariant(),
            new BuildHostServices(new WindowsToolchainWorkspaceProvider(), StringComparer.OrdinalIgnoreCase,
                ["platforms/Windows/build/Inno.Build.Windows.Host/WindowsBuildHostProvider.cs",
                    "platforms/Windows/build/Inno.Build.Windows.Host/Workspace/WindowsToolchainWorkspaceProvider.cs",
                    "platforms/Windows/build/Inno.Build.Windows.Host/Workspace/WindowsToolchainDirectory.cs"], ".exe"));
}
