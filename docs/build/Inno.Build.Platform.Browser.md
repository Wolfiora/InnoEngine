# Inno.Build.Platform.Browser

[Build 索引](README.md) · [Wiki 首页](../README.md) · [Inno.Build](Inno.Build.md) · [Web Player 架构](../architecture/WEB_PLAYER_ARCHITECTURE.md)

## 职责与边界

该项目提供浏览器平台的内容约束、内容编译与静态站点布局。它只依赖通用 `Inno.Build` 契约与离线 BGFX 内容编译器，不负责托管运行时发布、脚本编译、浏览器宿主运行或 Support Pack 生产。Editor 和 Build CLI 均注册 `browser-wasm` target。

## 依赖与初始化顺序

作者端的 `AssetPipeline`、`SerializationRegistry` 和 `TypeCatalog` 成功建立候选代际后，构造 `BrowserWasmGameBuildTarget`。`BuildPipeline` 先捕获通用 Project 快照，再调用 target 的 `BuildContentAsync` 生成 WebGL 2 Shader 与纹理产物，最后把验证过的浏览器 Support Pack 与通用内容包交给 `PackageAsync`。

## Public API

| API | 当前语义 |
| --- | --- |
| `BrowserWasmGameBuildTarget(AssetPipeline, SerializationRegistry, TypeCatalog)` | 绑定一组作者端服务，创建 WebGL 2 内容编译器。 |
| `id` | 固定返回 `BuildTargetId.browserWasm`。 |
| `runtimeIdentifier` / `defaultManagedDeployment` | 分别为 `browser-wasm` 与 `mono-wasm`；托管 compiler 由独立 catalog 解析，profile 可选择 AOT。 |
| `displayName` | 返回 `Web (WebGL 2)`。 |
| `isPreferredOnCurrentHost` | 返回 `false`，不替换 Windows/macOS 原生默认目标。 |
| `BuildContentAsync(GameBuildContentContext, CancellationToken)` | 在隔离 staging 中编译浏览器 Shader 与纹理产物。 |
| `PackageAsync(GameBuildPackageContext, CancellationToken)` | 将已验证托管发布的 `wwwroot`、内容包和内容目录标识组织为 `<Product>-Web`；缺少页面入口时失败。 |

没有额外的 `protected` 扩展点。`IGameBuildTarget` 的公开契约见 [Inno.Build](Inno.Build.md)。

## 工作流与错误边界

下例展示组合入口的调用风格；`assets`、`serialization`、`types` 由已有作者端 composition root 提供。

```csharp
using Inno.Build;
using Inno.Build.Platform.Browser;

IGameBuildTarget target = new BrowserWasmGameBuildTarget(
    assets,
    serialization,
    types);
```

target 不持有运行时脚本实例，也不运行 dotnet 子进程。通用 pipeline 将冻结代码输入交给 [托管 compiler](Inno.Build.Managed.DotNet.md)，验证其产物后再调用平台打包。最终站点只包含浏览器运行时、Webcil、Content Pack 和页面文件。取消由公共 process runner 终止发布子进程，再由 BuildPipeline 清理 staging；完整产物才可提交。浏览器 Player 的 Missing 处理归 [Runtime](../runtime/README.md) 所有。

## 相邻页面

[Windows target](Inno.Build.Platform.Windows.md) · [macOS target](Inno.Build.Platform.MacOS.md) · [BGFX 工具链](Inno.Build.Toolchains.Bgfx.Tools.md)

## 发布前校验

公开 `BrowserSupportPackValidator` 实现 IPlayerSupportPackValidator，Validate(directory) 验证所需平台文件；对应 GameBuildTarget.Validate 调用同一规则，源码 Support Pack publisher 也复用它。缺失文件/跨平台 Native 二进制明确失败，没有重复平台闭包规则。
