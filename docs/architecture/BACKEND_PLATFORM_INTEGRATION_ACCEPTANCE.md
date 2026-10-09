# 平台、后端与集成边界整改验收

[架构索引](README.md) · [批准计划](BACKEND_PLATFORM_INTEGRATION_PLAN.md) · [剩余实机验收](BACKEND_PLATFORM_INTEGRATION_HANDOFF.md)

## 当前结论

**源码边界整改及本机无人值守构建、测试、发布和运行矩阵完成。** Windows Editor 的真实焦点/浮动窗口、多屏 DPI 与 macOS 实机仍未完成本轮验收；整体不作“全部实机无保留通过”声明。用户最新禁止 Computer Use，本轮没有恢复桌面自动操作。

当前 HEAD 为 `fc6a1f5117b0ff61ee87ea2fecafaadc87a96468`，集成整改已经由用户提交；本次续跑保留其提交，并新增下述 Task 并发修复、Canvas 测试迁移和文档更正，未自动提交。BGCS、extern 和生成绑定没有本轮手工修改。

## 用户提出的问题与对应结果

| 用户问题 | 当前解决结果 |
| --- | --- |
| BgfxBuildProfile 放在平台中是否让平台和 backend 耦合 | 四个真实 BGFX integration 程序集拥有 Native/Shader profile 与 compiler 工厂。Windows/macOS/Browser/Linux 基础构建模块不引用 backend；集成依赖对应平台与 backend，Standard Distribution 唯一绑定。 |
| SDL、MiniAudio 等是否也有同类问题 | Windows/macOS/Browser SDL integration 提供初始化、窗口、surface、焦点与能力。ImGui 使用 SDL 应用的唯一窗口目录和 surface，交互参数显式注入；删除 WindowsX64 FramebufferScale 返回 ABI。MiniAudio/Text/RmlUi 使用显式链接请求，没有机械新增逐平台空项目。 |
| 如何使新增平台、backend 和部署清晰 | 平台拥有目标/SDK/packager，backend 拥有领域实现/recipe/compiler，有真实差异才建 integration。GameBuildContribution 完整绑定 packager/compiler；新 backend 不改平台 packager，新平台不改共同 Runtime。步骤和文件树见[扩展指南](PLATFORM_EXTENSION_GUIDE.md)。 |

```text
Standard Distribution / 平台产品
├─ 平台基础：target、SDK、位置、布局、packager
├─ backend：领域实现、Native facade、component recipe
└─ integration：实际平台与 backend 配置/调用
   ├─ 对应平台契约
   └─ 对应 backend 契约

共同 Runtime / Build / Editor Hosting → 中立契约
```

新增四个 BGFX、三个 SDL 集成库及 SDL Adapter 测试项目；没有新增生产 Program。`IGameContentCompiler` 与 `IGameBuildTarget` 分离，`GameBuildTargetBinding` 立即拒绝 target 不一致。平台工厂不接收创作服务，compiler 借用当前 generation；`CreateBindings` 替代旧入口，没有兼容 wrapper。`NativeComponentBuildOptions` 冻结 Static/Shared、有序 CMake 参数和配置输入，全部进入指纹，拒绝覆盖 SDK/组件 owner 及 unsupported recipe。

## 续跑发现并修复的问题

### Task 引导共享写入冲突

真实并发 MSBuild 发现 shared bootstrap 的 obj 输出发生 `CS2012`。修复位于中立 `Inno.Build.TaskHosting`，没有进入平台或 backend：

- 最小 publisher 使用本次载入拥有的私有 bootstrap；不并发写同一编译输出。
- 新 `BuildTaskRuntimeTask` 用 Core.IO FileLease 协调完整 Task runtime 的共享编译。等待期间 Yield MSBuild 节点，取得 owner 后 Reacquire；失败、取消或重新取得节点失败均释放 lease。
- 使用当前 `IBuildEngine3` 构建完整依赖，不再启动第二条 SDK/CLI 流程。保留文件 hash metadata、immutable host publication、私有载入与 owner retirement。
- 五个新增公开边界测试通过；包含旧 publication/static graph 回归的最终定向测试 16/16。两个真实并发 MSBuild 进程均成功，139.609 / 215.031 秒，无 CS2012。Design-time 引导成功且不执行 Native。

