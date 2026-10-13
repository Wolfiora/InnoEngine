# Inno.Editor.PlayMode

[Editor 索引](README.md) · [Interactions](Inno.Editor.Interactions.md) · [Scene](Inno.Editor.Scene.md) · [Scripting](Inno.Editor.Scripting.md) · [Application](Inno.Editor.Hosting.md)

`Inno.Editor.PlayMode` 负责在可编辑 Scene 文档与可运行游戏副本之间执行原子切换。它只编排脚本就绪、Scene 隔离、Audio generation、History 隔离和游戏生命周期；不拥有 Scene 文件格式、脚本编译器、音频后端、渲染 Pipeline 或具体 Panel。

## 职责与边界

启动和退出资源按字段即时登记。Pending Dispose 不清空 Session/History，不把 Controller 标为 disposed，
正常退出保持 `Stopping` 并在后续安全点重试；Scene entry 失败也使用这条路径，完全补偿后才进入 `Failed`。
退休期间持有 generation change reservation，阻止新 Play、Reload、Build/Export；超时明确 Fault，不能继续重试启动。
`EditorPlayModeLoop.Quiesce()` 是产品退出入口：EditorHost 在拆 Module/Layer/presentation 之前先排空 Play Session，
它只服务产品退出，不作为手动 Reload 自动停止游戏的入口。它不是第二套 Session owner，不向脚本导出。

```mermaid
flowchart LR
    UI["Toolbar / Shortcut"] --> API["IEditorPlayMode"]
    API --> Scripts["IEditorScriptCompilation"]
    API --> History["EditorInteractions history isolation"]
    API --> Scenes["IEditorScenePlayMode"]
    Host["EditorPlayModeLoop"] --> Runtime["RuntimeSession.Tick"]
    API --> Audio["IEditorAudioHost"]
    Scenes --> API
    API --> UI
```

- `EditorPlayModeModule` 是 order `220` 的 internal 协调器；Scene workspace 与 Scene edits 分别为 `200`、`210`，因此 Play 切换只会观察已准备好的文档服务。
- Entry 主动申请 fresh compilation ticket，并只接受该 ticket 成功发布的脚本 generation；失败、取消或被更新请求取代时保持 Edit Scene，不创建半成品 Runtime Session。
- Scene session 用当前序列化契约把所有已加载 Edit Scene 复制到独立 `RuntimeSession`，保留 Scene、GameObject、Component 与 System 的 persistent ID、顺序和 active Scene。Edit 对象始终留在 Edit Session，不参与游戏生命周期。
- 全部 runtime Scene 准备成功后，Game View、Scene View、Hierarchy、Inspector、Selection 与 Gizmo 在同一安全点切换到 Play Session。它们读取并操作同一批 runtime 对象，不会出现画面已经运行而 Inspector 仍指向 Edit 对象的分裂状态。
- 游戏模拟进入 Play Session scope。Editor 模块更新和脚本激活始终进入 Edit Session；运行对象的呈现与修改由其所属 workspace/Session 明确进入作用域。
- Play runtime Scene 允许通过 Hierarchy、Inspector、Gizmo 和 Scene Action 临时编辑，但 `IEditorSceneWorkspace.canPersist` 为 `false`、`IsDirty` 恒为 `false`，Scene/Prefab Open/Save 被拒绝，因此不会显示未保存 `*`，也不会把 runtime 状态写入项目 Asset。
- Undo/Redo 使用临时分支；Play 中产生的记录在退出时释放，进入 Play 前的 Undo 与 Redo 分支完整恢复。
- runtime fixed/update/late callback 发生异常时立即停止后续模拟并请求恢复 Edit；异常只保存为字符串，不长期持有可卸载脚本 generation 的 `Exception` 或 delegate。
- Play Mode 实现 `IEditorReloadGuard`。手动 Scripts/Plugins Reload 仅在 Editing 可用；自动变化暂存，完整停止后处理。Playing 期间持有 generation 读取 lease，不能发布新代码 generation；Preparing、Stopping、Failed 同样阻止激活。
- Play 切换 action 对所有交互区域注册；Windows 的 `Ctrl+P`、macOS 的 `Command+P` 在 GameView 等面板聚焦时仍可停止 Play。工具栏保留主菜单区域展示；文本编辑与独占 Modal 继续遵守现有快捷键路由。
- 进入 Play 所需编译可以在 Compiling、尚未创建运行 Session 时完成。该例外不允许用户在 Compiling 状态追加手动 Reload；取消和停止仍由唯一 controller 处理。
- Console 默认启用 `Clear on Play`：新请求进入 `Compiling` 时清除上一轮普通日志，但保留仍然有效的 Compiler、Importer、Rendering 等 current diagnostics。当前 Play 的全部等级日志在退出后仍可检查，并在下一次 Play 开始时清除。用户只通过 `Editor/Diagnostics/Console` Settings 修改该策略，Console toolbar 不持有重复状态。

