# 三项优先修复与专项验收报告

日期：2026-10-25；平台：Windows x64；分支：`dotnet10`。

## 结论与边界

本轮处理前次审计的三个 P1：F01 Browser Mono UInt64 调用、F02 Coroutine 退休、F08 Coroutine 重入停止。三个 P1 的引擎修复均已完成专项验收。Browser 采用官方 LibraryImport；解释执行与 AOT 的示例产品都已发布并各实际运行 3,000 帧、有序退出。SDK 自身的两项问题独立保留，没有修改 SDK 来掩盖它们。

本报告不把其余问题当作已修复，也不宣称完整平台矩阵或整个引擎已完美。历史全仓审计保留于 [FULL_ENGINE_AUDIT_2026_10_24.md](FULL_ENGINE_AUDIT_2026_10_24.md)，本报告更新其中三项状态。

## F02 / F08：在唯一 Core owner 中修复

- 正常耗尽、handle stop、owner stop、stop-all、执行失败和 scheduler Dispose 统一退休枚举器栈，子到父释放。
- 释放前先弹栈；一个 Dispose 失败仍继续尝试其他 eligible 清理。未推进过的 pending 枚举器也会被 Dispose；尚未进入的 C# iterator finally 不会被错误宣称已执行。
- `MoveNext`、`Current`、wait predicate 和清理异常在最外层操作完成 eligible 清理后以 `AggregateException` 传播，不再静默吞掉异常，也不把失败留在 scheduler 字段中。
- 退休先失效 handle、移除状态并清除 waiter；正在执行的枚举器在回调返回后释放。活动列表保留稳定槽位，整帧结束统一压缩，避免旧索引再次删除其他协程。
- 回调可提交开始/停止请求；递归 Tick 和 yield 自己/祖先被明确拒绝。枚举器成功交接后由 scheduler 独占，调用者不得复用或自行释放。
- 快速等待与枚举器推进分成两个阶段；没有退休时不扫描压缩列表。没有增加另一个 scheduler、事件总线、identity registry 或 public 测试入口。

## F01：平台选择官方 LibraryImport

独立 C 函数接受一个指针并返回 `0xfedcba9876543210`。使用 BGCS 4.1.1 公开 `GenerationRequest.FromFile` 和官方 Browser plugin，分别生成两套未经手改的绑定：

| 策略 | 生成 / 发布 | 实际 Mono 解释执行 |
| --- | --- | --- |
| FunctionTable | 通过 | 复现 `aot-runtime-wasm.c:188` 断言，作为负面对照保留 |
| LibraryImport | 通过 | UInt64 精确返回，通过 |

无 BGCS 的 calli / DllImport 对照也能复现同一差异，因此不能把它重新归咎匿名 union 或 BGCS 核心解析器。官方 Browser 扩展对 FunctionTable 执行能力的验证缺口已单独提交 SDK issue。

生产修复的真实调用链：

1. Browser integration 的 `NativeBindingProfile` 明确选择 `BindingImportMode.LibraryImport`。
2. 有效配置在分析和指纹计算前冻结；namespace、组件语义和 ABI 仍由现有组件 / SDK 契约提供。
3. 生成描述符记录实际 `usesRuntimeImports`，与 Static/Shared 链接独立。
4. 产品 selection 冻结这两个事实，发布继续比较完整 selection。
5. 无 selection 的独立构建通过 `GenerateBindingsTask.UsesRuntimeImports` 取得同一事实；有 selection 的快速复用路径直接消费冻结值。
6. 共同编译 target 在 CoreCompile 前应用机制开关并拒绝缺失 / 冲突输入。
7. 各 Native owner 按真实机制调用官方 `Create`、`FromStatic` 或 `Load`，没有平台名判断，没有 UInt64 特判。

没有添加假 import 补签名表，没有手写某个 LI trampoline，没有改音频函数返回类型，没有静默改为仅 AOT，没有修改 BGCS / extern / 生成文件来绕过问题。

### 必要的构建 API 调整

