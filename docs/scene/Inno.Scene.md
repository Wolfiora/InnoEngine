# Inno.Scene

完整 Scene 捕获通过 Core Serialization 的显式中立 key 复用未变化的对象条目。Scene 自己拥有结构、层级、对象元数据与 Transform 写入的失效责任；每个条目同时覆盖精确 source/parent ID、sibling 位置与组件属性。本地/世界变换、层级保持变换和属性恢复均更新计数。计数不序列化、不导出为领域 API，耗尽后继续完整捕获。任意插件属性、自定义 Converter、Missing 和 Prefab 等无法完整证明的状态保留现有 writer；不按 UI 通知猜测场景是否变化。结构、根属性、别名和 System 仍执行既有写入流程。详细借用与失败语义见 [Serialization](../core/Inno.Core.Serialization.md)。

[Engine 索引](README.md) · [Scene Assets](Inno.Scene.Assets.md) · [Wiki 首页](../README.md)

内部完整图写入复用 `SceneStore` 已有结构快照中的对象/组件 source-ID 投影。每次捕获仍逐项检查当前 Core persistent ID；不一致时创建新的只读投影，已经借用旧投影的同步写入不受替换影响。结构变化由原 owner 使快照失效。System 归独立生命周期 owner，其身份只在本次写入冻结，不进入结构快照缓存。该投影只表达序列化成员关系，不提供 live-object 解析；Subtree/Prefab 保持各自精确边界，外部引用仍通过 Core Identity 与完整序列化上下文处理。以上均为内部实现，不增加公开配置或持久化字段。

`Inno.Scene` 提供 Scene、GameObject、Component、Transform hierarchy、GameBehavior 和 GameSystem 的运行时模型。`GameBehavior` 是唯一具有独立启停和帧生命周期的 Component 基类，统一负责 `enabled`、`isActiveAndEnabled`、`Awake`、`Start`、`OnEnable`、`OnDisable`、`Update`、`FixedUpdate`、`LateUpdate` 与 `OnDestroy`。Project Script、Renderer、Camera、Light 等场景功能都直接继承 `GameBehavior`，不存在第二层 `Behavior` 类型或兼容 façade。

`Transform` 除了便捷的 local/world TRS 外，还公开精确的 `localToWorldMatrix`、`worldToLocalMatrix`、`TransformPoint`、`InverseTransformPoint` 与原子 `SetWorldTransform`。`worldPosition` 与点变换统一由完整层级矩阵确定。旋转且非均匀缩放的多级层级可能包含不能由单一 world TRS 无损表示的 shear；渲染、包围盒和空间查询应优先使用矩阵/点变换 API，Inspector 与 Gizmo 的 TRS 编辑则使用原子 setter 避免三次中间重算。零缩放层级不可逆，世界到本地转换和保持世界值的重设父级会明确失败，不会静默使用 identity inverse。

## 多 Scene

内部存活校验读取对象对既有 `SceneObjectRecord` 的反向借用；记录仍由同一 SceneStore 和 Core IndexedObjectStore 持有。注册成功后才建立借用，移除和 Clear 解除精确 owner，跨 Scene 转移及失败回滚重新连接对应记录。它不构成另一份对象目录，不跨 generation 保存 live object；外部身份解析继续经过 Core Identity。此机制不属于公开 API，也不改变序列化数据。

`GameScene.hierarchyRevision` 是当前 live Scene 的只读变更计数。对象注册、移除、重设父级和 sibling 排序使其失效；它只用于刷新派生视图，不是持久 ID 或文件格式。内部层级 owner 同时维护有序 roots 和派生位置索引，Editor 不保存第二份 live 层级。名字、属性及 active 状态不由这个计数描述，呈现仍在提交行时解析当前对象。

Scene reload 的旧 element 普通退休失败会聚合抛出，且仍尝试其余旧 element；统一 generation 的不可逆 Complete 阶段据此 Fault。RetirementPendingException 则保持 owner、立即阻止后续清理并交给共享 gate Fault。不能只发布 Warning 后继续报告成功，也不能撤销已经提交的新 generation 假装回滚。

以下脚本例在有效 Runtime Session 与 SceneWorld 执行作用域内调用；first/second 为同一 world 准备的不同 Scene，player 属于 first。LoadScene 会卸载当前集合，不用于保留尚未保存的编辑场景。

```csharp
using InnoEngine.Scene;

static void LoadPair(
    GameScene first,
    GameScene second,
    GameObject player
) {
    SceneManager.LoadScene(first);
    SceneManager.LoadSceneAdditive(second);
    SceneManager.SetSceneIndex(second, 0);
    SceneManager.SetActiveScene(first);
    SceneManager.MoveGameObjectToScene(player, second);
}
```

`loadedScenes` 返回 Hierarchy 展示顺序的稳定快照。Editor 双击 SceneAsset 使用 additive open；已经打开的同一路径会被激活和选择，而不会创建重复实例。Ctrl/Cmd+S 保存全部打开的 Scene。

Hierarchy 的 Scene context menu 和 Delete hotkey 会关闭该内存 Scene，但不会删除对应的 SceneAsset。最后一个已加载 Scene 也可以关闭；此时 `SceneManager.activeScene` 为 `null`，Hierarchy 保持为空，直到用户显式创建或打开 Scene。

## 运行时实例化 Prefab

Prefab 单属性差异复用 Host 的 `SerializationRegistry.GetMetadata` 及声明类型 Reader/Writer。
静态 Player 与动态 Editor 使用同一编解码流程；Scene 不再自行扫描属性或用 `MakeGenericMethod` 构造属性转换入口。

脚本可在已加载的 Scene 中实例化序列化引用的 prefab，并把实例放入同一 Scene 的父物体下：

```csharp
using InnoEngine.Scene;
using InnoEngine.Serialization;

public sealed class ObstacleSpawner : GameBehavior
{
    [SerializableProperty]
    public PrefabAsset? obstacle { get; set; }

    protected override void Start()
    {
        if (obstacle is not null)
            gameObject.scene.InstantiatePrefab(obstacle, gameObject.transform);
    }
}
```

`GameScene.InstantiatePrefab` 返回已连接 prefab 的根对象，并使用加载该 Scene 的 world；呈现阶段回调即使处于其他临时作用域，也会在正确的场景与 Identity 作用域中创建实例。Prefab 或目标 Scene 不可用、父物体不属于目标 Scene 时会明确失败。Host 在会话启动时通过 `SceneWorld.ConfigurePrefabInstantiation` 注入当前序列化器与资产解析器；该装配方法不属于脚本 API。底层 `PrefabAsset.Instantiate` 先恢复独立实例，再设置目标父级，避免把父物体带进 prefab 内部用于差异对比的临时 Scene。

`GameBehavior.Update` 等执行阶段允许创建 prefab。新建对象及其组件会在执行阶段结束时一同提交；反序列化可在提交前恢复该新对象的组件顺序，但仍禁止在执行阶段重排已提交对象的组件。
Prefab 差异映射在这个阶段读取当前有效对象（包含尚待提交的实例及组件），不触发要求场景已稳定的完整 Scene Capture。一次实例化只校准其新建子树中的嵌套 prefab；场景序列化仍要求结构修改全部提交。

Scene 顺序决定当前 `SceneManager` 的跨 Scene traversal 顺序，但业务脚本不应把它作为精确的脚本执行顺序契约；显式依赖应放入可排序的 GameSystem 或独立 scheduler。

`MoveGameObjectToScene` 要求 source 与 destination 都已加载。被移动对象会成为目标 Scene 的 root；完整 child subtree、GameObject/Component 实例、persistent ID、世界变换和生命周期状态保持不变。该操作不会通过序列化复制对象，也不会调用 Reset 或 Destroy。

## Name 与 Tag 查询

每个 `GameObject` 默认使用 `GameObject.defaultTag`（`"Untagged"`），并允许通过普通字符串设置项目 Tag：

```csharp
using System.Collections.Generic;
using InnoEngine.Scene;

static IReadOnlyList<GameObject> FindPlayers(GameScene scene)
    => scene.FindObjectsWithTag("Player");

static void AssignPlayerTag(GameObject player) => player.tag = "Player";
```

Name 与 Tag 都按 `StringComparison.Ordinal` 匹配，复数查询保持 Scene 创建顺序。内部 `SceneStore` 除 Name/Tag/Layer 索引外还维护私有 Unique + Ordered 顺序键；删除对象导致 dense storage swap-back 时不会改变其余对象的查询顺序。查询通过标准 `Query().Find(...).OrderBy(...).Get()/First()` 契约执行，不依赖 Scene 专用 Storage API；对象元数据变化时只更新对应 entry。Tag 会随 Scene、Prefab 和 prefab override 一起序列化。

可用 Tag 定义由 `GameTagCatalog` 这个普通 Project Setting 提供；assignment 与 definition 明确分离：

```csharp
using InnoEngine.Scene;
using InnoEngine.Settings;

static bool AssignDefinedPlayerTag(GameObject player)
{
    GameTagCatalog tags = Settings.Get<GameTagCatalog>(GameTagCatalog.settingId);
    if (!tags.IsDefined("Player"))
        return false;
    player.tag = "Player";
    return true;
}
```

`GetTags` 返回确定性隔离快照；`Add`/`Remove` 只修改当前可编辑 setting 实例。删除定义不会改写 Scene 中已有字符串，因此重新定义同名 Tag 可恢复其配置语义。

## GameLayer、GameLayerMask 与 GameLayerCatalog

`Inno.Scene.Layers` 将对象所属层、筛选集合与项目配置明确分开：

| 类型 | 职责 |
| --- | --- |
| 类型 | 职责 |
| --- | --- |
| `GameLayerId` | 当前 `ProjectId` 与 layer local key 组合出的 `projectId.name` 身份。 |
| `GameLayer` | 0–31 的紧凑运行时 slot；Scene/Prefab 只保存这个值。 |
| `GameLayerMask` | 32 位多层集合，用于渲染、物理和查询过滤。 |
| `GameLayerDefinition` | 自动 local key、当前 slot 与显示名称的只读快照。 |
| `GameLayerCatalog` | 最多 32 个自动 local key/name 映射及对称 interaction matrix。 |

用户只编辑 Layer 名称和 slot，不输入 ID。新 slot 自动得到稳定 local key（例如 `layer.01`）；完整 ID 只在需要时由当前 Project ID 解析。Project ID 改名不会触碰 Layer setting、Scene 或 Prefab；Layer 显示名改名也不会改变已经生成的 local key。

```csharp
using InnoEngine.Scene;
using InnoEngine.Settings;

static GameLayerMask AssignPlayerLayer(GameObject gameObject)
{
    GameLayerCatalog layers = Settings.Get<GameLayerCatalog>(GameLayerCatalog.settingId);
    gameObject.layer = layers.GetLayer("Player");
    return layers.GetMask(["Player", "Enemy"]);
}
```

`GameLayer.defaultLayer` 固定为 slot 0、local key `default` 和名称 `Default`。其完整 ID 会随项目身份解析为 `projectId.default`。Plugin contribution 同样只携带 local key/slot/name 和 interaction operation，不携带导出项目的 Project ID；导入后自然落在消费项目命名空间下。

Composer 对相同 local key、slot、name 的声明去重；同一 slot/key 或 interaction pair 的不兼容声明仍要求显式依赖与 override。32 个 slot 是 Layer mask 的真实有限资源，Composer 不会重排 slot 或改写 Scene assignment。

## Component 顺序

```csharp
using InnoEngine.Scene;

static bool MoveThirdComponent(GameObject gameObject)
{
    GameComponent[] components = [.. gameObject.GetComponents()];
    if (components.Length < 3)
        return false;
    gameObject.SetComponentIndex(components[2], 1);
    return gameObject.GetComponentIndex(components[2]) == 1;
}
```

- `Transform` 仍是每个 GameObject 唯一且不可删除的必需组件，但可以和其他 Component 一样调整显示/序列化顺序。
- 顺序由 Scene/Prefab serialization 保存。
- `GetComponents()` 与 Inspector 使用相同顺序。
- Inspector 通过拖动完整 header 调整 Component 顺序；Transform 与其他 Component 使用同一拖拽契约。
- 手动顺序不改变 GameBehavior Update 优先级；需要确定性调度时使用专门 scheduler，而不是依赖 Inspector 位置。

## GameSystem 顺序

GameSystem 有两个明确分离的排序概念：

| 顺序 | API | 含义 |
| --- | --- | --- |
| 显示/序列化顺序 | `GetSystems`、`GetSystemIndex`、`SetSystemIndex` | Inspector 排列与 Scene round-trip。 |
| 执行优先级 | `GameSystem.order` | 生命周期按数值从小到大执行。 |

```csharp
using InnoEngine.Scene;

public sealed class EarlyGameSystem : GameSystem
{
    public override int order => -100;
}
```

Inspector header 提供 Reset 和 Remove，并通过拖拽调整显示与序列化顺序。该顺序不会修改代码声明的 `order`；相同 `order` 时显示顺序作为稳定 tie-breaker。

## GameSystem 定位

`GameSystem` 是附加到 `GameScene` 而不是单个 `GameObject` 的有状态对象。它适合表达“每个 Scene 一份、需要序列化、需要 Inspector 配置、并参与 Scene 生命周期”的协调逻辑，例如物理世界、导航世界、寻路网格实例、场景级音频环境、昼夜控制、波次导演、实体索引或面向某种渲染模型的 Scene extraction cache。默认每个具体类型在一个 Scene 中只允许一个实例；只有显式标记 `AllowMultipleSystem` 的类型才能重复。

`GameSystem` 不等于所有引擎 service 的通用基类。图形设备、RenderGraph、Player loop、AssetDatabase、编译器和 Editor service 的 owner 都高于或独立于单个 Scene，把它们继承 `GameSystem` 会让后端生命周期被 Scene 加载状态绑死，并制造 Rendering → Scene 的反向依赖。当前 Rendering Runtime 因此使用 host-owned frame boundary 和 attribute 驱动的 `RenderRequestProvider`；`SceneContentSource` 把 `SceneWorld` 投影成共享的 `ContentReadScope`。具体渲染 Plugin 可以在确实需要持久化 Scene 级配置或增量 extraction 状态时提供自己的 `GameSystem`，但普通 Camera、Renderer 与 Light 仍应是直接附着到对象的 `GameBehavior`。

生命周期调度缓存 loaded Scene、GameBehavior 与 GameSystem 的稳定数组，只在 Scene 结构、System 显示顺序或类型 generation 变化时重建。两种类型的 `enabled` 变化都会在 loaded Scene 中立即协调 `OnEnable`/`OnDisable`，不等待下一帧。`GameSystem.order` 仍会在每个阶段开始前读取；值变化时复用现有数组原地排序。`GameSystem.Query<T...>()` 内部使用最多三个规范排序的当前 generation runtime type ID 组成值类型 key，缓存命中时也不再创建 `Type[]`、规范化数组或字符串 key。因此正常 FixedUpdate、Update 与 LateUpdate 不再为这些 traversal 分配新数组。

`SceneTypeCatalog` 在 candidate generation 构建时一次性判断每个 `GameBehavior` 是否实际 override
Awake/Start/Enable/Disable/Update/Fixed/Late/Destroy，并把结果压缩为 lifecycle phase mask。
Runner 按 mask 维护 activation、一次性 Start、Update、FixedUpdate 与 LateUpdate 的独立索引。
Awake/Enable/Disable 通过结构或 enabled/hierarchy 变化事件进入一次性同步队列，Start 成功后立即退出
启动队列；没有覆盖帧 callback 的 Renderer 不会进入对应逐帧数组。结构 replacement/removal 会立即
清空旧索引引用，普通结构 revision 或类型 generation 变化才重建索引。因此仍然只有唯一
`GameBehavior` 基类，不需要用第二个 Renderer/Behavior 层级换取性能或牺牲 ALC 卸载。

Scene 级增量 extraction cache 直接继承 `GameSystem`。其 protected `GetObjects()` 返回由 Scene
持有的不可变结构快照；对象或 Component attachment 变化会使快照 identity 和 structure revision
一起失效，普通 Transform/材质/颜色数值变化不会触发全量重新索引。具体 Rendering Plugin 可据此
缓存 Camera/Drawable/Light 引用，在每帧读取当前值；设备、RenderGraph 和 Runtime 仍不属于
`GameSystem`，不会产生 Rendering Core → Scene 的反向依赖。

## 内部索引与类型身份

以下是当前内部实现细节，不属于额外公开 API：

- GameObject、GameComponent 与 GameSystem 都以 `IndexedObjectStore<T>` 保存；引用身份、persistent Guid、元数据、owner、commit 状态和 runtime type ID 分别使用 typed `IndexedObjectKey<TKey>`。
- GameObject 与 GameComponent 的 persistent Guid 使用 Unique key，因此 `FindObject(Guid)` 与 `FindComponent(Guid)` 为平均 O(1) 查找，不再递归或线性扫描 Scene。
- Component/System 的具体类型索引、Entry、查询缓存和组合查询 key 全部保存当前 TypeCache generation 的 `int runtimeId`。可赋值关系由类型目录预计算为 runtime ID 集合，查询期间不调用 `Type.IsAssignableFrom`，也不在 Scene 中保存 CLR `Type`。
- 每个对象的 Component list 仅维护公开契约要求的 attachment order；它不承担对象身份、Guid 或类型索引职责。System 的 display list 同理只维护显示与序列化顺序。

Scene/Prefab 的 History、Missing、序列化和 reload previous/candidate 边界继续使用 `TypeRef`，序列化只写其 Stable ID（Guid），绝不写入 runtime ID。Scene 的当前代内存索引只保存 runtime ID，并在 generation 切换时由 migration 使用候选或 previous `TypeRef.runtimeId` 原地替换或回滚。`Type` 参数只存在于 Add/Query、实例创建和序列化反射等调用边界的短生命周期局部变量中；Component 构造器与 Scene property metadata 不建立静态 `Type` 缓存。

## Missing 脚本元素

`GameBehavior` 与 `GameSystem` 分别使用 Core Scripting 的 `ScriptingAttachableTypeAttribute` 声明自己的脚本 manifest 类别。Editor Scripting 只读取该中立 metadata，不硬编码或引用 Scene 类型；具体实例迁移和 Missing 行为仍完全由 Scene 领域拥有。

| 公开类型 | 说明 |
| --- | --- |
| `MissingGameComponent` | 原 Component 类型暂时不可用时，占据相同 attachment index 和 persistent ID。公开 `TypeRef missingType`、`missingTypeName` 供 Inspector/工具识别。 |
| `MissingGameSystem` | 原 System 类型暂时不可用时，占据相同 display index 和 persistent ID，并保持禁用。公开同样的 missing 类型信息。 |

这两个类型只能由 Scene restore 或脚本 reload 管线创建，不能通过普通 `AddComponent` / `AddSystem` 添加，也不进入 Scripting API facade。占位对象只保存 `TypeRef`、类型名、中立 property bytes、资产依赖和引用别名；`TypeRef` 不保存旧 `Type`、反射 metadata、委托或旧脚本实例，所以本身不会阻止 collectible ALC 卸载。原 Stable ID 再次可解析时，`missingType.IsValid(types)` 返回 true；reload 原位创建真实类型，严格恢复全部原属性和当前图引用。构造、属性失败回滚到原占位，提交后的退休清理失败则 Fault，不伪回滚。

Missing 是运行时占位状态，不是 Scene 数据格式中的额外元素类型或 dirty 修改。序列化仍写原逻辑 Stable Type ID、原类型名和原 property bytes，不写 `MissingGameComponent` / `MissingGameSystem` 的 Stable ID，也不写 missing 标志；普通 Scene 中恒等的引用 token 不产生冗余 alias。因而 clean Scene 在类型消失或恢复时保持 clean，Hierarchy 不显示 `*`。用户可以在 missing 存在时修改并保存其他内容；后续相同 Stable ID 恢复时，保存过的原始状态仍会原位还原。

Prefab 等复制图恢复时会为实例分配新的 persistent ID。若脚本类型尚未可用，Missing 状态中的 Scene 引用别名也会按源对象到实例对象的映射重建；脚本恢复后引用仍指向该实例的对象。

## 热重载同步

Scene 使用一个随 `TypeRegistry<TSnapshot>` 事务刷新的中立类型目录。候选目录构建时可以读取 candidate `TypeCacheSnapshot`，但发布后的目录不保留任何 `Type` 或 `TypeRef`：Component/System descriptor 保存 runtime type ID、可赋值 runtime ID 集合与 multiplicity 标志；Store、Scheduler 与查询缓存直接以 `int` 为 key。

Reload 的同步顺序如下：

1. Capture 阶段把旧实例属性编码为不含 `Type` 的中立 bytes，并记录 previous runtime type ID、Stable Type ID、资产依赖和图引用命名空间。
2. TypeCache 与中立 Scene 类型目录原子激活 candidate，同时清除所有存活 SceneStore 的类型派生数组缓存。
3. Scene domain participant 经 `ReferenceRecoveryTransaction` 创建 replacement 或 host-owned missing 占位；已有占位在 Stable Type ID 恢复时创建真实实例。三种路径以 candidate runtime type ID 更新 typed key，相同对象保留 persistent ID，取得新 runtime ID。
4. 统一解析真实 element 与 Asset dependency slots，再做 commit 前验证。element 使用对象 persistent ID，类型 ID 只用作约束；嵌套 Asset 仍 Missing 不阻止元素恢复，也不能被改成 null。
5. 失败时先恢复 Scene 结构，再回滚 Type/Serializer 和外部 Asset publication，最后恢复旧属性与 lifecycle。成功 commit 后释放旧实例图、snapshot 和候选 resolver；Pending 例外必须继续保留 owner。

这样旧 Scene 索引或引擎内部数组不会因为持有 collectible ALC 的 `Type` 而阻止卸载。调用方如果自行长期保留旧 `TypeCacheSnapshot`、旧 Component/System 实例或先前返回的强引用快照，仍会按 .NET 规则延长旧 ALC 生命周期，调用方应在 reload safe point 释放它们。

`SceneReloadService(world, serialization, assets).Capture(typeReload)` 返回 `ISceneReloadStateTransfer`，包含
`retiredObjects`、`diagnostics`、`recoveryChanges` 和 PrepareForActivation / Apply / Complete / RollbackStructure /
RestorePreviousState。`recoveryChanges` 在 Apply 后提供真实槽位结果，在 rollback 后为空；完成后不持有旧 Type/实例。
它是 Host 边界，不导出游戏脚本。内部 SceneReloadRecovery 只负责组合共享事务与 Scene state-transfer owner，
SceneElementReferenceResolver 只通过 SceneWorld 的 Identity allocator 定位 live element，没有新的全局对象索引。

活跃类型到活跃类型的代码更新继续保留既有逐属性失败诊断政策；Missing 到具体类型则要求严格完整恢复，
忽略或失败的属性不能被静默丢弃。完整 C07 仍需继续接 Assets 全量、Graph、Settings 与 Plugin availability。

## 局部元素状态与 History

| API | 当前语义 |
| --- | --- |
| `ScenePropertySerialization.CaptureProperty / CaptureProperties / RestoreProperties` | 单属性或纯 property-data；不包含完整 Missing 元素元数据 |
| `SceneElementSerialization.CaptureState(target, serialization, assets)` | Component/System 完整中立状态：逻辑 type ID/name、属性、Asset dependencies、Scene alias；接受 Missing 占位 |
| `SceneElementSerialization.RestoreState(target, stateData, serialization, assets)` | 恢复同一逻辑类型的状态；具体实例要求完整属性恢复，Missing 保存原 bytes 和依赖 |
| `SceneElementSerialization.RestoreComponent(owner, type, persistentId, componentIndex, stateData, serialization, assets)` | 不调用 Reset，按原身份/位置恢复；类型缺失时创建 MissingGameComponent |
| `SceneElementSerialization.RestoreSystem(scene, type, persistentId, systemIndex, stateData, serialization, assets)` | 对称的 System 恢复；类型缺失时创建 MissingGameSystem |
| `SceneSubtreeSerialization.Capture / Restore` | 完整最小子树，不用于小属性/元素操作 |

Element 的 stateData 必须来自 CaptureState，而不是普通 CaptureProperties。内部 SceneElementState 通过
ISerializable/SerializableProperty 与同一个 SerializationRegistry 编码；不引入自定义 JSON、版本字段或旧格式读取。
对象 identity 和结构 index 仍由调用 owner 持有；element state 内不保存 runtime ID、Type 或 collectible delegate。

```csharp
using System;
using Inno.Assets;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.Scene;

static GameComponent RestoreRemovedComponent(GameObject owner, TypeRef type, Guid id, int index,
    byte[] state, SerializationRegistry serialization, IAssetReferenceResolver assets)
{
    return SceneElementSerialization.RestoreComponent(owner, type, id, index, state, serialization, assets);
}
```

History 通过 SceneEdits 捕获这些 bytes，不直接使用上面的 Host 工具操作用户数据。
删除/移动 Missing 元素保存其原逻辑类型，不保存 placeholder 类型；Undo 可以先恢复占位，类型返回后原 Redo 仍可用。
缺少 owner/Handler、身份冲突、错误 payload 或补偿失败仍明确阻断且保持栈位置。普通元素恢复失败必须移除所有半创建状态；
不可逆退休失败属于 Fault。已保存 Scene 的自动 Missing/Recovery 不产生 dirty 或伪 History entry。

## 局部属性的引用作用域

