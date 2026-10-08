# Inno.Native.Sdl3

[分类索引](README.md) · [Platform SDL3](Inno.Adapter.Platform.Sdl3.md) · [Wiki 首页](../../README.md)

该项目是 SDL3 generated binding assembly。`Generated/Bindings.cs` 由 BGCS 从 SDL3 header 和配置生成函数、结构、Flags enum、delegate 与 pointer wrapper；`SDL.cs` 仅加载原生库并初始化函数表。不另附手写 `Point32`、`Msg` 或 Flags 类型。精确成员以生成 XML 为准。

只有 `Inno.Adapter.Platform.Sdl3`、SDL3 toolchain 与 Native tests 可以引用。上层通过中立 `IPlatformApplication`/`IPlatformWindow` 工作，不直接调用 SDL。

## 静态目标

同一项目通过 `Bindings/bindgen.browser-wasm.json` 生成 wasm32 的静态符号绑定，继承 `common.json` 的声明；目标编译使用 `INNO_STATIC_NATIVE`，不初始化动态 loader。Host profile 使用动态库命名与显式初始化。宿主生成输出位于 `Generated/Bindings.cs`，目标输出位于 `obj/browser-wasm/<generationFingerprint>/Generated/Bindings.cs`。

## 声明与实际导出

共同定义排除 SDL 头文件中声明的编译器内建函数 `__debugbreak`。它属于 MSVC 的断言实现，不是 SDL DLL 的导出。该规则属于 SDL 的绑定定义，BGCS 不包含 SDL 专用识别逻辑。动态函数表初始化要求全部已选择符号存在；Native 测试通过实际初始化和调用检查部署闭包。

## 源码归属

当前唯一源码 owner：`backends/Sdl3/native/Inno.Native.Sdl3/Inno.Native.Sdl3.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Native.Sdl3.SDL`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Native.Sdl3.SDL`](../../../backends/Sdl3/native/Inno.Native.Sdl3/SDL.cs#L10) | Initializes generated imports against the component's dynamic native library. |

## 项目依赖

- [Inno.Native.LibraryLoading](../Interop/Inno.Native.LibraryLoading.md)：实现依赖，PrivateAssets="compile"。
- `$(BGCSRuntimeProject)`：公开引用边界由实际签名核对。
