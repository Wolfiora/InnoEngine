# Inno.Editor.Assets

[Editor 索引](README.md) · [Wiki 首页](../README.md) · [Material](Inno.Editor.Shaders.md) · [Pipeline](Inno.Editor.Rendering.md)

## 职责与边界

提供原生资产草稿的可复用生命周期，以及 File Browser 可发现的资产创建模板协议。Material 与 Pipeline 共用草稿实现；本项目不认识 Shader、Sprite 或具体插件配置。
资产源编解码、原子保存和引用上下文属于 AssetPipeline/AssetSourceStore，文档与撤销属于 EditorInteractions。
本项目不另建资产数据库、文档服务或 Undo 栈。

## Asset 创建扩展

`AssetCreationTemplate` 的派生关系是发现协议；`AssetCreationMenuAttribute` 只携带无法从 `AssetObject` 推导的稳定模板 ID、菜单路径、扩展名、默认文件名和排序。File Browser Registry 构造并持有全部模板，因此模板本身不重复声明实例 ID。一个插件新增资产类型时不需要修改 File Browser 中央分支：

```csharp
using InnoEditor.Assets;
using InnoEngine.Assets;
using InnoEngine.Serialization;

public sealed class DialogueAsset : AssetObject
{
    [SerializableProperty]
    public string text { get; set; } = string.Empty;
}

[AssetCreationMenu("example.asset-create.dialogue", "Gameplay/Dialogue", ".dialogue", "New Dialogue", groupOrder: 400)]
public sealed class DialogueCreationTemplate : AssetCreationTemplate<DialogueAsset>
{
}
```

`AssetCreationTemplate<TAsset>` 默认构造 `TAsset` 并调用 `AssetCreationContext.EncodeNative`，适合 Inno 原生结构化源。自定义文本或二进制 Importer 可 override `Encode`，因此协议支持任意具体 `AssetObject`，并不要求所有资产共享一种源格式。模板必须返回其声明的精确类型；空返回、重复 ID、重复菜单路径、非法扩展名或错误类型会使候选 Registry 明确失败，不会部分发布菜单。

创建模板与 `EditorAction` 不竞争职责。`AssetCreationMenu` Registry 只提供动态菜单模型与初始内容；每个菜单项都把模板 ID 作为参数派发给同一个 `CreateAssetCommand : EditorAction<string, string>`。因此快捷键、enabled 查询、目标目录验证和实际写盘仍只有一条 Action 执行链，新增资产类型不会新增一套交互系统。

“继承 `AssetObject`”本身不会自动生成 Create 项：扩展名、默认内容和是否存在有意义的空白资产无法可靠推导。Texture、Geometry、AudioClip、普通 Text/Binary 等外部导入资产应通过导入文件产生，不注册空白模板；Scene/Prefab 继续由各自的领域工作流创建。这里是语义分类，不是 File Browser 的类型白名单。

## Importer 与 Build Processor 身份

`AssetImporter<TAsset>` 和 `AssetBuildProcessor<TDefinition>` 只表达行为契约；可发现实现的不可变协议 ID 分别写在 `[AssetImporter(id)]` 与 `[AssetBuildProcessor(id)]` 中。Registry 先读取 Attribute、验证类型与重复 ID，再构造并绑定实例，因此扩展不再通过 `override importerId` 或 `override processorId` 返回常量。扩展名、部署范围和构建行为仍属于实例能力。重复 ID、重复扩展名或同一 definition 的多个 processor 都会使候选 Registry 整体失败。

## 初始化与公开 API

Feature Module 在获得服务后创建 `AssetDraftDocuments<TAsset>(assets, serialization, interactions, providerId, historyKind, extension, label)`。
TAsset 必须是原生 AssetObject。providerId 和 historyKind 是稳定协议 ID，extension 包括前导点。
Feature 必须提供相同 historyKind 的 EditorHistoryHandler；不能保存回调式 Undo。

