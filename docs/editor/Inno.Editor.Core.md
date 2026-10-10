# Inno.Editor.Core

[Editor 索引](README.md) · [Interactions](Inno.Editor.Interactions.md) · [Wiki 首页](../README.md)

`Inno.Editor.Core` 保存 Editor 的基础生命周期契约与领域无关的 assembly reload 协调协议。它不知道 Action、Menu、Selection、ImGui、Assets、Scene、Scripting 或具体 Panel，因此可以被任意表现后端和 feature 安全引用。

## 目录与职责

```text
Inno.Editor.Core/
├─ Runtime/
│  ├─ EditorContext.cs
│  ├─ EditorFrame.cs
│  ├─ EditorStatistics.cs
│  ├─ EditorRuntime.cs
│  ├─ EditorState.cs
│  └─ EditorLayoutSettings.cs
├─ Extensions/
│  ├─ EditorModule.cs
│  ├─ EditorModuleAttribute.cs
│  ├─ EditorPanel.cs
│  ├─ EditorPanelAttribute.cs
│  ├─ EditorModal.cs
│  ├─ EditorModalAttribute.cs
│  └─ IEditorPanelReloadState.cs
├─ Reloading/
│  ├─ EditorReloadCoordinator.cs
│  └─ IEditorReloadParticipant.cs
└─ Properties/ScriptingApi.cs
```

所有目录中的类型都使用物理 namespace `Inno.Editor.Core`；目录只表达职责，不扩展 namespace。Runtime 聚合项目上下文、帧、状态参数与 layout 实现，Extensions 聚合 Module/Panel/Modal 的发现契约，不再为每一种基类建立单独目录。

旧的 Commands、Menus、DragDrop、Selection 和 Rename 状态均不属于 Core，现已迁入 `Inno.Editor.Interactions` 或对应 Panel。

## Runtime API

| API | 说明 |
| --- | --- |
| `EditorContext` | 对扩展公开只读项目根目录、最新 `EditorFrame`、焦点和窄作用域的帧统计交换；不提供 service locator 或持久化写入口。 |
| `EditorLayoutSettings` | internal 实现；协调 `editor.ini` 中互不覆盖的 ImGui layout 与可读具名 section。它不会成为跨项目公开依赖。 |
| `EditorFrame` | 一帧的 `deltaTime`、`totalTime`、`isFocused` 不可变快照。 |
| `EditorStatisticId` / `EditorStatisticGroupId` | 跨 Panel、Module 和 Plugin 唯一的稳定统计项/分组标识。 |
| `EditorStatistic` | 只保存 stable ID、显示顺序和字符串值的不可变贡献，不保留 Plugin 类型或实例。 |
| `EditorStatistics` | 当前帧发布、同 ID 替换和上一完成帧 handoff 的顺序无关交换。 |
| `EditorRuntime` | 表现无关的 `Start`、`Update(EditorFrame)`、`Dispose` 抽象。 |

`EditorContext` 是由 Application host 创建并注入扩展的中立数据，不承担路由：

```csharp
Console.WriteLine(context.projectDirectory);
Console.WriteLine(context.frame.totalTime);
context.statistics.Publish(new EditorStatistic(
    new EditorStatisticId("sample.animation.playing-clips"),
    new EditorStatisticGroupId("sample.animation"),
    "Animation",
    "Playing Clips",
    playingClipCount.ToString()));
```

Statistics 是唯一允许写入 Context 的帧数据通道，不是任意 service bag。每次 runtime
推进 `EditorFrame` 时，当前贡献成为上一完成帧快照并开始新的 current frame；读取时 current
覆盖同 ID 的 completed 值。因此 Stats Panel 先于或后于某个扩展绘制都不会丢数据，停止贡献
后值最多保留一个 handoff frame。值已经是面向人的字符串，采样器、Profiler 历史和持久配置
仍应由各自领域拥有。

构造函数、`layoutPath`、`imguiLayout`、section 读写、ImGui layout 更新和 Save 是 CLR host 边界。由于 Application 与 Interactions 是独立程序集，这些成员是 public CLR API，但全部标记 `ScriptingApiIgnore`，不会进入 EditorScripts facade。测试通过真实公开契约验证，不允许反射穿透、测试后门或 `InternalsVisibleTo`。EditorScripts 不能创建第二个 Context、读取原始 section、覆盖其他扩展状态或主动写入 `editor.ini`。

这些 API 只处理 Module/Panel 项目状态与 Dear ImGui 使用的 `editor.ini`。业务设置由 [Inno.Editor.Settings](Inno.Editor.Settings.md) 通过 SerializationRegistry 写入项目根 `Settings.Editor.inno`。业务扩展通过构造注入接收正式服务，不向 `EditorContext` 添加全局 service locator。