公开 Task 入口的必要性是 MSBuild 跨进程写入协调；它不进入游戏脚本 API。公共 XML、Wiki 和 AGENTS 同步。

### 消费者与说明同步

Canvas 的 Metal 测试仍使用被移除的封闭平台枚举；现通过 MacOS BGFX integration 的显式 profile，测试项目只增加所需构建期引用。Canvas 6/6；Windows 上 Metal 条件分支不等于 macOS 原生实测。

扩展指南的旧依赖图和 Composition 注册说明已更正。中立 Composition 只承载组合契约；Standard Distribution 才注册具体发行，平台基础不直接调用 backend。

## 环境、命令与可追溯证据

Windows 10.0.26200；.NET SDK 9.0.318。Web SDK 为 `C:/Users/23842/AppData/Local/InnoWebDotnet/dotnet.exe`，含 wasm-tools；独立 BGCS 使用其自有 fixture，Clang 为 `C:/Program Files/LLVM/bin/clang.exe`。不升级源码目标框架。BGCS 工程 global.json 要求 SDK 10，本机独立消费者在 owned 验收目录声明可用 SDK 9；未修改 BGCS 配置。

实施前 revision `818163b5222632a6bc2cbe92804b0c6b62b8921d` 的 baseline 与旧失败/中断日志保留。最终验收对象为当前 HEAD 加本轮未提交修复，源文件 hash 记录在 `results/current-source-identity.json`，各仓库状态在 baseline 与最终 status 日志中。

证据根：`artifacts/acceptance/backend-platform-integration`。精确命令、工作目录、耗时、返回值在 `results/contract-executions.json`、`remaining-gates.json`、`product-executions.json`、`quiet-executions.json`、`bgcs-executions.json` 和对应 logs 中。旧失败记录不覆盖，也不计作本轮成功。

```powershell
& 'C:/Users/23842/AppData/Local/InnoWebDotnet/dotnet.exe' build InnoEngine.sln -m:1 -nodeReuse:false
& 'C:/Users/23842/AppData/Local/InnoWebDotnet/dotnet.exe' test tests/build/Inno.Build.Tests/Inno.Build.Tests.csproj -m:1 -nodeReuse:false
& 'C:/Users/23842/.cache/codex-runtimes/codex-primary-runtime/dependencies/python/python.exe' artifacts/acceptance/backend-platform-integration/products.py
```

Debug/Release Solution 构建零 warning/error；产品入口不在 Solution 默认批量构建中，因此另行执行 Windows Editor 普通 Debug/Release 产品 Build。Debug/Release 有效 MSBuild 架构验证均 225/225 通过。Native 冷准备中的第三方 C4819/C4702 与 HarfBuzz CMake 提示单独保留，不把 Solution 的零 warning 外推为全部第三方编译无 warning；未修改 extern 或抑制日志。

## 契约与独立测试

最终 TRX 按稳定 test ID 去重：引擎与 Canvas **864 passed / 0 failed / 16 未执行**；不是全仓所有无关测试总数。Build 147、Architecture 86、Hosting 6、Authoring 33、Shaders 112、Input 18、PlayMode 54、Interactions 99、Reload 22、Runtime 61、Rendering Runtime 86、Rendering Core 49、SDL 7、最终 ImGui 35、Canvas 6、BGFX 原生设备/Shader 38，加新增 Task 5。定向 Task 16 包含 11 项重复回归，不重复累计。

Shader 的 16 个 macOS fixture 条件发现案例未执行，不能记为通过。没有 InternalsVisibleTo、反射穿透或测试专用生产入口。测试覆盖 target mismatch、预取消、缺 SDK、链接/参数身份、损坏 Native 缓存、窗口归属与失败清理、外部窗口不重复销毁、viewport surface/DPI、GameView 消费/隔离、Reload GC barrier 与 Faulted。

BGCS 独立 managed **1023/1023**；NativeAOT、Wasm 解释执行、Wasm AOT 各 **56 项实际 API 检查**，三种 import mode 为 DllImport、LibraryImport、FunctionTable。其证据分别在 `bgcs/native-aot`、`bgcs/wasm`、`bgcs/wasm-aot` 的独立 report.json；不把引擎游戏作为 BGCS 独立验收。

