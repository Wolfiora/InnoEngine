using BGCS.Configuration;
using BGCS.Core.Targets;
using Inno.Build.Bindings;
using Inno.Build.MacOSArm64;

namespace Inno.Integration.MacOSArm64.Build;

internal static class MacOSArm64BindingProfile
{
    internal static NativeBindingProfile Create() => new(
        MacOSArm64BuildModule.target.id.value,
        new TargetConfiguration { id = "macos-arm64-darwin" },
        new ManagedRuntimeId("dotnet.coreclr"),
        ["platforms/MacOSArm64/integrations/Inno.Integration.MacOSArm64.Build/Bindings/MacOSArm64BindingProfile.cs"]);
}
