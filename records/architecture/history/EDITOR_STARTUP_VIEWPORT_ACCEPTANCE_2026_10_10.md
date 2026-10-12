# Windows Editor 启动配置、还原闭包与 Viewport 移动修复

[架构索引](../../../docs/architecture/README.md) · [产品启动](../../../docs/platform/PRODUCT_STARTUP.md) · [Task Hosting](../../../docs/build/Inno.Build.TaskHosting.md) · [ImGui SDL](../../../docs/backends/ImGui/Inno.Adapter.Presentation.ImGui.Sdl3.md)

## 用户报告与根因

| 问题 | 已确认原因 | 修复 |
| --- | --- | --- |
| Rider 启动列表包含 Player、其他系统入口与 CacheProbe | Rider 为所有 Exe 自动建立本机运行配置；不是废弃项目 | 仓库共享配置只提供 Editor、Build；本机 RunManager 清理并备份 |
| Windows Editor 关闭后再次构建要求 wasm-tools | Task bootstrap 继承 IDE 的 SolutionPath；NuGet 静态图从整个 Solution 进入 Browser Player | 工具引导和 owned project restore 排除父 Solution entry point，保持静态图与准确项目闭包 |
| Viewport 移动一顿一顿 | drawable metrics 同步回写 ImGui 的请求位置/逻辑尺寸，并制造 Move/Resize 请求 | 保留 ImGui 几何 owner；实际 SDL 事件请求协调，metrics 只同步 drawable/DPI/renderer |

第二项从 Rider `2026_10_10_06_32_04_5775` 日志确认：错误发生于
`Inno.Build.TaskHosting` 的 `Restore;PublishInnoTaskPublisherBootstrap`，不是 Editor 退出后的 Native 初始化。
仅给 TaskHosting 的 Restore 注入 `SolutionPath=InnoEngine.sln` 即能复现 NETSDK1147。
因此“第一次/第二次”的表象来自 IDE 是否执行了带 Solution metadata 的工具引导，不能把它解释为退出时没有释放窗口。
首次成功时的具体缓存和启动路径没有日志，不据此推断其内部状态。

第三项对照当前 `extern/cimgui/imgui/imgui.cpp` 的协议核对：NewFrame 只在 PlatformRequestMove/Resize 为真时从 getter 读取系统位置，
UpdatePlatformWindows 只在没有对应平台请求时应用 ImGui 的新位置/尺寸。
原有 metric refresh 在 BeginFrame 和附加 viewport 绘制/呈现期间强制回写这些字段，会与该协议相互干扰。
没有修改第三方源码、生成绑定、BGFX 生命周期或共同渲染契约。

## 环境与证据

- 引擎基线 HEAD：`8070302d0daa7ea19b2211b224c2bd6cb4dca1b9`；保留本轮之前的 CI 文档/workflow 修改。
- Windows x64；系统 CLI：`C:/Program Files/dotnet/dotnet.exe`；工程 SDK：9.0.318。
- 系统 CLI 的 workload list 为空。未安装 wasm-tools 掩盖 Windows Editor 闭包错误。
- 本轮证据：`artifacts/acceptance/editor-launch-2026-10-10/`；包含初始 diff、Rider workspace 备份、binlog、测试结果与命令日志。
- 全部修复未自动提交。

## 验证命令与当前结果

```powershell
# 修复前的最小复现：失败 NETSDK1147。
dotnet msbuild build/tasks/Inno.Build.TaskHosting/Inno.Build.TaskHosting.csproj -t:Restore -p:SolutionPath=C:/Dev/GameEngineDev/InnoEngine/InnoEngine.sln

# 修复后：未安装 Wasm workload 的系统 CLI 已通过。
dotnet msbuild platforms/Windows/editor/Inno.Editor.Windows/Inno.Editor.Windows.csproj -t:RestoreProductTargetGraph -p:SolutionPath=C:/Dev/GameEngineDev/InnoEngine/InnoEngine.sln

# 真实 SDL/ImGui 窗口与逐帧位置回归。
dotnet test tests/editor/Inno.Editor.ImGui.Tests/Inno.Editor.ImGui.Tests.csproj --filter FullyQualifiedName~ViewportMetricsTests -p:InnoTestNativeProduct=

# owned restore、继承的 Solution 属性与超长 IDE 全局属性回归。
dotnet test tests/build/Inno.Build.Tests/Inno.Build.Tests.csproj --filter 'FullyQualifiedName~OwnedRestoreTests|FullyQualifiedName~StaticGraphRestoreTests'
```

