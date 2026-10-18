# Inno.Adapter.Content.FileSystem

[分类索引](README.md) · [Wiki 首页](../../README.md) · [内容契约](../../assets/Inno.Content.md)

## 职责与依赖

将一个借用的只读内容来源准备为完整验证的文件缓存。依赖 `Inno.Content`、`Inno.Storage` 与 `Inno.Core.IO`，不依赖 Build。宿主提供应用缓存根；运行消费者仍使用逻辑 key 与流。

## 公开 API

| 入口 | 语义 |
| --- | --- |
| `FileContentCacheOptions(applicationCacheRoot, lockTimeout)` | 宿主明确指定绝对缓存根与有限写入等待时间 |
| `FileContentPreparation.PrepareAsync(source, options, cancellationToken)` | 验证、修复、发布后交出 owned store；调用者在完成前保留 source |
| `FileContentStore.descriptor`、`index`、`Acquire(key)`、`Dispose()` | 固定一个 generation 的只读 store |
| `FileContentStore.retirementDiagnostics` | 发布后的旧代清理错误；不改变新代已经提交的事实 |

## 文件部署入口

`FileContentDeployment` 由平台组合传入明确的部署目录和缓存目录。它不猜测 OS 布局；`ReadDocumentAsync` 只接受单个文件名，读取上限为 16 MiB。`PrepareAsync` 先完整验证 Pack，再按 `StorageScope` 隔离文件缓存，返回调用方拥有的内容 store。该操作结束时关闭其临时 Pack store，返回的缓存 store 独立持有自己的读取保护。

## 缓存与所有权

```text
<applicationCacheRoot>/Content/<packHash>/
├─ install.lock
├─ current
├─ leases/<generation>.lock
├─ generations/<generation>/<logical content>
└─ staging/<candidate>/<logical content>
```

`install.lock` 是跨进程写入 owner；reader 持有 generation 的共享 lease。锁文件持续保留，释放不通过删除文件实现。目录、generation 和 staging 是内部实现，不是公开定位 API。

每次准备检查完整文件和目录集合、长度及实际 SHA-256；相同长度与 mtime 不能掩盖篡改。不以 `.complete` 判断有效。无效缓存先生成独立候选，验证后用 `AtomicDirectory.Publish` 发布，再以 `AtomicFile` 切换 `current`。旧读者固定旧代，最后一个读取流释放后才允许退休。

取消前台请求不会撤销已提交的有效新代。提交前失败清理 staging，并保留原指针。持续权限错误明确失败；清理失败单独报告并在后续准备重试。缺失旧代锁文件时无法证明它没有读者，保留旧目录并报告，不自行创建锁以推断安全。

```csharp
using System.Threading;
using System.Threading.Tasks;
using Inno.Content;
using Inno.Adapter.Content.FileSystem;

static async Task<FileContentStore> PrepareAsync(
    IRuntimeContentStore borrowedSource,
    string hostCacheRoot,
    CancellationToken cancellationToken
) {
    return await FileContentPreparation.PrepareAsync(borrowedSource,
        new FileContentCacheOptions(hostCacheRoot), cancellationToken);
}
```

## 验证与扩展

[文件缓存测试](../../../backends/FileSystem/tests/Inno.Adapter.Content.FileSystem.Tests/Inno.Adapter.Content.FileSystem.Tests.csproj) 覆盖损坏、额外文件／目录、缺失 lease、取消、写锁超时、发布后清理诊断和两个独立进程的 reader 退休。新平台可换用其他 store，或在自己的 composition 中选择此缓存实现；共享 Player 与 Assets 无需修改。

## 源码归属

