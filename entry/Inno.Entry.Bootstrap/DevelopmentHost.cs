using System;
using System.Runtime.InteropServices;

namespace Inno.Entry.Bootstrap;

internal sealed record DevelopmentHost(
    string target,
    string? editorAssembly,
    string? editorProject,
    string? editorEntryType
) {
    internal static DevelopmentHost Resolve() => (OperatingSystem.IsWindows(), OperatingSystem.IsMacOS(),
        RuntimeInformation.ProcessArchitecture) switch
    {
        (true, false, Architecture.X64) => new("windows-x64", "Inno.Editor.WindowsX64",
            "platforms/WindowsX64/editor/Inno.Editor.WindowsX64/Inno.Editor.WindowsX64.csproj",
            "Inno.Editor.WindowsX64.WindowsX64EditorComposition"),
        (false, true, Architecture.Arm64) => new("macos-arm64", "Inno.Editor.MacOSArm64",
            "platforms/MacOSArm64/editor/Inno.Editor.MacOSArm64/Inno.Editor.MacOSArm64.csproj",
            "Inno.Editor.MacOSArm64.MacOSArm64EditorComposition"),
        _ => throw new PlatformNotSupportedException(
            $"No development host is implemented for {RuntimeInformation.OSDescription} / {RuntimeInformation.ProcessArchitecture}.")
    };

    internal string RequireEditor() => editorEntryType ?? throw new PlatformNotSupportedException(
        $"'{target}' has no implemented Editor product.");
}
