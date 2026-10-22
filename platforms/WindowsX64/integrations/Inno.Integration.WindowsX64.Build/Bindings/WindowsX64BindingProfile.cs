using BGCS.Configuration;
using BGCS.Contracts.Targets;
using Inno.Build.Bindings;
using Inno.Build.WindowsX64;

namespace Inno.Integration.WindowsX64.Build;

internal static class WindowsX64BindingProfile
{
    internal static NativeBindingProfile Create() => new(
        WindowsX64BuildModule.target.id.value,
        new TargetConfiguration { id = "windows-x64-msvc" },
        ManagedRuntimeKind.CoreClr,
        ["platforms/WindowsX64/integrations/Inno.Integration.WindowsX64.Build/Bindings/WindowsX64BindingProfile.cs"]);
}
