# Inno.References

[References 索引](README.md) · [Wiki 首页](../README.md) · [专项强制标准](../architecture/IDENTITY_REFERENCE_RELOAD_STANDARD.md)

## 职责与边界

该 Content 层程序集统一持久引用意图、Missing 槽位和批量恢复，不拥有 Assets/Scene/Editor，也不扫描插件程序集。
公开依赖为 Core.Identity 与 Extensibility.Reload；Core.Execution 只用于内部退休控制。
Foundation 的 Settings/Graphs 不得反向引用本项目，相关恢复应由上层 owner 组合。

## 公开协议

| API | 稳定语义 |
| --- | --- |
| `ReferenceKindId` | 开放的解析协议 ID，不是对象 Identity |
| `ReferenceKey(ownerPersistentId, path)` | 一个持久 owner 的稳定槽位；重复 key 在事务捕获时失败 |
| `ReferenceDescriptor(kindId, targetPersistentId, expectedStableTypeId, lastKnownName, lastKnownPath)` | 持久对象引用；名称/路径仅用于诊断，空 target 是 Unassigned |
| `ReferenceResolutionState` | Unassigned / Resolved / Missing / TypeMismatch / Invalid |
| `ReferenceResolution` | descriptor、state、可选 runtimeIdentity、diagnostic；只有 Resolved 携带当前 domain 的 runtimeIdentity |
| `IReferenceResolver.kindId / Resolve` | 当前代解析边界，不发布对象；不得替换传入 descriptor |
| `ReferenceCatalog.empty / Create / Resolve / generation` | immutable resolver 集合，拒绝重复 kind；未安装 resolver 保留 Missing |
| `SerializedMissingState(key, descriptor, payload)` | 中立恢复槽位及复制的 bytes；也用于捕获即将变成 Missing 的 live 槽位 |
| `ReferenceRecoveryChange.missingState / resolution` | provisional 对象应用后的槽位解析结果，不持有目标实例 |
| `IReferenceRecoveryParticipant` | 继承统一 `IGenerationChange` 五阶段协议，并实现 commit 前的 `Validate(changes)` |
| `ReferenceRecoveryTransaction` | 将中立槽位和有序 participant 纳入同一 generation 事务；无自行 Activate/Dispose 的旁路 |

不存在可继承的 Recovery 基类。领域通过接口组合，不必继承与其生命周期无关的类。

## 初始化、发布与回滚

构造 `ReferenceRecoveryTransaction(catalog, missingStates, participants)` 只捕获并检查 key，不改变 live state。
调用 owner 必须在捕获线程和安全点使用同一个 GenerationCoordinator：

1. `PrepareForActivation`：旧 publication 仍有效，逐个登记并 quiesce participant。
2. publication 激活 Assembly/Type/Serializer 与外部 Asset 候选。
3. `Apply`：所有 participant 构建 provisional 对象与属性，然后统一解析 slots、调用所有 `Validate`。
4. publication commit 后调用 `Complete`；退休完成才释放 participant 与 resolver 引用。
5. 提交前失败：逆序 `RollbackStructure` → publication rollback → 逆序 `RestorePreviousState`。

不能在 Apply 的 catch 或 Dispose 中自行恢复旧属性，因为旧 converter/resolver 当时尚未发布。
原子性由整个 generation owner 保证：provisional 状态只允许本次事务访问，不能插入游戏帧或对外报告成功。

```csharp
using System.Collections.Generic;
using Inno.Extensibility.Reload;
using Inno.References;

static TProbe Recover<TProbe>(
    GenerationCoordinator generations,
    IGenerationPublication<TProbe> publication,
    ReferenceCatalog candidate,
    IEnumerable<SerializedMissingState> states,
    IEnumerable<IReferenceRecoveryParticipant> participants)
    where TProbe : IAssemblyUnloadProbe
{
    var recovery = new ReferenceRecoveryTransaction(candidate, states, participants);
    return generations.Execute("reference recovery", publication, [recovery]);
}
```

方法返回 monitor 不代表 collectible ALC 已完成回收；最外层 owner 仍须推进同一 GC barrier。
不要在仍持有 candidate/旧 Type 的调用栈里同步等待自己造成的保留。

## 失败与所有权

- Prepare 开始前登记 participant；部分失败的 Prepare/Apply 也会得到补偿，未开始的 participant 不执行领域回滚。
- Validate 可以允许嵌套 Missing，但不能把 Missing 改写成 null 或伪造 Resolved。
- 构造、阶段顺序、跨线程调用、重复槽位明确失败。
- 普通补偿/清理失败继续尝试其他 owner，聚合上报；generation gate 决定 Fault，不只记录日志。
- RetirementPendingException（包括 Aggregate/InnerException 包装）不归类为普通失败，不执行更低层清理，不清空 participant；保留原始外层错误，共享 gate Fault 后需重启 Host。
- 成功与完整回滚释放 generation owner；`changes` 在成功后可保留用于诊断，回滚后为空。
- `changes` 中的 RuntimeIdentity 是瞬时解析结果，不得写入 History、Settings、Scene 或 Asset metadata。
- `ReferenceCatalog` 冻结的是 resolver 集合；resolver 本身按其明确 owner/generation 读取当前 canonical identity。不能把旧 catalog 当成跨代 live object 缓存。

## 生产接入

SceneReloadService 已用真实 Component/System 和 Asset dependency 槽位创建该事务：
element 槽位使用对象 persistent ID，Stable Type ID 只表达类型约束；Asset 槽位沿用 AssetReferenceProtocol。
Scene domain participant 保留原结构和中立 bytes；类型缺失产生占位，恢复失败按上述两阶段顺序撤销。

Editor History 使用 SceneElementSerialization 的中立 element state，能在类型缺失时 Undo 创建原 ID 的占位；
恢复后原 Redo 可继续使用。History 不建立第二个 Recovery owner，不保存 candidate 或插件对象。
Assets 的全量 importer/canonical recovery、Graph、Settings、Plugin availability 的统一批次接入仍未完成，
不能把这条 Scene/Asset-reference/History 生产链写成整个 C07 已关闭。

RuntimeSession 的 catalog 由 Composition 注入 authoring resolver；Player AssetDatabase 自动贡献 Asset resolver。
领域从明确构造边界接收解析器，通用 RuntimeSubsystemContext 不暴露整个 Session。
引用感知序列化统一使用 owner 的 AssetSerializationContext，不从 SerializationContext.empty 临时拼接。

## 中立内容读取

