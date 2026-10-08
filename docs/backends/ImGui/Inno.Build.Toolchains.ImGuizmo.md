# Inno.Build.Toolchains.ImGuizmo

[分类索引](README.md) · [Wiki 首页](../../README.md) · [统一 CLI](../../build/Inno.Build.Cli.md) · [Native](Inno.Native.ImGuizmo.md)

## 职责、依赖和公开 API

组件的宿主原生构建库，没有 Program。复用 ToolchainEnvironment、现有平台 builder 和产物复制规则，将 pinned extern/Native 源构建为当前宿主目标。唯一公开构建入口是 `ImGuizmoToolchain.BuildAsync(NativeBuildContext context, NativeBuildProduct imGui, CancellationToken cancellationToken = default)`。context 明确给出源码 checkout 与 debug/release 配置；取消杀死活动进程树，构建失败明确抛出。没有供外部派生者使用的 protected 扩展点；平台 builder 是内部实现。

只依赖共同构建契约。调用方显式传入已发布的 ImGui product，验证 component、target 和唯一链接库；不会访问另一个工具链的中间目录。两个组件的中间态分别属于自己的 `obj/native/<target>/<fingerprint>`。

```csharp
using Inno.Build.Toolchains;
using Inno.Build.Toolchains.ImGui;
using Inno.Build.Toolchains.ImGuizmo;

var context = new NativeBuildContext(engineRoot, "release");
NativeBuildProduct imGui = await ImGuiToolchain.BuildAsync(context, cancellationToken);
NativeBuildProduct gizmo = await ImGuizmoToolchain.BuildAsync(context, imGui, cancellationToken);
```

## 常见流程与生命周期

通常使用 `Inno.Build.Cli engine` 按依赖顺序构建全部组件，再构建 Editor/Support Pack。库不启动其他组件 Program。构建返回 `NativeBuildProduct`；产物属于 `artifacts/native/<component>/<target>/<fingerprint>`，配置、源码、编译器与 SDK 共同决定指纹。升级 Native facade 或 extern 后，先通过统一 bindings 路线生成，再重新构建相关目标。失败不表示可以部署缺失的动态库；最终 closure 必须通过平台 validator。

具体组件只读取 context.engineRoot，CMake/overlay 与产物目录不从工具程序集位置选择。取消检查发生在进程启动和产物复制前；已安装的可重建产物不作为成功发布的替代品。

## 源码归属

当前唯一源码 owner：`backends/ImGui/build/Inno.Build.Toolchains.ImGuizmo/Inno.Build.Toolchains.ImGuizmo.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Build.Toolchains.ImGuizmo.ImGuizmoToolchain`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.ImGuizmo.ImGuizmoToolchain`](../../../backends/ImGui/build/Inno.Build.Toolchains.ImGuizmo/ImGuizmoToolchain.cs#L14) | Builds and installs this component through the shared native process lifecycle. |
| [`static Inno.Build.Toolchains.NativeComponentDescriptor Inno.Build.Toolchains.ImGuizmo.ImGuizmoToolchain.componentDescriptor`](../../../backends/ImGui/build/Inno.Build.Toolchains.ImGuizmo/ImGuizmoToolchain.cs#L19) | Gets the unique native and recipe owners used for sources and target-scoped intermediates. |
| [`static System.Threading.Tasks.Task<Inno.Build.Toolchains.NativeBuildProduct> Inno.Build.Toolchains.ImGuizmo.ImGuizmoToolchain.BuildAsync(Inno.Build.Toolchains.NativeBuildContext context, Inno.Build.Toolchains.NativeBuildProduct imGui, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../backends/ImGui/build/Inno.Build.Toolchains.ImGuizmo/ImGuizmoToolchain.cs#L49) | Builds and installs the component for the current native host. |

## 项目依赖

- [Inno.Build.Toolchains](../../build/Inno.Build.Toolchains.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
