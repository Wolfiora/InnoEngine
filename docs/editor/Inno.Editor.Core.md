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

`EditorContext` 是由 Application host 创建并注入扩展的中立数据，不承担路由：

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

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Editor.Core.EditorContext`

<!-- api: T:Inno.Editor.Core.EditorContext 2ab755a33912446f2a8ce3d84771ede1755d46f9cddc7f8cd6eb189b485ec517 -->
<!-- api: M:Inno.Editor.Core.EditorContext.#ctor(System.String,Inno.Editor.Core.EditorKeyboardPolicy) 8d1a42ee558b59f0d4af487594839b84cad46e9c52c8701a019cd7d2a8b6d974 -->
<!-- api: P:Inno.Editor.Core.EditorContext.projectDirectory 0e4a7d632f8da73e02ef47aff686d69bb379251a227f9e4434c1001227dc778b -->
<!-- api: P:Inno.Editor.Core.EditorContext.keyboard e05c2786e601cf36e8a5b9195251f7d8a8b40bf88555ff8ca53a4647aa9b755a -->
<!-- api: P:Inno.Editor.Core.EditorContext.statistics c0b2ae92df07f8a75a39c9b46f3df2d7879192b44ed47fdc60440774ae633a06 -->
<!-- api: P:Inno.Editor.Core.EditorContext.layoutPath a925c5f662b399c088a81bd0d7f68668242bc69b3f14572a6d00c338be327421 -->
<!-- api: P:Inno.Editor.Core.EditorContext.imguiLayout f51cf25ff507f4a4d5ad64272b164d4de1be46191d10b4c0fd4e7095fdcd2254 -->
<!-- api: M:Inno.Editor.Core.EditorContext.GetLayoutSectionNames(System.String) 3d18e7da136bf4e059fa3ac9d060cae15b9ec350c79abe4075c1f7674352c3ab -->
<!-- api: M:Inno.Editor.Core.EditorContext.TryGetLayoutSection(System.String,System.Collections.Generic.IReadOnlyDictionary{System.String,System.String}@) e090d585eb5f639b6edd99f6459b8eef6f20766227bfbfd57b3027e53e40279a -->
<!-- api: M:Inno.Editor.Core.EditorContext.SetLayoutSection(System.String,System.Collections.Generic.IEnumerable{System.Collections.Generic.KeyValuePair{System.String,System.String}}) 1030f7fb77e1af88e975736482acb3a1d30a1c6fbe3a1ebfff3c9b1cb691ac00 -->
<!-- api: M:Inno.Editor.Core.EditorContext.RemoveLayoutSection(System.String) efaab9e10b1345cf13d4446942343a9fd4811a2ae87bb493d83f83283f7cb56b -->
<!-- api: M:Inno.Editor.Core.EditorContext.SetImGuiLayout(System.String) 82d910fb612194b742ecc4d1a60774360f70210fff1b3c3a996bc82030513bd9 -->
<!-- api: M:Inno.Editor.Core.EditorContext.SaveLayoutIfChanged 2098883faea2849abaea3ea79e6b75f150697e77892c3d8645d21ad92b3218f6 -->
<!-- api: M:Inno.Editor.Core.EditorContext.SaveLayout e9ac474b3c3ef26d6280cb45a0851f8a7eed35ef602f19c1c00c280b18fc28cf -->
<!-- api: P:Inno.Editor.Core.EditorContext.frame 24fe3f41524d6813fc6b4e27b2f94cbad72c72ccc6b0591907a6d41ae42f799c -->
<!-- api: P:Inno.Editor.Core.EditorContext.isFocused 4a724009884d87769c555cd94ce0856e6f074e8710e2e773157cd9c13527476b -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-f0cc99bb6c5fdf2f"></a>[`public Inno.Editor.Core.EditorContext.EditorContext(string projectDirectory, Inno.Editor.Core.EditorKeyboardPolicy keyboard)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorContext.cs#L31) | Creates a passive editor context for one project.<br>`projectDirectory`：The project root containing Assets and Library.<br>`keyboard`：The explicit immutable conventions selected by product composition. | — | `System.ArgumentNullException`：The keyboard policy is null.<br>`System.ArgumentException`：Thrown when projectDirectory is empty. |
| <a id="api-7b8718c401fb7094"></a>[`public System.Collections.Generic.IReadOnlyList<string> Inno.Editor.Core.EditorContext.GetLayoutSectionNames(string prefix = "")`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorContext.cs#L86) | Gets layout section names matching an ordinal prefix.<br>返回：A stable sorted snapshot of matching section names.<br>`prefix`：The prefix to match, or an empty string for every section. | — | `System.ArgumentNullException`：Thrown when prefix is null. |
| <a id="api-885b681ef7923882"></a>[`public bool Inno.Editor.Core.EditorContext.RemoveLayoutSection(string sectionName)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorContext.cs#L145) | Removes one editor layout section from the in-memory document.<br>返回：true when an existing section was removed.<br>`sectionName`：The section name without its INI header syntax. | — | `System.ArgumentException`：Thrown when sectionName is empty or cannot be represented in the layout. |
| <a id="api-557494db2acd998e"></a>[`public void Inno.Editor.Core.EditorContext.SaveLayout()`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorContext.cs#L181) | Atomically rewrites the complete project editor layout document. | — | `System.IO.IOException`：Thrown when the layout document cannot be written atomically.<br>`System.UnauthorizedAccessException`：Thrown when the layout document is inaccessible. |
| <a id="api-41aff91a14b50bfc"></a>[`public bool Inno.Editor.Core.EditorContext.SaveLayoutIfChanged()`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorContext.cs#L169) | Atomically saves the project editor layout when it changed.<br>返回：true when a changed document was written. | — | `System.IO.IOException`：Thrown when the layout document cannot be written atomically.<br>`System.UnauthorizedAccessException`：Thrown when the layout document is inaccessible. |
| <a id="api-41518e0ff8a33d3a"></a>[`public void Inno.Editor.Core.EditorContext.SetImGuiLayout(string? value)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorContext.cs#L154) | Replaces the Dear ImGui layout while retaining editor module and panel state sections.<br>`value`：The complete layout text returned by Dear ImGui. | — | — |
| <a id="api-9d8d869bb3aebbaa"></a>[`public void Inno.Editor.Core.EditorContext.SetLayoutSection(string sectionName, System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, string>> values)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorContext.cs#L126) | Adds or replaces one human-readable editor layout section in memory.<br>`sectionName`：The section name without its INI header syntax.<br>`values`：The complete scalar or JSON-formatted values. | — | `System.ArgumentNullException`：Thrown when values is null.<br>`System.ArgumentException`：Thrown when a section name, key, or value cannot be represented in the layout. |
| <a id="api-883f09ae900dca9f"></a>[`public bool Inno.Editor.Core.EditorContext.TryGetLayoutSection(string sectionName, out System.Collections.Generic.IReadOnlyDictionary<string, string> values)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorContext.cs#L104) | Tries to read one independent editor layout section snapshot.<br>返回：true when the requested section exists.<br>`sectionName`：The section name without its INI header syntax.<br>`values`：The copied values when the section exists. | — | `System.ArgumentException`：Thrown when sectionName is empty or cannot be represented in the layout. |
| <a id="api-3f23ac3ab6a67183"></a>[`public Inno.Editor.Core.EditorFrame Inno.Editor.Core.EditorContext.frame { get; internal set; }`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorContext.cs#L186) | Gets the latest immutable editor frame snapshot. | — | — |
| <a id="api-383eaf0ea93542e4"></a>[`public string Inno.Editor.Core.EditorContext.imguiLayout { get; }`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorContext.cs#L69) | Gets the Dear ImGui layout text without editor module or panel state sections. | — | — |
| <a id="api-9004d9bbf0274e21"></a>[`public bool Inno.Editor.Core.EditorContext.isFocused { get; }`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorContext.cs#L191) | Gets whether any editor viewport currently owns application focus. | — | — |
| <a id="api-b4ea832b3b012b54"></a>[`public Inno.Editor.Core.EditorKeyboardPolicy Inno.Editor.Core.EditorContext.keyboard { get; }`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorContext.cs#L52) | Gets the immutable keyboard conventions shared by discovery, dispatch and presentation. | — | — |
| <a id="api-27672bc5e6d79e29"></a>[`public string Inno.Editor.Core.EditorContext.layoutPath { get; }`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorContext.cs#L63) | Gets the absolute path of the project editor layout document. | — | — |
| <a id="api-357bc4fe3ec63570"></a>[`public string Inno.Editor.Core.EditorContext.projectDirectory { get; }`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorContext.cs#L47) | Gets the normalized project root directory. | — | — |
| <a id="api-b42b3d2d9d9be9b6"></a>[`public Inno.Editor.Core.EditorStatistics Inno.Editor.Core.EditorContext.statistics { get; }`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorContext.cs#L57) | Gets the frame-scoped statistics exchange used by independent editor features and viewers. | — | — |
| <a id="api-34fe6aefc4b257f3"></a>[`public sealed class Inno.Editor.Core.EditorContext`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorContext.cs#L13) | Provides passive project and frame state shared by editor extensions. Interaction routing is supplied separately by Inno.Editor.Interactions. | — | — |

### `Inno.Editor.Core.EditorFrame`

<!-- api: T:Inno.Editor.Core.EditorFrame e0f6e10611a987754c935378a84c2d3bfadea4828a38399358879269431321ef -->
<!-- api: M:Inno.Editor.Core.EditorFrame.#ctor(System.Single,System.Single,System.Boolean) 91371509d5abffdc7e1276d49f3a86e65f800fa39a2f1f0005e5e2fa0720abfe -->
<!-- api: P:Inno.Editor.Core.EditorFrame.deltaTime 1f0c136b7460b915b0fb483a405245435a8116ad53120807e88c6eccff661f88 -->
<!-- api: P:Inno.Editor.Core.EditorFrame.totalTime d706c0c4f657ce72bdc1653ba2555171ef601b76ec97e0cbc1ef016ef4ad9089 -->
<!-- api: P:Inno.Editor.Core.EditorFrame.isFocused c75286e4c7da90a1fd02f85b7c85586ae22e46b42858dfa71274e4b1fc250a0e -->
<!-- api: M:Inno.Editor.Core.EditorFrame.ToString c398769f9cb9b1d7ea027b655e60d948e30fede8380fef71d3d11785d6b867a3 -->
<!-- api: M:Inno.Editor.Core.EditorFrame.op_Inequality(Inno.Editor.Core.EditorFrame,Inno.Editor.Core.EditorFrame) c30d4ef4edf8327b3a6344b2c7f87ff184bee3fc8e34f9ef0b714c4ef1f4805a -->
<!-- api: M:Inno.Editor.Core.EditorFrame.op_Equality(Inno.Editor.Core.EditorFrame,Inno.Editor.Core.EditorFrame) 51b39701de12324b6128ab2b797af95d80d1b21331164578b794384988781c6a -->
<!-- api: M:Inno.Editor.Core.EditorFrame.GetHashCode 59155ff711ec617ef5cb37361429464219000599e9747588e43676e09e424ecd -->
<!-- api: M:Inno.Editor.Core.EditorFrame.Equals(System.Object) 05c0276d9acdb66f24d980754c146dd7ae6c247c0ff542b00b931a6755b1839e -->
<!-- api: M:Inno.Editor.Core.EditorFrame.Equals(Inno.Editor.Core.EditorFrame) 1eaa7d0a076b25b76ef6212081d336c76af423485e68d785561260ee8ca0d9be -->
<!-- api: M:Inno.Editor.Core.EditorFrame.Deconstruct(System.Single@,System.Single@,System.Boolean@) 7412c3928339178f8936277accbb73dac6de03c2c18921718a67242199943d11 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-4dbe95097f6c24d6"></a>[`public Inno.Editor.Core.EditorFrame.EditorFrame(float deltaTime, float totalTime, bool isFocused)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorFrame.cs#L15) | Describes one immutable editor frame.<br>`deltaTime`：The elapsed frame time in seconds.<br>`totalTime`：The absolute editor runtime in seconds.<br>`isFocused`：Whether an editor viewport owns application focus. | — | — |
| <a id="api-45860fffd12b7fb9"></a>[`public void Inno.Editor.Core.EditorFrame.Deconstruct(out float deltaTime, out float totalTime, out bool isFocused)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorFrame.cs#L15) | C# record 生成的解构入口，按主构造声明顺序返回位置成员。 | — | — |
| <a id="api-101cda5a5a8b4e66"></a>[`public bool Inno.Editor.Core.EditorFrame.Equals(Inno.Editor.Core.EditorFrame other)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorFrame.cs#L15) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-4b15d12fc73da6e6"></a>[`public override bool Inno.Editor.Core.EditorFrame.Equals(object obj)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorFrame.cs#L15) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-b5f23f20b78a2f8c"></a>[`public override int Inno.Editor.Core.EditorFrame.GetHashCode()`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorFrame.cs#L15) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-e41a80ae2e687e5f"></a>[`public static bool Inno.Editor.Core.EditorFrame.operator ==(Inno.Editor.Core.EditorFrame left, Inno.Editor.Core.EditorFrame right)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorFrame.cs#L15) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-cdacac4e6381642b"></a>[`public static bool Inno.Editor.Core.EditorFrame.operator !=(Inno.Editor.Core.EditorFrame left, Inno.Editor.Core.EditorFrame right)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorFrame.cs#L15) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-457d39037cc54a32"></a>[`public override string Inno.Editor.Core.EditorFrame.ToString()`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorFrame.cs#L15) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-599550f6a5aa2c33"></a>[`public float Inno.Editor.Core.EditorFrame.deltaTime { get; init; }`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorFrame.cs#L16) | The elapsed frame time in seconds. | — | — |
| <a id="api-38b418164c61eebd"></a>[`public bool Inno.Editor.Core.EditorFrame.isFocused { get; init; }`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorFrame.cs#L18) | Whether an editor viewport owns application focus. | — | — |
| <a id="api-834cd4cd9b3abf0d"></a>[`public float Inno.Editor.Core.EditorFrame.totalTime { get; init; }`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorFrame.cs#L17) | The absolute editor runtime in seconds. | — | — |
| <a id="api-2ca91ef48ac254c4"></a>[`public readonly record struct Inno.Editor.Core.EditorFrame : System.IEquatable<Inno.Editor.Core.EditorFrame>`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorFrame.cs#L15) | Describes one immutable editor frame.<br>`deltaTime`：The elapsed frame time in seconds.<br>`totalTime`：The absolute editor runtime in seconds.<br>`isFocused`：Whether an editor viewport owns application focus. | — | — |

### `Inno.Editor.Core.EditorKeyboardPolicy`

<!-- api: T:Inno.Editor.Core.EditorKeyboardPolicy 600bfcdb407e066a80657dd9afae9bd829c7c135497b229448a521aa4681a38f -->
<!-- api: M:Inno.Editor.Core.EditorKeyboardPolicy.#ctor(Inno.Core.Input.KeyModifier,System.String) dfbbb99a5346aa9e30c135826aac305193bb81a71889723a9cdc89cc5fee3edf -->
<!-- api: P:Inno.Editor.Core.EditorKeyboardPolicy.primaryModifier 6245617538b6b6a679a81fd5d72a2a0162e6e09217020c38543de22c98feb03c -->
<!-- api: P:Inno.Editor.Core.EditorKeyboardPolicy.superModifierLabel a07b9502e437548ec542344200870bb0b353121c5c6a1d71a59eee36b2ece29d -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-70a17f6286ebeffe"></a>[`public Inno.Editor.Core.EditorKeyboardPolicy.EditorKeyboardPolicy(Inno.Core.Input.KeyModifier primaryModifier, string superModifierLabel)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorKeyboardPolicy.cs#L23) | Defines the primary shortcut modifier and the display name of the Super modifier.<br>`primaryModifier`：One Control, Super or Alt modifier used by symbolic primary shortcuts.<br>`superModifierLabel`：The nonempty product-specific name displayed for the Super modifier. | — | `System.ArgumentException`：The modifier is not one supported modifier or the label is empty. |
| <a id="api-3d67bc832c8c2830"></a>[`public Inno.Core.Input.KeyModifier Inno.Editor.Core.EditorKeyboardPolicy.primaryModifier { get; }`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorKeyboardPolicy.cs#L37) | Gets the modifier used when resolving symbolic primary shortcuts. | — | — |
| <a id="api-3fee6f0681d812aa"></a>[`public string Inno.Editor.Core.EditorKeyboardPolicy.superModifierLabel { get; }`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorKeyboardPolicy.cs#L42) | Gets the product-selected display name of the Super modifier. | — | — |
| <a id="api-8db049cf92c63d29"></a>[`public sealed class Inno.Editor.Core.EditorKeyboardPolicy`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorKeyboardPolicy.cs#L9) | Supplies immutable keyboard conventions selected by the Editor product composition. | — | — |

### `Inno.Editor.Core.EditorModal`

<!-- api: T:Inno.Editor.Core.EditorModal 2a40067ac02689dfe00115ebf96bd890945c4277bf60965649d6fe8d72e5bbed -->
<!-- api: P:Inno.Editor.Core.EditorModal.isVisible a2a27bb3a4339b3745b08e8388a49400ed4d92b16b82ddf4921fdea18b25509c -->
<!-- api: P:Inno.Editor.Core.EditorModal.blocksInteraction 676c00157894cf9834967f8132f0e257a10bf89826b569e1f808fe0771f0d051 -->
<!-- api: P:Inno.Editor.Core.EditorModal.canMove 0b23c61ac8b8bcf57dd11cca986892ef536a38234e84f3b34432bd7aed499f09 -->
<!-- api: P:Inno.Editor.Core.EditorModal.canResize 4890b81baa0ad46c09392109aaa043d8a2553cc1830538c380e5e0aad340b76f -->
<!-- api: P:Inno.Editor.Core.EditorModal.allowScrolling cae6b5e54ae8bd8e7af8e9f12e0968980019bc53e26b4a329eae480288f03b6e -->
<!-- api: P:Inno.Editor.Core.EditorModal.initialSize ca6967cd982fe5a56a07c3e71e87ce11606cf4b61929fa347f562638aa8ab3d5 -->
<!-- api: P:Inno.Editor.Core.EditorModal.minimumSize a545e77a535b922cc39423df49c470098ae694f0f4ea5a58d8a6972594e670ad -->
<!-- api: M:Inno.Editor.Core.EditorModal.Draw(Inno.Editor.Core.EditorContext) 55d9d80a2810cdf651e6c00f82fb60850a2388e3fc88202f51725d1c301c452f -->
<!-- api: M:Inno.Editor.Core.EditorModal.OnDraw(Inno.Editor.Core.EditorContext) 4bea4448e68bf29ede57d2837a3d69564b4cab8af4cf3fe7a0c77ca4487fd8c0 -->
<!-- api: M:Inno.Editor.Core.EditorModal.#ctor aba9c3da5a3bc755a59b7fd76d500347a3b51a192bcbb0a3dafc291b5a1ef6aa -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-7800f1179f76dd4e"></a>[`protected Inno.Editor.Core.EditorModal.EditorModal()`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModal.cs#L10) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-7dbc34246e1674aa"></a>[`public void Inno.Editor.Core.EditorModal.Draw(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModal.cs#L60) | Draws the modal body inside the runtime-managed centered window.<br>`context`：The shared editor context containing current project and frame state. | — | `System.ArgumentNullException`：Thrown when context is null. |
| <a id="api-e137f94422e60641"></a>[`protected abstract void Inno.Editor.Core.EditorModal.OnDraw(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModal.cs#L72) | Draws the modal body inside the runtime-managed centered window.<br>`context`：The shared editor context containing current project and frame state. | — | — |
| <a id="api-df7f566cfe2a898d"></a>[`public virtual bool Inno.Editor.Core.EditorModal.allowScrolling { get; }`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModal.cs#L36) | Gets whether the modal window owns scrolling when its content overflows. Disable this when a child region owns all scrolling. | — | — |
| <a id="api-f74f7d33f23907e6"></a>[`public virtual bool Inno.Editor.Core.EditorModal.blocksInteraction { get; }`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModal.cs#L20) | Gets whether the modal prevents interaction with regular editor views. | — | — |
| <a id="api-3394467651cdb298"></a>[`public virtual bool Inno.Editor.Core.EditorModal.canMove { get; }`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModal.cs#L25) | Gets whether the modal window can be moved inside the main viewport. | — | — |
| <a id="api-7e33c68163a29877"></a>[`public virtual bool Inno.Editor.Core.EditorModal.canResize { get; }`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModal.cs#L30) | Gets whether the modal window can be resized by the user. | — | — |
| <a id="api-8cdda07a429d4421"></a>[`public virtual System.Numerics.Vector2 Inno.Editor.Core.EditorModal.initialSize { get; }`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModal.cs#L42) | Gets the initial modal size in unscaled editor units, or System.Numerics.Vector2.Zero when the runtime should size the modal from its content. | — | — |
| <a id="api-93e533277a2addb1"></a>[`public abstract bool Inno.Editor.Core.EditorModal.isVisible { get; }`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModal.cs#L15) | Gets whether the modal should currently be visible. | — | — |
| <a id="api-a31e36fe5ced1e55"></a>[`public virtual System.Numerics.Vector2 Inno.Editor.Core.EditorModal.minimumSize { get; }`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModal.cs#L48) | Gets the minimum modal size in unscaled editor units, or System.Numerics.Vector2.Zero when no explicit minimum is required. | — | — |
| <a id="api-9af4611dd1558a67"></a>[`public abstract class Inno.Editor.Core.EditorModal`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModal.cs#L10) | Defines non-dockable modal editor content. | — | — |

### `Inno.Editor.Core.EditorModalAttribute`

<!-- api: T:Inno.Editor.Core.EditorModalAttribute d02c51f431fd3a38ab5c9872a4d576bde801d3695533b6b55a664017895e5b70 -->
<!-- api: M:Inno.Editor.Core.EditorModalAttribute.#ctor(System.String,System.String,System.Int32) d617764bf5186d39e28663fafd2bb4c2d5722152ea0a93db94f1896cbb1ceebf -->
<!-- api: P:Inno.Editor.Core.EditorModalAttribute.id 1d322a019d3056d7c1715557c6fa9102e95be4559543ef2d7fa7b42c6d27d319 -->
<!-- api: P:Inno.Editor.Core.EditorModalAttribute.title 90af8e83fb50e2ed06534a726ba62ebcd3cc707d3f1d849dbf8ce53a56c33f85 -->
<!-- api: P:Inno.Editor.Core.EditorModalAttribute.order 6dba366007ed926143da78d18cea15cb459167cdf011cac05b6b1c3964876d3d -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b17e68547d365dcf"></a>[`public Inno.Editor.Core.EditorModalAttribute.EditorModalAttribute(string id, string title, int order = 0)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModalAttribute.cs#L26) | Creates a centered modal registration with a stable identity and draw order.<br>`id`：The stable identity used to retain modal transition state across refreshes.<br>`title`：The visible title rendered by the modal host.<br>`order`：The stable ordering value when multiple modals are visible. | — | `System.ArgumentException`：Thrown when id or title is empty. |
| <a id="api-aea42db76177d0fb"></a>[`public string Inno.Editor.Core.EditorModalAttribute.id { get; }`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModalAttribute.cs#L43) | Gets the stable modal identifier. | — | — |
| <a id="api-773022be1873d0af"></a>[`public int Inno.Editor.Core.EditorModalAttribute.order { get; }`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModalAttribute.cs#L53) | Gets the stable draw order. | — | — |
| <a id="api-a129dd8705482597"></a>[`public string Inno.Editor.Core.EditorModalAttribute.title { get; }`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModalAttribute.cs#L48) | Gets the visible modal title. | — | — |
| <a id="api-ddf389a816fd560d"></a>[`public sealed class Inno.Editor.Core.EditorModalAttribute : System.Attribute`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModalAttribute.cs#L9) | Registers a centered editor modal for automatic discovery. | — | — |

### `Inno.Editor.Core.EditorModule`

<!-- api: T:Inno.Editor.Core.EditorModule 726212138efc98689b563c671352dc62b2c63c6782126587b843fb9b9e89d06a -->
<!-- api: P:Inno.Editor.Core.EditorModule.blocksFollowingUpdates fd053cc6e55ee447283c06570592573227caa2100bfee388fbf2106aa13a49de -->
<!-- api: M:Inno.Editor.Core.EditorModule.Start(Inno.Editor.Core.EditorContext) 445e4b4fe8dba14a0d0766dc9f042b4abf564b3d1fbfaa14b8318155d324c9fa -->
<!-- api: M:Inno.Editor.Core.EditorModule.Update(Inno.Editor.Core.EditorContext) 492a217661738609e95d3f2c8260420f0903004691469411bcbb50e7af878c51 -->
<!-- api: M:Inno.Editor.Core.EditorModule.Stop(Inno.Editor.Core.EditorContext) 9e903df9cd5292a68e63a963f85df057386b64b9ab7ac6a5a16b9172cc9def88 -->
<!-- api: M:Inno.Editor.Core.EditorModule.OnStart(Inno.Editor.Core.EditorContext) 7ac260f324d8fb387bdc88bb7794b2e410b3d5ac48489db7a246f161c1a93186 -->
<!-- api: M:Inno.Editor.Core.EditorModule.OnUpdate(Inno.Editor.Core.EditorContext) c2c9c14a515fa4df6df5c26861d213c6fd8e28b51ac0420e7d2db2238052dda6 -->
<!-- api: M:Inno.Editor.Core.EditorModule.OnStop(Inno.Editor.Core.EditorContext) 9256ab419c3636fbc989662ee1f7806b001880395b29294ce554020e969dbbb1 -->
<!-- api: M:Inno.Editor.Core.EditorModule.Capture(Inno.Editor.Core.EditorState) 2524d58e6491e9daa1e3e4b1e47f67f2396b27fb0d390fdc09d4a5840b979deb -->
<!-- api: M:Inno.Editor.Core.EditorModule.Restore(Inno.Editor.Core.EditorState) 17c56abf3337a5dfcedb7f1b19a7194045f1ec70f4d01fa8cca1e29e5f1756c3 -->
<!-- api: M:Inno.Editor.Core.EditorModule.OnDispose ca1c31f2f5ff0cf45a9afd3fc696b001b83f6c5765be172739c64e7d98d44ece -->
<!-- api: M:Inno.Editor.Core.EditorModule.#ctor 53a990d62940faf2b40d9ef8aa7afc9d3b1eaaad369429cfe1d03f43a4a28676 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-daec7896019a507c"></a>[`protected Inno.Editor.Core.EditorModule.EditorModule()`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModule.cs#L13) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-bf60119211c4d91b"></a>[`protected virtual void Inno.Editor.Core.EditorModule.Capture(Inno.Editor.Core.EditorState state)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModule.cs#L172) | Captures readable project state owned by this module.<br>`state`：The writable parameter that receives the complete readable state for this module. | Overriding this method opts the module into project-state IO. Modules that keep the base implementation are never registered with the persistence coordinator and therefore perform no state reads or writes. | — |
| <a id="api-dad64923f4e43ab5"></a>[`protected virtual void Inno.Editor.Core.EditorModule.OnDispose()`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModule.cs#L195) | Releases resources owned by this module after it has stopped and left the active extension generation. | — | — |
| <a id="api-5c93d28e44789d14"></a>[`protected virtual void Inno.Editor.Core.EditorModule.OnStart(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModule.cs#L133) | Runs after the module generation becomes active and before its first update.<br>`context`：The shared editor context for the active runtime. | — | — |
| <a id="api-f407acd300714b1f"></a>[`protected virtual void Inno.Editor.Core.EditorModule.OnStop(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModule.cs#L157) | Runs before the module generation is released and its disposable instances are destroyed.<br>`context`：The shared editor context for the runtime being stopped. | This includes partial startup compensation. Pending retirement must throw RetirementPendingException and preserve unfinished ownership; completed steps must be idempotent across retries. | — |
| <a id="api-ec65d912677fc113"></a>[`protected virtual void Inno.Editor.Core.EditorModule.OnUpdate(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModule.cs#L143) | Runs once per editor frame before views are drawn.<br>`context`：The shared editor context containing the current frame state. | — | — |
| <a id="api-3b912009674f83ec"></a>[`protected virtual void Inno.Editor.Core.EditorModule.Restore(Inno.Editor.Core.EditorState state)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModule.cs#L187) | Restores readable project state owned by this module.<br>`state`：The read-only state parameter for this module. Missing or incompatible values return the fallback supplied to Inno.Editor.Core.EditorState.Get``1(System.String,``0). | This method is called only when Inno.Editor.Core.EditorModule.Capture(Inno.Editor.Core.EditorState) is overridden. It runs once after the module is started and before the module is allowed to capture replacement state. | — |
| <a id="api-f4283f9848253786"></a>[`public void Inno.Editor.Core.EditorModule.Start(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModule.cs#L46) | Starts the module after the containing extension generation becomes active.<br>`context`：The shared editor context for the active runtime. | — | `System.ArgumentNullException`：Thrown when context is null.<br>`System.ObjectDisposedException`：Thrown after the module has been disposed.<br>`System.InvalidOperationException`：Thrown when the module is already started. |
| <a id="api-1e22a81d7dc08e2b"></a>[`public void Inno.Editor.Core.EditorModule.Stop(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModule.cs#L102) | Stops the module before the containing extension generation is released.<br>`context`：The shared editor context for the runtime being stopped. | Also compensates a partially failed Start. Completed stops are idempotent; pending stops may be retried. | `System.ArgumentNullException`：Thrown when context is null.<br>`System.ObjectDisposedException`：Thrown after the module has been disposed.<br>`Inno.Core.Execution.RetirementPendingException`：Live work still depends on this module. Its owner must retain it and retry Stop before disposal. |
| <a id="api-0b352784bd222871"></a>[`public void Inno.Editor.Core.EditorModule.Update(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModule.cs#L74) | Updates the module once per editor frame before panels and modals are drawn.<br>`context`：The shared editor context containing the current frame state. | — | `System.ArgumentNullException`：Thrown when context is null.<br>`System.ObjectDisposedException`：Thrown after the module has been disposed.<br>`System.InvalidOperationException`：Thrown when the module is not started. |
| <a id="api-d42d0970118884e6"></a>[`public virtual bool Inno.Editor.Core.EditorModule.blocksFollowingUpdates { get; }`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModule.cs#L28) | Gets whether modules ordered after this module must defer their updates for the current frame. | A module may return true while completing a project-wide bootstrap or atomic transition that later modules must not observe partially. Panels and modals remain drawable. | — |
| <a id="api-f2db28e069de36fd"></a>[`public abstract class Inno.Editor.Core.EditorModule : System.IDisposable`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModule.cs#L13) | Owns optional shared state and lifecycle for one editor feature. Simple panels and actions do not need a module. | — | — |

### `Inno.Editor.Core.EditorModuleAttribute`

<!-- api: T:Inno.Editor.Core.EditorModuleAttribute 8a9287dcbd8a488c28b6710196b34ac85ae3f9401c68c9470283926a09a0e022 -->
<!-- api: M:Inno.Editor.Core.EditorModuleAttribute.#ctor(System.String,System.Int32) ff52b7895ed6392d7fceded3a637b398c16d882f444a07e96d9a36607060a67d -->
<!-- api: P:Inno.Editor.Core.EditorModuleAttribute.id 09f8b6d5ee24e164eefec680a9ee50b1ca2dd687222224f1fd496285fd25644e -->
<!-- api: P:Inno.Editor.Core.EditorModuleAttribute.order 3afebae9a23967db296004bd27d7dc6ae862bcca179dbc6f05035a3ab386a41b -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-640ef6c928fa2e02"></a>[`public Inno.Editor.Core.EditorModuleAttribute.EditorModuleAttribute(string id, int order = 0)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModuleAttribute.cs#L24) | Creates a module registration with stable identity and deterministic lifecycle order.<br>`id`：The globally unique module identifier used for discovery diagnostics and optional persisted module state.<br>`order`：The ascending order used to start and update the module. | — | `System.ArgumentException`：Thrown when id is empty. |
| <a id="api-366994a72c8309aa"></a>[`public string Inno.Editor.Core.EditorModuleAttribute.id { get; }`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModuleAttribute.cs#L37) | Gets the stable module identifier. | — | — |
| <a id="api-2d2ceebcfc794b91"></a>[`public int Inno.Editor.Core.EditorModuleAttribute.order { get; }`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModuleAttribute.cs#L42) | Gets the stable lifecycle order. | — | — |
| <a id="api-94860657e3662e9a"></a>[`public sealed class Inno.Editor.Core.EditorModuleAttribute : System.Attribute`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorModuleAttribute.cs#L9) | Registers an editor feature module with a stable identity for automatic discovery. | — | — |

### `Inno.Editor.Core.EditorPanel`

<!-- api: T:Inno.Editor.Core.EditorPanel daf5469521ed69f80dfba50e6238ac464d2386e4b26b44a0f4fa6d9ea4c87776 -->
<!-- api: P:Inno.Editor.Core.EditorPanel.useWindowPadding d9ef6e3a7611884dd4aef8fe2267e079600c0d1c8e6b35073f26d9dd5a84ee3a -->
<!-- api: P:Inno.Editor.Core.EditorPanel.allowScrolling 566b4df617ed3ebb2face65a20b8773ae5e38150e6631dc97f4ec261fc68b8e1 -->
<!-- api: P:Inno.Editor.Core.EditorPanel.initialSize 7880d303fb87d41f11f4e9eb50f388896dad96b93337f0ff67d737676e54123d -->
<!-- api: P:Inno.Editor.Core.EditorPanel.isOpen 985eb75e51dbe5a9209bd757c1b0746508090d4a0e2070e3ad2dc18b591cff53 -->
<!-- api: M:Inno.Editor.Core.EditorPanel.Attach(Inno.Editor.Core.EditorContext) 5adb34b13429334662625c7c227b991305504c3118cc4985894a3595ac9f2cea -->
<!-- api: M:Inno.Editor.Core.EditorPanel.Detach(Inno.Editor.Core.EditorContext) 4dcde96f791b23b97597103892188f3a091de794da734698932129eb0cdac5c9 -->
<!-- api: M:Inno.Editor.Core.EditorPanel.Draw(Inno.Editor.Core.EditorContext) cc30af9ffbfe00ddd10bb39816227de3adfdde2bfd88ae2b85206a4bd0d75065 -->
<!-- api: M:Inno.Editor.Core.EditorPanel.OnDraw(Inno.Editor.Core.EditorContext) 6592d0de3755e3a35bc095aab0b47fe98eee6ad34de1404adbbf38266b9bee13 -->
<!-- api: M:Inno.Editor.Core.EditorPanel.OnAttach(Inno.Editor.Core.EditorContext) cb670a6264577edef64595f54667e345e0eb855eee9ffad2f31b4bc38b2dd73b -->
<!-- api: M:Inno.Editor.Core.EditorPanel.OnDetach(Inno.Editor.Core.EditorContext) 527be3bd689076a58b2a48d9576cc652f9b0e11941cd47bd6adc0da265494eb2 -->
<!-- api: M:Inno.Editor.Core.EditorPanel.Capture(Inno.Editor.Core.EditorState) 2664dffe8a7dfa5712ce000c2172d5f2ce878aac4e39b8821b61cb6cec289587 -->
<!-- api: M:Inno.Editor.Core.EditorPanel.Restore(Inno.Editor.Core.EditorState) fb4d7f3bfbc63b2e864575d95fc520b2bf784c3fdd6780b86dbe5b8b60e0377f -->
<!-- api: M:Inno.Editor.Core.EditorPanel.#ctor b8618b5d4b169c221f535f4a034e1259aba6eeb369f34320b2c7e3fc65204dfc -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-3d0bd2d3155ff860"></a>[`protected Inno.Editor.Core.EditorPanel.EditorPanel()`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorPanel.cs#L13) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-a9eed9eac9adea2c"></a>[`public void Inno.Editor.Core.EditorPanel.Attach(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorPanel.cs#L56) | Attaches the panel after its extension generation becomes active.<br>`context`：The shared editor context for the active runtime. | — | `System.ArgumentNullException`：Thrown when context is null.<br>`System.InvalidOperationException`：Thrown when the panel is already attached. |
| <a id="api-3694992a813ea91f"></a>[`protected virtual void Inno.Editor.Core.EditorPanel.Capture(Inno.Editor.Core.EditorState state)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorPanel.cs#L168) | Captures readable project state owned by this panel.<br>`state`：The writable parameter that receives the complete readable state for this panel. | Overriding this method opts the panel into project-state IO. Panels that keep the base implementation are never registered with the persistence coordinator and therefore perform no state reads or writes. Panel visibility is persisted separately for every registered panel. | — |
| <a id="api-8e960e6ebff41f01"></a>[`public void Inno.Editor.Core.EditorPanel.Detach(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorPanel.cs#L82) | Detaches the panel before its extension generation is released.<br>`context`：The shared editor context for the runtime being detached. | Also compensates partial attachment. Completed detachment is idempotent; a pending panel cannot draw. | `System.ArgumentNullException`：Thrown when context is null.<br>`Inno.Core.Execution.RetirementPendingException`：Panel work is still active. The owner must retain the panel and its dependencies and retry detachment. |
| <a id="api-a4f525b647e04bcc"></a>[`public void Inno.Editor.Core.EditorPanel.Draw(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorPanel.cs#L117) | Draws the complete dockable contents of the panel for the current frame.<br>`context`：The shared editor context containing current selection, focus, and frame state. | — | `System.ArgumentNullException`：Thrown when context is null.<br>`System.InvalidOperationException`：Thrown when the panel is not attached. |
| <a id="api-95a893532c21d880"></a>[`protected virtual void Inno.Editor.Core.EditorPanel.OnAttach(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorPanel.cs#L139) | Runs after the panel is attached to an active extension generation.<br>`context`：The shared editor context for the active runtime. | — | — |
| <a id="api-c023cbc68fd90f76"></a>[`protected virtual void Inno.Editor.Core.EditorPanel.OnDetach(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorPanel.cs#L153) | Runs before the panel is detached from its active extension generation.<br>`context`：The shared editor context for the runtime being detached. | This includes failed attachment compensation. Retain unfinished resources when throwing RetirementPendingException and avoid repeating completed cleanup steps on the next attempt. | — |
| <a id="api-89e615bff6a69406"></a>[`protected abstract void Inno.Editor.Core.EditorPanel.OnDraw(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorPanel.cs#L131) | Draws the complete dockable contents of this panel for the current frame.<br>`context`：The shared editor context containing current selection, focus, and frame state. | — | — |
| <a id="api-b5234148be264088"></a>[`protected virtual void Inno.Editor.Core.EditorPanel.Restore(Inno.Editor.Core.EditorState state)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorPanel.cs#L183) | Restores readable project state owned by this panel.<br>`state`：The read-only state parameter for this panel. Missing or incompatible values return the fallback supplied to Inno.Editor.Core.EditorState.Get``1(System.String,``0). | This method is called only when Inno.Editor.Core.EditorPanel.Capture(Inno.Editor.Core.EditorState) is overridden. It runs once after the panel is attached and before the panel is allowed to capture replacement state. | — |
| <a id="api-2d0048f8b11d4820"></a>[`public virtual bool Inno.Editor.Core.EditorPanel.allowScrolling { get; }`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorPanel.cs#L31) | Gets whether the presentation backend may create a scroll range for this panel window. | Full-canvas panels should return false and manage navigation inside their own content instead of allowing the host window to scroll. | — |
| <a id="api-bafa428cc7d50f1c"></a>[`public virtual System.Numerics.Vector2 Inno.Editor.Core.EditorPanel.initialSize { get; }`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorPanel.cs#L36) | Gets the initial floating size in UI coordinates; zero leaves native auto-sizing in control. Saved layouts take precedence. | — | — |
| <a id="api-b36486f687a04231"></a>[`public bool Inno.Editor.Core.EditorPanel.isOpen { get; set; }`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorPanel.cs#L41) | Gets or sets whether panel is visible. | — | — |
| <a id="api-92dc10e8cd8bbff9"></a>[`public virtual bool Inno.Editor.Core.EditorPanel.useWindowPadding { get; }`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorPanel.cs#L22) | Gets whether the presentation backend should inset this panel body by its standard window padding. | — | — |
| <a id="api-6c44cde3a9f0fdac"></a>[`public abstract class Inno.Editor.Core.EditorPanel`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorPanel.cs#L13) | Base class for editor panel implementations. | — | — |

### `Inno.Editor.Core.EditorPanelAttribute`

<!-- api: T:Inno.Editor.Core.EditorPanelAttribute 99f5fcb176c53a29520536c3aa63e962951f9dfaf175752e1c6b117ad58c6247 -->
<!-- api: M:Inno.Editor.Core.EditorPanelAttribute.#ctor(System.String,System.String,System.Int32,System.Boolean,System.String,System.Boolean) 347c245bef741eacc6575a0a5d149059eba7f2f5143057f27146d40d926a2c01 -->
<!-- api: P:Inno.Editor.Core.EditorPanelAttribute.id cd29490a84e2afa90fca8959720a57b2a6ccafc4c275c1bc1d8653e117deedf4 -->
<!-- api: P:Inno.Editor.Core.EditorPanelAttribute.title 6c250c8ec1d3a58873c8163505cebc7ed37f9683614474e97a0418bf5a948d46 -->
<!-- api: P:Inno.Editor.Core.EditorPanelAttribute.order ba712d0ae585d3afaca65cde02c61dbb0d6e81d1e9c432db735b0c1373c6d231 -->
<!-- api: P:Inno.Editor.Core.EditorPanelAttribute.defaultOpen 3eb8a394fc8fc5da30fc8b78bd7e29a7a45b22102eb63202ac457aedb799b5d7 -->
<!-- api: P:Inno.Editor.Core.EditorPanelAttribute.menuPath 4bbf7c0dbec0b8330285e8c583015805c0148ef2daaadcbe8dbd492779806ef1 -->
<!-- api: P:Inno.Editor.Core.EditorPanelAttribute.separatorBefore a268253599342460be327c407cc7b1bc81a787a890bc646b833a8e3a3aca0e49 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-73994bece100a8c0"></a>[`public Inno.Editor.Core.EditorPanelAttribute.EditorPanelAttribute(string id, string title, int order = 0, bool defaultOpen = true, string menuPath = "", bool separatorBefore = false)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorPanelAttribute.cs#L36) | Creates a dockable panel registration with stable identity and presentation metadata.<br>`id`：The stable identity used for Panel-menu routing and reload-state retention.<br>`title`：The visible dockable-window title.<br>`order`：The stable panel and generated Panel-menu ordering value.<br>`defaultOpen`：Whether a newly discovered panel is visible before retained state is available.<br>`menuPath`：The optional slash-delimited category path under the generated Panel menu.<br>`separatorBefore`：Whether the generated toggle is preceded by a separator within its final menu category. | — | `System.ArgumentException`：Thrown when id or title is empty, or when menuPath contains an empty segment. |
| <a id="api-eba562c879bd76ad"></a>[`public bool Inno.Editor.Core.EditorPanelAttribute.defaultOpen { get; }`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorPanelAttribute.cs#L74) | Gets whether a newly discovered panel is open by default. | — | — |
| <a id="api-cc3c3fe6c8baea0c"></a>[`public string Inno.Editor.Core.EditorPanelAttribute.id { get; }`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorPanelAttribute.cs#L59) | Gets the stable panel identifier. | — | — |
| <a id="api-273362142c65aa9a"></a>[`public string Inno.Editor.Core.EditorPanelAttribute.menuPath { get; }`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorPanelAttribute.cs#L79) | Gets the optional category path under the generated Panel menu. | — | — |
| <a id="api-65cd88f0f2f263e5"></a>[`public int Inno.Editor.Core.EditorPanelAttribute.order { get; }`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorPanelAttribute.cs#L69) | Gets the stable panel ordering value. | — | — |
| <a id="api-dc575ffa6a48c170"></a>[`public bool Inno.Editor.Core.EditorPanelAttribute.separatorBefore { get; }`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorPanelAttribute.cs#L84) | Gets whether the generated panel toggle begins a new visual group. | — | — |
| <a id="api-7441180cea50c370"></a>[`public string Inno.Editor.Core.EditorPanelAttribute.title { get; }`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorPanelAttribute.cs#L64) | Gets the visible panel title. | — | — |
| <a id="api-5ee5255661c07840"></a>[`public sealed class Inno.Editor.Core.EditorPanelAttribute : System.Attribute`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/EditorPanelAttribute.cs#L9) | Registers an editor panel for automatic discovery. | — | — |

### `Inno.Editor.Core.EditorReloadCoordinator`

<!-- api: T:Inno.Editor.Core.EditorReloadCoordinator b138461d895bf6cc7d377f0aa5245f0e364afb223566eb6ac6ef6684d15f44bf -->
<!-- api: M:Inno.Editor.Core.EditorReloadCoordinator.Register(Inno.Editor.Core.IEditorReloadParticipant) 5bd9de960a1af2a9b86c2543b78737b7b3d9dfb2ec5e88dbabcc329776b44b8a -->
<!-- api: M:Inno.Editor.Core.EditorReloadCoordinator.Execute(Inno.Extensibility.Modules.AssemblyReloadSession,Inno.Extensibility.Reload.IGenerationChange) 1574feedc59a3c6a17fd4a3f4accd5034ebac09a719c540317ca05fc454783d9 -->
<!-- api: M:Inno.Editor.Core.EditorReloadCoordinator.RefreshDiagnostics ebd132914fc490d8820a39860ab6b2dc2d43d591b6a0b48f36ef4dada482c9c0 -->
<!-- api: M:Inno.Editor.Core.EditorReloadCoordinator.#ctor 5aad47dc1903909bf1fc86100f911b5b5078a7d094619b21bb72885cf4ea0191 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-3f1b42a59a162d7c"></a>[`public Inno.Editor.Core.EditorReloadCoordinator.EditorReloadCoordinator()`](../../src/composition/editor/framework/Inno.Editor.Core/Reloading/EditorReloadCoordinator.cs#L16) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-11db6eafebcf5299"></a>[`public Inno.Extensibility.Reload.IAssemblyUnloadProbe Inno.Editor.Core.EditorReloadCoordinator.Execute(Inno.Extensibility.Modules.AssemblyReloadSession reload, Inno.Extensibility.Reload.IGenerationChange? externalChange = null)`](../../src/composition/editor/framework/Inno.Editor.Core/Reloading/EditorReloadCoordinator.cs#L66) | Applies one prepared assembly reload together with every registered editor feature migration.<br>返回：A monitor that observes cooperative unloading of assemblies retired by the committed reload.<br>`reload`：The prepared assembly reload session to activate and complete.<br>`externalChange`：Content recovery prepared before type publication, applied before Editor recovery, and restored before old values. | — | `System.ArgumentNullException`：Thrown when reload is null.<br>`System.AggregateException`：Thrown when activation fails and one or more feature or assembly rollback stages also fail. |
| <a id="api-ecc8be09c7074a67"></a>[`public void Inno.Editor.Core.EditorReloadCoordinator.RefreshDiagnostics()`](../../src/composition/editor/framework/Inno.Editor.Core/Reloading/EditorReloadCoordinator.cs#L79) | Requests every live participant to republish diagnostics derived from its current state. | — | — |
| <a id="api-24989cbb15bcf851"></a>[`public System.IDisposable Inno.Editor.Core.EditorReloadCoordinator.Register(Inno.Editor.Core.IEditorReloadParticipant participant)`](../../src/composition/editor/framework/Inno.Editor.Core/Reloading/EditorReloadCoordinator.cs#L34) | Registers an editor feature that owns generation-bound live state.<br>返回：A registration lease that strongly retains the participant until the lease is disposed. The coordinator itself retains only a weak reference.<br>`participant`：The feature participant. The coordinator retains only a weak reference to this instance. | — | `System.ArgumentNullException`：Thrown when participant is null. |
| <a id="api-f63ac6e4029fdd38"></a>[`public sealed class Inno.Editor.Core.EditorReloadCoordinator : Inno.Scripting.Reload.IScriptReloadCoordinator`](../../src/composition/editor/framework/Inno.Editor.Core/Reloading/EditorReloadCoordinator.cs#L16) | Coordinates weakly registered editor feature migrations around atomic assembly reload sessions. | — | — |

### `Inno.Editor.Core.EditorRuntime`

<!-- api: T:Inno.Editor.Core.EditorRuntime d5fc25ba689f013900efb48db8eefd73be025d5d77da8adae22ca83708308ce4 -->
<!-- api: M:Inno.Editor.Core.EditorRuntime.#ctor(Inno.Editor.Core.EditorContext) cfd706b33295f81408edd7db1f6c3a0b7ac4a4965c09ce8ecad69ff4604172dd -->
<!-- api: P:Inno.Editor.Core.EditorRuntime.context e6b8f61982826433a1580c3e056f7d2b317193d95d93f043138c590a08ef6265 -->
<!-- api: M:Inno.Editor.Core.EditorRuntime.Start aaa9b79492438df08d0473d2b5237274bfc451463bc2593fb90ab8e1e6109ea8 -->
<!-- api: M:Inno.Editor.Core.EditorRuntime.Update(Inno.Editor.Core.EditorFrame) ef9a3e86e3bbd5e729216dc02b3f62831d6c7f824e80ce36fada630b6d5f1d99 -->
<!-- api: M:Inno.Editor.Core.EditorRuntime.SetFrame(Inno.Editor.Core.EditorFrame) 245a5a9cd165d51d21b40bfece5141e2af91e851a8a504c1a4b399565a9412e2 -->
<!-- api: M:Inno.Editor.Core.EditorRuntime.Dispose 57a9fb05592dfca454eb8a0f765b10772517c57af651c53fbe03bece6fd0a114 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-17069a217a0fc06b"></a>[`protected Inno.Editor.Core.EditorRuntime.EditorRuntime(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorRuntime.cs#L19) | Creates a runtime for the supplied passive context.<br>`context`：The shared editor context. | — | `System.ArgumentNullException`：Thrown when context is null. |
| <a id="api-cb7601e7ce0cf430"></a>[`public abstract void Inno.Editor.Core.EditorRuntime.Dispose()`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorRuntime.cs#L57) | Stops the runtime and releases active extensions. | — | — |
| <a id="api-ba5469b8be5c949d"></a>[`protected void Inno.Editor.Core.EditorRuntime.SetFrame(Inno.Editor.Core.EditorFrame frame)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorRuntime.cs#L48) | Advances frame-scoped data and publishes the latest immutable frame.<br>`frame`：The frame to publish. | — | — |
| <a id="api-1323e5545f4b9610"></a>[`public abstract void Inno.Editor.Core.EditorRuntime.Start()`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorRuntime.cs#L32) | Starts the runtime and activates its initial extension generation. | — | — |
| <a id="api-13eac3cf03a2d0f6"></a>[`public abstract void Inno.Editor.Core.EditorRuntime.Update(Inno.Editor.Core.EditorFrame frame)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorRuntime.cs#L40) | Updates the runtime for one editor frame.<br>`frame`：The immutable frame state. | — | — |
| <a id="api-7fcf4a7352e5db77"></a>[`public Inno.Editor.Core.EditorContext Inno.Editor.Core.EditorRuntime.context { get; }`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorRuntime.cs#L27) | Gets the shared passive editor context. | — | — |
| <a id="api-6eb2823543f339af"></a>[`public abstract class Inno.Editor.Core.EditorRuntime : System.IDisposable`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorRuntime.cs#L8) | Defines the presentation-independent lifecycle of an editor runtime. | — | — |

### `Inno.Editor.Core.EditorState`

<!-- api: T:Inno.Editor.Core.EditorState 37ad83ba4b1f2245a76d924dad7d5274be296d54bcb659f3d9988d223fb3c35c -->
<!-- api: M:Inno.Editor.Core.EditorState.#ctor 63ec8ea90efcca5b92bf2589a6ea32487c19961ad6fe0b2d6bc7c9bf9fbee1c8 -->
<!-- api: M:Inno.Editor.Core.EditorState.Get``1(System.String,``0) d39bb9954cafd078cfa3159d85be496aa037c9f43ca83b9fd28ef978b1d2cffb -->
<!-- api: M:Inno.Editor.Core.EditorState.Set``1(System.String,``0) 7ab54d3447e1ae52fac0db0d4bb32a8254683cc58a70d16353f34c08f28cdf6b -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-4ef91b29ca324944"></a>[`protected Inno.Editor.Core.EditorState.EditorState()`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorState.cs#L19) | Initializes the base contract for a runtime-owned editor state parameter. | — | — |
| <a id="api-68ef2546c9a48915"></a>[`public abstract T Inno.Editor.Core.EditorState.Get<T>(string key, T fallback)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorState.cs#L42) | Reads a compatible value or returns the caller-provided fallback.<br>返回：The stored compatible value, or fallback.<br>`key`：The stable key local to the owning module or panel.<br>`fallback`：The value returned when the key is absent, malformed, or incompatible with T. | — | `System.ArgumentException`：Thrown when key is empty. |
| <a id="api-93bae0856d430324"></a>[`public abstract void Inno.Editor.Core.EditorState.Set<T>(string key, T value)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorState.cs#L68) | Writes one neutral value under a stable extension-local key.<br>`key`：The stable key local to the owning module or panel.<br>`value`：The value to persist. | — | `System.ArgumentException`：Thrown when key is empty.<br>`System.InvalidOperationException`：Thrown when the state parameter was supplied for restoration and is therefore read-only.<br>`System.NotSupportedException`：Thrown when value cannot be represented by the runtime state serializer. |
| <a id="api-a350a03ecf0d89ea"></a>[`public abstract class Inno.Editor.Core.EditorState`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorState.cs#L14) | Provides the serialization-neutral parameter used to capture or restore one editor extension's project state. | The editor supplies this object to the protected state hooks on Inno.Editor.Core.EditorModule and Inno.Editor.Core.EditorPanel. Extensions should write values during capture and read values during restore; storage format and persistence remain private runtime details. | — |

### `Inno.Editor.Core.EditorStatistic`

<!-- api: T:Inno.Editor.Core.EditorStatistic a24284df9c83164b2b09e14d3419e69d24b6480e519a0c11e767cf55ca9088e1 -->
<!-- api: M:Inno.Editor.Core.EditorStatistic.#ctor(Inno.Editor.Core.EditorStatisticId,Inno.Editor.Core.EditorStatisticGroupId,System.String,System.String,System.String,System.Int32,System.Int32) 0e2f623704af913edf3641bf324fc1187f7e26c43db53600a6ab211fdfcc4a02 -->
<!-- api: P:Inno.Editor.Core.EditorStatistic.id ff69609c48d31dc8f8e6fbbda707a2516b4a4cc7e9126db0e1583c0cc08739dd -->
<!-- api: P:Inno.Editor.Core.EditorStatistic.groupId ce365f84244ebd4d778fb5b4040639d8b74070d34f952b264e782d70a0c83d97 -->
<!-- api: P:Inno.Editor.Core.EditorStatistic.groupName 6eb0e1d90c0f2199ddf5fed3f99bb7384435df61cbdf3c77d9afb3e2a0b6861c -->
<!-- api: P:Inno.Editor.Core.EditorStatistic.label 1746c9bc51de041761644e83d8d366dc6cc2a77ffe1ec755180e0a1519d3b279 -->
<!-- api: P:Inno.Editor.Core.EditorStatistic.value e0c8042b915ea5c360f94b0e56ae7ca700477bf40c043abe3f68eaa0cff659b5 -->
<!-- api: P:Inno.Editor.Core.EditorStatistic.groupOrder 7b332bf3f2b5137347d755041eeba7d267ba69174318cf985b98a84d8cbb22cd -->
<!-- api: P:Inno.Editor.Core.EditorStatistic.order 505040211669795bb7263c5b7cbc2d4d08f0636eb0304f229f362133e2369e4f -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-6284d799df6c4faf"></a>[`public Inno.Editor.Core.EditorStatistic.EditorStatistic(Inno.Editor.Core.EditorStatisticId id, Inno.Editor.Core.EditorStatisticGroupId groupId, string groupName, string label, string value, int groupOrder = 0, int order = 0)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L117) | Creates one statistic contribution.<br>`id`：Globally stable statistic identity used for replacement within a frame.<br>`groupId`：Stable visual group identity.<br>`groupName`：User-facing group heading.<br>`label`：User-facing metric label.<br>`value`：Already formatted user-facing value.<br>`groupOrder`：Ascending group presentation order.<br>`order`：Ascending metric order within the group. | — | `System.ArgumentException`：Thrown when an identifier, group name, label, or value is empty. |
| <a id="api-ff82927d25f6cd49"></a>[`public Inno.Editor.Core.EditorStatisticGroupId Inno.Editor.Core.EditorStatistic.groupId { get; }`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L150) | Gets the stable visual group identity. | — | — |
| <a id="api-da2092419de6b4eb"></a>[`public string Inno.Editor.Core.EditorStatistic.groupName { get; }`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L155) | Gets the user-facing group heading. | — | — |
| <a id="api-9d686e2ce537eb38"></a>[`public int Inno.Editor.Core.EditorStatistic.groupOrder { get; }`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L170) | Gets ascending group presentation order. | — | — |
| <a id="api-0cad9a41683007dc"></a>[`public Inno.Editor.Core.EditorStatisticId Inno.Editor.Core.EditorStatistic.id { get; }`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L145) | Gets the globally stable statistic identity. | — | — |
| <a id="api-a48ed53cd6c9f0ce"></a>[`public string Inno.Editor.Core.EditorStatistic.label { get; }`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L160) | Gets the user-facing metric label. | — | — |
| <a id="api-0e9e5bc082858485"></a>[`public int Inno.Editor.Core.EditorStatistic.order { get; }`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L175) | Gets ascending metric order within the group. | — | — |
| <a id="api-bd68675acfd10458"></a>[`public string Inno.Editor.Core.EditorStatistic.value { get; }`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L165) | Gets the presentation-ready value. | — | — |
| <a id="api-b45f122b9ed58989"></a>[`public sealed class Inno.Editor.Core.EditorStatistic`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L88) | Describes one immutable, presentation-ready statistic without retaining provider instances or types. | — | — |

### `Inno.Editor.Core.EditorStatisticGroupId`

<!-- api: T:Inno.Editor.Core.EditorStatisticGroupId c1eb8ef6d2c2a7cfd7368e4535bee656c087044a104f8609959bc28af8cb9fc2 -->
<!-- api: M:Inno.Editor.Core.EditorStatisticGroupId.#ctor(System.String) ba1aada37020d769e04ad3a5754b8531316986f6e9b5ddc0434db6a9d1f12329 -->
<!-- api: P:Inno.Editor.Core.EditorStatisticGroupId.value 4ec1addd02221c818023fd0555a88a1786e29ae69dfe11f1984b07f0c1a4b017 -->
<!-- api: P:Inno.Editor.Core.EditorStatisticGroupId.isValid a62db98f6b3567e17797b055c0464d3ea42f5f6f1b6fb6a123b384859f42a0a0 -->
<!-- api: M:Inno.Editor.Core.EditorStatisticGroupId.ToString 0be6bdb2fc26ded799177a2c66bece3991eb158d040dee825770d0bc579e232f -->
<!-- api: M:Inno.Editor.Core.EditorStatisticGroupId.op_Inequality(Inno.Editor.Core.EditorStatisticGroupId,Inno.Editor.Core.EditorStatisticGroupId) 0acc7c0c03ad9bb5874cb19996afd90df6fd362619b37c62108dfa5a9333ae9c -->
<!-- api: M:Inno.Editor.Core.EditorStatisticGroupId.op_Equality(Inno.Editor.Core.EditorStatisticGroupId,Inno.Editor.Core.EditorStatisticGroupId) 941d91b654e8977beab47f5b0f7878e5b0b2c1df14142347eabcdb1aa41955a4 -->
<!-- api: M:Inno.Editor.Core.EditorStatisticGroupId.GetHashCode 2d9cbeff5a872ee96332df5afe524e11af52d75b603265e48fd1958971e8394d -->
<!-- api: M:Inno.Editor.Core.EditorStatisticGroupId.Equals(System.Object) 5f74287cb94cab13d09767ceea890b63359fa8c39e3e1edd2570e0d26187c397 -->
<!-- api: M:Inno.Editor.Core.EditorStatisticGroupId.Equals(Inno.Editor.Core.EditorStatisticGroupId) 3555cb549c5bdac83c1c89e2be0e240384294513ea5a6d7d90593b816fc1e510 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d87e28e4bfb2c491"></a>[`public Inno.Editor.Core.EditorStatisticGroupId.EditorStatisticGroupId(string value)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L60) | Creates a globally stable statistic-group identifier.<br>`value`：Non-empty globally stable identity. | — | `System.ArgumentException`：Thrown when value is empty. |
| <a id="api-59025ed5b5115156"></a>[`public bool Inno.Editor.Core.EditorStatisticGroupId.Equals(Inno.Editor.Core.EditorStatisticGroupId other)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L49) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-e3080d1a85f83f59"></a>[`public override bool Inno.Editor.Core.EditorStatisticGroupId.Equals(object obj)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L49) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-cdc1cda442ffaf0b"></a>[`public override int Inno.Editor.Core.EditorStatisticGroupId.GetHashCode()`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L49) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-7a886fead7e3a2e5"></a>[`public static bool Inno.Editor.Core.EditorStatisticGroupId.operator ==(Inno.Editor.Core.EditorStatisticGroupId left, Inno.Editor.Core.EditorStatisticGroupId right)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L49) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-3227b5d5e4bbe2a2"></a>[`public static bool Inno.Editor.Core.EditorStatisticGroupId.operator !=(Inno.Editor.Core.EditorStatisticGroupId left, Inno.Editor.Core.EditorStatisticGroupId right)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L49) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-20a28e5943142c2d"></a>[`public override string Inno.Editor.Core.EditorStatisticGroupId.ToString()`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L82) | Formats this value as a human-readable representation.<br>返回：The human-readable representation of this value. | — | — |
| <a id="api-05f33f724f9976af"></a>[`public bool Inno.Editor.Core.EditorStatisticGroupId.isValid { get; }`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L74) | Gets whether this identifier contains a usable value. | — | — |
| <a id="api-4af4df86f5253e0f"></a>[`public string Inno.Editor.Core.EditorStatisticGroupId.value { get; }`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L69) | Gets the globally stable identity. | — | — |
| <a id="api-c1734a80ddcb8a31"></a>[`public readonly record struct Inno.Editor.Core.EditorStatisticGroupId : System.IEquatable<Inno.Editor.Core.EditorStatisticGroupId>`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L49) | Identifies one visual group in the editor statistics presentation. | — | — |

### `Inno.Editor.Core.EditorStatisticId`

<!-- api: T:Inno.Editor.Core.EditorStatisticId f0f8f244dd6697dce2fc089aff85002af97683c37d89b596b778552f69d78de9 -->
<!-- api: M:Inno.Editor.Core.EditorStatisticId.#ctor(System.String) 2670803cda0bc58157bedded1e8c3cd3bf4e15633342abfcff96fe09b1beec3e -->
<!-- api: P:Inno.Editor.Core.EditorStatisticId.value 909c8073bd7f839c404fbeac5c757d3b9a4dff5fb498967e250cccc657a06616 -->
<!-- api: P:Inno.Editor.Core.EditorStatisticId.isValid 6e78a7d2b374dc994b7bb23ac2011260da2c907714fa90f7b3e24d4d5b7647e0 -->
<!-- api: M:Inno.Editor.Core.EditorStatisticId.ToString c49b59beee0523680b2d17a3e93f61cdb94a20bb0c8709c299b85f07aaf2b08f -->
<!-- api: M:Inno.Editor.Core.EditorStatisticId.op_Inequality(Inno.Editor.Core.EditorStatisticId,Inno.Editor.Core.EditorStatisticId) 3758938e90b9e54bea82aa16686a82a3283829847358445d13aa3f0018541a3c -->
<!-- api: M:Inno.Editor.Core.EditorStatisticId.op_Equality(Inno.Editor.Core.EditorStatisticId,Inno.Editor.Core.EditorStatisticId) d16b4281f72cc4afa554c8d37176f2aa54194e509f70d465b3c9acb271e69f22 -->
<!-- api: M:Inno.Editor.Core.EditorStatisticId.GetHashCode 22e74bbf6fda984b51ae71e6b7c73c2ddadb0440281d442bd97e8a26cd98af02 -->
<!-- api: M:Inno.Editor.Core.EditorStatisticId.Equals(System.Object) a9ab96e491a13797ff65408e0cd2f1a47495aaad766381ae5a9e428d57ab004d -->
<!-- api: M:Inno.Editor.Core.EditorStatisticId.Equals(Inno.Editor.Core.EditorStatisticId) 1ce73aef276e76a709ad6c60d698ca80f4d3b3fe73622c035a39ea4c5fcd82a0 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-c0bd577f679d9c81"></a>[`public Inno.Editor.Core.EditorStatisticId.EditorStatisticId(string value)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L21) | Creates a globally stable statistic identifier.<br>`value`：Non-empty globally stable identity. | — | `System.ArgumentException`：Thrown when value is empty. |
| <a id="api-3ec71f1e4ab4e7fc"></a>[`public bool Inno.Editor.Core.EditorStatisticId.Equals(Inno.Editor.Core.EditorStatisticId other)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L10) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-2580e173dffa8cf1"></a>[`public override bool Inno.Editor.Core.EditorStatisticId.Equals(object obj)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L10) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-564af67703bd071e"></a>[`public override int Inno.Editor.Core.EditorStatisticId.GetHashCode()`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L10) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-2093c91af41ce576"></a>[`public static bool Inno.Editor.Core.EditorStatisticId.operator ==(Inno.Editor.Core.EditorStatisticId left, Inno.Editor.Core.EditorStatisticId right)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L10) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-95bc1d07dce63a22"></a>[`public static bool Inno.Editor.Core.EditorStatisticId.operator !=(Inno.Editor.Core.EditorStatisticId left, Inno.Editor.Core.EditorStatisticId right)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L10) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-e4d305d7c6d3228a"></a>[`public override string Inno.Editor.Core.EditorStatisticId.ToString()`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L43) | Formats this value as a human-readable representation.<br>返回：The human-readable representation of this value. | — | — |
| <a id="api-2004d5c17b85e2e2"></a>[`public bool Inno.Editor.Core.EditorStatisticId.isValid { get; }`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L35) | Gets whether this identifier contains a usable value. | — | — |
| <a id="api-b0c1cfafa27722b6"></a>[`public string Inno.Editor.Core.EditorStatisticId.value { get; }`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L30) | Gets the globally stable identity. | — | — |
| <a id="api-c6bb11d4e5b4ccac"></a>[`public readonly record struct Inno.Editor.Core.EditorStatisticId : System.IEquatable<Inno.Editor.Core.EditorStatisticId>`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L10) | Identifies one editor statistic across panels, modules, and reloadable extensions. | — | — |

### `Inno.Editor.Core.EditorStatistics`

<!-- api: T:Inno.Editor.Core.EditorStatistics 315a02d03e58e6766bff506605835f6b649230f75711c0c0a61d9de236413324 -->
<!-- api: M:Inno.Editor.Core.EditorStatistics.Publish(Inno.Editor.Core.EditorStatistic) 2c7d80c557d438de0bc21d0fcae3b053d1e9479a03332685921af8d05eda6e05 -->
<!-- api: M:Inno.Editor.Core.EditorStatistics.Publish(System.Collections.Generic.IEnumerable{Inno.Editor.Core.EditorStatistic}) ebeafb93092b7ba0ea1be840201c609eb5bf39bf044b7a47342ef5aa56b53788 -->
<!-- api: M:Inno.Editor.Core.EditorStatistics.GetSnapshot 13a566f21062f7295cdaee9e168de36f67fa93e58c2bc35a525441c39978cff3 -->
<!-- api: M:Inno.Editor.Core.EditorStatistics.#ctor efac35a9c85cd7700eaafde9f3e12c26f2de41b7bb1332b91c0bfbd37ee2c325 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-11f38f58f353f128"></a>[`public Inno.Editor.Core.EditorStatistics.EditorStatistics()`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L185) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-d569e7c004928ea1"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Editor.Core.EditorStatistic> Inno.Editor.Core.EditorStatistics.GetSnapshot()`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L230) | Gets a deterministic snapshot combining current contributions with the previous completed frame for providers that draw after their consumer.<br>返回：A detached, sorted snapshot containing at most one value for each stable statistic ID. | — | — |
| <a id="api-9e16cd3c99f20354"></a>[`public void Inno.Editor.Core.EditorStatistics.Publish(Inno.Editor.Core.EditorStatistic statistic)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L200) | Publishes or replaces one statistic for the current editor frame.<br>`statistic`：Immutable contribution containing no runtime provider references. | — | `System.ArgumentNullException`：Thrown when statistic is null. |
| <a id="api-ccc45b5d15a14517"></a>[`public void Inno.Editor.Core.EditorStatistics.Publish(System.Collections.Generic.IEnumerable<Inno.Editor.Core.EditorStatistic> statistics)`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L216) | Publishes or replaces several statistics for the current editor frame.<br>`statistics`：Contributions to publish in enumeration order. | — | `System.ArgumentNullException`：Thrown when statistics or one of its values is null. |
| <a id="api-b7254ac55c2ccaae"></a>[`public sealed class Inno.Editor.Core.EditorStatistics`](../../src/composition/editor/framework/Inno.Editor.Core/Runtime/EditorStatistics.cs#L185) | Exchanges frame-scoped, reload-safe statistics between independent editor features and viewers. | Contributions contain only stable identifiers and strings. A contribution is retained for the current frame and one completed-frame handoff so panel draw order cannot lose data. | — |

### `Inno.Editor.Core.IEditorPanelReloadState`

<!-- api: T:Inno.Editor.Core.IEditorPanelReloadState 201b9c1ed33f2ae0a1f42428b5a0de8f680701bd6c468ad2f0b5d2392369e4a9 -->
<!-- api: M:Inno.Editor.Core.IEditorPanelReloadState.CaptureReloadState e8b8eb473b326354c568b24424f2a31d89cf7b0e4fed9ba58ea036f05cfb7e57 -->
<!-- api: M:Inno.Editor.Core.IEditorPanelReloadState.RestoreReloadState(System.ReadOnlyMemory{System.Byte}) 0a026579d4dc06a094e266c67d37df404766a16f1266386d4a450c3690ea7a4b -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-dcf0add48fbbf948"></a>[`System.ReadOnlyMemory<byte> Inno.Editor.Core.IEditorPanelReloadState.CaptureReloadState()`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/IEditorPanelReloadState.cs#L16) | Captures presentation-neutral state without retaining runtime or plugin object references.<br>返回：An immutable byte payload that can be consumed by the replacement panel generation. | — | — |
| <a id="api-e56e6b398022760e"></a>[`void Inno.Editor.Core.IEditorPanelReloadState.RestoreReloadState(System.ReadOnlyMemory<byte> state)`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/IEditorPanelReloadState.cs#L24) | Restores presentation-neutral state captured from the previous panel generation.<br>`state`：The immutable payload returned by the retiring panel generation. | — | — |
| <a id="api-6e09fdf4fb4a2118"></a>[`public interface Inno.Editor.Core.IEditorPanelReloadState`](../../src/composition/editor/framework/Inno.Editor.Core/Extensions/IEditorPanelReloadState.cs#L8) | Optionally migrates presentation-neutral panel state across assembly reloads. | — | — |

### `Inno.Editor.Core.IEditorReloadParticipant`

<!-- api: T:Inno.Editor.Core.IEditorReloadParticipant 9fb84922b68e171337c0651f2860a8593f5108627920e10900a3dba28c062fd4 -->
<!-- api: M:Inno.Editor.Core.IEditorReloadParticipant.Capture(Inno.Extensibility.Modules.AssemblyReloadContext) a97e1e82330a341126e70b26056ca9bc4b3fbe28a1db68c0451e07c328a8c68b -->
<!-- api: M:Inno.Editor.Core.IEditorReloadParticipant.RefreshDiagnostics aa986d97059e4d2caed90a1ea09007d4e85bf3640ac4bae22dcc27e7f6413e86 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-f23d48c097bce75c"></a>[`Inno.Extensibility.Reload.IGenerationChange Inno.Editor.Core.IEditorReloadParticipant.Capture(Inno.Extensibility.Modules.AssemblyReloadContext context)`](../../src/composition/editor/framework/Inno.Editor.Core/Reloading/IEditorReloadParticipant.cs#L21) | Captures one isolated transaction for the prepared assembly generation.<br>返回：A transaction that has captured current state but has not modified live editor objects.<br>`context`：The prepared assembly reload context containing previous and candidate participant state. | — | — |
| <a id="api-8be2833be6e5f52b"></a>[`void Inno.Editor.Core.IEditorReloadParticipant.RefreshDiagnostics()`](../../src/composition/editor/framework/Inno.Editor.Core/Reloading/IEditorReloadParticipant.cs#L26) | Republishes diagnostics derived from the participant's current live state. | — | — |
| <a id="api-fa4de390b36f1fe8"></a>[`public interface Inno.Editor.Core.IEditorReloadParticipant`](../../src/composition/editor/framework/Inno.Editor.Core/Reloading/IEditorReloadParticipant.cs#L10) | Contributes editor-owned live state to an assembly reload without coupling the reload requester to a feature domain. | — | — |

## 项目依赖

- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Modules](../extensibility/Inno.Extensibility.Modules.md)：公开引用边界由实际签名核对。
- [Inno.Scripting.Reload](../scripting/Inno.Scripting.Reload.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Reload](../extensibility/Inno.Extensibility.Reload.md)：公开引用边界由实际签名核对。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
- [Inno.Core.Input](../core/Inno.Core.Input.md)：公开引用边界由实际签名核对。
