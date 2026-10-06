# Inno.Build.SupportPacks

[分类索引](README.md) · [Wiki 首页](../README.md) · [本轮整改计划](../architecture/ARCHITECTURE_CLEANUP_PLAN_2026_10_06.md)

## 职责与边界

提供桌面与浏览器的 IPlayerSupportPackSource 实现；本项目准备构建输入，不维护内置注册名单。唯一注册属于 Inno.Build.Composition，事务、取消与原子安装属于 Inno.Build.SupportPacks.Core。本项目没有 Program。

## 准备流程与所有权

平台 source 在独立 staging 中准备 Native 闭包、运行程序集、源生成器、工程 SDK 配置和 PlayerLink 模板。桌面提供动态原生库；浏览器提供静态 archive。最终 managed compiler 按实际游戏代码闭包发布，发行目录不部署模板、SDK 或分析器。

Player 源码来自所选项目经 MSBuild 求值的 Compile items，保留相对目录或明确 Link。收集器校验源文件属于引擎、输出路径不逃逸且没有大小写折叠冲突；不维护第二份手写源码名单。新增宿主文件会自动进入输入闭包。平台 validator 同时检查必须存在的内容来源实现，缺失文件立即拒绝安装。

桌面只准备五个 runtime Native 组件，不构建 Editor ImGui/ImGuizmo。浏览器 SDK 来自目标项目选择的 workload；CMake/Ninja 由宿主工具环境明确提供。生成绑定与 archive 的 target/fingerprint 必须一致。缺工具、取消、子进程失败或不完整输出都明确失败，不回退到旧缓存。

## 使用示例

```csharp
using Inno.Build.SupportPacks;

static IPlayerSupportPackSource CreateBrowserSource()
{
    return new BrowserPlayerSupportPackSource();
}
```

内置 source、target 与 managed compiler 的共享组合见 [Build Composition](Inno.Build.Composition.md)。完整发布事务见 [Support Pack 核心](Inno.Build.SupportPacks.Core.md)。Editor、CLI 与 MSBuild 通过该共享组合供给宿主上下文；不会分别创建平台名单。

## 验证

真实 FlappyBird 导出验证 MSBuild 源码闭包能够编译并运行；BuildPipelineTests、BrowserSupportPackTests 验证缺失内容来源和不完整 Pack 明确失败。

## 当前源码公开 API 清单

以下仅列出当前程序集自己声明的 public/protected 契约；继承成员遵循所属基类页面。internal/private 实现不作为稳定公开 API。签名依据当前源码语义模型生成，行为、参数、异常与所有权说明同时以对应英文 XML 为准。

### `Inno.Build.SupportPacks.BrowserPlayerSupportPackSource`

| 当前声明 | 行为 |
| --- | --- |
| [`System.Threading.Tasks.ValueTask Inno.Build.SupportPacks.BrowserPlayerSupportPackSource.PrepareAsync(Inno.Build.SupportPacks.PlayerSupportPackBuildContext context, System.Threading.CancellationToken cancellationToken)`](../../build/support/Inno.Build.SupportPacks/BrowserPlayerSupportPackSource.cs#L36) | Prepares the target runtime and compilation inputs in isolated staging. |
| [`void Inno.Build.SupportPacks.BrowserPlayerSupportPackSource.Validate(string directory)`](../../build/support/Inno.Build.SupportPacks/BrowserPlayerSupportPackSource.cs#L69) | Validates the prepared platform inputs before atomic installation. |
| [`Inno.Build.BuildTargetId Inno.Build.SupportPacks.BrowserPlayerSupportPackSource.target`](../../build/support/Inno.Build.SupportPacks/BrowserPlayerSupportPackSource.cs#L22) | Gets the platform identity whose closure this source prepares. |
| [`Inno.Build.SupportPacks.BrowserPlayerSupportPackSource`](../../build/support/Inno.Build.SupportPacks/BrowserPlayerSupportPackSource.cs#L17) | Prepares browser runtime and native link inputs for an explicitly composed distribution. |

### `Inno.Build.SupportPacks.DesktopPlayerSupportPackSource`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Build.SupportPacks.DesktopPlayerSupportPackSource.DesktopPlayerSupportPackSource(Inno.Build.BuildTargetId target, string runtimeIdentifier, string nativePlatform, string nativeExtension, Inno.Build.IPlayerSupportPackValidator validator)`](../../build/support/Inno.Build.SupportPacks/DesktopPlayerSupportPackSource.cs#L48) | Captures the platform layout and validator without preparing any tools or files. |
| [`System.Threading.Tasks.ValueTask Inno.Build.SupportPacks.DesktopPlayerSupportPackSource.PrepareAsync(Inno.Build.SupportPacks.PlayerSupportPackBuildContext context, System.Threading.CancellationToken cancellationToken)`](../../build/support/Inno.Build.SupportPacks/DesktopPlayerSupportPackSource.cs#L84) | Prepares the target runtime and compilation inputs in isolated staging. |
| [`void Inno.Build.SupportPacks.DesktopPlayerSupportPackSource.Validate(string directory)`](../../build/support/Inno.Build.SupportPacks/DesktopPlayerSupportPackSource.cs#L126) | Validates the prepared platform inputs before atomic installation. |
| [`Inno.Build.BuildTargetId Inno.Build.SupportPacks.DesktopPlayerSupportPackSource.target`](../../build/support/Inno.Build.SupportPacks/DesktopPlayerSupportPackSource.cs#L70) | Gets the platform identity whose closure this source prepares. |
| [`Inno.Build.SupportPacks.DesktopPlayerSupportPackSource`](../../build/support/Inno.Build.SupportPacks/DesktopPlayerSupportPackSource.cs#L16) | Prepares a desktop runtime closure using an explicitly selected native host and managed target. |

## 项目依赖

- [Inno.Build.Platform.Browser](Inno.Build.Platform.Browser.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Core.IO](../core/Inno.Core.IO.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Build.Toolchains](Inno.Build.Toolchains.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Build.Toolchains.Host](Inno.Build.Toolchains.Host.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Build.Toolchains.Browser](Inno.Build.Toolchains.Browser.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Build](Inno.Build.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Build.SupportPacks.Core](Inno.Build.SupportPacks.Core.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：项目引用；公开签名可见性由语义边界检查确认。

共同 MSBuild 注入的 analyzer 与编译规则属于构建依赖，完整有效项目图记录在本轮验收证据中。
