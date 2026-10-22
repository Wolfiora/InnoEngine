# 插件 Events / Identity 复验与 Browser SDK 阻塞

日期：2026-10-22。引擎基线 `401c59e430e8959bc8b8320194edd969c49ad63a`；Windows x64，SDK 10.0.402 / C# 14。不使用 computer use，macOS 不在本轮实机范围。

## 结果与实际修正

- **R01 关闭。** 当前 Rendering2D 源码已经通过 `SpriteAnimator2D.Listen`、Core EventDispatcher/EventHub 和中立帧事件工作，旧台账仍描述 C# delegate 事件。本轮核对源和安装包，并补充监听者抛错、释放 token 后继续分发的回归。原有优先级、销毁释放与重入播放回归同时通过，没有增加另一条事件路径。
- **R02 关闭此项具体缺口。** CanvasState 使用已有 `Identity` 弱快照和 `Resolve<Canvas>()`，不强持有 Canvas，不依赖当前 allocator 猜测原域。命中、输入、帧更新和事件交付校验 owner、启用状态、文档、Context 与实际服务。外部 UI 调用后再次检查，失效状态幂等释放自身绘制资源；组件继续独占 UI Context。没有新 resolver、事件队列、全局服务或公开 API。
- **G03 仍阻塞。** 实际 workload 下，官方 BGCS 3.0.1 再次拒绝 `bgfx_uniform_handle_s` 按值调用：`BGCS-CSHARP-ABI-PLAN` / 缺少 aggregate calling profile。宏常量诊断亦完整保留。未改 SDK、包内容、生成结果或目标 ABI，未发布失败候选。

## 验收

| 检查 | 实际结果 |
| --- | --- |
| 仓库架构 | 正式 `verify` 完成 238/238 工程 MSBuild ownership 求值并通过架构门禁；`architecture.log`。 |
| Rendering2D 正式 scripts 与契约测试 | 42 通过、0 失败、0 跳过；包含监听异常后清理。 |
| Canvas 正式源码/脚本与测试 | 15 通过、0 失败、1 跳过；跳过项仍按既有设备条件报告。 |
| 新生命周期用例 | 10 项均通过：销毁、禁用、文档解除、组件删除/替换、Context 替换、场景卸载、注销、UI 更新中销毁，以及保留旧 target/source 时 Canvas 经完整 GC 可回收。 |
| 安装包 | Canvas 由正式 `plugin` 命令原子导出，包内两份修改源码与当前源文件逐字节相同；SHA256 `eea1a677419aafd3537665b716302a90d6a19ccbca470e87546c3d77d6b3b719`。Rendering2D Assets 未变，既有包 SHA256 `f12ccf984185e722a9ebfedb723eb62c90eff2392ec13c14fb9941b475604498` 核对一致。 |
| 安装态示例 | FlappyBird 正式 scripts / IDE 编译通过；D3D11、D3D12、Vulkan 各 3,000 帧、正常 Dispose、exit 0。 |
| 第三方完整性 | 原基线 129 个 BGCS 包文件哈希均未变；BGCS 仓库无本轮修改。 |

## 文件与所有权

```text
InnoEngine.Canvas/
├── Assets/Runtime/Components/Canvas.cs
├── Assets/Runtime/Rendering/CanvasViewContentSource.cs
├── Tests/InnoEngine.Canvas.Tests/CanvasViewContentSourceTests.cs
├── Tests/InnoEngine.Canvas.Tests/CanvasAuthoringTests.cs
├── README.md
└── docs/ARCHITECTURE.md
InnoEngine.Rendering2D/
└── Tests/InnoEngine.Rendering2D.Tests/AnimatorLifecycleTests.cs
InnoEngine.Samples/
└── FlappyBird/Plugins/InnoEngine.Canvas.iplugin
InnoEngine/
├── docs/architecture/CURRENT_ISSUES.md
├── docs/architecture/CURRENT_ACCEPTANCE.md
└── records/architecture/acceptance/PLUGIN_EVENTS_IDENTITY_ACCEPTANCE_2026_10_22.md
```

引擎生产源码、Core Identity、Core Events 及平台依赖方向均未改变；插件消费既有稳定契约。退休键使用复用的列表，不新增逐帧完整状态快照。核对了 Identity/Reference/Reload 标准与 AGENTS 第 30 节；弱 owner 回收和正常退出不替代完整 collectible ALC、高频 reload、音频听感或所有设备验收，也不宣称帧性能量化提升。此前记录的 Scene 保存身份差异尚无本轮独立复现，不能用本次 Canvas 结果关闭。

## Browser 继续条件

`IInteropTransform` 处理最终互操作契约，不能伪称原生 struct ABI 已改变；现有 C header helper bridge 保留原调用形状，Cpp callable lowering 属于 C++ bridge。保持当前 C API 契约时，需要 SDK 正式提供 Wasm aggregate profile 或经过验证的 C API pointer bridge，再重跑 Mono Wasm / Wasm AOT。另一种完整 backend facade 重构必须对所有目标统一设计、生成和验收，不能加 Browser 专用手写 import 或以同大小 scalar 替换绕过 ABI 证据。

Canvas 首轮测试编译发现旧 fixture 仍读取已删除的 `BuildCompositionContext.bindingGenerator`；现通过发行的 `ResolveBindingGenerator(target)` 取得目标贡献，没有恢复旧 API。原始 `canvas-initial-build-failure.log` 与最终通过的 `canvas-tests.log` 分别保留。

证据位于本地 `artifacts/acceptance/plugin-boundaries-2026-10-22/`：`rendering2d-final-tests.log`、`canvas-tests.log` 及 TRX、`canvas-export.log`、`flappy-scripts.log`、`flappy-ide.log`、`flappy-*.log`、`browser-bindings.log`、`browser-bindings-configured.log` 和 `results.json`。首次 Browser 命令因 PATH 缺 CMake 提前拒绝；配置本机既有 CMake/Ninja 后才取得上述 ABI 失败，两个结果分别保留。提示音文件在 Windows 不可用，未播放。
