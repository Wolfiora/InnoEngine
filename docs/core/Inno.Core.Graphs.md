# Inno.Core.Graphs

[上一页：Storage](Inno.Core.Collections.md) · [Core 索引](README.md) · [Wiki 首页](../README.md) · [下一页：Rendering](../rendering/README.md)

`Inno.Core.Graphs` 是不依赖 Rendering、Scene、Assets、Editor 或 ImGui 的通用图模型。它只持久化稳定 ID、Inno Serialization 中立 bytes、节点位置和连接，不保存 CLR `Type`、extension 实例或 runtime delegate。因此节点插件卸载后，文档及连线仍能完整保留。

## 职责与边界

- `GraphDocument` 拥有有序 `GraphNodeRecord`、`GraphEdgeRecord` 和中立 metadata。
- `GraphNodeId`、`GraphEdgeId`、`GraphPortId` 是稳定字符串 ID，不依赖进程内 hash。
- `GraphNodeDefinition.GetPorts` 支持根据节点中立数据生成动态端口。
- `GraphValidator` 检查方向、类型转换、端口容量、必填输入、缺失 endpoint，并复用 Core Collections DependencyGraph 检查确定性有向循环。
- Missing Node 是 warning：文档保持可编辑；真正缺失的物理 node/port endpoint 才是 error。

该项目只私有引用 `Inno.Scripting.Api` 以声明逻辑脚本 API `InnoEngine.Graphs`。Shader 图是上层消费者，Core Graph 不知道 Material、Shader 或 ImGui 语义。

## 公开 API

| API | 作用 |
| --- | --- |
| `GraphDocument` | 增删节点/边、查询节点、保存中立 metadata。 |
| `GraphNodeRecord` | 保存 definition ID、位置和稳定 property 序列化 bytes。 |
| `GraphEdgeRecord` / `GraphEndpoint` | 保存 output 到 input 的稳定连接。 |
| `GraphSerializedValue` | 通过共同 SerializationRegistry 按类型读写中立 bytes；不是独立 JSON 协议。 |
| `GraphNodeDefinition` | reload-scoped 节点定义与动态端口扩展点。 |
| `IGraphNodeDefinitionResolver` | 通过 Stable ID 查询当前 generation 候选快照。 |
| `IGraphTypeConversion` | 声明有方向的隐式类型转换。 |
| `GraphValidator.Validate` | 生成确定顺序的结构化诊断。 |

`GraphNodeDefinition` 不使用 marker Attribute。静态节点由拥有该图语义的领域 Registry 按基类或接口发现；数据驱动节点由领域 `IGraphNodeDefinitionResolver` 根据当前 generation 构造并解析。定义自身的 `id` 是唯一身份来源，不能再通过 Attribute 维护第二份 ID。

## 常见工作流

```csharp
using Inno.Core.Graphs;
using Inno.Core.Serialization;

static GraphValidationResult CreateAndValidate(
    SerializationRegistry serialization,
    IGraphNodeDefinitionResolver activeDefinitions
) {
    GraphDocument graph = new();
    GraphNodeRecord node = new(new GraphNodeId("constant-1"), "math.float");
    node.position = new GraphPosition(120f, 80f);
    node.SetValue("value", GraphSerializedValue.From(0.5f, serialization));
    graph.AddNode(node);
    return GraphValidator.Validate(graph, activeDefinitions);
}
```

## 生命周期、错误与热重载

节点、边、values、metadata 集合公开为不可写容器视图，不能强转成 List/Dictionary 绕过领域修改入口。GraphDocument 本身仍是可编辑创作对象，不等同于已编译的不可变运行快照；跨帧发布需要由 owner 显式 Clone/编译。

`GraphDocument()` 创建空文档；集合视图在文档生命周期内保持同一实例并反映领域修改。
`FindNode(GraphNodeId)` 通过文档维护的稳定 ID 索引查询，平均 O(1)，不分配委托或容器。
`AddNode`、`RemoveNode` 和 `ReplaceContents` 同步维护有序记录和索引；替换先完成候选深复制，失败保留原内容。
这只是中立记录的查询索引，不保存运行时扩展实例，也不取代 Core Identity 的 live object 协议。

`GraphDocument` 可以跨 extension generation 存活；`GraphNodeDefinition` 和 resolver 只能属于当前候选快照。Registry 切换失败时继续使用上一份 resolver。Missing Node 不删除节点、属性或连线，脚本扩展重新可用后再次验证即可恢复。

