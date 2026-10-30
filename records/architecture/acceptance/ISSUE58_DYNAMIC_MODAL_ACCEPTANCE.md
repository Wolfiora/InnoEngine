# Issue #58：动态弹窗与局部 CI 验收

本报告覆盖 [#58](https://github.com/Wolfiora/InnoEngine/issues/58) 和直接关联的 [#59](https://github.com/Wolfiora/InnoEngine/issues/59)。SDK 为 10.0.402，目标为 .NET 10 / C# 14，BGCS 为 4.1.2。原始基线是 `dbae93e8f211d45a69cdffdb6afa3a33a0af0fa5`；该源码树与合并 PR #26 后的 master 相同。

当前状态：本地适用的功能、构建、原生回归、架构、Wiki、FlappyBird 工作流/退出与最终性能验收均已通过；远端 CI 尚待完成。Issue 保持 Open，不能在合并前关闭。

## 已确认原因与修复

旧 Renderer 的 `AlwaysAutoResize` 使用上一帧内容高度。实际原生基线轨迹证明，短文字变为五行时仍显示短窗口，随后收缩也会保留旧高度。它证明了切换帧错误；用户此前观察到的所有持续卡顿不能由这条轨迹一并解释。

当前链路是：工作流发布中立快照 → `OnPrepare` 捕获一次 → 按当前字体、宽度和尺度测量 → Renderer 决定本帧窗口边界 → 绘制一次 → 将携带操作身份的动作交回原 owner。

- Core 只增加中立的尺寸/准备契约，不引用 ImGui、导出或脚本实现。
- Interactions 继续负责扩展异常隔离；缺少测量的自适应窗口明确失败，不回退上一帧内容尺寸。
- ImGui 负责实际字体、边界、像素取整、换行和原生作用域；正宽度和零高度的部分尺寸声明得到保留。
- Dialog Widget 统一消息、详情、进度、操作行与一个正文滚动区。正文过长时按钮仍在正文滚动区外；不添加补偿空行，不隐藏实际溢出。
- Plugin Export 使用自身同一份字段声明测量和绘制；不把表单语义放进通用 Renderer。
- Script Reload、Sample Import 和 Export 的状态权威仍在各自工作流 owner。快照包含操作身份，旧操作、取消后和退休后的更新不能覆盖当前状态。
- Shader Check 的诊断发布和完成处理移至原 owner 的 Update，测量和绘制不执行编译或发布。
- 脚本编译的 80% 明确表示等待激活、状态恢复和退休验证。Full GC、finalizer、弱 monitor、Faulted 和原子发布规则未放宽。
- 未知总量显示活动状态，不显示虚假的 0%。Support Pack 的真实阶段采集仍由 #57 负责。
- 活动指示只在工作仍持有资源时显示；失败或取消完成后的淡出不会把空进度重新标为 Working。取消请求仍等待实际工作退休。

## 实际文件与职责

```text
src/composition/editor/
├── framework/
│   ├── Inno.Editor.Core/Extensions/EditorModal.cs
│   └── Inno.Editor.Interactions/Runtime/EditorModalExtension.cs
├── features/
│   ├── Inno.Editor.Assets/Samples/
│   │   ├── AssetSampleImportModule.cs
│   │   └── AssetSampleImportProgress.cs
│   ├── Inno.Editor.Exporting/Runtime/
│   │   ├── EditorExportService.cs
│   │   └── EditorExportProgress.cs
│   └── Inno.Editor.Scripting/Runtime/
│       ├── IEditorScriptCompilation.cs
│       ├── EditorScripting.cs
│       └── EditorScriptCompilationProgress.cs
├── presentation/
│   ├── Inno.Editor.ImGui/
│   │   ├── Runtime/EditorModalRenderer.cs
│   │   ├── TextInput/ImGuiUtf8Buffer.cs
│   │   └── Widgets/ImGuiWidget.Dialog.cs
│   └── Inno.Editor.Workbench/
│       ├── Exporting/{ExportWindowModule,GameExportProgressModal,PluginExportModal}.cs
│       └── Scripting/ScriptCompilationModal.cs
└── panels/
    ├── Inno.Editor.Panel.FileBrowser/Samples/AssetSampleImportModal.cs
    └── Inno.Editor.Panel.ShaderEditor/
        ├── Documents/ShaderEditorDocuments.cs
        └── Presentation/ShaderCheckModal.cs
src/runtime/scripting/Inno.Scripting.Reload/
├── ScriptReloadHost.cs
└── ScriptReloadProgress.cs
tests/
├── editor/Inno.Editor.ImGui.Tests/DynamicModalTests.cs
├── scripting/Inno.Editor.Scripting.Tests/ScriptingPipelineTests.Progress.cs
├── build/Inno.Build.Tests/BuildPipelineTests.ExportPresentation.cs
├── ci/{Select-ValidationScope,Test-ValidationScope,New-ProductionSolutionFilter}.ps1
└── Invoke-EditorUiAcceptance.ps1
.github/workflows/rendering-ci.yml
```

没有新增程序集、生产入口、事件总线、身份分配器、Modal 注册器或依赖解析器。`ImGuiUtf8Buffer` 复用原有文本桥接 owner；短文本使用调用内缓冲，长文本使用 BCL ArrayPool，并在 finally 清理归还，不保存原生借用。

测量缓存仅属于现有 UI Session 的 Widget，最多两项、每项合计 16,384 个字符，只保存字符串、标量和尺寸。键包含实际内容、可用尺寸、字体/baked 身份及实际布局参数；共享 atlas 和 remapped font 不使用该缓存。Session 退休清空缓存，不保存插件对象、Type、委托或原生地址。字体与 Zoom 切换具有原生回归。

## 构建与功能证据

本地原始证据 owner 为 `artifacts/acceptance/issue58/`。失败尝试保留独立名称，不覆盖为成功记录。

| 检查 | 结果与证据 |
| --- | --- |
| 正式 Editor Debug 构建 | 通过；`editor-verified.log/.binlog`，6m09s，0 warning / 0 error。随后两处私有实现调整通过 SDK Compile/Copy 更新真实调用方。 |
| 中立 Assets 引用闭包 | 通过；`assets-neutral-verified.log`，0 warning / 0 error。Compiler 已收回实现依赖，未保留无必要的公开接口。 |
| 原生动态 Modal、Widget、Viewport | 73 passed / 0 failed / 0 skipped；`dynamic-accepted.trx`。 |
| 动态尺度 | 28 个 Zoom×DPI 组合，覆盖 1280×720、1920×1080、640×480；为真实 ImGui Context 下的模拟显示指标，不冒充 28 台物理显示器。 |
| 输入、异常与生命周期 | 原生鼠标、Tab/Enter/Escape、取消、异常后下一帧、两个 Session、字体变化、100 次开关循环。见同一原生 TRX 的输出和几何记录。 |
| 脚本与 Sample | 七项独立进程回归通过：旧操作回调、编译失败、取消后的依赖保留、复制取消、Missing 激活、旧代退休、快照不保留 ALC。对应同名 TRX。 |
| Shader Check | 通过；`shader-verified.trx`，实际工作流、History 与文件选择不被绘制修改。 |
| 导出表单/状态/退休 | 5 passed / 0 failed / 0 skipped；`export-terminal.trx`，包含非合作 Support Pack 准备的严格退休。没有生成完整 Support Pack。 |
| 正式 IDE 投影 | Canvas、Rendering2D、FlappyBird 均通过；`projection-*.log`，未编辑生成文件。 |
| 架构、XML、脚本导出与 Wiki | 通过；`wiki-verified.log` 及目录，241/241 项目所有权求值、34,656 API entries、198 示例均无编译错误、0 未求值示例。其他平台产品编译单列未执行。 |
| CI 范围策略 | 19 项通过；`ci-policy-verified.log`。真实 Git 移动/删除、PR merge-base、混合/未知输入、手动 Full、过滤器空集和引用可见性证明。 |
| 生产项目过滤 | 从现有 Solution 派生 169 个实际生产项目；MSBuild 仍解释引用闭包。没有第二套 resolver。 |

## 产品、性能与限制

FlappyBird 使用独立复制的 Assets、Plugins 和项目设置，原 Samples 设置修改得到保护。第一次默认 30,000 帧验收在约 80 秒内完成八项工作流，但随后等待帧上限触发 600 秒 deadline，记录为失败，不能算正常退出通过。第二次 6,000 帧在约 36 秒正常退出，但冷脚本准备尚未完成，也记为失败。后续采用 15,000 帧及相同八项断言复验；不延长 timeout、不删除断言。帧数既不是脚本就绪条件，也不能代替功能断言。

性能在同一机器、SDK、Debug 配置、输入和原生闭包下比较。初始自动高度基线在长短交替时裁剪内容，不能把其较少的可见工作量当作性能优势。保留原始失败采样，并增加相同可见窗口面积的成对比较；动态切换正确性仍由实际自动高度 Renderer 测试验证，不通过固定窗口替代。

FlappyBird 最终 15,000 帧验收通过八项工作流，退出码 0，记录了 Editor state 冻结保存和 product resources released；证据为 `flappybird-accepted/`。前两次失败保留原结果，不计入通过。

最终采样为每个场景五组成对 AB/BA 独立进程、每次 1,000 帧预热及 100,000 次实际绘制；两边都断言真实 460×684 窗口及绘制次数。下面是五次中位数之比，不使用单次最好值。

| 场景 | CPU 变化 | P95 变化 | 托管分配变化 |
| --- | --- | --- | --- |
| 短消息 | +8.065% | +5.126% | −7.636% |
| 长消息 | +9.516% | +7.307% | −7.636% |
| 长短交替 | +7.372% | +2.733% | −7.636% |
| 滚动正文 | +1.581% | −1.945% | −2.182% |

成对差值中位数也全部满足 CPU/P95 ≤10%：CPU 分别为 +6.349%、+7.439%、+4.821%、+7.115%；P95 为 +4.638%、+6.847%、−0.244%、+6.164%。不是零成本修复：当前帧测量/边界保证增加了少量 CPU，不能称作整体 CPU 加速。分配减少来自调用内 UTF-8 缓冲和复用布局；JIT 热路径注解不删除校验或改变 DEBUG 定义。两边 SDK 和配置相同，不归因为运行时升级。

环境为 Ryzen 9 6900HX / Windows 11 10.0.26200，拥有者进程固定最高可用逻辑 CPU；同一工作站同时使用中。证据是原生 UI CPU，不是整引擎 GPU/帧率结论。[完整原始数据及失败采样](ISSUE58_MODAL_PERFORMANCE.json)、[实际公开边界探针](ISSUE58_MODAL_PERFORMANCE_PROBE.cs.txt) 随报告保存。此前 `performance-equal` 的滚动 P95 +10.483% 明确作为失败保留，最终集在绘制热路径调整后重新完整采样。

复现时从基线提交恢复 ImGui 项目的全部手写源码，以标准 net10.0/C#14 审计工程生成原始同名程序集；以相同 supporting DLL 构建探针，候选探针额外定义 `CANDIDATE`。两者分别通过测试输出的 runtimeconfig/deps 运行 `dotnet exec ... ModalPerfProbe.dll <scenario> <result.json>`，按 AB/BA 顺序各五次。`DOTNET_TieredCompilation=0` 只设置于这两个探针进程。该临时审计工程不进入生产 Solution 或两个开发入口。

本轮未重做 Release、Player CoreCLR/NativeAOT、Browser、Support Pack、十次 Native 热构建和全仓无关测试。macOS 物理设备、不同物理显示器与人工视觉体验未执行。#56/#57、整体卡顿归因及其他独立 issue 不由本报告宣称解决。

## CI #59

范围由完整 Git diff 决定，包含删除与移动；PR 使用 merge-base，push 使用直接 before/head，无法确认的输入失败或进入 Full。纯 Editor 路径及其必要测试/文档可以选择 Editor；SDK、共享 build/runtime、平台、backend、未知变更选择 Full。只有可由实际 Git 差异证明未改变边的 Editor ProjectReference 可见性收紧允许局部验证，其余项目图变更仍 Full。

Editor CI 保留真实 Debug 产品、renderer smoke、普通及进程隔离的相关套件、当前架构/XML/Wiki；跳过 Release、NativeAOT 还原、十次热构建、无关全套测试和 Support Pack 生成。输出明确的选择原因及省略项；零测试和工具失败不能报成功。手动完整验收保留。冷 Native Debug 和全局 API/Wiki 仍有成本，尚未把未测量的加速倍数写成事实。

## 十一项质量自查

以下评价只针对本次改动及真实调用链，不宣称整个引擎已达到没有缺陷的“现象级”。

| 维度 | 判断与证据 | 限制 / 后续改进 |
| --- | --- | --- |
| 架构 | Core 中立准备，Interactions 隔离，ImGui 测量，工作流拥有状态；架构验证通过。 | 其他领域问题仍按既有 issue 处理。 |
| 代码整洁度 | 共享 Widget 替代重复状态窗口布局；被替换读取 API 与调用方同步删除。 | 不据此证明全部历史文件排版一致。 |
| 代码干净程度 | 无 BGCS/vendor/generated 改动、无旧路径兼容、无双绘制和测试后门。 | 原始失败证据不被删改为成功。 |
| 抽象程度 | 公开边界只承载必要尺寸、呈现值和工作流快照；内部几何不扩大为通用布局框架。 | 后续新表单仍需由真实需求决定抽象。 |
| 可复用程度 | 编译、导出、Sample、Shader 共用 Dialog；Plugin 表单复用文本和尺寸机制。 | 不强迫领域表单使用消息窗的数据模型。 |
| 样式统一 | 同一字体、宽度、padding、spacing、像素取整和单一滚动 owner；尺度回归通过。 | 没有对全仓所有第三方 Drawer 做穷尽视觉审计；未来仍需规范和测试约束。 |
| 可拓展程度 | 扩展 override OnPrepare；不改 Renderer switch，不存跨代 callback。 | 自适应扩展必须实现当前测量契约，不能隐式依赖旧高度。 |
| 操作逻辑性 | 取消带操作身份；未知进度与确定进度区分，80% 子阶段明确说明。 | #57 尚需提供实际 Support Pack 阶段。 |
| Undo/Redo | 展示和测量不修改 History；Shader 完成回 owner Update；FlappyBird 包含手势与 Undo/Redo。 | 未重写也未重新验收所有无关 History 协议。 |
| 菜单清晰度 | 既有命令与 Modal ID 保持，未增加平行菜单入口。 | 本轮没有重新设计菜单信息架构。 |
| 组件颗粒度 | 无新增程序集；布局、会话、工作流各有 owner，现有基础能力得到复用。 | 不为将来可能的变化拆空项目或造新系统。 |

最终代码完成 73 项完整原生回归；最后的绘制热路径注解后另有 12 项动态原生回归通过（`dynamic-hotpath.trx`）。远端 CI 必须在 PR head 上另行确认，不能拼接不同提交的远端成功结果。Issue 在分支验收后保持 Open，由完成验收的 PR 的 `Fixes` 在合并后关闭；不自动合并。Windows 不存在指定的 Glass.aiff，提示音未播放。
