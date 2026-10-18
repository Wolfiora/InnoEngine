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

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Editor.Graph.GraphCanvasState`

<!-- api: T:Inno.Editor.Graph.GraphCanvasState 7001b776849f9c108afe699d70b136fb3c65ae3d90d436a571240789f474a257 -->
<!-- api: P:Inno.Editor.Graph.GraphCanvasState.pan 98019c10914193873d4f07dffa6203f8ef34bceb8660c5fcaf7194e2e054f8f9 -->
<!-- api: P:Inno.Editor.Graph.GraphCanvasState.zoom 6d8e32814c426d986eb3bbdb6f2fee7dfe4651aba58aa3e598e030c97f684c38 -->
<!-- api: P:Inno.Editor.Graph.GraphCanvasState.selectedNodes 1c1118d8f12c0d4e0a4b65eef8b74d7e2aabc066cc4f54b2f258bb65f0aedf6f -->
<!-- api: P:Inno.Editor.Graph.GraphCanvasState.selectedEdges 00c2b2ef6b1e50bef4f82bd7679d813a57a38bd54219aa6a92fad08973ae5f52 -->
<!-- api: P:Inno.Editor.Graph.GraphCanvasState.pendingConnection 0cb41c3d3d489c25036c86e43858dd86cb1b3b0d5c71330f3730398c33530b4c -->
<!-- api: P:Inno.Editor.Graph.GraphCanvasState.pendingConnectionDirection cb80a0a15350dec6b50a5563b2d735ed106794c9301f22555bd2d28a3b513c2a -->
<!-- api: M:Inno.Editor.Graph.GraphCanvasState.SetViewport(Inno.Core.Graphs.GraphPosition,System.Single) 3379326ed4b4815aef99977ea526a54cdd30ecec8cc35aa148de2cb9866b6bb8 -->
<!-- api: M:Inno.Editor.Graph.GraphCanvasState.PanBy(System.Single,System.Single) 7efde6930acf642538314c131ef04a0228a89a530798ee6197c82481a38100cd -->
<!-- api: M:Inno.Editor.Graph.GraphCanvasState.ZoomAt(System.Single,System.Single,System.Single) 766971bf2e645bcb7c9af576b4764efd8b2d97501074ebdbabb723bb09b9c8fb -->
<!-- api: M:Inno.Editor.Graph.GraphCanvasState.SelectNodes(System.Collections.Generic.IEnumerable{Inno.Core.Graphs.GraphNodeId}) 56028b698e39ce5a91729bdf4acd3648b7d8f998caae486f3bed40ef65a6ec62 -->
<!-- api: M:Inno.Editor.Graph.GraphCanvasState.ToggleNode(Inno.Core.Graphs.GraphNodeId) ca8f32b4c89a366305f14ecd11d1c5434ec3acc43708bf0ac8fb76dd66321219 -->
<!-- api: M:Inno.Editor.Graph.GraphCanvasState.ClearSelection 705efe9cb19af8bd69da48fa7e220ded25177db8652b778213db90fbea7f7dbb -->
<!-- api: M:Inno.Editor.Graph.GraphCanvasState.BeginConnection(Inno.Core.Graphs.GraphEndpoint,Inno.Core.Graphs.GraphPortDirection) f45f7482f3fc2f94a4dccfa3a473334d42a5ca2b03f9b9657151bfc74ef7540d -->
<!-- api: M:Inno.Editor.Graph.GraphCanvasState.BeginConnection(Inno.Core.Graphs.GraphEndpoint) c339d9676f8fa83406e5b849d5b0159e294c8d4a98ca9539556bcc3b1ef41d7b -->
<!-- api: M:Inno.Editor.Graph.GraphCanvasState.CancelConnection a416cd573f97e2a9ff9307f057dc88976ba9530a6aab8c1522908895dc70a774 -->
<!-- api: M:Inno.Editor.Graph.GraphCanvasState.#ctor 29bdfd7a0fe7b7ebb3cbbd30a1a6bb903b819812d282aa4cb078ec3cc54264b7 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-79e999ad38b6ec13"></a>[`public Inno.Editor.Graph.GraphCanvasState.GraphCanvasState()`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphCanvasState.cs#L12) | C# 隐式构造函数；按类型声明执行字段初始化和基类构造。 | — | — |
| <a id="api-663b44de75a79a7d"></a>[`public void Inno.Editor.Graph.GraphCanvasState.BeginConnection(Inno.Core.Graphs.GraphEndpoint endpoint, Inno.Core.Graphs.GraphPortDirection direction)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphCanvasState.cs#L164) | Begins a connection drag from one endpoint.<br>`endpoint`：Stable endpoint.<br>`direction`：Direction of the endpoint within its node interface. | — | — |
| <a id="api-42c0d5d6a886b977"></a>[`public void Inno.Editor.Graph.GraphCanvasState.BeginConnection(Inno.Core.Graphs.GraphEndpoint output)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphCanvasState.cs#L178) | Begins a connection drag from one output endpoint.<br>`output`：Stable output endpoint. | — | — |
| <a id="api-14d6c962a1643b2a"></a>[`public void Inno.Editor.Graph.GraphCanvasState.CancelConnection()`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphCanvasState.cs#L183) | Cancels any active connection drag. | — | — |
| <a id="api-45c25474a6e257b8"></a>[`public void Inno.Editor.Graph.GraphCanvasState.ClearSelection()`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphCanvasState.cs#L147) | Clears all transient node and edge selection. | — | — |
| <a id="api-12175988079819db"></a>[`public void Inno.Editor.Graph.GraphCanvasState.PanBy(float deltaX, float deltaY)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphCanvasState.cs#L82) | Moves the canvas origin by screen-space pixels.<br>`deltaX`：Horizontal screen-space delta.<br>`deltaY`：Vertical screen-space delta. | — | — |
| <a id="api-7621f4f2485563e0"></a>[`public void Inno.Editor.Graph.GraphCanvasState.SelectNodes(System.Collections.Generic.IEnumerable<Inno.Core.Graphs.GraphNodeId> nodes)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphCanvasState.cs#L121) | Replaces node selection.<br>`nodes`：Complete selected-node set. | — | — |
| <a id="api-3277c91b484a9e2c"></a>[`public void Inno.Editor.Graph.GraphCanvasState.SetViewport(Inno.Core.Graphs.GraphPosition pan, float zoom)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphCanvasState.cs#L58) | Restores persistent canvas navigation state with bounded zoom.<br>`pan`：Graph-space pan offset.<br>`zoom`：Requested zoom in the inclusive range 0.1 to 4. | — | — |
| <a id="api-83553f9eb9600577"></a>[`public void Inno.Editor.Graph.GraphCanvasState.ToggleNode(Inno.Core.Graphs.GraphNodeId nodeId)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphCanvasState.cs#L135) | Toggles one node while preserving the remaining selection.<br>`nodeId`：Node to toggle. | — | — |
| <a id="api-0263dd5736ecda92"></a>[`public void Inno.Editor.Graph.GraphCanvasState.ZoomAt(float factor, float pivotX, float pivotY)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphCanvasState.cs#L99) | Changes zoom while preserving the graph point beneath a screen-space pivot.<br>`factor`：Positive multiplicative zoom factor.<br>`pivotX`：Screen-space pivot X.<br>`pivotY`：Screen-space pivot Y. | — | — |
| <a id="api-16de2eab9ea84502"></a>[`public Inno.Core.Graphs.GraphPosition Inno.Editor.Graph.GraphCanvasState.pan { get; private set; }`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphCanvasState.cs#L22) | Gets the graph-space origin mapped to canvas screen origin. | — | — |
| <a id="api-8b29d68934bd5917"></a>[`public Inno.Core.Graphs.GraphEndpoint? Inno.Editor.Graph.GraphCanvasState.pendingConnection { get; private set; }`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphCanvasState.cs#L42) | Gets the endpoint currently being connected, or null. | — | — |
| <a id="api-719858168087e1e5"></a>[`public Inno.Core.Graphs.GraphPortDirection? Inno.Editor.Graph.GraphCanvasState.pendingConnectionDirection { get; private set; }`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphCanvasState.cs#L47) | Gets the direction of Inno.Editor.Graph.GraphCanvasState.pendingConnection. | — | — |
| <a id="api-e9a6c04d1ab5124d"></a>[`public System.Collections.Generic.IReadOnlyCollection<Inno.Core.Graphs.GraphEdgeId> Inno.Editor.Graph.GraphCanvasState.selectedEdges { get; }`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphCanvasState.cs#L37) | Gets selected edge identities. | — | — |
| <a id="api-04ed3a6771096ee4"></a>[`public System.Collections.Generic.IReadOnlyCollection<Inno.Core.Graphs.GraphNodeId> Inno.Editor.Graph.GraphCanvasState.selectedNodes { get; }`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphCanvasState.cs#L32) | Gets selected node identities. | — | — |
| <a id="api-a57e62e374295ec8"></a>[`public float Inno.Editor.Graph.GraphCanvasState.zoom { get; private set; }`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphCanvasState.cs#L27) | Gets the current graph-to-screen scale. | — | — |
| <a id="api-75bb02d84ea26de0"></a>[`public sealed class Inno.Editor.Graph.GraphCanvasState`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphCanvasState.cs#L12) | Stores transient pan, zoom, selection and connection interaction state without ImGui dependencies. | — | — |

### `Inno.Editor.Graph.GraphClipboardData`

<!-- api: T:Inno.Editor.Graph.GraphClipboardData 59bdb05a1f90a73a69cc756c77f1f05e60a76e116a1d9f766d2c6a689daa2f39 -->
<!-- api: P:Inno.Editor.Graph.GraphClipboardData.nodeCount e00fff116070c61d452bbd720547c2648643f4d8e3efdf353c215804bb39ece8 -->
<!-- api: P:Inno.Editor.Graph.GraphClipboardData.edgeCount 6b5f60789f653326ac4aa730cb669933cd3977d465ef78fde9df848db23c5faa -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-1dfc6d0b65e98dda"></a>[`public int Inno.Editor.Graph.GraphClipboardData.edgeCount { get; }`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphDocumentController.cs#L28) | Gets the copied connection count whose endpoints both belong to the fragment. | — | — |
| <a id="api-c5d7e4976d83f2b2"></a>[`public int Inno.Editor.Graph.GraphClipboardData.nodeCount { get; }`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphDocumentController.cs#L23) | Gets the copied node count. | — | — |
| <a id="api-d3383be96101c019"></a>[`public sealed class Inno.Editor.Graph.GraphClipboardData`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphDocumentController.cs#L14) | Contains a neutral copied node fragment suitable for graph clipboard operations. | — | — |

### `Inno.Editor.Graph.GraphDocumentController`

<!-- api: T:Inno.Editor.Graph.GraphDocumentController 0a7506be449540e3d5734f5758a5e8b7304b4088f57ea75db9f0b094934d6c83 -->
<!-- api: P:Inno.Editor.Graph.GraphDocumentController.documentId 6560c80d736276febac789e8c149c97f4dbce15ca124d8d2d7c57765d0044de3 -->
<!-- api: P:Inno.Editor.Graph.GraphDocumentController.isAvailable 6232ebd2ec00773d37746520b04d848b15b526357b778c16329cc66a265dd1e6 -->
<!-- api: P:Inno.Editor.Graph.GraphDocumentController.document 81410251dc6efeb261ac72ffd403aeee244c392edb5e39d0af7be687d77f84f4 -->
<!-- api: P:Inno.Editor.Graph.GraphDocumentController.revision b315637bb36057c25723add38d58f790a94190a58ef1b7cd58438fd4125162c5 -->
<!-- api: P:Inno.Editor.Graph.GraphDocumentController.isDirty ba05da72298ecce14a130755e28c9b56103c3f35cb5cb7c8c092dbeecbbd417a -->
<!-- api: M:Inno.Editor.Graph.GraphDocumentController.MarkSaved 3836d8c73b8cadb708ae2668ec33247c167a26986b9ee5bee2c87cd9d50872a5 -->
<!-- api: M:Inno.Editor.Graph.GraphDocumentController.ReplaceDocument(Inno.Core.Graphs.GraphDocument,System.String,System.String) 01032a5469f3d58bb01f8dcf43423f949030c99ea2e95708530be6c1483ee6bc -->
<!-- api: M:Inno.Editor.Graph.GraphDocumentController.AddNode(System.String,Inno.Core.Graphs.GraphPosition,System.Collections.Generic.IReadOnlyDictionary{System.String,Inno.Core.Graphs.GraphSerializedValue}) e25c86d5d0ff2d98809a8b3592ebb4c8943f238ba6d22f4097ff64d3bcaf4748 -->
<!-- api: M:Inno.Editor.Graph.GraphDocumentController.RemoveNodes(System.Collections.Generic.IEnumerable{Inno.Core.Graphs.GraphNodeId}) 9df2c37c4d0e14a41dc0e79278efdb5ba4ad88088ee39efda5ccbc429eb5f115 -->
<!-- api: M:Inno.Editor.Graph.GraphDocumentController.MoveNodes(System.Collections.Generic.IReadOnlyDictionary{Inno.Core.Graphs.GraphNodeId,Inno.Core.Graphs.GraphPosition}) 264ab47e7749d605321424610b90131a3f4be9efeb8b510f75a297231dbb45fd -->
<!-- api: M:Inno.Editor.Graph.GraphDocumentController.Connect(Inno.Core.Graphs.GraphEndpoint,Inno.Core.Graphs.GraphEndpoint) b94483e03fc8bd866f3787b4a6fbdb58047f7a01b6079d30e288194f8575435e -->
<!-- api: M:Inno.Editor.Graph.GraphDocumentController.Disconnect(Inno.Core.Graphs.GraphEdgeId) 69906acaf7485fa57b9db3ec09073bff386312c5108c3ca86b83770f20675cf0 -->
<!-- api: M:Inno.Editor.Graph.GraphDocumentController.SetNodeValue(Inno.Core.Graphs.GraphNodeId,System.String,Inno.Core.Graphs.GraphSerializedValue,System.String) a1197e369fcd898dd23ae77a4a43040c66d9121bf609862affb35a62e6c3a4ec -->
<!-- api: M:Inno.Editor.Graph.GraphDocumentController.RemoveNodeValue(Inno.Core.Graphs.GraphNodeId,System.String) d83344782edf7b0b2e1b5015d3a8d470e83d0c9b3a65204d12d6fd2ecb9d0746 -->
<!-- api: M:Inno.Editor.Graph.GraphDocumentController.Copy(System.Collections.Generic.IEnumerable{Inno.Core.Graphs.GraphNodeId}) d1489905a44e93706e6a209acc75b8fb41d36461847efa2a73e3cab061ac17b6 -->
<!-- api: M:Inno.Editor.Graph.GraphDocumentController.Paste(Inno.Editor.Graph.GraphClipboardData,Inno.Core.Graphs.GraphPosition,System.Action{Inno.Core.Graphs.GraphNodeRecord,System.Collections.Generic.IReadOnlyDictionary{Inno.Core.Graphs.GraphNodeId,Inno.Core.Graphs.GraphNodeId}}) 9ccc9cbc621114943a8fc4c508d7880a4a4178eff392ee3c913d5ee1463c6633 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-44a94e590fd0df86"></a>[`public Inno.Core.Graphs.GraphNodeId Inno.Editor.Graph.GraphDocumentController.AddNode(string definitionId, Inno.Core.Graphs.GraphPosition position, System.Collections.Generic.IReadOnlyDictionary<string, Inno.Core.Graphs.GraphSerializedValue>? values = null)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphDocumentController.cs#L122) | Adds a node with a generated stable identity.<br>返回：The new stable node identity.<br>`definitionId`：Stable node definition ID.<br>`position`：Initial graph-space position.<br>`values`：Optional neutral property values. | — | — |
| <a id="api-c1deeb1ec3bd97f4"></a>[`public Inno.Core.Graphs.GraphEdgeId Inno.Editor.Graph.GraphDocumentController.Connect(Inno.Core.Graphs.GraphEndpoint output, Inno.Core.Graphs.GraphEndpoint input)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphDocumentController.cs#L217) | Creates or reconnects one input endpoint to an output endpoint.<br>返回：The generated connection identity.<br>`output`：Source endpoint.<br>`input`：Destination endpoint. | — | — |
| <a id="api-4ffc9095acbf2e86"></a>[`public Inno.Editor.Graph.GraphClipboardData Inno.Editor.Graph.GraphDocumentController.Copy(System.Collections.Generic.IEnumerable<Inno.Core.Graphs.GraphNodeId> nodeIds)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphDocumentController.cs#L331) | Copies selected nodes and only their internal connections.<br>返回：A detached neutral clipboard fragment.<br>`nodeIds`：Nodes to copy. | — | — |
| <a id="api-f4bcfe49b96799d1"></a>[`public bool Inno.Editor.Graph.GraphDocumentController.Disconnect(Inno.Core.Graphs.GraphEdgeId edgeId)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphDocumentController.cs#L254) | Removes one stable connection.<br>返回：true after a connection was removed.<br>`edgeId`：Connection to remove. | — | — |
| <a id="api-2b2010fdac4abc4a"></a>[`public void Inno.Editor.Graph.GraphDocumentController.MarkSaved()`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphDocumentController.cs#L82) | Marks the current revision as saved without creating a data History entry. | — | — |
| <a id="api-6e575c88f881530c"></a>[`public void Inno.Editor.Graph.GraphDocumentController.MoveNodes(System.Collections.Generic.IReadOnlyDictionary<Inno.Core.Graphs.GraphNodeId, Inno.Core.Graphs.GraphPosition> positions)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphDocumentController.cs#L180) | Moves one or more nodes as one completed gesture, independently undoable from earlier drags.<br>`positions`：Complete destination positions keyed by node identity. | — | — |
| <a id="api-eb735e849c099b8a"></a>[`public System.Collections.Generic.IReadOnlyList<Inno.Core.Graphs.GraphNodeId> Inno.Editor.Graph.GraphDocumentController.Paste(Inno.Editor.Graph.GraphClipboardData clipboard, Inno.Core.Graphs.GraphPosition offset, System.Action<Inno.Core.Graphs.GraphNodeRecord, System.Collections.Generic.IReadOnlyDictionary<Inno.Core.Graphs.GraphNodeId, Inno.Core.Graphs.GraphNodeId>>? remapNode = null)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphDocumentController.cs#L365) | Pastes a clipboard fragment with new stable identities and a graph-space offset.<br>返回：New node identities in clipboard document order.<br>`clipboard`：Detached neutral fragment.<br>`offset`：Offset added to every copied node position.<br>`remapNode`：Optional synchronous domain remapping of neutral node values after identity allocation. The callback is never stored in History. | — | — |
| <a id="api-dfaf31e04479f40d"></a>[`public void Inno.Editor.Graph.GraphDocumentController.RemoveNodes(System.Collections.Generic.IEnumerable<Inno.Core.Graphs.GraphNodeId> nodeIds)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphDocumentController.cs#L151) | Removes nodes and all incident connections as one structural edit.<br>`nodeIds`：Nodes to remove. | — | — |
| <a id="api-27681a5074031c0b"></a>[`public bool Inno.Editor.Graph.GraphDocumentController.RemoveNodeValue(Inno.Core.Graphs.GraphNodeId nodeId, string propertyId)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphDocumentController.cs#L307) | Removes one neutral node property as a structural edit.<br>返回：true when the property existed.<br>`nodeId`：Owning node.<br>`propertyId`：Stable property ID. | — | — |
| <a id="api-e54a40900f887bbc"></a>[`public void Inno.Editor.Graph.GraphDocumentController.ReplaceDocument(Inno.Core.Graphs.GraphDocument replacement, string historyName, string? gestureId = null)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphDocumentController.cs#L96) | Replaces the complete neutral document as one atomic, undoable authoring operation.<br>`replacement`：Complete replacement document copied by the operation.<br>`historyName`：Concise artist-facing history entry name.<br>`gestureId`：Unique active gesture identity for continuous samples, or null for an independent edit. | — | — |
| <a id="api-f0ca26f2e956259e"></a>[`public void Inno.Editor.Graph.GraphDocumentController.SetNodeValue(Inno.Core.Graphs.GraphNodeId nodeId, string propertyId, Inno.Core.Graphs.GraphSerializedValue value, string? gestureId = null)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphDocumentController.cs#L280) | Creates or replaces one neutral node property; only samples carrying the same explicit gesture identity merge.<br>`nodeId`：Owning node.<br>`propertyId`：Stable property ID.<br>`value`：New neutral value.<br>`gestureId`：Unique current text/numeric gesture identity, or null for an independent edit. | — | — |
| <a id="api-db37fb8ad560fd3b"></a>[`public Inno.Core.Graphs.GraphDocument Inno.Editor.Graph.GraphDocumentController.document { get; }`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphDocumentController.cs#L67) | Gets the mutable neutral document used by the current asset generation. | — | — |
| <a id="api-8f6d5c1ddd11c809"></a>[`public System.Guid Inno.Editor.Graph.GraphDocumentController.documentId { get; }`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphDocumentController.cs#L57) | Gets the stable project-relative document identity. | — | — |
| <a id="api-3130ffe4dd90f4de"></a>[`public bool Inno.Editor.Graph.GraphDocumentController.isAvailable { get; }`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphDocumentController.cs#L62) | Gets whether the current document identity and source are both available. | — | — |
| <a id="api-a355fd30c0761d5b"></a>[`public bool Inno.Editor.Graph.GraphDocumentController.isDirty { get; }`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphDocumentController.cs#L77) | Gets whether content changed after its last explicit saved marker. | — | — |
| <a id="api-7f8676837d3b6374"></a>[`public ulong Inno.Editor.Graph.GraphDocumentController.revision { get; }`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphDocumentController.cs#L72) | Gets the monotonic in-session content revision. | — | — |
| <a id="api-c7294bbc2f124f61"></a>[`public sealed class Inno.Editor.Graph.GraphDocumentController`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphDocumentController.cs#L36) | Applies neutral graph edits atomically and records every data mutation in reload-safe Editor History. | — | — |

### `Inno.Editor.Graph.GraphEditorModule`

<!-- api: T:Inno.Editor.Graph.GraphEditorModule cf79c44d5d97410c635db3a022340a107f450bec92ceabeed10c5cc93d9b7736 -->
<!-- api: M:Inno.Editor.Graph.GraphEditorModule.OpenDocument(System.Guid,Inno.Core.Graphs.GraphDocument,Inno.Editor.Interactions.IEditorHistory) f9d1c7c05fc926f08d8bb5c16961c982a0a65c37336909761b911077155f9280 -->
<!-- api: M:Inno.Editor.Graph.GraphEditorModule.CloseDocument(System.Guid) 73ac251298684794779914e24087e93df65cd6d5e381c5bcca6943625949b5d3 -->
<!-- api: M:Inno.Editor.Graph.GraphEditorModule.TryOpenDocument(System.Guid,Inno.Editor.Interactions.IEditorHistory,Inno.Editor.Graph.GraphDocumentController@) 235296ce2f76bd5b2410d6d036f3f280c227461459a8a59a248135ebd0331aba -->
<!-- api: M:Inno.Editor.Graph.GraphEditorModule.SetAvailability(System.Guid,System.Boolean) ce5f4d5c39a636286e15322629ab0bbcb4b0a89baad42a3a31d95564ff18ae9d -->
<!-- api: M:Inno.Editor.Graph.GraphEditorModule.RebindDocument(System.Guid,Inno.Core.Graphs.GraphDocument) c361fa3cbd83624091c4b560838d48600a377b83f58232bc2478b2a5ef9fcd0c -->
<!-- api: M:Inno.Editor.Graph.GraphEditorModule.OnStart(Inno.Editor.Core.EditorContext) ea780b04958acccd7882b6c152ffe69be87daaabcd741829a87b811142c57564 -->
<!-- api: M:Inno.Editor.Graph.GraphEditorModule.OnStop(Inno.Editor.Core.EditorContext) 8cd4507b4ff2f357b85dc702736529eca6f0070f401c1c295b5830eb08d46188 -->
<!-- api: M:Inno.Editor.Graph.GraphEditorModule.OnDispose ae254ca47698a4b5c4cf884c155ccfd3b10ab4ade3e9dcd0df703483fadc4f4d -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-f83f78c4be225636"></a>[`public bool Inno.Editor.Graph.GraphEditorModule.CloseDocument(System.Guid documentId)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphEditorModule.cs#L89) | Closes a document explicitly; its neutral History entries remain unavailable until the identity is reopened.<br>返回：True when a live document was closed.<br>`documentId`：The persistent document identity to release. | — | — |
| <a id="api-acdf657f0fb7a949"></a>[`protected override void Inno.Editor.Graph.GraphEditorModule.OnDispose()`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphEditorModule.cs#L208) | Unregisters every document identity so issued controllers cannot resolve retired sessions. | — | — |
| <a id="api-70a7ee3828f9f515"></a>[`protected override void Inno.Editor.Graph.GraphEditorModule.OnStart(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphEditorModule.cs#L191) | Initializes this feature when its owning runtime becomes active.<br>`context`：The context that supplies state and services for this operation. | — | — |
| <a id="api-f67531bd97f2c315"></a>[`protected override void Inno.Editor.Graph.GraphEditorModule.OnStop(Inno.Editor.Core.EditorContext context)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphEditorModule.cs#L199) | Stops this feature before its owning runtime releases the active generation.<br>`context`：The context that supplies state and services for this operation. | — | — |
| <a id="api-5887a63af5577123"></a>[`public Inno.Editor.Graph.GraphDocumentController Inno.Editor.Graph.GraphEditorModule.OpenDocument(System.Guid documentId, Inno.Core.Graphs.GraphDocument document, Inno.Editor.Interactions.IEditorHistory history)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphEditorModule.cs#L57) | Opens or joins an active graph document session.<br>返回：A presentation-independent document controller.<br>`documentId`：Persistent document identity; use the Asset identity for asset-backed graphs.<br>`document`：Mutable neutral document owned by the current asset generation.<br>`history`：Shared editor history used for every data mutation. | — | `System.InvalidOperationException`：The bounded document owner is full. |
| <a id="api-a5804ef1d83ce974"></a>[`public void Inno.Editor.Graph.GraphEditorModule.RebindDocument(System.Guid documentId, Inno.Core.Graphs.GraphDocument document)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphEditorModule.cs#L156) | Rebinds an active session to a newly imported current-generation document.<br>`documentId`：Stable document identity.<br>`document`：New current-generation document instance. | — | `System.InvalidOperationException`：Thrown when the document has no active session. |
| <a id="api-34fbf9215a1f5ba6"></a>[`public void Inno.Editor.Graph.GraphEditorModule.SetAvailability(System.Guid documentId, bool available)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphEditorModule.cs#L135) | Marks a temporarily Missing source without discarding its graph or advancing its History stack.<br>`documentId`：The persistent identity of an open document.<br>`available`：Whether its source can be resolved in the current generation. | — | — |
| <a id="api-62d748b2e1b23e6f"></a>[`public bool Inno.Editor.Graph.GraphEditorModule.TryOpenDocument(System.Guid documentId, Inno.Editor.Interactions.IEditorHistory history, out Inno.Editor.Graph.GraphDocumentController? controller)`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphEditorModule.cs#L115) | Resolves an already open document without replacing its authored contents.<br>返回：True when the document is still owned by this module.<br>`documentId`：The persistent document identity.<br>`history`：The shared history owner for the returned controller.<br>`controller`：The joined controller, or null when the document is closed. | — | — |
| <a id="api-e783863c315e1b44"></a>[`public sealed class Inno.Editor.Graph.GraphEditorModule : Inno.Editor.Core.EditorModule, Inno.Editor.Core.IEditorReloadParticipant`](../../src/composition/editor/framework/Inno.Editor.Graph/GraphEditorModule.cs#L19) | Owns active neutral graph document sessions resolved by reload-safe Editor History. | — | — |

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
