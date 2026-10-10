# 平台边界收口、静态图还原与补充验收

[架构索引](README.md) · [本轮计划](PLATFORM_BOUNDARY_CLOSEOUT_PLAN_2026_10_08.md) · [发现记录](PLATFORM_OWNERSHIP_PLAN_AUDIT_2026_10_08.md) · [平台扩展](PLATFORM_EXTENSION_GUIDE.md) · [产品启动](../platform/PRODUCT_STARTUP.md)

## 当前执行状态

本轮两项平台边界收口、静态图还原修复、当前消费者及 Windows 可执行的受影响门禁均已完成。
四条最终 Player 发布/运行成功；411 项受影响测试通过，16 项 macOS Metal 测试按环境跳过。
没有把当前 Windows 结果声明为 macOS/Linux/iOS/NS 实机验收；具体限制见本文最后两节。
证据根：`artifacts/acceptance/2026-10-08-static-graph-restore`。
基线 HEAD：`818163b5222632a6bc2cbe92804b0c6b62b8921d`；验收包含当前未提交源码，没有自动暂存或提交。
当前 Windows 宿主使用 .NET SDK 9.0.318，Web SDK 位置由启动组合显式指定。

## 用户问题与源码结果

| 用户问题 | 当前解决方式 |
| --- | --- |
| 两个需要收口的边界如何解耦 | BGFX 接收平台配置；Support Pack 先只读预检、再执行冻结计划、最后原子发布。 |
| 新平台是否还要改共享 BGFX 名单 | 删除命名平台 builder、封闭 Shader target enum 和三个固定内容 compiler 工厂；新增配置从贡献边界进入。 |
| NuGet 静态图还原为何进程启动失败 | 巨大全局属性进入 Windows 命令行触发启动长度限制；使用 NuGet 的标准输入序列化传输，保留静态图和完整属性。 |
| Plan 与 Solution 是否真的干净 | 重新核对当前有效引用图、项目/Folder owner、生产入口、旧 API 与文档；不把历史结果作为本轮修复证据。 |
| 中断后是否重复工作或遗失进度 | 每个独立 gate 保存命令、退出码、耗时和日志；仅恢复本次冻结源码运行中已成功的阶段。 |
| C 盘缓存清理 | 核对本轮 owner、活跃进程、绝对路径和链接；测量私有 bin/obj 的删除被自动审批拒绝，实际保留约 24 MB 并记录路径，不宣称已经释放。 |

本次采用公开边界测试和实际发布消费验证，没有为了测试扩大 internal API，也没有给共同领域增加平台判断。
新平台需要在平台包贡献实际 SDK invocation、Shader profile、Support Pack 计划和产品组合；
共同 backend 不再为已有能力增加命名平台选择项。新的第三方能力、ABI 或 SDK 仍需对应实现与设备验收。

连接的 `remote compact task / stream disconnected` 报错来自远端会话传输。
本次没有该服务的服务器诊断权限，不能宣称修复服务故障；源码与验收均落盘，不依赖连续的聊天连接保存状态。

## 最终边界

```text
平台构建模块
├─ Native SDK / GENie invocation / 输出 token
├─ Shader target / 方言 / 能力 / renderer 闭包
└─ Support Pack source → 冻结 plan
          ↓
标准发行：绑定同一平台 contribution 与 backend 配置
          ↓
共同 Build / Toolchains / SupportPacks.Core
├─ 完整指纹、只读预检顺序、取消和 lease
├─ 共享 backend recipe 与实际工具执行
└─ 验证、原子发布与失败清理

领域 Runtime / Rendering Core / Input / Player Runtime
└─ 不引用上述平台配置或 SDK
```

中立 `BuildDistribution` 仍不引用 BGFX；具体 `StandardBuildDistribution` 持有其 `.build` 和 Shader 配置。
具体发行在一个绑定列表中注册平台 contribution 与 Shader profile，CLI、Task 和平台产品消费者解析同一结果。
Linux 当前仅贡献已有离线工具能力，不增加完整游戏发布目标。

### 必要 API 与所有权

