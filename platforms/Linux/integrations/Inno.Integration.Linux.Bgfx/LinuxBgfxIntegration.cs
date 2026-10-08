using System;
using Inno.Build.Toolchains.Bgfx;

namespace Inno.Integration.Linux.Bgfx;

/// <summary>
/// Connects the implemented Linux SDK targets to BGFX tool builds without registering a Player.
/// </summary>
public static class LinuxBgfxIntegration
{
    /// <summary>
    /// Resolves SDK invocation data for an implemented Linux native target.
    /// </summary>
    /// <param name="targetId">
    /// The explicitly requested native target.
    /// </param>
    /// <returns>
    /// Immutable backend configuration; unsupported targets fail without tool execution.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// The native target is not implemented by this module.
    /// </exception>
    public static BgfxNativeBuildProfile CreateNativeProfile(string targetId)
        => targetId is "linux-x64" or "linux-arm64" ? new LinuxBgfxBuildProfile(targetId)
            : throw new NotSupportedException($"Linux native target '{targetId}' is unavailable.");

}
