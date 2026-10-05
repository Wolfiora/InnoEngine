# 平台中立的范围与新平台接入

[架构索引](README.md) · [当前平台架构](PLATFORM_RUNTIME_ARCHITECTURE.md) · [本轮工程整理验收](SOLUTION_CLEANUP_ACCEPTANCE_2026_10_05.md)

## 1. 当前能保证什么

玩法、领域契约、Scene、类型目录、共同 Player 生命周期和帧执行不选择 Windows、macOS 或浏览器。
平台宿主把服务、帧驱动、线程策略和部署目录注入共同运行流程；具体 Adapter 使用相同领域契约。

平台中立不能解释成整个仓库没有平台代码，也不能解释成新增平台无需实现或验收。
窗口、GPU surface、文件系统、原生 ABI、托管发布、SDK 与签名都有真实差异，由各自边界处理。

当前源码还有文件系统相关的 OS 判断：Core.IO 的路径比较和 Windows 文件移动重试，以及
`RuntimeContentDeployment`、`FileRenderTargetArtifactProvider` 的文件路径防越界比较。
后两者是具体的文件部署/读取实现，不是纯领域契约；因此不能声称所有 Runtime/Services 文件都没有 OS 判断。
它们没有选择另一套玩法、事件总线或渲染执行流程。

## 2. 真实例子：FlappyBird

当前 Sample 的 `Assets/FlappyBird/Gameplay/FlappyBirdController.cs` 使用同一套脚本入口：

```csharp
using InnoEngine.Input;
using InnoEngine.Storage;

bool flap = Input.WasKeyPressed(KeyCode.Space);
StorageKey bestScoreKey = new("flappy-bird/best-score.txt");
byte[]? saved = await Storage.ReadAsync(bestScoreKey);
```

这是已经启动 Session 后的调用片段。玩法不需要知道键盘事件来自桌面还是浏览器，
也不需要知道保存介质是普通磁盘还是浏览器持久存储。

| 部分 | 桌面组合 | 浏览器组合 | 共同机制 |
| --- | --- | --- | --- |
| 帧调度 | `PollingShellFrameDriver` | `ScheduledShellFrameDriver` 接收浏览器帧机会 | Shell 执行同一帧流程 |
| 持久数据 | `FileSystemStorageBackendProvider` | `BrowserStorageBackendProvider` | Storage API 与游戏数据 key |
| 输入 | 桌面 SDL3 平台实现 | 浏览器 SDL3 平台实现 | Core Events、Input Runtime 和 `Input` |
| 模块与类型 | 生成的静态目录 | 生成的静态目录 | TypeCatalog、Registry 与序列化注册 |
| 线程策略 | 宿主选择 worker/render 策略 | 宿主选择单线程及当前线程渲染 | Runtime 的显式执行策略 |

`DesktopPlayerComposition` 与 `BrowserPlayerComposition` 都通过 `PlayerLaunchOptions` 注入这些选择，
并调用 `PlayerApplication.RunAsync`。默认 Adapter catalog 可以替换某一个领域的 provider，
不要求复制 EngineHost 或 Player。
实际 Windows CoreCLR/NativeAOT、Web 解释执行/AOT 证据见[完整重构验收](PLATFORM_RUNTIME_ACCEPTANCE.md)。

## 3. 新平台例子：未来接入 iOS

以下是接入设计，不表示 iOS Player 已实现或可导出。

```text
src/composition/player/Inno.Player.IOS/
  Inno.Player.IOS.csproj
  Program.cs
  AppDelegate.cs
  IosPlayerComposition.cs
  IosFrameDriver.cs

build/pipeline/Inno.Build.Platform.IOS/
  Inno.Build.Platform.IOS.csproj
  IosArm64GameBuildTarget.cs
  IosSupportPackValidator.cs
  Packaging/
    IosApplicationPackager.cs
    IosApplicationSigner.cs

build/toolchains/Inno.Build.Toolchains.Apple/
  Inno.Build.Toolchains.Apple.csproj
  AppleToolchain.cs
  AppleSdkResolver.cs

build/managed/Inno.Build.Managed.DotNet/Apple/
  IosAotDeploymentCompiler.cs

build/support/Inno.Build.SupportPacks/
  IosPlayerSupportPackSource.cs
```