| API | 必要性与稳定语义 |
| --- | --- |
| `BgfxNativeBuildProfile` | 平台提供目标、GENie 参数和 SDK invocation；共同执行器不按 Inno target 创建实现。 |
| `BgfxBuildInvocation` | 构造时复制参数，固定可执行工具及有序 arguments。 |
| `BgfxShaderCompilerProfile` | 冻结方言、defines 与完整能力事实，全部参与确定性 key。 |
| `BgfxShaderTargetProfile` | 开放 target ID 对应不可变 renderer 配置，不以封闭平台枚举选择。 |
| `BgfxGameContentCompiler` 构造入口 | 接收实际 renderer 闭包，拒绝配置缺失。 |
| `PlayerSupportPackPlanningContext` | 只读预检输入，不提供 output/staging。 |
| `IPlayerSupportPackSource.CreatePlanAsync` | 在输出创建前解析真实 SDK、项目与模板，并返回固定目标计划。 |
| `PlayerSupportPackPlan.PrepareAsync` | 接收 publisher 的 owned staging，使用预检时冻结的 managed CLI/Native selection。 |
| `NativeToolchainContribution.nativeProducts` | 明确贡献实际产品闭包，Linux 不隐式获取 Editor 或 Player。 |

Catalog/config/profile 为借用的不可变描述；publisher 拥有发布 lease 和候选目录；
各 executor 拥有进程与临时输入。计划不持有正在执行的进程或设备。
预取消、SDK 缺失、错误宿主、计划目标错误不会创建输出；执行失败/取消仍保持旧完整产物。
静态组件只有真实 `staticBuild` recipe，没有不会执行的独立 producer；独立执行器明确拒绝聚合专属步骤。

### 删除与保留

- 删除 `BgfxBuilderFactory`、`BgfxBuilder`、Windows/macOS/Linux 命名 builder、`BgfxShaderTargetPlatform`、`BgfxRendererProfileCatalog`。
- 删除 `CreateWindowsX64/CreateMacOSArm64/CreateBrowserWasm` 内容 compiler 工厂、旧 `CompileShaderTask.Platform` 和 `*ShaderPlatform` 属性。
- 不保留旧 overload、alias 或 wrapper；同步平台模块、产品 props、CLI、Task、Editor、模板和 Wiki。
- BGCS、生成绑定与 `extern` 没有为本次收口增加平台代码；BGCS 继续作为独立工具被消费。
- 未增加生产项目或 `Program`；验收目录的 measurement driver 仅为通过真实公开构建边界采集数据。

## 还原故障证据

`before.json`：65,536 字符全局属性经旧命令行传递，exit 1，复现启动错误。
`after.json`：同一真实 Windows Editor 静态图还原入口，启用 `RestoreSerializeGlobalProperties` 后 exit 0。
根 `Directory.Build.props` 和 `Directory.Solution.props` 使用 NuGet 支持的传输选项，未关闭静态图，未截断或丢弃属性。
`StaticGraphRestoreTests` 在真实产品还原入口验证该行为。

## 测试、运行与性能

### 受影响的自动契约

本轮六个项目合计 **411 passed、0 failed、16 macOS Metal skipped**：

| 项目 | Passed | Skipped |
| --- | ---: | ---: |
| BGFX Adapter | 38 | 0 |
| Build | 140 | 0 |
| Architecture | 82 | 0 |
| Editor Hosting | 6 | 0 |
| Rendering Assets Authoring | 33 | 0 |
| Rendering Shaders | 112 | 16 |

`results/final-test-counts.json` 汇总最终 TRX；28 项定向契约包含在 Build 的 140 项中，不重复计数。
成功、SDK 缺失、预取消、错误目标、冻结 SDK、输出保全和平台配置指纹均有公开边界回归。
测试迁移中发现并修复三个消费引用问题：Hosting fixture 改用发行解析 Shader target；两个跨平台 Shader 测试显式引用其真实 Windows/macOS 配置 owner。
未把平台引用加入共享 Hosting、领域或 backend。

### 当前可见 Editor 验收

- Debug 普通产品 Build 成功；FlappyBird 启动，未再出现 Text export 缺失。
- Hierarchy 左键选择、展开与 Inspector 对应通过；Context Menu 搜索宽度正确。
- Export 表单为 2:3，目标列表向下展开，三个条目没有多余滚动条。
- Windows 导出成功与 macOS 在 Windows 宿主明确失败后，设置和进度 Modal 均自动关闭。
- 导出期间点击底层 File Browser 的关闭位置，没有关闭底层 Panel。
- DefaultSprite Shader 图右上角小地图显示节点与视口；点击小地图后画布定位到对应节点区域。
- 浮动 GameView 聚焦后 Space 进入玩法；焦点转回 Editor 后 Space 不重启游戏。
- Play 停止后正常退出，先保存项目 Editor 状态，再完成 Dispose；进程退出码为 0。

