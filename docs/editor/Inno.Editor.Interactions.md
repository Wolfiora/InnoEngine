# Inno.Editor.Interactions

## 扩展退休失败

Editor extension snapshot 现在使用 TypeRegistry 的统一 `DisposeExtensions`，去重、逆序并尝试全部实例后聚合失败。
Dispose 异常不再只写 LogError：调用者收到异常，共享 generation gate 进入 Faulted，禁止继续 reload。
当前 shutdown 的 Panel → Module → History/Interactions → extension Dispose 顺序不变；相关测试同时验证顺序、次数与 Fault。
这不表示所有 Stop/Detach 分支都已完成相同审计，剩余范围见[续轮报告](../architecture/ENGINE_CLOSURE_CONTINUATION_2026_09_07.md)。

[Editor 索引](README.md) · [Core](Inno.Editor.Core.md) · [ImGui](Inno.Editor.ImGui.md) · [Identity、Missing 与 reload 标准](../architecture/IDENTITY_REFERENCE_RELOAD_STANDARD.md)

`Inno.Editor.Interactions` 提供表现后端无关的交互语言：稳定的 `string` area/action/panel ID、可选 `target`、轻量 `EditorInteraction`，以及 Attribute 自动发现的 Action、Menu、Shortcut 和 Drop。它不引用 ImGui、Assets、Scene、Scripting 或任何 Panel project；跨 feature reload 协议位于 [Inno.Editor.Core](Inno.Editor.Core.md)。

## 最小心智模型

```csharp
EditorInteraction interaction = interactions.For(
    AnimationInteractionIds.C_GRAPH_AREA,
    selectedState);

interaction.Focus();
interaction.Select();
interaction.Execute(AnimationInteractionIds.C_RENAME_STATE);
EditorMenuModel menu = interaction.BuildMenu();
```

- Attribute 和运行时 API 都直接使用 feature 自己维护的稳定 `const string`。
- `target` 是当前实际对象，决定 typed Action/Drop 是否匹配。
- Action ID 表示语义操作，例如 `asset.rename`；它与 area 是两个正交维度。
- 旧的强类型 ID/command wrapper 已删除，不存在兼容 wrapper 或转发 overload。
- 不需要 `Surface` marker type，也不需要创建 Context/Menu/Command service。

## 定义 area

每个 feature 在自己的项目根目录保留一个常量类：

```csharp
internal static class AnimationInteractionIds
{
    internal const string C_GRAPH_AREA = "panel/animation.graph";
    internal const string C_STATE_AREA = "panel/animation.graph/state";
    internal const string C_DELETE_STATE = "animation.state.delete";
    internal const string C_RENAME_STATE = "animation.state.rename";
    internal const string C_CREATE_TEMPLATE = "animation.template.create";
}
```

命名建议使用小写、以 `/` 分层。area 不需要注册；第一次传给 `For` 或 Attribute 时即可使用。所有公开入口对空白 ID 抛出 `ArgumentException`，匹配使用 ordinal string comparison。

## Action

立即完成的操作：

```csharp
[EditorAction(
    AnimationInteractionIds.C_DELETE_STATE,
    AnimationInteractionIds.C_STATE_AREA)]
public sealed class DeleteAnimationStateAction : EditorAction<AnimationState>
{
    protected override EditorActionState Query(
        EditorActionContext<AnimationState> context)
        => context.target.canDelete
            ? EditorActionState.enabled
            : EditorActionState.disabled;

    protected override void Execute(
        EditorActionContext<AnimationState> context)
    {
        context.target.Delete();
    }
}
```

解析优先级为精确 area、target 类型距离和 Attribute priority。找不到或禁用时 `Execute` 返回 `false`；需要参数的实现继承 `EditorAction<TTarget,TArgument>` 或 `EditorArgumentAction<TArgument>`，调用方使用 `Execute(actionId, argument)`，dispatch 前严格校验 action argument 类型。扩展仍只看到强类型 context，不暴露原始 `object argument` 或 `TryGetArgument`。

### 多帧 Action

Rename、分步创建和参数预览仍是普通 `EditorAction`。Action 自己持有状态，并调用 `Activate`、`Complete`、`Cancel`；视图只在目标位置调用 `Present`：

```csharp
protected override void Execute(EditorActionContext<AnimationState> context)
{
    m_name = context.target.name;
    Activate(context);
}

protected override bool Present(
    EditorActionContext<AnimationState, RenamePresentation> context)
{
    RenamePresentation presentation = context.argument;
    m_name = presentation.value;
    if (presentation.cancel)
        Cancel();
    else if (presentation.submit && TryCommit(context.target, m_name))
        Complete();
    return true;
}
```

上述 Rename 以 `EditorPresentationAction<AnimationState,RenamePresentation>` 实现。调用方用同一 action ID 的 `Execute(id)` 启动、`Present(id, presentation)` 呈现；presentation 数据在 Action 实现中始终是编译期强类型。Action 执行或呈现抛异常时，运行时会尝试取消其活动状态；取消回调本身失败也会被独立记录，不会破坏后续 Action。

没有单独的 `EditorActionInteraction<TState>` 或全局 Rename service。FileBrowser 和 Hierarchy 各自拥有 Rename Action，因为验证、提交和呈现目标属于各自 feature。

Selection 切换 target 时，Interactions 会通知旧 target 上仍活跃的多帧 Action 失去 presentation。默认实现取消操作；需要提交临时值的 Action 可以覆盖 `OnPresentationLost()`，在其中验证并调用 `Complete()`。如果覆盖返回时 Action 仍处于 active 状态，运行时会自动取消，避免不可见的输入操作永久残留。

## Menu

Action 可声明任意层级菜单路径：

```csharp
[EditorAction("animation.state.create", AnimationInteractionIds.C_GRAPH_AREA)]
[EditorMenu(
    AnimationInteractionIds.C_GRAPH_AREA,
    "Create/Animation/State",
    order: 200,
    separatorBefore: true)]
public sealed class CreateAnimationStateAction : EditorAction<AnimationGraph>
{
    protected override void Execute(EditorActionContext<AnimationGraph> context)
    {
    }
}
```

同一个 Attribute 同时适用于主菜单和右键菜单；区别只在 area。`EditorMenuRenderer` 会递归创建一级、二级或任意更深的菜单。Create 类命令应使用 `Create/...` 路径形成统一子菜单，不在 label 中手写层级或分隔线。`separatorBefore` 只表达两个相邻可见命令组之间的边界；当前菜单/子菜单的首个可见项永远不会绘制分隔线，因此前置命令被 Query 隐藏时也不会留下孤立横线。

动态列表使用 `EditorMenuSource`：

```csharp
[EditorMenuSource(AnimationInteractionIds.C_GRAPH_AREA)]
public sealed class AnimationTemplateMenu : EditorMenuSource
{
    public override void Build(EditorMenuContext context, EditorMenuBuilder builder)
    {
        builder.Add(
            "Create/From Template/Humanoid",
            AnimationInteractionIds.C_CREATE_TEMPLATE,
            argument: "Humanoid");
    }
}
```

Action 的 `Query` 决定条目是否可见、可用、勾选和动态标题；快捷键标签从 `[EditorShortcut]` 自动生成。

File Browser 的资产操作同样使用 Action/Menu 路由：条目 Action 只负责识别 source entry 类型并转发稳定语义动作，实际 Scene Load、Prefab Instantiate 等行为由对应资产类型的 `editor/open` Action 完成。双击和右键因此共享一条执行链；未来资产类型可按相同方式贡献自己的右键功能，不在 File Browser renderer 中增加类型 switch。

Panel 主菜单使用同一棵层级菜单模型。`EditorPanelAttribute.menuPath` 是 `Panel/` 下的开放分类路径，支持任意斜杠层级；`separatorBefore` 在条目所在分类内开启视觉分组。每次构建菜单时，generated Panel leaf 直接从当前 extension generation 的 `isOpen` 生成 checked 状态，因此勾选与窗口关闭按钮、reload 后恢复状态始终一致。Host 不维护封闭类别枚举，内置 Panel 当前按 Workspace、Viewports、Authoring、Content 与 Diagnostics 分类，Plugin Panel 可以声明自己的稳定分类而无需修改 Editor。

快捷键显示与键盘 dispatch 共用同一个 resolver：先按 action ID、area、target specificity 与 priority 解析实际 registration，再读取它的 gesture。同一 Action 可声明多个不同 gesture，每个都可 dispatch；菜单只显示当前 area 的第一个可用 gesture。精确 area shortcut 存在时会覆盖该 registration 的 global shortcuts。同一 Action 在同一有效 area 重复同一 gesture，或不同 Action 形成同 specificity 歧义，都会在 catalog Build 阶段被拒绝。

## Toolbar

紧凑 icon toolbar 与 Menu 复用同一个 Action resolver，不引入 command service：

```csharp
[EditorAction("simulation.toggle", "editor/main-menu")]
[EditorToolbarItem(
    "editor/main-menu",
    EditorToolbarIcon.Play,
    "Start Simulation",
    activeIcon: EditorToolbarIcon.Stop)]
public sealed class ToggleSimulationAction : EditorAction
{
    protected override EditorActionState Query(EditorActionContext context)
        => new(true, true, isChecked: Simulation.isRunning);

    protected override void Execute(EditorActionContext context)
        => Simulation.Toggle();
}
```

`EditorInteraction.BuildToolbar()` 返回 immutable `EditorToolbarModel`。每个 `EditorToolbarItem` 包含 `actionId`、解析后的语义 `EditorToolbarIcon`、`tooltip`、`order` 与完整 `EditorActionState`。checked 时使用 `activeIcon`；`displayName` 非空时替代 Attribute 的静态 tooltip。Toolbar Action 必须无 target、无 argument，且 placement area 不能越过 Action 自己的精确 area；重复 placement 会在 candidate Build 时拒绝。表现层只把语义 icon 映射到自己的 glyph，不把 ImGui 字符串带入 Interactions。

## Selection 与 Focus

```csharp
EditorInteraction row = interactions.For(AnimationInteractionIds.C_STATE_AREA, state);
if (clicked)
    row.Select();
if (panelFocused)
    row.Focus();
```

`Select()` 本身走内建 Action，因此和其他操作共享队列与代际规则。`EditorSelectionState` 只公开读取和 `TryGet<T>`，不公开可绕过 Action 的 mutator。

`IEditorSelectionCoordinator` 是给会替换对象实例的 host feature 使用的窄接口，只暴露 `selectedTarget` 与 `SetSelection`。当前 `EditorInteractions` 实现该接口；extension activator 可按此接口注入同一个稳定 interaction instance。普通 Panel 仍应使用 `EditorInteraction.Select()`，只有 Scene generation/Play Mode 这类必须在原子替换期间重绑 persistent identity 的基础设施才依赖 coordinator。

## Scene 与图画布的共享平面导航

`EditorPlanarNavigation` 是不依赖 ImGui、Assets 或 Scene 的公开输入状态机；Scene View 与 Shader Editor 共用它，
坐标均为逻辑像素，表现后端负责 framebuffer DPI 换算。

| API | 行为 |
| --- | --- |
| `isPanning` | 只读，表示当前画布持有平移手势，包括指针已离开画布时 |
| `Update(hovered, primaryPressed, middlePressed, primaryDown, middleDown, alt, allowAltPrimary)` | 悬停时开始中键或 Alt 左键手势；用全局按钮 down 状态释放捕获。`allowAltPrimary=false` 可把 Alt 左键留给 3D orbit，中键不受影响 |
| `Cancel()` | Esc、失去文档、关闭画布时明确释放捕获 |
| `WheelFactor(wheel, sensitivity=0.16f)` | 公用指数缩放倍率；拒绝非有限输入与非正 sensitivity，极端输入限幅以保持有限结果 |
| `ZoomOrigin(origin, pivot, previousScale, nextScale)` | 用缩放前后倍率重算屏幕原点，保持鼠标下的内容坐标不变；拒绝非有限或非正倍率 |

平移/缩放属于视图状态，不进入数据 History。每帧传入全局按钮状态，不能只在鼠标悬停时调用 `Update`，否则窗口外
释放会丢失。它不拥有场景或图的 live object，也不代替节点拖动、3D orbit、飞行和业务工具的状态机。

```csharp
using System.Numerics;
using Inno.Editor.Interactions;

static Vector2 Zoom(Vector2 origin, Vector2 cursor, float previous, float next)
    => EditorPlanarNavigation.ZoomOrigin(origin, cursor, previous, next);
```

脚本通过 Editor-only 逻辑命名空间 `InnoEditor.Interactions` 使用已导出的类型；上例为宿主源码写法。

## Drag and Drop

```csharp
[EditorDrop(AnimationInteractionIds.C_GRAPH_AREA)]
public sealed class ClipToStateDrop
    : EditorDrop<AnimationClipAsset, AnimationGraph>
{
    protected override EditorDropStatus Query(
        EditorDropContext<AnimationClipAsset, AnimationGraph> context)
        => EditorDropStatus.Accept(EditorDropVisual.Highlight);

    protected override EditorDropResult Drop(
        EditorDropContext<AnimationClipAsset, AnimationGraph> context)
    {
        AnimationState state = context.target.CreateState(context.source);
        return EditorDropResult.Accepted(selectionTarget: state);
    }
}
```

视图提交 `EditorDragData(IdentityObject source, string label)`；该值只保存 `RuntimeIdentity sourceIdentity` 和显示文本，
不保存源对象。内部 BeginDrag/QueryDrop/Drop 按 domain 选择已注册的 `IdentityAllocator`，Preview 与 Delivery 均重新解析
源 runtime ID。ImGui payload kind 标识 domain，原生 bytes 只有该 domain 内的 runtime ID，没有随机 token 或 managed object 回查表。
generation 变化、源注销、domain 不匹配或成功 Drop 会结束当前拖拽。落盘与 History 必须改用 persistent ID。

## Runtime 与热重载

`EditorInteractionRuntime` 从当前 TypeCache snapshot 原子构建 Module、Action、Menu source、Drop、Panel 和 Modal。候选冲突或构造失败会拒绝新 snapshot，旧 generation 继续工作。Host 类型实例会尽量保留；插件类型会 Detach/Stop/Dispose，避免固定旧 ALC。

`EditorModalExtension.Presentation` 捕获 Modal 的移动、缩放、初始尺寸与 `allowScrolling` 布局策略，供表现层决定窗口标志。其值只在当前 generation 的展示快照内有效，不作为持久状态保存。

Editor 同时拥有 Scene、Asset、Graph 等多个 `IdentityAllocator` domain。需要跨帧保留对象身份的通用 UI（例如 Inspector lock）必须保存完整 `RuntimeIdentity`，并通过 `EditorInteractions.TryResolveIdentity` 回到该 identity 自己的 domain；不得使用 `IdentityAllocator.current` 猜测当前 domain，也不得只保存 persistent Guid 后在错误 allocator 中查询。Inspector lock 同时保留原 domain 与 persistent ID，当前 runtime slot 退休后只在原 domain 重绑定同一稳定对象，因此 generation 替换不会锁到同 ID 的其他域。非 identity 的 collectible 插件对象只允许弱引用，避免 lock 阻止旧 ALC 回收。

