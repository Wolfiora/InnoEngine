# Inno.Build.Toolchains.ImGui

[分类索引](README.md) · [Wiki 首页](../../README.md) · [统一 CLI](../../build/Inno.Build.Cli.md) · [Native](Inno.Native.ImGui.md)

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

## 源码归属

当前唯一源码 owner：`backends/ImGui/build/Inno.Build.Toolchains.ImGui/Inno.Build.Toolchains.ImGui.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 当前源码公开 API 清单

只列当前源码 public/protected 表面；内部机制不是稳定 API，参数、返回、失败及所有权以英文 XML 为准。

### `Inno.Build.Toolchains.ImGui.ImGuiToolchain`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.Toolchains.ImGui.ImGuiToolchain`](../../../backends/ImGui/build/Inno.Build.Toolchains.ImGui/ImGuiToolchain.cs#L12) | Owns this backend's native recipe while SDK selection remains with the platform provider. |
| [`static Inno.Build.Toolchains.NativeComponentDescriptor Inno.Build.Toolchains.ImGui.ImGuiToolchain.componentDescriptor`](../../../backends/ImGui/build/Inno.Build.Toolchains.ImGui/ImGuiToolchain.cs#L17) | Gets the component's sole source, binding and intermediate owner. |
| [`static System.Threading.Tasks.Task<Inno.Build.Toolchains.NativeBuildProduct> Inno.Build.Toolchains.ImGui.ImGuiToolchain.BuildAsync(Inno.Build.Toolchains.NativeBuildContext context, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../backends/ImGui/build/Inno.Build.Toolchains.ImGui/ImGuiToolchain.cs#L41) | Builds the component using the explicitly selected target SDK and validates the complete native product. |

## 项目依赖

- [Inno.Build.Toolchains](../../build/Inno.Build.Toolchains.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
