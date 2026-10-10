# Inno.Rendering.Assets

[分类索引](README.md) · [Wiki 首页](../README.md) · [本轮整改计划](../architecture/ARCHITECTURE_CLEANUP_PLAN_2026_10_06.md)

## 职责与边界

保存 Shader、Material、Texture、Geometry 与 Pipeline 的后端中立运行资产协议、开放 ID、序列化与目标产物。依赖 Rendering Core、Assets、References 与必要 Foundation；不引用 Runtime、Shaders、Assets Pipeline 或资产创作项目。

## 数据和所有权

`ShaderDefinition` 是可编辑声明；`ShaderAsset.SetDefinition` 捕获并验证完整嵌套状态，失败不改变原状态。getter 返回独立声明副本。Material 默认值和纹理 Asset 引用属于本层；编译后的纯绑定接口属于 Core。

Material 根据开放 `ShaderContractId`、`ShaderPassRoleId` 与设备能力选择 Technique；Resolver 不内建 sprite、PBR 或固定 Pass Tag。Pipeline 资产只保存扩展 Stable Type ID 与中立配置。GPU 创建、缓存和退休由 Runtime 独占管理。

`RenderTargetArtifactPath` 返回逻辑内容定位；产物通过 `ArtifactLease.OpenRead()` 或内容 store 读取。持久模型不公开物理路径，不保存设备 handle。

## 使用示例

```csharp
using Inno.Rendering;
using Inno.Rendering.Assets;

static MaterialPassResolution? SelectPass(
    MaterialAsset material,
    GraphicsCapabilities capabilities
) {
    return MaterialPassResolver.Resolve(material, new ShaderContractId("sample.surface"),
        new ShaderPassRoleId("sample.draw"), capabilities);
}
```

导入、图编辑与目标编译位于 [Authoring](Inno.Rendering.Assets.Authoring.md)。本项目可进入静态 Player 闭包；Authoring 不进入 Player。迁移后的持久类型保留原 Stable Type ID，逻辑脚本 namespace 仍为 `InnoEngine.Rendering`。

## 验证

