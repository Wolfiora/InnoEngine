# Inno.Editor.Graph

[返回 Editor 索引](README.md) · [Wiki 首页](../README.md) · [通用 Graph](../core/Inno.Core.Graphs.md) · [Shader Editor](Inno.Editor.Panel.ShaderEditor.md)

`Inno.Editor.Graph` 提供不含 Shader 或 ImGui 语义的编辑控制层。`GraphEditorModule` 管理按稳定 document ID 索引的 session；`GraphDocumentController` 完成节点增删移动、连接重连、值修改、复制粘贴与 dirty/revision；`GraphCanvasState` 保存 session 内 pan/zoom、选择和 pending connection。

`GraphDocumentHistory`、`GraphHistoryData` 与 `GraphHistoryTransition` 把 before/after 文档编码为中立 bytes，经 `EditorInteractions.history` 执行。一次拖动在释放时调用 MoveNodes；不同拖动不合并。SetNodeValue / ReplaceDocument 可显式传入本次值编辑的唯一 gesture ID。结构修改不合并。History payload 不保存 CLR `Type`、节点实例、GPU 对象或 delegate。

`GraphCanvasState.selectedNodes/selectedEdges` 返回冻结集合快照，不能强转 HashSet 绕过选择 API；它们仍是当前 Session 瞬时状态，不持久化到 editor.ini。

公开 API 由 `GraphEditorModule`、`GraphDocumentController`、`GraphClipboardData`、`GraphCanvasState` 组成。History codec/transition 是 internal 实现，不是扩展契约。

## 文档身份与恢复

| API | 当前契约 |
| --- | --- |
| `OpenDocument(Guid, GraphDocument, IEditorHistory)` | 新建或加入已有 session；Asset-backed 文档使用 Asset persistent ID，重接 UI 不覆盖 dirty 内容；最多同时拥有 128 个文档 |
| `TryOpenDocument(Guid, IEditorHistory, out controller)` | 只加入已有状态，不创建空白替代品 |
| `RebindDocument(Guid, GraphDocument)` | 仅未修改状态接受新导入内容，保留 dirty 文档 |
| `SetAvailability(Guid, bool)` | 源 Missing 时保留图、位置、属性 bytes 和 History；恢复后原 Undo/Redo 自动可用 |
| `CloseDocument(Guid)` | 显式注销 identity；已发出的 controller 永久失效，即使相同 persistent ID 重开 |
| `GraphDocumentController` | documentId/isAvailable/document/revision/isDirty、MarkSaved、AddNode/RemoveNodes/MoveNodes、Connect/Disconnect、SetNodeValue/RemoveNodeValue、Copy/Paste |

Module 的 live session 只由自己的 IdentityAllocator 解析，Guid 集合是次级目录，controller 保存弱 Identity 而非跨代 session 对象。Identity 索引本身不承担强引用保活；Module 的独立资源集合拥有 session，直到 CloseDocument 或 Module 退出时释放，不能把弱索引误当作 lifetime owner。OnStart/OnStop 自动注册/注销 IEditorReloadParticipant，Capture 使用 ReferenceRecoveryTransaction 五阶段。Prepare 捕获中立 Graph bytes；Apply 完整解码后发布；Validate 检查节点、边、顺序与属性不丢失；失败精确恢复 revision、dirty 和可用性。Graph 的 Stable Node ID 是语义，不强塞入 object reference slot。

GraphDocument 是可编辑创作模型，不是假称不可变的运行快照。回调/编译产物需要独立副本；GraphCanvasState 仅保存 session 瞬时展示状态。

`Paste(data, offset, remapNode)` 的可选同步回调在同一次原子修改中重映射领域内部 ID；回调失败回滚整个粘贴，回调不进入 History。Shader Editor 用它重映射复制的 stage owner，而不是按端口位置猜测引用。

