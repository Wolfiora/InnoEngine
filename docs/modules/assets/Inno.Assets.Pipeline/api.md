# Inno.Assets.Pipeline API

[项目入口](../Inno.Assets.Pipeline.md)

## 公开 API

| 分组 | 主要 API |
| --- | --- |
| Composition | `AssetPipeline`, `AssetPipelineOptions`, `AssetPipelineMode`, `AssetSourcePolicy`, `AssetCacheOptions` |
| Source | `IAssetSourceSnapshot`, `AssetSampleImportTransaction`, `AssetSourceMount`, `AssetSourceMountTransaction`, `AssetFileSystem`, `AssetFileEntry`, `AssetSample`, `AssetSampleTransformContext`, `IAssetSampleSourceRewriter`, `AssetSampleSourceRewriterAttribute`, `AssetChangedEvent` |
| Import | `AssetImporter`, `AssetImporter<T>`, `AssetImportContext`, `AssetImportWriter<T>`, `AssetImportHealthSnapshot`, `AssetImportFailure` |
| Import settings | `AssetImportSettingsSnapshot`、`AssetImporter.CreateImportSettings()`、`AssetImportContext.importSettings`、Pipeline/Loader 的 `GetImportSettings` 与 `SaveImportSettings` |
| Build | `AssetBuildProcessor`, `AssetBuildProcessor<T>`, `AssetBuildContext<T>`, `AssetArtifactWriter` |
| Transactions/export | `AssetCatalogCandidate`, `AssetExportContext`, `AssetSerializationServices`, `AssetDeploymentScope`, `NativeAssetSourceSerialization` |
| Script authoring | `EditorAssets` 提供当前 Editor Session 中显式受限的保存入口；不暴露 Pipeline owner 或 mutation graph。 |
| Advanced facade | `AssetLoader`，用于独立 authoring host；普通 Application 优先使用 `AssetPipeline` |

Plugin Importer 继承 `InnoEditor.Assets` 中的 Importer base，并使用 Context、Writer 与
`NativeAssetSourceSerialization`。原生结构化 Source helper 只接收 Context 提供的
`AssetSerializationServices`，不会向 Plugin 泄漏 `TypeCatalog`、`SerializationRegistry` 或候选
Asset resolver。Importer 源码属于 Plugin 的 Editor assembly，不能进入 Runtime Plugin assembly。