| API | 必要性与稳定语义 |
| --- | --- |
| `NativeBindingProfile.importMode` 与对应构造参数 | BGCS 接入边界内的显式平台选择；null 保留组件机制，实际值进入有效配置 / 指纹 |
| `NativeBindingGenerationDescriptor.usesRuntimeImports` | 中立、required 的生成事实；区分 runtime import 初始化和 provider 初始化，独立于链接方式 |
| `GenerateBindingsTask.UsesRuntimeImports` | 让独立 MSBuild 构建消费生成结果，不能仅依赖产品 selection 的提前求值 |

没有新增或修改游戏领域 API、Stable ID、持久化内容形状或脚本导出。

## 最终源码与文件归属

```text
AGENTS.md                                      # section 31: English issues and title policy
Directory.Build.targets                        # generation result / selected fact -> CoreCompile
src/foundation/core/Inno.Core.Coroutines/
  CoroutineScheduler.cs                        # execution, retirement and reentrancy
build/bindings/Inno.Build.Bindings/
  Configuration/NativeBindingProfile.cs         # official import choice before freezing
  Generation/BindingGenerationOperation.cs      # report actual generation facts
  Generation/TargetBindingGeneration.cs
build/toolchains/Inno.Build.Toolchains/Native/
  NativeBindingGenerationDescriptor.cs          # independent import and linkage selection
build/tasks/Inno.Build.Tasks/
  GenerateBindingsTask.cs                      # thin output mapping
build/msbuild/Inno.Build.Tasks.targets           # isolate product facts from compiler tooling
platforms/BrowserWasm32/integrations/
  Inno.Integration.BrowserWasm32.Build/Bindings/
    BrowserWasm32BindingProfile.cs              # the only Browser import policy
backends/<component>/native/Inno.Native.*/
  <handwritten module initializer>.cs          # seven mechanism-based initializers
  Generated/Bindings.cs                        # official SDK output, never hand-edited
  Native/Generated/                            # official SDK output, never hand-edited
tests/core/Inno.Core.Coroutines.Tests/
  CoroutineRetirementTests.cs                   # public-boundary regressions
tests/build/Inno.Build.Tests/
  NativeBindingGenerationTests.cs              # mode identity and linkage independence
  <affected existing descriptor fixtures>.cs
docs/core/Inno.Core.Coroutines.md
docs/build/Inno.Build.{Bindings,Toolchains,Tasks}.md
docs/platform/BrowserWasm32/Inno.Integration.BrowserWasm32.Build.md
records/architecture/acceptance/
  PRIORITY_FIX_ACCEPTANCE_2026_10_25.md
```

没有新增项目、入口或解决方案分组；普通 ProjectReference 和 SDK/MSBuild 流程保留。

## 测试与证据

证据根为 [`artifacts/acceptance/priority-fixes-2026-10-25`](../../../artifacts/acceptance/priority-fixes-2026-10-25)。所有结果都保留原始日志 / TRX / binlog，不覆盖失败记录。

| 检查 | 结果 | 证据 |
| --- | --- | --- |
| Coroutine Debug | 40 通过，0 失败 / 跳过 | `core-final-Debug.log`、`core-final-Debug/*.trx` |
| Coroutine Release | 40 通过，0 失败 / 跳过 | `core-final-Release.log`、`core-final-Release/*.trx` |
| Runtime 回归 | 62 通过，0 失败 / 跳过 | `runtime-tests-final.log`、`runtime-tests-final/*.trx` |
| 构建 / binding 契约 | 43 通过，0 失败 / 跳过 | `build-tests-final.log`、`build-tests-final/*.trx` |
| 架构 / XML / Wiki | 238/238 工程通过，33,567 API symbols，196 示例 | `wiki-final.log`、`wiki-api-final.json`、`wiki-examples-final.json` |
| 正式 Build 入口 | 零警告、零错误 | `cli.log`、`cli-final.log`、`cli.binlog` |
| 独立 Browser MiniAudio 构建 | 零警告、零错误；未借助产品 selection | `standalone-final.log`、`standalone-final.binlog` |
| Browser 五组件生成 / 原生产品 | 正式生成、538 步 native 编译、原子发布通过 | `browser-support.log` |
| FlappyBird Mono 发布 | 通过 | `game-mono.log` |
| FlappyBird Mono 实测 | 3,000 帧，6 views，94 draws；输入、存储读取、正常退出 | `browser-mono-result.json`、`browser-mono-network-result.json` |
| FlappyBird AOT 发布 | 通过 | `game-aot.log` |
| FlappyBird AOT 实测 | 3,000 帧，6 views，94 draws；输入、存储读取、正常退出 | `browser-aot-result.json` |
| 源码冻结 | 25 个已变更源码 / build 文件哈希未改变 | `frozen-source-hashes.json`、`validation-summary.json` |
| BGCS 完整性 | 148 个包文件未改变；BGCS 仓库 clean；Agent 文件 0 | `bgcs-integrity-final.json` |

