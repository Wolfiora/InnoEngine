# Inno.Scene

[Engine 索引](README.md) · [Scene Assets](Inno.Scene.Assets.md) · [Wiki 首页](../README.md)

`Inno.Scene` 提供 Scene、GameObject、Component、Transform hierarchy、GameBehavior 和 GameSystem 的运行时模型。`GameBehavior` 是唯一具有独立启停和帧生命周期的 Component 基类，统一负责 `enabled`、`isActiveAndEnabled`、`Awake`、`Start`、`OnEnable`、`OnDisable`、`Update`、`FixedUpdate`、`LateUpdate` 与 `OnDestroy`。Project Script、Renderer、Camera、Light 等场景功能都直接继承 `GameBehavior`，不存在第二层 `Behavior` 类型或兼容 façade。

`Transform` 除了便捷的 local/world TRS 外，还公开精确的 `localToWorldMatrix`、`worldToLocalMatrix`、`TransformPoint`、`InverseTransformPoint` 与原子 `SetWorldTransform`。`worldPosition` 与点变换统一由完整层级矩阵确定。旋转且非均匀缩放的多级层级可能包含不能由单一 world TRS 无损表示的 shear；渲染、包围盒和空间查询应优先使用矩阵/点变换 API，Inspector 与 Gizmo 的 TRS 编辑则使用原子 setter 避免三次中间重算。零缩放层级不可逆，世界到本地转换和保持世界值的重设父级会明确失败，不会静默使用 identity inverse。

## 多 Scene

Scene reload 的旧 element 普通退休失败会聚合抛出，且仍尝试其余旧 element；统一 generation 的不可逆 Complete 阶段据此 Fault。RetirementPendingException 则保持 owner、立即阻止后续清理并交给共享 gate Fault。不能只发布 Warning 后继续报告成功，也不能撤销已经提交的新 generation 假装回滚。

```csharp
SceneManager.LoadScene(first);                 // Single: unload current set.
SceneManager.LoadSceneAdditive(second);        // Add to the bottom and make active.
SceneManager.SetSceneIndex(second, 0);         // Change hierarchy/enumeration order.
SceneManager.SetActiveScene(first);             // Does not reorder scenes.
SceneManager.MoveGameObjectToScene(player, second); // Moves the complete subtree.
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

`GameScene.InstantiatePrefab` 返回已连接 prefab 的根对象，并使用加载该 Scene 的 world；Canvas 等呈现阶段回调即使处于其他临时作用域，也会在正确的场景与 Identity 作用域中创建实例。Prefab 或目标 Scene 不可用、父物体不属于目标 Scene 时会明确失败。Host 在会话启动时通过 `SceneWorld.ConfigurePrefabInstantiation` 注入当前序列化器与资产解析器；该装配方法不属于脚本 API。底层 `PrefabAsset.Instantiate` 先恢复独立实例，再设置目标父级，避免把父物体带进 prefab 内部用于差异对比的临时 Scene。

`GameBehavior.Update` 等执行阶段允许创建 prefab。新建对象及其组件会在执行阶段结束时一同提交；反序列化可在提交前恢复该新对象的组件顺序，但仍禁止在执行阶段重排已提交对象的组件。
Prefab 差异映射在这个阶段读取当前有效对象（包含尚待提交的实例及组件），不触发要求场景已稳定的完整 Scene Capture。一次实例化只校准其新建子树中的嵌套 prefab；场景序列化仍要求结构修改全部提交。

Scene 顺序决定当前 `SceneManager` 的跨 Scene traversal 顺序，但业务脚本不应把它作为精确的脚本执行顺序契约；显式依赖应放入可排序的 GameSystem 或独立 scheduler。

`MoveGameObjectToScene` 要求 source 与 destination 都已加载。被移动对象会成为目标 Scene 的 root；完整 child subtree、GameObject/Component 实例、persistent ID、世界变换和生命周期状态保持不变。该操作不会通过序列化复制对象，也不会调用 Reset 或 Destroy。

## Name 与 Tag 查询

每个 `GameObject` 默认使用 `GameObject.defaultTag`（`"Untagged"`），并允许通过普通字符串设置项目 Tag：

```csharp
player.tag = "Player";

GameObject? named = scene.FindObject("Player Root");
GameObject? firstPlayer = scene.FindObjectWithTag("Player");
IReadOnlyList<GameObject> players = scene.FindObjectsWithTag("Player");
```

Name 与 Tag 都按 `StringComparison.Ordinal` 匹配，复数查询保持 Scene 创建顺序。内部 `SceneStore` 除 Name/Tag/Layer 索引外还维护私有 Unique + Ordered 顺序键；删除对象导致 dense storage swap-back 时不会改变其余对象的查询顺序。查询通过标准 `Query().Find(...).OrderBy(...).Get()/First()` 契约执行，不依赖 Scene 专用 Storage API；对象元数据变化时只更新对应 entry。Tag 会随 Scene、Prefab 和 prefab override 一起序列化。

可用 Tag 定义由 `GameTagCatalog` 这个普通 Project Setting 提供；assignment 与 definition 明确分离：

```csharp
GameTagCatalog tags = Settings.Get<GameTagCatalog>(GameTagCatalog.settingId);
if (tags.IsDefined("Player"))
    player.tag = "Player";
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

GameLayerCatalog layers = Settings.Get<GameLayerCatalog>(GameLayerCatalog.settingId);
GameLayer player = layers.GetLayer("Player");
GameLayer enemy = layers.GetLayer("Enemy");
GameLayerId playerId = layers.GetId(Settings.projectId, player)!.Value;
GameLayerMask visible = layers.GetMask(["Player", "Enemy"]);

gameObject.layer = player;
layers.SetInteraction(player, enemy, canInteract: false);
```

`GameLayer.defaultLayer` 固定为 slot 0、local key `default` 和名称 `Default`。其完整 ID 会随项目身份解析为 `projectId.default`。Plugin contribution 同样只携带 local key/slot/name 和 interaction operation，不携带导出项目的 Project ID；导入后自然落在消费项目命名空间下。

Composer 对相同 local key、slot、name 的声明去重；同一 slot/key 或 interaction pair 的不兼容声明仍要求显式依赖与 override。32 个 slot 是 Layer mask 的真实有限资源，Composer 不会重排 slot 或改写 Scene assignment。

## Component 顺序

```csharp
GameComponent[] components = [.. gameObject.GetComponents()];
gameObject.SetComponentIndex(components[2], 1);
int index = gameObject.GetComponentIndex(components[2]);
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
public sealed class PhysicsSystem : GameSystem
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

## Editor Scene 名称与资产路径