| API | 语义 |
| --- | --- |
| `Start()` | 注册共享文档 Provider；重复启动明确失败 |
| `Open(path)` | 打开来源与身份，不要求导入成功；不显示第二个 Inspector |
| `Read(assetId)` | 解码独立值；引用的 canonical 资产仍是只读输入 |
| `Replace(assetId, candidate, finishGesture)` | 修改草稿；手势结束记录一次 History，不写源 |
| `Commit(assetId)` | 结束手势，不保存 |
| `ReplaceMany(candidates, finishGesture)` / `CommitMany(ids)` | 多文档共用一次 History 事务；不是跨文件原子保存 |
| `GetDraft(assetId)` | 获取不含 Asset/Type/delegate 的中立状态对象 |
| `TouchInspection(assetId)` | 标记当前帧仍参与编辑；离开 Inspector 后完成遗留手势 |
| `ValidateHistory(change, direction)` / `ApplyHistory(change, direction)` | Feature 的注册 Handler 路由公共 History 协议，不独立移动历史栈 |
| `Update()` | 处理明确保存后的导入、源变动和离开 Inspector 的手势 |
| `Dispose()` | 保留脏草稿恢复数据，注销当前 Provider |

创建协议的公开 API：

| API | 语义 |
| --- | --- |
| `AssetCreationMenuAttribute` | 声明稳定模板 ID、分层菜单路径、源扩展名、默认名称和组排序 |
| `AssetCreationTemplate` | 通过派生被发现，创建精确的 detached `AssetObject`，并允许覆盖源编码 |
| `AssetCreationTemplate<TAsset>` | 使用默认构造和原生结构化编码的常用实现 |
| `AssetCreationContext.EncodeNative` | 使用当前 AssetPipeline 的完整引用上下文编码原生结构化源 |
| `EditorAssets.EncodeNative` / `DecodeNative<TAsset>` | 使用当前 AssetPipeline 引用上下文捕获、恢复独立的原生资产值；供 Inspector 草稿与 reload-safe History 使用 |

嵌套 `Draft` 只有公开只读属性：`id`、`documentId`、`path`、`readOnly`、`error`、`isDirty`。
它保存中立 bytes，不保存解码对象；状态属性不能绕过 Store 修改数据。

```csharp
using System;
using Inno.Assets;
using Inno.Editor.Assets;

static void ReplaceEditedDraft<T>(
    AssetDraftDocuments<T> documents,
    AssetPath path,
    Action<T> editDetachedDraft
) where T : AssetObject
{
    var id = documents.Open(path);
    T draft = documents.Read(id);
    editDetachedDraft(draft);
    documents.Replace(id, draft);
}
```

## Save、恢复与生命周期

Save/Revert/关闭脏文档使用共享 Document Service。Save 原子写源，下一次 Update 导入；导入失败明确显示已保存但失败。
Revert 读取最新源作为新基线并进入 History。Undo/Redo 只改草稿，再次 Save 才应用到运行时。
恢复文件位于 `Library/Editor/AssetDrafts/<providerId>/<persistent-id>.inno`，包含源指纹、基线和草稿。
外部冲突保留草稿且拒绝覆盖源。恢复写入失败时，内存草稿和历史保留，并在状态中明确报告风险。

Store 不保留插件资产实例、设置实例或 Type；消费者在每次绘制时 Read 并结束引用。
缺失源保留 bytes/history；缺失设置由领域层呈现，不清除中立属性。
只读安装资产禁止 Replace/Save。选择切换不是 Save；关闭确认由无可见 Panel 的共享 Document Service 负责。