观察记录：`results/ui-observations.json`；启动、导出和退出：`editor-visible.log`。
本轮使用 Computer Use 实际操作，未把源码推断写成可见验收。

Release 普通产品 Build 也已通过，托管摘要为 0 warning、0 error，耗时 2599.938 秒。
其实际 FlappyBird 120 帧运行退出码为 0，正常保存状态并完成 Dispose，证据为
`logs/editor-release-build.log` 与 `logs/editor-release-smoke.log`。
原生第三方编译自身产生 warning；上述零 warning 只指最终托管 Build 摘要。

Rendering2D 首次使用本次 runner 的 600 帧设置时，只运行约 5 秒就退出，
后续严格脚本构建因 IDE 项目尚未生成而失败，原记录为 `rendering2d-editor-600frames`。
源码检查确认初始编译立即启动且不要求焦点；同一项目在后台 6000 帧调查运行中，
两个脚本项目于启动后约 24 秒生成，正常退出。因此本次修正的是验收运行预算，
没有修改 Editor 的焦点策略或添加测试专用接口，也没有移除生成脚本实际编译门禁。
调查证据：`logs/rendering2d-investigation.log`。完整 6000 帧 gate 已通过，退出码为 0，
耗时 246.297 秒；普通 Debug Build、真实 D3D11 运行、生成的运行/Editor 脚本实际编译均成功。
该次 Native 准备的生产工具进程数为 0。完整日志为 `logs/rendering2d-editor-6000frames.log`。

### 原生闭包测量

通过发行公开边界构建 Windows Editor 的八个组件；配置为 Debug：

| 次数 | 总耗时（秒） | 哈希文件数 | 哈希字节数 | Native 工具进程 | 部署文件变化 |
| --- | ---: | ---: | ---: | ---: | ---: |
| 首次当前配置 | 1030.775 | 262867 | 26066536047 | 16 | 15 |
| 热构建 1 | 586.768 | 139951 | 13948093738 | 0 | 0 |
| 热构建 2 | 118.630 | 139951 | 13948093738 | 0 | 0 |
| 热构建 3 | 98.401 | 139951 | 13948093738 | 0 | 0 |

证据：`results/native-measurements/native-hot-measurements.json`。
首次包含本次配置的缓存未命中，不是清空全机器缓存后的受控冷构建。
热构建 1 与其他产品冷构建同时执行，不能用它推导单任务性能退化；没有相同负载下的前后测量，不宣称固定提速比例。
三次均没有重新生成原生产物或替换已部署文件；完整哈希读取仍约 13.95 GB，不能称为零 IO 或廉价热构建。

### 结构与有效引用

最终静态归属检查为 217 项目、138 个有效 Solution Folder、1881 份手写源码、1549 项显式项目引用；
没有空 Folder、游离项目、无 owner 源码或迁移遗留空目录。
217 项当前 MSBuild 有效引用图与仓库架构验证均通过。
六个生产可执行入口符合白名单；五个平台产品采用显式构建，Solution 的 `not built` 不表示废弃。
实际入口与启动方式见[产品启动指南](../platform/PRODUCT_STARTUP.md)。

### 最终发布与运行矩阵

以下全部使用 Samples 的 FlappyBird、统一 Build CLI 和实际输出产品，exit code 均为 0：

| 路径 | 发布耗时（秒） | 运行验收耗时（秒） | 实际结果 |
| --- | ---: | ---: | --- |
| WindowsX64 CoreCLR | 317.171 | 4.172 | 隐藏窗口运行 120 帧，2 views / 25 draws，正常退出。 |
| WindowsX64 NativeAOT | 235.657 | 1.562 | 实际原生可执行文件运行 120 帧，2 views / 25 draws，正常退出。 |
| Browser Wasm 解释执行 | 1818.547 | 46.921 | 最终原生链接/发布成功，11 项实际浏览器行为检查通过。 |
| Browser Wasm AOT | 808.641 | 29.922 | AOT 与最终链接/发布成功，同样 11 项实际浏览器行为检查通过。 |

Web 两条路径均验证：日间画面、指针/键盘昼夜切换、夜间星光、点击开始、按键飞行与得分、
光照随鸟在屏幕空间同向移动、非零音频采样、最高分写入及页面重新加载后回读。
星光检测分别为 134 / 107 个亮像素；光照最大偏移分别为 9.173 / 11.685 像素；
音频采样峰值为 0.676 / 1.078。两个独立浏览器 profile 的得分均写入后成功回读。
这些是检测场景的实际测量，不是所有画面、GPU 或音量范围的性能/质量保证。

