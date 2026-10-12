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

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Build.Toolchains.ImGuizmo.ImGuizmoToolchain`

<!-- api: T:Inno.Build.Toolchains.ImGuizmo.ImGuizmoToolchain 15c8e72ef65d4950609548919db88ecdb1f5aaa453fc70790a228550fbf93dee -->
<!-- api: P:Inno.Build.Toolchains.ImGuizmo.ImGuizmoToolchain.componentDescriptor 8c17fa661de61bd5f047badc60fb617f7940f29cb3fb5fc7c9998b08cb1d8a58 -->
<!-- api: M:Inno.Build.Toolchains.ImGuizmo.ImGuizmoToolchain.BuildAsync(Inno.Build.Toolchains.NativeBuildContext,Inno.Build.Toolchains.NativeBuildProduct,System.Threading.CancellationToken) 1aa10b8217b67c6bc41952a81683b690bbe65588abaf18d1cd0f2bdb849c482c -->

<!-- api: M:Inno.Build.Toolchains.ImGuizmo.ImGuizmoToolchain.CreateRecipe(Inno.Build.Toolchains.NativeBuildContext) 4a65a038232a2656dd0c8443b6a0e65cf4e514023e18bfe36f615e2f6a353293 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-395f2da4230f08fb"></a>[`public static System.Threading.Tasks.Task<Inno.Build.Toolchains.NativeBuildProduct> Inno.Build.Toolchains.ImGuizmo.ImGuizmoToolchain.BuildAsync(Inno.Build.Toolchains.NativeBuildContext context, Inno.Build.Toolchains.NativeBuildProduct imGui, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../backends/ImGui/build/Inno.Build.Toolchains.ImGuizmo/ImGuizmoToolchain.cs#L49) | Builds and installs the component for the explicitly selected native target.<br>返回：The validated gizmo library linked against the explicitly supplied ImGui product.<br>`context`：The checkout and configuration whose sources and outputs belong to this operation.<br>`imGui`：The matching published ImGui product providing the native link dependency.<br>`cancellationToken`：Cancels child processes and prevents artifact installation after cancellation. | — | `System.ArgumentException`：The configuration is invalid.<br>`System.InvalidOperationException`：A native build process fails.<br>`System.OperationCanceledException`：The operation was canceled. |
| <a id="api-2d14cacb08205e7d"></a>[`public static Inno.Build.Toolchains.NativeComponentDescriptor Inno.Build.Toolchains.ImGuizmo.ImGuizmoToolchain.componentDescriptor { get; }`](../../../backends/ImGui/build/Inno.Build.Toolchains.ImGuizmo/ImGuizmoToolchain.cs#L19) | Gets the unique native and recipe owners used for sources and target-scoped intermediates. | — | — |
| <a id="api-4977392cfe1a4a7c"></a>[`public static class Inno.Build.Toolchains.ImGuizmo.ImGuizmoToolchain`](../../../backends/ImGui/build/Inno.Build.Toolchains.ImGuizmo/ImGuizmoToolchain.cs#L14) | Builds and installs this component through the shared native process lifecycle. | — | — |
| <a id="api-044245144f0cecef"></a>[`public static Inno.Build.Toolchains.NativeBuildRecipe Inno.Build.Toolchains.ImGuizmo.ImGuizmoToolchain.CreateRecipe(Inno.Build.Toolchains.NativeBuildContext context)`](../../../backends/ImGui/build/Inno.Build.Toolchains.ImGuizmo/ImGuizmoToolchain.cs#L106) | 冻结当前明确目标的源码、工具与 shared 编译参数；不要求生成输出已经存在。 | 借用 context，返回不可变声明；实际发布加入选定 binding 字节，ImGuizmo 还加入 ImGui 链接库与身份。 | 缺 SDK/options/source 或不支持的链接方式立即失败。 |

## 项目依赖

- [Inno.Build.Toolchains](../../build/Inno.Build.Toolchains.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