`IEditorDocumentService` 是无可见 Panel 的共享创作文档所有权服务。它统一管理 Shader、Material、Pipeline 等草稿的单实例身份、dirty 状态、Save/Revert/Close、恢复和 provider generation 重连；Shader Editor 与 Inspector 是各资产的唯一呈现入口。打开文档时 persistent asset ID 与规范化 source path 同时保持唯一：同一路径的干净陈旧上下文会在 provider 回调后安全退休，含未保存修改的上下文则拒绝被替换并要求用户先 Save、Revert 或 Close，不能静默丢弃草稿。

candidate 激活前会取消 drag、清空 pending action/presentation/menu model。Selection 与 Focus 指向 retiring collectible 类型时先清除；若对象继承 `IdentityObject`，则暂存 persistent ID 并在下一次 Update 尝试绑定当前 generation 对象，解析失败才保持清空。

候选 snapshot 作为 staging catalog 对生命周期回调可见。重入请求的新 rebuild 只能在当前全局 Complete 之后作为独立 transaction 运行。
激活先逆序 Detach 旧 Panel、Stop 旧 Module，再按 `EditorModuleAttribute.order` Start 新 Module、Attach 新 Panel，最后 Restore。
尝试 Start/Attach 前即登记 candidate ownership，部分失败也参与补偿。Module Start 失败后逆序补偿 candidate 并恢复旧 snapshot、
旧 History handler map 和已停止的旧扩展；Panel Attach 失败只有在 Detach 补偿成功后才能单独隔离。
Complete 后释放旧 snapshot 实例。共同保留的 Host instance 不重复 Start/Attach，也不被旧 snapshot Dispose。

无法 Attach 或 Draw 的 Panel 会被关闭并进入当前 generation quarantine；Panel `useWindowPadding`、Module `blocksFollowingUpdates`
等 getter 抛异常仍按实例隔离。Module Update、Modal 状态读取/Draw 的失败也是帧执行隔离边界。
**Stop/Detach/Dispose 则是所有权边界，不允许只记日志。** 普通退出错误聚合上抛并 Fault；Pending 使用 Core RetirementBarrier
在控制线程有界排空，超时保留 owner、封锁整个 generation，不继续释放 History、模块依赖或 native backend。
恢复旧 generation 的 Start/Attach 失败同样报告 Fault。quarantine 不会被用来掩盖清理失败；失败 Panel 不参与 Restore、Capture 或 Draw。

Editor 正常关闭先捕获并原子写入最终状态，再逆序 Detach Panel、Stop Module，然后清空 Action/drag 并 Dispose History，最后退休 extension snapshot。
EditorInteractionRuntime 记录已完成 shutdown stage，Pending 不标 disposed、不清空 action/snapshot，且拒绝 Start/Update。
普通终结错误仍尝试后续阶段并统一报告；不能把 Pending 包进普通 AggregateException 后继续。ImGui runtime、EditorLayer、Host resource stack
逐层遵循相同规则；EditorLayer 用 Core LifetimeScope 拥有 runtime 与 diagnostics，无第二套生命周期容器。

## Undo / Redo

`EditorInteractions.history` 向扩展返回 `IEditorHistory`。具体 `EditorHistory`、delegate operation、`RecordValue`、handler-map 更新、Clear/Dispose 都是 host-only internal 能力；脚本只能提交中立 change，不能把 collectible delegate、`Type` 或 runtime object 固定在栈中。稳定记录由下列项组成：

Host workflow 可使用 `EditorInteractions.BeginHistoryIsolation()` 临时切换到空 History 分支。scope 保留进入时完整的 Undo、Redo、fault 状态与 reload-safe payload；scope 内的新记录受同一容量和内存/磁盘预算约束，Dispose 时全部释放并恢复原分支。该入口标记为 `ScriptingApiIgnore`，用于 [Play Mode](Inno.Editor.PlayMode.md) 等可控瞬时会话，不是 EditorScripts 创建任意第二套 Undo 栈的许可。

- `kind`：全局唯一的协议 ID，例如 `animation/state-property`。
- `payload`：只含 ID、索引、字符串和序列化字节的中立数据。
- `mergeKey`：可选的连续编辑合并键。

领域 Module 在修改成功后使用 `RecordApplied`：

```csharp
byte[] data = AnimationHistoryData.Encode(
    controllerId,
    stateId,
    beforeName,
    afterName);

context.history.RecordApplied(
    "Rename Animation State",
    new EditorHistoryChange(
        "animation/state-name",
        EditorHistoryPayload.FromBytes(data),
        mergeKey: $"animation-state:{stateId}:name"));
```

当前 generation 的 Handler 由 TypeCache 自动发现：

```csharp
[EditorHistoryHandler("animation/state-name")]
public sealed class AnimationStateNameHistoryHandler : EditorHistoryHandler
{
    protected override EditorHistoryAvailability Query(
        EditorHistoryContext context,
        EditorHistoryChange change,
        EditorHistoryDirection direction)
    {
        AnimationHistoryData data = AnimationHistoryData.Decode(
            change.payload.ReadBytes());
        return AnimationDatabase.Contains(data.controllerId, data.stateId)
            ? EditorHistoryAvailability.Available()
            : EditorHistoryAvailability.Unavailable("The animation state no longer exists.");
    }

    protected override EditorHistoryResult Apply(
        EditorHistoryContext context,
        EditorHistoryChange change,
        EditorHistoryDirection direction)
    {
        AnimationHistoryData data = AnimationHistoryData.Decode(
            change.payload.ReadBytes());
        string value = direction == EditorHistoryDirection.Undo
            ? data.beforeName
            : data.afterName;
        AnimationDatabase.Rename(data.controllerId, data.stateId, value);
        return EditorHistoryResult.Success();
    }
}
```

`Query` 不修改状态，只给菜单和快捷键提供可用性与 barrier 原因。`Apply` 必须在修改前捕获最小 rollback state；普通 `Failure` 表示操作失败但输入状态已严格保留，失败的 Undo/Redo 不移动栈指针，可以重试。只有 History 内部可以生成 `statePreserved=false` 的状态完整性丢失结果；此时 History 进入 faulted，拒绝继续记录、Undo 和 Redo，直到宿主显式 Clear。Handler 缺失、目标删除或 Stable Type ID 不可解析是可诊断 barrier，不会丢弃记录或使用错误对象。

`Execute(name, EditorHistoryChange)` 适合 Handler 自己安全执行初次 Redo 的命令；多数 Editor UI 已先应用修改，因此使用 `RecordApplied`。业务源码不得使用 delegate operation、派生 `EditorHistoryOperation` 或 runtime object merge key。

相邻中立记录只有在 `kind`、非空 `mergeKey` 与 Handler 的 `TryMerge` 都匹配时才会合并。单击开关、创建、删除和排序不设置 merge key；拖动数值、连续文字输入等可合并编辑才设置。

### 事务与资源预算

多个已经独立可逆的修改可以组成一个顶层事务：

```csharp
using EditorHistoryTransaction transaction = context.history.BeginTransaction("Create Controller");
// Apply and RecordApplied each independent neutral child change.
transaction.Commit();
```

事务 Undo 按反序、Redo 按正序执行；任一 child 失败时验证并执行相反方向补偿。全部补偿成功返回原失败且顶层栈不移动；任一补偿失败会 fault History。显式 Rollback 只有完整成功后才出栈和释放，普通失败时 transaction 与 child 保持可重试；未 Commit 的 Dispose 若遇到状态仍完整的回滚失败，会把仍然应用的 transaction 提交到 Undo 栈并抛出明确异常，避免修改成为无 History 状态。

默认保留 256 个顶层记录，同时受 `EditorHistoryOptions.maxResidentBytes` 与 `maxDiskBytes` 限制。小 payload 驻留内存；达到 `inlinePayloadThreshold` 的 payload 自动进入 `<Project>/Library/Editor/History` session blob store。清空 History、淘汰记录、Runtime 关闭或丢弃 Redo branch 时立即释放对应文件。磁盘 payload 不随 Scene、Prefab、`.imeta` 或 `editor.ini` 持久化。

扩展 reload 对 Handler map 执行 Prepare → Activate → Rollback/Complete。Activate 只临时切换 candidate map；其他 registry 稍后失败时 Rollback 恢复旧 map，只有全局 Complete 才释放旧 handler generation 并丢弃 reload-unsafe host entry。kind 冲突在 candidate Build 阶段拒绝，不进入生命周期回调。

内建 `Edit/Undo` 与 `Edit/Redo` 菜单自动显示下一操作名称。快捷键为 Command/Ctrl+Z、Command/Ctrl+Shift+Z，并额外支持 Command/Ctrl+Y。

## Module/Panel 状态存储

Interactions 从 `EditorModuleAttribute.id` / `EditorPanelAttribute.id` 取得唯一身份，并只为真正 override protected `Capture(EditorState)` 的 Module/Panel 建立内部状态注册。没有 override Capture 的实例不进入状态 IO，也不会创建空 section。项目语义状态写入：

```text
<Project>/editor.ini
```

每个有状态的 Module/Panel 使用独立、可读的 INI section。单个复杂值使用一行 JSON 表示数组或字符串，不使用 Base64，也不把全部状态包进 opaque payload：

```ini
[InnoEditor][Module.scene-workspace]
activeScene="Scenes/Main.iscene"
openScenes=["Scenes/Main.iscene","Scenes/UI.iscene"]

[InnoEditor][Panel.asset.file-browser]
filter=""
viewMode="List"
treePaneRatio=0.5
listNameSeparator=0.4
listTypeSeparator=0.7

[InnoEditor][Panels]
asset.file-browser=true
scene.hierarchy=true
```

文件通过临时文件 flush 后原子替换，并在运行期间进行约两秒的内容变化节流。`EditorInteractionRuntime.SaveState()` 可显式捕获并 flush；退出时 Application 会在扩展停止前强制捕获所有有状态实例和最新 ImGui layout，然后强制写入完整文档。未知 Module/Panel section 会保留，因此暂时移除插件不会销毁其设置；损坏的单个值由 `EditorState.Get` 回退。Panel 的 `isOpen` 按稳定 Panel ID 自动保存，即使 Panel 没有 override Capture 也不受影响。

Core 的 layout document 在内存中分别维护 ImGui layout 和具名 Inno Editor sections，避免两类内容互相覆盖。只有 Interactions/Application 的 host CLR 路径会调用这些标记为 `ScriptingApiIgnore` 的成员；扩展拿到的脚本 facade 没有 layout API，也不存在第二套 Module/Panel 状态文档。

Module/Panel 的恢复状态按实例弱跟踪。只有成功完成 protected `Restore` 的实例才能参与后续 capture；脚本启动、TypeCache 重建或 Registry 事务在恢复回调中触发重入刷新时，同一实例不会被再次调用，也不会用尚未初始化的默认字段覆盖磁盘 section。被新 snapshot 保留的 Module/Panel 仍以实际恢复状态为准，而不是仅因实例相同就跳过首次恢复。

Undo 栈、dirty Scene 内容、runtime 对象和编译中间态不会跨进程保存。它们要么无法安全跨代际恢复，要么本身可以由 Asset Database 和脚本构建图重建。

Selection 同样是当前 Editor session 的瞬时交互状态，不写入 `editor.ini`。Scene Workspace 只保存打开顺序与 active Scene；项目恢复完成后 selection 保持为空，直到用户或明确的打开操作重新选择对象。

Scene 内容历史由独立的 [Inno.Editor.Scene](Inno.Editor.Scene.md) 实现。它不会为一次小修改序列化整张 Scene 图，而是按属性、元素、子树、placement 或文档记录最小 payload。

## EditorScripts facade

物理源码无论位于 `Actions`、`Menus`、`DragDrop`、`Runtime` 或 `Selection`，都使用项目级 namespace `Inno.Editor.Interactions`。EditorScripts 对应的唯一逻辑 namespace 是 `InnoEditor.Interactions`。

