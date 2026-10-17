# 集成整改剩余实机验收

[本轮报告](BACKEND_PLATFORM_INTEGRATION_ACCEPTANCE.md) · [批准计划](BACKEND_PLATFORM_INTEGRATION_PLAN.md)

## 已完成与后续边界

源码整改、当前契约门禁、Debug/Release 有效引用图、四条无人值守发布/运行、隐藏 Editor 与消费者和三次热构建已完成，精确结果以本轮报告为准。当前 HEAD 包含用户已提交的集成整改；本次新增 Task bootstrap 协调修复、Canvas 测试迁移及文档更正尚未自动提交。

当前用户禁止 Computer Use，本轮没有恢复桌面操作。剩余项目不是重新运行架构迁移或完整 SDK 冷构建：

1. 用户允许空闲桌面操作后，分别启动 Debug/Release Windows Editor，使用 Samples/FlappyBird，验证 Play/Stop、浮动 GameView 焦点/失焦释放、前景 Popup/Modal 吸收输入、重叠窗口和退出。
2. 验证 Export 设置/进度/完成自动关闭、FileBrowser/Selector 的单一滚动 owner、长 label/小窗口、ShaderEditor 小地图。
3. 使用真实多屏高 DPI 验证附加 viewport、移动、最小化/恢复和 live-resize；已有原生尺度测试不能替代设备组合。
4. Windows CoreCLR/NativeAOT Player 补人工玩法、听音与图像对照；Web 的两条 headless 路径已有昼夜/星光/移动光/输入/存档/非零样本，其他硬件和浏览器另验。
5. macOS Native/Editor/Player 在 macOS 环境实测。Linux 只验证已实现的工具能力，不宣称完整 Player；WindowsX86、iOS、NS 没有实现/注册，接入步骤见扩展指南。

## Windows 启动入口

```powershell
& 'C:/Users/23842/AppData/Local/InnoWebDotnet/dotnet.exe' run --no-build --project platforms/Windows/editor/Inno.Editor.Windows/Inno.Editor.Windows.csproj -- 'C:/Dev/GameEngineDev/InnoEngine.Samples/FlappyBird'
& 'C:/Users/23842/AppData/Local/InnoWebDotnet/dotnet.exe' run --no-build --configuration Release --project platforms/Windows/editor/Inno.Editor.Windows/Inno.Editor.Windows.csproj -- 'C:/Dev/GameEngineDev/InnoEngine.Samples/FlappyBird'
```

这两条命令会打开真实 Editor，当前不要自动执行。Player 使用本轮 products/windows-coreclr 或 products/windows-nativeaot 中发布后的 FlappyBird.exe，不把未附带部署清单和游戏代码的基础 Player 项目当作已导出的游戏。Solution 的产品 opt-in 配置不参与默认批量 Build，`not built` 不代表项目被废弃。

## 可追溯与缓存

证据目录为 `artifacts/acceptance/backend-platform-integration`；当前 source hashes、精确命令、TRX、产品日志、headless 图片和热构建观测均保留。不要重执行 phase/edit/finalize-handoff 旧迁移脚本，不用历史截图替代当前交互。

继续前核对当前源码 revision/工作区；修改了被验证的源文件时重新运行对应 gate，不盲目使用脚本的成功跳过记录。Native 输入冻结后不能一边编译一边修改 recipe/配置。

没有创建周期自动化、外部通知或自动提交。只清理明确归属且无活跃进程的任务中间缓存；保留当前 Native 热缓存、SDK、用户文件、最终产物和验收证据。指定 Glass.aiff 在 Windows 不存在，提示音未播放。
