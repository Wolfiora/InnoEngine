# .NET 10 / C# 14 非 BGCS 续验收报告

## 结论

**主体升级具备较扎实的托管构建、语言、缓存及内容工作流证据，但当前不能宣称全面验收通过或性能已经优秀到没有改良空间。**

本轮发现并修复了一个真实的 MSBuild 作用域缺陷。并行渲染录制的逐命令分配问题仍可复现；完整 Native 产品、真实 GPU、其他平台与长期负载验收尚未闭合。

验证日期：2026-10-18，Asia/Taipei。源码为 `1f53d6645a0e999469bb4898e8457d86d58b0a31` 加当前工作区改动。使用用户目录 SDK 10.0.402、运行时 10.0.13、C# 14；Windows x64、Ryzen 9 6900HX（8 核 / 16 线程）。设备清单有 RTX 3070 Ti Laptop GPU 和 AMD 集显，设备清单不代表已经执行 GPU 验收。

原始证据位于 `artifacts/acceptance/net10-followup/`；前轮结果位于 `artifacts/acceptance/net10/`。本轮不更新 BGCS、不执行绑定生成或 ABI 专项、不手改生成绑定及第三方源码。

## 本轮修复

触发条件：Project 位于引擎 checkout 子目录，随后构建生成的 `InnoProject.sln`。

`Directory.Solution.props` 原先把嵌套 Solution 的还原输入替换为引擎源码项目，造成脚本工程缺少自己的 `project.assets.json`，出现 `NETSDK1004`。同时，生成工程可能继承父目录的 `Directory.Build.props/targets`，使 IDE 编译输入偏离脚本编译器声明的契约。

修复内容：

- 根 Solution 的还原图及 Task owner 规则仅作用于所属 `InnoEngine.sln`。
- 生成的 Runtime/Editor 工程在 SDK 导入前明确禁用无关 `Directory.Build.props/targets`。
- 通过公开 `ScriptCompiler` 和 SDK 命令增加两个独立回归；不增加引擎公开 API、测试后门或内部可见性。

回归证明：带有拒绝构建的 ambient MSBuild 文件时，生成工程仍正常构建；checkout 子目录的独立 Solution 能还原自己的工程。SourceDebug 下根 Solution 和托管 `.slnf` 保留 224 个源码还原根（Debug 包含 Editor entry，为 225 个），嵌套项目的引擎还原根数量为 0。修复后的全源码 Debug / Release 均构建通过，0 警告、0 错误。

相关实现：[Solution 还原](../../../Directory.Solution.props)、[Solution owner](../../../Directory.Solution.targets)、[工程生成器](../../../src/runtime/scripting/Inno.Scripting.Compiler/Compilation/ScriptProjectGenerator.cs)、[回归测试](../../../tests/scripting/Inno.Scripting.Compiler.Tests/IdeProjectIsolationTests.cs)。

## 验收结果

| 范围 | 结果与限制 |
| --- | --- |
| 最终托管套件 | 修复后 1,112 项通过、0 失败、0 未执行，38 份 TRX；包含普通与进程隔离用例。 |
| 新增隔离回归及语言共享用例 | 4 项共享用例及 1 项框架内容失效隔离用例通过，计入上述 1,112 项，不重复计数。 |
| Native 消费项目中的托管测试 | 补跑 13 个程序集，共 441 个用例。初次 341 通过、84 失败、16 因平台限制未执行；正式部署 Shader 工具后 8 个失败用例复验通过，最终为 349 通过、76 仍未通过、16 未执行。 |
| 其余 76 项 | 73 项缺少完整 Native 部署，3 项要求目标程序集嵌入的 Shader 资源；没有将这些结果计为功能通过。 |
| 独立 Shader 工具链 | 标准发行的 `shader-tools` plan 不含 binding producer；通过真实 `ProductNativeBuildPlan` 和 `ProductNativeDeployment` 准备、部署。10 项编译工具链测试及 1 项部署 ToolRunner 测试全部通过；其中 3 项此前已经通过，不重复计入新增数量。 |
| 内置 Shader 编译 | 正式 `CompileCompositionShaderGraphs` / `CompileImGuiShaderGraphs` 目标通过；Composition、OutputTransfer、ImGui 各 4 份产物，共 12 份，覆盖 Direct3D11、Direct3D12、Vulkan、OpenGL。 |
| Sample 导入 | Canvas、Rendering2D 的 `~Samples` 分别通过正式 Import Sample 事务导入独立 FlappyBird 副本，保留 `~Samples` 目录名；修复后的引擎脚本和 SDK IDE 构建均通过。 |
| 三个实际 Project | Canvas、Rendering2D、FlappyBird 的最终正式脚本编译、IDE 投影及 SDK 构建全部通过，0 警告、0 错误。 |
| 架构 / Wiki | 修复后的源码架构最终复验通过。前轮完整 Wiki 的 4 条 Windows 产品引用及所属示例诊断仍未闭合，不能称为完整 Wiki 全绿。 |

补充测试包含 Editor Interactions 99 项、PlayMode 的 47 个可执行托管用例、Rendering Assets Authoring 33 项及 Shaders 112 项通过。依赖 Native 的 PlayMode 用例另行保留失败记录，不由上述成功结果替代。

13 个程序集的原始 TRX 在 `consumer-tests/`，逐用例记录在 `consumer-cases.json`；工具部署后的复验在 `shader-tool-tests/`。`product-closures.json` 记录标准产品声明：Windows Editor 包含 7 个绑定 owner，Player 包含 5 个；`shader-tools` 不含 binding owner。完整产品准备会进入这些依赖，因本轮 BGCS 边界而没有启动该准备链。

