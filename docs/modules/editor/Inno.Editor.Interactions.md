# Inno.Editor.Interactions

## 扩展退休失败

Editor extension snapshot 现在使用 TypeRegistry 的统一 `DisposeExtensions`，去重、逆序并尝试全部实例后聚合失败。
Dispose 异常不再只写 LogError：调用者收到异常，共享 generation gate 进入 Faulted，禁止继续 reload。
当前 shutdown 的 Panel → Module → History/Interactions → extension Dispose 顺序不变；相关测试同时验证顺序、次数与 Fault。
当前验证范围与尚未执行的产品回归见[当前验收](../../../.agents/records/acceptance/current.md)。

[Editor 索引](README.md) · [Core](Inno.Editor.Core.md) · [ImGui](Inno.Editor.ImGui.md) · [Identity、Missing 与 reload 标准](../../architecture/identity-and-reload.md)

`Inno.Editor.Interactions` 提供表现后端无关的交互语言：稳定的 `string` area/action/panel ID、可选 `target`、轻量 `EditorInteraction`，以及 Attribute 自动发现的 Action、Menu、Shortcut 和 Drop。它不引用 ImGui、Assets、Scene、Scripting 或任何 Panel project；跨 feature reload 协议位于 [Inno.Editor.Core](Inno.Editor.Core.md)。

## 最小心智模型

```csharp
using Inno.Core.Identity;
using Inno.Editor.Interactions;

static EditorMenuModel SelectAndReadMenu(
    EditorInteractions interactions,
    IdentityObject target
) {
    EditorInteraction row = interactions.For("sample/items", target);
    row.Focus();
    row.Select();
    return row.BuildMenu();
}
```

- Attribute 和运行时 API 都直接使用 feature 自己维护的稳定 `const string`。
- `target` 是当前实际对象，决定 typed Action/Drop 是否匹配。
- Action ID 表示语义操作，例如 `asset.rename`；它与 area 是两个正交维度。
- 旧的强类型 ID/command wrapper 已删除，不存在兼容 wrapper 或转发 overload。
- 不需要 `Surface` marker type，也不需要创建 Context/Menu/Command service。

## 定义 area

每个 feature 在自己的项目根目录保留一个常量类：

```csharp
internal static class ItemInteractionIds
{
    internal const string C_AREA = "sample/items";
    internal const string C_SELECT = "sample/items/select";
}
```

命名建议使用小写、以 `/` 分层。area 不需要注册；第一次传给 `For` 或 Attribute 时即可使用。所有公开入口对空白 ID 抛出 `ArgumentException`，匹配使用 ordinal string comparison。

## Action

立即完成的选择操作（只改变交互状态，不修改项目数据）：

```csharp
using Inno.Core.Identity;
using Inno.Editor.Interactions;

[EditorAction("sample/items/select", "sample/items")]
public sealed class SelectItemAction : EditorAction<IdentityObject>
{
    protected override void Execute(EditorActionContext<IdentityObject> context)
        => context.interactions.For(context.area, context.target).Select();
}
```

解析优先级为精确 area、target 类型距离和 Attribute priority。找不到或禁用时 `Execute` 返回 `false`；需要参数的实现继承 `EditorAction<TTarget,TArgument>` 或 `EditorArgumentAction<TArgument>`，调用方使用 `Execute(actionId, argument)`，dispatch 前严格校验 action argument 类型。扩展仍只看到强类型 context，不暴露原始 `object argument` 或 `TryGetArgument`。

### 多帧 Action

Rename、分步创建和参数预览仍是普通 `EditorAction`。下例使用确认选择说明多帧状态。Action 调用 `Activate`、`Complete`、`Cancel`；视图只在目标位置调用 `Present`：

```csharp
using Inno.Core.Identity;
using Inno.Editor.Interactions;

public readonly record struct SelectionConfirmation(
    bool accept,
    bool cancel
);

[EditorAction("sample/items/confirm-selection", "sample/items")]
public sealed class ConfirmSelectionAction
    : EditorPresentationAction<IdentityObject, SelectionConfirmation>
{
    protected override void Execute(EditorActionContext<IdentityObject> context)
        => Activate(context);

    protected override bool Present(
        EditorActionContext<IdentityObject, SelectionConfirmation> context
    ) {
        if (context.argument.cancel)
            Cancel();
        else if (context.argument.accept)
        {
            context.interactions.For(context.area, context.target).Select();
            Complete();
        }
        return true;
    }
}
```

上述确认选择以 `EditorPresentationAction<IdentityObject,SelectionConfirmation>` 实现。调用方用同一 action ID 的 `Execute(id)` 启动、`Present(id, presentation)` 呈现；presentation 数据在 Action 实现中始终是编译期强类型。Action 执行或呈现抛异常时，运行时会尝试取消其活动状态；取消回调本身失败也会被独立记录，不会破坏后续 Action。

没有单独的 `EditorActionInteraction<TState>` 或全局 Rename service。FileBrowser 和 Hierarchy 各自拥有 Rename Action，因为验证、提交和呈现目标属于各自 feature。

