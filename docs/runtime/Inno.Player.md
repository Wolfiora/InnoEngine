# Inno.Player

桌面入口支持 `--window-visible false`，通过共同 `PlatformWindowOptions.visible` 请求隐藏窗口。与 `--smoke-frames` 配合可在后台运行实际渲染验证，无需更改游戏代码或使用替代图形实现。

[Runtime 索引](README.md) · [Shell](Inno.Shell.md) · [Build](../build/README.md)

`Inno.Player` 是标准游戏产品的 Composition Root，不提供脚本稳定 API。共享项目 [Inno.Player.Runtime](Inno.Player.Runtime.md) 的内部 `GamePlayerHost : Shell` 负责读取已冻结 runtime manifest、创建 EngineHost/RuntimeSession、装配中立 Runtime Subsystems 并加载 startup Scene。

Player 通过 `DefaultAdapterCatalog` 与 `AdapterSelection` 选择默认后端，Session 同时装配 Text 与 UI。Host 源码和公开/protected surface 不包含 SDL3、BGFX、MiniAudio、FreeType、HarfBuzz、RmlUi、FileSystem 或 ImGui implementation 类型。

Player closure 必须保持 source-free：不得包含 Editor、Build、AssetPipeline、Scripting Compiler/Reload、动态模块 Adapter、Plugin Authoring 或 Toolchain。Support Pack 提供生成绑定、原生 Release 库和链接模板；托管 compiler 按当前游戏闭包发布，平台 packager 组织最终布局。

Player 先从 manifest envelope 读取并验证持久数据相对子目录，再在系统 Local Application Data 下创建游戏自己的根目录；默认子目录是 Application ID，不附加引擎品牌名。Build Settings 或单次导出可以改为 `publisher/game` 等可移植目录。`Storage`、`Logs`、运行时内容缓存和元数据都在该根目录下。桌面可选择 CoreCLR 单文件或 NativeAOT 部署；游戏程序集在发布时进入静态目录，不在运行时从 DLL 目录动态探测。

项目 Settings 在 RuntimeSession 创建 AssetDatabase 后、领域子系统构造前初始化，并使用该数据库的
完整 Asset resolver context。配置中的冷资产引用因此落在正确的 Session identity domain；不再先创建
没有 resolver 的 Settings。默认 Audio 配置通过定义明确的 Settings 契约读取，不使用缺失时静默 new 默认值的旁路。

Player 的单一 Project Settings 执行作用域覆盖完整 Shell 循环，而不只覆盖 `session.Tick()`。渲染 Request Provider 和 Pipeline 在 Tick 之后执行，仍必须解析到同一设置 owner；此职责属于产品宿主，不在 Rendering Core 或具体插件中增加作用域补丁。

`--smoke-frames` 使用 Shell 的有界 run loop，并输出最终 rendering statistics；如果 DiagnosticHub 仍有活动错误则返回失败，不把空提交当作成功。普通启动运行到 application 或 primary window 请求退出。Player 接入既有 `ConsoleLogSink`，领域和渲染诊断不会在无 Editor Console 时被丢弃。

`Program` 只委托 `DesktopPlayerComposition`。后者选择内容位置、系统持久数据父目录、生成的静态 metadata/activator 和 PollingShellFrameDriver，然后调用 PlayerApplication.RunAsync。本项目没有供外部调用的 public/protected API。
