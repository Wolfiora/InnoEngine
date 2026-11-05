# Inno.Editor.Core

[Editor 索引](README.md) · [Interactions](Inno.Editor.Interactions.md) · [Wiki 首页](../../README.md)

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

Commands、Menus、DragDrop、Selection 和 Rename 状态属于 `Inno.Editor.Interactions` 或对应 Panel。

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

`EditorStatistics.GetSnapshot()` 返回按稳定 group/item 顺序排列的不可变快照。内容和排序不变时复用同一实例，旧快照不会被后续发布修改。重复发布相同值不会迫使消费 Panel 重建分组；当前帧与上一完成帧的 handoff 仍保持，停止发布的项按原帧生命周期退出。发布和快照操作由同一内部同步边界保护，不保存扩展类型或回调。

`EditorContext` 是由 Application host 创建并注入扩展的中立数据，不承担路由：

Host 构造入口为 `EditorContext(string projectDirectory, EditorKeyboardPolicy keyboard, string? initialLayoutDocument = null)`；该构造函数不导出给脚本。可选初始文档由组合层提供，Core 不内置标准面板、停靠比例或任何项目内容。仅当 `editor.ini` 不存在时通过 Core IO `AtomicFile` 创建，已有文件（包括空文件）保持原始内容及时间戳；并发创建不覆盖先完成者。读取、权限或创建失败明确传播。未提供初始文档时，Context 保持被动，不在构造时创建文件。

普通保存继续使用同一份可读文档和原子替换；没有第二套 Writer、模板 Reader 或格式兼容分支。

```csharp
using System;
using Inno.Editor.Core;

static void PublishCount(
    EditorContext context,
    int playingClipCount
) {
    Console.WriteLine(context.projectDirectory);
    Console.WriteLine(context.frame.totalTime);
    context.statistics.Publish(new EditorStatistic(
        new EditorStatisticId("sample.animation.playing-clips"),
        new EditorStatisticGroupId("sample.animation"),
        "Animation", "Playing Clips", playingClipCount.ToString()));
}
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
using Inno.Editor.Core;

[EditorModule("example.frame-counter", order: 100)]
public sealed class FrameCounterModule : EditorModule
{
    public long frames { get; private set; }

    protected override void OnStart(EditorContext context) => frames = 0;
    protected override void OnUpdate(EditorContext context) => frames++;
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
using Inno.Editor.Core;

[EditorPanel("example.inspection", "Inspection", order: 500, defaultOpen: true, menuPath: "Authoring")]
public sealed class InspectionPanel : EditorPanel
{
    protected override void OnDraw(EditorContext context)
    {
        // The presentation-specific implementation draws only this panel's body.
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
using System;
using Inno.Editor.Core;

[EditorPanel("example.reload-state", "Reload State")]
public sealed class ReloadStatePanel : EditorPanel, IEditorPanelReloadState
{
    private byte[] m_stateBytes = [];

    public ReadOnlyMemory<byte> CaptureReloadState() => m_stateBytes;
    public void RestoreReloadState(ReadOnlyMemory<byte> state) => m_stateBytes = state.ToArray();
    protected override void OnDraw(EditorContext context) { }
}
```

Attach 同样先记录所有权：`OnAttach` 失败会调用 `OnDetach` 补偿，只有补偿成功才能把 Panel 隔离后继续。
Detach 完成后幂等；Pending 时 Panel 禁止 Draw，资源和依赖保持受拥有。不是把失败 Attach 当作从未启动。

## Modal

`EditorModal` 是被发现的阻塞或非阻塞浮层契约。具体位置、尺寸、淡入淡出和输入阻塞由 ImGui runtime 统一处理。

```csharp
using System.Numerics;
using Inno.Editor.Core;

[EditorModule("example.operation")]
public sealed class OperationModule : EditorModule
{
    public bool isRunning { get; set; }
}

[EditorModal("example.operation-progress", "Operation Progress", order: 200)]
public sealed class OperationModal(OperationModule operation) : EditorModal
{
    public override bool isVisible => operation.isRunning;
    public override bool blocksInteraction => true;
    public override bool canMove => true;
    public override bool canResize => true;
    public override Vector2 initialSize => new(900f, 600f);
    public override Vector2 minimumSize => new(640f, 420f);

    protected override void OnDraw(EditorContext context)
    {
        // Draw progress inside the runtime-managed modal body.
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
using System;
using Inno.Editor.Core;

[EditorModule("example.navigation")]
public sealed class NavigationModule : EditorModule
{
    private Guid m_documentId;
    private float m_zoom = 1f;

    protected override void Capture(EditorState state)
    {
        state.Set("document", m_documentId);
        state.Set("zoom", m_zoom);
    }

    protected override void Restore(EditorState state)
    {
        m_documentId = state.Get("document", Guid.Empty);
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

## Reload 准入与执行作用域

`EditorReloadCoordinator.RegisterGuard` 返回调用者拥有的注册 lease；coordinator 只弱引用 guard。PlayMode 注册 guard，Scripting 仅依赖 Core 契约。手动请求与编译/激活分别查询，菜单显示和实际执行都检查当前状态。

Playing、Preparing、Stopping 禁止手动 Reload；自动变化保留在现有队列，完整回到 Editing 后处理。进入 Play 前的编译可以推进，Play Session 创建后固定 generation 读取权，退休完成后才重新允许发布。guard 不主动停止游戏，不替代 Pending/Faulted gate。

Editor 模块更新与 Reload 事务使用 Edit Session；Play Tick 和运行对象操作进入所属 Play Session。Scene 事务每个阶段独立进入 owner scope，Identity 替换不依赖 UI 当前 ambient domain。



## 不依赖绘制的名称格式化

`EditorDisplayNames.Nicify` 为菜单、Inspector 与插件控件格式化标识符，不需要 ImGui session 或原生 context。空白输入得到空标签；下划线、连字符及小写到大写的边界产生空格。

## Modal 的本帧准备契约

内容自适应 Modal 通过 protected `OnPrepare(EditorContext, Vector2)` 捕获本帧状态并返回所需内容尺寸。输入、输出均为已经应用当前尺度的呈现坐标，不再乘 Zoom。高度未明确声明时必须测量；无效或缺失结果会隔离该扩展并报告诊断。

内容自适应窗口在打开前准备；明确尺寸的可调整窗口在原生输入确定本帧内容区之后准备。两种策略每次呈现都只准备一次、绘制一次。准备只能读取快照和测量，不能构建、发布、取消或修改 History；用户动作在绘制后交回原业务 owner。

`initialSize` 的两个维度分别解释：正宽度即使配合零高度也有效；零宽度使用通用默认宽度；零高度使用本帧测量。显式尺寸和用户可调整尺寸保持自己的策略。公开 `Prepare` 与 `Draw` 是框架驱动入口，继续从脚本 API 排除；派生扩展实现 protected hooks。

## API 参考

[完整声明、行为与调用约束](Inno.Editor.Core/api.md)

<!-- api-include: Inno.Editor.Core/api.md -->

## 项目依赖

| 工程 | 边界 |
| --- | --- |
| `Inno.Scripting.Api` | 实现依赖；不传递 compile surface |
| `Inno.Core.Execution` | 实现依赖；不传递 compile surface |
| `Inno.Extensibility.Modules` | 公开签名依赖 |
| `Inno.Scripting.Reload` | 公开签名依赖 |
| `Inno.Extensibility.Reload` | 公开签名依赖 |
| `Inno.Core.Serialization` | 公开签名依赖 |
| `Inno.Extensibility.Catalogs` | 公开签名依赖 |
| `Inno.Core.Input` | 公开签名依赖 |