读取 `editor.ini` 时，`EditorLayoutSettings` 将 ImGui layout 的行分隔符规范化为 LF，避免同一项目从 macOS 转到 Windows 后仅因宿主平台的 `Environment.NewLine` 改变 `imguiLayout` 的文本值；具名 Editor section 仍保持原有解析规则。

## Module

`EditorModule` 表示跨 Panel 共享、随扩展 generation 启停的 feature 状态：

```csharp
[EditorModule("animation", order: 100)]
public sealed class AnimationModule : EditorModule
{
    private bool m_isBaking;

    public override bool blocksFollowingUpdates => m_isBaking;

    protected override void OnStart(EditorContext context)
    {
    }

    protected override void OnUpdate(EditorContext context)
    {
    }

    protected override void OnStop(EditorContext context)
    {
    }
}
```

`blocksFollowingUpdates` 是通用的原子启动/切换屏障：当前 Module 返回 `true` 时，排序在它之后的 Module 本帧不更新，但 Panel 和 Modal 仍可绘制。Scripting 用它保证脚本类型激活后 Scene 才恢复；未来 Shader/Pipeline bootstrap 也可以使用同一机制，避免业务模块互相引用。

`EditorModuleAttribute.id` 是必填且全局唯一的 Module identity。它同时用于发现冲突、诊断和可选状态 section；Module 不再声明第二个 workspace ID。

`EditorModule` 由扩展 Catalog 通过 `IDisposable` 统一释放，但 `Dispose` 是显式接口实现，不是派生类型的公开成员。Module 若拥有 Registry、watcher 或其他资源，只重写 `protected virtual OnDispose()`；Catalog 会在 Stop 且 generation 离开活动状态后调用一次。`IDisposable` 在这里仍有明确用途：它是 Catalog 与所有 Module 共用的基础设施 teardown 协议，而不是 feature 自己暴露或手工调用的生命周期 API。

启动前即建立 Module 所有权：`OnStart` 失败也必须执行 `OnStop` 补偿。只有 Start 成功后才可 Update；
Stop 开始后不再 Update。完成的 Stop 幂等，`IDisposable.Dispose` 会补停尚未停止的 Module，再执行 OnDispose。
`RetirementPendingException` 不表示已释放：Stop/Dispose 必须保留未完成步骤供 owner 重试；不能提前标 disposed。
普通 Stop/Dispose 错误在完成其余清理后聚合报告。Catalog 使用 Core 的有界退休协议，超时 Fault 并要求重启。

Module、Panel、Action、Menu source 和 Drop handler 可以在唯一构造函数中请求 `EditorContext`、`EditorInteractions` 或一个无歧义的已发现 `EditorModule`。不存在手工注册表。

## Panel

```csharp
[EditorPanel(
    "animation.graph",
    "Animator",
    order: 500,
    defaultOpen: true,
    menuPath: "Authoring")]
public sealed class AnimatorPanel(AnimationModule animation) : EditorPanel
{
    protected override void OnDraw(EditorContext context)
    {
        // Render the panel body.
    }
}
```

`EditorPanel.useWindowPadding` 默认返回 `true`，表示表现后端应使用标准窗口内边距。需要让背景、Tree 行或根滚动区域与 Dock body 边缘对齐的 Panel 可以重写为 `false`；正文仍可通过表现层的统一 content region 恢复恰好一层内边距。这是 Panel 的布局策略，不要求业务代码修改或重置滚动位置。

`EditorPanel.allowScrolling` 默认返回 `true`。Scene/Game/Graph 等自行管理画布导航的
全画布 Panel 应返回 `false`，Host 会同时禁用窗口滚动条和鼠标滚动范围。普通列表、
Inspector 与文档 Panel 保持默认值，并自动使用 Editor 的全局 overlay scrollbar。

`EditorPanelAttribute.id` 必须稳定且全局唯一；它用于 Panel 菜单、窗口 identity 和 reload 状态。`title` 只用于显示，可以变化。可选 `menuPath` 是 `Panel` 主菜单下的开放斜杠分类路径，`separatorBefore` 可在最终分类内开始新的视觉分组；Panel 分类不由 Host 维护封闭名单。

运行时始终按 ID 迁移 `isOpen`。需要迁移更多中立状态时实现 `IEditorPanelReloadState`，只返回不引用插件对象的字节：

```csharp
public ReadOnlyMemory<byte> CaptureReloadState() => m_stateBytes;

public void RestoreReloadState(ReadOnlyMemory<byte> state)
{
    m_stateBytes = state.ToArray();
}
```

Attach 同样先记录所有权：`OnAttach` 失败会调用 `OnDetach` 补偿，只有补偿成功才能把 Panel 隔离后继续。
Detach 完成后幂等；Pending 时 Panel 禁止 Draw，资源和依赖保持受拥有。不是把失败 Attach 当作从未启动。