## 公共 API

### EditorPlayModeState

| 值 | 含义 |
| --- | --- |
| `Editing` | 可编辑 Scene 文档已加载，模拟停止。 |
| `Compiling` | 等待本次请求对应的 fresh compilation ticket 成功。 |
| `Preparing` | 创建 Runtime Session 并物化隔离 Scene 候选。 |
| `Playing` | runtime Scene 副本接收完整游戏生命周期。 |
| `Stopping` | 停止模拟并按顺序释放 Scene lease、Runtime Session 与 History isolation。 |
| `Failed` | 最近一次编译、准备或模拟失败；诊断保留到再次 Enter 或显式 Exit。 |

### IEditorPlayMode

| 成员 | 语义 |
| --- | --- |
| `state` | 当前六态状态机。 |
| `isPlaying` | 仅在 `Playing` 时为 `true`。 |
| `lastFailure` | 最近一次编译门禁、切换或模拟失败；新 entry 请求会清空。 |
| `activeSessionId` | 当前 Play 日志 Session；不在 Play 请求内时为 `none`。 |
| `stateChanged` | 状态实际变化后触发；单个订阅者异常被隔离。 |
| `EnterPlayMode()` | 从 `Editing` 或 `Failed` 接受 fresh 请求；返回值表示是否接受。 |
| `ExitPlayMode()` | 取消 Compiling、停止 Preparing/Playing，或把 `Failed` 清回 `Editing`；其他状态返回 `false`。 |

最小调用不需要触碰 Scene 或 Scripting：

```csharp
using Inno.Editor.PlayMode;

static bool Toggle(IEditorPlayMode playMode)
{
    return playMode.state is EditorPlayModeState.Editing or EditorPlayModeState.Failed
        ? playMode.EnterPlayMode()
        : playMode.ExitPlayMode();
}
```

EditorScripts 使用逻辑 namespace：

```csharp
using InnoEditor.PlayMode;

static bool TryEnter(IEditorPlayMode playMode)
    => playMode.EnterPlayMode();
```

脚本清单只导出 `EditorPlayModeState` 与 `IEditorPlayMode`。实现 Module、Scene session、Toolbar Action 和 host loop 不作为脚本契约。

### EditorPlayModeController

`EditorPlayModeController` 是 Editor host 组合使用的确定性状态机实现。构造函数接收 `EngineHost`、Play `RuntimeSessionOptions`、`IEditorScenePlayMode`、`IEditorScriptCompilation`、`IEditorHistoryIsolation` 与 `LogRouter`；调用方通过 `AdvanceTransition()` 在 frame-safe point 推进状态，通过 `Tick(deltaTime)` 推进当前 Play Session，并最终 `Dispose()`。它同时实现 `IEditorReloadParticipant`：该接口由 `EditorPlayModeModule` 注册到统一 reload coordinator，不加入 `InnoEditor.PlayMode` 脚本 facade。

### EditorPlayModeLoop

`EditorPlayModeLoop` 是 Application 组合根使用的稳定 host service。每个 Editor frame 调用一次 `Tick(deltaTime)`；只有 `Playing` 状态才转发给当前隔离 `RuntimeSession.Tick`，由 Session 统一推进 fixed、variable、late lifecycle、Jobs、Coroutines 与 Event queue。普通 feature 不得自行推进该 loop，避免同一 Play Session 收到重复生命周期。

## UI 与快捷键

Play 是 `editor/main-menu` area 的 targetless Action，通过通用 `[EditorToolbarItem]` 放在 MenuBar 几何中心。Edit 状态显示 Play icon；Compiling、Preparing 和 Playing 显示 Stop icon 与 checked accent；Stopping 时保持 Stop icon 但禁用，直到资源释放完成。Failed 状态允许重新 Enter 或清除失败。Tooltip 来自 Action 的动态 `EditorActionState.displayName`。

