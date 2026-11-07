# Inno.Assets

[分类索引](README.md) · [Wiki 首页](../../README.md)

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

## API 参考

[完整声明、行为与调用约束](Inno.Assets/api.md)

<!-- api-include: Inno.Assets/api.md -->

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
| <a id="api-9f6e7162e0b7c353"></a>[`public sealed class Inno.Assets.ArtifactRetention : Inno.Assets.AssetResidencyProvider`](../../../src/content/assets/Inno.Assets/Artifacts/ArtifactRetention.cs#L12) | 独立管理 immutable artifact key 的显式保留计数。 | 它不替代资产对象驻留，也不承担物理缓存收集。 | Retain 的输入由具体 provider 验证。 |
| <a id="api-600087bc7ab023ec"></a>[`public Inno.Assets.ArtifactLease Inno.Assets.ArtifactRetention.Retain(Inno.Assets.AssetArtifactInfo artifact, System.Func<System.IO.Stream> openRead)`](../../../src/content/assets/Inno.Assets/Artifacts/ArtifactRetention.cs#L29) | 为已验证的输出增加一次保留，返回调用者拥有的 ArtifactLease；OpenRead 再建立独立 stream pin。 | 直到全部 lease 和流释放，artifact 才能退休；计数在同步边界内维护。 | metadata/openRead 为空会失败；provider 必须协调校验、acquisition 与收集。 |
| <a id="api-223892c5bb244960"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Assets.AssetArtifactKey> Inno.Assets.ArtifactRetention.GetRetainedKeys()`](../../../src/content/assets/Inno.Assets/Artifacts/ArtifactRetention.cs#L55) | 返回不可修改的 artifact key 快照，用于保护显式保留的输出。 | Collection owner 必须协调收集与新增 acquisition；快照本身不延长 lease。 | 不能仅凭旧快照认定之后没有新 reader。 |
| <a id="api-f92ce462c8b557ce"></a>[`public Inno.Assets.ArtifactRetention.ArtifactRetention()`](../../../src/content/assets/Inno.Assets/Artifacts/ArtifactRetention.cs#L12) | 独立管理 immutable artifact key 的显式保留计数。 | 它不替代资产对象驻留，也不承担物理缓存收集。 | Retain 的输入由具体 provider 验证。 |
| <a id="api-a83da64c5ca875fe"></a>[`public sealed class Inno.Assets.AssetArtifactInfo`](../../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactInfo.cs#L6) | 保存 bundle key、逻辑 outputName、contentHash 与长度的只读描述。 | 不取得读取保护；实际消费通过 ArtifactLease。 | 构造描述不替代 provider 的内容完整性验证。 |
| <a id="api-df41d16040083810"></a>[`public Inno.Assets.AssetArtifactInfo.AssetArtifactInfo(Inno.Assets.AssetArtifactKey key, string outputName, string contentHash, long length)`](../../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactInfo.cs#L23) | 规范化为大写 SHA-256 十六进制值；空白表示未指定。 | 纯值身份，不保存物理路径或 live object。 | 非空值必须为 64 位十六进制。 |
| <a id="api-4728bb31d1e00a9e"></a>[`public Inno.Assets.AssetArtifactKey Inno.Assets.AssetArtifactInfo.key { get; }`](../../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactInfo.cs#L38) | 保存 bundle key、逻辑 outputName、contentHash 与长度的只读描述。 | 不取得读取保护；实际消费通过 ArtifactLease。 | 构造描述不替代 provider 的内容完整性验证。 |
| <a id="api-71856a582ec7d21e"></a>[`public string Inno.Assets.AssetArtifactInfo.outputName { get; }`](../../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactInfo.cs#L43) | 保存 bundle key、逻辑 outputName、contentHash 与长度的只读描述。 | 不取得读取保护；实际消费通过 ArtifactLease。 | 构造描述不替代 provider 的内容完整性验证。 |
| <a id="api-4ce3defb362615cb"></a>[`public string Inno.Assets.AssetArtifactInfo.contentHash { get; }`](../../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactInfo.cs#L48) | 保存 bundle key、逻辑 outputName、contentHash 与长度的只读描述。 | 不取得读取保护；实际消费通过 ArtifactLease。 | 构造描述不替代 provider 的内容完整性验证。 |
| <a id="api-d010df966d2b5637"></a>[`public long Inno.Assets.AssetArtifactInfo.length { get; }`](../../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactInfo.cs#L53) | 保存 bundle key、逻辑 outputName、contentHash 与长度的只读描述。 | 不取得读取保护；实际消费通过 ArtifactLease。 | 构造描述不替代 provider 的内容完整性验证。 |
| <a id="api-fe5cc4e99d598cc3"></a>[`public readonly struct Inno.Assets.AssetArtifactKey : System.IEquatable<Inno.Assets.AssetArtifactKey>`](../../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactKey.cs#L8) | 返回规范化内容身份文本；未指定时返回空字符串。 | 纯值结果，不暴露缓存路径。 | 不返回 null。 |
| <a id="api-e1ba5872e21f6082"></a>[`public Inno.Assets.AssetArtifactKey.AssetArtifactKey(string value)`](../../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactKey.cs#L21) | 规范化为大写 SHA-256 十六进制值；空白表示未指定。 | 纯值身份，不保存物理路径或 live object。 | 非空值必须为 64 位十六进制。 |
| <a id="api-54e4875f36e9ffef"></a>[`public static Inno.Assets.AssetArtifactKey Inno.Assets.AssetArtifactKey.empty { get; }`](../../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactKey.cs#L40) | 返回规范化内容身份文本；未指定时返回空字符串。 | 纯值结果，不暴露缓存路径。 | 不返回 null。 |
| <a id="api-aa52a0de9c914085"></a>[`public string Inno.Assets.AssetArtifactKey.value { get; }`](../../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactKey.cs#L45) | 返回规范化内容身份文本；未指定时返回空字符串。 | 纯值结果，不暴露缓存路径。 | 不返回 null。 |
| <a id="api-4ceda1d86fbb8f14"></a>[`public bool Inno.Assets.AssetArtifactKey.isEmpty { get; }`](../../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactKey.cs#L50) | 返回规范化内容身份文本；未指定时返回空字符串。 | 纯值结果，不暴露缓存路径。 | 不返回 null。 |
| <a id="api-351260a331f0fb99"></a>[`public bool Inno.Assets.AssetArtifactKey.Equals(Inno.Assets.AssetArtifactKey other)`](../../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactKey.cs#L61) | 按规范化 value 比较内容身份；默认空值与显式空值具有相同语义。 | 纯值比较；没有资源 ownership transfer。 | 不是 object persistent ID 或 Stable Type ID。 |
| <a id="api-1fa3f24fb64301d8"></a>[`public override bool Inno.Assets.AssetArtifactKey.Equals(object? obj)`](../../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactKey.cs#L72) | 按规范化 value 比较内容身份；默认空值与显式空值具有相同语义。 | 纯值比较；没有资源 ownership transfer。 | 不是 object persistent ID 或 Stable Type ID。 |
| <a id="api-e83f3e8603620d59"></a>[`public override int Inno.Assets.AssetArtifactKey.GetHashCode()`](../../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactKey.cs#L80) | 返回与规范化 value 相等关系一致的哈希码。 | 仅用于当前集合；不作为持久内容指纹。 | 默认空值同样可用。 |
| <a id="api-a89ab6f97e8aee25"></a>[`public override string Inno.Assets.AssetArtifactKey.ToString()`](../../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactKey.cs#L88) | 返回规范化内容身份文本；未指定时返回空字符串。 | 纯值结果，不暴露缓存路径。 | 不返回 null。 |
| <a id="api-4f90ccc31919e78f"></a>[`public static bool Inno.Assets.AssetArtifactKey.operator ==(Inno.Assets.AssetArtifactKey left, Inno.Assets.AssetArtifactKey right)`](../../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactKey.cs#L102) | 按规范化 value 比较内容身份；默认空值与显式空值具有相同语义。 | 纯值比较；没有资源 ownership transfer。 | 不是 object persistent ID 或 Stable Type ID。 |
| <a id="api-5590d16970ab1d2b"></a>[`public static bool Inno.Assets.AssetArtifactKey.operator !=(Inno.Assets.AssetArtifactKey left, Inno.Assets.AssetArtifactKey right)`](../../../src/content/assets/Inno.Assets/Artifacts/AssetArtifactKey.cs#L119) | 按规范化 value 比较内容身份；默认空值与显式空值具有相同语义。 | 纯值比较；没有资源 ownership transfer。 | 不是 object persistent ID 或 Stable Type ID。 |
| <a id="api-117e7edb67f44d06"></a>[`public sealed class Inno.Assets.AssetRuntimeContentInfo`](../../../src/content/assets/Inno.Assets/Artifacts/AssetRuntimeContentInfo.cs#L8) | 冻结运行部署的 source 列表，并记录资产数、唯一 bundle 数及总字节。 | source 值在构造时复制；原列表之后的变化不影响摘要。 | sources 为空会抛 ArgumentNullException；计数由部署 owner 提供。 |
| <a id="api-8c89bf52e46ab353"></a>[`public Inno.Assets.AssetRuntimeContentInfo.AssetRuntimeContentInfo(System.Collections.Generic.IReadOnlyList<Inno.Assets.AssetSourceId> sources, int assetCount, int artifactBundleCount, long totalBytes)`](../../../src/content/assets/Inno.Assets/Artifacts/AssetRuntimeContentInfo.cs#L25) | 冻结运行部署的 source 列表，并记录资产数、唯一 bundle 数及总字节。 | source 值在构造时复制；原列表之后的变化不影响摘要。 | sources 为空会抛 ArgumentNullException；计数由部署 owner 提供。 |
| <a id="api-dc93b21fde35e700"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Assets.AssetSourceId> Inno.Assets.AssetRuntimeContentInfo.sources { get; }`](../../../src/content/assets/Inno.Assets/Artifacts/AssetRuntimeContentInfo.cs#L40) | 已复制的只读 source identity 列表。 | 摘要不保留内容流、文件路径或运行对象。 | 不作为缓存有效性证明。 |
| <a id="api-1e06400babc22119"></a>[`public int Inno.Assets.AssetRuntimeContentInfo.assetCount { get; }`](../../../src/content/assets/Inno.Assets/Artifacts/AssetRuntimeContentInfo.cs#L45) | 运行 catalog 的资产数量。 | 摘要不保留内容流、文件路径或运行对象。 | 不作为缓存有效性证明。 |
| <a id="api-647ea0d4be067039"></a>[`public int Inno.Assets.AssetRuntimeContentInfo.artifactBundleCount { get; }`](../../../src/content/assets/Inno.Assets/Artifacts/AssetRuntimeContentInfo.cs#L50) | 唯一内容 bundle 数量。 | 摘要不保留内容流、文件路径或运行对象。 | 不作为缓存有效性证明。 |
| <a id="api-aaa246fbba2d66a3"></a>[`public long Inno.Assets.AssetRuntimeContentInfo.totalBytes { get; }`](../../../src/content/assets/Inno.Assets/Artifacts/AssetRuntimeContentInfo.cs#L55) | 已复制至运行内容根的总字节。 | 摘要不保留内容流、文件路径或运行对象。 | 不作为缓存有效性证明。 |
| <a id="api-66ee3b34aa4f6278"></a>[`public interface Inno.Assets.IAssetArtifactLookup`](../../../src/content/assets/Inno.Assets/Artifacts/IAssetArtifactLookup.cs#L8) | 提供路径中立的命名 artifact 查询与获取。 | 具体 provider 负责内容来源、完整性和收集协调。 | 不暴露物理部署位置。 |
| <a id="api-227b1da82fdf7c44"></a>[`Inno.Assets.ArtifactLease Inno.Assets.IAssetArtifactLookup.AcquireArtifact(System.Guid persistentId, string outputName)`](../../../src/content/assets/Inno.Assets/Artifacts/IAssetArtifactLookup.cs#L25) | 返回 caller-owned immutable artifact lease；消费者在读取/复制结束后释放。 | OpenRead 的流独立 pin，可在基础 lease 释放后继续读取，直到流自身关闭。 | 输出不可用或无效会抛 InvalidOperationException。 |
| <a id="api-fb4a51439ed7f490"></a>[`bool Inno.Assets.IAssetArtifactLookup.TryGetArtifact(System.Guid persistentId, string outputName, out Inno.Assets.AssetArtifactInfo? artifact)`](../../../src/content/assets/Inno.Assets/Artifacts/IAssetArtifactLookup.cs#L45) | 保存 bundle key、逻辑 outputName、contentHash 与长度的只读描述。 | 不取得读取保护；实际消费通过 ArtifactLease。 | 构造描述不替代 provider 的内容完整性验证。 |