当前唯一源码 owner：`backends/FileSystem/runtime/Inno.Adapter.Content.FileSystem/Inno.Adapter.Content.FileSystem.csproj`。共同领域与平台产品通过明确契约组合，本项目不提供旧目录兼容入口。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Adapter.Content.FileSystem.FileContentCacheOptions`

<!-- api: T:Inno.Adapter.Content.FileSystem.FileContentCacheOptions 67d618effac1241d6fc93fc3ae55c9ad720941f1c07e681a41658993587bcbdf -->
<!-- api: M:Inno.Adapter.Content.FileSystem.FileContentCacheOptions.#ctor(System.String,System.Nullable{System.TimeSpan}) c4e664ada21ad8497b352eab7272674d8ddb3337dcb59affd27d0a8935ce3dc0 -->
<!-- api: P:Inno.Adapter.Content.FileSystem.FileContentCacheOptions.applicationCacheRoot 77500d1b5d372360e2de8635a3ccfdabe7ed49ce2d25629e370c2644cca321c9 -->
<!-- api: P:Inno.Adapter.Content.FileSystem.FileContentCacheOptions.lockTimeout 37edfbb90d250059085e117e6e93f548a1d4cd8a6f48f564582956a2727d14e7 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-45dbeec9e885603c"></a>[`public Inno.Adapter.Content.FileSystem.FileContentCacheOptions.FileContentCacheOptions(string applicationCacheRoot, System.TimeSpan? lockTimeout = null)`](../../../backends/FileSystem/runtime/Inno.Adapter.Content.FileSystem/FileContentCacheOptions.cs#L23) | Selects an application-owned absolute cache root without imposing a product prefix.<br>`applicationCacheRoot`：The host-selected root under which this adapter owns only its Content subtree.<br>`lockTimeout`：The maximum wait for another preparing process; null selects thirty seconds. | — | `System.ArgumentException`：The root is empty or relative, or the timeout is negative. |
| <a id="api-9b8ec072b0f27d3e"></a>[`public string Inno.Adapter.Content.FileSystem.FileContentCacheOptions.applicationCacheRoot { get; }`](../../../backends/FileSystem/runtime/Inno.Adapter.Content.FileSystem/FileContentCacheOptions.cs#L39) | Gets the host-selected application cache location. | — | — |
| <a id="api-9876c372c6a573ac"></a>[`public System.TimeSpan Inno.Adapter.Content.FileSystem.FileContentCacheOptions.lockTimeout { get; }`](../../../backends/FileSystem/runtime/Inno.Adapter.Content.FileSystem/FileContentCacheOptions.cs#L44) | Gets the bounded exclusive preparation wait. | — | — |
| <a id="api-d2d0e800d7f67d7d"></a>[`public sealed class Inno.Adapter.Content.FileSystem.FileContentCacheOptions`](../../../backends/FileSystem/runtime/Inno.Adapter.Content.FileSystem/FileContentCacheOptions.cs#L9) | Freezes the host-selected cache location and bounded publication wait for immutable content. | — | — |

### `Inno.Adapter.Content.FileSystem.FileContentDeployment`

<!-- api: T:Inno.Adapter.Content.FileSystem.FileContentDeployment 18aba7a96749f34df499fd058efd4d92d59386df675e43092566febeefcb9aa4 -->
<!-- api: M:Inno.Adapter.Content.FileSystem.FileContentDeployment.#ctor(System.String,System.String) 366e4cf4d8d6de9e79d38ab72c5027ccd11a8df82f36ca69fd99caba87ed40fe -->
<!-- api: M:Inno.Adapter.Content.FileSystem.FileContentDeployment.PrepareAsync(Inno.Content.ContentPackDescriptor,Inno.Storage.StorageScope,Inno.Core.Serialization.SerializationGeneration,System.Threading.CancellationToken) a3a7ee753b02bc5be97eb596f317da666096db9df544af974c4ea50dd63045ef -->
<!-- api: M:Inno.Adapter.Content.FileSystem.FileContentDeployment.ReadDocumentAsync(System.String,System.Threading.CancellationToken) fd533836db97f7ed5d8fd7b1319b0d3a22ce8d4ca228e45afddbb56499d51a90 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-f3a2378981a633f1"></a>[`public Inno.Adapter.Content.FileSystem.FileContentDeployment.FileContentDeployment(string contentDirectory, string cacheRoot)`](../../../backends/FileSystem/runtime/Inno.Adapter.Content.FileSystem/FileContentDeployment.cs#L35) | Captures explicit deployment and application cache locations without inspecting platform layout.<br>`contentDirectory`：The directory containing the current deployment's metadata and content pack.<br>`cacheRoot`：The host-owned root beneath which application-scoped verified generations are prepared. | — | `System.ArgumentException`：A location is empty or cannot be normalized. |
| <a id="api-235d796a9fdd076c"></a>[`public System.Threading.Tasks.ValueTask<Inno.Content.IRuntimeContentStore> Inno.Adapter.Content.FileSystem.FileContentDeployment.PrepareAsync(Inno.Content.ContentPackDescriptor pack, Inno.Storage.StorageScope scope, Inno.Core.Serialization.SerializationGeneration serialization, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../backends/FileSystem/runtime/Inno.Adapter.Content.FileSystem/FileContentDeployment.cs#L69) | Opens, verifies and materializes one immutable content pack into an application-scoped generation.<br>返回：An owned verified store; the caller must dispose it after all readers retire.<br>`pack`：The frozen expected pack identity.<br>`scope`：The portable application namespace.<br>`serialization`：The owning serialization generation.<br>`cancellationToken`：Cancels acquisition and candidate preparation before publication. | — | `System.IO.InvalidDataException`：The pack or its index fails integrity validation.<br>`System.OperationCanceledException`：Preparation was canceled. |
| <a id="api-c9901ee644057714"></a>[`public System.Threading.Tasks.Task<byte[]> Inno.Adapter.Content.FileSystem.FileContentDeployment.ReadDocumentAsync(string fileName, System.Threading.CancellationToken cancellationToken)`](../../../backends/FileSystem/runtime/Inno.Adapter.Content.FileSystem/FileContentDeployment.cs#L118) | Acquires a bounded metadata document directly beneath the declared deployment root.<br>返回：Owned document bytes; an absent or changing document fails explicitly.<br>`fileName`：A single portable document filename, without directory segments.<br>`cancellationToken`：Cancels the owned stream read. | — | `System.ArgumentException`：The filename is not a single portable segment.<br>`System.IO.InvalidDataException`：The document is empty, too large or changes during acquisition. |
| <a id="api-6aaba1dd9c3acb44"></a>[`public sealed class Inno.Adapter.Content.FileSystem.FileContentDeployment`](../../../backends/FileSystem/runtime/Inno.Adapter.Content.FileSystem/FileContentDeployment.cs#L15) | Reads explicit file deployments using the shared content protocol and verified generation cache. | — | — |

### `Inno.Adapter.Content.FileSystem.FileContentPreparation`

<!-- api: T:Inno.Adapter.Content.FileSystem.FileContentPreparation 537cbd1aa483689184717d29db2a295ca3c1bf3b913252ba0ab1e77de1aac0a6 -->
<!-- api: M:Inno.Adapter.Content.FileSystem.FileContentPreparation.PrepareAsync(Inno.Content.IRuntimeContentStore,Inno.Adapter.Content.FileSystem.FileContentCacheOptions,System.Threading.CancellationToken) c72fa1fc27ce413d8d17bd1147ffaf38620aeb6b0e00c4a9657b7af0b6aeb447 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-7de727bfadd21a9f"></a>[`public static System.Threading.Tasks.ValueTask<Inno.Adapter.Content.FileSystem.FileContentStore> Inno.Adapter.Content.FileSystem.FileContentPreparation.PrepareAsync(Inno.Content.IRuntimeContentStore source, Inno.Adapter.Content.FileSystem.FileContentCacheOptions options, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../../backends/FileSystem/runtime/Inno.Adapter.Content.FileSystem/FileContentPreparation.cs#L39) | Reuses only a completely verified generation or publishes a fully validated replacement.<br>返回：A caller-owned store pinning the selected generation, with separate post-publication cleanup diagnostics.<br>`source`：The borrowed immutable store retained by the caller until preparation completes.<br>`options`：Host-selected cache ownership and bounded lock wait.<br>`cancellationToken`：Cancels preparation before ownership is returned; already committed content remains valid. | — | `System.IO.InvalidDataException`：The source cannot produce the indexed content or candidate validation fails.<br>`System.TimeoutException`：Another preparing process retains exclusive ownership beyond the host-selected timeout.<br>`System.OperationCanceledException`：Preparation was canceled; no store ownership is returned. |
| <a id="api-7a5565f16ec08f58"></a>[`public static class Inno.Adapter.Content.FileSystem.FileContentPreparation`](../../../backends/FileSystem/runtime/Inno.Adapter.Content.FileSystem/FileContentPreparation.cs#L13) | Coordinates complete cache validation, generation publication, and reader-safe retirement across processes. | — | — |

### `Inno.Adapter.Content.FileSystem.FileContentStore`

<!-- api: T:Inno.Adapter.Content.FileSystem.FileContentStore 968fb7f9a0e36649ca7b458b0feccfdcb9b32754e041522b5a2f3b0c901ab596 -->
<!-- api: P:Inno.Adapter.Content.FileSystem.FileContentStore.descriptor 5ddfd75bb9ca2b664e3f05d4c4190d2b89f59bc193a7bfaa62eb4e6e707d7188 -->
<!-- api: P:Inno.Adapter.Content.FileSystem.FileContentStore.index e1dba994eb16848bb64336021628bfbfd9c3423996295e19df9d66d45b7c6025 -->
<!-- api: P:Inno.Adapter.Content.FileSystem.FileContentStore.retirementDiagnostics f6f9e5f762ee6ade39f926ff9412b9cfe8235709f55da446aa6ce94de0ec445f -->
<!-- api: M:Inno.Adapter.Content.FileSystem.FileContentStore.Acquire(Inno.Content.ContentKey) 96956a550eb1bdb111de2ecae17e6224681b2bd81de057ecbbddc2ddcdee3989 -->
<!-- api: M:Inno.Adapter.Content.FileSystem.FileContentStore.Dispose f5327f12eba3dd6d80de20733d087bdd1caf1616e6e55508bddf46dd31e66925 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-0bc602a001abda26"></a>[`public Inno.Content.ContentReadLease Inno.Adapter.Content.FileSystem.FileContentStore.Acquire(Inno.Content.ContentKey key)`](../../../backends/FileSystem/runtime/Inno.Adapter.Content.FileSystem/FileContentStore.cs#L40) | Pins an entry against source retirement and supplies independent read streams.<br>返回：A caller-owned lease whose streams remain usable after this store closes.<br>`key`：The exact assigned logical key. | — | `System.IO.FileNotFoundException`：The exact content key is not present.<br>`System.ObjectDisposedException`：The store has closed and cannot issue new leases. |
| <a id="api-1b387ee48a61be85"></a>[`public void Inno.Adapter.Content.FileSystem.FileContentStore.Dispose()`](../../../backends/FileSystem/runtime/Inno.Adapter.Content.FileSystem/FileContentStore.cs#L54) | Stops new reads and releases the store pin after existing leases and streams retain their own ownership. | — | — |
| <a id="api-8da3bb6aa95dd90f"></a>[`public Inno.Content.ContentPackDescriptor Inno.Adapter.Content.FileSystem.FileContentStore.descriptor { get; }`](../../../backends/FileSystem/runtime/Inno.Adapter.Content.FileSystem/FileContentStore.cs#L29) | Gets the immutable complete-pack identity. | — | — |
| <a id="api-3bed98d372ba475a"></a>[`public Inno.Content.ContentPackIndex Inno.Adapter.Content.FileSystem.FileContentStore.index { get; }`](../../../backends/FileSystem/runtime/Inno.Adapter.Content.FileSystem/FileContentStore.cs#L32) | Gets the immutable complete payload inventory. | — | — |
| <a id="api-eea8173b47b97ceb"></a>[`public System.Collections.Generic.IReadOnlyList<string> Inno.Adapter.Content.FileSystem.FileContentStore.retirementDiagnostics { get; }`](../../../backends/FileSystem/runtime/Inno.Adapter.Content.FileSystem/FileContentStore.cs#L37) | Gets post-publication cleanup failures; these do not invalidate the verified published generation. | — | — |
| <a id="api-1c431dc2c16d0df1"></a>[`public sealed class Inno.Adapter.Content.FileSystem.FileContentStore : Inno.Content.IRuntimeContentStore`](../../../backends/FileSystem/runtime/Inno.Adapter.Content.FileSystem/FileContentStore.cs#L11) | Pins one fully verified filesystem generation while exposing only logical content reads. | — | — |

## 项目依赖

- [Inno.Storage](../../storage/Inno.Storage.md)：公开引用边界由实际签名核对。
- [Inno.Core.IO](../../core/Inno.Core.IO.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Content](../../assets/Inno.Content.md)：公开引用边界由实际签名核对。