Command/Ctrl + `P` 与 icon 使用同一个 Action 和状态查询，因此不会形成第二套切换逻辑。

## 完整切换顺序

进入 Play：

1. `EnterPlayMode()` 申请新的 `IScriptCompilationTicket` 并切换到 `Compiling`。
2. 只等待该 ticket；stale、canceled、superseded 或 failed ticket 都不能激活 Play。
3. ticket 成功后切换到 `Preparing`，保留 Edit Undo/Redo 并启动隔离 History 分支。
4. 创建新的 Play `RuntimeSession`，捕获 Edit Scene 起始快照，并在候选 Play SceneWorld 中完整反序列化同 persistent ID 的对象图。
5. 全部 Scene、顺序与 active Scene 成功后提交 Scene lease，并为 Play Session 创建独立 Audio generation；任一失败都会逆序回滚候选资源。
6. workspace 按 persistent ID 把 Scene selection 重映射到 runtime copy，并让全部 Scene-facing Editor feature 一次切换到 Play Scene。
7. 切换为 `Playing`，下一帧开始由 Play Session 在对应 Audio scope 中推进完整游戏生命周期。

退出 Play：

1. Action 立即切换到 `Stopping`，因此下一次 host callback 不再模拟。
2. 释放 Play Audio generation，再释放 Scene lease；全部 Scene-facing Editor feature 原子切回始终存在的 Edit Scene，并恢复 Edit master Bus。
3. 若当前 runtime selection 在 Edit 世界有同 ID 对象则重映射，否则恢复进入 Play 前的 selection。
4. Dispose Play `RuntimeSession`，统一卸载 runtime-only Scene、对象、Coroutines、Jobs、Events、Asset 和日志资源。
5. 释放临时 History，恢复进入 Play 前的完整 Undo/Redo 分支。
6. 切回 `Editing`。若释放失败则保留 `Stopping` 和诊断，并在后续安全点重试，不会错误宣称已回到 Edit。

## Reload、保存与生命周期注意事项

- Play Session 不通过“修改 Edit 后恢复 bytes”实现隔离；即使 runtime Transform、Component 或层级发生任意变化，Edit 对象引用和值都不变。
- Editor Application 的 Update、Draw 与键盘操作都进入当前 Play `RuntimeSession` execution scope。这样 Inspector Drawer、Hierarchy Action、Gizmo 和脚本静态门面解析的是同一个 Session；进入 Play 的首个绘制帧也不会短暂回落到 Edit context。
- Asset Source change 在 Play 中排队；退出后 Edit workspace 才消费重命名等 source 同步，避免 authoring 元数据与 runtime 副本混用。
- `editor.ini` capture 在 Play 中仍写入进入 Play 前的已保存 Scene setup，不会把 runtime-only Scene 变成下次启动工作区。
- Assembly reload 请求会结束当前 Play，而不是迁移一个正在执行的游戏世界。该退出发生在 TypeCache/Plugin candidate 激活之前，因此旧 Component、System、Coroutine、Job 与脚本对象不会固定退休 ALC；即使后续 candidate activation 回滚，也保持 Edit，不尝试重建已经丢弃的瞬态 simulation。
- 正常 Editor shutdown 会先恢复 Edit state，再执行 Scene workspace teardown；History scope 与 Scene session 都是幂等释放。
- Play Mode 不持久化自己的状态。Editor 重启始终从 Edit 开始，不自动恢复运行中的游戏。

## 测试覆盖

`Inno.Editor.PlayMode.Tests` 覆盖 compilation ticket 等待/失败/取消、History 分支恢复、fixed/update/late dispatch、模拟异常恢复、Play 日志保留策略、Edit/Play graph identity，以及 viewport presentation 的 Edit → Play → Edit 原子切换。`Inno.Editor.Scripting.Tests.PluginReloadQuiescesPlaySessionBeforeRetiringItsAssemblyGeneration` 使用真实 collectible Plugin Component 验证重载前退出 Play、恢复 Edit/History，并确认退休 Plugin 与脚本 ALC 可回收。测试还验证 runtime Transform 变化由 presentation 立即观察、Edit Transform 不变、非法候选 Play world 不会替换 Edit presentation。`Inno.Editor.Interactions.Tests` 另覆盖 Toolbar 模型发现与 History isolation 的 Undo/Redo 双分支保留。

