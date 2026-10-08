using System;
using System.Collections.Generic;
using Inno.Build.Composition;

namespace Inno.Build.Linux;

/// <summary>
/// Contributes the existing Linux native SDK capabilities without registering an unsupported Player product.
/// </summary>
public static class LinuxBuildModule
{
    /// <summary>
    /// Gets the implemented explicit native targets and their SDK providers.
    /// </summary>
    /// <param name="context">
    /// The explicit managed generator and tool execution host.
    /// </param>
    /// <returns>
    /// Immutable SDK contributions; these targets do not appear in game publication choices.
    /// </returns>
    public static IReadOnlyList<NativeToolchainContribution> CreateNativeContributions(BuildCompositionContext context) =>
        [new(new(new BuildTargetId("linux-x64"), "Linux", "x64", "linux-gnu-x64", "linux-x64", false),
                new LinuxNativeToolchainProvider(context.dotnetHost)),
            new(new(new BuildTargetId("linux-arm64"), "Linux", "arm64", "linux-gnu-arm64", "linux-arm64", false),
                new LinuxNativeToolchainProvider(context.dotnetHost))];
}
