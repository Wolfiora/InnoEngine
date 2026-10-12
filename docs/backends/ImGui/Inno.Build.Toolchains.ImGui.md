# Inno.Build.Toolchains.ImGui

[分类索引](README.md) · [Wiki 首页](../../README.md) · [统一 CLI](../../build/Inno.Build.Cli.md) · [Native](Inno.Native.ImGui.md)

## 职责、依赖和公开 API

组件的目标原生构建库，没有 Program。复用 ToolchainEnvironment、现有平台 builder 和产物复制规则，将 pinned extern/Native 源构建为显式目标；CreateRecipe 提供准备前声明，实际构建入口是 `ImGuiToolchain.BuildAsync(NativeBuildContext context, CancellationToken cancellationToken = default)`。context 明确给出源码 checkout 与 debug/release 配置；取消杀死活动进程树，构建失败明确抛出。没有供外部派生者使用的 protected 扩展点；平台 builder 是内部实现。

```csharp
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Build.Toolchains.ImGui;

static Task<NativeBuildProduct> PrepareComponentAsync(
    NativeBuildContext preparedContext,
    CancellationToken cancellationToken
) {
    return ImGuiToolchain.BuildAsync(preparedContext, cancellationToken);
}
```

上述调用用于已有构建操作：`preparedContext` 必须包含冻结的工具链、该组件的构建选项及所需绑定 generation。调用方在整个编译期间保留这些输入的读取保护，并在部署或链接完成后 Dispose 返回的 `NativeBuildProduct`。正常产品构建由发行组合的 `ProductNativeBuildPlan` 统一准备这些依赖。


## 常见流程与生命周期

通常使用 `InnoEngine.Build editor` 按依赖顺序构建全部组件，再构建 Editor；Support Pack 通过独立命令准备。库不启动其他组件 Program。构建返回 `NativeBuildProduct`；产物属于 `artifacts/native/<component>/<target>/<fingerprint>`，配置、源码、编译器与 SDK 共同决定指纹。升级 Native facade 或 extern 后，先通过统一 bindings 路线生成，再重新构建相关目标。失败不表示可以部署缺失的动态库；最终 closure 必须通过平台 validator。

具体组件只读取 context.engineRoot，CMake/overlay 与产物目录不从工具程序集位置选择。取消检查发生在进程启动和产物复制前；已安装的可重建产物不作为成功发布的替代品。

## 源码归属

当前唯一源码 owner：`backends/ImGui/build/Inno.Build.Toolchains.ImGui/Inno.Build.Toolchains.ImGui.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Build.Toolchains.ImGui.ImGuiToolchain`

<!-- api: T:Inno.Build.Toolchains.ImGui.ImGuiToolchain faaf5019ee8e8ccc5d1361f6404dcc2045f282830a537e7779f4a84383c557be -->
<!-- api: P:Inno.Build.Toolchains.ImGui.ImGuiToolchain.componentDescriptor 064777d83c6313d7d53c02142a9df6c677766f3daab81c269f221703e94a6230 -->
<!-- api: M:Inno.Build.Toolchains.ImGui.ImGuiToolchain.BuildAsync(Inno.Build.Toolchains.NativeBuildContext,System.Threading.CancellationToken) e1c4c8d960588ce133709b5d0ef4cdb19fac575d99f44171772452ae66bb2d80 -->

<!-- api: M:Inno.Build.Toolchains.ImGui.ImGuiToolchain.CreateRecipe(Inno.Build.Toolchains.NativeBuildContext) 29e432c6aef29c85d53887dded8785795393c706c64e30169278258181530dab -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-c42114783caa835a"></a>[`public static System.Threading.Tasks.Task<Inno.Build.Toolchains.NativeBuildProduct> Inno.Build.Toolchains.ImGui.ImGuiToolchain.BuildAsync(Inno.Build.Toolchains.NativeBuildContext context, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../backends/ImGui/build/Inno.Build.Toolchains.ImGui/ImGuiToolchain.cs#L41) | Builds the component using the explicitly selected target SDK and validates the complete native product.<br>返回：The immutable validated product; a failed or canceled candidate is not published.<br>`context`：The operation-owned checkout, configuration, tools and target.<br>`cancellationToken`：Cancels and drains active tool processes before publication. | — | `System.InvalidOperationException`：The SDK is unassigned or native compilation or export validation fails.<br>`System.OperationCanceledException`：The operation was canceled before publication. |
| <a id="api-4ada589813d66796"></a>[`public static Inno.Build.Toolchains.NativeComponentDescriptor Inno.Build.Toolchains.ImGui.ImGuiToolchain.componentDescriptor { get; }`](../../../backends/ImGui/build/Inno.Build.Toolchains.ImGui/ImGuiToolchain.cs#L17) | Gets the component's sole source, binding and intermediate owner. | — | — |
| <a id="api-e0a9740b39ddd554"></a>[`public static class Inno.Build.Toolchains.ImGui.ImGuiToolchain`](../../../backends/ImGui/build/Inno.Build.Toolchains.ImGui/ImGuiToolchain.cs#L12) | Owns this backend's native recipe while SDK selection remains with the platform provider. | — | — |
| <a id="api-c427c58c10960b64"></a>[`public static Inno.Build.Toolchains.NativeBuildRecipe Inno.Build.Toolchains.ImGui.ImGuiToolchain.CreateRecipe(Inno.Build.Toolchains.NativeBuildContext context)`](../../../backends/ImGui/build/Inno.Build.Toolchains.ImGui/ImGuiToolchain.cs#L89) | 冻结当前明确目标的源码、工具与 shared 编译参数；不要求生成输出已经存在。 | 借用 context，返回不可变声明；实际发布加入选定 binding 字节，ImGuizmo 还加入 ImGui 链接库与身份。 | 缺 SDK/options/source 或不支持的链接方式立即失败。 |

## 项目依赖

- [Inno.Build.Toolchains](../../build/Inno.Build.Toolchains.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
