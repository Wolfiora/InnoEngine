# Inno.Native.Bgfx

[分类索引](README.md) · [Rendering BGFX](Inno.Adapter.Rendering.Bgfx.md) · [Wiki 首页](../../README.md)

该项目提供 BGFX C API 的 generated binding surface。`Generated/Bindings.cs` 由 BGCS 生成 handle、enum、descriptor、callback 和 API；`bgfx.cs` 只负责原生库加载。完整声明见本页生成 ABI 索引；这些声明保持 backend 边界，不成为中立领域 API。

离线工具执行器 `BgfxTool`、`ToolRunner` 和 `ToolRunResult` 属于
[BGFX build toolchain](Inno.Build.Toolchains.Bgfx.Tools.md) 的 `Execution/`。
Native 项目只持有绑定和库加载边界，不引用离线进程执行、Shader 创作或构建策略。
宿主明确部署本次工具产物，运行时加载器不扫描仓库或复制旧 `.lib` 输出。

只有 `Inno.Adapter.Rendering.Bgfx`、BGFX toolchain 和 Native tests 可以引用本项目。上层 public/protected API 不得泄漏任一 BGFX 类型或原生指针。

## 静态目标

同一项目通过 bindgen.browser-wasm.json 生成 wasm32 的静态符号绑定，继承 common.json 的声明；目标编译使用 INNO_STATIC_NATIVE，不初始化动态 loader。Host profile 仍使用原动态库命名与显式初始化。生成输出分别属于宿主 Generated/Bindings.cs 和目标 `artifacts/bindings/bgfx/browser-wasm/<generationFingerprint>/Generated/Bindings.cs`，没有 Native Browser 副本。

## 源码归属

当前唯一源码 owner：`backends/Bgfx/native/Inno.Native.Bgfx/Inno.Native.Bgfx.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Native.Bgfx.bgfx`

<!-- api: T:Inno.Native.Bgfx.bgfx b565d3fb16c45cb493209a0c0900ec59bee9b046223c4cc8aaaa14e084abf33e -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-6653f12dad3194b1"></a>[`public static class Inno.Native.Bgfx.bgfx`](../../../backends/Bgfx/native/Inno.Native.Bgfx/bgfx.cs#L8) | bgfx native bindings loader. | — | — |

## 项目依赖

- [Inno.Native.LibraryLoading](../Interop/Inno.Native.LibraryLoading.md)：实现依赖，PrivateAssets="compile"。
- `$(BGCSRuntimeProject)`：公开引用边界由实际签名核对。

## 完整 ABI 参考

[按类型与函数分组的 ABI 参考](Inno.Native.Bgfx.Api/README.md)保留全部生成声明及稳定锚点。项目页是职责与生命周期的唯一入口；完整参考按同一项目的主题分页，避免在一次阅读中展开数 MB 声明。

<!-- api-include: Inno.Native.Bgfx.Api/ABI-01.md -->
<!-- api-include: Inno.Native.Bgfx.Api/ABI-02.md -->
<!-- api-include: Inno.Native.Bgfx.Api/ABI-03.md -->
<!-- api-include: Inno.Native.Bgfx.Api/ABI-04.md -->