`ScenePropertySerialization` 和元素状态恢复使用所属 Scene 的 Core Identity domain。对象、Component、System 引用在转换时解析当前注册实例，并检查所属 Scene、代际及预期类型；已销毁、脱离、跨 Scene 或跨 domain 的引用明确失败。纯值属性不会枚举无关对象，也不会为每次前后快照建立整场景双向引用表。调用方仍提供完整的 Asset resolver，资产引用保持 Catalog 的规范实例。

完整 Scene、Prefab、子树和 reload 候选仍需要自己的冻结图边界与局部 ID 映射。它们和当前属性定位是不同的操作：前者描述正在捕获或构造的完整图，后者通过 Core 解析当前存活对象。内部共用同一同步引用作用域，不建立第二个 live object authority；这些内部类型不是公开扩展 API。

恢复别名仅为本次元素状态的中立 `Guid → Guid` 映射；目标必须能在同一 owner 中解析，身份冲突、缺失目标或损坏的别名数据都会拒绝。嵌套序列化和 getter/converter 异常退出时恢复先前作用域，不把它保存在快照、History 或插件状态中。公开签名及 reference token 的持久形状不变。

## Editor Scene 名称与资产路径

已保存 Scene 的 Inspector 名称可以编辑。名称变化立即使文档进入 dirty 状态；保存时同目录 SceneAsset 被事务式重命名，`.imeta`、persistent ID、canonical instance 和 artifact identity 保持不变。目标文件已存在时保存会给出冲突错误，不覆盖另一个资产。

## 当前源码公开 API 清单

此清单对应当前 SDK 求值与源码符号；内部类型不构成公开 API。