相邻页面：[Inno.Core.Graphs](../core/Inno.Core.Graphs.md) · [Shader Editor](Inno.Editor.Panel.ShaderEditor.md) · [Editor Interactions](Inno.Editor.Interactions.md)

## 当前源码公开 API 清单

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Editor.Graph.GraphCanvasState`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Graphs.GraphEndpoint? Inno.Editor.Graph.GraphCanvasState.pendingConnection`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphCanvasState.cs#L42) | Gets the endpoint currently being connected, or . |
| [`Inno.Core.Graphs.GraphPortDirection? Inno.Editor.Graph.GraphCanvasState.pendingConnectionDirection`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphCanvasState.cs#L47) | Gets the direction of . |
| [`Inno.Core.Graphs.GraphPosition Inno.Editor.Graph.GraphCanvasState.pan`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphCanvasState.cs#L22) | Gets the graph-space origin mapped to canvas screen origin. |
| [`Inno.Editor.Graph.GraphCanvasState`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphCanvasState.cs#L12) | Stores transient pan, zoom, selection and connection interaction state without ImGui dependencies. |
| [`System.Collections.Generic.IReadOnlyCollection<Inno.Core.Graphs.GraphEdgeId> Inno.Editor.Graph.GraphCanvasState.selectedEdges`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphCanvasState.cs#L37) | Gets selected edge identities. |
| [`System.Collections.Generic.IReadOnlyCollection<Inno.Core.Graphs.GraphNodeId> Inno.Editor.Graph.GraphCanvasState.selectedNodes`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphCanvasState.cs#L32) | Gets selected node identities. |
| [`float Inno.Editor.Graph.GraphCanvasState.zoom`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphCanvasState.cs#L27) | Gets the current graph-to-screen scale. |
| [`void Inno.Editor.Graph.GraphCanvasState.BeginConnection(Inno.Core.Graphs.GraphEndpoint endpoint, Inno.Core.Graphs.GraphPortDirection direction)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphCanvasState.cs#L164) | Begins a connection drag from one endpoint. |
| [`void Inno.Editor.Graph.GraphCanvasState.BeginConnection(Inno.Core.Graphs.GraphEndpoint output)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphCanvasState.cs#L178) | Begins a connection drag from one output endpoint. |
| [`void Inno.Editor.Graph.GraphCanvasState.CancelConnection()`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphCanvasState.cs#L183) | Cancels any active connection drag. |
| [`void Inno.Editor.Graph.GraphCanvasState.ClearSelection()`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphCanvasState.cs#L147) | Clears all transient node and edge selection. |
| [`void Inno.Editor.Graph.GraphCanvasState.PanBy(float deltaX, float deltaY)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphCanvasState.cs#L82) | Moves the canvas origin by screen-space pixels. |
| [`void Inno.Editor.Graph.GraphCanvasState.SelectNodes(System.Collections.Generic.IEnumerable<Inno.Core.Graphs.GraphNodeId> nodes)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphCanvasState.cs#L121) | Replaces node selection. |
| [`void Inno.Editor.Graph.GraphCanvasState.SetViewport(Inno.Core.Graphs.GraphPosition pan, float zoom)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphCanvasState.cs#L58) | Restores persistent canvas navigation state with bounded zoom. |
| [`void Inno.Editor.Graph.GraphCanvasState.ToggleNode(Inno.Core.Graphs.GraphNodeId nodeId)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphCanvasState.cs#L135) | Toggles one node while preserving the remaining selection. |
| [`void Inno.Editor.Graph.GraphCanvasState.ZoomAt(float factor, float pivotX, float pivotY)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphCanvasState.cs#L99) | Changes zoom while preserving the graph point beneath a screen-space pivot. |

### `Inno.Editor.Graph.GraphClipboardData`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Graph.GraphClipboardData`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphDocumentController.cs#L14) | Contains a neutral copied node fragment suitable for graph clipboard operations. |
| [`int Inno.Editor.Graph.GraphClipboardData.edgeCount`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphDocumentController.cs#L28) | Gets the copied connection count whose endpoints both belong to the fragment. |
| [`int Inno.Editor.Graph.GraphClipboardData.nodeCount`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphDocumentController.cs#L23) | Gets the copied node count. |

### `Inno.Editor.Graph.GraphDocumentController`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Graphs.GraphDocument Inno.Editor.Graph.GraphDocumentController.document`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphDocumentController.cs#L67) | Gets the mutable neutral document used by the current asset generation. |
| [`Inno.Core.Graphs.GraphEdgeId Inno.Editor.Graph.GraphDocumentController.Connect(Inno.Core.Graphs.GraphEndpoint output, Inno.Core.Graphs.GraphEndpoint input)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphDocumentController.cs#L217) | Creates or reconnects one input endpoint to an output endpoint. |
| [`Inno.Core.Graphs.GraphNodeId Inno.Editor.Graph.GraphDocumentController.AddNode(string definitionId, Inno.Core.Graphs.GraphPosition position, System.Collections.Generic.IReadOnlyDictionary<string, Inno.Core.Graphs.GraphSerializedValue>? values = null)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphDocumentController.cs#L122) | Adds a node with a generated stable identity. |
| [`Inno.Editor.Graph.GraphClipboardData Inno.Editor.Graph.GraphDocumentController.Copy(System.Collections.Generic.IEnumerable<Inno.Core.Graphs.GraphNodeId> nodeIds)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphDocumentController.cs#L331) | Copies selected nodes and only their internal connections. |
| [`Inno.Editor.Graph.GraphDocumentController`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphDocumentController.cs#L36) | Applies neutral graph edits atomically and records every data mutation in reload-safe Editor History. |
| [`System.Collections.Generic.IReadOnlyList<Inno.Core.Graphs.GraphNodeId> Inno.Editor.Graph.GraphDocumentController.Paste(Inno.Editor.Graph.GraphClipboardData clipboard, Inno.Core.Graphs.GraphPosition offset, System.Action<Inno.Core.Graphs.GraphNodeRecord, System.Collections.Generic.IReadOnlyDictionary<Inno.Core.Graphs.GraphNodeId, Inno.Core.Graphs.GraphNodeId>>? remapNode = null)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphDocumentController.cs#L365) | Pastes a clipboard fragment with new stable identities and a graph-space offset. |
| [`System.Guid Inno.Editor.Graph.GraphDocumentController.documentId`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphDocumentController.cs#L57) | Gets the stable project-relative document identity. |
| [`bool Inno.Editor.Graph.GraphDocumentController.Disconnect(Inno.Core.Graphs.GraphEdgeId edgeId)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphDocumentController.cs#L254) | Removes one stable connection. |
| [`bool Inno.Editor.Graph.GraphDocumentController.RemoveNodeValue(Inno.Core.Graphs.GraphNodeId nodeId, string propertyId)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphDocumentController.cs#L307) | Removes one neutral node property as a structural edit. |
| [`bool Inno.Editor.Graph.GraphDocumentController.isAvailable`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphDocumentController.cs#L62) | Gets whether the current document identity and source are both available. |
| [`bool Inno.Editor.Graph.GraphDocumentController.isDirty`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphDocumentController.cs#L77) | Gets whether content changed after its last explicit saved marker. |
| [`ulong Inno.Editor.Graph.GraphDocumentController.revision`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphDocumentController.cs#L72) | Gets the monotonic in-session content revision. |
| [`void Inno.Editor.Graph.GraphDocumentController.MarkSaved()`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphDocumentController.cs#L82) | Marks the current revision as saved without creating a data History entry. |
| [`void Inno.Editor.Graph.GraphDocumentController.MoveNodes(System.Collections.Generic.IReadOnlyDictionary<Inno.Core.Graphs.GraphNodeId, Inno.Core.Graphs.GraphPosition> positions)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphDocumentController.cs#L180) | Moves one or more nodes as one completed gesture, independently undoable from earlier drags. |
| [`void Inno.Editor.Graph.GraphDocumentController.RemoveNodes(System.Collections.Generic.IEnumerable<Inno.Core.Graphs.GraphNodeId> nodeIds)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphDocumentController.cs#L151) | Removes nodes and all incident connections as one structural edit. |
| [`void Inno.Editor.Graph.GraphDocumentController.ReplaceDocument(Inno.Core.Graphs.GraphDocument replacement, string historyName, string? gestureId = null)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphDocumentController.cs#L96) | Replaces the complete neutral document as one atomic, undoable authoring operation. |
| [`void Inno.Editor.Graph.GraphDocumentController.SetNodeValue(Inno.Core.Graphs.GraphNodeId nodeId, string propertyId, Inno.Core.Graphs.GraphSerializedValue value, string? gestureId = null)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphDocumentController.cs#L280) | Creates or replaces one neutral node property; only samples carrying the same explicit gesture identity merge. |

### `Inno.Editor.Graph.GraphEditorModule`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Editor.Graph.GraphDocumentController Inno.Editor.Graph.GraphEditorModule.OpenDocument(System.Guid documentId, Inno.Core.Graphs.GraphDocument document, Inno.Editor.Interactions.IEditorHistory history)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphEditorModule.cs#L57) | Opens or joins an active graph document session. |
| [`Inno.Editor.Graph.GraphEditorModule`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphEditorModule.cs#L18) | Owns active neutral graph document sessions resolved by reload-safe Editor History. |
| [`bool Inno.Editor.Graph.GraphEditorModule.CloseDocument(System.Guid documentId)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphEditorModule.cs#L89) | Closes a document explicitly; its neutral History entries remain unavailable until the identity is reopened. |
| [`bool Inno.Editor.Graph.GraphEditorModule.TryOpenDocument(System.Guid documentId, Inno.Editor.Interactions.IEditorHistory history, out Inno.Editor.Graph.GraphDocumentController? controller)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphEditorModule.cs#L115) | Resolves an already open document without replacing its authored contents. |
| [`override void Inno.Editor.Graph.GraphEditorModule.OnDispose()`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphEditorModule.cs#L208) | Unregisters every document identity so issued controllers cannot resolve retired sessions. |
| [`override void Inno.Editor.Graph.GraphEditorModule.OnStart(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphEditorModule.cs#L191) | Initializes this feature when its owning runtime becomes active. |
| [`override void Inno.Editor.Graph.GraphEditorModule.OnStop(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphEditorModule.cs#L199) | Stops this feature before its owning runtime releases the active generation. |
| [`void Inno.Editor.Graph.GraphEditorModule.RebindDocument(System.Guid documentId, Inno.Core.Graphs.GraphDocument document)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphEditorModule.cs#L156) | Rebinds an active session to a newly imported current-generation document. |
| [`void Inno.Editor.Graph.GraphEditorModule.SetAvailability(System.Guid documentId, bool available)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphEditorModule.cs#L135) | Marks a temporarily Missing source without discarding its graph or advancing its History stack. |

## 项目依赖

- [Inno.Core.Identity](../core/Inno.Core.Identity.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Reload](../extensibility/Inno.Extensibility.Reload.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Extensibility.Modules](../extensibility/Inno.Extensibility.Modules.md)：实现依赖，PrivateAssets="compile"。
- [Inno.References](../references/Inno.References.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Scripting.Api](../scripting/Inno.Scripting.Api.md)：实现依赖，PrivateAssets="compile"。
- [Inno.Core.Graphs](../core/Inno.Core.Graphs.md)：公开引用边界由实际签名核对。
- [Inno.Editor.Core](Inno.Editor.Core.md)：公开引用边界由实际签名核对。
- [Inno.Editor.Interactions](Inno.Editor.Interactions.md)：公开引用边界由实际签名核对。
- [Inno.Core.Serialization](../core/Inno.Core.Serialization.md)：公开引用边界由实际签名核对。
- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
