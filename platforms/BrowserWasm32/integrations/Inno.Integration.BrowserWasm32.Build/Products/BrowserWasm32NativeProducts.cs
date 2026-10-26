using System;
using Inno.Build.BrowserWasm32;
using Inno.Build.Toolchains;
using Inno.Integration.BrowserWasm32.Bgfx;

namespace Inno.Integration.BrowserWasm32.Build;

internal static class BrowserWasm32NativeProducts
{
    private const string C_INPUT = "platforms/BrowserWasm32/integrations/Inno.Integration.BrowserWasm32.Build/Products/BrowserWasm32NativeProducts.cs";

    internal static NativeComponentBuildOptions Options(string component)
    {
        NativeComponentBuildOptions graphics = BrowserWasm32BgfxIntegration.nativeOptions;
        return new(NativeLibraryKind.Static,
            component == "bgfx" ? graphics.cmakeArguments : [],
            [C_INPUT, ..(component == "bgfx" ? graphics.inputPaths : [])],
            bindingLibraryName: component switch
            {
                "bgfx" => "bgfxRelease",
                "sdl3" => "libSDL3",
                "miniaudio" => "libminiaudio",
                "text" => "libinno-text",
                "ui" => "libinno-ui",
                _ => throw new ArgumentException("This component is outside the Browser product closure.", nameof(component))
            });
    }
}
