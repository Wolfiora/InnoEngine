using System.Collections.Generic;
using Inno.Build.Toolchains;

namespace Inno.Build.MacOSArm64;

/// <summary>
/// Owns the target's native library formats and linker policy.
/// </summary>
public static class MacOSArm64LinkerSettings
{
    /// <summary>
    /// Creates the target linker plan without inspecting component names.
    /// </summary>
    /// <param name="runtimeSearchPaths">
    /// Explicit runtime dependency paths, or null when none are needed.
    /// </param>
    /// <returns>
    /// The immutable target linker plan.
    /// </returns>
    public static NativeLinkPlan Create(IEnumerable<string>? runtimeSearchPaths = null)
        => new(".dylib", null, ".a", ["-DCMAKE_BUILD_WITH_INSTALL_NAME_DIR=ON", "-DCMAKE_INSTALL_NAME_DIR=@rpath", "-DCMAKE_BUILD_RPATH=" + string.Join(';', runtimeSearchPaths ?? [])]);
}
