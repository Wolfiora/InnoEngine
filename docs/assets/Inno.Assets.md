# Inno.Assets

[分类索引](README.md) · [Wiki 首页](../README.md)

## 职责与边界

AssetArtifactInfo 只保存 key、output、hash 和 length，不公开物理位置。ArtifactLease.OpenRead() 是唯一 artifact 读取入口；每条流持有自己的 pin，外层 lease 提前释放不破坏读取。AssetDatabase 借用 content store，不运行 importer、不创建目录。加载、依赖 retention、预加载预算和退休仍由 Asset residency owner 协调。

## 读取与所有权

```csharp
using System.IO;
using Inno.Assets;

static int ReadFirstByte(ArtifactLease lease)
{
    using Stream input = lease.OpenRead();
    return input.ReadByte();
}
```

ArtifactLease 固定内容身份，每个打开的流取得独立 pin；释放外层 lease 后现有流仍可完成读取。关闭 provider 后禁止取得新 lease；已借用的 store 由外部 owner 在 AssetDatabase 退休后释放。内容缺失与内容损坏分别报告，不伪造空数据。AssetDatabase 不执行 importer 或管理创作目录；创作服务见 [Assets Pipeline](Inno.Assets.Pipeline.md)。

Residency 管理加载、依赖 retention、预算、预加载取消和退休；RetirementPendingException 保留仍在释放的值及 callback，普通终止失败不得重复执行释放。Missing Asset 保留 persistent ID、Stable Type ID、原数据与依赖，恢复进入统一候选事务。所有包含引用的数据使用 owner 的完整 SerializationContext。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Assets.ArtifactLease`

<!-- api: T:Inno.Assets.ArtifactLease 93ea9774b31515f19c776bd6e2196d652e0dc6489e49a89658e6d6fd9c46dfd9 -->
<!-- api: P:Inno.Assets.ArtifactLease.info a8d281289ad7b1222b08947da2007c402125f42abec2b150d63b3c1808c9a43b -->
<!-- api: M:Inno.Assets.ArtifactLease.OpenRead 4e10d736456c338cc396ce8aaa443d491a02dd99c67b5aa1b9ec2b64de675c7d -->
<!-- api: M:Inno.Assets.ArtifactLease.ReadAllBytes 3a1d1dd9ab7cae248111a81cf989b3bde6d8665ffb9a95e3ef310a6671434bee -->
<!-- api: M:Inno.Assets.ArtifactLease.Dispose cb928cbe930e04e51a74f7c19ff192c81a2982c708c8e8185758788d1ea424a6 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-3d1c044af81d7da6"></a>[`public void Inno.Assets.ArtifactLease.Dispose()`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L242) | Releases artifact ownership, retaining metadata and callback while the provider reports Pending. | Pending may be wrapped in another exception; classify the complete failure with RetirementPendingException.Find. Ordinary sibling failures remain observable after a later release attempt succeeds. New reads stop when release begins; already opened streams retain their independent provider pins. | `Inno.Core.Execution.RetirementPendingException`：Provider retirement is unfinished or a concurrent release is in progress; retain this lease and retry at a safe point.<br>`System.AggregateException`：The provider reports a wrapped failure, or release completes with ordinary errors retained from earlier pending attempts. |
| <a id="api-bf0edfa2f856a457"></a>[`public System.IO.Stream Inno.Assets.ArtifactLease.OpenRead()`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L197) | Opens a read-only stream over the retained immutable artifact.<br>返回：A new independently owned read stream. | — | — |
| <a id="api-514191706160904e"></a>[`public byte[] Inno.Assets.ArtifactLease.ReadAllBytes()`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L215) | Reads one indexed payload through this lease's independently pinned stream.<br>返回：Newly owned bytes with the exact indexed length; oversized or truncated payloads fail explicitly. | — | `System.IO.InvalidDataException`：The payload exceeds the supported array size or does not have its indexed length. |
| <a id="api-95f13d6893bf7c69"></a>[`public Inno.Assets.AssetArtifactInfo Inno.Assets.ArtifactLease.info { get; }`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L189) | Gets verified metadata for the retained artifact. | — | — |
| <a id="api-c2652dab8f13f789"></a>[`public sealed class Inno.Assets.ArtifactLease : System.IDisposable`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L171) | Keeps one verified immutable artifact generation available for an explicit lifetime. | — | — |

### `Inno.Assets.AssetChange`

<!-- api: T:Inno.Assets.AssetChange 8dc0dd6da43dbab1b29e11734ea0b7809144b7f50a3490d73f5b5272291b39c7 -->
<!-- api: M:Inno.Assets.AssetChange.#ctor(Inno.Assets.AssetChangeKind,System.Guid,Inno.Assets.AssetPath,System.Nullable{Inno.Assets.AssetPath}) 05a4ee552ac89c1d1203eb4d4950fe6b5ed7eff3a0b500493e101204a1a3b30b -->
<!-- api: P:Inno.Assets.AssetChange.kind feff2b8e95d786ba58ea8f9c69ef07a8838340f33bf5e43dbbd937ebedff6a83 -->
<!-- api: P:Inno.Assets.AssetChange.persistentId 0a9f42ec390c5bc11f33b9be33687570bdb9af8116fecbe1552271e3939ac4b5 -->
<!-- api: P:Inno.Assets.AssetChange.assetPath ca4e583699c9ca603cad9fd55faa774d1c5296e641e20fa268e2e199e30c3bc0 -->
<!-- api: P:Inno.Assets.AssetChange.previousAssetPath 59b6d837b744abf9b693d9ede6505f28b8652f58ac3becd7292d9c438640ac3f -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-4f769ec70baad8d3"></a>[`public Inno.Assets.AssetChange.AssetChange(Inno.Assets.AssetChangeKind kind, System.Guid persistentId, Inno.Assets.AssetPath assetPath, Inno.Assets.AssetPath? previousAssetPath = null)`](../../src/content/assets/Inno.Assets/Changes/AssetChange.cs#L25) | Creates an asset change descriptor.<br>`kind`：Committed change kind.<br>`persistentId`：Persistent identity affected by the change, or empty for a catalog-wide change.<br>`assetPath`：Current isolated path, or an invalid path for a catalog-wide change.<br>`previousAssetPath`：Previous isolated path for a move operation. | — | — |
| <a id="api-c67bae6df8d91846"></a>[`public Inno.Assets.AssetPath Inno.Assets.AssetChange.assetPath { get; }`](../../src/content/assets/Inno.Assets/Changes/AssetChange.cs#L50) | Gets the current isolated source path. | — | — |
| <a id="api-8a6debe9ad1afdac"></a>[`public Inno.Assets.AssetChangeKind Inno.Assets.AssetChange.kind { get; }`](../../src/content/assets/Inno.Assets/Changes/AssetChange.cs#L40) | Gets the change kind. | — | — |
| <a id="api-67348e2495037efa"></a>[`public System.Guid Inno.Assets.AssetChange.persistentId { get; }`](../../src/content/assets/Inno.Assets/Changes/AssetChange.cs#L45) | Gets the persistent identity affected by the change. | — | — |
| <a id="api-c1508e53c0f67301"></a>[`public Inno.Assets.AssetPath? Inno.Assets.AssetChange.previousAssetPath { get; }`](../../src/content/assets/Inno.Assets/Changes/AssetChange.cs#L55) | Gets the previous isolated path for move operations. | — | — |
| <a id="api-93f8efe110f55952"></a>[`public readonly struct Inno.Assets.AssetChange`](../../src/content/assets/Inno.Assets/Changes/AssetChange.cs#L8) | Describes one committed asset database change. | — | — |

### `Inno.Assets.AssetChangeKind`

<!-- api: T:Inno.Assets.AssetChangeKind 68d44db85e9c8b14e1f389c7c7003ad92858138c5454422393bc4cd1a60e3a36 -->
<!-- api: F:Inno.Assets.AssetChangeKind.Added b10d5a98e100fc719184fb1d10f3988089a5eed6523a6a371e0d0f6bbc24e85d -->
<!-- api: F:Inno.Assets.AssetChangeKind.Modified 15c7ee41afc11bf48d8fae32d63c0b751868076e7649a56448401373845aca93 -->
<!-- api: F:Inno.Assets.AssetChangeKind.Moved 2f7d7c1bbd645d9e7eea82674bb05cb47508d0b5a69afbea4704df114ac1fb07 -->
<!-- api: F:Inno.Assets.AssetChangeKind.Removed 24d435711c5ba253b7fa16fe192ad39b1f0892b4290858a6d1f677caf8b1de0c -->
<!-- api: F:Inno.Assets.AssetChangeKind.Missing 95f9cf6bb31c650fafeb99f204bb064e016e1b3d521d1713248ea76c32f054b1 -->
<!-- api: F:Inno.Assets.AssetChangeKind.Replaced ab2155c420bc14ced8ae651c48ea7f6ef0190c0424f86ab54a1b7a17eb818500 -->
<!-- api: F:Inno.Assets.AssetChangeKind.StatusChanged fe211c82b1dd068a6efc5d5c42cffd51e9c18666dab90a3797a8d8c8271d4492 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-637875542e22ce3d"></a>[`Inno.Assets.AssetChangeKind.Added = 0`](../../src/content/assets/Inno.Assets/Changes/AssetChangeKind.cs#L11) | A source was added to the database. | — | — |
| <a id="api-1af23b90045b2c0f"></a>[`Inno.Assets.AssetChangeKind.Missing = 4`](../../src/content/assets/Inno.Assets/Changes/AssetChangeKind.cs#L31) | A source became unavailable while retaining its identity. | — | — |
| <a id="api-f5bf6c98964b25ce"></a>[`Inno.Assets.AssetChangeKind.Modified = 1`](../../src/content/assets/Inno.Assets/Changes/AssetChangeKind.cs#L16) | An existing source or artifact changed. | — | — |
| <a id="api-8b1e7d240c3a11f1"></a>[`Inno.Assets.AssetChangeKind.Moved = 2`](../../src/content/assets/Inno.Assets/Changes/AssetChangeKind.cs#L21) | A source moved while retaining its persistent identity. | — | — |
| <a id="api-ecbd46dc8db8a373"></a>[`Inno.Assets.AssetChangeKind.Removed = 3`](../../src/content/assets/Inno.Assets/Changes/AssetChangeKind.cs#L26) | A source and its persistent metadata were removed. | — | — |
| <a id="api-05e0c84481ce7ce5"></a>[`Inno.Assets.AssetChangeKind.Replaced = 5`](../../src/content/assets/Inno.Assets/Changes/AssetChangeKind.cs#L36) | A canonical runtime object was replaced by an incompatible imported type. | — | — |
| <a id="api-e5a27106547c1c48"></a>[`Inno.Assets.AssetChangeKind.StatusChanged = 6`](../../src/content/assets/Inno.Assets/Changes/AssetChangeKind.cs#L41) | An import status or diagnostic changed without replacing the source. | — | — |
| <a id="api-38372437d89ce571"></a>[`public enum Inno.Assets.AssetChangeKind : int`](../../src/content/assets/Inno.Assets/Changes/AssetChangeKind.cs#L6) | Identifies a committed asset database change. | — | — |

### `Inno.Assets.AssetChangeSet`

<!-- api: T:Inno.Assets.AssetChangeSet 1dd62c39b4d2feeddfea2bb8bc0307aa3471283a3618015a2f104993cb75bde0 -->
<!-- api: M:Inno.Assets.AssetChangeSet.#ctor(System.Int64,System.Collections.Generic.IReadOnlyList{Inno.Assets.AssetChange}) 8e105187fa2df1a0bca569bd09e452288038b9b0c60f428e67fd62ea3b9b6300 -->
<!-- api: P:Inno.Assets.AssetChangeSet.revision a8293c0f4e6344d3c74a5f20a701437d64b5a634e16ddc0fac59ee0e6d97e06e -->
<!-- api: P:Inno.Assets.AssetChangeSet.changes 73ff694e8418f81a1495280b0825398e4ad19396d26fae2416dd7fa8b9e47d1b -->
<!-- api: P:Inno.Assets.AssetChangeSet.isEmpty 844acf43dd64272d5170cbecda479704f3d5e6079403dc27ebdbba0490ded237 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-1d9d1b331b2ab1e6"></a>[`public Inno.Assets.AssetChangeSet.AssetChangeSet(long revision, System.Collections.Generic.IReadOnlyList<Inno.Assets.AssetChange>? changes)`](../../src/content/assets/Inno.Assets/Changes/AssetChangeSet.cs#L20) | Creates a committed change set.<br>`revision`：The monotonic database revision assigned to the atomic commit.<br>`changes`：The ordered changes published by the commit, or null for an empty commit. | — | — |
| <a id="api-014756a45d1f5ff2"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Assets.AssetChange> Inno.Assets.AssetChangeSet.changes { get; }`](../../src/content/assets/Inno.Assets/Changes/AssetChangeSet.cs#L36) | Gets the changes committed by this revision. | — | — |
| <a id="api-02ea027a34f8b0de"></a>[`public bool Inno.Assets.AssetChangeSet.isEmpty { get; }`](../../src/content/assets/Inno.Assets/Changes/AssetChangeSet.cs#L41) | Gets whether the change set contains no changes. | — | — |
| <a id="api-32c39ed71f79486b"></a>[`public long Inno.Assets.AssetChangeSet.revision { get; }`](../../src/content/assets/Inno.Assets/Changes/AssetChangeSet.cs#L31) | Gets the monotonically increasing database revision. | — | — |
| <a id="api-6c37f57fcc2a2c98"></a>[`public sealed class Inno.Assets.AssetChangeSet`](../../src/content/assets/Inno.Assets/Changes/AssetChangeSet.cs#L9) | Contains one atomically committed asset database revision. | — | — |

### `Inno.Assets.AssetDatabase`

<!-- api: T:Inno.Assets.AssetDatabase b7d64667c1798967f7320a26256a0d4b80bd5c11ad8c55f9506fb9131800e29b -->
<!-- api: M:Inno.Assets.AssetDatabase.RestoreProperties``1(System.Guid,System.Byte[],``0) 2168d5a90d6986bae1123296fad5528bbae8cedcdb80968f7d64bf2b9d88c0eb -->
<!-- api: M:Inno.Assets.AssetDatabase.#ctor(Inno.Content.IRuntimeContentStore,Inno.Core.Serialization.SerializationGeneration,Inno.Extensibility.Types.TypeCacheSnapshot,Inno.Core.Identity.IdentityAllocator,System.Int64,System.Int64) 73dbecaa53b3e05401f9141685628464e44d4f026f187bfe4694a0ed09c6478c -->
<!-- api: M:Inno.Assets.AssetDatabase.Load``1(Inno.Assets.AssetPath) 226fc320a8afa109d020995eaa7910213ee4da184ee088a0158f533c17b4a5fe -->
<!-- api: M:Inno.Assets.AssetDatabase.Load``1(System.Guid) 949754cb9280e9ff976eec088a3c1ff554f36a77db8c0c37e7ff5288255d5724 -->
<!-- api: M:Inno.Assets.AssetDatabase.TryLoad``1(Inno.Assets.AssetPath,``0@) 21eea1515fb730415f6ca5c8ae0d288ef966cd642576e83e9c14900bc7f93a9e -->
<!-- api: M:Inno.Assets.AssetDatabase.TryLoad``1(System.Guid,``0@) 10ffc7765f0bb706c2a671ebd20505a25691acf2e2b5ed7bba8baf4c3ab8da20 -->
<!-- api: P:Inno.Assets.AssetDatabase.residencyStatistics f4fbeed29191753e74a28500f02e94ddecb30af2fdd895175644282de05d4994 -->
<!-- api: M:Inno.Assets.AssetDatabase.AcquireAsync``1(Inno.Assets.AssetPath,System.Threading.CancellationToken) 4d844535cbbea9c5459ec6cdaf148a00e96d50ed0147ac68280213b6ccc467ba -->
<!-- api: M:Inno.Assets.AssetDatabase.AcquireAsync``1(System.Guid,System.Threading.CancellationToken) ddf09b54cdfe351d14e32f287d1bc995f164f9f049d6cac2bdc7125105980c98 -->
<!-- api: M:Inno.Assets.AssetDatabase.GetDependencies(Inno.Assets.AssetObject) c00d91e5843c38b642d367aa8ed03ac6af7115396c01ed4b91a6bf228015c1e6 -->
<!-- api: M:Inno.Assets.AssetDatabase.TryGetArtifact(System.Guid,System.String,Inno.Assets.AssetArtifactInfo@) b76846c11e65ada2600e727c0e12a36570e989a3592548efb201c86cd4b5d3f5 -->
<!-- api: M:Inno.Assets.AssetDatabase.AcquireArtifact(System.Guid,System.String) 117fa0b00aa986aca68634f921ef7a750b2bbe0e29e0765565ffcd42a4e3ca2e -->
<!-- api: M:Inno.Assets.AssetDatabase.TrimToBudget 4b2aada3307fd6b0954d726f0c2b46a10be1bd452ae27dc4c258e4c0e68ceef3 -->
<!-- api: M:Inno.Assets.AssetDatabase.Dispose 616bd3353ac775fd4f929c850f8ddbdc63e9c91cf297f312daeb0050eafa81ce -->
<!-- api: P:Inno.Assets.AssetDatabase.preparationStatistics a4b11b3842584e433965ab4f9e98663d3194ad2b2b10acc8262b090b7cab5195 -->
<!-- api: P:Inno.Assets.AssetDatabase.preparingBytes 35f9740cd524ebae6bf4ddb4f124140ba348af1233e64ab0d5cabcbfac7b9554 -->
<!-- api: M:Inno.Assets.AssetDatabase.CompletePendingLoads(System.Int32) 11fce5fd3cc4daf4c0797d8f14b8776db5bddccae1a8fcd756b487f10045bf99 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-58a5b2ece2cde17e"></a>[`public Inno.Assets.AssetDatabase.AssetDatabase(Inno.Content.IRuntimeContentStore content, Inno.Core.Serialization.SerializationGeneration serialization, Inno.Extensibility.Types.TypeCacheSnapshot types, Inno.Core.Identity.IdentityAllocator identities, long residencyBudgetBytes = 9223372036854775807, long preparationBudgetBytes = 67108864)`](../../src/content/assets/Inno.Assets/Runtime/AssetDatabase.cs#L108) | Creates a read-only runtime asset database from one verified immutable content store.<br>`content`：The borrowed content store; its owner closes it after this database and all outstanding reads retire.<br>`serialization`：The immutable converter generation pinned by the owning runtime session.<br>`identities`：The session-owned allocator used for canonical runtime asset identities.<br>`types`：The immutable type generation pinned by this runtime session, without refreshing authoring registries.<br>`residencyBudgetBytes`：Maximum unpinned runtime payload bytes retained after lease release.<br>`preparationBudgetBytes`：Maximum encoded payload bytes reserved by unfinished cold-load closures, independently of residency. | — | `System.ArgumentNullException`：Thrown when serialization is null.<br>`System.IO.InvalidDataException`：Thrown when the deployed catalog or its runtime artifact closure is incomplete or malformed. |
| <a id="api-949f9bb696f72ba5"></a>[`public Inno.Assets.ArtifactLease Inno.Assets.AssetDatabase.AcquireArtifact(System.Guid persistentId, string outputName)`](../../src/content/assets/Inno.Assets/Runtime/AssetDatabase.cs#L466) | Acquires one verified immutable artifact output for an explicit lifetime.<br>返回：A lease over the verified deployed artifact.<br>`persistentId`：Persistent identity of the artifact owner.<br>`outputName`：Stable artifact output name. | — | — |
| <a id="api-5507aba26fff2fc1"></a>[`public System.Threading.Tasks.ValueTask<Inno.Assets.AssetLease<TAsset>> Inno.Assets.AssetDatabase.AcquireAsync<TAsset>(Inno.Assets.AssetPath path, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) where TAsset : Inno.Assets.AssetObject`](../../src/content/assets/Inno.Assets/Runtime/AssetDatabase.cs#L329) | Acquires a canonical runtime asset by path for an explicit residency lifetime.<br>返回：A lease completed after verified preparation and owner-thread publication.<br>`path`：Mount-qualified logical asset path.<br>`cancellationToken`：Cancellation observed during preparation and before owner-thread materialization. | — | — |
| <a id="api-6853ba47f013d64a"></a>[`public System.Threading.Tasks.ValueTask<Inno.Assets.AssetLease<TAsset>> Inno.Assets.AssetDatabase.AcquireAsync<TAsset>(System.Guid persistentId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) where TAsset : Inno.Assets.AssetObject`](../../src/content/assets/Inno.Assets/Runtime/AssetDatabase.cs#L360) | Acquires a canonical runtime asset by persistent identity for an explicit residency lifetime.<br>返回：A lease completed after verified preparation and owner-thread publication.<br>`persistentId`：Persistent asset identity.<br>`cancellationToken`：Cancellation observed during preparation and before owner-thread materialization. | — | — |
| <a id="api-8dc06b91879f4f41"></a>[`public int Inno.Assets.AssetDatabase.CompletePendingLoads(int completionBudget = 8)`](../../src/content/assets/Inno.Assets/Runtime/AssetDatabase.Residency.cs#L70) | Publishes completed cold loads on the database owner thread without waiting for outstanding IO.<br>返回：The number of requests completed, failed, or canceled during this call.<br>`completionBudget`：The maximum number of ready requests committed at this frame safety point. | — | `System.InvalidOperationException`：The caller is not the owner thread that created this database.<br>`System.ObjectDisposedException`：The database has already retired. |
| <a id="api-07704b30e0c50860"></a>[`public void Inno.Assets.AssetDatabase.Dispose()`](../../src/content/assets/Inno.Assets/Runtime/AssetDatabase.cs#L508) | Stops publication and releases canonical assets before dependency edges and the payload read gate. | — | `Inno.Core.Execution.RetirementPendingException`：An unload hook still owns active work. Retain this database and retry at the session retirement safe point.<br>`System.AggregateException`：Completed releases failed after all quiescent resources were attempted. |
| <a id="api-3342e428f6dfa0cf"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Assets.AssetDependency> Inno.Assets.AssetDatabase.GetDependencies(Inno.Assets.AssetObject asset)`](../../src/content/assets/Inno.Assets/Runtime/AssetDatabase.cs#L394) | Gets the direct persistent dependencies declared by one loaded or cataloged asset.<br>返回：A deterministic immutable snapshot of direct dependencies.<br>`asset`：The canonical asset whose deployed dependency descriptors are requested. | — | `System.InvalidOperationException`：Thrown when asset is not owned by this database.<br>`System.ObjectDisposedException`：Thrown when this database has been disposed. |
| <a id="api-f808248d0bfa59c0"></a>[`public TAsset Inno.Assets.AssetDatabase.Load<TAsset>(Inno.Assets.AssetPath path) where TAsset : Inno.Assets.AssetObject`](../../src/content/assets/Inno.Assets/Runtime/AssetDatabase.cs#L175) | Loads the canonical asset at one catalog path.<br>返回：The canonical asset instance owned by this database.<br>`path`：The mount-qualified logical catalog path. | — | `System.InvalidOperationException`：Thrown when no asset exists at the path or its concrete type is incompatible with TAsset.<br>`System.ObjectDisposedException`：Thrown when this database has been disposed. |
| <a id="api-f6cbc5cbbda731bc"></a>[`public TAsset Inno.Assets.AssetDatabase.Load<TAsset>(System.Guid persistentId) where TAsset : Inno.Assets.AssetObject`](../../src/content/assets/Inno.Assets/Runtime/AssetDatabase.cs#L206) | Loads the canonical asset with one persistent identity.<br>返回：The canonical asset instance owned by this database.<br>`persistentId`：The non-empty persistent asset identity. | — | `System.InvalidOperationException`：Thrown when no asset has the identity or its concrete type is incompatible with TAsset.<br>`System.ObjectDisposedException`：Thrown when this database has been disposed. |
| <a id="api-76ec6743082ff4d0"></a>[`public void Inno.Assets.AssetDatabase.RestoreProperties<TValue>(System.Guid stableTypeId, byte[] propertyData, TValue target) where TValue : class, Inno.Core.Serialization.ISerializable`](../../src/content/assets/Inno.Assets/Runtime/AssetDatabase.cs#L64) | Restores serialized properties to the existing asset object.<br>`stableTypeId`：The stable type id consumed by restore properties; ownership remains with the caller unless explicitly stated otherwise.<br>`propertyData`：The property data consumed by restore properties; ownership remains with the caller unless explicitly stated otherwise.<br>`target`：The existing target that receives the validated result. | — | — |
| <a id="api-2d9a11b2bdd2d274"></a>[`public int Inno.Assets.AssetDatabase.TrimToBudget()`](../../src/content/assets/Inno.Assets/Runtime/AssetDatabase.cs#L490) | Evicts least-recently-used unpinned assets until the configured payload budget is met.<br>返回：The number of canonical assets released by this trim operation. | — | — |
| <a id="api-3823f9d6ffc13129"></a>[`public bool Inno.Assets.AssetDatabase.TryGetArtifact(System.Guid persistentId, string outputName, out Inno.Assets.AssetArtifactInfo? artifact)`](../../src/content/assets/Inno.Assets/Runtime/AssetDatabase.cs#L428) | Tries to resolve one named immutable artifact output by persistent asset identity.<br>返回：true when the deployed bundle contains the requested output; otherwise, false.<br>`persistentId`：The persistent identity of the artifact owner.<br>`outputName`：The exact artifact output name.<br>`artifact`：Receives verified output metadata without prescribing a physical deployment location. | — | `System.ObjectDisposedException`：Thrown when this database has been disposed. |
| <a id="api-32ac7fb8e49345e7"></a>[`public bool Inno.Assets.AssetDatabase.TryLoad<TAsset>(Inno.Assets.AssetPath path, out TAsset? asset) where TAsset : Inno.Assets.AssetObject`](../../src/content/assets/Inno.Assets/Runtime/AssetDatabase.cs#L237) | Tries to load the canonical asset at one catalog path.<br>返回：true when the deployed catalog contains a compatible asset; otherwise, false.<br>`path`：The mount-qualified logical catalog path.<br>`asset`：Receives the canonical compatible asset when available. | — | `System.ObjectDisposedException`：Thrown when this database has been disposed. |
| <a id="api-46e2fbffad0d2859"></a>[`public bool Inno.Assets.AssetDatabase.TryLoad<TAsset>(System.Guid persistentId, out TAsset? asset) where TAsset : Inno.Assets.AssetObject`](../../src/content/assets/Inno.Assets/Runtime/AssetDatabase.cs#L276) | Tries to load the canonical asset with one persistent identity.<br>返回：true when the deployed catalog contains a compatible asset; otherwise, false.<br>`persistentId`：The non-empty persistent asset identity.<br>`asset`：Receives the canonical compatible asset when available. | — | `System.ObjectDisposedException`：Thrown when this database has been disposed. |
| <a id="api-7906596192bfdb22"></a>[`public Inno.Assets.AssetPreparationStatistics Inno.Assets.AssetDatabase.preparationStatistics { get; }`](../../src/content/assets/Inno.Assets/Runtime/AssetDatabase.Residency.cs#L33) | Gets immutable cold-load admission, unique IO and peak reservation counters for this database lifetime. | — | — |
| <a id="api-d1101b010ac34d7d"></a>[`public long Inno.Assets.AssetDatabase.preparingBytes { get; }`](../../src/content/assets/Inno.Assets/Runtime/AssetDatabase.Residency.cs#L46) | Gets encoded bytes reserved by cold-load closures, including completed IO awaiting owner publication. | — | — |
| <a id="api-0757e440a0d33554"></a>[`public Inno.Assets.AssetResidencyStatistics Inno.Assets.AssetDatabase.residencyStatistics { get; }`](../../src/content/assets/Inno.Assets/Runtime/AssetDatabase.cs#L299) | Gets current materialized asset count, payload bytes, and the configured budget. | — | — |
| <a id="api-6d1e482179c7cd34"></a>[`public sealed class Inno.Assets.AssetDatabase : Inno.Assets.AssetResidencyProvider, System.IDisposable, Inno.Assets.IAssetLookup, Inno.Assets.IAssetReferenceResolver, Inno.Assets.IAssetPropertyStateResolver, Inno.Assets.IAssetArtifactLookup, Inno.Assets.IAssetResidency`](../../src/content/assets/Inno.Assets/Runtime/AssetDatabase.cs#L24) | Loads canonical runtime assets exclusively from a verified catalog and content-addressed artifact bundles. | This database never reads project source files, creates source mounts, runs importers, or writes content. One runtime session owns one database instance and disposes it before releasing its identity services. | — |

### `Inno.Assets.AssetDependency`

<!-- api: T:Inno.Assets.AssetDependency 3cef3c15af060bee7a7cd5af8d6cc0d4f50a8dc12361e2b47b4606c620d57fc9 -->
<!-- api: M:Inno.Assets.AssetDependency.#ctor(System.Guid,Inno.Extensibility.Types.TypeRef,System.String) bbd8fd0d6d4bae67e03b860b663a40e29bf35c119a4b1bcf391c3bb8ddee9512 -->
<!-- api: P:Inno.Assets.AssetDependency.persistentId 8dbb5ea684434ee52457f20b4601ad20d58062f6c02ea48cc1b9f64f64301183 -->
<!-- api: P:Inno.Assets.AssetDependency.type ef9daf424ac75ad69a3879de72574b3118e30e35c82da27f67c0802237d1f0e2 -->
<!-- api: P:Inno.Assets.AssetDependency.lastKnownPath 3378541a7c082e017b5ad1f7962a83ad777cfba901c3082ab2c9aaa30618cc7f -->
<!-- api: M:Inno.Assets.AssetDependency.Equals(Inno.Assets.AssetDependency) 25444972d699e1b857cc93a4a003d4a24b0b917a4edd7381e787877870f527ab -->
<!-- api: M:Inno.Assets.AssetDependency.Equals(System.Object) 86f150ce7f74e1312f6fe89b05afb8e51f1339a3885001da36e0cfeeb263cf65 -->
<!-- api: M:Inno.Assets.AssetDependency.GetHashCode 290b977ec4ed2b8ba0efe49f8a026c897ed7acc3d3df83e3e7bb93248bf9feeb -->
<!-- api: M:Inno.Assets.AssetDependency.op_Equality(Inno.Assets.AssetDependency,Inno.Assets.AssetDependency) 0674db8cee09c2725646a397731e780b55baf73df5ad378f35097aa47eb56410 -->
<!-- api: M:Inno.Assets.AssetDependency.op_Inequality(Inno.Assets.AssetDependency,Inno.Assets.AssetDependency) 1e2a31567b66ea8a1bd76632e7265173a532211ab22cf3d185526a7b58ab93ef -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-841449d8a9bad8de"></a>[`public Inno.Assets.AssetDependency.AssetDependency(System.Guid persistentId, Inno.Extensibility.Types.TypeRef type, string lastKnownPath)`](../../src/content/assets/Inno.Assets/AssetDependency.cs#L29) | Creates an asset dependency descriptor.<br>`persistentId`：The persistent identity of the referenced asset.<br>`type`：The reload-safe identity of the expected asset type.<br>`lastKnownPath`：The last known source-relative path. | — | `System.ArgumentException`：Thrown when persistentId is empty. |
| <a id="api-b658d2b0c9983238"></a>[`public bool Inno.Assets.AssetDependency.Equals(Inno.Assets.AssetDependency other)`](../../src/content/assets/Inno.Assets/AssetDependency.cs#L66) | Determines whether this instance and the supplied value represent the same logical state.<br>返回：true when both values represent the same logical state; otherwise, false.<br>`other`：The value to compare with this instance. | — | — |
| <a id="api-40cdef2d883d6f0b"></a>[`public override bool Inno.Assets.AssetDependency.Equals(object? obj)`](../../src/content/assets/Inno.Assets/AssetDependency.cs#L77) | Determines whether this instance and the supplied value represent the same logical state.<br>返回：true when both values represent the same logical state; otherwise, false.<br>`obj`：The object to compare with this instance. | — | — |
| <a id="api-d3b702ec3449accb"></a>[`public override int Inno.Assets.AssetDependency.GetHashCode()`](../../src/content/assets/Inno.Assets/AssetDependency.cs#L85) | Computes a hash code from the fields that participate in logical equality.<br>返回：A hash code consistent with the implemented equality contract. | — | — |
| <a id="api-7cd53397435dd163"></a>[`public static bool Inno.Assets.AssetDependency.operator ==(Inno.Assets.AssetDependency left, Inno.Assets.AssetDependency right)`](../../src/content/assets/Inno.Assets/AssetDependency.cs#L99) | Determines whether two descriptors refer to the same persistent asset.<br>返回：true when both descriptors reference the same persistent asset; otherwise, false.<br>`left`：The first dependency descriptor to compare.<br>`right`：The second dependency descriptor to compare. | — | — |
| <a id="api-ec26e7d0e38b7b5f"></a>[`public static bool Inno.Assets.AssetDependency.operator !=(Inno.Assets.AssetDependency left, Inno.Assets.AssetDependency right)`](../../src/content/assets/Inno.Assets/AssetDependency.cs#L116) | Determines whether two descriptors refer to different persistent assets.<br>返回：true when the descriptors reference different persistent assets; otherwise, false.<br>`left`：The first dependency descriptor to compare.<br>`right`：The second dependency descriptor to compare. | — | — |
| <a id="api-daa71cd2edafaf0b"></a>[`public string Inno.Assets.AssetDependency.lastKnownPath { get; }`](../../src/content/assets/Inno.Assets/AssetDependency.cs#L55) | Gets the last known source-relative path. | — | — |
| <a id="api-3399a41e7f08be3c"></a>[`public System.Guid Inno.Assets.AssetDependency.persistentId { get; }`](../../src/content/assets/Inno.Assets/AssetDependency.cs#L44) | Gets the persistent identity of the referenced asset. | — | — |
| <a id="api-902b56681f73eccd"></a>[`public Inno.Extensibility.Types.TypeRef Inno.Assets.AssetDependency.type { get; }`](../../src/content/assets/Inno.Assets/AssetDependency.cs#L50) | Gets the reload-safe identity of the expected asset type. | — | — |
| <a id="api-53b4a6a7aaa45fd7"></a>[`public readonly struct Inno.Assets.AssetDependency : System.IEquatable<Inno.Assets.AssetDependency>`](../../src/content/assets/Inno.Assets/AssetDependency.cs#L11) | Describes a persistent direct dependency on another asset. | — | — |

### `Inno.Assets.AssetDependencyCollection`

<!-- api: T:Inno.Assets.AssetDependencyCollection 882fd3964970866278f0b867d955fdc7f5f2181a1dc34e8b363a4a0dc7ab88b6 -->
<!-- api: M:Inno.Assets.AssetDependencyCollection.#ctor 3c69fd9f3c2dc02e45d337d6683f78587c2a104effc3887b09f70f875a4ae3af -->
<!-- api: M:Inno.Assets.AssetDependencyCollection.#ctor(System.Boolean) 09c67b0f15514f9ed80f23dd881a63ed12301a7021d27c6c826148e230689a0c -->
<!-- api: P:Inno.Assets.AssetDependencyCollection.includeLastKnownPaths 8670b140a1de6e55dd360aea7a65d70ab16283c08be28c91204e92a326475a32 -->
<!-- api: P:Inno.Assets.AssetDependencyCollection.dependencies ea44fdb03f43cf71d1a7f32c1cec57c0d8f6b5e4452a7aaaff364ce3731d58f4 -->
<!-- api: M:Inno.Assets.AssetDependencyCollection.Add(Inno.Assets.AssetDependency) d517ba652c4c596cbcfd7584ebfef73227cce46dd3cf01c078e63c194b333374 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-4440673e877d66fa"></a>[`public Inno.Assets.AssetDependencyCollection.AssetDependencyCollection()`](../../src/content/assets/Inno.Assets/Serialization/AssetDependencyCollection.cs#L24) | Creates a dependency collector that preserves last-known source paths in serialized references. | — | — |
| <a id="api-262e2980bbafbfe1"></a>[`public Inno.Assets.AssetDependencyCollection.AssetDependencyCollection(bool includeLastKnownPaths)`](../../src/content/assets/Inno.Assets/Serialization/AssetDependencyCollection.cs#L36) | Creates a dependency collector with an explicit location-hint policy.<br>`includeLastKnownPaths`：Whether serialized asset references and collected dependency descriptors retain source path hints. Disable this only for semantic content comparison; persistent identity and stable type identity remain encoded. | — | — |
| <a id="api-7ab1d1dd0c766f97"></a>[`public void Inno.Assets.AssetDependencyCollection.Add(Inno.Assets.AssetDependency dependency)`](../../src/content/assets/Inno.Assets/Serialization/AssetDependencyCollection.cs#L79) | Includes a dependency already captured inside a neutral nested property payload.<br>`dependency`：Stable asset/type identity and optional location hint. | — | — |
| <a id="api-54a459dc52a80eed"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Assets.AssetDependency> Inno.Assets.AssetDependencyCollection.dependencies { get; }`](../../src/content/assets/Inno.Assets/Serialization/AssetDependencyCollection.cs#L49) | Gets the collected dependencies in deterministic persistent-identity order. | — | — |
| <a id="api-0ebd0b0274139971"></a>[`public bool Inno.Assets.AssetDependencyCollection.includeLastKnownPaths { get; }`](../../src/content/assets/Inno.Assets/Serialization/AssetDependencyCollection.cs#L44) | Gets whether serialized asset references and collected dependencies retain source path hints. | — | — |
| <a id="api-559aa4b1fff17720"></a>[`public sealed class Inno.Assets.AssetDependencyCollection`](../../src/content/assets/Inno.Assets/Serialization/AssetDependencyCollection.cs#L17) | Collects direct asset dependencies encountered by a serialization operation. | Add this instance to a Inno.Core.Serialization.SerializationContext when serializing an asset source that must publish dependency metadata. | — |

### `Inno.Assets.AssetExecutionContext`

<!-- api: T:Inno.Assets.AssetExecutionContext e59cef334a28172d3d34ee7caae79dd3006dacafd4984774147444790c316f3c -->
<!-- api: P:Inno.Assets.AssetExecutionContext.current 866ec2266cc0eb53c8febc8cf0f28116ef62c4b4919067013277c7c4b5832146 -->
<!-- api: M:Inno.Assets.AssetExecutionContext.EnterScope(Inno.Assets.IAssetLookup) 34f87744cec7f70bc129360042d3f34bea367b41ec16959898b19b2222e0a67e -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-3fc2357a73deebb8"></a>[`public static System.IDisposable Inno.Assets.AssetExecutionContext.EnterScope(Inno.Assets.IAssetLookup assets)`](../../src/content/assets/Inno.Assets/Runtime/AssetExecutionContext.cs#L38) | Binds an asset lookup until the returned strict last-in-first-out scope is disposed.<br>返回：A strict last-in-first-out execution scope owned by the caller.<br>`assets`：The host-owned lookup to expose to script-facing asset operations. | — | `System.ArgumentNullException`：Thrown when assets is null. |
| <a id="api-3333e6d2e9f1a183"></a>[`public static Inno.Assets.IAssetLookup Inno.Assets.AssetExecutionContext.current { get; }`](../../src/content/assets/Inno.Assets/Runtime/AssetExecutionContext.cs#L24) | Gets the asset lookup bound to the current asynchronous execution context. | — | `System.InvalidOperationException`：Thrown when no asset lookup is active for the caller. |
| <a id="api-6f83482cacbfa610"></a>[`public static class Inno.Assets.AssetExecutionContext`](../../src/content/assets/Inno.Assets/Runtime/AssetExecutionContext.cs#L14) | Binds one host-owned asset lookup to the current asynchronous script execution context. | This type is a composition boundary for Editor and Player hosts. Game code should use Inno.Assets.Assets instead of managing execution scopes directly. | — |

### `Inno.Assets.AssetImportStatus`

<!-- api: T:Inno.Assets.AssetImportStatus a2d04175b72f29c1310f0d367f488f9ff7e7bd57b12cb4f94d7bc4c85f5ea73f -->
<!-- api: F:Inno.Assets.AssetImportStatus.Unsupported 75730946d2e677ec53151ccf8ab753f192455486f051bcacbea30ed36454a263 -->
<!-- api: F:Inno.Assets.AssetImportStatus.Pending 718fbbf3bdc219478fe2b06d68dc31b5880525f84a7de902cd4fd9319ca78f69 -->
<!-- api: F:Inno.Assets.AssetImportStatus.Imported c691f022fb6c7655ed54e679231721a1c426f1f6d99958ab689e117b32c6d923 -->
<!-- api: F:Inno.Assets.AssetImportStatus.Failed b4dc7a0e1755def7fa2412b275d8725b75cf863d32d54d0c7dfe80b0dd56b250 -->
<!-- api: F:Inno.Assets.AssetImportStatus.Missing 0a34f0a0c8a53048025e7ea3e186d7e929b867f8a5f61f3b7339d2ab17458f15 -->
<!-- api: F:Inno.Assets.AssetImportStatus.Conflict ca10000ad012cade2c2933bfd13a169f1787dcd4a23d13c4f4d34cece6acc761 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-0ddd565ec73967a8"></a>[`Inno.Assets.AssetImportStatus.Conflict = 5`](../../src/content/assets/Inno.Assets/AssetImportStatus.cs#L36) | The source cannot be reconciled without resolving an identity conflict. | — | — |
| <a id="api-ca0e8dba923ceac9"></a>[`Inno.Assets.AssetImportStatus.Failed = 3`](../../src/content/assets/Inno.Assets/AssetImportStatus.cs#L26) | The latest import failed and diagnostics are available. | — | — |
| <a id="api-88845ea590d68e74"></a>[`Inno.Assets.AssetImportStatus.Imported = 2`](../../src/content/assets/Inno.Assets/AssetImportStatus.cs#L21) | A valid artifact is committed for the source. | — | — |
| <a id="api-dc3186b782a3b340"></a>[`Inno.Assets.AssetImportStatus.Missing = 4`](../../src/content/assets/Inno.Assets/AssetImportStatus.cs#L31) | The source is unavailable while its persistent identity is retained. | — | — |
| <a id="api-247610d4333b1178"></a>[`Inno.Assets.AssetImportStatus.Pending = 1`](../../src/content/assets/Inno.Assets/AssetImportStatus.cs#L16) | The source is waiting for import or commit. | — | — |
| <a id="api-3784bcb4175af0ba"></a>[`Inno.Assets.AssetImportStatus.Unsupported = 0`](../../src/content/assets/Inno.Assets/AssetImportStatus.cs#L11) | No importer currently accepts the source. | — | — |
| <a id="api-ac72d9da4b52c9f1"></a>[`public enum Inno.Assets.AssetImportStatus : int`](../../src/content/assets/Inno.Assets/AssetImportStatus.cs#L6) | Identifies the current source and import state of an asset. | — | — |

### `Inno.Assets.AssetInfo`

<!-- api: T:Inno.Assets.AssetInfo ae9008bebf7349db042d83b2c703aa6779b9dbd012397a023ede6348fa7e67ee -->
<!-- api: M:Inno.Assets.AssetInfo.#ctor(System.Guid,Inno.Assets.AssetPath,Inno.Assets.AssetSourceKind,Inno.Assets.AssetImportStatus,System.String,System.Guid,Inno.Assets.AssetArtifactKey,Inno.Assets.AssetArtifactKey,System.Collections.Generic.IReadOnlyList{System.String}) d03a1a0b69ba499d5f04b6b508f1d0ff456be3fae49c5dca8053d1c6ed2c9243 -->
<!-- api: P:Inno.Assets.AssetInfo.persistentId a943384791e0249abef4aadf285d0be605168c9b77c794a10e4300acfff01b4e -->
<!-- api: P:Inno.Assets.AssetInfo.assetPath 7a466adf3580c29a207b03bfcbff9877a5c62f71f86a6dd3152c66b5d6a3ab98 -->
<!-- api: P:Inno.Assets.AssetInfo.sourceKind aebb1376b66ee31f96bd9f4a1071cf47840405e65fe9aa49fbb128519b096de2 -->
<!-- api: P:Inno.Assets.AssetInfo.status a0995a6dd64311d460150c0ea7b97dcbd7bb10a52e3a917012f364ff6ebea97b -->
<!-- api: P:Inno.Assets.AssetInfo.importerId bdd34004c9d78a80754fdf6796f6f2ee23c98912d78edb0b97324843d3b52c05 -->
<!-- api: P:Inno.Assets.AssetInfo.stableAssetTypeId 4c3f52772aa15ff3d2763a42692c7d878411aaba1b5a5e3ff6a27f6d281b963f -->
<!-- api: P:Inno.Assets.AssetInfo.artifactKey 5afa60113952697c84737eeac6566d8a5a1faf181906b9168fe9188d65c145f1 -->
<!-- api: P:Inno.Assets.AssetInfo.lastSuccessfulArtifactKey 1949fb038c7807e0fc9fb1bb5d6fabcd2b696312c28f8c1bb826fafdff65e407 -->
<!-- api: P:Inno.Assets.AssetInfo.diagnostics 7d35ff987d997272daec798e290549682877160cd6c76f4adeac944b9f3c61a7 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-ac868349a43897c2"></a>[`public Inno.Assets.AssetInfo.AssetInfo(System.Guid persistentId, Inno.Assets.AssetPath assetPath, Inno.Assets.AssetSourceKind sourceKind, Inno.Assets.AssetImportStatus status, string importerId, System.Guid stableAssetTypeId, Inno.Assets.AssetArtifactKey artifactKey, Inno.Assets.AssetArtifactKey lastSuccessfulArtifactKey, System.Collections.Generic.IReadOnlyList<string>? diagnostics = null)`](../../src/content/assets/Inno.Assets/AssetInfo.cs#L44) | Creates an asset information snapshot.<br>`persistentId`：Persistent identity assigned to the source asset.<br>`assetPath`：Current isolated source path.<br>`sourceKind`：Whether the catalog entry represents a file or directory.<br>`status`：Current import status.<br>`importerId`：Stable importer identifier, or an empty value when none applies.<br>`stableAssetTypeId`：Stable imported asset type identity.<br>`artifactKey`：Current committed artifact key.<br>`lastSuccessfulArtifactKey`：Most recent successfully imported artifact key.<br>`diagnostics`：Latest import diagnostics, or null for none. | — | `System.ArgumentException`：Thrown when assetPath is invalid. |
| <a id="api-251b9bc778f56a64"></a>[`public Inno.Assets.AssetArtifactKey Inno.Assets.AssetInfo.artifactKey { get; }`](../../src/content/assets/Inno.Assets/AssetInfo.cs#L101) | Gets the current committed artifact key. | — | — |
| <a id="api-6b9b9132bed937da"></a>[`public Inno.Assets.AssetPath Inno.Assets.AssetInfo.assetPath { get; }`](../../src/content/assets/Inno.Assets/AssetInfo.cs#L76) | Gets the current isolated source path. | — | — |
| <a id="api-4f0b02a63e759f09"></a>[`public System.Collections.Generic.IReadOnlyList<string> Inno.Assets.AssetInfo.diagnostics { get; }`](../../src/content/assets/Inno.Assets/AssetInfo.cs#L111) | Gets diagnostics produced by the latest reconciliation or import. | — | — |
| <a id="api-084d9fbcb0d961e4"></a>[`public string Inno.Assets.AssetInfo.importerId { get; }`](../../src/content/assets/Inno.Assets/AssetInfo.cs#L91) | Gets the selected importer identifier. | — | — |
| <a id="api-0b2115d2cd36fa32"></a>[`public Inno.Assets.AssetArtifactKey Inno.Assets.AssetInfo.lastSuccessfulArtifactKey { get; }`](../../src/content/assets/Inno.Assets/AssetInfo.cs#L106) | Gets the most recent successfully imported artifact key. | — | — |
| <a id="api-cb212f2c86bf767e"></a>[`public System.Guid Inno.Assets.AssetInfo.persistentId { get; }`](../../src/content/assets/Inno.Assets/AssetInfo.cs#L71) | Gets the persistent asset identity. | — | — |
| <a id="api-57c04d91ea2e179c"></a>[`public Inno.Assets.AssetSourceKind Inno.Assets.AssetInfo.sourceKind { get; }`](../../src/content/assets/Inno.Assets/AssetInfo.cs#L81) | Gets whether the catalog entry represents a file or directory source. | — | — |
| <a id="api-c6e0898aa832fdfc"></a>[`public System.Guid Inno.Assets.AssetInfo.stableAssetTypeId { get; }`](../../src/content/assets/Inno.Assets/AssetInfo.cs#L96) | Gets the stable imported asset type identity. | — | — |
| <a id="api-4d06d61b6cc1cf86"></a>[`public Inno.Assets.AssetImportStatus Inno.Assets.AssetInfo.status { get; }`](../../src/content/assets/Inno.Assets/AssetInfo.cs#L86) | Gets the current import status. | — | — |
| <a id="api-a29f49e0b140d272"></a>[`public sealed class Inno.Assets.AssetInfo`](../../src/content/assets/Inno.Assets/AssetInfo.cs#L9) | Provides an immutable public snapshot of one cataloged asset. | — | — |

### `Inno.Assets.AssetLease`1`

<!-- api: T:Inno.Assets.AssetLease`1 02b6e81b4fc6175e74dfba3ea28bcf622796bc9fc3d08de552c5e4e45074d87b -->
<!-- api: P:Inno.Assets.AssetLease`1.asset b20b767611ab1aba923ae846e80330056c38c06388e15623e9e948316bdca5ed -->
<!-- api: M:Inno.Assets.AssetLease`1.Dispose 92ce9a670c6d6595fa9bf1e38d1d8cffaea12970c53a6b67bcda10fcbce8f94a -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-f4a4c84c001f92ed"></a>[`public void Inno.Assets.AssetLease<TAsset>.Dispose()`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L165) | Releases this caller's residency ownership, retaining its value and callback while the provider reports Pending. | Pending may be wrapped in another exception; classify the complete failure with RetirementPendingException.Find. Ordinary sibling failures remain observable after a later release attempt succeeds. | `Inno.Core.Execution.RetirementPendingException`：Provider retirement is unfinished or a concurrent release is in progress; retain this lease and retry at a safe point.<br>`System.AggregateException`：The provider reports a wrapped failure, or release completes with ordinary errors retained from earlier pending attempts. |
| <a id="api-65535079b2e42067"></a>[`public TAsset Inno.Assets.AssetLease<TAsset>.asset { get; }`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L150) | Gets the retained canonical asset while this lease is active. | — | `System.ObjectDisposedException`：Thrown after the lease has been released. |
| <a id="api-3702bf7480a2982a"></a>[`public sealed class Inno.Assets.AssetLease<TAsset> where TAsset : Inno.Assets.AssetObject : System.IDisposable`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L132) | Keeps one canonical asset generation resident for an explicit lifetime. | — | — |

### `Inno.Assets.AssetObject`

<!-- api: T:Inno.Assets.AssetObject ccccf5d5d8c53f1917e238ca6a0ef10af6f6671cb8c5ab9a2b800ec60d44fd3f -->
<!-- api: P:Inno.Assets.AssetObject.assetPath d50a68d17ce8a7686c60924df2b563600bfc439b89dd3fec384e89c9dbd4876b -->
<!-- api: P:Inno.Assets.AssetObject.name 29fc30f6f71388227af1635a7ae828255a022de96e96088e8c2d136164446d7e -->
<!-- api: P:Inno.Assets.AssetObject.isMissing 2ead3e19e0f962bafddcc45172dab25400062b4d321e143391fb38ec4c890547 -->
<!-- api: P:Inno.Assets.AssetObject.contentVersion 9bd603e2f248906196e0ad4ad7eac2f7c725fb6ad6b5e02ffcc190e90a2a5cdb -->
<!-- api: P:Inno.Assets.AssetObject.runtimePayload 7b63b0f568de9bbdaf6d2e283b02eec860f64734f740328f6956a415434f4589 -->
<!-- api: M:Inno.Assets.AssetObject.RestoreProperties``1(System.Guid,System.Byte[],``0) f9f2e1d6aa161bb48e5d717ef5253b8e5c8b2a350db2b5af13c93f070195a817 -->
<!-- api: M:Inno.Assets.AssetObject.OnRuntimePayloadChanged(System.ReadOnlyMemory{System.Byte},System.ReadOnlyMemory{System.Byte}) 72670a4c85c69e7498aa89cf52f4b83cbe76886c546208264837a2a7dfb4c0c1 -->
<!-- api: M:Inno.Assets.AssetObject.OnUnloading eb9cd76d3b98bde638b3b92176360e482e9d74df33f7ca756a6e9096af5ac929 -->
<!-- api: M:Inno.Assets.AssetObject.#ctor 83dbec7ffe62e0ce0b4b50a7683aa266f92ce89cf8451e282cd700226ae3390c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b63e38367b6251b6"></a>[`protected Inno.Assets.AssetObject.AssetObject()`](../../src/content/assets/Inno.Assets/AssetObject.cs#L13) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-329e061ce8d3574b"></a>[`protected virtual void Inno.Assets.AssetObject.OnRuntimePayloadChanged(System.ReadOnlyMemory<byte> previousPayload, System.ReadOnlyMemory<byte> currentPayload)`](../../src/content/assets/Inno.Assets/AssetObject.cs#L89) | Called after a new runtime payload has been committed to this instance.<br>`previousPayload`：The previously committed payload.<br>`currentPayload`：The newly committed payload. | — | — |
| <a id="api-9823bc68dfd0135e"></a>[`protected virtual void Inno.Assets.AssetObject.OnUnloading()`](../../src/content/assets/Inno.Assets/AssetObject.cs#L105) | Releases the runtime resources owned by this asset without clearing its payload before quiescence. | An unfinished release may throw RetirementPendingException. The owner retains the asset and retries this hook; completed steps must not be repeated. Other failures are terminal and are reported by the owner. | `Inno.Core.Execution.RetirementPendingException`：Owned work has not quiesced; runtime payload and ownership must remain intact until a later attempt. |
| <a id="api-90e82823cb2203d2"></a>[`public void Inno.Assets.AssetObject.RestoreProperties<TValue>(System.Guid stableTypeId, byte[] propertyData, TValue target) where TValue : class, Inno.Core.Serialization.ISerializable`](../../src/content/assets/Inno.Assets/AssetObject.cs#L69) | Restores detached extension settings using this asset's actual owner and converter generation.<br>`stableTypeId`：Persisted settings type identity.<br>`propertyData`：Native property payload to restore.<br>`target`：Detached target, never retained by the asset. | — | `System.InvalidOperationException`：The asset is unowned, retired, or lacks property-state services. |
| <a id="api-8f17abdce88a7fb5"></a>[`public Inno.Assets.AssetPath Inno.Assets.AssetObject.assetPath { get; private set; }`](../../src/content/assets/Inno.Assets/AssetObject.cs#L27) | Gets the isolated source path associated with this asset. | — | — |
| <a id="api-2d5d446413325a4d"></a>[`public long Inno.Assets.AssetObject.contentVersion { get; }`](../../src/content/assets/Inno.Assets/AssetObject.cs#L44) | Gets the version of the currently committed runtime content. | — | — |
| <a id="api-29eea3f355266536"></a>[`public bool Inno.Assets.AssetObject.isMissing { get; }`](../../src/content/assets/Inno.Assets/AssetObject.cs#L39) | Gets whether this instance represents an unavailable persistent asset. | — | — |
| <a id="api-3830ac0f06fce813"></a>[`public string Inno.Assets.AssetObject.name { get; }`](../../src/content/assets/Inno.Assets/AssetObject.cs#L32) | Gets a display name derived from the source-local path. | — | — |
| <a id="api-a2c14acedc78c5cb"></a>[`public System.ReadOnlyMemory<byte> Inno.Assets.AssetObject.runtimePayload { get; }`](../../src/content/assets/Inno.Assets/AssetObject.cs#L49) | Gets the runtime artifact payload produced by the importer. | — | — |
| <a id="api-c349f4a6858ea399"></a>[`public abstract class Inno.Assets.AssetObject : Inno.Core.Identity.IdentityObject, Inno.Core.Serialization.ISerializable`](../../src/content/assets/Inno.Assets/AssetObject.cs#L13) | Provides the common runtime identity and payload contract for imported assets. | — | — |

### `Inno.Assets.AssetPath`

<!-- api: T:Inno.Assets.AssetPath f1a9c916908c653a551e40435c56f1ad5a230776fac994f8e4f22012768a3b41 -->
<!-- api: M:Inno.Assets.AssetPath.#ctor(Inno.Assets.AssetSourceId,System.String) 0fdb23aad180361d04d76c192bc116f6b3583ec0339fed9c224a737dabd3f3c2 -->
<!-- api: P:Inno.Assets.AssetPath.source c50e600835faabda63e4ec2ca7ff77de4e0091041420802af3ab64c03d2f2213 -->
<!-- api: P:Inno.Assets.AssetPath.localPath d1478ae44a82a6161c8694ca80552a8f1ec4e0f331318c2723e7f38a58a063c0 -->
<!-- api: P:Inno.Assets.AssetPath.isValid 67df81d3f3340016597c3bd21821885891c6e274f77e62d56298221849ab63b5 -->
<!-- api: M:Inno.Assets.AssetPath.Project(System.String) 5b1584f808369bbe654445cbb0e495aeb1091ec97e0416a7f0261a48c20daf9c -->
<!-- api: M:Inno.Assets.AssetPath.Parse(System.String) 0eff86c381eeafb29465ba43d53209b6b00fb5f0c4bc2429d29e1b31cfa1b3d3 -->
<!-- api: M:Inno.Assets.AssetPath.ToString f5c9fbaf8f0422ddb1177c708daad0fe22e7cdc40ac120ff8257ecb64c7385ad -->
<!-- api: M:Inno.Assets.AssetPath.op_Inequality(Inno.Assets.AssetPath,Inno.Assets.AssetPath) f12a29cd912bb67635ba3874e0e2290b73c7d33a29da49e0a68e0fdb7931afbf -->
<!-- api: M:Inno.Assets.AssetPath.op_Equality(Inno.Assets.AssetPath,Inno.Assets.AssetPath) 19c8403db8703d34e5fecbe25501614c2c6ab5669734f552ecba5cd65f8f9227 -->
<!-- api: M:Inno.Assets.AssetPath.GetHashCode 53a4d2a628f428c33dd82f0fdaa80ed27a1490cbc2a0fe036acf2fdda387e02e -->
<!-- api: M:Inno.Assets.AssetPath.Equals(System.Object) 986cdf2ecf4acb6d9e544f251a428a6499d80056921234977970de3ed09439de -->
<!-- api: M:Inno.Assets.AssetPath.Equals(Inno.Assets.AssetPath) 1ed64562a1e46705f9ce0305fcaa3d89136d8be3ab1f3757d86c448e45af665e -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-08b3f61330cc137a"></a>[`public Inno.Assets.AssetPath.AssetPath(Inno.Assets.AssetSourceId source, string localPath)`](../../src/content/assets/Inno.Assets/AssetPath.cs#L74) | Creates an asset path.<br>`source`：Owning source mount.<br>`localPath`：Source-local path using forward slashes. | — | — |
| <a id="api-cb9315e63bc44cd9"></a>[`public readonly bool Inno.Assets.AssetPath.Equals(Inno.Assets.AssetPath other)`](../../src/content/assets/Inno.Assets/AssetPath.cs#L61) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-74b594f3aa2d1761"></a>[`public override readonly bool Inno.Assets.AssetPath.Equals(object obj)`](../../src/content/assets/Inno.Assets/AssetPath.cs#L61) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-511d0aa8bec31d10"></a>[`public override readonly int Inno.Assets.AssetPath.GetHashCode()`](../../src/content/assets/Inno.Assets/AssetPath.cs#L61) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-dcabbeeb2dbb898b"></a>[`public static bool Inno.Assets.AssetPath.operator ==(Inno.Assets.AssetPath left, Inno.Assets.AssetPath right)`](../../src/content/assets/Inno.Assets/AssetPath.cs#L61) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-c5815e1f56d12d9e"></a>[`public static bool Inno.Assets.AssetPath.operator !=(Inno.Assets.AssetPath left, Inno.Assets.AssetPath right)`](../../src/content/assets/Inno.Assets/AssetPath.cs#L61) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-0bfc6942994d1913"></a>[`public static Inno.Assets.AssetPath Inno.Assets.AssetPath.Parse(string value)`](../../src/content/assets/Inno.Assets/AssetPath.cs#L119) | Parses a canonical path, treating an unqualified value as project-owned.<br>返回：The isolated asset path.<br>`value`：Canonical or project-local path. | — | — |
| <a id="api-eb1948a2973fbd5d"></a>[`public static Inno.Assets.AssetPath Inno.Assets.AssetPath.Project(string localPath)`](../../src/content/assets/Inno.Assets/AssetPath.cs#L108) | Creates a path in the writable project source.<br>返回：A project-owned asset path.<br>`localPath`：The local path text validated by the project operation. | — | — |
| <a id="api-7f8b60f15108c93e"></a>[`public override readonly string Inno.Assets.AssetPath.ToString()`](../../src/content/assets/Inno.Assets/AssetPath.cs#L134) | Formats this value as a human-readable representation.<br>返回：The human-readable representation of this value. | — | — |
| <a id="api-694e7713504f5606"></a>[`public readonly bool Inno.Assets.AssetPath.isValid { get; }`](../../src/content/assets/Inno.Assets/AssetPath.cs#L97) | Gets whether the source and local path are valid. | — | — |
| <a id="api-17fd7a179cf42c2b"></a>[`public string Inno.Assets.AssetPath.localPath { get; set; }`](../../src/content/assets/Inno.Assets/AssetPath.cs#L92) | Gets or sets the source-local path. | — | — |
| <a id="api-cd92be5da659bf4f"></a>[`public Inno.Assets.AssetSourceId Inno.Assets.AssetPath.source { get; set; }`](../../src/content/assets/Inno.Assets/AssetPath.cs#L87) | Gets or sets the owning source mount. | — | — |
| <a id="api-3e2b94dc3a1cc757"></a>[`public record struct Inno.Assets.AssetPath : System.IEquatable<Inno.Assets.AssetPath>`](../../src/content/assets/Inno.Assets/AssetPath.cs#L61) | Addresses one asset without conflating isolated source mounts. | — | — |

### `Inno.Assets.AssetPreparationStatistics`

<!-- api: T:Inno.Assets.AssetPreparationStatistics 01ba1d5025fa28a5daeee12e6f2e76b5a65d1b00acc8fb2e1603e0256d7fb1da -->
<!-- api: P:Inno.Assets.AssetPreparationStatistics.pendingRequests fd65ceb771e29ddb9e2fcfe99ae774c7b5d7d6d4d425d71a35eb714133d7a5a7 -->
<!-- api: P:Inno.Assets.AssetPreparationStatistics.peakPendingRequests 8c88dd81d46004e3f384bbcd98b3f7edd65dc714814724fb70d961515224f4ae -->
<!-- api: P:Inno.Assets.AssetPreparationStatistics.rejectedRequests 788c4320a308721a73dba617d64bdf3682075d601a2ea2e165bd4d6d6a15f306 -->
<!-- api: P:Inno.Assets.AssetPreparationStatistics.pendingPayloads 9a7a46cef816092601171abd1401f34460ebddaa9e290a54649c2857d49e488c -->
<!-- api: P:Inno.Assets.AssetPreparationStatistics.payloadReadsStarted 3f436dce2d49203978b3219271010a7f7bbc02955ccfae9245febdcf4644b8b3 -->
<!-- api: P:Inno.Assets.AssetPreparationStatistics.sharedPayloadReads 05c7e25534fce5a5fd746078bc79f89828e6a3ff27c1c8f402c9ba44ae155f17 -->
<!-- api: P:Inno.Assets.AssetPreparationStatistics.reservedBytes 9abd4233e547673ae86fd66e4c24b03adec25c2692ee0862c404cc8ff8af2a7e -->
<!-- api: P:Inno.Assets.AssetPreparationStatistics.peakReservedBytes fd7f8420d10639c0ff589104c17107a18809ba36665acefafe8b9216eb9ec334 -->
<!-- api: M:Inno.Assets.AssetPreparationStatistics.ToString 00e0cfa9c716a3277312adf2199887a97592e0cf4601bc8b454d76c435ba544e -->
<!-- api: M:Inno.Assets.AssetPreparationStatistics.op_Inequality(Inno.Assets.AssetPreparationStatistics,Inno.Assets.AssetPreparationStatistics) 16e1aa0368d77902966bebbe84e3791f88e22cd0fd55471524839c52215c79fe -->
<!-- api: M:Inno.Assets.AssetPreparationStatistics.op_Equality(Inno.Assets.AssetPreparationStatistics,Inno.Assets.AssetPreparationStatistics) 6cef72606af45b58bf11dcdb93093b883ab92512d3495034a9ed771de7792df0 -->
<!-- api: M:Inno.Assets.AssetPreparationStatistics.GetHashCode 16358b13c3c030b839d1d4245f097af41aeed5d78f3545ff38e5222431547217 -->
<!-- api: M:Inno.Assets.AssetPreparationStatistics.Equals(System.Object) 76b7e276e7536e539110b098645612080918b726a6dd65b99d36bab02f607e51 -->
<!-- api: M:Inno.Assets.AssetPreparationStatistics.Equals(Inno.Assets.AssetPreparationStatistics) df0161682c33eaf8f17691efc68537462c72032edb9066c6ed328073b79e55de -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-9fa032226fb682e2"></a>[`public bool Inno.Assets.AssetPreparationStatistics.Equals(Inno.Assets.AssetPreparationStatistics other)`](../../src/content/assets/Inno.Assets/Runtime/AssetPreparationStatistics.cs#L6) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-5582f3b22bdbccc9"></a>[`public override bool Inno.Assets.AssetPreparationStatistics.Equals(object obj)`](../../src/content/assets/Inno.Assets/Runtime/AssetPreparationStatistics.cs#L6) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-56bae047a536afbf"></a>[`public override int Inno.Assets.AssetPreparationStatistics.GetHashCode()`](../../src/content/assets/Inno.Assets/Runtime/AssetPreparationStatistics.cs#L6) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-d0827978b39ebdbd"></a>[`public static bool Inno.Assets.AssetPreparationStatistics.operator ==(Inno.Assets.AssetPreparationStatistics left, Inno.Assets.AssetPreparationStatistics right)`](../../src/content/assets/Inno.Assets/Runtime/AssetPreparationStatistics.cs#L6) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-56bca081877e4dbe"></a>[`public static bool Inno.Assets.AssetPreparationStatistics.operator !=(Inno.Assets.AssetPreparationStatistics left, Inno.Assets.AssetPreparationStatistics right)`](../../src/content/assets/Inno.Assets/Runtime/AssetPreparationStatistics.cs#L6) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-8a8b7d7e1facc3a6"></a>[`public override string Inno.Assets.AssetPreparationStatistics.ToString()`](../../src/content/assets/Inno.Assets/Runtime/AssetPreparationStatistics.cs#L6) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-ead18ae5cd5e8dc3"></a>[`public long Inno.Assets.AssetPreparationStatistics.payloadReadsStarted { get; }`](../../src/content/assets/Inno.Assets/Runtime/AssetPreparationStatistics.cs#L51) | Gets the cumulative number of unique physical payload reads started. | — | — |
| <a id="api-3e4700f4ed9a7930"></a>[`public int Inno.Assets.AssetPreparationStatistics.peakPendingRequests { get; }`](../../src/content/assets/Inno.Assets/Runtime/AssetPreparationStatistics.cs#L36) | Gets the largest simultaneous admitted request count during this database lifetime. | — | — |
| <a id="api-0787269ee222eee1"></a>[`public long Inno.Assets.AssetPreparationStatistics.peakReservedBytes { get; }`](../../src/content/assets/Inno.Assets/Runtime/AssetPreparationStatistics.cs#L66) | Gets the largest simultaneous unique encoded-byte reservation. | — | — |
| <a id="api-86440ea07a3014c7"></a>[`public int Inno.Assets.AssetPreparationStatistics.pendingPayloads { get; }`](../../src/content/assets/Inno.Assets/Runtime/AssetPreparationStatistics.cs#L46) | Gets unique payload reservations still retained by one or more cold-load roots. | — | — |
| <a id="api-e6ab5d4bae852124"></a>[`public int Inno.Assets.AssetPreparationStatistics.pendingRequests { get; }`](../../src/content/assets/Inno.Assets/Runtime/AssetPreparationStatistics.cs#L31) | Gets requests still awaiting owner-thread completion, including canceled callers not yet drained. | — | — |
| <a id="api-2a87773e2e3aff60"></a>[`public long Inno.Assets.AssetPreparationStatistics.rejectedRequests { get; }`](../../src/content/assets/Inno.Assets/Runtime/AssetPreparationStatistics.cs#L41) | Gets requests rejected by the pending-count or encoded-byte budget. | — | — |
| <a id="api-6359cd270621bceb"></a>[`public long Inno.Assets.AssetPreparationStatistics.reservedBytes { get; }`](../../src/content/assets/Inno.Assets/Runtime/AssetPreparationStatistics.cs#L61) | Gets unique encoded bytes retained until all requesting roots complete, fail or cancel. | — | — |
| <a id="api-8417f596a1d4e3fc"></a>[`public long Inno.Assets.AssetPreparationStatistics.sharedPayloadReads { get; }`](../../src/content/assets/Inno.Assets/Runtime/AssetPreparationStatistics.cs#L56) | Gets payload reads reused by distinct roots with overlapping dependency closures. | — | — |
| <a id="api-2c566a9977b45da6"></a>[`public readonly record struct Inno.Assets.AssetPreparationStatistics : System.IEquatable<Inno.Assets.AssetPreparationStatistics>`](../../src/content/assets/Inno.Assets/Runtime/AssetPreparationStatistics.cs#L6) | Reports owner-thread cold-load admission and unique encoded IO reservations without retaining assets or tasks. | — | — |

### `Inno.Assets.AssetPropertySnapshot`

<!-- api: T:Inno.Assets.AssetPropertySnapshot a585a42e9ce88cccf14de6f85f139da1c3bf1a31a7c890476f0e66bff1d9f939 -->
<!-- api: M:Inno.Assets.AssetPropertySnapshot.#ctor(System.Guid,System.ReadOnlySpan{System.Byte},System.Collections.Generic.IEnumerable{Inno.Assets.AssetDependency}) 4dd743cec1c0c8e4a0ef76f05f92c31974a46d01016a890c1641fdb563b75537 -->
<!-- api: P:Inno.Assets.AssetPropertySnapshot.stableTypeId ea78db95b31ff42e636f36baf7eeaa7458f91eced4215fc9ff83147b0e986ed0 -->
<!-- api: P:Inno.Assets.AssetPropertySnapshot.data b628d53f1a4b65ea7c1be743a0f6cb86236e52e1c7affec4b9cea4e7f1e518ef -->
<!-- api: P:Inno.Assets.AssetPropertySnapshot.dependencies dfafdc2c8b68e593f210abaa9a9093fc35211e0da0af38df5fc20cc9b7e2f5a8 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-fa8426a6b257ad4d"></a>[`public Inno.Assets.AssetPropertySnapshot.AssetPropertySnapshot(System.Guid stableTypeId, System.ReadOnlySpan<byte> data, System.Collections.Generic.IEnumerable<Inno.Assets.AssetDependency> dependencies)`](../../src/content/assets/Inno.Assets/Serialization/AssetPropertySnapshot.cs#L25) | Freezes a complete owner-produced property value.<br>`stableTypeId`：Persistent identity of the exact serialized settings type.<br>`data`：Native property payload, copied before returning.<br>`dependencies`：Direct dependencies collected during the same serialization operation. | — | — |
| <a id="api-7350cada5308453d"></a>[`public System.ReadOnlyMemory<byte> Inno.Assets.AssetPropertySnapshot.data { get; }`](../../src/content/assets/Inno.Assets/Serialization/AssetPropertySnapshot.cs#L44) | Gets the frozen native property bytes. | — | — |
| <a id="api-2450970d1c993873"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Assets.AssetDependency> Inno.Assets.AssetPropertySnapshot.dependencies { get; }`](../../src/content/assets/Inno.Assets/Serialization/AssetPropertySnapshot.cs#L48) | Gets the frozen direct dependency declarations. | — | — |
| <a id="api-14dbca83f3ebc991"></a>[`public System.Guid Inno.Assets.AssetPropertySnapshot.stableTypeId { get; }`](../../src/content/assets/Inno.Assets/Serialization/AssetPropertySnapshot.cs#L40) | Gets the persistent settings type identity. | — | — |
| <a id="api-9bda3cc3f893ae2f"></a>[`public sealed class Inno.Assets.AssetPropertySnapshot`](../../src/content/assets/Inno.Assets/Serialization/AssetPropertySnapshot.cs#L10) | Contains native properties and automatically captured asset dependencies without retaining their owner or typed object. | — | — |

### `Inno.Assets.AssetReferenceInfo`

<!-- api: T:Inno.Assets.AssetReferenceInfo 8a8ba11653b431f5e7a4f1d235df13a1b4ca8e843ac24af9d96c46502a245989 -->
<!-- api: M:Inno.Assets.AssetReferenceInfo.#ctor(System.Guid,Inno.Assets.AssetPath,System.Int64,System.Boolean,System.Nullable{System.Boolean},System.Collections.Generic.IReadOnlyList{Inno.Assets.AssetReferenceLocation}) cc6e4d07f6912fcdb4750ce49de83a855868045b58bb468ec3a0ea6a50ba7071 -->
<!-- api: P:Inno.Assets.AssetReferenceInfo.persistentId 5837b2df7aa587da19505b8385586061020d0e7044c93358cd2b2f696b313122 -->
<!-- api: P:Inno.Assets.AssetReferenceInfo.assetPath ab92bd00193bda459553375611c887f6924e35816ee4dc35e9d4aeee54f7579d -->
<!-- api: P:Inno.Assets.AssetReferenceInfo.contentVersion a0e9f8ea17d105a7cd27fbab4c32ce8d1d92aa056b2d29400f4f04e4e8da88b8 -->
<!-- api: P:Inno.Assets.AssetReferenceInfo.isLoaded 60da52c2cca88fdd12a4b85e0219d3d33b340bbaedbdd47fa9b27d90622d9d03 -->
<!-- api: P:Inno.Assets.AssetReferenceInfo.lastSweepReachability 2f73395b04b64a3d1811352cb7293a9dde6c21acb397beedd2edff22c689a5af -->
<!-- api: P:Inno.Assets.AssetReferenceInfo.knownReferenceCount b1b5e890be2e1dec749dcfcf5507fb4cad75b62b4605264da187d0aea00925ec -->
<!-- api: P:Inno.Assets.AssetReferenceInfo.references f3d27694c2b68d7686a57d71252fe0844a606bfbf0d9f879904b997c87bd6b14 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-c4c5ab5778d05fb0"></a>[`public Inno.Assets.AssetReferenceInfo.AssetReferenceInfo(System.Guid persistentId, Inno.Assets.AssetPath assetPath, long contentVersion, bool isLoaded, bool? lastSweepReachability, System.Collections.Generic.IReadOnlyList<Inno.Assets.AssetReferenceLocation> references)`](../../src/content/assets/Inno.Assets/AssetReferenceInfo.cs#L37) | Freezes one diagnostic view of the engine-known references to an asset.<br>`persistentId`：The persistent asset identity.<br>`assetPath`：The mount-qualified source location.<br>`contentVersion`：The observed content revision.<br>`isLoaded`：Whether a canonical instance is resident.<br>`lastSweepReachability`：The previous reachability result, or null if not inspected.<br>`references`：Reference locations copied into this immutable snapshot. | — | — |
| <a id="api-61271a253fb2e58e"></a>[`public Inno.Assets.AssetPath Inno.Assets.AssetReferenceInfo.assetPath { get; }`](../../src/content/assets/Inno.Assets/AssetReferenceInfo.cs#L62) | Gets the current mount-qualified asset path. | — | — |
| <a id="api-15751c69021d2ef4"></a>[`public long Inno.Assets.AssetReferenceInfo.contentVersion { get; }`](../../src/content/assets/Inno.Assets/AssetReferenceInfo.cs#L67) | Gets the current runtime content version. | — | — |
| <a id="api-6085edfb27d2694c"></a>[`public bool Inno.Assets.AssetReferenceInfo.isLoaded { get; }`](../../src/content/assets/Inno.Assets/AssetReferenceInfo.cs#L72) | Gets whether the asset is currently held by the loader cache. | — | — |
| <a id="api-4f34858e8f582900"></a>[`public int Inno.Assets.AssetReferenceInfo.knownReferenceCount { get; }`](../../src/content/assets/Inno.Assets/AssetReferenceInfo.cs#L83) | Gets the number of engine-known reference locations. | — | — |
| <a id="api-54f57bec1fc8da42"></a>[`public bool? Inno.Assets.AssetReferenceInfo.lastSweepReachability { get; }`](../../src/content/assets/Inno.Assets/AssetReferenceInfo.cs#L78) | Gets whether an external managed reference was found by the previous unused-asset sweep, or null when no sweep has inspected this asset. | — | — |
| <a id="api-277b1634fd3fcf8d"></a>[`public System.Guid Inno.Assets.AssetReferenceInfo.persistentId { get; }`](../../src/content/assets/Inno.Assets/AssetReferenceInfo.cs#L57) | Gets the persistent asset identity. | — | — |
| <a id="api-e895085475fdfcba"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Assets.AssetReferenceLocation> Inno.Assets.AssetReferenceInfo.references { get; }`](../../src/content/assets/Inno.Assets/AssetReferenceInfo.cs#L88) | Gets the engine-known reference locations. | — | — |
| <a id="api-e599d05b5d964cf8"></a>[`public sealed class Inno.Assets.AssetReferenceInfo`](../../src/content/assets/Inno.Assets/AssetReferenceInfo.cs#L14) | Provides a stable diagnostic snapshot of references known to the asset pipeline. | The known reference count is not the number of CLR strong references and is never used to decide whether an asset can be collected. | — |

### `Inno.Assets.AssetReferenceKind`

<!-- api: T:Inno.Assets.AssetReferenceKind 5a40fd4cbe1a7aa8d971cb82491648e8ce21e2e7ceaaa9364e14f591f19c12fc -->
<!-- api: F:Inno.Assets.AssetReferenceKind.AssetDependency 1d7cf1556899ae83b7625dd80608b71162f06d5afeefa6f7ba9e303f07db0ad4 -->
<!-- api: F:Inno.Assets.AssetReferenceKind.SerializedProperty e655ad443612abc2a69efa6b3c0689aad8b3533c2bb9004c36afab6c877e080b -->
<!-- api: F:Inno.Assets.AssetReferenceKind.SceneResource c0972a5a5a4f5e5fca25771b1b8fccd9cc6adbdf0d8c84aea719758d450b7855 -->
<!-- api: F:Inno.Assets.AssetReferenceKind.PrefabSource b183027d1fb3f192c84697dc67904d4454195955f6cdb75535bab57847831004 -->
<!-- api: F:Inno.Assets.AssetReferenceKind.Editor 866bef820a5d06fbdc5d20fbd43978857c29f926a9ae9bbef202563a640fe298 -->
<!-- api: F:Inno.Assets.AssetReferenceKind.RuntimeSubsystem 1a146fab941ce0d118dc410acfe38c393b7aa54e0efcc5f84e8c3882b685d84f -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-8b5cd4c345d0a8ca"></a>[`Inno.Assets.AssetReferenceKind.AssetDependency = 0`](../../src/content/assets/Inno.Assets/AssetReferenceKind.cs#L12) | The reference originates from another asset's runtime dependencies. | — | — |
| <a id="api-f76a78fb42d55a12"></a>[`Inno.Assets.AssetReferenceKind.Editor = 4`](../../src/content/assets/Inno.Assets/AssetReferenceKind.cs#L28) | The reference originates from an editor-only view or selection. | — | — |
| <a id="api-f391f9eba41edc53"></a>[`Inno.Assets.AssetReferenceKind.PrefabSource = 3`](../../src/content/assets/Inno.Assets/AssetReferenceKind.cs#L24) | The reference originates from a prefab source. | — | — |
| <a id="api-30501d59781f8e59"></a>[`Inno.Assets.AssetReferenceKind.RuntimeSubsystem = 5`](../../src/content/assets/Inno.Assets/AssetReferenceKind.cs#L32) | The reference originates from a runtime subsystem. | — | — |
| <a id="api-802c9730bd8b8945"></a>[`Inno.Assets.AssetReferenceKind.SceneResource = 2`](../../src/content/assets/Inno.Assets/AssetReferenceKind.cs#L20) | The reference originates from a scene resource. | — | — |
| <a id="api-34f38e2211c17ec9"></a>[`Inno.Assets.AssetReferenceKind.SerializedProperty = 1`](../../src/content/assets/Inno.Assets/AssetReferenceKind.cs#L16) | The reference originates from a serialized property. | — | — |
| <a id="api-9876cd335660f59a"></a>[`public enum Inno.Assets.AssetReferenceKind : int`](../../src/content/assets/Inno.Assets/AssetReferenceKind.cs#L7) | Identifies a reference source known to the asset pipeline. | — | — |

### `Inno.Assets.AssetReferenceLocation`

<!-- api: T:Inno.Assets.AssetReferenceLocation 7e0588b3dbb80aa919fe94451596cb57d66b2a192ef0c464f324ea002c346bf0 -->
<!-- api: M:Inno.Assets.AssetReferenceLocation.#ctor(Inno.Assets.AssetReferenceKind,System.Guid,System.String,System.String) cf3a7f7c1929b271407c8daa19a21bd62ee02701a985a7391dd6f5e41fe655fa -->
<!-- api: P:Inno.Assets.AssetReferenceLocation.kind 455424ba56ac5a747af508bb3ee109be3e16d7794df19aae874dd32836b660e1 -->
<!-- api: P:Inno.Assets.AssetReferenceLocation.ownerId 39ffd274b37db3b8b47d4ed440f0f4b70ab93650553ab1efd05aec0ab137e931 -->
<!-- api: P:Inno.Assets.AssetReferenceLocation.ownerName 24cc732d40010832b048b28388f7506d914e99791a1d427e57839ada41ce9338 -->
<!-- api: P:Inno.Assets.AssetReferenceLocation.propertyPath c5bac3db35e25ae8e492b486c520a7f4711711d8963ced0ac13b5ed83508f241 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-5eeb54f6d0db4677"></a>[`public Inno.Assets.AssetReferenceLocation.AssetReferenceLocation(Inno.Assets.AssetReferenceKind kind, System.Guid ownerId, string ownerName, string propertyPath)`](../../src/content/assets/Inno.Assets/AssetReferenceLocation.cs#L25) | Describes a neutral, persistent location without retaining its live owner.<br>`kind`：The category of reference owner.<br>`ownerId`：The owner's persistent identity.<br>`ownerName`：The display name captured for diagnostics.<br>`propertyPath`：The serialized property or subsystem-relative location. | — | — |
| <a id="api-074917eaeaa193c8"></a>[`public Inno.Assets.AssetReferenceKind Inno.Assets.AssetReferenceLocation.kind { get; }`](../../src/content/assets/Inno.Assets/AssetReferenceLocation.cs#L40) | Gets the category of the known reference. | — | — |
| <a id="api-bc396645e145d2b3"></a>[`public System.Guid Inno.Assets.AssetReferenceLocation.ownerId { get; }`](../../src/content/assets/Inno.Assets/AssetReferenceLocation.cs#L45) | Gets the persistent identity of the known owner, when available. | — | — |
| <a id="api-2107c635d5693f74"></a>[`public string Inno.Assets.AssetReferenceLocation.ownerName { get; }`](../../src/content/assets/Inno.Assets/AssetReferenceLocation.cs#L50) | Gets the display name of the known owner. | — | — |
| <a id="api-6a7ed4270fc49d51"></a>[`public string Inno.Assets.AssetReferenceLocation.propertyPath { get; }`](../../src/content/assets/Inno.Assets/AssetReferenceLocation.cs#L55) | Gets the serialized or subsystem-relative property path. | — | — |
| <a id="api-18c8a4deceef6c44"></a>[`public sealed class Inno.Assets.AssetReferenceLocation`](../../src/content/assets/Inno.Assets/AssetReferenceLocation.cs#L8) | Describes one engine-known reference location for an asset. | — | — |

### `Inno.Assets.AssetReferenceProtocol`

<!-- api: T:Inno.Assets.AssetReferenceProtocol 4771a672bd541cdc274ae4d4f53a1e5388b8a16dcef62e66391fca144f02dbf5 -->
<!-- api: P:Inno.Assets.AssetReferenceProtocol.id 0ea1fdbc241db0dcf6e23d9d539e85df009a9eaefd0c901efacfc02d9e16f19e -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-dc085a6e626e57cb"></a>[`public static Inno.References.ReferenceKindId Inno.Assets.AssetReferenceProtocol.id { get; }`](../../src/content/assets/Inno.Assets/Serialization/AssetReferenceProtocol.cs#L13) | Gets the stable asset reference kind registered in shared reference catalogs. | — | — |
| <a id="api-a1271bd06a423e1e"></a>[`public static class Inno.Assets.AssetReferenceProtocol`](../../src/content/assets/Inno.Assets/Serialization/AssetReferenceProtocol.cs#L8) | Defines the open cross-domain protocol used to resolve persistent asset identities. | — | — |

### `Inno.Assets.AssetResidencyProvider`

<!-- api: T:Inno.Assets.AssetResidencyProvider d94a0a02dc62c21ca83be4266a59a7ef49a7b3825845d6de8c86d17b658f9e7b -->
<!-- api: M:Inno.Assets.AssetResidencyProvider.CreateAssetLease``1(``0,System.Action) baa7862b17892c76d42261f03e9ea76c5fbde75391d8e2a3adf41119f6e0cc4c -->
<!-- api: M:Inno.Assets.AssetResidencyProvider.CreateArtifactLease(Inno.Assets.AssetArtifactInfo,System.Func{System.IO.Stream},System.Action) 34bb67732b017e13523afc32f66cecfc5fb8b5b70574c918230cc33f40f54d14 -->
<!-- api: M:Inno.Assets.AssetResidencyProvider.#ctor 3b0cbfa27d40f133cf511ad5c5f730846c4776d646891bddacfa3b996614a1a7 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e43a10da740d1195"></a>[`protected Inno.Assets.AssetResidencyProvider.AssetResidencyProvider()`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L80) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-d81d0dc5cb5edf34"></a>[`protected static Inno.Assets.ArtifactLease Inno.Assets.AssetResidencyProvider.CreateArtifactLease(Inno.Assets.AssetArtifactInfo artifact, System.Func<System.IO.Stream> openRead, System.Action release)`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L119) | Creates one immutable artifact residency lease.<br>返回：A lease that retains its value until release completes; a Pending callback is retried by the owning lifecycle.<br>`artifact`：Verified artifact metadata retained by the lease.<br>`openRead`：Opens immutable bytes with an independent provider pin owned by the returned stream.<br>`release`：Provider-owned release callback. Pending retirement must be retryable without repeating completed effects. | — | — |
| <a id="api-32c55277020a8808"></a>[`protected static Inno.Assets.AssetLease<TAsset> Inno.Assets.AssetResidencyProvider.CreateAssetLease<TAsset>(TAsset asset, System.Action release) where TAsset : Inno.Assets.AssetObject`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L97) | Creates one strongly typed asset residency lease.<br>返回：A lease that retains its value until release completes; a Pending callback is retried by the owning lifecycle.<br>`asset`：Canonical asset retained by the lease.<br>`release`：Provider-owned release callback. Pending retirement must be retryable without repeating completed effects. | — | — |
| <a id="api-2ec72f0add4abbd7"></a>[`public abstract class Inno.Assets.AssetResidencyProvider`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L80) | Provides controlled lease construction to concrete asset residency implementations. | — | — |

### `Inno.Assets.AssetResidencyStatistics`

<!-- api: T:Inno.Assets.AssetResidencyStatistics 64ea2f93ed4e4660612454d769b58da863287ffd3b05421e591b97bb9fc87fe7 -->
<!-- api: M:Inno.Assets.AssetResidencyStatistics.#ctor(System.Int32,System.Int64,System.Int64) 9469da029b6f899abeab1900449a4886f1080d8fb3d19c419110600f18e19111 -->
<!-- api: P:Inno.Assets.AssetResidencyStatistics.residentAssetCount fc2b38435947cd0ea136df973dade49dd8d19dfc3e51c4878bc8b1bf6f88e26b -->
<!-- api: P:Inno.Assets.AssetResidencyStatistics.residentBytes 135945cc61227604fe2967bd3f0df0bb3c2327b90bea903e2be8956b791be4e9 -->
<!-- api: P:Inno.Assets.AssetResidencyStatistics.budgetBytes c3bb303cfa82d7438573fef7b71e07e7ccd24cf94a2554a498394cf0d61122d0 -->
<!-- api: M:Inno.Assets.AssetResidencyStatistics.ToString 30c2ccd0c888361dc6b4ced6a846d7c3085ddeb849628f043962a52f13b292c0 -->
<!-- api: M:Inno.Assets.AssetResidencyStatistics.op_Inequality(Inno.Assets.AssetResidencyStatistics,Inno.Assets.AssetResidencyStatistics) 02b63cd675fa636474dbbafa353b4c7cc76622b0878e9aa6d92da9673c130cb7 -->
<!-- api: M:Inno.Assets.AssetResidencyStatistics.op_Equality(Inno.Assets.AssetResidencyStatistics,Inno.Assets.AssetResidencyStatistics) 6967e00e3cc6f0a30d9a33faea4537198061760c1a92908721f83abb9a31bfa3 -->
<!-- api: M:Inno.Assets.AssetResidencyStatistics.GetHashCode 2e20e91101f84144e584a83af5d9ec6dc5cefa67b845296e647ab9b8878ed630 -->
<!-- api: M:Inno.Assets.AssetResidencyStatistics.Equals(System.Object) 33e1dd3e9b2edea35450fd689e7257f0eb9f70a131ff86a005c8839e0cb8dd48 -->
<!-- api: M:Inno.Assets.AssetResidencyStatistics.Equals(Inno.Assets.AssetResidencyStatistics) 8e90489caf9bdd774ee2b65eb5f80e1cac8c8eeae373fc60c84041f971b65261 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-75469d171fb2cc1d"></a>[`public Inno.Assets.AssetResidencyStatistics.AssetResidencyStatistics(int residentAssetCount, long residentBytes, long budgetBytes)`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L307) | Creates one immutable residency snapshot.<br>`residentAssetCount`：Number of currently materialized canonical assets.<br>`residentBytes`：Runtime payload bytes retained by materialized assets.<br>`budgetBytes`：Configured runtime payload budget. | — | — |
| <a id="api-7cd7b57e0e4f8ef0"></a>[`public bool Inno.Assets.AssetResidencyStatistics.Equals(Inno.Assets.AssetResidencyStatistics other)`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L293) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-d08c8f72ff72b0a1"></a>[`public override bool Inno.Assets.AssetResidencyStatistics.Equals(object obj)`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L293) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-d29bf78e8e9c49c8"></a>[`public override int Inno.Assets.AssetResidencyStatistics.GetHashCode()`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L293) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-5ff0ef6817a2e95e"></a>[`public static bool Inno.Assets.AssetResidencyStatistics.operator ==(Inno.Assets.AssetResidencyStatistics left, Inno.Assets.AssetResidencyStatistics right)`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L293) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-eb685e461137aaa5"></a>[`public static bool Inno.Assets.AssetResidencyStatistics.operator !=(Inno.Assets.AssetResidencyStatistics left, Inno.Assets.AssetResidencyStatistics right)`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L293) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-2f276d36b943b0be"></a>[`public override string Inno.Assets.AssetResidencyStatistics.ToString()`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L293) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-8f17bdfa3d0955e0"></a>[`public long Inno.Assets.AssetResidencyStatistics.budgetBytes { get; }`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L330) | Gets the configured runtime payload budget. | — | — |
| <a id="api-aa8a00386b146b03"></a>[`public int Inno.Assets.AssetResidencyStatistics.residentAssetCount { get; }`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L320) | Gets the number of materialized canonical assets. | — | — |
| <a id="api-94c25a794f21ec5a"></a>[`public long Inno.Assets.AssetResidencyStatistics.residentBytes { get; }`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L325) | Gets retained runtime payload bytes. | — | — |
| <a id="api-51901cc220016d08"></a>[`public readonly record struct Inno.Assets.AssetResidencyStatistics : System.IEquatable<Inno.Assets.AssetResidencyStatistics>`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L293) | Reports current explicit runtime asset residency accounting. | — | — |

### `Inno.Assets.AssetRuntimeOwner`

<!-- api: T:Inno.Assets.AssetRuntimeOwner 323a487e9205d2cf7b8d83678fe84b279b33ba833748fffc05401ae9cb40bc15 -->
<!-- api: M:Inno.Assets.AssetRuntimeOwner.#ctor(Inno.Assets.IAssetPropertyStateResolver) 9cbfd9548033f256215258c54e8bf846f1f73cf6d1164430a3e1c6cf6ccff612 -->
<!-- api: M:Inno.Assets.AssetRuntimeOwner.GetSourceHash(Inno.Assets.AssetObject) 6342332e250dbc34c9f46bb109e2c75d15a356c5bd1b6d896ce0d03a9018a055 -->
<!-- api: M:Inno.Assets.AssetRuntimeOwner.Initialize(Inno.Assets.AssetObject,Inno.Assets.AssetPath,System.String,System.ReadOnlyMemory{System.Byte},System.Boolean,System.Int64) 2be7fd83673d4f70ca96744a942dcb141f4124d22dc396a3cd8a961cea628adb -->
<!-- api: M:Inno.Assets.AssetRuntimeOwner.UpdateAssetPath(Inno.Assets.AssetObject,Inno.Assets.AssetPath) 5d45a2894ddd4828761ebc2f8e846793a29f0ca9379e7fcef940a725b2f4004c -->
<!-- api: M:Inno.Assets.AssetRuntimeOwner.Release(Inno.Assets.AssetObject) b10ba8ac9dde543f6c91c0a525c345f6d0cee8868c8c9751ba47a1d8cfb12a42 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-55f958777a359103"></a>[`public Inno.Assets.AssetRuntimeOwner.AssetRuntimeOwner(Inno.Assets.IAssetPropertyStateResolver? properties = null)`](../../src/content/assets/Inno.Assets/Runtime/AssetRuntimeOwner.cs#L24) | Creates exclusive mutation authority with optional owner-bound extension state restoration.<br>`properties`：Owner resolver, weakly referenced so stale assets cannot retain a retired database. | — | — |
| <a id="api-b1093c1341bff2f4"></a>[`public string Inno.Assets.AssetRuntimeOwner.GetSourceHash(Inno.Assets.AssetObject asset)`](../../src/content/assets/Inno.Assets/Runtime/AssetRuntimeOwner.cs#L38) | Reads the source fingerprint of an asset claimed by this owner.<br>返回：The fingerprint currently committed to the asset.<br>`asset`：The owned asset. | — | `System.InvalidOperationException`：The asset belongs to a different owner. |
| <a id="api-c982685b9a72e6f3"></a>[`public void Inno.Assets.AssetRuntimeOwner.Initialize(Inno.Assets.AssetObject asset, Inno.Assets.AssetPath assetPath, string sourceHash, System.ReadOnlyMemory<byte> payload, bool isMissing, long version)`](../../src/content/assets/Inno.Assets/Runtime/AssetRuntimeOwner.cs#L68) | Claims an unowned asset or commits new runtime state to an already owned instance.<br>`asset`：The instance to initialize; ownership cannot be transferred between databases.<br>`assetPath`：The current mount-qualified source path.<br>`sourceHash`：The immutable source fingerprint.<br>`payload`：Runtime bytes copied before publication.<br>`isMissing`：Whether the preserved reference is currently unavailable.<br>`version`：The current content revision used to invalidate runtime caches. | — | `System.InvalidOperationException`：The asset belongs to a different owner. |
| <a id="api-b0c21d407b5dafcd"></a>[`public void Inno.Assets.AssetRuntimeOwner.Release(Inno.Assets.AssetObject asset)`](../../src/content/assets/Inno.Assets/Runtime/AssetRuntimeOwner.cs#L112) | Releases an owned instance after quiescence, without relinquishing its mutation authority.<br>`asset`：The canonical instance or uncommitted candidate to retire. | — | `System.InvalidOperationException`：The asset belongs to a different owner.<br>`Inno.Core.Execution.RetirementPendingException`：Owned work remains active. Retain the instance and retry at the owner's retirement safe point. |
| <a id="api-e8ac71847515294a"></a>[`public void Inno.Assets.AssetRuntimeOwner.UpdateAssetPath(Inno.Assets.AssetObject asset, Inno.Assets.AssetPath assetPath)`](../../src/content/assets/Inno.Assets/Runtime/AssetRuntimeOwner.cs#L92) | Updates source location without replacing committed runtime content.<br>`asset`：The owned canonical instance.<br>`assetPath`：The new mount-qualified location. | — | `System.InvalidOperationException`：The asset belongs to a different owner. |
| <a id="api-9c5b677450cbad79"></a>[`public sealed class Inno.Assets.AssetRuntimeOwner`](../../src/content/assets/Inno.Assets/Runtime/AssetRuntimeOwner.cs#L13) | Grants one database exclusive mutation authority over the asset instances it initializes. | This capability is not a global mutation bridge and is not exported to scripts. A database keeps its owner private and explicitly releases canonical assets; the capability itself retains no assets. | — |

### `Inno.Assets.Assets`

<!-- api: T:Inno.Assets.Assets 7482cd44ade4221eb8b0f3d5d9303d242b980c5a7598417e611762e0ef343c6c -->
<!-- api: M:Inno.Assets.Assets.LocalPath(System.String,System.String) 45b0f2bd2f52193a93de01dd80b6883939426694b6e2396cca5a7614c5f5e1aa -->
<!-- api: M:Inno.Assets.Assets.Load``1(Inno.Assets.AssetPath) 5b90c85a57895934adb4f02c2032408cb7fa9a84ad6d202d9980f2926bb3c35f -->
<!-- api: M:Inno.Assets.Assets.Load``1(System.Guid) f8f22d776be2393cd0a35243492959fa1ed48a8dd57c4c4321a9330941c92620 -->
<!-- api: M:Inno.Assets.Assets.TryLoad``1(Inno.Assets.AssetPath,``0@) 82b64b7e3ab724fade45d7991df214f987a24dad1235603a593084eb7939276a -->
<!-- api: M:Inno.Assets.Assets.TryLoad``1(System.Guid,``0@) 09af33216552bab2cda0c8e6e8d1ca8a6775184f1b062c670792b80851a63e34 -->
<!-- api: M:Inno.Assets.Assets.AcquireAsync``1(Inno.Assets.AssetPath,System.Threading.CancellationToken) 517f13efce9a5ba5e962f7ac34acecbb22793222f4416959654b213e2c761369 -->
<!-- api: M:Inno.Assets.Assets.AcquireAsync``1(System.Guid,System.Threading.CancellationToken) 767a8878b655d0c41692160e5a08fe894c7ec8898ff4ed542c470b90ddf09239 -->
<!-- api: M:Inno.Assets.Assets.AcquireArtifact(System.Guid,System.String) eef5d7fc7914feb8ef43d0c2335320518f1e4b9332ae9ad329c21d46ab594a64 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-8d361776361f9d58"></a>[`public static Inno.Assets.ArtifactLease Inno.Assets.Assets.AcquireArtifact(System.Guid persistentId, string outputName)`](../../src/content/assets/Inno.Assets/Runtime/Assets.cs#L201) | Acquires one verified immutable artifact output for an explicit lifetime.<br>返回：A lease over verified artifact metadata and bytes.<br>`persistentId`：Persistent identity of the artifact owner.<br>`outputName`：Stable artifact output name. | — | — |
| <a id="api-36359fc4eef4b75a"></a>[`public static System.Threading.Tasks.ValueTask<Inno.Assets.AssetLease<TAsset>> Inno.Assets.Assets.AcquireAsync<TAsset>(Inno.Assets.AssetPath path, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) where TAsset : Inno.Assets.AssetObject`](../../src/content/assets/Inno.Assets/Runtime/Assets.cs#L160) | Acquires a canonical asset and explicitly retains it until the returned lease is disposed.<br>返回：A lease that owns residency for the canonical asset.<br>`path`：Mount-qualified logical asset path.<br>`cancellationToken`：Cancellation for this caller's asynchronous wait. | — | — |
| <a id="api-11830605d8580826"></a>[`public static System.Threading.Tasks.ValueTask<Inno.Assets.AssetLease<TAsset>> Inno.Assets.Assets.AcquireAsync<TAsset>(System.Guid persistentId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) where TAsset : Inno.Assets.AssetObject`](../../src/content/assets/Inno.Assets/Runtime/Assets.cs#L182) | Acquires a canonical asset by persistent identity and explicitly retains it.<br>返回：A lease that owns residency for the canonical asset.<br>`persistentId`：Persistent asset identity.<br>`cancellationToken`：Cancellation for this caller's asynchronous wait. | — | — |
| <a id="api-d1a48e626779f6c6"></a>[`public static TAsset Inno.Assets.Assets.Load<TAsset>(Inno.Assets.AssetPath path) where TAsset : Inno.Assets.AssetObject`](../../src/content/assets/Inno.Assets/Runtime/Assets.cs#L70) | Loads the canonical asset at a logical catalog path in the current session.<br>返回：The canonical compatible asset owned by the current lookup.<br>`path`：The mount-qualified logical catalog path. | — | `System.InvalidOperationException`：Thrown when no asset lookup is active, the asset does not exist, or its type is incompatible. |
| <a id="api-fc63fd1354f7230f"></a>[`public static TAsset Inno.Assets.Assets.Load<TAsset>(System.Guid persistentId) where TAsset : Inno.Assets.AssetObject`](../../src/content/assets/Inno.Assets/Runtime/Assets.cs#L89) | Loads the canonical asset with a persistent identity in the current session.<br>返回：The canonical compatible asset owned by the current lookup.<br>`persistentId`：The non-empty persistent asset identity. | — | `System.InvalidOperationException`：Thrown when no asset lookup is active, the asset does not exist, or its type is incompatible. |
| <a id="api-15d09f326a9a4839"></a>[`public static Inno.Assets.AssetPath Inno.Assets.Assets.LocalPath(string localPath, string sourceFile = "")`](../../src/content/assets/Inno.Assets/Runtime/Assets.cs#L35) | Creates a path relative to the Asset source that owns the calling script.<br>返回：A mount-qualified path that follows the script from Project development into an installed Plugin.<br>`localPath`：The normalized path relative to the caller's Project or installed Plugin Assets root.<br>`sourceFile`：The compiler-supplied canonical Asset path of the calling source file; callers normally omit it. | — | `System.InvalidOperationException`：The caller does not provide a canonical source location produced by the scripting compiler.<br>`System.ArgumentNullException`：The local path or source location is null. |
| <a id="api-72efbcd60f7064f4"></a>[`public static bool Inno.Assets.Assets.TryLoad<TAsset>(Inno.Assets.AssetPath path, out TAsset? asset) where TAsset : Inno.Assets.AssetObject`](../../src/content/assets/Inno.Assets/Runtime/Assets.cs#L112) | Tries to load the canonical asset at a logical catalog path in the current session.<br>返回：true when the current lookup contains a compatible asset; otherwise, false.<br>`path`：The mount-qualified logical catalog path.<br>`asset`：Receives the canonical compatible asset when available. | — | `System.InvalidOperationException`：Thrown when no asset lookup is active for the caller. |
| <a id="api-c94970c0bceee50b"></a>[`public static bool Inno.Assets.Assets.TryLoad<TAsset>(System.Guid persistentId, out TAsset? asset) where TAsset : Inno.Assets.AssetObject`](../../src/content/assets/Inno.Assets/Runtime/Assets.cs#L138) | Tries to load the canonical asset with a persistent identity in the current session.<br>返回：true when the current lookup contains a compatible asset; otherwise, false.<br>`persistentId`：The non-empty persistent asset identity.<br>`asset`：Receives the canonical compatible asset when available. | — | `System.InvalidOperationException`：Thrown when no asset lookup is active for the caller. |
| <a id="api-1f6f83c3e4eac6b0"></a>[`public static class Inno.Assets.Assets`](../../src/content/assets/Inno.Assets/Runtime/Assets.cs#L15) | Provides script-facing asset queries through the lookup bound to the current runtime session. | This façade owns no asset state. Engine and Editor infrastructure should depend on an explicit Inno.Assets.IAssetLookup instance. | — |

### `Inno.Assets.AssetSerializationContext`

<!-- api: T:Inno.Assets.AssetSerializationContext a9b15a881b4365e01385653fa74dfd25ae1eda8eb458e8a9235cd417b4f051ab -->
<!-- api: M:Inno.Assets.AssetSerializationContext.Create(Inno.Assets.IAssetReferenceResolver,Inno.Assets.AssetDependencyCollection) 9165563fd5737b3733eb88ce87f21166781b5af8d94822f1f7c5378ea065c72d -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e61ec1420d1df84a"></a>[`public static Inno.Core.Serialization.SerializationContext Inno.Assets.AssetSerializationContext.Create(Inno.Assets.IAssetReferenceResolver references, Inno.Assets.AssetDependencyCollection? dependencies = null)`](../../src/content/assets/Inno.Assets/Serialization/AssetSerializationContext.cs#L27) | Creates a context containing the required asset resolver and an optional dependency collector.<br>返回：A complete immutable asset-aware serialization context.<br>`references`：The authoritative asset reference resolver for the operation's complete generation.<br>`dependencies`：Optional collector that records direct asset dependencies during serialization. | — | `System.ArgumentNullException`：Thrown when references is null. |
| <a id="api-c46383f931d8102f"></a>[`public static class Inno.Assets.AssetSerializationContext`](../../src/content/assets/Inno.Assets/Serialization/AssetSerializationContext.cs#L10) | Creates complete asset-aware converter contexts from an owner-provided reference generation. | — | — |

### `Inno.Assets.AssetSourceId`

<!-- api: T:Inno.Assets.AssetSourceId aa2f8adb642cb30ef2b5e0a9afaec52b82150663eef82c0332e300539a774ba5 -->
<!-- api: P:Inno.Assets.AssetSourceId.project e5e573c1f3467d139275096684a4b31a350f0ae0b364fda0ab07ea3851bba6e8 -->
<!-- api: M:Inno.Assets.AssetSourceId.#ctor(System.String) 9ad6d0bdeb01afdcd5bc9cc828693952ef4f2d8abc43669de8be9e5216731f9c -->
<!-- api: P:Inno.Assets.AssetSourceId.value e7d558ece4134bb5ea401d88ffb990a573fef6c6258225f55b65b221a62fb1a1 -->
<!-- api: P:Inno.Assets.AssetSourceId.isValid 32a612c12e5a9a3369a4d58f0dafb075d076f19c0309d395e98411d773fa0e68 -->
<!-- api: M:Inno.Assets.AssetSourceId.ToString f8f33b9820b0408ba47389fe0319aea88dfc32b09c5f4f29272ad86872dd4f6d -->
<!-- api: M:Inno.Assets.AssetSourceId.op_Inequality(Inno.Assets.AssetSourceId,Inno.Assets.AssetSourceId) 11933b9a1bacce5b89564c44b1e757498c66fa475b5afa8130faee5b54eab509 -->
<!-- api: M:Inno.Assets.AssetSourceId.op_Equality(Inno.Assets.AssetSourceId,Inno.Assets.AssetSourceId) 3c2feed3b62d6c9400a773db7ac506a66e6efaaf1626d4a5a125aac96b74b49a -->
<!-- api: M:Inno.Assets.AssetSourceId.GetHashCode 70e2c50421b014e8cf3e4e2b4ea4d9b1fdc95b9b49e5010d075e2340493158d8 -->
<!-- api: M:Inno.Assets.AssetSourceId.Equals(System.Object) 83b7cbbf3ab05b632f81938ed51eaa03d1ae5cc3a8e2afb8da8983cf5bb19c68 -->
<!-- api: M:Inno.Assets.AssetSourceId.Equals(Inno.Assets.AssetSourceId) 68c7669c2f280fdbe067f701d01c46fd22303c681330b5cf123429d0c8ded2e9 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-ce975e898e5af2cf"></a>[`public Inno.Assets.AssetSourceId.AssetSourceId(string value)`](../../src/content/assets/Inno.Assets/AssetPath.cs#L23) | Creates a globally stable source identifier.<br>`value`：Lowercase project or Plugin source identity. | — | — |
| <a id="api-7c924072e9bacc55"></a>[`public readonly bool Inno.Assets.AssetSourceId.Equals(Inno.Assets.AssetSourceId other)`](../../src/content/assets/Inno.Assets/AssetPath.cs#L10) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-e09221fbf1b89964"></a>[`public override readonly bool Inno.Assets.AssetSourceId.Equals(object obj)`](../../src/content/assets/Inno.Assets/AssetPath.cs#L10) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-35d9c080ab84ff97"></a>[`public override readonly int Inno.Assets.AssetSourceId.GetHashCode()`](../../src/content/assets/Inno.Assets/AssetPath.cs#L10) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-1bd6597781422e13"></a>[`public static bool Inno.Assets.AssetSourceId.operator ==(Inno.Assets.AssetSourceId left, Inno.Assets.AssetSourceId right)`](../../src/content/assets/Inno.Assets/AssetPath.cs#L10) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-7c111845281895d9"></a>[`public static bool Inno.Assets.AssetSourceId.operator !=(Inno.Assets.AssetSourceId left, Inno.Assets.AssetSourceId right)`](../../src/content/assets/Inno.Assets/AssetPath.cs#L10) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-000eb67bc446d871"></a>[`public override readonly string Inno.Assets.AssetSourceId.ToString()`](../../src/content/assets/Inno.Assets/AssetPath.cs#L55) | Formats this value as a human-readable representation.<br>返回：The human-readable representation of this value. | — | — |
| <a id="api-c877589ed840ea9c"></a>[`public readonly bool Inno.Assets.AssetSourceId.isValid { get; }`](../../src/content/assets/Inno.Assets/AssetPath.cs#L47) | Gets whether the source identity has a usable value. | — | — |
| <a id="api-8fdc071598820923"></a>[`public static Inno.Assets.AssetSourceId Inno.Assets.AssetSourceId.project { get; }`](../../src/content/assets/Inno.Assets/AssetPath.cs#L15) | Gets the writable project source identifier. | — | — |
| <a id="api-5e149022feca4181"></a>[`public string Inno.Assets.AssetSourceId.value { get; set; }`](../../src/content/assets/Inno.Assets/AssetPath.cs#L42) | Gets or sets the globally stable source value. | — | — |
| <a id="api-7e3015e65759be9b"></a>[`public record struct Inno.Assets.AssetSourceId : System.IEquatable<Inno.Assets.AssetSourceId>`](../../src/content/assets/Inno.Assets/AssetPath.cs#L10) | Identifies one isolated asset source mount. | — | — |

### `Inno.Assets.AssetSourceKind`

<!-- api: T:Inno.Assets.AssetSourceKind 013d7750e6e9296baeca5f995638483e333be505170c0390e6a07a9e48ebd449 -->
<!-- api: F:Inno.Assets.AssetSourceKind.File 1ac3a811d6356cd97c42dbe7ab97b6a6db8afdd81c27123ae70ec62424fa0a32 -->
<!-- api: F:Inno.Assets.AssetSourceKind.Directory d230c418b536f412bfadbb75510096b5c8c5d7bf3828bf180c22297d514458f6 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-397cf675a7f2196f"></a>[`Inno.Assets.AssetSourceKind.Directory = 1`](../../src/content/assets/Inno.Assets/AssetSourceKind.cs#L16) | A source directory with persistent identity but no runtime artifact. | — | — |
| <a id="api-233240842d0066f6"></a>[`Inno.Assets.AssetSourceKind.File = 0`](../../src/content/assets/Inno.Assets/AssetSourceKind.cs#L11) | A regular imported source file. | — | — |
| <a id="api-7d8b07fce6230a93"></a>[`public enum Inno.Assets.AssetSourceKind : int`](../../src/content/assets/Inno.Assets/AssetSourceKind.cs#L6) | Identifies the durable source role recorded by an asset metadata sidecar. | — | — |

### `Inno.Assets.BinaryAsset`

<!-- api: T:Inno.Assets.BinaryAsset 3ca86c40d1c60a79c734d9f091bd11d943bc23786e9d9ca74ec586f23e0011db -->
<!-- api: P:Inno.Assets.BinaryAsset.byteLength 91b193dcc37747b4b94f806120aabdb8086e19b06e7d84ad6698168cb38fdfac -->
<!-- api: M:Inno.Assets.BinaryAsset.#ctor ec3a4d2677fb027871467b7ebd3a06921c6c859317e721d145432486ce443729 -->
<!-- api: M:Inno.Assets.BinaryAsset.#ctor(System.Int32) 7da2cbf1b7ea44d348c9031e7a38e27a47e3caa393fcc9ecd3d1d7e31ccc5e31 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-93817e5e47b5f4ea"></a>[`public Inno.Assets.BinaryAsset.BinaryAsset()`](../../src/content/assets/Inno.Assets/Types/BinaryAsset.cs#L22) | Creates an empty binary asset descriptor. | — | — |
| <a id="api-6e3bdc5bb673c9d4"></a>[`public Inno.Assets.BinaryAsset.BinaryAsset(int byteLength)`](../../src/content/assets/Inno.Assets/Types/BinaryAsset.cs#L32) | Creates a binary asset descriptor with a byte length.<br>`byteLength`：Payload length in bytes. | — | — |
| <a id="api-1618349b96b8f904"></a>[`public int Inno.Assets.BinaryAsset.byteLength { get; private set; }`](../../src/content/assets/Inno.Assets/Types/BinaryAsset.cs#L17) | Gets the imported payload length in bytes. | — | — |
| <a id="api-1de82d2035da3df7"></a>[`public sealed class Inno.Assets.BinaryAsset : Inno.Assets.AssetObject`](../../src/content/assets/Inno.Assets/Types/BinaryAsset.cs#L11) | Describes an imported opaque binary payload. | — | — |

### `Inno.Assets.IAssetLookup`

<!-- api: T:Inno.Assets.IAssetLookup 44f6df68c7f2ff620f1000ec2e18f6b0a68b0a96b02efc1adc8ce252b9200ab0 -->
<!-- api: M:Inno.Assets.IAssetLookup.Load``1(Inno.Assets.AssetPath) 3a554bfc29d5e72870639118e3ae1d9d827f8ebfc77128c74c2becbf28d7b0fb -->
<!-- api: M:Inno.Assets.IAssetLookup.Load``1(System.Guid) 78ac70fdc119109b19a966a6308c92348fb8cf1a6bf4987484e9ca98eaa92b54 -->
<!-- api: M:Inno.Assets.IAssetLookup.TryLoad``1(Inno.Assets.AssetPath,``0@) c2867ea083b239fde22dc21038b86424c38266faec3bc169829674e67048c8a5 -->
<!-- api: M:Inno.Assets.IAssetLookup.TryLoad``1(System.Guid,``0@) 97aa701a3021fe18ebc2f3d5f4bb90ffd8693b52c39d3590f89e91da4acbe746 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-0e22932d8991131b"></a>[`TAsset Inno.Assets.IAssetLookup.Load<TAsset>(Inno.Assets.AssetPath path) where TAsset : Inno.Assets.AssetObject`](../../src/content/assets/Inno.Assets/Runtime/IAssetLookup.cs#L25) | Loads the canonical asset at a logical catalog path.<br>返回：The canonical compatible asset owned by this lookup.<br>`path`：The mount-qualified logical catalog path. | — | `System.InvalidOperationException`：Thrown when the asset does not exist or has an incompatible concrete type. |
| <a id="api-2fdfc82fed4e04b5"></a>[`TAsset Inno.Assets.IAssetLookup.Load<TAsset>(System.Guid persistentId) where TAsset : Inno.Assets.AssetObject`](../../src/content/assets/Inno.Assets/Runtime/IAssetLookup.cs#L43) | Loads the canonical asset with a persistent identity.<br>返回：The canonical compatible asset owned by this lookup.<br>`persistentId`：The non-empty persistent asset identity. | — | `System.InvalidOperationException`：Thrown when the asset does not exist or has an incompatible concrete type. |
| <a id="api-4174cacbefa3332e"></a>[`bool Inno.Assets.IAssetLookup.TryLoad<TAsset>(Inno.Assets.AssetPath path, out TAsset? asset) where TAsset : Inno.Assets.AssetObject`](../../src/content/assets/Inno.Assets/Runtime/IAssetLookup.cs#L62) | Tries to load the canonical asset at a logical catalog path.<br>返回：true when the lookup contains a compatible asset; otherwise, false.<br>`path`：The mount-qualified logical catalog path.<br>`asset`：Receives the canonical compatible asset when available. | — | — |
| <a id="api-19cb0269e8269f37"></a>[`bool Inno.Assets.IAssetLookup.TryLoad<TAsset>(System.Guid persistentId, out TAsset? asset) where TAsset : Inno.Assets.AssetObject`](../../src/content/assets/Inno.Assets/Runtime/IAssetLookup.cs#L84) | Tries to load the canonical asset with a persistent identity.<br>返回：true when the lookup contains a compatible asset; otherwise, false.<br>`persistentId`：The non-empty persistent asset identity.<br>`asset`：Receives the canonical compatible asset when available. | — | — |
| <a id="api-8de5d384bb898514"></a>[`public interface Inno.Assets.IAssetLookup`](../../src/content/assets/Inno.Assets/Runtime/IAssetLookup.cs#L8) | Defines the read-only asset lookup boundary shared by authoring and deployed runtime databases. | — | — |

### `Inno.Assets.IAssetPropertyStateResolver`

<!-- api: T:Inno.Assets.IAssetPropertyStateResolver 0de2d1b37d62972ba66470c2881ecc838282d366d85ffb4b496326a3524772a6 -->
<!-- api: M:Inno.Assets.IAssetPropertyStateResolver.RestoreProperties``1(System.Guid,System.Byte[],``0) 8b598a4c8fac7568191d0e03244897f1942f3e7725d23938377b040742e02d57 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-7d78fdc9f2c6891b"></a>[`void Inno.Assets.IAssetPropertyStateResolver.RestoreProperties<TValue>(System.Guid stableTypeId, byte[] propertyData, TValue target) where TValue : class, Inno.Core.Serialization.ISerializable`](../../src/content/assets/Inno.Assets/Serialization/IAssetPropertyStateResolver.cs#L26) | Restores a stable typed property payload into a caller-owned current-generation value.<br>`stableTypeId`：Stable identity captured with the properties.<br>`propertyData`：Native property bytes.<br>`target`：Detached destination. Referenced assets resolve in this owner. | — | — |
| <a id="api-7c484e70885ce01c"></a>[`public interface Inno.Assets.IAssetPropertyStateResolver`](../../src/content/assets/Inno.Assets/Serialization/IAssetPropertyStateResolver.cs#L9) | Restores nested neutral asset properties with the owning database's converter generation and references. | — | — |

### `Inno.Assets.IAssetReferenceResolver`

<!-- api: T:Inno.Assets.IAssetReferenceResolver a8a757a866328c886e772e76756fd4a599b54de9c40a2dbce47ccf1c82e7c527 -->
<!-- api: M:Inno.Assets.IAssetReferenceResolver.Resolve(System.Guid,System.Guid,System.String,System.Type,System.String) bc2cc118bbd52adf04fd6887910decfaf651b69d9b2dd33478d895e7a2dc17e1 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e90e7b853c27aa77"></a>[`Inno.Assets.AssetObject Inno.Assets.IAssetReferenceResolver.Resolve(System.Guid persistentId, System.Guid stableTypeId, string lastKnownPath, System.Type expectedType, string propertyPath)`](../../src/content/assets/Inno.Assets/Serialization/IAssetReferenceResolver.cs#L113) | Resolves one serialized asset reference to the canonical object owned by this resolver.<br>返回：The canonical compatible asset owned by this resolver's generation.<br>`persistentId`：The persistent asset identity encoded in the reference.<br>`stableTypeId`：The stable concrete asset type identity encoded in the reference.<br>`lastKnownPath`：The optional logical path retained for diagnostics and authoring recovery.<br>`expectedType`：The declared property type that the resolved object must satisfy.<br>`propertyPath`：The serialization property path used when reporting resolution failures. | — | `System.InvalidOperationException`：Thrown when the reference cannot be resolved to a compatible asset. |
| <a id="api-93723d32e186b90c"></a>[`public interface Inno.Assets.IAssetReferenceResolver : Inno.References.IReferenceResolver`](../../src/content/assets/Inno.Assets/Serialization/IAssetReferenceResolver.cs#L13) | Resolves persistent asset references against one isolated asset database generation. | — | — |

### `Inno.Assets.IAssetResidency`

<!-- api: T:Inno.Assets.IAssetResidency 1efec412aff5cb0e98bdb8ac672336cd51c4d94e2bf6ef3cb8d93c88b83c950b -->
<!-- api: M:Inno.Assets.IAssetResidency.AcquireAsync``1(Inno.Assets.AssetPath,System.Threading.CancellationToken) 8dff615e8f26cd05db285fa77825254a5e709bde85c808fae4ef15ee442b8f70 -->
<!-- api: M:Inno.Assets.IAssetResidency.AcquireAsync``1(System.Guid,System.Threading.CancellationToken) ee7299313179d3ac25a6092ecd0d2c22951a722b7c0de43866680f536f3986f5 -->
<!-- api: M:Inno.Assets.IAssetResidency.AcquireArtifact(System.Guid,System.String) 4418cd2e0d5f60db04e853faf1766b84823a24fda041ac14be7afa5210df3e1d -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-de997f98057c0f37"></a>[`Inno.Assets.ArtifactLease Inno.Assets.IAssetResidency.AcquireArtifact(System.Guid persistentId, string outputName)`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L71) | Acquires one verified immutable artifact output.<br>返回：A lease that keeps the artifact generation available to the caller.<br>`persistentId`：Persistent identity of the artifact owner.<br>`outputName`：Stable artifact output name. | — | `System.InvalidOperationException`：Thrown when the requested artifact does not exist or fails verification. |
| <a id="api-da1022d7c26ca2b2"></a>[`System.Threading.Tasks.ValueTask<Inno.Assets.AssetLease<TAsset>> Inno.Assets.IAssetResidency.AcquireAsync<TAsset>(Inno.Assets.AssetPath path, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) where TAsset : Inno.Assets.AssetObject`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L29) | Acquires a canonical asset by logical path and keeps it resident until the lease is released.<br>返回：A lease that owns residency for the canonical asset.<br>`path`：Mount-qualified logical asset path.<br>`cancellationToken`：Cancellation for this caller's asynchronous wait. | — | — |
| <a id="api-9448870a4f155298"></a>[`System.Threading.Tasks.ValueTask<Inno.Assets.AssetLease<TAsset>> Inno.Assets.IAssetResidency.AcquireAsync<TAsset>(System.Guid persistentId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken)) where TAsset : Inno.Assets.AssetObject`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L50) | Acquires a canonical asset by persistent identity and keeps it resident until release.<br>返回：A lease that owns residency for the canonical asset.<br>`persistentId`：Non-empty persistent asset identity.<br>`cancellationToken`：Cancellation for this caller's asynchronous wait. | — | — |
| <a id="api-6c902a02635d98f6"></a>[`public interface Inno.Assets.IAssetResidency`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L12) | Exposes explicit asynchronous asset and artifact residency ownership. | — | — |

### `Inno.Assets.RetentionScope`

<!-- api: T:Inno.Assets.RetentionScope 747ede8ceb753f30c9f5a0190391df697df12542c0fee6b7c97d2fb2a16797d1 -->
<!-- api: M:Inno.Assets.RetentionScope.Retain``1(``0) 20c7aa5759acb918d989c1a550b0c6918c225d3cc70f9cd99568af0af30b8a3b -->
<!-- api: M:Inno.Assets.RetentionScope.Dispose e635172a052b59b42fdc9cf421386a67f075324dcc4f40439ac590b72ef22516 -->
<!-- api: M:Inno.Assets.RetentionScope.#ctor 711a77eb41c44506d7fb742a1a62d554aa5293288847e860d82e31dc9baa0d33 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-3f03fe344ab75fbd"></a>[`public Inno.Assets.RetentionScope.RetentionScope()`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L253) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-ed2a35c4b60a26a6"></a>[`public void Inno.Assets.RetentionScope.Dispose()`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L287) | Releases all retained leases in reverse acquisition order. | — | `System.AggregateException`：Thrown after every lease is attempted when one or more releases fail.<br>`Inno.Core.Execution.RetirementPendingException`：A lease is not retired. The current and remaining dependencies are retained for retry. |
| <a id="api-36b85e7c02331dfb"></a>[`public TLease Inno.Assets.RetentionScope.Retain<TLease>(TLease lease) where TLease : System.IDisposable`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L272) | Transfers one lease into this scope.<br>返回：The same lease for convenient initialization.<br>`lease`：Lease whose disposal ownership transfers to this scope. | — | `System.InvalidOperationException`：Retirement has begun and the scope cannot accept another lease. |
| <a id="api-fd5df7a5fba4e47d"></a>[`public sealed class Inno.Assets.RetentionScope : System.IDisposable`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L253) | Owns multiple asset, artifact, or subsystem leases as one reverse-order lifetime. | — | — |

### `Inno.Assets.TextAsset`

<!-- api: T:Inno.Assets.TextAsset 3ff9cf10510913005e7c6e89c7b2da69db7021ad215977864e24e52ebf767595 -->
<!-- api: P:Inno.Assets.TextAsset.content 7199ffc876527d9a6bbd4c2dc734d2fac5a9d8b38822322525e77fd5a20257a6 -->
<!-- api: P:Inno.Assets.TextAsset.languageHint 0c45c5eed77c1dd48d74387b90553ea082de91bf0117d87480e5decfe004a5e5 -->
<!-- api: M:Inno.Assets.TextAsset.#ctor 57f53149e5eb756ad47ae536c588554bef75d89884681f4e4d825b557a66c7ad -->
<!-- api: M:Inno.Assets.TextAsset.#ctor(System.String,System.String) 1c7d2536352a4ed18f4914778c9379ead2ef3143b0f7089784d95f05e2e0d680 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-44978b593ce207b4"></a>[`public Inno.Assets.TextAsset.TextAsset()`](../../src/content/assets/Inno.Assets/Types/TextAsset.cs#L28) | Creates an empty text asset. | — | — |
| <a id="api-3a66fc71f9e579a9"></a>[`public Inno.Assets.TextAsset.TextAsset(string content, string languageHint = "plain")`](../../src/content/assets/Inno.Assets/Types/TextAsset.cs#L41) | Creates a text asset with content and an optional language hint.<br>`content`：Decoded text content.<br>`languageHint`：Language or format hint. | — | — |
| <a id="api-cf052ee8254a5587"></a>[`public string Inno.Assets.TextAsset.content { get; private set; }`](../../src/content/assets/Inno.Assets/Types/TextAsset.cs#L17) | Gets the decoded text content. | — | — |
| <a id="api-c93d9d431e87eacd"></a>[`public string Inno.Assets.TextAsset.languageHint { get; private set; }`](../../src/content/assets/Inno.Assets/Types/TextAsset.cs#L23) | Gets the language or format hint associated with the text. | — | — |
| <a id="api-3b1065bf3cc86507"></a>[`public sealed class Inno.Assets.TextAsset : Inno.Assets.AssetObject`](../../src/content/assets/Inno.Assets/Types/TextAsset.cs#L11) | Stores decoded text content and its language hint. | — | — |

## 项目依赖

- [Inno.Core.IO](../core/Inno.Core.IO.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Content](Inno.Content.md)：公开引用边界由实际签名核对。
- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：公开引用边界由实际签名核对。
- [Inno.Core.Identity](../core/Inno.Core.Identity.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：公开引用边界由实际签名核对。
- [Inno.References](../references/Inno.References.md)：公开引用边界由实际签名核对。
- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Runtime.Contracts](../runtime/Inno.Runtime.Contracts.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。

## Artifact 与部署摘要补充契约

<!-- api: T:Inno.Assets.ArtifactRetention df9fd74ca19ca1b016ed70b45437c2e8b8ece5d399cade4f384049117696ba98 -->
<!-- api: M:Inno.Assets.ArtifactRetention.Retain(Inno.Assets.AssetArtifactInfo,System.Func{System.IO.Stream}) 9f175c1b7375ac9ed8a7dda0faf877f09fdc7f93b0d8f8a20642d3cbad6026d4 -->
<!-- api: M:Inno.Assets.ArtifactRetention.GetRetainedKeys bfede757ee9c168be015e6d8531f18a532c1aeed3eb4aa14dc3e623c28a86c32 -->
<!-- api: M:Inno.Assets.ArtifactRetention.#ctor b2cff036b53114c5d8f2f4b52591a4c1971dc928fde648580660f81743ad2074 -->
<!-- api: T:Inno.Assets.AssetArtifactInfo f891b9d9035cd59d9f2a14c2fc81466af6213f03ba2bcd260be6da64cddc8858 -->
<!-- api: M:Inno.Assets.AssetArtifactInfo.#ctor(Inno.Assets.AssetArtifactKey,System.String,System.String,System.Int64) f244d7bb3b19d4f99120c599f5c524448747213be3aec1bc1e277daf371c5e55 -->
<!-- api: P:Inno.Assets.AssetArtifactInfo.key e6104b05bb331b7988945d8d0bb7f43aa0c2042ef9203e8d2394d668f05c7463 -->
<!-- api: P:Inno.Assets.AssetArtifactInfo.outputName bbc7ab0439c7ef25bca97a14c706e56036f04d89564ca137abe930faa639928e -->
<!-- api: P:Inno.Assets.AssetArtifactInfo.contentHash e16bf3550ef41028f206047b1222705c18a69eaf36a0eb13c352b72a393bcce2 -->
<!-- api: P:Inno.Assets.AssetArtifactInfo.length 10225b234ff845c41f6c8a6d5bbd5b18e291539319269b2cb9da33540a5c969b -->
<!-- api: T:Inno.Assets.AssetArtifactKey f878c7fa0cb2dbbbe747779b03ede47565921fcf347c9ff8887b60a8ee7020ea -->
<!-- api: M:Inno.Assets.AssetArtifactKey.#ctor(System.String) 87394484e00919302b85ed5cb058fc112db2a8891cefae8d624d7f58be992a87 -->
<!-- api: P:Inno.Assets.AssetArtifactKey.empty f8bb2dc6c91d13d8e122bc1297d8341cf382ec5affb0ea1d599388caae6685c8 -->
<!-- api: P:Inno.Assets.AssetArtifactKey.value 9469930a3855eb9df166a69169fa0cc914a410d8897405aa4eaa5f12ffc95897 -->
<!-- api: P:Inno.Assets.AssetArtifactKey.isEmpty 1aea23c42c5b48ad17fc3491315ecdee9b6a6bc1273e5cccaf2cd855e584a26b -->
<!-- api: M:Inno.Assets.AssetArtifactKey.Equals(Inno.Assets.AssetArtifactKey) 0f4962c1f671b9a72803c3169d8c22d5848f7ee3f58c1f89bd2606c7c9d1b2bc -->
<!-- api: M:Inno.Assets.AssetArtifactKey.Equals(System.Object) cb46d92d4d0552368d5399102398c3a81747c470dfe0e40a3deb6eba3344c8ba -->
<!-- api: M:Inno.Assets.AssetArtifactKey.GetHashCode dc593257341756743706f7630027a3ec5d6d63327f67fbb9616230cfd83d8e79 -->
<!-- api: M:Inno.Assets.AssetArtifactKey.ToString 3bd50a27e50f505f6745c5417f96180631d90f3fbf76b8ba13458e5af2600400 -->
<!-- api: M:Inno.Assets.AssetArtifactKey.op_Equality(Inno.Assets.AssetArtifactKey,Inno.Assets.AssetArtifactKey) 1c013e9e4859aed57834d04d200cc0fc7a40dc190a2f097fd7d0626dcfa1aaa2 -->
<!-- api: M:Inno.Assets.AssetArtifactKey.op_Inequality(Inno.Assets.AssetArtifactKey,Inno.Assets.AssetArtifactKey) fa64d20fe2efb9d9992aa94ecc7229c920072dada561c0360a5487d99e284602 -->
<!-- api: T:Inno.Assets.AssetRuntimeContentInfo ff0043fa539173adb658d1fa5b2fce3b534ff8942da00c8c1c14dbb06e9bd9eb -->
<!-- api: M:Inno.Assets.AssetRuntimeContentInfo.#ctor(System.Collections.Generic.IReadOnlyList{Inno.Assets.AssetSourceId},System.Int32,System.Int32,System.Int64) 94d352d46f22487bb7be5097c82738a3d6dabd7093e9572f663ac0ceeabfb2dd -->
<!-- api: P:Inno.Assets.AssetRuntimeContentInfo.sources f1dd6d6f5b13962592efadd76adf6466301b7dc1e17aae802e1496f739a951ae -->
<!-- api: P:Inno.Assets.AssetRuntimeContentInfo.assetCount eae9617e53528a7a55c7b53d4e92e0471e611e1f7ff7954aa2ec126e8df62535 -->
<!-- api: P:Inno.Assets.AssetRuntimeContentInfo.artifactBundleCount f83afc437a2d9c72b289359802dfff387d62e2f049bfd8ceffba829a5b93ea76 -->
<!-- api: P:Inno.Assets.AssetRuntimeContentInfo.totalBytes 16a8059bd4f95f332ea935cab84b7d9d1174086573279fef4d57ee4f4ff1b703 -->
<!-- api: T:Inno.Assets.IAssetArtifactLookup 6c36d9a7cd7068717bd7d6dd5a5c61206d3c6b56ac058f9df0fcd97d62cec929 -->
<!-- api: M:Inno.Assets.IAssetArtifactLookup.AcquireArtifact(System.Guid,System.String) a91a063d5cf6153332465a6d1841dd394b27f24a247091bb694ee43d24268d28 -->
<!-- api: M:Inno.Assets.IAssetArtifactLookup.TryGetArtifact(System.Guid,System.String,Inno.Assets.AssetArtifactInfo@) 9a3fbc9fa8feb96c25e20a302250b26309530cb4681ce6328dd251f6b22da22c -->

| 声明 | 行为与结果 | 所有权 / 线程 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-9f6e7162e0b7c353"></a>[`public sealed class Inno.Assets.ArtifactRetention : Inno.Assets.AssetResidencyProvider`](../../src/content/assets/Inno.Assets/Artifacts/ArtifactRetention.cs#L12) | 独立管理 immutable artifact key 的显式保留计数。 | 它不替代资产对象驻留，也不承担物理缓存收集。 | Retain 的输入由具体 provider 验证。 |
| <a id="api-600087bc7ab023ec"></a>[`public Inno.Assets.ArtifactLease Inno.Assets.ArtifactRetention.Retain(Inno.Assets.AssetArtifactInfo artifact, System.Func<System.IO.Stream> openRead)`](../../src/content/assets/Inno.Assets/Artifacts/ArtifactRetention.cs#L29) | 为已验证的输出增加一次保留，返回调用者拥有的 ArtifactLease；OpenRead 再建立独立 stream pin。 | 直到全部 lease 和流释放，artifact 才能退休；计数在同步边界内维护。 | metadata/openRead 为空会失败；provider 必须协调校验、acquisition 与收集。 |
| <a id="api-223892c5bb244960"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Assets.AssetArtifactKey> Inno.Assets.ArtifactRetention.GetRetainedKeys()`](../../src/content/assets/Inno.Assets/Artifacts/ArtifactRetention.cs#L55) | 返回不可修改的 artifact key 快照，用于保护显式保留的输出。 | Collection owner 必须协调收集与新增 acquisition；快照本身不延长 lease。 | 不能仅凭旧快照认定之后没有新 reader。 |
| <a id="api-f92ce462c8b557ce"></a>[`public Inno.Assets.ArtifactRetention.ArtifactRetention()`](../../src/content/assets/Inno.Assets/Artifacts/ArtifactRetention.cs#L12) | 独立管理 immutable artifact key 的显式保留计数。 | 它不替代资产对象驻留，也不承担物理缓存收集。 | Retain 的输入由具体 provider 验证。 |
| <a id="api-a83da64c5ca875fe"></a>[`public sealed class Inno.Assets.AssetArtifactInfo`](../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactInfo.cs#L6) | 保存 bundle key、逻辑 outputName、contentHash 与长度的只读描述。 | 不取得读取保护；实际消费通过 ArtifactLease。 | 构造描述不替代 provider 的内容完整性验证。 |
| <a id="api-df41d16040083810"></a>[`public Inno.Assets.AssetArtifactInfo.AssetArtifactInfo(Inno.Assets.AssetArtifactKey key, string outputName, string contentHash, long length)`](../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactInfo.cs#L23) | 规范化为大写 SHA-256 十六进制值；空白表示未指定。 | 纯值身份，不保存物理路径或 live object。 | 非空值必须为 64 位十六进制。 |
| <a id="api-4728bb31d1e00a9e"></a>[`public Inno.Assets.AssetArtifactKey Inno.Assets.AssetArtifactInfo.key { get; }`](../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactInfo.cs#L38) | 保存 bundle key、逻辑 outputName、contentHash 与长度的只读描述。 | 不取得读取保护；实际消费通过 ArtifactLease。 | 构造描述不替代 provider 的内容完整性验证。 |
| <a id="api-71856a582ec7d21e"></a>[`public string Inno.Assets.AssetArtifactInfo.outputName { get; }`](../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactInfo.cs#L43) | 保存 bundle key、逻辑 outputName、contentHash 与长度的只读描述。 | 不取得读取保护；实际消费通过 ArtifactLease。 | 构造描述不替代 provider 的内容完整性验证。 |
| <a id="api-4ce3defb362615cb"></a>[`public string Inno.Assets.AssetArtifactInfo.contentHash { get; }`](../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactInfo.cs#L48) | 保存 bundle key、逻辑 outputName、contentHash 与长度的只读描述。 | 不取得读取保护；实际消费通过 ArtifactLease。 | 构造描述不替代 provider 的内容完整性验证。 |
| <a id="api-d010df966d2b5637"></a>[`public long Inno.Assets.AssetArtifactInfo.length { get; }`](../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactInfo.cs#L53) | 保存 bundle key、逻辑 outputName、contentHash 与长度的只读描述。 | 不取得读取保护；实际消费通过 ArtifactLease。 | 构造描述不替代 provider 的内容完整性验证。 |
| <a id="api-fe5cc4e99d598cc3"></a>[`public readonly struct Inno.Assets.AssetArtifactKey : System.IEquatable<Inno.Assets.AssetArtifactKey>`](../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactKey.cs#L8) | 返回规范化内容身份文本；未指定时返回空字符串。 | 纯值结果，不暴露缓存路径。 | 不返回 null。 |
| <a id="api-e1ba5872e21f6082"></a>[`public Inno.Assets.AssetArtifactKey.AssetArtifactKey(string value)`](../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactKey.cs#L21) | 规范化为大写 SHA-256 十六进制值；空白表示未指定。 | 纯值身份，不保存物理路径或 live object。 | 非空值必须为 64 位十六进制。 |
| <a id="api-54e4875f36e9ffef"></a>[`public static Inno.Assets.AssetArtifactKey Inno.Assets.AssetArtifactKey.empty { get; }`](../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactKey.cs#L40) | 返回规范化内容身份文本；未指定时返回空字符串。 | 纯值结果，不暴露缓存路径。 | 不返回 null。 |
| <a id="api-aa52a0de9c914085"></a>[`public string Inno.Assets.AssetArtifactKey.value { get; }`](../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactKey.cs#L45) | 返回规范化内容身份文本；未指定时返回空字符串。 | 纯值结果，不暴露缓存路径。 | 不返回 null。 |
| <a id="api-4ceda1d86fbb8f14"></a>[`public bool Inno.Assets.AssetArtifactKey.isEmpty { get; }`](../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactKey.cs#L50) | 返回规范化内容身份文本；未指定时返回空字符串。 | 纯值结果，不暴露缓存路径。 | 不返回 null。 |
| <a id="api-351260a331f0fb99"></a>[`public bool Inno.Assets.AssetArtifactKey.Equals(Inno.Assets.AssetArtifactKey other)`](../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactKey.cs#L61) | 按规范化 value 比较内容身份；默认空值与显式空值具有相同语义。 | 纯值比较；没有资源 ownership transfer。 | 不是 object persistent ID 或 Stable Type ID。 |
| <a id="api-1fa3f24fb64301d8"></a>[`public override bool Inno.Assets.AssetArtifactKey.Equals(object? obj)`](../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactKey.cs#L72) | 按规范化 value 比较内容身份；默认空值与显式空值具有相同语义。 | 纯值比较；没有资源 ownership transfer。 | 不是 object persistent ID 或 Stable Type ID。 |
| <a id="api-e83f3e8603620d59"></a>[`public override int Inno.Assets.AssetArtifactKey.GetHashCode()`](../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactKey.cs#L80) | 返回与规范化 value 相等关系一致的哈希码。 | 仅用于当前集合；不作为持久内容指纹。 | 默认空值同样可用。 |
| <a id="api-a89ab6f97e8aee25"></a>[`public override string Inno.Assets.AssetArtifactKey.ToString()`](../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactKey.cs#L88) | 返回规范化内容身份文本；未指定时返回空字符串。 | 纯值结果，不暴露缓存路径。 | 不返回 null。 |
| <a id="api-4f90ccc31919e78f"></a>[`public static bool Inno.Assets.AssetArtifactKey.operator ==(Inno.Assets.AssetArtifactKey left, Inno.Assets.AssetArtifactKey right)`](../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactKey.cs#L102) | 按规范化 value 比较内容身份；默认空值与显式空值具有相同语义。 | 纯值比较；没有资源 ownership transfer。 | 不是 object persistent ID 或 Stable Type ID。 |
| <a id="api-5590d16970ab1d2b"></a>[`public static bool Inno.Assets.AssetArtifactKey.operator !=(Inno.Assets.AssetArtifactKey left, Inno.Assets.AssetArtifactKey right)`](../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactKey.cs#L119) | 按规范化 value 比较内容身份；默认空值与显式空值具有相同语义。 | 纯值比较；没有资源 ownership transfer。 | 不是 object persistent ID 或 Stable Type ID。 |
| <a id="api-117e7edb67f44d06"></a>[`public sealed class Inno.Assets.AssetRuntimeContentInfo`](../../src/content/assets/Inno.Assets/Artifacts/AssetRuntimeContentInfo.cs#L8) | 冻结运行部署的 source 列表，并记录资产数、唯一 bundle 数及总字节。 | source 值在构造时复制；原列表之后的变化不影响摘要。 | sources 为空会抛 ArgumentNullException；计数由部署 owner 提供。 |
| <a id="api-8c89bf52e46ab353"></a>[`public Inno.Assets.AssetRuntimeContentInfo.AssetRuntimeContentInfo(System.Collections.Generic.IReadOnlyList<Inno.Assets.AssetSourceId> sources, int assetCount, int artifactBundleCount, long totalBytes)`](../../src/content/assets/Inno.Assets/Artifacts/AssetRuntimeContentInfo.cs#L25) | 冻结运行部署的 source 列表，并记录资产数、唯一 bundle 数及总字节。 | source 值在构造时复制；原列表之后的变化不影响摘要。 | sources 为空会抛 ArgumentNullException；计数由部署 owner 提供。 |
| <a id="api-dc93b21fde35e700"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Assets.AssetSourceId> Inno.Assets.AssetRuntimeContentInfo.sources { get; }`](../../src/content/assets/Inno.Assets/Artifacts/AssetRuntimeContentInfo.cs#L40) | 已复制的只读 source identity 列表。 | 摘要不保留内容流、文件路径或运行对象。 | 不作为缓存有效性证明。 |
| <a id="api-1e06400babc22119"></a>[`public int Inno.Assets.AssetRuntimeContentInfo.assetCount { get; }`](../../src/content/assets/Inno.Assets/Artifacts/AssetRuntimeContentInfo.cs#L45) | 运行 catalog 的资产数量。 | 摘要不保留内容流、文件路径或运行对象。 | 不作为缓存有效性证明。 |
| <a id="api-647ea0d4be067039"></a>[`public int Inno.Assets.AssetRuntimeContentInfo.artifactBundleCount { get; }`](../../src/content/assets/Inno.Assets/Artifacts/AssetRuntimeContentInfo.cs#L50) | 唯一内容 bundle 数量。 | 摘要不保留内容流、文件路径或运行对象。 | 不作为缓存有效性证明。 |
| <a id="api-aaa246fbba2d66a3"></a>[`public long Inno.Assets.AssetRuntimeContentInfo.totalBytes { get; }`](../../src/content/assets/Inno.Assets/Artifacts/AssetRuntimeContentInfo.cs#L55) | 已复制至运行内容根的总字节。 | 摘要不保留内容流、文件路径或运行对象。 | 不作为缓存有效性证明。 |
| <a id="api-66ee3b34aa4f6278"></a>[`public interface Inno.Assets.IAssetArtifactLookup`](../../src/content/assets/Inno.Assets/Artifacts/IAssetArtifactLookup.cs#L8) | 提供路径中立的命名 artifact 查询与获取。 | 具体 provider 负责内容来源、完整性和收集协调。 | 不暴露物理部署位置。 |
| <a id="api-227b1da82fdf7c44"></a>[`Inno.Assets.ArtifactLease Inno.Assets.IAssetArtifactLookup.AcquireArtifact(System.Guid persistentId, string outputName)`](../../src/content/assets/Inno.Assets/Artifacts/IAssetArtifactLookup.cs#L25) | 返回 caller-owned immutable artifact lease；消费者在读取/复制结束后释放。 | OpenRead 的流独立 pin，可在基础 lease 释放后继续读取，直到流自身关闭。 | 输出不可用或无效会抛 InvalidOperationException。 |
| <a id="api-fb4a51439ed7f490"></a>[`bool Inno.Assets.IAssetArtifactLookup.TryGetArtifact(System.Guid persistentId, string outputName, out Inno.Assets.AssetArtifactInfo? artifact)`](../../src/content/assets/Inno.Assets/Artifacts/IAssetArtifactLookup.cs#L45) | 保存 bundle key、逻辑 outputName、contentHash 与长度的只读描述。 | 不取得读取保护；实际消费通过 ArtifactLease。 | 构造描述不替代 provider 的内容完整性验证。 |
