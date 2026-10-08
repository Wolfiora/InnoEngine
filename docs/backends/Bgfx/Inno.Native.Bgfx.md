# Inno.Native.Bgfx

[分类索引](README.md) · [Rendering BGFX](Inno.Adapter.Rendering.Bgfx.md) · [Wiki 首页](../../README.md)

该项目提供 BGFX C API 的 generated binding surface。`Generated/Bindings.cs` 由 BGCS 生成 handle、enum、descriptor、callback 和 API；`bgfx.cs` 只负责原生库加载。完整成员契约由同项目生成 XML 记录，Wiki 不把生成实现提升为引擎稳定领域 API。

离线工具执行器 `BgfxTool`、`ToolRunner` 和 `ToolRunResult` 属于
[BGFX build toolchain](Inno.Build.Toolchains.Bgfx.Tools.md) 的 `Execution/`。
Native 项目只持有绑定和库加载边界，不引用离线进程执行、Shader 创作或构建策略。
宿主明确部署本次工具产物，运行时加载器不扫描仓库或复制旧 `.lib` 输出。

只有 `Inno.Adapter.Rendering.Bgfx`、BGFX toolchain 和 Native tests 可以引用本项目。上层 public/protected API 不得泄漏任一 BGFX 类型或原生指针。

## 静态目标

同一项目通过 bindgen.browser-wasm.json 生成 wasm32 的静态符号绑定，继承 common.json 的声明；目标编译使用 INNO_STATIC_NATIVE，不初始化动态 loader。Host profile 仍使用原动态库命名与显式初始化。生成输出分别属于宿主 Generated/Bindings.cs 和目标 obj/browser-wasm/<generationFingerprint>/Generated/Bindings.cs，没有 Native Browser 副本。

## 源码归属

当前唯一源码 owner：`backends/Bgfx/native/Inno.Native.Bgfx/Inno.Native.Bgfx.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Native.Bgfx.bgfx`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Native.Bgfx.bgfx`](../../../backends/Bgfx/native/Inno.Native.Bgfx/bgfx.cs#L8) | bgfx native bindings loader. |

## 项目依赖

- [Inno.Native.LibraryLoading](../Interop/Inno.Native.LibraryLoading.md)：实现依赖，PrivateAssets="compile"。
- `$(BGCSRuntimeProject)`：公开引用边界由实际签名核对。