## 产品与消费者运行矩阵

| 路径 | 本轮证据与范围 |
| --- | --- |
| WindowsX64 CoreCLR | 发布 2456.891 秒；运行 18.594 秒，返回 0。不可见 Player 120 帧、Native 初始化与正常退出。 |
| WindowsX64 NativeAOT | 发布 1221.734 秒；运行 4.485 秒，返回 0。不可见 Player 120 帧、Native 初始化与正常退出。 |
| Web 解释执行 | 发布 2429.797 秒；运行 37.140 秒，返回 0。headless 实际昼夜、星光、移动光、输入、得分、音频样本及重载存储。 |
| Web AOT | 发布 1062.078 秒；运行 32.047 秒，返回 0。headless 实际昼夜、星光、移动光、输入、得分、音频样本及重载存储。 |
| Windows Debug Editor | 隐藏共享 Editor 公开消费者 6000 帧，FlappyBird Direct3D11，正常保存与释放；247.813 秒，返回 0。 |
| Windows Release Editor | 隐藏共享 Editor 公开消费者 6000 帧，FlappyBird Direct3D11，正常保存与释放；52.188 秒，返回 0。 |
| Rendering2D | 隐藏共享 Editor 公开消费者 6000 帧，Direct3D12，另完成生成的 EditorScripts 项目编译，正常保存与释放；425.312 秒，返回 0。 |

隐藏 Editor 是验收目录中的 public consumer，复用正式 Hosting、Windows integration 与 backend；为遵守用户桌面限制，明确取消 detached viewport/live-resize，只创建不可见主窗口。它不是完整多窗口或 Play UI 验收。第一次 fixture 仍请求额外窗口，与其单窗口限制不匹配；已修正 fixture 的明确 Presentation 选择，失败日志保留，没有为验收改生产行为或增加测试后门。

Rendering2D 验收副本位于引擎 artifacts 内，普通 SDK 首次构建因继承引擎 Directory.Build.targets 而错误增加内部注册 generator。最终构建显式隔离父级 Directory.Build.props/targets，复现真实独立 Project 的边界；只使用当前生成的裁剪脚本 reference 与 analyzer，没有修改生成项目或增加实现程序集引用。

Windows Player 的隐藏运行验证发布闭包和生命周期，不能代替人工玩法、听音或画面比较。Web 使用独立 headless Edge/SwiftShader、私有 profile 与公开 canvas 输入，静音但验证实际 WebAudio 非零样本；不能等同于所有浏览器或硬件 GPU 验收。

正式 Windows Editor Release 普通 Build 为 1595.282 秒，返回 0；对应日志是 `logs/editor-release-build.log`。它与隐藏消费者的构建/运行记录分别保留。

### Web 画面与玩法

- `web-interpreted`：11 项检查通过，星光像素 107，垂直移动 102.17 px，光与鸟最大偏差 11.69 px；实际得分 1，audio peak 1.077534，运行 page error 0，console warning 315，console error 2。
- `web-aot`：11 项检查通过，星光像素 109，垂直移动 102.04 px，光与鸟最大偏差 11.43 px；实际得分 1，audio peak 0.967756，运行 page error 0，console warning 316，console error 2。

两条路径均重新读取上一轮真实得分，没有直接注入 gameplay 状态；画面 day/night/flight/scored/reloaded 留在 `captures/<deployment>`。warning 单独保留在 report.json，不把它们隐藏为“完全无日志”。本次 headless HTTP 服务器还记录未提供 favicon.ico 的 404；console error 与游戏 page error 分开统计，不能宣称全部 HTTP 请求无错误。第三方 WebGL capability 查询 warning 与 favicon 缺口保持可见，不修改 extern 或 suppression 来隐藏。视图中的灯光跟随位置得到像素验证；并非穷举所有自定义 Shader 的光照方向。

## 构建性能与缓存

本机第一次普通 Debug Editor 产品准备耗时 808.250 秒；Native 阶段 272.439 秒，哈希 139978 次文件读取、13948278105 bytes，Native 工具进程 0，组件产物 8。这不是冷 Native 编译基准，也不是无争用的前后对照；本轮四条发布的准备/冷编译耗时以上表及完整日志为准。

