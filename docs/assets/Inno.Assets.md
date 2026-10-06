# Inno.Assets

[分类索引](README.md) · [Wiki 首页](../README.md) · [本轮整改计划](../architecture/ARCHITECTURE_CLEANUP_PLAN_2026_10_06.md)

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

以下仅列出当前程序集自己声明的 public/protected 契约；继承成员遵循所属基类页面。internal/private 实现不作为稳定公开 API。签名依据当前源码语义模型生成，行为、参数、异常与所有权说明同时以对应英文 XML 为准。

### `Inno.Assets.ArtifactLease`

| 当前声明 | 行为 |
| --- | --- |
| [`void Inno.Assets.ArtifactLease.Dispose()`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L242) | Releases artifact ownership, retaining metadata and callback while the provider reports Pending. |
| [`System.IO.Stream Inno.Assets.ArtifactLease.OpenRead()`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L197) | Opens a read-only stream over the retained immutable artifact. |
| [`byte[] Inno.Assets.ArtifactLease.ReadAllBytes()`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L215) | Reads one indexed payload through this lease's independently pinned stream. |
| [`Inno.Assets.AssetArtifactInfo Inno.Assets.ArtifactLease.info`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L189) | Gets verified metadata for the retained artifact. |
| [`Inno.Assets.ArtifactLease`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L171) | Keeps one verified immutable artifact generation available for an explicit lifetime. |

### `Inno.Assets.ArtifactRetention`