脚本只能看到明确导出的契约，不能看到 Router、Catalog、TypeCache snapshot 或实现侧 `Inno.*`。完全禁止 global using。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Editor.Interactions.EditorAction`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorAction`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorAction.cs#L8) | Defines one automatically discovered editor operation and owns its complete multi-frame lifecycle. |
| [`abstract void Inno.Editor.Interactions.EditorAction.Execute(Inno.Editor.Interactions.EditorActionContext context)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorAction.cs#L121) | Executes the action for the supplied context. |
| [`bool Inno.Editor.Interactions.EditorAction.isActive`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorAction.cs#L17) | Gets whether the action currently owns an active multi-frame operation. |
| [`virtual Inno.Editor.Interactions.EditorActionState Inno.Editor.Interactions.EditorAction.Query(Inno.Editor.Interactions.EditorActionContext context)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorAction.cs#L113) | Evaluates the action for the supplied context. |
| [`virtual System.Type? Inno.Editor.Interactions.EditorAction.targetType`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorAction.cs#L22) | Gets the required target type, or for a targetless action. |
| [`virtual bool Inno.Editor.Interactions.EditorAction.Present(Inno.Editor.Interactions.EditorActionContext context)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorAction.cs#L132) | Presents an active action at the current target location. |
| [`virtual void Inno.Editor.Interactions.EditorAction.OnCancelled()`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorAction.cs#L144) | Runs after an active operation is cancelled. |
| [`virtual void Inno.Editor.Interactions.EditorAction.OnCompleted()`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorAction.cs#L137) | Runs after an active operation completes successfully. |
| [`virtual void Inno.Editor.Interactions.EditorAction.OnPresentationLost()`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorAction.cs#L156) | Runs when this action's active target loses editor presentation focus. |
| [`void Inno.Editor.Interactions.EditorAction.Activate(Inno.Editor.Interactions.EditorActionContext context)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorAction.cs#L72) | Activates this action for a target and cancels any operation it previously owned. |
| [`void Inno.Editor.Interactions.EditorAction.Cancel()`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorAction.cs#L95) | Cancels the current operation and returns the action to its idle state. |
| [`void Inno.Editor.Interactions.EditorAction.Complete()`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorAction.cs#L83) | Completes the current operation and returns the action to its idle state. |
| [`void Inno.Editor.Interactions.EditorAction.Dispose()`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorAction.cs#L161) | Cancels active work and releases this action instance. |

### `Inno.Editor.Interactions.EditorAction<TTarget, TArgument>`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorAction<TTarget, TArgument>`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorAction.cs#L180) | Defines a target action that requires one strongly typed command argument. |
| [`abstract void Inno.Editor.Interactions.EditorAction<TTarget, TArgument>.Execute(Inno.Editor.Interactions.EditorActionContext<TTarget, TArgument> context)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorAction.cs#L250) | Executes the action for a typed target and argument. |
| [`override sealed Inno.Editor.Interactions.EditorActionState Inno.Editor.Interactions.EditorAction<TTarget, TArgument>.Query(Inno.Editor.Interactions.EditorActionContext context)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorAction.cs#L199) | Evaluates the operation's current availability and presentation state. |
| [`override sealed System.Type Inno.Editor.Interactions.EditorAction<TTarget, TArgument>.targetType`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorAction.cs#L186) | Gets the concrete type handled by this extension implementation. |
| [`override sealed bool Inno.Editor.Interactions.EditorAction<TTarget, TArgument>.Present(Inno.Editor.Interactions.EditorActionContext context)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorAction.cs#L230) | Presents this action through the current editor interaction surface. |
| [`override sealed void Inno.Editor.Interactions.EditorAction<TTarget, TArgument>.Execute(Inno.Editor.Interactions.EditorActionContext context)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorAction.cs#L210) | Applies the editor action to the supplied interaction context. |
| [`virtual Inno.Editor.Interactions.EditorActionState Inno.Editor.Interactions.EditorAction<TTarget, TArgument>.Query(Inno.Editor.Interactions.EditorActionContext<TTarget, TArgument> context)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorAction.cs#L242) | Evaluates the action for a typed target and argument. |
| [`virtual bool Inno.Editor.Interactions.EditorAction<TTarget, TArgument>.Present(Inno.Editor.Interactions.EditorActionContext<TTarget, TArgument> context)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorAction.cs#L261) | Presents an active action for a typed target and argument. |

### `Inno.Editor.Interactions.EditorAction<TTarget>`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorAction<TTarget>`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorAction.cs#L380) | Defines a discoverable action whose target must be assignable to a specific reference type. |
| [`abstract void Inno.Editor.Interactions.EditorAction<TTarget>.Execute(Inno.Editor.Interactions.EditorActionContext<TTarget> context)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorAction.cs#L455) | Executes the action for a strongly typed target. |
| [`override sealed Inno.Editor.Interactions.EditorActionState Inno.Editor.Interactions.EditorAction<TTarget>.Query(Inno.Editor.Interactions.EditorActionContext context)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorAction.cs#L397) | Evaluates the operation's current availability and presentation state. |
| [`override sealed System.Type Inno.Editor.Interactions.EditorAction<TTarget>.targetType`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorAction.cs#L386) | Gets the concrete type handled by this extension implementation. |
| [`override sealed bool Inno.Editor.Interactions.EditorAction<TTarget>.Present(Inno.Editor.Interactions.EditorActionContext context)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorAction.cs#L431) | Presents this action through the current editor interaction surface. |
| [`override sealed void Inno.Editor.Interactions.EditorAction<TTarget>.Execute(Inno.Editor.Interactions.EditorActionContext context)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorAction.cs#L411) | Applies the editor action to the supplied interaction context. |
| [`virtual Inno.Editor.Interactions.EditorActionState Inno.Editor.Interactions.EditorAction<TTarget>.Query(Inno.Editor.Interactions.EditorActionContext<TTarget> context)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorAction.cs#L447) | Evaluates the action for a strongly typed target. |
| [`virtual bool Inno.Editor.Interactions.EditorAction<TTarget>.Present(Inno.Editor.Interactions.EditorActionContext<TTarget> context)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorAction.cs#L466) | Presents an active action for a strongly typed target. |

### `Inno.Editor.Interactions.EditorActionArgumentContext<TArgument>`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorActionArgumentContext<TArgument>`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorActionContext.cs#L146) | Provides a strongly typed action argument to a targetless editor action. |
| [`TArgument Inno.Editor.Interactions.EditorActionArgumentContext<TArgument>.argument`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorActionContext.cs#L160) | Gets the strongly typed action argument. |

### `Inno.Editor.Interactions.EditorActionAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorActionAttribute`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorActionAttribute.cs#L8) | Registers an editor action for automatic discovery and dispatch. |
| [`Inno.Editor.Interactions.EditorActionAttribute.EditorActionAttribute(string action, int priority = 0)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorActionAttribute.cs#L23) | Creates an action registration available from every area. |
| [`Inno.Editor.Interactions.EditorActionAttribute.EditorActionAttribute(string action, string area, int priority = 0)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorActionAttribute.cs#L46) | Creates an action registration restricted to one exact area. |
| [`int Inno.Editor.Interactions.EditorActionAttribute.priority`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorActionAttribute.cs#L71) | Gets the tie-breaking priority. |
| [`string Inno.Editor.Interactions.EditorActionAttribute.action`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorActionAttribute.cs#L61) | Gets the stable action name. |
| [`string Inno.Editor.Interactions.EditorActionAttribute.area`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorActionAttribute.cs#L66) | Gets the optional exact interaction area. |

### `Inno.Editor.Interactions.EditorActionContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Core.EditorContext Inno.Editor.Interactions.EditorActionContext.editor`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorActionContext.cs#L54) | Gets the shared passive editor context. |
| [`Inno.Editor.Interactions.EditorActionContext`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorActionContext.cs#L10) | Provides contextual state to an editor action. |
| [`Inno.Editor.Interactions.EditorInteractions Inno.Editor.Interactions.EditorActionContext.interactions`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorActionContext.cs#L59) | Gets the active interaction entry point. |
| [`Inno.Editor.Interactions.IEditorHistory Inno.Editor.Interactions.EditorActionContext.history`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorActionContext.cs#L64) | Gets the transactional history used to record reversible mutations performed by this action. |
| [`object? Inno.Editor.Interactions.EditorActionContext.target`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorActionContext.cs#L74) | Gets the contextual action target. |
| [`string Inno.Editor.Interactions.EditorActionContext.area`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorActionContext.cs#L69) | Gets the stable interaction area. |

### `Inno.Editor.Interactions.EditorActionContext<TTarget, TArgument>`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorActionContext<TTarget, TArgument>`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorActionContext.cs#L115) | Provides a strongly typed target and action argument to an editor action. |
| [`TArgument Inno.Editor.Interactions.EditorActionContext<TTarget, TArgument>.argument`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorActionContext.cs#L137) | Gets the strongly typed action argument. |
| [`TTarget Inno.Editor.Interactions.EditorActionContext<TTarget, TArgument>.target`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorActionContext.cs#L132) | Gets the strongly typed action target. |

### `Inno.Editor.Interactions.EditorActionContext<TTarget>`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorActionContext<TTarget>`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorActionContext.cs#L88) | Provides a strongly typed target to an editor action implementation. |
| [`TTarget Inno.Editor.Interactions.EditorActionContext<TTarget>.target`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorActionContext.cs#L103) | Gets the strongly typed action target. |

### `Inno.Editor.Interactions.EditorActionState`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorActionState`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorActionState.cs#L6) | Describes the current presentation and availability of an editor action. |
| [`Inno.Editor.Interactions.EditorActionState.EditorActionState(bool isVisible, bool isEnabled, bool isChecked = false, string? displayName = null)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorActionState.cs#L23) | Creates the contextual presentation state of an editor action. |
| [`bool Inno.Editor.Interactions.EditorActionState.isChecked`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorActionState.cs#L48) | Gets whether the action is currently checked. |
| [`bool Inno.Editor.Interactions.EditorActionState.isEnabled`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorActionState.cs#L43) | Gets whether the action can execute. |
| [`bool Inno.Editor.Interactions.EditorActionState.isVisible`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorActionState.cs#L38) | Gets whether the action should be displayed. |
| [`static Inno.Editor.Interactions.EditorActionState Inno.Editor.Interactions.EditorActionState.disabled`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorActionState.cs#L63) | Gets a visible but disabled action state. |
| [`static Inno.Editor.Interactions.EditorActionState Inno.Editor.Interactions.EditorActionState.enabled`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorActionState.cs#L58) | Gets a visible and enabled action state. |
| [`static Inno.Editor.Interactions.EditorActionState Inno.Editor.Interactions.EditorActionState.hidden`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorActionState.cs#L68) | Gets a hidden action state. |
| [`string? Inno.Editor.Interactions.EditorActionState.displayName`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorActionState.cs#L53) | Gets an optional contextual display name. |

### `Inno.Editor.Interactions.EditorArgumentAction<TArgument>`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorArgumentAction<TArgument>`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorAction.cs#L283) | Defines a targetless editor action that requires one strongly typed command argument. |
| [`abstract void Inno.Editor.Interactions.EditorArgumentAction<TArgument>.Execute(Inno.Editor.Interactions.EditorActionArgumentContext<TArgument> context)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorAction.cs#L334) | Executes the action for a typed argument. |
| [`override sealed Inno.Editor.Interactions.EditorActionState Inno.Editor.Interactions.EditorArgumentAction<TArgument>.Query(Inno.Editor.Interactions.EditorActionContext context)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorAction.cs#L296) | Evaluates the operation's current availability and presentation state. |
| [`override sealed void Inno.Editor.Interactions.EditorArgumentAction<TArgument>.Execute(Inno.Editor.Interactions.EditorActionContext context)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorAction.cs#L307) | Applies the editor action to the supplied interaction context. |
| [`virtual Inno.Editor.Interactions.EditorActionState Inno.Editor.Interactions.EditorArgumentAction<TArgument>.Query(Inno.Editor.Interactions.EditorActionArgumentContext<TArgument> context)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorAction.cs#L326) | Evaluates the action for a typed argument. |

### `Inno.Editor.Interactions.EditorDocumentCloseMode`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorDocumentCloseMode`](../../src/composition/editor/framework/Inno.Editor.Interactions/Documents/IEditorDocumentService.cs#L9) | Selects how a dirty document responds to a close request. |
| [`Inno.Editor.Interactions.EditorDocumentCloseMode.Cancel`](../../src/composition/editor/framework/Inno.Editor.Interactions/Documents/IEditorDocumentService.cs#L14) | Keeps the document open. |
| [`Inno.Editor.Interactions.EditorDocumentCloseMode.Discard`](../../src/composition/editor/framework/Inno.Editor.Interactions/Documents/IEditorDocumentService.cs#L22) | Closes the document and discards unsaved state. |
| [`Inno.Editor.Interactions.EditorDocumentCloseMode.Save`](../../src/composition/editor/framework/Inno.Editor.Interactions/Documents/IEditorDocumentService.cs#L18) | Closes the document after saving. |

### `Inno.Editor.Interactions.EditorDocumentContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorDocumentContext`](../../src/composition/editor/framework/Inno.Editor.Interactions/Documents/EditorDocumentContext.cs#L10) | Stores only stable, reload-safe document identity and provider-owned draft parameters. |
| [`System.Collections.Generic.IReadOnlyDictionary<string, string> Inno.Editor.Interactions.EditorDocumentContext.viewParameters`](../../src/composition/editor/framework/Inno.Editor.Interactions/Documents/EditorDocumentContext.cs#L65) | Gets an immutable snapshot of stable view parameters. |
| [`System.Guid Inno.Editor.Interactions.EditorDocumentContext.assetId`](../../src/composition/editor/framework/Inno.Editor.Interactions/Documents/EditorDocumentContext.cs#L35) | Gets the persistent asset identity, or an empty value before the source has one. |
| [`System.Guid Inno.Editor.Interactions.EditorDocumentContext.documentId`](../../src/composition/editor/framework/Inno.Editor.Interactions/Documents/EditorDocumentContext.cs#L30) | Gets the stable identity of this open source document. |
| [`bool Inno.Editor.Interactions.EditorDocumentContext.TryGetViewParameter(string key, out string value)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Documents/EditorDocumentContext.cs#L97) | Tries to read one stable view parameter. |
| [`bool Inno.Editor.Interactions.EditorDocumentContext.isDirty`](../../src/composition/editor/framework/Inno.Editor.Interactions/Documents/EditorDocumentContext.cs#L55) | Gets whether unsaved source or staged changes exist. |
| [`bool Inno.Editor.Interactions.EditorDocumentContext.isProviderAvailable`](../../src/composition/editor/framework/Inno.Editor.Interactions/Documents/EditorDocumentContext.cs#L60) | Gets whether a current-generation provider is available. |
| [`string Inno.Editor.Interactions.EditorDocumentContext.assetPath`](../../src/composition/editor/framework/Inno.Editor.Interactions/Documents/EditorDocumentContext.cs#L40) | Gets the normalized project asset path. |
| [`string Inno.Editor.Interactions.EditorDocumentContext.providerId`](../../src/composition/editor/framework/Inno.Editor.Interactions/Documents/EditorDocumentContext.cs#L45) | Gets the stable provider identity used to recover across extension reload. |
| [`string Inno.Editor.Interactions.EditorDocumentContext.title`](../../src/composition/editor/framework/Inno.Editor.Interactions/Documents/EditorDocumentContext.cs#L50) | Gets or sets the author-facing source title used by diagnostics and dedicated editors. |
| [`void Inno.Editor.Interactions.EditorDocumentContext.SetViewParameter(string key, string value)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Documents/EditorDocumentContext.cs#L76) | Adds or replaces one stable scalar or JSON-formatted view parameter. |

### `Inno.Editor.Interactions.EditorDocumentProvider`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorDocumentProvider`](../../src/composition/editor/framework/Inno.Editor.Interactions/Documents/EditorDocumentProvider.cs#L8) | Implements one reloadable asset-document kind without owning presentation or source identity. |
| [`abstract bool Inno.Editor.Interactions.EditorDocumentProvider.CanOpen(string assetPath)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Documents/EditorDocumentProvider.cs#L24) | Returns whether this provider can open the supplied project asset path. |
| [`abstract bool Inno.Editor.Interactions.EditorDocumentProvider.Save(Inno.Editor.Interactions.EditorDocumentContext context)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Documents/EditorDocumentProvider.cs#L43) | Saves all document changes to its asset source. |
| [`abstract string Inno.Editor.Interactions.EditorDocumentProvider.id`](../../src/composition/editor/framework/Inno.Editor.Interactions/Documents/EditorDocumentProvider.cs#L13) | Gets the globally stable provider identifier. |
| [`virtual bool Inno.Editor.Interactions.EditorDocumentProvider.Apply(Inno.Editor.Interactions.EditorDocumentContext context)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Documents/EditorDocumentProvider.cs#L54) | Applies staged authoring changes and saves their asset source. |
| [`virtual bool Inno.Editor.Interactions.EditorDocumentProvider.Revert(Inno.Editor.Interactions.EditorDocumentContext context)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Documents/EditorDocumentProvider.cs#L65) | Discards staged authoring changes and reloads the last saved source. |
| [`virtual void Inno.Editor.Interactions.EditorDocumentProvider.Close(Inno.Editor.Interactions.EditorDocumentContext context)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Documents/EditorDocumentProvider.cs#L77) | Releases transient provider state after a document closes. |
| [`virtual void Inno.Editor.Interactions.EditorDocumentProvider.Open(Inno.Editor.Interactions.EditorDocumentContext context)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Documents/EditorDocumentProvider.cs#L32) | Initializes transient provider state for an opened or restored document. |

### `Inno.Editor.Interactions.EditorDragContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Core.EditorContext Inno.Editor.Interactions.EditorDragContext.editor`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDragContext.cs#L49) | Gets the shared passive editor context. |
| [`Inno.Editor.Interactions.EditorDragContext`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDragContext.cs#L10) | Provides contextual state while beginning an editor drag. |
| [`Inno.Editor.Interactions.EditorDragContext.EditorDragContext(Inno.Editor.Core.EditorContext editor, Inno.Editor.Interactions.EditorInteractions interactions, string area, Inno.Editor.Interactions.EditorDragData data)`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDragContext.cs#L33) | Creates a managed editor drag request. |
| [`Inno.Editor.Interactions.EditorDragData Inno.Editor.Interactions.EditorDragContext.data`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDragContext.cs#L64) | Gets the managed drag data. |
| [`Inno.Editor.Interactions.EditorInteractions Inno.Editor.Interactions.EditorDragContext.interactions`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDragContext.cs#L54) | Gets the active interaction entry point. |
| [`string Inno.Editor.Interactions.EditorDragContext.area`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDragContext.cs#L59) | Gets the source interaction area. |

### `Inno.Editor.Interactions.EditorDragData`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Identity.RuntimeIdentity Inno.Editor.Interactions.EditorDragData.sourceIdentity`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDragData.cs#L41) | Gets the domain-qualified transient identity written to the native drag protocol. |
| [`Inno.Editor.Interactions.EditorDragData`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDragData.cs#L10) | Contains the transient identity and preview label for one editor drag operation. |
| [`Inno.Editor.Interactions.EditorDragData.EditorDragData(Inno.Core.Identity.IdentityObject source, string label)`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDragData.cs#L27) | Creates drag data for a registered identity object. |
| [`string Inno.Editor.Interactions.EditorDragData.label`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDragData.cs#L46) | Gets the drag preview label. |

### `Inno.Editor.Interactions.EditorDrop`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorDrop`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDrop.cs#L11) | Defines one discoverable typed editor drop operation. |
| [`abstract Inno.Editor.Interactions.EditorDropResult Inno.Editor.Interactions.EditorDrop.Drop(Inno.Editor.Interactions.EditorDropContext context)`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDrop.cs#L43) | Executes a delivered drop after a successful compatibility query. |
| [`abstract Inno.Editor.Interactions.EditorDropStatus Inno.Editor.Interactions.EditorDrop.Query(Inno.Editor.Interactions.EditorDropContext context)`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDrop.cs#L32) | Evaluates whether the current managed source may be dropped on the supplied target. |
| [`abstract System.Type Inno.Editor.Interactions.EditorDrop.sourceType`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDrop.cs#L16) | Gets the accepted drag source type. |
| [`abstract System.Type Inno.Editor.Interactions.EditorDrop.targetType`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDrop.cs#L21) | Gets the accepted drop target type. |

### `Inno.Editor.Interactions.EditorDrop<TSource, TTarget>`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorDrop<TSource, TTarget>`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDrop.cs#L55) | Defines a typed drop operation for one managed source and target pair. |
| [`abstract Inno.Editor.Interactions.EditorDropResult Inno.Editor.Interactions.EditorDrop<TSource, TTarget>.Drop(Inno.Editor.Interactions.EditorDropContext<TSource, TTarget> context)`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDrop.cs#L123) | Executes a delivered drop for the strongly typed source and target. |
| [`abstract Inno.Editor.Interactions.EditorDropStatus Inno.Editor.Interactions.EditorDrop<TSource, TTarget>.Query(Inno.Editor.Interactions.EditorDropContext<TSource, TTarget> context)`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDrop.cs#L112) | Evaluates whether the strongly typed source may be dropped on the strongly typed target. |
| [`override sealed Inno.Editor.Interactions.EditorDropResult Inno.Editor.Interactions.EditorDrop<TSource, TTarget>.Drop(Inno.Editor.Interactions.EditorDropContext context)`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDrop.cs#L95) | Validates and applies the current editor drag-and-drop interaction atomically. |
| [`override sealed Inno.Editor.Interactions.EditorDropStatus Inno.Editor.Interactions.EditorDrop<TSource, TTarget>.Query(Inno.Editor.Interactions.EditorDropContext context)`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDrop.cs#L78) | Evaluates the operation's current availability and presentation state. |
| [`override sealed System.Type Inno.Editor.Interactions.EditorDrop<TSource, TTarget>.sourceType`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDrop.cs#L62) | Gets the concrete type handled by this extension implementation. |
| [`override sealed System.Type Inno.Editor.Interactions.EditorDrop<TSource, TTarget>.targetType`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDrop.cs#L67) | Gets the concrete type handled by this extension implementation. |

### `Inno.Editor.Interactions.EditorDropAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorDropAttribute`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDropAttribute.cs#L8) | Registers a typed editor drop handler for an optional exact area. |
| [`Inno.Editor.Interactions.EditorDropAttribute.EditorDropAttribute(int priority = 0)`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDropAttribute.cs#L17) | Creates a drop registration that can participate on any interaction surface. |
| [`Inno.Editor.Interactions.EditorDropAttribute.EditorDropAttribute(string area, int priority = 0)`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDropAttribute.cs#L31) | Creates a drop registration scoped to an exact interaction area. |
| [`int Inno.Editor.Interactions.EditorDropAttribute.priority`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDropAttribute.cs#L47) | Gets the tie-breaking priority. |
| [`string Inno.Editor.Interactions.EditorDropAttribute.area`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDropAttribute.cs#L42) | Gets the optional exact interaction area. |

### `Inno.Editor.Interactions.EditorDropContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Identity.IdentityObject Inno.Editor.Interactions.EditorDropContext.source`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDropContext.cs#L86) | Gets the live source resolved for this query or delivery operation. |
| [`Inno.Editor.Core.EditorContext Inno.Editor.Interactions.EditorDropContext.editor`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDropContext.cs#L66) | Gets the shared passive editor context. |
| [`Inno.Editor.Interactions.EditorDragData Inno.Editor.Interactions.EditorDropContext.data`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDropContext.cs#L81) | Gets the active managed drag data. |
| [`Inno.Editor.Interactions.EditorDropContext`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDropContext.cs#L11) | Provides contextual state to an editor drop handler. |
| [`Inno.Editor.Interactions.EditorDropContext.EditorDropContext(Inno.Editor.Core.EditorContext editor, Inno.Editor.Interactions.EditorInteractions interactions, string area, Inno.Editor.Interactions.EditorDragData data, Inno.Core.Identity.IdentityObject source, object target, Inno.Editor.Interactions.EditorDropPlacement placement = Inno.Editor.Interactions.EditorDropPlacement.None)`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDropContext.cs#L44) | Creates a managed drop request. |
| [`Inno.Editor.Interactions.EditorDropPlacement Inno.Editor.Interactions.EditorDropContext.placement`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDropContext.cs#L96) | Gets the requested placement relative to the target. |
| [`Inno.Editor.Interactions.EditorInteractions Inno.Editor.Interactions.EditorDropContext.interactions`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDropContext.cs#L71) | Gets the active interaction entry point. |
| [`object Inno.Editor.Interactions.EditorDropContext.target`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDropContext.cs#L91) | Gets the managed drop target. |
| [`string Inno.Editor.Interactions.EditorDropContext.area`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDropContext.cs#L76) | Gets the target interaction area. |

### `Inno.Editor.Interactions.EditorDropContext<TSource, TTarget>`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Core.EditorContext Inno.Editor.Interactions.EditorDropContext<TSource, TTarget>.editor`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDrop.cs#L157) | Gets the active editor context. |
| [`Inno.Editor.Interactions.EditorDropContext Inno.Editor.Interactions.EditorDropContext<TSource, TTarget>.untyped`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDrop.cs#L152) | Gets the untyped drop context. |
| [`Inno.Editor.Interactions.EditorDropContext<TSource, TTarget>`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDrop.cs#L135) | Provides strongly typed source and target values to a drop operation. |
| [`Inno.Editor.Interactions.EditorDropPlacement Inno.Editor.Interactions.EditorDropContext<TSource, TTarget>.placement`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDrop.cs#L182) | Gets the requested placement. |
| [`Inno.Editor.Interactions.EditorInteractions Inno.Editor.Interactions.EditorDropContext<TSource, TTarget>.interactions`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDrop.cs#L162) | Gets the active interaction entry point. |
| [`TSource Inno.Editor.Interactions.EditorDropContext<TSource, TTarget>.source`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDrop.cs#L172) | Gets the typed drag source. |
| [`TTarget Inno.Editor.Interactions.EditorDropContext<TSource, TTarget>.target`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDrop.cs#L177) | Gets the typed drop target. |
| [`string Inno.Editor.Interactions.EditorDropContext<TSource, TTarget>.area`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDrop.cs#L167) | Gets the interaction area. |

### `Inno.Editor.Interactions.EditorDropPlacement`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorDropPlacement`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDropPlacement.cs#L6) | Describes the requested placement relative to an editor drop target. |
| [`Inno.Editor.Interactions.EditorDropPlacement.After`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDropPlacement.cs#L26) | Insert after the target. |
| [`Inno.Editor.Interactions.EditorDropPlacement.Before`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDropPlacement.cs#L16) | Insert before the target. |
| [`Inno.Editor.Interactions.EditorDropPlacement.Into`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDropPlacement.cs#L21) | Drop into the target. |
| [`Inno.Editor.Interactions.EditorDropPlacement.None`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDropPlacement.cs#L11) | No positional placement is requested. |

### `Inno.Editor.Interactions.EditorDropResult`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorDropResult`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDropResult.cs#L6) | Describes the observable result of a completed editor drop. |
| [`Inno.Editor.Interactions.EditorDropResult.EditorDropResult(bool accepted, object? selectionTarget = null, object? revealTarget = null)`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDropResult.cs#L20) | Creates the observable result of a completed drop operation. |
| [`bool Inno.Editor.Interactions.EditorDropResult.accepted`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDropResult.cs#L33) | Gets whether the drop was accepted. |
| [`object? Inno.Editor.Interactions.EditorDropResult.revealTarget`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDropResult.cs#L43) | Gets the optional hierarchy target that should be revealed. |
| [`object? Inno.Editor.Interactions.EditorDropResult.selectionTarget`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDropResult.cs#L38) | Gets the optional target that should become selected. |
| [`static Inno.Editor.Interactions.EditorDropResult Inno.Editor.Interactions.EditorDropResult.Accepted(object? selectionTarget = null, object? revealTarget = null)`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDropResult.cs#L62) | Creates an accepted drop result with optional presentation requests. |
| [`static Inno.Editor.Interactions.EditorDropResult Inno.Editor.Interactions.EditorDropResult.rejected`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDropResult.cs#L48) | Gets a rejected drop result. |

### `Inno.Editor.Interactions.EditorDropStatus`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorDropStatus`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDropStatus.cs#L6) | Describes whether and how a target accepts the active drag. |
| [`Inno.Editor.Interactions.EditorDropStatus.EditorDropStatus(bool canDrop, Inno.Editor.Interactions.EditorDropVisual visual)`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDropStatus.cs#L17) | Creates the compatibility and presentation state of a potential drop target. |
| [`Inno.Editor.Interactions.EditorDropVisual Inno.Editor.Interactions.EditorDropStatus.visual`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDropStatus.cs#L33) | Gets the standard target visual. |
| [`bool Inno.Editor.Interactions.EditorDropStatus.canDrop`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDropStatus.cs#L28) | Gets whether the active source may be dropped. |
| [`static Inno.Editor.Interactions.EditorDropStatus Inno.Editor.Interactions.EditorDropStatus.Accept(Inno.Editor.Interactions.EditorDropVisual visual = Inno.Editor.Interactions.EditorDropVisual.Highlight)`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDropStatus.cs#L49) | Creates an accepted drop status with the requested standard target visual. |
| [`static Inno.Editor.Interactions.EditorDropStatus Inno.Editor.Interactions.EditorDropStatus.rejected`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDropStatus.cs#L38) | Gets an incompatible drop status. |

### `Inno.Editor.Interactions.EditorDropVisual`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorDropVisual`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDropVisual.cs#L6) | Defines the standard visual used for a compatible drop target. |
| [`Inno.Editor.Interactions.EditorDropVisual.Disabled`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDropVisual.cs#L31) | Draw a disabled target visual. |
| [`Inno.Editor.Interactions.EditorDropVisual.Highlight`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDropVisual.cs#L16) | Draw a content highlight. |
| [`Inno.Editor.Interactions.EditorDropVisual.InsertAfter`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDropVisual.cs#L26) | Draw an insertion line after the target. |
| [`Inno.Editor.Interactions.EditorDropVisual.InsertBefore`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDropVisual.cs#L21) | Draw an insertion line before the target. |
| [`Inno.Editor.Interactions.EditorDropVisual.None`](../../src/composition/editor/framework/Inno.Editor.Interactions/DragDrop/EditorDropVisual.cs#L11) | No visual is drawn. |

### `Inno.Editor.Interactions.EditorHistoryAvailability`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorHistoryAvailability`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryAvailability.cs#L8) | Describes whether a history change can currently move in one direction. |
| [`bool Inno.Editor.Interactions.EditorHistoryAvailability.isAvailable`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryAvailability.cs#L21) | Gets whether the requested transition is currently available. |
| [`static Inno.Editor.Interactions.EditorHistoryAvailability Inno.Editor.Interactions.EditorHistoryAvailability.Available()`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryAvailability.cs#L34) | Creates an available transition result. |
| [`static Inno.Editor.Interactions.EditorHistoryAvailability Inno.Editor.Interactions.EditorHistoryAvailability.Unavailable(string message)`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryAvailability.cs#L48) | Creates an unavailable transition result. |
| [`string Inno.Editor.Interactions.EditorHistoryAvailability.message`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryAvailability.cs#L26) | Gets the diagnostic explaining why the transition is unavailable, or an empty string when available. |

### `Inno.Editor.Interactions.EditorHistoryChange`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorHistoryChange`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryChange.cs#L8) | Describes one neutral reload-safe mutation interpreted by an attribute-discovered history handler. |
| [`Inno.Editor.Interactions.EditorHistoryChange.EditorHistoryChange(string kind, Inno.Editor.Interactions.EditorHistoryPayload payload, string? mergeKey = null)`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryChange.cs#L30) | Creates a neutral history change. |
| [`Inno.Editor.Interactions.EditorHistoryPayload Inno.Editor.Interactions.EditorHistoryChange.payload`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryChange.cs#L49) | Gets the immutable neutral payload interpreted by the active handler generation. |
| [`string Inno.Editor.Interactions.EditorHistoryChange.kind`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryChange.cs#L44) | Gets the stable globally unique handler protocol identifier. |
| [`string? Inno.Editor.Interactions.EditorHistoryChange.mergeKey`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryChange.cs#L54) | Gets the optional stable key used to merge adjacent changes to the same logical value. |
| [`void Inno.Editor.Interactions.EditorHistoryChange.Dispose()`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryChange.cs#L73) | Releases the payload storage owned by this history change. |

### `Inno.Editor.Interactions.EditorHistoryContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Core.EditorContext Inno.Editor.Interactions.EditorHistoryContext.editor`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryContext.cs#L23) | Gets the passive editor context for the active project. |
| [`Inno.Editor.Interactions.EditorHistoryContext`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryContext.cs#L10) | Provides current-generation editor services to a history change handler. |
| [`Inno.Editor.Interactions.EditorInteractions Inno.Editor.Interactions.EditorHistoryContext.interactions`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryContext.cs#L28) | Gets the active interaction entry point used for selection and feature coordination. |

### `Inno.Editor.Interactions.EditorHistoryDirection`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorHistoryDirection`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryDirection.cs#L6) | Identifies the direction in which an editor history change is being applied. |
| [`Inno.Editor.Interactions.EditorHistoryDirection.Redo`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryDirection.cs#L16) | Restores the state produced by the committed change. |
| [`Inno.Editor.Interactions.EditorHistoryDirection.Undo`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryDirection.cs#L11) | Restores the state that existed before the change was committed. |

### `Inno.Editor.Interactions.EditorHistoryHandler`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorHistoryHandler`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryHandler.cs#L8) | Interprets neutral editor history payloads for one current-generation feature protocol. |
| [`abstract Inno.Editor.Interactions.EditorHistoryAvailability Inno.Editor.Interactions.EditorHistoryHandler.Query(Inno.Editor.Interactions.EditorHistoryContext context, Inno.Editor.Interactions.EditorHistoryChange change, Inno.Editor.Interactions.EditorHistoryDirection direction)`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryHandler.cs#L25) | Determines whether a neutral change can currently transition in the requested direction. |
| [`abstract Inno.Editor.Interactions.EditorHistoryResult Inno.Editor.Interactions.EditorHistoryHandler.Apply(Inno.Editor.Interactions.EditorHistoryContext context, Inno.Editor.Interactions.EditorHistoryChange change, Inno.Editor.Interactions.EditorHistoryDirection direction)`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryHandler.cs#L46) | Atomically applies a neutral history change in the requested direction. |
| [`static Inno.Editor.Interactions.EditorHistoryResult Inno.Editor.Interactions.EditorHistoryHandler.StateIntegrityFailure(string message)`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryHandler.cs#L85) | Creates a failed result for a transition whose compensation also failed. |
| [`virtual bool Inno.Editor.Interactions.EditorHistoryHandler.TryMerge(Inno.Editor.Interactions.EditorHistoryChange older, Inno.Editor.Interactions.EditorHistoryChange newer, out Inno.Editor.Interactions.EditorHistoryChange? merged)`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryHandler.cs#L67) | Attempts to merge two adjacent changes to the same logical value. |

### `Inno.Editor.Interactions.EditorHistoryHandlerAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorHistoryHandlerAttribute`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryHandlerAttribute.cs#L8) | Registers a stateless editor history handler for one stable change protocol. |
| [`Inno.Editor.Interactions.EditorHistoryHandlerAttribute.EditorHistoryHandlerAttribute(string kind)`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryHandlerAttribute.cs#L20) | Creates a history handler registration. |
| [`string Inno.Editor.Interactions.EditorHistoryHandlerAttribute.kind`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryHandlerAttribute.cs#L29) | Gets the stable globally unique change protocol identifier. |

### `Inno.Editor.Interactions.EditorHistoryOptions`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorHistoryOptions`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryOptions.cs#L8) | Configures retention and payload storage for one editor history. |
| [`int Inno.Editor.Interactions.EditorHistoryOptions.inlinePayloadThreshold`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryOptions.cs#L28) | Gets or initializes the payload size at which immutable bytes are moved to the temporary disk store. |
| [`int Inno.Editor.Interactions.EditorHistoryOptions.maxEntries`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryOptions.cs#L13) | Gets or initializes the maximum number of committed top-level entries retained in memory. |
| [`long Inno.Editor.Interactions.EditorHistoryOptions.maxDiskBytes`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryOptions.cs#L23) | Gets or initializes the maximum payload bytes retained in the temporary disk store. |
| [`long Inno.Editor.Interactions.EditorHistoryOptions.maxResidentBytes`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryOptions.cs#L18) | Gets or initializes the maximum estimated resident payload bytes retained by the history. |
| [`string? Inno.Editor.Interactions.EditorHistoryOptions.cacheDirectory`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryOptions.cs#L33) | Gets or initializes the optional directory that owns temporary history payloads for this editor session. |

### `Inno.Editor.Interactions.EditorHistoryPayload`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorHistoryPayload`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryPayload.cs#L9) | Owns immutable neutral bytes used by a reload-safe editor history change. |
| [`bool Inno.Editor.Interactions.EditorHistoryPayload.isStoredOnDisk`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryPayload.cs#L40) | Gets whether the payload is retained in the temporary disk store instead of resident memory. |
| [`byte[] Inno.Editor.Interactions.EditorHistoryPayload.ReadBytes()`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryPayload.cs#L66) | Reads the complete immutable payload into a newly allocated byte array. |
| [`long Inno.Editor.Interactions.EditorHistoryPayload.length`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryPayload.cs#L35) | Gets the number of immutable bytes represented by this payload. |
| [`static Inno.Editor.Interactions.EditorHistoryPayload Inno.Editor.Interactions.EditorHistoryPayload.FromBytes(System.ReadOnlySpan<byte> bytes)`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryPayload.cs#L55) | Creates an immutable history payload by copying the supplied bytes. |
| [`void Inno.Editor.Interactions.EditorHistoryPayload.Dispose()`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryPayload.cs#L90) | Releases the resident or temporary disk storage owned by this payload. |

### `Inno.Editor.Interactions.EditorHistoryResult`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorHistoryResult`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryResult.cs#L8) | Describes the outcome of applying, undoing, or redoing an editor history operation. |
| [`bool Inno.Editor.Interactions.EditorHistoryResult.statePreserved`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryResult.cs#L32) | Gets whether a failed transition restored the domain state that existed before the attempt. |
| [`bool Inno.Editor.Interactions.EditorHistoryResult.succeeded`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryResult.cs#L23) | Gets whether the requested history transition completed successfully. |
| [`static Inno.Editor.Interactions.EditorHistoryResult Inno.Editor.Interactions.EditorHistoryResult.Failure(string message)`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryResult.cs#L59) | Creates a failed history result without changing the owning history stack. |
| [`static Inno.Editor.Interactions.EditorHistoryResult Inno.Editor.Interactions.EditorHistoryResult.Success()`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryResult.cs#L45) | Creates a successful history result. |
| [`string Inno.Editor.Interactions.EditorHistoryResult.message`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryResult.cs#L37) | Gets the diagnostic message associated with a failed transition, or an empty string after success. |

### `Inno.Editor.Interactions.EditorHistoryTransaction`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorHistoryResult Inno.Editor.Interactions.EditorHistoryTransaction.Rollback()`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryTransaction.cs#L52) | Reverts every operation recorded by the transaction and does not add a history entry. |
| [`Inno.Editor.Interactions.EditorHistoryTransaction`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryTransaction.cs#L8) | Collects several history operations into one atomic undo and redo entry. |
| [`string Inno.Editor.Interactions.EditorHistoryTransaction.name`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryTransaction.cs#L26) | Gets the user-facing name assigned to the transaction. |
| [`void Inno.Editor.Interactions.EditorHistoryTransaction.Commit()`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryTransaction.cs#L36) | Commits all recorded child operations as one atomic history entry. |
| [`void Inno.Editor.Interactions.EditorHistoryTransaction.Dispose()`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/EditorHistoryTransaction.cs#L66) | Rolls back an uncommitted transaction before releasing it. |

### `Inno.Editor.Interactions.EditorInteraction`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Identity.RuntimeIdentity Inno.Editor.Interactions.EditorInteraction.BeginDrag(Inno.Editor.Interactions.EditorDragData data)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteraction.cs#L205) | Begins a managed drag originating from this area. |
| [`Inno.Editor.Interactions.EditorActionState Inno.Editor.Interactions.EditorInteraction.Query(string action, object? argument = null)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteraction.cs#L72) | Queries an action for this area and target. |
| [`Inno.Editor.Interactions.EditorDropResult Inno.Editor.Interactions.EditorInteraction.Drop(Inno.Core.Identity.RuntimeIdentity identity, Inno.Editor.Interactions.EditorDropPlacement placement = Inno.Editor.Interactions.EditorDropPlacement.None)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteraction.cs#L237) | Delivers a managed drop to this target. |
| [`Inno.Editor.Interactions.EditorDropStatus Inno.Editor.Interactions.EditorInteraction.QueryDrop(Inno.Core.Identity.RuntimeIdentity identity, Inno.Editor.Interactions.EditorDropPlacement placement = Inno.Editor.Interactions.EditorDropPlacement.None)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteraction.cs#L219) | Queries this handle as a drop target. |
| [`Inno.Editor.Interactions.EditorInteraction`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteraction.cs#L10) | Provides a lightweight fluent handle for one interaction area and optional target. |
| [`Inno.Editor.Interactions.EditorMenuModel Inno.Editor.Interactions.EditorInteraction.BuildMenu()`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteraction.cs#L154) | Builds the complete contextual menu for this area and target. |
| [`Inno.Editor.Interactions.EditorToolbarModel Inno.Editor.Interactions.EditorInteraction.BuildToolbar()`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteraction.cs#L162) | Builds the complete compact toolbar for this area and target. |
| [`bool Inno.Editor.Interactions.EditorInteraction.Execute(string action, object? argument = null)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteraction.cs#L92) | Executes an action for this area and target. |
| [`bool Inno.Editor.Interactions.EditorInteraction.IsActive(string action)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteraction.cs#L146) | Gets whether an action owns an active multi-frame operation for this target. |
| [`bool Inno.Editor.Interactions.EditorInteraction.Present(string action, object? argument = null)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteraction.cs#L129) | Presents an active action in place of this target's normal content. |
| [`bool Inno.Editor.Interactions.EditorInteraction.Select()`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteraction.cs#L52) | Selects this target, or clears selection when the target is . |
| [`bool Inno.Editor.Interactions.EditorInteraction.TryGetActiveDragIdentity(out Inno.Core.Identity.RuntimeIdentity identity)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteraction.cs#L194) | Gets the currently active drag identity, if its source remains registered. |
| [`bool Inno.Editor.Interactions.EditorInteraction.TryGetShortcut(string action, out Inno.Editor.Interactions.HotKeyGesture gesture)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteraction.cs#L179) | Resolves the shortcut displayed for an action in this area. |
| [`bool Inno.Editor.Interactions.EditorInteraction.isSelected`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteraction.cs#L38) | Gets whether this handle's target is the current editor selection. |
| [`object? Inno.Editor.Interactions.EditorInteraction.target`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteraction.cs#L33) | Gets the optional target represented by this handle. |
| [`string Inno.Editor.Interactions.EditorInteraction.area`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteraction.cs#L28) | Gets the stable interaction area. |
| [`void Inno.Editor.Interactions.EditorInteraction.Enqueue(string action, object? argument = null)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteraction.cs#L109) | Queues an action until the current UI traversal completes. |
| [`void Inno.Editor.Interactions.EditorInteraction.Focus()`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteraction.cs#L44) | Marks this area and target as the active keyboard context. |

### `Inno.Editor.Interactions.EditorInteractionRuntime`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorInteractionRuntime`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteractionRuntime.cs#L17) | Hosts attribute-discovered editor extensions without depending on a presentation backend. |
| [`Inno.Editor.Interactions.EditorInteractionRuntime.EditorInteractionRuntime(Inno.Editor.Core.EditorContext context, Inno.Extensibility.Types.TypeCatalog types, Inno.Core.Logging.LogRouter logs)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteractionRuntime.cs#L71) | Creates an interaction runtime for an existing passive editor context. |
| [`Inno.Editor.Interactions.EditorInteractionRuntime.EditorInteractionRuntime(Inno.Editor.Core.EditorContext context, Inno.Extensibility.Types.TypeCatalog types, Inno.Core.Logging.LogRouter logs, System.Collections.Generic.IEnumerable<object> hostServices)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteractionRuntime.cs#L98) | Creates an interaction runtime with stable host-owned extension services. |
| [`Inno.Editor.Interactions.EditorInteractionRuntime.EditorInteractionRuntime(string projectDirectory, Inno.Extensibility.Types.TypeCatalog types, Inno.Core.Logging.LogRouter logs, Inno.Editor.Core.EditorKeyboardPolicy keyboard)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteractionRuntime.cs#L46) | Creates an interaction runtime for one project. |
| [`Inno.Editor.Interactions.EditorInteractions Inno.Editor.Interactions.EditorInteractionRuntime.interactions`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteractionRuntime.cs#L128) | Gets the active presentation-independent interaction entry point. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Editor.Interactions.EditorModalExtension> Inno.Editor.Interactions.EditorInteractionRuntime.modals`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteractionRuntime.cs#L145) | Gets active modal extensions in deterministic order. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Editor.Interactions.EditorPanelExtension> Inno.Editor.Interactions.EditorInteractionRuntime.panels`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteractionRuntime.cs#L133) | Gets active dockable panel extensions in deterministic order. |
| [`int Inno.Editor.Interactions.EditorInteractionRuntime.panelCount`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteractionRuntime.cs#L157) | Gets the number of active dockable panels. |
| [`override void Inno.Editor.Interactions.EditorInteractionRuntime.Dispose()`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteractionRuntime.cs#L248) | Releases the resources owned by this implementation. |
| [`override void Inno.Editor.Interactions.EditorInteractionRuntime.Start()`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteractionRuntime.cs#L162) | Starts value processing after validating the current state. |
| [`override void Inno.Editor.Interactions.EditorInteractionRuntime.Update(Inno.Editor.Core.EditorFrame frame)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteractionRuntime.cs#L179) | Recomputes owned state from the current validated inputs. |
| [`void Inno.Editor.Interactions.EditorInteractionRuntime.Flush()`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteractionRuntime.cs#L194) | Flushes actions queued during the current presentation traversal. |
| [`void Inno.Editor.Interactions.EditorInteractionRuntime.HandleKeyPressed(Inno.Core.Events.KeyPressedEvent keyEvent)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteractionRuntime.cs#L232) | Dispatches an unhandled keyboard event through contextual shortcuts. |
| [`void Inno.Editor.Interactions.EditorInteractionRuntime.PrepareShutdown()`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteractionRuntime.cs#L220) | Freezes automatic extension-state persistence and writes the final state before modules begin shutting down. |
| [`void Inno.Editor.Interactions.EditorInteractionRuntime.SaveState()`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteractionRuntime.cs#L203) | Captures every stateful active module and panel and atomically flushes changed project state to disk. |

### `Inno.Editor.Interactions.EditorInteractions`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorInteraction Inno.Editor.Interactions.EditorInteractions.For(string area, object? target = null)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteractions.cs#L137) | Creates a lightweight interaction handle for one area and optional target. |
| [`Inno.Editor.Interactions.EditorInteractions`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteractions.cs#L18) | Provides the single presentation-independent entry point for editor actions, menus, selection, focus, and drag-and-drop. |
| [`Inno.Editor.Interactions.EditorSelectionState Inno.Editor.Interactions.EditorInteractions.selection`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteractions.cs#L57) | Gets the shared read-only editor selection state. |
| [`Inno.Editor.Interactions.IEditorDocumentService Inno.Editor.Interactions.EditorInteractions.documents`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteractions.cs#L69) | Gets the headless reload-safe document lifetime used by dedicated asset editors and Inspectors. |
| [`Inno.Editor.Interactions.IEditorHistory Inno.Editor.Interactions.EditorInteractions.history`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteractions.cs#L64) | Gets the transactional Undo and Redo history owned by this editor runtime. |
| [`System.IDisposable Inno.Editor.Interactions.EditorInteractions.BeginHistoryIsolation()`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteractions.cs#L107) | Starts an isolated temporary Undo and Redo branch while retaining the current editing branch. |
| [`bool Inno.Editor.Interactions.EditorInteractions.ClosePanel(string panelId)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteractions.cs#L284) | Closes one panel in the active extension generation without toggling its current state. |
| [`bool Inno.Editor.Interactions.EditorInteractions.OpenPanel(string panelId)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteractions.cs#L266) | Opens and requests presentation focus for one panel in the active extension generation. |
| [`bool Inno.Editor.Interactions.EditorInteractions.TogglePanel(string panelId)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteractions.cs#L197) | Toggles one panel in the currently active extension generation. |
| [`bool Inno.Editor.Interactions.EditorInteractions.TryGetActiveDragIdentity(out Inno.Core.Identity.RuntimeIdentity identity)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteractions.cs#L183) | Gets the active drag identity used to select the matching native payload domain. |
| [`bool Inno.Editor.Interactions.EditorInteractions.TryGetDragSource(Inno.Core.Identity.RuntimeIdentity identity, out Inno.Core.Identity.IdentityObject? source)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteractions.cs#L169) | Resolves the live source for an active runtime identity. |
| [`bool Inno.Editor.Interactions.EditorInteractions.TryGetModule<TModule>(out TModule? module)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteractions.cs#L86) | Resolves an active feature module for immediate use in the current Editor callback. |
| [`bool Inno.Editor.Interactions.EditorInteractions.TryResolveIdentity(Inno.Core.Identity.IdentityDomainId domainId, System.Guid persistentId, out Inno.Core.Identity.IdentityObject? target)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteractions.cs#L241) | Resolves a persistent identity through one explicitly selected Editor identity domain. |
| [`bool Inno.Editor.Interactions.EditorInteractions.TryResolveIdentity(Inno.Core.Identity.RuntimeIdentity identity, out Inno.Core.Identity.IdentityObject? target)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteractions.cs#L215) | Resolves a domain-qualified runtime identity through the Editor's complete identity-domain set. |
| [`object? Inno.Editor.Interactions.EditorInteractions.focusedTarget`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteractions.cs#L120) | Gets the target associated with the focused area. |
| [`string Inno.Editor.Interactions.EditorInteractions.focusedArea`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteractions.cs#L115) | Gets the area that most recently received keyboard focus. |
| [`void Inno.Editor.Interactions.EditorInteractions.SetSelection(object? target)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorInteractions.cs#L148) | Replaces the editor selection after closing presentations owned by other targets. |

### `Inno.Editor.Interactions.EditorMenuAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorMenuAttribute`](../../src/composition/editor/framework/Inno.Editor.Interactions/Menus/EditorMenuAttribute.cs#L8) | Places an editor action at an arbitrary path on a menu surface. |
| [`Inno.Editor.Interactions.EditorMenuAttribute.EditorMenuAttribute(string area, string path, int order = 0, bool separatorBefore = false)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Menus/EditorMenuAttribute.cs#L29) | Creates a static placement for the annotated action on an exact menu surface. |
| [`bool Inno.Editor.Interactions.EditorMenuAttribute.separatorBefore`](../../src/composition/editor/framework/Inno.Editor.Interactions/Menus/EditorMenuAttribute.cs#L63) | Gets whether a separator is rendered before the item. |
| [`int Inno.Editor.Interactions.EditorMenuAttribute.order`](../../src/composition/editor/framework/Inno.Editor.Interactions/Menus/EditorMenuAttribute.cs#L58) | Gets the stable menu ordering value. |
| [`string Inno.Editor.Interactions.EditorMenuAttribute.area`](../../src/composition/editor/framework/Inno.Editor.Interactions/Menus/EditorMenuAttribute.cs#L48) | Gets the menu area. |
| [`string Inno.Editor.Interactions.EditorMenuAttribute.path`](../../src/composition/editor/framework/Inno.Editor.Interactions/Menus/EditorMenuAttribute.cs#L53) | Gets the slash-delimited menu path. |

### `Inno.Editor.Interactions.EditorMenuBuilder`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorMenuBuilder`](../../src/composition/editor/framework/Inno.Editor.Interactions/Menus/EditorMenuBuilder.cs#L9) | Collects dynamic menu item placements. |
| [`void Inno.Editor.Interactions.EditorMenuBuilder.Add(string path, string actionId, int order = 0, bool separatorBefore = false, object? argument = null)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Menus/EditorMenuBuilder.cs#L57) | Adds a dynamic action placement to the menu currently being constructed. |
| [`void Inno.Editor.Interactions.EditorMenuBuilder.AddGroup(string path, int order = 0, bool separatorBefore = false)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Menus/EditorMenuBuilder.cs#L26) | Declares presentation for an intermediate menu group without inventing a no-op action. |

### `Inno.Editor.Interactions.EditorMenuContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Core.EditorContext Inno.Editor.Interactions.EditorMenuContext.editor`](../../src/composition/editor/framework/Inno.Editor.Interactions/Menus/EditorMenuContext.cs#L49) | Gets the shared passive editor context. |
| [`Inno.Editor.Interactions.EditorInteractions Inno.Editor.Interactions.EditorMenuContext.interactions`](../../src/composition/editor/framework/Inno.Editor.Interactions/Menus/EditorMenuContext.cs#L54) | Gets the active interaction entry point. |
| [`Inno.Editor.Interactions.EditorMenuContext`](../../src/composition/editor/framework/Inno.Editor.Interactions/Menus/EditorMenuContext.cs#L10) | Provides contextual state while constructing an editor menu. |
| [`Inno.Editor.Interactions.EditorMenuContext.EditorMenuContext(Inno.Editor.Core.EditorContext editor, Inno.Editor.Interactions.EditorInteractions interactions, string area, object? target = null)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Menus/EditorMenuContext.cs#L33) | Creates a contextual menu request. |
| [`object? Inno.Editor.Interactions.EditorMenuContext.target`](../../src/composition/editor/framework/Inno.Editor.Interactions/Menus/EditorMenuContext.cs#L64) | Gets the contextual menu target. |
| [`string Inno.Editor.Interactions.EditorMenuContext.area`](../../src/composition/editor/framework/Inno.Editor.Interactions/Menus/EditorMenuContext.cs#L59) | Gets the requested menu area. |

### `Inno.Editor.Interactions.EditorMenuItem`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorActionState Inno.Editor.Interactions.EditorMenuItem.status`](../../src/composition/editor/framework/Inno.Editor.Interactions/Menus/EditorMenuItem.cs#L79) | Gets the current command presentation state. |
| [`Inno.Editor.Interactions.EditorMenuItem`](../../src/composition/editor/framework/Inno.Editor.Interactions/Menus/EditorMenuItem.cs#L11) | Represents one immutable contextual menu node. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Editor.Interactions.EditorMenuItem> Inno.Editor.Interactions.EditorMenuItem.children`](../../src/composition/editor/framework/Inno.Editor.Interactions/Menus/EditorMenuItem.cs#L84) | Gets child menu nodes. |
| [`bool Inno.Editor.Interactions.EditorMenuItem.separatorBefore`](../../src/composition/editor/framework/Inno.Editor.Interactions/Menus/EditorMenuItem.cs#L74) | Gets whether a separator precedes this node. |
| [`int Inno.Editor.Interactions.EditorMenuItem.order`](../../src/composition/editor/framework/Inno.Editor.Interactions/Menus/EditorMenuItem.cs#L69) | Gets the stable ordering value. |
| [`object? Inno.Editor.Interactions.EditorMenuItem.argument`](../../src/composition/editor/framework/Inno.Editor.Interactions/Menus/EditorMenuItem.cs#L89) | Gets the generation-local action argument consumed by a presentation backend. |
| [`string Inno.Editor.Interactions.EditorMenuItem.actionId`](../../src/composition/editor/framework/Inno.Editor.Interactions/Menus/EditorMenuItem.cs#L63) | Gets the stable action identifier consumed by a presentation backend. |
| [`string Inno.Editor.Interactions.EditorMenuItem.label`](../../src/composition/editor/framework/Inno.Editor.Interactions/Menus/EditorMenuItem.cs#L58) | Gets the visible node label. |

### `Inno.Editor.Interactions.EditorMenuModel`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorMenuModel`](../../src/composition/editor/framework/Inno.Editor.Interactions/Menus/EditorMenuModel.cs#L9) | Contains a complete immutable menu tree. |
| [`Inno.Editor.Interactions.EditorMenuModel.EditorMenuModel(System.Collections.Generic.IReadOnlyList<Inno.Editor.Interactions.EditorMenuItem>? items)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Menus/EditorMenuModel.cs#L17) | Creates a complete immutable menu tree from resolved root nodes. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Editor.Interactions.EditorMenuItem> Inno.Editor.Interactions.EditorMenuModel.items`](../../src/composition/editor/framework/Inno.Editor.Interactions/Menus/EditorMenuModel.cs#L25) | Gets root menu nodes in display order. |

### `Inno.Editor.Interactions.EditorMenuSource`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorMenuSource`](../../src/composition/editor/framework/Inno.Editor.Interactions/Menus/EditorMenuSource.cs#L8) | Contributes dynamic entries to one or more editor menu surfaces. |
| [`abstract void Inno.Editor.Interactions.EditorMenuSource.Build(Inno.Editor.Interactions.EditorMenuContext context, Inno.Editor.Interactions.EditorMenuBuilder builder)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Menus/EditorMenuSource.cs#L19) | Adds context-dependent placements for the supplied menu request. |

### `Inno.Editor.Interactions.EditorMenuSourceAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorMenuSourceAttribute`](../../src/composition/editor/framework/Inno.Editor.Interactions/Menus/EditorMenuSourceAttribute.cs#L8) | Registers a dynamic editor menu source. |
| [`Inno.Editor.Interactions.EditorMenuSourceAttribute.EditorMenuSourceAttribute(string area, int priority = 0)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Menus/EditorMenuSourceAttribute.cs#L23) | Creates a dynamic menu-source registration for an exact interaction surface. |
| [`int Inno.Editor.Interactions.EditorMenuSourceAttribute.priority`](../../src/composition/editor/framework/Inno.Editor.Interactions/Menus/EditorMenuSourceAttribute.cs#L41) | Gets the provider ordering priority. |
| [`string Inno.Editor.Interactions.EditorMenuSourceAttribute.area`](../../src/composition/editor/framework/Inno.Editor.Interactions/Menus/EditorMenuSourceAttribute.cs#L36) | Gets the contributed menu area. |

### `Inno.Editor.Interactions.EditorModalExtension`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorModalExtension`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorModalExtension.cs#L11) | Describes one active modal extension. |
| [`bool Inno.Editor.Interactions.EditorModalExtension.Draw(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorModalExtension.cs#L95) | Safely draws the modal body and quarantines a failing extension instance. |
| [`bool Inno.Editor.Interactions.EditorModalExtension.TryGetPresentation(out Inno.Editor.Interactions.EditorModalExtension.Presentation presentation)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorModalExtension.cs#L55) | Safely captures generation-local modal presentation values. |
| [`int Inno.Editor.Interactions.EditorModalExtension.order`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorModalExtension.cs#L44) | Gets the stable modal ordering value. |
| [`string Inno.Editor.Interactions.EditorModalExtension.id`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorModalExtension.cs#L34) | Gets the stable modal identifier. |
| [`string Inno.Editor.Interactions.EditorModalExtension.title`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorModalExtension.cs#L39) | Gets the visible modal title. |

### `Inno.Editor.Interactions.EditorModalExtension.Presentation`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorModalExtension.Presentation`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorModalExtension.cs#L116) | Stores immutable modal window policy values read from one extension generation. |
| [`Inno.Editor.Interactions.EditorModalExtension.Presentation.Presentation(bool isVisible, bool blocksInteraction, bool canMove, bool canResize, bool allowScrolling, System.Numerics.Vector2 initialSize, System.Numerics.Vector2 minimumSize)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorModalExtension.cs#L142) | Creates an immutable modal presentation snapshot. |
| [`System.Numerics.Vector2 Inno.Editor.Interactions.EditorModalExtension.Presentation.initialSize`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorModalExtension.cs#L188) | Gets the initial size in unscaled editor units. |
| [`System.Numerics.Vector2 Inno.Editor.Interactions.EditorModalExtension.Presentation.minimumSize`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorModalExtension.cs#L193) | Gets the minimum size in unscaled editor units. |
| [`bool Inno.Editor.Interactions.EditorModalExtension.Presentation.allowScrolling`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorModalExtension.cs#L183) | Gets whether overflow scrolling belongs to the modal window. |
| [`bool Inno.Editor.Interactions.EditorModalExtension.Presentation.blocksInteraction`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorModalExtension.cs#L168) | Gets whether the modal blocks regular editor interaction. |
| [`bool Inno.Editor.Interactions.EditorModalExtension.Presentation.canMove`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorModalExtension.cs#L173) | Gets whether the modal window can be moved. |
| [`bool Inno.Editor.Interactions.EditorModalExtension.Presentation.canResize`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorModalExtension.cs#L178) | Gets whether the modal window can be resized. |
| [`bool Inno.Editor.Interactions.EditorModalExtension.Presentation.isVisible`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorModalExtension.cs#L163) | Gets whether the modal should currently be visible. |

### `Inno.Editor.Interactions.EditorPanelExtension`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorPanelExtension`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorPanelExtension.cs#L11) | Describes one active dockable panel extension. |
| [`bool Inno.Editor.Interactions.EditorPanelExtension.Draw(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorPanelExtension.cs#L118) | Safely draws the panel body and quarantines a failing extension instance. |
| [`bool Inno.Editor.Interactions.EditorPanelExtension.TakeFocusRequest()`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorPanelExtension.cs#L64) | Consumes a pending request to focus this panel window. |
| [`bool Inno.Editor.Interactions.EditorPanelExtension.TryGetWindowPresentation(out bool useWindowPadding, out bool allowScrolling, out System.Numerics.Vector2 initialSize)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorPanelExtension.cs#L81) | Safely reads the panel window-presentation policy through the active extension boundary. |
| [`bool Inno.Editor.Interactions.EditorPanelExtension.isOpen`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorPanelExtension.cs#L52) | Gets or sets whether this panel is open in the current extension generation. |
| [`int Inno.Editor.Interactions.EditorPanelExtension.order`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorPanelExtension.cs#L47) | Gets the stable panel ordering value. |
| [`string Inno.Editor.Interactions.EditorPanelExtension.id`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorPanelExtension.cs#L37) | Gets the stable panel identifier. |
| [`string Inno.Editor.Interactions.EditorPanelExtension.title`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorPanelExtension.cs#L42) | Gets the visible panel title. |

### `Inno.Editor.Interactions.EditorPlanarNavigation`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorPlanarNavigation`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorPlanarNavigation.cs#L9) | Shares captured planar gestures and mouse-anchored zoom math between scene and graph canvases. |
| [`bool Inno.Editor.Interactions.EditorPlanarNavigation.Update(bool hovered, bool primaryPressed, bool middlePressed, bool primaryDown, bool middleDown, bool alt, bool allowAltPrimary = true)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorPlanarNavigation.cs#L45) | Advances pointer capture using logical button state rather than window-local release events. |
| [`bool Inno.Editor.Interactions.EditorPlanarNavigation.isPanning`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorPlanarNavigation.cs#L16) | Gets whether this canvas owns a pan gesture, including while the pointer is outside its bounds. |
| [`static System.Numerics.Vector2 Inno.Editor.Interactions.EditorPlanarNavigation.ZoomOrigin(System.Numerics.Vector2 origin, System.Numerics.Vector2 pivot, float previousScale, float nextScale)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorPlanarNavigation.cs#L116) | Changes the screen-space origin so zoom preserves the same content point beneath the cursor. |
| [`static float Inno.Editor.Interactions.EditorPlanarNavigation.WheelFactor(float wheel, float sensitivity = 0.16)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorPlanarNavigation.cs#L84) | Converts wheel input to the common exponential magnification used by Editor canvases. |
| [`void Inno.Editor.Interactions.EditorPlanarNavigation.Cancel()`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorPlanarNavigation.cs#L67) | Releases capture when a canvas closes or a gesture is cancelled. |

### `Inno.Editor.Interactions.EditorPresentationAction<TTarget, TPresentation>`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorPresentationAction<TTarget, TPresentation>`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorAction.cs#L346) | Defines a target action with a typed presentation-only argument. |
| [`abstract bool Inno.Editor.Interactions.EditorPresentationAction<TTarget, TPresentation>.Present(Inno.Editor.Interactions.EditorActionContext<TTarget, TPresentation> context)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorAction.cs#L371) | Presents active content using strongly typed presentation data. |
| [`override sealed bool Inno.Editor.Interactions.EditorPresentationAction<TTarget, TPresentation>.Present(Inno.Editor.Interactions.EditorActionContext<TTarget> context)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorAction.cs#L358) | Presents this action through the current editor interaction surface. |

### `Inno.Editor.Interactions.EditorSelectionState`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorSelectionState`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorSelectionState.cs#L9) | Stores editor-wide object selection state. |
| [`bool Inno.Editor.Interactions.EditorSelectionState.TryGet<TTarget>(out TTarget? target)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorSelectionState.cs#L67) | Tries to read the current target as a requested type. |
| [`object? Inno.Editor.Interactions.EditorSelectionState.selectedTarget`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorSelectionState.cs#L16) | Gets the selected target, or when nothing is selected. |
| [`ulong Inno.Editor.Interactions.EditorSelectionState.version`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/EditorSelectionState.cs#L21) | Gets a monotonically increasing selection change version. |

### `Inno.Editor.Interactions.EditorShortcutAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Input.KeyCode Inno.Editor.Interactions.EditorShortcutAttribute.key`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorShortcutAttribute.cs#L69) | Gets the shortcut key. |
| [`Inno.Core.Input.KeyModifier Inno.Editor.Interactions.EditorShortcutAttribute.modifiers`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorShortcutAttribute.cs#L74) | Gets additional shortcut modifiers. |
| [`Inno.Editor.Interactions.EditorShortcutAttribute`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorShortcutAttribute.cs#L10) | Associates an editor action with a keyboard shortcut. |
| [`Inno.Editor.Interactions.EditorShortcutAttribute.EditorShortcutAttribute(Inno.Core.Input.KeyCode key, Inno.Core.Input.KeyModifier modifiers = Inno.Core.Input.KeyModifier.None, bool primary = false)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorShortcutAttribute.cs#L25) | Creates a shortcut that can be dispatched from any focused interaction surface. |
| [`Inno.Editor.Interactions.EditorShortcutAttribute.EditorShortcutAttribute(string area, Inno.Core.Input.KeyCode key, Inno.Core.Input.KeyModifier modifiers = Inno.Core.Input.KeyModifier.None, bool primary = false)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorShortcutAttribute.cs#L49) | Creates a shortcut scoped to an exact interaction area. |
| [`bool Inno.Editor.Interactions.EditorShortcutAttribute.primary`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorShortcutAttribute.cs#L79) | Gets whether the platform primary modifier is required. |
| [`string Inno.Editor.Interactions.EditorShortcutAttribute.area`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorShortcutAttribute.cs#L64) | Gets the optional exact interaction area. |

### `Inno.Editor.Interactions.EditorToolbarIcon`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorToolbarIcon`](../../src/composition/editor/framework/Inno.Editor.Interactions/Toolbars/EditorToolbarIcon.cs#L6) | Identifies a presentation-independent symbol for an editor toolbar command. |
| [`Inno.Editor.Interactions.EditorToolbarIcon.Edit`](../../src/composition/editor/framework/Inno.Editor.Interactions/Toolbars/EditorToolbarIcon.cs#L36) | Returns to an editing state. |
| [`Inno.Editor.Interactions.EditorToolbarIcon.None`](../../src/composition/editor/framework/Inno.Editor.Interactions/Toolbars/EditorToolbarIcon.cs#L11) | No symbol is requested. |
| [`Inno.Editor.Interactions.EditorToolbarIcon.Pause`](../../src/composition/editor/framework/Inno.Editor.Interactions/Toolbars/EditorToolbarIcon.cs#L26) | Pauses an operation or simulation. |
| [`Inno.Editor.Interactions.EditorToolbarIcon.Play`](../../src/composition/editor/framework/Inno.Editor.Interactions/Toolbars/EditorToolbarIcon.cs#L16) | Starts an operation or simulation. |
| [`Inno.Editor.Interactions.EditorToolbarIcon.Step`](../../src/composition/editor/framework/Inno.Editor.Interactions/Toolbars/EditorToolbarIcon.cs#L31) | Advances a paused operation by one step. |
| [`Inno.Editor.Interactions.EditorToolbarIcon.Stop`](../../src/composition/editor/framework/Inno.Editor.Interactions/Toolbars/EditorToolbarIcon.cs#L21) | Stops an operation or simulation. |

### `Inno.Editor.Interactions.EditorToolbarItem`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorActionState Inno.Editor.Interactions.EditorToolbarItem.status`](../../src/composition/editor/framework/Inno.Editor.Interactions/Toolbars/EditorToolbarItem.cs#L45) | Gets the current action availability and checked state. |
| [`Inno.Editor.Interactions.EditorToolbarIcon Inno.Editor.Interactions.EditorToolbarItem.icon`](../../src/composition/editor/framework/Inno.Editor.Interactions/Toolbars/EditorToolbarItem.cs#L30) | Gets the resolved presentation-independent symbol. |
| [`Inno.Editor.Interactions.EditorToolbarItem`](../../src/composition/editor/framework/Inno.Editor.Interactions/Toolbars/EditorToolbarItem.cs#L6) | Describes one resolved toolbar command. |
| [`int Inno.Editor.Interactions.EditorToolbarItem.order`](../../src/composition/editor/framework/Inno.Editor.Interactions/Toolbars/EditorToolbarItem.cs#L40) | Gets the stable ordering value. |
| [`string Inno.Editor.Interactions.EditorToolbarItem.actionId`](../../src/composition/editor/framework/Inno.Editor.Interactions/Toolbars/EditorToolbarItem.cs#L25) | Gets the stable action dispatched when this item is pressed. |
| [`string Inno.Editor.Interactions.EditorToolbarItem.tooltip`](../../src/composition/editor/framework/Inno.Editor.Interactions/Toolbars/EditorToolbarItem.cs#L35) | Gets the resolved contextual tooltip. |

### `Inno.Editor.Interactions.EditorToolbarItemAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorToolbarIcon Inno.Editor.Interactions.EditorToolbarItemAttribute.activeIcon`](../../src/composition/editor/framework/Inno.Editor.Interactions/Toolbars/EditorToolbarItemAttribute.cs#L75) | Gets the optional replacement symbol shown while the action is checked. |
| [`Inno.Editor.Interactions.EditorToolbarIcon Inno.Editor.Interactions.EditorToolbarItemAttribute.icon`](../../src/composition/editor/framework/Inno.Editor.Interactions/Toolbars/EditorToolbarItemAttribute.cs#L70) | Gets the symbol shown while the action is not checked. |
| [`Inno.Editor.Interactions.EditorToolbarItemAttribute`](../../src/composition/editor/framework/Inno.Editor.Interactions/Toolbars/EditorToolbarItemAttribute.cs#L8) | Places an editor action on a compact toolbar surface. |
| [`Inno.Editor.Interactions.EditorToolbarItemAttribute.EditorToolbarItemAttribute(string area, Inno.Editor.Interactions.EditorToolbarIcon icon, string tooltip, int order = 0, Inno.Editor.Interactions.EditorToolbarIcon activeIcon = Inno.Editor.Interactions.EditorToolbarIcon.None)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Toolbars/EditorToolbarItemAttribute.cs#L37) | Creates a toolbar placement for the annotated targetless action. |
| [`int Inno.Editor.Interactions.EditorToolbarItemAttribute.order`](../../src/composition/editor/framework/Inno.Editor.Interactions/Toolbars/EditorToolbarItemAttribute.cs#L85) | Gets the stable ordering value. |
| [`string Inno.Editor.Interactions.EditorToolbarItemAttribute.area`](../../src/composition/editor/framework/Inno.Editor.Interactions/Toolbars/EditorToolbarItemAttribute.cs#L65) | Gets the exact toolbar interaction area. |
| [`string Inno.Editor.Interactions.EditorToolbarItemAttribute.tooltip`](../../src/composition/editor/framework/Inno.Editor.Interactions/Toolbars/EditorToolbarItemAttribute.cs#L80) | Gets the fallback tooltip. |

### `Inno.Editor.Interactions.EditorToolbarModel`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorToolbarModel`](../../src/composition/editor/framework/Inno.Editor.Interactions/Toolbars/EditorToolbarModel.cs#L8) | Contains an immutable resolved toolbar for one interaction area. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Editor.Interactions.EditorToolbarItem> Inno.Editor.Interactions.EditorToolbarModel.items`](../../src/composition/editor/framework/Inno.Editor.Interactions/Toolbars/EditorToolbarModel.cs#L18) | Gets visible toolbar commands in deterministic display order. |

### `Inno.Editor.Interactions.EditorValidationResult`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorValidationResult`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorValidationResult.cs#L12) | Describes whether a requested editor operation is valid and carries a diagnostic when it is rejected. |
| [`static Inno.Editor.Interactions.EditorValidationResult Inno.Editor.Interactions.EditorValidationResult.Invalid(string message)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorValidationResult.cs#L30) | Creates a failed validation result with a user-facing diagnostic. |
| [`static Inno.Editor.Interactions.EditorValidationResult Inno.Editor.Interactions.EditorValidationResult.valid`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/EditorValidationResult.cs#L19) | Gets a successful validation result. |

### `Inno.Editor.Interactions.EditorViewportCursor`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorViewportCursor`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportTool.cs#L33) | Selects a presentation-independent cursor requested by a viewport tool. |
| [`Inno.Editor.Interactions.EditorViewportCursor.Arrow`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportTool.cs#L38) | Uses the normal arrow cursor. |
| [`Inno.Editor.Interactions.EditorViewportCursor.Crosshair`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportTool.cs#L42) | Uses a precise crosshair. |
| [`Inno.Editor.Interactions.EditorViewportCursor.Hand`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportTool.cs#L46) | Uses a hand suitable for panning. |
| [`Inno.Editor.Interactions.EditorViewportCursor.Hidden`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportTool.cs#L62) | Hides the pointer while the tool owns it. |
| [`Inno.Editor.Interactions.EditorViewportCursor.Move`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportTool.cs#L50) | Uses four-direction movement arrows. |
| [`Inno.Editor.Interactions.EditorViewportCursor.ResizeHorizontal`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportTool.cs#L54) | Uses horizontal resizing arrows. |
| [`Inno.Editor.Interactions.EditorViewportCursor.ResizeVertical`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportTool.cs#L58) | Uses vertical resizing arrows. |

### `Inno.Editor.Interactions.EditorViewportPointerEvent`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Input.KeyModifier Inno.Editor.Interactions.EditorViewportPointerEvent.modifiers`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportTool.cs#L178) | Gets active keyboard modifiers. |
| [`Inno.Core.Mathematics.Vector2 Inno.Editor.Interactions.EditorViewportPointerEvent.screenPosition`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportTool.cs#L163) | Gets the viewport-local pixel position. |
| [`Inno.Core.Mathematics.Vector2 Inno.Editor.Interactions.EditorViewportPointerEvent.worldPosition`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportTool.cs#L168) | Gets the corresponding world position. |
| [`Inno.Editor.Interactions.EditorViewportPointerEvent`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportTool.cs#L111) | Contains one immutable viewport pointer sample in screen and world coordinates. |
| [`Inno.Editor.Interactions.EditorViewportPointerEvent.EditorViewportPointerEvent(int pointerId, Inno.Editor.Interactions.EditorViewportPointerPhase phase, Inno.Core.Mathematics.Vector2 screenPosition, Inno.Core.Mathematics.Vector2 worldPosition, int button, Inno.Core.Input.KeyModifier modifiers)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportTool.cs#L134) | Creates one viewport pointer sample. |
| [`Inno.Editor.Interactions.EditorViewportPointerPhase Inno.Editor.Interactions.EditorViewportPointerEvent.phase`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportTool.cs#L158) | Gets the gesture lifecycle phase. |
| [`int Inno.Editor.Interactions.EditorViewportPointerEvent.button`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportTool.cs#L173) | Gets the platform button number. |
| [`int Inno.Editor.Interactions.EditorViewportPointerEvent.pointerId`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportTool.cs#L153) | Gets the platform pointer identity. |

### `Inno.Editor.Interactions.EditorViewportPointerPhase`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorViewportPointerPhase`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportTool.cs#L10) | Selects the lifecycle phase of one viewport pointer sample. |
| [`Inno.Editor.Interactions.EditorViewportPointerPhase.Cancel`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportTool.cs#L27) | The platform cancelled the gesture. |
| [`Inno.Editor.Interactions.EditorViewportPointerPhase.Down`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportTool.cs#L15) | The pointer button began a gesture. |
| [`Inno.Editor.Interactions.EditorViewportPointerPhase.Move`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportTool.cs#L19) | The pointer moved or changed hover position. |
| [`Inno.Editor.Interactions.EditorViewportPointerPhase.Up`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportTool.cs#L23) | The pointer button ended a gesture. |

### `Inno.Editor.Interactions.EditorViewportShortcut`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Input.KeyCode Inno.Editor.Interactions.EditorViewportShortcut.key`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportTool.cs#L95) | Gets the pressed key. |
| [`Inno.Core.Input.KeyModifier Inno.Editor.Interactions.EditorViewportShortcut.modifiers`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportTool.cs#L100) | Gets active keyboard modifiers. |
| [`Inno.Editor.Interactions.EditorViewportShortcut`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportTool.cs#L68) | Contains one presentation-independent shortcut sample for a focused viewport tool. |
| [`Inno.Editor.Interactions.EditorViewportShortcut.EditorViewportShortcut(Inno.Core.Input.KeyCode key, Inno.Core.Input.KeyModifier modifiers = Inno.Core.Input.KeyModifier.None, bool repeat = false)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportTool.cs#L82) | Creates one viewport shortcut sample. |
| [`bool Inno.Editor.Interactions.EditorViewportShortcut.repeat`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportTool.cs#L105) | Gets whether the shortcut is an auto-repeat press. |

### `Inno.Editor.Interactions.EditorViewportTool`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorViewportTool`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportTool.cs#L212) | Defines one presentation-independent, reloadable viewport editing tool. |
| [`abstract string Inno.Editor.Interactions.EditorViewportTool.id`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportTool.cs#L217) | Gets the globally stable tool identity. |
| [`virtual Inno.Editor.Interactions.EditorViewportCursor Inno.Editor.Interactions.EditorViewportTool.cursor`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportTool.cs#L222) | Gets the cursor requested while this tool is active. |
| [`virtual bool Inno.Editor.Interactions.EditorViewportTool.OnShortcut(Inno.Editor.Interactions.EditorViewportToolContext context, Inno.Editor.Interactions.EditorViewportShortcut shortcut)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportTool.cs#L302) | Handles a focused viewport shortcut before global action routing. |
| [`virtual void Inno.Editor.Interactions.EditorViewportTool.DrawOverlay(Inno.Editor.Interactions.EditorViewportToolContext context)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportTool.cs#L316) | Draws transient gizmos and diagnostics over the viewport. |
| [`virtual void Inno.Editor.Interactions.EditorViewportTool.OnPointerCancel(Inno.Editor.Interactions.EditorViewportToolContext context, Inno.Editor.Interactions.EditorViewportPointerEvent pointer)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportTool.cs#L281) | Handles platform cancellation of an active gesture. |
| [`virtual void Inno.Editor.Interactions.EditorViewportTool.OnPointerDown(Inno.Editor.Interactions.EditorViewportToolContext context, Inno.Editor.Interactions.EditorViewportPointerEvent pointer)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportTool.cs#L233) | Handles a pointer press. |
| [`virtual void Inno.Editor.Interactions.EditorViewportTool.OnPointerMove(Inno.Editor.Interactions.EditorViewportToolContext context, Inno.Editor.Interactions.EditorViewportPointerEvent pointer)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportTool.cs#L249) | Handles pointer movement or hover. |
| [`virtual void Inno.Editor.Interactions.EditorViewportTool.OnPointerUp(Inno.Editor.Interactions.EditorViewportToolContext context, Inno.Editor.Interactions.EditorViewportPointerEvent pointer)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportTool.cs#L265) | Handles a pointer release. |

### `Inno.Editor.Interactions.EditorViewportToolContext`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Mathematics.Vector2 Inno.Editor.Interactions.EditorViewportToolContext.ScreenToWorld(Inno.Core.Mathematics.Vector2 screenPosition)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportToolContext.cs#L106) | Converts viewport-local pixel coordinates to world coordinates. |
| [`Inno.Core.Mathematics.Vector2 Inno.Editor.Interactions.EditorViewportToolContext.WorldToScreen(Inno.Core.Mathematics.Vector2 worldPosition)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportToolContext.cs#L117) | Converts world coordinates to viewport-local pixel coordinates. |
| [`Inno.Editor.Interactions.EditorViewportToolContext`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportToolContext.cs#L9) | Coordinates pointer capture, transforms, and one history transaction for a viewport gesture. |
| [`bool Inno.Editor.Interactions.EditorViewportToolContext.hasHistoryGesture`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportToolContext.cs#L37) | Gets whether a history transaction is active for the current gesture. |
| [`bool Inno.Editor.Interactions.EditorViewportToolContext.hasPointerCapture`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportToolContext.cs#L27) | Gets whether this tool owns a pointer. |
| [`int? Inno.Editor.Interactions.EditorViewportToolContext.capturedPointerId`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportToolContext.cs#L32) | Gets the captured pointer identity, or . |
| [`void Inno.Editor.Interactions.EditorViewportToolContext.BeginHistoryGesture(string name)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportToolContext.cs#L68) | Begins the only history transaction allowed for the current captured gesture. |
| [`void Inno.Editor.Interactions.EditorViewportToolContext.CapturePointer(int pointerId)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportToolContext.cs#L45) | Captures one pointer until explicit release or cancellation. |
| [`void Inno.Editor.Interactions.EditorViewportToolContext.CompleteHistoryGesture(bool commit)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportToolContext.cs#L84) | Commits or rolls back the current gesture transaction exactly once. |
| [`void Inno.Editor.Interactions.EditorViewportToolContext.ReleasePointer()`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportToolContext.cs#L55) | Releases the currently captured pointer. |

### `Inno.Editor.Interactions.EditorViewportToolSession`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorViewportCursor Inno.Editor.Interactions.EditorViewportToolSession.cursor`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportToolSession.cs#L38) | Gets the requested cursor for the active tool. |
| [`Inno.Editor.Interactions.EditorViewportTool? Inno.Editor.Interactions.EditorViewportToolSession.tool`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportToolSession.cs#L33) | Gets the active tool, or . |
| [`Inno.Editor.Interactions.EditorViewportToolSession`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportToolSession.cs#L8) | Routes focused viewport input to one active tool while enforcing pointer-capture ownership. |
| [`Inno.Editor.Interactions.EditorViewportToolSession.EditorViewportToolSession(Inno.Editor.Interactions.IEditorHistory history, Inno.Editor.Interactions.IEditorViewportCoordinateConverter coordinates)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportToolSession.cs#L23) | Creates a viewport tool session. |
| [`bool Inno.Editor.Interactions.EditorViewportToolSession.HandlePointer(Inno.Editor.Interactions.EditorViewportPointerEvent pointer)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportToolSession.cs#L64) | Routes one immutable pointer sample. |
| [`bool Inno.Editor.Interactions.EditorViewportToolSession.HandleShortcut(Inno.Editor.Interactions.EditorViewportShortcut shortcut)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportToolSession.cs#L97) | Routes one focused keyboard shortcut. |
| [`void Inno.Editor.Interactions.EditorViewportToolSession.Dispose()`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportToolSession.cs#L115) | Cancels active gesture state and deactivates the tool. |
| [`void Inno.Editor.Interactions.EditorViewportToolSession.DrawOverlay()`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportToolSession.cs#L106) | Draws the active tool overlay. |
| [`void Inno.Editor.Interactions.EditorViewportToolSession.SetTool(Inno.Editor.Interactions.EditorViewportTool? tool)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportToolSession.cs#L46) | Activates a tool after cancelling any current gesture. |

### `Inno.Editor.Interactions.HotKeyGesture`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Input.KeyCode Inno.Editor.Interactions.HotKeyGesture.key`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/HotKeyGesture.cs#L45) | Gets the main key. |
| [`Inno.Core.Input.KeyModifier Inno.Editor.Interactions.HotKeyGesture.modifiers`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/HotKeyGesture.cs#L50) | Gets the required modifiers. |
| [`Inno.Editor.Interactions.HotKeyGesture`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/HotKeyGesture.cs#L12) | Describes one keyboard gesture used to invoke an editor command. |
| [`Inno.Editor.Interactions.HotKeyGesture.HotKeyGesture(Inno.Core.Input.KeyCode key, Inno.Core.Input.KeyModifier modifiers = Inno.Core.Input.KeyModifier.None, string superModifierLabel = "Super")`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/HotKeyGesture.cs#L31) | Creates a keyboard gesture with an exact set of modifier keys after symbolic-key normalization. |
| [`override string Inno.Editor.Interactions.HotKeyGesture.ToString()`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/HotKeyGesture.cs#L82) | Formats the gesture as a human-readable editor menu shortcut label. |
| [`static Inno.Editor.Interactions.HotKeyGesture Inno.Editor.Interactions.HotKeyGesture.Primary(Inno.Core.Input.KeyCode key, Inno.Editor.Core.EditorKeyboardPolicy keyboard, Inno.Core.Input.KeyModifier additionalModifiers = Inno.Core.Input.KeyModifier.None)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Actions/HotKeyGesture.cs#L67) | Creates a gesture that includes the product-selected primary modifier. |

### `Inno.Editor.Interactions.IEditorDocumentService`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorDocumentContext Inno.Editor.Interactions.IEditorDocumentService.Open(string assetPath, System.Guid assetId = default(System.Guid))`](../../src/composition/editor/framework/Inno.Editor.Interactions/Documents/IEditorDocumentService.cs#L58) | Opens a document or returns its existing single instance. |
| [`Inno.Editor.Interactions.IEditorDocumentService`](../../src/composition/editor/framework/Inno.Editor.Interactions/Documents/IEditorDocumentService.cs#L28) | Owns single-instance editor documents independently from reloadable providers and presentation. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Editor.Interactions.EditorDocumentContext> Inno.Editor.Interactions.IEditorDocumentService.documents`](../../src/composition/editor/framework/Inno.Editor.Interactions/Documents/IEditorDocumentService.cs#L33) | Gets the currently owned source documents. |
| [`System.IDisposable Inno.Editor.Interactions.IEditorDocumentService.RegisterProvider(Inno.Editor.Interactions.EditorDocumentProvider provider)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Documents/IEditorDocumentService.cs#L44) | Registers one current-generation document provider. |
| [`bool Inno.Editor.Interactions.IEditorDocumentService.Apply(System.Guid documentId)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Documents/IEditorDocumentService.cs#L122) | Applies staged changes through the current provider. |
| [`bool Inno.Editor.Interactions.IEditorDocumentService.Close(System.Guid documentId, Inno.Editor.Interactions.EditorDocumentCloseMode mode)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Documents/IEditorDocumentService.cs#L147) | Closes one document using an explicit dirty-document decision. |
| [`bool Inno.Editor.Interactions.IEditorDocumentService.Revert(System.Guid documentId)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Documents/IEditorDocumentService.cs#L133) | Reverts staged changes through the current provider. |
| [`bool Inno.Editor.Interactions.IEditorDocumentService.Save(System.Guid documentId)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Documents/IEditorDocumentService.cs#L103) | Saves one open document through its current provider. |
| [`bool Inno.Editor.Interactions.IEditorDocumentService.SaveAll()`](../../src/composition/editor/framework/Inno.Editor.Interactions/Documents/IEditorDocumentService.cs#L111) | Saves every dirty open document. |
| [`bool Inno.Editor.Interactions.IEditorDocumentService.UpdateAssetPath(System.Guid documentId, string assetPath)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Documents/IEditorDocumentService.cs#L75) | Updates an open document's source location after an identity-preserving asset move, without changing history or focus. |
| [`void Inno.Editor.Interactions.IEditorDocumentService.SetDirty(System.Guid documentId, bool isDirty = true)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Documents/IEditorDocumentService.cs#L89) | Updates a document's unsaved state without saving, discarding or changing its History. |

### `Inno.Editor.Interactions.IEditorHistory`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.EditorHistoryResult Inno.Editor.Interactions.IEditorHistory.Execute(string name, Inno.Editor.Interactions.EditorHistoryChange change)`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/IEditorHistory.cs#L81) | Applies and records a neutral change through its current-generation handler. |
| [`Inno.Editor.Interactions.EditorHistoryResult Inno.Editor.Interactions.IEditorHistory.Redo()`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/IEditorHistory.cs#L114) | Attempts to reapply the newest reverted operation. |
| [`Inno.Editor.Interactions.EditorHistoryResult Inno.Editor.Interactions.IEditorHistory.Undo()`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/IEditorHistory.cs#L106) | Attempts to restore the state preceding the newest committed operation. |
| [`Inno.Editor.Interactions.EditorHistoryTransaction Inno.Editor.Interactions.IEditorHistory.BeginTransaction(string name)`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/IEditorHistory.cs#L67) | Begins an atomic group of neutral history operations. |
| [`Inno.Editor.Interactions.IEditorHistory`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/IEditorHistory.cs#L6) | Exposes reload-safe Undo and Redo operations based exclusively on neutral history changes. |
| [`bool Inno.Editor.Interactions.IEditorHistory.canRedo`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/IEditorHistory.cs#L16) | Gets whether a Redo transition is currently available. |
| [`bool Inno.Editor.Interactions.IEditorHistory.canUndo`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/IEditorHistory.cs#L11) | Gets whether an Undo transition is currently available. |
| [`bool Inno.Editor.Interactions.IEditorHistory.isFaulted`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/IEditorHistory.cs#L21) | Gets whether a failed compensation left the domain state indeterminate. |
| [`long Inno.Editor.Interactions.IEditorHistory.diskBytes`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/IEditorHistory.cs#L56) | Gets the temporary disk payload bytes retained by committed entries. |
| [`long Inno.Editor.Interactions.IEditorHistory.residentBytes`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/IEditorHistory.cs#L51) | Gets the resident payload bytes retained by committed entries. |
| [`string? Inno.Editor.Interactions.IEditorHistory.faultReason`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/IEditorHistory.cs#L46) | Gets the diagnostic that faulted this history, or . |
| [`string? Inno.Editor.Interactions.IEditorHistory.redoName`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/IEditorHistory.cs#L31) | Gets the next Redo operation name, or . |
| [`string? Inno.Editor.Interactions.IEditorHistory.redoUnavailableReason`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/IEditorHistory.cs#L41) | Gets why the next Redo entry is unavailable, or . |
| [`string? Inno.Editor.Interactions.IEditorHistory.undoName`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/IEditorHistory.cs#L26) | Gets the next Undo operation name, or . |
| [`string? Inno.Editor.Interactions.IEditorHistory.undoUnavailableReason`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/IEditorHistory.cs#L36) | Gets why the next Undo entry is unavailable, or . |
| [`void Inno.Editor.Interactions.IEditorHistory.RecordApplied(string name, Inno.Editor.Interactions.EditorHistoryChange change)`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/IEditorHistory.cs#L95) | Records a neutral change whose domain mutation is already applied. |

### `Inno.Editor.Interactions.IEditorHistoryIsolation`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.IEditorHistoryIsolation`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/IEditorHistoryIsolation.cs#L8) | Creates temporary history branches for transient editor workflows without exposing history internals. |
| [`System.IDisposable Inno.Editor.Interactions.IEditorHistoryIsolation.BeginHistoryIsolation()`](../../src/composition/editor/framework/Inno.Editor.Interactions/History/IEditorHistoryIsolation.cs#L19) | Starts an independently disposable history branch while retaining the current editing branch. |

### `Inno.Editor.Interactions.IEditorSelectionCoordinator`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Interactions.IEditorSelectionCoordinator`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/IEditorSelectionCoordinator.cs#L6) | Provides the narrow selection boundary used by editor features that replace object instances. |
| [`object? Inno.Editor.Interactions.IEditorSelectionCoordinator.selectedTarget`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/IEditorSelectionCoordinator.cs#L11) | Gets the currently selected target, or when selection is empty. |
| [`void Inno.Editor.Interactions.IEditorSelectionCoordinator.SetSelection(object? target)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Runtime/IEditorSelectionCoordinator.cs#L19) | Replaces the current target after closing presentations owned by another target. |

### `Inno.Editor.Interactions.IEditorViewportCoordinateConverter`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Mathematics.Vector2 Inno.Editor.Interactions.IEditorViewportCoordinateConverter.ScreenToWorld(Inno.Core.Mathematics.Vector2 screenPosition)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportTool.cs#L195) | Converts viewport-local pixel coordinates to world coordinates. |
| [`Inno.Core.Mathematics.Vector2 Inno.Editor.Interactions.IEditorViewportCoordinateConverter.WorldToScreen(Inno.Core.Mathematics.Vector2 worldPosition)`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportTool.cs#L206) | Converts world coordinates to viewport-local pixel coordinates. |
| [`Inno.Editor.Interactions.IEditorViewportCoordinateConverter`](../../src/composition/editor/framework/Inno.Editor.Interactions/Viewport/EditorViewportTool.cs#L184) | Converts coordinates for one viewport camera without exposing its presentation backend. |

## 项目依赖

- [Inno.Core.Diagnostics](../core/Inno.Core.Diagnostics.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Editor.Core](Inno.Editor.Core.md)：公开引用边界由实际签名核对。
- [Inno.Core.Identity](../core/Inno.Core.Identity.md)：公开引用边界由实际签名核对。
- [Inno.Core.Events](../core/Inno.Core.Events.md)：公开引用边界由实际签名核对。
- [Inno.Core.Input](../core/Inno.Core.Input.md)：公开引用边界由实际签名核对。
- [Inno.Core.Mathematics](../core/Inno.Core.Mathematics.md)：公开引用边界由实际签名核对。
- [Inno.Core.Logging](../core/Inno.Core.Logging.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：公开引用边界由实际签名核对。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
