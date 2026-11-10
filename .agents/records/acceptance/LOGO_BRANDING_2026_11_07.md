# Logo、默认字体图标与署名验收

[获批计划](../plans/LOGO_BRANDING.md) · [品牌资源](../../../assets/branding/README.md)

## 2026-11-10 字体迁移与合并后 CI 复查

按用户要求在 `logo` 分支先快进到已合并的 master `9d336f486076c8fa9e903c9ce7f1b5b6a414584f`，先完成字体迁移，再修复 CI。8 份字体及已有来源和许可原样迁入 `assets/fonts`，由 ImGui SDL3 后端的 MSBuild 链接维持产品内 `BuiltIn/Fonts`、`BuiltIn/Icons` 路径；源码与实际构建部署的 8 份字体哈希逐项相同。资源仍由后端拥有，没有 Core、平台中立接口或源码目录 fallback。

字体阶段提交为 `e39e2b9c`。统一计划 `artifacts/validation/font-assets-plan/plan.json` 和结果 `font-assets-run/results.json` 已完成：47 项必需检查全部 Passed、60 项明确 NotApplicable，无 Failed 或 Blocked。输入 tree 为 `e178fe8b4a33d3e425e5b0bee55cd1a2cd8fcdf1488b7058c3958c469e6b4051`，环境身份为 `5ca4b34ee8d4f0647f6769ee16a0a67c0b04bd2f37bf9a0f265f230fd7363a60`，SDK 10.0.402。包括编译、API、Wiki、95 项 ImGui、433 项 PlayMode、99 项 Presentation、260 项 Scripting 必需用例和真实 Editor 启动／工作流／100 轮面板旅程；产品检查耗时 128.55 秒。自动缓存维护因时间预算推迟，没有退休产物；不将此范围冒充全量 Release 或 macOS 通过。

