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
        OperatingSystem.IsLinux(), RuntimeInformation.ProcessArchitecture) switch
    {
        (true, false, false, Architecture.X64) => new("windows-x64", "Inno.Editor.Windows",
            "platforms/Windows/editor/Inno.Editor.Windows/Inno.Editor.Windows.csproj",
            "Inno.Editor.Windows.WindowsEditorComposition"),
        (false, true, false, Architecture.Arm64) => new("macos-arm64", "Inno.Editor.MacOS",
            "platforms/MacOS/editor/Inno.Editor.MacOS/Inno.Editor.MacOS.csproj",
            "Inno.Editor.MacOS.MacOSEditorComposition"),
        (false, false, true, Architecture.X64) => new("linux-x64", null, null, null),
        (false, false, true, Architecture.Arm64) => new("linux-arm64", null, null, null),
        _ => throw new PlatformNotSupportedException(
            $"No development host is implemented for {RuntimeInformation.OSDescription} / {RuntimeInformation.ProcessArchitecture}.")
    };

    internal string RequireEditor() => editorEntryType ?? throw new PlatformNotSupportedException(
        $"'{target}' currently supports Native tooling only. A Linux Editor has not been implemented.");
}
