using System;
using Inno.Build.MacOSArm64;
using Inno.Build.Toolchains;
using Inno.Build.Windows.Host;

namespace Inno.Build.Distribution.Standard;

/// <summary>
/// Registers supported build hosts without duplicating their execution facts.
/// </summary>
public static class StandardBuildHosts
{
    /// <summary>
    /// Captures the current execution machine from registered platform providers.
    /// </summary>
    /// <returns>
    /// The selected immutable host and execution capabilities.
    /// </returns>
    /// <exception cref="PlatformNotSupportedException">
    /// No registered provider supports the current machine.
    /// </exception>
    public static BuildHostDescriptor Capture()
    {
        IBuildHostProvider[] providers = [new WindowsBuildHostProvider(), new MacOSArm64BuildHostProvider()];
        foreach (IBuildHostProvider provider in providers)
            if (provider.TryCapture() is BuildHostDescriptor host)
                return host;
        throw new PlatformNotSupportedException("No registered build host supports this machine.");
    }
}