<!-- api: T:Inno.Scene.EngineAssetContent 832126f67b8a836dabd772da890296827e787906d869c6b7946b8558e52e6c4f -->
<!-- api: M:Inno.Scene.EngineAssetContent.#ctor(System.ReadOnlySpan{System.Byte},System.Collections.Generic.IReadOnlyList{Inno.Assets.AssetDependency}) 3dd77b3ab107350f00d39242bbaec7b00bce951009ad28b85c8df1e1385e06a0 -->
<!-- api: M:Inno.Scene.EngineAssetContent.GetPayload 6ec51f2602be9e884720d5647f89fd86a4ab2b418f4bd80c691c382d91cae68c -->
<!-- api: M:Inno.Scene.EngineAssetContent.GetDependencies 0579eeed870ef8070687edf0757c785fbe9a1d11dd34137566704bff5fcecdda -->
<!-- api: T:Inno.Scene.PrefabAsset 079a825a95380e627749f700ae1ae1d69ac1c80f808e6dc62529961dfb96c918 -->
<!-- api: M:Inno.Scene.PrefabAsset.Capture(Inno.Scene.GameObject,Inno.Core.Serialization.SerializationRegistry,Inno.Assets.IAssetReferenceResolver) 07f79aff26fcbf4b98f6bb964fbaec11ae8c6fad2313906e4caad5ff7cb3c421 -->
<!-- api: M:Inno.Scene.PrefabAsset.Instantiate(Inno.Scene.GameScene,Inno.Core.Serialization.SerializationRegistry,Inno.Assets.IAssetReferenceResolver,Inno.Scene.Components.Transform) 6e2dc16d91e39214fe49e2ad40873908aad282d3e7c31db6b618686935a8c858 -->
<!-- api: M:Inno.Scene.PrefabAsset.CreateImported(System.ReadOnlySpan{System.Byte},System.Collections.Generic.IReadOnlyList{Inno.Assets.AssetDependency}) ca54a653d71e8b675361f5e504f2578516a710931bde3d3d97df0d75d668a19b -->
<!-- api: M:Inno.Scene.PrefabAsset.CaptureContent 03e7af32eb575f3f607c5db72efe003dfbe0882f0fa831b5b7c397e470defd41 -->
<!-- api: M:Inno.Scene.PrefabAsset.#ctor 5b6efb532fb87c435dc7740080e2d8dfd9aa49fd5cbd6f8f04802b5653e30336 -->
<!-- api: T:Inno.Scene.SceneAsset 170534b9917f208628f1e6822fcd15e58c510bc78eaf622ca31ab45dab46722f -->
<!-- api: M:Inno.Scene.SceneAsset.Capture(Inno.Scene.GameScene,Inno.Core.Serialization.SerializationRegistry,Inno.Assets.IAssetReferenceResolver) ede958e7d78907acb8742028852ec36439ebad0adc4b43946bab79fa0e430836 -->
<!-- api: M:Inno.Scene.SceneAsset.CaptureFrom(Inno.Scene.GameScene,Inno.Core.Serialization.SerializationRegistry,Inno.Assets.IAssetReferenceResolver) f47b5b324bf0e2d155b75e1cdd2546a75d7c1d9154bf494be96bcc44971ffb47 -->
<!-- api: M:Inno.Scene.SceneAsset.Instantiate(Inno.Core.Serialization.SerializationRegistry,Inno.Assets.IAssetReferenceResolver) b9aa2fba1d0813a35fd51db1f4caf012ede58147b761271807125dc8f8b9f3b8 -->
<!-- api: M:Inno.Scene.SceneAsset.CreateImported(System.ReadOnlySpan{System.Byte},System.Collections.Generic.IReadOnlyList{Inno.Assets.AssetDependency}) e566667cf76ba44939d0d61e5eff8af554d4af7b1fb1f478e5ee4c843011de82 -->
<!-- api: M:Inno.Scene.SceneAsset.CaptureContent f515cb78877d6dcd3d86993174dace9bd35067a5f4f2a8b59f64673a77f60b82 -->
<!-- api: M:Inno.Scene.SceneAsset.OnRuntimePayloadChanged(System.ReadOnlyMemory{System.Byte},System.ReadOnlyMemory{System.Byte}) 2fb50caf3b02084008da12c8d35ae6a82ff9e484f13850a1c5bb2bf6d9f38edf -->
<!-- api: M:Inno.Scene.SceneAsset.#ctor 2175aaff445f1b6bbae27647b289a0493f66e1cbd202b00aebefbc923cfb7bc5 -->
<!-- api: T:Inno.Scene.AllowMultipleComponentAttribute 6d4fd6384d2230e5eb8620eaa05ab5e8fd2e9936f329bdecd4cc4c50c9873909 -->
<!-- api: M:Inno.Scene.AllowMultipleComponentAttribute.#ctor 5b2e2a3c7f8a7c948e660ef11ab69bea9b3403353fa58e78772e940006db56b4 -->
<!-- api: T:Inno.Scene.AllowMultipleSystemAttribute 8505d02f1f6060cfac5bb43ac6b6acdb6354a7828dac18b1b2f7b3e3101f472f -->
<!-- api: M:Inno.Scene.AllowMultipleSystemAttribute.#ctor 4c278d85eaea5c085e8756315bd82bb2257616de667f6cfb57d2aa3d16e35a5e -->
<!-- api: T:Inno.Scene.MissingGameComponent 2d4e1df80a9bdcfa7936df0a96a2e1b5bc249357c16f4c0c9d6909883625cd9c -->
<!-- api: P:Inno.Scene.MissingGameComponent.missingType 320c9ef217b12015c1223f0488b98e79256548719b092cb48a46c8e4deee2bda -->
<!-- api: P:Inno.Scene.MissingGameComponent.missingTypeName 95ea1c062d5aa451739b3282d035e25ec538dcf0527e850e4af22d92c60b8f6a -->
<!-- api: T:Inno.Scene.MissingGameSystem ff79817b94274ddea4262e1510b51f2b27099662d798b83556a49cb852642289 -->
<!-- api: P:Inno.Scene.MissingGameSystem.missingType 3e0220459dc7933e14a9581c3814997d01d8d96b7ec7ba719881211b73e1132c -->
<!-- api: P:Inno.Scene.MissingGameSystem.missingTypeName f8ba5aab6cbfeed3cb0931f82b95cfdcdb08bbed5f99f3162700b42ed73ae245 -->
<!-- api: T:Inno.Scene.Components.Transform be63513345b91ef7024f68d5abe63f53be82471c1b8d3f43d57d14036856317d -->
<!-- api: M:Inno.Scene.Components.Transform.#ctor dae3700f8960a20f27e8837e33c938797ac0671d544de540f16d73e32c137b1f -->
<!-- api: P:Inno.Scene.Components.Transform.localPosition 7ff88717e6eeb06820bdae6d837a0960c1fc5435ca2b829ee9522b5d39c6c3cf -->
<!-- api: P:Inno.Scene.Components.Transform.localRotation 861ef37990d3dfa79a0678239533459639ff36684647a8e087d4e945d3e2875c -->
<!-- api: P:Inno.Scene.Components.Transform.localScale 39f23cc53d57fed2c5a69d57e78b35f9dd8730697f408c9d471f66cf23e5db54 -->
<!-- api: P:Inno.Scene.Components.Transform.worldPosition 4608c358eaf35e305fb66fef24ddd2ae27b2936014b0bd5d99049e70348fcf1d -->
<!-- api: P:Inno.Scene.Components.Transform.worldRotation 7d5131240ade5cc485808ee1493da1b2f31b865d2b4d420f8dda4eacaab9ed43 -->
<!-- api: P:Inno.Scene.Components.Transform.worldScale f8db3464f0664b68126c20434b13e44146bbcafd48f45e47bd22f04069911c64 -->
<!-- api: P:Inno.Scene.Components.Transform.localToWorldMatrix ca5c116c9e690585b61115d38ae494865d4bd50f17cccc1e00ebd95a80491ebd -->
<!-- api: P:Inno.Scene.Components.Transform.worldToLocalMatrix d2e8f2ba55edf3694609c53e24f8e00608293c3a84783ac1ce135f3c7240383e -->
<!-- api: P:Inno.Scene.Components.Transform.parent 3ac9b05e28607681d30f06f9f5685d22394e8724dfc82e9df3b5e5b466ab9a62 -->
<!-- api: P:Inno.Scene.Components.Transform.children 86d46abc503e58802db23d183d56ea3e09978c729f7ba68f31fba5f2a66daaee -->
<!-- api: P:Inno.Scene.Components.Transform.siblingIndex d28a325368ec300df3edb8a656c97244c95e2467b173df585969b4ef1f9f6d97 -->
<!-- api: M:Inno.Scene.Components.Transform.SetParent(Inno.Scene.Components.Transform) 5a9e9d70277786bf121761991fc0cfa171dbe979d46d2ff4f9d6e207087fa52b -->
<!-- api: M:Inno.Scene.Components.Transform.SetSiblingIndex(System.Int32) d6f8949e721308446e3ab1f409a153c6793c778bc35f9cb74815ceca8c94c205 -->
<!-- api: M:Inno.Scene.Components.Transform.SetWorldTransform(Inno.Core.Mathematics.Vector3,Inno.Core.Mathematics.Quaternion,Inno.Core.Mathematics.Vector3) 3a6ecce28dc9bc02f34a0cfefa5877a35d928808de693ba8e099955175add54e -->
<!-- api: M:Inno.Scene.Components.Transform.TransformPoint(Inno.Core.Mathematics.Vector3) 884a44d17429cf1fe81233bd2e832cd67a7ab4be30a00d28f556485981ce5a46 -->
<!-- api: M:Inno.Scene.Components.Transform.InverseTransformPoint(Inno.Core.Mathematics.Vector3) 7d3f50d785042f141cbd40e08b3d6680c24febcf405ce0ce279f2cb6ee03f82f -->
<!-- api: M:Inno.Scene.Components.Transform.Reset cbf8fbad61d7bc3058a9bd43aa7dc11752a9e6ebfb0f0d33848ef6195fd02510 -->
<!-- api: T:Inno.Scene.SceneContentSource f5f700911f42350a7899d3c22569c37c541500a3b1b440e3bc0d803634af3b0f -->
<!-- api: M:Inno.Scene.SceneContentSource.CreateScope(Inno.Scene.SceneWorld) b35af48f185fb4ea0ea7b4a6caa3d5f087dc5d5682a4c81db763305d8e65cc6c -->
<!-- api: M:Inno.Scene.SceneContentSource.CreateScope(System.Collections.Generic.IEnumerable{Inno.Scene.GameScene},Inno.Scene.GameScene) a0f3185a2648d90378091fbc6c8524516e1601925efad1c04b4a2bc0ffcd01b1 -->
<!-- api: T:Inno.Scene.EngineObject 63354ff3c7e07762fe74641fdd5e5548d316dc82728a8a4cc91a2d035785b1bf -->
<!-- api: P:Inno.Scene.EngineObject.isDestroyed 4baf7fc48b32db392b4e8b2ce2d0f8e53d3ad344a98daf7d41e9080eae392e43 -->
<!-- api: M:Inno.Scene.EngineObject.op_Equality(Inno.Scene.EngineObject,Inno.Scene.EngineObject) 6dd485b4cd4a0092008f6a257595c3bbcf6df16853d97595403e0f8d3c0346cc -->
<!-- api: M:Inno.Scene.EngineObject.op_Inequality(Inno.Scene.EngineObject,Inno.Scene.EngineObject) e6afdf84a25d172fe19e4fe3e20870548891b77f2548cfe453183a2a80986650 -->
<!-- api: M:Inno.Scene.EngineObject.Equals(System.Object) 537f30a532b70dd68a86d217876874b96a8b8fef6833ee0c3b23cfe20c227b38 -->
<!-- api: M:Inno.Scene.EngineObject.GetHashCode 9bec232bd8e40096f3ccbbe28f1442f0f9eac3877077254572e2f314d440098e -->
<!-- api: M:Inno.Scene.EngineObject.#ctor 8c16c584b747107bdafaf109e48c3384964c758b7ee21fcc571f8570d407c8a7 -->
<!-- api: T:Inno.Scene.GameBehavior 7ff12bd4f235007beb29b918ff52c3f60bc7aaf8a0351e78bcdc49323f0c9c2c -->
<!-- api: P:Inno.Scene.GameBehavior.enabled 3e999fbaf3531743f27dcc8ec44e708425834f9965de42ea0de8ee979937f16e -->
<!-- api: P:Inno.Scene.GameBehavior.isActiveAndEnabled bca11fd0a09ed2891545af9ee2d099fdb6747063fcb579ee405d032e0e952e6d -->
<!-- api: M:Inno.Scene.GameBehavior.Awake 4193fac2e0868044bea1cdf916a84c927f505c2d37c5222b0aab2faf08b8f6c9 -->
<!-- api: M:Inno.Scene.GameBehavior.Start 229b74c6a1fa6ad010c5387191c1f0eca6b4b6354458e4eaa879cece2d87f77b -->
<!-- api: M:Inno.Scene.GameBehavior.OnEnable f27744e5cfeaedca56104a533fba8cc195a13de6d7896e5ba9bcff1d332535fc -->
<!-- api: M:Inno.Scene.GameBehavior.OnDisable ff18aadb29c6a11586278be77e36a27b7a15cbaae6b6e38d5124c787197c6961 -->
<!-- api: M:Inno.Scene.GameBehavior.Update a208c3b1f6efcee8f1d0e9b0aed371fce7ce60a4c37d864c7759952f86251711 -->
<!-- api: M:Inno.Scene.GameBehavior.FixedUpdate 8152dd1d2d0c7496010ed30856b9362fe0ca735edeb9d1574f2a56275fc87955 -->
<!-- api: M:Inno.Scene.GameBehavior.LateUpdate 0b29e94a92c8962de728ae71c09429e94c919b9c7542ae7263a2c0aafb710995 -->
<!-- api: M:Inno.Scene.GameBehavior.OnDestroy 848aee7d1ceafbc43736a2f5f621cdcae51c13f7b9fc01fe6dcbba9176e24bc5 -->
<!-- api: M:Inno.Scene.GameBehavior.#ctor 5d859ca6315dd08b62dbcbc48d4ae1c9fea5985256c0df333b3c98b6d3d2cdd6 -->
<!-- api: T:Inno.Scene.GameComponent 8d9c6bb05b324161892ab6640ef28015d0e222847cafcec2b770143bb1dfe173 -->
<!-- api: P:Inno.Scene.GameComponent.gameObject c4563c76f70500a579d6440a4493e2da13e190b29fe4853e421e84bf0d2d85ca -->
<!-- api: P:Inno.Scene.GameComponent.transform 1d2dab6f5f1e7a9a4fe9ea7a8b53a8798379458a8c772ac961b85570e3a8c91e -->
<!-- api: M:Inno.Scene.GameComponent.Reset a7453c41547d3bd86ee2b566babd4fa9bc66323f2563fc41fe902f5b71a04fa1 -->
<!-- api: M:Inno.Scene.GameComponent.#ctor d1db44f223dcbd7ac78edebca91a4998167fc194c80657996b5f8fc607152fa2 -->
<!-- api: T:Inno.Scene.GameObject 39d25fbe32ebf322ddcab26ac4f6e05f7ee26ae8f0984dbfc79fdc03ba848e7d -->
<!-- api: F:Inno.Scene.GameObject.defaultTag e117023cda38dc6908af45e73ea35dfbbebed8f93eba145ed1e9c96c451794f3 -->
<!-- api: P:Inno.Scene.GameObject.isRuntimeValid 5c00e15c3318d954c49970b7a98e185090163add752c1299aedd3e033dd5516a -->
<!-- api: P:Inno.Scene.GameObject.scene b40c499a7b82766d5a24fc717932e7bc7fe555f120dc4ea9821a93c665b0d84e -->
<!-- api: P:Inno.Scene.GameObject.name 0b6a064b0a7369ee0b3ec4dbbda02dbd36b3ce8eb176f3d79d696ceb54c1be97 -->
<!-- api: P:Inno.Scene.GameObject.tag 5e3626fb28f6458ab7b8beec48f31cbc6477e65f22c7ecb39bc7729b97af480e -->
<!-- api: P:Inno.Scene.GameObject.layer 5a7bf6781499913f0f909bc0ec4059775b2c6319401fac66da05b07b019abe2a -->
<!-- api: P:Inno.Scene.GameObject.transform 5f95c9e64a42bfe8673ef50913e7381f9a97f35b4e5bd4413a52058aef4593d3 -->
<!-- api: P:Inno.Scene.GameObject.activeSelf 80dca851855e5702649c969e3bb7dd4857c1fb69edc5234f72b20362b8081c94 -->
<!-- api: P:Inno.Scene.GameObject.activeInHierarchy 84e2871aa532c5e3b72da6c0cf260f0052662f5057a4fe5cc744d518bb563bf6 -->
<!-- api: P:Inno.Scene.GameObject.isPartOfPrefabInstance 3d24af51809fb7917755c5c9ef80f4898acc6a05635f9895f34439416679c3ca -->
<!-- api: P:Inno.Scene.GameObject.prefabInstanceRoot 72197d7d7fbe8f1dd9f571e553891ab94abf5ff2c6b386e5d32453adeed7cbb4 -->
<!-- api: P:Inno.Scene.GameObject.prefabInstance a446088d904d78468222b56e7b579ce75c619bb4b8fd6e33b1108dd44cdb1c75 -->
<!-- api: M:Inno.Scene.GameObject.SetActive(System.Boolean) 8646857ffeda61f384c8dc6337c253a1cfacf9a3d3824715aaf3bbb87195e8c0 -->
<!-- api: M:Inno.Scene.GameObject.AddComponent``1 8f7ca960143d05d6a153287c56845a48f19e192456591f36892a63971425fcae -->
<!-- api: M:Inno.Scene.GameObject.AddComponent(System.Type) e2b3ed5b739f3cd50363049436252e565616303dd78939c04c75e0d3cc1091ae -->
<!-- api: M:Inno.Scene.GameObject.GetComponent``1 f0d2fe5f40b0c206ba71312bce385ef9d01f8d4d755b77ffbc19d029cd7e5481 -->
<!-- api: M:Inno.Scene.GameObject.GetComponent(System.Type) 32c72dad75ac736556f6d927dda13c1f9c01a14504c5e853d3e130604507976e -->
<!-- api: M:Inno.Scene.GameObject.TryGetComponent``1(``0@) a3d35f247de36dc8249e5a06ba7ea5ee232580534b8f25bc54690b11b1af23ed -->
<!-- api: M:Inno.Scene.GameObject.HasComponent``1 fabf6dcd16c02e791d0a680409d17743b5b91de287e3f46929e27b2af881f520 -->
<!-- api: M:Inno.Scene.GameObject.GetComponents``1 09eb3337a0eed370d74b0f6858ed245f397e1b122a1ec8c5d4455b0b9af40bdd -->
<!-- api: M:Inno.Scene.GameObject.GetComponents abf48d2a7ec2357a34d040e2f040604b1cb5a38ff63e3d19b49e7b1954aa6d38 -->
<!-- api: M:Inno.Scene.GameObject.GetComponentIndex(Inno.Scene.GameComponent) b9acdf9b043d0226a2d7fbeca07f2e2195ed4bf81aafdc4d245de2181895d988 -->
<!-- api: M:Inno.Scene.GameObject.SetComponentIndex(Inno.Scene.GameComponent,System.Int32) 85d8f563eb28f51e81a126956ae867f774d5cca72ff1131bd122b37d67ef21f3 -->
<!-- api: M:Inno.Scene.GameObject.ResetComponent(Inno.Scene.GameComponent) 25ba4f33a1399c67cf0c8afe982398187639686e46f74f72e5e63f22a8767a43 -->
<!-- api: M:Inno.Scene.GameObject.RemoveComponent(Inno.Scene.GameComponent) 28c055e32410d3df2e7d3ce2df005b1a2333cf8ef7790947ab088d1b63bcd4b8 -->
<!-- api: T:Inno.Scene.GameScene e04d4058f0f97edcc77d12f71c8c13da31a4327034379f3adcfffabc94addcda -->
<!-- api: M:Inno.Scene.GameScene.#ctor(System.String) 19902b5adc0939ad09b9ee0d938dcb51f4d46dc2f9fa59012625200e61676b04 -->
<!-- api: P:Inno.Scene.GameScene.name 69f3787006caf1330f8437af6c146dcbb71e2920985da37ce80d376565cbb11a -->
<!-- api: P:Inno.Scene.GameScene.isLoaded 323e3afc26556b06060abb13649d99193622174f7c6ab6d041ed514ea2d03ca4 -->
<!-- api: P:Inno.Scene.GameScene.hierarchyRevision 1c4d5a41ec986366a76f247db551dbac156c833c9f4b0d4839c8e671f7a002e9 -->
<!-- api: M:Inno.Scene.GameScene.CreateObject(System.String) 136b864bebaabac6ca8dabfbaea937e267e26557e472271370ec61cc1e8ca6e6 -->
<!-- api: M:Inno.Scene.GameScene.InstantiatePrefab(Inno.Scene.PrefabAsset,Inno.Scene.Components.Transform) 8d764d4fabf3d6c152bd2bda1474d0f8eac441ecab0210f5779cb574f36632c8 -->
<!-- api: M:Inno.Scene.GameScene.DestroyObject(Inno.Scene.GameObject) 84cbd5ba11ceb594228f3f491ee6540b41058033de85de1d5dcecc89da1f9683 -->
<!-- api: M:Inno.Scene.GameScene.GetObjects 40f39ffd7ca1a6f33bf0792324f72abd38351c7afde5457ff0ecc0e745e37576 -->
<!-- api: M:Inno.Scene.GameScene.FindObject(System.String) e5d7f6d59a1f4a86212336d91a40cb5e3eea04a85caaa52230c5001e67db482d -->
<!-- api: M:Inno.Scene.GameScene.FindObjectWithTag(System.String) bde9a7642abb5493304223c21265c3938973bfafc0a5ff80ab46e1aaa43311d6 -->
<!-- api: M:Inno.Scene.GameScene.FindObjectsWithTag(System.String) c0fb8bd34cff29a6952ac4d3282abe33aadfd20c45c0652cdcb7d74b8a694c1e -->
<!-- api: M:Inno.Scene.GameScene.FindObjectWithLayer(Inno.Scene.Layers.GameLayer) 5315a380576e4dc2c6e8eb71160bac0de0cfaa4aac0317754d63585bad87aeed -->
<!-- api: M:Inno.Scene.GameScene.FindObjectsWithLayer(Inno.Scene.Layers.GameLayer) a432316527f5e26faccaac35958331fcc1257acf5e419f93701454e36bb5dac3 -->
<!-- api: M:Inno.Scene.GameScene.FindObjectsWithLayers(Inno.Scene.Layers.GameLayerMask) 690fa09d49330b76375530c8c3d63bf030837778755869244f2f1977b676e2b1 -->
<!-- api: M:Inno.Scene.GameScene.AddSystem``1 b82e781098404d3fe8f5dd6fb08416665b5aae4f21daee654f9af20a65828e84 -->
<!-- api: M:Inno.Scene.GameScene.AddSystem(System.Type) e27923d54686760753fe5c2918129ab5aec7dc7d358231460ec07f2c953d3ece -->
<!-- api: M:Inno.Scene.GameScene.AddSystem(Inno.Scene.GameSystem) a21e28c4f1e2c5958e8cb7c081e0ba33f25552188ed48397755e5739359c9d43 -->
<!-- api: M:Inno.Scene.GameScene.RemoveSystem(Inno.Scene.GameSystem) d98c06a58e80e528a520a76e8ceca7ae28a800641c109d0f03b9964f3716e44d -->
<!-- api: M:Inno.Scene.GameScene.ResetSystem(Inno.Scene.GameSystem) 589e53567522fe4eee4d0601ed2b7ff4d44a3755e298e24e4c7b7536be278cc1 -->
<!-- api: M:Inno.Scene.GameScene.GetSystems 75962e96dc0acf90f2571be919abe7f45026319afc3cdafc29977dfb811acc8f -->
<!-- api: M:Inno.Scene.GameScene.GetSystemIndex(Inno.Scene.GameSystem) 81e4615c63a9f3ff240eae146ef22ed9d3476fd090ef3e2464caaaaf07a2110c -->
<!-- api: M:Inno.Scene.GameScene.SetSystemIndex(Inno.Scene.GameSystem,System.Int32) 9646cd4facc8dce2dc51264cdd469644024618c9a81ab6d7ab9d0867f8129b45 -->
<!-- api: T:Inno.Scene.GameSystem d77cecad47fbd0e232887783ecf37360b304d7580b811865c331d6b706a58141 -->
<!-- api: P:Inno.Scene.GameSystem.enabled 6add502350e7f5ebc7b4f908cef8847faf1d52d76c207cff15c44d9952c38126 -->
<!-- api: P:Inno.Scene.GameSystem.isActiveAndEnabled 08cf9cd9918572ddf7132226cdbc8e99b90ec5da44322b13171236bc99ca78b5 -->
<!-- api: P:Inno.Scene.GameSystem.order bd3fa84e69916cc93bb288e1d7177243936059432b81b213d6cfd1bcc36080bc -->
<!-- api: P:Inno.Scene.GameSystem.scene 13906905f3383acd15aa5ca1abe6f95257cd0f13ecdbe858bf927bf113bed9e5 -->
<!-- api: M:Inno.Scene.GameSystem.Reset 28f59d8fac84aad39e1af9ae84b21bdd1ea12b9bfe5210988be4781e3566951a -->
<!-- api: M:Inno.Scene.GameSystem.Awake 1b64ee775edf7b06eb4e167a92817c5b599524ea44e0832352a2f6133e61a566 -->
<!-- api: M:Inno.Scene.GameSystem.Start 497daa3aa4fee206a7c9fd058741558f58cb13562ee8647395d47664030f1ec2 -->
<!-- api: M:Inno.Scene.GameSystem.OnEnable 4f80abad17aa8cefc52a9bf444054092e4561a0e388b986273e063deb4b05f0f -->
<!-- api: M:Inno.Scene.GameSystem.OnDisable 1133d4ca51ff1c114aea7c4665851b0ecb610fe5be409a7fe64b55e982af5159 -->
<!-- api: M:Inno.Scene.GameSystem.OnDestroy 194e6d5a792e46b21a3299f22300fd9bc1538e5958ea366558b994de275ea4ba -->
<!-- api: M:Inno.Scene.GameSystem.OnFixedUpdate 8aaa04ddfcf448eb8407146f2c4c69297382b5e0401a366581239c385471af16 -->
<!-- api: M:Inno.Scene.GameSystem.OnUpdate d81129ae05d3920b6957992f33b1d856f562855aaf4da45a0da498b3ead16f62 -->
<!-- api: M:Inno.Scene.GameSystem.OnLateUpdate ca6eca1a2cd6358295c557ed4d3a8e2a6323c6c747cfcd4127d5407b9746623e -->
<!-- api: M:Inno.Scene.GameSystem.GetComponents``1 813aa2cb7afb42bd51977e52081f41abd7c62e0bfabcc9c17d932869580200f3 -->
<!-- api: M:Inno.Scene.GameSystem.GetObjects a70092b5bf9c27b75f31d9b1d91e55efaa4e002623558754a8b1915b39ac3e20 -->
<!-- api: M:Inno.Scene.GameSystem.Query``1 9c9af7d2cb6eb655a28bc70b84e1932b4622ab07bb948569b9f3eff5ced90b73 -->
<!-- api: M:Inno.Scene.GameSystem.Query``2 790cd1bc100dedb407f9e3036518e2358cf0e168d3784a9460c4ab6cf6b64ced -->
<!-- api: M:Inno.Scene.GameSystem.Query``3 366e3e3a1aaa7c5ce6ebb249f7e6879ecbfd9d0e2cb7c7cdaffd3638a0f4886d -->
<!-- api: M:Inno.Scene.GameSystem.#ctor 8d1d9dc5678d32a5e593e6fa3bbfd7a79e19697c536c910d103e5e8f3308cb66 -->
<!-- api: T:Inno.Scene.GameTagCatalog ad6bbfdec27542420d446c3e199be8b84f6c094560d4b23ef99221bcd932ad8c -->
<!-- api: F:Inno.Scene.GameTagCatalog.settingProtocolId 6000f0c33b85998f12924e91bfa04144c1017be7a428c5fadb309725960b458f -->
<!-- api: P:Inno.Scene.GameTagCatalog.settingId 79beab2d6297badc950fad66c9e46d2785df6da91621114b5e7c03b6b5d11d4c -->
<!-- api: M:Inno.Scene.GameTagCatalog.GetTags 5b8782252fde0462aec979877d477fe19612fc80dcd9f43ed6af685765d43bf4 -->
<!-- api: M:Inno.Scene.GameTagCatalog.GetLocalId(System.String) 5ef0198c7cac0bda0fb84d61536ad63df0041232366397cca724aa00b73da701 -->
<!-- api: M:Inno.Scene.GameTagCatalog.GetId(Inno.Core.Settings.ProjectId,System.String) a9ae12d0ea10a435ba21fe2636f2b31c6ad2489ff1ba63fae30c648e687b0988 -->
<!-- api: M:Inno.Scene.GameTagCatalog.IsDefined(System.String) 55e95095035424f791077a3b17ed37ed3d858bb030a7b5c0ecf7dc8aaec5e8ba -->
<!-- api: M:Inno.Scene.GameTagCatalog.Add(System.String) bad026d8f90564c24232063b01c14777ca442f060cb5f100c2f5abd81167cf88 -->
<!-- api: M:Inno.Scene.GameTagCatalog.Remove(System.String) d2fe3e79fcf3c5d3ddaa8c6eae24787dd84bf683fd4564281f725cec54e4c32d -->
<!-- api: M:Inno.Scene.GameTagCatalog.Clone b92dfbff9b7a7367c5d14a721e95672a99353484dff248cdfead7f583fc6a1b7 -->
<!-- api: M:Inno.Scene.GameTagCatalog.#ctor ab87ebfe2e4542dad3c843cfe8820a1c0bdda6a60aba071327bd34283629f440 -->
<!-- api: T:Inno.Scene.Layers.GameLayer 12e77af3925646e07cbe5ce3d8c462f9952123566efef8a6d67b5ee1e3727d68 -->
<!-- api: F:Inno.Scene.Layers.GameLayer.C_MAX_COUNT 792ce39c337e7d601e736f238699fd5ab5f9793e255c8a5c14dfc7ce6b3c6554 -->
<!-- api: P:Inno.Scene.Layers.GameLayer.defaultLayer c5f78340b30f597fa7c934b74d53efa78c81470d0a61d6b4b13e42ecb6ce3048 -->
<!-- api: M:Inno.Scene.Layers.GameLayer.#ctor(System.Int32) 5cb6356d325f7854c143fd3b877043f80349102c3b6d4ec92140af5d9cd32dad -->
<!-- api: P:Inno.Scene.Layers.GameLayer.index b04bd7773574ecf2ff1902f5cc7ead0e07af07f03656bf5a6081d38748028ada -->
<!-- api: M:Inno.Scene.Layers.GameLayer.CompareTo(Inno.Scene.Layers.GameLayer) d2fdc37157fae80f3c07d0b9f7f8d04f7e68f0e5fe2df2f549e1e92f27daa923 -->
<!-- api: M:Inno.Scene.Layers.GameLayer.Equals(Inno.Scene.Layers.GameLayer) 79e93306e4f0f48c0f353a02d4a67279da0b4b42c0e0bda00f07a417f3ca820a -->
<!-- api: M:Inno.Scene.Layers.GameLayer.Equals(System.Object) 33c66f3face869c5b9161b768b81f222087faafe05371be22f3df9603231c979 -->
<!-- api: M:Inno.Scene.Layers.GameLayer.GetHashCode a284fb890c95518d1ce4d806772e54d0611bb18735a781bb6f15a202ac907bf7 -->
<!-- api: M:Inno.Scene.Layers.GameLayer.ToString 13a2b65c5312fdcb1527f99f7f12e350c87b0365b6fcd3da79639694ac93f41d -->
<!-- api: M:Inno.Scene.Layers.GameLayer.op_Equality(Inno.Scene.Layers.GameLayer,Inno.Scene.Layers.GameLayer) f4ee36d3100a9af69841a47d37bd1dfca43e983e707891a2958070c9aacc7126 -->
<!-- api: M:Inno.Scene.Layers.GameLayer.op_Inequality(Inno.Scene.Layers.GameLayer,Inno.Scene.Layers.GameLayer) 92f33046f8afc1860fad0be7c5ba91f8484bfb0e5de3eb2e8725064c85955442 -->
<!-- api: T:Inno.Scene.Layers.GameLayerCatalog d9f50878062ca9d756cb102c9eaf84d8191494ef181fba64442ddb0fa1245c96 -->
<!-- api: F:Inno.Scene.Layers.GameLayerCatalog.settingProtocolId 3993d0d3ef0e6bc7ad76ccaa93b20d95f3c570795134f7a39cf90581aba82ee5 -->
<!-- api: P:Inno.Scene.Layers.GameLayerCatalog.settingId 17ad9110be73344e708026fa439bde7dc175b675ab125632a2156822e934a083 -->
<!-- api: M:Inno.Scene.Layers.GameLayerCatalog.#ctor b6d8031382fc549b049201faccb0cb5120f289da49f2d4405ff9d7d9e39b015d -->
<!-- api: P:Inno.Scene.Layers.GameLayerCatalog.count 9cc9ad649f211b3de0d2035a72ab454ae4ad98334bff05ff99fe224e7610fd12 -->
<!-- api: M:Inno.Scene.Layers.GameLayerCatalog.GetDefinitions 80c8c7a371af20682e2defd9ec9d6cd5a541ff105056b5163385265fa8c71799 -->
<!-- api: M:Inno.Scene.Layers.GameLayerCatalog.IsDefined(Inno.Scene.Layers.GameLayer) af8ed5a09eeb8b868337c19b421743d95bd26705050009652c108a3310fdd5c5 -->
<!-- api: M:Inno.Scene.Layers.GameLayerCatalog.GetName(Inno.Scene.Layers.GameLayer) e21af3b61556368bffc20f1e04029b41763ba5679d0b926660a3b9eb1f5859a1 -->
<!-- api: M:Inno.Scene.Layers.GameLayerCatalog.GetLocalId(Inno.Scene.Layers.GameLayer) 44a37f65e7882570dcfb6fa3f2b7f419df88792543f637ca1d8764311c440297 -->
<!-- api: M:Inno.Scene.Layers.GameLayerCatalog.GetId(Inno.Core.Settings.ProjectId,Inno.Scene.Layers.GameLayer) 2e6311ca31fd3613eb1535c106a23ea0e00077ee63398432a8d582e0d9c729ab -->
<!-- api: M:Inno.Scene.Layers.GameLayerCatalog.TryGetLayer(Inno.Core.Settings.ProjectLocalId,Inno.Scene.Layers.GameLayer@) a367998d49c9931516ea2dd5018c7ff2df18e8b04166665e35ee24d36420de3d -->
<!-- api: M:Inno.Scene.Layers.GameLayerCatalog.TryGetLayer(Inno.Core.Settings.ProjectId,Inno.Scene.Layers.GameLayerId,Inno.Scene.Layers.GameLayer@) 60b7dbdac59178443403d3b19f5a85cf184ede2da4895fc7e50c00a117148756 -->
<!-- api: M:Inno.Scene.Layers.GameLayerCatalog.TryGetLayer(System.String,Inno.Scene.Layers.GameLayer@) 17af7ff0b3bf4c1f8ea34cbfd18b0ee953e7c36495fd56e67a5244cb45c26dbb -->
<!-- api: M:Inno.Scene.Layers.GameLayerCatalog.GetLayer(System.String) 181e36a7ad0f2fd9f37de15eef49b6dc1ab2b8a026cefc29837254e19c6e5ee3 -->
<!-- api: M:Inno.Scene.Layers.GameLayerCatalog.GetMask(System.Collections.Generic.IEnumerable{System.String}) 3882265196720ba70910219448651dd1e6aac56324774fcd161ac0ffb40ad410 -->
<!-- api: M:Inno.Scene.Layers.GameLayerCatalog.Define(Inno.Scene.Layers.GameLayer,System.String) 784515ffd571bf1295168395fa383a3a00c6da154d6cfed759d8479ee145fc95 -->
<!-- api: M:Inno.Scene.Layers.GameLayerCatalog.Remove(Inno.Scene.Layers.GameLayer) 791c93ed0fb37510831311d4975729cb998cbf3d6c3b312f7ffbbf7c8e3db57a -->
<!-- api: M:Inno.Scene.Layers.GameLayerCatalog.GetInteractionMask(Inno.Scene.Layers.GameLayer) c76d0afd6d92abc2fb3bf282c29cd4b5f56bd60719e7c7e48bc477dab573dcf0 -->
<!-- api: M:Inno.Scene.Layers.GameLayerCatalog.CanInteract(Inno.Scene.Layers.GameLayer,Inno.Scene.Layers.GameLayer) 62c916cdfc818206fc591c6ae9fdde9af5db1b00bf636c30d6cf3a68b7eb0330 -->
<!-- api: M:Inno.Scene.Layers.GameLayerCatalog.SetInteraction(Inno.Scene.Layers.GameLayer,Inno.Scene.Layers.GameLayer,System.Boolean) cc29d71121d85d730075a4dfec6f749d3d78208c858b50c05b37fef5d240561b -->
<!-- api: M:Inno.Scene.Layers.GameLayerCatalog.Clone b79b6a5f6450585c957b9e1106270b6055ce72c8ba5c1958ab92ff34ee74890e -->
<!-- api: T:Inno.Scene.Layers.GameLayerDefinition 7155924f7def18e5732e55689150756a4e2141e78e22c825d0369fb4de863218 -->
<!-- api: M:Inno.Scene.Layers.GameLayerDefinition.#ctor(Inno.Core.Settings.ProjectLocalId,Inno.Scene.Layers.GameLayer,System.String) 2b35301366b724595004840ec978a1c1af4ce7ced0a2dfa39c3839e756a8217e -->
<!-- api: P:Inno.Scene.Layers.GameLayerDefinition.localId a58d7bef34e3abe4ecda210b373886acfac411beda3d06acc5dc045e89284fa7 -->
<!-- api: P:Inno.Scene.Layers.GameLayerDefinition.layer 56b701dd0e91f8a92885ae478c386fe135e3dfbfad9631a04be3058144a5b58f -->
<!-- api: P:Inno.Scene.Layers.GameLayerDefinition.name 959b68082120232a24867347321cde29eab9651545c16417cb14e822c8bfa980 -->
<!-- api: M:Inno.Scene.Layers.GameLayerDefinition.GetId(Inno.Core.Settings.ProjectId) 93b0478eba7873756d36a6b581879470fcdb46b7be7610c1ac644779382bd105 -->
<!-- api: T:Inno.Scene.Layers.GameLayerId ca3b85fe423f15f3a0bb8c8cf129164b6f96570a92c260b0c51f37b9069399b6 -->
<!-- api: M:Inno.Scene.Layers.GameLayerId.#ctor(Inno.Core.Settings.ProjectScopedId) f6ceef3aa0bae8c3e79f22e2a57cbb8bae8d1053d3b43424365c897c23aef26e -->
<!-- api: M:Inno.Scene.Layers.GameLayerId.#ctor(Inno.Core.Settings.ProjectId,Inno.Core.Settings.ProjectLocalId) af5ead6246e6815b2f2fc318db230afa8e33c5b1056f0efd6b82a966f3280b0e -->
<!-- api: P:Inno.Scene.Layers.GameLayerId.value 3e5330c3cd7a9505ab3bac0ec6e3854636465407d1a79d3026c892eda214b8c6 -->
<!-- api: P:Inno.Scene.Layers.GameLayerId.isValid 4063ee87d0a7eb52041332caddc541348c1fe74a82d6d348669410adb7cb39fd -->
<!-- api: M:Inno.Scene.Layers.GameLayerId.ToString 746fdafd91e9bbaef59c2c24286367376e72af5944d3a24f80bcecb19508a306 -->
<!-- api: M:Inno.Scene.Layers.GameLayerId.op_Inequality(Inno.Scene.Layers.GameLayerId,Inno.Scene.Layers.GameLayerId) a28a31b2b7753076ecbacee147a8d97ee77fa8b1135ae2c0120a4c64edbc9e5c -->
<!-- api: M:Inno.Scene.Layers.GameLayerId.op_Equality(Inno.Scene.Layers.GameLayerId,Inno.Scene.Layers.GameLayerId) e78e84ad0183d49f091f630b250749be5d064fc4cf0c610f53e5f22fcf567eac -->
<!-- api: M:Inno.Scene.Layers.GameLayerId.GetHashCode 62a48ff5657e926c5b79f35d2fb3b30b9b34dbfa86ff82051bd962cb2587f2f3 -->
<!-- api: M:Inno.Scene.Layers.GameLayerId.Equals(System.Object) afaa32ac386c7e71a543c968611e25024310300d608ae4cdf428637a5af67bf4 -->
<!-- api: M:Inno.Scene.Layers.GameLayerId.Equals(Inno.Scene.Layers.GameLayerId) decbba2deb50475d44565934d59b43825f49c9f529a2b57df199c1416e725e50 -->
<!-- api: T:Inno.Scene.Layers.GameLayerMask fdf906da88f76797b254e9b837a454ce97f1084054f38c7ebbdafa681b4d38b3 -->
<!-- api: P:Inno.Scene.Layers.GameLayerMask.none 29d159cba3c35a2e16f58305d9673149193038c1eb221d40ba4f3f7d104b33d0 -->
<!-- api: P:Inno.Scene.Layers.GameLayerMask.everything 9813f9a4b0d86d360c33271a172d545f6f99b85708f3fe3c267ac2eb190e8fe3 -->
<!-- api: M:Inno.Scene.Layers.GameLayerMask.#ctor(System.UInt32) 4cdf57942ccdcdaf417ba69a38d46ce7c354b5eb2abe03c0b57669c77cf93877 -->
<!-- api: P:Inno.Scene.Layers.GameLayerMask.value 1f779ab3e50d1c42a32529caf5f7a5865a3c0e26f7cebd7e3cbc6d192bb44745 -->
<!-- api: M:Inno.Scene.Layers.GameLayerMask.FromLayers(System.Collections.Generic.IEnumerable{Inno.Scene.Layers.GameLayer}) 8ed1b40c4af0fb681cc406ba86b4ee9931c873926c3b43c026b25221102f5fe0 -->
<!-- api: M:Inno.Scene.Layers.GameLayerMask.Contains(Inno.Scene.Layers.GameLayer) 9e655ab32f2cfd63cbe82680047b13906b21ae4c08eb00f73193e5f25372572e -->
<!-- api: M:Inno.Scene.Layers.GameLayerMask.With(Inno.Scene.Layers.GameLayer) 88aa4b78c6f72313b778daf0b00662405124f63f9fe024da2bed1694bfa552d6 -->
<!-- api: M:Inno.Scene.Layers.GameLayerMask.Without(Inno.Scene.Layers.GameLayer) 4330dce1073777224f8834ce8f19b8d1677633dcd108dd2960a4a1b01e1f7155 -->
<!-- api: M:Inno.Scene.Layers.GameLayerMask.Equals(Inno.Scene.Layers.GameLayerMask) 3d45e85da55c094af5a73321793ceaf30a0195130b853de9600ff17df76349be -->
<!-- api: M:Inno.Scene.Layers.GameLayerMask.Equals(System.Object) 2db9aa397d760a4d27c9ecf0f340f218bb1ab5be33d5cf8aa1a36b0240278a2c -->
<!-- api: M:Inno.Scene.Layers.GameLayerMask.GetHashCode 30dc2fa3301812343a5282f7ee891eff0608501c69eb9a4ff0dced1fb49a9800 -->
<!-- api: M:Inno.Scene.Layers.GameLayerMask.ToString 43b5487057fb565984692267c38c974148c3f79238ebce0b80a1095f24580daf -->
<!-- api: M:Inno.Scene.Layers.GameLayerMask.op_BitwiseOr(Inno.Scene.Layers.GameLayerMask,Inno.Scene.Layers.GameLayerMask) bc7812575971e990b5346de194d2610ce8b57bfc7f97c53d96a81a4f1e4a62d8 -->
<!-- api: M:Inno.Scene.Layers.GameLayerMask.op_BitwiseAnd(Inno.Scene.Layers.GameLayerMask,Inno.Scene.Layers.GameLayerMask) c884ca5057e60069e1c208b3cc89dd2c19b8b2830d2fcaa43c8c930bceb22f42 -->
<!-- api: M:Inno.Scene.Layers.GameLayerMask.op_OnesComplement(Inno.Scene.Layers.GameLayerMask) a06b5ab27fd6ee9585ba97752737fd6b38e45d1a24af50a87ecbad6d07cbe933 -->
<!-- api: M:Inno.Scene.Layers.GameLayerMask.op_Equality(Inno.Scene.Layers.GameLayerMask,Inno.Scene.Layers.GameLayerMask) f2230713470b627fbcb8bcc7e3a4cc1962b58782b1f6a4a6fab66bbe42da2bc9 -->
<!-- api: M:Inno.Scene.Layers.GameLayerMask.op_Inequality(Inno.Scene.Layers.GameLayerMask,Inno.Scene.Layers.GameLayerMask) 7487035a727a192bd189b40f30bf488e3c730f34b0ae70bcc780ae327c8ab1de -->
<!-- api: T:Inno.Scene.PrefabInstanceInfo cd8193dc1377428ae638a9b17dfea1757d202ef228b666b7750f71001aefa78d -->
<!-- api: P:Inno.Scene.PrefabInstanceInfo.sourceAssetId 6f6846765812e0d5255034bcb708947b96a234b990ca3e9449f34afb8258b69d -->
<!-- api: P:Inno.Scene.PrefabInstanceInfo.sourceObjectId 02c6db8347bbef5375f37f268c1de441617edfe70c32bf4d7642353326ec7ce2 -->
<!-- api: P:Inno.Scene.PrefabInstanceInfo.instanceRoot f84864bdb328dcadb5364f28f8f6edf3de77eeb3499b46b64f66c18b87e2a30d -->
<!-- api: P:Inno.Scene.PrefabInstanceInfo.isRoot 0b0884c3aae9afb291259db166b0858fa779df045af42c8dceb6820952c3b545 -->
<!-- api: P:Inno.Scene.PrefabInstanceInfo.isVariant 3ee883758ac5bd588e2aaaf4cda39f2682f63ddb4a294d8123a0f91924536e0f -->
<!-- api: P:Inno.Scene.PrefabInstanceInfo.isMissing 369cef81c4cd8c8934ff19e842cc2ea018d75dac52e63be43c876d58e6cc608b -->
<!-- api: P:Inno.Scene.PrefabInstanceInfo.overrideCount 7ae60bee5a2188e8475d5ad2a98e0e88fcc40a41c102e7555287aaf758261fe1 -->
<!-- api: P:Inno.Scene.PrefabInstanceInfo.orphanedOverrideCount 0ea15274274478d9346950033aff4bfba69027781668367d84f94e8ff3ea9236 -->
<!-- api: T:Inno.Scene.ISceneReloadStateTransfer 49498b0284a2351f0da710cb0f9b1951c5d1b214c318164c5be90373be0aeaa5 -->
<!-- api: P:Inno.Scene.ISceneReloadStateTransfer.retiredObjects e06b231cb44cb41e569bec69a9ef89337792a21f35b0562688fcc5bb59a3e141 -->
<!-- api: P:Inno.Scene.ISceneReloadStateTransfer.diagnostics 852ca31d65c3e5360d1248f6e6f575044aa01fa0d4136d3c92fb47a5493a9d91 -->
<!-- api: P:Inno.Scene.ISceneReloadStateTransfer.recoveryChanges 3e68715c791930e7d3fd7731444cab00cdfbfdec105de6033969b65c1a79aed5 -->
<!-- api: M:Inno.Scene.ISceneReloadStateTransfer.PrepareForActivation e0b2720b0fb89acfd442ec57da58e871172904d84871f53ad28cf36bf9b30e4b -->
<!-- api: M:Inno.Scene.ISceneReloadStateTransfer.Apply 5f50d16d96ab611f935b7fffb4d0f2ff8e017c17eccc207659d66d2f0d8dd325 -->
<!-- api: M:Inno.Scene.ISceneReloadStateTransfer.RollbackStructure d57e7f173d480afe8b99b3556b2861f937a1c22d35b45e4178ce2ed7bf71bc2c -->
<!-- api: M:Inno.Scene.ISceneReloadStateTransfer.RestorePreviousState 7ac92b3f85d2925a13e78e824d79dd210e3d7241fb1ec125a9e689cf130ecbc8 -->
<!-- api: M:Inno.Scene.ISceneReloadStateTransfer.Complete 239acd14114da9bba519ee8a80b449011b8e8e58bb8b491aa0ec44e318ee94a2 -->
<!-- api: T:Inno.Scene.SceneReloadDiagnostic 8f65c6e87513a7263786cd10cd960d4dee756c3ce23bd2b15facd3c190988438 -->
<!-- api: P:Inno.Scene.SceneReloadDiagnostic.code d66461186e1b86baab199062f61d3061252c6c1bd5de83bec27c654322a25256 -->
<!-- api: P:Inno.Scene.SceneReloadDiagnostic.severity d2e7ab8d45ebc93d3d9bb2787b31a6f47c2d63a2875cd8e39b5d31172a4103a4 -->
<!-- api: P:Inno.Scene.SceneReloadDiagnostic.message b78847281a69288b84f0fc48e1ff397e6e38c1098c194de00d0a680669dc6b69 -->
<!-- api: P:Inno.Scene.SceneReloadDiagnostic.scenePersistentId c83c95530c1ca9df151713ecd0fa3571ca1668003952dc3b066d5217cd8f26e2 -->
<!-- api: P:Inno.Scene.SceneReloadDiagnostic.objectPersistentId 92d88195fe007f37960609b3438bbc4f6b51730736da93738405d25cbffb8537 -->
<!-- api: P:Inno.Scene.SceneReloadDiagnostic.propertyName ddf0b187da9f913acdabd833c68255c46618d4325050c12ecd0fbfb77b1b499a -->
<!-- api: P:Inno.Scene.SceneReloadDiagnostic.previousPropertyType 8b23e19cfdb4266b1a1b555868ae4d844e04dacf4be5f4a567f78f1b64bccf10 -->
<!-- api: P:Inno.Scene.SceneReloadDiagnostic.currentPropertyType c6513d9eaeb8b58e0b5de06656459b6ef9e50e1628d48ac6e09dd590a9e15dc6 -->
<!-- api: T:Inno.Scene.SceneReloadService 3d35629252baee42b0faa7ea84c287741565fa55f1f1fecd5d569d668b60a3b8 -->
<!-- api: M:Inno.Scene.SceneReloadService.#ctor(Inno.Scene.SceneWorld,Inno.Core.Serialization.SerializationRegistry,Inno.Assets.IAssetReferenceResolver) 02e52622dad7f3d796ef90c5f8bea679993e522dfa73af82fcae3bb2622d75a3 -->
<!-- api: M:Inno.Scene.SceneReloadService.Capture(Inno.Extensibility.Types.TypeCacheReloadContext) cabe8bfb5cd0c279508a90f29527a781d6ffe9a17441edea66b8e29cd7d0075f -->
<!-- api: T:Inno.Scene.SceneManager fc35ca2d4e851ebe701c055abf5caf14760812439e7295c7a4f33fe9a378358c -->
<!-- api: P:Inno.Scene.SceneManager.activeScene 9a5d6db40e27f22f50dae094dc857f1dbc86b0ed07bae973bb21fcee42a52f4f -->
<!-- api: P:Inno.Scene.SceneManager.hasActiveScene 6286e2a976bbe28e9770cb4e8526f8ee5ab784856416c12da06d225b906d359e -->
<!-- api: P:Inno.Scene.SceneManager.loadedScenes 3c666a8f82fa5c9476da2fa44bd354ef61c493c30a4734854e14ed7450fae46b -->
<!-- api: M:Inno.Scene.SceneManager.GetSceneIndex(Inno.Scene.GameScene) 82051dca0d12749d43b80425a665ba1ec8d5bdaec5b359c8e99077380482a37e -->
<!-- api: M:Inno.Scene.SceneManager.SetSceneIndex(Inno.Scene.GameScene,System.Int32) 0bd25cd34ba35f918da085bea49e9367ca90b7ad6b66e26d2c421795b1133250 -->
<!-- api: M:Inno.Scene.SceneManager.LoadScene(Inno.Scene.GameScene) 8b55f504ba1bde61b796ef03331b448558996b0fe130fdf1ce870928602159a9 -->
<!-- api: M:Inno.Scene.SceneManager.LoadSceneAdditive(Inno.Scene.GameScene,System.Boolean) 3f9bb0626311370a58e65994fd2acf33f092bbc5c476406a04fae51044ef9da0 -->
<!-- api: M:Inno.Scene.SceneManager.LoadNewScene(System.String) 167a57eb9e6d8e1d69361eac0a8fadb990c527dd29961750f99bb72e904ced9d -->
<!-- api: M:Inno.Scene.SceneManager.LoadNewSceneAdditive(System.String,System.Boolean) 3e1c9ae4bfbe21419b416a6f65bb340b1d4c6500dede77dac2737ca7bfb5b1df -->
<!-- api: M:Inno.Scene.SceneManager.SetActiveScene(Inno.Scene.GameScene) a1fcfbd1b56940427e420371a21cd63feb302fb6878e548a6e90f4523c799e8d -->
<!-- api: M:Inno.Scene.SceneManager.MoveGameObjectToScene(Inno.Scene.GameObject,Inno.Scene.GameScene) 10d41a5437713a53bf86ae8cdd792548667fd7e8b265499260bd62d5ce0cea44 -->
<!-- api: M:Inno.Scene.SceneManager.UnloadActiveScene a6451f9d383d891acc931f25a66cedd8effc3bd1abb137019765461202802701 -->
<!-- api: M:Inno.Scene.SceneManager.UnloadScene(Inno.Scene.GameScene) 981661d836c4a7173fb7e8485610f3467a9e3a187a1962069fe466d391079646 -->
<!-- api: M:Inno.Scene.SceneManager.UnloadAllScenes 9810cacffaf308379070a273b87db19a34aa3bf57b67d2c75459cb8ceb4a243e -->
<!-- api: M:Inno.Scene.SceneManager.FixedUpdate(System.Single) f2348f9812ba4d9cd5b0c7f4365612659cc415e68fac0da6730e98d773158b2f -->
<!-- api: M:Inno.Scene.SceneManager.Update(System.Single) e60ff43e59ae0032d0634ad36012f4d214354dd52dbcb2334c387d331e227887 -->
<!-- api: M:Inno.Scene.SceneManager.LateUpdate(System.Single) e1e01552a515679fae57abb5481af73f1e4798926abf932e37c756bdbdd5ade5 -->
<!-- api: T:Inno.Scene.SceneWorld e93dccb1b91ef4339a40e34b76378b13b460e13fd2db50622c1ca15753ee010f -->
<!-- api: M:Inno.Scene.SceneWorld.#ctor(Inno.Core.Identity.IdentityAllocator,Inno.Extensibility.Types.TypeCatalog) de84f4883713c65bb0b42bd97e4e5ceebb59d66718d70ed919fb4b9a21773358 -->
<!-- api: M:Inno.Scene.SceneWorld.ConfigurePrefabInstantiation(Inno.Core.Serialization.SerializationRegistry,Inno.Assets.IAssetReferenceResolver) 281627915e3694338a194c2cb2fc8b01870abd55366d0683fa6b35abc4c6922c -->
<!-- api: P:Inno.Scene.SceneWorld.activeScene 8fcf23d65153210f093eb8741efc89a57b819e384e03adab689a8e4bc3be30ee -->
<!-- api: P:Inno.Scene.SceneWorld.hasActiveScene b2b552555be28f1c2bfa44d92e86eefc22ac9477e8495c715eb76ca12b5fd94e -->
<!-- api: P:Inno.Scene.SceneWorld.loadedScenes 6a52b1a8bf1de0f94fb8022d41ad24cb4711d820b162d2ef7cbe2c7e7b2a9a47 -->
<!-- api: M:Inno.Scene.SceneWorld.Find``1(System.Guid) ffacaf7ea8e8d52b2a2488f301698d14c52a369c85d6e753ab294013aa9ba6fc -->
<!-- api: M:Inno.Scene.SceneWorld.EnterScope 0df4b35d0d7d7f54227fabbd13a7efdaab7538c0ffc404cd03033c9440071f93 -->
<!-- api: M:Inno.Scene.SceneWorld.GetSceneIndex(Inno.Scene.GameScene) afed1fa35eec06ae87dfc98fc0b5a468e044468431e6f10a6dd2e02b46300a3c -->
<!-- api: M:Inno.Scene.SceneWorld.SetSceneIndex(Inno.Scene.GameScene,System.Int32) c2fcb967ab4c533b713012ed7c051efc47ebf3bb55dbef24d571b0757ffa7974 -->
<!-- api: M:Inno.Scene.SceneWorld.LoadScene(Inno.Scene.GameScene) 38cacbbaf2e783b670b3a164d229b77bf1dceb757dc109d7e6e477acf3c68cd2 -->
<!-- api: M:Inno.Scene.SceneWorld.LoadSceneAdditive(Inno.Scene.GameScene,System.Boolean) e23a83d8b90f753d0078fde195086837ea6210ea57cc68aad490f842e7fea77a -->
<!-- api: M:Inno.Scene.SceneWorld.LoadNewScene(System.String) b0dce470e72ef1daef9d6d23b1e9b07bee3c05863fca5f296e4954ce507b51a4 -->
<!-- api: M:Inno.Scene.SceneWorld.LoadNewSceneAdditive(System.String,System.Boolean) c6ae3f9ce83d162ba388a948668e3ca52c35971b27369981d384382b26bc08b5 -->
<!-- api: M:Inno.Scene.SceneWorld.SetActiveScene(Inno.Scene.GameScene) c3aae4d4a101c117ec5d8f9158e9bc4ca364b3a564cf1519a90a617c4ce6e515 -->
<!-- api: M:Inno.Scene.SceneWorld.MoveGameObjectToScene(Inno.Scene.GameObject,Inno.Scene.GameScene) c4b937ea2b3d8fc12f875dd4ab43a53f7265fd592dc7c84cd56df3d971bd0284 -->
<!-- api: M:Inno.Scene.SceneWorld.UnloadActiveScene 6b3a71677c1acb82efd28d6fd5f238ff4454e7af45199603c8703c7a68818737 -->
<!-- api: M:Inno.Scene.SceneWorld.UnloadScene(Inno.Scene.GameScene) 4a7c2e3f9e8c94a2684d4d93a006edf55b3a48392a992e82c77972b2c51f8a4c -->
<!-- api: M:Inno.Scene.SceneWorld.UnloadAllScenes 0e4cee3d886ef7d3f6578f65c5aff847f6747f2e9cf57effe481f2d9e6300b83 -->
<!-- api: M:Inno.Scene.SceneWorld.FixedUpdate(System.Single) acb290459653e7c7a7dd23dcbf27f2d1124c35f6ce570ad2d94210c258881074 -->
<!-- api: M:Inno.Scene.SceneWorld.Update(System.Single) 10cc7580258fbc6dc4db2c7e9b47f6b9d64c6f4d7b472275106a235faf8a6607 -->
<!-- api: M:Inno.Scene.SceneWorld.LateUpdate(System.Single) 27553f802dc3df502cab1c4ad32b7a5ac2c4dbe8655dd324086715eaf486a919 -->
<!-- api: M:Inno.Scene.SceneWorld.Dispose 1b315d6b83dea4dd5900836aa285f7dc729f0bd61e3c71c67135bb52df41c984 -->
<!-- api: T:Inno.Scene.SceneElementSerialization 8a96c0fd657dce53a122ee7dbd1ec18ce1e9d8393f2e08ebbfb2618babaf5d4b -->
<!-- api: M:Inno.Scene.SceneElementSerialization.CaptureState(Inno.Scene.EngineObject,Inno.Core.Serialization.SerializationRegistry,Inno.Assets.IAssetReferenceResolver) 220b8f89e616b0a5b8981d931de4c2fa3091686f2f83daead61aa1950d6ace5e -->
<!-- api: M:Inno.Scene.SceneElementSerialization.RestoreState(Inno.Scene.EngineObject,System.ReadOnlySpan{System.Byte},Inno.Core.Serialization.SerializationRegistry,Inno.Assets.IAssetReferenceResolver) 58b671e7d1fc612c24a71d88a7e0c05c900384935b7fda47c286086ac56485e7 -->
<!-- api: M:Inno.Scene.SceneElementSerialization.RestoreComponent(Inno.Scene.GameObject,Inno.Extensibility.Types.TypeRef,System.Guid,System.Int32,System.ReadOnlySpan{System.Byte},Inno.Core.Serialization.SerializationRegistry,Inno.Assets.IAssetReferenceResolver) 74c7813881aaa686fb1fc1b9180d6902a6b1f53e84a813dff9b55600bb1cc00e -->
<!-- api: M:Inno.Scene.SceneElementSerialization.RestoreSystem(Inno.Scene.GameScene,Inno.Extensibility.Types.TypeRef,System.Guid,System.Int32,System.ReadOnlySpan{System.Byte},Inno.Core.Serialization.SerializationRegistry,Inno.Assets.IAssetReferenceResolver) 824c11ade5cd7953aa27c6b7d685c92c60a4d62f65c11833ba380b2f762e9a17 -->
<!-- api: T:Inno.Scene.ScenePropertySerialization ebd629e97fc167bf41afb0dd3b9bae6e1e54c4e09e08b60faa6156f513ac9027 -->
<!-- api: M:Inno.Scene.ScenePropertySerialization.CaptureProperty(Inno.Scene.EngineObject,System.String,Inno.Core.Serialization.SerializationRegistry,Inno.Assets.IAssetReferenceResolver) 57661b07e4ef0874501162cb6f939ea6f266b046b90ced51a13b83abde65c0b8 -->
<!-- api: M:Inno.Scene.ScenePropertySerialization.CaptureProperties(Inno.Scene.EngineObject,Inno.Core.Serialization.SerializationRegistry,Inno.Assets.IAssetReferenceResolver) 0dab6e7f3fb02cd0153f060a41d991aa39bb4b4886ebfce8e6cc653ee11182c5 -->
<!-- api: M:Inno.Scene.ScenePropertySerialization.CapturePropertySnapshots(Inno.Scene.EngineObject,Inno.Core.Serialization.SerializationRegistry,Inno.Assets.IAssetReferenceResolver) 52c04cf71b40dc5a9297d8eceae19a0b9f14021211d793c1c177306638005513 -->
<!-- api: M:Inno.Scene.ScenePropertySerialization.RestoreProperties(Inno.Scene.EngineObject,System.ReadOnlySpan{System.Byte},Inno.Core.Serialization.SerializationRegistry,Inno.Assets.IAssetReferenceResolver,Inno.Core.Serialization.SerializationPropertyRestoreMode) 135a606e48cc820aad19360482794eaeda773c67d79f84ce46d490c4ed89a7ff -->
<!-- api: T:Inno.Scene.SceneSubtreeSerialization fc4776832c3e3e5ae9613fabcc204f564d9f1fad775f45ba744eb5c567da514a -->
<!-- api: M:Inno.Scene.SceneSubtreeSerialization.Capture(Inno.Scene.GameObject,Inno.Core.Serialization.SerializationRegistry,Inno.Assets.IAssetReferenceResolver) 6a2004cc7916d08047186d700d8ff7469496c744dad0cc77030ee9a03f49289e -->
<!-- api: M:Inno.Scene.SceneSubtreeSerialization.Restore(Inno.Scene.GameScene,System.ReadOnlySpan{System.Byte},Inno.Core.Serialization.SerializationRegistry,Inno.Assets.IAssetReferenceResolver,Inno.Scene.Components.Transform,System.Int32) 8ff52a3667036c49a1a3fc9d197f0ec981aa6a36e6afbd6503ad9e18c5b05361 -->
<!-- api: T:Inno.Scene.SceneTypeResolutionException 6a52fa46e1e3e63cd7676129b4bf022600c7b105da53b37c120a72caaffef4de -->
<!-- api: M:Inno.Scene.SceneTypeResolutionException.#ctor(System.Guid,System.String) 032ee2119959182d4358591984c2bb07f2508388b9fabf0a345c5dfc9cd2360c -->
<!-- api: P:Inno.Scene.SceneTypeResolutionException.stableTypeId c67f72033215fdb2a40b65584adca41a59801655298f2da1054e3bf1e8e9e624 -->
<!-- api: P:Inno.Scene.SceneTypeResolutionException.elementKind 73479ba2ec2dc52a0aee2989e4ea39f9c35ddc844afab05cfc0c95a3c1385c84 -->

