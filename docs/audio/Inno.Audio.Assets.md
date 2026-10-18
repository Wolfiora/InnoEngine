# Inno.Audio.Assets

[Audio 索引](README.md) · [Assets](../assets/Inno.Assets.md) · [Core contract](Inno.Audio.md) · [Runtime](Inno.Audio.Runtime.md)

`Inno.Audio.Assets` 是 authoring-only importer 项目。`AudioClipImporter` 通过 `AssetImporter<AudioClipAsset>` 基类自动发现，直接把 `.wav`、`.flac` 和 `.mp3` 文件作为 `AudioClipAsset` 创作源，不创建 companion asset。

## 输出契约

| output | 内容 |
| --- | --- |
| `runtime` | codec、channels、sample rate、frame count 与 encoded byte length 的小型严格 payload。 |
| `audio-data` | 原始编码数据的独立、不可变 CAS Artifact；不放入 `AssetObject.runtimePayload`。 |

Importer 校验 WAV chunk、FLAC STREAMINFO 和 MP3 frame/header。截断、矛盾或无法确定播放 metadata 的源会导入失败并产生正常 Asset diagnostic，不生成可播放的半成品。

```csharp
using Inno.Assets;
using Inno.Audio;

static AssetArtifactInfo? FindEncodedAudio(
    IAssetArtifactLookup artifacts,
    AudioClipAsset clip
) => artifacts.TryGetArtifact(clip.identity.persistentId, "audio-data", out AssetArtifactInfo? data)
    ? data
    : null;
```

上述方法只查询 metadata；读取音频 bytes 时必须通过 `AcquireArtifact` 持有租约，读取完成再 Dispose，不能把 metadata 当作长期读取保护。

Runtime 只依赖 `IAssetArtifactLookup`，因此同一代码可在 Editor 的 `AssetPipeline` 与 Player 的 `AssetDatabase` 上解析 Artifact。Ogg、Opus 与 transcoding 不属于当前项目；未来格式由独立 importer/codec Plugin 提供。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Audio.Assets.AudioClipImporter`

<!-- api: T:Inno.Audio.Assets.AudioClipImporter 1579a220637d1971a357aaa3120c2aea76498c535b3720b34aba461213bc8694 -->
<!-- api: P:Inno.Audio.Assets.AudioClipImporter.supportedExtensions 638768a0a5aa704efe50910ae8f623171ac83aeb630d55c0c7949a1dd170c722 -->
<!-- api: M:Inno.Audio.Assets.AudioClipImporter.ImportAsync(Inno.Assets.Pipeline.AssetImportContext,Inno.Assets.Pipeline.AssetImportWriter{Inno.Audio.AudioClipAsset},System.Threading.CancellationToken) 5138ff38180aff773bcdf7b22fcbfd025ad1519f8bbcba72e7710a415a0bc630 -->
<!-- api: M:Inno.Audio.Assets.AudioClipImporter.#ctor f1c079a2b1c3e3b6d84a9084a933c3c5b1587853ce3c799b994b62c22199e8c9 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-cc9f838377de094b"></a>[`public Inno.Audio.Assets.AudioClipImporter.AudioClipImporter()`](../../src/services/audio/Inno.Audio.Assets/AudioClipImporter.cs#L15) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-200d1810709eaa14"></a>[`protected override System.Threading.Tasks.ValueTask Inno.Audio.Assets.AudioClipImporter.ImportAsync(Inno.Assets.Pipeline.AssetImportContext context, Inno.Assets.Pipeline.AssetImportWriter<Inno.Audio.AudioClipAsset> output, System.Threading.CancellationToken cancellationToken)`](../../src/services/audio/Inno.Audio.Assets/AudioClipImporter.cs#L37) | Validates source metadata and emits the compact runtime and immutable encoded-data artifacts.<br>返回：A task that completes after both artifacts have been committed.<br>`context`：Source bytes and normalized source extension for the current import.<br>`output`：Writer that receives the audio asset and its two runtime artifacts.<br>`cancellationToken`：Token used to cancel artifact writes. | — | — |
| <a id="api-3de0f610b65742c0"></a>[`public override System.Collections.Generic.IReadOnlyList<string> Inno.Audio.Assets.AudioClipImporter.supportedExtensions { get; }`](../../src/services/audio/Inno.Audio.Assets/AudioClipImporter.cs#L20) | Gets the source extensions decoded by the standard audio metadata reader. | — | — |
| <a id="api-2c8da7a6efb66263"></a>[`public sealed class Inno.Audio.Assets.AudioClipImporter : Inno.Assets.Pipeline.AssetImporter<Inno.Audio.AudioClipAsset>`](../../src/services/audio/Inno.Audio.Assets/AudioClipImporter.cs#L15) | Imports standard encoded audio sources into compact runtime metadata and immutable encoded-data artifacts. | — | — |

## 项目依赖

- [Inno.Assets.Pipeline](../assets/Inno.Assets.Pipeline.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Audio](Inno.Audio.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