执行器使用 owned headless Edge/WebGL 2/SwiftShader 和静音输出；本次没有操作用户正在使用的浏览器。
游戏运行异常 `errors` 均为空；浏览器控制台仍有 316 / 315 条第三方/软件驱动 warning：
BGFX/WebGL 格式能力探测、MiniAudio 的 ScriptProcessorNode 和 SwiftShader 队列诊断。
每条路径还有两次 `/favicon.ico` 404 及对应 console error。没有隐藏或把这些记成零诊断，
也没有修改 `extern` 或在领域层拦截诊断来修补第三方行为。详细分类保留在汇总与原报告。

完整参数、cwd、耗时和失败历史：`results/closeout-executions.json`；
最终矩阵：`results/final-product-matrix.json`；运行图像、采样与原始诊断：`captures/web-interpreted`、`captures/web-aot`。
产品位于 `products/windows-coreclr`、`products/windows-nativeaot`、`products/web-interpreted`、`products/web-aot`。
本轮验收代码身份为 `results/frozen-source.json`，2207 个手写源码/配置输入的 SHA-256 在最终复核仍一致。

此前完整矩阵及独立 BGCS/Canvas 证据保留在[平台归属验收](PLATFORM_OWNERSHIP_REFACTOR_ACCEPTANCE.md)；
本次没有重跑无源码变化的全部 BGCS 独立矩阵，也不把历史 1752 项测试加进本轮 411 项汇总。

### 缓存清理权限

本轮已完成测量的私有 `native-verification/bin` 为 24,191,874 字节，`obj` 为 694,687 字节。
检查确认都在本 checkout 的验收目录内、没有 reparse point、没有活跃测量进程。
自动审批对验证后的动态命令及明确绝对路径的递归删除均返回 `blocked by policy`，没有给出进一步原因。
没有绕过该拒绝，也没有删除源码、SDK、NuGet 全局缓存或当前构建产物。
详情见 `results/measurement-cache-cleanup.json`。先前报告的 66.1 GiB 清理是历史结果，不能作为本轮释放量。

最终磁盘可用空间为 **95.555 GiB**。本次两份浏览器 profile、两份 Rendering2D 临时 Project
及上述测量 bin/obj 合计逻辑大小约 **175.7 MiB**，均已记录在 `results/final-environment.json`。
这些目录未删除：测量删除已被自动审批拒绝，未通过换工具规避；另外几项保留并列出，
没有把“未再次尝试”写成它们各自也收到了审批拒绝。日志、运行图像、当前发布产品和 SDK 保留。
该统计是逻辑文件大小，不是实际释放量。

最终 `git diff --check` exit 0；242 个引擎页面、54 个 BGCS 页面及三个消费者文档的本地链接检查均通过。
独立 BGCS 与 Samples 工作区 clean；Canvas 两项、Rendering2D 三项既有消费调整及引擎 825 个 Git 状态条目仍待人工审阅提交。
Git 条目包含完整目录迁移，不能把 deleted/新增的移动对自动解释成源码丢失。

## 实测边界

- macOS、Linux SDK 的本机编译/运行以及未来 iOS/NS 未在当前 Windows 环境实测。
- fixture target 验证配置入口开放，不等于虚构硬件已经编译或运行。
- 第三方 C++ 编译 warning 与托管编译 summary 分别报告；不修改 `extern` 消除或隐藏诊断。
- 图像、音频波形和真实玩法检查不等于任意多屏 DPI 或声学听音验收。
- 本轮 Debug FlappyBird 的实际 UI 操作已补齐 Hierarchy、Export、Modal、GameView 和小地图。
  Rendering2D 的本轮证据是实际 D3D11 Editor 6000 帧和脚本消费；没有把后台日志写成其 Scene 画面的人工视觉复核。
  后续 Computer Use 收到物理 Escape 停止信号后已停止界面输入。
- 当前门禁没有发现两项整改的新增提交阻断；这不等于所有第三方诊断已消失或未来平台已受支持。
- `/System/Library/Sounds/Glass.aiff` 在当前 Windows 环境不可用，完成提示音未播放。

## 建议提交标题

- `refactor(platform): externalize backend profiles and preflight support packs`
- `fix(build): serialize static restore properties over stdin`

本轮没有自动提交。