专项测试合计 **185 通过，0 失败，0 跳过**；这不是把前次全仓 2,000 多项结果重新计作本轮运行。真实公开契约 / 可替换边界测试，没有 IVT、私有反射或测试后门。BGCS 专项没有整体排除。

Runtime 的 62 项仅证明相关回归没有新增失败，不证明私有 session scheduler 已拥有实际 admission 路径；深查确认该缺口并提交 R02。

### 原始失败与复验

- 首轮构建契约为 42 通过 / 1 失败：测试运行中源码变化，被 Task-runtime 输入冻结正确拒绝；固定源码后同范围 43/43 通过。没有修改校验或把失败改成成功。
- 首轮 Wiki 发现新增公开签名的 5 个映射缺口；补齐当前文档后完整 gate 通过。
- 独立 Browser 项目首轮缺 CMake，工具预检明确失败；按当前工具要求，在验收进程用 vswhere 定位已安装的 CMake/Ninja 后通过。没有写死安装路径或改 SDK。
- Shader 工具构建存在上游 MSVC 编码 / unreachable / 临时路径告警，未修改 extern 或全局屏蔽告警；源码编码问题单独提交 B01。
- 自动 cache inventory 仍会超过 5 秒预算；按既有策略单独报告延期，不把维护延期改写为成功，F07 继续开放。

### 性能对照的实际范围

同 SDK / Release / 输入，固定 `DOTNET_TieredCompilation=0`，各 5 次，1,000 个等待协程、每次 1,000 Tick：baseline 中位 13.0346 ms，当前 11.8309 ms，所有轮次分配均为 0。仅说明该等待路径没有测到回退，不宣称整体引擎、帧时间或所有协程模式提速。

早期默认 tiering 的混合阶段数据单独保留，不用不一致优化阶段作性能结论。证据：`coroutine-perf.log`、`coroutine-perf-final.log`、`validation-summary.json`。

## 已提交的后续 issues

标题全部采用 `type(scope): description`，标题和正文全英文；检索去重后分别提交到实际 owner 仓库。已复现缺陷与待验证风险在正文明确区分，并给出验收条件。

