using Inno.Build.Toolchains;

namespace Inno.Build.BrowserWasm32;

/// <summary>
/// Owns the target's native library formats and linker policy.
/// </summary>
public static class BrowserWasm32LinkerSettings
{
    /// <summary>
    /// Creates the target linker plan without inspecting component names.
    /// </summary>
    /// <returns>
    /// The immutable target linker plan.
    /// </returns>
    public static NativeLinkPlan Create()
        => new(staticLibraryExtension: ".a");
}
