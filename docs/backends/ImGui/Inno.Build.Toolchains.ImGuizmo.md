# Inno.Build.Toolchains.ImGuizmo

[分类索引](README.md) · [Wiki 首页](../../README.md) · [统一 CLI](../../build/Inno.Build.Cli.md) · [Native](Inno.Native.ImGuizmo.md)

## 职责、依赖和公开 API

组件的目标原生构建库，没有 Program。复用 ToolchainEnvironment、现有平台 builder 和产物复制规则，将 pinned extern/Native 源构建为显式目标；CreateRecipe 提供准备前声明，实际构建入口是 `ImGuizmoToolchain.BuildAsync(NativeBuildContext context, NativeBuildProduct imGui, CancellationToken cancellationToken = default)`。context 明确给出源码 checkout 与 debug/release 配置；取消杀死活动进程树，构建失败明确抛出。没有供外部派生者使用的 protected 扩展点；平台 builder 是内部实现。

只依赖共同构建契约。调用方显式传入已发布的 ImGui product，验证 component、target 和唯一链接库；不会访问另一个工具链的中间目录。两个组件的中间态分别属于自己的 `obj/native/<target>/<workspaceIdentity>`。

```csharp
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Build.Toolchains.ImGuizmo;

static Task<NativeBuildProduct> PrepareGizmoAsync(
    NativeBuildContext preparedContext,
    NativeBuildProduct imGui,
    CancellationToken cancellationToken
) {
    return ImGuizmoToolchain.BuildAsync(preparedContext, imGui, cancellationToken);
}
```

上述调用用于已有构建操作：`preparedContext` 必须包含冻结的工具链、该组件的构建选项及所需绑定 generation。调用方在整个编译期间保留这些输入的读取保护，并在部署或链接完成后 Dispose 返回的 `NativeBuildProduct`。正常产品构建由发行组合的 `ProductNativeBuildPlan` 统一准备这些依赖。


## 常见流程与生命周期

通常使用 `InnoEngine.Build editor` 按依赖顺序构建全部组件，再构建 Editor；Support Pack 通过独立命令准备。库不启动其他组件 Program。构建返回 `NativeBuildProduct`；产物属于 `artifacts/native/<component>/<target>/<fingerprint>`，配置、源码、编译器与 SDK 共同决定指纹。升级 Native facade 或 extern 后，先通过统一 bindings 路线生成，再重新构建相关目标。失败不表示可以部署缺失的动态库；最终 closure 必须通过平台 validator。

具体组件只读取 context.engineRoot，CMake/overlay 与产物目录不从工具程序集位置选择。取消检查发生在进程启动和产物复制前；已安装的可重建产物不作为成功发布的替代品。

## 源码归属

