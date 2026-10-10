# Inno.Native.ImGui

[分类索引](README.md) · [Editor ImGui](../../editor/Inno.Editor.ImGui.md) · [Wiki 首页](../../README.md)

该项目提供 cimgui generated bindings。运行时源码只有 `ImGui.cs` 原生库/函数表加载器和 `Generated/Bindings.cs`。配置与 BGCS emitter plugin 均属于本项目的 `Bindings/` 目录；`ImVector<T>`、`ImTextureID` 的显式 ABI 实现由 `Bindings/Extension` 合入生成文件，而非 BGCS 核心中的库名特判。生成方式见 [Native binding generation](../../native/BindingGeneration.md)。完整清单由生成 XML 提供。

它只服务 Editor/SDL3-ImGui/BGFX-ImGui adapter。业务脚本与 Runtime 不应直接依赖 Native ImGui。

## 源码归属

当前唯一源码 owner：`backends/ImGui/native/Inno.Native.ImGui/Inno.Native.ImGui.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 当前源码公开 API 清单

只列当前源码 public/protected 表面；内部机制不是稳定 API，参数、返回、失败及所有权以英文 XML 为准。

### `Inno.Native.ImGui.ImGui`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Native.ImGui.ImGui`](../../../backends/ImGui/native/Inno.Native.ImGui/ImGui.cs#L9) | Initializes generated imports against the component's dynamic native library. |

## 项目依赖

- [Inno.Native.LibraryLoading](../Interop/Inno.Native.LibraryLoading.md)：实现依赖，PrivateAssets="compile"。
- `$(BGCSRuntimeProject)`：公开引用边界由实际签名核对。
