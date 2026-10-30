# Inno.Editor.Assets

[Editor 索引](README.md) · [Wiki 首页](../README.md) · [Material](Inno.Editor.Shaders.md) · [Pipeline](Inno.Editor.Rendering.md)

## 职责与边界

提供原生资产草稿的可复用生命周期，以及 File Browser 可发现的资产创建模板协议。Material 与 Pipeline 共用草稿实现；本项目不认识 Shader、Sprite 或具体插件配置。
资产源编解码、原子保存和引用上下文属于 AssetPipeline/AssetSourceStore，文档与撤销属于 EditorInteractions。
本项目不另建资产数据库、文档服务或 Undo 栈。

## Asset 创建扩展

`AssetCreationTemplate` 的派生关系是发现协议；`AssetCreationMenuAttribute` 只携带无法从 `AssetObject` 推导的稳定模板 ID、菜单路径、扩展名、默认文件名和排序。Assets feature Registry 构造并持有全部模板，因此模板本身不重复声明实例 ID。一个插件新增资产类型时不需要修改 File Browser 中央分支：

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
| `Replace(assetId, candidate, mergeKey)` | 修改草稿；手势结束记录一次 History，不写源 |
| `Commit(assetId)` | 结束手势，不保存 |
| `ReplaceMany(candidates, mergeKey)` / `ReplaceMany(ids)` | 多文档共用一次 History 事务；不是跨文件原子保存 |
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



## 共享编辑入口

AssetEditingModule 拥有创建、删除、重命名、移动、只读校验与 source History；AssetImportSettingsEdits 拥有导入设置；AssetSampleImportModule 复用正式 Sample import。File Browser 只发出交互。

AssetDraftDocuments 的 Replace/ReplaceMany 立即记录中立 bytes，不延迟到下一帧或失焦。连续输入由 EditorEditGesture 开启事务；mergeKey 只标识当前逻辑值手势。Cancel 通过同一个 History 回滚。Save 不再承担补记 History 的职责。

批量删除和移动先验证整个选择，再将被选父目录覆盖的子条目归一化，最后用现有 History transaction 原子执行。后续失败自动恢复先前步骤。安装源始终只读。

## Sample Import 呈现快照

`AssetSampleImportModule.progress` 在原有进度锁内一次捕获 `AssetSampleImportProgress`；后台验证观察者绑定具体操作，退休后不再接收进度。`Cancel(operationId)` 校验当前请求，并在取消底层工作之前发布不可被普通进度覆盖的取消状态。成功状态在发布及 payload 释放完成后设置；History、回滚和依赖退休仍由原事务负责。

## 当前源码公开 API 清单

此清单对应当前 SDK 求值与源码符号；内部类型不构成公开 API。