正式 Release 首次本轮准备的 Native 阶段为 1307.570 秒，262918 次文件读取、26066900325 bytes、16 个 Native 工具进程、8 个组件产物。它使用当次已冻结的工具选择，不是从清空全部 SDK/缓存开始的全链冷基准；此前隐藏消费者等其他验证也有自己的准备记录。

| 串行热构建 | 总秒数 | Native 秒数 | 哈希文件读取 | 哈希 bytes | Native 工具进程 | 新 Task 载入目录 | 变化的部署 Native 文件 |
| --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| editor-hot-1 | 242.281 | 80.703 | 139978 | 13948289625 | 0 | 17 | 0 |
| editor-hot-2 | 215.640 | 77.159 | 139978 | 13948289625 | 0 | 17 | 0 |
| editor-hot-3 | 215.250 | 76.350 | 139978 | 13948289625 | 0 | 17 | 0 |

测量文件 `results/hot-build-observations.json` 包含每个部署 Native 文件的 SHA-256、长度和 mtime 前后值。Native counter 不包括 SDK/Task 的托管编译进程，Task 载入目录计数也不是进程数；不能混用。零变化只证明这组完整 Native 部署没有字节或 mtime 替换，不是全仓写操作统计。

热命中仍进行完整内容/输出校验，哈希 IO 成本显著；本轮没有可信的冷/热前后同环境对照，不宣称性能提升、零 IO 或零构建成本。保留正确失效、并发 lease、冻结后输入变化拒绝发布、缺 export 和原子提交。无需改为 mtime 判定或弱化完整性来制造性能数字。

## 清洁度、所有权与剩余实机 gate

结构扫描 225 项目、142 非空 Solution Folder，无游离源码、重复程序集、空物理目录或冗余生产入口；生产 Program 只有六个。0 incoming reference 的测试、入口和动态发现 Panel 具有各自真实 owner，不据此误删。Debug/Release 引用图、公开 XML、多参数排版、原生消费与 Player closure 门禁通过。309 页 Markdown 本地链接通过。

compiler 借用 generation 服务，Pipeline 拥有 staging/任务并在失败时 drain；SDL application 拥有自建窗口、只借用外部原生窗口，wrapper 先失效再由原 owner 销毁。ImGui 注销 callback/viewport 后才释放 SDL；Core Events、Identity、Missing/last-good、唯一 History/Workspace 与 Full GC → finalizers → Full GC/弱监测保持原协议。

仍需本轮真实设备/桌面验证：

- Windows Debug/Release：Play/Stop 实际操作，浮动 GameView 的焦点与失焦释放，前景/Modal/Popup 阻止穿透，Export 自动关闭、滚动/长 label、ShaderEditor 小地图。
- 任意多屏高 DPI、live-resize、最小化与恢复的桌面画面；原生契约测试不替代这组交互。
- Windows Player 人工玩法、声学听音和画面对照；Web 的其他硬件/浏览器。
- macOS Native/Editor/Player 实机；Linux 仅承接已有工具能力，不声明完整 Player。WindowsX86、iOS、NS 未实现/未注册。

上述是清楚记录的证据缺口；没有用历史截图、本机源码编译或后台帧运行冒充当前实机通过。未启动周期自动化或外部通知，也未自动提交。

建议本轮 commit：`fix(build): coordinate concurrent task runtime bootstrapping`；Canvas：`test(canvas): use explicit macos bgfx integration`；文档可独立为 `docs(architecture): close integration acceptance and clarify ownership`。指定 Glass.aiff 在 Windows 不存在，提示音未播放。

## 本轮缓存清理

四个已关闭的 owned headless 浏览器 profile 已删除，逻辑文件大小合计 230.65 MiB。清理时 C 盘可用 82.16 GiB；其他 Native 编译当时仍可能写入，因此这个数值不是清理前后的独立磁盘差分。删除前检查了精确绝对边界、reparse point 和活跃进程；最终产物、日志、图片、fixture、当前 Native 缓存及 SDK 全部保留。详见 `results/profile-cleanup.json`。
