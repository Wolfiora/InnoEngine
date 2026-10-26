# Inno.Native.ImGui

[分类索引](README.md) · [Editor ImGui](../../editor/Inno.Editor.ImGui.md) · [Wiki 首页](../../README.md)

该项目提供 cimgui generated bindings。运行时源码只有 `ImGui.cs` 原生库/函数表加载器和 `Generated/Bindings.cs`。正式配置属于本项目 `Bindings/`；容器、纹理引用、指针与 DTO 均采用 SDK 的实际生成 ABI，Native 类型不越过适配边界。生成方式见 [Native binding generation](../../native/BindingGeneration.md)。完整声明从本页末尾的 ABI 参考进入。

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

`ImGuiNativeContext*`、`ImDrawData*` 和专门化 vector 存储只描述地址或布局，不自动获得内存 owner。访问前必须确认对应 context/frame 仍有效；复制包装值不延长原生对象寿命。字符串转换和 ABI carrier 以当前生成签名为准，不将任何临时转换缓冲区留给长期 callback。

所有 Native callbacks 的托管 owner 必须保留到注销和排空完成。异常不得穿过 Native callback 边界。实际 context 创建、失败清理和唯一 renderer owner 位于所属 SDL/ImGui presentation；正常业务不要自己再建立第二个 context 管理器。

## Viewport callback ABI 适配

BGCS 4.1.2 对无法安全表达的聚合参数 callback 保留不透明地址；它不保证这些地址可以强转成托管函数指针。`ImVec2` 按值传递的窗口位置、尺寸 callback 因目标 ABI 而具有不同的寄存器分类。`Native/ImGuiPlatformCallbacks.*` 是此限制所需的窄 C ABI adapter：C++ 使用真实 ImGui callback 签名，再以 `viewport, float, float` 调用托管侧。所有目标共用此实现，不维护 CPU 分支，不修改 BGCS、cimgui 或生成输出。

`Raw.InnoCreatePlatformWindowCallbacks` 只在当前 context、owner thread 和未占用的 `BackendPlatformUserData` 上注册；空结果不保留 callback。返回 handle 同时拥有两个 callback，其寿命必须短于 context。两个 callback 的目标是宿主静态方法，不保存插件实例、delegate 或 collectible 类型。SDK 配置显式声明 registration lifetime、event-loop threading、空结果和注销 quiescence；这里不使用自动 owning wrapper，因为两个 callback 共同属于一个需要 owner-thread 检查的 handle。

`Raw.InnoCanDestroyPlatformWindowCallbacks` 检查 owner thread、current context、userdata 归属和原生 callback frame 是否退出。只有它成功且同线程没有介入其他原生调用时，才能执行 `Raw.InnoDestroyPlatformWindowCallbacks`；注销清空两个槽与 userdata，再释放 handle。callback 内或错误 context/thread 不能退休，presentation 保留 owner 并报告 `RetirementPendingException`。注销后 handle 失效，不能再次查询或释放。

原生回归通过真实 `NewFrame`、窗口布局和 `UpdatePlatformWindows` 触发两个 callback，核对位置与尺寸标量、callback 内禁止退休、独立 context 与错误线程拒绝；不通过强转不透明地址或测试专用导出来证明 ABI。

加载时依据产品 `Deployment.index` 选定库；缺失索引/映射或 required export 必须失败。宿主 ABI 参考不能证明其他目标、DPI 或设备已经实测。

## 源码归属

当前唯一源码 owner：`backends/ImGui/native/Inno.Native.ImGui/Inno.Native.ImGui.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 当前源码公开 API 清单

当前签名由正式源码语义清单核对；生命周期与错误约束见前文及对应 XML。

<!-- api: T:Inno.Native.ImGui.ImGui 8369a021570193ec214d8295c8de5266b53a0c265ab5d24277ee513b907498c0 -->

| 声明 | 行为与约束 |
| --- | --- |
| <a id="api-061ca570826da2e3"></a>[`public static class Inno.Native.ImGui.ImGui`](../../../backends/ImGui/native/Inno.Native.ImGui/ImGui.cs#L8) | Initializes the explicitly deployed module through the generated BGCS API.<br>Owns one explicitly shared context. Replacement closes the previous context; existing resource leases retain its native cleanup capability. |

## 项目依赖

- [Inno.Native.LibraryLoading](../Interop/Inno.Native.LibraryLoading.md)：实现依赖，PrivateAssets="compile"。
- `BGCS.Runtime`：通过固定版本 NuGet 包提供互操作运行时，不引用生成器。

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

<!-- api-include: Inno.Native.ImGui.Api/ABI-28.md -->
