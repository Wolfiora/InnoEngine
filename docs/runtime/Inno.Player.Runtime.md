# Inno.Player.Runtime

[Runtime 索引](README.md) · [Wiki 首页](../README.md) · [Shell](Inno.Shell.md) · [桌面入口](Inno.Player.md) · [Web 入口](Inno.Player.Browser.md)

## 职责与边界

这是桌面和 Web 共用的 Player 应用层。唯一运行入口是 `PlayerApplication.RunAsync`。平台入口准备内容和宿主服务，运行时模块装配、Scene、Settings、Input、Rendering、Diagnostics 与退休流程在这里完成。内部 `GamePlayerHost : Shell` 不属于公开扩展契约，不通过源码链接共享。

## 所有公开 API

| API | 稳定语义 |
| --- | --- |
| `PlayerApplication.RunAsync(options, cancellationToken)` | 启动冻结部署并运行共享生命周期；正常退出返回 0，失败和取消传播异常。 |
| `PlayerLaunchOptions.adapters` | 必填的后端目录，决定平台、输入、渲染、音频、文本、UI 和存储工厂。 |
| `modules` / `types` / `serializationMetadata` | 必填的模块、类型与序列化元数据来源，由同一 composition 选择；模块来源的所有权移交给应用。 |
| `contentDirectory` / `persistentDataRoot` | 必填的已准备内容目录和持久数据父目录；游戏相对目录来自 manifest。 |
| `moduleActivator` / `frameDriver` | 必填的程序集激活策略和宿主帧调度契约。 |
| `adapterSelection` | 后端选择，默认使用目录默认项。 |
| `jobExecutionMode` / `renderOnCallingThread` | 显式线程能力，不通过浏览器判断改变领域行为。 |
| `logDeliveryMode` / `consoleColors` | 显式日志交付与终端显示能力。 |
| `graphicsApi` / `smokeFrameLimit` | 可选图形 API 偏好与有界验证帧数。 |
| `windowVisible` | 默认显示主窗口；false 请求隐藏窗口，保留完整渲染生命周期。 |
| 主窗口暂停策略 | 可见 Player 使用 Shell 的隐藏/最小化暂停；隐藏验证窗口继续渲染。应用暂停始终经过同一 Core Events 生命周期。 |
| `IPlayerModuleActivator.Activate(modules, deployment)` | 激活经验证的逻辑代码部署，不要求 DLL 目录。 |
| `StaticPlayerModuleActivator(linkedDeployment, assemblies)` | 冻结构建时模块与实际链接程序集的对应关系；运行时核对模块顺序、domain、依赖和内容指纹，再通过同一 ModuleHost 事务激活。 |

没有供派生实现者使用的 protected 扩展点。插件通过已有 Runtime Subsystem、TypeRegistry 和 ModuleHost 契约扩展。

## 初始化和示例

先准备内容，再创建 Adapter Catalog，最后调用共享入口：

```csharp
using System.Collections.Generic;
using System.Reflection;
using Inno.Adapter.Default;
using Inno.Player.Runtime;
using Inno.Runtime;
using Inno.Shell;

await PlayerApplication.RunAsync(new PlayerLaunchOptions
{
    modules = moduleCatalogSource,
    types = typeCatalogSource,
    serializationMetadata = serializationMetadataSource,
    adapters = new DefaultAdapterCatalog(),
    contentDirectory = contentDirectory,
    persistentDataRoot = persistentDataRoot,
    moduleActivator = new StaticPlayerModuleActivator(linkedDeployment, linkedAssemblies),
    frameDriver = new PollingShellFrameDriver()
});
```

变量由调用方 composition 提供；`linkedAssemblies` 的类型是 `IReadOnlyDictionary<string, IReadOnlyList<Assembly>>`。生产 Player 的源生成器根据编译引用和构建生成的部署定义创建这些输入。执行顺序是 manifest envelope 校验与内容准备 → 引擎初始化 → 模块身份、指纹与链接闭包核对和激活 → Shell Adapter 初始化 → Session/Settings/Scene → Shell 帧循环 → 资源退休。

## 错误、生命周期和热重载

缺失内容、无效模块闭包和启动失败明确抛出异常；不会制造默认 Scene 或吞掉错误。输入仍使用同一 `IInputEventSource`，Session 拥有独立 backend。帧驱动只调度，不实现第二套游戏循环。

发行 Player 使用生成的静态目录，不携带 collectible loader 或运行时编译器。Editor 的动态来源仍独立保留 RetirementBarrier、Full GC、finalizer wait、弱 monitor 和 Faulted gate；静态部署不会改变该契约。

共享 Player 将有效 `OnSuspensionChanged` 同步发送到 Session 的 `EventDispatcher`。
Audio Runtime 通过生命周期所属的 Core Events Hub 暂停 active/retained mixer 的 master bus，恢复时保留用户原本的 bus 暂停设置；
更换设备或 mixer 仍保留宿主暂停状态。领域服务不判断浏览器或桌面平台。