## Modal

`EditorModal` 是被发现的阻塞或非阻塞浮层契约。具体位置、尺寸、淡入淡出和输入阻塞由 ImGui runtime 统一处理。

```csharp
using System.Numerics;

[EditorModal("animation.baking", "Baking Animation", order: 200)]
public sealed class AnimationBakeModal(AnimationModule animation) : EditorModal
{
    public override bool isVisible => animation.isBaking;
    public override bool blocksInteraction => true;
    public override bool canMove => true;
    public override bool canResize => true;
    public override Vector2 initialSize => new(900f, 600f);
    public override Vector2 minimumSize => new(640f, 420f);

    protected override void OnDraw(EditorContext context)
    {
        // Draw body only; do not position the popup here.
    }
}
```

`canMove`、`canResize` 默认均为 false，因此既有进度 Modal 继续保持居中 auto-size。需要 Settings 风格窗口时可分别开启移动和缩放，并用 `initialSize` / `minimumSize` 提供未乘 zoom 的逻辑尺寸。`allowScrolling` 默认开启；当完整正文由一个或多个 Child 承担滚动时应关闭，防止 Modal 与 Child 重复生成滚动条。Modal 仍是非 Dock 契约；具体 backend 必须阻止 Dock 与 Collapse/最小化。

## Reload coordination

`EditorReloadCoordinator` 是 Core 中唯一的跨 feature reload 协调入口。Coordinator 的索引只弱持有参与者；`Register(IEditorReloadParticipant)` 返回的 registration lease 则强持有参与者，调用方必须在 feature 生命周期内保存该 lease。释放 lease 会先注销再解除强引用；如果整个 feature 与 lease 一起失去所有权，弱索引也不会阻止它们被回收。Core 不知道 Scene、Missing、Panel 或脚本编译，仅编排中立事务。

协调顺序固定为：全部 `PrepareForActivation` → Assembly candidate `Activate` → 可选外部状态同步 → 全部 `Apply` → Assembly `Complete` → 各 participant cleanup-only `Complete`。Assembly 与外部 Asset/Settings 构成同一个 publication 边界；提交前失败先逆序恢复 feature 结构，再恢复 Assembly 和外部 resolver，最后逆序恢复 previous feature 属性。外部恢复不能放到最后一个领域 participant，否则 Scene 会用候选 Asset resolver 恢复旧属性。提交后普通清理异常聚合抛出并 Fault，不能伪回滚，也不能只记日志继续。
任何阶段的未退休信号都不得被普通异常聚合掩盖：共享 coordinator 保留未完成事务，停止后续卸载/恢复，封锁 Play、Build、Export 和下一次 reload。

| API | 说明 |
| --- | --- |
| `EditorReloadCoordinator.Register` | 在 Coordinator 中弱注册领域参与者，并返回负责强生命周期所有权的 registration lease。 |
| `EditorReloadCoordinator.Execute` | 把 prepared `AssemblyReloadSession` 与所有领域事务作为一次原子切换执行。 |
| `EditorReloadCoordinator.RefreshDiagnostics` | 请求所有存活领域按当前状态重新发布诊断。 |
| `IEditorReloadParticipant.Capture` | 只捕获事务，不在 capture 阶段修改 live state。 |
| `IEditorReloadParticipant.RefreshDiagnostics` | 重建当前状态诊断；不依赖某一种 reload 请求。 |
| `IGenerationChange`（来自 `Inno.Extensibility.Reload`） | 领域 Capture 返回的统一 prepare、apply、结构回滚、旧状态恢复与 cleanup-only complete 协议；没有 Editor 专属事务副本。 |

Scripting 负责准备 assembly session 并调用协调器；Scene 独立注册自己的 participant。因此二者都只依赖 Core，不互相引用。

## Scripting API

EditorScripts 使用唯一逻辑命名空间 `InnoEditor.Core`。它导出 Context、Frame、frame-scoped Statistics、Runtime、Module、Panel、Modal、`EditorState` 和 Panel Reload State 接口；不导出 assembly reload coordinator、layout reader/writer 或 JSON DOM。脚本 Module/Panel 只能实现 protected `OnStart/OnUpdate/OnStop`、`OnAttach/OnDetach/OnDraw` 与 `Capture/Restore` hooks，不能直接调用标记为 `ScriptingApiIgnore` 的 Start、Update、Stop、Attach、Detach 或 Draw；所有脚本必须显式写普通 `using`。

## Module/Panel 项目状态

状态能力直接属于 `EditorModule` 与 `EditorPanel`。派生类型默认不保存任何状态；Catalog 只为真正 override `Capture(EditorState)` 的类型建立内部状态注册。没有 override Capture 的 Module/Panel 不会进入 restore、capture 或 section IO。唯一 ID 直接来自 `EditorModuleAttribute.id` 或 `EditorPanelAttribute.id`：

