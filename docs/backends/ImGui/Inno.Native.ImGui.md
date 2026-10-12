# Inno.Native.ImGui

[分类索引](README.md) · [Editor ImGui](../../editor/Inno.Editor.ImGui.md) · [Wiki 首页](../../README.md)

该项目提供 cimgui generated bindings。运行时源码只有 `ImGui.cs` 原生库/函数表加载器和 `Generated/Bindings.cs`。配置与 BGCS emitter plugin 均属于本项目的 `Bindings/` 目录；`ImVector<T>`、`ImTextureID` 的显式 ABI 实现由 `Bindings/Extension` 合入生成文件，而非 BGCS 核心中的库名特判。生成方式见 [Native binding generation](../../native/BindingGeneration.md)。完整声明从本页末尾的 ABI 参考进入。

它只服务 Editor/SDL3-ImGui/BGFX-ImGui adapter。业务脚本与 Runtime 不应直接依赖 Native ImGui。

## 调用工作流与所有权

推荐从共享 ImGui presentation 与 Widgets 使用；直接 Native 调用只属于 backend 实现。

| 阶段 | 本项目对应入口 | 必须保持的条件 |
| --- | --- | --- |
| 建立 context | `CreateContext`、`SetCurrentContext` | 创建者拥有 context；每次操作先选定所属 context，不把进程 current context 当作会话身份。 |
| 开始布局 | `NewFrame` | 平台 owner 已同步窗口尺寸、像素尺寸、输入与 DPI；同一 owner thread 驱动。 |
| 布局窗口 | `Begin`、`End` 及相应 widget | 成对结束布局；Begin 的可见性结果不授予省略 End 的权限。 |
| 渲染 | `Render`、`GetDrawData` | DrawData 与列表、顶点及索引是本 context 当前帧的借用；不保存到下一帧或旧 generation。 |
| 附加窗口关闭 | 通过 presentation renderer 的退休接口 | 先取消路由和提交，再等待渲染 surface 退休；确认后由 SDL 窗口 owner 销毁。 |
| 最终释放 | `DestroyContext` | 先排空 renderer 的 viewport 退休并注销 callbacks、字体登记；禁止在故障或 Pending 时提前销毁依赖 owner。 |

`ImGuiContextPtr`、`ImDrawDataPtr`、`ImVector<T>` 等包装值只描述地址或布局，不自动获得内存 owner。访问前必须确认对应 context/frame 仍有效；复制包装值不延长原生对象寿命。字符串转换和 ABI carrier 以当前生成签名为准，不将任何临时转换缓冲区留给长期 callback。

所有 Native callbacks 的托管 owner 必须保留到注销和排空完成。异常不得穿过 Native callback 边界。实际 context 创建、失败清理和唯一 renderer owner 位于所属 SDL/ImGui presentation；正常业务不要自己再建立第二个 context 管理器。

加载时依据产品 `Deployment.index` 选定库；缺失索引/映射或 required export 必须失败。宿主 ABI 参考不能证明其他目标、DPI 或设备已经实测。

## 源码归属

当前唯一源码 owner：`backends/ImGui/native/Inno.Native.ImGui/Inno.Native.ImGui.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Native.ImGui.ImGui`

<!-- api: T:Inno.Native.ImGui.ImGui 8369a021570193ec214d8295c8de5266b53a0c265ab5d24277ee513b907498c0 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-061ca570826da2e3"></a>[`public static class Inno.Native.ImGui.ImGui`](../../../backends/ImGui/native/Inno.Native.ImGui/ImGui.cs#L9) | Initializes generated imports against the component's dynamic native library. | — | — |

## 项目依赖

- [Inno.Native.LibraryLoading](../Interop/Inno.Native.LibraryLoading.md)：实现依赖，PrivateAssets="compile"。
- `$(BGCSRuntimeProject)`：公开引用边界由实际签名核对。

## 完整 ABI 参考

[按类型与函数分组的 ABI 参考](Inno.Native.ImGui.Api/README.md)保留全部生成声明及稳定锚点。项目页是职责与生命周期的唯一入口；完整参考按同一项目的主题分页，避免在一次阅读中展开数 MB 声明。

<!-- api-include: Inno.Native.ImGui.Api/ABI-01.md -->
<!-- api-include: Inno.Native.ImGui.Api/ABI-02.md -->
<!-- api-include: Inno.Native.ImGui.Api/ABI-03.md -->
<!-- api-include: Inno.Native.ImGui.Api/ABI-04.md -->
<!-- api-include: Inno.Native.ImGui.Api/ABI-05.md -->
<!-- api-include: Inno.Native.ImGui.Api/ABI-06.md -->
<!-- api-include: Inno.Native.ImGui.Api/ABI-07.md -->
<!-- api-include: Inno.Native.ImGui.Api/ABI-08.md -->
<!-- api-include: Inno.Native.ImGui.Api/ABI-09.md -->
<!-- api-include: Inno.Native.ImGui.Api/ABI-10.md -->
<!-- api-include: Inno.Native.ImGui.Api/ABI-11.md -->
<!-- api-include: Inno.Native.ImGui.Api/ABI-12.md -->
<!-- api-include: Inno.Native.ImGui.Api/ABI-13.md -->
<!-- api-include: Inno.Native.ImGui.Api/ABI-14.md -->
<!-- api-include: Inno.Native.ImGui.Api/ABI-15.md -->
<!-- api-include: Inno.Native.ImGui.Api/ABI-16.md -->
<!-- api-include: Inno.Native.ImGui.Api/ABI-17.md -->
<!-- api-include: Inno.Native.ImGui.Api/ABI-18.md -->
<!-- api-include: Inno.Native.ImGui.Api/ABI-19.md -->
<!-- api-include: Inno.Native.ImGui.Api/ABI-20.md -->
<!-- api-include: Inno.Native.ImGui.Api/ABI-21.md -->
<!-- api-include: Inno.Native.ImGui.Api/ABI-22.md -->
<!-- api-include: Inno.Native.ImGui.Api/ABI-23.md -->
<!-- api-include: Inno.Native.ImGui.Api/ABI-24.md -->
<!-- api-include: Inno.Native.ImGui.Api/ABI-25.md -->
<!-- api-include: Inno.Native.ImGui.Api/ABI-26.md -->
<!-- api-include: Inno.Native.ImGui.Api/ABI-27.md -->
