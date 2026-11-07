# .NET 10 / C# 14 实施与验收

后续的非 BGCS 扩展验收、工程隔离修复、Sample 导入、当前性能基线与仍开放问题见[2026-10-18 续验收报告](NET10_NON_BGCS_ACCEPTANCE_2026_10_18.md)。下表保留首轮证据，最终复验结果以续验收为准。

## 当前源码

范围：InnoEngine、InnoEngine.Canvas、InnoEngine.Rendering2D、InnoEngine.Samples。四仓库使用 SDK 10.0.402、`latestPatch`、禁用预览 SDK；运行库、工具、测试使用 net10.0，Browser 使用 net10.0-browser。两个 Roslyn 生成器保留 netstandard2.0 工具产物。源码与脚本明确使用 C# 14，隐式及全局 using 保持禁用。

Roslyn 包统一 5.9.0，MSBuild 包为 18.9.6，SDK 实际编译器为 5.9.0-1.26505.111、MSBuild 宿主为 18.9.15。xUnit/VSTest 保持原有体系。BGCS 包为 2.1.0。

工作区初始状态和二进制 patch 保存于 `artifacts/acceptance/net10/*.initial-*` / `*.initial.patch`；保留四仓库已有源码、验证脚本和安装包修改。

## 改良

- 一次脚本或 IDE 操作固定目标、语言、编译器身份和 framework reference 的不可变字节快照。公开 API 契约指纹与产物指纹分离，框架内容和工具变化进入 API、脚本及 IDE 插件引用缓存键。
- MSBuild 输入捕获执行 SDK 的符号准备 target，记录实际编译符号；不执行编译或原生产品准备。
- framework 选择以 SDK 求值的 TFM 对应到实际还原 `targetAlias`，避免把 Browser 的平台 moniker 误作 NuGet target 名称。Task 缓存缺少当前 target 时先执行正常还原，再验证和发布。
- Runtime 目录生成器识别 C# 14 extension 声明，避免将声明组错误注册为运行时类型。
- 修复 C# 14 的 `scoped` Lambda 参数歧义和 RmlUi 数组到 Span 重载选择；原生输出缓冲区显式使用可写 Span。
- `field` 用于平台帧率、音频、渲染资产和插件中完全归属于属性的状态；保留校验、默认值、通知、序列化属性及 Stable ID。
- 使用 `Convert.ToHexStringLower` 直接生成哈希字符串，减少中间字符串；使用空条件赋值简化适用的局部接收者更新。
- `scripts --ide` 通过正式编译器入口同时编译并生成 IDE 工程。Canvas 测试和 Rendering2D GPU 脚本使用正式开发入口及 MSBuild 求值输出。
- Build 入口部署 CLI 自身的依赖清单和 SDK 求值的私有运行依赖；模块发现按该清单的准确部署路径装载，Canvas/UI 和 Rendering 的逻辑脚本 API 在正式入口中可用。
- 独立 `Inno.Scripting.Compiler.Tests` 无 Editor 呈现或原生绑定依赖，可由普通 CI 验证语言与 IDE 契约。
- Wiki 源码解析明确使用 C# 14，引用扫描通过 PE 元数据识别托管程序集，避免将原生 DLL 送入 Roslyn。

领域公开 API 和持久数据形状保持一致。

## 验证命令与证据

所有 dotnet 命令使用 `global.json` 所选 SDK。原始输出位于 `artifacts/acceptance/net10/`，CI 上传对应目录与 TRX。本机验证使用 `C:\Users\23842\.dotnet\dotnet.exe`，并为子进程设置相同的 `DOTNET_ROOT`、`DOTNET_HOST_PATH` 和 PATH。系统默认 `C:\Program Files\dotnet` 尚未安装此 SDK；普通终端及 IDE 需要选择已安装的 .NET 10 SDK。

| 检查 | 命令 / 证据 | 当前结果 |
| --- | --- | --- |
| 源码 Debug | `dotnet build InnoEngine.sln -c SourceDebug --disable-build-servers -m:1 -nr:false -p:UseSharedCompilation=false`；`build-source-debug-final.log` | 通过，0 警告、0 错误 |
| 源码 Release | 相同参数、`-c SourceRelease`；`build-source-release-final.log` | 通过，0 警告、0 错误 |
| 独立托管 Debug | `tests/Invoke-ManagedChecks.ps1`；`managed-final/`，38 份 TRX | 构建通过；1,110 通过、0 失败、0 跳过 |
| 独立托管 Release | `Invoke-IsolatedTests.ps1 -Configuration SourceRelease`；`managed-release-final/`，38 份 TRX | 1,110 通过、0 失败、0 跳过；包含普通与进程隔离测试 |
| 构建工具共享套件 | `build-tests-final/`、`cli-closure-final/` | 307 项整套通过；新增断言的 namespace 修正后单项复验通过，合计 308 项通过；2 项 Unix 链接用例在 Windows 跳过 |
| 构建工具专项 | `build-tests-fixes/`、`cli-closure-final/` | 框架选择、缺失 target 还原、Task Publisher 和 CLI 公开脚本引用均已复验通过；属于共享套件覆盖，不重复计数 |
| 编译器契约 | `compiler-tests/` | 3 通过、0 失败 |
| SDK 派生符号 | `sdk-symbol-final.log`、`sdk-symbol-final/` | 1 项进程隔离测试通过 |
| 源码架构 | `architecture-final.log` | 通过 |
| Wiki/API/示例 | `wiki-gate.log`；`artifacts/acceptance/wiki-gate/1c85aa82547141188bbac9c328f2ff6c/` | 45,590 个 API 符号、190 个示例已扫描；4 项 Windows 产品引用诊断暂缓，详见下文 |
| 当前 Markdown 链接 | `changed-markdown-links.json` | 已修改页面没有失效的相对链接 |
| 实际项目脚本 / IDE | `canvas-scripts-final.log`、`rendering2d-scripts-final.log`、`flappybird-scripts-final.log`；对应 `*-ide-build.log` | 三个项目的引擎脚本编译、IDE 生成和 SDK IDE 构建均通过；IDE 构建 0 警告、0 错误 |
| Canvas 契约 | `canvas-tests.log`、`canvas-tests/` | 6 项通过；Windows 未执行 Metal 专用方法体 |
| 插件导出与安装 | `canvas-export.log`、`rendering2d-export.log`、`package-content-check.json` | 正式重新导出并完成 FlappyBird 安装态编译；47 / 155 个 Asset 包条目与各自当前源码一致 |
| 作用域锁文件 | `restore-packages*.log` | 664 份已跟踪锁文件真实还原完成 |
| Wasm SDK | `workload-install.log`、`wasm-link-*.log` | 安装及最小链接验证通过 |

