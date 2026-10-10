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
GraphDocument graph = new();
GraphNodeRecord node = new(new GraphNodeId("constant-1"), "math.float");
node.position = new GraphPosition(120f, 80f);
node.SetValue("value", GraphSerializedValue.From(0.5f, serialization));
graph.AddNode(node);

GraphValidationResult validation = GraphValidator.Validate(graph, activeDefinitions);
if (!validation.isValid)
{
    // Map stable diagnostics back to the graph canvas.
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

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Core.Graphs.GraphDiagnostic`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Diagnostics.DiagnosticSeverity Inno.Core.Graphs.GraphDiagnostic.severity`](../../src/foundation/core/Inno.Core.Graphs/GraphValidation.cs#L62) | Gets the diagnostic impact. |
| [`Inno.Core.Graphs.GraphDiagnostic`](../../src/foundation/core/Inno.Core.Graphs/GraphValidation.cs#L13) | Reports one graph validation problem using stable document identifiers. |
| [`Inno.Core.Graphs.GraphDiagnostic.GraphDiagnostic(string code, string message, Inno.Core.Diagnostics.DiagnosticSeverity severity, Inno.Core.Graphs.GraphNodeId? nodeId = null, Inno.Core.Graphs.GraphEdgeId? edgeId = null)`](../../src/foundation/core/Inno.Core.Graphs/GraphValidation.cs#L33) | Creates a graph diagnostic. |
| [`Inno.Core.Graphs.GraphEdgeId? Inno.Core.Graphs.GraphDiagnostic.edgeId`](../../src/foundation/core/Inno.Core.Graphs/GraphValidation.cs#L72) | Gets the related edge identifier, if any. |
| [`Inno.Core.Graphs.GraphNodeId? Inno.Core.Graphs.GraphDiagnostic.nodeId`](../../src/foundation/core/Inno.Core.Graphs/GraphValidation.cs#L67) | Gets the related node identifier, if any. |
| [`string Inno.Core.Graphs.GraphDiagnostic.code`](../../src/foundation/core/Inno.Core.Graphs/GraphValidation.cs#L52) | Gets the stable machine-readable diagnostic code. |
| [`string Inno.Core.Graphs.GraphDiagnostic.message`](../../src/foundation/core/Inno.Core.Graphs/GraphValidation.cs#L57) | Gets the artist-facing diagnostic text. |

### `Inno.Core.Graphs.GraphDocument`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Graphs.GraphDocument`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L187) | Owns neutral graph records and graph-level canvas metadata. |
| [`Inno.Core.Graphs.GraphDocument Inno.Core.Graphs.GraphDocument.Clone()`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L228) | Creates a deep neutral copy that shares no mutable node, edge, value, or metadata records. |
| [`Inno.Core.Graphs.GraphDocument.GraphDocument()`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L200) | Creates an empty document with stable, read-only collection views. |
| [`Inno.Core.Graphs.GraphNodeRecord? Inno.Core.Graphs.GraphDocument.FindNode(Inno.Core.Graphs.GraphNodeId nodeId)`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L345) | Finds a node by stable identifier. |
| [`System.Collections.Generic.IReadOnlyDictionary<string, Inno.Core.Graphs.GraphSerializedValue> Inno.Core.Graphs.GraphDocument.metadata`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L220) | Gets graph-level neutral metadata such as groups or comments. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Core.Graphs.GraphEdgeRecord> Inno.Core.Graphs.GraphDocument.edges`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L215) | Gets all edge records in stable document order. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Core.Graphs.GraphNodeRecord> Inno.Core.Graphs.GraphDocument.nodes`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L210) | Gets all node records in stable document order. |
| [`bool Inno.Core.Graphs.GraphDocument.RemoveEdge(Inno.Core.Graphs.GraphEdgeId edgeId)`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L376) | Removes an edge by stable identifier. |
| [`bool Inno.Core.Graphs.GraphDocument.RemoveMetadata(string key)`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L415) | Removes graph-level metadata by stable key. |
| [`bool Inno.Core.Graphs.GraphDocument.RemoveNode(Inno.Core.Graphs.GraphNodeId nodeId)`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L324) | Removes a node and every edge connected to it. |
| [`void Inno.Core.Graphs.GraphDocument.AddEdge(Inno.Core.Graphs.GraphEdgeRecord edge)`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L356) | Adds an edge while preserving document order. |
| [`void Inno.Core.Graphs.GraphDocument.AddNode(Inno.Core.Graphs.GraphNodeRecord node)`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L304) | Adds a node while preserving document order. |
| [`void Inno.Core.Graphs.GraphDocument.ReplaceContents(Inno.Core.Graphs.GraphDocument source)`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L244) | Atomically replaces all records with deep copies from another neutral document. |
| [`void Inno.Core.Graphs.GraphDocument.SetMetadata(string key, Inno.Core.Graphs.GraphSerializedValue value)`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L397) | Creates or replaces graph-level neutral metadata. |

### `Inno.Core.Graphs.GraphDocumentCodec`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Graphs.GraphDocumentCodec`](../../src/foundation/core/Inno.Core.Graphs/GraphDocumentCodec.cs#L10) | Persists neutral graph documents through the common Inno serialization pipeline. |
| [`static Inno.Core.Graphs.GraphDocument Inno.Core.Graphs.GraphDocumentCodec.Decode(System.ReadOnlySpan<byte> bytes, Inno.Core.Serialization.SerializationRegistry serialization)`](../../src/foundation/core/Inno.Core.Graphs/GraphDocumentCodec.cs#L90) | Decodes one current native graph document. |
| [`static byte[] Inno.Core.Graphs.GraphDocumentCodec.Encode(Inno.Core.Graphs.GraphDocument document, Inno.Core.Serialization.SerializationRegistry serialization)`](../../src/foundation/core/Inno.Core.Graphs/GraphDocumentCodec.cs#L24) | Encodes a graph document into deterministic native bytes. |

### `Inno.Core.Graphs.GraphEdgeId`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Graphs.GraphEdgeId`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L42) | Identifies an edge within one graph document using a stable serialized value. |
| [`Inno.Core.Graphs.GraphEdgeId.GraphEdgeId(string value)`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L53) | Creates a stable graph edge identifier. |
| [`override string Inno.Core.Graphs.GraphEdgeId.ToString()`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L70) | Formats this value as a human-readable representation. |
| [`string Inno.Core.Graphs.GraphEdgeId.value`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L62) | Gets the serialized identifier value. |

### `Inno.Core.Graphs.GraphEdgeRecord`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Graphs.GraphEdgeId Inno.Core.Graphs.GraphEdgeRecord.id`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L171) | Gets the stable edge identifier. |
| [`Inno.Core.Graphs.GraphEdgeRecord`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L144) | Stores one typed connection between two graph endpoints. |
| [`Inno.Core.Graphs.GraphEdgeRecord.GraphEdgeRecord(Inno.Core.Graphs.GraphEdgeId id, Inno.Core.Graphs.GraphEndpoint output, Inno.Core.Graphs.GraphEndpoint input)`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L158) | Creates a graph edge record. |
| [`Inno.Core.Graphs.GraphEndpoint Inno.Core.Graphs.GraphEdgeRecord.input`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L181) | Gets the destination endpoint. |
| [`Inno.Core.Graphs.GraphEndpoint Inno.Core.Graphs.GraphEdgeRecord.output`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L176) | Gets the source endpoint. |

### `Inno.Core.Graphs.GraphEndpoint`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Graphs.GraphEndpoint`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L111) | Identifies one endpoint of a graph edge. |
| [`Inno.Core.Graphs.GraphEndpoint.GraphEndpoint(Inno.Core.Graphs.GraphNodeId nodeId, Inno.Core.Graphs.GraphPortId portId)`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L122) | Creates a graph edge endpoint. |
| [`Inno.Core.Graphs.GraphNodeId Inno.Core.Graphs.GraphEndpoint.nodeId`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L133) | Gets the owning node identifier. |
| [`Inno.Core.Graphs.GraphPortId Inno.Core.Graphs.GraphEndpoint.portId`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L138) | Gets the node-local port identifier. |

### `Inno.Core.Graphs.GraphNodeDefinition`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Graphs.GraphNodeDefinition`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L114) | Describes node presentation and resolves ports without entering graph persistence. |
| [`Inno.Core.Graphs.GraphNodeDefinition.GraphNodeDefinition(string id, string displayName, string category)`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L128) | Creates a graph node definition. |
| [`abstract System.Collections.Generic.IReadOnlyList<Inno.Core.Graphs.GraphPortDefinition> Inno.Core.Graphs.GraphNodeDefinition.GetPorts(Inno.Core.Graphs.GraphNodeRecord node)`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L165) | Resolves ports for one node record, including any data-driven dynamic ports. |
| [`string Inno.Core.Graphs.GraphNodeDefinition.category`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L154) | Gets the search-menu category path. |
| [`string Inno.Core.Graphs.GraphNodeDefinition.displayName`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L149) | Gets the artist-facing display name. |
| [`string Inno.Core.Graphs.GraphNodeDefinition.id`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L144) | Gets the globally stable definition identifier. |

### `Inno.Core.Graphs.GraphNodeId`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Graphs.GraphNodeId`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L8) | Identifies a node within one graph document using a stable serialized value. |
| [`Inno.Core.Graphs.GraphNodeId.GraphNodeId(string value)`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L19) | Creates a stable graph node identifier. |
| [`override string Inno.Core.Graphs.GraphNodeId.ToString()`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L36) | Formats this value as a human-readable representation. |
| [`string Inno.Core.Graphs.GraphNodeId.value`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L28) | Gets the serialized identifier value. |

### `Inno.Core.Graphs.GraphNodeRecord`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Graphs.GraphNodeId Inno.Core.Graphs.GraphNodeRecord.id`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L37) | Gets the stable node identifier. |
| [`Inno.Core.Graphs.GraphNodeRecord`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L10) | Stores one node without retaining a CLR type or runtime extension instance. |
| [`Inno.Core.Graphs.GraphNodeRecord.GraphNodeRecord(Inno.Core.Graphs.GraphNodeId id, string definitionId)`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L24) | Creates a graph node record. |
| [`Inno.Core.Graphs.GraphPosition Inno.Core.Graphs.GraphNodeRecord.position`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L47) | Gets or sets the graph-space node position. |
| [`System.Collections.Generic.IReadOnlyDictionary<string, Inno.Core.Graphs.GraphSerializedValue> Inno.Core.Graphs.GraphNodeRecord.values`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L52) | Gets neutral serialized property values keyed by stable property identifier. |
| [`bool Inno.Core.Graphs.GraphNodeRecord.RemoveValue(string propertyId)`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L101) | Removes a neutral serialized property value. |
| [`bool Inno.Core.Graphs.GraphNodeRecord.TryGetValue(string propertyId, out Inno.Core.Graphs.GraphSerializedValue? value)`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L84) | Tries to read a neutral serialized property value. |
| [`string Inno.Core.Graphs.GraphNodeRecord.definitionId`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L42) | Gets the stable node definition identifier. |
| [`void Inno.Core.Graphs.GraphNodeRecord.SetValue(string propertyId, Inno.Core.Graphs.GraphSerializedValue value)`](../../src/foundation/core/Inno.Core.Graphs/GraphDocument.cs#L63) | Creates or replaces a neutral serialized property value. |

### `Inno.Core.Graphs.GraphPortCapacity`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Graphs.GraphPortCapacity`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L24) | Declares how many edges may connect to a graph port. |
| [`Inno.Core.Graphs.GraphPortCapacity.Multiple`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L33) | Any number of edges may connect to the port. |
| [`Inno.Core.Graphs.GraphPortCapacity.Single`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L29) | At most one edge may connect to the port. |

### `Inno.Core.Graphs.GraphPortDefinition`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Graphs.GraphPortCapacity Inno.Core.Graphs.GraphPortDefinition.capacity`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L103) | Gets the allowed edge capacity. |
| [`Inno.Core.Graphs.GraphPortDefinition`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L39) | Describes one dynamically or statically resolved node port. |
| [`Inno.Core.Graphs.GraphPortDefinition.GraphPortDefinition(Inno.Core.Graphs.GraphPortId id, string displayName, string valueTypeId, Inno.Core.Graphs.GraphPortDirection direction, Inno.Core.Graphs.GraphPortCapacity capacity = Inno.Core.Graphs.GraphPortCapacity.Single, bool required = false)`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L62) | Creates a graph port definition. |
| [`Inno.Core.Graphs.GraphPortDirection Inno.Core.Graphs.GraphPortDefinition.direction`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L98) | Gets the value-flow direction. |
| [`Inno.Core.Graphs.GraphPortId Inno.Core.Graphs.GraphPortDefinition.id`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L83) | Gets the stable node-local port identifier. |
| [`bool Inno.Core.Graphs.GraphPortDefinition.required`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L108) | Gets whether an input must be connected. |
| [`string Inno.Core.Graphs.GraphPortDefinition.displayName`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L88) | Gets the artist-facing display name. |
| [`string Inno.Core.Graphs.GraphPortDefinition.valueTypeId`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L93) | Gets the stable value type identifier. |

### `Inno.Core.Graphs.GraphPortDirection`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Graphs.GraphPortDirection`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L9) | Declares the direction in which values flow through a graph port. |
| [`Inno.Core.Graphs.GraphPortDirection.Input`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L14) | Receives a value from another node. |
| [`Inno.Core.Graphs.GraphPortDirection.Output`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L18) | Produces a value for another node. |

### `Inno.Core.Graphs.GraphPortId`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Graphs.GraphPortId`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L76) | Identifies a port within its owning node definition. |
| [`Inno.Core.Graphs.GraphPortId.GraphPortId(string value)`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L87) | Creates a stable graph port identifier. |
| [`override string Inno.Core.Graphs.GraphPortId.ToString()`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L104) | Formats this value as a human-readable representation. |
| [`string Inno.Core.Graphs.GraphPortId.value`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L96) | Gets the serialized identifier value. |

### `Inno.Core.Graphs.GraphPosition`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Graphs.GraphPosition`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L110) | Stores a graph-space position independently from any editor UI framework. |
| [`Inno.Core.Graphs.GraphPosition.GraphPosition(float x, float y)`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L121) | Creates a graph-space position. |
| [`float Inno.Core.Graphs.GraphPosition.x`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L132) | Gets the horizontal graph-space coordinate. |
| [`float Inno.Core.Graphs.GraphPosition.y`](../../src/foundation/core/Inno.Core.Graphs/GraphIds.cs#L137) | Gets the vertical graph-space coordinate. |

### `Inno.Core.Graphs.GraphSerializedValue`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Graphs.GraphSerializedValue`](../../src/foundation/core/Inno.Core.Graphs/GraphSerializedValue.cs#L10) | Stores one graph property as backend-neutral Inno serialization bytes. |
| [`Inno.Core.Graphs.GraphSerializedValue Inno.Core.Graphs.GraphSerializedValue.Clone()`](../../src/foundation/core/Inno.Core.Graphs/GraphSerializedValue.cs#L84) | Creates an independent copy of the neutral value. |
| [`Inno.Core.Graphs.GraphSerializedValue.GraphSerializedValue(System.ReadOnlySpan<byte> data)`](../../src/foundation/core/Inno.Core.Graphs/GraphSerializedValue.cs#L23) | Creates a graph value from one complete native serialization payload. |
| [`System.ReadOnlyMemory<byte> Inno.Core.Graphs.GraphSerializedValue.data`](../../src/foundation/core/Inno.Core.Graphs/GraphSerializedValue.cs#L33) | Gets an immutable view of the native serialized value. |
| [`T Inno.Core.Graphs.GraphSerializedValue.Deserialize<T>(Inno.Core.Serialization.SerializationRegistry serialization)`](../../src/foundation/core/Inno.Core.Graphs/GraphSerializedValue.cs#L71) | Deserializes this value through the common Inno serializer. |
| [`byte[] Inno.Core.Graphs.GraphSerializedValue.ToArray()`](../../src/foundation/core/Inno.Core.Graphs/GraphSerializedValue.cs#L92) | Copies the native payload for persistence or reload-safe history. |
| [`static Inno.Core.Graphs.GraphSerializedValue Inno.Core.Graphs.GraphSerializedValue.From<T>(T value, Inno.Core.Serialization.SerializationRegistry serialization)`](../../src/foundation/core/Inno.Core.Graphs/GraphSerializedValue.cs#L50) | Serializes a neutral graph property through the common Inno serializer. |

### `Inno.Core.Graphs.GraphValidationResult`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Graphs.GraphValidationResult`](../../src/foundation/core/Inno.Core.Graphs/GraphValidation.cs#L78) | Contains deterministic graph validation diagnostics. |
| [`Inno.Core.Graphs.GraphValidationResult.GraphValidationResult(System.Collections.Generic.IReadOnlyList<Inno.Core.Graphs.GraphDiagnostic> diagnostics)`](../../src/foundation/core/Inno.Core.Graphs/GraphValidation.cs#L88) | Creates a graph validation result. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Core.Graphs.GraphDiagnostic> Inno.Core.Graphs.GraphValidationResult.diagnostics`](../../src/foundation/core/Inno.Core.Graphs/GraphValidation.cs#L97) | Gets diagnostics in deterministic validation order. |
| [`bool Inno.Core.Graphs.GraphValidationResult.isValid`](../../src/foundation/core/Inno.Core.Graphs/GraphValidation.cs#L102) | Gets whether no error diagnostic is present. |

### `Inno.Core.Graphs.GraphValidator`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Graphs.GraphValidator`](../../src/foundation/core/Inno.Core.Graphs/GraphValidation.cs#L122) | Validates neutral graph topology against generation-scoped node definitions. |
| [`static Inno.Core.Graphs.GraphValidationResult Inno.Core.Graphs.GraphValidator.Validate(Inno.Core.Graphs.GraphDocument document, Inno.Core.Graphs.IGraphNodeDefinitionResolver resolver, Inno.Core.Graphs.IGraphTypeConversion? conversion = null, bool allowCycles = false)`](../../src/foundation/core/Inno.Core.Graphs/GraphValidation.cs#L142) | Validates node availability, ports, edge types, capacity, required inputs and cycles. |

### `Inno.Core.Graphs.IGraphNodeDefinitionResolver`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Graphs.IGraphNodeDefinitionResolver`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L171) | Resolves generation-scoped node definitions by stable identifier. |
| [`bool Inno.Core.Graphs.IGraphNodeDefinitionResolver.TryResolve(string definitionId, out Inno.Core.Graphs.GraphNodeDefinition? definition)`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L185) | Tries to resolve an active node definition. |

### `Inno.Core.Graphs.IGraphTypeConversion`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Graphs.IGraphTypeConversion`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L194) | Determines whether one graph value type can be converted to another. |
| [`bool Inno.Core.Graphs.IGraphTypeConversion.CanConvert(string sourceTypeId, string destinationTypeId)`](../../src/foundation/core/Inno.Core.Graphs/GraphDefinitions.cs#L208) | Tests a directed value conversion. |

## 项目依赖

- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Serialization](Inno.Core.Serialization.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Collections](Inno.Core.Collections.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Diagnostics](Inno.Core.Diagnostics.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