`tests/rendering/Inno.Rendering.Assets.Tests` 覆盖资产 round-trip、Material contract 和 Geometry artifact；真实 GPU 发布由 Runtime 测试和 Player 验收负责。

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Rendering.Assets.GeometryArtifact`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.GeometryArtifact`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryArtifact.cs#L12) | Encodes and validates the stable backend-neutral geometry artifact shared by import and runtime stages. |
| [`static Inno.Rendering.Assets.GeometryData Inno.Rendering.Assets.GeometryArtifact.Decode(System.ReadOnlySpan<byte> bytes)`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryArtifact.cs#L80) | Decodes and strictly validates one immutable geometry artifact. |
| [`static byte[] Inno.Rendering.Assets.GeometryArtifact.Encode(Inno.Rendering.Assets.GeometryData data)`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryArtifact.cs#L29) | Encodes normalized geometry into the immutable runtime artifact layout. |

### `Inno.Rendering.Assets.GeometryAsset`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Mathematics.Vector3 Inno.Rendering.Assets.GeometryAsset.boundsCenter`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryAsset.cs#L84) | Gets the object-space center of imported geometry bounds. |
| [`Inno.Core.Mathematics.Vector3 Inno.Rendering.Assets.GeometryAsset.boundsExtents`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryAsset.cs#L90) | Gets the non-negative object-space half-extents of imported geometry bounds. |
| [`Inno.Rendering.Assets.GeometryAsset`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryAsset.cs#L16) | Represents imported geometry without prescribing scene or draw semantics. |
| [`Inno.Rendering.Assets.GeometryAsset.GeometryAsset(int vertexCount, int indexCount, int sectionCount, Inno.Core.Mathematics.Vector3 boundsCenter, Inno.Core.Mathematics.Vector3 boundsExtents)`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryAsset.cs#L44) | Creates an immutable imported geometry description. |
| [`int Inno.Rendering.Assets.GeometryAsset.indexCount`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryAsset.cs#L72) | Gets the number of indices. |
| [`int Inno.Rendering.Assets.GeometryAsset.sectionCount`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryAsset.cs#L78) | Gets the number of independently submitted geometry sections. |
| [`int Inno.Rendering.Assets.GeometryAsset.vertexCount`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryAsset.cs#L66) | Gets the number of normalized vertices. |

### `Inno.Rendering.Assets.GeometryAssetRuntime`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.GeometryAssetRuntime`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryAssetRuntime.cs#L12) | Decodes normalized CPU geometry committed by mesh importers. |
| [`static Inno.Rendering.Assets.GeometryData Inno.Rendering.Assets.GeometryAssetRuntime.GetGeometryData(Inno.Rendering.Assets.GeometryAsset geometry)`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryAssetRuntime.cs#L27) | Decodes the current committed mesh payload. |

### `Inno.Rendering.Assets.GeometryData`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.GeometryData`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryData.cs#L12) | Contains normalized CPU mesh data ready for backend upload. |
| [`Inno.Rendering.Assets.GeometryData.GeometryData(Inno.Rendering.Assets.GeometryVertex[] vertices, uint[] indices, Inno.Rendering.Assets.GeometrySection[] sections)`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryData.cs#L27) | Creates normalized mesh data. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Assets.GeometrySection> Inno.Rendering.Assets.GeometryData.sections`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryData.cs#L53) | Gets contiguous submesh ranges. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Assets.GeometryVertex> Inno.Rendering.Assets.GeometryData.vertices`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryData.cs#L43) | Gets the normalized vertex stream. |
| [`System.Collections.Generic.IReadOnlyList<uint> Inno.Rendering.Assets.GeometryData.indices`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryData.cs#L48) | Gets the triangle index stream. |

### `Inno.Rendering.Assets.GeometrySection`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.GeometrySection`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometrySection.cs#L12) | Identifies a contiguous triangle-index range. |
| [`Inno.Rendering.Assets.GeometrySection.GeometrySection(int firstIndex, int indexCount)`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometrySection.cs#L24) | Creates a submesh range. |
| [`int Inno.Rendering.Assets.GeometrySection.firstIndex`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometrySection.cs#L37) | Gets the first index in the shared index buffer. |
| [`int Inno.Rendering.Assets.GeometrySection.indexCount`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometrySection.cs#L42) | Gets the number of indices in the range. |

### `Inno.Rendering.Assets.GeometryVertex`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Mathematics.Vector2 Inno.Rendering.Assets.GeometryVertex.textureCoordinate`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryVertex.cs#L60) | Gets the primary texture coordinate. |
| [`Inno.Core.Mathematics.Vector3 Inno.Rendering.Assets.GeometryVertex.normal`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryVertex.cs#L50) | Gets the object-space unit normal. |
| [`Inno.Core.Mathematics.Vector3 Inno.Rendering.Assets.GeometryVertex.position`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryVertex.cs#L45) | Gets the object-space position. |
| [`Inno.Core.Mathematics.Vector4 Inno.Rendering.Assets.GeometryVertex.tangent`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryVertex.cs#L55) | Gets the object-space tangent and handedness. |
| [`Inno.Rendering.Assets.GeometryVertex`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryVertex.cs#L12) | Stores one normalized vertex shared by all rendering backends. |
| [`Inno.Rendering.Assets.GeometryVertex.GeometryVertex(Inno.Core.Mathematics.Vector3 position, Inno.Core.Mathematics.Vector3 normal, Inno.Core.Mathematics.Vector4 tangent, Inno.Core.Mathematics.Vector2 textureCoordinate)`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryVertex.cs#L30) | Creates a normalized mesh vertex. |

### `Inno.Rendering.Assets.IRenderTextureArtifactSource`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.IRenderTextureArtifactSource`](../../src/services/rendering/Inno.Rendering.Assets/Textures/IRenderTextureArtifactSource.cs#L10) | Exposes one or more portable image artifacts owned by an imported asset. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Assets.RenderTextureArtifactSlot> Inno.Rendering.Assets.IRenderTextureArtifactSource.textureArtifacts`](../../src/services/rendering/Inno.Rendering.Assets/Textures/IRenderTextureArtifactSource.cs#L16) | Gets the complete immutable set of stable texture slots owned by the current asset content. |

### `Inno.Rendering.Assets.MaterialAsset`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.MaterialAsset`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialAsset.cs#L15) | Represents material state keyed by stable shader property identifiers. |
| [`Inno.Rendering.Assets.ShaderAsset? Inno.Rendering.Assets.MaterialAsset.shader`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialAsset.cs#L30) | Gets or sets the referenced shader asset. |
| [`Inno.Rendering.Assets.ShaderTechniqueId Inno.Rendering.Assets.MaterialAsset.techniqueId`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialAsset.cs#L36) | Gets or sets an optional explicitly selected technique. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Assets.MaterialMetadataEntry> Inno.Rendering.Assets.MaterialAsset.metadata`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialAsset.cs#L52) | Gets open provider-defined metadata. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Assets.MaterialPropertyEntry> Inno.Rendering.Assets.MaterialAsset.properties`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialAsset.cs#L42) | Gets persistent material values in stable insertion order. |
| [`System.Collections.Generic.IReadOnlyList<string> Inno.Rendering.Assets.MaterialAsset.keywords`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialAsset.cs#L47) | Gets enabled stable keyword option identifiers. |
| [`bool Inno.Rendering.Assets.MaterialAsset.RemoveMetadata(string key)`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialAsset.cs#L205) | Removes one provider-defined metadata value. |
| [`bool Inno.Rendering.Assets.MaterialAsset.TryGet(Inno.Rendering.ShaderPropertyId id, out Inno.Rendering.Assets.MaterialValue value)`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialAsset.cs#L119) | Tries to read one material property. |
| [`bool Inno.Rendering.Assets.MaterialAsset.TryGetMetadata(string key, out string? value)`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialAsset.cs#L187) | Tries to read one provider-defined metadata value. |
| [`void Inno.Rendering.Assets.MaterialAsset.ReplaceProperties(System.Collections.Generic.IEnumerable<Inno.Rendering.Assets.MaterialPropertyEntry> properties)`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialAsset.cs#L86) | Atomically replaces all persistent material values with an isolated, deterministically ordered set. |
| [`void Inno.Rendering.Assets.MaterialAsset.Set(Inno.Rendering.ShaderPropertyId id, Inno.Rendering.Assets.MaterialValue value)`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialAsset.cs#L63) | Creates or replaces one material property. |
| [`void Inno.Rendering.Assets.MaterialAsset.SetKeyword(string keyword, bool enabled)`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialAsset.cs#L137) | Enables or disables a declared static keyword option. |
| [`void Inno.Rendering.Assets.MaterialAsset.SetMetadata(string key, string value)`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialAsset.cs#L159) | Creates or replaces one provider-defined metadata value. |

### `Inno.Rendering.Assets.MaterialMetadataEntry`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.MaterialMetadataEntry`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialMetadataEntry.cs#L15) | Stores one open material metadata key and value. |
| [`Inno.Rendering.Assets.MaterialMetadataEntry.MaterialMetadataEntry(string key, string value)`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialMetadataEntry.cs#L27) | Creates a material metadata entry. |
| [`string Inno.Rendering.Assets.MaterialMetadataEntry.key`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialMetadataEntry.cs#L39) | Gets or sets the stable metadata key. |
| [`string Inno.Rendering.Assets.MaterialMetadataEntry.value`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialMetadataEntry.cs#L44) | Gets or sets the metadata value. |

### `Inno.Rendering.Assets.MaterialPassResolution`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.MaterialPassResolution`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialPassResolution.cs#L15) | Describes one capability-compatible material pass selection. |
| [`Inno.Rendering.Assets.MaterialPassResolution.MaterialPassResolution(Inno.Rendering.Assets.ShaderTechniqueDefinition technique, Inno.Rendering.Assets.ShaderPassDefinition pass)`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialPassResolution.cs#L30) | Creates a material pass resolution. |
| [`Inno.Rendering.Assets.ShaderPassDefinition Inno.Rendering.Assets.MaterialPassResolution.pass`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialPassResolution.cs#L46) | Gets the concrete shader pass with detached metadata storage. |
| [`Inno.Rendering.Assets.ShaderTechniqueDefinition Inno.Rendering.Assets.MaterialPassResolution.technique`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialPassResolution.cs#L41) | Gets the selected technique with detached role mapping storage. |

### `Inno.Rendering.Assets.MaterialPassResolver`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.MaterialPassResolver`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialPassResolver.cs#L15) | Resolves materials through open provider-owned shader contracts and roles. |
| [`static Inno.Rendering.Assets.MaterialPassResolution? Inno.Rendering.Assets.MaterialPassResolver.Resolve(Inno.Rendering.Assets.MaterialAsset material, Inno.Rendering.Assets.ShaderContractId contractId, Inno.Rendering.Assets.ShaderPassRoleId passRoleId, Inno.Rendering.GraphicsCapabilities capabilities)`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialPassResolver.cs#L36) | Resolves one capability-compatible pass. |
| [`static Inno.Rendering.Assets.MaterialPassResolution? Inno.Rendering.Assets.MaterialPassResolver.Resolve(Inno.Rendering.Assets.ShaderDefinition definition, Inno.Rendering.Assets.ShaderTechniqueId techniqueId, Inno.Rendering.Assets.ShaderContractId contractId, Inno.Rendering.Assets.ShaderPassRoleId passRoleId, Inno.Rendering.GraphicsCapabilities capabilities)`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialPassResolver.cs#L71) | Resolves a role against an exact published shader contract rather than a possibly newer authoring asset. |

### `Inno.Rendering.Assets.MaterialPropertyBlock`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.MaterialPropertyBlock`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialPropertyBlock.cs#L15) | Holds frame-local material overrides without modifying a shared asset. |
| [`bool Inno.Rendering.Assets.MaterialPropertyBlock.TryGet(Inno.Rendering.ShaderPropertyId id, out Inno.Rendering.Assets.MaterialValue value)`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialPropertyBlock.cs#L55) | Tries to read one frame-local override. |
| [`int Inno.Rendering.Assets.MaterialPropertyBlock.count`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialPropertyBlock.cs#L23) | Gets the number of active overrides. |
| [`void Inno.Rendering.Assets.MaterialPropertyBlock.Clear()`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialPropertyBlock.cs#L63) | Removes all frame-local overrides. |
| [`void Inno.Rendering.Assets.MaterialPropertyBlock.Set(Inno.Rendering.ShaderPropertyId id, Inno.Rendering.Assets.MaterialValue value)`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialPropertyBlock.cs#L34) | Creates or replaces one frame-local override. |

### `Inno.Rendering.Assets.MaterialPropertyEntry`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.MaterialPropertyEntry`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialPropertyEntry.cs#L15) | Stores one persistent material property entry. |
| [`Inno.Rendering.Assets.MaterialPropertyEntry.MaterialPropertyEntry(Inno.Rendering.ShaderPropertyId id, Inno.Rendering.Assets.MaterialValue value)`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialPropertyEntry.cs#L27) | Creates a persistent material property entry. |
| [`Inno.Rendering.Assets.MaterialValue Inno.Rendering.Assets.MaterialPropertyEntry.value`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialPropertyEntry.cs#L45) | Gets or sets the neutral material value. |
| [`Inno.Rendering.ShaderPropertyId Inno.Rendering.Assets.MaterialPropertyEntry.id`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialPropertyEntry.cs#L40) | Gets or sets the stable shader property identifier. |

### `Inno.Rendering.Assets.MaterialValue`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Mathematics.Matrix Inno.Rendering.Assets.MaterialValue.matrix`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialValue.cs#L73) | Gets or sets the matrix value. |
| [`Inno.Core.Mathematics.Vector4 Inno.Rendering.Assets.MaterialValue.vector`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialValue.cs#L68) | Gets or sets scalar, vector, or color components. |
| [`Inno.Rendering.Assets.MaterialValue`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialValue.cs#L43) | Stores one native-serializable material value without a GPU binding. |
| [`Inno.Rendering.Assets.MaterialValueKind Inno.Rendering.Assets.MaterialValue.kind`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialValue.cs#L63) | Gets or sets the stored value kind. |
| [`Inno.Rendering.Assets.TextureAsset? Inno.Rendering.Assets.MaterialValue.texture`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialValue.cs#L78) | Gets or sets the texture reference. |
| [`Inno.Rendering.RenderSamplerState Inno.Rendering.Assets.MaterialValue.sampler`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialValue.cs#L83) | Gets or sets the sampler used when this value stores a texture. |
| [`static Inno.Rendering.Assets.MaterialValue Inno.Rendering.Assets.MaterialValue.FromColor(Inno.Core.Mathematics.Color value)`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialValue.cs#L117) | Creates a linear color material value. |
| [`static Inno.Rendering.Assets.MaterialValue Inno.Rendering.Assets.MaterialValue.FromFloat(float value)`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialValue.cs#L94) | Creates a scalar material value. |
| [`static Inno.Rendering.Assets.MaterialValue Inno.Rendering.Assets.MaterialValue.FromMatrix(Inno.Core.Mathematics.Matrix value)`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialValue.cs#L129) | Creates a matrix material value. |
| [`static Inno.Rendering.Assets.MaterialValue Inno.Rendering.Assets.MaterialValue.FromTexture(Inno.Rendering.Assets.TextureAsset value, Inno.Rendering.RenderSamplerState? sampler = null)`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialValue.cs#L143) | Creates a texture material value. |
| [`static Inno.Rendering.Assets.MaterialValue Inno.Rendering.Assets.MaterialValue.FromVector(Inno.Core.Mathematics.Vector4 value)`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialValue.cs#L106) | Creates a vector material value. |

### `Inno.Rendering.Assets.MaterialValueKind`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.MaterialValueKind`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialValue.cs#L15) | Identifies the neutral value stored by a material property. |
| [`Inno.Rendering.Assets.MaterialValueKind.Color`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialValue.cs#L29) | Linear color value. |
| [`Inno.Rendering.Assets.MaterialValueKind.Float`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialValue.cs#L21) | Scalar floating-point value. |
| [`Inno.Rendering.Assets.MaterialValueKind.Matrix`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialValue.cs#L33) | Four-by-four matrix value. |
| [`Inno.Rendering.Assets.MaterialValueKind.Texture`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialValue.cs#L37) | Texture asset reference. |
| [`Inno.Rendering.Assets.MaterialValueKind.Vector`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialValue.cs#L25) | Four-component vector value. |

### `Inno.Rendering.Assets.RenderFeatureConfiguration`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.RenderFeatureConfiguration`](../../src/services/rendering/Inno.Rendering.Assets/Pipelines/RenderFeatureConfiguration.cs#L15) | Stores one ordered feature extension selection using only stable data. |
| [`Inno.Rendering.Assets.RenderFeatureConfiguration.RenderFeatureConfiguration()`](../../src/services/rendering/Inno.Rendering.Assets/Pipelines/RenderFeatureConfiguration.cs#L21) | Creates an empty feature configuration for deserialization. |
| [`Inno.Rendering.Assets.RenderFeatureConfiguration.RenderFeatureConfiguration(string featureTypeId, Inno.Rendering.Assets.SerializedRenderExtensionState? state = null, bool enabled = true)`](../../src/services/rendering/Inno.Rendering.Assets/Pipelines/RenderFeatureConfiguration.cs#L37) | Creates a feature configuration. |
| [`Inno.Rendering.Assets.SerializedRenderExtensionState Inno.Rendering.Assets.RenderFeatureConfiguration.state`](../../src/services/rendering/Inno.Rendering.Assets/Pipelines/RenderFeatureConfiguration.cs#L57) | Gets or sets reload-safe settings state. |
| [`bool Inno.Rendering.Assets.RenderFeatureConfiguration.enabled`](../../src/services/rendering/Inno.Rendering.Assets/Pipelines/RenderFeatureConfiguration.cs#L63) | Gets or sets whether the feature participates in graph building. |
| [`string Inno.Rendering.Assets.RenderFeatureConfiguration.featureTypeId`](../../src/services/rendering/Inno.Rendering.Assets/Pipelines/RenderFeatureConfiguration.cs#L51) | Gets or sets the stable feature extension identifier. |

### `Inno.Rendering.Assets.RenderPipelineAsset`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.RenderFeatureConfiguration[] Inno.Rendering.Assets.RenderPipelineAsset.features`](../../src/services/rendering/Inno.Rendering.Assets/Pipelines/RenderPipelineAsset.cs#L35) | Gets ordered feature configurations. |
| [`Inno.Rendering.Assets.RenderPipelineAsset`](../../src/services/rendering/Inno.Rendering.Assets/Pipelines/RenderPipelineAsset.cs#L15) | Selects a pipeline extension and ordered feature configuration without defining a render path. |
| [`Inno.Rendering.Assets.SerializedRenderExtensionState Inno.Rendering.Assets.RenderPipelineAsset.pipelineState`](../../src/services/rendering/Inno.Rendering.Assets/Pipelines/RenderPipelineAsset.cs#L29) | Gets or sets reload-safe pipeline settings. |
| [`string Inno.Rendering.Assets.RenderPipelineAsset.pipelineTypeId`](../../src/services/rendering/Inno.Rendering.Assets/Pipelines/RenderPipelineAsset.cs#L23) | Gets or sets the globally stable pipeline extension identifier. |
| [`void Inno.Rendering.Assets.RenderPipelineAsset.SetFeatures(System.Collections.Generic.IEnumerable<Inno.Rendering.Assets.RenderFeatureConfiguration> features)`](../../src/services/rendering/Inno.Rendering.Assets/Pipelines/RenderPipelineAsset.cs#L52) | Replaces ordered feature configurations. |

### `Inno.Rendering.Assets.RenderShaderArtifact`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.RenderShaderArtifact`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderArtifact.cs#L12) | Contains one immutable, source-free shader artifact ready for runtime GPU resource creation. |
| [`Inno.Rendering.Assets.RenderShaderArtifact.RenderShaderArtifact(string shaderName, string targetKey, Inno.Rendering.Assets.RenderShaderVariant variant, Inno.Rendering.ShaderInterface shaderInterface, System.Collections.Generic.IReadOnlyList<Inno.Rendering.Assets.RenderShaderPassArtifact> passes, System.ReadOnlySpan<byte> definitionData)`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderArtifact.cs#L42) | Creates a complete deployed shader artifact. |
| [`Inno.Rendering.Assets.RenderShaderVariant Inno.Rendering.Assets.RenderShaderArtifact.variant`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderArtifact.cs#L93) | Gets the canonical static keyword selection. |
| [`Inno.Rendering.ShaderInterface Inno.Rendering.Assets.RenderShaderArtifact.shaderInterface`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderArtifact.cs#L98) | Gets the complete validated shader resource binding contract. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Assets.RenderShaderPassArtifact> Inno.Rendering.Assets.RenderShaderArtifact.passes`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderArtifact.cs#L103) | Gets every source-free pass available to the runtime shader definition. |
| [`System.ReadOnlyMemory<byte> Inno.Rendering.Assets.RenderShaderArtifact.definitionData`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderArtifact.cs#L73) | Gets the immutable native-serialized runtime contract paired with this exact program publication. |
| [`string Inno.Rendering.Assets.RenderShaderArtifact.contentHash`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderArtifact.cs#L78) | Gets a semantic content identity covering the contract, variant, target, bindings and all compiled stages. |
| [`string Inno.Rendering.Assets.RenderShaderArtifact.shaderName`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderArtifact.cs#L83) | Gets the stable shader name expected by the runtime asset definition. |
| [`string Inno.Rendering.Assets.RenderShaderArtifact.targetKey`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderArtifact.cs#L88) | Gets the target compiler profile and policy identity. |

### `Inno.Rendering.Assets.RenderShaderArtifactCodec`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.RenderShaderArtifactCodec`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderArtifactCodec.cs#L13) | Encodes and validates the strict source-free binary format shared by game builds and Players. |
| [`static Inno.Rendering.Assets.RenderShaderArtifact Inno.Rendering.Assets.RenderShaderArtifactCodec.Decode(System.ReadOnlySpan<byte> bytes, string expectedShaderName, Inno.Rendering.Assets.RenderShaderVariant expectedVariant)`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderArtifactCodec.cs#L80) | Decodes a deployed shader artifact and validates it against the requesting runtime shader and variant. |
| [`static byte[] Inno.Rendering.Assets.RenderShaderArtifactCodec.Encode(Inno.Rendering.Assets.RenderShaderArtifact artifact)`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderArtifactCodec.cs#L31) | Encodes a validated shader artifact into its deterministic runtime deployment representation. |

### `Inno.Rendering.Assets.RenderShaderPassArtifact`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.RenderShaderPassArtifact`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderPassArtifact.cs#L12) | Carries one immutable compiled shader pass and its reflected runtime bindings. |
| [`Inno.Rendering.Assets.RenderShaderPassArtifact.RenderShaderPassArtifact(string name, Inno.Rendering.ShaderProgramKind programKind, Inno.Rendering.RenderRasterState rasterState, Inno.Rendering.ShaderInterface shaderInterface, System.Collections.Generic.IReadOnlyList<Inno.Rendering.Assets.RenderShaderStageArtifact> stages)`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderPassArtifact.cs#L38) | Creates a deployed shader pass. |
| [`Inno.Rendering.RenderRasterState Inno.Rendering.Assets.RenderShaderPassArtifact.rasterState`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderPassArtifact.cs#L73) | Gets an immutable copy of the backend-neutral raster state. |
| [`Inno.Rendering.ShaderInterface Inno.Rendering.Assets.RenderShaderPassArtifact.shaderInterface`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderPassArtifact.cs#L78) | Gets the validated pass-local resource binding contract. |
| [`Inno.Rendering.ShaderProgramKind Inno.Rendering.Assets.RenderShaderPassArtifact.programKind`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderPassArtifact.cs#L68) | Gets the programmable stage combination used by this pass. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Assets.RenderShaderStageArtifact> Inno.Rendering.Assets.RenderShaderPassArtifact.stages`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderPassArtifact.cs#L83) | Gets the complete target stage binaries required by this pass. |
| [`string Inno.Rendering.Assets.RenderShaderPassArtifact.name`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderPassArtifact.cs#L63) | Gets the stable pass name within the owning shader. |

### `Inno.Rendering.Assets.RenderShaderStageArtifact`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.RenderShaderStageArtifact`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderStageArtifact.cs#L12) | Stores one immutable target shader stage without retaining authoring source or compiler state. |
| [`Inno.Rendering.Assets.RenderShaderStageArtifact.RenderShaderStageArtifact(Inno.Rendering.ShaderStage stage, System.ReadOnlySpan<byte> bytes)`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderStageArtifact.cs#L29) | Creates a deployed shader stage. |
| [`Inno.Rendering.ShaderStage Inno.Rendering.Assets.RenderShaderStageArtifact.stage`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderStageArtifact.cs#L44) | Gets the programmable stage represented by this artifact. |
| [`System.ReadOnlyMemory<byte> Inno.Rendering.Assets.RenderShaderStageArtifact.bytes`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderStageArtifact.cs#L49) | Gets the immutable target backend program bytes. |

### `Inno.Rendering.Assets.RenderShaderVariant`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.RenderShaderVariant`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderVariant.cs#L12) | Identifies one deterministic selection of static shader keyword options shared by authoring and runtime. |
| [`Inno.Rendering.Assets.RenderShaderVariant.RenderShaderVariant(System.Collections.Generic.IReadOnlyDictionary<string, string> options)`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderVariant.cs#L30) | Creates a canonical shader variant from stable keyword selections. |
| [`System.Collections.Generic.IReadOnlyDictionary<string, string> Inno.Rendering.Assets.RenderShaderVariant.options`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderVariant.cs#L53) | Gets the immutable stable keyword selections. |
| [`bool Inno.Rendering.Assets.RenderShaderVariant.Equals(Inno.Rendering.Assets.RenderShaderVariant other)`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderVariant.cs#L174) | Determines whether this instance and the supplied value represent the same logical state. |
| [`override bool Inno.Rendering.Assets.RenderShaderVariant.Equals(object? obj)`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderVariant.cs#L185) | Determines whether this instance and the supplied object represent the same logical state. |
| [`override int Inno.Rendering.Assets.RenderShaderVariant.GetHashCode()`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderVariant.cs#L193) | Computes a hash code from the canonical variant representation. |
| [`override string Inno.Rendering.Assets.RenderShaderVariant.ToString()`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderVariant.cs#L235) | Formats this value as its canonical representation. |
| [`static Inno.Rendering.Assets.RenderShaderVariant Inno.Rendering.Assets.RenderShaderVariant.FromMaterial(Inno.Rendering.Assets.MaterialAsset material)`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderVariant.cs#L108) | Resolves the deterministic variant selected by a material. |
| [`static Inno.Rendering.Assets.RenderShaderVariant Inno.Rendering.Assets.RenderShaderVariant.FromMaterial(Inno.Rendering.Assets.MaterialAsset material, Inno.Rendering.Assets.ShaderDefinition definition)`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderVariant.cs#L133) | Resolves material keyword selections against an exact published shader contract. |
| [`static Inno.Rendering.Assets.RenderShaderVariant Inno.Rendering.Assets.RenderShaderVariant.Parse(string value)`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderVariant.cs#L72) | Parses the canonical representation stored in a deployed artifact. |
| [`static Inno.Rendering.Assets.RenderShaderVariant Inno.Rendering.Assets.RenderShaderVariant.empty`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderVariant.cs#L48) | Gets the empty default variant. |
| [`static bool Inno.Rendering.Assets.RenderShaderVariant.operator !=(Inno.Rendering.Assets.RenderShaderVariant left, Inno.Rendering.Assets.RenderShaderVariant right)`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderVariant.cs#L224) | Determines whether two variants select different stable options. |
| [`static bool Inno.Rendering.Assets.RenderShaderVariant.operator ==(Inno.Rendering.Assets.RenderShaderVariant left, Inno.Rendering.Assets.RenderShaderVariant right)`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderVariant.cs#L207) | Determines whether two variants select identical stable options. |
| [`string Inno.Rendering.Assets.RenderShaderVariant.value`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderVariant.cs#L58) | Gets the canonical cache-key representation. |

### `Inno.Rendering.Assets.RenderTargetArtifactPath`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.RenderTargetArtifactPath`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderTargetArtifactPath.cs#L12) | Defines deterministic relative paths shared by target-artifact producers and runtime consumers. |
| [`static string Inno.Rendering.Assets.RenderTargetArtifactPath.GetShaderPath(System.Guid shaderId, Inno.Rendering.GraphicsApi backend, Inno.Rendering.Assets.RenderShaderVariant variant)`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderTargetArtifactPath.cs#L33) | Gets the relative deployment path for one target shader variant. |
| [`static string Inno.Rendering.Assets.RenderTargetArtifactPath.GetTexturePath(Inno.Rendering.Assets.RenderTextureArtifactReference texture)`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderTargetArtifactPath.cs#L60) | Gets the relative deployment path for one portable texture artifact. |

### `Inno.Rendering.Assets.RenderTextureArtifactReference`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.RenderTextureArtifactReference`](../../src/services/rendering/Inno.Rendering.Assets/Textures/RenderTextureArtifactReference.cs#L10) | Identifies one immutable texture source across authoring, deployment, and runtime generations. |
| [`Inno.Rendering.Assets.RenderTextureArtifactReference.RenderTextureArtifactReference(System.Guid assetId, long contentRevision, Inno.Rendering.Assets.RenderTextureArtifactSlot slot)`](../../src/services/rendering/Inno.Rendering.Assets/Textures/RenderTextureArtifactReference.cs#L28) | Creates a reference to one texture slot owned by an imported asset generation. |
| [`Inno.Rendering.Assets.RenderTextureArtifactSlot Inno.Rendering.Assets.RenderTextureArtifactReference.slot`](../../src/services/rendering/Inno.Rendering.Assets/Textures/RenderTextureArtifactReference.cs#L56) | Gets the referenced stable texture slot. |
| [`System.Guid Inno.Rendering.Assets.RenderTextureArtifactReference.assetId`](../../src/services/rendering/Inno.Rendering.Assets/Textures/RenderTextureArtifactReference.cs#L46) | Gets the persistent identity of the owning asset. |
| [`long Inno.Rendering.Assets.RenderTextureArtifactReference.contentRevision`](../../src/services/rendering/Inno.Rendering.Assets/Textures/RenderTextureArtifactReference.cs#L51) | Gets the committed owner content revision used for last-good replacement. |

### `Inno.Rendering.Assets.RenderTextureArtifactSlot`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.RenderTextureArtifactSlot`](../../src/services/rendering/Inno.Rendering.Assets/Textures/RenderTextureArtifactSlot.cs#L10) | Declares an immutable source artifact that is compiled into one sampled texture slot. |
| [`Inno.Rendering.Assets.RenderTextureArtifactSlot.RenderTextureArtifactSlot(string id, string sourceOutputName, Inno.Rendering.Assets.TextureColorSpace colorSpace)`](../../src/services/rendering/Inno.Rendering.Assets/Textures/RenderTextureArtifactSlot.cs#L28) | Creates one stable texture slot declaration. |
| [`Inno.Rendering.Assets.TextureColorSpace Inno.Rendering.Assets.RenderTextureArtifactSlot.colorSpace`](../../src/services/rendering/Inno.Rendering.Assets/Textures/RenderTextureArtifactSlot.cs#L53) | Gets the sampling color-space interpretation. |
| [`string Inno.Rendering.Assets.RenderTextureArtifactSlot.id`](../../src/services/rendering/Inno.Rendering.Assets/Textures/RenderTextureArtifactSlot.cs#L43) | Gets the asset-local stable slot identifier. |
| [`string Inno.Rendering.Assets.RenderTextureArtifactSlot.sourceOutputName`](../../src/services/rendering/Inno.Rendering.Assets/Textures/RenderTextureArtifactSlot.cs#L48) | Gets the named portable image artifact consumed by target compilation. |

### `Inno.Rendering.Assets.SerializedRenderExtensionState`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Assets.AssetDependency[] Inno.Rendering.Assets.SerializedRenderExtensionState.dependencies`](../../src/services/rendering/Inno.Rendering.Assets/Pipelines/SerializedRenderExtensionState.cs#L74) | Gets or sets asset dependencies captured with the neutral property payload. |
| [`Inno.Rendering.Assets.SerializedRenderExtensionState`](../../src/services/rendering/Inno.Rendering.Assets/Pipelines/SerializedRenderExtensionState.cs#L15) | Stores reload-safe configuration for one pipeline or feature extension generation. |
| [`Inno.Rendering.Assets.SerializedRenderExtensionState.SerializedRenderExtensionState()`](../../src/services/rendering/Inno.Rendering.Assets/Pipelines/SerializedRenderExtensionState.cs#L21) | Creates empty extension state for deserialization. |
| [`Inno.Rendering.Assets.SerializedRenderExtensionState.SerializedRenderExtensionState(Inno.Assets.AssetPropertySnapshot properties)`](../../src/services/rendering/Inno.Rendering.Assets/Pipelines/SerializedRenderExtensionState.cs#L51) | Copies an owner-captured property snapshot, preserving its automatic resource dependency declarations. |
| [`Inno.Rendering.Assets.SerializedRenderExtensionState.SerializedRenderExtensionState(System.Guid stableTypeId, System.ReadOnlySpan<byte> propertyData)`](../../src/services/rendering/Inno.Rendering.Assets/Pipelines/SerializedRenderExtensionState.cs#L36) | Creates immutable extension state from stable identity and property bytes. |
| [`System.Guid Inno.Rendering.Assets.SerializedRenderExtensionState.stableTypeId`](../../src/services/rendering/Inno.Rendering.Assets/Pipelines/SerializedRenderExtensionState.cs#L62) | Gets or sets the stable settings type identity. |
| [`byte[] Inno.Rendering.Assets.SerializedRenderExtensionState.propertyData`](../../src/services/rendering/Inno.Rendering.Assets/Pipelines/SerializedRenderExtensionState.cs#L68) | Gets or sets neutral serialized property bytes. |

### `Inno.Rendering.Assets.ShaderAsset`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.ShaderAsset`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderAsset.cs#L16) | Represents the runtime contract imported from a shader graph. |
| [`Inno.Rendering.Assets.ShaderDefinition? Inno.Rendering.Assets.ShaderAsset.definition`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderAsset.cs#L28) | Gets a detached editable copy of the committed backend-neutral definition, or null before import. Nested declaration arrays are copied; referenced assets remain identity-owned resources. |
| [`void Inno.Rendering.Assets.ShaderAsset.SetDefinition(Inno.Rendering.Assets.ShaderDefinition value, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderAsset.cs#L48) | Commits a validated definition through the native serialization channel. |

### `Inno.Rendering.Assets.ShaderCompareFunction`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.ShaderCompareFunction`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L36) | Selects the comparison used by depth testing. |
| [`Inno.Rendering.Assets.ShaderCompareFunction.Always`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L70) | Always accepts the fragment. |
| [`Inno.Rendering.Assets.ShaderCompareFunction.Equal`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L50) | Accepts a fragment with an equal depth. |
| [`Inno.Rendering.Assets.ShaderCompareFunction.Greater`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L58) | Accepts a fragment with a greater depth. |
| [`Inno.Rendering.Assets.ShaderCompareFunction.GreaterEqual`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L66) | Accepts a fragment with a greater or equal depth. |
| [`Inno.Rendering.Assets.ShaderCompareFunction.Less`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L46) | Accepts a fragment with a smaller depth. |
| [`Inno.Rendering.Assets.ShaderCompareFunction.LessEqual`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L54) | Accepts a fragment with a smaller or equal depth. |
| [`Inno.Rendering.Assets.ShaderCompareFunction.Never`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L42) | Always rejects the fragment. |
| [`Inno.Rendering.Assets.ShaderCompareFunction.NotEqual`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L62) | Accepts a fragment with a different depth. |

### `Inno.Rendering.Assets.ShaderContractId`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.ShaderContractId`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderContractId.cs#L16) | Identifies an open shader and pipeline compatibility contract. |
| [`Inno.Rendering.Assets.ShaderContractId.ShaderContractId(string value)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderContractId.cs#L25) | Creates a shader contract identifier. |
| [`bool Inno.Rendering.Assets.ShaderContractId.isValid`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderContractId.cs#L39) | Gets whether the identifier has a usable value. |
| [`override string Inno.Rendering.Assets.ShaderContractId.ToString()`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderContractId.cs#L47) | Formats this value as a human-readable representation. |
| [`string Inno.Rendering.Assets.ShaderContractId.value`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderContractId.cs#L34) | Gets or sets the globally stable contract value. |

### `Inno.Rendering.Assets.ShaderCullMode`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.ShaderCullMode`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L16) | Selects the triangle face rejected by a raster pass. |
| [`Inno.Rendering.Assets.ShaderCullMode.Back`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L30) | Rejects back-facing triangles. |
| [`Inno.Rendering.Assets.ShaderCullMode.Front`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L26) | Rejects front-facing triangles. |
| [`Inno.Rendering.Assets.ShaderCullMode.None`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L22) | Does not reject either face orientation. |

### `Inno.Rendering.Assets.ShaderDefinition`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.ShaderDefinition`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderDefinition.cs#L16) | Contains the source-of-truth definition shared by shader import and material tooling. |
| [`Inno.Rendering.Assets.ShaderDefinition.ShaderDefinition()`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderDefinition.cs#L22) | Creates an empty shader definition for native deserialization. |
| [`Inno.Rendering.Assets.ShaderDefinition.ShaderDefinition(string name, System.Collections.Generic.IEnumerable<Inno.Rendering.Assets.ShaderPropertyDefinition> properties, System.Collections.Generic.IEnumerable<Inno.Rendering.Assets.ShaderKeywordDefinition> keywords, System.Collections.Generic.IEnumerable<Inno.Rendering.Assets.ShaderPassDefinition> passes, System.Collections.Generic.IEnumerable<Inno.Rendering.Assets.ShaderTechniqueDefinition>? techniques = null)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderDefinition.cs#L44) | Creates a shader definition. |
| [`Inno.Rendering.Assets.ShaderKeywordDefinition[] Inno.Rendering.Assets.ShaderDefinition.keywords`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderDefinition.cs#L77) | Gets or sets static keyword declarations. |
| [`Inno.Rendering.Assets.ShaderPassDefinition[] Inno.Rendering.Assets.ShaderDefinition.passes`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderDefinition.cs#L83) | Gets or sets backend-neutral pass declarations. |
| [`Inno.Rendering.Assets.ShaderPropertyDefinition[] Inno.Rendering.Assets.ShaderDefinition.properties`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderDefinition.cs#L71) | Gets or sets stable property declarations. |
| [`Inno.Rendering.Assets.ShaderTechniqueDefinition[] Inno.Rendering.Assets.ShaderDefinition.techniques`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderDefinition.cs#L89) | Gets or sets open contract and role mappings. |
| [`string Inno.Rendering.Assets.ShaderDefinition.name`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderDefinition.cs#L65) | Gets or sets the artist-facing shader name. |

### `Inno.Rendering.Assets.ShaderDefinitionValidator`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.ShaderDefinitionValidator`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderDefinitionValidator.cs#L12) | Validates the source-free material, pass and technique contract shared by graph compilation and deployed shaders. |
| [`static System.Collections.Generic.IReadOnlyList<Inno.Rendering.Assets.ShaderDiagnostic> Inno.Rendering.Assets.ShaderDefinitionValidator.Validate(Inno.Rendering.Assets.ShaderDefinition definition, Inno.Rendering.GraphicsCapabilities? capabilities = null)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderDefinitionValidator.cs#L30) | Checks declaration identities, resource kinds, stage visibility and open technique mappings without mutating the candidate. |

### `Inno.Rendering.Assets.ShaderDiagnostic`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Diagnostics.DiagnosticSeverity Inno.Rendering.Assets.ShaderDiagnostic.severity`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderDiagnostic.cs#L52) | Gets the diagnostic severity. |
| [`Inno.Rendering.Assets.ShaderDiagnostic`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderDiagnostic.cs#L12) | Reports one structured shader validation or compilation issue. |
| [`Inno.Rendering.Assets.ShaderDiagnostic.ShaderDiagnostic(string code, Inno.Core.Diagnostics.DiagnosticSeverity severity, string message, Inno.Rendering.Assets.ShaderSourceLocation? location = null)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderDiagnostic.cs#L30) | Creates a shader diagnostic. |
| [`Inno.Rendering.Assets.ShaderSourceLocation? Inno.Rendering.Assets.ShaderDiagnostic.location`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderDiagnostic.cs#L62) | Gets the optional source mapping. |
| [`string Inno.Rendering.Assets.ShaderDiagnostic.code`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderDiagnostic.cs#L47) | Gets the stable diagnostic code. |
| [`string Inno.Rendering.Assets.ShaderDiagnostic.message`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderDiagnostic.cs#L57) | Gets the artist-facing message. |

### `Inno.Rendering.Assets.ShaderKeywordDefinition`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.ShaderKeywordDefinition`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderKeywordDefinition.cs#L16) | Declares one static shader keyword that may produce compiled variants. |
| [`Inno.Rendering.Assets.ShaderKeywordDefinition.ShaderKeywordDefinition(string id, System.Collections.Generic.IEnumerable<string> options)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderKeywordDefinition.cs#L28) | Creates a shader keyword definition. |
| [`string Inno.Rendering.Assets.ShaderKeywordDefinition.id`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderKeywordDefinition.cs#L41) | Gets or sets the stable keyword identifier. |
| [`string[] Inno.Rendering.Assets.ShaderKeywordDefinition.options`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderKeywordDefinition.cs#L46) | Gets or sets allowed stable option identifiers. |

### `Inno.Rendering.Assets.ShaderMetadataEntry`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.ShaderMetadataEntry`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderMetadataEntry.cs#L16) | Stores one open metadata key and value. |
| [`Inno.Rendering.Assets.ShaderMetadataEntry.ShaderMetadataEntry(string key, string value)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderMetadataEntry.cs#L28) | Creates one metadata entry. |
| [`string Inno.Rendering.Assets.ShaderMetadataEntry.key`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderMetadataEntry.cs#L40) | Gets or sets the stable metadata key. |
| [`string Inno.Rendering.Assets.ShaderMetadataEntry.value`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderMetadataEntry.cs#L45) | Gets or sets the metadata value. |

### `Inno.Rendering.Assets.ShaderPassDefinition`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.ShaderMetadataEntry[] Inno.Rendering.Assets.ShaderPassDefinition.metadata`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPassDefinition.cs#L75) | Gets or sets provider-defined metadata. |
| [`Inno.Rendering.Assets.ShaderPassDefinition`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPassDefinition.cs#L16) | Defines one backend-neutral raster or compute shader pass. |
| [`Inno.Rendering.Assets.ShaderPassDefinition Inno.Rendering.Assets.ShaderPassDefinition.Copy()`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPassDefinition.cs#L84) | Copies the pass declaration with independently owned metadata storage. |
| [`Inno.Rendering.Assets.ShaderPassDefinition.ShaderPassDefinition(string name, Inno.Rendering.ShaderProgramKind programKind, Inno.Rendering.GraphicsCapability requiredFeatures = Inno.Rendering.GraphicsCapability.None, Inno.Rendering.Assets.ShaderRenderState? renderState = null, System.Collections.Generic.IEnumerable<Inno.Rendering.Assets.ShaderMetadataEntry>? metadata = null)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPassDefinition.cs#L37) | Creates a shader pass definition. |
| [`Inno.Rendering.Assets.ShaderRenderState Inno.Rendering.Assets.ShaderPassDefinition.renderState`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPassDefinition.cs#L70) | Gets or sets backend-neutral fixed-function state. |
| [`Inno.Rendering.GraphicsCapability Inno.Rendering.Assets.ShaderPassDefinition.requiredFeatures`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPassDefinition.cs#L65) | Gets or sets required device capabilities. |
| [`Inno.Rendering.ShaderProgramKind Inno.Rendering.Assets.ShaderPassDefinition.programKind`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPassDefinition.cs#L60) | Gets or sets the programmable stage combination. |
| [`string Inno.Rendering.Assets.ShaderPassDefinition.name`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPassDefinition.cs#L55) | Gets or sets the stable pass name. |

### `Inno.Rendering.Assets.ShaderPassRoleId`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.ShaderPassRoleId`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPassRoleId.cs#L16) | Identifies an open pass purpose defined by a rendering provider. |
| [`Inno.Rendering.Assets.ShaderPassRoleId.ShaderPassRoleId(string value)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPassRoleId.cs#L25) | Creates a shader pass role identifier. |
| [`bool Inno.Rendering.Assets.ShaderPassRoleId.isValid`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPassRoleId.cs#L39) | Gets whether the identifier has a usable value. |
| [`override string Inno.Rendering.Assets.ShaderPassRoleId.ToString()`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPassRoleId.cs#L47) | Formats this value as a human-readable representation. |
| [`string Inno.Rendering.Assets.ShaderPassRoleId.value`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPassRoleId.cs#L34) | Gets or sets the stable role value. |

### `Inno.Rendering.Assets.ShaderPropertyBindingOwner`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.ShaderPropertyBindingOwner`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPropertyBindingOwner.cs#L16) | Identifies which layer supplies a declared shader property's value for each dispatch or draw. |
| [`Inno.Rendering.Assets.ShaderPropertyBindingOwner.Material`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPropertyBindingOwner.cs#L22) | The material and its optional property block supply the value. |
| [`Inno.Rendering.Assets.ShaderPropertyBindingOwner.RenderPass`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPropertyBindingOwner.cs#L26) | The render pass supplies the value directly through its command encoder. |

### `Inno.Rendering.Assets.ShaderPropertyDefinition`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.MaterialValue Inno.Rendering.Assets.ShaderPropertyDefinition.defaultValue`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPropertyDefinition.cs#L98) | Gets or sets the native serializable default value. |
| [`Inno.Rendering.Assets.ShaderPropertyBindingOwner Inno.Rendering.Assets.ShaderPropertyDefinition.bindingOwner`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPropertyDefinition.cs#L113) | Gets or sets the layer responsible for supplying this binding at execution time. |
| [`Inno.Rendering.Assets.ShaderPropertyDefinition`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPropertyDefinition.cs#L16) | Declares one shader property and its reflected stage visibility. |
| [`Inno.Rendering.Assets.ShaderPropertyDefinition.ShaderPropertyDefinition(Inno.Rendering.ShaderPropertyId id, string displayName, Inno.Rendering.ShaderPropertyType type, Inno.Rendering.ShaderStage stages, Inno.Rendering.Assets.MaterialValue defaultValue, Inno.Rendering.ShaderPropertyBindingKind? bindingKind = null, Inno.Rendering.RenderStorageAccess storageAccess = Inno.Rendering.RenderStorageAccess.Read, Inno.Rendering.Assets.ShaderPropertyBindingOwner bindingOwner = Inno.Rendering.Assets.ShaderPropertyBindingOwner.Material)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPropertyDefinition.cs#L47) | Creates a shader property definition. |
| [`Inno.Rendering.RenderStorageAccess Inno.Rendering.Assets.ShaderPropertyDefinition.storageAccess`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPropertyDefinition.cs#L108) | Gets or sets required access for storage texture or buffer bindings. |
| [`Inno.Rendering.ShaderPropertyBindingKind Inno.Rendering.Assets.ShaderPropertyDefinition.bindingKind`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPropertyDefinition.cs#L103) | Gets or sets how the property enters the shader resource interface. |
| [`Inno.Rendering.ShaderPropertyId Inno.Rendering.Assets.ShaderPropertyDefinition.id`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPropertyDefinition.cs#L78) | Gets or sets the stable property identifier. |
| [`Inno.Rendering.ShaderPropertyType Inno.Rendering.Assets.ShaderPropertyDefinition.type`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPropertyDefinition.cs#L88) | Gets or sets the property type. |
| [`Inno.Rendering.ShaderStage Inno.Rendering.Assets.ShaderPropertyDefinition.stages`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPropertyDefinition.cs#L93) | Gets or sets stages that access the property. |
| [`string Inno.Rendering.Assets.ShaderPropertyDefinition.displayName`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPropertyDefinition.cs#L83) | Gets or sets the artist-facing property name. |

### `Inno.Rendering.Assets.ShaderRenderState`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.ShaderCompareFunction Inno.Rendering.Assets.ShaderRenderState.depthCompare`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L112) | Gets or sets the depth comparison function. |
| [`Inno.Rendering.Assets.ShaderCullMode Inno.Rendering.Assets.ShaderRenderState.cull`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L102) | Gets or sets the face culling mode. |
| [`Inno.Rendering.Assets.ShaderRenderState`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L76) | Declares backend-neutral fixed-function state for one shader pass. |
| [`Inno.Rendering.RenderBlendState Inno.Rendering.Assets.ShaderRenderState.blend`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L122) | Gets or sets independent RGB and alpha blending. |
| [`Inno.Rendering.RenderFrontFace Inno.Rendering.Assets.ShaderRenderState.frontFace`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L107) | Gets or sets the winding order interpreted as the front face. |
| [`Inno.Rendering.RenderPrimitiveTopology Inno.Rendering.Assets.ShaderRenderState.topology`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L97) | Gets or sets the primitive assembly used by raster draws. |
| [`bool Inno.Rendering.Assets.ShaderRenderState.depthWrite`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L117) | Gets or sets whether accepted fragments update depth. |
| [`bool Inno.Rendering.Assets.ShaderRenderState.multisampling`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L132) | Gets or sets whether compatible targets use multisample rasterization. |
| [`byte Inno.Rendering.Assets.ShaderRenderState.colorWriteMask`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L127) | Gets or sets the four-bit RGBA color write mask. |
| [`static Inno.Rendering.Assets.ShaderRenderState Inno.Rendering.Assets.ShaderRenderState.opaque`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L82) | Gets the default opaque raster state. |

### `Inno.Rendering.Assets.ShaderSourceLocation`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.ShaderSourceLocation`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderSourceLocation.cs#L12) | Maps a shader diagnostic back to an asset, pass, stage, and source position. |
| [`Inno.Rendering.Assets.ShaderSourceLocation.ShaderSourceLocation(string assetPath, string passName, Inno.Rendering.ShaderStage stage, int line = 0, int column = 0)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderSourceLocation.cs#L33) | Creates a shader source location. |
| [`Inno.Rendering.ShaderStage Inno.Rendering.Assets.ShaderSourceLocation.stage`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderSourceLocation.cs#L64) | Gets the shader stage. |
| [`int Inno.Rendering.Assets.ShaderSourceLocation.column`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderSourceLocation.cs#L74) | Gets the one-based column, or zero when unavailable. |
| [`int Inno.Rendering.Assets.ShaderSourceLocation.line`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderSourceLocation.cs#L69) | Gets the one-based line, or zero when unavailable. |
| [`string Inno.Rendering.Assets.ShaderSourceLocation.assetPath`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderSourceLocation.cs#L54) | Gets the project-relative source asset path. |
| [`string Inno.Rendering.Assets.ShaderSourceLocation.passName`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderSourceLocation.cs#L59) | Gets the stable pass name. |

### `Inno.Rendering.Assets.ShaderTechniqueDefinition`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.ShaderContractId Inno.Rendering.Assets.ShaderTechniqueDefinition.contract`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderTechniqueDefinition.cs#L59) | Gets or sets the open rendering-provider contract. |
| [`Inno.Rendering.Assets.ShaderTechniqueDefinition`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderTechniqueDefinition.cs#L16) | Declares one contract-compatible pass mapping selectable by materials. |
| [`Inno.Rendering.Assets.ShaderTechniqueDefinition.ShaderTechniqueDefinition(Inno.Rendering.Assets.ShaderTechniqueId id, Inno.Rendering.Assets.ShaderContractId contract, System.Collections.Generic.IEnumerable<Inno.Rendering.Assets.ShaderTechniquePass> passes, Inno.Rendering.GraphicsCapability requiredFeatures = Inno.Rendering.GraphicsCapability.None)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderTechniqueDefinition.cs#L34) | Creates a shader technique definition. |
| [`Inno.Rendering.Assets.ShaderTechniqueId Inno.Rendering.Assets.ShaderTechniqueDefinition.id`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderTechniqueDefinition.cs#L54) | Gets or sets the stable technique identifier. |
| [`Inno.Rendering.Assets.ShaderTechniquePass[] Inno.Rendering.Assets.ShaderTechniqueDefinition.passes`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderTechniqueDefinition.cs#L64) | Gets or sets role-to-pass mappings. |
| [`Inno.Rendering.GraphicsCapability Inno.Rendering.Assets.ShaderTechniqueDefinition.requiredFeatures`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderTechniqueDefinition.cs#L69) | Gets or sets capabilities required by the complete technique. |

### `Inno.Rendering.Assets.ShaderTechniqueId`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.ShaderTechniqueId`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderTechniqueId.cs#L16) | Identifies one material-selectable technique in a shader. |
| [`Inno.Rendering.Assets.ShaderTechniqueId.ShaderTechniqueId(string value)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderTechniqueId.cs#L25) | Creates a shader technique identifier. |
| [`bool Inno.Rendering.Assets.ShaderTechniqueId.isValid`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderTechniqueId.cs#L39) | Gets whether the identifier has a usable value. |
| [`override string Inno.Rendering.Assets.ShaderTechniqueId.ToString()`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderTechniqueId.cs#L47) | Formats this value as a human-readable representation. |
| [`string Inno.Rendering.Assets.ShaderTechniqueId.value`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderTechniqueId.cs#L34) | Gets or sets the stable technique value. |

### `Inno.Rendering.Assets.ShaderTechniquePass`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.ShaderPassRoleId Inno.Rendering.Assets.ShaderTechniquePass.role`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderTechniquePass.cs#L42) | Gets or sets the provider-defined role. |
| [`Inno.Rendering.Assets.ShaderTechniquePass`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderTechniquePass.cs#L16) | Maps one provider-defined role to one concrete shader pass. |
| [`Inno.Rendering.Assets.ShaderTechniquePass.ShaderTechniquePass(Inno.Rendering.Assets.ShaderPassRoleId role, string passName)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderTechniquePass.cs#L28) | Creates a technique pass mapping. |
| [`string Inno.Rendering.Assets.ShaderTechniquePass.passName`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderTechniquePass.cs#L47) | Gets or sets the concrete pass name. |

### `Inno.Rendering.Assets.TextureAsset`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.RenderTextureArtifactReference Inno.Rendering.Assets.TextureAsset.GetTextureArtifactReference()`](../../src/services/rendering/Inno.Rendering.Assets/Textures/TextureAsset.cs#L105) | Creates a stable reference to this texture's current imported content. |
| [`Inno.Rendering.Assets.TextureAsset`](../../src/services/rendering/Inno.Rendering.Assets/Textures/TextureAsset.cs#L16) | Represents imported texture content without owning a GPU handle. |
| [`Inno.Rendering.Assets.TextureAsset.TextureAsset(int width, int height, Inno.Rendering.Assets.TextureColorSpace colorSpace, string sourceFormat)`](../../src/services/rendering/Inno.Rendering.Assets/Textures/TextureAsset.cs#L49) | Creates an immutable imported texture description. |
| [`Inno.Rendering.Assets.TextureColorSpace Inno.Rendering.Assets.TextureAsset.colorSpace`](../../src/services/rendering/Inno.Rendering.Assets/Textures/TextureAsset.cs#L79) | Gets the declared sample color space. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Assets.RenderTextureArtifactSlot> Inno.Rendering.Assets.TextureAsset.textureArtifacts`](../../src/services/rendering/Inno.Rendering.Assets/Textures/TextureAsset.cs#L91) | Gets the single source artifact compiled for this imported texture. |
| [`int Inno.Rendering.Assets.TextureAsset.height`](../../src/services/rendering/Inno.Rendering.Assets/Textures/TextureAsset.cs#L73) | Gets the source pixel height. |
| [`int Inno.Rendering.Assets.TextureAsset.width`](../../src/services/rendering/Inno.Rendering.Assets/Textures/TextureAsset.cs#L67) | Gets the source pixel width. |
| [`string Inno.Rendering.Assets.TextureAsset.sourceFormat`](../../src/services/rendering/Inno.Rendering.Assets/Textures/TextureAsset.cs#L85) | Gets the normalized source container name. |

### `Inno.Rendering.Assets.TextureColorSpace`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Rendering.Assets.TextureColorSpace`](../../src/services/rendering/Inno.Rendering.Assets/Textures/TextureColorSpace.cs#L16) | Selects how texture samples are decoded for shader use. |
| [`Inno.Rendering.Assets.TextureColorSpace.Linear`](../../src/services/rendering/Inno.Rendering.Assets/Textures/TextureColorSpace.cs#L22) | Samples are interpreted as linear values. |
| [`Inno.Rendering.Assets.TextureColorSpace.Srgb`](../../src/services/rendering/Inno.Rendering.Assets/Textures/TextureColorSpace.cs#L26) | Color samples are decoded from sRGB at sampling time. |

## 项目依赖

- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Rendering](Inno.Rendering.md)：公开引用边界由实际签名核对。
- [Inno.Assets](../assets/Inno.Assets.md)：公开引用边界由实际签名核对。
- [Inno.References](../references/Inno.References.md)：公开引用边界由实际签名核对。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：公开引用边界由实际签名核对。
- [Inno.Core.Mathematics](../core/Inno.Core.Mathematics.md)：公开引用边界由实际签名核对。
- [Inno.Core.Diagnostics](../core/Inno.Core.Diagnostics.md)：公开引用边界由实际签名核对。