## 相邻页面

- 上一页：[Inno.Editor.Interactions](Inno.Editor.Interactions.md)
- Scene 隔离实现：[Inno.Editor.Scene](Inno.Editor.Scene.md)
- 编译门禁：[Inno.Editor.Scripting](Inno.Editor.Scripting.md)
- 下一页：[Inno.Editor.Hosting](Inno.Editor.Hosting.md)

## 当前源码公开 API 清单

签名来自当前 SDK 求值与语义符号；按类型列出 public/protected 契约。工作流与生命周期见本页前文。

### `Inno.Editor.PlayMode.EditorPlayModeController`

<!-- api: T:Inno.Editor.PlayMode.EditorPlayModeController a85413018fb9723a7daa8ba7d89391635eeed4e93c08e98ad7bf35c19e98bd38 -->
<!-- api: M:Inno.Editor.PlayMode.EditorPlayModeController.#ctor(Inno.Runtime.EngineHost,Inno.Runtime.RuntimeSessionOptions,Inno.Editor.Scene.IEditorScenePlayMode,Inno.Editor.Scripting.IEditorScriptCompilation,Inno.Editor.Interactions.IEditorHistoryIsolation,Inno.Core.Logging.LogRouter,Inno.Editor.Audio.IEditorAudioHost) 0bc283c26075c11cc02c02bb09919fe6b4662a90cd424e73021493d8aab7965f -->
<!-- api: P:Inno.Editor.PlayMode.EditorPlayModeController.state bd5e26ee3a1716374314aeada6782818849ef345bd248ac426e519b076e2cfaa -->
<!-- api: P:Inno.Editor.PlayMode.EditorPlayModeController.isPlaying 1cc41bfc46803069ea9d1192cac0cd919022b1027898ca076015dc0dd2404435 -->
<!-- api: P:Inno.Editor.PlayMode.EditorPlayModeController.lastFailure 9be2869aae1113699bce9e3a3db6f4dd307888a12d0ebb97f7f85abf40f528df -->
<!-- api: P:Inno.Editor.PlayMode.EditorPlayModeController.activeSessionId 8eed81cf2b3916d47dffeeff137c50bf3ef3d5418be24143a38144e366c949c0 -->
<!-- api: E:Inno.Editor.PlayMode.EditorPlayModeController.stateChanged 35affb50c360bd0ea79e0b2cc8bd9c9246969269c531b581f6e748d4bc379093 -->
<!-- api: M:Inno.Editor.PlayMode.EditorPlayModeController.EnterPlayMode b1a87c10339a0163c40ac5c6827a150cf6f275c2f1be3a1a0881049fd17a86fc -->
<!-- api: M:Inno.Editor.PlayMode.EditorPlayModeController.ExitPlayMode 12d399ce43e1b556f20d6016ca746c3ddb60514dcb0d20bdd204e193b6c3aca1 -->
<!-- api: M:Inno.Editor.PlayMode.EditorPlayModeController.AdvanceTransition e591120e3a76ee1230e3d7eabaa7b9251f47d5d1da89148f337ccf2df00263d2 -->
<!-- api: M:Inno.Editor.PlayMode.EditorPlayModeController.Tick(System.Single) e54b988cffa9ace3909caefba3e6bcebd5c603d0991ca4e0a4bd8f1f47bfb783 -->
<!-- api: M:Inno.Editor.PlayMode.EditorPlayModeController.Dispose fe2fe6005aff6d024110a32dc6989819b35bf95281add58c72b80cd50f250550 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-fa140b484be47e03"></a>[`public System.Action<Inno.Editor.PlayMode.EditorPlayModeState>? Inno.Editor.PlayMode.EditorPlayModeController.stateChanged`](../../src/composition/editor/features/Inno.Editor.PlayMode/Runtime/EditorPlayModeController.cs#L144) | Occurs after the controller commits a state transition. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-8a83f18cc10d7856"></a>[`public Inno.Editor.PlayMode.EditorPlayModeController.EditorPlayModeController(Inno.Runtime.EngineHost engineHost, Inno.Runtime.RuntimeSessionOptions runtimeOptions, Inno.Editor.Scene.IEditorScenePlayMode scenes, Inno.Editor.Scripting.IEditorScriptCompilation scripting, Inno.Editor.Interactions.IEditorHistoryIsolation history, Inno.Core.Logging.LogRouter logs, Inno.Editor.Audio.IEditorAudioHost? audio = null)`](../../src/composition/editor/features/Inno.Editor.PlayMode/Runtime/EditorPlayModeController.cs#L84) | Creates a Play Mode controller around the runtime host, scene snapshot, scripting, and history boundaries.<br>`engineHost`：The application host that owns every isolated Play Mode runtime session.<br>`runtimeOptions`：The validated Play Mode storage and timing policy used for every accepted request.<br>`scenes`：The scene workspace that creates isolated runtime scene sessions.<br>`scripting`：The script compilation service that publishes generation readiness.<br>`history`：The editor history boundary isolated during simulation.<br>`logs`：The application log router that receives Play Mode lifecycle failures.<br>`audio`：Optional Editor audio host that creates an isolated backend generation for Play Mode. | 约束由所属类型与本页生命周期定义。 | `System.ArgumentNullException`：Thrown when any dependency is null. |
| <a id="api-e40848c2064247ac"></a>[`public void Inno.Editor.PlayMode.EditorPlayModeController.AdvanceTransition()`](../../src/composition/editor/features/Inno.Editor.PlayMode/Runtime/EditorPlayModeController.cs#L206) | Advances at most one Play Mode transition at an editor-controlled frame-safe point. | 约束由所属类型与本页生命周期定义。 | `System.ObjectDisposedException`：Thrown after the controller has been disposed. |
| <a id="api-36e4b08af4c8a6f9"></a>[`public void Inno.Editor.PlayMode.EditorPlayModeController.Dispose()`](../../src/composition/editor/features/Inno.Editor.PlayMode/Runtime/EditorPlayModeController.cs#L242) | Releases the isolated runtime session and history scope owned by this controller. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-f592c1a1b44dabcc"></a>[`public bool Inno.Editor.PlayMode.EditorPlayModeController.EnterPlayMode()`](../../src/composition/editor/features/Inno.Editor.PlayMode/Runtime/EditorPlayModeController.cs#L155) | Requests a fresh script generation before preparing an isolated runtime session.<br>返回：true when the controller accepted a new request; otherwise, false. | 约束由所属类型与本页生命周期定义。 | `System.ObjectDisposedException`：Thrown after the controller has been disposed. |
| <a id="api-63e0df825ec0858f"></a>[`public bool Inno.Editor.PlayMode.EditorPlayModeController.ExitPlayMode()`](../../src/composition/editor/features/Inno.Editor.PlayMode/Runtime/EditorPlayModeController.cs#L179) | Requests cancellation, runtime-session disposal, or dismissal of a failed transition.<br>返回：true when the current state accepted the request; otherwise, false. | 约束由所属类型与本页生命周期定义。 | `System.ObjectDisposedException`：Thrown after the controller has been disposed. |
| <a id="api-360228a4d92246f1"></a>[`public void Inno.Editor.PlayMode.EditorPlayModeController.Tick(float deltaTime)`](../../src/composition/editor/features/Inno.Editor.PlayMode/Runtime/EditorPlayModeController.cs#L230) | Advances the isolated runtime session by one complete frame.<br>`deltaTime`：The elapsed editor frame time in seconds. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-f1f36c3f847ac78e"></a>[`public Inno.Core.Logging.LogSessionId Inno.Editor.PlayMode.EditorPlayModeController.activeSessionId { get; }`](../../src/composition/editor/features/Inno.Editor.PlayMode/Runtime/EditorPlayModeController.cs#L139) | Gets the isolated runtime log session associated with the current request. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-df13bc1e0dfb7eca"></a>[`public bool Inno.Editor.PlayMode.EditorPlayModeController.isPlaying { get; }`](../../src/composition/editor/features/Inno.Editor.PlayMode/Runtime/EditorPlayModeController.cs#L117) | Gets whether isolated runtime scenes are actively simulating. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-5879632dfc21294e"></a>[`public string? Inno.Editor.PlayMode.EditorPlayModeController.lastFailure { get; }`](../../src/composition/editor/features/Inno.Editor.PlayMode/Runtime/EditorPlayModeController.cs#L134) | Gets the most recent transition or simulation failure. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-647bb00bc3d77434"></a>[`public Inno.Editor.PlayMode.EditorPlayModeState Inno.Editor.PlayMode.EditorPlayModeController.state { get; }`](../../src/composition/editor/features/Inno.Editor.PlayMode/Runtime/EditorPlayModeController.cs#L112) | Gets the current Play Mode transition state. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-ca14c24fbc73933a"></a>[`public sealed class Inno.Editor.PlayMode.EditorPlayModeController : Inno.Editor.PlayMode.IEditorPlayMode, Inno.Editor.Core.IEditorReloadGuard, System.IDisposable`](../../src/composition/editor/features/Inno.Editor.PlayMode/Runtime/EditorPlayModeController.cs#L28) | Implements the deterministic state machine for one editor Play Mode workflow. | The editor host advances transitions at frame-safe points. Editable scenes remain owned by the scene workspace while the controller owns only the isolated runtime session. | 具体失败按方法与类型契约处理。 |

### `Inno.Editor.PlayMode.EditorPlayModeLoop`

<!-- api: T:Inno.Editor.PlayMode.EditorPlayModeLoop 4268cc2db0e09e0dcb97f2606ba854160f772e4e7028404e9a8e3cd6109cbc2b -->
<!-- api: M:Inno.Editor.PlayMode.EditorPlayModeLoop.Tick(System.Single) e43e91aaa084a0607925ed8b8dd45c7e061ba39e93b1a93e5a16dcfdebfa3fb4 -->
<!-- api: M:Inno.Editor.PlayMode.EditorPlayModeLoop.EnterPresentationScope 5b6f48948503b80d20b04a20ee94032b1020240fc8da312c07dfaf46be0cc014 -->
<!-- api: M:Inno.Editor.PlayMode.EditorPlayModeLoop.Quiesce ce69521273a1cab820ee5b65bcff770f435223ee8723b900403b0ec74616873d -->
<!-- api: M:Inno.Editor.PlayMode.EditorPlayModeLoop.#ctor a9727bfc98b0320dfd26c89600b8cf7c2122422d40a9e09491c5c83f0b4cd501 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-9ae4b4d3b23ada13"></a>[`public Inno.Editor.PlayMode.EditorPlayModeLoop.EditorPlayModeLoop()`](../../src/composition/editor/features/Inno.Editor.PlayMode/Runtime/EditorPlayModeLoop.cs#L8) | C# 生成的声明；状态与值语义遵循所属类型契约。 | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-1608ab01cea48f6e"></a>[`public System.IDisposable Inno.Editor.PlayMode.EditorPlayModeLoop.EnterPresentationScope()`](../../src/composition/editor/features/Inno.Editor.PlayMode/Runtime/EditorPlayModeLoop.cs#L29) | Binds editor presentation and interaction work to the isolated Play session while simulation is active, or returns an inert scope while the editor is not presenting runtime copies.<br>返回：A scope that must be disposed after the current editor presentation phase completes. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-fd48f78305f6ab6f"></a>[`public void Inno.Editor.PlayMode.EditorPlayModeLoop.Quiesce()`](../../src/composition/editor/features/Inno.Editor.PlayMode/Runtime/EditorPlayModeLoop.cs#L37) | Drains the active Play session before the product tears down editor modules, history or presentation. | 约束由所属类型与本页生命周期定义。 | `System.InvalidOperationException`：The session cannot retire within its deadline; the host is faulted and its dependencies must remain alive. |
| <a id="api-de0d21db86662da1"></a>[`public void Inno.Editor.PlayMode.EditorPlayModeLoop.Tick(float deltaTime)`](../../src/composition/editor/features/Inno.Editor.PlayMode/Runtime/EditorPlayModeLoop.cs#L20) | Advances the active simulation by one complete frame when Play Mode is running.<br>`deltaTime`：The elapsed frame time in seconds. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-d5a8d006ea1cf669"></a>[`public sealed class Inno.Editor.PlayMode.EditorPlayModeLoop`](../../src/composition/editor/features/Inno.Editor.PlayMode/Runtime/EditorPlayModeLoop.cs#L8) | Bridges the editor host frame clock to the active Play Mode session. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

### `Inno.Editor.PlayMode.EditorPlayModeState`

<!-- api: T:Inno.Editor.PlayMode.EditorPlayModeState adf2567f0f1733e24d323b15c09274e97a0815dec3f594b284839941d971522b -->
<!-- api: F:Inno.Editor.PlayMode.EditorPlayModeState.Editing 183dbd387a8a3fa478b81ef9625d4b0dc9a97709985ae35bfd2594c79a675c49 -->
<!-- api: F:Inno.Editor.PlayMode.EditorPlayModeState.Compiling c4aa585ecd537c002b352af1d37c90c9c489236e7dd6b229ddde8c948b924b58 -->
<!-- api: F:Inno.Editor.PlayMode.EditorPlayModeState.Preparing 490ecff5458a6f6adbd51ed25ab278003b936e16fd6fa0d8ebca9ce2e51af7be -->
<!-- api: F:Inno.Editor.PlayMode.EditorPlayModeState.Playing 3576a152773ffa7ceee0d35e0f671a41d1981737be04e24c1121d07c92cf983d -->
<!-- api: F:Inno.Editor.PlayMode.EditorPlayModeState.Stopping 25918310c03f6f70437f8aa2c9cfaad7baaeaf19adcf31f9e42cfec7efb3d494 -->
<!-- api: F:Inno.Editor.PlayMode.EditorPlayModeState.Failed 6a66229c7a85c554f5142c7d1c3a285978b91193ecf9d92d3dac2fc3528e426c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-ec60e99b27f8857b"></a>[`Inno.Editor.PlayMode.EditorPlayModeState.Compiling = 1`](../../src/composition/editor/features/Inno.Editor.PlayMode/EditorPlayModeState.cs#L16) | Play Mode is waiting for the requested script generation to become active. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-27814f6c91ebf262"></a>[`Inno.Editor.PlayMode.EditorPlayModeState.Editing = 0`](../../src/composition/editor/features/Inno.Editor.PlayMode/EditorPlayModeState.cs#L11) | Editable scene documents are loaded and game simulation is stopped. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-47d739aae8f15f69"></a>[`Inno.Editor.PlayMode.EditorPlayModeState.Failed = 5`](../../src/composition/editor/features/Inno.Editor.PlayMode/EditorPlayModeState.cs#L36) | The most recent transition failed and its diagnostic remains available for inspection. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-26641219af9a603e"></a>[`Inno.Editor.PlayMode.EditorPlayModeState.Playing = 3`](../../src/composition/editor/features/Inno.Editor.PlayMode/EditorPlayModeState.cs#L26) | Isolated runtime scenes are receiving the game update lifecycle. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-10ff0c598d1007e6"></a>[`Inno.Editor.PlayMode.EditorPlayModeState.Preparing = 2`](../../src/composition/editor/features/Inno.Editor.PlayMode/EditorPlayModeState.cs#L21) | The active script generation is ready and isolated runtime state is being prepared. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-584c569ce060140e"></a>[`Inno.Editor.PlayMode.EditorPlayModeState.Stopping = 4`](../../src/composition/editor/features/Inno.Editor.PlayMode/EditorPlayModeState.cs#L31) | The isolated runtime session is stopping and releasing its owned state. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-d7ec9ccecc5de6a4"></a>[`public enum Inno.Editor.PlayMode.EditorPlayModeState : int`](../../src/composition/editor/features/Inno.Editor.PlayMode/EditorPlayModeState.cs#L6) | Identifies the current relationship between editor documents and game simulation. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

### `Inno.Editor.PlayMode.IEditorPlayMode`

<!-- api: T:Inno.Editor.PlayMode.IEditorPlayMode 5beb83a248feec7b28f19ae14e278879dbc93d2f4f2447fd63a5b918a6610426 -->
<!-- api: P:Inno.Editor.PlayMode.IEditorPlayMode.state 4504a739d9b5d26b51056ef5056fbf3b31a26e5b5a1d35511e90edba3a0967a4 -->
<!-- api: P:Inno.Editor.PlayMode.IEditorPlayMode.isPlaying d466fb26c7285b436f26043b8874fbeee209b5f0e28bfec611c23f875b515929 -->
<!-- api: P:Inno.Editor.PlayMode.IEditorPlayMode.lastFailure 79215248e6a2157833f32c617798db37604190748fab6ecd029e2d5566f106c9 -->
<!-- api: P:Inno.Editor.PlayMode.IEditorPlayMode.activeSessionId a8a3b1c668ff811a0b9351cb0b58171d32ffbc60221a61da54bc782f807fd7e6 -->
<!-- api: E:Inno.Editor.PlayMode.IEditorPlayMode.stateChanged 82cd26deacd4ddbabe35b0f473a338d6828eb2bd1a222efbaee7bccd65d76065 -->
<!-- api: M:Inno.Editor.PlayMode.IEditorPlayMode.EnterPlayMode 0e15c2bcb18884139c64df74cec02f3f63572fd8ff18904072d24f6705036533 -->
<!-- api: M:Inno.Editor.PlayMode.IEditorPlayMode.ExitPlayMode 762a151d253f06162984580a5b44f4c4fc93cd2e95d0848ac61b6651eb686922 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-75115a3d5b9c13b7"></a>[`System.Action<Inno.Editor.PlayMode.EditorPlayModeState>? Inno.Editor.PlayMode.IEditorPlayMode.stateChanged`](../../src/composition/editor/features/Inno.Editor.PlayMode/IEditorPlayMode.cs#L35) | Occurs after P:Inno.Editor.PlayMode.IEditorPlayMode.state changes. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-791b2fe37f3c7e6b"></a>[`bool Inno.Editor.PlayMode.IEditorPlayMode.EnterPlayMode()`](../../src/composition/editor/features/Inno.Editor.PlayMode/IEditorPlayMode.cs#L43) | Requests entry after the active script generation becomes ready.<br>返回：true when a new entry request was accepted. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-9d02689b36c5c6f6"></a>[`bool Inno.Editor.PlayMode.IEditorPlayMode.ExitPlayMode()`](../../src/composition/editor/features/Inno.Editor.PlayMode/IEditorPlayMode.cs#L51) | Requests disposal of the active runtime session or dismisses a failed transition.<br>返回：true when entry was cancelled or a new exit request was accepted. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-6486e544d3786d14"></a>[`Inno.Core.Logging.LogSessionId Inno.Editor.PlayMode.IEditorPlayMode.activeSessionId { get; }`](../../src/composition/editor/features/Inno.Editor.PlayMode/IEditorPlayMode.cs#L30) | Gets the active isolated runtime log session, or P:Inno.Core.Logging.LogSessionId.none outside a Play Mode request. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-4fd7458bc3658a2d"></a>[`bool Inno.Editor.PlayMode.IEditorPlayMode.isPlaying { get; }`](../../src/composition/editor/features/Inno.Editor.PlayMode/IEditorPlayMode.cs#L20) | Gets whether isolated runtime scenes are actively simulating. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-ece56d9c83f11001"></a>[`string? Inno.Editor.PlayMode.IEditorPlayMode.lastFailure { get; }`](../../src/composition/editor/features/Inno.Editor.PlayMode/IEditorPlayMode.cs#L25) | Gets the most recent transition or simulation failure, or null when no failure is active. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-ed080788229232d2"></a>[`Inno.Editor.PlayMode.EditorPlayModeState Inno.Editor.PlayMode.IEditorPlayMode.state { get; }`](../../src/composition/editor/features/Inno.Editor.PlayMode/IEditorPlayMode.cs#L15) | Gets the current Play Mode transition state. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |
| <a id="api-9ce09b2dc7b41bf3"></a>[`public interface Inno.Editor.PlayMode.IEditorPlayMode`](../../src/composition/editor/features/Inno.Editor.PlayMode/IEditorPlayMode.cs#L10) | Controls the editor's isolated game-simulation session. | 约束由所属类型与本页生命周期定义。 | 具体失败按方法与类型契约处理。 |

## 项目依赖

- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.UI.Runtime](../ui/Inno.UI.Runtime.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Scripting.Compiler](../scripting/Inno.Scripting.Compiler.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Input](../core/Inno.Core.Input.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Scene](../scene/Inno.Scene.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Diagnostics](../core/Inno.Core.Diagnostics.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Reload](../extensibility/Inno.Extensibility.Reload.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Editor.Audio](Inno.Editor.Audio.md)：公开引用边界由实际签名核对。
- [Inno.Editor.Core](Inno.Editor.Core.md)：公开引用边界由实际签名核对。
- [Inno.Runtime](../runtime/Inno.Runtime.md)：公开引用边界由实际签名核对。
- [Inno.Editor.Interactions](Inno.Editor.Interactions.md)：公开引用边界由实际签名核对。
- [Inno.Editor.Scene](Inno.Editor.Scene.md)：公开引用边界由实际签名核对。
- [Inno.Editor.Scripting](Inno.Editor.Scripting.md)：公开引用边界由实际签名核对。
- [Inno.Core.Logging](../core/Inno.Core.Logging.md)：公开引用边界由实际签名核对。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
