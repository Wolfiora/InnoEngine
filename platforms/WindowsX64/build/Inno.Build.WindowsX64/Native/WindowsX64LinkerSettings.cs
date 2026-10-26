using Inno.Build.Toolchains;

namespace Inno.Build.WindowsX64;

/// <summary>
/// Owns the target's native library formats and linker policy.
/// </summary>
public static class WindowsX64LinkerSettings
{
    /// <summary>
    /// Creates the target linker plan without inspecting component names.
    /// </summary>
    /// <returns>
    /// The immutable target linker plan.
    /// </returns>
    public static NativeLinkPlan Create()
        => new(".dll", ".lib", ".lib", []);
}