Selection 切换 target 时，Interactions 会通知旧 target 上仍活跃的多帧 Action 失去 presentation。默认实现取消操作；需要提交临时值的 Action 可以覆盖 `OnPresentationLost()`，在其中验证并调用 `Complete()`。如果覆盖返回时 Action 仍处于 active 状态，运行时会自动取消，避免不可见的输入操作永久残留。

## Menu

Action 可声明任意层级菜单路径：

```csharp
using Inno.Core.Identity;
using Inno.Editor.Interactions;

[EditorAction("sample/items/select", "sample/items")]
[EditorMenu("sample/items", "Selection/Select Item", order: 200)]
public sealed class SelectItemMenuAction : EditorAction<IdentityObject>
{
    protected override void Execute(EditorActionContext<IdentityObject> context)
        => context.interactions.For(context.area, context.target).Select();
}
```

同一个 Attribute 同时适用于主菜单和右键菜单；区别只在 area。`EditorMenuRenderer` 会递归创建一级、二级或任意更深的菜单。Create 类命令应使用 `Create/...` 路径形成统一子菜单，不在 label 中手写层级或分隔线。`separatorBefore` 只表达两个相邻可见命令组之间的边界；当前菜单/子菜单的首个可见项永远不会绘制分隔线，因此前置命令被 Query 隐藏时也不会留下孤立横线。

动态列表使用 `EditorMenuSource`。下例与前面的静态菜单是两种替代注册方式，同一个 Action 实现和菜单位置不要同时重复注册：

```csharp
using Inno.Editor.Interactions;

[EditorMenuSource("sample/items")]
public sealed class ItemMenuSource : EditorMenuSource
{
    public override void Build(
        EditorMenuContext context,
        EditorMenuBuilder builder
    ) {
        builder.Add("Selection/Select Item", "sample/items/select");
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
using Inno.Editor.Interactions;

static EditorToolbarModel ReadToolbar(EditorInteractions interactions)
    => interactions.For("editor/main-menu").BuildToolbar();
```

`EditorInteraction.BuildToolbar()` 返回 immutable `EditorToolbarModel`。每个 `EditorToolbarItem` 包含 `actionId`、解析后的语义 `EditorToolbarIcon`、`tooltip`、`order` 与完整 `EditorActionState`。checked 时使用 `activeIcon`；`displayName` 非空时替代 Attribute 的静态 tooltip。Toolbar Action 必须无 target、无 argument，且 placement area 不能越过 Action 自己的精确 area；重复 placement 会在 candidate Build 时拒绝。表现层只把语义 icon 映射到自己的 glyph，不把 ImGui 字符串带入 Interactions。

## Selection 与 Focus

```csharp
using Inno.Core.Identity;
using Inno.Editor.Interactions;

static void UpdateSelection(
    EditorInteractions interactions,
    IdentityObject target,
    bool clicked,
    bool focused
) {
    EditorInteraction row = interactions.For("sample/items", target);
    if (clicked)
        row.Select();
    if (focused)
        row.Focus();
}
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
using Inno.Core.Identity;
using Inno.Editor.Interactions;

[EditorDrop("sample/items")]
public sealed class SelectDroppedItem : EditorDrop<IdentityObject, IdentityObject>
{
    protected override EditorDropStatus Query(
        EditorDropContext<IdentityObject, IdentityObject> context
    ) => EditorDropStatus.Accept(EditorDropVisual.Highlight);

    protected override EditorDropResult Drop(
        EditorDropContext<IdentityObject, IdentityObject> context
    ) => EditorDropResult.Accepted(selectionTarget: context.source);
}
```

此 Drop 示例只选择源对象，不改变内容。真正的移动、插入与复制应调用领域 mutation facade 并记录 History。

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

领域 Module 在修改成功后使用 `RecordApplied`。下例接收已注册 kind 与由领域序列化器编码的中立 payload；payload 必须与对应 Handler 的当前契约一致，不能包含对象、Type 或 delegate。History 会保留独立数据，原 change 的所有权在调用后结束：

```csharp
using System;
using Inno.Editor.Interactions;

static void RecordAppliedChange(
    IEditorHistory history,
    string displayName,
    string registeredKind,
    ReadOnlySpan<byte> neutralPayload
) {
    using var change = new EditorHistoryChange(
        registeredKind, EditorHistoryPayload.FromBytes(neutralPayload));
    history.RecordApplied(displayName, change);
}
```

当前 generation 的 Handler 由 TypeCache 自动发现。领域实现派生 `EditorHistoryHandler`，声明带稳定 kind 的 `[EditorHistoryHandler(kind)]`，并实现 `Query` 和 `Apply`。必须使用所属领域的 resolver 与事务，不能另建全局 live-object 数据库。可参考 [Scene scalar handler 源码](../../../src/composition/editor/features/Inno.Editor.Scene/History/SceneScalarHistoryHandler.cs)（内部实现示例，并非公开类型）。调用者通过 History 查询当前 Handler 的结果：

