using Inno.Build.Toolchains;

namespace Inno.Integration.WindowsX64.Build;

internal static class WindowsX64NativeProducts
{
    private const string C_INPUT = "platforms/WindowsX64/integrations/Inno.Integration.WindowsX64.Build/Products/WindowsX64NativeProducts.cs";

    internal static NativeComponentBuildOptions Options(string component)
    {
        return new(NativeLibraryKind.Shared,
            cmakeArguments: component == "cimgui" ? ["-DINNO_CIMGUI_COMPILE_DEFINITIONS=IMGUI_API=__declspec(dllexport)"] : [],
            inputPaths: [C_INPUT]);
    }
}