`ContentReadScope(contents, activeContent)` 捕获 Identity 数组，不保留 Scene、Asset 或其他 live root。
`contents` 是冻结副本；`GetValues<T>()` / `TryGetValue<T>(persistentId, out value)` 通过原弱 Identity registry
解析当前 runtime slot。重复、未注册 identity 和不在集合内的 activeContent 被拒绝。
Dispose 或跨线程读取失败；不会自动重绑定到新 generation。

SceneContentSource.CreateScope(world) 为 Rendering 与 Audio 提供同一个内容协议。
当前验证范围和未执行项见[当前验收](../architecture/CURRENT_ACCEPTANCE.md)。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.References.ContentReadScope`

<!-- api: T:Inno.References.ContentReadScope c7e6c73fe43a9eb127b19640f7bead4563495f5afb89a9d93dd9b8e9fc6ea788 -->
<!-- api: M:Inno.References.ContentReadScope.#ctor(System.Collections.Generic.IEnumerable{Inno.Core.Identity.Identity},System.Nullable{System.Guid}) 92d4e6e5eaab0e09c800687810d6657414f246b80f852487adb0e54bb7d2360c -->
<!-- api: P:Inno.References.ContentReadScope.empty 9c4deae0c882bd82d8ff243e48d0e4d4847ce2586f2793af98d2a161baf4b4de -->
<!-- api: P:Inno.References.ContentReadScope.contents 469143d9ad1d1a8edd5bcc0c0cb9d07d2aa142fe78d4fec49de54ef844698222 -->
<!-- api: P:Inno.References.ContentReadScope.activeContent ae8de64479df1f81237aa7982c2db1bd6d76d6e8de8e8689d0ae34a0a4b3403b -->
<!-- api: M:Inno.References.ContentReadScope.GetValues``1 ec4c067024fd9660e4a7de175f12d15960beaf5257bfc825fa451d88234cf42f -->
<!-- api: M:Inno.References.ContentReadScope.TryGetValue``1(System.Guid,``0@) 6695c0f65644d80503563e630004e8da2d2590e2346b3beabd99438d222ba7b6 -->
<!-- api: M:Inno.References.ContentReadScope.Dispose 8ec00f70d5abd375a8de5590309761b33b4d71dbca408608b2e87247074fbeff -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-7c376803f7945d0d"></a>[`public Inno.References.ContentReadScope.ContentReadScope(System.Collections.Generic.IEnumerable<Inno.Core.Identity.Identity> contents, System.Guid? activeContent = null)`](../../src/content/references/Inno.References/ContentReadScope.cs#L32) | Captures immutable root identities without retaining their live objects or registry.<br>`contents`：Registered identities in deterministic root order.<br>`activeContent`：The optional persistent identity of the primary root. | — | `System.ArgumentException`：A root is detached, duplicated, or the active root is not present. |
| <a id="api-0c06c316950fb408"></a>[`public void Inno.References.ContentReadScope.Dispose()`](../../src/content/references/Inno.References/ContentReadScope.cs#L126) | Revokes all future root resolution without affecting the owner objects. | — | — |
| <a id="api-7572085436d8520d"></a>[`public System.Collections.Generic.IReadOnlyList<TValue> Inno.References.ContentReadScope.GetValues<TValue>() where TValue : Inno.Core.Identity.IdentityObject`](../../src/content/references/Inno.References/ContentReadScope.cs#L78) | Resolves all live roots matching the requested content contract.<br>返回：A read-only operation snapshot; stale or incompatible roots are omitted. | — | `System.ObjectDisposedException`：The operation scope has ended.<br>`System.InvalidOperationException`：Lookup occurs off the owning control thread. |
| <a id="api-55364057e9e17079"></a>[`public bool Inno.References.ContentReadScope.TryGetValue<TValue>(System.Guid id, out TValue? value) where TValue : Inno.Core.Identity.IdentityObject`](../../src/content/references/Inno.References/ContentReadScope.cs#L106) | Resolves a selected root without rebinding a stale snapshot to a replacement generation.<br>返回：True only when the captured runtime identity is still live and compatible.<br>`id`：The persistent identity selecting a root already captured by this scope.<br>`value`：The live compatible object, or null when unavailable. | — | `System.ObjectDisposedException`：The operation scope has ended.<br>`System.InvalidOperationException`：Lookup occurs off the owning control thread. |
| <a id="api-e54722a004804472"></a>[`public System.Guid? Inno.References.ContentReadScope.activeContent { get; }`](../../src/content/references/Inno.References/ContentReadScope.cs#L61) | Gets the optional persistent identity selected as primary content. | — | — |
| <a id="api-730b7982d5f1d557"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Core.Identity.Identity> Inno.References.ContentReadScope.contents { get; }`](../../src/content/references/Inno.References/ContentReadScope.cs#L56) | Gets the ordered weak identity snapshots; no live root object is retained. | — | — |
| <a id="api-96b1986731361095"></a>[`public static Inno.References.ContentReadScope Inno.References.ContentReadScope.empty { get; }`](../../src/content/references/Inno.References/ContentReadScope.cs#L51) | Gets a new empty operation scope owned by the caller. | — | — |
| <a id="api-327be83e57ee1253"></a>[`public sealed class Inno.References.ContentReadScope : System.IDisposable`](../../src/content/references/Inno.References/ContentReadScope.cs#L14) | Exposes ordered content roots through weak, generation-checked identity snapshots for one control-thread operation. | Disposing the scope revokes lookup. Copy values into domain snapshots; never retain resolved objects across frames or reloads. | — |

### `Inno.References.IReferenceRecoveryParticipant`

<!-- api: T:Inno.References.IReferenceRecoveryParticipant a9bfedcd95383063b391d5485db18a882ea38f3de1bc5a4db1608f9fe44c4ab4 -->
<!-- api: M:Inno.References.IReferenceRecoveryParticipant.Validate(System.Collections.Generic.IReadOnlyList{Inno.References.ReferenceRecoveryChange}) 79f149c99e6bd71b4bde54266bbdf19f441d8250d0641bcd711b814e9cbda0dc -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-29f5da24bd317785"></a>[`void Inno.References.IReferenceRecoveryParticipant.Validate(System.Collections.Generic.IReadOnlyList<Inno.References.ReferenceRecoveryChange> changes)`](../../src/content/references/Inno.References/IReferenceRecoveryParticipant.cs#L17) | Validates resolved slots after provisional structures and values have been applied, before commit.<br>`changes`：The complete immutable set of candidate recovery changes; optional missing slots remain preserved. | — | — |
| <a id="api-378d66627b415a97"></a>[`public interface Inno.References.IReferenceRecoveryParticipant : Inno.Extensibility.Reload.IGenerationChange`](../../src/content/references/Inno.References/IReferenceRecoveryParticipant.cs#L9) | Applies domain-owned missing and recovered representations as one candidate transaction participant. | — | — |

### `Inno.References.IReferenceResolver`

<!-- api: T:Inno.References.IReferenceResolver 0f74c2bdf7ce16bb9e2fb84fa8c5a06f0faacacc5db6a39f752e69cab83f2d28 -->
<!-- api: P:Inno.References.IReferenceResolver.kindId 59a690ed7b8a12e4a65898afb7dfbcc4916ca56f691c4d4634c1c1f30374ddc6 -->
<!-- api: M:Inno.References.IReferenceResolver.Resolve(Inno.References.ReferenceDescriptor) 2523d88ca2779aef752ee017d7f807965906c3e1a7a348938e63d79d01eca2b1 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-95ef2d82fe04ccc5"></a>[`Inno.References.ReferenceResolution Inno.References.IReferenceResolver.Resolve(Inno.References.ReferenceDescriptor descriptor)`](../../src/content/references/Inno.References/IReferenceResolver.cs#L22) | Resolves a descriptor without mutating its owner or the authoritative catalog.<br>返回：A complete resolution result that preserves the supplied descriptor.<br>`descriptor`：The descriptor whose Inno.References.ReferenceDescriptor.kindId matches this resolver. | — | — |
| <a id="api-84245e9b38d2c926"></a>[`Inno.References.ReferenceKindId Inno.References.IReferenceResolver.kindId { get; }`](../../src/content/references/Inno.References/IReferenceResolver.cs#L11) | Gets the unique reference kind implemented by this resolver. | — | — |
| <a id="api-724def1662d5024b"></a>[`public interface Inno.References.IReferenceResolver`](../../src/content/references/Inno.References/IReferenceResolver.cs#L6) | Resolves one open reference kind against a complete immutable domain generation. | — | — |

### `Inno.References.ReferenceCatalog`

<!-- api: T:Inno.References.ReferenceCatalog cda5e08ddb76250c99c1cbb77688c03d39cf82f5facd15659137d59b6c4f2586 -->
<!-- api: P:Inno.References.ReferenceCatalog.empty 6c9e393df012b041d9502500e4f09dd7639b80257e9c1de060b1eb8821f47d0e -->
<!-- api: P:Inno.References.ReferenceCatalog.generation afe8d278ae23c3a0c7df86a1cd349be26e9be4833e9c74d5df0d498f54cc0aa4 -->
<!-- api: M:Inno.References.ReferenceCatalog.Create(System.Int64,System.Collections.Generic.IEnumerable{Inno.References.IReferenceResolver}) bda231813284efb30882be38c167093f0cd62e24e06089409b9c6b956647805c -->
<!-- api: M:Inno.References.ReferenceCatalog.Resolve(Inno.References.ReferenceDescriptor) f885f35ec34be7a4bb79279038374be381ab407f47d33778c72cf6cfb1202e1a -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-2f5d6ec536389766"></a>[`public static Inno.References.ReferenceCatalog Inno.References.ReferenceCatalog.Create(long generation, System.Collections.Generic.IEnumerable<Inno.References.IReferenceResolver> resolvers)`](../../src/content/references/Inno.References/ReferenceCatalog.cs#L52) | Builds a complete resolver generation and rejects duplicate protocol identifiers.<br>返回：An immutable reference catalog ready for publication.<br>`generation`：The positive generation assigned by the catalog owner.<br>`resolvers`：Every resolver that belongs to the candidate generation. | — | `System.ArgumentOutOfRangeException`：Thrown when generation is not positive.<br>`System.ArgumentNullException`：Thrown when resolvers or one of its elements is null.<br>`System.InvalidOperationException`：Thrown when a resolver kind is invalid or duplicated. |
| <a id="api-d63544c925305759"></a>[`public Inno.References.ReferenceResolution Inno.References.ReferenceCatalog.Resolve(Inno.References.ReferenceDescriptor descriptor)`](../../src/content/references/Inno.References/ReferenceCatalog.cs#L86) | Resolves a persistent descriptor using the resolver selected by its kind.<br>返回：The resolver result, an unassigned result for an empty target, or a missing result when no resolver is installed.<br>`descriptor`：The descriptor to resolve. | — | `System.ArgumentNullException`：Thrown when descriptor is null.<br>`System.InvalidOperationException`：Thrown when a resolver returns a result for a different descriptor. |
| <a id="api-677b1258bbd55b00"></a>[`public static Inno.References.ReferenceCatalog Inno.References.ReferenceCatalog.empty { get; }`](../../src/content/references/Inno.References/ReferenceCatalog.cs#L24) | Gets an empty initial reference catalog. | — | — |
| <a id="api-e0a66941a49c405f"></a>[`public long Inno.References.ReferenceCatalog.generation { get; }`](../../src/content/references/Inno.References/ReferenceCatalog.cs#L29) | Gets the owner-assigned generation represented by this snapshot. | — | — |
| <a id="api-9485d0799274b30f"></a>[`public sealed class Inno.References.ReferenceCatalog`](../../src/content/references/Inno.References/ReferenceCatalog.cs#L9) | Owns one complete immutable generation of reference resolvers. | — | — |

### `Inno.References.ReferenceDescriptor`

<!-- api: T:Inno.References.ReferenceDescriptor a91a24bcf1b0d8448460a9d062f6e07c67ab2a23deb0907a0776abe4921e6163 -->
<!-- api: M:Inno.References.ReferenceDescriptor.#ctor(Inno.References.ReferenceKindId,System.Guid,System.Guid,System.String,System.String) bca2bfcc16fcfc833d72b179e3ab177aef93a4e0c58db8fe7b62e2cf76a1e676 -->
<!-- api: P:Inno.References.ReferenceDescriptor.kindId 9f35f6242b6067380c01a713173f24830281af4080f4328f87ea607388ce0943 -->
<!-- api: P:Inno.References.ReferenceDescriptor.targetPersistentId b7b622b6177acf4654e1bb0ae9ee1ffe0481c88394121a318010e7d3b8a650ef -->
<!-- api: P:Inno.References.ReferenceDescriptor.expectedStableTypeId 61e803d8bdf2ec34f364d7fe6165e56548a00e6670ed2965f54212db064e54b7 -->
<!-- api: P:Inno.References.ReferenceDescriptor.lastKnownName 4c93f94a94d65b62995054b6cc406eb30bab1cf2d28e2f19a516ab5df94e902d -->
<!-- api: P:Inno.References.ReferenceDescriptor.lastKnownPath eae85fd593616925ba8960c587d562c350a2ef37befc77d5eabb3a7637548095 -->
<!-- api: P:Inno.References.ReferenceDescriptor.isUnassigned daf7ec6e8983069a583b1f3d05ba8e69cbdc8d3ff14680193fe49afbddbfed0f -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-9d15a551f4142f54"></a>[`public Inno.References.ReferenceDescriptor.ReferenceDescriptor(Inno.References.ReferenceKindId kindId, System.Guid targetPersistentId, System.Guid expectedStableTypeId = default(System.Guid), string? lastKnownName = null, string? lastKnownPath = null)`](../../src/content/references/Inno.References/ReferenceDescriptor.cs#L31) | Creates a persistent reference descriptor.<br>`kindId`：The open protocol used to resolve the target.<br>`targetPersistentId`：The target object's persistent identity, or an empty value for an explicitly unassigned slot.<br>`expectedStableTypeId`：An optional stable type constraint interpreted by the resolver.<br>`lastKnownName`：Optional non-authoritative display text retained while the target is missing.<br>`lastKnownPath`：Optional non-authoritative source path retained for diagnostics. | — | `System.ArgumentException`：Thrown when kindId is invalid. |
| <a id="api-225a44ba37b039d9"></a>[`public System.Guid Inno.References.ReferenceDescriptor.expectedStableTypeId { get; }`](../../src/content/references/Inno.References/ReferenceDescriptor.cs#L60) | Gets the optional stable type constraint interpreted by the selected resolver. | — | — |
| <a id="api-3cc7a43dd3effa5b"></a>[`public bool Inno.References.ReferenceDescriptor.isUnassigned { get; }`](../../src/content/references/Inno.References/ReferenceDescriptor.cs#L75) | Gets whether the user explicitly left this reference unassigned. | — | — |
| <a id="api-acb19c44bf1b20a4"></a>[`public Inno.References.ReferenceKindId Inno.References.ReferenceDescriptor.kindId { get; }`](../../src/content/references/Inno.References/ReferenceDescriptor.cs#L50) | Gets the protocol used to resolve this reference. | — | — |
| <a id="api-fceaf75e30a1ee1c"></a>[`public string? Inno.References.ReferenceDescriptor.lastKnownName { get; }`](../../src/content/references/Inno.References/ReferenceDescriptor.cs#L65) | Gets optional display text retained only for diagnostics and missing-state presentation. | — | — |
| <a id="api-5f978099d7506cdf"></a>[`public string? Inno.References.ReferenceDescriptor.lastKnownPath { get; }`](../../src/content/references/Inno.References/ReferenceDescriptor.cs#L70) | Gets an optional last-known source path that is never used as resolution authority. | — | — |
| <a id="api-0e2648447791233d"></a>[`public System.Guid Inno.References.ReferenceDescriptor.targetPersistentId { get; }`](../../src/content/references/Inno.References/ReferenceDescriptor.cs#L55) | Gets the persistent identity of the intended target, or an empty value when unassigned. | — | — |
| <a id="api-7478933caa756035"></a>[`public sealed class Inno.References.ReferenceDescriptor`](../../src/content/references/Inno.References/ReferenceDescriptor.cs#L8) | Preserves the stable intent and diagnostic metadata for one logical object reference. | — | — |

### `Inno.References.ReferenceKey`

<!-- api: T:Inno.References.ReferenceKey b9bc5b46a022dd6a093bd18edc35f0c6e4f1ca2c00756ca13c4337e797c5e194 -->
<!-- api: M:Inno.References.ReferenceKey.#ctor(System.Guid,System.String) 16828d41a4cfd6eda09fe017096f57f44c526bcc3c799fb95171590196fb6888 -->
<!-- api: P:Inno.References.ReferenceKey.ownerPersistentId 7e123ca88779f32b7170a31cece700d7920d6a86ecc67e934f734fd66db4abed -->
<!-- api: P:Inno.References.ReferenceKey.path bc6f7b1be7d64acef88e2c81a58a59e8c954f63c2bb6f52b1dd2832321480b8b -->
<!-- api: M:Inno.References.ReferenceKey.Equals(Inno.References.ReferenceKey) d9c06b6f9706e8758e757a08cf713cf91601ad2c0323447b58fbdb7ee75aa28c -->
<!-- api: M:Inno.References.ReferenceKey.Equals(System.Object) 2cd8c9a32e05f93789a546f3fd5e60f59d7528eb9bed9425bf512a6381c2404a -->
<!-- api: M:Inno.References.ReferenceKey.GetHashCode 411d325ad74dcf3840a9808021be49978e75b180b0f3659ab88872f6ec8445f9 -->
<!-- api: M:Inno.References.ReferenceKey.ToString dbeaa998c2bb6ed22cd8fbf2e4a01df585f4dc1118ae76f83d8ff978c2348238 -->
<!-- api: M:Inno.References.ReferenceKey.op_Equality(Inno.References.ReferenceKey,Inno.References.ReferenceKey) 63c6886954e3fe26eb40a618c507109f017066451f809d91cb544d363c486fe8 -->
<!-- api: M:Inno.References.ReferenceKey.op_Inequality(Inno.References.ReferenceKey,Inno.References.ReferenceKey) 63e18091c35d1c6959c412cabb723094adbc3ee968626ca87f4c9db3e0d937df -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-2a1e5a44099c19f6"></a>[`public Inno.References.ReferenceKey.ReferenceKey(System.Guid ownerPersistentId, string path)`](../../src/content/references/Inno.References/ReferenceKey.cs#L22) | Creates a key for one reference slot.<br>`ownerPersistentId`：The persistent identity of the object that owns the slot.<br>`path`：The stable property or structural path inside the owner. | — | `System.ArgumentException`：Thrown when the owner identity or path is empty. |
| <a id="api-c8ad2136ac85ab22"></a>[`public bool Inno.References.ReferenceKey.Equals(Inno.References.ReferenceKey other)`](../../src/content/references/Inno.References/ReferenceKey.cs#L53) | Determines whether two values identify the same owner slot.<br>返回：true when the owner identity and path are equal; otherwise, false.<br>`other`：The key to compare with this value. | — | — |
| <a id="api-0bec50fc049ca2ce"></a>[`public override bool Inno.References.ReferenceKey.Equals(object? obj)`](../../src/content/references/Inno.References/ReferenceKey.cs#L65) | Determines whether an object identifies the same owner slot.<br>返回：true when the object is an equal key; otherwise, false.<br>`obj`：The object to compare with this value. | — | — |
| <a id="api-84e04252a7168ca5"></a>[`public override int Inno.References.ReferenceKey.GetHashCode()`](../../src/content/references/Inno.References/ReferenceKey.cs#L73) | Computes a hash code from the owner identity and stable path.<br>返回：A hash code consistent with reference-key equality. | — | — |
| <a id="api-e52aa886fecb1379"></a>[`public static bool Inno.References.ReferenceKey.operator ==(Inno.References.ReferenceKey left, Inno.References.ReferenceKey right)`](../../src/content/references/Inno.References/ReferenceKey.cs#L95) | Determines whether two reference keys are equal.<br>返回：true when the keys are equal; otherwise, false.<br>`left`：The first key.<br>`right`：The second key. | — | — |
| <a id="api-5ceec4608814ab52"></a>[`public static bool Inno.References.ReferenceKey.operator !=(Inno.References.ReferenceKey left, Inno.References.ReferenceKey right)`](../../src/content/references/Inno.References/ReferenceKey.cs#L112) | Determines whether two reference keys are different.<br>返回：true when the keys are different; otherwise, false.<br>`left`：The first key.<br>`right`：The second key. | — | — |
| <a id="api-106e9fc3ab8e4fe7"></a>[`public override string Inno.References.ReferenceKey.ToString()`](../../src/content/references/Inno.References/ReferenceKey.cs#L81) | Formats this slot identity for diagnostics.<br>返回：The owner identity followed by the stable slot path. | — | — |
| <a id="api-223388bdb65271ea"></a>[`public System.Guid Inno.References.ReferenceKey.ownerPersistentId { get; }`](../../src/content/references/Inno.References/ReferenceKey.cs#L37) | Gets the persistent identity of the object that owns this reference slot. | — | — |
| <a id="api-d9c14a5e4f524ad5"></a>[`public string Inno.References.ReferenceKey.path { get; }`](../../src/content/references/Inno.References/ReferenceKey.cs#L42) | Gets the stable property or structural path inside the owner. | — | — |
| <a id="api-15dcdec238303654"></a>[`public readonly struct Inno.References.ReferenceKey : System.IEquatable<Inno.References.ReferenceKey>`](../../src/content/references/Inno.References/ReferenceKey.cs#L8) | Identifies one persistent reference slot owned by a logical object. | — | — |

### `Inno.References.ReferenceKindId`

<!-- api: T:Inno.References.ReferenceKindId 8c77405bcab3fd7bac4b6d901d078529e46ea610d848ca098656f3b10e8abe99 -->
<!-- api: M:Inno.References.ReferenceKindId.#ctor(System.String) 807e961a9a0add68a4cbc5850f7e3afaadc2382fa26c17d64d48bdd4bdb4e609 -->
<!-- api: P:Inno.References.ReferenceKindId.value 043d26c95fbe508d1446552d666b2340262c38e74934d5478c65e5fbdffa2bbf -->
<!-- api: P:Inno.References.ReferenceKindId.isValid df49a3c87c56b7753032073f940a4c6c44f1485d34acc3429fb59b3e50690f50 -->
<!-- api: M:Inno.References.ReferenceKindId.Equals(Inno.References.ReferenceKindId) 2177ecf0bee6319afa4b35f305f4897ee1c51babfa6f0ed499ff7860c0215663 -->
<!-- api: M:Inno.References.ReferenceKindId.Equals(System.Object) 3693b28f565b9ed6870cadfae8951bda15f8a312e7ee518a24e3af9ec4153654 -->
<!-- api: M:Inno.References.ReferenceKindId.GetHashCode 37245fb0bb90b0633b038699fc01ebee8797899122dbbd626d5ed8fa170fecf0 -->
<!-- api: M:Inno.References.ReferenceKindId.ToString 1f543f8fdd8da87a233d2aff412c53223050f1ce3134076ef80aed65acb659bf -->
<!-- api: M:Inno.References.ReferenceKindId.op_Equality(Inno.References.ReferenceKindId,Inno.References.ReferenceKindId) 90b77bb294f4b43cc8b697a36bc06ea8f1923aff6b188f598640d317fb087aab -->
<!-- api: M:Inno.References.ReferenceKindId.op_Inequality(Inno.References.ReferenceKindId,Inno.References.ReferenceKindId) 04abee6dfaef8f998801ef03979efd854f7ea425b62b84cdbeb9c89146bebecb -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-1281d97bde975f83"></a>[`public Inno.References.ReferenceKindId.ReferenceKindId(string value)`](../../src/content/references/Inno.References/ReferenceKindId.cs#L19) | Creates a stable reference kind identifier.<br>`value`：The non-empty protocol identifier. | — | `System.ArgumentException`：Thrown when value is empty or contains surrounding whitespace. |
| <a id="api-88fc066e7a8cd027"></a>[`public bool Inno.References.ReferenceKindId.Equals(Inno.References.ReferenceKindId other)`](../../src/content/references/Inno.References/ReferenceKindId.cs#L45) | Compares two identifiers using ordinal protocol identity.<br>返回：true when the identifiers are equal; otherwise, false.<br>`other`：The identifier to compare with this value. | — | — |
| <a id="api-50698b456e55a2ef"></a>[`public override bool Inno.References.ReferenceKindId.Equals(object? obj)`](../../src/content/references/Inno.References/ReferenceKindId.cs#L56) | Determines whether an object contains the same reference kind identifier.<br>返回：true when the object is an equal identifier; otherwise, false.<br>`obj`：The object to compare with this value. | — | — |
| <a id="api-e7a5b2abdcebe730"></a>[`public override int Inno.References.ReferenceKindId.GetHashCode()`](../../src/content/references/Inno.References/ReferenceKindId.cs#L64) | Computes an ordinal hash code for this identifier.<br>返回：A hash code consistent with ordinal equality. | — | — |
| <a id="api-cedd8d9c606ea28b"></a>[`public static bool Inno.References.ReferenceKindId.operator ==(Inno.References.ReferenceKindId left, Inno.References.ReferenceKindId right)`](../../src/content/references/Inno.References/ReferenceKindId.cs#L86) | Determines whether two reference kind identifiers are equal.<br>返回：true when the identifiers are equal; otherwise, false.<br>`left`：The first identifier.<br>`right`：The second identifier. | — | — |
| <a id="api-22d6d237e7412d32"></a>[`public static bool Inno.References.ReferenceKindId.operator !=(Inno.References.ReferenceKindId left, Inno.References.ReferenceKindId right)`](../../src/content/references/Inno.References/ReferenceKindId.cs#L103) | Determines whether two reference kind identifiers are different.<br>返回：true when the identifiers are different; otherwise, false.<br>`left`：The first identifier.<br>`right`：The second identifier. | — | — |
| <a id="api-e410c0e29134532e"></a>[`public override string Inno.References.ReferenceKindId.ToString()`](../../src/content/references/Inno.References/ReferenceKindId.cs#L72) | Formats the protocol identifier for diagnostics.<br>返回：The stable identifier, or <invalid> when empty. | — | — |
| <a id="api-5cc8eb41544c568f"></a>[`public bool Inno.References.ReferenceKindId.isValid { get; }`](../../src/content/references/Inno.References/ReferenceKindId.cs#L34) | Gets whether this value contains a usable protocol identifier. | — | — |
| <a id="api-2372e49a3c87f319"></a>[`public string Inno.References.ReferenceKindId.value { get; }`](../../src/content/references/Inno.References/ReferenceKindId.cs#L29) | Gets the stable protocol identifier. | — | — |
| <a id="api-de15ead94eef7195"></a>[`public readonly struct Inno.References.ReferenceKindId : System.IEquatable<Inno.References.ReferenceKindId>`](../../src/content/references/Inno.References/ReferenceKindId.cs#L8) | Identifies an open reference-resolution protocol independently of its current implementation. | — | — |

### `Inno.References.ReferenceRecoveryChange`

<!-- api: T:Inno.References.ReferenceRecoveryChange 824f0c24aaf09f74736943c01f37ea07330d137d4e4769dfd21959fccbcd83d1 -->
<!-- api: M:Inno.References.ReferenceRecoveryChange.#ctor(Inno.References.SerializedMissingState,Inno.References.ReferenceResolution) 50e23c7c5605e49b7af2bfdb124546d12446a182219d3474d2df3ee096ce18be -->
<!-- api: P:Inno.References.ReferenceRecoveryChange.missingState 37cacc343dbf18a85a2862bb0aa1dd603fd86eedcc5cc330555d5e8f408922bd -->
<!-- api: P:Inno.References.ReferenceRecoveryChange.resolution 91a348126d76cb6728c554ddff55589d18a14f864ca7e226c5250c9308868ddb -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-0f0274f7df7685e9"></a>[`public Inno.References.ReferenceRecoveryChange.ReferenceRecoveryChange(Inno.References.SerializedMissingState missingState, Inno.References.ReferenceResolution resolution)`](../../src/content/references/Inno.References/ReferenceRecoveryChange.cs#L20) | Creates a recovery change from preserved state and its candidate resolution.<br>`missingState`：The immutable state captured before the candidate generation was applied.<br>`resolution`：The result produced by the candidate reference catalog. | — | `System.ArgumentNullException`：Thrown when either argument is null. |
| <a id="api-0038a1eb33a3be06"></a>[`public Inno.References.SerializedMissingState Inno.References.ReferenceRecoveryChange.missingState { get; }`](../../src/content/references/Inno.References/ReferenceRecoveryChange.cs#L33) | Gets the preserved owner and target state. | — | — |
| <a id="api-c9158ba2d82bf88c"></a>[`public Inno.References.ReferenceResolution Inno.References.ReferenceRecoveryChange.resolution { get; }`](../../src/content/references/Inno.References/ReferenceRecoveryChange.cs#L38) | Gets the candidate generation's resolution for the preserved descriptor. | — | — |
| <a id="api-c09fea0ea84a0817"></a>[`public sealed class Inno.References.ReferenceRecoveryChange`](../../src/content/references/Inno.References/ReferenceRecoveryChange.cs#L6) | Describes one candidate missing-state transition at an owner-thread safe point. | — | — |

### `Inno.References.ReferenceRecoveryTransaction`

<!-- api: T:Inno.References.ReferenceRecoveryTransaction bbed3d1ecd182e85bb6343658eb22b5897416e09076e74468fd460576ee0c976 -->
<!-- api: M:Inno.References.ReferenceRecoveryTransaction.#ctor(Inno.References.ReferenceCatalog,System.Collections.Generic.IEnumerable{Inno.References.SerializedMissingState},System.Collections.Generic.IEnumerable{Inno.References.IReferenceRecoveryParticipant}) 2d1900738658540ce26f471a5539fd8b83d5d5e0ba81df139f6135e5a3b0fe5c -->
<!-- api: P:Inno.References.ReferenceRecoveryTransaction.changes e3ae4297045145ce263d4f401ae6d2059e017272339d7acf0566bc8f42a5508d -->
<!-- api: M:Inno.References.ReferenceRecoveryTransaction.PrepareForActivation 5cda3b5dcf50ba5c75a0e5d7e6fe86f4ed16f57ea8d9cb2addebcf2f9bff9a9c -->
<!-- api: M:Inno.References.ReferenceRecoveryTransaction.Apply 65182cf023863f5824db4ec0b20245e12c4b292606f3e358264ea2d7cf386bc6 -->
<!-- api: M:Inno.References.ReferenceRecoveryTransaction.Complete 72f8e121c5553ccfb2c3a868aa1b526f739bcb2a5b8598e698196b66db6bb804 -->
<!-- api: M:Inno.References.ReferenceRecoveryTransaction.RollbackStructure b234e42db1fa3066f1275c4e94b0e590c70596202c6924ff7a1eb70cc7a941c7 -->
<!-- api: M:Inno.References.ReferenceRecoveryTransaction.RestorePreviousState aa47b90144ede7e566051b288ecb91e56a08755bf40a2a64c6300ec2fa28fbf6 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b61ab80a24872ab8"></a>[`public Inno.References.ReferenceRecoveryTransaction.ReferenceRecoveryTransaction(Inno.References.ReferenceCatalog catalog, System.Collections.Generic.IEnumerable<Inno.References.SerializedMissingState> missingStates, System.Collections.Generic.IEnumerable<Inno.References.IReferenceRecoveryParticipant> participants)`](../../src/content/references/Inno.References/ReferenceRecoveryTransaction.cs#L45) | Captures neutral slots and ordered domain owners without resolving or mutating live state.<br>`catalog`：The candidate resolver set, consulted after provisional objects are registered.<br>`missingStates`：Preserved object and dependency slots, each with a unique owner key.<br>`participants`：Domain changes in dependency order, owned until commit or complete rollback. | — | `System.ArgumentNullException`：An input or collection entry is null.<br>`System.ArgumentException`：A slot key is invalid or duplicated. |
| <a id="api-996bf223bf0f9c26"></a>[`public void Inno.References.ReferenceRecoveryTransaction.Apply()`](../../src/content/references/Inno.References/ReferenceRecoveryTransaction.cs#L97) | Applies provisional objects and values, resolves preserved slots, and validates the candidate before commit. | — | `System.InvalidOperationException`：Preparation is incomplete or the caller is not the owner thread. |
| <a id="api-d5cd52dfd7df292c"></a>[`public void Inno.References.ReferenceRecoveryTransaction.Complete()`](../../src/content/references/Inno.References/ReferenceRecoveryTransaction.cs#L122) | Retires previous objects after irreversible publication and releases every completed domain owner. | — | `System.InvalidOperationException`：Application is incomplete or the caller is not the owner thread.<br>`System.AggregateException`：One or more completed retirement operations failed.<br>`Inno.Core.Execution.RetirementPendingException`：Unfinished retirement retains all owners and blocks further phases. |
| <a id="api-d83ff7601097298e"></a>[`public void Inno.References.ReferenceRecoveryTransaction.PrepareForActivation()`](../../src/content/references/Inno.References/ReferenceRecoveryTransaction.cs#L79) | Quiesces domain owners while the previous type and serialization publication is still active. | — | `System.InvalidOperationException`：The phase is out of order or the caller is not the owner thread. |
| <a id="api-cf2c098d471b5855"></a>[`public void Inno.References.ReferenceRecoveryTransaction.RestorePreviousState()`](../../src/content/references/Inno.References/ReferenceRecoveryTransaction.cs#L169) | Restores previous values and lifecycle after the old type and serializer publication is active again. | — | `System.InvalidOperationException`：Structure rollback is incomplete or the caller is not the owner thread.<br>`System.AggregateException`：One or more previous-state restorations failed.<br>`Inno.Core.Execution.RetirementPendingException`：An unfinished restoration retains its owners. |
| <a id="api-f9a3b8016b7882a9"></a>[`public void Inno.References.ReferenceRecoveryTransaction.RollbackStructure()`](../../src/content/references/Inno.References/ReferenceRecoveryTransaction.cs#L141) | Restores attempted domain structures in reverse order before the previous publication is restored. | — | `System.InvalidOperationException`：The transaction is terminal or the caller is not the owner thread.<br>`System.AggregateException`：One or more structure compensations failed.<br>`Inno.Core.Execution.RetirementPendingException`：An unfinished compensation retains its owners. |
| <a id="api-e7a2a30736b8ad3b"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.References.ReferenceRecoveryChange> Inno.References.ReferenceRecoveryTransaction.changes { get; }`](../../src/content/references/Inno.References/ReferenceRecoveryTransaction.cs#L71) | Gets candidate resolutions after provisional application, or an empty set before resolution or after rollback. | — | — |
| <a id="api-69bd6f7e2916e24d"></a>[`public sealed class Inno.References.ReferenceRecoveryTransaction : Inno.Extensibility.Reload.IGenerationChange`](../../src/content/references/Inno.References/ReferenceRecoveryTransaction.cs#L16) | Coordinates preserved reference slots using the host generation publication and two-phase rollback protocol. | The generation owner calls these phases at its safe point. This transaction never publishes types or rolls back automatically: old structures must be restored before old converters, and old values after them. | — |

### `Inno.References.ReferenceResolution`

<!-- api: T:Inno.References.ReferenceResolution e544086b328d16ea776e85f4502b397fe17d4ed66bb6c677afe12c8484a33176 -->
<!-- api: M:Inno.References.ReferenceResolution.#ctor(Inno.References.ReferenceDescriptor,Inno.References.ReferenceResolutionState,System.Nullable{Inno.Core.Identity.RuntimeIdentity},System.String) 6d75c93d22a8330c47c062925ccd488f3a8fe0e3e93416a8a657c0f5529d7db0 -->
<!-- api: P:Inno.References.ReferenceResolution.descriptor 69f6e51e06f07d40118470d78593a5a953bc220d5d20f060d0fbc510bc1f2909 -->
<!-- api: P:Inno.References.ReferenceResolution.state 6ee04d4b7952854757c9255201a99504695628b8f40a4df017a129803e0fbe5f -->
<!-- api: P:Inno.References.ReferenceResolution.runtimeIdentity cf9b953b909cf7d6d25fd85ae891795e5140d49aac0b3810b58f64725284b8b7 -->
<!-- api: P:Inno.References.ReferenceResolution.diagnostic d2bc7a379d7d3ee0fdcd9d9a867c6eb5ff7a011be10f57b3e5c701994ff7e56f -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-a31e5af3c549de4d"></a>[`public Inno.References.ReferenceResolution.ReferenceResolution(Inno.References.ReferenceDescriptor descriptor, Inno.References.ReferenceResolutionState state, Inno.Core.Identity.RuntimeIdentity? runtimeIdentity = null, string? diagnostic = null)`](../../src/content/references/Inno.References/ReferenceResolution.cs#L32) | Creates a reference-resolution result.<br>`descriptor`：The persistent descriptor that was resolved.<br>`state`：The resulting state.<br>`runtimeIdentity`：The domain-qualified live target identity when resolved.<br>`diagnostic`：Optional caller-facing detail for non-resolved states. | — | `System.ArgumentNullException`：Thrown when descriptor is null.<br>`System.ArgumentException`：Thrown when resolved state and runtime identity are inconsistent. |
| <a id="api-29e6307e435f8ea5"></a>[`public Inno.References.ReferenceDescriptor Inno.References.ReferenceResolution.descriptor { get; }`](../../src/content/references/Inno.References/ReferenceResolution.cs#L50) | Gets the persistent descriptor that produced this result. | — | — |
| <a id="api-1f3f4be09a73b3ab"></a>[`public string? Inno.References.ReferenceResolution.diagnostic { get; }`](../../src/content/references/Inno.References/ReferenceResolution.cs#L65) | Gets optional diagnostics describing why the reference is not resolved. | — | — |
| <a id="api-6dc923eca7a50cbb"></a>[`public Inno.Core.Identity.RuntimeIdentity? Inno.References.ReferenceResolution.runtimeIdentity { get; }`](../../src/content/references/Inno.References/ReferenceResolution.cs#L60) | Gets the live target identity when Inno.References.ReferenceResolution.state is Inno.References.ReferenceResolutionState.Resolved. | — | — |
| <a id="api-e42e67e875721358"></a>[`public Inno.References.ReferenceResolutionState Inno.References.ReferenceResolution.state { get; }`](../../src/content/references/Inno.References/ReferenceResolution.cs#L55) | Gets the current resolution state. | — | — |
| <a id="api-3cc0741d247ebad8"></a>[`public sealed class Inno.References.ReferenceResolution`](../../src/content/references/Inno.References/ReferenceResolution.cs#L9) | Reports how one descriptor resolves in an immutable reference-catalog generation. | — | — |

### `Inno.References.ReferenceResolutionState`

<!-- api: T:Inno.References.ReferenceResolutionState 52b0f6c8d2f2b2ae84365381db24eb861e422393ad5434239208dafd229c00cc -->
<!-- api: F:Inno.References.ReferenceResolutionState.Unassigned 3cd46ad4ec1b00b252c6ca3cbd523727f28095b8afe4b69650c298d90bfd1fbe -->
<!-- api: F:Inno.References.ReferenceResolutionState.Resolved f7528756009baa806d05fc67d619065b79c4b98509a2a3e01b5b284acf0a798a -->
<!-- api: F:Inno.References.ReferenceResolutionState.Missing a37f3bd4050f0c096171f4a3e593d447faf1dc996fe86eda57d00395bd67f416 -->
<!-- api: F:Inno.References.ReferenceResolutionState.TypeMismatch 8eb7b3a0cfdc226333cd6a9e7f795fbdf22c36b3efc8fc04dfe4d52754e504ee -->
<!-- api: F:Inno.References.ReferenceResolutionState.Invalid c28a97d6b7076250e9b0191eeb02b8f082a17b03c2f10b8ce40f7e3d34d7b244 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-44bee6c88a0236e0"></a>[`Inno.References.ReferenceResolutionState.Invalid = 4`](../../src/content/references/Inno.References/ReferenceResolutionState.cs#L31) | The descriptor or authoritative source is invalid. | — | — |
| <a id="api-e688054114437215"></a>[`Inno.References.ReferenceResolutionState.Missing = 2`](../../src/content/references/Inno.References/ReferenceResolutionState.cs#L21) | The intended target is temporarily unavailable. | — | — |
| <a id="api-7ca9bbd1aaa5b8f6"></a>[`Inno.References.ReferenceResolutionState.Resolved = 1`](../../src/content/references/Inno.References/ReferenceResolutionState.cs#L16) | The current generation resolved a compatible live target. | — | — |
| <a id="api-018a86922d641e6e"></a>[`Inno.References.ReferenceResolutionState.TypeMismatch = 3`](../../src/content/references/Inno.References/ReferenceResolutionState.cs#L26) | The intended target exists but does not satisfy the stable type constraint. | — | — |
| <a id="api-0fbc8919eeabe166"></a>[`Inno.References.ReferenceResolutionState.Unassigned = 0`](../../src/content/references/Inno.References/ReferenceResolutionState.cs#L11) | The slot has no assigned target. | — | — |
| <a id="api-4cd97975e9f19575"></a>[`public enum Inno.References.ReferenceResolutionState : int`](../../src/content/references/Inno.References/ReferenceResolutionState.cs#L6) | Describes the current resolution state without discarding persistent reference intent. | — | — |

### `Inno.References.SerializedMissingState`

<!-- api: T:Inno.References.SerializedMissingState 8a914475fad5483d1d7767ae506aa1a58d1b12f561fe328d7c70bdb3f7319cba -->
<!-- api: M:Inno.References.SerializedMissingState.#ctor(Inno.References.ReferenceKey,Inno.References.ReferenceDescriptor,System.ReadOnlySpan{System.Byte}) 60905e1f1a7f6d4962efb4a216d24e9004617988e1ef0c676d9819a1e4b4c159 -->
<!-- api: P:Inno.References.SerializedMissingState.key d9f07b096ef21afb3d73f80ddd48588307129895bcf23bec3036ec1ff977414a -->
<!-- api: P:Inno.References.SerializedMissingState.descriptor bdb5121f1215657c0eaff28e466b72da33724da6bb66a459e1168a6feb8022e4 -->
<!-- api: P:Inno.References.SerializedMissingState.payload 8bae13400f83da1b1a01c357755e257c81866fcac437a674eb4d2d7cae449d81 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-71b5e83d0d8941cd"></a>[`public Inno.References.SerializedMissingState.SerializedMissingState(Inno.References.ReferenceKey key, Inno.References.ReferenceDescriptor descriptor, System.ReadOnlySpan<byte> payload)`](../../src/content/references/Inno.References/SerializedMissingState.cs#L27) | Creates an immutable missing-state record.<br>`key`：The persistent owner slot represented by this record.<br>`descriptor`：The persistent target intent that must survive unavailability.<br>`payload`：The neutral serialized owner state required for reconstruction. | — | `System.ArgumentNullException`：Thrown when descriptor is null. |
| <a id="api-398deaafa303cc8f"></a>[`public Inno.References.ReferenceDescriptor Inno.References.SerializedMissingState.descriptor { get; }`](../../src/content/references/Inno.References/SerializedMissingState.cs#L46) | Gets the persistent target intent preserved by this record. | — | — |
| <a id="api-d40e30531c0c7bb4"></a>[`public Inno.References.ReferenceKey Inno.References.SerializedMissingState.key { get; }`](../../src/content/references/Inno.References/SerializedMissingState.cs#L41) | Gets the persistent owner slot represented by this record. | — | — |
| <a id="api-dce8bf2d6fec543b"></a>[`public System.ReadOnlyMemory<byte> Inno.References.SerializedMissingState.payload { get; }`](../../src/content/references/Inno.References/SerializedMissingState.cs#L51) | Gets immutable neutral bytes used to reconstruct the owner state. | — | — |
| <a id="api-4b2d191ddba28bf3"></a>[`public sealed class Inno.References.SerializedMissingState`](../../src/content/references/Inno.References/SerializedMissingState.cs#L8) | Preserves one recoverable slot and its neutral owner state without retaining runtime objects. | — | — |

## 项目依赖

- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Reload](../extensibility/Inno.Extensibility.Reload.md)：公开引用边界由实际签名核对。
- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Identity](../core/Inno.Core.Identity.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