托管入口按真实项目引用闭包选择 152 个独立项目，另将 78 个原生/绑定消费工程记录为暂缓，并附依赖链。实际调用 BGCS 的测试使用 `ExternalDependency=BGCS`，筛选同时用于普通、隔离发现及隔离执行。中立模拟工具测试继续运行。

构建工具整套收尾执行最初包含一条新增断言拼写错误：使用了实现目录中的 `Runtime`，实际逻辑契约为 `InnoEngine.Rendering.RenderPersistentResourceId`。已修正断言并通过真实 CLI 引用生成复验；原始失败日志保留。`build-tests-fixes/` 中另一次关闭 ProjectReference 构建的探测缺少私有运行依赖；使用完整正常构建后，`cli-closure-final/` 复验通过。没有通过放宽测试或装载规则处理部署缺失。

`configuration-audit.json` 核对了 230 个工程与 665 份当前锁文件（664 份原有作用域锁文件及新增编译器测试锁文件）。锁文件全部使用当前目标；两个轻量入口通过 `entry/Directory.Build.props` 显式取得 net10.0。BGCS 暂缓用例的精确发现清单位于 `bgcs-deferred-tests.txt`。

完整 Editor 脚本套件的一次探测得到 78 通过、73 失败；73 项均因 ImGui 原生部署索引缺失而失败，不能计为通过。这些产品依赖检查需要正常准备对应原生闭包后复验，不通过复制 DLL 或伪造索引绕过。

完整 Wiki 检查未通过：三项诊断要求先构建 Windows 目标的 `Inno.Editor.WindowsX64` / `Inno.Adapter.Audio` 编译引用，另一项是所属 Editor 项目缺少语义编译的连带示例诊断。其余扫描没有报告源码架构、公开 XML 或示例签名问题。需要通过正式入口准备 Windows Editor/Player 产品闭包后，原命令重新验收；不能将当前结果记为完整 Wiki 通过。

## 性能

同一 SDK、.NET 10.0.13、Release、相同 32 字节哈希输入，在关闭 tiered compilation 的同一进程中交替采样 5 次，每项 200,000 次操作。`hex-benchmark.json` 记录结果。

| 实现 | 分配 / 次 | 200,000 次耗时范围 |
| --- | --- | --- |
| ToHexString + ToLowerInvariant | 304 bytes | 12.28–14.03 ms |
| ToHexStringLower | 152 bytes | 5.50–6.30 ms |

分配减少 50%，归因于标准库直接格式化及调用方替换。该结果只适用于这条字符串路径，不推断整帧、构建或游戏性能。`field` 与空条件赋值属于表达改良，不声称性能收益。首次原生工具构建期间的整体耗时受并行负载影响，不作为源码优化对比。

完整构建、脚本冷/增量编译及产品帧时间的实现前后性能比较未执行；当前只将上述可重复微基准计为性能收益。产品 GC、帧时间和资源释放采样随完整产品验收复验。

## 产品与暂缓

- Canvas、Rendering2D、FlappyBird 正式脚本编译、IDE 投影、IDE 构建及插件重新导出已通过。场景、Prefab、Play、存储及产品退出没有由这些编译结果代替验收。
- Windows Editor/GPU、CoreCLR/NativeAOT、Browser Mono/Wasm AOT 的最终发布与运行，需要有效的完整原生闭包；如果绑定身份需要重新生成，等待 BGCS 更新后通过正常入口准备并复验。
- macOS ARM64/Metal、真实视觉、焦点、DPI 和音频检查没有在 Windows 机器上执行。
- SjLj 最小程序在所选 Emscripten 中不带补丁时出现 `__wasm_setjmp` / `__wasm_setjmp_test` 未定义；加入当前手写补丁后链接成功。该结果不替代完整 Browser 产品验收。

尚未完成的检查不记为成功。只有全部适用门禁通过后才能标注本轮完成；完整产品矩阵单独报告。

Windows 环境没有 `/System/Library/Sounds/Glass.aiff`，未播放该提示音。
