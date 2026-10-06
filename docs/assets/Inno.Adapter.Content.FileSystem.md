# Inno.Adapter.Content.FileSystem

[Assets 索引](README.md) · [Wiki 首页](../README.md) · [内容契约](Inno.Content.md)

## 职责与依赖

将一个借用的只读内容来源准备为完整验证的文件缓存。依赖 `Inno.Content` 与 `Inno.Core.IO`，不依赖 Build。宿主提供应用缓存根；运行消费者仍使用逻辑 key 与流。

## 公开 API

| 入口 | 语义 |
| --- | --- |
| `FileContentCacheOptions(applicationCacheRoot, lockTimeout)` | 宿主明确指定绝对缓存根与有限写入等待时间 |
| `FileContentPreparation.PrepareAsync(source, options, cancellationToken)` | 验证、修复、发布后交出 owned store；调用者在完成前保留 source |
| `FileContentStore.descriptor`、`index`、`Acquire(key)`、`Dispose()` | 固定一个 generation 的只读 store |
| `FileContentStore.retirementDiagnostics` | 发布后的旧代清理错误；不改变新代已经提交的事实 |

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

`tests/content/Inno.Adapter.Content.FileSystem.Tests` 覆盖损坏、额外文件／目录、缺失 lease、取消、写锁超时、发布后清理诊断和两个独立进程的 reader 退休。新平台可换用其他 store，或在自己的 composition 中选择此缓存实现；共享 Player 与 Assets 无需修改。

## 当前源码公开 API 清单

以下仅列出当前程序集自己声明的 public/protected 契约；继承成员遵循所属基类页面。internal/private 实现不作为稳定公开 API。签名依据当前源码语义模型生成，行为、参数、异常与所有权说明同时以对应英文 XML 为准。

### `Inno.Adapter.Content.FileSystem.FileContentCacheOptions`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Adapter.Content.FileSystem.FileContentCacheOptions.FileContentCacheOptions(string applicationCacheRoot, System.TimeSpan? lockTimeout = null)`](../../src/adapters/content/Inno.Adapter.Content.FileSystem/FileContentCacheOptions.cs#L23) | Selects an application-owned absolute cache root without imposing a product prefix. |
| [`string Inno.Adapter.Content.FileSystem.FileContentCacheOptions.applicationCacheRoot`](../../src/adapters/content/Inno.Adapter.Content.FileSystem/FileContentCacheOptions.cs#L39) | Gets the host-selected application cache location. |
| [`System.TimeSpan Inno.Adapter.Content.FileSystem.FileContentCacheOptions.lockTimeout`](../../src/adapters/content/Inno.Adapter.Content.FileSystem/FileContentCacheOptions.cs#L44) | Gets the bounded exclusive preparation wait. |
| [`Inno.Adapter.Content.FileSystem.FileContentCacheOptions`](../../src/adapters/content/Inno.Adapter.Content.FileSystem/FileContentCacheOptions.cs#L9) | Freezes the host-selected cache location and bounded publication wait for immutable content. |

### `Inno.Adapter.Content.FileSystem.FileContentPreparation`

| 当前声明 | 行为 |
| --- | --- |
| [`static System.Threading.Tasks.ValueTask<Inno.Adapter.Content.FileSystem.FileContentStore> Inno.Adapter.Content.FileSystem.FileContentPreparation.PrepareAsync(Inno.Content.IRuntimeContentStore source, Inno.Adapter.Content.FileSystem.FileContentCacheOptions options, System.Threading.CancellationToken cancellationToken = default(System.Threading.CancellationToken))`](../../src/adapters/content/Inno.Adapter.Content.FileSystem/FileContentPreparation.cs#L39) | Reuses only a completely verified generation or publishes a fully validated replacement. |
| [`Inno.Adapter.Content.FileSystem.FileContentPreparation`](../../src/adapters/content/Inno.Adapter.Content.FileSystem/FileContentPreparation.cs#L13) | Coordinates complete cache validation, generation publication, and reader-safe retirement across processes. |

### `Inno.Adapter.Content.FileSystem.FileContentStore`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Content.ContentReadLease Inno.Adapter.Content.FileSystem.FileContentStore.Acquire(Inno.Content.ContentKey key)`](../../src/adapters/content/Inno.Adapter.Content.FileSystem/FileContentStore.cs#L40) | See the implemented contract. |
| [`void Inno.Adapter.Content.FileSystem.FileContentStore.Dispose()`](../../src/adapters/content/Inno.Adapter.Content.FileSystem/FileContentStore.cs#L54) | Stops new reads and releases the store pin after existing leases and streams retain their own ownership. |
| [`Inno.Content.ContentPackDescriptor Inno.Adapter.Content.FileSystem.FileContentStore.descriptor`](../../src/adapters/content/Inno.Adapter.Content.FileSystem/FileContentStore.cs#L29) | See the implemented contract. |
| [`Inno.Content.ContentPackIndex Inno.Adapter.Content.FileSystem.FileContentStore.index`](../../src/adapters/content/Inno.Adapter.Content.FileSystem/FileContentStore.cs#L32) | See the implemented contract. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Adapter.Content.FileSystem.FileContentStore.retirementDiagnostics`](../../src/adapters/content/Inno.Adapter.Content.FileSystem/FileContentStore.cs#L37) | Gets post-publication cleanup failures; these do not invalidate the verified published generation. |
| [`Inno.Adapter.Content.FileSystem.FileContentStore`](../../src/adapters/content/Inno.Adapter.Content.FileSystem/FileContentStore.cs#L11) | Pins one fully verified filesystem generation while exposing only logical content reads. |

## 项目依赖

- [Inno.Core.IO](../core/Inno.Core.IO.md)：实现依赖（`PrivateAssets="compile"`）。
- [Inno.Content](Inno.Content.md)：项目引用；公开签名可见性由语义边界检查确认。

共同 MSBuild 注入的 analyzer 与编译规则属于构建依赖，完整有效项目图记录在本轮验收证据中。