合并前 [PR CI](https://github.com/Wolfiora/InnoEngine/actions/runs/42740936596) 与合并后 [master CI](https://github.com/Wolfiora/InnoEngine/actions/runs/42768759341) 的 Git tree 相同。后者 Windows 通过，macOS 首次失败为 `editor-products` 的面板内存观察：最终 `GC.GetTotalMemory(false)` 比基线增加 5,442,096 字节，超过 4 MiB；后面的 required-evidence 错误和 Release 等阻塞是该失败的后果。两轮 macOS 原始 TSV 都有上涨后回落的序列；成功轮中途同样越过门槛，场景、历史、选择和事件计数稳定。

本机 .NET 10.0.13 的独立固定对象／堆碎片复现进一步证明旧估算值不能作为存活内存：释放 3,991,008 字节固定缓冲区后，同一完整 GC 快照的存活内存下降 3,993,888 字节，估算值反而上升 4,623,464 字节，足以误报 4 MiB 超限；增加压缩 GC 不能修复这个估算 API。原始复现和回归输出位于 `artifacts/branding-work/gc-observation-large.log`、`gc-snapshot-regression.log`、`product-journey-memory.log`。Mac 历史 artifact 的原始 ZIP 与 TSV 保留于 `artifacts/validation/macos-panel-retention`，不以仅重跑变绿解释问题。

修复将采样留在普通 Editor 验收源码中，以同一完整 GC 快照的 `HeapSizeBytes - FragmentedBytes` 表示存活字节，保留估算值用于诊断。驱动核对 GC 身份、最高代、压缩／并发状态及字节一致性，原有 4 MiB、循环数、历史、事件、退出和退休断言全部保留。隔离回归编译真实采样文件，验证固定对象和回收、实际 8 MiB 保留，以及 25 种产品证据成功／失败协议，全部通过。

CI 协议检查现在把旅程驱动及共享夹具纳入选择和证据身份；只改共享采样代码也不能复用旧 CI 协议结果。两个真实 SDK／Git 计划回归通过，日志为 `artifacts/branding-work/journey-input-regression.log`。使用已验收且生产输入未变的 Editor 产物，实际编译新旅程源码并执行 100 轮面板开关，11 轮完整采样、4 MiB 门槛和正常退出均通过，最终存活内存增长 730,272 字节；结果为 `artifacts/validation/font-ci-lifecycle/result.json` 和同目录 `Project/Library/Acceptance`。源码架构、文档结构／链接和 agent 记录检查通过。新的全量计划需要覆盖 Windows 和 macOS、Debug／Release 产品及打包，最终由 `logo` 到 `master` 的 PR 所关联 Engine CI 对确切提交执行；上述定向结果不替代远端全量证据。原始失败保留，不循环重跑旧提交直到绿色。

## 2026-11-10 复查发现：暖构建运行时依赖部署

最终复核 `logo-alias-fixture-final-run` 的规则、文档、全部所选测试、Debug 产品、Release 编译和编译 API 均通过，但 Release smoke 失败，不能认证全量成功。首次产品日志报告缺少 `Inno.Editor.Annotations.dll`；依赖清单核对同时发现 `Inno.Editor.Diagnostics.dll` 缺失，后续 Support Pack、NativeAOT restore、发行归档被正确阻塞。日志保存在 `artifacts/validation/logo-alias-fixture-final-run/editor-release-products/Source/Logs/`。

问题位于先前 #76 的已验证依赖复用与既有运行时闭包导出的交界：依赖仅执行 `GetTargetPath`，没有设置 SDK 的 `BuildingProject`；单独执行 `ResolveReferences` 仍不足，SDK 因此关闭传递依赖查找。实际平台选择和依赖方向没有越层，但暖构建产品不完整，属于必须修复的构建契约问题。最终修复在 `Directory.Build.targets` 的原有 `ExposeInnoHostRuntimeClosure` 中依次执行 SDK 的 `BuildOnlySettings;ResolveReferences;GenerateBuildDependencyFile`，不重新编译未选中依赖，不复制文件清单、不增加平台条件或运行时 fallback。

最小四项目复现通过正常冷构建、仅清理入口、暖构建、实际进程加载证实：修改前冷构建可运行，暖构建缺少私有传递依赖；修改后两次均成功。首次缩减 fixture 未引用叶子类型，不能代表原失败；修正为实现中引用叶子类型而公开 API 不暴露该类型后复现成立。原始输出为 `artifacts/branding-work/warm-deployment-before.log`、`warm-deployment-after.log`。正式回归 `ValidationRuntimeClosureTests` 同时核对叶子程序集字节与依赖 Build 执行记录，防止用重复编译掩盖问题。

最终统一计划与结果为 `artifacts/validation/logo-warm-closure-plan/plan.json`、`logo-warm-closure-run/results.json`，日志为 `artifacts/branding-work/logo-warm-closure-run.log`。执行已完成，退出码 0、`completed=true`：328 项 Passed、1 项 Reused（规则检查输入完全一致）、3 项 NotApplicable，无 Failed 或 Blocked。329 项必需检查均有有效成功证据。输入 tree 为 `2cd1e7aa83b15973c33772fe389bd13ed05e88af0e6be94aa3c8cf426579a971`，基线为 `6024657d748688a0e2a43794c34ffa0339d6c8fe`，SDK 为 10.0.402，环境身份为 `5ca4b34ee8d4f0647f6769ee16a0a67c0b04bd2f37bf9a0f265f230fd7363a60`。

- 源码架构、规则／CI 契约、文档、managed/native/release 编译 API、所选测试与 Debug/Release Editor 产品检查均通过。Validation 66 项、Build 357 项、NativePerformance 6 项、Editor Performance 15 项、PlayMode 433 项、Presentation 99 项、Scripting 260 项必需用例通过；宿主专属用例另由 `host-contracts.json` 核对，不计为当前宿主通过。
- 10 轮热构建通过，中位耗时 174669.749 ms，原生编译进程数和绑定生成次数均为 0。Release smoke、工作流与生命周期检查通过，未重现缺失程序集；Support Pack 独立源码构建、NativeAOT restore、发行归档均通过。NativeAOT restore 不代表完整 NativeAOT 发布。
- 3 项产品排除为 BrowserWasm32 Player、MacOSArm64 Editor 和 Player；不声明其他平台实机通过。自动缓存维护因扫描时间预算被推迟，没有退休产物，不影响构建成功。
- 完成后仅补写本段、计划状态与开放记录，不改变受验代码、项目、规则或使用文档；这些记录收尾单独执行 agent 文档结构／链接检查与 `git diff --check`，不以记录修改或提交为由重复产品验收。首次失败和下文旧输入证据保留，不替代本轮结论。

## 后续修订：局部别名与限定名规范

2026-11-10 接续时先 fetch，并将工作区快进到 `origin/master` 的 `6024657d`，再建立 `codex/icon-alias-fixture`。远端相对原基线 `21948bcd` 只有 PR #77 合并提交，两者 Git tree 相同，没有需要覆盖或移植的他人源码修改；原有 12 个未提交文件完整保留。

Editor 图标类的 44 个后端常量引用统一使用文件级 `BackendIcon` 别名，解决 `Inno` 字段遮蔽根命名空间的问题；调用方仍直接使用 `ImGuiIcon.Inno`，公开名称、码点、字体资源和加载流程不变。coding 的 using 规则新增手写 C# 禁用 `global::` 的约束，保留生成代码、生成模板及其输出验证的协议边界。

Editor 项目本轮正常构建通过，0 警告、0 错误；新程序集的全部 44 个公开图标常量与改动前名称、值完全相同。日志为 `artifacts/branding-work/alias-build.log`，比较输入和结果为 `alias-before.json`、`alias-after.json`。相关 API 源码链接同步到实际行号。

用户随后要求一并修复原生 Editor 支持库分类：显式 `IsTestProject=false`，将仅用于断言的 `xunit` 依赖收窄为 `xunit.assert`；PlayMode runner 的 contract 层、Performance runner 的 performance 层及二者原生产品准备声明不变。规则加载器仍核对完整历史原文，新增条款通过登记批准依据及独立正文哈希追加；首次直接插入条款被原文保护拒绝，改用该机制，不删除或放宽历史检查。隔离回归覆盖追加内容篡改、标记缺失、批准依据缺失及原文删改。

### 完整 Windows 验收与证据复用

上次后台统一执行已完成：`artifacts/validation/logo-alias-fixture-plan/plan.json` 对应的 `logo-alias-fixture-run/results.json` 为 `completed=true`，326 项 Passed、5 项 NotApplicable，无 Failed 或 Blocked。输入 tree 为 `2dfbdad8fee65a65101f91bdce968f0165e86d1cc48dcf69d36a388733e227ee`，SDK 为 10.0.402，环境身份为 `5ca4b34ee8d4f0647f6769ee16a0a67c0b04bd2f37bf9a0f265f230fd7363a60`。

- 源码架构、规则／CI 契约、文档、managed/native/release 编译 API 通过；原 `compiled-api:native` 支持库分类阻塞已消除。
- PlayMode 433 项、Editor Performance 15 项、Validation 65 项必需用例通过；Debug/Release Editor smoke、工作流和生命周期旅程通过；Release 编译、Support Pack、NativeAOT restore 和发行归档通过。NativeAOT restore 不等于完整 NativeAOT 发布。
- 5 项明确排除为 Browser 产品、两个 macOS 产品，以及本轮未选择 extended 的 NativePerformance 构建与测试。没有把其他平台实机、扩展冷／热缓存或长期人工体验标为通过。
- 同步 master 未改变产品源码；本轮只规范登记缩进并补写记录。使用 `logo-alias-fixture-final-plan/plan.json` 和 `logo-alias-fixture-final-run/results.json` 重新计算当前输入，并由统一 runner 核对旧证据的工具、环境、依赖及输出，补跑失效检查。日志为 `artifacts/branding-work/alias-fixture-final-run.log`；最终状态以该结果为准。

### 已合入 PR 的平台边界复查

复查最近已合入的 #76（`1294d148`）与 #77（`6024657d`）相关边界，没有发现本范围需要阻断交付的平台反向依赖：

- `ValidationPlanner` 从正式 Editor 入口的 SDK 求值读取目标和平台产品 props；`ValidationRunner` 读取平台声明的 `InnoValidationBackendPattern`，缺失即失败。Direct3D／Metal 预期分别由 Windows／macOS owner 声明，未移入领域层。
- `Inno.Tooling.Validation` 只引用 Core IO、中立 Toolchains 和 Architecture。宿主环境指纹、测试宿主适用性、归档保留 Unix 权限属于验证工具的执行环境处理，没有据此在中立运行时选择平台 backend。
- 原生准备复用仍通过既有 Core `FileLease`、构建期登记及独立产物读取租约完成；复用前重验输入，失败释放已取得的租约，未新增平行资源管理器。Logo 资源仍归 backend 字体 owner，Editor 只公开字符串常量，Core/SPI 未引入品牌数据。
- 本轮不改变 UI 状态所有权、交互、Undo/Redo、菜单或字形；API、组织、抽象与复用保持原边界。以上是相关链路审查与现有架构门禁的结论，不是对两个大型 PR 的所有历史行为作无缺陷保证。

## 后续修订：Inno 命名与 README 尺寸

基线 `e3b3143e`，分支 `logo`。将两个公开图标常量改为 `ImGuiIcon.Inno`，同步字体测试、使用示例和 API 映射；目录从公开常量自动取得名称 `Inno`，不保留旧别名。Editor 常量引用使用 `global::` 避免与根命名空间同名。README 的 Logo 宽度改为 240，居中与替代文本保留。按用户最后确认，只删除根 README 的 Logo 设计致谢；品牌说明、代码注释和 TTF 作者元数据保留。字体、PNG、SVG 内容没有变化。

本轮采用同一显式 SDK 10.0.402 与原生构建上下文。当前通过：

- Editor ImGui 测试工程及其实际引用闭包正常构建，0 警告、0 错误，保留原生准备和部署核验：`artifacts/branding-work/inno-build.log`。
- 改名后的 FontCoverageTests 9 项通过、0 失败、0 跳过：`artifacts/validation/logo-inno-focused/fonts/fonts.trx`。
- 两个生产 owner 的编译 API 检查、全仓源码架构和最终文档结构检查通过：`artifacts/branding-work/inno-api.log`、`inno-source.log`、`inno-documentation-final.log`。
- 文档工具及其闭包正常构建，0 警告、0 错误；使用本轮生成的逻辑脚本引用核验两个相关项目页，API 映射和全部示例通过：`artifacts/branding-work/inno-cli-build-retry.log`、`inno-wiki-projection.log`、`inno-wiki.log`。清单位于 `artifacts/validation/logo-inno-focused/wiki-api.json`、`wiki-examples.json`。
- 根 README 不再包含作者致谢，品牌说明、代码注释、字体 name 表的 designer 和 URL 仍保留；TTF 和全部图片资源相对基线无差异。

统一计划为 `artifacts/validation/logo-inno-plan-final/plan.json`（35 个受影响 owner、47 个必需检查）。当前 SDK 元数据预检仍复现既有原生 fixture 执行层分类违规，见 `artifacts/branding-work/inno-fixture-preflight.log`；没有执行或声称通过整个统一计划、完整产品旅程和其他平台矩阵。一次文档查询命令参数写法错误，改用 `docs symbol --project ... --id ...` 后定位正确页面；首次 CLI 准备在用户调整署名范围时取消，后续正常重建，不将取消的执行记为通过。

最终文件输入清单位于 `artifacts/validation/logo-inno-focused/input-manifest.json`。收尾只补写记录与 README 致谢，不改变已验证的生产代码、测试或字体输入。架构、owner、加载方式、状态和生命周期均延续原实现；此次只改变公开常量与目录名称、README 展示，未修改交互、Undo/Redo 或菜单流程。下文旧证据不作为重命名后的编译证据。

## 当前修订：空心字体与移除内置原图

- 基线为 `402a191e`，分支 `logo`。用户确认四块各自描边的预览，并要求替换为其提供的 v2 SVG、彻底删除内置原图能力。
- 正式 TTF 与已确认预览字节相同，U+E000、度量、作者署名和合并入口不变；字形由八条内外轮廓组成，四块内部透明。
- 正式 SVG 与用户提供文件字节相同；512px 透明 PNG 从新 SVG 导出，仅保存在品牌目录。默认组合的图片 API、资源链接、专用测试和测试引用已删除，产品文档同步清理。
- SVG SHA-256：`62f84df21f90b626db5aef502cb064645c05d3bcd92a6d1fc152cf3ef0ba55ed`。
- TTF SHA-256：`143dce4e0ca463e7a71d647fd52d607e34e56068089dfb70620c105124ca5cc3`。
- SDK 显式使用 `C:/Users/23842/.dotnet/dotnet.exe` 10.0.402；没有修改全局 SDK 配置。实现后的记录收尾于 2026-11-08；以下专项证据属于本轮输入。

### 本轮实际证据

| 检查 | 结果与证据 |
| --- | --- |
| 正式资源 | TTF 与批准预览、新 SVG 与提供文件逐字节一致。八条内外轮廓方向相反，四块内部采样透明，SVG 精确复现 TTF 轮廓；PNG 为 512×512 RGBA，可见边界 (34,16)…(478,496)，中央接缝 57 个采样 alpha 均为 255。`artifacts/branding-work/outline-checks.json`。 |
| 视觉 | 批准的空心字体预览保存在 `artifacts/branding-work/outline-preview/icon-preview.png`；新灰阶 PNG 深浅背景检查见 `artifacts/branding-work/logo-v2-review.png`。 |
| 原生专项编译 | 正常构建默认组合、Editor ImGui 测试及 Build 工具依赖闭包，没有跳过原生准备或项目引用；0 警告、0 错误。`artifacts/branding-work/outline-focused-build.log`。 |
| 字体回归 | 当前部署的 TTF 哈希与批准字体一致；FontCoverageTests 9 项通过、0 失败、0 跳过，覆盖四种样式、缩放、CJK、既有图标、字体源共享和 Context 生命周期。`artifacts/validation/logo-outline-focused/fonts/font-coverage.trx`。 |
| 运行时回归 | 移除专用图片测试与额外引用后重新构建，0 警告、0 错误；剩余 62 项通过、0 失败、0 跳过。`artifacts/branding-work/outline-runtime-build.log`、`artifacts/validation/logo-outline-focused/runtime/runtime.trx`。 |
| 删除完整性 | 源码、项目文件、测试和当前使用文档没有被删 API 或资源名的引用。对新编译默认组合的 PE 元数据检查 21 个类型，无品牌类型，资源表为空。`artifacts/validation/logo-outline-focused/default-assembly.json`。 |
| 编译 API | 默认组合、ImGui SDL3 backend、Editor ImGui 三个 owner 检查通过。`artifacts/branding-work/outline-api.log`。 |
| Wiki 契约 | 使用本次生成的 Windows 逻辑脚本引用检查三个相关项目页，API 映射和示例通过。`artifacts/branding-work/outline-wiki-retry.log`；清单位于 `artifacts/validation/logo-outline-focused/wiki-api.json`、`wiki-examples.json`。 |
| 最终源码与文档 | 249 个工程的源码架构检查、规则登记检查、产品文档结构与 agent 文档检查通过；`git diff --check` 通过。日志为 `outline-source-final.log`、`outline-rules-final-retry.log`、`outline-documentation-final.log`、`outline-agent-documents-final.log`（均在 `artifacts/branding-work`）。规则脚本首次多路径参数写法有误，改为 PowerShell 数组后通过，首次输出保留在 `outline-rules-final.log`。 |

PNG SHA-256：`2656f31f52669a51345928718b18d8bb94f2a239d5ee8e93f754e32359e60aa9`。默认组合 DLL SHA-256：`4ccaad2c86c753425d9d09070d61aa2482e50824c3e2a6293d940745c56db64b`。最终文件输入清单位于 `artifacts/validation/logo-outline-focused/input-manifest.json`；收尾只补写记录，未改变已验证的产品或测试输入。

### 本轮统一计划与未完成项

1. 正确工具生成的统一计划为 `artifacts/validation/logo-outline-plan-02/plan.json`；品牌文件未知 owner 触发保守全量选择（249 个 owner、324 个必需检查），没有手工改写计划。一次较早的运行误用了旧缓存 CLI，识别后终止，保留 `outline-run.log`，不作为证据。
2. 正确全量运行 `logo-outline-run-02` 执行过程中，独立的当前 SDK 元数据预检再次确认既有 fixture 问题：`Inno.Testing.Editor.Native` 为 `IsTestProject=true`，但 `InnoValidationLayer` 为空；预检退出 1，仅此一项违规。日志为 `artifacts/branding-work/outline-fixture-preflight.log`，对应 [现有问题](../open-items.md#原生-editor-测试支持库的执行层分类)。
3. 确认该全量门禁阻塞后终止尚在进行的全量原生构建；该运行 `completed:false`，不提供全量完成认证，也不将其中已观察到的通过数作为完成证据。上表来自随后独立完成的专项执行，不替代统一全量门禁。
4. 首次 Wiki 命令错误地从输出子目录寻找生成项目，缺少引用参数而失败；改为实际项目根目录读取引用后检查通过。首次日志 `outline-wiki.log` 保留。脚本投影成功，但自动缓存盘点超过预算而推迟；没有退休缓存，见 `outline-wiki-projection.log`。
5. 全量原生测试、产品旅程、Release、Support Pack、NativeAOT、发布归档及 macOS/其他设备没有本轮完成证据；未将既有环境阻塞记为通过。Windows 不存在 `/System/Library/Sounds/Glass.aiff`，未播放提示音。

### 本轮语义审查

品牌 SVG/PNG 只保留在仓库资源目录；默认组合删除图片 API、资源链接与专用测试，没有兼容入口、替代管理器或运行时图片依赖。字体沿用现有 loader、常量、码点和 EditorIconCatalog；生命周期由原有字体测试覆盖。空心图标精确采用用户批准预览，灰阶原稿采用用户新文件。交互、Undo/Redo 和菜单行为未修改，其全量旅程也未宣称通过。

以下均为初版历史证据，不代表本轮字形、资源或删除行为通过。

## 初版历史证据（已被上述范围取代）

## 范围与输入

- 分支 `logo`，基线 `1294d148f445f00df7e6a9dbf22de53a75b886fa`，工作区实现尚未提交。
- 完成品牌 SVG、512px 透明 PNG、单色源稿、U+E000 独立图标字体、README 展示与截图迁移、xdzzyq 署名、默认组合原图读取 API、字体回归和使用文档。
- SDK：`C:/Users/23842/.dotnet/dotnet.exe`，10.0.402，Windows x64。没有修改 global.json 或系统 PATH；验证子进程使用本次命令的 SDK PATH。
- 统一计划位于 `artifacts/validation/logo-plan/plan.json`。未归属生产工程的 SVG 与截图被计划器保守扩大为全量；没有手工缩减或改写计划。
- 本次执行源码快照为 `402e66aad4955278a2608b73fa9011c825e5096663081c4a427c24d115efd678`。记录补写与索引更新后仅复核文档结构；生产源码、测试、品牌资源未改变。最终文件身份另存于 `artifacts/validation/logo-focused/input-manifest.json`。

## 实际证据

| 检查 | 结果与证据 |
| --- | --- |
| PNG 与字体制作 | 512×512 RGBA；可见 alpha 边界 (34,16)…(478,496)；中心三个方向采样 alpha=255，无透底；TTF 为 512 units/em，ascender=459，descender=-75；全部默认字体 cmap 中 U+E000 原先空闲。`artifacts/branding-work/asset-checks.json`。 |
| 视觉检查 | 深浅背景，14/16/24/32px，100/125/150/200% 缩放预览；保留原灰阶、轮廓和镂空。`artifacts/branding-work/branding-review.png`。 |
| 截图迁移 | 原样移动至 assets/screenshots/editor.png；SHA-256 保持 `eee344a322fd0b8140f04089bd41c909bc9eafb1f529510cefe8df7e3284ec0d`。 |
| 全仓基础检查 | 规则、源码架构、agent 文档、产品文档结构、CI 协议检查通过。 |
| 托管编译与契约 | 编译 0 警告、0 错误；计划选择的托管 API、Wiki 示例与托管测试全部通过，包含验证工具自身的 65 项测试。 |
| PNG 读取 | EngineBrandingTests 两项通过：内容哈希匹配正式 PNG，无 Session 依赖，独立位置、独立释放、只读与重新打开成立。原始 TRX 位于 `artifacts/validation/logo-run-02/managed-trx`。 |
| 原生产品编译 | Windows Debug Editor 与计划选择的测试程序集构建成功，0 错误；1 条自动缓存盘点超时警告，未退休任何缓存。`artifacts/validation/logo-run-02/native-build.log`。 |
| 原生字体专项 | 使用上述已构建的 Inno.Editor.ImGui.Tests.dll 执行完整 FontCoverageTests：9 项通过、0 失败、0 跳过；覆盖四种样式、0.75/1/1.5/2 缩放、实际 Logo 与 CJK 字形、既有图标、共享补充字体源、Context 释放/重建。`artifacts/validation/logo-focused/fonts`。 |
| 受影响原生 API | 定向检查 Inno.Engine.Default、ImGui SDL3 backend 与 Editor ImGui 三个生产 owner，通过。`artifacts/branding-work/focused-api.log`。 |
| 受影响 Wiki | 使用当前 Windows 原生产物与本次生成的逻辑脚本引用验证三个项目页，API 映射与全部示例通过。`artifacts/validation/logo-focused/wiki-api.json`、`wiki-examples.json`。 |

PNG SHA-256：`5ac155bac41eb5ac99f157635b187fd27a84d8e6caf6f77733a748346c6ce2df`。
TTF SHA-256：`171afea1982bb6bb5326f4233bfa152a4d5118254116e75e8e45a01e2dc44cf3`。

## 首次失败、全量阻塞与未完成项

1. `logo-run-01` 的 CI 协议脚本从 PATH 取得旧 SDK，未找到 10.0.402；保留首次日志。`logo-run-02` 为本次命令及子进程设置正确 SDK PATH 后，该检查通过。
2. `logo-run-02` 全量结果为 295 项 Passed、1 项 Failed、28 项 Blocked、7 项 NotApplicable。失败是 `compiled-api:native`：现有支持库 `tests/editor/fixtures/Inno.Testing.Editor.Native/Inno.Testing.Editor.Native.csproj` 经 xUnit 包导入后 `IsTestProject=true`，但 `InnoValidationLayer` 为空。该工程本次未修改，诊断与 SDK 求值保留在 `native-sdk-graph.json` 与总日志。
3. 因上述前置门禁失败，全量原生 Wiki/测试、产品旅程、Release、Support Pack、NativeAOT restore 和发布归档没有取得通过证据。定向字体、API 与 Wiki 结果仅证明 Logo 任务范围，不代替全量门禁。
4. fixture 分类问题另记 [当前问题](../open-items.md#原生-editor-测试支持库的执行层分类)，由测试基础设施 owner 按支持库/runner 的实际职责修正；本次不改写无关测试基础设施。
5. macOS 产品与其他真实设备未验收。Windows 环境没有仓库约定的 `/System/Library/Sounds/Glass.aiff`，未播放提示音。

## 语义审查

| 方面 | 结论与边界 |
| --- | --- |
| 架构与组织 | 品牌源稿集中于 assets，字体归既有 backend，原图 API 归默认组合；未把品牌数据放入 Core 或中立 SPI。源码架构与三个受影响生产 API 检查通过。 |
| 状态与生命周期 | 原图 API 无缓存或全局可变状态，每次返回调用方所有流；字体继续由原生 atlas/Context 管理。读取契约和现有字体生命周期测试通过。 |
| 抽象、复用与颗粒度 | 仅增加一个小型资源读取入口，复用程序集资源、既有字体合并和 EditorIconCatalog；没有平行资源管理器、生成系统或绘制控件。 |
| 样式与视觉 | 灰阶图片与随文字颜色变化的单色字形各有明确用途；小尺寸和深浅背景预览已检查，未增加原设计没有的分面细缝。 |
| 扩展与使用 | 常量同步后端、Editor 和逻辑 API 所属类；公开 XML、项目页与示例通过实际编译检查。字体制作参数与码点保存在品牌说明。 |
| 交互、Undo/Redo 与菜单 | 本任务只增加资源和字形词汇，没有修改交互、History 或菜单行为；未将全量产品交互检查冒充通过。 |

功能实现与 Logo 专项验收完成；全量门禁仍被上述既有 fixture 分类问题阻塞。