当前唯一源码 owner：`backends/ImGui/build/Inno.Build.Toolchains.ImGuizmo/Inno.Build.Toolchains.ImGuizmo.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 当前源码公开 API 清单

当前声明由源码语义清单核对，包含 public 和可供外部派生实现使用的 protected 成员。构建契约不作为游戏脚本导出；线程、失败与所有权要求见前文及每项 XML。

### `Inno.Build.Toolchains.ImGuizmo.ImGuizmoToolchain`

<!-- api: T:Inno.Build.Toolchains.ImGuizmo.ImGuizmoToolchain 15c8e72ef65d4950609548919db88ecdb1f5aaa453fc70790a228550fbf93dee -->
<!-- api: P:Inno.Build.Toolchains.ImGuizmo.ImGuizmoToolchain.componentDescriptor 8c17fa661de61bd5f047badc60fb617f7940f29cb3fb5fc7c9998b08cb1d8a58 -->
<!-- api: M:Inno.Build.Toolchains.ImGuizmo.ImGuizmoToolchain.BuildAsync(Inno.Build.Toolchains.NativeBuildContext,Inno.Build.Toolchains.NativeBuildProduct,System.Threading.CancellationToken) 1aa10b8217b67c6bc41952a81683b690bbe65588abaf18d1cd0f2bdb849c482c -->
<!-- api: M:Inno.Build.Toolchains.ImGuizmo.ImGuizmoToolchain.CreateRecipe(Inno.Build.Toolchains.NativeBuildContext) 4a65a038232a2656dd0c8443b6a0e65cf4e514023e18bfe36f615e2f6a353293 -->
<!-- api: M:Inno.Build.Toolchains.ImGuizmo.ImGuizmoToolchain.BindImGui(Inno.Build.Toolchains.NativeBuildContext,Inno.Build.Toolchains.NativeBuildRecipe,Inno.Build.Toolchains.NativeBuildProduct) 64bae928c54972e481c912a47c931f6a2942f5183603f156bbfaede3966c9b6b -->

| 声明 | 行为、参数与结果 |
| --- | --- |
| <a id="api-3edcd1cd76a4bd6b"></a>[`public static Inno.Build.Toolchains.NativeBuildRecipe Inno.Build.Toolchains.ImGuizmo.ImGuizmoToolchain.BindImGui(Inno.Build.Toolchains.NativeBuildContext context, Inno.Build.Toolchains.NativeBuildRecipe sourceRecipe, Inno.Build.Toolchains.NativeBuildProduct imGui)`](../../../backends/ImGui/build/Inno.Build.Toolchains.ImGuizmo/ImGuizmoToolchain.cs#L149) | Freezes the exact ImGui link input for both compilation and complete product cache lookup.<br>`context`：The operation owning the explicit target SDK.<br>`sourceRecipe`：The component's previously declared source recipe.<br>`imGui`：The borrowed, integrity-checked dependency matching this target.<br>返回：A recipe including the selected link library and dependency fingerprint.<br>T:System.ArgumentException：The dependency belongs to a different component or target.<br>T:System.InvalidOperationException：The dependency does not expose exactly one link library. |
| <a id="api-395f2da4230f08fb"></a>[`public static System.Threading.Tasks.Task<Inno.Build.Toolchains.NativeBuildProduct> Inno.Build.Toolchains.ImGuizmo.ImGuizmoToolchain.BuildAsync(Inno.Build.Toolchains.NativeBuildContext context, Inno.Build.Toolchains.NativeBuildProduct imGui, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../backends/ImGui/build/Inno.Build.Toolchains.ImGuizmo/ImGuizmoToolchain.cs#L49) | Builds and installs the component for the explicitly selected native target.<br>`context`：The checkout and configuration whose sources and outputs belong to this operation.<br>`imGui`：The matching published ImGui product providing the native link dependency.<br>T:System.ArgumentException：The configuration is invalid.<br>T:System.InvalidOperationException：A native build process fails.<br>`cancellationToken`：Cancels child processes and prevents artifact installation after cancellation.<br>返回：The validated gizmo library linked against the explicitly supplied ImGui product.<br>T:System.OperationCanceledException：The operation was canceled. |
| <a id="api-044245144f0cecef"></a>[`public static Inno.Build.Toolchains.NativeBuildRecipe Inno.Build.Toolchains.ImGuizmo.ImGuizmoToolchain.CreateRecipe(Inno.Build.Toolchains.NativeBuildContext context)`](../../../backends/ImGui/build/Inno.Build.Toolchains.ImGuizmo/ImGuizmoToolchain.cs#L112) | Declares gizmo source inputs before preparing bindings or its ImGui link dependency.<br>`context`：The operation with a frozen target SDK and explicit shared component options.<br>返回：The source recipe; execution adds the validated ImGui library and its artifact identity.<br>T:System.ArgumentNullException：The context is null.<br>T:System.InvalidOperationException：Required upstream source, SDK or component options are unavailable.<br>T:System.NotSupportedException：The requested linkage is not supported by this recipe. |
| <a id="api-2d14cacb08205e7d"></a>[`public static Inno.Build.Toolchains.NativeComponentDescriptor Inno.Build.Toolchains.ImGuizmo.ImGuizmoToolchain.componentDescriptor { get; }`](../../../backends/ImGui/build/Inno.Build.Toolchains.ImGuizmo/ImGuizmoToolchain.cs#L19) | Gets the unique native and recipe owners used for sources and target-scoped intermediates. |
| <a id="api-4977392cfe1a4a7c"></a>[`public static class Inno.Build.Toolchains.ImGuizmo.ImGuizmoToolchain`](../../../backends/ImGui/build/Inno.Build.Toolchains.ImGuizmo/ImGuizmoToolchain.cs#L14) | Builds and installs this component through the shared native process lifecycle. |

## 项目依赖

- [Inno.Build.Toolchains](../../build/Inno.Build.Toolchains.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。


## 本轮公开契约补充

以下声明对应当前源码；工作流、所有权与失败说明见本页前文。

<!-- api: M:Inno.Build.Toolchains.ImGuizmo.ImGuizmoToolchain.BindImGui(Inno.Build.Toolchains.NativeBuildContext,Inno.Build.Toolchains.NativeBuildRecipe,Inno.Build.Toolchains.NativeBuildProduct) 64bae928c54972e481c912a47c931f6a2942f5183603f156bbfaede3966c9b6b -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-3edcd1cd76a4bd6b"></a>[`public static Inno.Build.Toolchains.NativeBuildRecipe Inno.Build.Toolchains.ImGuizmo.ImGuizmoToolchain.BindImGui(Inno.Build.Toolchains.NativeBuildContext context, Inno.Build.Toolchains.NativeBuildRecipe sourceRecipe, Inno.Build.Toolchains.NativeBuildProduct imGui)`](../../../backends/ImGui/build/Inno.Build.Toolchains.ImGuizmo/ImGuizmoToolchain.cs#L141) | Freezes the exact ImGui link input for both compilation and complete product cache lookup. | 遵循本页对应生命周期；借用依赖不转移其 owner。 | 参数、边界和操作状态不满足时拒绝执行；详见类型说明。 |