| API 与源码 | 契约与参数 | 返回值 | 重要异常 |
| --- | --- | --- | --- |
| <a id="api-c6d398afe947e188"></a>[`public sealed class Inno.Scene.EngineAssetContent`](../../src/content/scene/Inno.Scene/Assets/EngineAssetContent.cs#L11) | Carries immutable scene or prefab runtime content across the runtime-to-authoring asset boundary. | — | — |
| <a id="api-9c4677efd0cd5d4d"></a>[`public Inno.Scene.EngineAssetContent.EngineAssetContent(System.ReadOnlySpan<byte> payload, System.Collections.Generic.IReadOnlyList<Inno.Assets.AssetDependency> dependencies)`](../../src/content/scene/Inno.Scene/Assets/EngineAssetContent.cs#L28) | Creates a detached engine asset content snapshot.<br>`payload`: The complete serialized runtime graph.<br>`dependencies`: The direct persistent asset dependencies of the graph. | — | `System.ArgumentException`: Thrown when is empty. |
| <a id="api-b1f0c73a2b2839f0"></a>[`public byte[] Inno.Scene.EngineAssetContent.GetPayload()`](../../src/content/scene/Inno.Scene/Assets/EngineAssetContent.cs#L45) | Gets a detached copy of the serialized runtime graph. | A payload copy owned by the caller. | — |
| <a id="api-72e04414d0f920a5"></a>[`public Inno.Assets.AssetDependency[] Inno.Scene.EngineAssetContent.GetDependencies()`](../../src/content/scene/Inno.Scene/Assets/EngineAssetContent.cs#L53) | Gets a detached copy of direct persistent asset dependencies. | A dependency array owned by the caller. | — |
| <a id="api-9067f0c76d5f6edf"></a>[`public sealed class Inno.Scene.PrefabAsset : Inno.Assets.AssetObject`](../../src/content/scene/Inno.Scene/Assets/PrefabAsset.cs#L18) | Stores a persistent game object subtree that can be instantiated repeatedly. | — | — |
| <a id="api-ba353f304edfc74a"></a>[`public static Inno.Scene.PrefabAsset Inno.Scene.PrefabAsset.Capture(Inno.Scene.GameObject root, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Assets.IAssetReferenceResolver assets)`](../../src/content/scene/Inno.Scene/Assets/PrefabAsset.cs#L40) | Captures a game object subtree into a new unsaved prefab asset.<br>`root`: Prefab source root.<br>`serialization`: The serialization registry that owns the active prefab converter generation.<br>`assets`: The runtime asset generation used to resolve persistent references in the prefab graph. | A prefab asset ready to save. | — |
| <a id="api-d538b166cabd1172"></a>[`public Inno.Scene.GameObject Inno.Scene.PrefabAsset.Instantiate(Inno.Scene.GameScene scene, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Assets.IAssetReferenceResolver assets, Inno.Scene.Components.Transform? parent = null)`](../../src/content/scene/Inno.Scene/Assets/PrefabAsset.cs#L77) | Instantiates this prefab into a scene using newly generated object identities.<br>`scene`: Target scene.<br>`serialization`: The serialization registry that owns the active prefab converter generation.<br>`assets`: The runtime asset generation used to resolve persistent references in the prefab graph.<br>`parent`: Optional target parent. | The instantiated root game object. | — |
| <a id="api-8425adea680bcfc4"></a>[`public static Inno.Scene.PrefabAsset Inno.Scene.PrefabAsset.CreateImported(System.ReadOnlySpan<byte> payload, System.Collections.Generic.IReadOnlyList<Inno.Assets.AssetDependency> dependencies)`](../../src/content/scene/Inno.Scene/Assets/PrefabAsset.cs#L112) | Creates an imported prefab asset from validated runtime content produced by the prefab asset pipeline.<br>`payload`: The complete serialized runtime object subtree.<br>`dependencies`: The direct persistent asset dependencies captured with the subtree. | A prefab asset ready for an asset import writer to commit. | — |
| <a id="api-b4a972456ed39304"></a>[`public Inno.Scene.EngineAssetContent Inno.Scene.PrefabAsset.CaptureContent()`](../../src/content/scene/Inno.Scene/Assets/PrefabAsset.cs#L133) | Captures the immutable runtime payload and dependency descriptors required by the prefab asset pipeline. | A detached authoring-content snapshot owned by the caller. | — |
| <a id="api-a4ceba7c4c17b0ce"></a>[`public Inno.Scene.PrefabAsset.PrefabAsset()`](../../src/content/scene/Inno.Scene/Assets/PrefabAsset.cs#L18) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d54e073383184f99"></a>[`public sealed class Inno.Scene.SceneAsset : Inno.Assets.AssetObject`](../../src/content/scene/Inno.Scene/Assets/SceneAsset.cs#L16) | Stores imported scene source that can create independent runtime scenes. | — | — |
| <a id="api-bd7aeaa008d28ece"></a>[`public static Inno.Scene.SceneAsset Inno.Scene.SceneAsset.Capture(Inno.Scene.GameScene scene, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Assets.IAssetReferenceResolver assets)`](../../src/content/scene/Inno.Scene/Assets/SceneAsset.cs#L38) | Captures a runtime scene into a new unsaved scene asset.<br>`scene`: Scene to capture.<br>`serialization`: The serialization registry that owns the active scene converter generation.<br>`assets`: The asset database generation used to resolve references while collecting dependencies. | A detached scene asset that an authoring asset database can save. | — |
| <a id="api-c9bf58dce18cb7bb"></a>[`public void Inno.Scene.SceneAsset.CaptureFrom(Inno.Scene.GameScene scene, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Assets.IAssetReferenceResolver assets)`](../../src/content/scene/Inno.Scene/Assets/SceneAsset.cs#L61) | Replaces the pending source content with a fresh capture while preserving this asset identity.<br>`scene`: Scene to capture.<br>`serialization`: The serialization registry that owns the active scene converter generation.<br>`assets`: The asset database generation used to resolve references while collecting dependencies. | — | — |
| <a id="api-27e293e7e7946bfe"></a>[`public Inno.Scene.GameScene Inno.Scene.SceneAsset.Instantiate(Inno.Core.Serialization.SerializationRegistry serialization, Inno.Assets.IAssetReferenceResolver assets)`](../../src/content/scene/Inno.Scene/Assets/SceneAsset.cs#L88) | Creates an unloaded runtime scene and acquires its hard asset dependencies.<br>`serialization`: The serialization registry that owns the active scene converter generation.<br>`assets`: The runtime asset generation used to resolve persistent references in the scene graph. | A newly allocated scene. | — |
| <a id="api-bfb4472720a2a572"></a>[`public static Inno.Scene.SceneAsset Inno.Scene.SceneAsset.CreateImported(System.ReadOnlySpan<byte> payload, System.Collections.Generic.IReadOnlyList<Inno.Assets.AssetDependency> dependencies)`](../../src/content/scene/Inno.Scene/Assets/SceneAsset.cs#L115) | Creates an imported scene asset from validated runtime content produced by the scene asset pipeline.<br>`payload`: The complete serialized runtime scene graph.<br>`dependencies`: The direct persistent asset dependencies captured with the scene graph. | A scene asset ready for an asset import writer to commit. | — |
| <a id="api-c9c7b8524108519a"></a>[`public Inno.Scene.EngineAssetContent Inno.Scene.SceneAsset.CaptureContent()`](../../src/content/scene/Inno.Scene/Assets/SceneAsset.cs#L136) | Captures the immutable runtime payload and dependency descriptors required by the scene asset pipeline. | A detached authoring-content snapshot owned by the caller. | — |
| <a id="api-b738c62df17c4038"></a>[`protected override void Inno.Scene.SceneAsset.OnRuntimePayloadChanged(System.ReadOnlyMemory<byte> previousPayload, System.ReadOnlyMemory<byte> currentPayload)`](../../src/content/scene/Inno.Scene/Assets/SceneAsset.cs#L156) | Rebuilds runtime-derived state after the serialized asset payload changes.<br>`previousPayload`: The previous payload consumed by on runtime payload changed; ownership remains with the caller unless explicitly stated otherwise.<br>`currentPayload`: The current payload consumed by on runtime payload changed; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-50b51975bb09cb32"></a>[`public Inno.Scene.SceneAsset.SceneAsset()`](../../src/content/scene/Inno.Scene/Assets/SceneAsset.cs#L16) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ebb1044cb513cfde"></a>[`public sealed class Inno.Scene.AllowMultipleComponentAttribute : System.Attribute`](../../src/content/scene/Inno.Scene/Attributes/AllowMultipleComponentAttribute.cs#L9) | Allows multiple instances of a concrete component type on one game object. | — | — |
| <a id="api-52cd3a1554b3816c"></a>[`public Inno.Scene.AllowMultipleComponentAttribute.AllowMultipleComponentAttribute()`](../../src/content/scene/Inno.Scene/Attributes/AllowMultipleComponentAttribute.cs#L9) | Inherited or compiler-provided contract. | — | — |
| <a id="api-2dd200aed3d3c74c"></a>[`public sealed class Inno.Scene.AllowMultipleSystemAttribute : System.Attribute`](../../src/content/scene/Inno.Scene/Attributes/AllowMultipleSystemAttribute.cs#L9) | Allows multiple instances of a concrete type in one scene. | — | — |
| <a id="api-fd0c912e01d73c3a"></a>[`public Inno.Scene.AllowMultipleSystemAttribute.AllowMultipleSystemAttribute()`](../../src/content/scene/Inno.Scene/Attributes/AllowMultipleSystemAttribute.cs#L9) | Inherited or compiler-provided contract. | — | — |
| <a id="api-83abc338c761e599"></a>[`public sealed class Inno.Scene.MissingGameComponent : Inno.Scene.GameComponent`](../../src/content/scene/Inno.Scene/Concrete/MissingGameComponent.cs#L19) | Preserves the identity, order, and serialized state of a component whose managed type is unavailable. | — | — |
| <a id="api-0a13f37d2871b90c"></a>[`public Inno.Extensibility.Types.TypeRef Inno.Scene.MissingGameComponent.missingType { get; }`](../../src/content/scene/Inno.Scene/Concrete/MissingGameComponent.cs#L45) | Gets the logical identity of the unavailable component type. | — | — |
| <a id="api-5c5eaec56a4b63f9"></a>[`public string Inno.Scene.MissingGameComponent.missingTypeName { get; }`](../../src/content/scene/Inno.Scene/Concrete/MissingGameComponent.cs#L50) | Gets the last known managed type name for diagnostics and editor presentation. | — | — |
| <a id="api-fa7f9f58aebcae97"></a>[`public sealed class Inno.Scene.MissingGameSystem : Inno.Scene.GameSystem`](../../src/content/scene/Inno.Scene/Concrete/MissingGameSystem.cs#L19) | Preserves the identity, order, and serialized state of a scene system whose managed type is unavailable. | — | — |
| <a id="api-c0e839020f220381"></a>[`public Inno.Extensibility.Types.TypeRef Inno.Scene.MissingGameSystem.missingType { get; }`](../../src/content/scene/Inno.Scene/Concrete/MissingGameSystem.cs#L46) | Gets the logical identity of the unavailable system type. | — | — |
| <a id="api-d6ea197ae038cefe"></a>[`public string Inno.Scene.MissingGameSystem.missingTypeName { get; }`](../../src/content/scene/Inno.Scene/Concrete/MissingGameSystem.cs#L51) | Gets the last known managed type name for diagnostics and editor presentation. | — | — |
| <a id="api-bca5da9533a89e7b"></a>[`public sealed class Inno.Scene.Components.Transform : Inno.Scene.GameComponent`](../../src/content/scene/Inno.Scene/Concrete/Transform.cs#L15) | Stores local transform data and exposes the scene hierarchy relationship. | — | — |
| <a id="api-df72ab16dd66601b"></a>[`public Inno.Scene.Components.Transform.Transform()`](../../src/content/scene/Inno.Scene/Concrete/Transform.cs#L31) | Creates a transform with identity local values. | — | — |
| <a id="api-3fead801ce47b317"></a>[`public Inno.Core.Mathematics.Vector3 Inno.Scene.Components.Transform.localPosition { get; set; }`](../../src/content/scene/Inno.Scene/Concrete/Transform.cs#L40) | Gets or sets the local position relative to the parent. | — | — |
| <a id="api-67b00267d26f9cb2"></a>[`public Inno.Core.Mathematics.Quaternion Inno.Scene.Components.Transform.localRotation { get; set; }`](../../src/content/scene/Inno.Scene/Concrete/Transform.cs#L54) | Gets or sets the local rotation relative to the parent. | — | — |
| <a id="api-2c421b3ce0636ebe"></a>[`public Inno.Core.Mathematics.Vector3 Inno.Scene.Components.Transform.localScale { get; set; }`](../../src/content/scene/Inno.Scene/Concrete/Transform.cs#L68) | Gets or sets the local scale relative to the parent. | — | — |
| <a id="api-d6cef86a6b89ba86"></a>[`public Inno.Core.Mathematics.Vector3 Inno.Scene.Components.Transform.worldPosition { get; set; }`](../../src/content/scene/Inno.Scene/Concrete/Transform.cs#L81) | Gets or sets the world-space position. | — | — |
| <a id="api-2f1eccf1d9c768eb"></a>[`public Inno.Core.Mathematics.Quaternion Inno.Scene.Components.Transform.worldRotation { get; set; }`](../../src/content/scene/Inno.Scene/Concrete/Transform.cs#L90) | Gets or sets the world-space rotation. | — | — |
| <a id="api-424e285db70e89aa"></a>[`public Inno.Core.Mathematics.Vector3 Inno.Scene.Components.Transform.worldScale { get; set; }`](../../src/content/scene/Inno.Scene/Concrete/Transform.cs#L99) | Gets or sets the world-space scale. | — | — |
| <a id="api-6442179ac19af1c5"></a>[`public Inno.Core.Mathematics.Matrix Inno.Scene.Components.Transform.localToWorldMatrix { get; }`](../../src/content/scene/Inno.Scene/Concrete/Transform.cs#L108) | Gets the exact local-to-world matrix, including the complete parent hierarchy. | — | — |
| <a id="api-c7b95c48b47031c5"></a>[`public Inno.Core.Mathematics.Matrix Inno.Scene.Components.Transform.worldToLocalMatrix { get; }`](../../src/content/scene/Inno.Scene/Concrete/Transform.cs#L116) | Gets the inverse of the exact local-to-world matrix. | — | `System.InvalidOperationException`: Thrown when a zero-scale hierarchy is not invertible. |
| <a id="api-3081063399fae2b8"></a>[`public Inno.Scene.Components.Transform? Inno.Scene.Components.Transform.parent { get; }`](../../src/content/scene/Inno.Scene/Concrete/Transform.cs#L128) | Gets the parent transform, or for a scene-level object. | — | — |
| <a id="api-c52dee07e9ad5cc1"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Scene.Components.Transform> Inno.Scene.Components.Transform.children { get; }`](../../src/content/scene/Inno.Scene/Concrete/Transform.cs#L133) | Gets child transforms in sibling order. | — | — |
| <a id="api-4c6d14599a824bdb"></a>[`public int Inno.Scene.Components.Transform.siblingIndex { get; set; }`](../../src/content/scene/Inno.Scene/Concrete/Transform.cs#L138) | Gets or sets this transform's index among its siblings. | — | — |
| <a id="api-f35410e4baaf27c8"></a>[`public void Inno.Scene.Components.Transform.SetParent(Inno.Scene.Components.Transform? parent)`](../../src/content/scene/Inno.Scene/Concrete/Transform.cs#L153) | Sets the parent while preserving this transform's world-space values.<br>`parent`: New parent, or to move to scene level. | — | `System.InvalidOperationException`: Thrown for hierarchy cycles, cross-scene parents, or a non-invertible parent hierarchy. |
| <a id="api-ce219eabcc524f8b"></a>[`public void Inno.Scene.Components.Transform.SetSiblingIndex(int siblingIndex)`](../../src/content/scene/Inno.Scene/Concrete/Transform.cs#L161) | Moves this transform within its current sibling collection.<br>`siblingIndex`: Requested zero-based sibling index. | — | — |
| <a id="api-7197c393ac8a00ba"></a>[`public void Inno.Scene.Components.Transform.SetWorldTransform(Inno.Core.Mathematics.Vector3 position, Inno.Core.Mathematics.Quaternion rotation, Inno.Core.Mathematics.Vector3 scale)`](../../src/content/scene/Inno.Scene/Concrete/Transform.cs#L175) | Atomically applies world-space translation, rotation, and scale.<br>`position`: Requested world-space translation.<br>`rotation`: Requested world-space rotation.<br>`scale`: Requested world-space scale. | — | — |
| <a id="api-f0278759126e5288"></a>[`public Inno.Core.Mathematics.Vector3 Inno.Scene.Components.Transform.TransformPoint(Inno.Core.Mathematics.Vector3 point)`](../../src/content/scene/Inno.Scene/Concrete/Transform.cs#L190) | Transforms a local-space point through the complete parent hierarchy.<br>`point`: Point expressed in this transform's local space. | The corresponding world-space point. | — |
| <a id="api-ec594d6c7ffd9866"></a>[`public Inno.Core.Mathematics.Vector3 Inno.Scene.Components.Transform.InverseTransformPoint(Inno.Core.Mathematics.Vector3 point)`](../../src/content/scene/Inno.Scene/Concrete/Transform.cs#L201) | Transforms a world-space point into this transform's local space.<br>`point`: Point expressed in world space. | The corresponding local-space point. | — |
| <a id="api-ad86042da30c7c2b"></a>[`protected override void Inno.Scene.Components.Transform.Reset()`](../../src/content/scene/Inno.Scene/Concrete/Transform.cs#L206) | Restores this instance to its initial reusable state. | — | — |
| <a id="api-223aabd7a9d8e8fd"></a>[`public static class Inno.Scene.SceneContentSource`](../../src/content/scene/Inno.Scene/Content/SceneContentSource.cs#L11) | Projects loaded scenes into the shared content protocol without depending on a rendering or audio model. | — | — |
| <a id="api-abc07d5a9a1f75eb"></a>[`public static Inno.References.ContentReadScope Inno.Scene.SceneContentSource.CreateScope(Inno.Scene.SceneWorld world)`](../../src/content/scene/Inno.Scene/Content/SceneContentSource.cs#L22) | Captures the ordered roots and primary identity of one scene world.<br>`world`: The owner whose current scene set should be visible. | A caller-owned, operation-scoped identity view. | — |
| <a id="api-e2c2aa4d84dc653a"></a>[`public static Inno.References.ContentReadScope Inno.Scene.SceneContentSource.CreateScope(System.Collections.Generic.IEnumerable<Inno.Scene.GameScene> scenes, Inno.Scene.GameScene? activeScene)`](../../src/content/scene/Inno.Scene/Content/SceneContentSource.cs#L40) | Captures an explicit coherent scene presentation selected by a host.<br>`scenes`: The ordered live scene roots.<br>`activeScene`: The optional primary root, which must be in the collection. | A caller-owned scope that retains no scene instances. | — |
| <a id="api-4193365947cda839"></a>[`public abstract class Inno.Scene.EngineObject : Inno.Core.Identity.IdentityObject`](../../src/content/scene/Inno.Scene/Core/EngineObject.cs#L11) | Provides identity and destruction state shared by all managed scene objects. | — | — |
| <a id="api-49ed06b6f2418293"></a>[`public bool Inno.Scene.EngineObject.isDestroyed { get; }`](../../src/content/scene/Inno.Scene/Core/EngineObject.cs#L18) | Gets whether the engine has destroyed this object. | — | — |
| <a id="api-83002fede4f72e8e"></a>[`public static bool Inno.Scene.EngineObject.operator ==(Inno.Scene.EngineObject? left, Inno.Scene.EngineObject? right)`](../../src/content/scene/Inno.Scene/Core/EngineObject.cs#L32) | Compares engine objects using reference identity and destroyed-object null semantics.<br>`left`: Left operand.<br>`right`: Right operand. | when both operands refer to the same live object or both behave as null. | — |
| <a id="api-5b3278197e66d67f"></a>[`public static bool Inno.Scene.EngineObject.operator !=(Inno.Scene.EngineObject? left, Inno.Scene.EngineObject? right)`](../../src/content/scene/Inno.Scene/Core/EngineObject.cs#L57) | Compares engine objects using reference identity and destroyed-object null semantics.<br>`left`: Left operand.<br>`right`: Right operand. | when the operands do not represent the same object. | — |
| <a id="api-5d4fc557765064d4"></a>[`public override bool Inno.Scene.EngineObject.Equals(object? obj)`](../../src/content/scene/Inno.Scene/Core/EngineObject.cs#L71) | Compares this instance to another object using reference identity.<br>`obj`: The object to compare with this instance. | when is this instance. | — |
| <a id="api-6c3103c5e025835b"></a>[`public override int Inno.Scene.EngineObject.GetHashCode()`](../../src/content/scene/Inno.Scene/Core/EngineObject.cs#L79) | Returns a stable runtime reference hash code. | The reference hash code. | — |
| <a id="api-4df7458318f0d215"></a>[`protected Inno.Scene.EngineObject.EngineObject()`](../../src/content/scene/Inno.Scene/Core/EngineObject.cs#L11) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a242ae00b59fd2d9"></a>[`public abstract class Inno.Scene.GameBehavior : Inno.Scene.GameComponent, Inno.Scene.ISceneLifecycleObject`](../../src/content/scene/Inno.Scene/Core/GameBehavior.cs#L10) | Base component for independently enabled scene functionality with frame lifecycle callbacks. | — | — |
| <a id="api-d5c7292f6f751ad9"></a>[`public bool Inno.Scene.GameBehavior.enabled { get; set; }`](../../src/content/scene/Inno.Scene/Core/GameBehavior.cs#L22) | Gets or sets whether this component participates in scene lifecycle and frame updates. | — | — |
| <a id="api-eae58f0e400a6884"></a>[`public bool Inno.Scene.GameBehavior.isActiveAndEnabled { get; }`](../../src/content/scene/Inno.Scene/Core/GameBehavior.cs#L39) | Gets whether this component is enabled and active in its owning hierarchy. | — | — |
| <a id="api-09401c1a631d0e80"></a>[`protected virtual void Inno.Scene.GameBehavior.Awake()`](../../src/content/scene/Inno.Scene/Core/GameBehavior.cs#L44) | Called once before this behavior first becomes active. | — | — |
| <a id="api-2a783039be291949"></a>[`protected virtual void Inno.Scene.GameBehavior.Start()`](../../src/content/scene/Inno.Scene/Core/GameBehavior.cs#L51) | Called once immediately before the first update of this behavior. | — | — |
| <a id="api-d48df1188a3b1c2a"></a>[`protected virtual void Inno.Scene.GameBehavior.OnEnable()`](../../src/content/scene/Inno.Scene/Core/GameBehavior.cs#L58) | Called when this component becomes active and enabled. | — | — |
| <a id="api-6f72ab8347b20553"></a>[`protected virtual void Inno.Scene.GameBehavior.OnDisable()`](../../src/content/scene/Inno.Scene/Core/GameBehavior.cs#L65) | Called when this component stops being active and enabled. | — | — |
| <a id="api-294fcf678533efe4"></a>[`protected virtual void Inno.Scene.GameBehavior.Update()`](../../src/content/scene/Inno.Scene/Core/GameBehavior.cs#L72) | Called during the variable-rate update stage. Use Time.deltaTime for frame timing. | — | — |
| <a id="api-a9d213cf79580d63"></a>[`protected virtual void Inno.Scene.GameBehavior.FixedUpdate()`](../../src/content/scene/Inno.Scene/Core/GameBehavior.cs#L79) | Called during the fixed-rate update stage. Use Time.fixedDeltaTime for step timing. | — | — |
| <a id="api-6e3808cb7484c1d5"></a>[`protected virtual void Inno.Scene.GameBehavior.LateUpdate()`](../../src/content/scene/Inno.Scene/Core/GameBehavior.cs#L86) | Called during the late update stage. Use Time.deltaTime for frame timing. | — | — |
| <a id="api-21e9be392279fe53"></a>[`protected virtual void Inno.Scene.GameBehavior.OnDestroy()`](../../src/content/scene/Inno.Scene/Core/GameBehavior.cs#L93) | Called immediately before this behavior is detached and destroyed. | — | — |
| <a id="api-5ad8a5e4e2b4dbff"></a>[`protected Inno.Scene.GameBehavior.GameBehavior()`](../../src/content/scene/Inno.Scene/Core/GameBehavior.cs#L10) | Inherited or compiler-provided contract. | — | — |
| <a id="api-7ec592e790fda06e"></a>[`public abstract class Inno.Scene.GameComponent : Inno.Scene.EngineObject, Inno.Core.Serialization.ISerializable`](../../src/content/scene/Inno.Scene/Core/GameComponent.cs#L11) | Base type for data and behavior objects attached to a . | — | — |
| <a id="api-1eea17a5b0b41565"></a>[`public Inno.Scene.GameObject Inno.Scene.GameComponent.gameObject { get; }`](../../src/content/scene/Inno.Scene/Core/GameComponent.cs#L21) | Gets the owning game object. | — | `System.InvalidOperationException`: Thrown when the component is detached or destroyed. |
| <a id="api-16b25184d3564ff1"></a>[`public Inno.Scene.Components.Transform Inno.Scene.GameComponent.transform { get; }`](../../src/content/scene/Inno.Scene/Core/GameComponent.cs#L32) | Gets the owning game object's transform. | — | `System.InvalidOperationException`: Thrown when the component is detached or destroyed. |
| <a id="api-eaad18b41df52183"></a>[`protected virtual void Inno.Scene.GameComponent.Reset()`](../../src/content/scene/Inno.Scene/Core/GameComponent.cs#L37) | Restores this component to its default state when it is added or explicitly reset. | — | — |
| <a id="api-cfeaa30c400738b8"></a>[`protected Inno.Scene.GameComponent.GameComponent()`](../../src/content/scene/Inno.Scene/Core/GameComponent.cs#L11) | Inherited or compiler-provided contract. | — | — |
| <a id="api-460b7860543513db"></a>[`public sealed class Inno.Scene.GameObject : Inno.Scene.EngineObject, Inno.Core.Serialization.ISerializable`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L15) | Represents a scene-owned object and exposes its component-oriented API. | — | — |
| <a id="api-14becb0b5faac531"></a>[`public const string Inno.Scene.GameObject.defaultTag = "Untagged"`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L20) | Defines the tag assigned to newly created game objects. | — | — |
| <a id="api-df84997e85bcb370"></a>[`public bool Inno.Scene.GameObject.isRuntimeValid { get; }`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L43) | Gets whether this object is live in its owning scene. | — | — |
| <a id="api-3bac6ea069a929ef"></a>[`public Inno.Scene.GameScene Inno.Scene.GameObject.scene { get; }`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L51) | Gets the owning scene. | — | `System.InvalidOperationException`: Thrown when this object is destroyed or detached. |
| <a id="api-377dbf646d0e71bb"></a>[`public string Inno.Scene.GameObject.name { get; set; }`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L56) | Gets or sets the display name stored in the owning scene. | — | — |
| <a id="api-958dd524681e4724"></a>[`public string Inno.Scene.GameObject.tag { get; set; }`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L84) | Gets or sets the ordinal tag used to categorize and query this game object. | — | `System.ArgumentException`: Thrown when the assigned tag is empty or contains only white-space characters. |
| <a id="api-d722486bf52be2a8"></a>[`public Inno.Scene.Layers.GameLayer Inno.Scene.GameObject.layer { get; set; }`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L109) | Gets or sets the single runtime layer used to filter this game object. | — | — |
| <a id="api-aa14db106f08ce90"></a>[`public Inno.Scene.Components.Transform Inno.Scene.GameObject.transform { get; }`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L132) | Gets the mandatory transform component. | — | `System.InvalidOperationException`: Thrown when this object is destroyed or incompletely initialized. |
| <a id="api-c7d8f051f35f6e6f"></a>[`public bool Inno.Scene.GameObject.activeSelf { get; }`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L144) | Gets whether this object is explicitly active. | — | — |
| <a id="api-1f23631df7f662f7"></a>[`public bool Inno.Scene.GameObject.activeInHierarchy { get; }`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L156) | Gets whether this object is active after parent hierarchy state is applied. | — | — |
| <a id="api-0a28aa1aad86bcf1"></a>[`public bool Inno.Scene.GameObject.isPartOfPrefabInstance { get; }`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L161) | Gets whether this object retains a prefab source connection. | — | — |
| <a id="api-6584c1ac81792cc1"></a>[`public Inno.Scene.GameObject? Inno.Scene.GameObject.prefabInstanceRoot { get; }`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L166) | Gets the root of this object's prefab instance connection. | — | — |
| <a id="api-47b50f4fe3493569"></a>[`public Inno.Scene.PrefabInstanceInfo? Inno.Scene.GameObject.prefabInstance { get; }`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L171) | Gets read-only information about this object's prefab source connection. | — | — |
| <a id="api-f174e1ad444d737c"></a>[`public void Inno.Scene.GameObject.SetActive(bool value)`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L179) | Changes this object's explicit active state and updates its hierarchy subtree.<br>`value`: The concrete value read or transformed by this operation. | — | — |
| <a id="api-1633126aea5ea46f"></a>[`public TComponent Inno.Scene.GameObject.AddComponent<TComponent>() where TComponent : Inno.Scene.GameComponent`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L190) | Creates and attaches a component of the requested type.<br>`TComponent`: Concrete component type. | The attached component. | — |
| <a id="api-aa4e43f348b52838"></a>[`public Inno.Scene.GameComponent Inno.Scene.GameObject.AddComponent(System.Type componentType)`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L201) | Creates and attaches a component of the requested runtime type.<br>`componentType`: Concrete component type. | The attached component. | — |
| <a id="api-bcec30a148030e29"></a>[`public TComponent Inno.Scene.GameObject.GetComponent<TComponent>() where TComponent : Inno.Scene.GameComponent`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L216) | Gets the first attached component assignable to the requested type.<br>`TComponent`: Requested component type. | The first matching component. | `System.InvalidOperationException`: Thrown when no matching component exists. |
| <a id="api-dbe82fd0307121c7"></a>[`public Inno.Scene.GameComponent Inno.Scene.GameObject.GetComponent(System.Type componentType)`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L235) | Gets the first attached component assignable to a runtime type.<br>`componentType`: Requested component type. | The first matching component. | `System.InvalidOperationException`: Thrown when no matching component exists. |
| <a id="api-d779ee992084a43f"></a>[`public bool Inno.Scene.GameObject.TryGetComponent<TComponent>(out TComponent? component) where TComponent : Inno.Scene.GameComponent`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L255) | Tries to get the first attached component assignable to the requested type.<br>`component`: Matching component when found.<br>`TComponent`: Requested component type. | when a matching component exists. | — |
| <a id="api-8a0b6f2e6aff010b"></a>[`public bool Inno.Scene.GameObject.HasComponent<TComponent>() where TComponent : Inno.Scene.GameComponent`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L275) | Gets whether a matching component is attached.<br>`TComponent`: Requested component type. | when a matching component exists. | — |
| <a id="api-4106f29341134f0c"></a>[`public System.Collections.Generic.IReadOnlyList<TComponent> Inno.Scene.GameObject.GetComponents<TComponent>() where TComponent : Inno.Scene.GameComponent`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L286) | Gets all attached components assignable to the requested type in attachment order.<br>`TComponent`: Requested component type. | A stable component snapshot. | — |
| <a id="api-5b6b606d277eab3f"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Scene.GameComponent> Inno.Scene.GameObject.GetComponents()`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L295) | Gets all attached components in attachment order. | A stable component snapshot. | — |
| <a id="api-2e578abf08fd72cc"></a>[`public int Inno.Scene.GameObject.GetComponentIndex(Inno.Scene.GameComponent component)`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L306) | Gets the attachment index of a component on this object.<br>`component`: Attached component to locate. | The zero-based attachment index. | — |
| <a id="api-3687f73df7b6a875"></a>[`public void Inno.Scene.GameObject.SetComponentIndex(Inno.Scene.GameComponent component, int componentIndex)`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L322) | Moves an attached component to a requested attachment index. The mandatory Transform participates in ordering but remains non-removable.<br>`component`: Attached component to move.<br>`componentIndex`: Requested zero-based attachment index. | — | — |
| <a id="api-07e9f9f0049430db"></a>[`public void Inno.Scene.GameObject.ResetComponent(Inno.Scene.GameComponent component)`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L342) | Restores an attached component to the defaults defined by its optional Reset message.<br>`component`: GameComponent instance to reset. | — | `System.ArgumentNullException`: Thrown when is .<br>`System.InvalidOperationException`: Thrown when the object is invalid or the component is not attached to it. |
| <a id="api-b0ed89a796ce7f92"></a>[`public bool Inno.Scene.GameObject.RemoveComponent(Inno.Scene.GameComponent component)`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L360) | Removes and destroys a specific attached component.<br>`component`: GameComponent instance to remove. | when the component was attached and removed. | `System.InvalidOperationException`: Thrown when attempting to remove the mandatory transform. |
| <a id="api-168352cd50f38424"></a>[`public sealed class Inno.Scene.GameScene : Inno.Scene.EngineObject, Inno.Core.Serialization.ISerializable`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L17) | Owns a runtime scene, including its objects, components, hierarchy, and systems. | — | — |
| <a id="api-7ec9e66ed3f27579"></a>[`public Inno.Scene.GameScene.GameScene(string name = "Untitled Scene")`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L36) | Creates an empty scene.<br>`name`: Initial scene name. | — | — |
| <a id="api-cbd640d63f2fa05b"></a>[`public string Inno.Scene.GameScene.name { get; set; }`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L57) | Gets or sets the scene display name. | — | — |
| <a id="api-9b8bea2354891874"></a>[`public bool Inno.Scene.GameScene.isLoaded { get; }`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L70) | Gets whether this scene is currently owned by . | — | — |
| <a id="api-b2fd01dd04b45bfd"></a>[`public ulong Inno.Scene.GameScene.hierarchyRevision { get; }`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L79) | Gets the owner-local change counter for object membership, parentage and sibling ordering. | — | — |
| <a id="api-3c99ba3c2ae8cc91"></a>[`public Inno.Scene.GameObject Inno.Scene.GameScene.CreateObject(string name = "GameObject")`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L93) | Creates a game object with a mandatory transform component.<br>`name`: Initial game object name. | The created game object. | — |
| <a id="api-fada3b6477e6b68f"></a>[`public Inno.Scene.GameObject Inno.Scene.GameScene.InstantiatePrefab(Inno.Scene.PrefabAsset prefab, Inno.Scene.Components.Transform? parent = null)`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L114) | Creates a connected instance of an imported prefab in this loaded scene.<br>`prefab`: The prefab asset that supplies the object subtree.<br>`parent`: Optional parent for the new instance. | The newly instantiated prefab root. | `System.ArgumentNullException`: The prefab is null.<br>`System.InvalidOperationException`: The scene is not loaded in its owning world or prefab services are unavailable. |
| <a id="api-db54f0e3f1579504"></a>[`public bool Inno.Scene.GameScene.DestroyObject(Inno.Scene.GameObject gameObject)`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L130) | Recursively destroys a game object and its complete child subtree.<br>`gameObject`: Root object to destroy. | when the object belonged to this scene and was destroyed. | — |
| <a id="api-8df7285722a64f36"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Scene.GameObject> Inno.Scene.GameScene.GetObjects()`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L185) | Gets all committed live game objects in scene storage order. | A stable object snapshot. | — |
| <a id="api-5a0e4edbdd84ac4d"></a>[`public Inno.Scene.GameObject? Inno.Scene.GameScene.FindObject(string name)`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L200) | Finds the first game object with an ordinally matching name.<br>`name`: Name to find. | The first match, or when no object matches. | — |
| <a id="api-158e0e433eac6bd7"></a>[`public Inno.Scene.GameObject? Inno.Scene.GameScene.FindObjectWithTag(string tag)`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L219) | Finds the first game object with an ordinally matching tag in scene storage order.<br>`tag`: Tag to find. | The first match, or when no object matches. | `System.ArgumentException`: Thrown when is empty or contains only white-space characters. |
| <a id="api-935ce9f489ea8456"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Scene.GameObject> Inno.Scene.GameScene.FindObjectsWithTag(string tag)`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L237) | Finds every game object with an ordinally matching tag in scene storage order.<br>`tag`: Tag to find. | A stable snapshot containing every matching game object. | `System.ArgumentException`: Thrown when is empty or contains only white-space characters. |
| <a id="api-c292f70e344ce0ba"></a>[`public Inno.Scene.GameObject? Inno.Scene.GameScene.FindObjectWithLayer(Inno.Scene.Layers.GameLayer layer)`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L252) | Finds the first game object assigned to a layer in scene storage order.<br>`layer`: The layer slot to find. | The first matching game object, or when no object matches. | — |
| <a id="api-911b316d3cd5576d"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Scene.GameObject> Inno.Scene.GameScene.FindObjectsWithLayer(Inno.Scene.Layers.GameLayer layer)`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L267) | Finds every game object assigned to one layer in scene storage order.<br>`layer`: The layer slot to find. | A stable snapshot containing every matching game object. | — |
| <a id="api-e1662c9dbf262a04"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Scene.GameObject> Inno.Scene.GameScene.FindObjectsWithLayers(Inno.Scene.Layers.GameLayerMask layers)`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L282) | Finds every game object assigned to any layer contained in a mask.<br>`layers`: The set of accepted layer slots. | A stable snapshot in scene storage order. | — |
| <a id="api-64953fbbb3ceaa48"></a>[`public TSystem Inno.Scene.GameScene.AddSystem<TSystem>() where TSystem : Inno.Scene.GameSystem, new()`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L297) | Creates and registers a parameterless game system.<br>`TSystem`: Concrete system type. | The registered system. | — |
| <a id="api-c950ca38d6e2ca41"></a>[`public Inno.Scene.GameSystem Inno.Scene.GameScene.AddSystem(System.Type systemType)`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L312) | Creates and registers a concrete game system by runtime type.<br>`systemType`: The system type consumed by add system; ownership remains with the caller unless explicitly stated otherwise. | The validated game system that represents the completed operation. | — |
| <a id="api-6bb9b2f65ac5db0c"></a>[`public void Inno.Scene.GameScene.AddSystem(Inno.Scene.GameSystem system)`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L330) | Registers a game system instance.<br>`system`: System to register. | — | — |
| <a id="api-68eb21822dd0f3b6"></a>[`public bool Inno.Scene.GameScene.RemoveSystem(Inno.Scene.GameSystem system)`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L345) | Removes a registered game system.<br>`system`: System to remove. | when the system was registered. | — |
| <a id="api-49bf512be8d0504e"></a>[`public void Inno.Scene.GameScene.ResetSystem(Inno.Scene.GameSystem system)`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L358) | Explicitly restores a registered system to its default state.<br>`system`: The system consumed by reset system; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-bb8b0a735f297852"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Scene.GameSystem> Inno.Scene.GameScene.GetSystems()`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L372) | Gets registered systems in display and serialization order. Explicit values independently control lifecycle execution priority. | An immutable snapshot of the values selected by the operation. | — |
| <a id="api-a8e8ef7d325315b6"></a>[`public int Inno.Scene.GameScene.GetSystemIndex(Inno.Scene.GameSystem system)`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L387) | Gets the display index of a registered system.<br>`system`: Registered system to locate. | The zero-based display index. | — |
| <a id="api-bf54be32af0a7f19"></a>[`public void Inno.Scene.GameScene.SetSystemIndex(Inno.Scene.GameSystem system, int systemIndex)`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L404) | Moves a registered system to a requested display and serialization index. This operation does not change its explicit .<br>`system`: Registered system to move.<br>`systemIndex`: Requested zero-based display index. | — | — |
| <a id="api-65af3e79c0137419"></a>[`public abstract class Inno.Scene.GameSystem : Inno.Scene.EngineObject, Inno.Core.Serialization.ISerializable, Inno.Scene.ISceneLifecycleObject`](../../src/content/scene/Inno.Scene/Core/GameSystem.cs#L13) | Base type for serializable, ordered scene-level behaviors. | — | — |
| <a id="api-e767bd4b540e96e3"></a>[`public bool Inno.Scene.GameSystem.enabled { get; set; }`](../../src/content/scene/Inno.Scene/Core/GameSystem.cs#L22) | Gets or sets whether this system participates in scene lifecycle updates. | — | — |
| <a id="api-6ffc8e6d18652829"></a>[`public bool Inno.Scene.GameSystem.isActiveAndEnabled { get; }`](../../src/content/scene/Inno.Scene/Core/GameSystem.cs#L39) | Gets whether this system is registered, enabled, and dispatchable. | — | — |
| <a id="api-96eae0efccd82b64"></a>[`public virtual int Inno.Scene.GameSystem.order { get; }`](../../src/content/scene/Inno.Scene/Core/GameSystem.cs#L44) | Gets the ascending execution order used by the owning scene. | — | — |
| <a id="api-5745522a9dd23d37"></a>[`protected Inno.Scene.GameScene Inno.Scene.GameSystem.scene { get; }`](../../src/content/scene/Inno.Scene/Core/GameSystem.cs#L49) | Gets the owning scene after this system has been registered. | — | — |
| <a id="api-ad44538f533da3ff"></a>[`protected virtual void Inno.Scene.GameSystem.Reset()`](../../src/content/scene/Inno.Scene/Core/GameSystem.cs#L56) | Restores this system to defaults when added or explicitly reset. | — | — |
| <a id="api-206e523828fa9ece"></a>[`protected virtual void Inno.Scene.GameSystem.Awake()`](../../src/content/scene/Inno.Scene/Core/GameSystem.cs#L63) | Called once before the system first becomes active. | — | — |
| <a id="api-97b1fa74368c5f14"></a>[`protected virtual void Inno.Scene.GameSystem.Start()`](../../src/content/scene/Inno.Scene/Core/GameSystem.cs#L70) | Called once immediately before the first update. | — | — |
| <a id="api-e81d9bf7f8a34c2b"></a>[`protected virtual void Inno.Scene.GameSystem.OnEnable()`](../../src/content/scene/Inno.Scene/Core/GameSystem.cs#L77) | Called when the system becomes active and enabled. | — | — |
| <a id="api-745afb4cfbdb1ede"></a>[`protected virtual void Inno.Scene.GameSystem.OnDisable()`](../../src/content/scene/Inno.Scene/Core/GameSystem.cs#L84) | Called when the system stops being active and enabled. | — | — |
| <a id="api-956cdd8cf3de8957"></a>[`protected virtual void Inno.Scene.GameSystem.OnDestroy()`](../../src/content/scene/Inno.Scene/Core/GameSystem.cs#L91) | Called before a system that entered runtime lifecycle is destroyed. | — | — |
| <a id="api-212d83249d65c68d"></a>[`protected virtual void Inno.Scene.GameSystem.OnFixedUpdate()`](../../src/content/scene/Inno.Scene/Core/GameSystem.cs#L98) | Called during the fixed-rate scene stage. Use Time.fixedDeltaTime for step timing. | — | — |
| <a id="api-2af9a7de34f368a0"></a>[`protected virtual void Inno.Scene.GameSystem.OnUpdate()`](../../src/content/scene/Inno.Scene/Core/GameSystem.cs#L105) | Called during the variable-rate scene stage. Use Time.deltaTime for frame timing. | — | — |
| <a id="api-e7723170f2fdf71d"></a>[`protected virtual void Inno.Scene.GameSystem.OnLateUpdate()`](../../src/content/scene/Inno.Scene/Core/GameSystem.cs#L112) | Called during the late scene stage. Use Time.deltaTime for frame timing. | — | — |
| <a id="api-6760db39b41478fa"></a>[`protected System.Collections.Generic.IReadOnlyList<TComponent> Inno.Scene.GameSystem.GetComponents<TComponent>() where TComponent : Inno.Scene.GameComponent`](../../src/content/scene/Inno.Scene/Core/GameSystem.cs#L125) | Gets all scene components assignable to a requested type.<br>`TComponent`: The component base or concrete type included in the result. | An immutable snapshot of the values selected by the operation. | — |
| <a id="api-fa9b301d6dc18012"></a>[`protected System.Collections.Generic.IReadOnlyList<Inno.Scene.GameObject> Inno.Scene.GameSystem.GetObjects()`](../../src/content/scene/Inno.Scene/Core/GameSystem.cs#L133) | Gets all committed live game objects in deterministic scene storage order. | The immutable structure snapshot retained by the scene until its object or component structure changes. | — |
| <a id="api-1717ffdda6264a99"></a>[`protected System.Collections.Generic.IReadOnlyList<Inno.Scene.GameObject> Inno.Scene.GameSystem.Query<T1>() where T1 : Inno.Scene.GameComponent`](../../src/content/scene/Inno.Scene/Core/GameSystem.cs#L144) | Queries game objects containing one required component type.<br>`T1`: The first component type required on every returned game object. | An immutable snapshot of the values selected by the operation. | — |
| <a id="api-f87f83eacf18731a"></a>[`protected System.Collections.Generic.IReadOnlyList<Inno.Scene.GameObject> Inno.Scene.GameSystem.Query<T1, T2>() where T1 : Inno.Scene.GameComponent where T2 : Inno.Scene.GameComponent`](../../src/content/scene/Inno.Scene/Core/GameSystem.cs#L158) | Queries game objects containing two required component types.<br>`T1`: The first component type required on every returned game object.<br>`T2`: The second component type required on every returned game object. | An immutable snapshot of the values selected by the operation. | — |
| <a id="api-9e5695e9408c1782"></a>[`protected System.Collections.Generic.IReadOnlyList<Inno.Scene.GameObject> Inno.Scene.GameSystem.Query<T1, T2, T3>() where T1 : Inno.Scene.GameComponent where T2 : Inno.Scene.GameComponent where T3 : Inno.Scene.GameComponent`](../../src/content/scene/Inno.Scene/Core/GameSystem.cs#L178) | Queries game objects containing three required component types.<br>`T1`: The first component type required on every returned game object.<br>`T2`: The second component type required on every returned game object.<br>`T3`: The third component type required on every returned game object. | An immutable snapshot of the values selected by the operation. | — |
| <a id="api-2ec6eb93b08f51bf"></a>[`protected Inno.Scene.GameSystem.GameSystem()`](../../src/content/scene/Inno.Scene/Core/GameSystem.cs#L13) | Inherited or compiler-provided contract. | — | — |
| <a id="api-a261e179a8be25ca"></a>[`public sealed class Inno.Scene.GameTagCatalog : Inno.Core.Serialization.ISerializable`](../../src/content/scene/Inno.Scene/Core/GameTagCatalog.cs#L16) | Stores the project-wide tag definitions used to author and validate scene object assignments. | — | — |
| <a id="api-b7e01cb15066d27e"></a>[`public const string Inno.Scene.GameTagCatalog.settingProtocolId = "inno.scene.tags"`](../../src/content/scene/Inno.Scene/Core/GameTagCatalog.cs#L21) | Gets the immutable project-setting protocol value used by discovery metadata. | — | — |
| <a id="api-e5bd1eeeb2da478c"></a>[`public static Inno.Core.Settings.ProjectSettingId Inno.Scene.GameTagCatalog.settingId { get; }`](../../src/content/scene/Inno.Scene/Core/GameTagCatalog.cs#L29) | Gets the stable project setting protocol for the project-wide tag catalog. | — | — |
| <a id="api-f78c1475c45e6a81"></a>[`public System.Collections.Generic.IReadOnlyList<string> Inno.Scene.GameTagCatalog.GetTags()`](../../src/content/scene/Inno.Scene/Core/GameTagCatalog.cs#L37) | Gets the defined tags with the immutable default tag first. | An independently owned deterministic tag snapshot. | — |
| <a id="api-22e3f22985e3064b"></a>[`public Inno.Core.Settings.ProjectLocalId Inno.Scene.GameTagCatalog.GetLocalId(string tag)`](../../src/content/scene/Inno.Scene/Core/GameTagCatalog.cs#L52) | Gets the stable project-independent identity of a defined tag.<br>`tag`: The configured tag name. | The deterministic local identity. | — |
| <a id="api-b14254ee7fe60ab8"></a>[`public Inno.Core.Settings.ProjectScopedId Inno.Scene.GameTagCatalog.GetId(Inno.Core.Settings.ProjectId projectId, string tag)`](../../src/content/scene/Inno.Scene/Core/GameTagCatalog.cs#L72) | Gets the complete project-scoped identity of a defined tag.<br>`projectId`: The current project namespace.<br>`tag`: The configured tag name. | The canonical projectId.name identity. | — |
| <a id="api-d4d7e0188ae472d6"></a>[`public bool Inno.Scene.GameTagCatalog.IsDefined(string tag)`](../../src/content/scene/Inno.Scene/Core/GameTagCatalog.cs#L89) | Determines whether an ordinal tag is defined by the project.<br>`tag`: The tag to resolve. | when the normalized tag is defined. | `System.ArgumentException`: Thrown when is empty or invalid. |
| <a id="api-73ae95f41f064f0f"></a>[`public bool Inno.Scene.GameTagCatalog.Add(string tag)`](../../src/content/scene/Inno.Scene/Core/GameTagCatalog.cs#L108) | Adds one normalized project tag definition.<br>`tag`: The unique tag to define. | when a definition was added. | `System.ArgumentException`: Thrown when is empty or invalid. |
| <a id="api-0ab578893c030ff6"></a>[`public bool Inno.Scene.GameTagCatalog.Remove(string tag)`](../../src/content/scene/Inno.Scene/Core/GameTagCatalog.cs#L134) | Removes one custom project tag definition without rewriting scene assignments.<br>`tag`: The custom tag to remove. | when an existing definition was removed. | `System.ArgumentException`: Thrown when is empty or invalid. |
| <a id="api-954a1f32cc4f4acc"></a>[`public Inno.Scene.GameTagCatalog Inno.Scene.GameTagCatalog.Clone()`](../../src/content/scene/Inno.Scene/Core/GameTagCatalog.cs#L155) | Creates a detached mutable copy of this catalog. | An independently owned catalog with the same definitions. | — |
| <a id="api-e5d4dc4ec853b47b"></a>[`public Inno.Scene.GameTagCatalog.GameTagCatalog()`](../../src/content/scene/Inno.Scene/Core/GameTagCatalog.cs#L16) | Inherited or compiler-provided contract. | — | — |
| <a id="api-03d76f7e3826e994"></a>[`public readonly struct Inno.Scene.Layers.GameLayer : System.IEquatable<Inno.Scene.Layers.GameLayer>, System.IComparable<Inno.Scene.Layers.GameLayer>`](../../src/content/scene/Inno.Scene/Layers/GameLayer.cs#L8) | Identifies one of the thirty-two runtime layers available to scene objects. | — | — |
| <a id="api-f91e98d8d15c4e97"></a>[`public const int Inno.Scene.Layers.GameLayer.C_MAX_COUNT = 32`](../../src/content/scene/Inno.Scene/Layers/GameLayer.cs#L13) | Defines the number of layer slots supported by the runtime bit-mask representation. | — | — |
| <a id="api-78343193c33b0220"></a>[`public static Inno.Scene.Layers.GameLayer Inno.Scene.Layers.GameLayer.defaultLayer { get; }`](../../src/content/scene/Inno.Scene/Layers/GameLayer.cs#L18) | Gets the built-in default layer stored in slot zero. | — | — |
| <a id="api-3208d85aeb320f37"></a>[`public Inno.Scene.Layers.GameLayer.GameLayer(int index)`](../../src/content/scene/Inno.Scene/Layers/GameLayer.cs#L29) | Creates a layer identifier from a zero-based layer index.<br>`index`: The layer index in the inclusive range from zero through thirty-one. | — | `System.ArgumentOutOfRangeException`: Thrown when is outside the supported range. |
| <a id="api-070d1fa97604a477"></a>[`public int Inno.Scene.Layers.GameLayer.index { get; }`](../../src/content/scene/Inno.Scene/Layers/GameLayer.cs#L39) | Gets the zero-based layer index. | — | — |
| <a id="api-87ab6dc98f3e41af"></a>[`public int Inno.Scene.Layers.GameLayer.CompareTo(Inno.Scene.Layers.GameLayer other)`](../../src/content/scene/Inno.Scene/Layers/GameLayer.cs#L50) | Compares this layer with another layer by index.<br>`other`: The value to compare with this instance. | A signed value describing the relative index ordering. | — |
| <a id="api-46816a8a6847691a"></a>[`public bool Inno.Scene.Layers.GameLayer.Equals(Inno.Scene.Layers.GameLayer other)`](../../src/content/scene/Inno.Scene/Layers/GameLayer.cs#L61) | Determines whether another layer identifies the same index.<br>`other`: The layer to compare with this value. | when both layers identify the same index. | — |
| <a id="api-494b46a3ded30056"></a>[`public override bool Inno.Scene.Layers.GameLayer.Equals(object? obj)`](../../src/content/scene/Inno.Scene/Layers/GameLayer.cs#L72) | Determines whether this instance and the supplied value represent the same logical state.<br>`obj`: The object to compare with this instance. | when both values represent the same logical state; otherwise, . | — |
| <a id="api-88c34fe5acd8de57"></a>[`public override int Inno.Scene.Layers.GameLayer.GetHashCode()`](../../src/content/scene/Inno.Scene/Layers/GameLayer.cs#L80) | Computes a hash code from the fields that participate in logical equality. | A hash code consistent with the implemented equality contract. | — |
| <a id="api-05ed070821267fb1"></a>[`public override string Inno.Scene.Layers.GameLayer.ToString()`](../../src/content/scene/Inno.Scene/Layers/GameLayer.cs#L88) | Formats this value as a human-readable representation. | The human-readable representation of this value. | — |
| <a id="api-095c145c584515f1"></a>[`public static bool Inno.Scene.Layers.GameLayer.operator ==(Inno.Scene.Layers.GameLayer left, Inno.Scene.Layers.GameLayer right)`](../../src/content/scene/Inno.Scene/Layers/GameLayer.cs#L102) | Determines whether two layer identifiers contain the same index.<br>`left`: The first layer identifier.<br>`right`: The second layer identifier. | when the identifiers are equal. | — |
| <a id="api-c8c26751f50b8c5e"></a>[`public static bool Inno.Scene.Layers.GameLayer.operator !=(Inno.Scene.Layers.GameLayer left, Inno.Scene.Layers.GameLayer right)`](../../src/content/scene/Inno.Scene/Layers/GameLayer.cs#L119) | Determines whether two layer identifiers contain different indices.<br>`left`: The first layer identifier.<br>`right`: The second layer identifier. | when the identifiers are different. | — |
| <a id="api-3acd990876f01d6c"></a>[`public sealed class Inno.Scene.Layers.GameLayerCatalog : Inno.Core.Serialization.ISerializable`](../../src/content/scene/Inno.Scene/Layers/GameLayerCatalog.cs#L18) | Stores the named layer catalog and symmetric layer-interaction matrix used by a project. | — | — |
| <a id="api-c1807a3e5bfc57a0"></a>[`public const string Inno.Scene.Layers.GameLayerCatalog.settingProtocolId = "inno.scene.layers"`](../../src/content/scene/Inno.Scene/Layers/GameLayerCatalog.cs#L23) | Gets the immutable project-setting protocol value used by discovery metadata. | — | — |
| <a id="api-24f63b28876fe808"></a>[`public static Inno.Core.Settings.ProjectSettingId Inno.Scene.Layers.GameLayerCatalog.settingId { get; }`](../../src/content/scene/Inno.Scene/Layers/GameLayerCatalog.cs#L39) | Gets the stable project setting protocol for the project-wide layer catalog. | — | — |
| <a id="api-a5352d60291fbb81"></a>[`public Inno.Scene.Layers.GameLayerCatalog.GameLayerCatalog()`](../../src/content/scene/Inno.Scene/Layers/GameLayerCatalog.cs#L44) | Creates a layer stack containing the immutable default layer. | — | — |
| <a id="api-77846ad4fa987b57"></a>[`public int Inno.Scene.Layers.GameLayerCatalog.count { get; }`](../../src/content/scene/Inno.Scene/Layers/GameLayerCatalog.cs#L56) | Gets the number of currently named layer slots. | — | — |
| <a id="api-382ff5eb308c143a"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Scene.Layers.GameLayerDefinition> Inno.Scene.Layers.GameLayerCatalog.GetDefinitions()`](../../src/content/scene/Inno.Scene/Layers/GameLayerCatalog.cs#L71) | Gets an immutable snapshot of every named layer ordered by slot index. | The ordered layer-definition snapshot. | — |
| <a id="api-ec45fdeed16f67e9"></a>[`public bool Inno.Scene.Layers.GameLayerCatalog.IsDefined(Inno.Scene.Layers.GameLayer layer)`](../../src/content/scene/Inno.Scene/Layers/GameLayerCatalog.cs#L97) | Determines whether a layer slot is defined.<br>`layer`: The layer slot to test. | when the slot is defined. | — |
| <a id="api-9e08dd07a4fd473d"></a>[`public string? Inno.Scene.Layers.GameLayerCatalog.GetName(Inno.Scene.Layers.GameLayer layer)`](../../src/content/scene/Inno.Scene/Layers/GameLayerCatalog.cs#L112) | Gets the display name assigned to a layer slot.<br>`layer`: The layer slot to resolve. | The configured name, or . | — |
| <a id="api-aa69274f5719ade4"></a>[`public Inno.Core.Settings.ProjectLocalId? Inno.Scene.Layers.GameLayerCatalog.GetLocalId(Inno.Scene.Layers.GameLayer layer)`](../../src/content/scene/Inno.Scene/Layers/GameLayerCatalog.cs#L127) | Gets the stable project-independent identity assigned to a layer slot.<br>`layer`: The layer slot to resolve. | The local identity, or . | — |
| <a id="api-728e12988c3d5ece"></a>[`public Inno.Scene.Layers.GameLayerId? Inno.Scene.Layers.GameLayerCatalog.GetId(Inno.Core.Settings.ProjectId projectId, Inno.Scene.Layers.GameLayer layer)`](../../src/content/scene/Inno.Scene/Layers/GameLayerCatalog.cs#L146) | Gets the complete identity assigned to a layer slot.<br>`projectId`: The current project namespace.<br>`layer`: The layer slot to resolve. | The qualified identity, or . | — |
| <a id="api-1938a8f683b8744c"></a>[`public bool Inno.Scene.Layers.GameLayerCatalog.TryGetLayer(Inno.Core.Settings.ProjectLocalId localId, out Inno.Scene.Layers.GameLayer layer)`](../../src/content/scene/Inno.Scene/Layers/GameLayerCatalog.cs#L166) | Tries to resolve a local identity to its compact runtime slot.<br>`localId`: The stable project-local identity.<br>`layer`: The resolved runtime slot. | when the identity is defined. | — |
| <a id="api-13dd1ebebb80d8b4"></a>[`public bool Inno.Scene.Layers.GameLayerCatalog.TryGetLayer(Inno.Core.Settings.ProjectId projectId, Inno.Scene.Layers.GameLayerId id, out Inno.Scene.Layers.GameLayer layer)`](../../src/content/scene/Inno.Scene/Layers/GameLayerCatalog.cs#L199) | Tries to resolve a qualified identity to its compact runtime slot.<br>`projectId`: The current project namespace.<br>`id`: The complete layer identity.<br>`layer`: The resolved runtime slot. | when the identity belongs to this project and is defined. | — |
| <a id="api-45cbea1382ecc7fe"></a>[`public bool Inno.Scene.Layers.GameLayerCatalog.TryGetLayer(string name, out Inno.Scene.Layers.GameLayer layer)`](../../src/content/scene/Inno.Scene/Layers/GameLayerCatalog.cs#L233) | Tries to resolve an ordinal layer name to its compact slot.<br>`name`: The layer name to find.<br>`layer`: The resolved layer. | when a matching layer exists. | — |
| <a id="api-6b6f479d3058bbfc"></a>[`public Inno.Scene.Layers.GameLayer Inno.Scene.Layers.GameLayerCatalog.GetLayer(string name)`](../../src/content/scene/Inno.Scene/Layers/GameLayerCatalog.cs#L259) | Resolves an ordinal layer name to its compact slot.<br>`name`: The configured layer name. | The matching runtime layer. | — |
| <a id="api-316ff29a6e6a7533"></a>[`public Inno.Scene.Layers.GameLayerMask Inno.Scene.Layers.GameLayerCatalog.GetMask(System.Collections.Generic.IEnumerable<string> names)`](../../src/content/scene/Inno.Scene/Layers/GameLayerCatalog.cs#L275) | Creates a mask from configured ordinal layer names.<br>`names`: The configured names to include. | A mask containing every resolved layer. | — |
| <a id="api-e1da6b152d4ffe6f"></a>[`public void Inno.Scene.Layers.GameLayerCatalog.Define(Inno.Scene.Layers.GameLayer layer, string name)`](../../src/content/scene/Inno.Scene/Layers/GameLayerCatalog.cs#L293) | Defines or renames a project layer without accepting an authored ID.<br>`layer`: The compact runtime slot.<br>`name`: The unique display name. | — | — |
| <a id="api-db88313dbfb6b7fd"></a>[`public bool Inno.Scene.Layers.GameLayerCatalog.Remove(Inno.Scene.Layers.GameLayer layer)`](../../src/content/scene/Inno.Scene/Layers/GameLayerCatalog.cs#L347) | Removes a custom layer definition while retaining its numeric slot and interactions.<br>`layer`: The custom layer slot to undefine. | when a definition was removed. | — |
| <a id="api-b812095ed0967771"></a>[`public Inno.Scene.Layers.GameLayerMask Inno.Scene.Layers.GameLayerCatalog.GetInteractionMask(Inno.Scene.Layers.GameLayer layer)`](../../src/content/scene/Inno.Scene/Layers/GameLayerCatalog.cs#L368) | Gets the interaction mask assigned to one layer.<br>`layer`: The source layer. | The configured interaction mask. | — |
| <a id="api-651e64917367eaf0"></a>[`public bool Inno.Scene.Layers.GameLayerCatalog.CanInteract(Inno.Scene.Layers.GameLayer first, Inno.Scene.Layers.GameLayer second)`](../../src/content/scene/Inno.Scene/Layers/GameLayerCatalog.cs#L386) | Determines whether two layer slots may interact.<br>`first`: The first layer.<br>`second`: The second layer. | when interaction is enabled. | — |
| <a id="api-f46a2d652833fc94"></a>[`public void Inno.Scene.Layers.GameLayerCatalog.SetInteraction(Inno.Scene.Layers.GameLayer first, Inno.Scene.Layers.GameLayer second, bool canInteract)`](../../src/content/scene/Inno.Scene/Layers/GameLayerCatalog.cs#L406) | Sets a symmetric interaction pair.<br>`first`: The first layer.<br>`second`: The second layer.<br>`canInteract`: Whether interaction is enabled. | — | — |
| <a id="api-2f007a978859c10d"></a>[`public Inno.Scene.Layers.GameLayerCatalog Inno.Scene.Layers.GameLayerCatalog.Clone()`](../../src/content/scene/Inno.Scene/Layers/GameLayerCatalog.cs#L432) | Creates a detached copy of this stack. | An independent layer stack. | — |
| <a id="api-ef9506fc8b779ab0"></a>[`public sealed class Inno.Scene.Layers.GameLayerDefinition`](../../src/content/scene/Inno.Scene/Layers/GameLayerDefinition.cs#L10) | Describes one named layer slot in a snapshot. | — | — |
| <a id="api-514c44ec6d896607"></a>[`public Inno.Scene.Layers.GameLayerDefinition.GameLayerDefinition(Inno.Core.Settings.ProjectLocalId localId, Inno.Scene.Layers.GameLayer layer, string name)`](../../src/content/scene/Inno.Scene/Layers/GameLayerDefinition.cs#L24) | Creates an immutable layer definition.<br>`localId`: The stable project-independent identity.<br>`layer`: The layer slot represented by the definition.<br>`name`: The non-empty display and lookup name. | — | — |
| <a id="api-98f47107bc38fc58"></a>[`public Inno.Core.Settings.ProjectLocalId Inno.Scene.Layers.GameLayerDefinition.localId { get; }`](../../src/content/scene/Inno.Scene/Layers/GameLayerDefinition.cs#L39) | Gets the stable project-independent identity. | — | — |
| <a id="api-5a22082e93751e7b"></a>[`public Inno.Scene.Layers.GameLayer Inno.Scene.Layers.GameLayerDefinition.layer { get; }`](../../src/content/scene/Inno.Scene/Layers/GameLayerDefinition.cs#L44) | Gets the layer slot represented by this definition. | — | — |
| <a id="api-9ed1e82339318029"></a>[`public string Inno.Scene.Layers.GameLayerDefinition.name { get; }`](../../src/content/scene/Inno.Scene/Layers/GameLayerDefinition.cs#L49) | Gets the unique ordinal layer name. | — | — |
| <a id="api-63b389c59070cfda"></a>[`public Inno.Scene.Layers.GameLayerId Inno.Scene.Layers.GameLayerDefinition.GetId(Inno.Core.Settings.ProjectId projectId)`](../../src/content/scene/Inno.Scene/Layers/GameLayerDefinition.cs#L60) | Resolves the complete identity under a project namespace.<br>`projectId`: The current project namespace. | The canonical project-scoped layer identity. | — |
| <a id="api-d8b95516fabb359e"></a>[`public readonly record struct Inno.Scene.Layers.GameLayerId : System.IEquatable<Inno.Scene.Layers.GameLayerId>`](../../src/content/scene/Inno.Scene/Layers/GameLayerId.cs#L10) | Identifies one project-scoped game layer independently from its compact runtime slot. | — | — |
| <a id="api-f033ed62e96272d2"></a>[`public Inno.Scene.Layers.GameLayerId.GameLayerId(Inno.Core.Settings.ProjectScopedId value)`](../../src/content/scene/Inno.Scene/Layers/GameLayerId.cs#L18) | Creates a project-scoped layer identity.<br>`value`: The canonical qualified identity. | — | — |
| <a id="api-743cd755c9e056fc"></a>[`public Inno.Scene.Layers.GameLayerId.GameLayerId(Inno.Core.Settings.ProjectId projectId, Inno.Core.Settings.ProjectLocalId name)`](../../src/content/scene/Inno.Scene/Layers/GameLayerId.cs#L34) | Creates a project-scoped layer identity from its project and local parts.<br>`projectId`: The current project identity.<br>`name`: The stable project-local layer identity. | — | — |
| <a id="api-9882f58bb8d08d90"></a>[`public string Inno.Scene.Layers.GameLayerId.value { get; }`](../../src/content/scene/Inno.Scene/Layers/GameLayerId.cs#L63) | Gets the canonical projectId.name identity string. | — | — |
| <a id="api-22ad9926f0319df6"></a>[`public bool Inno.Scene.Layers.GameLayerId.isValid { get; }`](../../src/content/scene/Inno.Scene/Layers/GameLayerId.cs#L68) | Gets whether this value contains a usable identity. | — | — |
| <a id="api-8573f38a18b7ba45"></a>[`public override string Inno.Scene.Layers.GameLayerId.ToString()`](../../src/content/scene/Inno.Scene/Layers/GameLayerId.cs#L76) | Formats this value as a human-readable representation. | The qualified identity. | — |
| <a id="api-f561b0c4f1d1a48f"></a>[`public static bool Inno.Scene.Layers.GameLayerId.operator !=(Inno.Scene.Layers.GameLayerId left, Inno.Scene.Layers.GameLayerId right)`](../../src/content/scene/Inno.Scene/Layers/GameLayerId.cs#L10) | Inherited or compiler-provided contract. | — | — |
| <a id="api-d40ed447c552c00f"></a>[`public static bool Inno.Scene.Layers.GameLayerId.operator ==(Inno.Scene.Layers.GameLayerId left, Inno.Scene.Layers.GameLayerId right)`](../../src/content/scene/Inno.Scene/Layers/GameLayerId.cs#L10) | Inherited or compiler-provided contract. | — | — |
| <a id="api-ea3382194dcf8253"></a>[`public override int Inno.Scene.Layers.GameLayerId.GetHashCode()`](../../src/content/scene/Inno.Scene/Layers/GameLayerId.cs#L10) | Returns the hash code for this instance. | A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-484f26b3cbc549c5"></a>[`public override bool Inno.Scene.Layers.GameLayerId.Equals(object obj)`](../../src/content/scene/Inno.Scene/Layers/GameLayerId.cs#L10) | Indicates whether this instance and a specified object are equal.<br>`obj`: The object to compare with the current instance. | if and this instance are the same type and represent the same value; otherwise, . | — |
| <a id="api-877d6d15fefa8b2a"></a>[`public bool Inno.Scene.Layers.GameLayerId.Equals(Inno.Scene.Layers.GameLayerId other)`](../../src/content/scene/Inno.Scene/Layers/GameLayerId.cs#L10) | Indicates whether the current object is equal to another object of the same type.<br>`other`: An object to compare with this object. | if the current object is equal to the parameter; otherwise, . | — |
| <a id="api-b09093b6b1884c7c"></a>[`public readonly struct Inno.Scene.Layers.GameLayerMask : System.IEquatable<Inno.Scene.Layers.GameLayerMask>`](../../src/content/scene/Inno.Scene/Layers/GameLayerMask.cs#L9) | Stores a compact set of scene layers for rendering, physics, and query filtering. | — | — |
| <a id="api-8d57cca2f1215154"></a>[`public static Inno.Scene.Layers.GameLayerMask Inno.Scene.Layers.GameLayerMask.none { get; }`](../../src/content/scene/Inno.Scene/Layers/GameLayerMask.cs#L14) | Gets a mask containing no layers. | — | — |
| <a id="api-aa70a30c136760d7"></a>[`public static Inno.Scene.Layers.GameLayerMask Inno.Scene.Layers.GameLayerMask.everything { get; }`](../../src/content/scene/Inno.Scene/Layers/GameLayerMask.cs#L19) | Gets a mask containing every supported layer. | — | — |
| <a id="api-749e7df94527d245"></a>[`public Inno.Scene.Layers.GameLayerMask.GameLayerMask(uint value)`](../../src/content/scene/Inno.Scene/Layers/GameLayerMask.cs#L27) | Creates a mask from its raw thirty-two-bit representation.<br>`value`: The raw mask bits. | — | — |
| <a id="api-ca1151b3b8aba571"></a>[`public uint Inno.Scene.Layers.GameLayerMask.value { get; }`](../../src/content/scene/Inno.Scene/Layers/GameLayerMask.cs#L35) | Gets the raw thirty-two-bit mask value. | — | — |
| <a id="api-febda5e4931b5ef1"></a>[`public static Inno.Scene.Layers.GameLayerMask Inno.Scene.Layers.GameLayerMask.FromLayers(System.Collections.Generic.IEnumerable<Inno.Scene.Layers.GameLayer> layers)`](../../src/content/scene/Inno.Scene/Layers/GameLayerMask.cs#L49) | Creates a mask containing the supplied layers.<br>`layers`: The layers whose bits should be enabled. | A mask containing every supplied layer. | `System.ArgumentNullException`: Thrown when is . |
| <a id="api-f832dec1a098d72c"></a>[`public bool Inno.Scene.Layers.GameLayerMask.Contains(Inno.Scene.Layers.GameLayer layer)`](../../src/content/scene/Inno.Scene/Layers/GameLayerMask.cs#L67) | Determines whether the supplied layer is contained in this mask.<br>`layer`: The layer consumed by contains; ownership remains with the caller unless explicitly stated otherwise. | when the layer bit is enabled. | — |
| <a id="api-460ae2b2bd7313df"></a>[`public Inno.Scene.Layers.GameLayerMask Inno.Scene.Layers.GameLayerMask.With(Inno.Scene.Layers.GameLayer layer)`](../../src/content/scene/Inno.Scene/Layers/GameLayerMask.cs#L78) | Returns a mask with the supplied layer enabled.<br>`layer`: The layer consumed by with; ownership remains with the caller unless explicitly stated otherwise. | The updated immutable mask. | — |
| <a id="api-737a872e1cf8859e"></a>[`public Inno.Scene.Layers.GameLayerMask Inno.Scene.Layers.GameLayerMask.Without(Inno.Scene.Layers.GameLayer layer)`](../../src/content/scene/Inno.Scene/Layers/GameLayerMask.cs#L89) | Returns a mask with the supplied layer disabled.<br>`layer`: The layer consumed by without; ownership remains with the caller unless explicitly stated otherwise. | The updated immutable mask. | — |
| <a id="api-03db392b0dd9b99c"></a>[`public bool Inno.Scene.Layers.GameLayerMask.Equals(Inno.Scene.Layers.GameLayerMask other)`](../../src/content/scene/Inno.Scene/Layers/GameLayerMask.cs#L100) | Determines whether another mask contains the same layer bits.<br>`other`: The mask to compare with this value. | when both masks contain the same bits. | — |
| <a id="api-4f7ca7593f27ec5c"></a>[`public override bool Inno.Scene.Layers.GameLayerMask.Equals(object? obj)`](../../src/content/scene/Inno.Scene/Layers/GameLayerMask.cs#L111) | Determines whether this instance and the supplied value represent the same logical state.<br>`obj`: The object to compare with this instance. | when both values represent the same logical state; otherwise, . | — |
| <a id="api-8fd131bc11a2c151"></a>[`public override int Inno.Scene.Layers.GameLayerMask.GetHashCode()`](../../src/content/scene/Inno.Scene/Layers/GameLayerMask.cs#L119) | Computes a hash code from the fields that participate in logical equality. | A hash code consistent with the implemented equality contract. | — |
| <a id="api-0e37b5f5f259625a"></a>[`public override string Inno.Scene.Layers.GameLayerMask.ToString()`](../../src/content/scene/Inno.Scene/Layers/GameLayerMask.cs#L127) | Formats this value as a human-readable representation. | The human-readable representation of this value. | — |
| <a id="api-22b6a9e41f45aa40"></a>[`public static Inno.Scene.Layers.GameLayerMask Inno.Scene.Layers.GameLayerMask.operator \|(Inno.Scene.Layers.GameLayerMask left, Inno.Scene.Layers.GameLayerMask right)`](../../src/content/scene/Inno.Scene/Layers/GameLayerMask.cs#L141) | Combines the enabled bits from two masks.<br>`left`: The first mask.<br>`right`: The second mask. | The bitwise union of both masks. | — |
| <a id="api-3608e156acb3bc7a"></a>[`public static Inno.Scene.Layers.GameLayerMask Inno.Scene.Layers.GameLayerMask.operator &(Inno.Scene.Layers.GameLayerMask left, Inno.Scene.Layers.GameLayerMask right)`](../../src/content/scene/Inno.Scene/Layers/GameLayerMask.cs#L158) | Retains only layer bits enabled in both masks.<br>`left`: The first mask.<br>`right`: The second mask. | The bitwise intersection of both masks. | — |
| <a id="api-4343aa8f0117c767"></a>[`public static Inno.Scene.Layers.GameLayerMask Inno.Scene.Layers.GameLayerMask.operator ~(Inno.Scene.Layers.GameLayerMask mask)`](../../src/content/scene/Inno.Scene/Layers/GameLayerMask.cs#L172) | Inverts every layer bit in a mask.<br>`mask`: The mask to invert. | The complement of the supplied mask. | — |
| <a id="api-de6a099b82ebb583"></a>[`public static bool Inno.Scene.Layers.GameLayerMask.operator ==(Inno.Scene.Layers.GameLayerMask left, Inno.Scene.Layers.GameLayerMask right)`](../../src/content/scene/Inno.Scene/Layers/GameLayerMask.cs#L186) | Determines whether two masks contain the same bits.<br>`left`: The first mask.<br>`right`: The second mask. | when both masks are equal. | — |
| <a id="api-e7a6157d9604a159"></a>[`public static bool Inno.Scene.Layers.GameLayerMask.operator !=(Inno.Scene.Layers.GameLayerMask left, Inno.Scene.Layers.GameLayerMask right)`](../../src/content/scene/Inno.Scene/Layers/GameLayerMask.cs#L203) | Determines whether two masks contain different bits.<br>`left`: The first mask.<br>`right`: The second mask. | when both masks differ. | — |
| <a id="api-0e68a1242d012171"></a>[`public sealed class Inno.Scene.PrefabInstanceInfo`](../../src/content/scene/Inno.Scene/Prefabs/PrefabInstanceInfo.cs#L10) | Describes the read-only source connection retained by one prefab instance object. | — | — |
| <a id="api-80f95460b0b1470b"></a>[`public System.Guid Inno.Scene.PrefabInstanceInfo.sourceAssetId { get; }`](../../src/content/scene/Inno.Scene/Prefabs/PrefabInstanceInfo.cs#L37) | Gets the persistent identity of the source prefab asset. | — | — |
| <a id="api-79bf28fdf59bbd81"></a>[`public System.Guid Inno.Scene.PrefabInstanceInfo.sourceObjectId { get; }`](../../src/content/scene/Inno.Scene/Prefabs/PrefabInstanceInfo.cs#L42) | Gets the source-local identity represented by this object. | — | — |
| <a id="api-4efa8e7561974cfa"></a>[`public Inno.Scene.GameObject Inno.Scene.PrefabInstanceInfo.instanceRoot { get; }`](../../src/content/scene/Inno.Scene/Prefabs/PrefabInstanceInfo.cs#L47) | Gets the root object of this prefab instance connection. | — | — |
| <a id="api-c61de9712b11e961"></a>[`public bool Inno.Scene.PrefabInstanceInfo.isRoot { get; }`](../../src/content/scene/Inno.Scene/Prefabs/PrefabInstanceInfo.cs#L52) | Gets whether this object is the instance connection root. | — | — |
| <a id="api-c6e1adfd330707af"></a>[`public bool Inno.Scene.PrefabInstanceInfo.isVariant { get; }`](../../src/content/scene/Inno.Scene/Prefabs/PrefabInstanceInfo.cs#L57) | Gets whether the connection originates from a prefab variant. | — | — |
| <a id="api-5c83e601d4a17d7c"></a>[`public bool Inno.Scene.PrefabInstanceInfo.isMissing { get; }`](../../src/content/scene/Inno.Scene/Prefabs/PrefabInstanceInfo.cs#L62) | Gets whether the source prefab was unavailable during restoration. | — | — |
| <a id="api-393cf1d273ea7b39"></a>[`public int Inno.Scene.PrefabInstanceInfo.overrideCount { get; }`](../../src/content/scene/Inno.Scene/Prefabs/PrefabInstanceInfo.cs#L67) | Gets the number of retained overrides for this instance connection. | — | — |
| <a id="api-2fd70fe2d51ddeb5"></a>[`public int Inno.Scene.PrefabInstanceInfo.orphanedOverrideCount { get; }`](../../src/content/scene/Inno.Scene/Prefabs/PrefabInstanceInfo.cs#L72) | Gets the number of retained overrides that no longer match the current source. | — | — |
| <a id="api-da8cefed09569e70"></a>[`public interface Inno.Scene.ISceneReloadStateTransfer`](../../src/content/scene/Inno.Scene/Reloading/ISceneReloadStateTransfer.cs#L9) | Represents a staged scene object state transfer associated with one assembly reload transaction. | — | — |
| <a id="api-2ea9a386cfa5979e"></a>[`System.Collections.Generic.IReadOnlyList<object> Inno.Scene.ISceneReloadStateTransfer.retiredObjects { get; }`](../../src/content/scene/Inno.Scene/Reloading/ISceneReloadStateTransfer.cs#L14) | Gets old scene objects whose runtime types belong to the retiring assembly generation. | — | — |
| <a id="api-e4a60525aa55f668"></a>[`System.Collections.Generic.IReadOnlyList<Inno.Scene.SceneReloadDiagnostic> Inno.Scene.ISceneReloadStateTransfer.diagnostics { get; }`](../../src/content/scene/Inno.Scene/Reloading/ISceneReloadStateTransfer.cs#L19) | Gets non-fatal state transfer decisions produced while replacing reloadable scene objects. | — | — |
| <a id="api-f80bff751bc0d3ab"></a>[`System.Collections.Generic.IReadOnlyList<Inno.References.ReferenceRecoveryChange> Inno.Scene.ISceneReloadStateTransfer.recoveryChanges { get; }`](../../src/content/scene/Inno.Scene/Reloading/ISceneReloadStateTransfer.cs#L24) | Gets the shared candidate resolutions for actual scene element and asset dependency slots after application. | — | — |
| <a id="api-ee85618e130e7768"></a>[`void Inno.Scene.ISceneReloadStateTransfer.PrepareForActivation()`](../../src/content/scene/Inno.Scene/Reloading/ISceneReloadStateTransfer.cs#L29) | Disables active retiring lifecycle objects before the new assembly generation becomes active. | — | — |
| <a id="api-3955d702e1757396"></a>[`void Inno.Scene.ISceneReloadStateTransfer.Apply()`](../../src/content/scene/Inno.Scene/Reloading/ISceneReloadStateTransfer.cs#L34) | Creates replacement instances and restores their serialized state. | — | — |
| <a id="api-8ca76ac6a7d7d02f"></a>[`void Inno.Scene.ISceneReloadStateTransfer.RollbackStructure()`](../../src/content/scene/Inno.Scene/Reloading/ISceneReloadStateTransfer.cs#L39) | Restores the previous scene structure after a failed state transfer. | — | — |
| <a id="api-2f519000ebf9d24f"></a>[`void Inno.Scene.ISceneReloadStateTransfer.RestorePreviousState()`](../../src/content/scene/Inno.Scene/Reloading/ISceneReloadStateTransfer.cs#L44) | Restores lifecycle state on the previous scene instances after rollback. | — | — |
| <a id="api-8610481086d5e640"></a>[`void Inno.Scene.ISceneReloadStateTransfer.Complete()`](../../src/content/scene/Inno.Scene/Reloading/ISceneReloadStateTransfer.cs#L49) | Finalizes replacement instances after the assembly reload is committed. | — | — |
| <a id="api-56f92d2d774c00a5"></a>[`public sealed class Inno.Scene.SceneReloadDiagnostic`](../../src/content/scene/Inno.Scene/Reloading/SceneReloadDiagnostic.cs#L9) | Describes a non-fatal issue encountered while migrating scene state to a new assembly generation. | — | — |
| <a id="api-5dcfd320c28822fa"></a>[`public string Inno.Scene.SceneReloadDiagnostic.code { get; }`](../../src/content/scene/Inno.Scene/Reloading/SceneReloadDiagnostic.cs#L34) | Gets the stable diagnostic code. | — | — |
| <a id="api-511cbba7b315c7c7"></a>[`public Inno.Core.Diagnostics.DiagnosticSeverity Inno.Scene.SceneReloadDiagnostic.severity { get; }`](../../src/content/scene/Inno.Scene/Reloading/SceneReloadDiagnostic.cs#L39) | Gets the diagnostic severity. | — | — |
| <a id="api-8516903cf2d955d2"></a>[`public string Inno.Scene.SceneReloadDiagnostic.message { get; }`](../../src/content/scene/Inno.Scene/Reloading/SceneReloadDiagnostic.cs#L44) | Gets the human-readable diagnostic message. | — | — |
| <a id="api-4dd5d961d42909c2"></a>[`public System.Guid Inno.Scene.SceneReloadDiagnostic.scenePersistentId { get; }`](../../src/content/scene/Inno.Scene/Reloading/SceneReloadDiagnostic.cs#L49) | Gets the persistent identity of the affected scene. | — | — |
| <a id="api-731998518ae2eee7"></a>[`public System.Guid Inno.Scene.SceneReloadDiagnostic.objectPersistentId { get; }`](../../src/content/scene/Inno.Scene/Reloading/SceneReloadDiagnostic.cs#L54) | Gets the persistent identity of the affected component or system. | — | — |
| <a id="api-638f765104519da3"></a>[`public string Inno.Scene.SceneReloadDiagnostic.propertyName { get; }`](../../src/content/scene/Inno.Scene/Reloading/SceneReloadDiagnostic.cs#L59) | Gets the incompatible serialized property name. | — | — |
| <a id="api-90d40f7a0a28b984"></a>[`public string Inno.Scene.SceneReloadDiagnostic.previousPropertyType { get; }`](../../src/content/scene/Inno.Scene/Reloading/SceneReloadDiagnostic.cs#L64) | Gets the previous declared property type name. | — | — |
| <a id="api-13e8822d3c3fb645"></a>[`public string Inno.Scene.SceneReloadDiagnostic.currentPropertyType { get; }`](../../src/content/scene/Inno.Scene/Reloading/SceneReloadDiagnostic.cs#L69) | Gets the current declared property type name. | — | — |
| <a id="api-269fe7aca5134be6"></a>[`public sealed class Inno.Scene.SceneReloadService`](../../src/content/scene/Inno.Scene/Reloading/SceneReloadService.cs#L12) | Creates scene state-transfer transactions for assembly generation changes. | — | — |
| <a id="api-1c759f6295a49151"></a>[`public Inno.Scene.SceneReloadService.SceneReloadService(Inno.Scene.SceneWorld world, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Assets.IAssetReferenceResolver assets)`](../../src/content/scene/Inno.Scene/Reloading/SceneReloadService.cs#L34) | Creates a scene reload service bound to one serialization generation owner.<br>`serialization`: The serialization registry used to capture and restore scene state.<br>`world`: The isolated scene world whose active generation is captured.<br>`assets`: The asset reference generation used throughout capture, activation, and rollback. | — | `System.ArgumentNullException`: Thrown when , , or is null. |
| <a id="api-3e39b725064fa5d8"></a>[`public Inno.Scene.ISceneReloadStateTransfer Inno.Scene.SceneReloadService.Capture(Inno.Extensibility.Types.TypeCacheReloadContext context)`](../../src/content/scene/Inno.Scene/Reloading/SceneReloadService.cs#L53) | Captures all loaded scene objects affected by a prepared type-cache reload.<br>`context`: The prepared type-cache reload context. | A staged state transfer that has not modified live objects. | — |
| <a id="api-c70db86a68771ffe"></a>[`public static class Inno.Scene.SceneManager`](../../src/content/scene/Inno.Scene/SceneManager.cs#L13) | Provides Unity-style scene operations by resolving the world in the current runtime execution context. | — | — |
| <a id="api-3cf40579d411015c"></a>[`public static Inno.Scene.GameScene? Inno.Scene.SceneManager.activeScene { get; }`](../../src/content/scene/Inno.Scene/SceneManager.cs#L18) | Gets the active scene in the current runtime session. | — | — |
| <a id="api-91c69d8995a4be7d"></a>[`public static bool Inno.Scene.SceneManager.hasActiveScene { get; }`](../../src/content/scene/Inno.Scene/SceneManager.cs#L23) | Gets whether the current runtime session has an active scene. | — | — |
| <a id="api-54143757a30e24e3"></a>[`public static System.Collections.Generic.IReadOnlyList<Inno.Scene.GameScene> Inno.Scene.SceneManager.loadedScenes { get; }`](../../src/content/scene/Inno.Scene/SceneManager.cs#L28) | Gets an immutable snapshot of scenes loaded by the current runtime session. | — | — |
| <a id="api-4daba9e650e6afdf"></a>[`public static int Inno.Scene.SceneManager.GetSceneIndex(Inno.Scene.GameScene scene)`](../../src/content/scene/Inno.Scene/SceneManager.cs#L39) | Gets the hierarchy index of a scene loaded by the current runtime session.<br>`scene`: The loaded scene to locate. | The zero-based hierarchy index. | — |
| <a id="api-64e557fcd035a0d4"></a>[`public static void Inno.Scene.SceneManager.SetSceneIndex(Inno.Scene.GameScene scene, int sceneIndex)`](../../src/content/scene/Inno.Scene/SceneManager.cs#L50) | Moves a loaded scene to a hierarchy index without changing the active scene.<br>`scene`: The loaded scene to move.<br>`sceneIndex`: The requested zero-based hierarchy index. | — | — |
| <a id="api-bdb1420f40987aeb"></a>[`public static void Inno.Scene.SceneManager.LoadScene(Inno.Scene.GameScene scene)`](../../src/content/scene/Inno.Scene/SceneManager.cs#L61) | Replaces the current session scene set with one active scene.<br>`scene`: The scene to load. | — | — |
| <a id="api-50622d7397aa1f61"></a>[`public static void Inno.Scene.SceneManager.LoadSceneAdditive(Inno.Scene.GameScene scene, bool makeActive = true)`](../../src/content/scene/Inno.Scene/SceneManager.cs#L72) | Loads a scene alongside the current session scene set.<br>`scene`: The scene to load additively.<br>`makeActive`: Whether the loaded scene becomes active. | — | — |
| <a id="api-2ad27e9facbd70a2"></a>[`public static Inno.Scene.GameScene Inno.Scene.SceneManager.LoadNewScene(string name = "Untitled Scene")`](../../src/content/scene/Inno.Scene/SceneManager.cs#L87) | Creates and loads a new active scene in the current runtime session.<br>`name`: The initial scene display name. | The newly created and loaded scene. | — |
| <a id="api-f54017ef811b0f34"></a>[`public static Inno.Scene.GameScene Inno.Scene.SceneManager.LoadNewSceneAdditive(string name = "Untitled Scene", bool makeActive = true)`](../../src/content/scene/Inno.Scene/SceneManager.cs#L101) | Creates and additively loads a new scene in the current runtime session.<br>`name`: The initial scene display name.<br>`makeActive`: Whether the new scene becomes active. | The newly created and loaded scene. | — |
| <a id="api-d25fd1cec73adf35"></a>[`public static void Inno.Scene.SceneManager.SetActiveScene(Inno.Scene.GameScene scene)`](../../src/content/scene/Inno.Scene/SceneManager.cs#L113) | Makes a loaded scene active in the current runtime session.<br>`scene`: The loaded scene to activate. | — | — |
| <a id="api-2e1ce237020972fb"></a>[`public static void Inno.Scene.SceneManager.MoveGameObjectToScene(Inno.Scene.GameObject gameObject, Inno.Scene.GameScene destination)`](../../src/content/scene/Inno.Scene/SceneManager.cs#L124) | Moves a live object subtree into another scene owned by the current runtime session.<br>`gameObject`: The live root object to move.<br>`destination`: The loaded destination scene. | — | — |
| <a id="api-5b545641ec66ada0"></a>[`public static void Inno.Scene.SceneManager.UnloadActiveScene()`](../../src/content/scene/Inno.Scene/SceneManager.cs#L133) | Unloads the active scene in the current runtime session when one exists. | — | — |
| <a id="api-6c5c5f309498f956"></a>[`public static bool Inno.Scene.SceneManager.UnloadScene(Inno.Scene.GameScene scene)`](../../src/content/scene/Inno.Scene/SceneManager.cs#L144) | Unloads one scene from the current runtime session.<br>`scene`: The scene to unload. | when the scene was loaded; otherwise, . | — |
| <a id="api-2ed418028233634c"></a>[`public static void Inno.Scene.SceneManager.UnloadAllScenes()`](../../src/content/scene/Inno.Scene/SceneManager.cs#L149) | Unloads every scene from the current runtime session. | — | — |
| <a id="api-c6fafc462dd9b101"></a>[`public static void Inno.Scene.SceneManager.FixedUpdate(float fixedDeltaTime)`](../../src/content/scene/Inno.Scene/SceneManager.cs#L157) | Advances fixed-step scene lifecycle callbacks in the current runtime session.<br>`fixedDeltaTime`: The fixed simulation interval in seconds. | — | — |
| <a id="api-fee561ae3445c05b"></a>[`public static void Inno.Scene.SceneManager.Update(float deltaTime)`](../../src/content/scene/Inno.Scene/SceneManager.cs#L165) | Advances variable-step scene lifecycle callbacks in the current runtime session.<br>`deltaTime`: The elapsed frame time in seconds. | — | — |
| <a id="api-479921c395662666"></a>[`public static void Inno.Scene.SceneManager.LateUpdate(float deltaTime)`](../../src/content/scene/Inno.Scene/SceneManager.cs#L173) | Advances late scene lifecycle callbacks in the current runtime session.<br>`deltaTime`: The elapsed frame time in seconds. | — | — |
| <a id="api-f129eb26200d6814"></a>[`public sealed class Inno.Scene.SceneWorld : System.IDisposable`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L16) | Owns the loaded scene set and lifecycle state for one isolated runtime session. | — | — |
| <a id="api-d4d57d869dfa6d61"></a>[`public Inno.Scene.SceneWorld.SceneWorld(Inno.Core.Identity.IdentityAllocator identities, Inno.Extensibility.Types.TypeCatalog types)`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L41) | Creates an empty scene world bound to one session identity allocator.<br>`identities`: The allocator that owns identities for scenes and their elements.<br>`types`: The host type catalog used to derive scene component and system generations. | — | `System.ArgumentNullException`: Thrown when is null. |
| <a id="api-8da337d66548bba8"></a>[`public void Inno.Scene.SceneWorld.ConfigurePrefabInstantiation(Inno.Core.Serialization.SerializationRegistry serialization, Inno.Assets.IAssetReferenceResolver assets)`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L73) | Binds the session-owned serialization and asset generations used by scene prefab instances.<br>`serialization`: The active serialization registry for this session.<br>`assets`: The asset resolver that owns prefab dependencies. | — | `System.InvalidOperationException`: The binding was already configured. |
| <a id="api-dca95063ae8b82e2"></a>[`public Inno.Scene.GameScene? Inno.Scene.SceneWorld.activeScene { get; }`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L107) | Gets the scene currently selected for unqualified scene operations. | — | — |
| <a id="api-8a6e813a4bab4f63"></a>[`public bool Inno.Scene.SceneWorld.hasActiveScene { get; }`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L112) | Gets whether this world contains an active scene. | — | — |
| <a id="api-cc850d43ec93e450"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Scene.GameScene> Inno.Scene.SceneWorld.loadedScenes { get; }`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L117) | Gets an immutable snapshot of loaded scenes in hierarchy order. | — | — |
| <a id="api-2d299f878aa510af"></a>[`public TObject? Inno.Scene.SceneWorld.Find<TObject>(System.Guid persistentId) where TObject : Inno.Core.Identity.IdentityObject`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L131) | Resolves a live scene object by its persistent identity within this world.<br>`persistentId`: The persistent identity to resolve.<br>`TObject`: The required scene object contract. | The live compatible object, or when this world does not own one. | — |
| <a id="api-674557d8ca1350ea"></a>[`public System.IDisposable Inno.Scene.SceneWorld.EnterScope()`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L144) | Binds this world and its identity allocator to the current asynchronous execution context. | A strict last-in-first-out execution scope owned by the caller. | `System.ObjectDisposedException`: Thrown when this world has been disposed. |
| <a id="api-34b68a6992bc9dfe"></a>[`public int Inno.Scene.SceneWorld.GetSceneIndex(Inno.Scene.GameScene scene)`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L163) | Gets the hierarchy index of a loaded scene.<br>`scene`: The loaded scene to locate. | The zero-based hierarchy index. | `System.InvalidOperationException`: Thrown when the scene is not loaded by this world. |
| <a id="api-ec9df29dc94e9cd4"></a>[`public void Inno.Scene.SceneWorld.SetSceneIndex(Inno.Scene.GameScene scene, int sceneIndex)`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L185) | Moves a loaded scene to a hierarchy index without changing the active scene.<br>`scene`: The loaded scene to move.<br>`sceneIndex`: The requested zero-based hierarchy index. | — | `System.InvalidOperationException`: Thrown when the scene is not loaded by this world. |
| <a id="api-a80e6d29106f876c"></a>[`public void Inno.Scene.SceneWorld.LoadScene(Inno.Scene.GameScene scene)`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L208) | Replaces the loaded set with one scene and makes it active.<br>`scene`: The scene to load. | — | — |
| <a id="api-87b7dec00a42616c"></a>[`public void Inno.Scene.SceneWorld.LoadSceneAdditive(Inno.Scene.GameScene scene, bool makeActive = true)`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L230) | Loads a scene alongside the existing scene set.<br>`scene`: The scene to load additively.<br>`makeActive`: Whether the loaded scene becomes active. | — | — |
| <a id="api-56d324daa161f857"></a>[`public Inno.Scene.GameScene Inno.Scene.SceneWorld.LoadNewScene(string name = "Untitled Scene")`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L255) | Creates and loads a new active scene.<br>`name`: The initial scene display name. | The newly created and loaded scene. | — |
| <a id="api-6722e9a73eb42d5d"></a>[`public Inno.Scene.GameScene Inno.Scene.SceneWorld.LoadNewSceneAdditive(string name = "Untitled Scene", bool makeActive = true)`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L275) | Creates and additively loads a new scene.<br>`name`: The initial scene display name.<br>`makeActive`: Whether the new scene becomes active. | The newly created and loaded scene. | — |
| <a id="api-54e7afa28ca24fb0"></a>[`public void Inno.Scene.SceneWorld.SetActiveScene(Inno.Scene.GameScene scene)`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L294) | Makes one loaded scene active without changing the loaded set.<br>`scene`: The loaded scene to activate. | — | `System.InvalidOperationException`: Thrown when the scene is not loaded by this world. |
| <a id="api-bc396b3f5065a851"></a>[`public void Inno.Scene.SceneWorld.MoveGameObjectToScene(Inno.Scene.GameObject gameObject, Inno.Scene.GameScene destination)`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L315) | Moves a live object subtree between two scenes loaded by this world.<br>`gameObject`: The live root object to move.<br>`destination`: The loaded destination scene. | — | `System.InvalidOperationException`: Thrown when either scene is not loaded or the object is not live. |
| <a id="api-d13d7555df4c3a58"></a>[`public void Inno.Scene.SceneWorld.UnloadActiveScene()`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L334) | Unloads the active scene when one exists. | — | — |
| <a id="api-ccec87fbda52da4f"></a>[`public bool Inno.Scene.SceneWorld.UnloadScene(Inno.Scene.GameScene scene)`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L350) | Unloads one scene and selects another loaded scene when necessary.<br>`scene`: The scene to unload. | when the scene was loaded by this world; otherwise, . | — |
| <a id="api-3595d99924fec545"></a>[`public void Inno.Scene.SceneWorld.UnloadAllScenes()`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L372) | Unloads every scene while preserving the first lifecycle failure. | — | — |
| <a id="api-4efd87167b721450"></a>[`public void Inno.Scene.SceneWorld.FixedUpdate(float fixedDeltaTime)`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L400) | Advances fixed-step lifecycle callbacks for every loaded scene.<br>`fixedDeltaTime`: The fixed simulation interval in seconds. | — | — |
| <a id="api-4e06bb9a07414c31"></a>[`public void Inno.Scene.SceneWorld.Update(float deltaTime)`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L414) | Advances variable-step lifecycle callbacks for every loaded scene.<br>`deltaTime`: The elapsed frame time in seconds. | — | — |
| <a id="api-173072fa0a8b1b27"></a>[`public void Inno.Scene.SceneWorld.LateUpdate(float deltaTime)`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L428) | Advances late lifecycle callbacks for every loaded scene.<br>`deltaTime`: The elapsed frame time in seconds. | — | — |
| <a id="api-7a287526d6c33a96"></a>[`public void Inno.Scene.SceneWorld.Dispose()`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L439) | Unloads every scene and permanently releases this world. | — | — |
| <a id="api-079224a4ad8c5e0a"></a>[`public static class Inno.Scene.SceneElementSerialization`](../../src/content/scene/Inno.Scene/Serialization/SceneElementSerialization.cs#L16) | Recreates individual scene components and systems from stable type and object identities. | — | — |
| <a id="api-1be42e5528e8727f"></a>[`public static byte[] Inno.Scene.SceneElementSerialization.CaptureState(Inno.Scene.EngineObject target, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Assets.IAssetReferenceResolver assets)`](../../src/content/scene/Inno.Scene/Serialization/SceneElementSerialization.cs#L39) | Captures one element's logical type, neutral properties, asset dependencies and scene-reference aliases.<br>`target`: The live component or system, including a missing placeholder.<br>`serialization`: The owner's current serializer generation.<br>`assets`: The owner's complete asset resolver used during capture. | Neutral element bytes suitable for History and restoration without retaining the current managed type. | `System.ArgumentException`: The target is not a component or system.<br>`System.InvalidOperationException`: The element is not live in a loaded scene. |
| <a id="api-ca56d02111c0212a"></a>[`public static void Inno.Scene.SceneElementSerialization.RestoreState(Inno.Scene.EngineObject target, System.ReadOnlySpan<byte> stateData, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Assets.IAssetReferenceResolver assets)`](../../src/content/scene/Inno.Scene/Serialization/SceneElementSerialization.cs#L101) | Restores a captured element state into the same logical type, including neutral state on a missing placeholder.<br>`target`: The attached element receiving the state.<br>`stateData`: Bytes produced by .<br>`serialization`: The owner's current serializer generation.<br>`assets`: The owner's complete asset resolver. | — | `System.IO.InvalidDataException`: The payload is invalid or represents a different logical type.<br>`System.InvalidOperationException`: Property or reference restoration is incomplete. |
| <a id="api-f4815910a8f34f04"></a>[`public static Inno.Scene.GameComponent Inno.Scene.SceneElementSerialization.RestoreComponent(Inno.Scene.GameObject owner, Inno.Extensibility.Types.TypeRef type, System.Guid persistentId, int componentIndex, System.ReadOnlySpan<byte> stateData, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Assets.IAssetReferenceResolver assets)`](../../src/content/scene/Inno.Scene/Serialization/SceneElementSerialization.cs#L166) | Recreates one component without invoking Reset and restores its persistent properties.<br>`owner`: The live GameObject that will own the component.<br>`type`: The reload-safe identity of the current component implementation.<br>`persistentId`: The component instance identity to preserve.<br>`componentIndex`: The requested attachment index.<br>`stateData`: Complete neutral element bytes produced by CaptureState, including missing-state metadata.<br>`serialization`: The serialization registry that owns the active converter generation.<br>`assets`: The resolver used to materialize canonical asset references in restored properties. | The recreated component, or a preserved missing placeholder when its logical type is unavailable. | `System.ArgumentNullException`: Thrown when is .<br>`System.IO.InvalidDataException`: Thrown when the payload is invalid or the stable type resolves to an incompatible component.<br>`System.InvalidOperationException`: Thrown when the identity or component multiplicity conflicts, property restoration is incomplete, or a failed restoration cannot remove its partially created component. |
| <a id="api-e1478f7868f10728"></a>[`public static Inno.Scene.GameSystem Inno.Scene.SceneElementSerialization.RestoreSystem(Inno.Scene.GameScene scene, Inno.Extensibility.Types.TypeRef type, System.Guid persistentId, int systemIndex, System.ReadOnlySpan<byte> stateData, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Assets.IAssetReferenceResolver assets)`](../../src/content/scene/Inno.Scene/Serialization/SceneElementSerialization.cs#L239) | Recreates one scene system without invoking Reset and restores its persistent properties.<br>`scene`: The live loaded scene that will own the system.<br>`type`: The reload-safe identity of the current system implementation.<br>`persistentId`: The system instance identity to preserve.<br>`systemIndex`: The requested display and serialization index.<br>`stateData`: Complete neutral element bytes produced by CaptureState, including missing-state metadata.<br>`serialization`: The serialization registry that owns the active converter generation.<br>`assets`: The resolver used to materialize canonical asset references in restored properties. | The recreated system, or a preserved missing placeholder when its logical type is unavailable. | `System.ArgumentNullException`: Thrown when is .<br>`System.IO.InvalidDataException`: Thrown when the payload is invalid or the stable type resolves to an incompatible system.<br>`System.InvalidOperationException`: Thrown when the identity or system multiplicity conflicts, property restoration is incomplete, or a failed restoration cannot remove its partially created system. |
| <a id="api-a387628292985037"></a>[`public static class Inno.Scene.ScenePropertySerialization`](../../src/content/scene/Inno.Scene/Serialization/ScenePropertySerialization.cs#L14) | Captures and restores individual scene-object properties while preserving scene reference identities. | — | — |
| <a id="api-f31dff3c94bb23f9"></a>[`public static byte[] Inno.Scene.ScenePropertySerialization.CaptureProperty(Inno.Scene.EngineObject target, string propertyName, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Assets.IAssetReferenceResolver assets)`](../../src/content/scene/Inno.Scene/Serialization/ScenePropertySerialization.cs#L43) | Captures one persistent property without serializing the complete scene.<br>`target`: The live scene object containing the property.<br>`propertyName`: The exact serialized member key.<br>`serialization`: The serialization registry that owns the active converter generation.<br>`assets`: The resolver used to preserve canonical asset references in captured values. | Neutral bytes that can be restored into the same logical object identity. | `System.ArgumentNullException`: Thrown when is .<br>`System.ArgumentException`: Thrown when is empty or unknown.<br>`System.InvalidOperationException`: Thrown when the target is destroyed or not owned by a loaded scene. |
| <a id="api-592ddfb216dc523b"></a>[`public static byte[] Inno.Scene.ScenePropertySerialization.CaptureProperties(Inno.Scene.EngineObject target, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Assets.IAssetReferenceResolver assets)`](../../src/content/scene/Inno.Scene/Serialization/ScenePropertySerialization.cs#L82) | Captures all persistent properties without serializing the complete scene.<br>`target`: The live scene object whose state should be captured.<br>`serialization`: The serialization registry that owns the active converter generation.<br>`assets`: The resolver used to preserve canonical asset references in captured values. | Neutral bytes containing independently encoded properties. | `System.ArgumentNullException`: Thrown when is .<br>`System.InvalidOperationException`: Thrown when the target is destroyed or not owned by a loaded scene. |
| <a id="api-f422f1ec31dbc12b"></a>[`public static System.Collections.Generic.IReadOnlyList<Inno.Core.Serialization.SerializationPropertySnapshot> Inno.Scene.ScenePropertySerialization.CapturePropertySnapshots(Inno.Scene.EngineObject target, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Assets.IAssetReferenceResolver assets)`](../../src/content/scene/Inno.Scene/Serialization/ScenePropertySerialization.cs#L113) | Captures each persistent property independently while preserving scene-reference identities.<br>`target`: Live scene object whose properties should be captured.<br>`serialization`: Registry owning the active converter generation.<br>`assets`: Resolver used to preserve canonical asset references. | Stable ordered property snapshots suitable for compact delta comparison. | — |
| <a id="api-621563afc61f3cdb"></a>[`public static Inno.Core.Serialization.SerializationPropertyRestoreResult Inno.Scene.ScenePropertySerialization.RestoreProperties(Inno.Scene.EngineObject target, System.ReadOnlySpan<byte> data, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Assets.IAssetReferenceResolver assets, Inno.Core.Serialization.SerializationPropertyRestoreMode mode = Inno.Core.Serialization.SerializationPropertyRestoreMode.Strict)`](../../src/content/scene/Inno.Scene/Serialization/ScenePropertySerialization.cs#L156) | Restores independently captured properties into a live scene object.<br>`target`: The current object generation receiving the values.<br>`data`: Bytes produced by or .<br>`mode`: The compatibility policy applied to matching current properties.<br>`serialization`: The serialization registry that owns the active converter generation.<br>`assets`: The resolver used to materialize canonical asset references in restored values. | A summary of restored, ignored, and incompatible properties. | `System.ArgumentNullException`: Thrown when is .<br>`System.InvalidOperationException`: Thrown when strict restoration fails or the target is not in a loaded scene. |
| <a id="api-1ef1655b6606e367"></a>[`public static class Inno.Scene.SceneSubtreeSerialization`](../../src/content/scene/Inno.Scene/Serialization/SceneSubtreeSerialization.cs#L14) | Captures and restores one GameObject subtree with its persistent object and component identities. | — | — |
| <a id="api-dd7cfc15a74b70ec"></a>[`public static byte[] Inno.Scene.SceneSubtreeSerialization.Capture(Inno.Scene.GameObject root, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Assets.IAssetReferenceResolver assets)`](../../src/content/scene/Inno.Scene/Serialization/SceneSubtreeSerialization.cs#L37) | Captures one live GameObject and all descendants without serializing unrelated scene objects.<br>`root`: The live subtree root.<br>`serialization`: The serialization registry that owns the active converter generation.<br>`assets`: The asset-reference resolver used to encode referenced asset identities. | Neutral bytes containing the subtree structure, components, properties, and identity references. | `System.ArgumentNullException`: Thrown when is .<br>`System.InvalidOperationException`: Thrown when the root is destroyed or detached. |
| <a id="api-323e1a06ec154151"></a>[`public static Inno.Scene.GameObject Inno.Scene.SceneSubtreeSerialization.Restore(Inno.Scene.GameScene scene, System.ReadOnlySpan<byte> data, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Assets.IAssetReferenceResolver assets, Inno.Scene.Components.Transform? parent, int siblingIndex)`](../../src/content/scene/Inno.Scene/Serialization/SceneSubtreeSerialization.cs#L81) | Restores a previously captured subtree into a loaded scene with its original persistent identities.<br>`scene`: The loaded destination scene.<br>`data`: Bytes created by .<br>`parent`: The optional external parent restored after the subtree is created.<br>`siblingIndex`: The requested sibling index under or at the scene root.<br>`serialization`: The serialization registry that owns the active converter generation.<br>`assets`: The asset-reference resolver used to restore referenced asset identities. | The restored subtree root. | `System.ArgumentNullException`: Thrown when is .<br>`System.InvalidOperationException`: Thrown when the scene is unavailable or an identity conflicts. |
| <a id="api-7bdd6dd7608981ea"></a>[`public sealed class Inno.Scene.SceneTypeResolutionException : System.Exception`](../../src/content/scene/Inno.Scene/Serialization/SceneTypeResolutionException.cs#L9) | Reports that a serialized scene element cannot be created because its stable type is not present in the active type catalog. | — | — |
| <a id="api-f75d85a55121c213"></a>[`public Inno.Scene.SceneTypeResolutionException.SceneTypeResolutionException(System.Guid stableTypeId, string elementKind)`](../../src/content/scene/Inno.Scene/Serialization/SceneTypeResolutionException.cs#L23) | Creates an exception for one unresolved serialized scene type.<br>`stableTypeId`: The stable identity stored by the serialized scene element.<br>`elementKind`: The human-readable scene element kind, such as component or system. | — | `System.ArgumentException`: Thrown when is empty or is empty. |
| <a id="api-1c95c5c9349c5fea"></a>[`public System.Guid Inno.Scene.SceneTypeResolutionException.stableTypeId { get; }`](../../src/content/scene/Inno.Scene/Serialization/SceneTypeResolutionException.cs#L39) | Gets the unresolved stable type identity. | — | — |
| <a id="api-286ec47e6085f50c"></a>[`public string Inno.Scene.SceneTypeResolutionException.elementKind { get; }`](../../src/content/scene/Inno.Scene/Serialization/SceneTypeResolutionException.cs#L44) | Gets the scene element kind that expected the missing type. | — | — |

## 项目依赖

| 工程 | 边界 |
| --- | --- |
| `Inno.Extensibility.Reload` | 实现依赖；不传递 compile surface |
| `Inno.Core.Execution` | 实现依赖；不传递 compile surface |
| `Inno.Core.Collections` | 实现依赖；不传递 compile surface |
| `Inno.Scripting.Api` | 实现依赖；不传递 compile surface |
| `Inno.Core.Identity` | 公开签名依赖 |
| `Inno.Core.Mathematics` | 公开签名依赖 |
| `Inno.Extensibility.Types` | 公开签名依赖 |
| `Inno.Core.Serialization` | 公开签名依赖 |
| `Inno.Core.Settings` | 公开签名依赖 |
| `Inno.Assets` | 公开签名依赖 |
| `Inno.References` | 公开签名依赖 |
| `Inno.Core.Diagnostics` | 公开签名依赖 |
| `Inno.Extensibility.Catalogs` | 公开签名依赖 |