模板使用默认 `Encode`，经所属 source 的完整 SerializationContext 保存结构化资产。对应扩展还须提供使用同一格式的 importer；天然文本源才应覆盖编码，不能用自定义文本或 JSON 旁路保存结构化资产。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Editor.Assets.AssetCreationContext`

<!-- api: T:Inno.Editor.Assets.AssetCreationContext 8372d15d0451f5c727ff15d1175b2b41275b2681333d41322715ff53d0583da5 -->
<!-- api: M:Inno.Editor.Assets.AssetCreationContext.#ctor(Inno.Assets.Pipeline.AssetSourceStore) fdf680f5ec63af4ac22383e9fc19b6926babaa74430148570354d54fc7e2c394 -->
<!-- api: M:Inno.Editor.Assets.AssetCreationContext.EncodeNative(Inno.Assets.AssetObject) bfc353e4b213cfb83dedf4df5449e0decb9b8b67669c4fec4735e6dedf99c2f3 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-1d0b9bde8f5a28eb"></a>[`public Inno.Editor.Assets.AssetCreationContext.AssetCreationContext(Inno.Assets.Pipeline.AssetSourceStore sources)`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L180) | Creates a context over the source serializer owned by the active Asset pipeline.<br>`sources`：Source serializer carrying the active reference context. | — | — |
| <a id="api-6663a476394cff08"></a>[`public byte[] Inno.Editor.Assets.AssetCreationContext.EncodeNative(Inno.Assets.AssetObject asset)`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L191) | Encodes an Asset value through the current native structured source serializer.<br>返回：Complete native source bytes, including the concrete serialized type identity.<br>`asset`：Detached Asset value to encode. | — | — |
| <a id="api-6e7e027cad6396ae"></a>[`public sealed class Inno.Editor.Assets.AssetCreationContext`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L169) | Provides source encoding services to one detached Asset creation template invocation. | — | — |

### `Inno.Editor.Assets.AssetCreationMenuAttribute`

<!-- api: T:Inno.Editor.Assets.AssetCreationMenuAttribute 995d5d6ca4d9148fdc9e0882c22d536f97febedf34ac179f40c1ca7a1d802ed1 -->
<!-- api: M:Inno.Editor.Assets.AssetCreationMenuAttribute.#ctor(System.String,System.String,System.String,System.String,System.Int32,System.Int32,System.Boolean) 40d1b7173d7c21946a366ccff24a46ed25eb6be7fe755b190b1d3d786eec4e40 -->
<!-- api: P:Inno.Editor.Assets.AssetCreationMenuAttribute.id 9c8b6939c9319c6cbc0ae7577efde36ed19e78fd5650cc41d6430ac27ee64e54 -->
<!-- api: P:Inno.Editor.Assets.AssetCreationMenuAttribute.menuPath b11c8d6ecc208b16197c824855c5da581dab890f0ccb5c89b1f7ac2bdef81c9a -->
<!-- api: P:Inno.Editor.Assets.AssetCreationMenuAttribute.extension d1e9c4f8425f01c701727a4041b5ba6a07ff9ee609a06f43e23872bd6c7c17b7 -->
<!-- api: P:Inno.Editor.Assets.AssetCreationMenuAttribute.defaultName fb51fb2582dbc1b4621880ed7d6ffb5578875f6ab73730d40ff71f8ff3a86be7 -->
<!-- api: P:Inno.Editor.Assets.AssetCreationMenuAttribute.groupOrder 6a9e9d7cdf852de20fc1fe25f98f935b78fe8a85ef2dbc4a213c30749796aef3 -->
<!-- api: P:Inno.Editor.Assets.AssetCreationMenuAttribute.itemOrder ca252a154ad3498ae95dd6fd731f221fd7dbe6cc93d36ee06f7003df9288d323 -->
<!-- api: P:Inno.Editor.Assets.AssetCreationMenuAttribute.separatorBeforeGroup 70cb0225e932b7e83e6e39f4163d4e0253cbdb56f907c85784d556b581999014 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-762c7b79b6d33359"></a>[`public Inno.Editor.Assets.AssetCreationMenuAttribute.AssetCreationMenuAttribute(string id, string menuPath, string extension, string defaultName, int groupOrder = 0, int itemOrder = 0, bool separatorBeforeGroup = false)`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L42) | Creates one asset creation menu declaration.<br>`id`：Globally stable template identifier.<br>`menuPath`：Slash-delimited path below the File Browser Create menu.<br>`extension`：Native source extension, including its leading dot.<br>`defaultName`：Default source name without an extension or numeric suffix.<br>`groupOrder`：Stable order of the top-level category below Create.<br>`itemOrder`：Stable order of the leaf within its immediate category.<br>`separatorBeforeGroup`：Whether the top-level category starts after a separator. | — | `System.ArgumentException`：Thrown when an identifier, path, source name, or native extension is invalid. |
| <a id="api-60e3d5b4c2e448a4"></a>[`public string Inno.Editor.Assets.AssetCreationMenuAttribute.defaultName { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L99) | Gets the default source name without an extension or numeric suffix. | — | — |
| <a id="api-cc25f639e96f1bb3"></a>[`public string Inno.Editor.Assets.AssetCreationMenuAttribute.extension { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L94) | Gets the native source extension, including its leading dot. | — | — |
| <a id="api-8d2386a0d63fa904"></a>[`public int Inno.Editor.Assets.AssetCreationMenuAttribute.groupOrder { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L104) | Gets the stable order of the top-level category below Create. | — | — |
| <a id="api-19d9b5902aaf8326"></a>[`public string Inno.Editor.Assets.AssetCreationMenuAttribute.id { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L84) | Gets the globally stable template identifier. | — | — |
| <a id="api-9a7a0f4eb6662e1a"></a>[`public int Inno.Editor.Assets.AssetCreationMenuAttribute.itemOrder { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L109) | Gets the stable order of the leaf within its immediate category. | — | — |
| <a id="api-b486f7196cb4e58f"></a>[`public string Inno.Editor.Assets.AssetCreationMenuAttribute.menuPath { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L89) | Gets the slash-delimited path below the File Browser Create menu. | — | — |
| <a id="api-2186832ddf917cf6"></a>[`public bool Inno.Editor.Assets.AssetCreationMenuAttribute.separatorBeforeGroup { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L114) | Gets whether the top-level category starts after a separator. | — | — |
| <a id="api-fbdd5a649b6ec91c"></a>[`public sealed class Inno.Editor.Assets.AssetCreationMenuAttribute : System.Attribute`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L13) | Declares the source name, extension, and menu placement that cannot be inferred from an Inno.Assets.AssetObject type alone. | — | — |

### `Inno.Editor.Assets.AssetCreationTemplate`

<!-- api: T:Inno.Editor.Assets.AssetCreationTemplate f76fcac4ee2d5d3bc9013b771d2fae5f01a4475d2b2304b3bdaec37c33d54f4c -->
<!-- api: P:Inno.Editor.Assets.AssetCreationTemplate.assetType 12b29a03d38df90006287ccbd80c9bb62a10fdf54d24df6edb881771defb3b72 -->
<!-- api: M:Inno.Editor.Assets.AssetCreationTemplate.Create 4695ed9b778798d43578c56c7cf81239b100ce1a7d0a430edd8a4fce898afb2a -->
<!-- api: M:Inno.Editor.Assets.AssetCreationTemplate.Encode(Inno.Editor.Assets.AssetCreationContext,Inno.Assets.AssetObject) 5ce86f0d0303db3ef531ca9fade97820c5b465f5c1e5c9e8dca9210bc0204237 -->
<!-- api: M:Inno.Editor.Assets.AssetCreationTemplate.#ctor 48f4b104630428904ab26fba132218fd5fc74c030e19bd6e627c1f9459134025 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b0d192e9ee7fcc32"></a>[`protected Inno.Editor.Assets.AssetCreationTemplate.AssetCreationTemplate()`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L124) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-fd0e8e7637fbcb40"></a>[`public abstract Inno.Assets.AssetObject Inno.Editor.Assets.AssetCreationTemplate.Create()`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L137) | Creates a detached default value that has not been imported or published.<br>返回：A new asset value owned by the caller. | — | — |
| <a id="api-68fda62e88f5779b"></a>[`public virtual byte[] Inno.Editor.Assets.AssetCreationTemplate.Encode(Inno.Editor.Assets.AssetCreationContext context, Inno.Assets.AssetObject asset)`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L156) | Encodes the detached initial value into the source representation owned by this template.<br>返回：Complete source bytes ready for an atomic File Browser create operation.<br>`context`：Creation services scoped to the current Asset source pipeline.<br>`asset`：The exact value returned by Inno.Editor.Assets.AssetCreationTemplate.Create. | The default implementation uses native structured Asset serialization. Templates paired with a textual or custom binary importer can override this method without changing the File Browser or registering a second creation protocol. | — |
| <a id="api-cb95e8fd13be7458"></a>[`public abstract System.Type Inno.Editor.Assets.AssetCreationTemplate.assetType { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L129) | Gets the concrete asset type produced by this template. | — | — |
| <a id="api-689e3945d5e5c369"></a>[`public abstract class Inno.Editor.Assets.AssetCreationTemplate`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L124) | Produces the detached initial value for one authorable native asset source. | Derivation is the discovery contract. Inno.Editor.Assets.AssetCreationMenuAttribute supplies only source-format and presentation data that the asset inheritance hierarchy cannot express. | — |

### `Inno.Editor.Assets.AssetCreationTemplate`1`

<!-- api: T:Inno.Editor.Assets.AssetCreationTemplate`1 3e57bb39feb155aec3f9a717563ffeccf80f7fda1e0ae74d4e6ea96654162a4d -->
<!-- api: P:Inno.Editor.Assets.AssetCreationTemplate`1.assetType 5f23ca9106522b5521749a5b69b31e68f2bb0af19c5d88a3b9fb470184d2dae0 -->
<!-- api: M:Inno.Editor.Assets.AssetCreationTemplate`1.Create 9e7bd0e285ec73bf9cb1217507e734cc9e60b0fbfc5827365633cec080383503 -->
<!-- api: M:Inno.Editor.Assets.AssetCreationTemplate`1.CreateAsset c1b92988ec54aa596ecb95077c467cb2d0173737cc3184a57af6a2320a70b7f4 -->
<!-- api: M:Inno.Editor.Assets.AssetCreationTemplate`1.#ctor f24f39b18e735c68f5d37bf82600648f60acfaba5e2766b3a8f61d33ada6dd59 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-29021c5db7e2daee"></a>[`protected Inno.Editor.Assets.AssetCreationTemplate<TAsset>.AssetCreationTemplate()`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L204) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-04bf0386a8194175"></a>[`public override Inno.Assets.AssetObject Inno.Editor.Assets.AssetCreationTemplate<TAsset>.Create()`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L218) | Creates a value using this implementation's validated inputs.<br>返回：The validated asset object that represents the completed operation. | — | — |
| <a id="api-fa8c7adc3a1c89e3"></a>[`protected virtual TAsset Inno.Editor.Assets.AssetCreationTemplate<TAsset>.CreateAsset()`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L226) | Creates the initial detached value, allowing specialized deterministic defaults.<br>返回：A new asset value owned by the caller. | — | — |
| <a id="api-427b91599020e97e"></a>[`public override sealed System.Type Inno.Editor.Assets.AssetCreationTemplate<TAsset>.assetType { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L210) | Gets the asset type created by this Editor template. | — | — |
| <a id="api-aa8e1b491243d8a2"></a>[`public abstract class Inno.Editor.Assets.AssetCreationTemplate<TAsset> where TAsset : Inno.Assets.AssetObject, new() : Inno.Editor.Assets.AssetCreationTemplate`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L204) | Creates a default-constructed native source for an authorable asset type. | — | — |

### `Inno.Editor.Assets.AssetDraftDocuments`1`

<!-- api: T:Inno.Editor.Assets.AssetDraftDocuments`1 d36b720aa4455d2d98706325a4b743fdc907c9ca55b3fedb0cdd2b151d38beba -->
<!-- api: M:Inno.Editor.Assets.AssetDraftDocuments`1.#ctor(Inno.Assets.Pipeline.AssetPipeline,Inno.Core.Serialization.SerializationRegistry,Inno.Editor.Interactions.EditorInteractions,System.String,System.String,System.String,System.String) 8ec69f108da762460a7b01338b715f7e4479f24e33a7d84ca0827e3a086503d4 -->
<!-- api: M:Inno.Editor.Assets.AssetDraftDocuments`1.GetDraft(System.Guid) 72731a091d782d6dacedf5c0c58d82547e651cb52faa7b806c15663a51740457 -->
<!-- api: M:Inno.Editor.Assets.AssetDraftDocuments`1.TouchInspection(System.Guid) fe7bc4370f64374aaa312a905f9ab15e6749d8358b006d7cae9eb205f553a9c4 -->
<!-- api: M:Inno.Editor.Assets.AssetDraftDocuments`1.Open(Inno.Assets.AssetPath) 7ed4b3e72a0922668bcdbb25597c540ee3fcdd136bcc4d3683202f7ba2f8ff2e -->
<!-- api: M:Inno.Editor.Assets.AssetDraftDocuments`1.Read(System.Guid) 592046fb4df45bb4d433ca9dedfee386c93924272478beaf38aa1bf7c98d3361 -->
<!-- api: M:Inno.Editor.Assets.AssetDraftDocuments`1.Replace(System.Guid,`0,System.Boolean) 1d5e4cfd5cdcb3ea147c6f5187ee6cf73c4e9f8da5f181bf8eb32bdccaf6bfc9 -->
<!-- api: M:Inno.Editor.Assets.AssetDraftDocuments`1.Commit(System.Guid) e4975c9f7f1e0e6fb28a46e3b8fb4c8189ca76fe00b19882d9e46b3e19ef37f5 -->
<!-- api: M:Inno.Editor.Assets.AssetDraftDocuments`1.ReplaceMany(System.Collections.Generic.IReadOnlyDictionary{System.Guid,`0},System.Boolean) 239b8f06f18a1887d1313335d0800aa513026f180f4618eb1fbff37e3b0265d5 -->
<!-- api: M:Inno.Editor.Assets.AssetDraftDocuments`1.CommitMany(System.Collections.Generic.IEnumerable{System.Guid}) b38e02e02351b0ba94d7ddea14573a73cbaa1a1e2ff48ecd509bb04b433f3dcc -->
<!-- api: M:Inno.Editor.Assets.AssetDraftDocuments`1.ValidateHistory(Inno.Editor.Interactions.EditorHistoryChange,Inno.Editor.Interactions.EditorHistoryDirection) 6d9ae671d464a02a62f298dba3c7cd0b503b687089a1f15d23f268759c45d332 -->
<!-- api: M:Inno.Editor.Assets.AssetDraftDocuments`1.ApplyHistory(Inno.Editor.Interactions.EditorHistoryChange,Inno.Editor.Interactions.EditorHistoryDirection) 43d75245c31b09e34a49ec6ab92dfb03192431bc986d1a4ad2ded4c7d16a1a93 -->
<!-- api: M:Inno.Editor.Assets.AssetDraftDocuments`1.Start 17c71061768f46418ad0e4b755908530b3f60775932636feada5a818371d4b9b -->
<!-- api: M:Inno.Editor.Assets.AssetDraftDocuments`1.Update 00df66ca55682c7bba575cbdef17af28dcbabbb0b3c4a9fc0a783088c811425c -->
<!-- api: M:Inno.Editor.Assets.AssetDraftDocuments`1.Dispose f37ec6451a7a46e80aff54e53f57c812952884ef9e67435a481e4b20d7577eae -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-fd3e60cf4366c89c"></a>[`public Inno.Editor.Assets.AssetDraftDocuments<TAsset>.AssetDraftDocuments(Inno.Assets.Pipeline.AssetPipeline assets, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Editor.Interactions.EditorInteractions interactions, string providerId, string historyKind, string extension, string label)`](../../src/composition/editor/features/Inno.Editor.Assets/Documents/AssetDraftDocuments.cs#L61) | Creates a feature-owned draft store; call Start after document services become available.<br>`assets`：Owner of source identities, serialization and import.<br>`serialization`：Host registry for neutral History and recovery records.<br>`interactions`：Shared documents and History services.<br>`providerId`：Stable unique document provider identifier.<br>`historyKind`：Stable protocol handled by the feature's registered History handler.<br>`extension`：Native source extension including its leading dot.<br>`label`：Human-readable singular asset kind. | — | — |
| <a id="api-1a72deeadfbcd64e"></a>[`public void Inno.Editor.Assets.AssetDraftDocuments<TAsset>.ApplyHistory(Inno.Editor.Interactions.EditorHistoryChange change, Inno.Editor.Interactions.EditorHistoryDirection direction)`](../../src/composition/editor/features/Inno.Editor.Assets/Documents/AssetDraftDocuments.cs#L280) | Applies a previously validated neutral record; failed recovery leaves the draft unchanged.<br>`change`：Record owned by the shared History service.<br>`direction`：Requested traversal direction. | — | — |
| <a id="api-ce03b7b5b974b647"></a>[`public void Inno.Editor.Assets.AssetDraftDocuments<TAsset>.Commit(System.Guid assetId)`](../../src/composition/editor/features/Inno.Editor.Assets/Documents/AssetDraftDocuments.cs#L158) | Finishes the active edit gesture without saving or applying its draft.<br>`assetId`：Open asset identity. | — | — |
| <a id="api-4720284b683399a2"></a>[`public void Inno.Editor.Assets.AssetDraftDocuments<TAsset>.CommitMany(System.Collections.Generic.IEnumerable<System.Guid> assetIds)`](../../src/composition/editor/features/Inno.Editor.Assets/Documents/AssetDraftDocuments.cs#L204) | Commits all selected draft samples in a single shared history transaction.<br>`assetIds`：Open asset identities belonging to this gesture. | — | — |
| <a id="api-671928ddd5b9fa7e"></a>[`public void Inno.Editor.Assets.AssetDraftDocuments<TAsset>.Dispose()`](../../src/composition/editor/features/Inno.Editor.Assets/Documents/AssetDraftDocuments.cs#L481) | Preserves unsaved recovery and unregisters the current feature provider. | — | — |
| <a id="api-eb99746c49c9126b"></a>[`public Inno.Editor.Assets.AssetDraftDocuments<TAsset>.Draft Inno.Editor.Assets.AssetDraftDocuments<TAsset>.GetDraft(System.Guid assetId)`](../../src/composition/editor/features/Inno.Editor.Assets/Documents/AssetDraftDocuments.cs#L94) | Gets host-owned neutral presentation state for an open document.<br>返回：Read-only presentation state; it contains no live asset or extension objects.<br>`assetId`：Persistent source identity. | — | — |
| <a id="api-bc354be2e4f86a2b"></a>[`public System.Guid Inno.Editor.Assets.AssetDraftDocuments<TAsset>.Open(Inno.Assets.AssetPath path)`](../../src/composition/editor/features/Inno.Editor.Assets/Documents/AssetDraftDocuments.cs#L113) | Opens a native asset source in the shared document service without revealing a second Inspector.<br>返回：The persistent asset identity used for subsequent draft operations.<br>`path`：Source path, including sources with failed imports. | — | — |
| <a id="api-01ac309ac2712d8a"></a>[`public TAsset Inno.Editor.Assets.AssetDraftDocuments<TAsset>.Read(System.Guid assetId)`](../../src/composition/editor/features/Inno.Editor.Assets/Documents/AssetDraftDocuments.cs#L132) | Reads a detached current-generation value; callers must never mutate its referenced canonical assets.<br>返回：A detached editable asset whose changes have not been published.<br>`assetId`：Open asset identity. | — | — |
| <a id="api-ea988a22919ada3e"></a>[`public void Inno.Editor.Assets.AssetDraftDocuments<TAsset>.Replace(System.Guid assetId, TAsset candidate, bool finishGesture = true)`](../../src/composition/editor/features/Inno.Editor.Assets/Documents/AssetDraftDocuments.cs#L146) | Updates the open draft through shared History without saving or publishing a runtime asset.<br>`assetId`：Open asset identity.<br>`candidate`：Detached edited asset.<br>`finishGesture`：True completes one history transaction; false continues the same active gesture. | — | — |
| <a id="api-801295a029f8e10a"></a>[`public void Inno.Editor.Assets.AssetDraftDocuments<TAsset>.ReplaceMany(System.Collections.Generic.IReadOnlyDictionary<System.Guid, TAsset> candidates, bool finishGesture = true)`](../../src/composition/editor/features/Inno.Editor.Assets/Documents/AssetDraftDocuments.cs#L169) | Edits compatible selected drafts as one gesture without saving any source.<br>`candidates`：Detached candidate assets indexed by their open asset identities.<br>`finishGesture`：Whether this sample completes the shared gesture. | — | — |
| <a id="api-ce57369af31b1272"></a>[`public void Inno.Editor.Assets.AssetDraftDocuments<TAsset>.Start()`](../../src/composition/editor/features/Inno.Editor.Assets/Documents/AssetDraftDocuments.cs#L407) | Registers the feature provider with the headless document ownership service. | — | — |
| <a id="api-b0619e231fefa48d"></a>[`public void Inno.Editor.Assets.AssetDraftDocuments<TAsset>.TouchInspection(System.Guid assetId)`](../../src/composition/editor/features/Inno.Editor.Assets/Documents/AssetDraftDocuments.cs#L102) | Marks a document as inspected in this frame so a lost gesture can be completed.<br>`assetId`：Persistent source identity. | — | — |
| <a id="api-3dd24657e73211a1"></a>[`public void Inno.Editor.Assets.AssetDraftDocuments<TAsset>.Update()`](../../src/composition/editor/features/Inno.Editor.Assets/Documents/AssetDraftDocuments.cs#L417) | Completes abandoned gestures, imports explicit saves and reconciles source changes. | — | — |
| <a id="api-fe236e51a04108c9"></a>[`public void Inno.Editor.Assets.AssetDraftDocuments<TAsset>.ValidateHistory(Inno.Editor.Interactions.EditorHistoryChange change, Inno.Editor.Interactions.EditorHistoryDirection direction)`](../../src/composition/editor/features/Inno.Editor.Assets/Documents/AssetDraftDocuments.cs#L264) | Validates a feature's neutral History record without changing the draft.<br>`change`：Record owned by the shared History service.<br>`direction`：Requested traversal direction. | — | — |
| <a id="api-59b3edbc8dfa119f"></a>[`public sealed class Inno.Editor.Assets.AssetDraftDocuments<TAsset> where TAsset : Inno.Assets.AssetObject : System.IDisposable`](../../src/composition/editor/features/Inno.Editor.Assets/Documents/AssetDraftDocuments.cs#L20) | Owns detached native asset drafts with shared document, History, recovery and explicit-save semantics. | — | — |

### `Inno.Editor.Assets.AssetDraftDocuments`1.Draft`

<!-- api: T:Inno.Editor.Assets.AssetDraftDocuments`1.Draft 9f352bcabdb8ff706be88f6b70cb22025fa33596ca87730f10a7f90c7e67e5f8 -->
<!-- api: P:Inno.Editor.Assets.AssetDraftDocuments`1.Draft.id 86a280c8858fcc4942263eb262034fef6daed992cdc69b9537d5c9c6900675fd -->
<!-- api: P:Inno.Editor.Assets.AssetDraftDocuments`1.Draft.documentId e187420a9d58efddaf7350b59f29d693280ac0749afa5f86e3c8a2b040b08c2c -->
<!-- api: P:Inno.Editor.Assets.AssetDraftDocuments`1.Draft.path da488af4b485ae18f291bb4af024dfccbc785b56703897de29630e8aab17b68d -->
<!-- api: P:Inno.Editor.Assets.AssetDraftDocuments`1.Draft.readOnly 36e4bba7c758101774fdf9bd6824f75ac6d84f2672f90cb38e7c50721d82710b -->
<!-- api: P:Inno.Editor.Assets.AssetDraftDocuments`1.Draft.error 0d772158f57130402dcff5d04341d48c5d4ca877ac283157bd741a7b7acd6095 -->
<!-- api: P:Inno.Editor.Assets.AssetDraftDocuments`1.Draft.isDirty f1543f393ab7e7d69304415792903664160349c54c1f4a44df8c59cb71b9bed3 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-7d37439995dac066"></a>[`public System.Guid Inno.Editor.Assets.AssetDraftDocuments<TAsset>.Draft.documentId { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Documents/AssetDraftDocuments.cs#L595) | Gets the shared document identity used for Save/Revert and close confirmation. | — | — |
| <a id="api-e5f62feed864966e"></a>[`public string Inno.Editor.Assets.AssetDraftDocuments<TAsset>.Draft.error { get; internal set; }`](../../src/composition/editor/features/Inno.Editor.Assets/Documents/AssetDraftDocuments.cs#L607) | Gets the latest source, save or import diagnostic. | — | — |
| <a id="api-f981a637d217a7e3"></a>[`public System.Guid Inno.Editor.Assets.AssetDraftDocuments<TAsset>.Draft.id { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Documents/AssetDraftDocuments.cs#L591) | Gets the persistent source identity. | — | — |
| <a id="api-1b0aa8f4cad5ebce"></a>[`public bool Inno.Editor.Assets.AssetDraftDocuments<TAsset>.Draft.isDirty { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Documents/AssetDraftDocuments.cs#L611) | Gets whether detached bytes differ from the saved baseline. | — | — |
| <a id="api-5ce9d12a0aa6bd87"></a>[`public Inno.Assets.AssetPath Inno.Editor.Assets.AssetDraftDocuments<TAsset>.Draft.path { get; internal set; }`](../../src/composition/editor/features/Inno.Editor.Assets/Documents/AssetDraftDocuments.cs#L599) | Gets the current resolved source path. | — | — |
| <a id="api-44266f8d20cf960a"></a>[`public bool Inno.Editor.Assets.AssetDraftDocuments<TAsset>.Draft.readOnly { get; internal set; }`](../../src/composition/editor/features/Inno.Editor.Assets/Documents/AssetDraftDocuments.cs#L603) | Gets whether this source is installed read-only content. | — | — |
| <a id="api-63121595a158ace1"></a>[`public sealed class Inno.Editor.Assets.AssetDraftDocuments<TAsset>.Draft`](../../src/composition/editor/features/Inno.Editor.Assets/Documents/AssetDraftDocuments.cs#L564) | Contains neutral draft status; only the owning store can change it. | — | — |

## 项目依赖

- [Inno.Core.IO](../core/Inno.Core.IO.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Editor.Core](Inno.Editor.Core.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Native.ImGui](../backends/ImGui/Inno.Native.ImGui.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Assets](../assets/Inno.Assets.md)：公开引用边界由实际签名核对。
- [Inno.Assets.Pipeline](../assets/Inno.Assets.Pipeline.md)：公开引用边界由实际签名核对。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：公开引用边界由实际签名核对。
- [Inno.Editor.Interactions](Inno.Editor.Interactions.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
