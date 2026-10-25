# BGCS 4.1.2 消费升级与构建边界验收

日期：2026-10-25；宿主：Windows x64；分支：`dotnet10`；实施前 revision：`55815b16171ea21e653ce0d5eed7498e377f1079`。

## 结论与范围

公共 NuGet BGCS 4.1.2 已进入引擎实际恢复、生成、Task publication、原生构建和产品发布链。Browser 继续使用官方 LibraryImport；桌面既有 FunctionTable 保持。没有修改 BGCS 源码、包、二进制、第三方 extern 或手工修改生成结果。

本轮构建专项 **74 项通过，0 失败、0 跳过**。正式架构 / XML / Wiki 门禁通过；七个 HostSource owner 正式生成及检查通过。指定的 FlappyBird 示例完成 Browser Mono / AOT 各 3,000 帧；Windows CoreCLR / NativeAOT 的 D3D11、D3D12、Vulkan 各 300 帧并正常退出。

本次升级发现并修复一处引擎 MSBuild 求值隔离缺陷。它不是 BGCS 4.1.2 缺陷；根因、通用修复和真实公共 Task 回归见下文。此前 F01/F02/F08 三项优先修复的 Core 与运行链证据仍见 [三项优先修复报告](PRIORITY_FIX_ACCEPTANCE_2026_10_25.md)，不重复计入本轮测试数量。

这份报告不宣称整个引擎完美，也不把有界 smoke 当作完整交互、长期运行或所有设备验收。

## SDK 来源、版本与未修改证明

