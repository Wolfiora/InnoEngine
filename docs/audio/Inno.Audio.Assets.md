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
AudioClipAsset clip = assets.Load<AudioClipAsset>(AssetPath.Project("Audio/Jump.wav"));
if (assets.TryGetArtifact(clip.persistentId, "audio-data", out AssetArtifactInfo? data))
    Console.WriteLine($"{data.key}: {data.length} encoded bytes");
```

Runtime 只依赖 `IAssetArtifactLookup`，因此同一代码可在 Editor 的 `AssetPipeline` 与 Player 的 `AssetDatabase` 上解析 Artifact。Ogg、Opus 与 transcoding 不属于当前项目；未来格式由独立 importer/codec Plugin 提供。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Audio.Assets.AudioClipImporter`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Audio.Assets.AudioClipImporter`](../../src/services/audio/Inno.Audio.Assets/AudioClipImporter.cs#L14) | Imports standard encoded audio sources into compact runtime metadata and immutable encoded-data artifacts. |
| [`override System.Collections.Generic.IReadOnlyList<string> Inno.Audio.Assets.AudioClipImporter.supportedExtensions`](../../src/services/audio/Inno.Audio.Assets/AudioClipImporter.cs#L20) | Gets the source extensions decoded by the standard audio metadata reader. |
| [`override System.Threading.Tasks.ValueTask Inno.Audio.Assets.AudioClipImporter.ImportAsync(Inno.Assets.Pipeline.AssetImportContext context, Inno.Assets.Pipeline.AssetImportWriter<Inno.Audio.AudioClipAsset> output, System.Threading.CancellationToken cancellationToken)`](../../src/services/audio/Inno.Audio.Assets/AudioClipImporter.cs#L37) | Validates source metadata and emits the compact runtime and immutable encoded-data artifacts. |

## 项目依赖

- [Inno.Assets.Pipeline](../assets/Inno.Assets.Pipeline.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Audio](Inno.Audio.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
