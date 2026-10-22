using BGCS.Configuration;
using BGCS.Contracts.Targets;
using Inno.Build.Bindings;
using Inno.Build.BrowserWasm32;

namespace Inno.Integration.BrowserWasm32.Build;

internal static class BrowserWasm32BindingProfile
{
    internal static NativeBindingProfile Create() => new(
        BrowserWasm32BuildModule.target.id.value,
        new TargetConfiguration { id = "emscripten-wasm32-emscripten" },
        ManagedRuntimeKind.BrowserWasm,
        ["platforms/BrowserWasm32/integrations/Inno.Integration.BrowserWasm32.Build/Bindings/BrowserWasm32BindingProfile.cs"]);
}
