# Inno.Build.Toolchains.Host

[Build 索引](README.md) · [Wiki 首页](../README.md) · [共同契约](Inno.Build.Toolchains.md) · [Support Pack](Inno.Build.SupportPacks.md)

## 职责与边界

组合内置宿主 Native 工具链的顺序与闭包，不包含组件编译细节、平台业务或 Program。
实现依赖 BGFX、SDL3、MiniAudio、Text、UI、ImGui 和 ImGuizmo 工具链；
公开依赖只有 `Inno.Build.Toolchains` 的 `NativeBuildContext`。
不直接引用 Native binding，不进入 Runtime、Shell 或 Player。

## 全部公开 API

| API | 稳定语义 |
| --- | --- |
| `HostNativeBuild.BuildRuntimeAsync(context, cancellationToken = default)` | 顺序准备五个运行时组件，返回只读 `IReadOnlyList<NativeBuildProduct>`。 |
| `HostNativeBuild.BuildEditorAsync(context, cancellationToken = default)` | 先准备运行时闭包，再构建 BGFX authoring tools、ImGui 和 ImGuizmo，返回八个明确的 product。 |

没有 protected 扩展点；平台选择仍属于每个具体组件工具链。
同一个不可变 context 贯穿所有阶段；配置不会由执行程序集的 Debug/Release 推导。

## 初始化与工作流

```csharp
using Inno.Build.Toolchains;
using Inno.Build.Toolchains.Host;

var products = await HostNativeBuild.BuildEditorAsync(
    new NativeBuildContext(engineRoot, "debug"),
    cancellationToken);
await HostNativeDeployment.InstallAsync(products, applicationOutput, cancellationToken);
```

Editor 普通 Build 与 Publish 的薄 MSBuild Task 调用 Editor 组合；CLI `engine` 复用普通 Build。桌面 Support Pack source 调用 Runtime 组合并显式选择 `release`。
因此 Debug Editor 与独立 Pack 发布都可以从缺少 Release 输入的当前源码工作区开始。
新增组件仍在所属组件库实现；此处只维护内置组合的依赖顺序。

## 失败与生命周期

null context、缺少源码/工具、子进程失败或产物缺失明确失败。
取消在首个阶段前检查，并传递到各组件进程树；取消后不启动下一组件。
此库不会回滚已经成功安装的可重建组件；最终 Support Pack 仍由原子 staging 事务发布。
构建契约不参与 collectible Runtime generation，不保存插件 Type、实例或回调。

## 显式部署

`HostNativeDeployment.InstallAsync(products, applicationDirectory, cancellationToken = default)`
只消费传入 product，核对目标、组件唯一性及每个 product 的完整内容清单。
通过 Core.IO FileLease 排序同一输出的写入者，以 staging 准备完整 `native` 树，再原子安装。
部署前校验全部 product，并比较现有部署的完整文件集合、SHA-256 内容与 Unix 权限；一致时再次确认 product 完整性后返回，保留文件及时间戳。缺失、多余或内容不同的文件都触发完整替换，不能仅凭名称或时间戳判定可复用。
runtime 文件按 `native/<component>/<target>` 布局；BGFX 工具和 include 按现有离线工具布局部署。
ImGui 的 import library 只服务构建链接，不进入应用。
取消、缺失文件、内容变更或复制失败保留先前完整 native 树；不会扫描历史 artifact 选择“最新”文件。

## 当前源码公开 API 清单

以下仅列出当前程序集自己声明的 public/protected 契约；继承成员遵循所属基类页面。internal/private 实现不作为稳定公开 API。签名依据当前源码语义模型生成，行为、参数、异常与所有权说明同时以对应英文 XML 为准。

### `Inno.Build.Toolchains.Host.HostNativeBuild`

| 当前声明 | 行为 |
| --- | --- |
| [`static System.Threading.Tasks.Task<System.Collections.Generic.IReadOnlyList<Inno.Build.Toolchains.NativeBuildProduct>> Inno.Build.Toolchains.Host.HostNativeBuild.BuildEditorAsync(Inno.Build.Toolchains.NativeBuildContext context, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/toolchains/Inno.Build.Toolchains.Host/HostNativeBuild.cs#L86) | Prepares the runtime closure, offline graphics tools and Editor presentation components. |
| [`static System.Threading.Tasks.Task<System.Collections.Generic.IReadOnlyList<Inno.Build.Toolchains.NativeBuildProduct>> Inno.Build.Toolchains.Host.HostNativeBuild.BuildRuntimeAsync(Inno.Build.Toolchains.NativeBuildContext context, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/toolchains/Inno.Build.Toolchains.Host/HostNativeBuild.cs#L44) | Prepares every native component required by the desktop Player. |
| [`Inno.Build.Toolchains.Host.HostNativeBuild`](../../build/toolchains/Inno.Build.Toolchains.Host/HostNativeBuild.cs#L21) | Composes the built-in native toolchains using one explicit checkout and cancellation lifetime. |

### `Inno.Build.Toolchains.Host.HostNativeDeployment`

| 当前声明 | 行为 |
| --- | --- |
| [`static System.Threading.Tasks.ValueTask Inno.Build.Toolchains.Host.HostNativeDeployment.InstallAsync(System.Collections.Generic.IReadOnlyList<Inno.Build.Toolchains.NativeBuildProduct> products, string applicationDirectory, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../build/toolchains/Inno.Build.Toolchains.Host/HostNativeDeployment.cs#L43) | Validates the application's native tree and atomically replaces it when its exact contents differ. |
| [`Inno.Build.Toolchains.Host.HostNativeDeployment`](../../build/toolchains/Inno.Build.Toolchains.Host/HostNativeDeployment.cs#L15) | Materializes an explicitly built host-native closure into one application's deployment directory. |

## 项目依赖

- [Inno.Core.IO](../core/Inno.Core.IO.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Build.Toolchains.Bgfx](Inno.Build.Toolchains.Bgfx.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Build.Toolchains.Sdl3](Inno.Build.Toolchains.Sdl3.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Build.Toolchains.MiniAudio](Inno.Build.Toolchains.MiniAudio.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Build.Toolchains.Text](Inno.Build.Toolchains.Text.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Build.Toolchains.UI](Inno.Build.Toolchains.UI.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Build.Toolchains.ImGui](Inno.Build.Toolchains.ImGui.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Build.Toolchains.ImGuizmo](Inno.Build.Toolchains.ImGuizmo.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Build.Toolchains](Inno.Build.Toolchains.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：项目引用；公开签名可见性由语义边界检查确认。

共同 MSBuild 注入的 analyzer 与编译规则属于构建依赖，完整有效项目图记录在本轮验收证据中。