```csharp
using Inno.Editor.Interactions;

static EditorHistoryResult UndoAvailableChange(IEditorHistory history)
    => history.canUndo
        ? history.Undo()
        : EditorHistoryResult.Failure(
            history.undoUnavailableReason ?? "There is no undoable change.");
```

`Query` 不修改状态，只给菜单和快捷键提供可用性与 barrier 原因。`Apply` 必须在修改前捕获最小 rollback state；普通 `Failure` 表示操作失败但输入状态已严格保留，失败的 Undo/Redo 不移动栈指针，可以重试。只有 History 内部可以生成 `statePreserved=false` 的状态完整性丢失结果；此时 History 进入 faulted，拒绝继续记录、Undo 和 Redo，直到宿主显式 Clear。Handler 缺失、目标删除或 Stable Type ID 不可解析是可诊断 barrier，不会丢弃记录或使用错误对象。

`Execute(name, EditorHistoryChange)` 适合 Handler 自己安全执行初次 Redo 的命令；多数 Editor UI 已先应用修改，因此使用 `RecordApplied`。业务源码不得使用 delegate operation、派生 `EditorHistoryOperation` 或 runtime object merge key。

相邻中立记录只有在 `kind`、非空 `mergeKey` 与 Handler 的 `TryMerge` 都匹配时才会合并。单击开关、创建、删除和排序不设置 merge key；拖动数值、连续文字输入等可合并编辑才设置。

### 事务与资源预算

多个已经独立可逆的修改可以组成一个顶层事务。下例借用两个独立 change，由调用方以 `using` 管理；已执行的 change 被 History 消费，未执行者仍由调用方释放。发生失败时，未 Commit 的事务负责回滚此前成功的子操作：

```csharp
using Inno.Editor.Interactions;

static EditorHistoryResult ApplyPair(
    IEditorHistory history,
    EditorHistoryChange first,
    EditorHistoryChange second
) {
    using EditorHistoryTransaction transaction = history.BeginTransaction("Apply Pair");
    EditorHistoryResult firstResult = history.Execute("First", first);
    if (!firstResult.succeeded)
        return firstResult;
    EditorHistoryResult secondResult = history.Execute("Second", second);
    if (!secondResult.succeeded)
        return secondResult;
    transaction.Commit();
    return EditorHistoryResult.Success();
}
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



## 有序多选与连续编辑

EditorSelectionState 保存有序身份、primary、anchor 和变化计数；范围选择使用当前可见排序，Ctrl 切换单项，Shift 扩展范围。EditorSelectionSnapshot 不持有 live objects，CopyTargets 在原身份 domain 重新解析。Missing 仍保留在 snapshot 的 identities 中，不能被当作空选择跳过。

EditorEditGesture 是共享 History transaction 的所有者。每次 sample 立即 RecordApplied；Commit 完成一个 Undo，Cancel 回滚。拒绝非 LIFO Commit/Cancel 时不丢失事务。文档 Save/Revert/Apply/Close、命令和扩展切换发出 Core Events 的 EditorOperationPreparingEvent，presentation 在新操作开始前完成手势。该通知不是第二条事件总线。

Action 的延迟 target、focus 和选择均使用 Core Identity；执行前重新解析并拒绝 stale generation。纯值参数可以使用不可变值类型，避免无 identity 的临时 command wrapper 被 GC 回收。



## Host 临时 Identity domain

`RegisterIdentityDomain` 借用 Core allocator 并返回 owner-thread 路由 lease。它让同一套 Selection、focus、queue 和 DragDrop 解析独立的 Play 世界；不复制对象表，也不取得 allocator 的销毁权。重复登记、错误线程或退休后登记明确失败。Scene workspace 在恢复 Edit 选择成功后释放路由；失败保留 owner 供重试。`IEditorSelectionCoordinator` 是 Host-only SPI，不再作为项目脚本导出；脚本继续使用 `EditorInteractions`。

## API 参考

[完整声明、行为与调用约束](Inno.Editor.Interactions/api.md)

<!-- api-include: Inno.Editor.Interactions/api.md -->

## 项目依赖

| 工程 | 边界 |
| --- | --- |
| `Inno.Core.Diagnostics` | 实现依赖；不传递 compile surface |
| `Inno.Core.Execution` | 实现依赖；不传递 compile surface |
| `Inno.Scripting.Api` | 实现依赖；不传递 compile surface |
| `Inno.Editor.Core` | 公开签名依赖 |
| `Inno.Core.Identity` | 公开签名依赖 |
| `Inno.Core.Events` | 公开签名依赖 |
| `Inno.Core.Input` | 公开签名依赖 |
| `Inno.Core.Mathematics` | 公开签名依赖 |
| `Inno.Core.Logging` | 公开签名依赖 |
| `Inno.Extensibility.Types` | 公开签名依赖 |
| `Inno.Core.Serialization` | 公开签名依赖 |
| `Inno.Extensibility.Catalogs` | 公开签名依赖 |