- SDK：10.0.402，`latestPatch`，`allowPrerelease=false`；.NET 10 / C# 14 不变。
- BGCS 公共发布：[v4.1.2](https://github.com/FLwolfy/BindGen-CS/releases/tag/v4.1.2)，上游 revision `2894538911645ad28c74060840a02c154c15ce4d`。
- [上游公共 NuGet 验证 receipt](https://github.com/FLwolfy/BindGen-CS/releases/download/v4.1.2/nuget-public-verification.json)已单独保存。它证明上游发布；不能代替本报告的引擎消费验收。
- 本次操作专用 NuGet.Config 清空继承源，仅使用 `https://api.nuget.org/v3/index.json`。本机实际消费并逐文件检查五个包：BGCS、BGCS.Core、BGCS.Contracts、BGCS.Runtime、BGCS.Extension.BrowserWasm，均为 4.1.2。umbrella 包中附带的 SDK DLL 不应虚算为额外 NuGet 包；平台 scope 中其他 BGCS package entries 也纳入锁文件检查，但不将它们当作在对应设备上实际加载的证明。
- 439 个 tracked lock scope 的 683 个 BGCS package entries 全部解析到 4.1.2，tracked scope 中旧版本项为 0。额外的 438 个既有 untracked scope lock 不盲目加入或删除，归属问题继续由 engine issue #23 跟踪。
- 148 个实际包文件逐一 SHA-256 比较，改变 0；BGCS git 工作树 clean，Agent / AGENTS 文件 0。
- 引擎外三个调用方仓库工作树均 clean；没有为升级改动其业务源码或包内文件。
- 历史验收文档和旧的内容寻址 publication 保留其原版本身份，不被当作当前生产配置。没有为“无残留”删除未知 owner、活动产物或全局 NuGet 缓存。

## SDK issues 的消费复验

两个问题在 GitHub 上均已核实为 `closed / completed`：

| 问题 | 4.1.2 行为 | 本地消费证据 |
| --- | --- | --- |
| [BGCS #1](https://github.com/FLwolfy/BindGen-CS/issues/1) Browser FunctionTable admission | Browser profile 明确拒绝未支持的 FunctionTable；公开 DllImport / LibraryImport 路径保留 | 独立 SDK 探针拒绝 FunctionTable，原 last-good 的 7 个文件逐字节不变；正式产品继续 LibraryImport 并实际运行 |
| [BGCS #2](https://github.com/FLwolfy/BindGen-CS/issues/2) Browser target-provider fingerprint | provider 为 eligible stage 提供完整指纹 | 同一独立公开 API 请求，primaryFrontend 和 csharpEmission 冷 miss、热 hit；引擎真实生成不再报告 Browser target provider 缺少完整指纹 |

探针只引用公共 NuGet 包，通过 `BindingGenerator` builder、官方 Browser plugin 与 `GenerationRequest.FromFile` 执行，没有引擎 ProjectReference 或私有入口。测试包含返回高位 UInt64 的声明，使用当前 workload 解析的真实 Emscripten sysroot。

包含 volatile / token-pasted 预处理输入的前端按 SDK 安全策略重新读取，仍属于正确行为；eligible stage 命中不等于所有组件的所有阶段都必须命中。本轮不把产品构建耗时改进归因于单个 fingerprint 修复。

## 新发现的引擎缺陷：Restore 与 compilation 的求值身份

首轮 HostSource BGFX 生成失败，SDK 明确报告：`The deployed worker belongs to a different BGCS build.`

实际 publication 内 SDK DLL 已为 4.1.2，但 `worker-common.zip` 来自 4.1.1：

| 文件身份 | 长度 | SHA-256 |
| --- | --- | --- |
| 首轮私有加载 / 公共 4.1.1 worker | 2,591,205 | `9acb8f0b240b6ed69131c99fed90a65000fc8de2d14052e419b177cbec6801ff` |
| 公共 4.1.2 / 修复后 publication worker | 2,591,189 | `6c7e73541bfa4f7af1934d09387dc0277497c5e9b086af393fbea9f2a46ae5a8` |

NuGet 生成的 imports 和 assets 已更新。问题在于 Task 使用同一 MSBuild 配置执行 Restore 和后续 compilation：MSBuild 缓存了 Restore 开始前的 ProjectInstance / package content items，后续 ResolvePackageAssets 却读取新的 assets。最终出现新程序集与旧内容资源混装。SDK 的 worker 身份检查正确阻止了生成。

修复位置是中立 Task hosting：

1. owned Restore 明确传递标准 `ExcludeRestorePackageImports=true`。
2. Task runtime compilation 明确传递 `false`，让两个阶段拥有不同求值配置身份，重新读取恢复后的 imports。
3. 独立 input Restore 同样使用该阶段属性，并保证属性不会被 inherited-property removal 消除。
4. compilation 的实际阶段属性进入既有完整请求身份；不复用旧阶段身份下的 publication。
5. lease、Build 生命周期复用、取消、等待节点 Yield/Reacquire、冻结输入及原子 publication 流程保留。

没有按 BGCS 包名复制文件，没有手改 NuGet imports，没有删除缓存“碰运气”，没有添加第二个加载器、平台分支或 SDK 源码依赖。新 publication `36FE11E8D2317B47A8A0D9BEBA67CD6FFFFCA19B01B881CDBC79E1604AF63391` 的 worker 与公共 4.1.2 一致。

新增 `TaskRestoreEvaluationTests` 的两个用例通过真实 `dotnet msbuild` 加载公开 Task：一个覆盖 BuildTaskRuntimeTask，另一个覆盖 RestoreOwnedProjectTask。Restore 将已有 imports 从旧 payload 改到新 payload，后续编译必须消费新 payload。没有 IVT、私有反射或扩大 internal 可见性。初始实际混装失败与修复后的正式生成共同构成回归证据。

## 文件归属与公开契约

```text
AGENTS.md
  section 29: owned Restore / compilation evaluation ownership
  section 30: deep completion audit and SDK integrity
  section 31: English issue titles/body and type(scope): format
build/msbuild/Inno.Packages.props
  BGCS package family 4.1.2
build/tasks/Inno.Build.TaskHosting/
  BuildTaskRuntimeTask.cs
  RestoreOwnedProjectTask.cs
tests/build/Inno.Build.Tests/
  TaskRestoreEvaluationTests.cs
<existing project owners>/packages*.lock.json
  official scoped restores; tracked scopes only
docs/
  current package requirements, TaskHosting, native build and platform pages
  architecture/CURRENT_ACCEPTANCE.md
  architecture/CURRENT_ISSUES.md
records/architecture/acceptance/
  BGCS412_CONSUMER_ACCEPTANCE_2026_10_25.md
```

没有新建项目、入口、平台或空目录；不改 Solution / ProjectReference 图，没有新增 public API。游戏 API、Stable ID、脚本导出、序列化数据和场景结构不变。七个 HostSource owner 通过正式生成流程验证；新版本的当前输出与已跟踪生成源码一致，没有人为整理生成文件。

## 验收记录

独立证据根为 [`D:/InnoEngine-Verification/bgcs412-2026-10-25`](D:/InnoEngine-Verification/bgcs412-2026-10-25)。这是本次操作的本机日志和产物 owner，不是源码或产品中的固定路径。

| 验收 | 实际结果 | 证据文件 |
| --- | --- | --- |
| 公共包恢复 | 五个实际包、439 tracked scopes，均 4.1.2 | `NuGet.Config`、`restore-*.log`、`identity-baseline.json`、`tracked-scope-scan-final.json` |
| SDK 公开 API probe | eligible stages 冷 miss / 热 hit；FunctionTable 拒绝且 last-good 不变 | `public-sdk-probe.log`、`public-sdk-probe/result.json` |
| 正式 Build 入口 | 0 警告，0 错误 | `cli.log`、`cli.binlog`、`cli-final.log` |
| 七个 HostSource owner | GenerateBindings、CheckBindings 通过 | `generate-final-Inno.Native.*.log`、`check-Inno.Native.*.log`、`check-bindings-results.json` |
| Binding / native / Task 相关测试 | 65 通过，0 失败 / 跳过 | `tests.log`、`tests/*.trx` |
| Restore / static graph / generator scope | 9 通过，0 失败 / 跳过 | `restore-tests.log`、`restore-tests/*.trx` |
| 架构 / XML / Wiki | 238/238 工程；33,567 API symbols；196 示例 | `wiki.log`、`wiki-api.json`、`wiki-examples.json` |
| Browser native | 五组件官方生成；3 个 native processes；原子发布通过 | `browser-support.log` |
| Browser Mono 产品 | 正式发布；3,000 帧，6 views，93 draws；正常退出 | `game-mono-wasm.log`、`browser-mono-result.json` |
| Browser Wasm AOT 产品 | 正式发布；3,000 帧，6 views，94 draws；正常退出 | `game-mono-wasm-aot.log`、`browser-aot-result.json` |
| Windows CoreCLR 产品 | 正式发布；三图形 API 各 300 帧，exit 0 | `game-windows-coreclr.log`、`windows-coreclr-results.json` |
| Windows NativeAOT 产品 | 正式发布；三图形 API 各 300 帧，exit 0 | `game-windows-nativeaot.log`、`windows-nativeaot-results.json` |
| 包、源码冻结及仓库完整性 | 148 包文件未变；四个执行源码 / 配置文件哈希未变；其他仓库 clean | `package-baseline.json`、`frozen-source.json`、`integrity-final.json` |
| 修复身份复核 | 新 publication worker 与公共 4.1.2 一致 | `initial-worker-mix.json`、`private-worker-alignment.json` |

每次 Windows Player smoke 均输出 `INNO-SMOKE frames=300 views=2 draws=25 dispatches=0`，stderr 为空。Browser 实测 Edge 156.0.4314.42、NVIDIA GeForce RTX 3070 Ti Laptop GPU、ANGLE D3D11 / WebGL2。两种 Browser 路径均交付输入并观察到 best-score 存储读取；没有观察到新最高分写入。

439 个锁文件的语义 diff 仅涉及 BGCS 条目及 Project 条目中对应的 BGCS dependency 要求，没有其他包或项目依赖变化。196 个 Wiki 示例均已求值，errors 与 notEvaluated 均为 0。当前变更文档的 133 个本地链接检查通过；最终文档同步后另行执行架构复验，记录为 `architecture-final.log`。

### 可复验命令形状

以下从引擎根执行，`$dotnet` 是已冻结的 10.0.402 CLI，`$cli` 是本次 Build 输出 DLL，`$evidence` 是独立验收输出目录。工具环境在本次进程通过 vswhere 解析已安装 CMake/Ninja，不写入源码或用户环境。

```powershell
& $dotnet msbuild <native-owner.csproj> -t:GenerateBindings `
  -p:InnoBindingOutputMode=HostSource -p:InnoBindingTargetId=windows-x64 `
  -p:InnoBindingDotnetHost=$dotnet -p:Configuration=Release `
  -p:ArtifactsPath=$evidence/host-generation -m:1 -nr:false -v:minimal
# Use the same frozen inputs with -t:CheckBindings for all seven owners.

& $dotnet $cli support --engine-root . --tools-target windows-x64 `
  --target browser-wasm --configuration Release --product player --dotnet $dotnet

& $dotnet $cli game --engine-root . --tools-target windows-x64 --dotnet $dotnet `
  --project ../InnoEngine.Samples/FlappyBird --support-packs artifacts/support-packs `
  --target browser-wasm --deployment mono-wasm --output $evidence/games/mono-wasm
# Repeat for mono-wasm-aot; Windows uses target windows-x64 and coreclr / nativeaot.

& <published-player>/FlappyBird.exe --smoke-frames 300 `
  --graphics-api d3d11 --window-visible false
# Repeat d3d12 and vulkan for both Windows deployment products.
```

Browser 使用验收目录中的 `run-browser-product.cjs`、自身 localhost server 和独立 headless Edge。没有 computer use，没有控制用户 Rider 或个人浏览器。

## 失败、延期及未覆盖范围

- 初始 HostSource 混装失败原日志保留为 `generate-Inno.Native.Bgfx.log`，不覆盖为通过。
- 第一轮 Browser Mono harness 服务未正确提供 Content-Length / Range，Content.pack 请求中止。只修正验收 HTTP 服务的 HEAD / Range / 206 / 416 协议后完整重跑；没有忽略 aborted requests、修改产品或豁免失败。原日志为 `initial-browser-mono*`，最终 HTTP 记录为 `http-mono.log`、`http-aot.log`。
- 第一次 Windows NativeAOT 命令误用 `native-aot`，注册表正确拒绝；使用实际注册 ID `nativeaot` 后正式发布与运行通过。`game-windows-native-aot.log` 保留为操作命令错误，不归责 SDK。
- 自动维护多次超出既有 5 秒 inventory 预算，按原协议延期且不退休产物。engine #15 仍开放；有一轮能完成 inventory 也不能证明每次都会收敛。
- Browser 每条产品路径仍观察到两条未归因的 console 404，页面 HTTP response 监听没有对应失败；engine #22 继续跟踪，不猜测为 favicon 或资产缺失。
- 最高分跨重启写入没有触发，Samples #1 继续跟踪。不能把存储读取或输入交付写成完整玩法 / 存储验收。
- macOS / Metal 无实机，未执行；上游 SDK CI 不能代替引擎 Mac 产品验收。Windows Editor Debug/Release、完整 DPI / 焦点 / 音频 / 长期运行没有在本轮重跑。
- 其他已登记的 History 原子性、插件 UI / 2D 热路径、热构建成本、测试 assets scope、卷大小写语义、XML 语义、multi-SDK references、native 源码编码、session coroutine admission 和锁 scope 归属问题仍按 [既有 issue 清单](PRIORITY_FIX_ACCEPTANCE_2026_10_25.md#已提交的后续-issues) 单独验收。本轮没有偷偷扩大范围实现它们。

## 深层自查

本次沿实际链路复核：公共包声明 → owned Restore → MSBuild 求值 → imports / content items → Task 请求身份 → 不可变 publication → 私有 SDK / worker → 官方生成 → native linking → Support Pack → managed publish → 实际游戏调用。

- **职责与解耦：** 包阶段问题留在中立 Task hosting；Browser 执行策略仍归 Browser integration / 官方扩展；共享 backend 只消费冻结机制。没有在根组合或中立领域增加目标名、盘符、包名判断。
- **失败与所有权：** SDK 身份不匹配明确失败；阶段变化形成新的完整请求身份，旧 publication 不能误作当前请求。生成仍在既有 staging / lease / integrity / atomic publication 协议内完成，未绕过失败、取消或 last-good 保护。
- **基础能力复用：** 继续使用既有 Core IO lease、Task build scope 和 SDK 公共 API，没有另造 Events、Identity、serialization、缓存管理器或生命周期协议。
- **扩展性：** 修复对所有 package imports 生效，新增 backend 或 SDK 内容文件不需要加复制名单或共享执行器分支。没有新增 public 配置要求消费者重复声明 SDK 事实。
- **整洁与规范：** 不含新增 global / implicit using、MSBuild Using、legacy reader、schema compatibility、SDK 源码依赖、手工生成补丁或测试后门。issue 英文标题 / 正文规范与深层自查要求均已记录在引擎 AGENTS.md。

修复范围达到可解释且有真实消费证据的边界；开放 issues 和未执行设备矩阵明确保留。不能用本次通过宣称全部代码已无严重问题或整个引擎绝对完美。

提交使用当前分支、Aaron Liao 的 author / committer，不添加 co-author，不推送。Windows 无 `/System/Library/Sounds/Glass.aiff`，未播放提示音。