| 当前声明 | 行为 |
| --- | --- |
| [`System.Collections.Generic.IReadOnlyList<Inno.Assets.AssetArtifactKey> Inno.Assets.ArtifactRetention.GetRetainedKeys()`](../../src/content/assets/Inno.Assets/Artifacts/ArtifactRetention.cs#L55) | Captures the keys a collector must preserve regardless of catalog reachability. |
| [`Inno.Assets.ArtifactLease Inno.Assets.ArtifactRetention.Retain(Inno.Assets.AssetArtifactInfo artifact, System.Func<System.IO.Stream> openRead)`](../../src/content/assets/Inno.Assets/Artifacts/ArtifactRetention.cs#L29) | Retains a verified output before its owner permits cache collection. |
| [`Inno.Assets.ArtifactRetention`](../../src/content/assets/Inno.Assets/Artifacts/ArtifactRetention.cs#L12) | Counts explicit leases over immutable artifact keys independently of asset object residency. |

### `Inno.Assets.AssetArtifactInfo`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.AssetArtifactInfo.AssetArtifactInfo(Inno.Assets.AssetArtifactKey key, string outputName, string contentHash, long length)`](../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactInfo.cs#L23) | Creates an artifact output descriptor. |
| [`string Inno.Assets.AssetArtifactInfo.contentHash`](../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactInfo.cs#L48) | Gets the output content fingerprint. |
| [`Inno.Assets.AssetArtifactKey Inno.Assets.AssetArtifactInfo.key`](../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactInfo.cs#L38) | Gets the owning artifact bundle key. |
| [`long Inno.Assets.AssetArtifactInfo.length`](../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactInfo.cs#L53) | Gets the output length in bytes. |
| [`string Inno.Assets.AssetArtifactInfo.outputName`](../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactInfo.cs#L43) | Gets the stable output name. |
| [`Inno.Assets.AssetArtifactInfo`](../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactInfo.cs#L6) | Describes one named output in an immutable artifact bundle. |

### `Inno.Assets.AssetArtifactKey`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.AssetArtifactKey.AssetArtifactKey(string value)`](../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactKey.cs#L21) | Creates an artifact key from a hexadecimal content fingerprint. |
| [`bool Inno.Assets.AssetArtifactKey.Equals(Inno.Assets.AssetArtifactKey other)`](../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactKey.cs#L61) | Determines whether this instance and the supplied value represent the same logical state. |
| [`override bool Inno.Assets.AssetArtifactKey.Equals(object? obj)`](../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactKey.cs#L72) | Determines whether this instance and the supplied value represent the same logical state. |
| [`override int Inno.Assets.AssetArtifactKey.GetHashCode()`](../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactKey.cs#L80) | Computes a hash code from the fields that participate in logical equality. |
| [`override string Inno.Assets.AssetArtifactKey.ToString()`](../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactKey.cs#L88) | Formats this value as a human-readable representation. |
| [`static bool Inno.Assets.AssetArtifactKey.operator ==(Inno.Assets.AssetArtifactKey left, Inno.Assets.AssetArtifactKey right)`](../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactKey.cs#L102) | Determines whether two artifact keys are equal. |
| [`static bool Inno.Assets.AssetArtifactKey.operator !=(Inno.Assets.AssetArtifactKey left, Inno.Assets.AssetArtifactKey right)`](../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactKey.cs#L119) | Determines whether two artifact keys differ. |
| [`static Inno.Assets.AssetArtifactKey Inno.Assets.AssetArtifactKey.empty`](../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactKey.cs#L40) | Gets an empty artifact key. |
| [`bool Inno.Assets.AssetArtifactKey.isEmpty`](../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactKey.cs#L50) | Gets whether the key is empty. |
| [`string Inno.Assets.AssetArtifactKey.value`](../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactKey.cs#L45) | Gets the normalized SHA-256 hexadecimal fingerprint, or an empty value for an unassigned key. |
| [`Inno.Assets.AssetArtifactKey`](../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactKey.cs#L8) | Identifies one immutable content-addressed artifact bundle. |

### `Inno.Assets.AssetChange`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.AssetChange.AssetChange(Inno.Assets.AssetChangeKind kind, System.Guid persistentId, Inno.Assets.AssetPath assetPath, Inno.Assets.AssetPath? previousAssetPath = null)`](../../src/content/assets/Inno.Assets/Changes/AssetChange.cs#L25) | Creates an asset change descriptor. |
| [`Inno.Assets.AssetPath Inno.Assets.AssetChange.assetPath`](../../src/content/assets/Inno.Assets/Changes/AssetChange.cs#L50) | Gets the current isolated source path. |
| [`Inno.Assets.AssetChangeKind Inno.Assets.AssetChange.kind`](../../src/content/assets/Inno.Assets/Changes/AssetChange.cs#L40) | Gets the change kind. |
| [`System.Guid Inno.Assets.AssetChange.persistentId`](../../src/content/assets/Inno.Assets/Changes/AssetChange.cs#L45) | Gets the persistent identity affected by the change. |
| [`Inno.Assets.AssetPath? Inno.Assets.AssetChange.previousAssetPath`](../../src/content/assets/Inno.Assets/Changes/AssetChange.cs#L55) | Gets the previous isolated path for move operations. |
| [`Inno.Assets.AssetChange`](../../src/content/assets/Inno.Assets/Changes/AssetChange.cs#L8) | Describes one committed asset database change. |

### `Inno.Assets.AssetChangeKind`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.AssetChangeKind.Added`](../../src/content/assets/Inno.Assets/Changes/AssetChangeKind.cs#L11) | A source was added to the database. |
| [`Inno.Assets.AssetChangeKind.Missing`](../../src/content/assets/Inno.Assets/Changes/AssetChangeKind.cs#L31) | A source became unavailable while retaining its identity. |
| [`Inno.Assets.AssetChangeKind.Modified`](../../src/content/assets/Inno.Assets/Changes/AssetChangeKind.cs#L16) | An existing source or artifact changed. |
| [`Inno.Assets.AssetChangeKind.Moved`](../../src/content/assets/Inno.Assets/Changes/AssetChangeKind.cs#L21) | A source moved while retaining its persistent identity. |
| [`Inno.Assets.AssetChangeKind.Removed`](../../src/content/assets/Inno.Assets/Changes/AssetChangeKind.cs#L26) | A source and its persistent metadata were removed. |
| [`Inno.Assets.AssetChangeKind.Replaced`](../../src/content/assets/Inno.Assets/Changes/AssetChangeKind.cs#L36) | A canonical runtime object was replaced by an incompatible imported type. |
| [`Inno.Assets.AssetChangeKind.StatusChanged`](../../src/content/assets/Inno.Assets/Changes/AssetChangeKind.cs#L41) | An import status or diagnostic changed without replacing the source. |
| [`Inno.Assets.AssetChangeKind`](../../src/content/assets/Inno.Assets/Changes/AssetChangeKind.cs#L6) | Identifies a committed asset database change. |

### `Inno.Assets.AssetChangeSet`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.AssetChangeSet.AssetChangeSet(long revision, System.Collections.Generic.IReadOnlyList<Inno.Assets.AssetChange>? changes)`](../../src/content/assets/Inno.Assets/Changes/AssetChangeSet.cs#L20) | Creates a committed change set. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Assets.AssetChange> Inno.Assets.AssetChangeSet.changes`](../../src/content/assets/Inno.Assets/Changes/AssetChangeSet.cs#L36) | Gets the changes committed by this revision. |
| [`bool Inno.Assets.AssetChangeSet.isEmpty`](../../src/content/assets/Inno.Assets/Changes/AssetChangeSet.cs#L41) | Gets whether the change set contains no changes. |
| [`long Inno.Assets.AssetChangeSet.revision`](../../src/content/assets/Inno.Assets/Changes/AssetChangeSet.cs#L31) | Gets the monotonically increasing database revision. |
| [`Inno.Assets.AssetChangeSet`](../../src/content/assets/Inno.Assets/Changes/AssetChangeSet.cs#L9) | Contains one atomically committed asset database revision. |

### `Inno.Assets.AssetDatabase`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.AssetDatabase.AssetDatabase(Inno.Content.IRuntimeContentStore content, Inno.Core.Serialization.SerializationGeneration serialization, Inno.Extensibility.Types.TypeCacheSnapshot types, Inno.Core.Identity.IdentityAllocator identities, long residencyBudgetBytes = 9223372036854775807, long preparationBudgetBytes = 67108864)`](../../src/content/assets/Inno.Assets/Runtime/AssetDatabase.cs#L108) | Creates a read-only runtime asset database from one verified immutable content store. |
| [`Inno.Assets.ArtifactLease Inno.Assets.AssetDatabase.AcquireArtifact(System.Guid persistentId, string outputName)`](../../src/content/assets/Inno.Assets/Runtime/AssetDatabase.cs#L466) | Acquires one verified immutable artifact output for an explicit lifetime. |
| [`System.Threading.Tasks.ValueTask<Inno.Assets.AssetLease<TAsset>> Inno.Assets.AssetDatabase.AcquireAsync<TAsset>(Inno.Assets.AssetPath path, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/content/assets/Inno.Assets/Runtime/AssetDatabase.cs#L329) | Acquires a canonical runtime asset by path for an explicit residency lifetime. |
| [`System.Threading.Tasks.ValueTask<Inno.Assets.AssetLease<TAsset>> Inno.Assets.AssetDatabase.AcquireAsync<TAsset>(System.Guid persistentId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/content/assets/Inno.Assets/Runtime/AssetDatabase.cs#L360) | Acquires a canonical runtime asset by persistent identity for an explicit residency lifetime. |
| [`int Inno.Assets.AssetDatabase.CompletePendingLoads(int completionBudget = 8)`](../../src/content/assets/Inno.Assets/Runtime/AssetDatabase.Residency.cs#L70) | Publishes completed cold loads on the database owner thread without waiting for outstanding IO. |
| [`void Inno.Assets.AssetDatabase.Dispose()`](../../src/content/assets/Inno.Assets/Runtime/AssetDatabase.cs#L508) | Stops publication and releases canonical assets before dependency edges and the payload read gate. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Assets.AssetDependency> Inno.Assets.AssetDatabase.GetDependencies(Inno.Assets.AssetObject asset)`](../../src/content/assets/Inno.Assets/Runtime/AssetDatabase.cs#L394) | Gets the direct persistent dependencies declared by one loaded or cataloged asset. |
| [`TAsset Inno.Assets.AssetDatabase.Load<TAsset>(Inno.Assets.AssetPath path)`](../../src/content/assets/Inno.Assets/Runtime/AssetDatabase.cs#L175) | Loads the canonical asset at one catalog path. |
| [`TAsset Inno.Assets.AssetDatabase.Load<TAsset>(System.Guid persistentId)`](../../src/content/assets/Inno.Assets/Runtime/AssetDatabase.cs#L206) | Loads the canonical asset with one persistent identity. |
| [`void Inno.Assets.AssetDatabase.RestoreProperties<TValue>(System.Guid stableTypeId, byte[] propertyData, TValue target)`](../../src/content/assets/Inno.Assets/Runtime/AssetDatabase.cs#L64) | Restores serialized properties to the existing asset object. |
| [`int Inno.Assets.AssetDatabase.TrimToBudget()`](../../src/content/assets/Inno.Assets/Runtime/AssetDatabase.cs#L490) | Evicts least-recently-used unpinned assets until the configured payload budget is met. |
| [`bool Inno.Assets.AssetDatabase.TryGetArtifact(System.Guid persistentId, string outputName, out Inno.Assets.AssetArtifactInfo? artifact)`](../../src/content/assets/Inno.Assets/Runtime/AssetDatabase.cs#L428) | Tries to resolve one named immutable artifact output by persistent asset identity. |
| [`bool Inno.Assets.AssetDatabase.TryLoad<TAsset>(Inno.Assets.AssetPath path, out TAsset? asset)`](../../src/content/assets/Inno.Assets/Runtime/AssetDatabase.cs#L237) | Tries to load the canonical asset at one catalog path. |
| [`bool Inno.Assets.AssetDatabase.TryLoad<TAsset>(System.Guid persistentId, out TAsset? asset)`](../../src/content/assets/Inno.Assets/Runtime/AssetDatabase.cs#L276) | Tries to load the canonical asset with one persistent identity. |
| [`Inno.Assets.AssetPreparationStatistics Inno.Assets.AssetDatabase.preparationStatistics`](../../src/content/assets/Inno.Assets/Runtime/AssetDatabase.Residency.cs#L33) | Gets immutable cold-load admission, unique IO and peak reservation counters for this database lifetime. |
| [`long Inno.Assets.AssetDatabase.preparingBytes`](../../src/content/assets/Inno.Assets/Runtime/AssetDatabase.Residency.cs#L46) | Gets encoded bytes reserved by cold-load closures, including completed IO awaiting owner publication. |
| [`Inno.Assets.AssetResidencyStatistics Inno.Assets.AssetDatabase.residencyStatistics`](../../src/content/assets/Inno.Assets/Runtime/AssetDatabase.cs#L299) | Gets current materialized asset count, payload bytes, and the configured budget. |
| [`Inno.Assets.AssetDatabase`](../../src/content/assets/Inno.Assets/Runtime/AssetDatabase.cs#L24) | Loads canonical runtime assets exclusively from a verified catalog and content-addressed artifact bundles. |

### `Inno.Assets.AssetDependency`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.AssetDependency.AssetDependency(System.Guid persistentId, Inno.Extensibility.Types.TypeRef type, string lastKnownPath)`](../../src/content/assets/Inno.Assets/AssetDependency.cs#L28) | Creates an asset dependency descriptor. |
| [`bool Inno.Assets.AssetDependency.Equals(Inno.Assets.AssetDependency other)`](../../src/content/assets/Inno.Assets/AssetDependency.cs#L66) | Determines whether this instance and the supplied value represent the same logical state. |
| [`override bool Inno.Assets.AssetDependency.Equals(object? obj)`](../../src/content/assets/Inno.Assets/AssetDependency.cs#L77) | Determines whether this instance and the supplied value represent the same logical state. |
| [`override int Inno.Assets.AssetDependency.GetHashCode()`](../../src/content/assets/Inno.Assets/AssetDependency.cs#L85) | Computes a hash code from the fields that participate in logical equality. |
| [`static bool Inno.Assets.AssetDependency.operator ==(Inno.Assets.AssetDependency left, Inno.Assets.AssetDependency right)`](../../src/content/assets/Inno.Assets/AssetDependency.cs#L99) | Determines whether two descriptors refer to the same persistent asset. |
| [`static bool Inno.Assets.AssetDependency.operator !=(Inno.Assets.AssetDependency left, Inno.Assets.AssetDependency right)`](../../src/content/assets/Inno.Assets/AssetDependency.cs#L116) | Determines whether two descriptors refer to different persistent assets. |
| [`string Inno.Assets.AssetDependency.lastKnownPath`](../../src/content/assets/Inno.Assets/AssetDependency.cs#L55) | Gets the last known source-relative path. |
| [`System.Guid Inno.Assets.AssetDependency.persistentId`](../../src/content/assets/Inno.Assets/AssetDependency.cs#L44) | Gets the persistent identity of the referenced asset. |
| [`Inno.Extensibility.Types.TypeRef Inno.Assets.AssetDependency.type`](../../src/content/assets/Inno.Assets/AssetDependency.cs#L49) | Gets the reload-safe identity of the expected asset type. |
| [`Inno.Assets.AssetDependency`](../../src/content/assets/Inno.Assets/AssetDependency.cs#L11) | Describes a persistent direct dependency on another asset. |

### `Inno.Assets.AssetDependencyCollection`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.AssetDependencyCollection.AssetDependencyCollection()`](../../src/content/assets/Inno.Assets/Serialization/AssetDependencyCollection.cs#L24) | Creates a dependency collector that preserves last-known source paths in serialized references. |
| [`Inno.Assets.AssetDependencyCollection.AssetDependencyCollection(bool includeLastKnownPaths)`](../../src/content/assets/Inno.Assets/Serialization/AssetDependencyCollection.cs#L36) | Creates a dependency collector with an explicit location-hint policy. |
| [`void Inno.Assets.AssetDependencyCollection.Add(Inno.Assets.AssetDependency dependency)`](../../src/content/assets/Inno.Assets/Serialization/AssetDependencyCollection.cs#L79) | Includes a dependency already captured inside a neutral nested property payload. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Assets.AssetDependency> Inno.Assets.AssetDependencyCollection.dependencies`](../../src/content/assets/Inno.Assets/Serialization/AssetDependencyCollection.cs#L49) | Gets the collected dependencies in deterministic persistent-identity order. |
| [`bool Inno.Assets.AssetDependencyCollection.includeLastKnownPaths`](../../src/content/assets/Inno.Assets/Serialization/AssetDependencyCollection.cs#L44) | Gets whether serialized asset references and collected dependencies retain source path hints. |
| [`Inno.Assets.AssetDependencyCollection`](../../src/content/assets/Inno.Assets/Serialization/AssetDependencyCollection.cs#L17) | Collects direct asset dependencies encountered by a serialization operation. |

### `Inno.Assets.AssetExecutionContext`

| 当前声明 | 行为 |
| --- | --- |
| [`static System.IDisposable Inno.Assets.AssetExecutionContext.EnterScope(Inno.Assets.IAssetLookup assets)`](../../src/content/assets/Inno.Assets/Runtime/AssetExecutionContext.cs#L38) | Binds an asset lookup until the returned strict last-in-first-out scope is disposed. |
| [`static Inno.Assets.IAssetLookup Inno.Assets.AssetExecutionContext.current`](../../src/content/assets/Inno.Assets/Runtime/AssetExecutionContext.cs#L24) | Gets the asset lookup bound to the current asynchronous execution context. |
| [`Inno.Assets.AssetExecutionContext`](../../src/content/assets/Inno.Assets/Runtime/AssetExecutionContext.cs#L14) | Binds one host-owned asset lookup to the current asynchronous script execution context. |

### `Inno.Assets.AssetImportStatus`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.AssetImportStatus.Conflict`](../../src/content/assets/Inno.Assets/AssetImportStatus.cs#L36) | The source cannot be reconciled without resolving an identity conflict. |
| [`Inno.Assets.AssetImportStatus.Failed`](../../src/content/assets/Inno.Assets/AssetImportStatus.cs#L26) | The latest import failed and diagnostics are available. |
| [`Inno.Assets.AssetImportStatus.Imported`](../../src/content/assets/Inno.Assets/AssetImportStatus.cs#L21) | A valid artifact is committed for the source. |
| [`Inno.Assets.AssetImportStatus.Missing`](../../src/content/assets/Inno.Assets/AssetImportStatus.cs#L31) | The source is unavailable while its persistent identity is retained. |
| [`Inno.Assets.AssetImportStatus.Pending`](../../src/content/assets/Inno.Assets/AssetImportStatus.cs#L16) | The source is waiting for import or commit. |
| [`Inno.Assets.AssetImportStatus.Unsupported`](../../src/content/assets/Inno.Assets/AssetImportStatus.cs#L11) | No importer currently accepts the source. |
| [`Inno.Assets.AssetImportStatus`](../../src/content/assets/Inno.Assets/AssetImportStatus.cs#L6) | Identifies the current source and import state of an asset. |

### `Inno.Assets.AssetInfo`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.AssetInfo.AssetInfo(System.Guid persistentId, Inno.Assets.AssetPath assetPath, Inno.Assets.AssetSourceKind sourceKind, Inno.Assets.AssetImportStatus status, string importerId, System.Guid stableAssetTypeId, Inno.Assets.AssetArtifactKey artifactKey, Inno.Assets.AssetArtifactKey lastSuccessfulArtifactKey, System.Collections.Generic.IReadOnlyList<string>? diagnostics = null)`](../../src/content/assets/Inno.Assets/AssetInfo.cs#L44) | Creates an asset information snapshot. |
| [`Inno.Assets.AssetArtifactKey Inno.Assets.AssetInfo.artifactKey`](../../src/content/assets/Inno.Assets/AssetInfo.cs#L101) | Gets the current committed artifact key. |
| [`Inno.Assets.AssetPath Inno.Assets.AssetInfo.assetPath`](../../src/content/assets/Inno.Assets/AssetInfo.cs#L76) | Gets the current isolated source path. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Assets.AssetInfo.diagnostics`](../../src/content/assets/Inno.Assets/AssetInfo.cs#L111) | Gets diagnostics produced by the latest reconciliation or import. |
| [`string Inno.Assets.AssetInfo.importerId`](../../src/content/assets/Inno.Assets/AssetInfo.cs#L91) | Gets the selected importer identifier. |
| [`Inno.Assets.AssetArtifactKey Inno.Assets.AssetInfo.lastSuccessfulArtifactKey`](../../src/content/assets/Inno.Assets/AssetInfo.cs#L106) | Gets the most recent successfully imported artifact key. |
| [`System.Guid Inno.Assets.AssetInfo.persistentId`](../../src/content/assets/Inno.Assets/AssetInfo.cs#L71) | Gets the persistent asset identity. |
| [`Inno.Assets.AssetSourceKind Inno.Assets.AssetInfo.sourceKind`](../../src/content/assets/Inno.Assets/AssetInfo.cs#L81) | Gets whether the catalog entry represents a file or directory source. |
| [`System.Guid Inno.Assets.AssetInfo.stableAssetTypeId`](../../src/content/assets/Inno.Assets/AssetInfo.cs#L96) | Gets the stable imported asset type identity. |
| [`Inno.Assets.AssetImportStatus Inno.Assets.AssetInfo.status`](../../src/content/assets/Inno.Assets/AssetInfo.cs#L86) | Gets the current import status. |
| [`Inno.Assets.AssetInfo`](../../src/content/assets/Inno.Assets/AssetInfo.cs#L9) | Provides an immutable public snapshot of one cataloged asset. |

### `Inno.Assets.AssetLease<TAsset>`

| 当前声明 | 行为 |
| --- | --- |
| [`void Inno.Assets.AssetLease<TAsset>.Dispose()`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L165) | Releases this caller's residency ownership, retaining its value and callback while the provider reports Pending. |
| [`TAsset Inno.Assets.AssetLease<TAsset>.asset`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L150) | Gets the retained canonical asset while this lease is active. |
| [`Inno.Assets.AssetLease<TAsset>`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L132) | Keeps one canonical asset generation resident for an explicit lifetime. |

### `Inno.Assets.AssetObject`

| 当前声明 | 行为 |
| --- | --- |
| [`virtual void Inno.Assets.AssetObject.OnRuntimePayloadChanged(System.ReadOnlyMemory<byte> previousPayload, System.ReadOnlyMemory<byte> currentPayload)`](../../src/content/assets/Inno.Assets/AssetObject.cs#L89) | Called after a new runtime payload has been committed to this instance. |
| [`virtual void Inno.Assets.AssetObject.OnUnloading()`](../../src/content/assets/Inno.Assets/AssetObject.cs#L105) | Releases the runtime resources owned by this asset without clearing its payload before quiescence. |
| [`void Inno.Assets.AssetObject.RestoreProperties<TValue>(System.Guid stableTypeId, byte[] propertyData, TValue target)`](../../src/content/assets/Inno.Assets/AssetObject.cs#L69) | Restores detached extension settings using this asset's actual owner and converter generation. |
| [`Inno.Assets.AssetPath Inno.Assets.AssetObject.assetPath`](../../src/content/assets/Inno.Assets/AssetObject.cs#L26) | Gets the isolated source path associated with this asset. |
| [`long Inno.Assets.AssetObject.contentVersion`](../../src/content/assets/Inno.Assets/AssetObject.cs#L44) | Gets the version of the currently committed runtime content. |
| [`bool Inno.Assets.AssetObject.isMissing`](../../src/content/assets/Inno.Assets/AssetObject.cs#L39) | Gets whether this instance represents an unavailable persistent asset. |
| [`string Inno.Assets.AssetObject.name`](../../src/content/assets/Inno.Assets/AssetObject.cs#L32) | Gets a display name derived from the source-local path. |
| [`System.ReadOnlyMemory<byte> Inno.Assets.AssetObject.runtimePayload`](../../src/content/assets/Inno.Assets/AssetObject.cs#L49) | Gets the runtime artifact payload produced by the importer. |
| [`Inno.Assets.AssetObject`](../../src/content/assets/Inno.Assets/AssetObject.cs#L13) | Provides the common runtime identity and payload contract for imported assets. |

### `Inno.Assets.AssetPath`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.AssetPath.AssetPath(Inno.Assets.AssetSourceId source, string localPath)`](../../src/content/assets/Inno.Assets/AssetPath.cs#L74) | Creates an asset path. |
| [`static Inno.Assets.AssetPath Inno.Assets.AssetPath.Parse(string value)`](../../src/content/assets/Inno.Assets/AssetPath.cs#L119) | Parses a canonical path, treating an unqualified value as project-owned. |
| [`static Inno.Assets.AssetPath Inno.Assets.AssetPath.Project(string localPath)`](../../src/content/assets/Inno.Assets/AssetPath.cs#L108) | Creates a path in the writable project source. |
| [`override string Inno.Assets.AssetPath.ToString()`](../../src/content/assets/Inno.Assets/AssetPath.cs#L134) | Formats this value as a human-readable representation. |
| [`bool Inno.Assets.AssetPath.isValid`](../../src/content/assets/Inno.Assets/AssetPath.cs#L97) | Gets whether the source and local path are valid. |
| [`string Inno.Assets.AssetPath.localPath`](../../src/content/assets/Inno.Assets/AssetPath.cs#L92) | Gets or sets the source-local path. |
| [`Inno.Assets.AssetSourceId Inno.Assets.AssetPath.source`](../../src/content/assets/Inno.Assets/AssetPath.cs#L87) | Gets or sets the owning source mount. |
| [`Inno.Assets.AssetPath`](../../src/content/assets/Inno.Assets/AssetPath.cs#L61) | Addresses one asset without conflating isolated source mounts. |

### `Inno.Assets.AssetPreparationStatistics`

| 当前声明 | 行为 |
| --- | --- |
| [`long Inno.Assets.AssetPreparationStatistics.payloadReadsStarted`](../../src/content/assets/Inno.Assets/Runtime/AssetPreparationStatistics.cs#L51) | Gets the cumulative number of unique physical payload reads started. |
| [`int Inno.Assets.AssetPreparationStatistics.peakPendingRequests`](../../src/content/assets/Inno.Assets/Runtime/AssetPreparationStatistics.cs#L36) | Gets the largest simultaneous admitted request count during this database lifetime. |
| [`long Inno.Assets.AssetPreparationStatistics.peakReservedBytes`](../../src/content/assets/Inno.Assets/Runtime/AssetPreparationStatistics.cs#L66) | Gets the largest simultaneous unique encoded-byte reservation. |
| [`int Inno.Assets.AssetPreparationStatistics.pendingPayloads`](../../src/content/assets/Inno.Assets/Runtime/AssetPreparationStatistics.cs#L46) | Gets unique payload reservations still retained by one or more cold-load roots. |
| [`int Inno.Assets.AssetPreparationStatistics.pendingRequests`](../../src/content/assets/Inno.Assets/Runtime/AssetPreparationStatistics.cs#L31) | Gets requests still awaiting owner-thread completion, including canceled callers not yet drained. |
| [`long Inno.Assets.AssetPreparationStatistics.rejectedRequests`](../../src/content/assets/Inno.Assets/Runtime/AssetPreparationStatistics.cs#L41) | Gets requests rejected by the pending-count or encoded-byte budget. |
| [`long Inno.Assets.AssetPreparationStatistics.reservedBytes`](../../src/content/assets/Inno.Assets/Runtime/AssetPreparationStatistics.cs#L61) | Gets unique encoded bytes retained until all requesting roots complete, fail or cancel. |
| [`long Inno.Assets.AssetPreparationStatistics.sharedPayloadReads`](../../src/content/assets/Inno.Assets/Runtime/AssetPreparationStatistics.cs#L56) | Gets payload reads reused by distinct roots with overlapping dependency closures. |
| [`Inno.Assets.AssetPreparationStatistics`](../../src/content/assets/Inno.Assets/Runtime/AssetPreparationStatistics.cs#L6) | Reports owner-thread cold-load admission and unique encoded IO reservations without retaining assets or tasks. |

### `Inno.Assets.AssetPropertySnapshot`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.AssetPropertySnapshot.AssetPropertySnapshot(System.Guid stableTypeId, System.ReadOnlySpan<byte> data, System.Collections.Generic.IEnumerable<Inno.Assets.AssetDependency> dependencies)`](../../src/content/assets/Inno.Assets/Serialization/AssetPropertySnapshot.cs#L25) | Freezes a complete owner-produced property value. |
| [`System.ReadOnlyMemory<byte> Inno.Assets.AssetPropertySnapshot.data`](../../src/content/assets/Inno.Assets/Serialization/AssetPropertySnapshot.cs#L44) | Gets the frozen native property bytes. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Assets.AssetDependency> Inno.Assets.AssetPropertySnapshot.dependencies`](../../src/content/assets/Inno.Assets/Serialization/AssetPropertySnapshot.cs#L48) | Gets the frozen direct dependency declarations. |
| [`System.Guid Inno.Assets.AssetPropertySnapshot.stableTypeId`](../../src/content/assets/Inno.Assets/Serialization/AssetPropertySnapshot.cs#L40) | Gets the persistent settings type identity. |
| [`Inno.Assets.AssetPropertySnapshot`](../../src/content/assets/Inno.Assets/Serialization/AssetPropertySnapshot.cs#L10) | Contains native properties and automatically captured asset dependencies without retaining their owner or typed object. |

### `Inno.Assets.AssetReferenceInfo`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.AssetReferenceInfo.AssetReferenceInfo(System.Guid persistentId, Inno.Assets.AssetPath assetPath, long contentVersion, bool isLoaded, bool? lastSweepReachability, System.Collections.Generic.IReadOnlyList<Inno.Assets.AssetReferenceLocation> references)`](../../src/content/assets/Inno.Assets/AssetReferenceInfo.cs#L37) | Freezes one diagnostic view of the engine-known references to an asset. |
| [`Inno.Assets.AssetPath Inno.Assets.AssetReferenceInfo.assetPath`](../../src/content/assets/Inno.Assets/AssetReferenceInfo.cs#L62) | Gets the current mount-qualified asset path. |
| [`long Inno.Assets.AssetReferenceInfo.contentVersion`](../../src/content/assets/Inno.Assets/AssetReferenceInfo.cs#L67) | Gets the current runtime content version. |
| [`bool Inno.Assets.AssetReferenceInfo.isLoaded`](../../src/content/assets/Inno.Assets/AssetReferenceInfo.cs#L72) | Gets whether the asset is currently held by the loader cache. |
| [`int Inno.Assets.AssetReferenceInfo.knownReferenceCount`](../../src/content/assets/Inno.Assets/AssetReferenceInfo.cs#L83) | Gets the number of engine-known reference locations. |
| [`bool? Inno.Assets.AssetReferenceInfo.lastSweepReachability`](../../src/content/assets/Inno.Assets/AssetReferenceInfo.cs#L78) | Gets whether an external managed reference was found by the previous unused-asset sweep, or when no sweep has inspected this asset. |
| [`System.Guid Inno.Assets.AssetReferenceInfo.persistentId`](../../src/content/assets/Inno.Assets/AssetReferenceInfo.cs#L57) | Gets the persistent asset identity. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Assets.AssetReferenceLocation> Inno.Assets.AssetReferenceInfo.references`](../../src/content/assets/Inno.Assets/AssetReferenceInfo.cs#L88) | Gets the engine-known reference locations. |
| [`Inno.Assets.AssetReferenceInfo`](../../src/content/assets/Inno.Assets/AssetReferenceInfo.cs#L14) | Provides a stable diagnostic snapshot of references known to the asset pipeline. |

### `Inno.Assets.AssetReferenceKind`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.AssetReferenceKind.AssetDependency`](../../src/content/assets/Inno.Assets/AssetReferenceKind.cs#L12) | The reference originates from another asset's runtime dependencies. |
| [`Inno.Assets.AssetReferenceKind.Editor`](../../src/content/assets/Inno.Assets/AssetReferenceKind.cs#L28) | The reference originates from an editor-only view or selection. |
| [`Inno.Assets.AssetReferenceKind.PrefabSource`](../../src/content/assets/Inno.Assets/AssetReferenceKind.cs#L24) | The reference originates from a prefab source. |
| [`Inno.Assets.AssetReferenceKind.RuntimeSubsystem`](../../src/content/assets/Inno.Assets/AssetReferenceKind.cs#L32) | The reference originates from a runtime subsystem. |
| [`Inno.Assets.AssetReferenceKind.SceneResource`](../../src/content/assets/Inno.Assets/AssetReferenceKind.cs#L20) | The reference originates from a scene resource. |
| [`Inno.Assets.AssetReferenceKind.SerializedProperty`](../../src/content/assets/Inno.Assets/AssetReferenceKind.cs#L16) | The reference originates from a serialized property. |
| [`Inno.Assets.AssetReferenceKind`](../../src/content/assets/Inno.Assets/AssetReferenceKind.cs#L7) | Identifies a reference source known to the asset pipeline. |

### `Inno.Assets.AssetReferenceLocation`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.AssetReferenceLocation.AssetReferenceLocation(Inno.Assets.AssetReferenceKind kind, System.Guid ownerId, string ownerName, string propertyPath)`](../../src/content/assets/Inno.Assets/AssetReferenceLocation.cs#L25) | Describes a neutral, persistent location without retaining its live owner. |
| [`Inno.Assets.AssetReferenceKind Inno.Assets.AssetReferenceLocation.kind`](../../src/content/assets/Inno.Assets/AssetReferenceLocation.cs#L40) | Gets the category of the known reference. |
| [`System.Guid Inno.Assets.AssetReferenceLocation.ownerId`](../../src/content/assets/Inno.Assets/AssetReferenceLocation.cs#L45) | Gets the persistent identity of the known owner, when available. |
| [`string Inno.Assets.AssetReferenceLocation.ownerName`](../../src/content/assets/Inno.Assets/AssetReferenceLocation.cs#L50) | Gets the display name of the known owner. |
| [`string Inno.Assets.AssetReferenceLocation.propertyPath`](../../src/content/assets/Inno.Assets/AssetReferenceLocation.cs#L55) | Gets the serialized or subsystem-relative property path. |
| [`Inno.Assets.AssetReferenceLocation`](../../src/content/assets/Inno.Assets/AssetReferenceLocation.cs#L8) | Describes one engine-known reference location for an asset. |

### `Inno.Assets.AssetReferenceProtocol`

| 当前声明 | 行为 |
| --- | --- |
| [`static Inno.References.ReferenceKindId Inno.Assets.AssetReferenceProtocol.id`](../../src/content/assets/Inno.Assets/Serialization/AssetReferenceProtocol.cs#L13) | Gets the stable asset reference kind registered in shared reference catalogs. |
| [`Inno.Assets.AssetReferenceProtocol`](../../src/content/assets/Inno.Assets/Serialization/AssetReferenceProtocol.cs#L8) | Defines the open cross-domain protocol used to resolve persistent asset identities. |

### `Inno.Assets.AssetResidencyProvider`

| 当前声明 | 行为 |
| --- | --- |
| [`static Inno.Assets.ArtifactLease Inno.Assets.AssetResidencyProvider.CreateArtifactLease(Inno.Assets.AssetArtifactInfo artifact, System.Func<System.IO.Stream> openRead, System.Action release)`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L119) | Creates one immutable artifact residency lease. |
| [`static Inno.Assets.AssetLease<TAsset> Inno.Assets.AssetResidencyProvider.CreateAssetLease<TAsset>(TAsset asset, System.Action release)`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L97) | Creates one strongly typed asset residency lease. |
| [`Inno.Assets.AssetResidencyProvider`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L80) | Provides controlled lease construction to concrete asset residency implementations. |

### `Inno.Assets.AssetResidencyStatistics`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.AssetResidencyStatistics.AssetResidencyStatistics(int residentAssetCount, long residentBytes, long budgetBytes)`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L307) | Creates one immutable residency snapshot. |
| [`long Inno.Assets.AssetResidencyStatistics.budgetBytes`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L330) | Gets the configured runtime payload budget. |
| [`int Inno.Assets.AssetResidencyStatistics.residentAssetCount`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L320) | Gets the number of materialized canonical assets. |
| [`long Inno.Assets.AssetResidencyStatistics.residentBytes`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L325) | Gets retained runtime payload bytes. |
| [`Inno.Assets.AssetResidencyStatistics`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L293) | Reports current explicit runtime asset residency accounting. |

### `Inno.Assets.AssetRuntimeContentInfo`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.AssetRuntimeContentInfo.AssetRuntimeContentInfo(System.Collections.Generic.IReadOnlyList<Inno.Assets.AssetSourceId> sources, int assetCount, int artifactBundleCount, long totalBytes)`](../../src/content/assets/Inno.Assets/Artifacts/AssetRuntimeContentInfo.cs#L25) | Creates an immutable runtime content summary. |
| [`int Inno.Assets.AssetRuntimeContentInfo.artifactBundleCount`](../../src/content/assets/Inno.Assets/Artifacts/AssetRuntimeContentInfo.cs#L50) | Gets the number of unique content-addressed bundles. |
| [`int Inno.Assets.AssetRuntimeContentInfo.assetCount`](../../src/content/assets/Inno.Assets/Artifacts/AssetRuntimeContentInfo.cs#L45) | Gets the number of runtime-scoped assets in the deployed catalog. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Assets.AssetSourceId> Inno.Assets.AssetRuntimeContentInfo.sources`](../../src/content/assets/Inno.Assets/Artifacts/AssetRuntimeContentInfo.cs#L40) | Gets the source identities represented by the deployed catalog. |
| [`long Inno.Assets.AssetRuntimeContentInfo.totalBytes`](../../src/content/assets/Inno.Assets/Artifacts/AssetRuntimeContentInfo.cs#L55) | Gets the total bytes copied into the runtime content root. |
| [`Inno.Assets.AssetRuntimeContentInfo`](../../src/content/assets/Inno.Assets/Artifacts/AssetRuntimeContentInfo.cs#L8) | Describes one deployed runtime-only asset content snapshot. |

### `Inno.Assets.AssetRuntimeOwner`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.AssetRuntimeOwner.AssetRuntimeOwner(Inno.Assets.IAssetPropertyStateResolver? properties = null)`](../../src/content/assets/Inno.Assets/Runtime/AssetRuntimeOwner.cs#L24) | Creates exclusive mutation authority with optional owner-bound extension state restoration. |
| [`string Inno.Assets.AssetRuntimeOwner.GetSourceHash(Inno.Assets.AssetObject asset)`](../../src/content/assets/Inno.Assets/Runtime/AssetRuntimeOwner.cs#L38) | Reads the source fingerprint of an asset claimed by this owner. |
| [`void Inno.Assets.AssetRuntimeOwner.Initialize(Inno.Assets.AssetObject asset, Inno.Assets.AssetPath assetPath, string sourceHash, System.ReadOnlyMemory<byte> payload, bool isMissing, long version)`](../../src/content/assets/Inno.Assets/Runtime/AssetRuntimeOwner.cs#L68) | Claims an unowned asset or commits new runtime state to an already owned instance. |
| [`void Inno.Assets.AssetRuntimeOwner.Release(Inno.Assets.AssetObject asset)`](../../src/content/assets/Inno.Assets/Runtime/AssetRuntimeOwner.cs#L112) | Releases an owned instance after quiescence, without relinquishing its mutation authority. |
| [`void Inno.Assets.AssetRuntimeOwner.UpdateAssetPath(Inno.Assets.AssetObject asset, Inno.Assets.AssetPath assetPath)`](../../src/content/assets/Inno.Assets/Runtime/AssetRuntimeOwner.cs#L92) | Updates source location without replacing committed runtime content. |
| [`Inno.Assets.AssetRuntimeOwner`](../../src/content/assets/Inno.Assets/Runtime/AssetRuntimeOwner.cs#L13) | Grants one database exclusive mutation authority over the asset instances it initializes. |

### `Inno.Assets.AssetSerializationContext`

| 当前声明 | 行为 |
| --- | --- |
| [`static Inno.Core.Serialization.SerializationContext Inno.Assets.AssetSerializationContext.Create(Inno.Assets.IAssetReferenceResolver references, Inno.Assets.AssetDependencyCollection? dependencies = null)`](../../src/content/assets/Inno.Assets/Serialization/AssetSerializationContext.cs#L27) | Creates a context containing the required asset resolver and an optional dependency collector. |
| [`Inno.Assets.AssetSerializationContext`](../../src/content/assets/Inno.Assets/Serialization/AssetSerializationContext.cs#L10) | Creates complete asset-aware converter contexts from an owner-provided reference generation. |

### `Inno.Assets.AssetSourceId`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.AssetSourceId.AssetSourceId(string value)`](../../src/content/assets/Inno.Assets/AssetPath.cs#L23) | Creates a globally stable source identifier. |
| [`override string Inno.Assets.AssetSourceId.ToString()`](../../src/content/assets/Inno.Assets/AssetPath.cs#L55) | Formats this value as a human-readable representation. |
| [`bool Inno.Assets.AssetSourceId.isValid`](../../src/content/assets/Inno.Assets/AssetPath.cs#L47) | Gets whether the source identity has a usable value. |
| [`static Inno.Assets.AssetSourceId Inno.Assets.AssetSourceId.project`](../../src/content/assets/Inno.Assets/AssetPath.cs#L15) | Gets the writable project source identifier. |
| [`string Inno.Assets.AssetSourceId.value`](../../src/content/assets/Inno.Assets/AssetPath.cs#L42) | Gets or sets the globally stable source value. |
| [`Inno.Assets.AssetSourceId`](../../src/content/assets/Inno.Assets/AssetPath.cs#L10) | Identifies one isolated asset source mount. |

### `Inno.Assets.AssetSourceKind`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.AssetSourceKind.Directory`](../../src/content/assets/Inno.Assets/AssetSourceKind.cs#L16) | A source directory with persistent identity but no runtime artifact. |
| [`Inno.Assets.AssetSourceKind.File`](../../src/content/assets/Inno.Assets/AssetSourceKind.cs#L11) | A regular imported source file. |
| [`Inno.Assets.AssetSourceKind`](../../src/content/assets/Inno.Assets/AssetSourceKind.cs#L6) | Identifies the durable source role recorded by an asset metadata sidecar. |

### `Inno.Assets.Assets`

| 当前声明 | 行为 |
| --- | --- |
| [`static Inno.Assets.ArtifactLease Inno.Assets.Assets.AcquireArtifact(System.Guid persistentId, string outputName)`](../../src/content/assets/Inno.Assets/Runtime/Assets.cs#L201) | Acquires one verified immutable artifact output for an explicit lifetime. |
| [`static System.Threading.Tasks.ValueTask<Inno.Assets.AssetLease<TAsset>> Inno.Assets.Assets.AcquireAsync<TAsset>(Inno.Assets.AssetPath path, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/content/assets/Inno.Assets/Runtime/Assets.cs#L160) | Acquires a canonical asset and explicitly retains it until the returned lease is disposed. |
| [`static System.Threading.Tasks.ValueTask<Inno.Assets.AssetLease<TAsset>> Inno.Assets.Assets.AcquireAsync<TAsset>(System.Guid persistentId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/content/assets/Inno.Assets/Runtime/Assets.cs#L182) | Acquires a canonical asset by persistent identity and explicitly retains it. |
| [`static TAsset Inno.Assets.Assets.Load<TAsset>(Inno.Assets.AssetPath path)`](../../src/content/assets/Inno.Assets/Runtime/Assets.cs#L70) | Loads the canonical asset at a logical catalog path in the current session. |
| [`static TAsset Inno.Assets.Assets.Load<TAsset>(System.Guid persistentId)`](../../src/content/assets/Inno.Assets/Runtime/Assets.cs#L89) | Loads the canonical asset with a persistent identity in the current session. |
| [`static Inno.Assets.AssetPath Inno.Assets.Assets.LocalPath(string localPath, string sourceFile = "")`](../../src/content/assets/Inno.Assets/Runtime/Assets.cs#L35) | Creates a path relative to the Asset source that owns the calling script. |
| [`static bool Inno.Assets.Assets.TryLoad<TAsset>(Inno.Assets.AssetPath path, out TAsset? asset)`](../../src/content/assets/Inno.Assets/Runtime/Assets.cs#L112) | Tries to load the canonical asset at a logical catalog path in the current session. |
| [`static bool Inno.Assets.Assets.TryLoad<TAsset>(System.Guid persistentId, out TAsset? asset)`](../../src/content/assets/Inno.Assets/Runtime/Assets.cs#L138) | Tries to load the canonical asset with a persistent identity in the current session. |
| [`Inno.Assets.Assets`](../../src/content/assets/Inno.Assets/Runtime/Assets.cs#L15) | Provides script-facing asset queries through the lookup bound to the current runtime session. |

### `Inno.Assets.BinaryAsset`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.BinaryAsset.BinaryAsset()`](../../src/content/assets/Inno.Assets/Types/BinaryAsset.cs#L22) | Creates an empty binary asset descriptor. |
| [`Inno.Assets.BinaryAsset.BinaryAsset(int byteLength)`](../../src/content/assets/Inno.Assets/Types/BinaryAsset.cs#L32) | Creates a binary asset descriptor with a byte length. |
| [`int Inno.Assets.BinaryAsset.byteLength`](../../src/content/assets/Inno.Assets/Types/BinaryAsset.cs#L16) | Gets the imported payload length in bytes. |
| [`Inno.Assets.BinaryAsset`](../../src/content/assets/Inno.Assets/Types/BinaryAsset.cs#L10) | Describes an imported opaque binary payload. |

### `Inno.Assets.IAssetArtifactLookup`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.ArtifactLease Inno.Assets.IAssetArtifactLookup.AcquireArtifact(System.Guid persistentId, string outputName)`](../../src/content/assets/Inno.Assets/Artifacts/IAssetArtifactLookup.cs#L25) | Retains one verified output so cache collection cannot remove an active generation. |
| [`bool Inno.Assets.IAssetArtifactLookup.TryGetArtifact(System.Guid persistentId, string outputName, out Inno.Assets.AssetArtifactInfo? artifact)`](../../src/content/assets/Inno.Assets/Artifacts/IAssetArtifactLookup.cs#L45) | Tries to resolve one named immutable artifact output by persistent asset identity. |
| [`Inno.Assets.IAssetArtifactLookup`](../../src/content/assets/Inno.Assets/Artifacts/IAssetArtifactLookup.cs#L8) | Resolves verified named outputs from immutable asset artifact bundles. |

### `Inno.Assets.IAssetLookup`

| 当前声明 | 行为 |
| --- | --- |
| [`TAsset Inno.Assets.IAssetLookup.Load<TAsset>(Inno.Assets.AssetPath path)`](../../src/content/assets/Inno.Assets/Runtime/IAssetLookup.cs#L25) | Loads the canonical asset at a logical catalog path. |
| [`TAsset Inno.Assets.IAssetLookup.Load<TAsset>(System.Guid persistentId)`](../../src/content/assets/Inno.Assets/Runtime/IAssetLookup.cs#L43) | Loads the canonical asset with a persistent identity. |
| [`bool Inno.Assets.IAssetLookup.TryLoad<TAsset>(Inno.Assets.AssetPath path, out TAsset? asset)`](../../src/content/assets/Inno.Assets/Runtime/IAssetLookup.cs#L62) | Tries to load the canonical asset at a logical catalog path. |
| [`bool Inno.Assets.IAssetLookup.TryLoad<TAsset>(System.Guid persistentId, out TAsset? asset)`](../../src/content/assets/Inno.Assets/Runtime/IAssetLookup.cs#L84) | Tries to load the canonical asset with a persistent identity. |
| [`Inno.Assets.IAssetLookup`](../../src/content/assets/Inno.Assets/Runtime/IAssetLookup.cs#L8) | Defines the read-only asset lookup boundary shared by authoring and deployed runtime databases. |

### `Inno.Assets.IAssetPropertyStateResolver`

| 当前声明 | 行为 |
| --- | --- |
| [`void Inno.Assets.IAssetPropertyStateResolver.RestoreProperties<TValue>(System.Guid stableTypeId, byte[] propertyData, TValue target)`](../../src/content/assets/Inno.Assets/Serialization/IAssetPropertyStateResolver.cs#L26) | Restores a stable typed property payload into a caller-owned current-generation value. |
| [`Inno.Assets.IAssetPropertyStateResolver`](../../src/content/assets/Inno.Assets/Serialization/IAssetPropertyStateResolver.cs#L9) | Restores nested neutral asset properties with the owning database's converter generation and references. |

### `Inno.Assets.IAssetReferenceResolver`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.AssetObject Inno.Assets.IAssetReferenceResolver.Resolve(System.Guid persistentId, System.Guid stableTypeId, string lastKnownPath, System.Type expectedType, string propertyPath)`](../../src/content/assets/Inno.Assets/Serialization/IAssetReferenceResolver.cs#L113) | Resolves one serialized asset reference to the canonical object owned by this resolver. |
| [`Inno.Assets.IAssetReferenceResolver`](../../src/content/assets/Inno.Assets/Serialization/IAssetReferenceResolver.cs#L13) | Resolves persistent asset references against one isolated asset database generation. |

### `Inno.Assets.IAssetResidency`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.ArtifactLease Inno.Assets.IAssetResidency.AcquireArtifact(System.Guid persistentId, string outputName)`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L71) | Acquires one verified immutable artifact output. |
| [`System.Threading.Tasks.ValueTask<Inno.Assets.AssetLease<TAsset>> Inno.Assets.IAssetResidency.AcquireAsync<TAsset>(Inno.Assets.AssetPath path, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L29) | Acquires a canonical asset by logical path and keeps it resident until the lease is released. |
| [`System.Threading.Tasks.ValueTask<Inno.Assets.AssetLease<TAsset>> Inno.Assets.IAssetResidency.AcquireAsync<TAsset>(System.Guid persistentId, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L50) | Acquires a canonical asset by persistent identity and keeps it resident until release. |
| [`Inno.Assets.IAssetResidency`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L12) | Exposes explicit asynchronous asset and artifact residency ownership. |

### `Inno.Assets.RetentionScope`

| 当前声明 | 行为 |
| --- | --- |
| [`void Inno.Assets.RetentionScope.Dispose()`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L287) | Releases all retained leases in reverse acquisition order. |
| [`TLease Inno.Assets.RetentionScope.Retain<TLease>(TLease lease)`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L272) | Transfers one lease into this scope. |
| [`Inno.Assets.RetentionScope`](../../src/content/assets/Inno.Assets/Runtime/AssetResidency.cs#L253) | Owns multiple asset, artifact, or subsystem leases as one reverse-order lifetime. |

### `Inno.Assets.TextAsset`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.TextAsset.TextAsset()`](../../src/content/assets/Inno.Assets/Types/TextAsset.cs#L28) | Creates an empty text asset. |
| [`Inno.Assets.TextAsset.TextAsset(string content, string languageHint = "plain")`](../../src/content/assets/Inno.Assets/Types/TextAsset.cs#L41) | Creates a text asset with content and an optional language hint. |
| [`string Inno.Assets.TextAsset.content`](../../src/content/assets/Inno.Assets/Types/TextAsset.cs#L16) | Gets the decoded text content. |
| [`string Inno.Assets.TextAsset.languageHint`](../../src/content/assets/Inno.Assets/Types/TextAsset.cs#L22) | Gets the language or format hint associated with the text. |
| [`Inno.Assets.TextAsset`](../../src/content/assets/Inno.Assets/Types/TextAsset.cs#L10) | Stores decoded text content and its language hint. |

## 项目依赖

- [Inno.Core.IO](../core/Inno.Core.IO.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Content](Inno.Content.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Core.Identity](../core/Inno.Core.Identity.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.References](../references/Inno.References.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Core.Execution](../core/Inno.Core.Execution.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Runtime.Contracts](../runtime/Inno.Runtime.Contracts.md)：项目引用；公开签名可见性由语义边界检查确认。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：项目引用；公开签名可见性由语义边界检查确认。

共同 MSBuild 注入的 analyzer 与编译规则属于构建依赖，完整有效项目图记录在本轮验收证据中。