<!-- api: T:Inno.Editor.Assets.AssetCreationMenuAttribute 995d5d6ca4d9148fdc9e0882c22d536f97febedf34ac179f40c1ca7a1d802ed1 -->
<!-- api: M:Inno.Editor.Assets.AssetCreationMenuAttribute.#ctor(System.String,System.String,System.String,System.String,System.Int32,System.Int32,System.Boolean) 40d1b7173d7c21946a366ccff24a46ed25eb6be7fe755b190b1d3d786eec4e40 -->
<!-- api: P:Inno.Editor.Assets.AssetCreationMenuAttribute.id 9c8b6939c9319c6cbc0ae7577efde36ed19e78fd5650cc41d6430ac27ee64e54 -->
<!-- api: P:Inno.Editor.Assets.AssetCreationMenuAttribute.menuPath b11c8d6ecc208b16197c824855c5da581dab890f0ccb5c89b1f7ac2bdef81c9a -->
<!-- api: P:Inno.Editor.Assets.AssetCreationMenuAttribute.extension d1e9c4f8425f01c701727a4041b5ba6a07ff9ee609a06f43e23872bd6c7c17b7 -->
<!-- api: P:Inno.Editor.Assets.AssetCreationMenuAttribute.defaultName fb51fb2582dbc1b4621880ed7d6ffb5578875f6ab73730d40ff71f8ff3a86be7 -->
<!-- api: P:Inno.Editor.Assets.AssetCreationMenuAttribute.groupOrder 6a9e9d7cdf852de20fc1fe25f98f935b78fe8a85ef2dbc4a213c30749796aef3 -->
<!-- api: P:Inno.Editor.Assets.AssetCreationMenuAttribute.itemOrder ca252a154ad3498ae95dd6fd731f221fd7dbe6cc93d36ee06f7003df9288d323 -->
<!-- api: P:Inno.Editor.Assets.AssetCreationMenuAttribute.separatorBeforeGroup 70cb0225e932b7e83e6e39f4163d4e0253cbdb56f907c85784d556b581999014 -->
<!-- api: T:Inno.Editor.Assets.AssetCreationTemplate f76fcac4ee2d5d3bc9013b771d2fae5f01a4475d2b2304b3bdaec37c33d54f4c -->
<!-- api: P:Inno.Editor.Assets.AssetCreationTemplate.assetType 12b29a03d38df90006287ccbd80c9bb62a10fdf54d24df6edb881771defb3b72 -->
<!-- api: M:Inno.Editor.Assets.AssetCreationTemplate.Create 4695ed9b778798d43578c56c7cf81239b100ce1a7d0a430edd8a4fce898afb2a -->
<!-- api: M:Inno.Editor.Assets.AssetCreationTemplate.Encode(Inno.Editor.Assets.AssetCreationContext,Inno.Assets.AssetObject) 5ce86f0d0303db3ef531ca9fade97820c5b465f5c1e5c9e8dca9210bc0204237 -->
<!-- api: M:Inno.Editor.Assets.AssetCreationTemplate.#ctor 48f4b104630428904ab26fba132218fd5fc74c030e19bd6e627c1f9459134025 -->
<!-- api: T:Inno.Editor.Assets.AssetCreationContext 8372d15d0451f5c727ff15d1175b2b41275b2681333d41322715ff53d0583da5 -->
<!-- api: M:Inno.Editor.Assets.AssetCreationContext.#ctor(Inno.Assets.Pipeline.AssetSourceStore) fdf680f5ec63af4ac22383e9fc19b6926babaa74430148570354d54fc7e2c394 -->
<!-- api: M:Inno.Editor.Assets.AssetCreationContext.EncodeNative(Inno.Assets.AssetObject) bfc353e4b213cfb83dedf4df5449e0decb9b8b67669c4fec4735e6dedf99c2f3 -->
<!-- api: T:Inno.Editor.Assets.AssetCreationTemplate`1 3e57bb39feb155aec3f9a717563ffeccf80f7fda1e0ae74d4e6ea96654162a4d -->
<!-- api: P:Inno.Editor.Assets.AssetCreationTemplate`1.assetType 5f23ca9106522b5521749a5b69b31e68f2bb0af19c5d88a3b9fb470184d2dae0 -->
<!-- api: M:Inno.Editor.Assets.AssetCreationTemplate`1.Create 9e7bd0e285ec73bf9cb1217507e734cc9e60b0fbfc5827365633cec080383503 -->
<!-- api: M:Inno.Editor.Assets.AssetCreationTemplate`1.CreateAsset c1b92988ec54aa596ecb95077c467cb2d0173737cc3184a57af6a2320a70b7f4 -->
<!-- api: M:Inno.Editor.Assets.AssetCreationTemplate`1.#ctor f24f39b18e735c68f5d37bf82600648f60acfaba5e2766b3a8f61d33ada6dd59 -->
<!-- api: T:Inno.Editor.Assets.AssetDraftDocuments`1 d36b720aa4455d2d98706325a4b743fdc907c9ca55b3fedb0cdd2b151d38beba -->
<!-- api: M:Inno.Editor.Assets.AssetDraftDocuments`1.#ctor(Inno.Assets.Pipeline.AssetPipeline,Inno.Core.Serialization.SerializationRegistry,Inno.Editor.Interactions.EditorInteractions,System.String,System.String,System.String,System.String) 8ec69f108da762460a7b01338b715f7e4479f24e33a7d84ca0827e3a086503d4 -->
<!-- api: M:Inno.Editor.Assets.AssetDraftDocuments`1.GetDraft(System.Guid) 72731a091d782d6dacedf5c0c58d82547e651cb52faa7b806c15663a51740457 -->
<!-- api: M:Inno.Editor.Assets.AssetDraftDocuments`1.Open(Inno.Assets.AssetPath) 7ed4b3e72a0922668bcdbb25597c540ee3fcdd136bcc4d3683202f7ba2f8ff2e -->
<!-- api: M:Inno.Editor.Assets.AssetDraftDocuments`1.Read(System.Guid) 592046fb4df45bb4d433ca9dedfee386c93924272478beaf38aa1bf7c98d3361 -->
<!-- api: M:Inno.Editor.Assets.AssetDraftDocuments`1.Replace(System.Guid,`0,System.String) e9b7dd163bb3b09de309810ca2d837c61938dfdb71bda87e99bde3842588bb3d -->
<!-- api: M:Inno.Editor.Assets.AssetDraftDocuments`1.ReplaceMany(System.Collections.Generic.IReadOnlyDictionary{System.Guid,`0},System.String) 01662c182dffc3428c039d887d579d3b6d5663d801257ce2f15eb0fe61feda31 -->
<!-- api: M:Inno.Editor.Assets.AssetDraftDocuments`1.TryMergeHistory(Inno.Editor.Interactions.EditorHistoryChange,Inno.Editor.Interactions.EditorHistoryChange,Inno.Editor.Interactions.EditorHistoryChange@) f5a22b5465ad307b88bdc8bac3f06bc65b444dd9af9a35a7ab82161ceee67573 -->
<!-- api: M:Inno.Editor.Assets.AssetDraftDocuments`1.ValidateHistory(Inno.Editor.Interactions.EditorHistoryChange,Inno.Editor.Interactions.EditorHistoryDirection) 6d9ae671d464a02a62f298dba3c7cd0b503b687089a1f15d23f268759c45d332 -->
<!-- api: M:Inno.Editor.Assets.AssetDraftDocuments`1.ApplyHistory(Inno.Editor.Interactions.EditorHistoryChange,Inno.Editor.Interactions.EditorHistoryDirection) 43d75245c31b09e34a49ec6ab92dfb03192431bc986d1a4ad2ded4c7d16a1a93 -->
<!-- api: M:Inno.Editor.Assets.AssetDraftDocuments`1.Start 17c71061768f46418ad0e4b755908530b3f60775932636feada5a818371d4b9b -->
<!-- api: M:Inno.Editor.Assets.AssetDraftDocuments`1.Update 00df66ca55682c7bba575cbdef17af28dcbabbb0b3c4a9fc0a783088c811425c -->
<!-- api: M:Inno.Editor.Assets.AssetDraftDocuments`1.Dispose f37ec6451a7a46e80aff54e53f57c812952884ef9e67435a481e4b20d7577eae -->
<!-- api: T:Inno.Editor.Assets.AssetDraftDocuments`1.Draft 9f352bcabdb8ff706be88f6b70cb22025fa33596ca87730f10a7f90c7e67e5f8 -->
<!-- api: P:Inno.Editor.Assets.AssetDraftDocuments`1.Draft.id 86a280c8858fcc4942263eb262034fef6daed992cdc69b9537d5c9c6900675fd -->
<!-- api: P:Inno.Editor.Assets.AssetDraftDocuments`1.Draft.documentId e187420a9d58efddaf7350b59f29d693280ac0749afa5f86e3c8a2b040b08c2c -->
<!-- api: P:Inno.Editor.Assets.AssetDraftDocuments`1.Draft.path da488af4b485ae18f291bb4af024dfccbc785b56703897de29630e8aab17b68d -->
<!-- api: P:Inno.Editor.Assets.AssetDraftDocuments`1.Draft.readOnly 36e4bba7c758101774fdf9bd6824f75ac6d84f2672f90cb38e7c50721d82710b -->
<!-- api: P:Inno.Editor.Assets.AssetDraftDocuments`1.Draft.error 0d772158f57130402dcff5d04341d48c5d4ca877ac283157bd741a7b7acd6095 -->
<!-- api: P:Inno.Editor.Assets.AssetDraftDocuments`1.Draft.isDirty f1543f393ab7e7d69304415792903664160349c54c1f4a44df8c59cb71b9bed3 -->
<!-- api: T:Inno.Editor.Assets.AssetEditingModule 1dacfe1f271f57b7ae806ad6d3ccd49f4d4cffaff804f5bddb627ed13ebfec72 -->
<!-- api: M:Inno.Editor.Assets.AssetEditingModule.CreateDirectory(Inno.Assets.AssetPath) 4227705682860a5cc8cff6f6ba88a90cc0f97f1765b43b71e93c594f32e7e810 -->
<!-- api: M:Inno.Editor.Assets.AssetEditingModule.CreateSource(Inno.Assets.AssetPath,System.ReadOnlySpan{System.Byte}) 925d4aabfe3cd8da481429013bef90d47b40394d6eccb7a323a545caeedddbdf -->
<!-- api: M:Inno.Editor.Assets.AssetEditingModule.#ctor(Inno.Editor.Interactions.EditorInteractions,Inno.Assets.Pipeline.AssetPipeline,Inno.Plugins.Authoring.PluginEnvironment,Inno.Extensibility.Types.TypeCatalog,Inno.Core.Logging.LogRouter) a56118adaecfac7b3d11df598b5153962dfd32fc6888689b465bbc0996956501 -->
<!-- api: P:Inno.Editor.Assets.AssetEditingModule.interactions 09a2fe45a4025b428745c1bc2aadcf04c395f35700ec85f6f445d27c906c505f -->
<!-- api: P:Inno.Editor.Assets.AssetEditingModule.pipeline 1ddc54691a949dbec6868907b4874bf8728dd3526c3c9f56490ceacb07744f8a -->
<!-- api: P:Inno.Editor.Assets.AssetEditingModule.creationTemplates 2b0ebb5d754cd9c4c0b48fcc70c76689e9b8b3caac56ad97d2654afe3c15b213 -->
<!-- api: M:Inno.Editor.Assets.AssetEditingModule.CanCreateAsset(System.String,System.String) d8aaa1f2bf7b79f242d451c85c6053a8bfe55ab762851415c931e87b0feb3581 -->
<!-- api: M:Inno.Editor.Assets.AssetEditingModule.CreateAsset(System.String,System.String) 8213345296218f176b895bf4b441faa0c0dd58583be600ac74fdc79600c26d6c -->
<!-- api: M:Inno.Editor.Assets.AssetEditingModule.IsPluginSource(Inno.Assets.AssetSourceId) 60889ca64d070bc7cfd7e8b4b4e51c92005e337a2d3c047eacefb67b7a0d7638 -->
<!-- api: M:Inno.Editor.Assets.AssetEditingModule.IsPluginRoot(Inno.Assets.Pipeline.AssetFileEntry) b6ce9756693e394aca7e6590cc064e7a2d4be118ebcb800c51544a3d1c6c7127 -->
<!-- api: M:Inno.Editor.Assets.AssetEditingModule.TryCreateContext(Inno.Editor.Core.EditorContext,System.String,Inno.Editor.Assets.AssetEditorContext@) 557a5abcf45ab99fdc8ac171967087dbfd8dd4c149f4d16a3e886e7ad921e4a4 -->
<!-- api: M:Inno.Editor.Assets.AssetEditingModule.Open(Inno.Editor.Assets.AssetEditorContext) ea146260c08c12b55f6eb19e499fc6ab54bc4d80c7f8344b7755bd45ae47253e -->
<!-- api: M:Inno.Editor.Assets.AssetEditingModule.ValidateRename(Inno.Editor.Assets.AssetEditorContext,System.String) 7d8ffb80b5a000cd0cfc8da9b68c24726a18565ac1d36c24d6d3f3a887ca9f02 -->
<!-- api: M:Inno.Editor.Assets.AssetEditingModule.Rename(Inno.Editor.Assets.AssetEditorContext,System.String) e8abccb32682dba7b18f6a939641e5f660912e177109e382267e6c18e57b00f7 -->
<!-- api: M:Inno.Editor.Assets.AssetEditingModule.CanMoveToDirectory(System.String,System.String) e8d72e6296e49630ffed4d583e2210b1c984ee068c6fbfa95f63f2a8e6740043 -->
<!-- api: M:Inno.Editor.Assets.AssetEditingModule.MoveToDirectoryWithHistory(System.String,System.String,Inno.Editor.Interactions.IEditorHistory) 10faede73201753b8e04c5d40100f3c4f2fd24331a7e39496c0f36013f0a31f2 -->
<!-- api: M:Inno.Editor.Assets.AssetEditingModule.DeleteWithHistory(Inno.Editor.Assets.AssetEditorContext,Inno.Editor.Interactions.IEditorHistory) 19cd54c9881731a71e75ba15dec0eed828a8b4a16ba7ddca7fe515a791cfa9e9 -->
<!-- api: M:Inno.Editor.Assets.AssetEditingModule.SelectPath(System.String) 5f89d50cdaff7d726bde49a2a2c6cde0a295de9509602734ab71bd006b7c15c4 -->
<!-- api: M:Inno.Editor.Assets.AssetEditingModule.TryCreateDragData(Inno.Editor.Assets.AssetEditorContext,Inno.Editor.Interactions.EditorDragData@) a4658ae250db2f4b920bef6777303213331fd7b29409a7984213641f65cf2077 -->
<!-- api: M:Inno.Editor.Assets.AssetEditingModule.OnStart(Inno.Editor.Core.EditorContext) da50f202392440268c09ecfe00d5faa40691cc755bc1311c78bbcf2f5b678eff -->
<!-- api: M:Inno.Editor.Assets.AssetEditingModule.OnStop(Inno.Editor.Core.EditorContext) a4e66a07c00396451e5a3dfc29574bb99c272c31bc0ec3780a38516252ecb54e -->
<!-- api: M:Inno.Editor.Assets.AssetEditingModule.OnDispose d2fa5bfc77945c54b069c8bdacfc3e6de7cfa39b9b5e02acd19e956afe8a3b7f -->
<!-- api: M:Inno.Editor.Assets.AssetEditingModule.ValidateDeleteSelection(Inno.Editor.Core.EditorContext,System.Collections.Generic.IReadOnlyList{Inno.Assets.Pipeline.AssetFileEntry}) d161b456501c84722522eb5efb1a3b4f6ce505fe20b0bd203ac079cda16934d1 -->
<!-- api: M:Inno.Editor.Assets.AssetEditingModule.DeleteSelection(Inno.Editor.Core.EditorContext,System.Collections.Generic.IReadOnlyList{Inno.Assets.Pipeline.AssetFileEntry}) cfb22110ec5d19a393e9d18ca301223142c5b4f24adea8cf6fdc149042526376 -->
<!-- api: M:Inno.Editor.Assets.AssetEditingModule.CanMoveSelection(System.Collections.Generic.IReadOnlyList{Inno.Assets.Pipeline.AssetFileEntry},System.String) d1e15ba7c3844dd42c9be3d39a390f3ea4a8b87f40d203462c78585db0f8ce63 -->
<!-- api: M:Inno.Editor.Assets.AssetEditingModule.MoveSelection(System.Collections.Generic.IReadOnlyList{Inno.Assets.Pipeline.AssetFileEntry},System.String) 57d574e30dab01679a75869ce078c4c8a4e9b764b45973c7d3a3baaf6396fcab -->
<!-- api: T:Inno.Editor.Assets.AssetEditor c71b63fdc1ac54d736bc4d3119355d0c2e20d2db7cf9fdf848c353258e74dc13 -->
<!-- api: M:Inno.Editor.Assets.AssetEditor.CanOpen(Inno.Editor.Assets.AssetEditorContext) 7b906801359b13ed32e3c5b5fc136481b38d2e92133247db945eabee4d70cffa -->
<!-- api: M:Inno.Editor.Assets.AssetEditor.Open(Inno.Editor.Assets.AssetEditorContext) aa4fac2613e2a60574bbbdbe8bedc890b261642f0c198b21fb2f232a1f0e54fb -->
<!-- api: M:Inno.Editor.Assets.AssetEditor.ValidateRename(Inno.Editor.Assets.AssetEditorContext,System.String) bf66adc0b7be7860e23a0f8abf31db7d98035366d2b999552f992847f373e884 -->
<!-- api: M:Inno.Editor.Assets.AssetEditor.OnRenamed(Inno.Editor.Assets.AssetEditorContext,System.String,System.String) 9654ab7eb2b9ed03d189610b06e5a3059957d5a10407f1df354406991f178e19 -->
<!-- api: M:Inno.Editor.Assets.AssetEditor.ValidateDelete(Inno.Editor.Assets.AssetEditorContext) 4cdd6e1c96a171c64f7eaff7f50fd2b19b884feac4108099a049a670421eee92 -->
<!-- api: M:Inno.Editor.Assets.AssetEditor.OnDeleted(Inno.Editor.Assets.AssetEditorContext) 6b6e4dd166a3d8acda19fc82eb0bacc9c8ad2f62c9ac004381910aae21b3a43f -->
<!-- api: M:Inno.Editor.Assets.AssetEditor.CanStartDrag(Inno.Editor.Assets.AssetEditorContext) 5783dbb38e6df273100db89c725829f6c5fddce9b4e70dc9fb57986735b3309f -->
<!-- api: M:Inno.Editor.Assets.AssetEditor.CreateDragData(Inno.Editor.Assets.AssetEditorContext) 22e6970f44cefd89a36c28e050b7f1d533d551dc6f0811b3e4d397c75fc0d0fc -->
<!-- api: M:Inno.Editor.Assets.AssetEditor.#ctor 13a05c50225d947ad16fdbfa8ff078c2ee092fb5e736ca27f8a27c4d0562bf57 -->
<!-- api: T:Inno.Editor.Assets.AssetEditorAttribute c788b921b8c2f10a6c5c249c201020e24f1de08403acbd0abc8a9eea1023f678 -->
<!-- api: M:Inno.Editor.Assets.AssetEditorAttribute.#ctor(System.Type,System.Boolean,System.Int32) 39589d0bb9ebecf37a7a1105a396f19820ec46ce52369fdeb0b55f47b70e960a -->
<!-- api: P:Inno.Editor.Assets.AssetEditorAttribute.assetType fd277f118ac52d0ab2613f41d39bfc0f975b32b2e68189a9406071f6025b3b4c -->
<!-- api: P:Inno.Editor.Assets.AssetEditorAttribute.useForChildren 5728ce5c2fa1550344173f14b9d5191e321ee34678059de45f097c07ba3276b3 -->
<!-- api: P:Inno.Editor.Assets.AssetEditorAttribute.priority 6a0fc40a390e7875713144cd66c1e97c1e7b540d3e0ae65f792a3ae59894dfca -->
<!-- api: T:Inno.Editor.Assets.AssetEditorContext 6af8e29a26f64099539235f013b73cb9c435bd53c7d6b0d0f22514744ebcb701 -->
<!-- api: P:Inno.Editor.Assets.AssetEditorContext.editorContext 1bb5245da7fdf643bd4f5d8ad3156daabecfc078480fec75562c93cd56b25ac1 -->
<!-- api: P:Inno.Editor.Assets.AssetEditorContext.interactions 72ba2dfc87dc5b2ab013b07b29c0c14653f905bc35bfa30f4904c49222e5391a -->
<!-- api: P:Inno.Editor.Assets.AssetEditorContext.relativePath ca892e8f13e7ad40023d399e97379f7e2cf0ba229e3f7cb9cccc93d3a6708c96 -->
<!-- api: P:Inno.Editor.Assets.AssetEditorContext.name 2298457dfb1e84d1b497e6d30efe8b5b3387963d70a90018cdc6c69aad18c7e3 -->
<!-- api: P:Inno.Editor.Assets.AssetEditorContext.isDirectory 409a543c1b26fdf08c7cdce9ea77a68e4bec040edfe89169fb8b162923aa2fee -->
<!-- api: P:Inno.Editor.Assets.AssetEditorContext.info 43326763a06ee265e71fef69da7831e8160eabd2082bebe0d35c1dabc23ce0af -->
<!-- api: P:Inno.Editor.Assets.AssetEditorContext.assetType 6260d68acec09373033742a2bb3d4fee5de5acd1d22be634b8e60009d20e2864 -->
<!-- api: T:Inno.Editor.Assets.AssetOperationValidation 5a6b24b9d3e7c22e094f81757c9427db2dd7bb9598f622d0ceda9eeaf94d49df -->
<!-- api: M:Inno.Editor.Assets.AssetOperationValidation.#ctor(System.Boolean,System.String) 19d12c132a0de7285be304aec00cbc45fa5e0b310abff6c2ed8a7460687d46a2 -->
<!-- api: P:Inno.Editor.Assets.AssetOperationValidation.isValid 1da8072e8a4365f8da08006e90154a791963be3f59d84b72fd30380615e79458 -->
<!-- api: P:Inno.Editor.Assets.AssetOperationValidation.message 5a0b7f14df71b1b7aad6b3ab8f3860dd3e7f54ba199977fdcacb2cbe67623dfb -->
<!-- api: P:Inno.Editor.Assets.AssetOperationValidation.valid 8ee9072f9cbc028eb2f6eaf229288f33aadb1ca3dbf5fb690889412dc2826b55 -->
<!-- api: M:Inno.Editor.Assets.AssetOperationValidation.Invalid(System.String) c0fa98eb193e98ebc0e5ba9591ca4e217f1b7bddc0153936760920f2d4f5bc3c -->
<!-- api: M:Inno.Editor.Assets.AssetOperationValidation.ToString 5cb5df9fb82dbf45208451b32b5914971f9b21c53161c137968054320e48fe37 -->
<!-- api: M:Inno.Editor.Assets.AssetOperationValidation.op_Inequality(Inno.Editor.Assets.AssetOperationValidation,Inno.Editor.Assets.AssetOperationValidation) d3878ccb3f39a30266ae15bc6c54b209ec7c810ef0b3edc476c43352563a3178 -->
<!-- api: M:Inno.Editor.Assets.AssetOperationValidation.op_Equality(Inno.Editor.Assets.AssetOperationValidation,Inno.Editor.Assets.AssetOperationValidation) 0a0f62f2a5ff429f2533994e911af4e9419dd86d172550ad599750725d429cda -->
<!-- api: M:Inno.Editor.Assets.AssetOperationValidation.GetHashCode a106c86fa78df15918b511137e8c4efea345b2d3b30f7b148f34c55d3faac1bf -->
<!-- api: M:Inno.Editor.Assets.AssetOperationValidation.Equals(System.Object) bf6c1b22ef4cf6329558d301d02a4cc12c8d7b79fdb26ea2e78516b8db6619ab -->
<!-- api: M:Inno.Editor.Assets.AssetOperationValidation.Equals(Inno.Editor.Assets.AssetOperationValidation) f7013552d8803bc1829c04cac4b6ee42c6a9931432f015c26f618ab713ee16b4 -->
<!-- api: T:Inno.Editor.Assets.AssetImportSettingsEdits 186a666d0af09e005517f1eb435643d19cc0eab6d860672ca89832b6823f33ee -->
<!-- api: M:Inno.Editor.Assets.AssetImportSettingsEdits.#ctor(Inno.Assets.Pipeline.AssetPipeline,Inno.Core.Serialization.SerializationRegistry,Inno.Extensibility.Types.TypeCatalog,Inno.Editor.Interactions.EditorInteractions) 5a9276b78d5165997b9ee01328a3fbb2962b82f0e914e06f6b7858f402710a1b -->
<!-- api: M:Inno.Editor.Assets.AssetImportSettingsEdits.Apply(Inno.Assets.AssetPath,Inno.Core.Serialization.ISerializable,System.String) 7271e6e580837e30bed5f281c450781f49ebab4993470ed81aa6693a054bfbc2 -->
<!-- api: T:Inno.Editor.Assets.AssetSampleImportModule fd8dc42ca02790935591bd2ceed7820d15b88781f0dd0ecec4ac4ff2f5611934 -->
<!-- api: P:Inno.Editor.Assets.AssetSampleImportModule.blocksFollowingUpdates a873ceee5d5dd8bd4d3b84f2c57faa273fb1c1f0010d8c3cf7ef9f1178beb8f1 -->
<!-- api: P:Inno.Editor.Assets.AssetSampleImportModule.isBusy 74f7aa46907d0f9454a3195a738459d488d4c0d12df93c7adac37175fcf9870a -->
<!-- api: P:Inno.Editor.Assets.AssetSampleImportModule.canImport b9962f321addca50a2e6160515d0ea2f1eed961b50c932ac5a03693a5b67c6cf -->
<!-- api: P:Inno.Editor.Assets.AssetSampleImportModule.progress 4152c23887ba1850113ba4d4464801b0b1c2fa1db5e9acc31f589fd87aa91188 -->
<!-- api: M:Inno.Editor.Assets.AssetSampleImportModule.Request(Inno.Assets.AssetPath) 9a50252ac8b6b64f87d98a425b5ce66ec49d21f1044854958b3ed53e54e75631 -->
<!-- api: M:Inno.Editor.Assets.AssetSampleImportModule.Cancel(System.Int64) 6a638d20061a0fb2e62258e782f2e1aae1b194904f623a18ec35ba09c087b4dc -->
<!-- api: M:Inno.Editor.Assets.AssetSampleImportModule.OnUpdate(Inno.Editor.Core.EditorContext) 29531e22022bbb1580f80111b2188e3f3fbb787b72370f3e2eb377b180f4ba3c -->
<!-- api: M:Inno.Editor.Assets.AssetSampleImportModule.OnStop(Inno.Editor.Core.EditorContext) 0f70f1968886b798830f5244613ff355aea9015942f646c5308cbb0febb7157b -->
<!-- api: M:Inno.Editor.Assets.AssetSampleImportModule.OnDispose d437bf4bd249329cf35bf5cdc8026b7d3e9bc6cb6065f684be89e7aa7fd77b92 -->
<!-- api: T:Inno.Editor.Assets.AssetSampleImportProgress 386f7a7211cddd9e32c805d8b2a9eb0f5246229816604c8571279b2751019f20 -->
<!-- api: M:Inno.Editor.Assets.AssetSampleImportProgress.#ctor(System.Int64,System.Single,System.String,System.Boolean) f95646486cb0b93d93851c02f05b44334453804fbb5e4ecbda8904651cc3c985 -->
<!-- api: P:Inno.Editor.Assets.AssetSampleImportProgress.operationId 8f69939fc171a1bb1356d04bdd2e0346a941d96a4a21fdb9fa1e05642d76ca63 -->
<!-- api: P:Inno.Editor.Assets.AssetSampleImportProgress.fraction 55ed3dec7e4447c073dcb697a560b8e7b8cc1d1888f3910e48dfb5cd92204f5e -->
<!-- api: P:Inno.Editor.Assets.AssetSampleImportProgress.status ea4841b7a30589d6003224a467258a8514f196508862df0d663d313359de73de -->
<!-- api: P:Inno.Editor.Assets.AssetSampleImportProgress.cancellationRequested 5ef1a93db67b3e6677adae6e445691ac575eb008f865e510f9321072378ed953 -->
<!-- api: M:Inno.Editor.Assets.AssetSampleImportProgress.ToString b38dd6f2d4dcea7798a4224a47d40493dc774eb3081e0d5473d589091e9ccffc -->
<!-- api: M:Inno.Editor.Assets.AssetSampleImportProgress.op_Inequality(Inno.Editor.Assets.AssetSampleImportProgress,Inno.Editor.Assets.AssetSampleImportProgress) b2a7885d32c7e1d6cb6590a1ed3d0b3ec6d104ff3750b918b47f319c7e17d9d2 -->
<!-- api: M:Inno.Editor.Assets.AssetSampleImportProgress.op_Equality(Inno.Editor.Assets.AssetSampleImportProgress,Inno.Editor.Assets.AssetSampleImportProgress) c45e775db0d1913c028a02be75712cc551a29599bb4c45fecd65a7c5705bfbce -->
<!-- api: M:Inno.Editor.Assets.AssetSampleImportProgress.GetHashCode 5238c738066dfd1f2c15bb854ea5ee3e72c9b4a80912f80da6707bbf0c63b562 -->
<!-- api: M:Inno.Editor.Assets.AssetSampleImportProgress.Equals(System.Object) 497404322ec89c5a1943fdb74a7a0b53268d6c6103a7df6c495bb1bbd9416afb -->
<!-- api: M:Inno.Editor.Assets.AssetSampleImportProgress.Equals(Inno.Editor.Assets.AssetSampleImportProgress) fba71a406e38360e36874e5f0053341b7059b9b594984b38c8575f4237fe6d4a -->
<!-- api: M:Inno.Editor.Assets.AssetSampleImportProgress.Deconstruct(System.Int64@,System.Single@,System.String@,System.Boolean@) 717c0b11b8907c9d6bdd8458ac55a9aa52bf069f70dae65754c429b120a29ae6 -->

| API | 职责与参数 | 返回语义 | 失败条件 |
|---|---|---|---|
| <a id="api-fbdd5a649b6ec91c"></a>[`public sealed class Inno.Editor.Assets.AssetCreationMenuAttribute : System.Attribute`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L13) | Declares the source name, extension, and menu placement that cannot be inferred from an Inno.Assets.AssetObject type alone. | — | — |
| <a id="api-762c7b79b6d33359"></a>[`public Inno.Editor.Assets.AssetCreationMenuAttribute.AssetCreationMenuAttribute(string id, string menuPath, string extension, string defaultName, int groupOrder = 0, int itemOrder = 0, bool separatorBeforeGroup = false)`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L42) | Creates one asset creation menu declaration. <br>`id`: Globally stable template identifier.<br>`menuPath`: Slash-delimited path below the File Browser Create menu.<br>`extension`: Native source extension, including its leading dot.<br>`defaultName`: Default source name without an extension or numeric suffix.<br>`groupOrder`: Stable order of the top-level category below Create.<br>`itemOrder`: Stable order of the leaf within its immediate category.<br>`separatorBeforeGroup`: Whether the top-level category starts after a separator. | — | `System.ArgumentException`: Thrown when an identifier, path, source name, or native extension is invalid. |
| <a id="api-19d9b5902aaf8326"></a>[`public string Inno.Editor.Assets.AssetCreationMenuAttribute.id { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L84) | Gets the globally stable template identifier. | — | — |
| <a id="api-b486f7196cb4e58f"></a>[`public string Inno.Editor.Assets.AssetCreationMenuAttribute.menuPath { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L89) | Gets the slash-delimited path below the File Browser Create menu. | — | — |
| <a id="api-cc25f639e96f1bb3"></a>[`public string Inno.Editor.Assets.AssetCreationMenuAttribute.extension { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L94) | Gets the native source extension, including its leading dot. | — | — |
| <a id="api-60e3d5b4c2e448a4"></a>[`public string Inno.Editor.Assets.AssetCreationMenuAttribute.defaultName { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L99) | Gets the default source name without an extension or numeric suffix. | — | — |
| <a id="api-8d2386a0d63fa904"></a>[`public int Inno.Editor.Assets.AssetCreationMenuAttribute.groupOrder { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L104) | Gets the stable order of the top-level category below Create. | — | — |
| <a id="api-9a7a0f4eb6662e1a"></a>[`public int Inno.Editor.Assets.AssetCreationMenuAttribute.itemOrder { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L109) | Gets the stable order of the leaf within its immediate category. | — | — |
| <a id="api-2186832ddf917cf6"></a>[`public bool Inno.Editor.Assets.AssetCreationMenuAttribute.separatorBeforeGroup { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L114) | Gets whether the top-level category starts after a separator. | — | — |
| <a id="api-689e3945d5e5c369"></a>[`public abstract class Inno.Editor.Assets.AssetCreationTemplate`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L124) | Produces the detached initial value for one authorable native asset source. | — | — |
| <a id="api-cb95e8fd13be7458"></a>[`public abstract System.Type Inno.Editor.Assets.AssetCreationTemplate.assetType { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L129) | Gets the concrete asset type produced by this template. | — | — |
| <a id="api-fd0e8e7637fbcb40"></a>[`public abstract Inno.Assets.AssetObject Inno.Editor.Assets.AssetCreationTemplate.Create()`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L137) | Creates a detached default value that has not been imported or published. | A new asset value owned by the caller. | — |
| <a id="api-68fda62e88f5779b"></a>[`public virtual byte[] Inno.Editor.Assets.AssetCreationTemplate.Encode(Inno.Editor.Assets.AssetCreationContext context, Inno.Assets.AssetObject asset)`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L156) | Encodes the detached initial value into the source representation owned by this template. <br>`context`: Creation services scoped to the current Asset source pipeline.<br>`asset`: The exact value returned by Inno.Editor.Assets.AssetCreationTemplate.Create. | Complete source bytes ready for an atomic File Browser create operation. | — |
| <a id="api-b0d192e9ee7fcc32"></a>[`protected Inno.Editor.Assets.AssetCreationTemplate.AssetCreationTemplate()`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L124) | 由 C# 编译器生成的值契约成员。 | — | — |
| <a id="api-6e7e027cad6396ae"></a>[`public sealed class Inno.Editor.Assets.AssetCreationContext`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L169) | Provides source encoding services to one detached Asset creation template invocation. | — | — |
| <a id="api-1d0b9bde8f5a28eb"></a>[`public Inno.Editor.Assets.AssetCreationContext.AssetCreationContext(Inno.Assets.Pipeline.AssetSourceStore sources)`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L180) | Creates a context over the source serializer owned by the active Asset pipeline. <br>`sources`: Source serializer carrying the active reference context. | — | — |
| <a id="api-6663a476394cff08"></a>[`public byte[] Inno.Editor.Assets.AssetCreationContext.EncodeNative(Inno.Assets.AssetObject asset)`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L191) | Encodes an Asset value through the current native structured source serializer. <br>`asset`: Detached Asset value to encode. | Complete native source bytes, including the concrete serialized type identity. | — |
| <a id="api-aa8e1b491243d8a2"></a>[`public abstract class Inno.Editor.Assets.AssetCreationTemplate<TAsset> where TAsset : Inno.Assets.AssetObject, new() : Inno.Editor.Assets.AssetCreationTemplate`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L204) | Creates a default-constructed native source for an authorable asset type. <br>`TAsset`: Concrete serializable asset type. | — | — |
| <a id="api-427b91599020e97e"></a>[`public override sealed System.Type Inno.Editor.Assets.AssetCreationTemplate<TAsset>.assetType { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L210) | Gets the asset type created by this Editor template. | — | — |
| <a id="api-04bf0386a8194175"></a>[`public override Inno.Assets.AssetObject Inno.Editor.Assets.AssetCreationTemplate<TAsset>.Create()`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L218) | Creates a value using this implementation's validated inputs. | The validated asset object that represents the completed operation. | — |
| <a id="api-fa8c7adc3a1c89e3"></a>[`protected virtual TAsset Inno.Editor.Assets.AssetCreationTemplate<TAsset>.CreateAsset()`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L226) | Creates the initial detached value, allowing specialized deterministic defaults. | A new asset value owned by the caller. | — |
| <a id="api-29021c5db7e2daee"></a>[`protected Inno.Editor.Assets.AssetCreationTemplate<TAsset>.AssetCreationTemplate()`](../../src/composition/editor/features/Inno.Editor.Assets/Creation/AssetCreationTemplate.cs#L204) | 由 C# 编译器生成的值契约成员。 | — | — |
| <a id="api-59b3edbc8dfa119f"></a>[`public sealed class Inno.Editor.Assets.AssetDraftDocuments<TAsset> where TAsset : Inno.Assets.AssetObject : System.IDisposable`](../../src/composition/editor/features/Inno.Editor.Assets/Documents/AssetDraftDocuments.cs#L19) | Owns detached native asset drafts with shared document, History, recovery and explicit-save semantics. <br>`TAsset`: Native source type edited by the consuming feature. | — | — |
| <a id="api-fd3e60cf4366c89c"></a>[`public Inno.Editor.Assets.AssetDraftDocuments<TAsset>.AssetDraftDocuments(Inno.Assets.Pipeline.AssetPipeline assets, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Editor.Interactions.EditorInteractions interactions, string providerId, string historyKind, string extension, string label)`](../../src/composition/editor/features/Inno.Editor.Assets/Documents/AssetDraftDocuments.cs#L57) | Creates a feature-owned draft store; call Start after document services become available. <br>`assets`: Owner of source identities, serialization and import.<br>`serialization`: Host registry for neutral History and recovery records.<br>`interactions`: Shared documents and History services.<br>`providerId`: Stable unique document provider identifier.<br>`historyKind`: Stable protocol handled by the feature's registered History handler.<br>`extension`: Native source extension including its leading dot.<br>`label`: Human-readable singular asset kind. | — | — |
| <a id="api-eb99746c49c9126b"></a>[`public Inno.Editor.Assets.AssetDraftDocuments<TAsset>.Draft Inno.Editor.Assets.AssetDraftDocuments<TAsset>.GetDraft(System.Guid assetId)`](../../src/composition/editor/features/Inno.Editor.Assets/Documents/AssetDraftDocuments.cs#L95) | Gets host-owned neutral presentation state for an open document. <br>`assetId`: Persistent source identity. | Read-only presentation state; it contains no live asset or extension objects. | — |
| <a id="api-bc354be2e4f86a2b"></a>[`public System.Guid Inno.Editor.Assets.AssetDraftDocuments<TAsset>.Open(Inno.Assets.AssetPath path)`](../../src/composition/editor/features/Inno.Editor.Assets/Documents/AssetDraftDocuments.cs#L105) | Opens a native asset source in the shared document service without revealing a second Inspector. <br>`path`: Source path, including sources with failed imports. | The persistent asset identity used for subsequent draft operations. | — |
| <a id="api-01ac309ac2712d8a"></a>[`public TAsset Inno.Editor.Assets.AssetDraftDocuments<TAsset>.Read(System.Guid assetId)`](../../src/composition/editor/features/Inno.Editor.Assets/Documents/AssetDraftDocuments.cs#L124) | Reads a detached current-generation value; callers must never mutate its referenced canonical assets. <br>`assetId`: Open asset identity. | A detached editable asset whose changes have not been published. | — |
| <a id="api-36714b82efdb144a"></a>[`public void Inno.Editor.Assets.AssetDraftDocuments<TAsset>.Replace(System.Guid assetId, TAsset candidate, string? mergeKey = null)`](../../src/composition/editor/features/Inno.Editor.Assets/Documents/AssetDraftDocuments.cs#L137) | Updates the open draft through shared History without saving or publishing a runtime asset. <br>`assetId`: Open asset identity.<br>`candidate`: Detached edited asset.<br>`mergeKey`: The current gesture key, or null for an independent discrete edit. | — | — |
| <a id="api-206200b71725b824"></a>[`public void Inno.Editor.Assets.AssetDraftDocuments<TAsset>.ReplaceMany(System.Collections.Generic.IReadOnlyDictionary<System.Guid, TAsset> candidates, string? mergeKey = null)`](../../src/composition/editor/features/Inno.Editor.Assets/Documents/AssetDraftDocuments.cs#L157) | Records a fully validated selection sample atomically in the shared History. <br>`candidates`: Detached candidates keyed by their persistent source identity.<br>`mergeKey`: The owning gesture key, or null for a discrete edit. | — | — |
| <a id="api-58495933ae6abe75"></a>[`public bool Inno.Editor.Assets.AssetDraftDocuments<TAsset>.TryMergeHistory(Inno.Editor.Interactions.EditorHistoryChange older, Inno.Editor.Interactions.EditorHistoryChange newer, out Inno.Editor.Interactions.EditorHistoryChange? merged)`](../../src/composition/editor/features/Inno.Editor.Assets/Documents/AssetDraftDocuments.cs#L221) | Combines adjacent samples of the same document and gesture without retaining live assets. <br>`older`: The preceding owned record.<br>`newer`: The subsequent owned record.<br>`merged`: Receives a new independent payload when the records form a contiguous edit. | True when both records describe the same continuous value change. | — |
| <a id="api-fe236e51a04108c9"></a>[`public void Inno.Editor.Assets.AssetDraftDocuments<TAsset>.ValidateHistory(Inno.Editor.Interactions.EditorHistoryChange change, Inno.Editor.Interactions.EditorHistoryDirection direction)`](../../src/composition/editor/features/Inno.Editor.Assets/Documents/AssetDraftDocuments.cs#L252) | Validates a feature's neutral History record without changing the draft. <br>`change`: Record owned by the shared History service.<br>`direction`: Requested traversal direction. | — | — |
| <a id="api-1a72deeadfbcd64e"></a>[`public void Inno.Editor.Assets.AssetDraftDocuments<TAsset>.ApplyHistory(Inno.Editor.Interactions.EditorHistoryChange change, Inno.Editor.Interactions.EditorHistoryDirection direction)`](../../src/composition/editor/features/Inno.Editor.Assets/Documents/AssetDraftDocuments.cs#L270) | Applies a previously validated neutral record; failed recovery leaves the draft unchanged. <br>`change`: Record owned by the shared History service.<br>`direction`: Requested traversal direction. | — | — |
| <a id="api-ce57369af31b1272"></a>[`public void Inno.Editor.Assets.AssetDraftDocuments<TAsset>.Start()`](../../src/composition/editor/features/Inno.Editor.Assets/Documents/AssetDraftDocuments.cs#L402) | Registers the feature provider with the headless document ownership service. | — | — |
| <a id="api-3dd24657e73211a1"></a>[`public void Inno.Editor.Assets.AssetDraftDocuments<TAsset>.Update()`](../../src/composition/editor/features/Inno.Editor.Assets/Documents/AssetDraftDocuments.cs#L412) | Imports explicit saves and reconciles source changes without owning UI gesture lifetimes. | — | — |
| <a id="api-671928ddd5b9fa7e"></a>[`public void Inno.Editor.Assets.AssetDraftDocuments<TAsset>.Dispose()`](../../src/composition/editor/features/Inno.Editor.Assets/Documents/AssetDraftDocuments.cs#L478) | Preserves unsaved recovery and unregisters the current feature provider. | — | — |
| <a id="api-63121595a158ace1"></a>[`public sealed class Inno.Editor.Assets.AssetDraftDocuments<TAsset>.Draft`](../../src/composition/editor/features/Inno.Editor.Assets/Documents/AssetDraftDocuments.cs#L560) | Contains neutral draft status; only the owning store can change it. | — | — |
| <a id="api-f981a637d217a7e3"></a>[`public System.Guid Inno.Editor.Assets.AssetDraftDocuments<TAsset>.Draft.id { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Documents/AssetDraftDocuments.cs#L586) | Gets the persistent source identity. | — | — |
| <a id="api-7d37439995dac066"></a>[`public System.Guid Inno.Editor.Assets.AssetDraftDocuments<TAsset>.Draft.documentId { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Documents/AssetDraftDocuments.cs#L590) | Gets the shared document identity used for Save/Revert and close confirmation. | — | — |
| <a id="api-5ce9d12a0aa6bd87"></a>[`public Inno.Assets.AssetPath Inno.Editor.Assets.AssetDraftDocuments<TAsset>.Draft.path { get; internal set; }`](../../src/composition/editor/features/Inno.Editor.Assets/Documents/AssetDraftDocuments.cs#L594) | Gets the current resolved source path. | — | — |
| <a id="api-44266f8d20cf960a"></a>[`public bool Inno.Editor.Assets.AssetDraftDocuments<TAsset>.Draft.readOnly { get; internal set; }`](../../src/composition/editor/features/Inno.Editor.Assets/Documents/AssetDraftDocuments.cs#L598) | Gets whether this source is installed read-only content. | — | — |
| <a id="api-e5f62feed864966e"></a>[`public string Inno.Editor.Assets.AssetDraftDocuments<TAsset>.Draft.error { get; internal set; }`](../../src/composition/editor/features/Inno.Editor.Assets/Documents/AssetDraftDocuments.cs#L602) | Gets the latest source, save or import diagnostic. | — | — |
| <a id="api-1b0aa8f4cad5ebce"></a>[`public bool Inno.Editor.Assets.AssetDraftDocuments<TAsset>.Draft.isDirty { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Documents/AssetDraftDocuments.cs#L606) | Gets whether detached bytes differ from the saved baseline. | — | — |
| <a id="api-229cb70d003e1680"></a>[`public sealed class Inno.Editor.Assets.AssetEditingModule : Inno.Editor.Core.EditorModule`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditingModule.Creation.cs#L13) | Handles asset creation actions in the Editor file browser. | — | — |
| <a id="api-1b7a4541a35e413e"></a>[`public Inno.Assets.Pipeline.AssetFileEntry Inno.Editor.Assets.AssetEditingModule.CreateDirectory(Inno.Assets.AssetPath parent)`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditingModule.Creation.cs#L27) | Creates a uniquely named directory and records its removal and restoration in shared History. <br>`parent`: An existing writable source directory. | The indexed identity of the newly created directory. | `System.InvalidOperationException`: The source is read-only, unavailable, or cannot retain the History entry. |
| <a id="api-a7ee22bded39657c"></a>[`public Inno.Assets.Pipeline.AssetFileEntry Inno.Editor.Assets.AssetEditingModule.CreateSource(Inno.Assets.AssetPath path, System.ReadOnlySpan<byte> bytes)`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditingModule.Creation.cs#L75) | Creates a native or ordinary-text asset source as one recoverable shared-history operation. <br>`path`: A new file in an existing writable asset directory.<br>`bytes`: Complete source bytes supplied by the feature's native writer. | The indexed source identity, even when its first import reports a source error. | `System.IO.IOException`: The source already exists, has no existing parent, or cannot be installed.<br>`System.InvalidOperationException`: The source mount is unavailable/read-only or history cannot retain the change. |
| <a id="api-039f63debb9eea65"></a>[`public Inno.Editor.Assets.AssetEditingModule.AssetEditingModule(Inno.Editor.Interactions.EditorInteractions interactions, Inno.Assets.Pipeline.AssetPipeline pipeline, Inno.Plugins.Authoring.PluginEnvironment plugins, Inno.Extensibility.Types.TypeCatalog types, Inno.Core.Logging.LogRouter logs)`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditingModule.cs#L50) | Creates the asset editing service for one authoring generation. <br>`interactions`: The active editor interaction entry point.<br>`pipeline`: The isolated authoring asset pipeline used by browser operations.<br>`plugins`: The active Plugin catalog that authoritatively identifies Plugin-owned source mounts.<br>`types`: The active type catalog used to build extension registry snapshots.<br>`logs`: The application log router used for Asset Browser operation diagnostics. | — | `System.ArgumentNullException`: Thrown when any required service is null. |
| <a id="api-e1a986009d8c07f1"></a>[`public Inno.Editor.Interactions.EditorInteractions Inno.Editor.Assets.AssetEditingModule.interactions { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditingModule.cs#L71) | Gets the editor interaction owner used by source operations. | — | — |
| <a id="api-cc2507a0e094c124"></a>[`public Inno.Assets.Pipeline.AssetPipeline Inno.Editor.Assets.AssetEditingModule.pipeline { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditingModule.cs#L77) | Gets the authoritative authoring pipeline; mutations must use its existing ownership protocol. | — | — |
| <a id="api-6acbcf8306ad9dcb"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Editor.Assets.AssetCreationMenuAttribute> Inno.Editor.Assets.AssetEditingModule.creationTemplates { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditingModule.cs#L81) | Gets the current immutable creation menu declarations without retaining template instances. | — | — |
| <a id="api-395d642628c35fcb"></a>[`public bool Inno.Editor.Assets.AssetEditingModule.CanCreateAsset(string directory, string templateId)`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditingModule.cs#L96) | Checks that a creation template and writable destination are available. <br>`directory`: The source-relative writable destination directory.<br>`templateId`: The stable identity of a discovered creation template. | True when the request is accepted; otherwise false. | — |
| <a id="api-825d6fb380311056"></a>[`public Inno.Assets.Pipeline.AssetFileEntry Inno.Editor.Assets.AssetEditingModule.CreateAsset(string directory, string templateId)`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditingModule.cs#L120) | Creates and imports a source using a discovered template and records the committed change. <br>`directory`: The source-relative writable destination directory.<br>`templateId`: The stable identity of a discovered creation template. | The committed source identity, or a validation result describing why the operation is unavailable. | — |
| <a id="api-7364d050838e1f82"></a>[`public bool Inno.Editor.Assets.AssetEditingModule.IsPluginSource(Inno.Assets.AssetSourceId source)`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditingModule.cs#L170) | Determines whether the active Plugin catalog owns a source mount. <br>`source`: The source mount identity to query. | True when the request is accepted; otherwise false. | — |
| <a id="api-abf8ecc71d64fc56"></a>[`public bool Inno.Editor.Assets.AssetEditingModule.IsPluginRoot(Inno.Assets.Pipeline.AssetFileEntry entry)`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditingModule.cs#L181) | Determines whether an indexed entry is the root of an installed Plugin. <br>`entry`: The currently indexed source identity. | True when the request is accepted; otherwise false. | — |
| <a id="api-48afec75bfc531ef"></a>[`public bool Inno.Editor.Assets.AssetEditingModule.TryCreateContext(Inno.Editor.Core.EditorContext editor, string relativePath, out Inno.Editor.Assets.AssetEditorContext? context)`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditingModule.cs#L203) | Captures the currently indexed source and extension metadata for one immediate operation. <br>`editor`: The current editor context used only during this invocation.<br>`relativePath`: The isolated source-relative path to resolve.<br>`context`: Receives the current source snapshot, or null when the source is unavailable. | True when the request is accepted; otherwise false. | — |
| <a id="api-73bc9c692c43e0db"></a>[`public bool Inno.Editor.Assets.AssetEditingModule.Open(Inno.Editor.Assets.AssetEditorContext context)`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditingModule.cs#L230) | Opens a source using its current asset editor extension and reports rejected or failed opens. <br>`context`: The immutable metadata captured for this immediate source operation. | True when the request is accepted; otherwise false. | — |
| <a id="api-8773a1a2b2489eb9"></a>[`public Inno.Editor.Interactions.EditorValidationResult Inno.Editor.Assets.AssetEditingModule.ValidateRename(Inno.Editor.Assets.AssetEditorContext context, string newName)`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditingModule.cs#L260) | Validates a requested name without modifying the source. <br>`context`: The immutable metadata captured for this immediate source operation.<br>`newName`: The requested entry name without its protected file extension. | The committed source identity, or a validation result describing why the operation is unavailable. | — |
| <a id="api-b89c79b48de62def"></a>[`public void Inno.Editor.Assets.AssetEditingModule.Rename(Inno.Editor.Assets.AssetEditorContext context, string newName)`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditingModule.cs#L278) | Renames a source atomically and records its reversible history payload. <br>`context`: The immutable metadata captured for this immediate source operation.<br>`newName`: The requested entry name without its protected file extension. | — | — |
| <a id="api-b2ff5dc42f83972e"></a>[`public bool Inno.Editor.Assets.AssetEditingModule.CanMoveToDirectory(string sourcePath, string targetDirectory)`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditingModule.cs#L314) | Checks whether a source can move into the destination without a cycle or name collision. <br>`sourcePath`: The current isolated source-relative path.<br>`targetDirectory`: The isolated destination directory. | True when the request is accepted; otherwise false. | — |
| <a id="api-c5b3f1bb2bf64fa3"></a>[`public Inno.Assets.Pipeline.AssetFileEntry Inno.Editor.Assets.AssetEditingModule.MoveToDirectoryWithHistory(string sourcePath, string targetDirectory, Inno.Editor.Interactions.IEditorHistory history)`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditingModule.cs#L334) | Moves a source and records the committed mutation in the supplied history. <br>`sourcePath`: The current isolated source-relative path.<br>`targetDirectory`: The isolated destination directory.<br>`history`: The authoritative history that receives the committed mutation. | The committed source identity, or a validation result describing why the operation is unavailable. | — |
| <a id="api-979529e49ff08ad4"></a>[`public bool Inno.Editor.Assets.AssetEditingModule.DeleteWithHistory(Inno.Editor.Assets.AssetEditorContext asset, Inno.Editor.Interactions.IEditorHistory history)`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditingModule.cs#L411) | Deletes a source with a recoverable archive and records the committed mutation. <br>`asset`: The captured source metadata to validate and delete.<br>`history`: The authoritative History receiving this mutation. | True when the request is accepted; otherwise false. | — |
| <a id="api-5bcd4c47eb49239e"></a>[`public void Inno.Editor.Assets.AssetEditingModule.SelectPath(string relativePath)`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditingModule.cs#L462) | Selects the current source identity without retaining an indexed entry across generations. <br>`relativePath`: The isolated source-relative path to resolve. | — | — |
| <a id="api-317ae30fb67f4108"></a>[`public bool Inno.Editor.Assets.AssetEditingModule.TryCreateDragData(Inno.Editor.Assets.AssetEditorContext context, out Inno.Editor.Interactions.EditorDragData? data)`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditingModule.cs#L481) | Creates identity-based drag data when the current extension permits dragging. <br>`context`: The immutable metadata captured for this immediate source operation.<br>`data`: Receives identity-based drag data, or null when dragging is unavailable. | True when the request is accepted; otherwise false. | — |
| <a id="api-e675b77ce7138247"></a>[`protected override void Inno.Editor.Assets.AssetEditingModule.OnStart(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditingModule.cs#L502) | Initializes this feature when its owning runtime becomes active. <br>`context`: The context that supplies state and services for this operation. | — | — |
| <a id="api-83e2920e1c68ec16"></a>[`protected override void Inno.Editor.Assets.AssetEditingModule.OnStop(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditingModule.cs#L513) | Stops this feature before its owning runtime releases the active generation. <br>`context`: The context that supplies state and services for this operation. | — | — |
| <a id="api-11ed9c5714c80e58"></a>[`protected override void Inno.Editor.Assets.AssetEditingModule.OnDispose()`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditingModule.cs#L521) | Releases resources retained by this feature after it has stopped. | — | — |
| <a id="api-7f98f383bfd0092d"></a>[`public Inno.Editor.Assets.AssetOperationValidation Inno.Editor.Assets.AssetEditingModule.ValidateDeleteSelection(Inno.Editor.Core.EditorContext editor, System.Collections.Generic.IReadOnlyList<Inno.Assets.Pipeline.AssetFileEntry> targets)`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditingModule.Selection.cs#L28) | Checks the entire source selection before a batch mutation, including extension validation and read-only ownership. <br>`editor`: The immediate Editor context.<br>`targets`: Fresh source entries in selection order. | The first rejection, or a valid result when every source can be deleted. | — |
| <a id="api-f8f387430d31daba"></a>[`public void Inno.Editor.Assets.AssetEditingModule.DeleteSelection(Inno.Editor.Core.EditorContext editor, System.Collections.Generic.IReadOnlyList<Inno.Assets.Pipeline.AssetFileEntry> targets)`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditingModule.Selection.cs#L68) | Deletes minimal selected source roots in one History transaction, restoring earlier deletions if a later one fails. <br>`editor`: The immediate Editor context.<br>`targets`: Fresh selected source entries; nested entries are covered by their selected directory. | — | `System.InvalidOperationException`: Validation or a mutation fails; rollback failures remain explicit. |
| <a id="api-f0de8511db09ff2f"></a>[`public bool Inno.Editor.Assets.AssetEditingModule.CanMoveSelection(System.Collections.Generic.IReadOnlyList<Inno.Assets.Pipeline.AssetFileEntry> targets, string targetDirectory)`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditingModule.Selection.cs#L99) | Validates every minimal selected source and rejects destination collisions before a batch move. <br>`targets`: Live source entries in selection order.<br>`targetDirectory`: The writable project destination. | True when every source can move to a distinct destination. | — |
| <a id="api-5971cd7cf6ddc0b6"></a>[`public void Inno.Editor.Assets.AssetEditingModule.MoveSelection(System.Collections.Generic.IReadOnlyList<Inno.Assets.Pipeline.AssetFileEntry> targets, string targetDirectory)`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditingModule.Selection.cs#L126) | Moves selected source roots in one transaction; a later failure restores earlier moves. <br>`targets`: Live entries, including descendants covered by a selected directory.<br>`targetDirectory`: The writable project destination. | — | `System.InvalidOperationException`: Any source or destination rejects the operation. |
| <a id="api-5c488adfd681c908"></a>[`public abstract class Inno.Editor.Assets.AssetEditor`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditor.cs#L7) | Customizes editor interactions for one imported asset type. | — | — |
| <a id="api-6ea72eed5ad5e5a4"></a>[`public virtual bool Inno.Editor.Assets.AssetEditor.CanOpen(Inno.Editor.Assets.AssetEditorContext context)`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditor.cs#L18) | Gets whether the fallback asset editor can open the supplied source entry. <br>`context`: The immutable source and catalog snapshot for the operation. | true when Inno.Editor.Assets.AssetEditor.Open(Inno.Editor.Assets.AssetEditorContext) can handle the entry; otherwise, false. | — |
| <a id="api-7e5674d95bd29f89"></a>[`public virtual void Inno.Editor.Assets.AssetEditor.Open(Inno.Editor.Assets.AssetEditorContext context)`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditor.cs#L25) | Opens the supplied asset entry when no more specific typed open action handled it. <br>`context`: The immutable source and catalog snapshot for the operation. | — | — |
| <a id="api-67cc107d92086b31"></a>[`public virtual Inno.Editor.Assets.AssetOperationValidation Inno.Editor.Assets.AssetEditor.ValidateRename(Inno.Editor.Assets.AssetEditorContext context, string targetPath)`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditor.cs#L41) | Validates a requested asset move before the AssetPipeline transaction begins. <br>`context`: The immutable snapshot of the source entry before the move.<br>`targetPath`: The normalized source-relative destination path. | A valid result when the transaction may proceed, or a diagnostic describing the rejection. | — |
| <a id="api-ccec27ef9e34a870"></a>[`public virtual void Inno.Editor.Assets.AssetEditor.OnRenamed(Inno.Editor.Assets.AssetEditorContext context, string oldPath, string newPath)`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditor.cs#L57) | Runs after an asset move transaction commits successfully. <br>`context`: The immutable snapshot captured before the move.<br>`oldPath`: The previous source-relative path.<br>`newPath`: The committed source-relative destination path. | — | — |
| <a id="api-586eea434f8d75e5"></a>[`public virtual Inno.Editor.Assets.AssetOperationValidation Inno.Editor.Assets.AssetEditor.ValidateDelete(Inno.Editor.Assets.AssetEditorContext context)`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditor.cs#L73) | Validates a requested asset deletion before the AssetPipeline transaction begins. <br>`context`: The immutable snapshot of the entry that would be deleted. | A valid result when the transaction may proceed, or a diagnostic describing the rejection. | — |
| <a id="api-2104fc3634d46203"></a>[`public virtual void Inno.Editor.Assets.AssetEditor.OnDeleted(Inno.Editor.Assets.AssetEditorContext context)`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditor.cs#L80) | Runs after an asset deletion transaction commits successfully. <br>`context`: The immutable snapshot captured before the entry was deleted. | — | — |
| <a id="api-13bae727ca0171db"></a>[`public virtual bool Inno.Editor.Assets.AssetEditor.CanStartDrag(Inno.Editor.Assets.AssetEditorContext context)`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditor.cs#L93) | Gets whether the supplied entry can begin a managed editor drag operation. <br>`context`: The immutable source and catalog snapshot for the entry. | true when drag data can be created; otherwise, false. | — |
| <a id="api-c254ff7f5cbf0af4"></a>[`public virtual Inno.Editor.Interactions.EditorDragData Inno.Editor.Assets.AssetEditor.CreateDragData(Inno.Editor.Assets.AssetEditorContext context)`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditor.cs#L103) | Creates the managed source, preview label, and validity predicate for an asset drag. <br>`context`: The immutable source and catalog snapshot for the entry. | The managed drag data published by the Asset Browser. | — |
| <a id="api-17ac170374826ad7"></a>[`protected Inno.Editor.Assets.AssetEditor.AssetEditor()`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditor.cs#L7) | 由 C# 编译器生成的值契约成员。 | — | — |
| <a id="api-9e75a4054e1104b4"></a>[`public sealed class Inno.Editor.Assets.AssetEditorAttribute : System.Attribute`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditorAttribute.cs#L8) | Associates an asset editor with an imported asset type. | — | — |
| <a id="api-7cf3786ed12fd17d"></a>[`public Inno.Editor.Assets.AssetEditorAttribute.AssetEditorAttribute(System.Type assetType, bool useForChildren = false, int priority = 0)`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditorAttribute.cs#L25) | Creates an asset-editor registration for an imported runtime asset type. <br>`assetType`: The imported asset type handled by the editor.<br>`useForChildren`: Whether the editor may also handle assignable derived asset types.<br>`priority`: The tie-breaking priority after exactness and inheritance distance. | — | `System.ArgumentNullException`: Thrown when assetType is null. |
| <a id="api-91e6834db874ab13"></a>[`public System.Type Inno.Editor.Assets.AssetEditorAttribute.assetType { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditorAttribute.cs#L38) | Gets the imported asset type handled by the editor. | — | — |
| <a id="api-8b0f09ec5161a760"></a>[`public bool Inno.Editor.Assets.AssetEditorAttribute.useForChildren { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditorAttribute.cs#L42) | Gets whether derived asset types are accepted. | — | — |
| <a id="api-67b543dbda82d828"></a>[`public int Inno.Editor.Assets.AssetEditorAttribute.priority { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditorAttribute.cs#L46) | Gets the tie-breaking priority. | — | — |
| <a id="api-c26a033e4f65341b"></a>[`public sealed class Inno.Editor.Assets.AssetEditorContext`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditorContext.cs#L10) | Provides an immutable snapshot for an asset editor operation. | — | — |
| <a id="api-651c7fea5b7e56e5"></a>[`public Inno.Editor.Core.EditorContext Inno.Editor.Assets.AssetEditorContext.editorContext { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditorContext.cs#L66) | Gets the shared editor context. | — | — |
| <a id="api-78f80c669174f0de"></a>[`public Inno.Editor.Interactions.EditorInteractions Inno.Editor.Assets.AssetEditorContext.interactions { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditorContext.cs#L70) | Gets the active editor interaction entry point. | — | — |
| <a id="api-b8e33e1d40420343"></a>[`public string Inno.Editor.Assets.AssetEditorContext.relativePath { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditorContext.cs#L74) | Gets the source-relative path. | — | — |
| <a id="api-db7d06288707ee18"></a>[`public string Inno.Editor.Assets.AssetEditorContext.name { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditorContext.cs#L78) | Gets the final source path segment. | — | — |
| <a id="api-f615e7d52e376ea3"></a>[`public bool Inno.Editor.Assets.AssetEditorContext.isDirectory { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditorContext.cs#L82) | Gets whether the source represents a directory. | — | — |
| <a id="api-719e0ae233966466"></a>[`public Inno.Assets.AssetInfo? Inno.Editor.Assets.AssetEditorContext.info { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditorContext.cs#L86) | Gets the cataloged asset information when available. | — | — |
| <a id="api-dae823d362a52b3b"></a>[`public System.Type? Inno.Editor.Assets.AssetEditorContext.assetType { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetEditorContext.cs#L90) | Gets the resolved imported asset type when available. | — | — |
| <a id="api-50e990261b0c3636"></a>[`public readonly record struct Inno.Editor.Assets.AssetOperationValidation : System.IEquatable<Inno.Editor.Assets.AssetOperationValidation>`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetOperationValidation.cs#L5) | Describes whether an editor asset operation may proceed. | — | — |
| <a id="api-859088f723e68981"></a>[`public Inno.Editor.Assets.AssetOperationValidation.AssetOperationValidation(bool isValid, string message)`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetOperationValidation.cs#L16) | Creates the validation result returned before an asset transaction begins. <br>`isValid`: Whether the requested asset operation may proceed.<br>`message`: The user-facing rejection diagnostic, or an empty string for a valid result. | — | — |
| <a id="api-80b6301b71101d0a"></a>[`public bool Inno.Editor.Assets.AssetOperationValidation.isValid { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetOperationValidation.cs#L27) | Gets whether the operation may proceed. | — | — |
| <a id="api-b7934667c607e3ce"></a>[`public string Inno.Editor.Assets.AssetOperationValidation.message { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetOperationValidation.cs#L31) | Gets the validation diagnostic. | — | — |
| <a id="api-495933659e6980b9"></a>[`public static Inno.Editor.Assets.AssetOperationValidation Inno.Editor.Assets.AssetOperationValidation.valid { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetOperationValidation.cs#L35) | Gets a successful validation result. | — | — |
| <a id="api-4d5766ff502f9b09"></a>[`public static Inno.Editor.Assets.AssetOperationValidation Inno.Editor.Assets.AssetOperationValidation.Invalid(string message)`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetOperationValidation.cs#L46) | Creates a failed asset-operation validation result. <br>`message`: The user-facing diagnostic explaining why the operation was rejected. | An invalid validation result containing the supplied diagnostic. | — |
| <a id="api-3e437a0706ecebb2"></a>[`public override string Inno.Editor.Assets.AssetOperationValidation.ToString()`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetOperationValidation.cs#L5) | Returns the fully qualified type name of this instance. | The fully qualified type name. | — |
| <a id="api-c9087d90b157568b"></a>[`public static bool Inno.Editor.Assets.AssetOperationValidation.operator !=(Inno.Editor.Assets.AssetOperationValidation left, Inno.Editor.Assets.AssetOperationValidation right)`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetOperationValidation.cs#L5) | 由 C# 编译器生成的值契约成员。 | — | — |
| <a id="api-c7c78401d9d42c01"></a>[`public static bool Inno.Editor.Assets.AssetOperationValidation.operator ==(Inno.Editor.Assets.AssetOperationValidation left, Inno.Editor.Assets.AssetOperationValidation right)`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetOperationValidation.cs#L5) | 由 C# 编译器生成的值契约成员。 | — | — |
| <a id="api-6d67b2e5e8eac432"></a>[`public override int Inno.Editor.Assets.AssetOperationValidation.GetHashCode()`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetOperationValidation.cs#L5) | Returns the hash code for this instance. | A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-63c0b6540ff35372"></a>[`public override bool Inno.Editor.Assets.AssetOperationValidation.Equals(object obj)`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetOperationValidation.cs#L5) | Indicates whether this instance and a specified object are equal. <br>`obj`: The object to compare with the current instance. | true if obj and this instance are the same type and represent the same value; otherwise, false. | — |
| <a id="api-5af14db72c6fcc01"></a>[`public bool Inno.Editor.Assets.AssetOperationValidation.Equals(Inno.Editor.Assets.AssetOperationValidation other)`](../../src/composition/editor/features/Inno.Editor.Assets/Editing/AssetOperationValidation.cs#L5) | Indicates whether the current object is equal to another object of the same type. <br>`other`: An object to compare with this object. | true if the current object is equal to the other parameter; otherwise, false. | — |
| <a id="api-bb29963e7b7b9871"></a>[`public sealed class Inno.Editor.Assets.AssetImportSettingsEdits : Inno.Editor.Core.EditorModule`](../../src/composition/editor/features/Inno.Editor.Assets/ImportSettings/AssetImportSettingsEdits.cs#L15) | Applies importer configuration through the common sidecar pipeline and stable-identity Editor history. | — | — |
| <a id="api-83a3562a1aee14e3"></a>[`public Inno.Editor.Assets.AssetImportSettingsEdits.AssetImportSettingsEdits(Inno.Assets.Pipeline.AssetPipeline assets, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Extensibility.Types.TypeCatalog types, Inno.Editor.Interactions.EditorInteractions interactions)`](../../src/composition/editor/features/Inno.Editor.Assets/ImportSettings/AssetImportSettingsEdits.cs#L38) | Uses the authoring owners responsible for source identity, converter generations and shared undo. <br>`assets`: Authoritative asset pipeline.<br>`serialization`: Current owner converter registry.<br>`types`: Current type generation owner.<br>`interactions`: Shared Editor history owner. | — | — |
| <a id="api-319970551a9a1bda"></a>[`public bool Inno.Editor.Assets.AssetImportSettingsEdits.Apply(Inno.Assets.AssetPath path, Inno.Core.Serialization.ISerializable settings, string expectedFingerprint)`](../../src/composition/editor/features/Inno.Editor.Assets/ImportSettings/AssetImportSettingsEdits.cs#L65) | Saves one settings gesture, recording neutral before/after properties even when reimport reports an error. <br>`path`: Writable source asset path.<br>`settings`: Detached current-generation settings value.<br>`expectedFingerprint`: Sidecar fingerprint captured when editing began. | Whether the saved configuration reimported successfully; false does not mean the settings were discarded. | — |
| <a id="api-99453873fb49824b"></a>[`public sealed class Inno.Editor.Assets.AssetSampleImportModule : Inno.Editor.Core.EditorModule`](../../src/composition/editor/features/Inno.Editor.Assets/Samples/AssetSampleImportModule.cs#L20) | Owns sample import validation, publication and rollback independently of its progress window. | — | — |
| <a id="api-52cad00ef5033eae"></a>[`public override bool Inno.Editor.Assets.AssetSampleImportModule.blocksFollowingUpdates { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Samples/AssetSampleImportModule.cs#L51) | Defers later modules while the import pins their generation or requires a Host restart. | — | — |
| <a id="api-a0aaf56c08809a1f"></a>[`public bool Inno.Editor.Assets.AssetSampleImportModule.isBusy { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Samples/AssetSampleImportModule.cs#L55) | Gets whether an import still owns validation or publication work. | — | — |
| <a id="api-d4573738296864b6"></a>[`public bool Inno.Editor.Assets.AssetSampleImportModule.canImport { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Samples/AssetSampleImportModule.cs#L59) | Gets whether the current authoring generation admits a new sample import. | — | — |
| <a id="api-7011dbf2a3639c7a"></a>[`public Inno.Editor.Assets.AssetSampleImportProgress Inno.Editor.Assets.AssetSampleImportModule.progress { get; }`](../../src/composition/editor/features/Inno.Editor.Assets/Samples/AssetSampleImportModule.cs#L64) | Gets a complete immutable progress snapshot under the existing owner lock. | — | — |
| <a id="api-147bc5a76063959a"></a>[`public void Inno.Editor.Assets.AssetSampleImportModule.Request(Inno.Assets.AssetPath source)`](../../src/composition/editor/features/Inno.Editor.Assets/Samples/AssetSampleImportModule.cs#L82) | Begins validating an installed sample before its source copy can be committed. <br>`source`: The installed sample root to validate and import. | — | `System.InvalidOperationException`: The authoring generation is busy or faulted. |
| <a id="api-5f7d4a7c5f5d4ed8"></a>[`public void Inno.Editor.Assets.AssetSampleImportModule.Cancel(long operationId)`](../../src/composition/editor/features/Inno.Editor.Assets/Samples/AssetSampleImportModule.cs#L114) | Cancels only the import observed by the presentation. <br>`operationId`: The current import identity captured by the caller. | — | — |
| <a id="api-e8cc5cda1811a95d"></a>[`protected override void Inno.Editor.Assets.AssetSampleImportModule.OnUpdate(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/features/Inno.Editor.Assets/Samples/AssetSampleImportModule.cs#L126) | Advances completed background phases and publishes the validated import at a frame safe point. <br>`context`: The current Editor frame and interaction services. | — | — |
| <a id="api-1ef6f419b4bad7a5"></a>[`protected override void Inno.Editor.Assets.AssetSampleImportModule.OnStop(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/features/Inno.Editor.Assets/Samples/AssetSampleImportModule.cs#L202) | Cancels and retires pending work before the scripting and asset dependencies stop. <br>`context`: The stopping Editor's interaction services. | — | — |
| <a id="api-f1dcf004ca586efa"></a>[`protected override void Inno.Editor.Assets.AssetSampleImportModule.OnDispose()`](../../src/composition/editor/features/Inno.Editor.Assets/Samples/AssetSampleImportModule.cs#L211) | Retries retirement without releasing dependencies still used by background work. | — | — |
| <a id="api-f207cb2827c221a6"></a>[`public readonly record struct Inno.Editor.Assets.AssetSampleImportProgress : System.IEquatable<Inno.Editor.Assets.AssetSampleImportProgress>`](../../src/composition/editor/features/Inno.Editor.Assets/Samples/AssetSampleImportProgress.cs#L18) | Contains one atomically captured sample import status. <br>`operationId`: The import identity within its owner.<br>`fraction`: The completed validation and publication fraction.<br>`status`: The current stage description.<br>`cancellationRequested`: Whether cancellation is draining owned work. | — | — |
| <a id="api-20c56a5ee029088b"></a>[`public Inno.Editor.Assets.AssetSampleImportProgress.AssetSampleImportProgress(long operationId, float fraction, string status, bool cancellationRequested)`](../../src/composition/editor/features/Inno.Editor.Assets/Samples/AssetSampleImportProgress.cs#L18) | Contains one atomically captured sample import status. <br>`operationId`: The import identity within its owner.<br>`fraction`: The completed validation and publication fraction.<br>`status`: The current stage description.<br>`cancellationRequested`: Whether cancellation is draining owned work. | — | — |
| <a id="api-c66aee2ae5eef7fc"></a>[`public long Inno.Editor.Assets.AssetSampleImportProgress.operationId { get; init; }`](../../src/composition/editor/features/Inno.Editor.Assets/Samples/AssetSampleImportProgress.cs#L19) | The import identity within its owner. | — | — |
| <a id="api-344622e4c2b859f0"></a>[`public float Inno.Editor.Assets.AssetSampleImportProgress.fraction { get; init; }`](../../src/composition/editor/features/Inno.Editor.Assets/Samples/AssetSampleImportProgress.cs#L20) | The completed validation and publication fraction. | — | — |
| <a id="api-9a0d9d6edce69e80"></a>[`public string Inno.Editor.Assets.AssetSampleImportProgress.status { get; init; }`](../../src/composition/editor/features/Inno.Editor.Assets/Samples/AssetSampleImportProgress.cs#L21) | The current stage description. | — | — |
| <a id="api-e9db6d73e502c115"></a>[`public bool Inno.Editor.Assets.AssetSampleImportProgress.cancellationRequested { get; init; }`](../../src/composition/editor/features/Inno.Editor.Assets/Samples/AssetSampleImportProgress.cs#L22) | Whether cancellation is draining owned work. | — | — |
| <a id="api-3826d90b5215f564"></a>[`public override string Inno.Editor.Assets.AssetSampleImportProgress.ToString()`](../../src/composition/editor/features/Inno.Editor.Assets/Samples/AssetSampleImportProgress.cs#L18) | Returns the fully qualified type name of this instance. | The fully qualified type name. | — |
| <a id="api-23f591ac313404e0"></a>[`public static bool Inno.Editor.Assets.AssetSampleImportProgress.operator !=(Inno.Editor.Assets.AssetSampleImportProgress left, Inno.Editor.Assets.AssetSampleImportProgress right)`](../../src/composition/editor/features/Inno.Editor.Assets/Samples/AssetSampleImportProgress.cs#L18) | 由 C# 编译器生成的值契约成员。 | — | — |
| <a id="api-05def61b1e485d1d"></a>[`public static bool Inno.Editor.Assets.AssetSampleImportProgress.operator ==(Inno.Editor.Assets.AssetSampleImportProgress left, Inno.Editor.Assets.AssetSampleImportProgress right)`](../../src/composition/editor/features/Inno.Editor.Assets/Samples/AssetSampleImportProgress.cs#L18) | 由 C# 编译器生成的值契约成员。 | — | — |
| <a id="api-895e07a02d64b455"></a>[`public override int Inno.Editor.Assets.AssetSampleImportProgress.GetHashCode()`](../../src/composition/editor/features/Inno.Editor.Assets/Samples/AssetSampleImportProgress.cs#L18) | Returns the hash code for this instance. | A 32-bit signed integer that is the hash code for this instance. | — |
| <a id="api-2099a8a2fcf7c4e3"></a>[`public override bool Inno.Editor.Assets.AssetSampleImportProgress.Equals(object obj)`](../../src/composition/editor/features/Inno.Editor.Assets/Samples/AssetSampleImportProgress.cs#L18) | Indicates whether this instance and a specified object are equal. <br>`obj`: The object to compare with the current instance. | true if obj and this instance are the same type and represent the same value; otherwise, false. | — |
| <a id="api-91a3c2bb844b2290"></a>[`public bool Inno.Editor.Assets.AssetSampleImportProgress.Equals(Inno.Editor.Assets.AssetSampleImportProgress other)`](../../src/composition/editor/features/Inno.Editor.Assets/Samples/AssetSampleImportProgress.cs#L18) | Indicates whether the current object is equal to another object of the same type. <br>`other`: An object to compare with this object. | true if the current object is equal to the other parameter; otherwise, false. | — |
| <a id="api-43e01932b06869f1"></a>[`public void Inno.Editor.Assets.AssetSampleImportProgress.Deconstruct(out long operationId, out float fraction, out string status, out bool cancellationRequested)`](../../src/composition/editor/features/Inno.Editor.Assets/Samples/AssetSampleImportProgress.cs#L18) | 由 C# 编译器生成的值契约成员。 | — | — |

## 项目依赖

| 工程 | 边界 |
| --- | --- |
| `Inno.Editor.Scripting` | 实现依赖；不传递 compile surface |
| `Inno.Core.IO` | 实现依赖；不传递 compile surface |
| `Inno.Core.Execution` | 实现依赖；不传递 compile surface |
| `Inno.Scripting.Api` | 实现依赖；不传递 compile surface |
| `Inno.Scripting.Compiler` | 实现依赖；不传递 compile surface |
| `Inno.Core.Serialization` | 公开签名依赖 |
| `Inno.Core.Logging` | 公开签名依赖 |
| `Inno.Extensibility.Types` | 公开签名依赖 |
| `Inno.Extensibility.Catalogs` | 公开签名依赖 |
| `Inno.Plugins.Authoring` | 公开签名依赖 |
| `Inno.Assets` | 公开签名依赖 |
| `Inno.Assets.Pipeline` | 公开签名依赖 |
| `Inno.Editor.Core` | 公开签名依赖 |
| `Inno.Editor.Interactions` | 公开签名依赖 |
