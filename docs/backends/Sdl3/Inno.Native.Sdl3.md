# Inno.Native.Sdl3

[分类索引](README.md) · [Platform SDL3](Inno.Adapter.Platform.Sdl3.md) · [Wiki 首页](../../README.md)

该项目是 SDL3 generated binding assembly。`Generated/Bindings.cs` 由 BGCS 从 SDL3 header 和配置生成函数、结构、Flags enum、delegate 与 pointer wrapper；`SDL.cs` 仅加载原生库并初始化函数表。不另附手写 `Point32`、`Msg` 或 Flags 类型。完整声明见本页生成 ABI 索引。

只有 `Inno.Adapter.Platform.Sdl3`、SDL3 toolchain 与 Native tests 可以引用。上层通过中立 `IPlatformApplication`/`IPlatformWindow` 工作，不直接调用 SDL。

## 静态目标

组件仅维护公共 `Bindings/bindgen.json`。Browser Build integration 提供明确 wasm32 ABI、托管 runtime 和静态库名称；Windows、macOS integration 分别提供自己的 Profile，组件没有平台目标表。静态产品使用 `INNO_STATIC_NATIVE`，不初始化动态 loader。HostSource 输出属于组件的 `Generated/Bindings.cs` 和 `Native/Generated`；TargetArtifacts 的 managed/native 输出共同属于 `artifacts/bindings/sdl3/<target>/<generationFingerprint>`。是否可运行以[当前验收](../../architecture/CURRENT_ACCEPTANCE.md)为准。

## 声明与实际导出

共同定义排除 SDL 头文件中声明的编译器内建函数 `__debugbreak`。它属于 MSVC 的断言实现，不是 SDL DLL 的导出。该规则属于 SDL 的绑定定义，BGCS 不包含 SDL 专用识别逻辑。动态函数表初始化要求全部已选择符号存在；Native 测试通过实际初始化和调用检查部署闭包。

## 源码归属

当前唯一源码 owner：`backends/Sdl3/native/Inno.Native.Sdl3/Inno.Native.Sdl3.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 当前源码公开 API 清单

以下签名来自当前源码与正式生成结果；原生所有权、线程及关闭顺序见本页前文。

<!-- api: T:Inno.Native.Sdl3.SDL 75f9ea5744d371e26cd438217c5f4020f8be05b1e992b1a0a022671c99672036 -->

| 声明 | 语义与限制 |
| --- | --- |
| <a id="api-e67f3d5045477c09"></a>[`public static class Inno.Native.Sdl3.SDL`](../../../backends/Sdl3/native/Inno.Native.Sdl3/SDL.cs#L8) | Initializes the explicitly deployed module through the generated BGCS API. |

## 项目依赖

- [Inno.Native.LibraryLoading](../Interop/Inno.Native.LibraryLoading.md)：实现依赖，PrivateAssets="compile"。
- `BGCS.Runtime`：通过固定版本 NuGet 包提供互操作运行时，不引用生成器。

## 完整 ABI 参考

[按类型与函数分组的 ABI 参考](Inno.Native.Sdl3.Api/README.md)保留全部生成声明及稳定锚点。项目页是职责与生命周期的唯一入口；完整参考按同一项目的主题分页，避免在一次阅读中展开数 MB 声明。

<!-- api-include: Inno.Native.Sdl3.Api/ABI-01.md -->
<!-- api-include: Inno.Native.Sdl3.Api/ABI-02.md -->
<!-- api-include: Inno.Native.Sdl3.Api/ABI-03.md -->
<!-- api-include: Inno.Native.Sdl3.Api/ABI-04.md -->
<!-- api-include: Inno.Native.Sdl3.Api/ABI-05.md -->
<!-- api-include: Inno.Native.Sdl3.Api/ABI-06.md -->
<!-- api-include: Inno.Native.Sdl3.Api/ABI-07.md -->
<!-- api-include: Inno.Native.Sdl3.Api/ABI-08.md -->
<!-- api-include: Inno.Native.Sdl3.Api/ABI-09.md -->
<!-- api-include: Inno.Native.Sdl3.Api/ABI-10.md -->
<!-- api-include: Inno.Native.Sdl3.Api/ABI-11.md -->
<!-- api-include: Inno.Native.Sdl3.Api/ABI-12.md -->
<!-- api-include: Inno.Native.Sdl3.Api/ABI-13.md -->
<!-- api-include: Inno.Native.Sdl3.Api/ABI-14.md -->