当前稳定行为没有旧 schema reader、migration、former ID 或兼容 alias。图资产 writer/reader 将由具体上层资产项目负责。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Core.Graphs.GraphDiagnostic`

<!-- api: T:Inno.Core.Graphs.GraphDiagnostic bb3f11c6b30cb436536a7c4e877f4af015029bb71d07ca3e3abeea134bd5fe59 -->
<!-- api: M:Inno.Core.Graphs.GraphDiagnostic.#ctor(System.String,System.String,Inno.Core.Diagnostics.DiagnosticSeverity,System.Nullable{Inno.Core.Graphs.GraphNodeId},System.Nullable{Inno.Core.Graphs.GraphEdgeId}) 840183d8022dfba548d290eaed7b59a77797fd9d0f0b60752dd3761c9e11fa68 -->
<!-- api: P:Inno.Core.Graphs.GraphDiagnostic.code 2b012a03aee2a1e79e63946f34ac6d055d5d017a5e207672d05446da698b53bf -->
<!-- api: P:Inno.Core.Graphs.GraphDiagnostic.message 49c102beb4e8f3d2839d52738609597952fcae7394798f08889a40febac336ec -->
<!-- api: P:Inno.Core.Graphs.GraphDiagnostic.severity 65477586913b204caecabd583f6149e60a081eaf3e666ae3e17f9af3b583bd28 -->
<!-- api: P:Inno.Core.Graphs.GraphDiagnostic.nodeId 82b5db05762f8c10950844679a77eed62d9173711b555b9887c405e4c88bbcb6 -->
<!-- api: P:Inno.Core.Graphs.GraphDiagnostic.edgeId 740ca5be763ae24012b0009bd85c9510a00174090b52b952a9b901cf9e4bdf38 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-8d13a30f83ea6dfe"></a>[`public Inno.Core.Graphs.GraphDiagnostic.GraphDiagnostic(string code, string message, Inno.Core.Diagnostics.DiagnosticSeverity severity, Inno.Core.Graphs.GraphNodeId? nodeId = null, Inno.Core.Graphs.GraphEdgeId? edgeId = null)`](../../src/foundation/core/Inno.Core.Graphs/GraphValidation.cs#L33) | Creates a graph diagnostic.<br>`code`：Stable machine-readable diagnostic code.<br>`message`：Artist-facing diagnostic text.<br>`severity`：Diagnostic impact.<br>`nodeId`：Optional related node identifier.<br>`edgeId`：Optional related edge identifier. | — | — |
| <a id="api-013710118ecc82ef"></a>[`public string Inno.Core.Graphs.GraphDiagnostic.code { get; }`](../../src/foundation/core/Inno.Core.Graphs/GraphValidation.cs#L52) | Gets the stable machine-readable diagnostic code. | — | — |
| <a id="api-1a47afdec8c31836"></a>[`public Inno.Core.Graphs.GraphEdgeId? Inno.Core.Graphs.GraphDiagnostic.edgeId { get; }`](../../src/foundation/core/Inno.Core.Graphs/GraphValidation.cs#L72) | Gets the related edge identifier, if any. | — | — |
| <a id="api-bb932dc35e049dc7"></a>[`public string Inno.Core.Graphs.GraphDiagnostic.message { get; }`](../../src/foundation/core/Inno.Core.Graphs/GraphValidation.cs#L57) | Gets the artist-facing diagnostic text. | — | — |
| <a id="api-149b98043cbff144"></a>[`public Inno.Core.Graphs.GraphNodeId? Inno.Core.Graphs.GraphDiagnostic.nodeId { get; }`](../../src/foundation/core/Inno.Core.Graphs/GraphValidation.cs#L67) | Gets the related node identifier, if any. | — | — |
| <a id="api-c6655109ccff285b"></a>[`public Inno.Core.Diagnostics.DiagnosticSeverity Inno.Core.Graphs.GraphDiagnostic.severity { get; }`](../../src/foundation/core/Inno.Core.Graphs/GraphValidation.cs#L62) | Gets the diagnostic impact. | — | — |
| <a id="api-7d63568c6ad9099e"></a>[`public sealed class Inno.Core.Graphs.GraphDiagnostic`](../../src/foundation/core/Inno.Core.Graphs/GraphValidation.cs#L13) | Reports one graph validation problem using stable document identifiers. | — | — |

### `Inno.Core.Graphs.GraphDocument`

<!-- api: T:Inno.Core.Graphs.GraphDocument c6f57f6bc6d976900e14d50cfd6cc05a43516c878dfc9e9533be2f87a94af737 -->
<!-- api: M:Inno.Core.Graphs.GraphDocument.#ctor 9a1e9a8848f197b6e76c44390d2c9e4f2d4b09890c9c4a759e8d12b67e4dd2db -->
<!-- api: P:Inno.Core.Graphs.GraphDocument.nodes 062b1d2649557fc1307d4e251f5476a0b809e2eb2b9bc52a59c8a68a93c8a757 -->
<!-- api: P:Inno.Core.Graphs.GraphDocument.edges dce80d1fea3d1f0a8a2f322ed3f6cc5320ee6e8d659561152744c7ba3de147ae -->
<!-- api: P:Inno.Core.Graphs.GraphDocument.metadata 6c47ee25864f4da9d391192bdc1a6682329b0ceff2b1410daa5ac55d5de4f52e -->
<!-- api: M:Inno.Core.Graphs.GraphDocument.Clone 298829d4ae7eb9447458605505b955177915beb15f9561a955debca4b41a08eb -->
<!-- api: M:Inno.Core.Graphs.GraphDocument.ReplaceContents(Inno.Core.Graphs.GraphDocument) 221a4b09cbf3af087a4b9d010f42b370e700d43baaa19526d91ff35d99489d1b -->
<!-- api: M:Inno.Core.Graphs.GraphDocument.AddNode(Inno.Core.Graphs.GraphNodeRecord) 46e2434f60319aa6e4657f080a70edeed0c887ed01df98b48da28669d24f8dd4 -->
<!-- api: M:Inno.Core.Graphs.GraphDocument.RemoveNode(Inno.Core.Graphs.GraphNodeId) 420985016683f7dc3840f23ba1c6f70671cdf03636cbc2b55579fef6c15f5500 -->
<!-- api: M:Inno.Core.Graphs.GraphDocument.FindNode(Inno.Core.Graphs.GraphNodeId) fec5c3d026e909092f727819cb303ae0d23edbd88e81a3b0e5ccaca73dd08d68 -->
<!-- api: M:Inno.Core.Graphs.GraphDocument.AddEdge(Inno.Core.Graphs.GraphEdgeRecord) 2c7263329abfe123170010ff1718421c7837a43e3f4515e204a8e0ae52ca98c8 -->
<!-- api: M:Inno.Core.Graphs.GraphDocument.RemoveEdge(Inno.Core.Graphs.GraphEdgeId) 80c6894c9b27d324f68d76184b4a07a18fc1fda2173e8290ade96b5c1b377a61 -->
<!-- api: M:Inno.Core.Graphs.GraphDocument.SetMetadata(System.String,Inno.Core.Graphs.GraphSerializedValue) 15b7292eb657a3c482fc2256825b8a30569b406d27162b43aac1a75db204eb42 -->
<!-- api: M:Inno.Core.Graphs.GraphDocument.RemoveMetadata(System.String) 9e2b19fae9d7506f50ce9d603000019387bce53cf55ec0b35e553252c0492cfd -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-3dbc3fd26348ef9d"></a>[`public Inno.Core.Graphs.GraphDocument.GraphDocument()`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L200) | Creates an empty document with stable, read-only collection views. | — | — |
| <a id="api-e4681dad361f3c3b"></a>[`public void Inno.Core.Graphs.GraphDocument.AddEdge(Inno.Core.Graphs.GraphEdgeRecord edge)`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L356) | Adds an edge while preserving document order.<br>`edge`：Edge record to add. | — | `System.ArgumentException`：Thrown when the edge identifier already exists. |
| <a id="api-322d885f98d9bbb8"></a>[`public void Inno.Core.Graphs.GraphDocument.AddNode(Inno.Core.Graphs.GraphNodeRecord node)`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L304) | Adds a node while preserving document order.<br>`node`：Node record to add. | — | `System.ArgumentException`：Thrown when the node identifier already exists. |
| <a id="api-b431e5c8ed439929"></a>[`public Inno.Core.Graphs.GraphDocument Inno.Core.Graphs.GraphDocument.Clone()`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L228) | Creates a deep neutral copy that shares no mutable node, edge, value, or metadata records.<br>返回：An independently mutable graph document with identical stable identifiers and order. | — | — |
| <a id="api-aa2f203e0a6f6a11"></a>[`public Inno.Core.Graphs.GraphNodeRecord? Inno.Core.Graphs.GraphDocument.FindNode(Inno.Core.Graphs.GraphNodeId nodeId)`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L345) | Finds a node by stable identifier.<br>返回：The node record, or null when absent.<br>`nodeId`：Node identifier to find. | — | — |
| <a id="api-16ee677e04abc693"></a>[`public bool Inno.Core.Graphs.GraphDocument.RemoveEdge(Inno.Core.Graphs.GraphEdgeId edgeId)`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L376) | Removes an edge by stable identifier.<br>返回：true when the edge existed; otherwise false.<br>`edgeId`：Edge identifier to remove. | — | — |
| <a id="api-4ebe9d91b950f457"></a>[`public bool Inno.Core.Graphs.GraphDocument.RemoveMetadata(string key)`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L415) | Removes graph-level metadata by stable key.<br>返回：true when metadata existed and was removed.<br>`key`：Stable metadata key. | — | — |
| <a id="api-09bebca85d9a238a"></a>[`public bool Inno.Core.Graphs.GraphDocument.RemoveNode(Inno.Core.Graphs.GraphNodeId nodeId)`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L324) | Removes a node and every edge connected to it.<br>返回：true when the node existed; otherwise false.<br>`nodeId`：Node identifier to remove. | — | — |
| <a id="api-c12900fb94ddb15f"></a>[`public void Inno.Core.Graphs.GraphDocument.ReplaceContents(Inno.Core.Graphs.GraphDocument source)`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L244) | Atomically replaces all records with deep copies from another neutral document.<br>`source`：Complete source state. | — | `System.ArgumentException`：Thrown when source is this document. |
| <a id="api-cbfecf93edcd06f1"></a>[`public void Inno.Core.Graphs.GraphDocument.SetMetadata(string key, Inno.Core.Graphs.GraphSerializedValue value)`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L397) | Creates or replaces graph-level neutral metadata.<br>`key`：Stable metadata key.<br>`value`：Serialized metadata value. | — | — |
| <a id="api-ba13933d6cb96409"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Core.Graphs.GraphEdgeRecord> Inno.Core.Graphs.GraphDocument.edges { get; }`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L215) | Gets all edge records in stable document order. | — | — |
| <a id="api-c2613ce09b9d34a8"></a>[`public System.Collections.Generic.IReadOnlyDictionary<string, Inno.Core.Graphs.GraphSerializedValue> Inno.Core.Graphs.GraphDocument.metadata { get; }`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L220) | Gets graph-level neutral metadata such as groups or comments. | — | — |
| <a id="api-bb6e8d31fc6eb141"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Core.Graphs.GraphNodeRecord> Inno.Core.Graphs.GraphDocument.nodes { get; }`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L210) | Gets all node records in stable document order. | — | — |
| <a id="api-4c6d74c85656ab7b"></a>[`public sealed class Inno.Core.Graphs.GraphDocument`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L187) | Owns neutral graph records and graph-level canvas metadata. | — | — |

### `Inno.Core.Graphs.GraphDocumentCodec`

<!-- api: T:Inno.Core.Graphs.GraphDocumentCodec 3759b937896352a9c523cf7f05b28403307bb9f3114b675f8195ef72e3dc8c80 -->
<!-- api: M:Inno.Core.Graphs.GraphDocumentCodec.Encode(Inno.Core.Graphs.GraphDocument,Inno.Core.Serialization.SerializationRegistry) 68c83512744e7a9a06f5b1885be2b9151bb1fa54bf5e780a24e29c187aef2cbc -->
<!-- api: M:Inno.Core.Graphs.GraphDocumentCodec.Decode(System.ReadOnlySpan{System.Byte},Inno.Core.Serialization.SerializationRegistry) 8ab515e1bd68967d005de222cc19cf5757c96ddd626c92fb0dd47195218cec65 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-15a2a3d8c47646ef"></a>[`public static Inno.Core.Graphs.GraphDocument Inno.Core.Graphs.GraphDocumentCodec.Decode(System.ReadOnlySpan<byte> bytes, Inno.Core.Serialization.SerializationRegistry serialization)`](../../src/foundation/core/Inno.Core.Graphs/GraphDocumentCodec.cs#L90) | Decodes one current native graph document.<br>返回：A detached mutable neutral graph document.<br>`bytes`：Complete native graph payload.<br>`serialization`：The serialization registry that owns the converter generation for this operation. | — | — |
| <a id="api-3381f5150a4c2f33"></a>[`public static byte[] Inno.Core.Graphs.GraphDocumentCodec.Encode(Inno.Core.Graphs.GraphDocument document, Inno.Core.Serialization.SerializationRegistry serialization)`](../../src/foundation/core/Inno.Core.Graphs/GraphDocumentCodec.cs#L24) | Encodes a graph document into deterministic native bytes.<br>返回：Native graph bytes containing stable IDs and neutral values only.<br>`document`：Document to encode.<br>`serialization`：The serialization registry that owns the converter generation for this operation. | — | — |
| <a id="api-9936d2431e681f25"></a>[`public static class Inno.Core.Graphs.GraphDocumentCodec`](../../src/foundation/core/Inno.Core.Graphs/GraphDocumentCodec.cs#L10) | Persists neutral graph documents through the common Inno serialization pipeline. | — | — |

### `Inno.Core.Graphs.GraphEdgeId`

<!-- api: T:Inno.Core.Graphs.GraphEdgeId d906508d5a9df6841e60d31f05df3e7565a06758f3dcada633bb9918f0e14748 -->
<!-- api: M:Inno.Core.Graphs.GraphEdgeId.#ctor(System.String) 3c44fcbd0fecf332d166bdd6032464df6201156121745c5809b0f38f984c0c97 -->
<!-- api: P:Inno.Core.Graphs.GraphEdgeId.value f35d8bd057d4edbbf336ac494d0543db83f48c3a71a3057415e42b279ef5f596 -->
<!-- api: M:Inno.Core.Graphs.GraphEdgeId.ToString 386d78976f416272e4e782fb09d0ff67495debe2e067fd5864299b4733b43d11 -->
<!-- api: M:Inno.Core.Graphs.GraphEdgeId.op_Inequality(Inno.Core.Graphs.GraphEdgeId,Inno.Core.Graphs.GraphEdgeId) d9e21071da31acb597bcf962acbe3dbcdc7e1c0765a8ec9ea31cabe706542c6b -->
<!-- api: M:Inno.Core.Graphs.GraphEdgeId.op_Equality(Inno.Core.Graphs.GraphEdgeId,Inno.Core.Graphs.GraphEdgeId) 4864b6b6d5b5d1a202a769c61beece483659de53baafb598d6b04f626cc907b2 -->
<!-- api: M:Inno.Core.Graphs.GraphEdgeId.GetHashCode 5a6264b6d801e2fd1dd37ac602e544ddaddf2e227c90a38262ece3e17ec97fce -->
<!-- api: M:Inno.Core.Graphs.GraphEdgeId.Equals(System.Object) f841d1045710d8e43bacce2f75f2695e745c1cfe87fc07316800df2e48d4781a -->
<!-- api: M:Inno.Core.Graphs.GraphEdgeId.Equals(Inno.Core.Graphs.GraphEdgeId) 558a000240cd25f71bb0ca120e654a647834a35dfc93d51f9d6568fc3dbc52eb -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-4303f5457f16b673"></a>[`public Inno.Core.Graphs.GraphEdgeId.GraphEdgeId(string value)`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L53) | Creates a stable graph edge identifier.<br>`value`：Non-empty identifier value. | — | `System.ArgumentException`：Thrown when value is empty. |
| <a id="api-b41cfb828da06f73"></a>[`public bool Inno.Core.Graphs.GraphEdgeId.Equals(Inno.Core.Graphs.GraphEdgeId other)`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L42) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-d735db3fe8a21f0d"></a>[`public override bool Inno.Core.Graphs.GraphEdgeId.Equals(object obj)`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L42) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-0febebc6b71e6d79"></a>[`public override int Inno.Core.Graphs.GraphEdgeId.GetHashCode()`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L42) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-a146165a0f075867"></a>[`public static bool Inno.Core.Graphs.GraphEdgeId.operator ==(Inno.Core.Graphs.GraphEdgeId left, Inno.Core.Graphs.GraphEdgeId right)`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L42) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-8b92105d2edd9aa6"></a>[`public static bool Inno.Core.Graphs.GraphEdgeId.operator !=(Inno.Core.Graphs.GraphEdgeId left, Inno.Core.Graphs.GraphEdgeId right)`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L42) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-ecc16abd16379c63"></a>[`public override string Inno.Core.Graphs.GraphEdgeId.ToString()`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L70) | Formats this value as a human-readable representation.<br>返回：The human-readable representation of this value. | — | — |
| <a id="api-acf79dd7ea79e43c"></a>[`public string Inno.Core.Graphs.GraphEdgeId.value { get; }`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L62) | Gets the serialized identifier value. | — | — |
| <a id="api-326e87fd23b6e7f6"></a>[`public readonly record struct Inno.Core.Graphs.GraphEdgeId : System.IEquatable<Inno.Core.Graphs.GraphEdgeId>`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L42) | Identifies an edge within one graph document using a stable serialized value. | — | — |

### `Inno.Core.Graphs.GraphEdgeRecord`

<!-- api: T:Inno.Core.Graphs.GraphEdgeRecord 73bdaf840d80a6dd45110f783431c7e8016157018ff7f3068918ff7da77765b4 -->
<!-- api: M:Inno.Core.Graphs.GraphEdgeRecord.#ctor(Inno.Core.Graphs.GraphEdgeId,Inno.Core.Graphs.GraphEndpoint,Inno.Core.Graphs.GraphEndpoint) 81fa51b82b3996c7d6f0b4bc71b8c1c5cf78d2d6950cb20929ad9c84e6f14533 -->
<!-- api: P:Inno.Core.Graphs.GraphEdgeRecord.id 1d3e04f054e54623c442b57e161edc16e14273db302e9702c27c0c9c1685e259 -->
<!-- api: P:Inno.Core.Graphs.GraphEdgeRecord.output 0e5b074fd792b3077ed05587a20157013573803d32b4c9b56e1849e81ff63165 -->
<!-- api: P:Inno.Core.Graphs.GraphEdgeRecord.input 5ae1c0b1835d91d41af937de3be3361907c3147b18c0ed3d5b54a55d36c69c7d -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-7fc902456133d187"></a>[`public Inno.Core.Graphs.GraphEdgeRecord.GraphEdgeRecord(Inno.Core.Graphs.GraphEdgeId id, Inno.Core.Graphs.GraphEndpoint output, Inno.Core.Graphs.GraphEndpoint input)`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L158) | Creates a graph edge record.<br>`id`：Stable edge identifier within the document.<br>`output`：Source endpoint.<br>`input`：Destination endpoint. | — | — |
| <a id="api-dd1302d0ecfdca7a"></a>[`public Inno.Core.Graphs.GraphEdgeId Inno.Core.Graphs.GraphEdgeRecord.id { get; }`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L171) | Gets the stable edge identifier. | — | — |
| <a id="api-85765a222ce0c745"></a>[`public Inno.Core.Graphs.GraphEndpoint Inno.Core.Graphs.GraphEdgeRecord.input { get; }`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L181) | Gets the destination endpoint. | — | — |
| <a id="api-294700010ad79b42"></a>[`public Inno.Core.Graphs.GraphEndpoint Inno.Core.Graphs.GraphEdgeRecord.output { get; }`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L176) | Gets the source endpoint. | — | — |
| <a id="api-e433f4764eba6315"></a>[`public sealed class Inno.Core.Graphs.GraphEdgeRecord`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L144) | Stores one typed connection between two graph endpoints. | — | — |

### `Inno.Core.Graphs.GraphEndpoint`

<!-- api: T:Inno.Core.Graphs.GraphEndpoint 1eca7350377fc6034f81cf21653c2e0f99fddeb7882cc0f0ab0304ab7897c78c -->
<!-- api: M:Inno.Core.Graphs.GraphEndpoint.#ctor(Inno.Core.Graphs.GraphNodeId,Inno.Core.Graphs.GraphPortId) 4e18a68547c294fd188be2b347fcff5d3a5d04b35793c8402a977bc5649f9dce -->
<!-- api: P:Inno.Core.Graphs.GraphEndpoint.nodeId 859ab53501bf178a861077c6d1dbd7dfb6a8ce3f10a11009294f5d8a55402818 -->
<!-- api: P:Inno.Core.Graphs.GraphEndpoint.portId fe5fbd7cd63e9c1a57982ee5a343e9e13f32741c91a21f7dea5ff0c5980d74f4 -->
<!-- api: M:Inno.Core.Graphs.GraphEndpoint.ToString 15f4a420e7ae4a48d67b0ae383db2f4ae1bb582a82f21ba2f4a7a486e4a28f0d -->
<!-- api: M:Inno.Core.Graphs.GraphEndpoint.op_Inequality(Inno.Core.Graphs.GraphEndpoint,Inno.Core.Graphs.GraphEndpoint) c80b97d51f973f1d580bc7849c608984c51b338b85715cf23682216947ced277 -->
<!-- api: M:Inno.Core.Graphs.GraphEndpoint.op_Equality(Inno.Core.Graphs.GraphEndpoint,Inno.Core.Graphs.GraphEndpoint) 71082fd9437be81452edcc5ed847901d31b91329c28f808707016f37a34b102c -->
<!-- api: M:Inno.Core.Graphs.GraphEndpoint.GetHashCode cc15d0a3bdb0a802362536b1384c4f66fd49ad292377040485d83485e9d64a72 -->
<!-- api: M:Inno.Core.Graphs.GraphEndpoint.Equals(System.Object) eaf3dd6bc96511c5c00125722bfa15950e95346726ddc674ff5493edf7b3e5f1 -->
<!-- api: M:Inno.Core.Graphs.GraphEndpoint.Equals(Inno.Core.Graphs.GraphEndpoint) 296bd31e3bfd8d61120962f34f0788cc6c5818ea9beba96adc922b1f3d19c31f -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-10838bbeb1feb995"></a>[`public Inno.Core.Graphs.GraphEndpoint.GraphEndpoint(Inno.Core.Graphs.GraphNodeId nodeId, Inno.Core.Graphs.GraphPortId portId)`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L122) | Creates a graph edge endpoint.<br>`nodeId`：Owning node identifier.<br>`portId`：Port identifier within the node definition. | — | — |
| <a id="api-0b77d948b1305c34"></a>[`public bool Inno.Core.Graphs.GraphEndpoint.Equals(Inno.Core.Graphs.GraphEndpoint other)`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L111) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-9b278cb5e304bd9d"></a>[`public override bool Inno.Core.Graphs.GraphEndpoint.Equals(object obj)`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L111) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-1a87388930519023"></a>[`public override int Inno.Core.Graphs.GraphEndpoint.GetHashCode()`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L111) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-21c92508b31e1c93"></a>[`public static bool Inno.Core.Graphs.GraphEndpoint.operator ==(Inno.Core.Graphs.GraphEndpoint left, Inno.Core.Graphs.GraphEndpoint right)`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L111) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-136dca5850f2beb2"></a>[`public static bool Inno.Core.Graphs.GraphEndpoint.operator !=(Inno.Core.Graphs.GraphEndpoint left, Inno.Core.Graphs.GraphEndpoint right)`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L111) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-2a396a7d90fb0fb5"></a>[`public override string Inno.Core.Graphs.GraphEndpoint.ToString()`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L111) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-3c4532a6064fea9b"></a>[`public Inno.Core.Graphs.GraphNodeId Inno.Core.Graphs.GraphEndpoint.nodeId { get; }`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L133) | Gets the owning node identifier. | — | — |
| <a id="api-d06644a3165b96ef"></a>[`public Inno.Core.Graphs.GraphPortId Inno.Core.Graphs.GraphEndpoint.portId { get; }`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L138) | Gets the node-local port identifier. | — | — |
| <a id="api-0c75b06f9ab0b057"></a>[`public readonly record struct Inno.Core.Graphs.GraphEndpoint : System.IEquatable<Inno.Core.Graphs.GraphEndpoint>`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L111) | Identifies one endpoint of a graph edge. | — | — |

### `Inno.Core.Graphs.GraphNodeDefinition`

<!-- api: T:Inno.Core.Graphs.GraphNodeDefinition 181d7f45943032796e7b32bb6db54cf44836e5aba875f1187948efbe54f3cc61 -->
<!-- api: M:Inno.Core.Graphs.GraphNodeDefinition.#ctor(System.String,System.String,System.String) 4bb628d25a3558fa9fb5b107e760de86620baaae64957b04807892580c086410 -->
<!-- api: P:Inno.Core.Graphs.GraphNodeDefinition.id b92e92a9b4a178a133ca04a76a3770ee50af4ed0dd8e9f64d5e459a2b3cffddb -->
<!-- api: P:Inno.Core.Graphs.GraphNodeDefinition.displayName ed3a00c45ac8dadfa6e13fc88bd830b4d40cf1c63b30ff060f57733f95d42519 -->
<!-- api: P:Inno.Core.Graphs.GraphNodeDefinition.category 67b14837aa306c55874ff4e0ac60e59991edb9c501ec4badd9d3cdac0b450bf9 -->
<!-- api: M:Inno.Core.Graphs.GraphNodeDefinition.GetPorts(Inno.Core.Graphs.GraphNodeRecord) 7a307ec66d930381b623e5c42d1e5c99d41ebf9fba5e33dbc8a610827e33c070 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-77b0d5f17676d6d0"></a>[`protected Inno.Core.Graphs.GraphNodeDefinition.GraphNodeDefinition(string id, string displayName, string category)`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L128) | Creates a graph node definition.<br>`id`：Globally stable definition identifier.<br>`displayName`：Artist-facing display name.<br>`category`：Search-menu category path. | — | — |
| <a id="api-3712d4aaef3e1885"></a>[`public abstract System.Collections.Generic.IReadOnlyList<Inno.Core.Graphs.GraphPortDefinition> Inno.Core.Graphs.GraphNodeDefinition.GetPorts(Inno.Core.Graphs.GraphNodeRecord node)`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L165) | Resolves ports for one node record, including any data-driven dynamic ports.<br>返回：The complete port definitions for the current node state.<br>`node`：Neutral node record to inspect. | — | — |
| <a id="api-7f0f879d64185fb2"></a>[`public string Inno.Core.Graphs.GraphNodeDefinition.category { get; }`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L154) | Gets the search-menu category path. | — | — |
| <a id="api-bdf24df0db68f106"></a>[`public string Inno.Core.Graphs.GraphNodeDefinition.displayName { get; }`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L149) | Gets the artist-facing display name. | — | — |
| <a id="api-239bc3ce0304066f"></a>[`public string Inno.Core.Graphs.GraphNodeDefinition.id { get; }`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L144) | Gets the globally stable definition identifier. | — | — |
| <a id="api-d5402885075c5c99"></a>[`public abstract class Inno.Core.Graphs.GraphNodeDefinition`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L114) | Describes node presentation and resolves ports without entering graph persistence. | — | — |

### `Inno.Core.Graphs.GraphNodeId`

<!-- api: T:Inno.Core.Graphs.GraphNodeId 716f26c579831176c863ca4150b73203f5af77d01ecbd2f8d17f06a791911d9d -->
<!-- api: M:Inno.Core.Graphs.GraphNodeId.#ctor(System.String) 396fd9cbc55d2b1f5864bcf45eab4d3913ee3ab7827b37b83e6c0302cdb9a835 -->
<!-- api: P:Inno.Core.Graphs.GraphNodeId.value ede8abe43c14e5dac976d14e4f6a80fbb1554401a910141b8772c1b6195669df -->
<!-- api: M:Inno.Core.Graphs.GraphNodeId.ToString c365446e9f1932d93c02598bf3cbbd418328e2a70f41c56b4ae2ee3aa3fda509 -->
<!-- api: M:Inno.Core.Graphs.GraphNodeId.op_Inequality(Inno.Core.Graphs.GraphNodeId,Inno.Core.Graphs.GraphNodeId) 2e40c05b51dd2403ddb4571366b452962ad385e061d4dbe0015defd9ae3e3339 -->
<!-- api: M:Inno.Core.Graphs.GraphNodeId.op_Equality(Inno.Core.Graphs.GraphNodeId,Inno.Core.Graphs.GraphNodeId) d32e64b5678a2aa09d26abdc544d24ea368a8eaf69ddef2ef01dcdedeb46de17 -->
<!-- api: M:Inno.Core.Graphs.GraphNodeId.GetHashCode d0163f89fee79d64d6932c5ec304a5caab5039d4f14dc5f2a8fba2351755f36f -->
<!-- api: M:Inno.Core.Graphs.GraphNodeId.Equals(System.Object) 0f30a9c536190b552017b4c9a9ca5708d23b385c54cc9ce60cced26e5ec95df2 -->
<!-- api: M:Inno.Core.Graphs.GraphNodeId.Equals(Inno.Core.Graphs.GraphNodeId) cbb94f33918dbba5af218dac13a85c8121eb570c3830cee415ec237995634b52 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b43f233a0a04abc4"></a>[`public Inno.Core.Graphs.GraphNodeId.GraphNodeId(string value)`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L19) | Creates a stable graph node identifier.<br>`value`：Non-empty identifier value. | — | `System.ArgumentException`：Thrown when value is empty. |
| <a id="api-2e3a131567ab7804"></a>[`public bool Inno.Core.Graphs.GraphNodeId.Equals(Inno.Core.Graphs.GraphNodeId other)`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L8) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-32e32cdbb307aaac"></a>[`public override bool Inno.Core.Graphs.GraphNodeId.Equals(object obj)`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L8) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-7e960a4aa3e33580"></a>[`public override int Inno.Core.Graphs.GraphNodeId.GetHashCode()`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L8) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-5ac77a61670a7536"></a>[`public static bool Inno.Core.Graphs.GraphNodeId.operator ==(Inno.Core.Graphs.GraphNodeId left, Inno.Core.Graphs.GraphNodeId right)`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-1a34225a613fa955"></a>[`public static bool Inno.Core.Graphs.GraphNodeId.operator !=(Inno.Core.Graphs.GraphNodeId left, Inno.Core.Graphs.GraphNodeId right)`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L8) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-e813739ccf657859"></a>[`public override string Inno.Core.Graphs.GraphNodeId.ToString()`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L36) | Formats this value as a human-readable representation.<br>返回：The human-readable representation of this value. | — | — |
| <a id="api-af5d5872e19955ba"></a>[`public string Inno.Core.Graphs.GraphNodeId.value { get; }`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L28) | Gets the serialized identifier value. | — | — |
| <a id="api-7df3eec586f83774"></a>[`public readonly record struct Inno.Core.Graphs.GraphNodeId : System.IEquatable<Inno.Core.Graphs.GraphNodeId>`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L8) | Identifies a node within one graph document using a stable serialized value. | — | — |

### `Inno.Core.Graphs.GraphNodeRecord`

<!-- api: T:Inno.Core.Graphs.GraphNodeRecord a694321bca35b363c2b89416363da703c0f47e0ebb1ad6d6c3f00716d3c230cd -->
<!-- api: M:Inno.Core.Graphs.GraphNodeRecord.#ctor(Inno.Core.Graphs.GraphNodeId,System.String) f616a87314fbeae143f54c63745253f7e5a6bd6cd2e3ca664e4192a45c035b2a -->
<!-- api: P:Inno.Core.Graphs.GraphNodeRecord.id 46db1dcd1ba7fd730c550cee27d16211e2d7398df5010e3c25303043b60c4998 -->
<!-- api: P:Inno.Core.Graphs.GraphNodeRecord.definitionId a8eb40e0673423f315de546445145f2141dcac7a9e554c8ace91044f86271623 -->
<!-- api: P:Inno.Core.Graphs.GraphNodeRecord.position cb4233843dd87dfa7c0ec90e54dab552786ea616ef11f02ba244056055908755 -->
<!-- api: P:Inno.Core.Graphs.GraphNodeRecord.values d5f21b9991fb349b54aa6909f6fdae62d706a9351c8248598d2978a08fa76d23 -->
<!-- api: M:Inno.Core.Graphs.GraphNodeRecord.SetValue(System.String,Inno.Core.Graphs.GraphSerializedValue) dc8c46fed0836a3c4d6457ae2f2503c330c42d29f365acd6299435577794a8ee -->
<!-- api: M:Inno.Core.Graphs.GraphNodeRecord.TryGetValue(System.String,Inno.Core.Graphs.GraphSerializedValue@) d34a6f9f918cd2aa683795e4ec53d02c692ddf449e61d84d99d5bda463f41eed -->
<!-- api: M:Inno.Core.Graphs.GraphNodeRecord.RemoveValue(System.String) 2b51dc8f9612ac830a0c0c7761f6267a018390ac07a2cb0e943459201d3d5827 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-508ddcd2e69a0c10"></a>[`public Inno.Core.Graphs.GraphNodeRecord.GraphNodeRecord(Inno.Core.Graphs.GraphNodeId id, string definitionId)`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L24) | Creates a graph node record.<br>`id`：Stable identifier within the document.<br>`definitionId`：Stable node definition identifier. | — | — |
| <a id="api-9f7492bd9accabda"></a>[`public bool Inno.Core.Graphs.GraphNodeRecord.RemoveValue(string propertyId)`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L101) | Removes a neutral serialized property value.<br>返回：true when a property was removed; otherwise false.<br>`propertyId`：Stable property identifier. | — | — |
| <a id="api-519d65e986f48ce1"></a>[`public void Inno.Core.Graphs.GraphNodeRecord.SetValue(string propertyId, Inno.Core.Graphs.GraphSerializedValue value)`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L63) | Creates or replaces a neutral serialized property value.<br>`propertyId`：Stable property identifier.<br>`value`：Serialized value. | — | — |
| <a id="api-72c82aa853031547"></a>[`public bool Inno.Core.Graphs.GraphNodeRecord.TryGetValue(string propertyId, out Inno.Core.Graphs.GraphSerializedValue? value)`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L84) | Tries to read a neutral serialized property value.<br>返回：true when the property exists; otherwise false.<br>`propertyId`：Stable property identifier.<br>`value`：Receives the value when present. | — | — |
| <a id="api-3d33f88c9b7c5e53"></a>[`public string Inno.Core.Graphs.GraphNodeRecord.definitionId { get; }`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L42) | Gets the stable node definition identifier. | — | — |
| <a id="api-a703485c5790a5ed"></a>[`public Inno.Core.Graphs.GraphNodeId Inno.Core.Graphs.GraphNodeRecord.id { get; }`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L37) | Gets the stable node identifier. | — | — |
| <a id="api-9e0464bd248615ce"></a>[`public Inno.Core.Graphs.GraphPosition Inno.Core.Graphs.GraphNodeRecord.position { get; set; }`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L47) | Gets or sets the graph-space node position. | — | — |
| <a id="api-4aaa8a26ef48761b"></a>[`public System.Collections.Generic.IReadOnlyDictionary<string, Inno.Core.Graphs.GraphSerializedValue> Inno.Core.Graphs.GraphNodeRecord.values { get; }`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L52) | Gets neutral serialized property values keyed by stable property identifier. | — | — |
| <a id="api-ce1a8fa54b6e05bc"></a>[`public sealed class Inno.Core.Graphs.GraphNodeRecord`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L10) | Stores one node without retaining a CLR type or runtime extension instance. | — | — |

### `Inno.Core.Graphs.GraphPortCapacity`

<!-- api: T:Inno.Core.Graphs.GraphPortCapacity 021b9f65ab8deeea61be9c1d2b419aa2befb446e1e3854c4b47e736235328851 -->
<!-- api: F:Inno.Core.Graphs.GraphPortCapacity.Single 8b9aec7066566cbca252de05ed0957b8be0cf18afe9e0fecf724d9c17b312616 -->
<!-- api: F:Inno.Core.Graphs.GraphPortCapacity.Multiple 4fcea5ea5656fa10d82289c20c4ef81355efed1a90cbc96fb87ad08a0111f507 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-28e03d5786ce3350"></a>[`Inno.Core.Graphs.GraphPortCapacity.Multiple = 1`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L33) | Any number of edges may connect to the port. | — | — |
| <a id="api-e2c7d82eac0ad1d3"></a>[`Inno.Core.Graphs.GraphPortCapacity.Single = 0`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L29) | At most one edge may connect to the port. | — | — |
| <a id="api-bfb56b4b28c64963"></a>[`public enum Inno.Core.Graphs.GraphPortCapacity : int`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L24) | Declares how many edges may connect to a graph port. | — | — |

### `Inno.Core.Graphs.GraphPortDefinition`

<!-- api: T:Inno.Core.Graphs.GraphPortDefinition 6e8b1082fbe99996c83e0935e17768a5beebbd0ebdd2626c7f864ea4c2e3de49 -->
<!-- api: M:Inno.Core.Graphs.GraphPortDefinition.#ctor(Inno.Core.Graphs.GraphPortId,System.String,System.String,Inno.Core.Graphs.GraphPortDirection,Inno.Core.Graphs.GraphPortCapacity,System.Boolean) d0b8dabb0ad8a905d3c19253e655bb4ce71277713c3298be11f325c44c31820e -->
<!-- api: P:Inno.Core.Graphs.GraphPortDefinition.id 9a5d8405d6129104e002cae87ad30d7dc209cfea6c0ebb2d0cf51b8b4368e4ca -->
<!-- api: P:Inno.Core.Graphs.GraphPortDefinition.displayName 5c3f01422fe1b19b622c2efe6c4bb40e4ddb5bfe68593907497985ae0f981164 -->
<!-- api: P:Inno.Core.Graphs.GraphPortDefinition.valueTypeId d98b043bec5e9e4557166f020d8c9db1bfdcd615438ef11c62754073b0101014 -->
<!-- api: P:Inno.Core.Graphs.GraphPortDefinition.direction d119b9239308b207e90573160056c2267966555e292977eb8b2095cb495446b8 -->
<!-- api: P:Inno.Core.Graphs.GraphPortDefinition.capacity 15319b660a0cdc18d656627a4667b2e52423c8c76aa357c90dbef8f33aad2802 -->
<!-- api: P:Inno.Core.Graphs.GraphPortDefinition.required 887a54713cbbf302ced90b7263e70620d9b610dbd88e8abc386eed3708b64107 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-264a84c4a1047ecb"></a>[`public Inno.Core.Graphs.GraphPortDefinition.GraphPortDefinition(Inno.Core.Graphs.GraphPortId id, string displayName, string valueTypeId, Inno.Core.Graphs.GraphPortDirection direction, Inno.Core.Graphs.GraphPortCapacity capacity = Inno.Core.Graphs.GraphPortCapacity.Single, bool required = false)`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L62) | Creates a graph port definition.<br>`id`：Stable node-local port identifier.<br>`displayName`：Artist-facing display name.<br>`valueTypeId`：Stable value type identifier.<br>`direction`：Value-flow direction.<br>`capacity`：Allowed edge capacity.<br>`required`：Whether an input must be connected. | — | — |
| <a id="api-51309eeb22998819"></a>[`public Inno.Core.Graphs.GraphPortCapacity Inno.Core.Graphs.GraphPortDefinition.capacity { get; }`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L103) | Gets the allowed edge capacity. | — | — |
| <a id="api-e9735356cc7cbcdf"></a>[`public Inno.Core.Graphs.GraphPortDirection Inno.Core.Graphs.GraphPortDefinition.direction { get; }`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L98) | Gets the value-flow direction. | — | — |
| <a id="api-ce4a4182fa42810b"></a>[`public string Inno.Core.Graphs.GraphPortDefinition.displayName { get; }`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L88) | Gets the artist-facing display name. | — | — |
| <a id="api-869b0c1c13981752"></a>[`public Inno.Core.Graphs.GraphPortId Inno.Core.Graphs.GraphPortDefinition.id { get; }`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L83) | Gets the stable node-local port identifier. | — | — |
| <a id="api-d5747bdf90427680"></a>[`public bool Inno.Core.Graphs.GraphPortDefinition.required { get; }`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L108) | Gets whether an input must be connected. | — | — |
| <a id="api-1596413b542da1b4"></a>[`public string Inno.Core.Graphs.GraphPortDefinition.valueTypeId { get; }`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L93) | Gets the stable value type identifier. | — | — |
| <a id="api-4c8e935f53afde32"></a>[`public sealed class Inno.Core.Graphs.GraphPortDefinition`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L39) | Describes one dynamically or statically resolved node port. | — | — |

### `Inno.Core.Graphs.GraphPortDirection`

<!-- api: T:Inno.Core.Graphs.GraphPortDirection 6ec0ffd4a25ad5cf1fe3d6c25a01604460ea32ab3dc3ec380d94e0397ed83154 -->
<!-- api: F:Inno.Core.Graphs.GraphPortDirection.Input c5d5bc2b79261a7050cf3d29a4b9ba0b34a0c0e22b0c5dfa3ffeb2b61da80173 -->
<!-- api: F:Inno.Core.Graphs.GraphPortDirection.Output f59e6bb57e1fd794339af9e53aff5023f8a1841ddb26fdcc53aeffcbfd5d3cb1 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-c56f6ddd70052505"></a>[`Inno.Core.Graphs.GraphPortDirection.Input = 0`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L14) | Receives a value from another node. | — | — |
| <a id="api-85e064e80b29d06d"></a>[`Inno.Core.Graphs.GraphPortDirection.Output = 1`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L18) | Produces a value for another node. | — | — |
| <a id="api-38d846da63d8ef99"></a>[`public enum Inno.Core.Graphs.GraphPortDirection : int`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L9) | Declares the direction in which values flow through a graph port. | — | — |

### `Inno.Core.Graphs.GraphPortId`

<!-- api: T:Inno.Core.Graphs.GraphPortId 4de6587b7ebed6d8948267d112cce849547cd048ce09daeea5521f6847221902 -->
<!-- api: M:Inno.Core.Graphs.GraphPortId.#ctor(System.String) a97e0d569d336e91875bd49106933fc8de1d160a64c3c282bf6cee61e6ee62d1 -->
<!-- api: P:Inno.Core.Graphs.GraphPortId.value 2b18c20e3efcbe371c673d92923e47bb7009661d1894b3b617c5f4a6346af338 -->
<!-- api: M:Inno.Core.Graphs.GraphPortId.ToString c54773dd3104f8060261e6a09bf1e79d1859d4e79950ec7b0e6d44cf2c524560 -->
<!-- api: M:Inno.Core.Graphs.GraphPortId.op_Inequality(Inno.Core.Graphs.GraphPortId,Inno.Core.Graphs.GraphPortId) 60d5878b2ef5621371d5800e6c63bd43e97f10757288db63f66fbcf83befb554 -->
<!-- api: M:Inno.Core.Graphs.GraphPortId.op_Equality(Inno.Core.Graphs.GraphPortId,Inno.Core.Graphs.GraphPortId) a640d11ea94e90d3f6bf941728380bbe84d645e9ebd18a6d61f90021d078ed03 -->
<!-- api: M:Inno.Core.Graphs.GraphPortId.GetHashCode da524fe6259cdcc05e7ea05edd006528a8116b6dccba5076259dc8ccca72cbcb -->
<!-- api: M:Inno.Core.Graphs.GraphPortId.Equals(System.Object) 42e1be4dc3a6fb3dac280ca804d4a1c1f7db4b0c01413a35ba40741bb89bd39e -->
<!-- api: M:Inno.Core.Graphs.GraphPortId.Equals(Inno.Core.Graphs.GraphPortId) b434f687c4b1e661808874456be45174e7c0f3b765ed250e24ca8b39c95554d0 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-8515ca2b6c465e32"></a>[`public Inno.Core.Graphs.GraphPortId.GraphPortId(string value)`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L87) | Creates a stable graph port identifier.<br>`value`：Non-empty identifier value. | — | `System.ArgumentException`：Thrown when value is empty. |
| <a id="api-5ecc94313caa73a1"></a>[`public bool Inno.Core.Graphs.GraphPortId.Equals(Inno.Core.Graphs.GraphPortId other)`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L76) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-34177bc818fe1fab"></a>[`public override bool Inno.Core.Graphs.GraphPortId.Equals(object obj)`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L76) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-39c98cf5c2e5f8ce"></a>[`public override int Inno.Core.Graphs.GraphPortId.GetHashCode()`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L76) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-6b0ad43ef315b1ee"></a>[`public static bool Inno.Core.Graphs.GraphPortId.operator ==(Inno.Core.Graphs.GraphPortId left, Inno.Core.Graphs.GraphPortId right)`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L76) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-8647a68c8d8bb688"></a>[`public static bool Inno.Core.Graphs.GraphPortId.operator !=(Inno.Core.Graphs.GraphPortId left, Inno.Core.Graphs.GraphPortId right)`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L76) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-dc26d7eec7d9e14e"></a>[`public override string Inno.Core.Graphs.GraphPortId.ToString()`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L104) | Formats this value as a human-readable representation.<br>返回：The human-readable representation of this value. | — | — |
| <a id="api-52996d7b66aa708c"></a>[`public string Inno.Core.Graphs.GraphPortId.value { get; }`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L96) | Gets the serialized identifier value. | — | — |
| <a id="api-6b4218a0727cf748"></a>[`public readonly record struct Inno.Core.Graphs.GraphPortId : System.IEquatable<Inno.Core.Graphs.GraphPortId>`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L76) | Identifies a port within its owning node definition. | — | — |

### `Inno.Core.Graphs.GraphPosition`

<!-- api: T:Inno.Core.Graphs.GraphPosition 09193384201578fd1908ca6a08a01b80a4af86d08795dca6b185c42e6355f3c1 -->
<!-- api: M:Inno.Core.Graphs.GraphPosition.#ctor(System.Single,System.Single) 2c1ad419a9fdb8e944ae1f6158115d6414df4db6e8907ec52bcecdda9dcfc22f -->
<!-- api: P:Inno.Core.Graphs.GraphPosition.x eb4a505b0b7def8381ac9006f519cb829639f91d88455df04a8224ec0c19b9ad -->
<!-- api: P:Inno.Core.Graphs.GraphPosition.y c2bbbd6aae2700a79ebc1a3688197a625fd4ce1e8939d2aa7545fa65220b06c6 -->
<!-- api: M:Inno.Core.Graphs.GraphPosition.ToString dc28bfc9024a49fbe6e68a90c48b794e7a084ff6f71b8b713c6ba077c808e315 -->
<!-- api: M:Inno.Core.Graphs.GraphPosition.op_Inequality(Inno.Core.Graphs.GraphPosition,Inno.Core.Graphs.GraphPosition) 8f9ad02f68270798d9c9bbfc04cc950181bb7033c6015b27b395bb21910d2a15 -->
<!-- api: M:Inno.Core.Graphs.GraphPosition.op_Equality(Inno.Core.Graphs.GraphPosition,Inno.Core.Graphs.GraphPosition) 8b7c1a7f1d30b53498a61bd64f3d610bb13af594bfd72599e32f8de1c9104a66 -->
<!-- api: M:Inno.Core.Graphs.GraphPosition.GetHashCode 7355ae030537bdf49bb097a9fa6bb5988d86b149be84679148a92d060b6ca62a -->
<!-- api: M:Inno.Core.Graphs.GraphPosition.Equals(System.Object) 3758f1ce01fc3a50a164630b4aa1089a75c9eadd7df1eb146d84466cffa40539 -->
<!-- api: M:Inno.Core.Graphs.GraphPosition.Equals(Inno.Core.Graphs.GraphPosition) e9996a40777ce13c754fcf82685d96ef0e2cbdc2a9514ea02d81678079e8772a -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d1a10143667809b2"></a>[`public Inno.Core.Graphs.GraphPosition.GraphPosition(float x, float y)`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L121) | Creates a graph-space position.<br>`x`：Horizontal graph-space coordinate.<br>`y`：Vertical graph-space coordinate. | — | — |
| <a id="api-7ee11082da69ec31"></a>[`public bool Inno.Core.Graphs.GraphPosition.Equals(Inno.Core.Graphs.GraphPosition other)`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L110) | Indicates whether the current object is equal to another object of the same type.<br>返回：true if the current object is equal to the other parameter; otherwise, false.<br>`other`：An object to compare with this object. | — | — |
| <a id="api-7d1b2814be826883"></a>[`public override bool Inno.Core.Graphs.GraphPosition.Equals(object obj)`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L110) | Indicates whether this instance and a specified object are equal.<br>返回：true if obj and this instance are the same type and represent the same value; otherwise, false.<br>`obj`：The object to compare with the current instance. | — | — |
| <a id="api-883c9e53b18be1b1"></a>[`public override int Inno.Core.Graphs.GraphPosition.GetHashCode()`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L110) | Returns the hash code for this instance.<br>返回：A 32-bit signed integer that is the hash code for this instance. | — | — |
| <a id="api-669626e40a873c3e"></a>[`public static bool Inno.Core.Graphs.GraphPosition.operator ==(Inno.Core.Graphs.GraphPosition left, Inno.Core.Graphs.GraphPosition right)`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L110) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-345275b71fc77d0b"></a>[`public static bool Inno.Core.Graphs.GraphPosition.operator !=(Inno.Core.Graphs.GraphPosition left, Inno.Core.Graphs.GraphPosition right)`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L110) | C# record 生成的值相等性成员；比较当前 record 状态，并保持相等值的哈希一致。 | — | — |
| <a id="api-dba7a4022aae0d4e"></a>[`public override string Inno.Core.Graphs.GraphPosition.ToString()`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L110) | Returns the fully qualified type name of this instance.<br>返回：The fully qualified type name. | — | — |
| <a id="api-85d2b41bca73b859"></a>[`public float Inno.Core.Graphs.GraphPosition.x { get; }`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L132) | Gets the horizontal graph-space coordinate. | — | — |
| <a id="api-416d3c8f13cf5e98"></a>[`public float Inno.Core.Graphs.GraphPosition.y { get; }`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L137) | Gets the vertical graph-space coordinate. | — | — |
| <a id="api-589486febed2e377"></a>[`public readonly record struct Inno.Core.Graphs.GraphPosition : System.IEquatable<Inno.Core.Graphs.GraphPosition>`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L110) | Stores a graph-space position independently from any editor UI framework. | — | — |

### `Inno.Core.Graphs.GraphSerializedValue`

<!-- api: T:Inno.Core.Graphs.GraphSerializedValue 3575c2d3e5d00c494e158a27fc76f100e67a190153833e490cebf63330a1591a -->
<!-- api: M:Inno.Core.Graphs.GraphSerializedValue.#ctor(System.ReadOnlySpan{System.Byte}) 0ba60f80c19c9428a3a6d2776fc603ea157f515260b979c2d48f6197371adfcc -->
<!-- api: P:Inno.Core.Graphs.GraphSerializedValue.data e5ba5c9238cab2e6a1733c619e2566bfb24aee485b7244a473232ddb4cda265f -->
<!-- api: M:Inno.Core.Graphs.GraphSerializedValue.From``1(``0,Inno.Core.Serialization.SerializationRegistry) 608469e809745dc02339c859de72884ad3a69fc57b9c4358691cdf100fa04993 -->
<!-- api: M:Inno.Core.Graphs.GraphSerializedValue.Deserialize``1(Inno.Core.Serialization.SerializationRegistry) 70f9ff53720ee1530369b0beb89ad1b55ef9da33220c3be207de18c3d9bad18f -->
<!-- api: M:Inno.Core.Graphs.GraphSerializedValue.Clone d3a94b180f58225409d5c6fbe64b91d25bce8aa1bfdba6dd221ff4716b682712 -->
<!-- api: M:Inno.Core.Graphs.GraphSerializedValue.ToArray 83626da216d59591e6e35be75a84e84572cc600a9e79cabe73fa9b28333a4638 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-d643f9788f71359c"></a>[`public Inno.Core.Graphs.GraphSerializedValue.GraphSerializedValue(System.ReadOnlySpan<byte> data)`](../../src/foundation/core/Inno.Core.Graphs/GraphSerializedValue.cs#L23) | Creates a graph value from one complete native serialization payload.<br>`data`：Bytes produced by the common Inno serialization pipeline. | — | `System.ArgumentException`：Thrown when the payload is empty. |
| <a id="api-88fdcb2c0c879b60"></a>[`public Inno.Core.Graphs.GraphSerializedValue Inno.Core.Graphs.GraphSerializedValue.Clone()`](../../src/foundation/core/Inno.Core.Graphs/GraphSerializedValue.cs#L84) | Creates an independent copy of the neutral value.<br>返回：A graph value that shares no mutable byte storage. | — | — |
| <a id="api-331549476a0735d1"></a>[`public T Inno.Core.Graphs.GraphSerializedValue.Deserialize<T>(Inno.Core.Serialization.SerializationRegistry serialization)`](../../src/foundation/core/Inno.Core.Graphs/GraphSerializedValue.cs#L72) | Deserializes this value through the common Inno serializer.<br>返回：The restored neutral value.<br>`serialization`：The serialization registry that owns the converter generation for this operation. | — | — |
| <a id="api-cd8f0e3def4e8b15"></a>[`public static Inno.Core.Graphs.GraphSerializedValue Inno.Core.Graphs.GraphSerializedValue.From<T>(T value, Inno.Core.Serialization.SerializationRegistry serialization)`](../../src/foundation/core/Inno.Core.Graphs/GraphSerializedValue.cs#L51) | Serializes a neutral graph property through the common Inno serializer.<br>返回：A detached graph value containing native serialization bytes.<br>`value`：Value to serialize.<br>`serialization`：The serialization registry that owns the converter generation for this operation. | — | — |
| <a id="api-4f90098ed8eb0436"></a>[`public byte[] Inno.Core.Graphs.GraphSerializedValue.ToArray()`](../../src/foundation/core/Inno.Core.Graphs/GraphSerializedValue.cs#L92) | Copies the native payload for persistence or reload-safe history.<br>返回：A new byte array containing the complete native value. | — | — |
| <a id="api-3ee07e9c1fce66ba"></a>[`public System.ReadOnlyMemory<byte> Inno.Core.Graphs.GraphSerializedValue.data { get; }`](../../src/foundation/core/Inno.Core.Graphs/GraphSerializedValue.cs#L33) | Gets an immutable view of the native serialized value. | — | — |
| <a id="api-7997dcadb9640e98"></a>[`public sealed class Inno.Core.Graphs.GraphSerializedValue`](../../src/foundation/core/Inno.Core.Graphs/GraphSerializedValue.cs#L10) | Stores one graph property as backend-neutral Inno serialization bytes. | — | — |

### `Inno.Core.Graphs.GraphValidationResult`

<!-- api: T:Inno.Core.Graphs.GraphValidationResult 400afcb2e0977d2015fab4ddb320c6f23fadd0b32d75531c8747e75a4f9c065b -->
<!-- api: M:Inno.Core.Graphs.GraphValidationResult.#ctor(System.Collections.Generic.IReadOnlyList{Inno.Core.Graphs.GraphDiagnostic}) 9ca0a5c93e696de36d68cb7d4b8382b56d7e244820513acd2decf99226df24dd -->
<!-- api: P:Inno.Core.Graphs.GraphValidationResult.diagnostics 050dce8efc70901a6c8c35ee6aaf3368d3c0f8d7e7617e800d40da78d3d99ef0 -->
<!-- api: P:Inno.Core.Graphs.GraphValidationResult.isValid f3adef6688586147a0ea704b6c3b8ca7a80c4ee5974034b56faeb16526d75b75 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-6a263e9499d02d46"></a>[`public Inno.Core.Graphs.GraphValidationResult.GraphValidationResult(System.Collections.Generic.IReadOnlyList<Inno.Core.Graphs.GraphDiagnostic> diagnostics)`](../../src/foundation/core/Inno.Core.Graphs/GraphValidation.cs#L88) | Creates a graph validation result.<br>`diagnostics`：Diagnostics in deterministic validation order. | — | — |
| <a id="api-2a3ebeb9b2219ecb"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Core.Graphs.GraphDiagnostic> Inno.Core.Graphs.GraphValidationResult.diagnostics { get; }`](../../src/foundation/core/Inno.Core.Graphs/GraphValidation.cs#L97) | Gets diagnostics in deterministic validation order. | — | — |
| <a id="api-ffd440bfa012cae0"></a>[`public bool Inno.Core.Graphs.GraphValidationResult.isValid { get; }`](../../src/foundation/core/Inno.Core.Graphs/GraphValidation.cs#L102) | Gets whether no error diagnostic is present. | — | — |
| <a id="api-b1c02018fc6a015a"></a>[`public sealed class Inno.Core.Graphs.GraphValidationResult`](../../src/foundation/core/Inno.Core.Graphs/GraphValidation.cs#L78) | Contains deterministic graph validation diagnostics. | — | — |

### `Inno.Core.Graphs.GraphValidator`

<!-- api: T:Inno.Core.Graphs.GraphValidator e4a425c99aa82156084051f762f70b89a1fe125b8645dde74418c1dc705a2da5 -->
<!-- api: M:Inno.Core.Graphs.GraphValidator.Validate(Inno.Core.Graphs.GraphDocument,Inno.Core.Graphs.IGraphNodeDefinitionResolver,Inno.Core.Graphs.IGraphTypeConversion,System.Boolean) 97199353f1e63a63f82e7fc3aa451a4a005450e8b5395b93054a91914f7fcd08 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-69a2b245b8fffaae"></a>[`public static Inno.Core.Graphs.GraphValidationResult Inno.Core.Graphs.GraphValidator.Validate(Inno.Core.Graphs.GraphDocument document, Inno.Core.Graphs.IGraphNodeDefinitionResolver resolver, Inno.Core.Graphs.IGraphTypeConversion? conversion = null, bool allowCycles = false)`](../../src/foundation/core/Inno.Core.Graphs/GraphValidation.cs#L142) | Validates node availability, ports, edge types, capacity, required inputs and cycles.<br>返回：A deterministic validation result. Missing definitions remain warnings so documents stay editable.<br>`document`：Graph document to validate without modifying it.<br>`resolver`：Active node definition resolver.<br>`conversion`：Optional directed type conversion policy.<br>`allowCycles`：Whether directed cycles are permitted. | — | — |
| <a id="api-81c4efa1b2653e70"></a>[`public static class Inno.Core.Graphs.GraphValidator`](../../src/foundation/core/Inno.Core.Graphs/GraphValidation.cs#L122) | Validates neutral graph topology against generation-scoped node definitions. | — | — |

### `Inno.Core.Graphs.IGraphNodeDefinitionResolver`

<!-- api: T:Inno.Core.Graphs.IGraphNodeDefinitionResolver 1a492cc81eb2b6c1cf6009280216b04329af329e647e9032c074d3ce35e9eb57 -->
<!-- api: M:Inno.Core.Graphs.IGraphNodeDefinitionResolver.TryResolve(System.String,Inno.Core.Graphs.GraphNodeDefinition@) cd6414c2877f5bfa0c9fb10fd9a90177cc32ccf102299794c32979bcc8a157b4 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-5eeed7ae9c3c4d67"></a>[`bool Inno.Core.Graphs.IGraphNodeDefinitionResolver.TryResolve(string definitionId, out Inno.Core.Graphs.GraphNodeDefinition? definition)`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L185) | Tries to resolve an active node definition.<br>返回：true when active; otherwise false.<br>`definitionId`：Stable definition identifier.<br>`definition`：Receives the generation-scoped definition. | — | — |
| <a id="api-97d8fe14c1fd515b"></a>[`public interface Inno.Core.Graphs.IGraphNodeDefinitionResolver`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L171) | Resolves generation-scoped node definitions by stable identifier. | — | — |

### `Inno.Core.Graphs.IGraphTypeConversion`

<!-- api: T:Inno.Core.Graphs.IGraphTypeConversion 3386d7be80663ba8f8fb63a8698d3c933dc6b0d6f6f19cd48fbf28db24b5ee7f -->
<!-- api: M:Inno.Core.Graphs.IGraphTypeConversion.CanConvert(System.String,System.String) a314004aa55fe6dbe2cc865e5831adf2cda6df8c4b510d479c8fa37bd08f65f0 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-85eb4cf471500ab9"></a>[`bool Inno.Core.Graphs.IGraphTypeConversion.CanConvert(string sourceTypeId, string destinationTypeId)`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L208) | Tests a directed value conversion.<br>返回：true when the conversion is valid; otherwise false.<br>`sourceTypeId`：Source value type identifier.<br>`destinationTypeId`：Destination value type identifier. | — | — |
| <a id="api-0ac60beeac3d9bfa"></a>[`public interface Inno.Core.Graphs.IGraphTypeConversion`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L194) | Determines whether one graph value type can be converted to another. | — | — |

## 项目依赖

- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Serialization](Inno.Core.Serialization.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Collections](Inno.Core.Collections.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Diagnostics](Inno.Core.Diagnostics.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
