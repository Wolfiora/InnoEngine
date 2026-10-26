using System;
using Inno.Build.MacOSArm64;
using Inno.Build.Toolchains;
using Inno.Integration.MacOSArm64.Bgfx;

namespace Inno.Integration.MacOSArm64.Build;

internal static class MacOSArm64NativeProducts
{
    private const string C_INPUT = "platforms/MacOSArm64/integrations/Inno.Integration.MacOSArm64.Build/Products/MacOSArm64NativeProducts.cs";

    internal static NativeComponentBuildOptions Options(string component)
    {
        return new(NativeLibraryKind.Shared, inputPaths: [C_INPUT],
            bindingIncludeDirectories: component == "bgfx" ? ["extern/bx/include/compat/osx"] : [],
            linkPlan: component == "cimguizmo"
                ? MacOSArm64LinkerSettings.Create(["@loader_path/../../cimgui/" + MacOSArm64BuildModule.target.id.value]) : null);
    }
}