```csharp
[EditorModule("animation")]
public sealed class AnimationModule : EditorModule
{
    protected override void Capture(EditorState state)
    {
        state.Set("controller", m_controllerAssetId);
        state.Set("zoom", m_zoom);
    }

    protected override void Restore(EditorState state)
    {
        m_controllerAssetId = state.Get("controller", Guid.Empty);
        m_zoom = state.Get("zoom", 1f);
    }
}
```

`Restore` 只会为已 override Capture 的实例调用；section 不存在时，`state.Get` 直接返回调用者给出的 fallback。`EditorState` 是唯一公开参数契约，只提供 `Get` / `Set`；存储格式、JSON serializer 和 section 转换全部位于 Interactions 的 internal 实现中。Capture 参数可写，Restore 参数只读。状态只应保存项目相关、可重新解析的中立值，不保存 runtime 对象、线程、delegate 或插件实例，也不自行引入 schema 迁移字段。

Scene document 的公开查询/工作流面位于 `Inno.Editor.Scene.IEditorSceneWorkspace`；其 internal Module 实现通过同一组 Capture/Restore hooks 保存 Scene 路径，不向扩展暴露生命周期或文档修改入口。

## 边界规则

- 不向 Core 添加 Rename、Open、Save、Asset、Missing 或 Scene 等 feature 概念。
- 不向 Context 添加 `IWhateverService` 集合或可变注册接口。
- 不在 Core 引用 ImGui。
- Action/Menu/Drag/Selection 统一见 [Interactions](Inno.Editor.Interactions.md)。

