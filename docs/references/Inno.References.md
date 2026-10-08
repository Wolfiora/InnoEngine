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
完整未完成项见[累积收口报告](../architecture/ENGINE_CLOSURE_CONTINUATION_2026_09_07.md)。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.References.ContentReadScope`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.References.ContentReadScope`](../../src/content/references/Inno.References/ContentReadScope.cs#L14) | Exposes ordered content roots through weak, generation-checked identity snapshots for one control-thread operation. |
| [`Inno.References.ContentReadScope.ContentReadScope(System.Collections.Generic.IEnumerable<Inno.Core.Identity.Identity> contents, System.Guid? activeContent = null)`](../../src/content/references/Inno.References/ContentReadScope.cs#L32) | Captures immutable root identities without retaining their live objects or registry. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Core.Identity.Identity> Inno.References.ContentReadScope.contents`](../../src/content/references/Inno.References/ContentReadScope.cs#L56) | Gets the ordered weak identity snapshots; no live root object is retained. |
| [`System.Collections.Generic.IReadOnlyList<TValue> Inno.References.ContentReadScope.GetValues<TValue>()`](../../src/content/references/Inno.References/ContentReadScope.cs#L78) | Resolves all live roots matching the requested content contract. |
| [`System.Guid? Inno.References.ContentReadScope.activeContent`](../../src/content/references/Inno.References/ContentReadScope.cs#L61) | Gets the optional persistent identity selected as primary content. |
| [`bool Inno.References.ContentReadScope.TryGetValue<TValue>(System.Guid id, out TValue? value)`](../../src/content/references/Inno.References/ContentReadScope.cs#L106) | Resolves a selected root without rebinding a stale snapshot to a replacement generation. |
| [`static Inno.References.ContentReadScope Inno.References.ContentReadScope.empty`](../../src/content/references/Inno.References/ContentReadScope.cs#L51) | Gets a new empty operation scope owned by the caller. |
| [`void Inno.References.ContentReadScope.Dispose()`](../../src/content/references/Inno.References/ContentReadScope.cs#L126) | Revokes all future root resolution without affecting the owner objects. |

### `Inno.References.IReferenceRecoveryParticipant`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.References.IReferenceRecoveryParticipant`](../../src/content/references/Inno.References/IReferenceRecoveryParticipant.cs#L9) | Applies domain-owned missing and recovered representations as one candidate transaction participant. |
| [`void Inno.References.IReferenceRecoveryParticipant.Validate(System.Collections.Generic.IReadOnlyList<Inno.References.ReferenceRecoveryChange> changes)`](../../src/content/references/Inno.References/IReferenceRecoveryParticipant.cs#L17) | Validates resolved slots after provisional structures and values have been applied, before commit. |

### `Inno.References.IReferenceResolver`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.References.IReferenceResolver`](../../src/content/references/Inno.References/IReferenceResolver.cs#L6) | Resolves one open reference kind against a complete immutable domain generation. |
| [`Inno.References.ReferenceKindId Inno.References.IReferenceResolver.kindId`](../../src/content/references/Inno.References/IReferenceResolver.cs#L11) | Gets the unique reference kind implemented by this resolver. |
| [`Inno.References.ReferenceResolution Inno.References.IReferenceResolver.Resolve(Inno.References.ReferenceDescriptor descriptor)`](../../src/content/references/Inno.References/IReferenceResolver.cs#L22) | Resolves a descriptor without mutating its owner or the authoritative catalog. |

### `Inno.References.ReferenceCatalog`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.References.ReferenceCatalog`](../../src/content/references/Inno.References/ReferenceCatalog.cs#L9) | Owns one complete immutable generation of reference resolvers. |
| [`Inno.References.ReferenceResolution Inno.References.ReferenceCatalog.Resolve(Inno.References.ReferenceDescriptor descriptor)`](../../src/content/references/Inno.References/ReferenceCatalog.cs#L86) | Resolves a persistent descriptor using the resolver selected by its kind. |
| [`long Inno.References.ReferenceCatalog.generation`](../../src/content/references/Inno.References/ReferenceCatalog.cs#L29) | Gets the owner-assigned generation represented by this snapshot. |
| [`static Inno.References.ReferenceCatalog Inno.References.ReferenceCatalog.Create(long generation, System.Collections.Generic.IEnumerable<Inno.References.IReferenceResolver> resolvers)`](../../src/content/references/Inno.References/ReferenceCatalog.cs#L52) | Builds a complete resolver generation and rejects duplicate protocol identifiers. |
| [`static Inno.References.ReferenceCatalog Inno.References.ReferenceCatalog.empty`](../../src/content/references/Inno.References/ReferenceCatalog.cs#L24) | Gets an empty initial reference catalog. |

### `Inno.References.ReferenceDescriptor`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.References.ReferenceDescriptor`](../../src/content/references/Inno.References/ReferenceDescriptor.cs#L8) | Preserves the stable intent and diagnostic metadata for one logical object reference. |
| [`Inno.References.ReferenceDescriptor.ReferenceDescriptor(Inno.References.ReferenceKindId kindId, System.Guid targetPersistentId, System.Guid expectedStableTypeId = default(System.Guid), string? lastKnownName = null, string? lastKnownPath = null)`](../../src/content/references/Inno.References/ReferenceDescriptor.cs#L31) | Creates a persistent reference descriptor. |
| [`Inno.References.ReferenceKindId Inno.References.ReferenceDescriptor.kindId`](../../src/content/references/Inno.References/ReferenceDescriptor.cs#L50) | Gets the protocol used to resolve this reference. |
| [`System.Guid Inno.References.ReferenceDescriptor.expectedStableTypeId`](../../src/content/references/Inno.References/ReferenceDescriptor.cs#L60) | Gets the optional stable type constraint interpreted by the selected resolver. |
| [`System.Guid Inno.References.ReferenceDescriptor.targetPersistentId`](../../src/content/references/Inno.References/ReferenceDescriptor.cs#L55) | Gets the persistent identity of the intended target, or an empty value when unassigned. |
| [`bool Inno.References.ReferenceDescriptor.isUnassigned`](../../src/content/references/Inno.References/ReferenceDescriptor.cs#L75) | Gets whether the user explicitly left this reference unassigned. |
| [`string? Inno.References.ReferenceDescriptor.lastKnownName`](../../src/content/references/Inno.References/ReferenceDescriptor.cs#L65) | Gets optional display text retained only for diagnostics and missing-state presentation. |
| [`string? Inno.References.ReferenceDescriptor.lastKnownPath`](../../src/content/references/Inno.References/ReferenceDescriptor.cs#L70) | Gets an optional last-known source path that is never used as resolution authority. |

### `Inno.References.ReferenceKey`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.References.ReferenceKey`](../../src/content/references/Inno.References/ReferenceKey.cs#L8) | Identifies one persistent reference slot owned by a logical object. |
| [`Inno.References.ReferenceKey.ReferenceKey(System.Guid ownerPersistentId, string path)`](../../src/content/references/Inno.References/ReferenceKey.cs#L22) | Creates a key for one reference slot. |
| [`System.Guid Inno.References.ReferenceKey.ownerPersistentId`](../../src/content/references/Inno.References/ReferenceKey.cs#L37) | Gets the persistent identity of the object that owns this reference slot. |
| [`bool Inno.References.ReferenceKey.Equals(Inno.References.ReferenceKey other)`](../../src/content/references/Inno.References/ReferenceKey.cs#L53) | Determines whether two values identify the same owner slot. |
| [`override bool Inno.References.ReferenceKey.Equals(object? obj)`](../../src/content/references/Inno.References/ReferenceKey.cs#L65) | Determines whether an object identifies the same owner slot. |
| [`override int Inno.References.ReferenceKey.GetHashCode()`](../../src/content/references/Inno.References/ReferenceKey.cs#L73) | Computes a hash code from the owner identity and stable path. |
| [`override string Inno.References.ReferenceKey.ToString()`](../../src/content/references/Inno.References/ReferenceKey.cs#L81) | Formats this slot identity for diagnostics. |
| [`static bool Inno.References.ReferenceKey.operator !=(Inno.References.ReferenceKey left, Inno.References.ReferenceKey right)`](../../src/content/references/Inno.References/ReferenceKey.cs#L112) | Determines whether two reference keys are different. |
| [`static bool Inno.References.ReferenceKey.operator ==(Inno.References.ReferenceKey left, Inno.References.ReferenceKey right)`](../../src/content/references/Inno.References/ReferenceKey.cs#L95) | Determines whether two reference keys are equal. |
| [`string Inno.References.ReferenceKey.path`](../../src/content/references/Inno.References/ReferenceKey.cs#L42) | Gets the stable property or structural path inside the owner. |

### `Inno.References.ReferenceKindId`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.References.ReferenceKindId`](../../src/content/references/Inno.References/ReferenceKindId.cs#L8) | Identifies an open reference-resolution protocol independently of its current implementation. |
| [`Inno.References.ReferenceKindId.ReferenceKindId(string value)`](../../src/content/references/Inno.References/ReferenceKindId.cs#L19) | Creates a stable reference kind identifier. |
| [`bool Inno.References.ReferenceKindId.Equals(Inno.References.ReferenceKindId other)`](../../src/content/references/Inno.References/ReferenceKindId.cs#L45) | Compares two identifiers using ordinal protocol identity. |
| [`bool Inno.References.ReferenceKindId.isValid`](../../src/content/references/Inno.References/ReferenceKindId.cs#L34) | Gets whether this value contains a usable protocol identifier. |
| [`override bool Inno.References.ReferenceKindId.Equals(object? obj)`](../../src/content/references/Inno.References/ReferenceKindId.cs#L56) | Determines whether an object contains the same reference kind identifier. |
| [`override int Inno.References.ReferenceKindId.GetHashCode()`](../../src/content/references/Inno.References/ReferenceKindId.cs#L64) | Computes an ordinal hash code for this identifier. |
| [`override string Inno.References.ReferenceKindId.ToString()`](../../src/content/references/Inno.References/ReferenceKindId.cs#L72) | Formats the protocol identifier for diagnostics. |
| [`static bool Inno.References.ReferenceKindId.operator !=(Inno.References.ReferenceKindId left, Inno.References.ReferenceKindId right)`](../../src/content/references/Inno.References/ReferenceKindId.cs#L103) | Determines whether two reference kind identifiers are different. |
| [`static bool Inno.References.ReferenceKindId.operator ==(Inno.References.ReferenceKindId left, Inno.References.ReferenceKindId right)`](../../src/content/references/Inno.References/ReferenceKindId.cs#L86) | Determines whether two reference kind identifiers are equal. |
| [`string Inno.References.ReferenceKindId.value`](../../src/content/references/Inno.References/ReferenceKindId.cs#L29) | Gets the stable protocol identifier. |

### `Inno.References.ReferenceRecoveryChange`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.References.ReferenceRecoveryChange`](../../src/content/references/Inno.References/ReferenceRecoveryChange.cs#L6) | Describes one candidate missing-state transition at an owner-thread safe point. |
| [`Inno.References.ReferenceRecoveryChange.ReferenceRecoveryChange(Inno.References.SerializedMissingState missingState, Inno.References.ReferenceResolution resolution)`](../../src/content/references/Inno.References/ReferenceRecoveryChange.cs#L20) | Creates a recovery change from preserved state and its candidate resolution. |
| [`Inno.References.ReferenceResolution Inno.References.ReferenceRecoveryChange.resolution`](../../src/content/references/Inno.References/ReferenceRecoveryChange.cs#L38) | Gets the candidate generation's resolution for the preserved descriptor. |
| [`Inno.References.SerializedMissingState Inno.References.ReferenceRecoveryChange.missingState`](../../src/content/references/Inno.References/ReferenceRecoveryChange.cs#L33) | Gets the preserved owner and target state. |

### `Inno.References.ReferenceRecoveryTransaction`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.References.ReferenceRecoveryTransaction`](../../src/content/references/Inno.References/ReferenceRecoveryTransaction.cs#L16) | Coordinates preserved reference slots using the host generation publication and two-phase rollback protocol. |
| [`Inno.References.ReferenceRecoveryTransaction.ReferenceRecoveryTransaction(Inno.References.ReferenceCatalog catalog, System.Collections.Generic.IEnumerable<Inno.References.SerializedMissingState> missingStates, System.Collections.Generic.IEnumerable<Inno.References.IReferenceRecoveryParticipant> participants)`](../../src/content/references/Inno.References/ReferenceRecoveryTransaction.cs#L45) | Captures neutral slots and ordered domain owners without resolving or mutating live state. |
| [`System.Collections.Generic.IReadOnlyList<Inno.References.ReferenceRecoveryChange> Inno.References.ReferenceRecoveryTransaction.changes`](../../src/content/references/Inno.References/ReferenceRecoveryTransaction.cs#L71) | Gets candidate resolutions after provisional application, or an empty set before resolution or after rollback. |
| [`void Inno.References.ReferenceRecoveryTransaction.Apply()`](../../src/content/references/Inno.References/ReferenceRecoveryTransaction.cs#L97) | Applies provisional objects and values, resolves preserved slots, and validates the candidate before commit. |
| [`void Inno.References.ReferenceRecoveryTransaction.Complete()`](../../src/content/references/Inno.References/ReferenceRecoveryTransaction.cs#L122) | Retires previous objects after irreversible publication and releases every completed domain owner. |
| [`void Inno.References.ReferenceRecoveryTransaction.PrepareForActivation()`](../../src/content/references/Inno.References/ReferenceRecoveryTransaction.cs#L79) | Quiesces domain owners while the previous type and serialization publication is still active. |
| [`void Inno.References.ReferenceRecoveryTransaction.RestorePreviousState()`](../../src/content/references/Inno.References/ReferenceRecoveryTransaction.cs#L169) | Restores previous values and lifecycle after the old type and serializer publication is active again. |
| [`void Inno.References.ReferenceRecoveryTransaction.RollbackStructure()`](../../src/content/references/Inno.References/ReferenceRecoveryTransaction.cs#L141) | Restores attempted domain structures in reverse order before the previous publication is restored. |

### `Inno.References.ReferenceResolution`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Identity.RuntimeIdentity? Inno.References.ReferenceResolution.runtimeIdentity`](../../src/content/references/Inno.References/ReferenceResolution.cs#L60) | Gets the live target identity when is . |
| [`Inno.References.ReferenceDescriptor Inno.References.ReferenceResolution.descriptor`](../../src/content/references/Inno.References/ReferenceResolution.cs#L50) | Gets the persistent descriptor that produced this result. |
| [`Inno.References.ReferenceResolution`](../../src/content/references/Inno.References/ReferenceResolution.cs#L9) | Reports how one descriptor resolves in an immutable reference-catalog generation. |
| [`Inno.References.ReferenceResolution.ReferenceResolution(Inno.References.ReferenceDescriptor descriptor, Inno.References.ReferenceResolutionState state, Inno.Core.Identity.RuntimeIdentity? runtimeIdentity = null, string? diagnostic = null)`](../../src/content/references/Inno.References/ReferenceResolution.cs#L32) | Creates a reference-resolution result. |
| [`Inno.References.ReferenceResolutionState Inno.References.ReferenceResolution.state`](../../src/content/references/Inno.References/ReferenceResolution.cs#L55) | Gets the current resolution state. |
| [`string? Inno.References.ReferenceResolution.diagnostic`](../../src/content/references/Inno.References/ReferenceResolution.cs#L65) | Gets optional diagnostics describing why the reference is not resolved. |

### `Inno.References.ReferenceResolutionState`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.References.ReferenceResolutionState`](../../src/content/references/Inno.References/ReferenceResolutionState.cs#L6) | Describes the current resolution state without discarding persistent reference intent. |
| [`Inno.References.ReferenceResolutionState.Invalid`](../../src/content/references/Inno.References/ReferenceResolutionState.cs#L31) | The descriptor or authoritative source is invalid. |
| [`Inno.References.ReferenceResolutionState.Missing`](../../src/content/references/Inno.References/ReferenceResolutionState.cs#L21) | The intended target is temporarily unavailable. |
| [`Inno.References.ReferenceResolutionState.Resolved`](../../src/content/references/Inno.References/ReferenceResolutionState.cs#L16) | The current generation resolved a compatible live target. |
| [`Inno.References.ReferenceResolutionState.TypeMismatch`](../../src/content/references/Inno.References/ReferenceResolutionState.cs#L26) | The intended target exists but does not satisfy the stable type constraint. |
| [`Inno.References.ReferenceResolutionState.Unassigned`](../../src/content/references/Inno.References/ReferenceResolutionState.cs#L11) | The slot has no assigned target. |

### `Inno.References.SerializedMissingState`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.References.ReferenceDescriptor Inno.References.SerializedMissingState.descriptor`](../../src/content/references/Inno.References/SerializedMissingState.cs#L46) | Gets the persistent target intent preserved by this record. |
| [`Inno.References.ReferenceKey Inno.References.SerializedMissingState.key`](../../src/content/references/Inno.References/SerializedMissingState.cs#L41) | Gets the persistent owner slot represented by this record. |
| [`Inno.References.SerializedMissingState`](../../src/content/references/Inno.References/SerializedMissingState.cs#L8) | Preserves one recoverable slot and its neutral owner state without retaining runtime objects. |
| [`Inno.References.SerializedMissingState.SerializedMissingState(Inno.References.ReferenceKey key, Inno.References.ReferenceDescriptor descriptor, System.ReadOnlySpan<byte> payload)`](../../src/content/references/Inno.References/SerializedMissingState.cs#L27) | Creates an immutable missing-state record. |
| [`System.ReadOnlyMemory<byte> Inno.References.SerializedMissingState.payload`](../../src/content/references/Inno.References/SerializedMissingState.cs#L51) | Gets immutable neutral bytes used to reconstruct the owner state. |

## 项目依赖

- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Reload](../extensibility/Inno.Extensibility.Reload.md)：公开引用边界由实际签名核对。
- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Identity](../core/Inno.Core.Identity.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
