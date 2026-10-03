# Inno.Player.Runtime

[Runtime 索引](README.md) · [Wiki 首页](../README.md) · [Shell](Inno.Shell.md) · [桌面入口](Inno.Player.md) · [Web 入口](Inno.Player.Browser.md)

## 职责与边界

这是桌面和 Web 共用的 Player 应用层。唯一运行入口是 `PlayerApplication.RunAsync`。平台入口准备内容和宿主服务，运行时模块装配、Scene、Settings、Input、Rendering、Diagnostics 与退休流程在这里完成。内部 `GamePlayerHost : Shell` 不属于公开扩展契约，不通过源码链接共享。

## 所有公开 API

| API | 稳定语义 |
| --- | --- |
| `PlayerApplication.RunAsync(options, cancellationToken)` | 启动冻结部署并运行共享生命周期；正常退出返回 0，失败和取消传播异常。 |
| `PlayerLaunchOptions.adapters` | 必填的后端目录，决定平台、输入、渲染、音频、文本、UI 和存储工厂。 |
| `contentDirectory` / `persistentDataRoot` | 必填的已准备内容目录和持久数据父目录；游戏相对目录来自 manifest。 |
| `moduleActivator` / `frameDriver` | 必填的程序集激活策略和宿主帧调度契约。 |
| `adapterSelection` | 后端选择，默认使用目录默认项。 |
| `jobExecutionMode` / `renderOnCallingThread` | 显式线程能力，不通过浏览器判断改变领域行为。 |
| `logDeliveryMode` / `consoleColors` | 显式日志交付与终端显示能力。 |
| `graphicsApi` / `smokeFrameLimit` | 可选图形 API 偏好与有界验证帧数。 |
| `IPlayerModuleActivator.Activate(modules, deployment, managedDirectory)` | 在公共宿主已验证 DLL 闭包后激活全部声明模块。 |
| `CollectiblePlayerModuleActivator` | 使用原有 ModuleHost 候选激活与 collectible domain。 |
| `LinkedPlayerModuleActivator` | 激活已经链接进宿主的程序集；可用于任意静态链接宿主。 |

没有供派生实现者使用的 protected 扩展点。插件通过已有 Runtime Subsystem、TypeRegistry 和 ModuleHost 契约扩展。

## 初始化和示例

先准备内容，再创建 Adapter Catalog，最后调用共享入口：

```csharp
using Inno.Adapter.Default;
using Inno.Player.Runtime;
using Inno.Shell;

await PlayerApplication.RunAsync(new PlayerLaunchOptions
{
    adapters = new DefaultAdapterCatalog(),
    contentDirectory = contentDirectory,
    persistentDataRoot = persistentDataRoot,
    moduleActivator = new CollectiblePlayerModuleActivator(),
    frameDriver = new PollingShellFrameDriver()
});
```

`contentDirectory` 与 `persistentDataRoot` 是调用方已经准备好的字符串。执行顺序是 manifest envelope 校验与内容准备 → 引擎初始化 → 模块闭包校验与激活 → Shell Adapter 初始化 → Session/Settings/Scene → Shell 帧循环 → 资源退休。

## 错误、生命周期和热重载

缺失内容、无效模块闭包和启动失败明确抛出异常；不会制造默认 Scene 或吞掉错误。输入仍使用同一 `IInputEventSource`，Session 拥有独立 backend。帧驱动只调度，不实现第二套游戏循环。

collectible 代际仍由现有 RetirementBarrier、Full GC、finalizer wait 和弱 monitor 验证，超时仍失败。Linked 策略没有 collectible ALC，因此不伪造卸载或热重载成功；它不改变 Editor 的 reload 契约。