12 份 Shader 二进制生成成功不等于已验证目标程序集嵌入、产品部署、设备初始化或帧提交。缺少资源的 3 项与完整 Wiki 的 4 条诊断仍需要正常产品构建后复验。

## 性能与缓存

### 当前构建 / 脚本基线

连续三次、新 CLI 进程；SDK、输入及配置固定。托管构建使用 152 项目的独立图、已有输出、`SourceRelease`、`--no-restore`。脚本使用三个独立的 FlappyBird Assets + Plugins 副本，每个副本依次运行冷缓存、相同输入热缓存、单个 C# 文件追加换行后的增量编译。

| 操作 | 三次 wall time |
| --- | --- |
| 托管热构建 | 36.56 / 30.07 / 30.61 秒 |
| 脚本 CLI 冷缓存 | 52.00 / 55.32 / 51.68 秒 |
| 脚本 CLI 热缓存 | 15.97 / 15.89 / 15.70 秒 |
| 脚本 CLI 单文件变化 | 21.86 / 21.10 / 21.34 秒 |

三组操作全部成功；热运行的 16 个缓存 DLL 的路径、内容和 mtime 均保持一致。源码变化后产生独立产物；每组缓存 DLL 文件数量从 16 增至 26，该数字是物理文件数，不等于逻辑模块数。

这些是端到端命令耗时，包含工具准备、导入、引用处理及导出，不是纯 Roslyn 编译时间。没有实施修改前的同配置完整对照，也没有完全隔离机器后台负载；不能据此声明整体性能提升。采样发生于工程隔离修复前；修复改变的是 Solution/IDE 工程配置，表中保留实际采样状态。CPU 只统计父进程，峰值工作集未取得，不把空值当作 0。

一次正式 Shader 工具热命中记录读取 31,882 个文件、2,970,894,960 bytes，Native 构建进程数为 0。命中缓存仍有完整性验证成本；CLI 热耗时的具体分项尚未隔离，不能把全部时间归因于编译器或 SDK。

### 并行录制：确认仍有分配成本

通过公开 RenderGraph API、无 Native/GPU 的 backend，使用当前 Release Rendering 程序集。每个 case 48 帧预热、32 帧采样，三次独立进程，并校验实际 Draw 回放数量。`GC.GetTotalAllocatedBytes(true)` 计入全部线程。

| 负载 | 串行分配 / 帧 | 启用并行录制后的分配 / 帧 |
| --- | --- | --- |
| 1000 Draw | 约 1.61 KB | 约 107.78 KB |
| 10000 Draw | 约 1.61 KB | 约 1.15 MB |

该路径是 opt-in，并不表示默认所有项目都有这项分配。当前 [RecordedRenderCommandEncoder](../../../src/services/rendering/Inno.Rendering/Device/RecordedRenderCommandEncoder.cs) 逐命令保存捕获委托，并对必须跨录制阶段持有的 Span 数据创建快照。需要按所有权和生命周期重新设计录制数据后再测，不能仅通过语法简化消除成本。此结果不等于真实 GPU 帧率结论。

原始数据：`timings.json`、`script-cache-checks.json`、`measurement-environment.json`、`parallel-recording-1.json` 至 `parallel-recording-3.json`。前轮 `ToHexStringLower` 微基准的分配收益继续有效，但不外推到构建或游戏整体。

## 仍开放的问题与验收边界

| 优先级 | 范围 | 当前事实 / 后续关闭条件 |
| --- | --- | --- |
| P2 | 并行录制分配 | 当前 .NET 10 实测仍存在；优化后需相同公开边界回放与分配对照，保留并行和异常语义。 |
| P2 | 验收调度 | [隔离测试入口](../../../tests/Invoke-IsolatedTests.ps1) 在共享套件或单个隔离用例失败后直接抛出，后续独立用例不会继续；应最终汇总失败，同时完成能独立执行的用例。 |
| P2 | 超时诊断 | [NativeWindowTestHost](../../../backends/Bgfx/tests/Inno.Adapter.Rendering.Bgfx.Tests/NativeWindowTestHost.cs) 的超时分支终止进程后没有保留已读 stdout/stderr；属于诊断缺口。本轮未重现 macOS 退休超时，不能宣布该问题根治。 |
| P3 | XML 说明质量 | 仍有模板化的参数/ownership 文案；符号、标签和签名门禁通过不代表行为说明充分。 |
| 环境 | 默认 SDK | 系统默认 `dotnet --version` 无法满足仓库的 SDK 声明；本轮明确使用 `C:\Users\23842\.dotnet\dotnet.exe`。普通终端 / IDE 的 SDK 选择尚未由本轮验证为开箱可用。 |
| 暂缓 | 产品 / Wiki | 完整 Windows Editor/Player 及 Wiki 产品引用链需要正常 Native/绑定闭包；保留既有 73 个 Editor.Scripting 部署失败用例及本轮 76 个产品依赖失败，不计为通过。两组来自不同程序集，详见原始记录。 |
| 未执行 | 实机与长期运行 | macOS ARM64/Metal、Browser Mono/Wasm AOT 完整运行、Windows GPU/DPI/焦点/音频、长时间 reload 与资源趋势、最终 CoreCLR/NativeAOT 发布矩阵尚未在当前代码闭合。 |

本轮没有为了通过验收手工拼装绑定、伪造部署索引、删除断言、缩短 unload barrier 或吞掉生产错误。Shader 工具部署是标准无绑定产品的真实部署，不替代 Editor/Player 的完整部署。

因此，可评价为 **升级与核心契约验证扎实，工程边界明确；产品完整性、帧分配和验收诊断仍有工作要做**。当前证据不足以使用“无敌优秀”或“全面无问题”的结论。

Windows 环境没有 `/System/Library/Sounds/Glass.aiff`，本轮未播放该提示音。
