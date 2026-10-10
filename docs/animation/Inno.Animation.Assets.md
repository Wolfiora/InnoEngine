# Inno.Animation.Assets

[Animation 索引](README.md) · [Contract](Inno.Animation.md) · [Assets Pipeline](../assets/Inno.Assets.Pipeline.md)

`AnimationClipImporter` 自动发现 `.ianim` structured source，使用全局 `SerializationRegistry` 解码当前格式，调用 `AnimationClipAsset.Validate()`，并输出名为 `runtime` 的不可变 Artifact。导出走同一结构化序列化系统，不建立 JSON 旁路。

损坏 duration、乱序 keyframe/marker、重复 binding、混合 value kind 或空 event ID 会令 import 明确失败。Importer 是 authoring-only；Player closure 只包含 `Inno.Animation`、Runtime 与冻结 Artifact，不包含本项目。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Animation.Assets.AnimationClipImporter`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Animation.Assets.AnimationClipImporter`](../../src/content/animation/Inno.Animation.Assets/AnimationClipImporter.cs#L14) | Imports and exports structured .ianim animation clip sources. |
| [`override System.Collections.Generic.IReadOnlyList<string> Inno.Animation.Assets.AnimationClipImporter.supportedExtensions`](../../src/content/animation/Inno.Animation.Assets/AnimationClipImporter.cs#L22) | Gets the current structured animation source extension. |
| [`override System.Threading.Tasks.ValueTask Inno.Animation.Assets.AnimationClipImporter.ImportAsync(Inno.Assets.Pipeline.AssetImportContext context, Inno.Assets.Pipeline.AssetImportWriter<Inno.Animation.AnimationClipAsset> output, System.Threading.CancellationToken cancellationToken)`](../../src/content/animation/Inno.Animation.Assets/AnimationClipImporter.cs#L39) | Imports one validated structured animation clip. |
| [`override System.Threading.Tasks.ValueTask<System.ReadOnlyMemory<byte>?> Inno.Animation.Assets.AnimationClipImporter.ExportAsync(Inno.Assets.Pipeline.AssetExportContext context, Inno.Animation.AnimationClipAsset asset, System.Threading.CancellationToken cancellationToken)`](../../src/content/animation/Inno.Animation.Assets/AnimationClipImporter.cs#L76) | Serializes one validated animation clip back to its writable source mount. |

## 项目依赖

- [Inno.Assets.Pipeline](../assets/Inno.Assets.Pipeline.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Animation](Inno.Animation.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
