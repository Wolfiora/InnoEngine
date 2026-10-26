using System;
using System.Runtime.InteropServices;
using Inno.Build.Toolchains;

namespace Inno.Build.MacOSArm64;

/// <summary>
/// Contributes native Apple Silicon execution capabilities.
/// </summary>
public sealed class MacOSArm64BuildHostProvider : IBuildHostProvider
{
    /// <inheritdoc />
    public BuildHostDescriptor? TryCapture() => !OperatingSystem.IsMacOS()
        || RuntimeInformation.ProcessArchitecture != Architecture.Arm64 ? null
        : new("MacOS", "arm64", new BuildHostServices(new PhysicalToolchainWorkspaceProvider(),
            inputPaths: ["platforms/MacOSArm64/build/Inno.Build.MacOSArm64/Host/MacOSArm64BuildHostProvider.cs"]));
}