按以下顺序完成：

1. **定义目标和能力。** 使用开放的 `new BuildTargetId("ios-arm64")`；选择符合 SDK 要求的托管部署 provider。
   创建 ID 本身不会安装平台支持，设备与模拟器必须分开描述。
2. **实现原生工具链。** 解析 Apple SDK、compiler、sysroot、ABI、链接和目标产物指纹。
   复用现有 `native/Inno.Native.*` 的共同 facade 与绑定定义，按目标生成到独立 `obj`，
   不复制成 `Inno.Native.XXX.IOS`。第三方库缺失能力时，在所属 Adapter/Native 边界补齐。
3. **实现托管发布。** `IosAotDeploymentCompiler` 实现 `IManagedDeploymentCompiler`，负责冻结代码闭包的
   AOT 发布、取消、进程退休和产物校验，不负责应用包布局。复用静态类型、工厂与序列化注册，
   在构建阶段拒绝不支持的动态代码行为。
4. **实现平台包。** `IosArm64GameBuildTarget` 实现 `IGameBuildTarget` 的内容处理、打包和校验；
   packager/signer 管理资源、平台清单与签名。`IosPlayerSupportPackSource` 实现 `IPlayerSupportPackSource`，
   共同 publisher 继续拥有缓存、安装和输出保全事务。
5. **实现系统宿主。** `IosPlayerComposition` 管理启动与系统生命周期，注入帧驱动、存储目录、Adapter、
   模块目录和线程策略，再进入共同 `PlayerApplication.RunAsync`。
   可以用 `ScheduledShellFrameDriver` 包装系统帧回调；确有不同调度行为时实现 `IShellFrameDriver`。
   暂停、恢复和输入继续进入现有平台契约与 Core Events。
6. **注册组合。** Build CLI 和 Editor composition 注册 target、deployment compiler 与 Support Pack source，
   同步项目引用、Solution、Wiki 和架构分类。通用 Build Pipeline 不增加 iOS 专用分支；
   没有要求的领域不增加 Adapter。
7. **实机验收。** 分别验证设备/模拟器、触摸、焦点、后台恢复、GPU surface、音频、存储、回调释放、
   AOT/裁剪、取消和失败时保留旧输出。通过前不能标为已支持。

继续复用玩法源码、Scene/Assets、Input/Storage API、Core Events、共同 Player、TypeCatalog/Registry、
序列化、构建事务和内容闭包。若新 SDK 揭示当前契约无法表达的真实能力，仍需在对应领域完善中立契约；
现有分层不能保证未来永远不修改契约。

## 4. BGCS 的职责

BGCS 只处理 C/C++ 目标、ABI、分析、冻结 IR 和绑定输出，不处理 Player、游戏包或玩法。
生成器宿主与生成绑定的目标是两个独立选择。

当前 `AppleNativeTargetProvider` 已能区分 iOS 设备与模拟器目标。消费方提供正确 SDK 后，
请求使用 `ios-arm64-darwin`；这是目标解析能力，不能代替 iOS 实际原生调用验收。
独立说明及当前 API 示例见 BGCS 的[平台扩展说明](../../../BindGen-CS/docs/platform-extension.cn.md)。

尚未识别的新 ABI 通过 `INativeTargetProvider` 提供 triple、SDK、include 与 compiler 描述，
随后验证布局、参数、返回、回调和最终链接。现有分析与 emitter 继续消费中立目标事实；
确实不同的 ABI/interop 行为必须补充共享规则及独立回归，不能把假定的目标能力当成成功。