已保存 Scene 的 Inspector 名称可以编辑。名称变化立即使文档进入 dirty 状态；保存时同目录 SceneAsset 被事务式重命名，`.imeta`、persistent ID、canonical instance 和 artifact identity 保持不变。目标文件已存在时保存会给出冲突错误，不覆盖另一个资产。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Scene.AllowMultipleComponentAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Scene.AllowMultipleComponentAttribute`](../../src/content/scene/Inno.Scene/Attributes/AllowMultipleComponentAttribute.cs#L8) | Allows multiple instances of a concrete component type on one game object. |

### `Inno.Scene.AllowMultipleSystemAttribute`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Scene.AllowMultipleSystemAttribute`](../../src/content/scene/Inno.Scene/Attributes/AllowMultipleSystemAttribute.cs#L8) | Allows multiple instances of a concrete type in one scene. |

### `Inno.Scene.Components.Transform`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Mathematics.Matrix Inno.Scene.Components.Transform.localToWorldMatrix`](../../src/content/scene/Inno.Scene/Concrete/Transform.cs#L108) | Gets the exact local-to-world matrix, including the complete parent hierarchy. |
| [`Inno.Core.Mathematics.Matrix Inno.Scene.Components.Transform.worldToLocalMatrix`](../../src/content/scene/Inno.Scene/Concrete/Transform.cs#L116) | Gets the inverse of the exact local-to-world matrix. |
| [`Inno.Core.Mathematics.Quaternion Inno.Scene.Components.Transform.localRotation`](../../src/content/scene/Inno.Scene/Concrete/Transform.cs#L53) | Gets or sets the local rotation relative to the parent. |
| [`Inno.Core.Mathematics.Quaternion Inno.Scene.Components.Transform.worldRotation`](../../src/content/scene/Inno.Scene/Concrete/Transform.cs#L90) | Gets or sets the world-space rotation. |
| [`Inno.Core.Mathematics.Vector3 Inno.Scene.Components.Transform.InverseTransformPoint(Inno.Core.Mathematics.Vector3 point)`](../../src/content/scene/Inno.Scene/Concrete/Transform.cs#L201) | Transforms a world-space point into this transform's local space. |
| [`Inno.Core.Mathematics.Vector3 Inno.Scene.Components.Transform.TransformPoint(Inno.Core.Mathematics.Vector3 point)`](../../src/content/scene/Inno.Scene/Concrete/Transform.cs#L190) | Transforms a local-space point through the complete parent hierarchy. |
| [`Inno.Core.Mathematics.Vector3 Inno.Scene.Components.Transform.localPosition`](../../src/content/scene/Inno.Scene/Concrete/Transform.cs#L39) | Gets or sets the local position relative to the parent. |
| [`Inno.Core.Mathematics.Vector3 Inno.Scene.Components.Transform.localScale`](../../src/content/scene/Inno.Scene/Concrete/Transform.cs#L67) | Gets or sets the local scale relative to the parent. |
| [`Inno.Core.Mathematics.Vector3 Inno.Scene.Components.Transform.worldPosition`](../../src/content/scene/Inno.Scene/Concrete/Transform.cs#L81) | Gets or sets the world-space position. |
| [`Inno.Core.Mathematics.Vector3 Inno.Scene.Components.Transform.worldScale`](../../src/content/scene/Inno.Scene/Concrete/Transform.cs#L99) | Gets or sets the world-space scale. |
| [`Inno.Scene.Components.Transform`](../../src/content/scene/Inno.Scene/Concrete/Transform.cs#L14) | Stores local transform data and exposes the scene hierarchy relationship. |
| [`Inno.Scene.Components.Transform.Transform()`](../../src/content/scene/Inno.Scene/Concrete/Transform.cs#L31) | Creates a transform with identity local values. |
| [`Inno.Scene.Components.Transform? Inno.Scene.Components.Transform.parent`](../../src/content/scene/Inno.Scene/Concrete/Transform.cs#L128) | Gets the parent transform, or for a scene-level object. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Scene.Components.Transform> Inno.Scene.Components.Transform.children`](../../src/content/scene/Inno.Scene/Concrete/Transform.cs#L133) | Gets child transforms in sibling order. |
| [`int Inno.Scene.Components.Transform.siblingIndex`](../../src/content/scene/Inno.Scene/Concrete/Transform.cs#L138) | Gets or sets this transform's index among its siblings. |
| [`override void Inno.Scene.Components.Transform.Reset()`](../../src/content/scene/Inno.Scene/Concrete/Transform.cs#L206) | Restores this instance to its initial reusable state. |
| [`void Inno.Scene.Components.Transform.SetParent(Inno.Scene.Components.Transform? parent)`](../../src/content/scene/Inno.Scene/Concrete/Transform.cs#L153) | Sets the parent while preserving this transform's world-space values. |
| [`void Inno.Scene.Components.Transform.SetSiblingIndex(int siblingIndex)`](../../src/content/scene/Inno.Scene/Concrete/Transform.cs#L161) | Moves this transform within its current sibling collection. |
| [`void Inno.Scene.Components.Transform.SetWorldTransform(Inno.Core.Mathematics.Vector3 position, Inno.Core.Mathematics.Quaternion rotation, Inno.Core.Mathematics.Vector3 scale)`](../../src/content/scene/Inno.Scene/Concrete/Transform.cs#L175) | Atomically applies world-space translation, rotation, and scale. |

### `Inno.Scene.EngineAssetContent`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.AssetDependency[] Inno.Scene.EngineAssetContent.GetDependencies()`](../../src/content/scene/Inno.Scene/Assets/EngineAssetContent.cs#L53) | Gets a detached copy of direct persistent asset dependencies. |
| [`Inno.Scene.EngineAssetContent`](../../src/content/scene/Inno.Scene/Assets/EngineAssetContent.cs#L11) | Carries immutable scene or prefab runtime content across the runtime-to-authoring asset boundary. |
| [`Inno.Scene.EngineAssetContent.EngineAssetContent(System.ReadOnlySpan<byte> payload, System.Collections.Generic.IReadOnlyList<Inno.Assets.AssetDependency> dependencies)`](../../src/content/scene/Inno.Scene/Assets/EngineAssetContent.cs#L28) | Creates a detached engine asset content snapshot. |
| [`byte[] Inno.Scene.EngineAssetContent.GetPayload()`](../../src/content/scene/Inno.Scene/Assets/EngineAssetContent.cs#L45) | Gets a detached copy of the serialized runtime graph. |

### `Inno.Scene.EngineObject`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Scene.EngineObject`](../../src/content/scene/Inno.Scene/Core/EngineObject.cs#L11) | Provides identity and destruction state shared by all managed scene objects. |
| [`bool Inno.Scene.EngineObject.isDestroyed`](../../src/content/scene/Inno.Scene/Core/EngineObject.cs#L18) | Gets whether the engine has destroyed this object. |
| [`override bool Inno.Scene.EngineObject.Equals(object? obj)`](../../src/content/scene/Inno.Scene/Core/EngineObject.cs#L71) | Compares this instance to another object using reference identity. |
| [`override int Inno.Scene.EngineObject.GetHashCode()`](../../src/content/scene/Inno.Scene/Core/EngineObject.cs#L79) | Returns a stable runtime reference hash code. |
| [`static bool Inno.Scene.EngineObject.operator !=(Inno.Scene.EngineObject? left, Inno.Scene.EngineObject? right)`](../../src/content/scene/Inno.Scene/Core/EngineObject.cs#L57) | Compares engine objects using reference identity and destroyed-object null semantics. |
| [`static bool Inno.Scene.EngineObject.operator ==(Inno.Scene.EngineObject? left, Inno.Scene.EngineObject? right)`](../../src/content/scene/Inno.Scene/Core/EngineObject.cs#L32) | Compares engine objects using reference identity and destroyed-object null semantics. |

### `Inno.Scene.GameBehavior`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Scene.GameBehavior`](../../src/content/scene/Inno.Scene/Core/GameBehavior.cs#L9) | Base component for independently enabled scene functionality with frame lifecycle callbacks. |
| [`bool Inno.Scene.GameBehavior.enabled`](../../src/content/scene/Inno.Scene/Core/GameBehavior.cs#L21) | Gets or sets whether this component participates in scene lifecycle and frame updates. |
| [`bool Inno.Scene.GameBehavior.isActiveAndEnabled`](../../src/content/scene/Inno.Scene/Core/GameBehavior.cs#L39) | Gets whether this component is enabled and active in its owning hierarchy. |
| [`virtual void Inno.Scene.GameBehavior.Awake()`](../../src/content/scene/Inno.Scene/Core/GameBehavior.cs#L44) | Called once before this behavior first becomes active. |
| [`virtual void Inno.Scene.GameBehavior.FixedUpdate()`](../../src/content/scene/Inno.Scene/Core/GameBehavior.cs#L79) | Called during the fixed-rate update stage. Use Time.fixedDeltaTime for step timing. |
| [`virtual void Inno.Scene.GameBehavior.LateUpdate()`](../../src/content/scene/Inno.Scene/Core/GameBehavior.cs#L86) | Called during the late update stage. Use Time.deltaTime for frame timing. |
| [`virtual void Inno.Scene.GameBehavior.OnDestroy()`](../../src/content/scene/Inno.Scene/Core/GameBehavior.cs#L93) | Called immediately before this behavior is detached and destroyed. |
| [`virtual void Inno.Scene.GameBehavior.OnDisable()`](../../src/content/scene/Inno.Scene/Core/GameBehavior.cs#L65) | Called when this component stops being active and enabled. |
| [`virtual void Inno.Scene.GameBehavior.OnEnable()`](../../src/content/scene/Inno.Scene/Core/GameBehavior.cs#L58) | Called when this component becomes active and enabled. |
| [`virtual void Inno.Scene.GameBehavior.Start()`](../../src/content/scene/Inno.Scene/Core/GameBehavior.cs#L51) | Called once immediately before the first update of this behavior. |
| [`virtual void Inno.Scene.GameBehavior.Update()`](../../src/content/scene/Inno.Scene/Core/GameBehavior.cs#L72) | Called during the variable-rate update stage. Use Time.deltaTime for frame timing. |

### `Inno.Scene.GameComponent`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Scene.Components.Transform Inno.Scene.GameComponent.transform`](../../src/content/scene/Inno.Scene/Core/GameComponent.cs#L32) | Gets the owning game object's transform. |
| [`Inno.Scene.GameComponent`](../../src/content/scene/Inno.Scene/Core/GameComponent.cs#L11) | Base type for data and behavior objects attached to a . |
| [`Inno.Scene.GameObject Inno.Scene.GameComponent.gameObject`](../../src/content/scene/Inno.Scene/Core/GameComponent.cs#L21) | Gets the owning game object. |
| [`virtual void Inno.Scene.GameComponent.Reset()`](../../src/content/scene/Inno.Scene/Core/GameComponent.cs#L37) | Restores this component to its default state when it is added or explicitly reset. |

### `Inno.Scene.GameObject`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Scene.Components.Transform Inno.Scene.GameObject.transform`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L132) | Gets the mandatory transform component. |
| [`Inno.Scene.GameComponent Inno.Scene.GameObject.AddComponent(System.Type componentType)`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L201) | Creates and attaches a component of the requested runtime type. |
| [`Inno.Scene.GameComponent Inno.Scene.GameObject.GetComponent(System.Type componentType)`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L235) | Gets the first attached component assignable to a runtime type. |
| [`Inno.Scene.GameObject`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L14) | Represents a scene-owned object and exposes its component-oriented API. |
| [`Inno.Scene.GameObject? Inno.Scene.GameObject.prefabInstanceRoot`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L166) | Gets the root of this object's prefab instance connection. |
| [`Inno.Scene.GameScene Inno.Scene.GameObject.scene`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L51) | Gets the owning scene. |
| [`Inno.Scene.Layers.GameLayer Inno.Scene.GameObject.layer`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L109) | Gets or sets the single runtime layer used to filter this game object. |
| [`Inno.Scene.PrefabInstanceInfo? Inno.Scene.GameObject.prefabInstance`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L171) | Gets read-only information about this object's prefab source connection. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Scene.GameComponent> Inno.Scene.GameObject.GetComponents()`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L295) | Gets all attached components in attachment order. |
| [`System.Collections.Generic.IReadOnlyList<TComponent> Inno.Scene.GameObject.GetComponents<TComponent>()`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L286) | Gets all attached components assignable to the requested type in attachment order. |
| [`TComponent Inno.Scene.GameObject.AddComponent<TComponent>()`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L190) | Creates and attaches a component of the requested type. |
| [`TComponent Inno.Scene.GameObject.GetComponent<TComponent>()`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L216) | Gets the first attached component assignable to the requested type. |
| [`bool Inno.Scene.GameObject.HasComponent<TComponent>()`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L275) | Gets whether a matching component is attached. |
| [`bool Inno.Scene.GameObject.RemoveComponent(Inno.Scene.GameComponent component)`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L360) | Removes and destroys a specific attached component. |
| [`bool Inno.Scene.GameObject.TryGetComponent<TComponent>(out TComponent? component)`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L255) | Tries to get the first attached component assignable to the requested type. |
| [`bool Inno.Scene.GameObject.activeInHierarchy`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L156) | Gets whether this object is active after parent hierarchy state is applied. |
| [`bool Inno.Scene.GameObject.activeSelf`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L144) | Gets whether this object is explicitly active. |
| [`bool Inno.Scene.GameObject.isPartOfPrefabInstance`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L161) | Gets whether this object retains a prefab source connection. |
| [`bool Inno.Scene.GameObject.isRuntimeValid`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L43) | Gets whether this object is live in its owning scene. |
| [`const string Inno.Scene.GameObject.defaultTag`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L20) | Defines the tag assigned to newly created game objects. |
| [`int Inno.Scene.GameObject.GetComponentIndex(Inno.Scene.GameComponent component)`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L306) | Gets the attachment index of a component on this object. |
| [`string Inno.Scene.GameObject.name`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L56) | Gets or sets the display name stored in the owning scene. |
| [`string Inno.Scene.GameObject.tag`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L84) | Gets or sets the ordinal tag used to categorize and query this game object. |
| [`void Inno.Scene.GameObject.ResetComponent(Inno.Scene.GameComponent component)`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L342) | Restores an attached component to the defaults defined by its optional Reset message. |
| [`void Inno.Scene.GameObject.SetActive(bool value)`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L179) | Changes this object's explicit active state and updates its hierarchy subtree. |
| [`void Inno.Scene.GameObject.SetComponentIndex(Inno.Scene.GameComponent component, int componentIndex)`](../../src/content/scene/Inno.Scene/Core/GameObject.cs#L322) | Moves an attached component to a requested attachment index. The mandatory Transform participates in ordering but remains non-removable. |

### `Inno.Scene.GameScene`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Scene.GameObject Inno.Scene.GameScene.CreateObject(string name = "GameObject")`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L84) | Creates a game object with a mandatory transform component. |
| [`Inno.Scene.GameObject Inno.Scene.GameScene.InstantiatePrefab(Inno.Scene.PrefabAsset prefab, Inno.Scene.Components.Transform? parent = null)`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L105) | Creates a connected instance of an imported prefab in this loaded scene. |
| [`Inno.Scene.GameObject? Inno.Scene.GameScene.FindObject(string name)`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L191) | Finds the first game object with an ordinally matching name. |
| [`Inno.Scene.GameObject? Inno.Scene.GameScene.FindObjectWithLayer(Inno.Scene.Layers.GameLayer layer)`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L243) | Finds the first game object assigned to a layer in scene storage order. |
| [`Inno.Scene.GameObject? Inno.Scene.GameScene.FindObjectWithTag(string tag)`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L210) | Finds the first game object with an ordinally matching tag in scene storage order. |
| [`Inno.Scene.GameScene`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L16) | Owns a runtime scene, including its objects, components, hierarchy, and systems. |
| [`Inno.Scene.GameScene.GameScene(string name = "Untitled Scene")`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L36) | Creates an empty scene. |
| [`Inno.Scene.GameSystem Inno.Scene.GameScene.AddSystem(System.Type systemType)`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L303) | Creates and registers a concrete game system by runtime type. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Scene.GameObject> Inno.Scene.GameScene.FindObjectsWithLayer(Inno.Scene.Layers.GameLayer layer)`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L258) | Finds every game object assigned to one layer in scene storage order. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Scene.GameObject> Inno.Scene.GameScene.FindObjectsWithLayers(Inno.Scene.Layers.GameLayerMask layers)`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L273) | Finds every game object assigned to any layer contained in a mask. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Scene.GameObject> Inno.Scene.GameScene.FindObjectsWithTag(string tag)`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L228) | Finds every game object with an ordinally matching tag in scene storage order. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Scene.GameObject> Inno.Scene.GameScene.GetObjects()`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L176) | Gets all committed live game objects in scene storage order. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Scene.GameSystem> Inno.Scene.GameScene.GetSystems()`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L363) | Gets registered systems in display and serialization order. Explicit values independently control lifecycle execution priority. |
| [`TSystem Inno.Scene.GameScene.AddSystem<TSystem>()`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L288) | Creates and registers a parameterless game system. |
| [`bool Inno.Scene.GameScene.DestroyObject(Inno.Scene.GameObject gameObject)`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L121) | Recursively destroys a game object and its complete child subtree. |
| [`bool Inno.Scene.GameScene.RemoveSystem(Inno.Scene.GameSystem system)`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L336) | Removes a registered game system. |
| [`bool Inno.Scene.GameScene.isLoaded`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L70) | Gets whether this scene is currently owned by . |
| [`int Inno.Scene.GameScene.GetSystemIndex(Inno.Scene.GameSystem system)`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L378) | Gets the display index of a registered system. |
| [`string Inno.Scene.GameScene.name`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L57) | Gets or sets the scene display name. |
| [`void Inno.Scene.GameScene.AddSystem(Inno.Scene.GameSystem system)`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L321) | Registers a game system instance. |
| [`void Inno.Scene.GameScene.ResetSystem(Inno.Scene.GameSystem system)`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L349) | Explicitly restores a registered system to its default state. |
| [`void Inno.Scene.GameScene.SetSystemIndex(Inno.Scene.GameSystem system, int systemIndex)`](../../src/content/scene/Inno.Scene/Core/GameScene.cs#L395) | Moves a registered system to a requested display and serialization index. This operation does not change its explicit . |

### `Inno.Scene.GameSystem`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Scene.GameScene Inno.Scene.GameSystem.scene`](../../src/content/scene/Inno.Scene/Core/GameSystem.cs#L49) | Gets the owning scene after this system has been registered. |
| [`Inno.Scene.GameSystem`](../../src/content/scene/Inno.Scene/Core/GameSystem.cs#L12) | Base type for serializable, ordered scene-level behaviors. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Scene.GameObject> Inno.Scene.GameSystem.GetObjects()`](../../src/content/scene/Inno.Scene/Core/GameSystem.cs#L133) | Gets all committed live game objects in deterministic scene storage order. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Scene.GameObject> Inno.Scene.GameSystem.Query<T1, T2, T3>()`](../../src/content/scene/Inno.Scene/Core/GameSystem.cs#L178) | Queries game objects containing three required component types. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Scene.GameObject> Inno.Scene.GameSystem.Query<T1, T2>()`](../../src/content/scene/Inno.Scene/Core/GameSystem.cs#L158) | Queries game objects containing two required component types. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Scene.GameObject> Inno.Scene.GameSystem.Query<T1>()`](../../src/content/scene/Inno.Scene/Core/GameSystem.cs#L144) | Queries game objects containing one required component type. |
| [`System.Collections.Generic.IReadOnlyList<TComponent> Inno.Scene.GameSystem.GetComponents<TComponent>()`](../../src/content/scene/Inno.Scene/Core/GameSystem.cs#L125) | Gets all scene components assignable to a requested type. |
| [`bool Inno.Scene.GameSystem.enabled`](../../src/content/scene/Inno.Scene/Core/GameSystem.cs#L21) | Gets or sets whether this system participates in scene lifecycle updates. |
| [`bool Inno.Scene.GameSystem.isActiveAndEnabled`](../../src/content/scene/Inno.Scene/Core/GameSystem.cs#L39) | Gets whether this system is registered, enabled, and dispatchable. |
| [`virtual int Inno.Scene.GameSystem.order`](../../src/content/scene/Inno.Scene/Core/GameSystem.cs#L44) | Gets the ascending execution order used by the owning scene. |
| [`virtual void Inno.Scene.GameSystem.Awake()`](../../src/content/scene/Inno.Scene/Core/GameSystem.cs#L63) | Called once before the system first becomes active. |
| [`virtual void Inno.Scene.GameSystem.OnDestroy()`](../../src/content/scene/Inno.Scene/Core/GameSystem.cs#L91) | Called before a system that entered runtime lifecycle is destroyed. |
| [`virtual void Inno.Scene.GameSystem.OnDisable()`](../../src/content/scene/Inno.Scene/Core/GameSystem.cs#L84) | Called when the system stops being active and enabled. |
| [`virtual void Inno.Scene.GameSystem.OnEnable()`](../../src/content/scene/Inno.Scene/Core/GameSystem.cs#L77) | Called when the system becomes active and enabled. |
| [`virtual void Inno.Scene.GameSystem.OnFixedUpdate()`](../../src/content/scene/Inno.Scene/Core/GameSystem.cs#L98) | Called during the fixed-rate scene stage. Use Time.fixedDeltaTime for step timing. |
| [`virtual void Inno.Scene.GameSystem.OnLateUpdate()`](../../src/content/scene/Inno.Scene/Core/GameSystem.cs#L112) | Called during the late scene stage. Use Time.deltaTime for frame timing. |
| [`virtual void Inno.Scene.GameSystem.OnUpdate()`](../../src/content/scene/Inno.Scene/Core/GameSystem.cs#L105) | Called during the variable-rate scene stage. Use Time.deltaTime for frame timing. |
| [`virtual void Inno.Scene.GameSystem.Reset()`](../../src/content/scene/Inno.Scene/Core/GameSystem.cs#L56) | Restores this system to defaults when added or explicitly reset. |
| [`virtual void Inno.Scene.GameSystem.Start()`](../../src/content/scene/Inno.Scene/Core/GameSystem.cs#L70) | Called once immediately before the first update. |

### `Inno.Scene.GameTagCatalog`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Settings.ProjectLocalId Inno.Scene.GameTagCatalog.GetLocalId(string tag)`](../../src/content/scene/Inno.Scene/Core/GameTagCatalog.cs#L52) | Gets the stable project-independent identity of a defined tag. |
| [`Inno.Core.Settings.ProjectScopedId Inno.Scene.GameTagCatalog.GetId(Inno.Core.Settings.ProjectId projectId, string tag)`](../../src/content/scene/Inno.Scene/Core/GameTagCatalog.cs#L72) | Gets the complete project-scoped identity of a defined tag. |
| [`Inno.Scene.GameTagCatalog`](../../src/content/scene/Inno.Scene/Core/GameTagCatalog.cs#L14) | Stores the project-wide tag definitions used to author and validate scene object assignments. |
| [`Inno.Scene.GameTagCatalog Inno.Scene.GameTagCatalog.Clone()`](../../src/content/scene/Inno.Scene/Core/GameTagCatalog.cs#L155) | Creates a detached mutable copy of this catalog. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Scene.GameTagCatalog.GetTags()`](../../src/content/scene/Inno.Scene/Core/GameTagCatalog.cs#L37) | Gets the defined tags with the immutable default tag first. |
| [`bool Inno.Scene.GameTagCatalog.Add(string tag)`](../../src/content/scene/Inno.Scene/Core/GameTagCatalog.cs#L108) | Adds one normalized project tag definition. |
| [`bool Inno.Scene.GameTagCatalog.IsDefined(string tag)`](../../src/content/scene/Inno.Scene/Core/GameTagCatalog.cs#L89) | Determines whether an ordinal tag is defined by the project. |
| [`bool Inno.Scene.GameTagCatalog.Remove(string tag)`](../../src/content/scene/Inno.Scene/Core/GameTagCatalog.cs#L134) | Removes one custom project tag definition without rewriting scene assignments. |
| [`const string Inno.Scene.GameTagCatalog.settingProtocolId`](../../src/content/scene/Inno.Scene/Core/GameTagCatalog.cs#L21) | Gets the immutable project-setting protocol value used by discovery metadata. |
| [`static Inno.Core.Settings.ProjectSettingId Inno.Scene.GameTagCatalog.settingId`](../../src/content/scene/Inno.Scene/Core/GameTagCatalog.cs#L29) | Gets the stable project setting protocol for the project-wide tag catalog. |

### `Inno.Scene.ISceneReloadStateTransfer`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Scene.ISceneReloadStateTransfer`](../../src/content/scene/Inno.Scene/Reloading/ISceneReloadStateTransfer.cs#L9) | Represents a staged scene object state transfer associated with one assembly reload transaction. |
| [`System.Collections.Generic.IReadOnlyList<Inno.References.ReferenceRecoveryChange> Inno.Scene.ISceneReloadStateTransfer.recoveryChanges`](../../src/content/scene/Inno.Scene/Reloading/ISceneReloadStateTransfer.cs#L24) | Gets the shared candidate resolutions for actual scene element and asset dependency slots after application. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Scene.SceneReloadDiagnostic> Inno.Scene.ISceneReloadStateTransfer.diagnostics`](../../src/content/scene/Inno.Scene/Reloading/ISceneReloadStateTransfer.cs#L19) | Gets non-fatal state transfer decisions produced while replacing reloadable scene objects. |
| [`System.Collections.Generic.IReadOnlyList<object> Inno.Scene.ISceneReloadStateTransfer.retiredObjects`](../../src/content/scene/Inno.Scene/Reloading/ISceneReloadStateTransfer.cs#L14) | Gets old scene objects whose runtime types belong to the retiring assembly generation. |
| [`void Inno.Scene.ISceneReloadStateTransfer.Apply()`](../../src/content/scene/Inno.Scene/Reloading/ISceneReloadStateTransfer.cs#L34) | Creates replacement instances and restores their serialized state. |
| [`void Inno.Scene.ISceneReloadStateTransfer.Complete()`](../../src/content/scene/Inno.Scene/Reloading/ISceneReloadStateTransfer.cs#L49) | Finalizes replacement instances after the assembly reload is committed. |
| [`void Inno.Scene.ISceneReloadStateTransfer.PrepareForActivation()`](../../src/content/scene/Inno.Scene/Reloading/ISceneReloadStateTransfer.cs#L29) | Disables active retiring lifecycle objects before the new assembly generation becomes active. |
| [`void Inno.Scene.ISceneReloadStateTransfer.RestorePreviousState()`](../../src/content/scene/Inno.Scene/Reloading/ISceneReloadStateTransfer.cs#L44) | Restores lifecycle state on the previous scene instances after rollback. |
| [`void Inno.Scene.ISceneReloadStateTransfer.RollbackStructure()`](../../src/content/scene/Inno.Scene/Reloading/ISceneReloadStateTransfer.cs#L39) | Restores the previous scene structure after a failed state transfer. |

### `Inno.Scene.Layers.GameLayer`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Scene.Layers.GameLayer`](../../src/content/scene/Inno.Scene/Layers/GameLayer.cs#L8) | Identifies one of the thirty-two runtime layers available to scene objects. |
| [`Inno.Scene.Layers.GameLayer.GameLayer(int index)`](../../src/content/scene/Inno.Scene/Layers/GameLayer.cs#L29) | Creates a layer identifier from a zero-based layer index. |
| [`bool Inno.Scene.Layers.GameLayer.Equals(Inno.Scene.Layers.GameLayer other)`](../../src/content/scene/Inno.Scene/Layers/GameLayer.cs#L61) | Determines whether another layer identifies the same index. |
| [`const int Inno.Scene.Layers.GameLayer.C_MAX_COUNT`](../../src/content/scene/Inno.Scene/Layers/GameLayer.cs#L13) | Defines the number of layer slots supported by the runtime bit-mask representation. |
| [`int Inno.Scene.Layers.GameLayer.CompareTo(Inno.Scene.Layers.GameLayer other)`](../../src/content/scene/Inno.Scene/Layers/GameLayer.cs#L50) | Compares this layer with another layer by index. |
| [`int Inno.Scene.Layers.GameLayer.index`](../../src/content/scene/Inno.Scene/Layers/GameLayer.cs#L39) | Gets the zero-based layer index. |
| [`override bool Inno.Scene.Layers.GameLayer.Equals(object? obj)`](../../src/content/scene/Inno.Scene/Layers/GameLayer.cs#L72) | Determines whether this instance and the supplied value represent the same logical state. |
| [`override int Inno.Scene.Layers.GameLayer.GetHashCode()`](../../src/content/scene/Inno.Scene/Layers/GameLayer.cs#L80) | Computes a hash code from the fields that participate in logical equality. |
| [`override string Inno.Scene.Layers.GameLayer.ToString()`](../../src/content/scene/Inno.Scene/Layers/GameLayer.cs#L88) | Formats this value as a human-readable representation. |
| [`static Inno.Scene.Layers.GameLayer Inno.Scene.Layers.GameLayer.defaultLayer`](../../src/content/scene/Inno.Scene/Layers/GameLayer.cs#L18) | Gets the built-in default layer stored in slot zero. |
| [`static bool Inno.Scene.Layers.GameLayer.operator !=(Inno.Scene.Layers.GameLayer left, Inno.Scene.Layers.GameLayer right)`](../../src/content/scene/Inno.Scene/Layers/GameLayer.cs#L119) | Determines whether two layer identifiers contain different indices. |
| [`static bool Inno.Scene.Layers.GameLayer.operator ==(Inno.Scene.Layers.GameLayer left, Inno.Scene.Layers.GameLayer right)`](../../src/content/scene/Inno.Scene/Layers/GameLayer.cs#L102) | Determines whether two layer identifiers contain the same index. |

### `Inno.Scene.Layers.GameLayerCatalog`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Settings.ProjectLocalId? Inno.Scene.Layers.GameLayerCatalog.GetLocalId(Inno.Scene.Layers.GameLayer layer)`](../../src/content/scene/Inno.Scene/Layers/GameLayerCatalog.cs#L127) | Gets the stable project-independent identity assigned to a layer slot. |
| [`Inno.Scene.Layers.GameLayer Inno.Scene.Layers.GameLayerCatalog.GetLayer(string name)`](../../src/content/scene/Inno.Scene/Layers/GameLayerCatalog.cs#L259) | Resolves an ordinal layer name to its compact slot. |
| [`Inno.Scene.Layers.GameLayerCatalog`](../../src/content/scene/Inno.Scene/Layers/GameLayerCatalog.cs#L15) | Stores the named layer catalog and symmetric layer-interaction matrix used by a project. |
| [`Inno.Scene.Layers.GameLayerCatalog Inno.Scene.Layers.GameLayerCatalog.Clone()`](../../src/content/scene/Inno.Scene/Layers/GameLayerCatalog.cs#L432) | Creates a detached copy of this stack. |
| [`Inno.Scene.Layers.GameLayerCatalog.GameLayerCatalog()`](../../src/content/scene/Inno.Scene/Layers/GameLayerCatalog.cs#L44) | Creates a layer stack containing the immutable default layer. |
| [`Inno.Scene.Layers.GameLayerId? Inno.Scene.Layers.GameLayerCatalog.GetId(Inno.Core.Settings.ProjectId projectId, Inno.Scene.Layers.GameLayer layer)`](../../src/content/scene/Inno.Scene/Layers/GameLayerCatalog.cs#L146) | Gets the complete identity assigned to a layer slot. |
| [`Inno.Scene.Layers.GameLayerMask Inno.Scene.Layers.GameLayerCatalog.GetInteractionMask(Inno.Scene.Layers.GameLayer layer)`](../../src/content/scene/Inno.Scene/Layers/GameLayerCatalog.cs#L368) | Gets the interaction mask assigned to one layer. |
| [`Inno.Scene.Layers.GameLayerMask Inno.Scene.Layers.GameLayerCatalog.GetMask(System.Collections.Generic.IEnumerable<string> names)`](../../src/content/scene/Inno.Scene/Layers/GameLayerCatalog.cs#L275) | Creates a mask from configured ordinal layer names. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Scene.Layers.GameLayerDefinition> Inno.Scene.Layers.GameLayerCatalog.GetDefinitions()`](../../src/content/scene/Inno.Scene/Layers/GameLayerCatalog.cs#L71) | Gets an immutable snapshot of every named layer ordered by slot index. |
| [`bool Inno.Scene.Layers.GameLayerCatalog.CanInteract(Inno.Scene.Layers.GameLayer first, Inno.Scene.Layers.GameLayer second)`](../../src/content/scene/Inno.Scene/Layers/GameLayerCatalog.cs#L386) | Determines whether two layer slots may interact. |
| [`bool Inno.Scene.Layers.GameLayerCatalog.IsDefined(Inno.Scene.Layers.GameLayer layer)`](../../src/content/scene/Inno.Scene/Layers/GameLayerCatalog.cs#L97) | Determines whether a layer slot is defined. |
| [`bool Inno.Scene.Layers.GameLayerCatalog.Remove(Inno.Scene.Layers.GameLayer layer)`](../../src/content/scene/Inno.Scene/Layers/GameLayerCatalog.cs#L347) | Removes a custom layer definition while retaining its numeric slot and interactions. |
| [`bool Inno.Scene.Layers.GameLayerCatalog.TryGetLayer(Inno.Core.Settings.ProjectId projectId, Inno.Scene.Layers.GameLayerId id, out Inno.Scene.Layers.GameLayer layer)`](../../src/content/scene/Inno.Scene/Layers/GameLayerCatalog.cs#L199) | Tries to resolve a qualified identity to its compact runtime slot. |
| [`bool Inno.Scene.Layers.GameLayerCatalog.TryGetLayer(Inno.Core.Settings.ProjectLocalId localId, out Inno.Scene.Layers.GameLayer layer)`](../../src/content/scene/Inno.Scene/Layers/GameLayerCatalog.cs#L166) | Tries to resolve a local identity to its compact runtime slot. |
| [`bool Inno.Scene.Layers.GameLayerCatalog.TryGetLayer(string name, out Inno.Scene.Layers.GameLayer layer)`](../../src/content/scene/Inno.Scene/Layers/GameLayerCatalog.cs#L233) | Tries to resolve an ordinal layer name to its compact slot. |
| [`const string Inno.Scene.Layers.GameLayerCatalog.settingProtocolId`](../../src/content/scene/Inno.Scene/Layers/GameLayerCatalog.cs#L23) | Gets the immutable project-setting protocol value used by discovery metadata. |
| [`int Inno.Scene.Layers.GameLayerCatalog.count`](../../src/content/scene/Inno.Scene/Layers/GameLayerCatalog.cs#L56) | Gets the number of currently named layer slots. |
| [`static Inno.Core.Settings.ProjectSettingId Inno.Scene.Layers.GameLayerCatalog.settingId`](../../src/content/scene/Inno.Scene/Layers/GameLayerCatalog.cs#L39) | Gets the stable project setting protocol for the project-wide layer catalog. |
| [`string? Inno.Scene.Layers.GameLayerCatalog.GetName(Inno.Scene.Layers.GameLayer layer)`](../../src/content/scene/Inno.Scene/Layers/GameLayerCatalog.cs#L112) | Gets the display name assigned to a layer slot. |
| [`void Inno.Scene.Layers.GameLayerCatalog.Define(Inno.Scene.Layers.GameLayer layer, string name)`](../../src/content/scene/Inno.Scene/Layers/GameLayerCatalog.cs#L293) | Defines or renames a project layer without accepting an authored ID. |
| [`void Inno.Scene.Layers.GameLayerCatalog.SetInteraction(Inno.Scene.Layers.GameLayer first, Inno.Scene.Layers.GameLayer second, bool canInteract)`](../../src/content/scene/Inno.Scene/Layers/GameLayerCatalog.cs#L406) | Sets a symmetric interaction pair. |

### `Inno.Scene.Layers.GameLayerDefinition`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Settings.ProjectLocalId Inno.Scene.Layers.GameLayerDefinition.localId`](../../src/content/scene/Inno.Scene/Layers/GameLayerDefinition.cs#L39) | Gets the stable project-independent identity. |
| [`Inno.Scene.Layers.GameLayer Inno.Scene.Layers.GameLayerDefinition.layer`](../../src/content/scene/Inno.Scene/Layers/GameLayerDefinition.cs#L44) | Gets the layer slot represented by this definition. |
| [`Inno.Scene.Layers.GameLayerDefinition`](../../src/content/scene/Inno.Scene/Layers/GameLayerDefinition.cs#L10) | Describes one named layer slot in a snapshot. |
| [`Inno.Scene.Layers.GameLayerDefinition.GameLayerDefinition(Inno.Core.Settings.ProjectLocalId localId, Inno.Scene.Layers.GameLayer layer, string name)`](../../src/content/scene/Inno.Scene/Layers/GameLayerDefinition.cs#L24) | Creates an immutable layer definition. |
| [`Inno.Scene.Layers.GameLayerId Inno.Scene.Layers.GameLayerDefinition.GetId(Inno.Core.Settings.ProjectId projectId)`](../../src/content/scene/Inno.Scene/Layers/GameLayerDefinition.cs#L60) | Resolves the complete identity under a project namespace. |
| [`string Inno.Scene.Layers.GameLayerDefinition.name`](../../src/content/scene/Inno.Scene/Layers/GameLayerDefinition.cs#L49) | Gets the unique ordinal layer name. |

### `Inno.Scene.Layers.GameLayerId`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Scene.Layers.GameLayerId`](../../src/content/scene/Inno.Scene/Layers/GameLayerId.cs#L10) | Identifies one project-scoped game layer independently from its compact runtime slot. |
| [`Inno.Scene.Layers.GameLayerId.GameLayerId(Inno.Core.Settings.ProjectId projectId, Inno.Core.Settings.ProjectLocalId name)`](../../src/content/scene/Inno.Scene/Layers/GameLayerId.cs#L34) | Creates a project-scoped layer identity from its project and local parts. |
| [`Inno.Scene.Layers.GameLayerId.GameLayerId(Inno.Core.Settings.ProjectScopedId value)`](../../src/content/scene/Inno.Scene/Layers/GameLayerId.cs#L18) | Creates a project-scoped layer identity. |
| [`bool Inno.Scene.Layers.GameLayerId.isValid`](../../src/content/scene/Inno.Scene/Layers/GameLayerId.cs#L68) | Gets whether this value contains a usable identity. |
| [`override string Inno.Scene.Layers.GameLayerId.ToString()`](../../src/content/scene/Inno.Scene/Layers/GameLayerId.cs#L76) | Formats this value as a human-readable representation. |
| [`string Inno.Scene.Layers.GameLayerId.value`](../../src/content/scene/Inno.Scene/Layers/GameLayerId.cs#L63) | Gets the canonical projectId.name identity string. |

### `Inno.Scene.Layers.GameLayerMask`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Scene.Layers.GameLayerMask`](../../src/content/scene/Inno.Scene/Layers/GameLayerMask.cs#L9) | Stores a compact set of scene layers for rendering, physics, and query filtering. |
| [`Inno.Scene.Layers.GameLayerMask Inno.Scene.Layers.GameLayerMask.With(Inno.Scene.Layers.GameLayer layer)`](../../src/content/scene/Inno.Scene/Layers/GameLayerMask.cs#L78) | Returns a mask with the supplied layer enabled. |
| [`Inno.Scene.Layers.GameLayerMask Inno.Scene.Layers.GameLayerMask.Without(Inno.Scene.Layers.GameLayer layer)`](../../src/content/scene/Inno.Scene/Layers/GameLayerMask.cs#L89) | Returns a mask with the supplied layer disabled. |
| [`Inno.Scene.Layers.GameLayerMask.GameLayerMask(uint value)`](../../src/content/scene/Inno.Scene/Layers/GameLayerMask.cs#L27) | Creates a mask from its raw thirty-two-bit representation. |
| [`bool Inno.Scene.Layers.GameLayerMask.Contains(Inno.Scene.Layers.GameLayer layer)`](../../src/content/scene/Inno.Scene/Layers/GameLayerMask.cs#L67) | Determines whether the supplied layer is contained in this mask. |
| [`bool Inno.Scene.Layers.GameLayerMask.Equals(Inno.Scene.Layers.GameLayerMask other)`](../../src/content/scene/Inno.Scene/Layers/GameLayerMask.cs#L100) | Determines whether another mask contains the same layer bits. |
| [`override bool Inno.Scene.Layers.GameLayerMask.Equals(object? obj)`](../../src/content/scene/Inno.Scene/Layers/GameLayerMask.cs#L111) | Determines whether this instance and the supplied value represent the same logical state. |
| [`override int Inno.Scene.Layers.GameLayerMask.GetHashCode()`](../../src/content/scene/Inno.Scene/Layers/GameLayerMask.cs#L119) | Computes a hash code from the fields that participate in logical equality. |
| [`override string Inno.Scene.Layers.GameLayerMask.ToString()`](../../src/content/scene/Inno.Scene/Layers/GameLayerMask.cs#L127) | Formats this value as a human-readable representation. |
| [`static Inno.Scene.Layers.GameLayerMask Inno.Scene.Layers.GameLayerMask.FromLayers(System.Collections.Generic.IEnumerable<Inno.Scene.Layers.GameLayer> layers)`](../../src/content/scene/Inno.Scene/Layers/GameLayerMask.cs#L49) | Creates a mask containing the supplied layers. |
| [`static Inno.Scene.Layers.GameLayerMask Inno.Scene.Layers.GameLayerMask.everything`](../../src/content/scene/Inno.Scene/Layers/GameLayerMask.cs#L19) | Gets a mask containing every supported layer. |
| [`static Inno.Scene.Layers.GameLayerMask Inno.Scene.Layers.GameLayerMask.none`](../../src/content/scene/Inno.Scene/Layers/GameLayerMask.cs#L14) | Gets a mask containing no layers. |
| [`static Inno.Scene.Layers.GameLayerMask Inno.Scene.Layers.GameLayerMask.operator &(Inno.Scene.Layers.GameLayerMask left, Inno.Scene.Layers.GameLayerMask right)`](../../src/content/scene/Inno.Scene/Layers/GameLayerMask.cs#L158) | Retains only layer bits enabled in both masks. |
| [`static Inno.Scene.Layers.GameLayerMask Inno.Scene.Layers.GameLayerMask.operator \|(Inno.Scene.Layers.GameLayerMask left, Inno.Scene.Layers.GameLayerMask right)`](../../src/content/scene/Inno.Scene/Layers/GameLayerMask.cs#L141) | Combines the enabled bits from two masks. |
| [`static Inno.Scene.Layers.GameLayerMask Inno.Scene.Layers.GameLayerMask.operator ~(Inno.Scene.Layers.GameLayerMask mask)`](../../src/content/scene/Inno.Scene/Layers/GameLayerMask.cs#L172) | Inverts every layer bit in a mask. |
| [`static bool Inno.Scene.Layers.GameLayerMask.operator !=(Inno.Scene.Layers.GameLayerMask left, Inno.Scene.Layers.GameLayerMask right)`](../../src/content/scene/Inno.Scene/Layers/GameLayerMask.cs#L203) | Determines whether two masks contain different bits. |
| [`static bool Inno.Scene.Layers.GameLayerMask.operator ==(Inno.Scene.Layers.GameLayerMask left, Inno.Scene.Layers.GameLayerMask right)`](../../src/content/scene/Inno.Scene/Layers/GameLayerMask.cs#L186) | Determines whether two masks contain the same bits. |
| [`uint Inno.Scene.Layers.GameLayerMask.value`](../../src/content/scene/Inno.Scene/Layers/GameLayerMask.cs#L35) | Gets the raw thirty-two-bit mask value. |

### `Inno.Scene.MissingGameComponent`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Extensibility.Types.TypeRef Inno.Scene.MissingGameComponent.missingType`](../../src/content/scene/Inno.Scene/Concrete/MissingGameComponent.cs#L45) | Gets the logical identity of the unavailable component type. |
| [`Inno.Scene.MissingGameComponent`](../../src/content/scene/Inno.Scene/Concrete/MissingGameComponent.cs#L17) | Preserves the identity, order, and serialized state of a component whose managed type is unavailable. |
| [`string Inno.Scene.MissingGameComponent.missingTypeName`](../../src/content/scene/Inno.Scene/Concrete/MissingGameComponent.cs#L50) | Gets the last known managed type name for diagnostics and editor presentation. |

### `Inno.Scene.MissingGameSystem`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Extensibility.Types.TypeRef Inno.Scene.MissingGameSystem.missingType`](../../src/content/scene/Inno.Scene/Concrete/MissingGameSystem.cs#L46) | Gets the logical identity of the unavailable system type. |
| [`Inno.Scene.MissingGameSystem`](../../src/content/scene/Inno.Scene/Concrete/MissingGameSystem.cs#L17) | Preserves the identity, order, and serialized state of a scene system whose managed type is unavailable. |
| [`string Inno.Scene.MissingGameSystem.missingTypeName`](../../src/content/scene/Inno.Scene/Concrete/MissingGameSystem.cs#L51) | Gets the last known managed type name for diagnostics and editor presentation. |

### `Inno.Scene.PrefabAsset`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Scene.EngineAssetContent Inno.Scene.PrefabAsset.CaptureContent()`](../../src/content/scene/Inno.Scene/Assets/PrefabAsset.cs#L132) | Captures the immutable runtime payload and dependency descriptors required by the prefab asset pipeline. |
| [`Inno.Scene.GameObject Inno.Scene.PrefabAsset.Instantiate(Inno.Scene.GameScene scene, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Assets.IAssetReferenceResolver assets, Inno.Scene.Components.Transform? parent = null)`](../../src/content/scene/Inno.Scene/Assets/PrefabAsset.cs#L76) | Instantiates this prefab into a scene using newly generated object identities. |
| [`Inno.Scene.PrefabAsset`](../../src/content/scene/Inno.Scene/Assets/PrefabAsset.cs#L17) | Stores a persistent game object subtree that can be instantiated repeatedly. |
| [`static Inno.Scene.PrefabAsset Inno.Scene.PrefabAsset.Capture(Inno.Scene.GameObject root, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Assets.IAssetReferenceResolver assets)`](../../src/content/scene/Inno.Scene/Assets/PrefabAsset.cs#L39) | Captures a game object subtree into a new unsaved prefab asset. |
| [`static Inno.Scene.PrefabAsset Inno.Scene.PrefabAsset.CreateImported(System.ReadOnlySpan<byte> payload, System.Collections.Generic.IReadOnlyList<Inno.Assets.AssetDependency> dependencies)`](../../src/content/scene/Inno.Scene/Assets/PrefabAsset.cs#L111) | Creates an imported prefab asset from validated runtime content produced by the prefab asset pipeline. |

### `Inno.Scene.PrefabInstanceInfo`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Scene.GameObject Inno.Scene.PrefabInstanceInfo.instanceRoot`](../../src/content/scene/Inno.Scene/Prefabs/PrefabInstanceInfo.cs#L47) | Gets the root object of this prefab instance connection. |
| [`Inno.Scene.PrefabInstanceInfo`](../../src/content/scene/Inno.Scene/Prefabs/PrefabInstanceInfo.cs#L10) | Describes the read-only source connection retained by one prefab instance object. |
| [`System.Guid Inno.Scene.PrefabInstanceInfo.sourceAssetId`](../../src/content/scene/Inno.Scene/Prefabs/PrefabInstanceInfo.cs#L37) | Gets the persistent identity of the source prefab asset. |
| [`System.Guid Inno.Scene.PrefabInstanceInfo.sourceObjectId`](../../src/content/scene/Inno.Scene/Prefabs/PrefabInstanceInfo.cs#L42) | Gets the source-local identity represented by this object. |
| [`bool Inno.Scene.PrefabInstanceInfo.isMissing`](../../src/content/scene/Inno.Scene/Prefabs/PrefabInstanceInfo.cs#L62) | Gets whether the source prefab was unavailable during restoration. |
| [`bool Inno.Scene.PrefabInstanceInfo.isRoot`](../../src/content/scene/Inno.Scene/Prefabs/PrefabInstanceInfo.cs#L52) | Gets whether this object is the instance connection root. |
| [`bool Inno.Scene.PrefabInstanceInfo.isVariant`](../../src/content/scene/Inno.Scene/Prefabs/PrefabInstanceInfo.cs#L57) | Gets whether the connection originates from a prefab variant. |
| [`int Inno.Scene.PrefabInstanceInfo.orphanedOverrideCount`](../../src/content/scene/Inno.Scene/Prefabs/PrefabInstanceInfo.cs#L72) | Gets the number of retained overrides that no longer match the current source. |
| [`int Inno.Scene.PrefabInstanceInfo.overrideCount`](../../src/content/scene/Inno.Scene/Prefabs/PrefabInstanceInfo.cs#L67) | Gets the number of retained overrides for this instance connection. |

### `Inno.Scene.SceneAsset`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Scene.EngineAssetContent Inno.Scene.SceneAsset.CaptureContent()`](../../src/content/scene/Inno.Scene/Assets/SceneAsset.cs#L135) | Captures the immutable runtime payload and dependency descriptors required by the scene asset pipeline. |
| [`Inno.Scene.GameScene Inno.Scene.SceneAsset.Instantiate(Inno.Core.Serialization.SerializationRegistry serialization, Inno.Assets.IAssetReferenceResolver assets)`](../../src/content/scene/Inno.Scene/Assets/SceneAsset.cs#L87) | Creates an unloaded runtime scene and acquires its hard asset dependencies. |
| [`Inno.Scene.SceneAsset`](../../src/content/scene/Inno.Scene/Assets/SceneAsset.cs#L15) | Stores imported scene source that can create independent runtime scenes. |
| [`override void Inno.Scene.SceneAsset.OnRuntimePayloadChanged(System.ReadOnlyMemory<byte> previousPayload, System.ReadOnlyMemory<byte> currentPayload)`](../../src/content/scene/Inno.Scene/Assets/SceneAsset.cs#L156) | Rebuilds runtime-derived state after the serialized asset payload changes. |
| [`static Inno.Scene.SceneAsset Inno.Scene.SceneAsset.Capture(Inno.Scene.GameScene scene, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Assets.IAssetReferenceResolver assets)`](../../src/content/scene/Inno.Scene/Assets/SceneAsset.cs#L37) | Captures a runtime scene into a new unsaved scene asset. |
| [`static Inno.Scene.SceneAsset Inno.Scene.SceneAsset.CreateImported(System.ReadOnlySpan<byte> payload, System.Collections.Generic.IReadOnlyList<Inno.Assets.AssetDependency> dependencies)`](../../src/content/scene/Inno.Scene/Assets/SceneAsset.cs#L114) | Creates an imported scene asset from validated runtime content produced by the scene asset pipeline. |
| [`void Inno.Scene.SceneAsset.CaptureFrom(Inno.Scene.GameScene scene, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Assets.IAssetReferenceResolver assets)`](../../src/content/scene/Inno.Scene/Assets/SceneAsset.cs#L60) | Replaces the pending source content with a fresh capture while preserving this asset identity. |

### `Inno.Scene.SceneContentSource`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Scene.SceneContentSource`](../../src/content/scene/Inno.Scene/Content/SceneContentSource.cs#L11) | Projects loaded scenes into the shared content protocol without depending on a rendering or audio model. |
| [`static Inno.References.ContentReadScope Inno.Scene.SceneContentSource.CreateScope(Inno.Scene.SceneWorld world)`](../../src/content/scene/Inno.Scene/Content/SceneContentSource.cs#L22) | Captures the ordered roots and primary identity of one scene world. |
| [`static Inno.References.ContentReadScope Inno.Scene.SceneContentSource.CreateScope(System.Collections.Generic.IEnumerable<Inno.Scene.GameScene> scenes, Inno.Scene.GameScene? activeScene)`](../../src/content/scene/Inno.Scene/Content/SceneContentSource.cs#L40) | Captures an explicit coherent scene presentation selected by a host. |

### `Inno.Scene.SceneElementSerialization`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Scene.SceneElementSerialization`](../../src/content/scene/Inno.Scene/Serialization/SceneElementSerialization.cs#L16) | Recreates individual scene components and systems from stable type and object identities. |
| [`static Inno.Scene.GameComponent Inno.Scene.SceneElementSerialization.RestoreComponent(Inno.Scene.GameObject owner, Inno.Extensibility.Types.TypeRef type, System.Guid persistentId, int componentIndex, System.ReadOnlySpan<byte> stateData, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Assets.IAssetReferenceResolver assets)`](../../src/content/scene/Inno.Scene/Serialization/SceneElementSerialization.cs#L166) | Recreates one component without invoking Reset and restores its persistent properties. |
| [`static Inno.Scene.GameSystem Inno.Scene.SceneElementSerialization.RestoreSystem(Inno.Scene.GameScene scene, Inno.Extensibility.Types.TypeRef type, System.Guid persistentId, int systemIndex, System.ReadOnlySpan<byte> stateData, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Assets.IAssetReferenceResolver assets)`](../../src/content/scene/Inno.Scene/Serialization/SceneElementSerialization.cs#L239) | Recreates one scene system without invoking Reset and restores its persistent properties. |
| [`static byte[] Inno.Scene.SceneElementSerialization.CaptureState(Inno.Scene.EngineObject target, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Assets.IAssetReferenceResolver assets)`](../../src/content/scene/Inno.Scene/Serialization/SceneElementSerialization.cs#L39) | Captures one element's logical type, neutral properties, asset dependencies and scene-reference aliases. |
| [`static void Inno.Scene.SceneElementSerialization.RestoreState(Inno.Scene.EngineObject target, System.ReadOnlySpan<byte> stateData, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Assets.IAssetReferenceResolver assets)`](../../src/content/scene/Inno.Scene/Serialization/SceneElementSerialization.cs#L101) | Restores a captured element state into the same logical type, including neutral state on a missing placeholder. |

### `Inno.Scene.SceneManager`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Scene.SceneManager`](../../src/content/scene/Inno.Scene/SceneManager.cs#L13) | Provides Unity-style scene operations by resolving the world in the current runtime execution context. |
| [`static Inno.Scene.GameScene Inno.Scene.SceneManager.LoadNewScene(string name = "Untitled Scene")`](../../src/content/scene/Inno.Scene/SceneManager.cs#L87) | Creates and loads a new active scene in the current runtime session. |
| [`static Inno.Scene.GameScene Inno.Scene.SceneManager.LoadNewSceneAdditive(string name = "Untitled Scene", bool makeActive = true)`](../../src/content/scene/Inno.Scene/SceneManager.cs#L101) | Creates and additively loads a new scene in the current runtime session. |
| [`static Inno.Scene.GameScene? Inno.Scene.SceneManager.activeScene`](../../src/content/scene/Inno.Scene/SceneManager.cs#L18) | Gets the active scene in the current runtime session. |
| [`static System.Collections.Generic.IReadOnlyList<Inno.Scene.GameScene> Inno.Scene.SceneManager.loadedScenes`](../../src/content/scene/Inno.Scene/SceneManager.cs#L28) | Gets an immutable snapshot of scenes loaded by the current runtime session. |
| [`static bool Inno.Scene.SceneManager.UnloadScene(Inno.Scene.GameScene scene)`](../../src/content/scene/Inno.Scene/SceneManager.cs#L144) | Unloads one scene from the current runtime session. |
| [`static bool Inno.Scene.SceneManager.hasActiveScene`](../../src/content/scene/Inno.Scene/SceneManager.cs#L23) | Gets whether the current runtime session has an active scene. |
| [`static int Inno.Scene.SceneManager.GetSceneIndex(Inno.Scene.GameScene scene)`](../../src/content/scene/Inno.Scene/SceneManager.cs#L39) | Gets the hierarchy index of a scene loaded by the current runtime session. |
| [`static void Inno.Scene.SceneManager.FixedUpdate(float fixedDeltaTime)`](../../src/content/scene/Inno.Scene/SceneManager.cs#L157) | Advances fixed-step scene lifecycle callbacks in the current runtime session. |
| [`static void Inno.Scene.SceneManager.LateUpdate(float deltaTime)`](../../src/content/scene/Inno.Scene/SceneManager.cs#L173) | Advances late scene lifecycle callbacks in the current runtime session. |
| [`static void Inno.Scene.SceneManager.LoadScene(Inno.Scene.GameScene scene)`](../../src/content/scene/Inno.Scene/SceneManager.cs#L61) | Replaces the current session scene set with one active scene. |
| [`static void Inno.Scene.SceneManager.LoadSceneAdditive(Inno.Scene.GameScene scene, bool makeActive = true)`](../../src/content/scene/Inno.Scene/SceneManager.cs#L72) | Loads a scene alongside the current session scene set. |
| [`static void Inno.Scene.SceneManager.MoveGameObjectToScene(Inno.Scene.GameObject gameObject, Inno.Scene.GameScene destination)`](../../src/content/scene/Inno.Scene/SceneManager.cs#L124) | Moves a live object subtree into another scene owned by the current runtime session. |
| [`static void Inno.Scene.SceneManager.SetActiveScene(Inno.Scene.GameScene scene)`](../../src/content/scene/Inno.Scene/SceneManager.cs#L113) | Makes a loaded scene active in the current runtime session. |
| [`static void Inno.Scene.SceneManager.SetSceneIndex(Inno.Scene.GameScene scene, int sceneIndex)`](../../src/content/scene/Inno.Scene/SceneManager.cs#L50) | Moves a loaded scene to a hierarchy index without changing the active scene. |
| [`static void Inno.Scene.SceneManager.UnloadActiveScene()`](../../src/content/scene/Inno.Scene/SceneManager.cs#L133) | Unloads the active scene in the current runtime session when one exists. |
| [`static void Inno.Scene.SceneManager.UnloadAllScenes()`](../../src/content/scene/Inno.Scene/SceneManager.cs#L149) | Unloads every scene from the current runtime session. |
| [`static void Inno.Scene.SceneManager.Update(float deltaTime)`](../../src/content/scene/Inno.Scene/SceneManager.cs#L165) | Advances variable-step scene lifecycle callbacks in the current runtime session. |

### `Inno.Scene.ScenePropertySerialization`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Scene.ScenePropertySerialization`](../../src/content/scene/Inno.Scene/Serialization/ScenePropertySerialization.cs#L14) | Captures and restores individual scene-object properties while preserving scene reference identities. |
| [`static Inno.Core.Serialization.SerializationPropertyRestoreResult Inno.Scene.ScenePropertySerialization.RestoreProperties(Inno.Scene.EngineObject target, System.ReadOnlySpan<byte> data, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Assets.IAssetReferenceResolver assets, Inno.Core.Serialization.SerializationPropertyRestoreMode mode = Inno.Core.Serialization.SerializationPropertyRestoreMode.Strict)`](../../src/content/scene/Inno.Scene/Serialization/ScenePropertySerialization.cs#L156) | Restores independently captured properties into a live scene object. |
| [`static System.Collections.Generic.IReadOnlyList<Inno.Core.Serialization.SerializationPropertySnapshot> Inno.Scene.ScenePropertySerialization.CapturePropertySnapshots(Inno.Scene.EngineObject target, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Assets.IAssetReferenceResolver assets)`](../../src/content/scene/Inno.Scene/Serialization/ScenePropertySerialization.cs#L113) | Captures each persistent property independently while preserving scene-reference identities. |
| [`static byte[] Inno.Scene.ScenePropertySerialization.CaptureProperties(Inno.Scene.EngineObject target, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Assets.IAssetReferenceResolver assets)`](../../src/content/scene/Inno.Scene/Serialization/ScenePropertySerialization.cs#L82) | Captures all persistent properties without serializing the complete scene. |
| [`static byte[] Inno.Scene.ScenePropertySerialization.CaptureProperty(Inno.Scene.EngineObject target, string propertyName, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Assets.IAssetReferenceResolver assets)`](../../src/content/scene/Inno.Scene/Serialization/ScenePropertySerialization.cs#L43) | Captures one persistent property without serializing the complete scene. |

### `Inno.Scene.SceneReloadDiagnostic`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Diagnostics.DiagnosticSeverity Inno.Scene.SceneReloadDiagnostic.severity`](../../src/content/scene/Inno.Scene/Reloading/SceneReloadDiagnostic.cs#L39) | Gets the diagnostic severity. |
| [`Inno.Scene.SceneReloadDiagnostic`](../../src/content/scene/Inno.Scene/Reloading/SceneReloadDiagnostic.cs#L9) | Describes a non-fatal issue encountered while migrating scene state to a new assembly generation. |
| [`System.Guid Inno.Scene.SceneReloadDiagnostic.objectPersistentId`](../../src/content/scene/Inno.Scene/Reloading/SceneReloadDiagnostic.cs#L54) | Gets the persistent identity of the affected component or system. |
| [`System.Guid Inno.Scene.SceneReloadDiagnostic.scenePersistentId`](../../src/content/scene/Inno.Scene/Reloading/SceneReloadDiagnostic.cs#L49) | Gets the persistent identity of the affected scene. |
| [`string Inno.Scene.SceneReloadDiagnostic.code`](../../src/content/scene/Inno.Scene/Reloading/SceneReloadDiagnostic.cs#L34) | Gets the stable diagnostic code. |
| [`string Inno.Scene.SceneReloadDiagnostic.currentPropertyType`](../../src/content/scene/Inno.Scene/Reloading/SceneReloadDiagnostic.cs#L69) | Gets the current declared property type name. |
| [`string Inno.Scene.SceneReloadDiagnostic.message`](../../src/content/scene/Inno.Scene/Reloading/SceneReloadDiagnostic.cs#L44) | Gets the human-readable diagnostic message. |
| [`string Inno.Scene.SceneReloadDiagnostic.previousPropertyType`](../../src/content/scene/Inno.Scene/Reloading/SceneReloadDiagnostic.cs#L64) | Gets the previous declared property type name. |
| [`string Inno.Scene.SceneReloadDiagnostic.propertyName`](../../src/content/scene/Inno.Scene/Reloading/SceneReloadDiagnostic.cs#L59) | Gets the incompatible serialized property name. |

### `Inno.Scene.SceneReloadService`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Scene.ISceneReloadStateTransfer Inno.Scene.SceneReloadService.Capture(Inno.Extensibility.Types.TypeCacheReloadContext context)`](../../src/content/scene/Inno.Scene/Reloading/SceneReloadService.cs#L53) | Captures all loaded scene objects affected by a prepared type-cache reload. |
| [`Inno.Scene.SceneReloadService`](../../src/content/scene/Inno.Scene/Reloading/SceneReloadService.cs#L12) | Creates scene state-transfer transactions for assembly generation changes. |
| [`Inno.Scene.SceneReloadService.SceneReloadService(Inno.Scene.SceneWorld world, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Assets.IAssetReferenceResolver assets)`](../../src/content/scene/Inno.Scene/Reloading/SceneReloadService.cs#L34) | Creates a scene reload service bound to one serialization generation owner. |

### `Inno.Scene.SceneSubtreeSerialization`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Scene.SceneSubtreeSerialization`](../../src/content/scene/Inno.Scene/Serialization/SceneSubtreeSerialization.cs#L14) | Captures and restores one GameObject subtree with its persistent object and component identities. |
| [`static Inno.Scene.GameObject Inno.Scene.SceneSubtreeSerialization.Restore(Inno.Scene.GameScene scene, System.ReadOnlySpan<byte> data, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Assets.IAssetReferenceResolver assets, Inno.Scene.Components.Transform? parent, int siblingIndex)`](../../src/content/scene/Inno.Scene/Serialization/SceneSubtreeSerialization.cs#L81) | Restores a previously captured subtree into a loaded scene with its original persistent identities. |
| [`static byte[] Inno.Scene.SceneSubtreeSerialization.Capture(Inno.Scene.GameObject root, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Assets.IAssetReferenceResolver assets)`](../../src/content/scene/Inno.Scene/Serialization/SceneSubtreeSerialization.cs#L37) | Captures one live GameObject and all descendants without serializing unrelated scene objects. |

### `Inno.Scene.SceneTypeResolutionException`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Scene.SceneTypeResolutionException`](../../src/content/scene/Inno.Scene/Serialization/SceneTypeResolutionException.cs#L9) | Reports that a serialized scene element cannot be created because its stable type is not present in the active type catalog. |
| [`Inno.Scene.SceneTypeResolutionException.SceneTypeResolutionException(System.Guid stableTypeId, string elementKind)`](../../src/content/scene/Inno.Scene/Serialization/SceneTypeResolutionException.cs#L23) | Creates an exception for one unresolved serialized scene type. |
| [`System.Guid Inno.Scene.SceneTypeResolutionException.stableTypeId`](../../src/content/scene/Inno.Scene/Serialization/SceneTypeResolutionException.cs#L39) | Gets the unresolved stable type identity. |
| [`string Inno.Scene.SceneTypeResolutionException.elementKind`](../../src/content/scene/Inno.Scene/Serialization/SceneTypeResolutionException.cs#L44) | Gets the scene element kind that expected the missing type. |

### `Inno.Scene.SceneWorld`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Scene.GameScene Inno.Scene.SceneWorld.LoadNewScene(string name = "Untitled Scene")`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L255) | Creates and loads a new active scene. |
| [`Inno.Scene.GameScene Inno.Scene.SceneWorld.LoadNewSceneAdditive(string name = "Untitled Scene", bool makeActive = true)`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L275) | Creates and additively loads a new scene. |
| [`Inno.Scene.GameScene? Inno.Scene.SceneWorld.activeScene`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L107) | Gets the scene currently selected for unqualified scene operations. |
| [`Inno.Scene.SceneWorld`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L16) | Owns the loaded scene set and lifecycle state for one isolated runtime session. |
| [`Inno.Scene.SceneWorld.SceneWorld(Inno.Core.Identity.IdentityAllocator identities, Inno.Extensibility.Types.TypeCatalog types)`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L41) | Creates an empty scene world bound to one session identity allocator. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Scene.GameScene> Inno.Scene.SceneWorld.loadedScenes`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L117) | Gets an immutable snapshot of loaded scenes in hierarchy order. |
| [`System.IDisposable Inno.Scene.SceneWorld.EnterScope()`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L144) | Binds this world and its identity allocator to the current asynchronous execution context. |
| [`TObject? Inno.Scene.SceneWorld.Find<TObject>(System.Guid persistentId)`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L131) | Resolves a live scene object by its persistent identity within this world. |
| [`bool Inno.Scene.SceneWorld.UnloadScene(Inno.Scene.GameScene scene)`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L350) | Unloads one scene and selects another loaded scene when necessary. |
| [`bool Inno.Scene.SceneWorld.hasActiveScene`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L112) | Gets whether this world contains an active scene. |
| [`int Inno.Scene.SceneWorld.GetSceneIndex(Inno.Scene.GameScene scene)`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L163) | Gets the hierarchy index of a loaded scene. |
| [`void Inno.Scene.SceneWorld.ConfigurePrefabInstantiation(Inno.Core.Serialization.SerializationRegistry serialization, Inno.Assets.IAssetReferenceResolver assets)`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L72) | Binds the session-owned serialization and asset generations used by scene prefab instances. |
| [`void Inno.Scene.SceneWorld.Dispose()`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L439) | Unloads every scene and permanently releases this world. |
| [`void Inno.Scene.SceneWorld.FixedUpdate(float fixedDeltaTime)`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L400) | Advances fixed-step lifecycle callbacks for every loaded scene. |
| [`void Inno.Scene.SceneWorld.LateUpdate(float deltaTime)`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L428) | Advances late lifecycle callbacks for every loaded scene. |
| [`void Inno.Scene.SceneWorld.LoadScene(Inno.Scene.GameScene scene)`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L208) | Replaces the loaded set with one scene and makes it active. |
| [`void Inno.Scene.SceneWorld.LoadSceneAdditive(Inno.Scene.GameScene scene, bool makeActive = true)`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L230) | Loads a scene alongside the existing scene set. |
| [`void Inno.Scene.SceneWorld.MoveGameObjectToScene(Inno.Scene.GameObject gameObject, Inno.Scene.GameScene destination)`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L315) | Moves a live object subtree between two scenes loaded by this world. |
| [`void Inno.Scene.SceneWorld.SetActiveScene(Inno.Scene.GameScene scene)`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L294) | Makes one loaded scene active without changing the loaded set. |
| [`void Inno.Scene.SceneWorld.SetSceneIndex(Inno.Scene.GameScene scene, int sceneIndex)`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L185) | Moves a loaded scene to a hierarchy index without changing the active scene. |
| [`void Inno.Scene.SceneWorld.UnloadActiveScene()`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L334) | Unloads the active scene when one exists. |
| [`void Inno.Scene.SceneWorld.UnloadAllScenes()`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L372) | Unloads every scene while preserving the first lifecycle failure. |
| [`void Inno.Scene.SceneWorld.Update(float deltaTime)`](../../src/content/scene/Inno.Scene/SceneWorld.cs#L414) | Advances variable-step lifecycle callbacks for every loaded scene. |

## 项目依赖

- [Inno.Extensibility.Reload](../extensibility/Inno.Extensibility.Reload.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Collections](../core/Inno.Core.Collections.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Identity](../core/Inno.Core.Identity.md)：公开引用边界由实际签名核对。
- [Inno.Core.Mathematics](../core/Inno.Core.Mathematics.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：公开引用边界由实际签名核对。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：公开引用边界由实际签名核对。
- [Inno.Core.Settings](../core/Inno.Core.Settings.md)：公开引用边界由实际签名核对。
- [Inno.Assets](../assets/Inno.Assets.md)：公开引用边界由实际签名核对。
- [Inno.References](../references/Inno.References.md)：公开引用边界由实际签名核对。
- [Inno.Core.Diagnostics](../core/Inno.Core.Diagnostics.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
