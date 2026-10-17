# Inno.Animation.Assets

[Animation 索引](README.md) · [Contract](Inno.Animation.md) · [Assets Pipeline](../assets/Inno.Assets.Pipeline.md)

`AnimationClipImporter` 自动发现 `.ianim` structured source，使用全局 `SerializationRegistry` 解码当前格式，调用 `AnimationClipAsset.Validate()`，并输出名为 `runtime` 的不可变 Artifact。导出走同一结构化序列化系统，不建立 JSON 旁路。

损坏 duration、乱序 keyframe/marker、重复 binding、混合 value kind 或空 event ID 会令 import 明确失败。Importer 是 authoring-only；Player closure 只包含 `Inno.Animation`、Runtime 与冻结 Artifact，不包含本项目。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Animation.Assets.AnimationClipImporter`

<!-- api: T:Inno.Animation.Assets.AnimationClipImporter 9cff76bc432be1b46030adf8673ba1240dae217679cd709746c5d78ee497a35a -->
<!-- api: P:Inno.Animation.Assets.AnimationClipImporter.supportedExtensions d34c591b16348c674864efadc6a51e47fbc4022437c6c623235de937d7931d50 -->
<!-- api: M:Inno.Animation.Assets.AnimationClipImporter.ImportAsync(Inno.Assets.Pipeline.AssetImportContext,Inno.Assets.Pipeline.AssetImportWriter{Inno.Animation.AnimationClipAsset},System.Threading.CancellationToken) 8750fdda437a66889fe0930e26936b2b52caee203644281d6a8acc7b31ebe263 -->
<!-- api: M:Inno.Animation.Assets.AnimationClipImporter.ExportAsync(Inno.Assets.Pipeline.AssetExportContext,Inno.Animation.AnimationClipAsset,System.Threading.CancellationToken) 329e732f1be52f35299d5c40b166d30154f57a37d429b7b7d8e038b721495c13 -->
<!-- api: M:Inno.Animation.Assets.AnimationClipImporter.#ctor 898c50a05c8b1b4851c1b58a0f7bb77a9691db616138c51babcfde1170ec02b8 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-7cb3155a3548a354"></a>[`public Inno.Animation.Assets.AnimationClipImporter.AnimationClipImporter()`](../../src/content/animation/Inno.Animation.Assets/AnimationClipImporter.cs#L15) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-53b84da331b1c8fe"></a>[`protected override System.Threading.Tasks.ValueTask<System.ReadOnlyMemory<byte>?> Inno.Animation.Assets.AnimationClipImporter.ExportAsync(Inno.Assets.Pipeline.AssetExportContext context, Inno.Animation.AnimationClipAsset asset, System.Threading.CancellationToken cancellationToken)`](../../src/content/animation/Inno.Animation.Assets/AnimationClipImporter.cs#L76) | Serializes one validated animation clip back to its writable source mount.<br>返回：The complete current structured source bytes.<br>`context`：Generation-bound source serialization services.<br>`asset`：Animation clip to export.<br>`cancellationToken`：Cancellation token observed before source bytes are returned. | — | — |
| <a id="api-a5b553643505c091"></a>[`protected override System.Threading.Tasks.ValueTask Inno.Animation.Assets.AnimationClipImporter.ImportAsync(Inno.Assets.Pipeline.AssetImportContext context, Inno.Assets.Pipeline.AssetImportWriter<Inno.Animation.AnimationClipAsset> output, System.Threading.CancellationToken cancellationToken)`](../../src/content/animation/Inno.Animation.Assets/AnimationClipImporter.cs#L39) | Imports one validated structured animation clip.<br>返回：An operation that completes after the runtime artifact is staged.<br>`context`：Candidate source bytes and serialization generation.<br>`output`：Writer that receives the clip and immutable runtime artifact.<br>`cancellationToken`：Token that cancels artifact staging. | — | — |
| <a id="api-1f0090d19f77b631"></a>[`public override System.Collections.Generic.IReadOnlyList<string> Inno.Animation.Assets.AnimationClipImporter.supportedExtensions { get; }`](../../src/content/animation/Inno.Animation.Assets/AnimationClipImporter.cs#L22) | Gets the current structured animation source extension. | — | — |
| <a id="api-9d0b70bb326ead8c"></a>[`public sealed class Inno.Animation.Assets.AnimationClipImporter : Inno.Assets.Pipeline.AssetImporter<Inno.Animation.AnimationClipAsset>`](../../src/content/animation/Inno.Animation.Assets/AnimationClipImporter.cs#L15) | Imports and exports structured .ianim animation clip sources. | — | — |

## 项目依赖

- [Inno.Assets.Pipeline](../assets/Inno.Assets.Pipeline.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Animation](Inno.Animation.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
