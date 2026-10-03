using Inno.Build;

namespace Inno.Build.SupportPacks;

/// <summary>
/// Composes built-in target sources for Editor, MSBuild and the unified command-line host.
/// </summary>
public static class BuiltInPlayerSupportPacks
{
    /// <summary>
    /// Creates a publisher containing built-in desktop and browser target sources.
    /// </summary>
    /// <returns>
    /// An independent publisher whose platform registrations are immutable.
    /// </returns>
    public static PlayerSupportPackPublisher CreatePublisher()
        => new([
            new DesktopPlayerSupportPackSource(
                BuildTargetId.windowsX64,
                "win-x64",
                "windows-x64",
                ".dll",
                new Inno.Build.Platform.Windows.WindowsSupportPackValidator()),
            new DesktopPlayerSupportPackSource(
                BuildTargetId.macOSArm64,
                "osx-arm64",
                "osx-arm64",
                ".dylib",
                new Inno.Build.Platform.MacOS.MacOSSupportPackValidator()),
            new BrowserPlayerSupportPackSource()
        ]);
}
