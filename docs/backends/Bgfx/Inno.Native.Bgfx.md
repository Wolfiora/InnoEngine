# Inno.Native.Bgfx

[分类索引](README.md) · [Rendering BGFX](Inno.Adapter.Rendering.Bgfx.md) · [Wiki 首页](../../README.md)

该项目提供 BGFX C API 的 generated binding surface。`Generated/Bindings.cs` 由 BGCS 生成 handle、enum、descriptor、callback 和 API；`bgfx.cs` 只负责原生库加载。完整声明见本页生成 ABI 索引；这些声明保持 backend 边界，不成为中立领域 API。

离线工具执行器 `BgfxTool`、`ToolRunner` 和 `ToolRunResult` 属于
[BGFX build toolchain](Inno.Build.Toolchains.Bgfx.Tools.md) 的 `Execution/`。
Native 项目只持有绑定和库加载边界，不引用离线进程执行、Shader 创作或构建策略。
宿主明确部署本次工具产物，运行时加载器不扫描仓库或复制旧 `.lib` 输出。

只有 `Inno.Adapter.Rendering.Bgfx`、BGFX toolchain 和 Native tests 可以引用本项目。上层 public/protected API 不得泄漏任一 BGFX 类型或原生指针。

## 静态目标

组件仅维护公共 `Bindings/bindgen.json`。Browser Build integration 提供明确 wasm32 ABI、托管 runtime 和静态库名称；Windows、macOS integration 分别提供自己的 Profile，组件没有平台目标表。静态产品使用 `INNO_STATIC_NATIVE`，不初始化动态 loader。HostSource 输出属于组件的 `Generated/Bindings.cs` 和 `Native/Generated`；TargetArtifacts 的 managed/native 输出共同属于 `artifacts/bindings/bgfx/<target>/<generationFingerprint>`。是否可运行以[当前验收](../../architecture/CURRENT_ACCEPTANCE.md)为准。

## 源码归属

当前唯一源码 owner：`backends/Bgfx/native/Inno.Native.Bgfx/Inno.Native.Bgfx.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 当前源码公开 API 清单

当前签名由正式源码语义清单核对；生命周期与错误约束见前文及对应 XML。

<!-- api: T:Inno.Native.Bgfx.bgfx b565d3fb16c45cb493209a0c0900ec59bee9b046223c4cc8aaaa14e084abf33e -->

| 声明 | 行为与约束 |
| --- | --- |
| <a id="api-6653f12dad3194b1"></a>[`public static class Inno.Native.Bgfx.bgfx`](../../../backends/Bgfx/native/Inno.Native.Bgfx/bgfx.cs#L8) | Initializes the explicitly deployed module through the generated BGCS API.<br>Owns one explicitly shared context. Replacement closes the previous context; existing resource leases retain its native cleanup capability. |

## 项目依赖

- [Inno.Native.LibraryLoading](../Interop/Inno.Native.LibraryLoading.md)：实现依赖，PrivateAssets="compile"。
- `BGCS.Runtime`：通过固定版本 NuGet 包提供互操作运行时，不引用生成器。

## 完整 ABI 参考

[按类型与函数分组的 ABI 参考](Inno.Native.Bgfx.Api/README.md)保留全部生成声明及稳定锚点。项目页是职责与生命周期的唯一入口；完整参考按同一项目的主题分页，避免在一次阅读中展开数 MB 声明。

<!-- api-include: Inno.Native.Bgfx.Api/ABI-01.md -->
<!-- api-include: Inno.Native.Bgfx.Api/ABI-02.md -->
<!-- api-include: Inno.Native.Bgfx.Api/ABI-03.md -->
<!-- api-include: Inno.Native.Bgfx.Api/ABI-04.md -->
