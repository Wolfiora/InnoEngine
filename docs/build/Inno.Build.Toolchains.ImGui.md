# Inno.Build.Toolchains.ImGui

[Build 索引](README.md) · [Wiki 首页](../README.md) · [统一 CLI](Inno.Build.Cli.md) · [Native](../native/Inno.Native.ImGui.md)

## 职责、依赖和公开 API

组件的宿主原生构建库，没有 Program。复用 ToolchainEnvironment、现有平台 builder 和产物复制规则，将 pinned extern/Native 源构建为当前宿主目标。唯一公开构建入口是 `ImGuiToolchain.BuildAsync(NativeBuildContext context, CancellationToken cancellationToken = default)`。context 明确给出源码 checkout 与 debug/release 配置；取消杀死活动进程树，构建失败明确抛出。没有供外部派生者使用的 protected 扩展点；平台 builder 是内部实现。

```csharp
using Inno.Build.Toolchains;
using Inno.Build.Toolchains.ImGui;

await ImGuiToolchain.BuildAsync(new NativeBuildContext(engineRoot, "release"), cancellationToken);
```

## 常见流程与生命周期

通常使用 `Inno.Build.Cli engine` 按依赖顺序构建全部组件，再构建 Editor/Support Pack。库不启动其他组件 Program。构建返回 `NativeBuildProduct`；产物属于 `artifacts/native/<component>/<target>/<fingerprint>`，配置、源码、编译器与 SDK 共同决定指纹。升级 Native facade 或 extern 后，先通过统一 bindings 路线生成，再重新构建相关目标。失败不表示可以部署缺失的动态库；最终 closure 必须通过平台 validator。

具体组件只读取 context.engineRoot，CMake/overlay 与产物目录不从工具程序集位置选择。取消检查发生在进程启动和产物复制前；已安装的可重建产物不作为成功发布的替代品。

## 当前源码公开 API 清单

以下仅列出当前程序集自己声明的 public/protected 契约；继承成员遵循所属基类页面。internal/private 实现不作为稳定公开 API。签名依据当前源码语义模型生成，行为、参数、异常与所有权说明同时以对应英文 XML 为准。

### `Inno.Build.Toolchains.ImGui.ImGuiToolchain`

| 当前声明 | 行为 |
| --- | --- |
| [`static System.Threading.Tasks.Task<Inno.Build.Toolchains.NativeBuildProduct> Inno.Build.Toolchains.ImGui.ImGuiToolchain.BuildAsync(Inno.Build.Toolchains.NativeBuildContext context, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/toolchains/Inno.Build.Toolchains.ImGui/ImGuiToolchain.cs#L40) | Builds and installs the component for the current native host. |
| [`Inno.Build.Toolchains.ImGui.ImGuiToolchain`](../../build/toolchains/Inno.Build.Toolchains.ImGui/ImGuiToolchain.cs#L14) | Builds and installs this component through the shared native process lifecycle. |

## 项目依赖

- [Inno.Build.Toolchains](Inno.Build.Toolchains.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：项目引用；公开签名可见性由语义边界检查确认。

共同 MSBuild 注入的 analyzer 与编译规则属于构建依赖，完整有效项目图记录在本轮验收证据中。