| ID | 工作 | 链接 |
| --- | --- | --- |
| F03 | History transaction rejection | [issue](https://github.com/Wolfiora/InnoEngine/issues/13) |
| F04 | Immutable Canvas texture upload reuse | [issue](https://github.com/Wolfiora/InnoEngine.Canvas/issues/1) |
| F05 | Stable Rendering2D geometry reuse | [issue](https://github.com/Wolfiora/InnoEngine.Rendering2D/issues/1) |
| F06 | Root-build native preparation reuse | [issue](https://github.com/Wolfiora/InnoEngine/issues/14) |
| F07 | Bounded cache maintenance | [issue](https://github.com/Wolfiora/InnoEngine/issues/15) |
| F09 | Evaluated test assets scope | [issue](https://github.com/Wolfiora/InnoEngine/issues/16) |
| U07 | Filesystem case semantics validation | [issue](https://github.com/Wolfiora/InnoEngine/issues/17) |
| O06 | Public documentation semantics | [issue](https://github.com/Wolfiora/InnoEngine/issues/18) |
| R01 | Multi-SDK framework reference consistency | [issue](https://github.com/Wolfiora/InnoEngine/issues/19) |
| S01 | Browser FunctionTable execution support | [issue](https://github.com/FLwolfy/BindGen-CS/issues/1) |
| S02 | Browser target-provider cache fingerprint | [issue](https://github.com/FLwolfy/BindGen-CS/issues/2) |
| B01 | Explicit Windows native source encoding | [issue](https://github.com/Wolfiora/InnoEngine/issues/20) |
| R02 | Unused session-owned coroutine integration | [issue](https://github.com/Wolfiora/InnoEngine/issues/21) |
| T01 | test(browser): attribute nonfatal resource diagnostics in player smoke runs | [issue](https://github.com/Wolfiora/InnoEngine/issues/22) |
| T02 | test(gameplay): verify best-score persistence across browser restarts | [issue](https://github.com/Wolfiora/InnoEngine.Samples/issues/1) |
| B02 | build(restore): reconcile redundant target and host RID lock scopes | [issue](https://github.com/Wolfiora/InnoEngine/issues/23) |

R02 是本轮深查新增的实际集成缺口；没有通过扩大本轮公开运行 API 或再造系统掩盖它。S01 是官方 FunctionTable 执行支持 / 验证缺口，当前引擎已选择被正面对照验证的官方 LibraryImport。S02 的安全禁用 SDK cache 保留，未擅自补丁。

## 真实验收边界

- 实测设备：Windows x64，Edge 156.0.4314.42，NVIDIA GeForce RTX 3070 Ti Laptop GPU，ANGLE D3D11 / WebGL 2，1280×720。自身测试服务与 headless 浏览器在结束后关闭；未操作用户 Rider 或个人浏览器。
- 产品仅做有界帧运行和所记录输入。观察到 best-score 存储读取；未触发新的最高分写入，不把它报告为分数跨启动持久化成功。
- 两条非致命 console 404 诊断已提交 T01，分数跨启动写入覆盖提交 T02；加测页面 HTTP response 未捕获对应失败，不擅自归因为资产缺失或 favicon。完整资源诊断仍需进一步归因。
- macOS / Metal 没有设备，未执行。Windows 全 D3D11/D3D12/Vulkan、Editor Debug/Release、CoreCLR/NativeAOT 和全部设备视觉 / DPI / 音频矩阵没有在本轮重跑。
- 没有把前次 Windows 矩阵或独立 fixture 当作当前完整矩阵通过，也没有因 AOT 通过删除解释执行路径。
- 多余 target/RID 锁 scope 的证明与收敛提交 B02；未根据历史 inventory 删除当前可能已被新产品使用的文件。

## 深层自查与完成状态

已沿 profile → 官方生成 → descriptor → selection / 独立 Task → CoreCompile → 发布校验 → 实际 Native 调用检查输入和失败边界；也检查了 Coroutine 的命令 drain、回调重入、栈所有权、清理失败和 owner / waiter 引用释放。架构判断依据真实执行链，不依据目录或接口数量。

平台策略留在 Browser integration，中立层只传递冻结事实，共享 backend 不识别目标名称。Core 修复复用现有 scheduler / BCL 资源协议，没有另建 Events、Identity、缓存服务或生命周期管理器。无 global / implicit using、MSBuild Using、legacy reader、schema compatibility 或生成物手工补丁。

本轮修复链达到可解释、可验证的职责与失败边界；整个引擎仍有上述 issues 和未执行的产品矩阵，不能称为无遗留或绝对完美。

BGCS / Canvas / Rendering2D / Samples 工作树保持原状；本轮源码和规范改动属于引擎。已有引擎改动保留并按其内容审查，未将未使用的额外还原 scope 锁文件盲目加入提交。

提示音：当前 Windows 无 `/System/Library/Sounds/Glass.aiff`，未播放。
