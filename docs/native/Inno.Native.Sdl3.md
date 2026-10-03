# Inno.Native.Sdl3

[Native 索引](README.md) · [Platform SDL3](../platform/Inno.Adapter.Platform.Sdl3.md)

该项目是 SDL3 generated binding assembly。`Generated/Bindings.cs` 由 BGCS 从 SDL3 header 和配置生成函数、结构、Flags enum、delegate 与 pointer wrapper；`SDL.cs` 仅加载原生库并初始化函数表。不另附手写 `Point32`、`Msg` 或 Flags 类型。精确成员以生成 XML 为准。

只有 `Inno.Adapter.Platform.Sdl3`、SDL3 toolchain 与 Native tests 可以引用。上层通过中立 `IPlatformApplication`/`IPlatformWindow` 工作，不直接调用 SDL。

## 静态目标

同一项目通过 bindgen.browser-wasm.json 生成 wasm32 的静态符号绑定，继承 common.json 的声明；目标编译使用 INNO_STATIC_NATIVE，不初始化动态 loader。Host profile 仍使用原动态库命名与显式初始化。生成输出分别属于宿主 Generated/Bindings.cs 和目标 obj/browser-wasm/Generated/Bindings.cs，没有 Native Browser 副本。