| 验证 | 结果 | 证据 |
| --- | --- | --- |
| Owned Restore 与 Static Graph | 7 通过，0 失败，0 跳过 | `results/restore-boundaries.trx` |
| SDL/ImGui Viewport metrics 与位置 | 6 通过，0 失败，0 跳过 | `results/viewport-metrics.trx` |
| 全仓架构与有效项目引用 | 229/229 项目，通过 | `logs/architecture.log`、`results/project-graph.json` |
| Solution 归属、空 Folder、配置 XML、变更文档链接 | 229 个项目、143 个非空 Folder，无游离项目；通过 | `results/delivery.json` |
| Windows Editor 普通 Debug Build 第一次 | exit 0，438.43 秒；包含重新准备 Shader 工具 | `logs/editor-build-1.log`、`.binlog` |
| Windows Editor 普通 Debug Build 第二次 | exit 0，85.11 秒；未触发 Wasm workload 要求 | `logs/editor-build-2.log`、`.binlog` |

Viewport 最终测试复用本轮已经完成的原生部署，因此最后一次命令关闭重复的 Native 准备；
此前普通测试入口已实际执行完整 Editor Native 准备，记录在 `logs/viewport-tests.log`。
两个新增位置测试分别验证：metrics 不覆盖尚未提交的请求几何，以及真实附加 SDL 窗口的 24 次位置请求。
每次位置请求后通过实际 SDL 事件和下一帧完成系统对账；不清除平台事件标志绕过 ImGui 协议。
自动请求不同于人工鼠标拖动，不据此宣称实际手感已经验收。

连续两次普通产品构建使用系统 SDK 9.0.318，保留 Rider 的 `BuildingInsideVisualStudio`、`SolutionPath`、`SolutionDir` 上下文。
没有设置跳过 Native 准备；完整执行对应产品 Native、绑定与 Shader 流程。
本轮没有测量人工拖动帧时间，也没有将第一次准备构建与第二次热构建的耗时差写成优化百分比。
原生 Shader 工具重新准备出现第三方编译警告，退出码为 0；没有修改 `extern` 压制警告。

```powershell
& 'C:/Program Files/dotnet/dotnet.exe' msbuild platforms/Windows/editor/Inno.Editor.Windows/Inno.Editor.Windows.csproj -t:Build -p:Configuration=Debug -p:SolutionPath=C:/Dev/GameEngineDev/InnoEngine/InnoEngine.sln -p:SolutionDir=C:/Dev/GameEngineDev/InnoEngine/ -p:BuildingInsideVisualStudio=true -p:BuildProjectReferences=true -m:1 -nodeReuse:false
```

执行脚本、最终计数与逐文件映射分别位于本轮证据目录的 `build-editor.ps1`、`results/summary.json`、`file-map.tsv`。
本轮创建的 Native materialization 临时目录已由 owner 自动清理；当前有效产物和验收日志保留。
结束检查 C 盘约有 152 GiB 可用，没有清理用户、NuGet 或 SDK 全局缓存。

## 所有权与界面状态

- Player.Windows/MacOS/Browser 是导出游戏的真实宿主，不是可以直接打开创作 Project 的 Editor。
- CacheProbe 是跨进程缓存测试 fixture；删除其 Exe 会破坏真实进程间验收，保留项目而清理 IDE 快捷方式。
- Build 是统一生产 CLI；共享配置默认执行 Windows Debug engine/Support Pack 流程。
- 运行配置 XML 与本机 RunManager 已整理。用户通过 Escape 停止 Computer Use 后，不再操作 Rider。
  当前窗口的配置刷新未实测；IDE 仍可能覆盖其内存中的旧 workspace，重载后需确认列表。
  新 checkout 或删除本机 `.idea` 后，Rider 可能再次自动生成 Exe 配置；在 Edit Configurations 中删除额外快捷方式，保留共享 Editor/Build。
- 位置/逻辑尺寸 owner 是 ImGui；操作系统变化通过 SDL extension 路由；像素输出与 DPI 同步属于后端 metrics。
- Native surface 与窗口退休顺序保持既有协议，不绕过等待或重复销毁。

## 未实测范围

当前轮没有继续真实鼠标拖动、任意多屏 DPI、macOS 或 Browser 的人工运行复核。
自动位置回归与构建结果只能证明对应契约，不替代人工手感和多显示器观察。
Windows 没有 `/System/Library/Sounds/Glass.aiff`，提示音未播放。
