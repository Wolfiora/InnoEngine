# Inno.Rendering.Assets

[分类索引](README.md) · [Wiki 首页](../README.md)

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

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Rendering.Assets.GeometryArtifact`

<!-- api: T:Inno.Rendering.Assets.GeometryArtifact 13bfb104d2232ff5effd54dbb497c1817f679a814f8c6967ab52d5b8c569ff95 -->
<!-- api: M:Inno.Rendering.Assets.GeometryArtifact.Encode(Inno.Rendering.Assets.GeometryData) d6d4b3e90fb4c31d04ea1e44e1ff87f397206fb2a865242f7b2dff226c714086 -->
<!-- api: M:Inno.Rendering.Assets.GeometryArtifact.Decode(System.ReadOnlySpan{System.Byte}) ec9842c3440dceefae2511860191a0633323dd8d4cf9573de95c65100353cf3e -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-0ecded89517a4c3f"></a>[`public static Inno.Rendering.Assets.GeometryData Inno.Rendering.Assets.GeometryArtifact.Decode(System.ReadOnlySpan<byte> bytes)`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryArtifact.cs#L80) | Decodes and strictly validates one immutable geometry artifact.<br>返回：The normalized geometry represented by the payload.<br>`bytes`：The complete artifact payload. | — | `System.IO.InvalidDataException`：Thrown when the artifact is empty, corrupt, truncated, or contains invalid ranges. |
| <a id="api-d1ae629ab303c891"></a>[`public static byte[] Inno.Rendering.Assets.GeometryArtifact.Encode(Inno.Rendering.Assets.GeometryData data)`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryArtifact.cs#L29) | Encodes normalized geometry into the immutable runtime artifact layout.<br>返回：A complete geometry artifact owned by the caller.<br>`data`：The normalized vertices, indices, and sections to encode. | — | `System.ArgumentNullException`：Thrown when data is null. |
| <a id="api-d5275de500e15114"></a>[`public static class Inno.Rendering.Assets.GeometryArtifact`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryArtifact.cs#L13) | Encodes and validates the stable backend-neutral geometry artifact shared by import and runtime stages. | — | — |

### `Inno.Rendering.Assets.GeometryAsset`

<!-- api: T:Inno.Rendering.Assets.GeometryAsset 0169a06257d8ff5474db0b909b5164c1478bd1ebee931d639ce4a9bba1791935 -->
<!-- api: M:Inno.Rendering.Assets.GeometryAsset.#ctor(System.Int32,System.Int32,System.Int32,Inno.Core.Mathematics.Vector3,Inno.Core.Mathematics.Vector3) 922e1a6427934a4decc4f521ca247c155175d456cf176427ca1463b725ba8f93 -->
<!-- api: P:Inno.Rendering.Assets.GeometryAsset.vertexCount 2afa9a8e29f6baee4de2bec56874bfeb3a4e81940bedf91baf71730e00509714 -->
<!-- api: P:Inno.Rendering.Assets.GeometryAsset.indexCount f0593d14a95eec9000e6bd5f29fbd0be7a2dce9f1380c1940f88a371e0c3c0f9 -->
<!-- api: P:Inno.Rendering.Assets.GeometryAsset.sectionCount 2515ac711dcc7f760b7a3995441d01a1d9de38522558085c043031f4b80c2243 -->
<!-- api: P:Inno.Rendering.Assets.GeometryAsset.boundsCenter bf8921d8416d79aed69028492eee5324c42f05e94e1235d217b99a8ede1066f7 -->
<!-- api: P:Inno.Rendering.Assets.GeometryAsset.boundsExtents f3d911dada0e0c3936ddfd5e16baabf4996a6102e96edf55f1bb253dec2fefb3 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-396ab75bc7f03463"></a>[`public Inno.Rendering.Assets.GeometryAsset.GeometryAsset(int vertexCount, int indexCount, int sectionCount, Inno.Core.Mathematics.Vector3 boundsCenter, Inno.Core.Mathematics.Vector3 boundsExtents)`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryAsset.cs#L44) | Creates an immutable imported geometry description.<br>`vertexCount`：The number of normalized vertices.<br>`indexCount`：The number of indices.<br>`sectionCount`：The number of independently submitted sections.<br>`boundsCenter`：The object-space center of the geometry bounds.<br>`boundsExtents`：The non-negative object-space half-extents of the geometry bounds. | — | `System.ArgumentOutOfRangeException`：Thrown when any count is negative or a bounds extent is negative. |
| <a id="api-3e4df70327689cc5"></a>[`public Inno.Core.Mathematics.Vector3 Inno.Rendering.Assets.GeometryAsset.boundsCenter { get; internal set; }`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryAsset.cs#L85) | Gets the object-space center of imported geometry bounds. | — | — |
| <a id="api-1dfe21daaa2e7d7f"></a>[`public Inno.Core.Mathematics.Vector3 Inno.Rendering.Assets.GeometryAsset.boundsExtents { get; internal set; }`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryAsset.cs#L91) | Gets the non-negative object-space half-extents of imported geometry bounds. | — | — |
| <a id="api-18e8eceb043a759f"></a>[`public int Inno.Rendering.Assets.GeometryAsset.indexCount { get; internal set; }`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryAsset.cs#L73) | Gets the number of indices. | — | — |
| <a id="api-680e5481b5a7abdd"></a>[`public int Inno.Rendering.Assets.GeometryAsset.sectionCount { get; internal set; }`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryAsset.cs#L79) | Gets the number of independently submitted geometry sections. | — | — |
| <a id="api-8cec9485a44ca7f5"></a>[`public int Inno.Rendering.Assets.GeometryAsset.vertexCount { get; internal set; }`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryAsset.cs#L67) | Gets the number of normalized vertices. | — | — |
| <a id="api-872bbf217bf7b8b3"></a>[`public sealed class Inno.Rendering.Assets.GeometryAsset : Inno.Assets.AssetObject`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryAsset.cs#L17) | Represents imported geometry without prescribing scene or draw semantics. | — | — |

### `Inno.Rendering.Assets.GeometryAssetRuntime`

<!-- api: T:Inno.Rendering.Assets.GeometryAssetRuntime 3cef5feb6f2e3e2cfdfff83239177778258446754f11cfec8b4ac11640fc67ea -->
<!-- api: M:Inno.Rendering.Assets.GeometryAssetRuntime.GetGeometryData(Inno.Rendering.Assets.GeometryAsset) 29b6575d3f86234ae5145ee83bc9873a5875f2fe079e26367044df6784a573ad -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-956a6b281f5efb27"></a>[`public static Inno.Rendering.Assets.GeometryData Inno.Rendering.Assets.GeometryAssetRuntime.GetGeometryData(Inno.Rendering.Assets.GeometryAsset geometry)`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryAssetRuntime.cs#L27) | Decodes the current committed mesh payload.<br>返回：Normalized vertices, triangle indices and submeshes.<br>`geometry`：Imported geometry asset. | — | `System.IO.InvalidDataException`：Thrown when the payload is missing or corrupt. |
| <a id="api-01638060e7e865af"></a>[`public static class Inno.Rendering.Assets.GeometryAssetRuntime`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryAssetRuntime.cs#L13) | Decodes normalized CPU geometry committed by mesh importers. | — | — |

### `Inno.Rendering.Assets.GeometryData`

<!-- api: T:Inno.Rendering.Assets.GeometryData ca0304ad58a1adfa6930bb66ae5da2b2223dfd5e8d28c3102f5988c25e9ac7f1 -->
<!-- api: M:Inno.Rendering.Assets.GeometryData.#ctor(Inno.Rendering.Assets.GeometryVertex[],System.UInt32[],Inno.Rendering.Assets.GeometrySection[]) 27cba1bed7799fc1ef8e0d61fff655c01291a5507afc3bf6ad424ce8b1d4edcc -->
<!-- api: P:Inno.Rendering.Assets.GeometryData.vertices 769e8c170adf937c44715354b0863a86b1c7358e7c19736f6a960cb9a88f7896 -->
<!-- api: P:Inno.Rendering.Assets.GeometryData.indices 24a5e16057757d1e05902e055a928f1fab5c8110fdeeca15265c638e00b6ed4e -->
<!-- api: P:Inno.Rendering.Assets.GeometryData.sections 9acaaccd84bb783140c9b54bbb17a868cdb957e1a018ad725e34e33a49201b0b -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-232a10db1cd4458a"></a>[`public Inno.Rendering.Assets.GeometryData.GeometryData(Inno.Rendering.Assets.GeometryVertex[] vertices, uint[] indices, Inno.Rendering.Assets.GeometrySection[] sections)`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryData.cs#L27) | Creates normalized mesh data.<br>`vertices`：Vertex stream.<br>`indices`：Triangle index stream.<br>`sections`：Contiguous submesh ranges. | — | — |
| <a id="api-2b356da5dd0ad2e3"></a>[`public System.Collections.Generic.IReadOnlyList<uint> Inno.Rendering.Assets.GeometryData.indices { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryData.cs#L48) | Gets the triangle index stream. | — | — |
| <a id="api-7068df96c12ec89f"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Assets.GeometrySection> Inno.Rendering.Assets.GeometryData.sections { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryData.cs#L53) | Gets contiguous submesh ranges. | — | — |
| <a id="api-c7b202ebad8c6cbb"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Assets.GeometryVertex> Inno.Rendering.Assets.GeometryData.vertices { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryData.cs#L43) | Gets the normalized vertex stream. | — | — |
| <a id="api-c73466b8cb672553"></a>[`public sealed class Inno.Rendering.Assets.GeometryData`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryData.cs#L13) | Contains normalized CPU mesh data ready for backend upload. | — | — |

### `Inno.Rendering.Assets.GeometrySection`

<!-- api: T:Inno.Rendering.Assets.GeometrySection 412ee0440a57b67f25c4b9ddd4f17518e165b9c5acf664ef7c5711ee0d3d4014 -->
<!-- api: M:Inno.Rendering.Assets.GeometrySection.#ctor(System.Int32,System.Int32) f4d1c0e91cc0cff8f9a5cd5fc76e62a0ad500e1fd89e91096d44ceecddc1aafd -->
<!-- api: P:Inno.Rendering.Assets.GeometrySection.firstIndex 1a9557c71ba1b8f86ccd407e3fbb996e06bcaa017f4f7c0d9d34f61d6e9f2904 -->
<!-- api: P:Inno.Rendering.Assets.GeometrySection.indexCount bfa8af88c4bf6df08d379a34a415c8a20a8e0268e827952a96a4e73304738909 -->
<!-- api: M:Inno.Rendering.Assets.GeometrySection.ToString 211b214ea29e042f7bf53e4fb69a09145b4dd763d3e68c655f3dc30d3570b3f9 -->
<!-- api: M:Inno.Rendering.Assets.GeometrySection.op_Inequality(Inno.Rendering.Assets.GeometrySection,Inno.Rendering.Assets.GeometrySection) 5411337fcf9589abcce252ed4b7d8bc74e8626dfae9384ebe8a48b2089705bee -->
<!-- api: M:Inno.Rendering.Assets.GeometrySection.op_Equality(Inno.Rendering.Assets.GeometrySection,Inno.Rendering.Assets.GeometrySection) 6b31317b4705ab69b68a6113d7a607045cd2554464431c2f08bd6245927444b0 -->
<!-- api: M:Inno.Rendering.Assets.GeometrySection.GetHashCode bb6ac6f61f7c3ff58b0b5df8ba4962c4add26784faa5abf62b52a0f991506f6b -->
<!-- api: M:Inno.Rendering.Assets.GeometrySection.Equals(System.Object) ef1ac11acd825d50aef221cb13af4a9cefb6c6189f3a6a0def99564def02dab5 -->
<!-- api: M:Inno.Rendering.Assets.GeometrySection.Equals(Inno.Rendering.Assets.GeometrySection) 1c5d7b6ccbe50622630ec86952ed554d230da7709b490da72b5339da57fcee74 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-abb469343c0cbf5f"></a>[`public Inno.Rendering.Assets.GeometrySection.GeometrySection(int firstIndex, int indexCount)`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometrySection.cs#L24) | Creates a submesh range.<br>`firstIndex`：First index in the shared index buffer.<br>`indexCount`：Number of indices in the range. | — | — |
| <a id="api-c5fba3642479004d"></a>[`public bool Inno.Rendering.Assets.GeometrySection.Equals(Inno.Rendering.Assets.GeometrySection other)`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometrySection.cs#L13) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-ba5a186729b79124"></a>[`public override bool Inno.Rendering.Assets.GeometrySection.Equals(object obj)`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometrySection.cs#L13) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-37b198bd8224ce41"></a>[`public override int Inno.Rendering.Assets.GeometrySection.GetHashCode()`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometrySection.cs#L13) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-f8a3bdf61eef8713"></a>[`public static bool Inno.Rendering.Assets.GeometrySection.operator ==(Inno.Rendering.Assets.GeometrySection left, Inno.Rendering.Assets.GeometrySection right)`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometrySection.cs#L13) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-69581340ae7fefe3"></a>[`public static bool Inno.Rendering.Assets.GeometrySection.operator !=(Inno.Rendering.Assets.GeometrySection left, Inno.Rendering.Assets.GeometrySection right)`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometrySection.cs#L13) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-a048eb5825acc259"></a>[`public override string Inno.Rendering.Assets.GeometrySection.ToString()`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometrySection.cs#L13) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-aa2ee12a482c17b8"></a>[`public int Inno.Rendering.Assets.GeometrySection.firstIndex { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometrySection.cs#L37) | Gets the first index in the shared index buffer. | — | — |
| <a id="api-45de23e154d1b2ba"></a>[`public int Inno.Rendering.Assets.GeometrySection.indexCount { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometrySection.cs#L42) | Gets the number of indices in the range. | — | — |
| <a id="api-f082ad8cdae2f4d7"></a>[`public readonly record struct Inno.Rendering.Assets.GeometrySection : System.IEquatable<Inno.Rendering.Assets.GeometrySection>`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometrySection.cs#L13) | Identifies a contiguous triangle-index range. | — | — |

### `Inno.Rendering.Assets.GeometryVertex`

<!-- api: T:Inno.Rendering.Assets.GeometryVertex 81726ba3c4bcce9bac0b711a887f1b0d49a32a0c81238fcbee5f4e525fb6120c -->
<!-- api: M:Inno.Rendering.Assets.GeometryVertex.#ctor(Inno.Core.Mathematics.Vector3,Inno.Core.Mathematics.Vector3,Inno.Core.Mathematics.Vector4,Inno.Core.Mathematics.Vector2) f8dea7cf96dfd2d008f762d999f44c4b1db212cb6697b79e03b15da736051fdc -->
<!-- api: P:Inno.Rendering.Assets.GeometryVertex.position 3cbd98dd463bc775c9941030a59a04f985aafdc0f1628236ff876555ea070055 -->
<!-- api: P:Inno.Rendering.Assets.GeometryVertex.normal 4973b5da2767151e59e91d9c392cc8dd51b3041ea11d594e275f63dd6c6154d4 -->
<!-- api: P:Inno.Rendering.Assets.GeometryVertex.tangent 48d56993c9c5609871dccf59b7fd94cdb0f8e8c7cfddcdc780db8044c4241cb1 -->
<!-- api: P:Inno.Rendering.Assets.GeometryVertex.textureCoordinate 7027f74ba2228a40acd351aeb2a1008d9f35f5f5a1977153a109356d9e522c06 -->
<!-- api: M:Inno.Rendering.Assets.GeometryVertex.ToString 70ea408c9e8156c59ea00acb928b8afd0ce3bbf8ebca7e4069f80062daa334fc -->
<!-- api: M:Inno.Rendering.Assets.GeometryVertex.op_Inequality(Inno.Rendering.Assets.GeometryVertex,Inno.Rendering.Assets.GeometryVertex) afd8e26a5eec1c58bd655d6bf31b0f756308d541a91fd62a18421d632bdd94d6 -->
<!-- api: M:Inno.Rendering.Assets.GeometryVertex.op_Equality(Inno.Rendering.Assets.GeometryVertex,Inno.Rendering.Assets.GeometryVertex) ccebeae5bf196173ccae8a006853584485b30380333b092d904a7ea87d23965b -->
<!-- api: M:Inno.Rendering.Assets.GeometryVertex.GetHashCode 25602715ff504fb90e896c73d3ebba4b4e8f426c70d8c05579bdfef75d5bc2d7 -->
<!-- api: M:Inno.Rendering.Assets.GeometryVertex.Equals(System.Object) 0930cf55cd701ff08c4fb6510aa93d93a1b055ca92e2d23bcb9a5e70eb888323 -->
<!-- api: M:Inno.Rendering.Assets.GeometryVertex.Equals(Inno.Rendering.Assets.GeometryVertex) ce654fa7abb2558673b497d56f0761747460eff5b85f3cb8345ed6484edc207c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e09c7e9cf60c984e"></a>[`public Inno.Rendering.Assets.GeometryVertex.GeometryVertex(Inno.Core.Mathematics.Vector3 position, Inno.Core.Mathematics.Vector3 normal, Inno.Core.Mathematics.Vector4 tangent, Inno.Core.Mathematics.Vector2 textureCoordinate)`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryVertex.cs#L30) | Creates a normalized mesh vertex.<br>`position`：Object-space position.<br>`normal`：Object-space unit normal.<br>`tangent`：Object-space tangent and handedness.<br>`textureCoordinate`：Primary texture coordinate. | — | — |
| <a id="api-751dbd476e84a1aa"></a>[`public bool Inno.Rendering.Assets.GeometryVertex.Equals(Inno.Rendering.Assets.GeometryVertex other)`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryVertex.cs#L13) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-5547e9d81f3623e5"></a>[`public override bool Inno.Rendering.Assets.GeometryVertex.Equals(object obj)`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryVertex.cs#L13) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-49cc6716debdf970"></a>[`public override int Inno.Rendering.Assets.GeometryVertex.GetHashCode()`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryVertex.cs#L13) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-ff710522ec93ad26"></a>[`public static bool Inno.Rendering.Assets.GeometryVertex.operator ==(Inno.Rendering.Assets.GeometryVertex left, Inno.Rendering.Assets.GeometryVertex right)`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryVertex.cs#L13) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-2c838286f79f9864"></a>[`public static bool Inno.Rendering.Assets.GeometryVertex.operator !=(Inno.Rendering.Assets.GeometryVertex left, Inno.Rendering.Assets.GeometryVertex right)`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryVertex.cs#L13) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-d854fe8496703a8f"></a>[`public override string Inno.Rendering.Assets.GeometryVertex.ToString()`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryVertex.cs#L13) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-47850b00d4901b68"></a>[`public Inno.Core.Mathematics.Vector3 Inno.Rendering.Assets.GeometryVertex.normal { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryVertex.cs#L50) | Gets the object-space unit normal. | — | — |
| <a id="api-78efc4ce176c64eb"></a>[`public Inno.Core.Mathematics.Vector3 Inno.Rendering.Assets.GeometryVertex.position { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryVertex.cs#L45) | Gets the object-space position. | — | — |
| <a id="api-c6d95fe98d147446"></a>[`public Inno.Core.Mathematics.Vector4 Inno.Rendering.Assets.GeometryVertex.tangent { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryVertex.cs#L55) | Gets the object-space tangent and handedness. | — | — |
| <a id="api-2519cb582e82398d"></a>[`public Inno.Core.Mathematics.Vector2 Inno.Rendering.Assets.GeometryVertex.textureCoordinate { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryVertex.cs#L60) | Gets the primary texture coordinate. | — | — |
| <a id="api-cff4979a11211b91"></a>[`public readonly record struct Inno.Rendering.Assets.GeometryVertex : System.IEquatable<Inno.Rendering.Assets.GeometryVertex>`](../../src/services/rendering/Inno.Rendering.Assets/Geometry/GeometryVertex.cs#L13) | Stores one normalized vertex shared by all rendering backends. | — | — |

### `Inno.Rendering.Assets.IRenderTextureArtifactSource`

<!-- api: T:Inno.Rendering.Assets.IRenderTextureArtifactSource a82113e42249cf05b78cddd9c0fd98d9e427f16f0dc76d9a55692fc9b7c9638e -->
<!-- api: P:Inno.Rendering.Assets.IRenderTextureArtifactSource.textureArtifacts e4bff55915cb73f34cc6c6f976b86ef067509c750e15c6d3bc06b6c4c18b21f3 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-baa0d1b676fb3564"></a>[`System.Collections.Generic.IReadOnlyList<Inno.Rendering.Assets.RenderTextureArtifactSlot> Inno.Rendering.Assets.IRenderTextureArtifactSource.textureArtifacts { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Textures/IRenderTextureArtifactSource.cs#L16) | Gets the complete immutable set of stable texture slots owned by the current asset content. | — | — |
| <a id="api-b2f5fb3887f6e592"></a>[`public interface Inno.Rendering.Assets.IRenderTextureArtifactSource`](../../src/services/rendering/Inno.Rendering.Assets/Textures/IRenderTextureArtifactSource.cs#L11) | Exposes one or more portable image artifacts owned by an imported asset. | — | — |

### `Inno.Rendering.Assets.MaterialAsset`

<!-- api: T:Inno.Rendering.Assets.MaterialAsset 862f5cb506dfa44aa9722ffc1a2cc2bc5cb1129845c34612e6cdef921c8b95a7 -->
<!-- api: P:Inno.Rendering.Assets.MaterialAsset.shader 0b4e78b0e6a2857f8ab4bb7a03442e3fb2c41964bf54edcf84eeabfdf3551175 -->
<!-- api: P:Inno.Rendering.Assets.MaterialAsset.techniqueId f8a389a2d0e2678ecb8113dad9a6c00de84226bbde292e88f277eeb75b343642 -->
<!-- api: P:Inno.Rendering.Assets.MaterialAsset.properties a477baa93a426ea53c628ca5ae9812e2705702dbdb83805f23909a2a28ed97ad -->
<!-- api: P:Inno.Rendering.Assets.MaterialAsset.keywords 4da3a38e630a98dfeb81ba0d4ef6f05ddb0dad3a99bafdac36338ca0aa4078ff -->
<!-- api: P:Inno.Rendering.Assets.MaterialAsset.metadata 0b546fa75d5fbbb0bfa4b38739025803c943a8f67ecea036f7d7d7c952535ae5 -->
<!-- api: M:Inno.Rendering.Assets.MaterialAsset.Set(Inno.Rendering.ShaderPropertyId,Inno.Rendering.Assets.MaterialValue) 7cd05590a5ccaabb746199e9eaa35d2609953fa2fe209e22ee7696d4c10bcf25 -->
<!-- api: M:Inno.Rendering.Assets.MaterialAsset.ReplaceProperties(System.Collections.Generic.IEnumerable{Inno.Rendering.Assets.MaterialPropertyEntry}) b8d9775221c819fe32355dc41e22ceb5b96a24f771e9cd435b68013af822ffb0 -->
<!-- api: M:Inno.Rendering.Assets.MaterialAsset.TryGet(Inno.Rendering.ShaderPropertyId,Inno.Rendering.Assets.MaterialValue@) 601a37223c9546a55bd5daa72aa02d5aa34c84b8ff1e09fac6f6b4732995ac51 -->
<!-- api: M:Inno.Rendering.Assets.MaterialAsset.SetKeyword(System.String,System.Boolean) 8033e9bef29ce4b47ba77d3e7e9a3344a98fd11eb1099eb8aa1cadb7a106a41f -->
<!-- api: M:Inno.Rendering.Assets.MaterialAsset.SetMetadata(System.String,System.String) a4ef968283498cd1d3fb6ec3b48b4b1cd2fd2a1930a72005511eb502c3da2f8b -->
<!-- api: M:Inno.Rendering.Assets.MaterialAsset.TryGetMetadata(System.String,System.String@) d5a2febb51216d391a7ecf58502eb952c4d0f4b6d4215d743d486b6dacf6c2e6 -->
<!-- api: M:Inno.Rendering.Assets.MaterialAsset.RemoveMetadata(System.String) fde020407dec974f98d768f6ae3555408e6977dba7b932b6984c8f650c6dcde0 -->
<!-- api: M:Inno.Rendering.Assets.MaterialAsset.#ctor 67104d76cc149e8691294b217f3690c3cf3b1bb7f6a24f2c0f1e79401e304940 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-ac9b0e8ebcd40c15"></a>[`public Inno.Rendering.Assets.MaterialAsset.MaterialAsset()`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialAsset.cs#L16) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-d8b9a2fd4ffe0a9d"></a>[`public bool Inno.Rendering.Assets.MaterialAsset.RemoveMetadata(string key)`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialAsset.cs#L205) | Removes one provider-defined metadata value.<br>返回：true when an entry was removed.<br>`key`：Stable metadata key. | — | — |
| <a id="api-3d2622ed654910ce"></a>[`public void Inno.Rendering.Assets.MaterialAsset.ReplaceProperties(System.Collections.Generic.IEnumerable<Inno.Rendering.Assets.MaterialPropertyEntry> properties)`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialAsset.cs#L86) | Atomically replaces all persistent material values with an isolated, deterministically ordered set.<br>`properties`：Complete replacement property set. Duplicate identifiers are rejected. | — | — |
| <a id="api-67c11e0f814e53bf"></a>[`public void Inno.Rendering.Assets.MaterialAsset.Set(Inno.Rendering.ShaderPropertyId id, Inno.Rendering.Assets.MaterialValue value)`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialAsset.cs#L63) | Creates or replaces one material property.<br>`id`：Stable shader property identifier.<br>`value`：Neutral material value. | — | — |
| <a id="api-1184e812c88be77f"></a>[`public void Inno.Rendering.Assets.MaterialAsset.SetKeyword(string keyword, bool enabled)`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialAsset.cs#L137) | Enables or disables a declared static keyword option.<br>`keyword`：Stable keyword option identifier.<br>`enabled`：Whether the option should be enabled. | — | — |
| <a id="api-9c6732d8dbbd19e1"></a>[`public void Inno.Rendering.Assets.MaterialAsset.SetMetadata(string key, string value)`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialAsset.cs#L159) | Creates or replaces one provider-defined metadata value.<br>`key`：Stable metadata key.<br>`value`：Provider-defined value. | — | — |
| <a id="api-5c1ef544616ed7ee"></a>[`public bool Inno.Rendering.Assets.MaterialAsset.TryGet(Inno.Rendering.ShaderPropertyId id, out Inno.Rendering.Assets.MaterialValue value)`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialAsset.cs#L119) | Tries to read one material property.<br>返回：true when the property exists.<br>`id`：Stable shader property identifier.<br>`value`：Receives the neutral value when present. | — | — |
| <a id="api-7fa1b87c5169bed0"></a>[`public bool Inno.Rendering.Assets.MaterialAsset.TryGetMetadata(string key, out string? value)`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialAsset.cs#L187) | Tries to read one provider-defined metadata value.<br>返回：true when the key exists.<br>`key`：Stable metadata key.<br>`value`：Receives the metadata value when present. | — | — |
| <a id="api-dd64080b86aede49"></a>[`public System.Collections.Generic.IReadOnlyList<string> Inno.Rendering.Assets.MaterialAsset.keywords { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialAsset.cs#L47) | Gets enabled stable keyword option identifiers. | — | — |
| <a id="api-8d9829469d5dd192"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Assets.MaterialMetadataEntry> Inno.Rendering.Assets.MaterialAsset.metadata { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialAsset.cs#L52) | Gets open provider-defined metadata. | — | — |
| <a id="api-b2d9699ea7ff7bcb"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Assets.MaterialPropertyEntry> Inno.Rendering.Assets.MaterialAsset.properties { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialAsset.cs#L42) | Gets persistent material values in stable insertion order. | — | — |
| <a id="api-fec0889c21bd1b3e"></a>[`public Inno.Rendering.Assets.ShaderAsset? Inno.Rendering.Assets.MaterialAsset.shader { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialAsset.cs#L31) | Gets or sets the referenced shader asset. | — | — |
| <a id="api-ec64bfd425c2d5ac"></a>[`public Inno.Rendering.Assets.ShaderTechniqueId Inno.Rendering.Assets.MaterialAsset.techniqueId { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialAsset.cs#L37) | Gets or sets an optional explicitly selected technique. | — | — |
| <a id="api-adbc4ae5dbc9973a"></a>[`public class Inno.Rendering.Assets.MaterialAsset : Inno.Assets.AssetObject`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialAsset.cs#L16) | Represents material state keyed by stable shader property identifiers. | — | — |

### `Inno.Rendering.Assets.MaterialMetadataEntry`

<!-- api: T:Inno.Rendering.Assets.MaterialMetadataEntry 92089c615a7a4fefefa0e873a5b21469e691c43a4f10f45d521600f46449cddd -->
<!-- api: M:Inno.Rendering.Assets.MaterialMetadataEntry.#ctor(System.String,System.String) 385d4f51bd651d279c9edbe6d5cc7024f62718244641f5583478e659e664447f -->
<!-- api: P:Inno.Rendering.Assets.MaterialMetadataEntry.key c5650f2f25bd64728c88964642027401b0bdef9023daecc1af32c759c52c0256 -->
<!-- api: P:Inno.Rendering.Assets.MaterialMetadataEntry.value d148082e5f5cd45adb228fe91dbc4b7fbaefefd72c8e0abb104bd006ad647be6 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b8137660d9f79110"></a>[`public Inno.Rendering.Assets.MaterialMetadataEntry.MaterialMetadataEntry(string key, string value)`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialMetadataEntry.cs#L27) | Creates a material metadata entry.<br>`key`：Stable provider-defined key.<br>`value`：Provider-defined value. | — | — |
| <a id="api-814c4db2d4500745"></a>[`public string Inno.Rendering.Assets.MaterialMetadataEntry.key { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialMetadataEntry.cs#L39) | Gets or sets the stable metadata key. | — | — |
| <a id="api-999cff9163908dc7"></a>[`public string Inno.Rendering.Assets.MaterialMetadataEntry.value { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialMetadataEntry.cs#L44) | Gets or sets the metadata value. | — | — |
| <a id="api-c0d60a5cc501ff4e"></a>[`public struct Inno.Rendering.Assets.MaterialMetadataEntry`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialMetadataEntry.cs#L16) | Stores one open material metadata key and value. | — | — |

### `Inno.Rendering.Assets.MaterialPassResolution`

<!-- api: T:Inno.Rendering.Assets.MaterialPassResolution 4399cf9014425e092e730654a69a1aee44fc182fc557e64d2caee0c153fa8a50 -->
<!-- api: M:Inno.Rendering.Assets.MaterialPassResolution.#ctor(Inno.Rendering.Assets.ShaderTechniqueDefinition,Inno.Rendering.Assets.ShaderPassDefinition) 41671bb0aee0bca04cd3e10d61bdc1eeddbd5ba89a327c95a73666bc027e84e2 -->
<!-- api: P:Inno.Rendering.Assets.MaterialPassResolution.technique c61d69af84db0f15e4b58a57d884fe579e083a3f7a8c83156ad9728a3826f4da -->
<!-- api: P:Inno.Rendering.Assets.MaterialPassResolution.pass 91e97a4a4fd13d2cb86d0914494c7bdc6158fbf0941d117d6a8a8b8c9308dc71 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-70f28197c477ac20"></a>[`public Inno.Rendering.Assets.MaterialPassResolution.MaterialPassResolution(Inno.Rendering.Assets.ShaderTechniqueDefinition technique, Inno.Rendering.Assets.ShaderPassDefinition pass)`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialPassResolution.cs#L30) | Creates a material pass resolution.<br>`technique`：Selected open-contract technique.<br>`pass`：Concrete pass mapped to the requested role. | — | — |
| <a id="api-2fef39e06455294b"></a>[`public Inno.Rendering.Assets.ShaderPassDefinition Inno.Rendering.Assets.MaterialPassResolution.pass { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialPassResolution.cs#L46) | Gets the concrete shader pass with detached metadata storage. | — | — |
| <a id="api-abdd17436469c07c"></a>[`public Inno.Rendering.Assets.ShaderTechniqueDefinition Inno.Rendering.Assets.MaterialPassResolution.technique { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialPassResolution.cs#L41) | Gets the selected technique with detached role mapping storage. | — | — |
| <a id="api-cf8a6dab8f1aaaa2"></a>[`public sealed class Inno.Rendering.Assets.MaterialPassResolution`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialPassResolution.cs#L16) | Describes one capability-compatible material pass selection. | — | — |

### `Inno.Rendering.Assets.MaterialPassResolver`

<!-- api: T:Inno.Rendering.Assets.MaterialPassResolver 9dfc972964639be68e9bb7a31992b41d20543d84d21f9d9f14e2d3918722ddc9 -->
<!-- api: M:Inno.Rendering.Assets.MaterialPassResolver.Resolve(Inno.Rendering.Assets.MaterialAsset,Inno.Rendering.Assets.ShaderContractId,Inno.Rendering.Assets.ShaderPassRoleId,Inno.Rendering.GraphicsCapabilities) 211a20a45dbca1172d09d0360cf6d5173b1c5d4087c2c5e5825d3c427b3f62f1 -->
<!-- api: M:Inno.Rendering.Assets.MaterialPassResolver.Resolve(Inno.Rendering.Assets.ShaderDefinition,Inno.Rendering.Assets.ShaderTechniqueId,Inno.Rendering.Assets.ShaderContractId,Inno.Rendering.Assets.ShaderPassRoleId,Inno.Rendering.GraphicsCapabilities) 4e5c2090d5307563d94e90b53359f0395baef7b770171a873d8af6b4b40b4126 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-8f317896cc563bf5"></a>[`public static Inno.Rendering.Assets.MaterialPassResolution? Inno.Rendering.Assets.MaterialPassResolver.Resolve(Inno.Rendering.Assets.MaterialAsset material, Inno.Rendering.Assets.ShaderContractId contractId, Inno.Rendering.Assets.ShaderPassRoleId passRoleId, Inno.Rendering.GraphicsCapabilities capabilities)`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialPassResolver.cs#L36) | Resolves one capability-compatible pass.<br>返回：The selected technique and pass, or null when no compatible mapping exists.<br>`material`：Material whose shader and technique should be queried.<br>`contractId`：Rendering-provider contract required by the caller.<br>`passRoleId`：Provider-defined pass role required by the caller.<br>`capabilities`：Current device capability snapshot. | — | — |
| <a id="api-df3f2e6378bd20e7"></a>[`public static Inno.Rendering.Assets.MaterialPassResolution? Inno.Rendering.Assets.MaterialPassResolver.Resolve(Inno.Rendering.Assets.ShaderDefinition definition, Inno.Rendering.Assets.ShaderTechniqueId techniqueId, Inno.Rendering.Assets.ShaderContractId contractId, Inno.Rendering.Assets.ShaderPassRoleId passRoleId, Inno.Rendering.GraphicsCapabilities capabilities)`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialPassResolver.cs#L71) | Resolves a role against an exact published shader contract rather than a possibly newer authoring asset.<br>返回：The matching technique and pass, or null when this publication has no compatible mapping.<br>`definition`：The immutable publication's detached material contract.<br>`techniqueId`：Explicit material technique, or an invalid ID to select the first compatible technique.<br>`contractId`：Open rendering contract required by the caller.<br>`passRoleId`：Open pass role required by the caller.<br>`capabilities`：Current target capability snapshot. | — | `System.ArgumentException`：A required contract or role identity is invalid. |
| <a id="api-cf4aaa51e1b3cada"></a>[`public static class Inno.Rendering.Assets.MaterialPassResolver`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialPassResolver.cs#L16) | Resolves materials through open provider-owned shader contracts and roles. | — | — |

### `Inno.Rendering.Assets.MaterialPropertyBlock`

<!-- api: T:Inno.Rendering.Assets.MaterialPropertyBlock 3e3e2b185ce7a1b0861d5169b5398a2af8438f96c61696519ed19c4086a1e5fb -->
<!-- api: P:Inno.Rendering.Assets.MaterialPropertyBlock.count 9668d4ff212c8ada2453985e6662a5351edf0f6dc6da2e8335307c9b447f64f1 -->
<!-- api: M:Inno.Rendering.Assets.MaterialPropertyBlock.Set(Inno.Rendering.ShaderPropertyId,Inno.Rendering.Assets.MaterialValue) 12b9737882e402fd92e5509f0eec2ea729fe01ccb135601c95128d586ec997fb -->
<!-- api: M:Inno.Rendering.Assets.MaterialPropertyBlock.TryGet(Inno.Rendering.ShaderPropertyId,Inno.Rendering.Assets.MaterialValue@) 30daddf7d1fcf4856435bf5b7486dc189a4a6a042b33543683f936c721e0b5fd -->
<!-- api: M:Inno.Rendering.Assets.MaterialPropertyBlock.Clear 8e75b50de8a8b719aef63ac48185257c6d111058dcdaa4f3f64d0b177ac82b5c -->
<!-- api: M:Inno.Rendering.Assets.MaterialPropertyBlock.#ctor 83faed0d4b4b4f56b38da36b3e4e3b3c56171a2d0f2e1de8714484975ebd4805 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-1056febf2d4a2180"></a>[`public Inno.Rendering.Assets.MaterialPropertyBlock.MaterialPropertyBlock()`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialPropertyBlock.cs#L16) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-83f7cfd83497a82b"></a>[`public void Inno.Rendering.Assets.MaterialPropertyBlock.Clear()`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialPropertyBlock.cs#L63) | Removes all frame-local overrides. | — | — |
| <a id="api-892d3dcc85cd81bb"></a>[`public void Inno.Rendering.Assets.MaterialPropertyBlock.Set(Inno.Rendering.ShaderPropertyId id, Inno.Rendering.Assets.MaterialValue value)`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialPropertyBlock.cs#L34) | Creates or replaces one frame-local override.<br>`id`：Stable shader property identifier.<br>`value`：Neutral material value. | — | — |
| <a id="api-e39eefcfa94cd32e"></a>[`public bool Inno.Rendering.Assets.MaterialPropertyBlock.TryGet(Inno.Rendering.ShaderPropertyId id, out Inno.Rendering.Assets.MaterialValue value)`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialPropertyBlock.cs#L55) | Tries to read one frame-local override.<br>返回：true when the override exists.<br>`id`：Stable shader property identifier.<br>`value`：The concrete value read or transformed by this operation. | — | — |
| <a id="api-df8f89f944a3a970"></a>[`public int Inno.Rendering.Assets.MaterialPropertyBlock.count { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialPropertyBlock.cs#L23) | Gets the number of active overrides. | — | — |
| <a id="api-c9cbbc88a18bc0a6"></a>[`public sealed class Inno.Rendering.Assets.MaterialPropertyBlock`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialPropertyBlock.cs#L16) | Holds frame-local material overrides without modifying a shared asset. | — | — |

### `Inno.Rendering.Assets.MaterialPropertyEntry`

<!-- api: T:Inno.Rendering.Assets.MaterialPropertyEntry 18e02c095019d83f13e3401c0921d0d10a07b66e955360024dacdb0cd560df90 -->
<!-- api: M:Inno.Rendering.Assets.MaterialPropertyEntry.#ctor(Inno.Rendering.ShaderPropertyId,Inno.Rendering.Assets.MaterialValue) e53c46f6cd7a162ba1a147b12a43076e22d67b8cf0ec1db0a34f0854596cdcef -->
<!-- api: P:Inno.Rendering.Assets.MaterialPropertyEntry.id 2d92627a38aface6a53c9c1678eaabad531bfb3bda6db004fced1fd248b14fc6 -->
<!-- api: P:Inno.Rendering.Assets.MaterialPropertyEntry.value 7751ab458bb3b6d02c5da5e5ff9df3d5d7d7461b27289ebac2131055561c5899 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-7f0a25efd89db285"></a>[`public Inno.Rendering.Assets.MaterialPropertyEntry.MaterialPropertyEntry(Inno.Rendering.ShaderPropertyId id, Inno.Rendering.Assets.MaterialValue value)`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialPropertyEntry.cs#L27) | Creates a persistent material property entry.<br>`id`：Stable shader property identifier.<br>`value`：Neutral material value. | — | — |
| <a id="api-ef60f89f3e4aad20"></a>[`public Inno.Rendering.ShaderPropertyId Inno.Rendering.Assets.MaterialPropertyEntry.id { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialPropertyEntry.cs#L40) | Gets or sets the stable shader property identifier. | — | — |
| <a id="api-ecbe80923703aef9"></a>[`public Inno.Rendering.Assets.MaterialValue Inno.Rendering.Assets.MaterialPropertyEntry.value { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialPropertyEntry.cs#L45) | Gets or sets the neutral material value. | — | — |
| <a id="api-90d6b484019fb073"></a>[`public struct Inno.Rendering.Assets.MaterialPropertyEntry`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialPropertyEntry.cs#L16) | Stores one persistent material property entry. | — | — |

### `Inno.Rendering.Assets.MaterialValue`

<!-- api: T:Inno.Rendering.Assets.MaterialValue c69810440e1e318b1fb29b855f4460d69dc2154b752edb6a423013ffef1301eb -->
<!-- api: P:Inno.Rendering.Assets.MaterialValue.kind 1d70c42ff56374b89ad6a457592fb26dfece1ef19efdb8dbda840796875394ec -->
<!-- api: P:Inno.Rendering.Assets.MaterialValue.vector 6f920de5f303d9996411eca470cc1072f65d4b909fe9f50ff5c41b5c6abd2822 -->
<!-- api: P:Inno.Rendering.Assets.MaterialValue.matrix ce58736277a5062708c5f9d2a542309d7e5d7fcfcb88b87cb977f60bd2323bf9 -->
<!-- api: P:Inno.Rendering.Assets.MaterialValue.texture 8bd6453e78f42abc3a3d4dc09e6085d6e3b0be34243752cb9fe46db7d2e63aa0 -->
<!-- api: P:Inno.Rendering.Assets.MaterialValue.sampler e2f8b41a0b1ff5dfa0e7981e02d4a7c16e38e0f3ab738270700941722293a9e5 -->
<!-- api: M:Inno.Rendering.Assets.MaterialValue.FromFloat(System.Single) 0fde86519eed77753db46b20ba061459a278d0ea35e071d3a2a100eefd3406f1 -->
<!-- api: M:Inno.Rendering.Assets.MaterialValue.FromVector(Inno.Core.Mathematics.Vector4) 6c88cb85728522632419e87dcbe2da2c9803ae6d2bc92ae0a9982834a953ad6c -->
<!-- api: M:Inno.Rendering.Assets.MaterialValue.FromColor(Inno.Core.Mathematics.Color) 537f43efbe628c3f0e3022e4ad057c5f1e3b27003fb81fee6738bda5db437894 -->
<!-- api: M:Inno.Rendering.Assets.MaterialValue.FromMatrix(Inno.Core.Mathematics.Matrix) 6770d75cd8162becefb6f2b1fc0fd2be16dbcb6dcc1980c9a4589e12453c23e2 -->
<!-- api: M:Inno.Rendering.Assets.MaterialValue.FromTexture(Inno.Rendering.Assets.TextureAsset,System.Nullable{Inno.Rendering.RenderSamplerState}) 3bb67e0eeb0027be4ba51c393552115f4a4fd1f54a4d7c0b8517770379baef81 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-6789188502c51c72"></a>[`public static Inno.Rendering.Assets.MaterialValue Inno.Rendering.Assets.MaterialValue.FromColor(Inno.Core.Mathematics.Color value)`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialValue.cs#L117) | Creates a linear color material value.<br>返回：A color material value.<br>`value`：Color value. | — | — |
| <a id="api-a104027959c41d28"></a>[`public static Inno.Rendering.Assets.MaterialValue Inno.Rendering.Assets.MaterialValue.FromFloat(float value)`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialValue.cs#L94) | Creates a scalar material value.<br>返回：A scalar material value.<br>`value`：Scalar value. | — | — |
| <a id="api-b1234c6a14712ac8"></a>[`public static Inno.Rendering.Assets.MaterialValue Inno.Rendering.Assets.MaterialValue.FromMatrix(Inno.Core.Mathematics.Matrix value)`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialValue.cs#L129) | Creates a matrix material value.<br>返回：A matrix material value.<br>`value`：Four-by-four matrix value. | — | — |
| <a id="api-bf098e804495ba33"></a>[`public static Inno.Rendering.Assets.MaterialValue Inno.Rendering.Assets.MaterialValue.FromTexture(Inno.Rendering.Assets.TextureAsset value, Inno.Rendering.RenderSamplerState? sampler = null)`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialValue.cs#L143) | Creates a texture material value.<br>返回：A texture material value.<br>`value`：Texture asset reference.<br>`sampler`：Optional sampler state; linear clamp is used when omitted. | — | — |
| <a id="api-ec642b788e646066"></a>[`public static Inno.Rendering.Assets.MaterialValue Inno.Rendering.Assets.MaterialValue.FromVector(Inno.Core.Mathematics.Vector4 value)`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialValue.cs#L106) | Creates a vector material value.<br>返回：A vector material value.<br>`value`：Vector value. | — | — |
| <a id="api-bb96833d7a076ffc"></a>[`public Inno.Rendering.Assets.MaterialValueKind Inno.Rendering.Assets.MaterialValue.kind { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialValue.cs#L63) | Gets or sets the stored value kind. | — | — |
| <a id="api-f7cfb27bab7f464b"></a>[`public Inno.Core.Mathematics.Matrix Inno.Rendering.Assets.MaterialValue.matrix { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialValue.cs#L73) | Gets or sets the matrix value. | — | — |
| <a id="api-13815c0ff39de1ba"></a>[`public Inno.Rendering.RenderSamplerState Inno.Rendering.Assets.MaterialValue.sampler { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialValue.cs#L83) | Gets or sets the sampler used when this value stores a texture. | — | — |
| <a id="api-c745d26ac231e324"></a>[`public Inno.Rendering.Assets.TextureAsset? Inno.Rendering.Assets.MaterialValue.texture { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialValue.cs#L78) | Gets or sets the texture reference. | — | — |
| <a id="api-24cfbf59993d130c"></a>[`public Inno.Core.Mathematics.Vector4 Inno.Rendering.Assets.MaterialValue.vector { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialValue.cs#L68) | Gets or sets scalar, vector, or color components. | — | — |
| <a id="api-15e9a92bb41ac1fb"></a>[`public struct Inno.Rendering.Assets.MaterialValue`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialValue.cs#L44) | Stores one native-serializable material value without a GPU binding. | — | — |

### `Inno.Rendering.Assets.MaterialValueKind`

<!-- api: T:Inno.Rendering.Assets.MaterialValueKind 9ae971b7f35a5408c2d13551eb0e859ca8c48ed4f7bafbdcc8ac9af5176d804a -->
<!-- api: F:Inno.Rendering.Assets.MaterialValueKind.Float 227477f1ee519d1664177f405ceefb18effdcf85dd887b596e6cd470273a10ad -->
<!-- api: F:Inno.Rendering.Assets.MaterialValueKind.Vector 6dcc5e052f814af1d16f1f1d9405f60f9a7746bd1c55d0ce6df877fe201db91a -->
<!-- api: F:Inno.Rendering.Assets.MaterialValueKind.Color d838cad55a90bbd0f8e02736a285d8599b05faf02c03193598aa058ed281041a -->
<!-- api: F:Inno.Rendering.Assets.MaterialValueKind.Matrix ab0cd8dff90cb7a5800baa56554fca48175c1e2fc96fa758bc01ca7cb29c1f73 -->
<!-- api: F:Inno.Rendering.Assets.MaterialValueKind.Texture 8223fff295b740d9da764bbbba7c9c44dc5faabcc0977d7fa2ec3b48604eb733 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-91d82abcb8829131"></a>[`Inno.Rendering.Assets.MaterialValueKind.Color = 2`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialValue.cs#L29) | Linear color value. | — | — |
| <a id="api-16bab1789d30b6fc"></a>[`Inno.Rendering.Assets.MaterialValueKind.Float = 0`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialValue.cs#L21) | Scalar floating-point value. | — | — |
| <a id="api-d1a05d99975f7ffb"></a>[`Inno.Rendering.Assets.MaterialValueKind.Matrix = 3`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialValue.cs#L33) | Four-by-four matrix value. | — | — |
| <a id="api-5ae77afa0e82cfba"></a>[`Inno.Rendering.Assets.MaterialValueKind.Texture = 4`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialValue.cs#L37) | Texture asset reference. | — | — |
| <a id="api-a8c8a02be9b3f40b"></a>[`Inno.Rendering.Assets.MaterialValueKind.Vector = 1`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialValue.cs#L25) | Four-component vector value. | — | — |
| <a id="api-060c8d048d970c53"></a>[`public enum Inno.Rendering.Assets.MaterialValueKind : int`](../../src/services/rendering/Inno.Rendering.Assets/Materials/MaterialValue.cs#L16) | Identifies the neutral value stored by a material property. | — | — |

### `Inno.Rendering.Assets.RenderFeatureConfiguration`

<!-- api: T:Inno.Rendering.Assets.RenderFeatureConfiguration a7a61e8145fce74b794d8147e07a51cf42730279f0173571399709f3501ea5b2 -->
<!-- api: M:Inno.Rendering.Assets.RenderFeatureConfiguration.#ctor 3a20f4bb2f192ec02de993902d8cea771dba6e666d966a13f1e60b322dd9e0e0 -->
<!-- api: M:Inno.Rendering.Assets.RenderFeatureConfiguration.#ctor(System.String,System.Nullable{Inno.Rendering.Assets.SerializedRenderExtensionState},System.Boolean) 2e5f4bbccfdcdfb8bf991840fbd9b0c1cb29abff59991fe9a266ccd4a5f692f9 -->
<!-- api: P:Inno.Rendering.Assets.RenderFeatureConfiguration.featureTypeId 4173e3a6ae4e220fc2d8f6b349db948262b3e9ad6f05995c8b72dca56ec77cc5 -->
<!-- api: P:Inno.Rendering.Assets.RenderFeatureConfiguration.state 3994dd96169b8510d0792bd4bc68ca71314d7e90527f01ebe039df073c61759f -->
<!-- api: P:Inno.Rendering.Assets.RenderFeatureConfiguration.enabled 2076772094d3b96a0c2e39fbc0cba095d9d7c4106c7b0b631b823e45555b0570 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-892032b7a806ed61"></a>[`public Inno.Rendering.Assets.RenderFeatureConfiguration.RenderFeatureConfiguration()`](../../src/services/rendering/Inno.Rendering.Assets/Pipelines/RenderFeatureConfiguration.cs#L21) | Creates an empty feature configuration for deserialization. | — | — |
| <a id="api-ba303c00443c79ad"></a>[`public Inno.Rendering.Assets.RenderFeatureConfiguration.RenderFeatureConfiguration(string featureTypeId, Inno.Rendering.Assets.SerializedRenderExtensionState? state = null, bool enabled = true)`](../../src/services/rendering/Inno.Rendering.Assets/Pipelines/RenderFeatureConfiguration.cs#L37) | Creates a feature configuration.<br>`featureTypeId`：Globally stable feature extension identifier.<br>`state`：Optional reload-safe settings state.<br>`enabled`：Whether the feature participates in graph building. | — | — |
| <a id="api-f21b4ccb370793e2"></a>[`public bool Inno.Rendering.Assets.RenderFeatureConfiguration.enabled { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Pipelines/RenderFeatureConfiguration.cs#L64) | Gets or sets whether the feature participates in graph building. | — | — |
| <a id="api-705c4a625ae909cb"></a>[`public string Inno.Rendering.Assets.RenderFeatureConfiguration.featureTypeId { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Pipelines/RenderFeatureConfiguration.cs#L52) | Gets or sets the stable feature extension identifier. | — | — |
| <a id="api-9ea415188e3be73e"></a>[`public Inno.Rendering.Assets.SerializedRenderExtensionState Inno.Rendering.Assets.RenderFeatureConfiguration.state { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Pipelines/RenderFeatureConfiguration.cs#L58) | Gets or sets reload-safe settings state. | — | — |
| <a id="api-10a65b652e493154"></a>[`public struct Inno.Rendering.Assets.RenderFeatureConfiguration`](../../src/services/rendering/Inno.Rendering.Assets/Pipelines/RenderFeatureConfiguration.cs#L16) | Stores one ordered feature extension selection using only stable data. | — | — |

### `Inno.Rendering.Assets.RenderPipelineAsset`

<!-- api: T:Inno.Rendering.Assets.RenderPipelineAsset 34eb825d23df06915280ae1158b75ac23c60c6201fa23710a480e07f64026f4d -->
<!-- api: P:Inno.Rendering.Assets.RenderPipelineAsset.pipelineTypeId 2c8588ee88f1eb982168e0cbe7904b9b66a9d1dc679e48ed6962f21f943d5b90 -->
<!-- api: P:Inno.Rendering.Assets.RenderPipelineAsset.pipelineState d628cd7a91cb0c3997553f11790cd69c2e8a87d37a434ce6d7bec44e5be4e2c6 -->
<!-- api: P:Inno.Rendering.Assets.RenderPipelineAsset.features 43a31936e3b28170d75f9f8e8e88105f11bd87f69805dce4fcb68d24dc284c3b -->
<!-- api: M:Inno.Rendering.Assets.RenderPipelineAsset.SetFeatures(System.Collections.Generic.IEnumerable{Inno.Rendering.Assets.RenderFeatureConfiguration}) 0f93f42dccc3a7415556c9b7ebfa2a23922dbf38c3bcd10634278dc5f0ac9bef -->
<!-- api: M:Inno.Rendering.Assets.RenderPipelineAsset.#ctor 17688ac825bd8aab3847ff5765c6f865baa6e082051a0b3866759ac258cd5bd0 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-4e7b9bb9a39ec53e"></a>[`public Inno.Rendering.Assets.RenderPipelineAsset.RenderPipelineAsset()`](../../src/services/rendering/Inno.Rendering.Assets/Pipelines/RenderPipelineAsset.cs#L16) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-4f43b8b6bdc8d41a"></a>[`public void Inno.Rendering.Assets.RenderPipelineAsset.SetFeatures(System.Collections.Generic.IEnumerable<Inno.Rendering.Assets.RenderFeatureConfiguration> features)`](../../src/services/rendering/Inno.Rendering.Assets/Pipelines/RenderPipelineAsset.cs#L52) | Replaces ordered feature configurations.<br>`features`：Complete ordered feature configuration set. | — | — |
| <a id="api-6158ae0f9c80f8cf"></a>[`public Inno.Rendering.Assets.RenderFeatureConfiguration[] Inno.Rendering.Assets.RenderPipelineAsset.features { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Pipelines/RenderPipelineAsset.cs#L36) | Gets ordered feature configurations. | — | — |
| <a id="api-b75de605c6a94013"></a>[`public Inno.Rendering.Assets.SerializedRenderExtensionState Inno.Rendering.Assets.RenderPipelineAsset.pipelineState { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Pipelines/RenderPipelineAsset.cs#L30) | Gets or sets reload-safe pipeline settings. | — | — |
| <a id="api-a962969e3c10e175"></a>[`public string Inno.Rendering.Assets.RenderPipelineAsset.pipelineTypeId { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Pipelines/RenderPipelineAsset.cs#L24) | Gets or sets the globally stable pipeline extension identifier. | — | — |
| <a id="api-fbedbcde50eb1263"></a>[`public sealed class Inno.Rendering.Assets.RenderPipelineAsset : Inno.Assets.AssetObject`](../../src/services/rendering/Inno.Rendering.Assets/Pipelines/RenderPipelineAsset.cs#L16) | Selects a pipeline extension and ordered feature configuration without defining a render path. | — | — |

### `Inno.Rendering.Assets.RenderShaderArtifact`

<!-- api: T:Inno.Rendering.Assets.RenderShaderArtifact 03633b1470561ed38ad4bf021401fb05d2761d358baccc5961d51fad89ba3f0d -->
<!-- api: M:Inno.Rendering.Assets.RenderShaderArtifact.#ctor(System.String,System.String,Inno.Rendering.Assets.RenderShaderVariant,Inno.Rendering.ShaderInterface,System.Collections.Generic.IReadOnlyList{Inno.Rendering.Assets.RenderShaderPassArtifact},System.ReadOnlySpan{System.Byte}) 074c62f3b4aab0f874d9ecea465bbd2e2c9e9483ac38430accd2930efe8c7d40 -->
<!-- api: P:Inno.Rendering.Assets.RenderShaderArtifact.definitionData 6ffbd8c9ffadc95dd13234384a370b58270ab0e9c8767a4177f1a57b24bbeaf9 -->
<!-- api: P:Inno.Rendering.Assets.RenderShaderArtifact.contentHash 245d15c2b8d769235e3150f17a56b3c268dced5c60825b06d848a129a7d3b989 -->
<!-- api: P:Inno.Rendering.Assets.RenderShaderArtifact.shaderName f33b7f1325e9b2ed8a796e33e0112285d266d94ba71f3365680385b9a86bcb2b -->
<!-- api: P:Inno.Rendering.Assets.RenderShaderArtifact.targetKey 7d2a1b108437b8cf9e7cc18b23b8eff29370167c6cabe484c41e78288fa1e8a5 -->
<!-- api: P:Inno.Rendering.Assets.RenderShaderArtifact.variant e606820a505b839bb2919c1bb3bfd6571f56eaf128d23fe960fa9f23fd252f28 -->
<!-- api: P:Inno.Rendering.Assets.RenderShaderArtifact.shaderInterface 2473e8858d15648f8cabf7e7553f1f236e331169c903583fd3dcbb040909716b -->
<!-- api: P:Inno.Rendering.Assets.RenderShaderArtifact.passes b2f9ed8e8a4d1382afdb86765cd72e36fed28581370c9438317494ecacfbe358 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-96104c474f5a1fe9"></a>[`public Inno.Rendering.Assets.RenderShaderArtifact.RenderShaderArtifact(string shaderName, string targetKey, Inno.Rendering.Assets.RenderShaderVariant variant, Inno.Rendering.ShaderInterface shaderInterface, System.Collections.Generic.IReadOnlyList<Inno.Rendering.Assets.RenderShaderPassArtifact> passes, System.ReadOnlySpan<byte> definitionData)`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderArtifact.cs#L42) | Creates a complete deployed shader artifact.<br>`shaderName`：The stable shader name expected by the runtime asset definition.<br>`targetKey`：The target compiler profile and policy identity.<br>`variant`：The canonical static keyword selection.<br>`shaderInterface`：The complete validated shader resource binding contract.<br>`passes`：Every source-free pass available to the runtime shader definition.<br>`definitionData`：Native-serialized material, keyword, technique and pass contract captured with these programs; contains stable asset references only. | — | `System.ArgumentException`：Thrown when a required identity is empty, no pass exists, or pass names are duplicated. |
| <a id="api-bb9c02df10a3741f"></a>[`public string Inno.Rendering.Assets.RenderShaderArtifact.contentHash { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderArtifact.cs#L78) | Gets a semantic content identity covering the contract, variant, target, bindings and all compiled stages. | — | — |
| <a id="api-07e2aea81b4691d0"></a>[`public System.ReadOnlyMemory<byte> Inno.Rendering.Assets.RenderShaderArtifact.definitionData { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderArtifact.cs#L73) | Gets the immutable native-serialized runtime contract paired with this exact program publication. | — | — |
| <a id="api-22179aa447261964"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Assets.RenderShaderPassArtifact> Inno.Rendering.Assets.RenderShaderArtifact.passes { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderArtifact.cs#L103) | Gets every source-free pass available to the runtime shader definition. | — | — |
| <a id="api-b33b36b8208f8ca5"></a>[`public Inno.Rendering.ShaderInterface Inno.Rendering.Assets.RenderShaderArtifact.shaderInterface { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderArtifact.cs#L98) | Gets the complete validated shader resource binding contract. | — | — |
| <a id="api-3b41758a16d22477"></a>[`public string Inno.Rendering.Assets.RenderShaderArtifact.shaderName { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderArtifact.cs#L83) | Gets the stable shader name expected by the runtime asset definition. | — | — |
| <a id="api-fe27a56fffaa32f2"></a>[`public string Inno.Rendering.Assets.RenderShaderArtifact.targetKey { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderArtifact.cs#L88) | Gets the target compiler profile and policy identity. | — | — |
| <a id="api-efe30e4c8d142b04"></a>[`public Inno.Rendering.Assets.RenderShaderVariant Inno.Rendering.Assets.RenderShaderArtifact.variant { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderArtifact.cs#L93) | Gets the canonical static keyword selection. | — | — |
| <a id="api-1ce0d381c6ef8498"></a>[`public sealed class Inno.Rendering.Assets.RenderShaderArtifact`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderArtifact.cs#L13) | Contains one immutable, source-free shader artifact ready for runtime GPU resource creation. | — | — |

### `Inno.Rendering.Assets.RenderShaderArtifactCodec`

<!-- api: T:Inno.Rendering.Assets.RenderShaderArtifactCodec 791d1502c3f3835827198857747d8db6ff769411742ed587c98fa985060d13dd -->
<!-- api: M:Inno.Rendering.Assets.RenderShaderArtifactCodec.Encode(Inno.Rendering.Assets.RenderShaderArtifact) 56e8891d695f68974dc9d8fd3324b43e0ee4bb4acf860a3c3e4e79af372bef26 -->
<!-- api: M:Inno.Rendering.Assets.RenderShaderArtifactCodec.Decode(System.ReadOnlySpan{System.Byte},System.String,Inno.Rendering.Assets.RenderShaderVariant) 6b8a394f4500a5c3ff52dab8373fe86337c22df7c75c1c9ad97f34f556e746ae -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-f9363e2ebfc242d3"></a>[`public static Inno.Rendering.Assets.RenderShaderArtifact Inno.Rendering.Assets.RenderShaderArtifactCodec.Decode(System.ReadOnlySpan<byte> bytes, string expectedShaderName, Inno.Rendering.Assets.RenderShaderVariant expectedVariant)`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderArtifactCodec.cs#L80) | Decodes a deployed shader artifact and validates it against the requesting runtime shader and variant.<br>返回：A fully validated, source-free artifact ready for GPU resource creation.<br>`bytes`：The complete binary deployment payload.<br>`expectedShaderName`：The shader name declared by the requesting runtime asset.<br>`expectedVariant`：The exact static keyword selection requested by the material. | — | `System.IO.InvalidDataException`：Thrown when the payload is corrupt, exceeds safety limits, or does not match the request. |
| <a id="api-2ca2420c93ed6c6f"></a>[`public static byte[] Inno.Rendering.Assets.RenderShaderArtifactCodec.Encode(Inno.Rendering.Assets.RenderShaderArtifact artifact)`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderArtifactCodec.cs#L31) | Encodes a validated shader artifact into its deterministic runtime deployment representation.<br>返回：The deterministic binary deployment payload.<br>`artifact`：The complete source-free artifact to encode. | — | — |
| <a id="api-6e12fa6ca010e303"></a>[`public static class Inno.Rendering.Assets.RenderShaderArtifactCodec`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderArtifactCodec.cs#L14) | Encodes and validates the strict source-free binary format shared by game builds and Players. | — | — |

### `Inno.Rendering.Assets.RenderShaderPassArtifact`

<!-- api: T:Inno.Rendering.Assets.RenderShaderPassArtifact a70962b720a3ab575250fbe9fcb6ab50bae9af89f008f966e66f3f466cbce90f -->
<!-- api: M:Inno.Rendering.Assets.RenderShaderPassArtifact.#ctor(System.String,Inno.Rendering.ShaderProgramKind,Inno.Rendering.RenderRasterState,Inno.Rendering.ShaderInterface,System.Collections.Generic.IReadOnlyList{Inno.Rendering.Assets.RenderShaderStageArtifact}) 434bddb6cac94aacd6ce952d6df002e0056500ebf917ed5ed025a247bc908351 -->
<!-- api: P:Inno.Rendering.Assets.RenderShaderPassArtifact.name 1c8c86d901d192035ccce1eedef176b165eeac2662e42ff2160a53942a1fbec9 -->
<!-- api: P:Inno.Rendering.Assets.RenderShaderPassArtifact.programKind 74d87c07b86861df2e171409d92c644e06881d45fedafcf97289ed040d2ad28c -->
<!-- api: P:Inno.Rendering.Assets.RenderShaderPassArtifact.rasterState 6e7ac851291fe0569a368849150581ab10913dbb192c994cf2738d15176334ce -->
<!-- api: P:Inno.Rendering.Assets.RenderShaderPassArtifact.shaderInterface aa1e43bfa6343e4b6cb953c45f34cc59df82f7af1cdeb91fbad551dce5c75770 -->
<!-- api: P:Inno.Rendering.Assets.RenderShaderPassArtifact.stages da874809b39311927eb56976d9fd1b2a12e76b39f7a96832b6a33cba66c689c1 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-1270b8518421a69e"></a>[`public Inno.Rendering.Assets.RenderShaderPassArtifact.RenderShaderPassArtifact(string name, Inno.Rendering.ShaderProgramKind programKind, Inno.Rendering.RenderRasterState rasterState, Inno.Rendering.ShaderInterface shaderInterface, System.Collections.Generic.IReadOnlyList<Inno.Rendering.Assets.RenderShaderStageArtifact> stages)`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderPassArtifact.cs#L38) | Creates a deployed shader pass.<br>`name`：The stable pass name within the owning shader.<br>`programKind`：The programmable stage combination used by the pass.<br>`rasterState`：The backend-neutral fixed-function state used by raster passes.<br>`shaderInterface`：The validated pass-local resource binding contract.<br>`stages`：The complete target stage binaries required by the pass. | — | `System.ArgumentException`：Thrown when the name, stage set, or program kind is invalid. |
| <a id="api-dfbbd87dbba5cca7"></a>[`public string Inno.Rendering.Assets.RenderShaderPassArtifact.name { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderPassArtifact.cs#L63) | Gets the stable pass name within the owning shader. | — | — |
| <a id="api-3272abba3d7838dd"></a>[`public Inno.Rendering.ShaderProgramKind Inno.Rendering.Assets.RenderShaderPassArtifact.programKind { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderPassArtifact.cs#L68) | Gets the programmable stage combination used by this pass. | — | — |
| <a id="api-eab87e67a24e2723"></a>[`public Inno.Rendering.RenderRasterState Inno.Rendering.Assets.RenderShaderPassArtifact.rasterState { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderPassArtifact.cs#L73) | Gets an immutable copy of the backend-neutral raster state. | — | — |
| <a id="api-52bdc69784ad9272"></a>[`public Inno.Rendering.ShaderInterface Inno.Rendering.Assets.RenderShaderPassArtifact.shaderInterface { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderPassArtifact.cs#L78) | Gets the validated pass-local resource binding contract. | — | — |
| <a id="api-2a243f9dc253b9d5"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Assets.RenderShaderStageArtifact> Inno.Rendering.Assets.RenderShaderPassArtifact.stages { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderPassArtifact.cs#L83) | Gets the complete target stage binaries required by this pass. | — | — |
| <a id="api-116325ee19e21189"></a>[`public sealed class Inno.Rendering.Assets.RenderShaderPassArtifact`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderPassArtifact.cs#L13) | Carries one immutable compiled shader pass and its reflected runtime bindings. | — | — |

### `Inno.Rendering.Assets.RenderShaderStageArtifact`

<!-- api: T:Inno.Rendering.Assets.RenderShaderStageArtifact f30fe7df2c30a12e24bbe55b9a7e228b12a0efd44f482213d8b2f5b765890ccb -->
<!-- api: M:Inno.Rendering.Assets.RenderShaderStageArtifact.#ctor(Inno.Rendering.ShaderStage,System.ReadOnlySpan{System.Byte}) e6ba7af233ef1c3b030eeeab43400e0ee9a86812cbd120d269f9bacc40834b73 -->
<!-- api: P:Inno.Rendering.Assets.RenderShaderStageArtifact.stage 9501a8f4e29c26a2b53255d32c8acfe9bd7251688cd0d22a3014b6ce426eaae5 -->
<!-- api: P:Inno.Rendering.Assets.RenderShaderStageArtifact.bytes 69fdf02f5cafb565623dc41dd1f3c8a63409f9ed68772e9c55ec884a1e6196cc -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-44b69951da326850"></a>[`public Inno.Rendering.Assets.RenderShaderStageArtifact.RenderShaderStageArtifact(Inno.Rendering.ShaderStage stage, System.ReadOnlySpan<byte> bytes)`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderStageArtifact.cs#L29) | Creates a deployed shader stage.<br>`stage`：The single programmable stage represented by the target binary.<br>`bytes`：The non-empty target backend program bytes. | — | `System.ArgumentException`：Thrown when stage is not a single stage or bytes is empty. |
| <a id="api-516365655477996f"></a>[`public System.ReadOnlyMemory<byte> Inno.Rendering.Assets.RenderShaderStageArtifact.bytes { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderStageArtifact.cs#L49) | Gets the immutable target backend program bytes. | — | — |
| <a id="api-2cb56cd9b9591720"></a>[`public Inno.Rendering.ShaderStage Inno.Rendering.Assets.RenderShaderStageArtifact.stage { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderStageArtifact.cs#L44) | Gets the programmable stage represented by this artifact. | — | — |
| <a id="api-3932ddc9c4b2309b"></a>[`public sealed class Inno.Rendering.Assets.RenderShaderStageArtifact`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderStageArtifact.cs#L13) | Stores one immutable target shader stage without retaining authoring source or compiler state. | — | — |

### `Inno.Rendering.Assets.RenderShaderVariant`

<!-- api: T:Inno.Rendering.Assets.RenderShaderVariant bea579a5a54d07a07548829825ccfdee5de2aef5faa87d5f12f07981af1022cf -->
<!-- api: M:Inno.Rendering.Assets.RenderShaderVariant.#ctor(System.Collections.Generic.IReadOnlyDictionary{System.String,System.String}) 9e7dccfaff7246468041d1be690918c8b4bf7ddfce8a8110e819f37c747acb80 -->
<!-- api: P:Inno.Rendering.Assets.RenderShaderVariant.empty cc9ee77f472d319fdf4db058a374a266b4b60ea8c5106ccd6c05f5986a04e8ef -->
<!-- api: P:Inno.Rendering.Assets.RenderShaderVariant.options 8bd4406cf3e731ba8913490b503844e3404bfd1349222f087b41aeb1be9e3d00 -->
<!-- api: P:Inno.Rendering.Assets.RenderShaderVariant.value fa44d5c28b63cee243919818f296c009bfb08f94ad735e7dc0b885232afb7ae2 -->
<!-- api: M:Inno.Rendering.Assets.RenderShaderVariant.Parse(System.String) 76d36afe811972a25c7609b5dffffdd0d9a6e97f354cd39404bda650e1324f3a -->
<!-- api: M:Inno.Rendering.Assets.RenderShaderVariant.FromMaterial(Inno.Rendering.Assets.MaterialAsset) 62daacd3bc7866ac661e27b85356517b28e263153416ba87c8e11541a8cbc762 -->
<!-- api: M:Inno.Rendering.Assets.RenderShaderVariant.FromMaterial(Inno.Rendering.Assets.MaterialAsset,Inno.Rendering.Assets.ShaderDefinition) 142e9c17cb46f49920accede399fc10769de96f8ef4b8ce739158cdf55bc573e -->
<!-- api: M:Inno.Rendering.Assets.RenderShaderVariant.Equals(Inno.Rendering.Assets.RenderShaderVariant) aef670f1c2478e493b3865ea36eb51bb2cefadb06359c44939b86c1c96763793 -->
<!-- api: M:Inno.Rendering.Assets.RenderShaderVariant.Equals(System.Object) 98563051cba19ddbbb602df00e41e67e58c5f74aa3ab1ddaeee081ef38553842 -->
<!-- api: M:Inno.Rendering.Assets.RenderShaderVariant.GetHashCode 2be1961884440c01a43e4373b8893af62cb9f6f1b28380cc696d7f82f6c15cc3 -->
<!-- api: M:Inno.Rendering.Assets.RenderShaderVariant.op_Equality(Inno.Rendering.Assets.RenderShaderVariant,Inno.Rendering.Assets.RenderShaderVariant) fc71866b20c0541c613ec8de7e0c2f949dfb882ce5d62be4a184d35ac88b42f8 -->
<!-- api: M:Inno.Rendering.Assets.RenderShaderVariant.op_Inequality(Inno.Rendering.Assets.RenderShaderVariant,Inno.Rendering.Assets.RenderShaderVariant) f69da22eb24b45c5df454e8166281917aa8a509d855fb209f19db4cf16ee8b38 -->
<!-- api: M:Inno.Rendering.Assets.RenderShaderVariant.ToString 113d40c021a85e009822ca5178aecc95874427b8207ea780363e805547abe211 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-4bf11adf7b2951c3"></a>[`public Inno.Rendering.Assets.RenderShaderVariant.RenderShaderVariant(System.Collections.Generic.IReadOnlyDictionary<string, string> options)`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderVariant.cs#L30) | Creates a canonical shader variant from stable keyword selections.<br>`options`：Stable keyword ID to selected option mappings. | — | `System.ArgumentException`：Thrown when an identifier or option is empty or contains a reserved canonical separator. |
| <a id="api-d562932ee1d6a49f"></a>[`public bool Inno.Rendering.Assets.RenderShaderVariant.Equals(Inno.Rendering.Assets.RenderShaderVariant other)`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderVariant.cs#L174) | Determines whether this instance and the supplied value represent the same logical state.<br>返回：true when both values select identical options; otherwise, false.<br>`other`：The value to compare with this instance. | — | — |
| <a id="api-133381f13c7f4775"></a>[`public override bool Inno.Rendering.Assets.RenderShaderVariant.Equals(object? obj)`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderVariant.cs#L185) | Determines whether this instance and the supplied object represent the same logical state.<br>返回：true when the object is an identical variant; otherwise, false.<br>`obj`：The object to compare with this instance. | — | — |
| <a id="api-bf8b5582f40bcdbb"></a>[`public static Inno.Rendering.Assets.RenderShaderVariant Inno.Rendering.Assets.RenderShaderVariant.FromMaterial(Inno.Rendering.Assets.MaterialAsset material, Inno.Rendering.Assets.ShaderDefinition definition)`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderVariant.cs#L133) | Resolves material keyword selections against an exact published shader contract.<br>返回：The canonical keyword selection for this publication.<br>`material`：Material whose selected options are evaluated.<br>`definition`：Captured shader contract, which may be the last successful publication. | — | `System.InvalidOperationException`：A selected option is unknown or conflicts with another option. |
| <a id="api-4f7992f19b821e02"></a>[`public static Inno.Rendering.Assets.RenderShaderVariant Inno.Rendering.Assets.RenderShaderVariant.FromMaterial(Inno.Rendering.Assets.MaterialAsset material)`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderVariant.cs#L108) | Resolves the deterministic variant selected by a material.<br>返回：The canonical variant shared by authoring compilation and runtime lookup.<br>`material`：The material whose declared keyword options are evaluated. | — | `System.InvalidOperationException`：Thrown when the material has no shader definition or selects an unknown or conflicting option. |
| <a id="api-3b60f097d3e2cb82"></a>[`public override int Inno.Rendering.Assets.RenderShaderVariant.GetHashCode()`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderVariant.cs#L193) | Computes a hash code from the canonical variant representation.<br>返回：A hash code consistent with logical equality. | — | — |
| <a id="api-815001cea9b3f87e"></a>[`public static bool Inno.Rendering.Assets.RenderShaderVariant.operator ==(Inno.Rendering.Assets.RenderShaderVariant left, Inno.Rendering.Assets.RenderShaderVariant right)`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderVariant.cs#L207) | Determines whether two variants select identical stable options.<br>返回：true when both variants contain identical stable options; otherwise, false.<br>`left`：The first canonical shader variant to compare.<br>`right`：The second canonical shader variant to compare. | — | — |
| <a id="api-88088aadb43c4e1b"></a>[`public static bool Inno.Rendering.Assets.RenderShaderVariant.operator !=(Inno.Rendering.Assets.RenderShaderVariant left, Inno.Rendering.Assets.RenderShaderVariant right)`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderVariant.cs#L224) | Determines whether two variants select different stable options.<br>返回：true when the stable options differ; otherwise, false.<br>`left`：The first canonical shader variant to compare.<br>`right`：The second canonical shader variant to compare. | — | — |
| <a id="api-87dadeb2e685051a"></a>[`public static Inno.Rendering.Assets.RenderShaderVariant Inno.Rendering.Assets.RenderShaderVariant.Parse(string value)`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderVariant.cs#L72) | Parses the canonical representation stored in a deployed artifact.<br>返回：The validated shader variant represented by value.<br>`value`：The canonical semicolon-separated keyword selections. | — | `System.FormatException`：Thrown when the representation is malformed, unordered, or contains duplicate keyword IDs. |
| <a id="api-64f6d2014ad4a112"></a>[`public override string Inno.Rendering.Assets.RenderShaderVariant.ToString()`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderVariant.cs#L235) | Formats this value as its canonical representation.<br>返回：The canonical representation used by caches and deployed artifact paths. | — | — |
| <a id="api-3f0b915a5f3c8beb"></a>[`public static Inno.Rendering.Assets.RenderShaderVariant Inno.Rendering.Assets.RenderShaderVariant.empty { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderVariant.cs#L48) | Gets the empty default variant. | — | — |
| <a id="api-58e8c6f9fb1f33fe"></a>[`public System.Collections.Generic.IReadOnlyDictionary<string, string> Inno.Rendering.Assets.RenderShaderVariant.options { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderVariant.cs#L53) | Gets the immutable stable keyword selections. | — | — |
| <a id="api-0fb8f94435c96e46"></a>[`public string Inno.Rendering.Assets.RenderShaderVariant.value { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderVariant.cs#L58) | Gets the canonical cache-key representation. | — | — |
| <a id="api-2b2988566e359442"></a>[`public readonly struct Inno.Rendering.Assets.RenderShaderVariant : System.IEquatable<Inno.Rendering.Assets.RenderShaderVariant>`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderShaderVariant.cs#L13) | Identifies one deterministic selection of static shader keyword options shared by authoring and runtime. | — | — |

### `Inno.Rendering.Assets.RenderTargetArtifactPath`

<!-- api: T:Inno.Rendering.Assets.RenderTargetArtifactPath ef1290d320fbd883465f7a7d8c5b7f29f81200062c7af898fb202415c6b20099 -->
<!-- api: M:Inno.Rendering.Assets.RenderTargetArtifactPath.GetShaderPath(System.Guid,Inno.Rendering.GraphicsApi,Inno.Rendering.Assets.RenderShaderVariant) b0e11324641b1e2554d8bdf1aa6e247adbe576000e5bfcca9abee6b920aff24e -->
<!-- api: M:Inno.Rendering.Assets.RenderTargetArtifactPath.GetTexturePath(Inno.Rendering.Assets.RenderTextureArtifactReference) 26ce90678fb298dddd5ef461a720ed142e8a474e3a478f1f3a5a4c33cd9a87ef -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-c63e30bcfce82298"></a>[`public static string Inno.Rendering.Assets.RenderTargetArtifactPath.GetShaderPath(System.Guid shaderId, Inno.Rendering.GraphicsApi backend, Inno.Rendering.Assets.RenderShaderVariant variant)`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderTargetArtifactPath.cs#L33) | Gets the relative deployment path for one target shader variant.<br>返回：A platform-neutral path beneath the runtime content root.<br>`shaderId`：The persistent shader asset identity.<br>`backend`：The graphics backend selected by the target Player.<br>`variant`：The canonical static keyword selection. | — | `System.ArgumentException`：Thrown when shaderId is empty. |
| <a id="api-5a15ca1769310970"></a>[`public static string Inno.Rendering.Assets.RenderTargetArtifactPath.GetTexturePath(Inno.Rendering.Assets.RenderTextureArtifactReference texture)`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderTargetArtifactPath.cs#L60) | Gets the relative deployment path for one portable texture artifact.<br>返回：A platform-neutral path beneath the runtime content root.<br>`texture`：Stable texture artifact reference. | — | `System.ArgumentException`：Thrown when texture is invalid. |
| <a id="api-8a799db2aa18426c"></a>[`public static class Inno.Rendering.Assets.RenderTargetArtifactPath`](../../src/services/rendering/Inno.Rendering.Assets/Deployment/RenderTargetArtifactPath.cs#L13) | Defines deterministic relative paths shared by target-artifact producers and runtime consumers. | — | — |

### `Inno.Rendering.Assets.RenderTextureArtifactReference`

<!-- api: T:Inno.Rendering.Assets.RenderTextureArtifactReference 259c5ae92532ee557ddfe91c850ed9d42cac19fd6072162ec4e0fa8570c66987 -->
<!-- api: M:Inno.Rendering.Assets.RenderTextureArtifactReference.#ctor(System.Guid,System.Int64,Inno.Rendering.Assets.RenderTextureArtifactSlot) 047d4b1e8abae4819d30e412086bb79f89cd44715c25166c12981c52f201456c -->
<!-- api: P:Inno.Rendering.Assets.RenderTextureArtifactReference.assetId a7f27fcc69bd0ffd7b6e48ed395afd27ac90ee7094eb937b9c5e8bc9aafb2b8e -->
<!-- api: P:Inno.Rendering.Assets.RenderTextureArtifactReference.contentRevision 0323ec7d8486dd61df1a02816caf08107c5f1902d8f9df0c99d98ac0e4055050 -->
<!-- api: P:Inno.Rendering.Assets.RenderTextureArtifactReference.slot 23f77a5b4a46be098188bc0facdfb6b86c05d32441d71bc905e94307aeb15ed6 -->
<!-- api: M:Inno.Rendering.Assets.RenderTextureArtifactReference.ToString cb82b2fe875b4a0f268f3b83e02fd19d2f04c12c48ba7f86439a4e670cdb6b38 -->
<!-- api: M:Inno.Rendering.Assets.RenderTextureArtifactReference.op_Inequality(Inno.Rendering.Assets.RenderTextureArtifactReference,Inno.Rendering.Assets.RenderTextureArtifactReference) ea2e688317db897070199a3c9c9858e5549a4f56255588b246d15cead80cc2af -->
<!-- api: M:Inno.Rendering.Assets.RenderTextureArtifactReference.op_Equality(Inno.Rendering.Assets.RenderTextureArtifactReference,Inno.Rendering.Assets.RenderTextureArtifactReference) 410aeba4aee5b52da3e170306bed94f2efa949d8efd44316a112ed7d51e6407c -->
<!-- api: M:Inno.Rendering.Assets.RenderTextureArtifactReference.GetHashCode 09947f2f960547e7c8d43f79c5abab179f29fe7579c0c5fd49286ea39b555937 -->
<!-- api: M:Inno.Rendering.Assets.RenderTextureArtifactReference.Equals(System.Object) fed92c9dc8fb9f34a47b84aa696046ffe8160026ea484dbf51fb8933a878969e -->
<!-- api: M:Inno.Rendering.Assets.RenderTextureArtifactReference.Equals(Inno.Rendering.Assets.RenderTextureArtifactReference) 4faa26fbcfbecd9bfa990e3d7d19fedf875054e8216d8d112fd8220dce96a64e -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-9b7a652f9a96ddd6"></a>[`public Inno.Rendering.Assets.RenderTextureArtifactReference.RenderTextureArtifactReference(System.Guid assetId, long contentRevision, Inno.Rendering.Assets.RenderTextureArtifactSlot slot)`](../../src/services/rendering/Inno.Rendering.Assets/Textures/RenderTextureArtifactReference.cs#L28) | Creates a reference to one texture slot owned by an imported asset generation.<br>`assetId`：Persistent identity of the asset that owns the named source artifact.<br>`contentRevision`：Current committed content revision of the owning asset.<br>`slot`：Stable texture slot declaration. | — | `System.ArgumentException`：Thrown when assetId is empty or contentRevision is negative. |
| <a id="api-164c23a688eb38e3"></a>[`public bool Inno.Rendering.Assets.RenderTextureArtifactReference.Equals(Inno.Rendering.Assets.RenderTextureArtifactReference other)`](../../src/services/rendering/Inno.Rendering.Assets/Textures/RenderTextureArtifactReference.cs#L11) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-45ea0645bdbcbef1"></a>[`public override bool Inno.Rendering.Assets.RenderTextureArtifactReference.Equals(object obj)`](../../src/services/rendering/Inno.Rendering.Assets/Textures/RenderTextureArtifactReference.cs#L11) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-a1eeb8bbfaf987ec"></a>[`public override int Inno.Rendering.Assets.RenderTextureArtifactReference.GetHashCode()`](../../src/services/rendering/Inno.Rendering.Assets/Textures/RenderTextureArtifactReference.cs#L11) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-2a07230d84113021"></a>[`public static bool Inno.Rendering.Assets.RenderTextureArtifactReference.operator ==(Inno.Rendering.Assets.RenderTextureArtifactReference left, Inno.Rendering.Assets.RenderTextureArtifactReference right)`](../../src/services/rendering/Inno.Rendering.Assets/Textures/RenderTextureArtifactReference.cs#L11) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-8b7140d04ecadf5c"></a>[`public static bool Inno.Rendering.Assets.RenderTextureArtifactReference.operator !=(Inno.Rendering.Assets.RenderTextureArtifactReference left, Inno.Rendering.Assets.RenderTextureArtifactReference right)`](../../src/services/rendering/Inno.Rendering.Assets/Textures/RenderTextureArtifactReference.cs#L11) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-b972723d6132a6c0"></a>[`public override string Inno.Rendering.Assets.RenderTextureArtifactReference.ToString()`](../../src/services/rendering/Inno.Rendering.Assets/Textures/RenderTextureArtifactReference.cs#L11) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-cfaa1456df83c5a7"></a>[`public System.Guid Inno.Rendering.Assets.RenderTextureArtifactReference.assetId { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Textures/RenderTextureArtifactReference.cs#L46) | Gets the persistent identity of the owning asset. | — | — |
| <a id="api-0c2bb32f0a954091"></a>[`public long Inno.Rendering.Assets.RenderTextureArtifactReference.contentRevision { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Textures/RenderTextureArtifactReference.cs#L51) | Gets the committed owner content revision used for last-good replacement. | — | — |
| <a id="api-7db00c0e4fe3d52b"></a>[`public Inno.Rendering.Assets.RenderTextureArtifactSlot Inno.Rendering.Assets.RenderTextureArtifactReference.slot { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Textures/RenderTextureArtifactReference.cs#L56) | Gets the referenced stable texture slot. | — | — |
| <a id="api-4506acf5f09fe1bb"></a>[`public readonly record struct Inno.Rendering.Assets.RenderTextureArtifactReference : System.IEquatable<Inno.Rendering.Assets.RenderTextureArtifactReference>`](../../src/services/rendering/Inno.Rendering.Assets/Textures/RenderTextureArtifactReference.cs#L11) | Identifies one immutable texture source across authoring, deployment, and runtime generations. | — | — |

### `Inno.Rendering.Assets.RenderTextureArtifactSlot`

<!-- api: T:Inno.Rendering.Assets.RenderTextureArtifactSlot e3e6fb7c1d890d84d06d99efded5e1e3e247c74ef00018ec1398c0a0f381327c -->
<!-- api: M:Inno.Rendering.Assets.RenderTextureArtifactSlot.#ctor(System.String,System.String,Inno.Rendering.Assets.TextureColorSpace) af709fe6998bd7fb36d0bbc4a1ae07db6f6168def6e21c4fc0b170483a21765b -->
<!-- api: P:Inno.Rendering.Assets.RenderTextureArtifactSlot.id 1cfd22ea22b18203d1a47522fc1462f9f28705c478d36157c2d5f61285245276 -->
<!-- api: P:Inno.Rendering.Assets.RenderTextureArtifactSlot.sourceOutputName fcce0761deaa9f091266edd2e5fa33f043975ea6b9c95764201e9e77f9f5a830 -->
<!-- api: P:Inno.Rendering.Assets.RenderTextureArtifactSlot.colorSpace 9e1402ea57f45da5a52442487d5dddef82c69b7f6b7256fe476f1e40252cbeee -->
<!-- api: M:Inno.Rendering.Assets.RenderTextureArtifactSlot.ToString 5d6618d7bc683868d67fbdfd7f6a43fb3bd3e381b549944641d60231b1727f04 -->
<!-- api: M:Inno.Rendering.Assets.RenderTextureArtifactSlot.op_Inequality(Inno.Rendering.Assets.RenderTextureArtifactSlot,Inno.Rendering.Assets.RenderTextureArtifactSlot) b4d353fdf2810609b82a06854dcb25a1e9c12ec043f63abe02b7ea5f0ffe0500 -->
<!-- api: M:Inno.Rendering.Assets.RenderTextureArtifactSlot.op_Equality(Inno.Rendering.Assets.RenderTextureArtifactSlot,Inno.Rendering.Assets.RenderTextureArtifactSlot) ffe266dac874cfca721203c7aa0595c29c100b422e058f5a795930a136919b7b -->
<!-- api: M:Inno.Rendering.Assets.RenderTextureArtifactSlot.GetHashCode b39b65c088d4819342496d3193a0010d25dfe755a671d7530fcc38d5b173b51b -->
<!-- api: M:Inno.Rendering.Assets.RenderTextureArtifactSlot.Equals(System.Object) d1076151c35e3e8b955546ffdaf161680d0f7bd7eb030e00441dcc0f50cd22c0 -->
<!-- api: M:Inno.Rendering.Assets.RenderTextureArtifactSlot.Equals(Inno.Rendering.Assets.RenderTextureArtifactSlot) a9cb9f0836cbaa32c0233e1f05093b94372e02eb0e93fa5c36a83a994e3d22de -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-4f4be4c3da81bf66"></a>[`public Inno.Rendering.Assets.RenderTextureArtifactSlot.RenderTextureArtifactSlot(string id, string sourceOutputName, Inno.Rendering.Assets.TextureColorSpace colorSpace)`](../../src/services/rendering/Inno.Rendering.Assets/Textures/RenderTextureArtifactSlot.cs#L28) | Creates one stable texture slot declaration.<br>`id`：Asset-local stable slot identifier.<br>`sourceOutputName`：Named source artifact written by the owning asset importer.<br>`colorSpace`：Sampling color-space interpretation used by the target compiler and GPU resource. | — | `System.ArgumentException`：Thrown when id or sourceOutputName is empty. |
| <a id="api-e27b17aa42eb7d1f"></a>[`public bool Inno.Rendering.Assets.RenderTextureArtifactSlot.Equals(Inno.Rendering.Assets.RenderTextureArtifactSlot other)`](../../src/services/rendering/Inno.Rendering.Assets/Textures/RenderTextureArtifactSlot.cs#L11) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-a026df8a41da7a20"></a>[`public override bool Inno.Rendering.Assets.RenderTextureArtifactSlot.Equals(object obj)`](../../src/services/rendering/Inno.Rendering.Assets/Textures/RenderTextureArtifactSlot.cs#L11) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-399c95af40e6756e"></a>[`public override int Inno.Rendering.Assets.RenderTextureArtifactSlot.GetHashCode()`](../../src/services/rendering/Inno.Rendering.Assets/Textures/RenderTextureArtifactSlot.cs#L11) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-8353e397ff1c2a85"></a>[`public static bool Inno.Rendering.Assets.RenderTextureArtifactSlot.operator ==(Inno.Rendering.Assets.RenderTextureArtifactSlot left, Inno.Rendering.Assets.RenderTextureArtifactSlot right)`](../../src/services/rendering/Inno.Rendering.Assets/Textures/RenderTextureArtifactSlot.cs#L11) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-103806e7aac5dded"></a>[`public static bool Inno.Rendering.Assets.RenderTextureArtifactSlot.operator !=(Inno.Rendering.Assets.RenderTextureArtifactSlot left, Inno.Rendering.Assets.RenderTextureArtifactSlot right)`](../../src/services/rendering/Inno.Rendering.Assets/Textures/RenderTextureArtifactSlot.cs#L11) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-3964f8b09bac397b"></a>[`public override string Inno.Rendering.Assets.RenderTextureArtifactSlot.ToString()`](../../src/services/rendering/Inno.Rendering.Assets/Textures/RenderTextureArtifactSlot.cs#L11) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-d62d27f534ef21b2"></a>[`public Inno.Rendering.Assets.TextureColorSpace Inno.Rendering.Assets.RenderTextureArtifactSlot.colorSpace { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Textures/RenderTextureArtifactSlot.cs#L53) | Gets the sampling color-space interpretation. | — | — |
| <a id="api-d021053f9687584d"></a>[`public string Inno.Rendering.Assets.RenderTextureArtifactSlot.id { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Textures/RenderTextureArtifactSlot.cs#L43) | Gets the asset-local stable slot identifier. | — | — |
| <a id="api-26cbb1e771e99a63"></a>[`public string Inno.Rendering.Assets.RenderTextureArtifactSlot.sourceOutputName { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Textures/RenderTextureArtifactSlot.cs#L48) | Gets the named portable image artifact consumed by target compilation. | — | — |
| <a id="api-7caae3a33fe7e8aa"></a>[`public readonly record struct Inno.Rendering.Assets.RenderTextureArtifactSlot : System.IEquatable<Inno.Rendering.Assets.RenderTextureArtifactSlot>`](../../src/services/rendering/Inno.Rendering.Assets/Textures/RenderTextureArtifactSlot.cs#L11) | Declares an immutable source artifact that is compiled into one sampled texture slot. | — | — |

### `Inno.Rendering.Assets.SerializedRenderExtensionState`

<!-- api: T:Inno.Rendering.Assets.SerializedRenderExtensionState 625a2f727beb2a663258a37cce22cd2abb9dc5921b4ee0baa8db5a86263d5186 -->
<!-- api: M:Inno.Rendering.Assets.SerializedRenderExtensionState.#ctor 3b4f7376c42ef2ef681cfef99bb4ca18676ef15c4dcf36508ae981f080fc2553 -->
<!-- api: M:Inno.Rendering.Assets.SerializedRenderExtensionState.#ctor(System.Guid,System.ReadOnlySpan{System.Byte}) 5e6603cbbf7b8f073f1d01919d4735cbeebefef0f4aa47fcce6dfb500beb2d64 -->
<!-- api: M:Inno.Rendering.Assets.SerializedRenderExtensionState.#ctor(Inno.Assets.AssetPropertySnapshot) 74e4983c2024fc8bb8828e7e9b051d8cdfa8d15fb033e056841853aeee3492e5 -->
<!-- api: P:Inno.Rendering.Assets.SerializedRenderExtensionState.stableTypeId cefe15c31e1a696d2048dbd2b30a57abf14ef3b2d47517397ef506deb3713be3 -->
<!-- api: P:Inno.Rendering.Assets.SerializedRenderExtensionState.propertyData d0bcf5bcc9baaddf471beeaff11e973b51523b1ce781b6fb9c128fbc30639fa1 -->
<!-- api: P:Inno.Rendering.Assets.SerializedRenderExtensionState.dependencies 29b314436ffb96d72acc4e52aab3482e2fba37b3f7f15cb7b814cf4c7bf306d8 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-2e9d6551cd9ed4e6"></a>[`public Inno.Rendering.Assets.SerializedRenderExtensionState.SerializedRenderExtensionState()`](../../src/services/rendering/Inno.Rendering.Assets/Pipelines/SerializedRenderExtensionState.cs#L21) | Creates empty extension state for deserialization. | — | — |
| <a id="api-5f831736668ea49d"></a>[`public Inno.Rendering.Assets.SerializedRenderExtensionState.SerializedRenderExtensionState(Inno.Assets.AssetPropertySnapshot properties)`](../../src/services/rendering/Inno.Rendering.Assets/Pipelines/SerializedRenderExtensionState.cs#L51) | Copies an owner-captured property snapshot, preserving its automatic resource dependency declarations.<br>`properties`：Complete neutral properties produced by the asset authoring services. | — | — |
| <a id="api-63f5d891da1ea4ac"></a>[`public Inno.Rendering.Assets.SerializedRenderExtensionState.SerializedRenderExtensionState(System.Guid stableTypeId, System.ReadOnlySpan<byte> propertyData)`](../../src/services/rendering/Inno.Rendering.Assets/Pipelines/SerializedRenderExtensionState.cs#L36) | Creates immutable extension state from stable identity and property bytes.<br>`stableTypeId`：Stable type identity, or empty when the extension has no typed settings.<br>`propertyData`：Neutral native property bytes produced by asset serialization. Use the snapshot constructor when settings reference assets. | — | — |
| <a id="api-d3aa812f0df88eeb"></a>[`public Inno.Assets.AssetDependency[] Inno.Rendering.Assets.SerializedRenderExtensionState.dependencies { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Pipelines/SerializedRenderExtensionState.cs#L75) | Gets or sets asset dependencies captured with the neutral property payload. | — | — |
| <a id="api-7bee490a51f992f2"></a>[`public byte[] Inno.Rendering.Assets.SerializedRenderExtensionState.propertyData { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Pipelines/SerializedRenderExtensionState.cs#L69) | Gets or sets neutral serialized property bytes. | — | — |
| <a id="api-1dff7c35e9b058cc"></a>[`public System.Guid Inno.Rendering.Assets.SerializedRenderExtensionState.stableTypeId { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Pipelines/SerializedRenderExtensionState.cs#L63) | Gets or sets the stable settings type identity. | — | — |
| <a id="api-e751fdca5945ad0c"></a>[`public struct Inno.Rendering.Assets.SerializedRenderExtensionState`](../../src/services/rendering/Inno.Rendering.Assets/Pipelines/SerializedRenderExtensionState.cs#L16) | Stores reload-safe configuration for one pipeline or feature extension generation. | — | — |

### `Inno.Rendering.Assets.ShaderAsset`

<!-- api: T:Inno.Rendering.Assets.ShaderAsset 7909071a31523d4959ae0bfe3a2acc478829f94196c973e9c21a202f15739fb0 -->
<!-- api: P:Inno.Rendering.Assets.ShaderAsset.definition 694d98a42abded188d3da7445d0904136b3e538234752994e183be1449978ab9 -->
<!-- api: M:Inno.Rendering.Assets.ShaderAsset.SetDefinition(Inno.Rendering.Assets.ShaderDefinition,Inno.Core.Serialization.SerializationRegistry,Inno.Core.Serialization.SerializationContext) da584f42d2246ac4f86cb7b559f3ccd8a56be49c21ff6116c7a1e4cd87ca0669 -->
<!-- api: M:Inno.Rendering.Assets.ShaderAsset.#ctor d3593c14577336882eca81ab961b8963e3d0f6489d1fd222ef655614c1f5af56 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-3f1cc2e75146b206"></a>[`public Inno.Rendering.Assets.ShaderAsset.ShaderAsset()`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderAsset.cs#L17) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-3418127470838549"></a>[`public void Inno.Rendering.Assets.ShaderAsset.SetDefinition(Inno.Rendering.Assets.ShaderDefinition value, Inno.Core.Serialization.SerializationRegistry serialization, Inno.Core.Serialization.SerializationContext context)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderAsset.cs#L49) | Commits a validated definition through the native serialization channel.<br>`value`：Complete definition visible to materials and target compilation.<br>`serialization`：The serialization registry that owns the active shader converter generation.<br>`context`：The complete owner reference context used for material texture defaults. | — | `System.ArgumentException`：A declaration collection is null. The previous committed definition remains unchanged.<br>`System.InvalidOperationException`：Serialization cannot encode the candidate. No part of the candidate is published. |
| <a id="api-e3f4bc1d9451fed5"></a>[`public Inno.Rendering.Assets.ShaderDefinition? Inno.Rendering.Assets.ShaderAsset.definition { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderAsset.cs#L28) | Gets a detached editable copy of the committed backend-neutral definition, or null before import. Nested declaration arrays are copied; referenced assets remain identity-owned resources. | — | — |
| <a id="api-947ec34dc7046ca2"></a>[`public class Inno.Rendering.Assets.ShaderAsset : Inno.Assets.AssetObject`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderAsset.cs#L17) | Represents the runtime contract imported from a shader graph. | — | — |

### `Inno.Rendering.Assets.ShaderCompareFunction`

<!-- api: T:Inno.Rendering.Assets.ShaderCompareFunction 32db707b5867ebee4851c8d0be62eef4826ea52a6611f999fe62d036754cd281 -->
<!-- api: F:Inno.Rendering.Assets.ShaderCompareFunction.Never 5383161001763d0f672903fbeff4b96dfa51a539f29b0f2fd97c354a6d15c43e -->
<!-- api: F:Inno.Rendering.Assets.ShaderCompareFunction.Less f5bf8843a3622515e85dcd13cdb1777842549f7b46b0088b5ae0eead8a38ddd8 -->
<!-- api: F:Inno.Rendering.Assets.ShaderCompareFunction.Equal 078adb09959352e2f17bbc8e84efbf4e030d654447c3c74bd9484223a4f943fd -->
<!-- api: F:Inno.Rendering.Assets.ShaderCompareFunction.LessEqual 0565d1f44eabbadf7d5e057c0086e4c375d6de385362eefe79a9d4d461832020 -->
<!-- api: F:Inno.Rendering.Assets.ShaderCompareFunction.Greater 9162ba58941b0f5959a6ba973288062dd9d55cea4f776429dfdf39a62bb452ce -->
<!-- api: F:Inno.Rendering.Assets.ShaderCompareFunction.NotEqual 1689f37d3dda3f014d7b885e2d9ed5738d10dcf367a91c9b9d9fe6dcbcfee9b8 -->
<!-- api: F:Inno.Rendering.Assets.ShaderCompareFunction.GreaterEqual 793e8e79c9edf73b232e6d414298fd1aeb8a355f8466c47e4eb5f03619e503de -->
<!-- api: F:Inno.Rendering.Assets.ShaderCompareFunction.Always d022a341f1afcb8a08bde0bfae95b59cf5b096035412ee68eddf8238c863e192 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-ac7e35efba0a2ccf"></a>[`Inno.Rendering.Assets.ShaderCompareFunction.Always = 7`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L70) | Always accepts the fragment. | — | — |
| <a id="api-cb710a0ad3208908"></a>[`Inno.Rendering.Assets.ShaderCompareFunction.Equal = 2`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L50) | Accepts a fragment with an equal depth. | — | — |
| <a id="api-feb654a5a7ac763b"></a>[`Inno.Rendering.Assets.ShaderCompareFunction.Greater = 4`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L58) | Accepts a fragment with a greater depth. | — | — |
| <a id="api-84d3ee8980982ed3"></a>[`Inno.Rendering.Assets.ShaderCompareFunction.GreaterEqual = 6`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L66) | Accepts a fragment with a greater or equal depth. | — | — |
| <a id="api-407bd79fa8744a86"></a>[`Inno.Rendering.Assets.ShaderCompareFunction.Less = 1`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L46) | Accepts a fragment with a smaller depth. | — | — |
| <a id="api-29b1859ffe2c3d50"></a>[`Inno.Rendering.Assets.ShaderCompareFunction.LessEqual = 3`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L54) | Accepts a fragment with a smaller or equal depth. | — | — |
| <a id="api-300cf104e454250e"></a>[`Inno.Rendering.Assets.ShaderCompareFunction.Never = 0`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L42) | Always rejects the fragment. | — | — |
| <a id="api-37d2e08f52619194"></a>[`Inno.Rendering.Assets.ShaderCompareFunction.NotEqual = 5`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L62) | Accepts a fragment with a different depth. | — | — |
| <a id="api-3f2e4a4fbb45bc46"></a>[`public enum Inno.Rendering.Assets.ShaderCompareFunction : int`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L37) | Selects the comparison used by depth testing. | — | — |

### `Inno.Rendering.Assets.ShaderContractId`

<!-- api: T:Inno.Rendering.Assets.ShaderContractId 64e256b4e5381960e7bfaf434f300644e38c4e505845391970bfa78ca7816269 -->
<!-- api: M:Inno.Rendering.Assets.ShaderContractId.#ctor(System.String) bd7cb769bb48c1a513a9fa7a790d25722c40acebe6fc73c8a360ee5d66f75511 -->
<!-- api: P:Inno.Rendering.Assets.ShaderContractId.value e54c46ac230ae8bd66c25e54d7ff0365574affa07f88cec4bf96424fa3f016b0 -->
<!-- api: P:Inno.Rendering.Assets.ShaderContractId.isValid 1ccd79dfe48cd25094b1033907e1f9843a2c5a98c8a295af38e094a5c224e4e7 -->
<!-- api: M:Inno.Rendering.Assets.ShaderContractId.ToString 2095453995fb67c896af2560fd781832b73375ff943e70479b41547b2ace2b26 -->
<!-- api: M:Inno.Rendering.Assets.ShaderContractId.op_Inequality(Inno.Rendering.Assets.ShaderContractId,Inno.Rendering.Assets.ShaderContractId) ec8fbf3c4d47fffe75c847f83c820be75c951976095979ee9594dff1cdd60c69 -->
<!-- api: M:Inno.Rendering.Assets.ShaderContractId.op_Equality(Inno.Rendering.Assets.ShaderContractId,Inno.Rendering.Assets.ShaderContractId) 52d212c61e487b88cffbcf9ada37dc4988e5089ecd8a8a37a76dedfbd181bbcf -->
<!-- api: M:Inno.Rendering.Assets.ShaderContractId.GetHashCode 8bbcd969e46fd9dbe148e78f40fbee5ac2eeddd8812ad73a5340bbda483ebf58 -->
<!-- api: M:Inno.Rendering.Assets.ShaderContractId.Equals(System.Object) 35a1f7ddbc9e37a52f28a91c09b459110607ae6c5f3653fdf79394fbac2fdb23 -->
<!-- api: M:Inno.Rendering.Assets.ShaderContractId.Equals(Inno.Rendering.Assets.ShaderContractId) 1150e7450d41176738c758fc43d671b51b99deac6ba83dc78088f9201d0a602a -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-67178a5ac6b65dd6"></a>[`public Inno.Rendering.Assets.ShaderContractId.ShaderContractId(string value)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderContractId.cs#L25) | Creates a shader contract identifier.<br>`value`：Globally stable contract value. | — | — |
| <a id="api-2e85545e14663f92"></a>[`public readonly bool Inno.Rendering.Assets.ShaderContractId.Equals(Inno.Rendering.Assets.ShaderContractId other)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderContractId.cs#L17) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-e50aa0894752dd64"></a>[`public override readonly bool Inno.Rendering.Assets.ShaderContractId.Equals(object obj)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderContractId.cs#L17) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-e103f8d9252f9eed"></a>[`public override readonly int Inno.Rendering.Assets.ShaderContractId.GetHashCode()`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderContractId.cs#L17) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-b22913ff9b07fce9"></a>[`public static bool Inno.Rendering.Assets.ShaderContractId.operator ==(Inno.Rendering.Assets.ShaderContractId left, Inno.Rendering.Assets.ShaderContractId right)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderContractId.cs#L17) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-e5a2b338728c43fc"></a>[`public static bool Inno.Rendering.Assets.ShaderContractId.operator !=(Inno.Rendering.Assets.ShaderContractId left, Inno.Rendering.Assets.ShaderContractId right)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderContractId.cs#L17) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-8bc8df432a522ec2"></a>[`public override readonly string Inno.Rendering.Assets.ShaderContractId.ToString()`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderContractId.cs#L47) | Formats this value as a human-readable representation.<br>返回：The human-readable representation of this value. | — | — |
| <a id="api-2e3f59d54b408fe6"></a>[`public readonly bool Inno.Rendering.Assets.ShaderContractId.isValid { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderContractId.cs#L39) | Gets whether the identifier has a usable value. | — | — |
| <a id="api-4bdd564ed4c1b9fb"></a>[`public string Inno.Rendering.Assets.ShaderContractId.value { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderContractId.cs#L34) | Gets or sets the globally stable contract value. | — | — |
| <a id="api-5198559cdbe56865"></a>[`public record struct Inno.Rendering.Assets.ShaderContractId : System.IEquatable<Inno.Rendering.Assets.ShaderContractId>`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderContractId.cs#L17) | Identifies an open shader and pipeline compatibility contract. | — | — |

### `Inno.Rendering.Assets.ShaderCullMode`

<!-- api: T:Inno.Rendering.Assets.ShaderCullMode 3920f55ac70394f2823e114f54a0a9b44621ff8ba0db4266840f6d639ed3ac63 -->
<!-- api: F:Inno.Rendering.Assets.ShaderCullMode.None 4e9273ea2f1f79285a267badf3a0d190a2fd1708ea44748910c06fd2e6ab6dd0 -->
<!-- api: F:Inno.Rendering.Assets.ShaderCullMode.Front 3e5fe608ceb47f294708c0027c4e62b1c7c55692ec83c52ebca98af6f5c31e5e -->
<!-- api: F:Inno.Rendering.Assets.ShaderCullMode.Back 021b946ee4048ac12afd8cafa370ac33c65cfd0ac3a1252f4fde51df5f0d483f -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-a0f339dc89c01765"></a>[`Inno.Rendering.Assets.ShaderCullMode.Back = 2`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L30) | Rejects back-facing triangles. | — | — |
| <a id="api-843de420f6fcb2b2"></a>[`Inno.Rendering.Assets.ShaderCullMode.Front = 1`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L26) | Rejects front-facing triangles. | — | — |
| <a id="api-0dec49832618a235"></a>[`Inno.Rendering.Assets.ShaderCullMode.None = 0`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L22) | Does not reject either face orientation. | — | — |
| <a id="api-c29fc9a07d17df6c"></a>[`public enum Inno.Rendering.Assets.ShaderCullMode : int`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L17) | Selects the triangle face rejected by a raster pass. | — | — |

### `Inno.Rendering.Assets.ShaderDefinition`

<!-- api: T:Inno.Rendering.Assets.ShaderDefinition 1a083b47e91d68d6f1ad3c11131a418c3cffeaf7d0f49fc2e8217d6d5f01acc7 -->
<!-- api: M:Inno.Rendering.Assets.ShaderDefinition.#ctor 0c26b9fbf30a179f3d61dbd80074dab88d0593634c6048c7327e02274b71b517 -->
<!-- api: M:Inno.Rendering.Assets.ShaderDefinition.#ctor(System.String,System.Collections.Generic.IEnumerable{Inno.Rendering.Assets.ShaderPropertyDefinition},System.Collections.Generic.IEnumerable{Inno.Rendering.Assets.ShaderKeywordDefinition},System.Collections.Generic.IEnumerable{Inno.Rendering.Assets.ShaderPassDefinition},System.Collections.Generic.IEnumerable{Inno.Rendering.Assets.ShaderTechniqueDefinition}) 2fd5758656237289a30d87ce76406a4ac7834563fad3340ab5bb843140f2a123 -->
<!-- api: P:Inno.Rendering.Assets.ShaderDefinition.name b8504b51d9deace9a3114ab21de8819271d2f341f132c99c8ba90daf0cb5992e -->
<!-- api: P:Inno.Rendering.Assets.ShaderDefinition.properties 4de06863093801309680f81ce43cc39cc96f335aac805523be45e89d4cd2a32a -->
<!-- api: P:Inno.Rendering.Assets.ShaderDefinition.keywords 235a708898b84e53fa81fcd9b9f3559aa6cd450da98c1aa7718aad6b9114a409 -->
<!-- api: P:Inno.Rendering.Assets.ShaderDefinition.passes 1f4f0c590f7e2740107d02f1133854f634ed41b837eb2329f1c1567e7488acaf -->
<!-- api: P:Inno.Rendering.Assets.ShaderDefinition.techniques 73a6c398d0dc7d2282598a9acdb6643285b6c5da1971b510231caf2f997ea7ab -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-669812ac7157f9cb"></a>[`public Inno.Rendering.Assets.ShaderDefinition.ShaderDefinition()`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderDefinition.cs#L22) | Creates an empty shader definition for native deserialization. | — | — |
| <a id="api-e738e3e53cacb6fe"></a>[`public Inno.Rendering.Assets.ShaderDefinition.ShaderDefinition(string name, System.Collections.Generic.IEnumerable<Inno.Rendering.Assets.ShaderPropertyDefinition> properties, System.Collections.Generic.IEnumerable<Inno.Rendering.Assets.ShaderKeywordDefinition> keywords, System.Collections.Generic.IEnumerable<Inno.Rendering.Assets.ShaderPassDefinition> passes, System.Collections.Generic.IEnumerable<Inno.Rendering.Assets.ShaderTechniqueDefinition>? techniques = null)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderDefinition.cs#L44) | Creates a shader definition.<br>`name`：Artist-facing shader name.<br>`properties`：Stable property declarations.<br>`keywords`：Static keyword declarations.<br>`passes`：Backend-neutral pass declarations.<br>`techniques`：Open contract and role mappings. | — | — |
| <a id="api-d85fc01151cc73fb"></a>[`public Inno.Rendering.Assets.ShaderKeywordDefinition[] Inno.Rendering.Assets.ShaderDefinition.keywords { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderDefinition.cs#L78) | Gets or sets static keyword declarations. | — | — |
| <a id="api-434cee19f38809f5"></a>[`public string Inno.Rendering.Assets.ShaderDefinition.name { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderDefinition.cs#L66) | Gets or sets the artist-facing shader name. | — | — |
| <a id="api-4815d53d9eb2e98b"></a>[`public Inno.Rendering.Assets.ShaderPassDefinition[] Inno.Rendering.Assets.ShaderDefinition.passes { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderDefinition.cs#L84) | Gets or sets backend-neutral pass declarations. | — | — |
| <a id="api-78ac3af38eec34d3"></a>[`public Inno.Rendering.Assets.ShaderPropertyDefinition[] Inno.Rendering.Assets.ShaderDefinition.properties { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderDefinition.cs#L72) | Gets or sets stable property declarations. | — | — |
| <a id="api-fc003091f61277c0"></a>[`public Inno.Rendering.Assets.ShaderTechniqueDefinition[] Inno.Rendering.Assets.ShaderDefinition.techniques { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderDefinition.cs#L90) | Gets or sets open contract and role mappings. | — | — |
| <a id="api-59076dac6a040c56"></a>[`public sealed class Inno.Rendering.Assets.ShaderDefinition : Inno.Core.Serialization.ISerializable`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderDefinition.cs#L17) | Contains the source-of-truth definition shared by shader import and material tooling. | — | — |

### `Inno.Rendering.Assets.ShaderDefinitionValidator`

<!-- api: T:Inno.Rendering.Assets.ShaderDefinitionValidator 5ebb5a298a17970c72a663dab71bfebb213144fa1ef111b928ab5df65c8590b0 -->
<!-- api: M:Inno.Rendering.Assets.ShaderDefinitionValidator.Validate(Inno.Rendering.Assets.ShaderDefinition,Inno.Rendering.GraphicsCapabilities) 190f48a84907249c8817ab572a78d90aa30704c373293db31668066db0515a23 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b1fdce95e3bfcc8f"></a>[`public static System.Collections.Generic.IReadOnlyList<Inno.Rendering.Assets.ShaderDiagnostic> Inno.Rendering.Assets.ShaderDefinitionValidator.Validate(Inno.Rendering.Assets.ShaderDefinition definition, Inno.Rendering.GraphicsCapabilities? capabilities = null)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderDefinitionValidator.cs#L30) | Checks declaration identities, resource kinds, stage visibility and open technique mappings without mutating the candidate.<br>返回：Immutable diagnostics; an empty collection indicates a valid declaration contract.<br>`definition`：The candidate runtime contract.<br>`capabilities`：Optional device capabilities; unsupported optional passes produce warnings. | — | `System.ArgumentNullException`：The definition is null. |
| <a id="api-ab4eec24821b5773"></a>[`public static class Inno.Rendering.Assets.ShaderDefinitionValidator`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderDefinitionValidator.cs#L13) | Validates the source-free material, pass and technique contract shared by graph compilation and deployed shaders. | — | — |

### `Inno.Rendering.Assets.ShaderDiagnostic`

<!-- api: T:Inno.Rendering.Assets.ShaderDiagnostic 12cad327e06968c294707fcbb55e871b46bc0ce0a91352bfcbf6bdc86ea9ce8a -->
<!-- api: M:Inno.Rendering.Assets.ShaderDiagnostic.#ctor(System.String,Inno.Core.Diagnostics.DiagnosticSeverity,System.String,System.Nullable{Inno.Rendering.Assets.ShaderSourceLocation}) 997e2f841c2e322adea75f20c5d630ab953f53feabc9a30a55d7159d4d62ff9e -->
<!-- api: P:Inno.Rendering.Assets.ShaderDiagnostic.code 4ee92809942862b0ff9fceafd790248990e49e0c43429a3b35b9e7daa0a475f4 -->
<!-- api: P:Inno.Rendering.Assets.ShaderDiagnostic.severity 217dec9781c2bfca6dba3ee31d0afb3890b205fde525ae94bd9cf9d65930f7e1 -->
<!-- api: P:Inno.Rendering.Assets.ShaderDiagnostic.message aa45ba46b869764625acdcc6e355b5b12e2923c15d2cd27437a4f1d924b34731 -->
<!-- api: P:Inno.Rendering.Assets.ShaderDiagnostic.location 1a955d52beb5e46c495877a21cc5e62b115c3f0a4d6a6588162db127fa590fc7 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d9e156ff0469d5a7"></a>[`public Inno.Rendering.Assets.ShaderDiagnostic.ShaderDiagnostic(string code, Inno.Core.Diagnostics.DiagnosticSeverity severity, string message, Inno.Rendering.Assets.ShaderSourceLocation? location = null)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderDiagnostic.cs#L30) | Creates a shader diagnostic.<br>`code`：Stable diagnostic code.<br>`severity`：Diagnostic severity.<br>`message`：Artist-facing message.<br>`location`：Optional source mapping. | — | — |
| <a id="api-33bf98dd23ecb300"></a>[`public string Inno.Rendering.Assets.ShaderDiagnostic.code { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderDiagnostic.cs#L47) | Gets the stable diagnostic code. | — | — |
| <a id="api-c73e7fe652c6375c"></a>[`public Inno.Rendering.Assets.ShaderSourceLocation? Inno.Rendering.Assets.ShaderDiagnostic.location { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderDiagnostic.cs#L62) | Gets the optional source mapping. | — | — |
| <a id="api-9c92e2968b4f1f71"></a>[`public string Inno.Rendering.Assets.ShaderDiagnostic.message { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderDiagnostic.cs#L57) | Gets the artist-facing message. | — | — |
| <a id="api-213bfa524c41cd27"></a>[`public Inno.Core.Diagnostics.DiagnosticSeverity Inno.Rendering.Assets.ShaderDiagnostic.severity { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderDiagnostic.cs#L52) | Gets the diagnostic severity. | — | — |
| <a id="api-250be7838e265015"></a>[`public sealed class Inno.Rendering.Assets.ShaderDiagnostic`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderDiagnostic.cs#L13) | Reports one structured shader validation or compilation issue. | — | — |

### `Inno.Rendering.Assets.ShaderKeywordDefinition`

<!-- api: T:Inno.Rendering.Assets.ShaderKeywordDefinition 4c848317d04a3171c07b85a8876eb6650bc729a5e897197d4af1cb7a2dd9de6e -->
<!-- api: M:Inno.Rendering.Assets.ShaderKeywordDefinition.#ctor(System.String,System.Collections.Generic.IEnumerable{System.String}) 9e3229f447957864bffdff21fe213ea523a980d8cacacd70d0195473e7f64f9e -->
<!-- api: P:Inno.Rendering.Assets.ShaderKeywordDefinition.id a64a40dc5e988b605ad3d2b36549b4044cace020279ed2dd63db60a64e03dcfb -->
<!-- api: P:Inno.Rendering.Assets.ShaderKeywordDefinition.options 5aa9cec7afe28829ea10b2b790498414d9e2af468fa132bc6947b9bd13157b2e -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-1cabb073b2fc908a"></a>[`public Inno.Rendering.Assets.ShaderKeywordDefinition.ShaderKeywordDefinition(string id, System.Collections.Generic.IEnumerable<string> options)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderKeywordDefinition.cs#L28) | Creates a shader keyword definition.<br>`id`：Stable keyword identifier.<br>`options`：Allowed stable option identifiers. | — | — |
| <a id="api-9d528c5c83023ce1"></a>[`public string Inno.Rendering.Assets.ShaderKeywordDefinition.id { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderKeywordDefinition.cs#L41) | Gets or sets the stable keyword identifier. | — | — |
| <a id="api-a9acf836aa223c02"></a>[`public string[] Inno.Rendering.Assets.ShaderKeywordDefinition.options { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderKeywordDefinition.cs#L46) | Gets or sets allowed stable option identifiers. | — | — |
| <a id="api-c36d02f46cae9810"></a>[`public struct Inno.Rendering.Assets.ShaderKeywordDefinition`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderKeywordDefinition.cs#L17) | Declares one static shader keyword that may produce compiled variants. | — | — |

### `Inno.Rendering.Assets.ShaderMetadataEntry`

<!-- api: T:Inno.Rendering.Assets.ShaderMetadataEntry a52214c3e844526c7443a90eeff3835b181ed6373c821e773889993d8755acb5 -->
<!-- api: M:Inno.Rendering.Assets.ShaderMetadataEntry.#ctor(System.String,System.String) 3e6aa52aa25166d83d018ee60d4328bceb4835f0bbb018d13fac8cade089146b -->
<!-- api: P:Inno.Rendering.Assets.ShaderMetadataEntry.key 3a0cb2cebc1916f4fe26b913c7a0e49673b0adfeeed71eb202d03bae488cfb00 -->
<!-- api: P:Inno.Rendering.Assets.ShaderMetadataEntry.value 1158c3f26a048715eb6b11a3a474802c90a780bd880232d2359a896bb17f50b0 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e17b19a42c872ab2"></a>[`public Inno.Rendering.Assets.ShaderMetadataEntry.ShaderMetadataEntry(string key, string value)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderMetadataEntry.cs#L28) | Creates one metadata entry.<br>`key`：Stable provider-defined key.<br>`value`：Provider-defined value. | — | — |
| <a id="api-9acc50855cc807f3"></a>[`public string Inno.Rendering.Assets.ShaderMetadataEntry.key { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderMetadataEntry.cs#L40) | Gets or sets the stable metadata key. | — | — |
| <a id="api-b9def83f0770fe65"></a>[`public string Inno.Rendering.Assets.ShaderMetadataEntry.value { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderMetadataEntry.cs#L45) | Gets or sets the metadata value. | — | — |
| <a id="api-2d5e89b0e896c2b9"></a>[`public struct Inno.Rendering.Assets.ShaderMetadataEntry`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderMetadataEntry.cs#L17) | Stores one open metadata key and value. | — | — |

### `Inno.Rendering.Assets.ShaderPassDefinition`

<!-- api: T:Inno.Rendering.Assets.ShaderPassDefinition 65990e39acf66fdca0350996247a77715417d0d25ae3110b7c0a302433b9ac6e -->
<!-- api: M:Inno.Rendering.Assets.ShaderPassDefinition.#ctor(System.String,Inno.Rendering.ShaderProgramKind,Inno.Rendering.GraphicsCapability,System.Nullable{Inno.Rendering.Assets.ShaderRenderState},System.Collections.Generic.IEnumerable{Inno.Rendering.Assets.ShaderMetadataEntry}) 9975c8ba1fb1612d5e0a917bd2da866980667722d50eab203453229ee5a50f0e -->
<!-- api: P:Inno.Rendering.Assets.ShaderPassDefinition.name 64d3052fb07b95d0d4d5abbc54d170d488913d0ae591637d9ffe87e534fe3430 -->
<!-- api: P:Inno.Rendering.Assets.ShaderPassDefinition.programKind c8e8041cb02b0295aec9b410c71d99c9efef5ec203c83aac0465e004cba1671b -->
<!-- api: P:Inno.Rendering.Assets.ShaderPassDefinition.requiredFeatures 322b2155dfa12df6bf29480531a2745c4cea5e00da180324a14794995f93bf61 -->
<!-- api: P:Inno.Rendering.Assets.ShaderPassDefinition.renderState 5f8b17acc70cff5cb04384a00190aec2eee2673c858d0b923da96711c540925a -->
<!-- api: P:Inno.Rendering.Assets.ShaderPassDefinition.metadata 63b34db3a692628e1fb446b2f8cf8e10a9a64c0cf92c22a8ba972ee84c2f636d -->
<!-- api: M:Inno.Rendering.Assets.ShaderPassDefinition.Copy 2d2308bf7390667220a80cea00c4816076cc83e995d7984aed58a0c01427c611 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d5a55fbad5ec45ee"></a>[`public Inno.Rendering.Assets.ShaderPassDefinition.ShaderPassDefinition(string name, Inno.Rendering.ShaderProgramKind programKind, Inno.Rendering.GraphicsCapability requiredFeatures = Inno.Rendering.GraphicsCapability.None, Inno.Rendering.Assets.ShaderRenderState? renderState = null, System.Collections.Generic.IEnumerable<Inno.Rendering.Assets.ShaderMetadataEntry>? metadata = null)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPassDefinition.cs#L37) | Creates a shader pass definition.<br>`name`：Stable pass name within the shader.<br>`programKind`：Programmable stage combination.<br>`requiredFeatures`：Required device capability mask.<br>`renderState`：Backend-neutral fixed-function state.<br>`metadata`：Optional provider-defined metadata. | — | — |
| <a id="api-e6fb393be4d3205f"></a>[`public Inno.Rendering.Assets.ShaderPassDefinition Inno.Rendering.Assets.ShaderPassDefinition.Copy()`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPassDefinition.cs#L84) | Copies the pass declaration with independently owned metadata storage.<br>返回：A detached declaration whose metadata can be changed without affecting this declaration. Missing metadata is represented by an empty collection. | — | — |
| <a id="api-2d4fac53648d6ac6"></a>[`public Inno.Rendering.Assets.ShaderMetadataEntry[] Inno.Rendering.Assets.ShaderPassDefinition.metadata { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPassDefinition.cs#L75) | Gets or sets provider-defined metadata. | — | — |
| <a id="api-abd416ec9a178b0a"></a>[`public string Inno.Rendering.Assets.ShaderPassDefinition.name { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPassDefinition.cs#L55) | Gets or sets the stable pass name. | — | — |
| <a id="api-0cf35d8fe87db77a"></a>[`public Inno.Rendering.ShaderProgramKind Inno.Rendering.Assets.ShaderPassDefinition.programKind { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPassDefinition.cs#L60) | Gets or sets the programmable stage combination. | — | — |
| <a id="api-ebad19c5a37478a4"></a>[`public Inno.Rendering.Assets.ShaderRenderState Inno.Rendering.Assets.ShaderPassDefinition.renderState { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPassDefinition.cs#L70) | Gets or sets backend-neutral fixed-function state. | — | — |
| <a id="api-f198c71e131a8c8e"></a>[`public Inno.Rendering.GraphicsCapability Inno.Rendering.Assets.ShaderPassDefinition.requiredFeatures { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPassDefinition.cs#L65) | Gets or sets required device capabilities. | — | — |
| <a id="api-8b9457b5d985c28c"></a>[`public struct Inno.Rendering.Assets.ShaderPassDefinition`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPassDefinition.cs#L17) | Defines one backend-neutral raster or compute shader pass. | — | — |

### `Inno.Rendering.Assets.ShaderPassRoleId`

<!-- api: T:Inno.Rendering.Assets.ShaderPassRoleId 5a5b6923ab397ebcc258c433924df1aa281bb580a44cd39e3afd73e76b5fcc29 -->
<!-- api: M:Inno.Rendering.Assets.ShaderPassRoleId.#ctor(System.String) 902f2478b79285f4f4974340aac9140d84954c820b892fe3e0e0d1dd892c2022 -->
<!-- api: P:Inno.Rendering.Assets.ShaderPassRoleId.value 69c0077a2e41297a325916a21fbf1c3990a3d383218faae1e720834bdb5c265f -->
<!-- api: P:Inno.Rendering.Assets.ShaderPassRoleId.isValid d6d7f258279f04e11243692c69420e4c7fb3c4a6524155b6feea8784165cdb9e -->
<!-- api: M:Inno.Rendering.Assets.ShaderPassRoleId.ToString 37c69bf24fe5bfdd7ca28143cf4f730ff2e6eff7c9d3481129c948f88dfa3bb7 -->
<!-- api: M:Inno.Rendering.Assets.ShaderPassRoleId.op_Inequality(Inno.Rendering.Assets.ShaderPassRoleId,Inno.Rendering.Assets.ShaderPassRoleId) e3c07ad9e11096c8a2339751407dbeae78d80f69ff6f98314d388b2e15b5e78a -->
<!-- api: M:Inno.Rendering.Assets.ShaderPassRoleId.op_Equality(Inno.Rendering.Assets.ShaderPassRoleId,Inno.Rendering.Assets.ShaderPassRoleId) afcc2d99b1541df52eb7103814198d334048304828b11c9c90a09710811dfa21 -->
<!-- api: M:Inno.Rendering.Assets.ShaderPassRoleId.GetHashCode b5d7753c9e395ed8b1f62eb97645ee86f4ba3d4db7703fa0cbd82f8dade214a1 -->
<!-- api: M:Inno.Rendering.Assets.ShaderPassRoleId.Equals(System.Object) 1cf8a9e086742531fb9835082ae8a092e7874ab51701d0ee0eba03ff6520c92d -->
<!-- api: M:Inno.Rendering.Assets.ShaderPassRoleId.Equals(Inno.Rendering.Assets.ShaderPassRoleId) c6ddf14bf94cb8d412accc0ef950b7cdbcc404ff6e8edcd7cfc8891614cfa6e5 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-88d5e20b1f4f1cc1"></a>[`public Inno.Rendering.Assets.ShaderPassRoleId.ShaderPassRoleId(string value)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPassRoleId.cs#L25) | Creates a shader pass role identifier.<br>`value`：Stable role value within a contract. | — | — |
| <a id="api-1868ce1d8c5c1aec"></a>[`public readonly bool Inno.Rendering.Assets.ShaderPassRoleId.Equals(Inno.Rendering.Assets.ShaderPassRoleId other)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPassRoleId.cs#L17) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-d8c6b92e32683f22"></a>[`public override readonly bool Inno.Rendering.Assets.ShaderPassRoleId.Equals(object obj)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPassRoleId.cs#L17) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-57b0c2f8a83b2fdb"></a>[`public override readonly int Inno.Rendering.Assets.ShaderPassRoleId.GetHashCode()`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPassRoleId.cs#L17) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-bb14ba52b42eb808"></a>[`public static bool Inno.Rendering.Assets.ShaderPassRoleId.operator ==(Inno.Rendering.Assets.ShaderPassRoleId left, Inno.Rendering.Assets.ShaderPassRoleId right)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPassRoleId.cs#L17) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-222e5a53e50b509e"></a>[`public static bool Inno.Rendering.Assets.ShaderPassRoleId.operator !=(Inno.Rendering.Assets.ShaderPassRoleId left, Inno.Rendering.Assets.ShaderPassRoleId right)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPassRoleId.cs#L17) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-415ff84993d8e7a6"></a>[`public override readonly string Inno.Rendering.Assets.ShaderPassRoleId.ToString()`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPassRoleId.cs#L47) | Formats this value as a human-readable representation.<br>返回：The human-readable representation of this value. | — | — |
| <a id="api-ffcff8a9804b1011"></a>[`public readonly bool Inno.Rendering.Assets.ShaderPassRoleId.isValid { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPassRoleId.cs#L39) | Gets whether the identifier has a usable value. | — | — |
| <a id="api-2f8b2aa2611ab94f"></a>[`public string Inno.Rendering.Assets.ShaderPassRoleId.value { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPassRoleId.cs#L34) | Gets or sets the stable role value. | — | — |
| <a id="api-62d1b9229ee2dc65"></a>[`public record struct Inno.Rendering.Assets.ShaderPassRoleId : System.IEquatable<Inno.Rendering.Assets.ShaderPassRoleId>`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPassRoleId.cs#L17) | Identifies an open pass purpose defined by a rendering provider. | — | — |

### `Inno.Rendering.Assets.ShaderPropertyBindingOwner`

<!-- api: T:Inno.Rendering.Assets.ShaderPropertyBindingOwner e248a59ad9864d606f6ad63e471523a19f9cca281b290cffc1dba3a87645b5ca -->
<!-- api: F:Inno.Rendering.Assets.ShaderPropertyBindingOwner.Material af5103a8c5907cb41ea8ce50865f1b9b43a0217e6680391c02f84bca149db00c -->
<!-- api: F:Inno.Rendering.Assets.ShaderPropertyBindingOwner.RenderPass 81b6ef28e8f57f957cc778f5aef7e94a278d5c83c6a818ca646a04ee40aa85c5 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-eab982b77c16780e"></a>[`Inno.Rendering.Assets.ShaderPropertyBindingOwner.Material = 0`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPropertyBindingOwner.cs#L22) | The material and its optional property block supply the value. | — | — |
| <a id="api-9cfdc0b0d59d04c0"></a>[`Inno.Rendering.Assets.ShaderPropertyBindingOwner.RenderPass = 1`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPropertyBindingOwner.cs#L26) | The render pass supplies the value directly through its command encoder. | — | — |
| <a id="api-150c36b4a1aa7ce9"></a>[`public enum Inno.Rendering.Assets.ShaderPropertyBindingOwner : int`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPropertyBindingOwner.cs#L17) | Identifies which layer supplies a declared shader property's value for each dispatch or draw. | — | — |

### `Inno.Rendering.Assets.ShaderPropertyDefinition`

<!-- api: T:Inno.Rendering.Assets.ShaderPropertyDefinition a7c53695ab932ba1b847f7fe82e2018b2471953ea4b3177c3e044beed53b0749 -->
<!-- api: M:Inno.Rendering.Assets.ShaderPropertyDefinition.#ctor(Inno.Rendering.ShaderPropertyId,System.String,Inno.Rendering.ShaderPropertyType,Inno.Rendering.ShaderStage,Inno.Rendering.Assets.MaterialValue,System.Nullable{Inno.Rendering.ShaderPropertyBindingKind},Inno.Rendering.RenderStorageAccess,Inno.Rendering.Assets.ShaderPropertyBindingOwner) c7193d0aab856165a1bb0e15a2a32f42e4b7f31cea786e6640af15969b86effb -->
<!-- api: P:Inno.Rendering.Assets.ShaderPropertyDefinition.id 49ae4fe988d4df0b8430505e2b43985ce66fe6d42678ea35426ce0dc6a8ee789 -->
<!-- api: P:Inno.Rendering.Assets.ShaderPropertyDefinition.displayName f9e9e9d357104414608b8f0fba7f38e8068b07121f83765bd5157c4789affc91 -->
<!-- api: P:Inno.Rendering.Assets.ShaderPropertyDefinition.type 06a0dc87a6b7407b260e200e3e89c4664f331086a2f6f63d16ad0d92cf4edcf7 -->
<!-- api: P:Inno.Rendering.Assets.ShaderPropertyDefinition.stages b2d8815a972b32b1344741ee8279c89c2a6767c9e526d41fd15576df10b181e6 -->
<!-- api: P:Inno.Rendering.Assets.ShaderPropertyDefinition.defaultValue a95d8a90cec8be10e25d300939f9a36f0f0bb017295d1446386c4fe6740a1074 -->
<!-- api: P:Inno.Rendering.Assets.ShaderPropertyDefinition.bindingKind 1afc57522f5d50c52de1c69b80af1b2e33796d70ad9a73d7ba9504592dc3c162 -->
<!-- api: P:Inno.Rendering.Assets.ShaderPropertyDefinition.storageAccess 870efd8179e5e326932d6d5c3330e880ba47c892375c625429a350fdc7011a8b -->
<!-- api: P:Inno.Rendering.Assets.ShaderPropertyDefinition.bindingOwner 776c53ff538a0475e384e4bbd6784d507062606454e88e76c6b948b46a4c7bd8 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-18dc7f5e9564d00a"></a>[`public Inno.Rendering.Assets.ShaderPropertyDefinition.ShaderPropertyDefinition(Inno.Rendering.ShaderPropertyId id, string displayName, Inno.Rendering.ShaderPropertyType type, Inno.Rendering.ShaderStage stages, Inno.Rendering.Assets.MaterialValue defaultValue, Inno.Rendering.ShaderPropertyBindingKind? bindingKind = null, Inno.Rendering.RenderStorageAccess storageAccess = Inno.Rendering.RenderStorageAccess.Read, Inno.Rendering.Assets.ShaderPropertyBindingOwner bindingOwner = Inno.Rendering.Assets.ShaderPropertyBindingOwner.Material)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPropertyDefinition.cs#L47) | Creates a shader property definition.<br>`id`：Stable property identifier.<br>`displayName`：Artist-facing property name.<br>`type`：Property type.<br>`stages`：Stages that access the property.<br>`defaultValue`：Native serializable default value.<br>`bindingKind`：Optional explicit binding domain. When omitted, numeric values become uniforms, textures become sampled textures, and buffers become storage buffers.<br>`storageAccess`：Required access for storage texture or buffer bindings.<br>`bindingOwner`：Layer responsible for supplying the binding value at execution time. | — | — |
| <a id="api-0e021cee9c689143"></a>[`public Inno.Rendering.ShaderPropertyBindingKind Inno.Rendering.Assets.ShaderPropertyDefinition.bindingKind { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPropertyDefinition.cs#L103) | Gets or sets how the property enters the shader resource interface. | — | — |
| <a id="api-b16e57283622d457"></a>[`public Inno.Rendering.Assets.ShaderPropertyBindingOwner Inno.Rendering.Assets.ShaderPropertyDefinition.bindingOwner { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPropertyDefinition.cs#L113) | Gets or sets the layer responsible for supplying this binding at execution time. | — | — |
| <a id="api-b0dcd449a25ed027"></a>[`public Inno.Rendering.Assets.MaterialValue Inno.Rendering.Assets.ShaderPropertyDefinition.defaultValue { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPropertyDefinition.cs#L98) | Gets or sets the native serializable default value. | — | — |
| <a id="api-6a814bf4e564b75d"></a>[`public string Inno.Rendering.Assets.ShaderPropertyDefinition.displayName { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPropertyDefinition.cs#L83) | Gets or sets the artist-facing property name. | — | — |
| <a id="api-a7fcb89c06fcdb5f"></a>[`public Inno.Rendering.ShaderPropertyId Inno.Rendering.Assets.ShaderPropertyDefinition.id { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPropertyDefinition.cs#L78) | Gets or sets the stable property identifier. | — | — |
| <a id="api-4db6131a7068d761"></a>[`public Inno.Rendering.ShaderStage Inno.Rendering.Assets.ShaderPropertyDefinition.stages { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPropertyDefinition.cs#L93) | Gets or sets stages that access the property. | — | — |
| <a id="api-e9be0d1df423662b"></a>[`public Inno.Rendering.RenderStorageAccess Inno.Rendering.Assets.ShaderPropertyDefinition.storageAccess { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPropertyDefinition.cs#L108) | Gets or sets required access for storage texture or buffer bindings. | — | — |
| <a id="api-76343f9b35af5e77"></a>[`public Inno.Rendering.ShaderPropertyType Inno.Rendering.Assets.ShaderPropertyDefinition.type { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPropertyDefinition.cs#L88) | Gets or sets the property type. | — | — |
| <a id="api-6bde54bf9206013f"></a>[`public struct Inno.Rendering.Assets.ShaderPropertyDefinition`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderPropertyDefinition.cs#L17) | Declares one shader property and its reflected stage visibility. | — | — |

### `Inno.Rendering.Assets.ShaderRenderState`

<!-- api: T:Inno.Rendering.Assets.ShaderRenderState 17b67abf67c03d2d87e608bd087f4db2622b5347e671171474a6721792524d43 -->
<!-- api: P:Inno.Rendering.Assets.ShaderRenderState.opaque 3c62e4f24e915bda793b46e03b622c1cffc84edc2269910bce8b7ceea2729982 -->
<!-- api: P:Inno.Rendering.Assets.ShaderRenderState.topology d62780949f4a8cafbe693f5173ad946bf13662a07a7bd8d7e4ea2666fa893de7 -->
<!-- api: P:Inno.Rendering.Assets.ShaderRenderState.cull 8a168661d98ba1443850312dee0d92999b4582981e88bf5a38ba42004b7f89c7 -->
<!-- api: P:Inno.Rendering.Assets.ShaderRenderState.frontFace f87af62bd1e439f43795036fa40d4ce842f1ce60c8d6198b29b33d15ab37a6a1 -->
<!-- api: P:Inno.Rendering.Assets.ShaderRenderState.depthCompare a005573d90411535028b0cbe0c1a6e47951897d057535c9bb78257f53d74a601 -->
<!-- api: P:Inno.Rendering.Assets.ShaderRenderState.depthWrite 186e0aeb8273c0a5989eb5bd4de08f1264dd9a714902ca42cf9d2ad5ccfa03c0 -->
<!-- api: P:Inno.Rendering.Assets.ShaderRenderState.blend 4b391d49d8c8a22c16f96383e9c28e62ce2c769de97dc63dbbfcf09494dd21a7 -->
<!-- api: P:Inno.Rendering.Assets.ShaderRenderState.colorWriteMask 2cb308d625db4dfc7bb0821712aeadb27e0d836f2a1bd779263db570b5bd0ec4 -->
<!-- api: P:Inno.Rendering.Assets.ShaderRenderState.multisampling 4abfe24ac984bc0faa9b03b8fac20c78f2cf6485f02c1296fc36e42a080d231c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-012a4dcca95a6e03"></a>[`public Inno.Rendering.RenderBlendState Inno.Rendering.Assets.ShaderRenderState.blend { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L122) | Gets or sets independent RGB and alpha blending. | — | — |
| <a id="api-e29764dae6f819ce"></a>[`public byte Inno.Rendering.Assets.ShaderRenderState.colorWriteMask { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L127) | Gets or sets the four-bit RGBA color write mask. | — | — |
| <a id="api-9caa172ec7817318"></a>[`public Inno.Rendering.Assets.ShaderCullMode Inno.Rendering.Assets.ShaderRenderState.cull { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L102) | Gets or sets the face culling mode. | — | — |
| <a id="api-5a9cfdc6ea240df1"></a>[`public Inno.Rendering.Assets.ShaderCompareFunction Inno.Rendering.Assets.ShaderRenderState.depthCompare { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L112) | Gets or sets the depth comparison function. | — | — |
| <a id="api-2baf0fca35e3bbf5"></a>[`public bool Inno.Rendering.Assets.ShaderRenderState.depthWrite { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L117) | Gets or sets whether accepted fragments update depth. | — | — |
| <a id="api-205de8dfab6755ac"></a>[`public Inno.Rendering.RenderFrontFace Inno.Rendering.Assets.ShaderRenderState.frontFace { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L107) | Gets or sets the winding order interpreted as the front face. | — | — |
| <a id="api-2b6259a0448adc9c"></a>[`public bool Inno.Rendering.Assets.ShaderRenderState.multisampling { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L132) | Gets or sets whether compatible targets use multisample rasterization. | — | — |
| <a id="api-c06e8dffccda49fd"></a>[`public static Inno.Rendering.Assets.ShaderRenderState Inno.Rendering.Assets.ShaderRenderState.opaque { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L82) | Gets the default opaque raster state. | — | — |
| <a id="api-3c79e7adf9f1bbcf"></a>[`public Inno.Rendering.RenderPrimitiveTopology Inno.Rendering.Assets.ShaderRenderState.topology { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L97) | Gets or sets the primitive assembly used by raster draws. | — | — |
| <a id="api-0137c916220d79a7"></a>[`public struct Inno.Rendering.Assets.ShaderRenderState`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderRenderState.cs#L77) | Declares backend-neutral fixed-function state for one shader pass. | — | — |

### `Inno.Rendering.Assets.ShaderSourceLocation`

<!-- api: T:Inno.Rendering.Assets.ShaderSourceLocation 3358049c13c4331d1082a2c6a0168da0ee46dde4dc48039f4eb2b0b8ea08f766 -->
<!-- api: M:Inno.Rendering.Assets.ShaderSourceLocation.#ctor(System.String,System.String,Inno.Rendering.ShaderStage,System.Int32,System.Int32) 5cba4af786f840508952b85de55541a1fb9591e07721f09be0184437ba096216 -->
<!-- api: P:Inno.Rendering.Assets.ShaderSourceLocation.assetPath c986f1f9ef8e724e600e67da107807a6c461af5d1bd55311e34567cf71e82e0c -->
<!-- api: P:Inno.Rendering.Assets.ShaderSourceLocation.passName b3b23209bdcb896869f9113b7d413d447cb6319effd5dd02567ce9b6eee565e0 -->
<!-- api: P:Inno.Rendering.Assets.ShaderSourceLocation.stage 91dddf13c94134f948304b5a1adf73830316c26dc90279fbfe21163b3f52ea4f -->
<!-- api: P:Inno.Rendering.Assets.ShaderSourceLocation.line 357f7802f53bef51e19a9ac67ec85af2ad6d2d6830b25b8d64797b74ace14ceb -->
<!-- api: P:Inno.Rendering.Assets.ShaderSourceLocation.column f0fd25ae29a6c9ae35d0f32186e03db9c8211d110ca76354a1bed4fffd242cc4 -->
<!-- api: M:Inno.Rendering.Assets.ShaderSourceLocation.ToString 23571a099ae1fbb41253cca85db9c5ae6af872c121d332b1f50469b5f09bad33 -->
<!-- api: M:Inno.Rendering.Assets.ShaderSourceLocation.op_Inequality(Inno.Rendering.Assets.ShaderSourceLocation,Inno.Rendering.Assets.ShaderSourceLocation) df17031f7c181b2819c5ed103e32b1fa4fd4a685a8489cf19262a0f1680bdfc4 -->
<!-- api: M:Inno.Rendering.Assets.ShaderSourceLocation.op_Equality(Inno.Rendering.Assets.ShaderSourceLocation,Inno.Rendering.Assets.ShaderSourceLocation) 1d8c9456b55b7b07a80ec0b6aa7a62f024d38acc6fcd7d6c00f95dafff3acd14 -->
<!-- api: M:Inno.Rendering.Assets.ShaderSourceLocation.GetHashCode bf29949f26004a3632ed3c35c671b621ef55f55e0a06f5635a8292e2a620b223 -->
<!-- api: M:Inno.Rendering.Assets.ShaderSourceLocation.Equals(System.Object) fdb96ec2f71113b9db972cff71c078a2755b544beb3cc3d98ea5e33aa842dc87 -->
<!-- api: M:Inno.Rendering.Assets.ShaderSourceLocation.Equals(Inno.Rendering.Assets.ShaderSourceLocation) f99c6aeadceda4422887f12c0b886a0fa8e306597bc1fc66b68757caf7379751 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-2fff81c9cfafb29b"></a>[`public Inno.Rendering.Assets.ShaderSourceLocation.ShaderSourceLocation(string assetPath, string passName, Inno.Rendering.ShaderStage stage, int line = 0, int column = 0)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderSourceLocation.cs#L33) | Creates a shader source location.<br>`assetPath`：Project-relative source asset path.<br>`passName`：Stable pass name.<br>`stage`：Shader stage.<br>`line`：One-based source line, or zero when unavailable.<br>`column`：One-based source column, or zero when unavailable. | — | — |
| <a id="api-cea8deac470a372c"></a>[`public bool Inno.Rendering.Assets.ShaderSourceLocation.Equals(Inno.Rendering.Assets.ShaderSourceLocation other)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderSourceLocation.cs#L13) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-9a72dd51a611ea52"></a>[`public override bool Inno.Rendering.Assets.ShaderSourceLocation.Equals(object obj)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderSourceLocation.cs#L13) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-5b54d7956572fe2d"></a>[`public override int Inno.Rendering.Assets.ShaderSourceLocation.GetHashCode()`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderSourceLocation.cs#L13) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-fabbb162a96fba05"></a>[`public static bool Inno.Rendering.Assets.ShaderSourceLocation.operator ==(Inno.Rendering.Assets.ShaderSourceLocation left, Inno.Rendering.Assets.ShaderSourceLocation right)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderSourceLocation.cs#L13) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-6c3764fbeae3d20b"></a>[`public static bool Inno.Rendering.Assets.ShaderSourceLocation.operator !=(Inno.Rendering.Assets.ShaderSourceLocation left, Inno.Rendering.Assets.ShaderSourceLocation right)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderSourceLocation.cs#L13) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-6051383185bf9ea2"></a>[`public override string Inno.Rendering.Assets.ShaderSourceLocation.ToString()`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderSourceLocation.cs#L13) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-51afbbc369e8273f"></a>[`public string Inno.Rendering.Assets.ShaderSourceLocation.assetPath { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderSourceLocation.cs#L54) | Gets the project-relative source asset path. | — | — |
| <a id="api-3e668be1887d8ee5"></a>[`public int Inno.Rendering.Assets.ShaderSourceLocation.column { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderSourceLocation.cs#L74) | Gets the one-based column, or zero when unavailable. | — | — |
| <a id="api-c6fddc3ee8956885"></a>[`public int Inno.Rendering.Assets.ShaderSourceLocation.line { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderSourceLocation.cs#L69) | Gets the one-based line, or zero when unavailable. | — | — |
| <a id="api-f1ea8dd920efe745"></a>[`public string Inno.Rendering.Assets.ShaderSourceLocation.passName { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderSourceLocation.cs#L59) | Gets the stable pass name. | — | — |
| <a id="api-78ec00b5fd6dc436"></a>[`public Inno.Rendering.ShaderStage Inno.Rendering.Assets.ShaderSourceLocation.stage { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderSourceLocation.cs#L64) | Gets the shader stage. | — | — |
| <a id="api-f3acaf56038532ea"></a>[`public readonly record struct Inno.Rendering.Assets.ShaderSourceLocation : System.IEquatable<Inno.Rendering.Assets.ShaderSourceLocation>`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderSourceLocation.cs#L13) | Maps a shader diagnostic back to an asset, pass, stage, and source position. | — | — |

### `Inno.Rendering.Assets.ShaderTechniqueDefinition`

<!-- api: T:Inno.Rendering.Assets.ShaderTechniqueDefinition 5ccf42badcb03add5a48af7a5afe1de1bc715ecaa76df3eaf080db353d925417 -->
<!-- api: M:Inno.Rendering.Assets.ShaderTechniqueDefinition.#ctor(Inno.Rendering.Assets.ShaderTechniqueId,Inno.Rendering.Assets.ShaderContractId,System.Collections.Generic.IEnumerable{Inno.Rendering.Assets.ShaderTechniquePass},Inno.Rendering.GraphicsCapability) 305ff17ac5399ee06108097877dbd17f0411a4f741121342bdb5a7e8d3a2fdb1 -->
<!-- api: P:Inno.Rendering.Assets.ShaderTechniqueDefinition.id 2910ce8809bbb8c5845e7a1fa0a58fdf5545634f4eabeff956c119b5144bebcb -->
<!-- api: P:Inno.Rendering.Assets.ShaderTechniqueDefinition.contract 2a6c58f5a104da4dbedb1bbea12bed29d15677ff6e30402961287357000d9d8d -->
<!-- api: P:Inno.Rendering.Assets.ShaderTechniqueDefinition.passes 0b6a916f36dd9a20390207e1bf9a9ce6f1f524bfc85e267e45ca7676a6dbfa46 -->
<!-- api: P:Inno.Rendering.Assets.ShaderTechniqueDefinition.requiredFeatures 3de6e8d9b5ab83b9e8505a7588ab8c1619420c86aa4dea0d0ad0d82b3af9dac2 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-358d7f621a6d60dc"></a>[`public Inno.Rendering.Assets.ShaderTechniqueDefinition.ShaderTechniqueDefinition(Inno.Rendering.Assets.ShaderTechniqueId id, Inno.Rendering.Assets.ShaderContractId contract, System.Collections.Generic.IEnumerable<Inno.Rendering.Assets.ShaderTechniquePass> passes, Inno.Rendering.GraphicsCapability requiredFeatures = Inno.Rendering.GraphicsCapability.None)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderTechniqueDefinition.cs#L34) | Creates a shader technique definition.<br>`id`：Stable technique identifier.<br>`contract`：Open rendering-provider contract.<br>`passes`：Role-to-pass mappings.<br>`requiredFeatures`：Capabilities required by the complete technique. | — | — |
| <a id="api-a66c885cd1e3191a"></a>[`public Inno.Rendering.Assets.ShaderContractId Inno.Rendering.Assets.ShaderTechniqueDefinition.contract { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderTechniqueDefinition.cs#L59) | Gets or sets the open rendering-provider contract. | — | — |
| <a id="api-ea0449b7a03823b7"></a>[`public Inno.Rendering.Assets.ShaderTechniqueId Inno.Rendering.Assets.ShaderTechniqueDefinition.id { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderTechniqueDefinition.cs#L54) | Gets or sets the stable technique identifier. | — | — |
| <a id="api-596743001f089ba0"></a>[`public Inno.Rendering.Assets.ShaderTechniquePass[] Inno.Rendering.Assets.ShaderTechniqueDefinition.passes { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderTechniqueDefinition.cs#L64) | Gets or sets role-to-pass mappings. | — | — |
| <a id="api-1d8c33351b6782b9"></a>[`public Inno.Rendering.GraphicsCapability Inno.Rendering.Assets.ShaderTechniqueDefinition.requiredFeatures { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderTechniqueDefinition.cs#L69) | Gets or sets capabilities required by the complete technique. | — | — |
| <a id="api-89591603aa89f766"></a>[`public struct Inno.Rendering.Assets.ShaderTechniqueDefinition`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderTechniqueDefinition.cs#L17) | Declares one contract-compatible pass mapping selectable by materials. | — | — |

### `Inno.Rendering.Assets.ShaderTechniqueId`

<!-- api: T:Inno.Rendering.Assets.ShaderTechniqueId 706770a088ac072b6d31309800fd37ab5f5150c865d9a3bd7b99324385472cdd -->
<!-- api: M:Inno.Rendering.Assets.ShaderTechniqueId.#ctor(System.String) 8b8283e3972f192396cf5f6828aac254ecea9f9030ea15c37dc6542808d9f7f5 -->
<!-- api: P:Inno.Rendering.Assets.ShaderTechniqueId.value ff16c3de45b6f9e59ca5d28199287001805b42ea9561a3934c5cfa6b62444b96 -->
<!-- api: P:Inno.Rendering.Assets.ShaderTechniqueId.isValid a1df7ca87dc5a99fc8a47c520ac415268ced7988dd037fca332501afdc3010e2 -->
<!-- api: M:Inno.Rendering.Assets.ShaderTechniqueId.ToString abf9a30e67aae2a959ae7707ed6e3b2d632447e9de878342195fae3bc7227a82 -->
<!-- api: M:Inno.Rendering.Assets.ShaderTechniqueId.op_Inequality(Inno.Rendering.Assets.ShaderTechniqueId,Inno.Rendering.Assets.ShaderTechniqueId) b061178e6e57addb8ee96d7590c34ae4651f397e622f4c2b578f1346e39b752f -->
<!-- api: M:Inno.Rendering.Assets.ShaderTechniqueId.op_Equality(Inno.Rendering.Assets.ShaderTechniqueId,Inno.Rendering.Assets.ShaderTechniqueId) 99e796affc88d521062a795a1987c65ce98cdbab6694a724f9725eaba644ea98 -->
<!-- api: M:Inno.Rendering.Assets.ShaderTechniqueId.GetHashCode 08a830ec3c4e7f91a19fedc8c7671b0bef01ba9fea7ef04f01e94ed8094a5e8f -->
<!-- api: M:Inno.Rendering.Assets.ShaderTechniqueId.Equals(System.Object) f0b93e9d4e346ba99932230a6794008bcc7f67d30892eb47fd2ac286b3a0f02c -->
<!-- api: M:Inno.Rendering.Assets.ShaderTechniqueId.Equals(Inno.Rendering.Assets.ShaderTechniqueId) 3c21927f259d4d58df577c630124efc284e08b5bf9755428f61d8c33357df716 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-298f255c6e7ad2d7"></a>[`public Inno.Rendering.Assets.ShaderTechniqueId.ShaderTechniqueId(string value)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderTechniqueId.cs#L25) | Creates a shader technique identifier.<br>`value`：Stable technique value within a shader. | — | — |
| <a id="api-bec46bf9e8b52c23"></a>[`public readonly bool Inno.Rendering.Assets.ShaderTechniqueId.Equals(Inno.Rendering.Assets.ShaderTechniqueId other)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderTechniqueId.cs#L17) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-78ed36117ad9290d"></a>[`public override readonly bool Inno.Rendering.Assets.ShaderTechniqueId.Equals(object obj)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderTechniqueId.cs#L17) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-6cd86c3354a84640"></a>[`public override readonly int Inno.Rendering.Assets.ShaderTechniqueId.GetHashCode()`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderTechniqueId.cs#L17) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-32e591b2102795e7"></a>[`public static bool Inno.Rendering.Assets.ShaderTechniqueId.operator ==(Inno.Rendering.Assets.ShaderTechniqueId left, Inno.Rendering.Assets.ShaderTechniqueId right)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderTechniqueId.cs#L17) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-4ec8a2e1da9e9cdd"></a>[`public static bool Inno.Rendering.Assets.ShaderTechniqueId.operator !=(Inno.Rendering.Assets.ShaderTechniqueId left, Inno.Rendering.Assets.ShaderTechniqueId right)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderTechniqueId.cs#L17) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-34f21b5a8699e299"></a>[`public override readonly string Inno.Rendering.Assets.ShaderTechniqueId.ToString()`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderTechniqueId.cs#L47) | Formats this value as a human-readable representation.<br>返回：The human-readable representation of this value. | — | — |
| <a id="api-2d79da3274dfa9a1"></a>[`public readonly bool Inno.Rendering.Assets.ShaderTechniqueId.isValid { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderTechniqueId.cs#L39) | Gets whether the identifier has a usable value. | — | — |
| <a id="api-c8d3039dd39b279a"></a>[`public string Inno.Rendering.Assets.ShaderTechniqueId.value { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderTechniqueId.cs#L34) | Gets or sets the stable technique value. | — | — |
| <a id="api-a54df9f2d59b467b"></a>[`public record struct Inno.Rendering.Assets.ShaderTechniqueId : System.IEquatable<Inno.Rendering.Assets.ShaderTechniqueId>`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderTechniqueId.cs#L17) | Identifies one material-selectable technique in a shader. | — | — |

### `Inno.Rendering.Assets.ShaderTechniquePass`

<!-- api: T:Inno.Rendering.Assets.ShaderTechniquePass 7ee3b44869676f582ded6caa3c56704ede832827b80cfa3240d4cd9136514fa1 -->
<!-- api: M:Inno.Rendering.Assets.ShaderTechniquePass.#ctor(Inno.Rendering.Assets.ShaderPassRoleId,System.String) 6e065c67fdb35a0b7f0d141f55c0523561a7028a3a81e96ff57f18838ba0ae3a -->
<!-- api: P:Inno.Rendering.Assets.ShaderTechniquePass.role b0a0ae6d028f4bdad43183f01280d5a5c8d47d64e51bd4376e4005925e5e1599 -->
<!-- api: P:Inno.Rendering.Assets.ShaderTechniquePass.passName c9ab4a298c55971e918ab94bd551f1ecd752f388f71030bc5fe4f495738800e3 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-331dbf6a0658daca"></a>[`public Inno.Rendering.Assets.ShaderTechniquePass.ShaderTechniquePass(Inno.Rendering.Assets.ShaderPassRoleId role, string passName)`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderTechniquePass.cs#L28) | Creates a technique pass mapping.<br>`role`：Open role defined by the technique contract.<br>`passName`：Concrete pass name within the shader. | — | — |
| <a id="api-b7663bacb6910445"></a>[`public string Inno.Rendering.Assets.ShaderTechniquePass.passName { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderTechniquePass.cs#L47) | Gets or sets the concrete pass name. | — | — |
| <a id="api-1aad22c3bc0a8082"></a>[`public Inno.Rendering.Assets.ShaderPassRoleId Inno.Rendering.Assets.ShaderTechniquePass.role { get; set; }`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderTechniquePass.cs#L42) | Gets or sets the provider-defined role. | — | — |
| <a id="api-7eead97cdbd7fbef"></a>[`public struct Inno.Rendering.Assets.ShaderTechniquePass`](../../src/services/rendering/Inno.Rendering.Assets/Shaders/ShaderTechniquePass.cs#L17) | Maps one provider-defined role to one concrete shader pass. | — | — |

### `Inno.Rendering.Assets.TextureAsset`

<!-- api: T:Inno.Rendering.Assets.TextureAsset 4a365c838ab8cbe95e7a57fb99ddcd027e3334c416e133271902e65c22d67db8 -->
<!-- api: M:Inno.Rendering.Assets.TextureAsset.#ctor(System.Int32,System.Int32,Inno.Rendering.Assets.TextureColorSpace,System.String) 6473c1a445bd584a969e84c0a9fde7507cc0e49adf8835e84c5f0b0ae8d65384 -->
<!-- api: P:Inno.Rendering.Assets.TextureAsset.width 62be21d43e1573f4677eeb75de4477c49776d7f76fa5b933bd9161d52fec0c57 -->
<!-- api: P:Inno.Rendering.Assets.TextureAsset.height b3ff54c3712fefca17d3186e45ad46590aa49604b3b6d826f5dd9aee587235e4 -->
<!-- api: P:Inno.Rendering.Assets.TextureAsset.colorSpace 7073f18210a5c4b30d4b6dfa50ddce6af1b8138c93e85b2b58017277f7f48a0f -->
<!-- api: P:Inno.Rendering.Assets.TextureAsset.sourceFormat f6aedd82b039a29b8bab4d72438a119f5fe799a790ae11519103cfb2da2e8da5 -->
<!-- api: P:Inno.Rendering.Assets.TextureAsset.textureArtifacts b7b049458ebc2149fc029a0208fb4b826b27ae2be6a714386e3f230535a8067a -->
<!-- api: M:Inno.Rendering.Assets.TextureAsset.GetTextureArtifactReference cc849a4e00fded4df992a9db18574e35048bc264cf974df6880fed4d2f60b704 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-6871e0afc14923a6"></a>[`public Inno.Rendering.Assets.TextureAsset.TextureAsset(int width, int height, Inno.Rendering.Assets.TextureColorSpace colorSpace, string sourceFormat)`](../../src/services/rendering/Inno.Rendering.Assets/Textures/TextureAsset.cs#L49) | Creates an immutable imported texture description.<br>`width`：The positive source pixel width.<br>`height`：The positive source pixel height.<br>`colorSpace`：The color-space interpretation used when sampling the texture.<br>`sourceFormat`：The normalized source container name without a leading period. | — | `System.ArgumentOutOfRangeException`：Thrown when width or height is not positive.<br>`System.ArgumentException`：Thrown when sourceFormat is empty or contains only white-space characters. |
| <a id="api-ba74efd991e194eb"></a>[`public Inno.Rendering.Assets.RenderTextureArtifactReference Inno.Rendering.Assets.TextureAsset.GetTextureArtifactReference()`](../../src/services/rendering/Inno.Rendering.Assets/Textures/TextureAsset.cs#L105) | Creates a stable reference to this texture's current imported content.<br>返回：A reference suitable for target compilation and generation-scoped GPU resolution. | — | `System.InvalidOperationException`：Thrown when this texture has not been assigned a persistent asset identity. |
| <a id="api-3315430beb619f2d"></a>[`public Inno.Rendering.Assets.TextureColorSpace Inno.Rendering.Assets.TextureAsset.colorSpace { get; internal set; }`](../../src/services/rendering/Inno.Rendering.Assets/Textures/TextureAsset.cs#L80) | Gets the declared sample color space. | — | — |
| <a id="api-8fcbc3f07b945ae9"></a>[`public int Inno.Rendering.Assets.TextureAsset.height { get; internal set; }`](../../src/services/rendering/Inno.Rendering.Assets/Textures/TextureAsset.cs#L74) | Gets the source pixel height. | — | — |
| <a id="api-f5bded7254756180"></a>[`public string Inno.Rendering.Assets.TextureAsset.sourceFormat { get; internal set; }`](../../src/services/rendering/Inno.Rendering.Assets/Textures/TextureAsset.cs#L86) | Gets the normalized source container name. | — | — |
| <a id="api-70e2f18a8eca077e"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Rendering.Assets.RenderTextureArtifactSlot> Inno.Rendering.Assets.TextureAsset.textureArtifacts { get; }`](../../src/services/rendering/Inno.Rendering.Assets/Textures/TextureAsset.cs#L91) | Gets the single source artifact compiled for this imported texture. | — | — |
| <a id="api-de31b87425b3c05b"></a>[`public int Inno.Rendering.Assets.TextureAsset.width { get; internal set; }`](../../src/services/rendering/Inno.Rendering.Assets/Textures/TextureAsset.cs#L68) | Gets the source pixel width. | — | — |
| <a id="api-c7838bab97ebeea9"></a>[`public sealed class Inno.Rendering.Assets.TextureAsset : Inno.Assets.AssetObject, Inno.Rendering.Assets.IRenderTextureArtifactSource`](../../src/services/rendering/Inno.Rendering.Assets/Textures/TextureAsset.cs#L17) | Represents imported texture content without owning a GPU handle. | — | — |

### `Inno.Rendering.Assets.TextureColorSpace`

<!-- api: T:Inno.Rendering.Assets.TextureColorSpace ffeab6f240857a04f968733d97d1bb34c9e922c3b61f485d68bfbf94293ed94b -->
<!-- api: F:Inno.Rendering.Assets.TextureColorSpace.Linear 81fde6c8bf97c304616240cba10711107285b66599f41d43746111e45a79b5d8 -->
<!-- api: F:Inno.Rendering.Assets.TextureColorSpace.Srgb 0c9d7567d0865aa90770a0e71a2c339b8a3e91a3861c059cd3db19e6f0bc9005 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-4451a1a91afcaaa5"></a>[`Inno.Rendering.Assets.TextureColorSpace.Linear = 0`](../../src/services/rendering/Inno.Rendering.Assets/Textures/TextureColorSpace.cs#L22) | Samples are interpreted as linear values. | — | — |
| <a id="api-abb6a9185e56f99a"></a>[`Inno.Rendering.Assets.TextureColorSpace.Srgb = 1`](../../src/services/rendering/Inno.Rendering.Assets/Textures/TextureColorSpace.cs#L26) | Color samples are decoded from sRGB at sampling time. | — | — |
| <a id="api-87ef08dfbb58e7ca"></a>[`public enum Inno.Rendering.Assets.TextureColorSpace : int`](../../src/services/rendering/Inno.Rendering.Assets/Textures/TextureColorSpace.cs#L17) | Selects how texture samples are decoded for shader use. | — | — |

## 项目依赖

- [Inno.Extensibility.Types](../extensibility/Inno.Extensibility.Types.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Rendering](Inno.Rendering.md)：公开引用边界由实际签名核对。
- [Inno.Assets](../assets/Inno.Assets.md)：公开引用边界由实际签名核对。
- [Inno.References](../references/Inno.References.md)：公开引用边界由实际签名核对。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：公开引用边界由实际签名核对。
- [Inno.Core.Mathematics](../core/Inno.Core.Mathematics.md)：公开引用边界由实际签名核对。
- [Inno.Core.Diagnostics](../core/Inno.Core.Diagnostics.md)：公开引用边界由实际签名核对。