EditorReloadCoordinator.Execute(reload, externalChange) 的可选参数是 Foundation IGenerationChange，不是 activate/restore Action 对。Editor participant 的 Capture 在共享 gate 的 Prepare 阶段发生；外部 Plugin/Settings change 与 Editor 状态共同提交/回滚，Pending 和 timeout 穿透诊断隔离。没有公开测试后门或按具体领域硬编码的 coordinator。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Editor.Core.EditorContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Core.EditorContext`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorContext.cs#L13) | Provides passive project and frame state shared by editor extensions. Interaction routing is supplied separately by Inno.Editor.Interactions. |
| [`Inno.Editor.Core.EditorContext.EditorContext(string projectDirectory, Inno.Editor.Core.EditorKeyboardPolicy keyboard)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorContext.cs#L30) | Creates a passive editor context for one project. |
| [`Inno.Editor.Core.EditorFrame Inno.Editor.Core.EditorContext.frame`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorContext.cs#L186) | Gets the latest immutable editor frame snapshot. |
| [`Inno.Editor.Core.EditorKeyboardPolicy Inno.Editor.Core.EditorContext.keyboard`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorContext.cs#L52) | Gets the immutable keyboard conventions shared by discovery, dispatch and presentation. |
| [`Inno.Editor.Core.EditorStatistics Inno.Editor.Core.EditorContext.statistics`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorContext.cs#L57) | Gets the frame-scoped statistics exchange used by independent editor features and viewers. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Editor.Core.EditorContext.GetLayoutSectionNames(string prefix = "")`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorContext.cs#L85) | Gets layout section names matching an ordinal prefix. |
| [`bool Inno.Editor.Core.EditorContext.RemoveLayoutSection(string sectionName)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorContext.cs#L144) | Removes one editor layout section from the in-memory document. |
| [`bool Inno.Editor.Core.EditorContext.SaveLayoutIfChanged()`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorContext.cs#L168) | Atomically saves the project editor layout when it changed. |
| [`bool Inno.Editor.Core.EditorContext.TryGetLayoutSection(string sectionName, out System.Collections.Generic.IReadOnlyDictionary<string, string> values)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorContext.cs#L103) | Tries to read one independent editor layout section snapshot. |
| [`bool Inno.Editor.Core.EditorContext.isFocused`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorContext.cs#L191) | Gets whether any editor viewport currently owns application focus. |
| [`string Inno.Editor.Core.EditorContext.imguiLayout`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorContext.cs#L68) | Gets the Dear ImGui layout text without editor module or panel state sections. |
| [`string Inno.Editor.Core.EditorContext.layoutPath`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorContext.cs#L62) | Gets the absolute path of the project editor layout document. |
| [`string Inno.Editor.Core.EditorContext.projectDirectory`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorContext.cs#L47) | Gets the normalized project root directory. |
| [`void Inno.Editor.Core.EditorContext.SaveLayout()`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorContext.cs#L180) | Atomically rewrites the complete project editor layout document. |
| [`void Inno.Editor.Core.EditorContext.SetImGuiLayout(string? value)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorContext.cs#L153) | Replaces the Dear ImGui layout while retaining editor module and panel state sections. |
| [`void Inno.Editor.Core.EditorContext.SetLayoutSection(string sectionName, System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, string>> values)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorContext.cs#L125) | Adds or replaces one human-readable editor layout section in memory. |

### `Inno.Editor.Core.EditorFrame`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Core.EditorFrame`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorFrame.cs#L15) | Describes one immutable editor frame. |

### `Inno.Editor.Core.EditorKeyboardPolicy`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Input.KeyModifier Inno.Editor.Core.EditorKeyboardPolicy.primaryModifier`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorKeyboardPolicy.cs#L37) | Gets the modifier used when resolving symbolic primary shortcuts. |
| [`Inno.Editor.Core.EditorKeyboardPolicy`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorKeyboardPolicy.cs#L9) | Supplies immutable keyboard conventions selected by the Editor product composition. |
| [`Inno.Editor.Core.EditorKeyboardPolicy.EditorKeyboardPolicy(Inno.Core.Input.KeyModifier primaryModifier, string superModifierLabel)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorKeyboardPolicy.cs#L23) | Defines the primary shortcut modifier and the display name of the Super modifier. |
| [`string Inno.Editor.Core.EditorKeyboardPolicy.superModifierLabel`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorKeyboardPolicy.cs#L42) | Gets the product-selected display name of the Super modifier. |

### `Inno.Editor.Core.EditorModal`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Core.EditorModal`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModal.cs#L10) | Defines non-dockable modal editor content. |
| [`abstract bool Inno.Editor.Core.EditorModal.isVisible`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModal.cs#L15) | Gets whether the modal should currently be visible. |
| [`abstract void Inno.Editor.Core.EditorModal.OnDraw(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModal.cs#L72) | Draws the modal body inside the runtime-managed centered window. |
| [`virtual System.Numerics.Vector2 Inno.Editor.Core.EditorModal.initialSize`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModal.cs#L42) | Gets the initial modal size in unscaled editor units, or when the runtime should size the modal from its content. |
| [`virtual System.Numerics.Vector2 Inno.Editor.Core.EditorModal.minimumSize`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModal.cs#L48) | Gets the minimum modal size in unscaled editor units, or when no explicit minimum is required. |
| [`virtual bool Inno.Editor.Core.EditorModal.allowScrolling`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModal.cs#L36) | Gets whether the modal window owns scrolling when its content overflows. Disable this when a child region owns all scrolling. |
| [`virtual bool Inno.Editor.Core.EditorModal.blocksInteraction`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModal.cs#L20) | Gets whether the modal prevents interaction with regular editor views. |
| [`virtual bool Inno.Editor.Core.EditorModal.canMove`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModal.cs#L25) | Gets whether the modal window can be moved inside the main viewport. |
| [`virtual bool Inno.Editor.Core.EditorModal.canResize`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModal.cs#L30) | Gets whether the modal window can be resized by the user. |
| [`void Inno.Editor.Core.EditorModal.Draw(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModal.cs#L59) | Draws the modal body inside the runtime-managed centered window. |

### `Inno.Editor.Core.EditorModalAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Core.EditorModalAttribute`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModalAttribute.cs#L8) | Registers a centered editor modal for automatic discovery. |
| [`Inno.Editor.Core.EditorModalAttribute.EditorModalAttribute(string id, string title, int order = 0)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModalAttribute.cs#L26) | Creates a centered modal registration with a stable identity and draw order. |
| [`int Inno.Editor.Core.EditorModalAttribute.order`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModalAttribute.cs#L53) | Gets the stable draw order. |
| [`string Inno.Editor.Core.EditorModalAttribute.id`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModalAttribute.cs#L43) | Gets the stable modal identifier. |
| [`string Inno.Editor.Core.EditorModalAttribute.title`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModalAttribute.cs#L48) | Gets the visible modal title. |

### `Inno.Editor.Core.EditorModule`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Core.EditorModule`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModule.cs#L13) | Owns optional shared state and lifecycle for one editor feature. Simple panels and actions do not need a module. |
| [`virtual bool Inno.Editor.Core.EditorModule.blocksFollowingUpdates`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModule.cs#L28) | Gets whether modules ordered after this module must defer their updates for the current frame. |
| [`virtual void Inno.Editor.Core.EditorModule.Capture(Inno.Editor.Core.EditorState state)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModule.cs#L172) | Captures readable project state owned by this module. |
| [`virtual void Inno.Editor.Core.EditorModule.OnDispose()`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModule.cs#L195) | Releases resources owned by this module after it has stopped and left the active extension generation. |
| [`virtual void Inno.Editor.Core.EditorModule.OnStart(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModule.cs#L133) | Runs after the module generation becomes active and before its first update. |
| [`virtual void Inno.Editor.Core.EditorModule.OnStop(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModule.cs#L157) | Runs before the module generation is released and its disposable instances are destroyed. |
| [`virtual void Inno.Editor.Core.EditorModule.OnUpdate(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModule.cs#L143) | Runs once per editor frame before views are drawn. |
| [`virtual void Inno.Editor.Core.EditorModule.Restore(Inno.Editor.Core.EditorState state)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModule.cs#L187) | Restores readable project state owned by this module. |
| [`void Inno.Editor.Core.EditorModule.Start(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModule.cs#L45) | Starts the module after the containing extension generation becomes active. |
| [`void Inno.Editor.Core.EditorModule.Stop(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModule.cs#L101) | Stops the module before the containing extension generation is released. |
| [`void Inno.Editor.Core.EditorModule.Update(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModule.cs#L73) | Updates the module once per editor frame before panels and modals are drawn. |

### `Inno.Editor.Core.EditorModuleAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Core.EditorModuleAttribute`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModuleAttribute.cs#L8) | Registers an editor feature module with a stable identity for automatic discovery. |
| [`Inno.Editor.Core.EditorModuleAttribute.EditorModuleAttribute(string id, int order = 0)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModuleAttribute.cs#L24) | Creates a module registration with stable identity and deterministic lifecycle order. |
| [`int Inno.Editor.Core.EditorModuleAttribute.order`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModuleAttribute.cs#L42) | Gets the stable lifecycle order. |
| [`string Inno.Editor.Core.EditorModuleAttribute.id`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModuleAttribute.cs#L37) | Gets the stable module identifier. |

### `Inno.Editor.Core.EditorPanel`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Core.EditorPanel`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorPanel.cs#L13) | Base class for editor panel implementations. |
| [`abstract void Inno.Editor.Core.EditorPanel.OnDraw(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorPanel.cs#L131) | Draws the complete dockable contents of this panel for the current frame. |
| [`bool Inno.Editor.Core.EditorPanel.isOpen`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorPanel.cs#L41) | Gets or sets whether panel is visible. |
| [`virtual System.Numerics.Vector2 Inno.Editor.Core.EditorPanel.initialSize`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorPanel.cs#L36) | Gets the initial floating size in UI coordinates; zero leaves native auto-sizing in control. Saved layouts take precedence. |
| [`virtual bool Inno.Editor.Core.EditorPanel.allowScrolling`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorPanel.cs#L31) | Gets whether the presentation backend may create a scroll range for this panel window. |
| [`virtual bool Inno.Editor.Core.EditorPanel.useWindowPadding`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorPanel.cs#L22) | Gets whether the presentation backend should inset this panel body by its standard window padding. |
| [`virtual void Inno.Editor.Core.EditorPanel.Capture(Inno.Editor.Core.EditorState state)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorPanel.cs#L168) | Captures readable project state owned by this panel. |
| [`virtual void Inno.Editor.Core.EditorPanel.OnAttach(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorPanel.cs#L139) | Runs after the panel is attached to an active extension generation. |
| [`virtual void Inno.Editor.Core.EditorPanel.OnDetach(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorPanel.cs#L153) | Runs before the panel is detached from its active extension generation. |
| [`virtual void Inno.Editor.Core.EditorPanel.Restore(Inno.Editor.Core.EditorState state)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorPanel.cs#L183) | Restores readable project state owned by this panel. |
| [`void Inno.Editor.Core.EditorPanel.Attach(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorPanel.cs#L55) | Attaches the panel after its extension generation becomes active. |
| [`void Inno.Editor.Core.EditorPanel.Detach(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorPanel.cs#L81) | Detaches the panel before its extension generation is released. |
| [`void Inno.Editor.Core.EditorPanel.Draw(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorPanel.cs#L116) | Draws the complete dockable contents of the panel for the current frame. |

### `Inno.Editor.Core.EditorPanelAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Core.EditorPanelAttribute`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorPanelAttribute.cs#L8) | Registers an editor panel for automatic discovery. |
| [`Inno.Editor.Core.EditorPanelAttribute.EditorPanelAttribute(string id, string title, int order = 0, bool defaultOpen = true, string menuPath = "", bool separatorBefore = false)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorPanelAttribute.cs#L36) | Creates a dockable panel registration with stable identity and presentation metadata. |
| [`bool Inno.Editor.Core.EditorPanelAttribute.defaultOpen`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorPanelAttribute.cs#L74) | Gets whether a newly discovered panel is open by default. |
| [`bool Inno.Editor.Core.EditorPanelAttribute.separatorBefore`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorPanelAttribute.cs#L84) | Gets whether the generated panel toggle begins a new visual group. |
| [`int Inno.Editor.Core.EditorPanelAttribute.order`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorPanelAttribute.cs#L69) | Gets the stable panel ordering value. |
| [`string Inno.Editor.Core.EditorPanelAttribute.id`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorPanelAttribute.cs#L59) | Gets the stable panel identifier. |
| [`string Inno.Editor.Core.EditorPanelAttribute.menuPath`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorPanelAttribute.cs#L79) | Gets the optional category path under the generated Panel menu. |
| [`string Inno.Editor.Core.EditorPanelAttribute.title`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorPanelAttribute.cs#L64) | Gets the visible panel title. |

### `Inno.Editor.Core.EditorReloadCoordinator`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Core.EditorReloadCoordinator`](../../src/composition/editor/framework/Inno.Editor.Core/Reloading/EditorReloadCoordinator.cs#L16) | Coordinates weakly registered editor feature migrations around atomic assembly reload sessions. |
| [`Inno.Extensibility.Reload.IAssemblyUnloadProbe Inno.Editor.Core.EditorReloadCoordinator.Execute(Inno.Extensibility.Modules.AssemblyReloadSession reload, Inno.Extensibility.Reload.IGenerationChange? externalChange = null)`](../../src/composition/editor/framework/Inno.Editor.Core/Reloading/EditorReloadCoordinator.cs#L66) | Applies one prepared assembly reload together with every registered editor feature migration. |
| [`System.IDisposable Inno.Editor.Core.EditorReloadCoordinator.Register(Inno.Editor.Core.IEditorReloadParticipant participant)`](../../src/composition/editor/framework/Inno.Editor.Core/Reloading/EditorReloadCoordinator.cs#L34) | Registers an editor feature that owns generation-bound live state. |
| [`void Inno.Editor.Core.EditorReloadCoordinator.RefreshDiagnostics()`](../../src/composition/editor/framework/Inno.Editor.Core/Reloading/EditorReloadCoordinator.cs#L79) | Requests every live participant to republish diagnostics derived from its current state. |

### `Inno.Editor.Core.EditorRuntime`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Core.EditorContext Inno.Editor.Core.EditorRuntime.context`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorRuntime.cs#L27) | Gets the shared passive editor context. |
| [`Inno.Editor.Core.EditorRuntime`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorRuntime.cs#L8) | Defines the presentation-independent lifecycle of an editor runtime. |
| [`Inno.Editor.Core.EditorRuntime.EditorRuntime(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorRuntime.cs#L19) | Creates a runtime for the supplied passive context. |
| [`abstract void Inno.Editor.Core.EditorRuntime.Dispose()`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorRuntime.cs#L57) | Stops the runtime and releases active extensions. |
| [`abstract void Inno.Editor.Core.EditorRuntime.Start()`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorRuntime.cs#L32) | Starts the runtime and activates its initial extension generation. |
| [`abstract void Inno.Editor.Core.EditorRuntime.Update(Inno.Editor.Core.EditorFrame frame)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorRuntime.cs#L40) | Updates the runtime for one editor frame. |
| [`void Inno.Editor.Core.EditorRuntime.SetFrame(Inno.Editor.Core.EditorFrame frame)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorRuntime.cs#L48) | Advances frame-scoped data and publishes the latest immutable frame. |

### `Inno.Editor.Core.EditorState`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Core.EditorState`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorState.cs#L14) | Provides the serialization-neutral parameter used to capture or restore one editor extension's project state. |
| [`Inno.Editor.Core.EditorState.EditorState()`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorState.cs#L19) | Initializes the base contract for a runtime-owned editor state parameter. |
| [`abstract T Inno.Editor.Core.EditorState.Get<T>(string key, T fallback)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorState.cs#L42) | Reads a compatible value or returns the caller-provided fallback. |
| [`abstract void Inno.Editor.Core.EditorState.Set<T>(string key, T value)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorState.cs#L68) | Writes one neutral value under a stable extension-local key. |

### `Inno.Editor.Core.EditorStatistic`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Core.EditorStatistic`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L88) | Describes one immutable, presentation-ready statistic without retaining provider instances or types. |
| [`Inno.Editor.Core.EditorStatistic.EditorStatistic(Inno.Editor.Core.EditorStatisticId id, Inno.Editor.Core.EditorStatisticGroupId groupId, string groupName, string label, string value, int groupOrder = 0, int order = 0)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L117) | Creates one statistic contribution. |
| [`Inno.Editor.Core.EditorStatisticGroupId Inno.Editor.Core.EditorStatistic.groupId`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L150) | Gets the stable visual group identity. |
| [`Inno.Editor.Core.EditorStatisticId Inno.Editor.Core.EditorStatistic.id`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L145) | Gets the globally stable statistic identity. |
| [`int Inno.Editor.Core.EditorStatistic.groupOrder`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L170) | Gets ascending group presentation order. |
| [`int Inno.Editor.Core.EditorStatistic.order`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L175) | Gets ascending metric order within the group. |
| [`string Inno.Editor.Core.EditorStatistic.groupName`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L155) | Gets the user-facing group heading. |
| [`string Inno.Editor.Core.EditorStatistic.label`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L160) | Gets the user-facing metric label. |
| [`string Inno.Editor.Core.EditorStatistic.value`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L165) | Gets the presentation-ready value. |

### `Inno.Editor.Core.EditorStatisticGroupId`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Core.EditorStatisticGroupId`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L49) | Identifies one visual group in the editor statistics presentation. |
| [`Inno.Editor.Core.EditorStatisticGroupId.EditorStatisticGroupId(string value)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L60) | Creates a globally stable statistic-group identifier. |
| [`bool Inno.Editor.Core.EditorStatisticGroupId.isValid`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L74) | Gets whether this identifier contains a usable value. |
| [`override string Inno.Editor.Core.EditorStatisticGroupId.ToString()`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L82) | Formats this value as a human-readable representation. |
| [`string Inno.Editor.Core.EditorStatisticGroupId.value`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L69) | Gets the globally stable identity. |

### `Inno.Editor.Core.EditorStatisticId`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Core.EditorStatisticId`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L10) | Identifies one editor statistic across panels, modules, and reloadable extensions. |
| [`Inno.Editor.Core.EditorStatisticId.EditorStatisticId(string value)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L21) | Creates a globally stable statistic identifier. |
| [`bool Inno.Editor.Core.EditorStatisticId.isValid`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L35) | Gets whether this identifier contains a usable value. |
| [`override string Inno.Editor.Core.EditorStatisticId.ToString()`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L43) | Formats this value as a human-readable representation. |
| [`string Inno.Editor.Core.EditorStatisticId.value`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L30) | Gets the globally stable identity. |

### `Inno.Editor.Core.EditorStatistics`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Core.EditorStatistics`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L185) | Exchanges frame-scoped, reload-safe statistics between independent editor features and viewers. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Editor.Core.EditorStatistic> Inno.Editor.Core.EditorStatistics.GetSnapshot()`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L230) | Gets a deterministic snapshot combining current contributions with the previous completed frame for providers that draw after their consumer. |
| [`void Inno.Editor.Core.EditorStatistics.Publish(Inno.Editor.Core.EditorStatistic statistic)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L200) | Publishes or replaces one statistic for the current editor frame. |
| [`void Inno.Editor.Core.EditorStatistics.Publish(System.Collections.Generic.IEnumerable<Inno.Editor.Core.EditorStatistic> statistics)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L216) | Publishes or replaces several statistics for the current editor frame. |

### `Inno.Editor.Core.IEditorPanelReloadState`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Core.IEditorPanelReloadState`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/IEditorPanelReloadState.cs#L8) | Optionally migrates presentation-neutral panel state across assembly reloads. |
| [`System.ReadOnlyMemory<byte> Inno.Editor.Core.IEditorPanelReloadState.CaptureReloadState()`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/IEditorPanelReloadState.cs#L16) | Captures presentation-neutral state without retaining runtime or plugin object references. |
| [`void Inno.Editor.Core.IEditorPanelReloadState.RestoreReloadState(System.ReadOnlyMemory<byte> state)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/IEditorPanelReloadState.cs#L24) | Restores presentation-neutral state captured from the previous panel generation. |

### `Inno.Editor.Core.IEditorReloadParticipant`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Core.IEditorReloadParticipant`](../../src/composition/editor/framework/Inno.Editor.Core/Reloading/IEditorReloadParticipant.cs#L10) | Contributes editor-owned live state to an assembly reload without coupling the reload requester to a feature domain. |
| [`Inno.Extensibility.Reload.IGenerationChange Inno.Editor.Core.IEditorReloadParticipant.Capture(Inno.Extensibility.Modules.AssemblyReloadContext context)`](../../src/composition/editor/framework/Inno.Editor.Core/Reloading/IEditorReloadParticipant.cs#L21) | Captures one isolated transaction for the prepared assembly generation. |
| [`void Inno.Editor.Core.IEditorReloadParticipant.RefreshDiagnostics()`](../../src/composition/editor/framework/Inno.Editor.Core/Reloading/IEditorReloadParticipant.cs#L26) | Republishes diagnostics derived from the participant's current live state. |

## 项目依赖

- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Modules](../extensibility/Inno.Extensibility.Modules.md)：公开引用边界由实际签名核对。
- [Inno.Scripting.Reload](../scripting/Inno.Scripting.Reload.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Reload](../extensibility/Inno.Extensibility.Reload.md)：公开引用边界由实际签名核对。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
- [Inno.Core.Input](../core/Inno.Core.Input.md)：公开引用边界由实际签名核对。
