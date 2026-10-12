# Inno.Core.Collections

[上一页：Mathematics](Inno.Core.Mathematics.md) · [Core 索引](README.md) · [前往 Assets](../assets/README.md)

Storage 提供两个独立的数据结构：线程安全、方向明确的 `DependencyGraph<TKey>`，以及支持多索引与查询优化的 `IndexedObjectStore<T>`。

## DependencyGraph&lt;TKey&gt;

边 `node -> dependency` 表示“node 依赖 dependency”。Graph 接受环；只有要求 DAG 的操作会显式报告环。

### 构造与状态

```csharp
using System;
using Inno.Core.Collections;

DependencyGraph<string> graph = new(
    equalityComparer: StringComparer.Ordinal,
    orderingComparer: StringComparer.Ordinal);
```

- `count`：节点数。
- `version`：成功结构变更后递增。
- 未提供 ordering comparer 时，查询使用节点插入顺序保证确定性。

### 修改 API

| 方法 | 说明 |
| --- | --- |
| `ContainsNode(node)` | 节点是否存在。 |
| `AddNode(node)` | 新增节点，存在时 false。 |
| `RemoveNode(node)` | 删除节点及所有入/出边。 |
| `AddDependency(node, dependency)` | 自动创建两端节点并添加边。 |
| `RemoveDependency(node, dependency)` | 移除边。 |
| `ReplaceDependencies(node, IEnumerable<TKey>)` | 原子替换全部直接依赖。 |
| `Clear()` | 清除全部节点和边。 |

### 查询 API

| 方法 | 说明 |
| --- | --- |
| `GetDependencies(node, recursive=false)` | 直接或传递依赖。 |
| `GetDependents(node, recursive=false)` | 直接或传递反向依赖。 |
| `DependsOn(node, dependency, recursive=false)` | 判断依赖关系。 |
| `TopologicalSort()` | dependency 在 dependent 之前；有环时抛异常。 |
| `TryFindCycle(out cycle)` | 找到一条确定性的环路径。 |
| `GetStronglyConnectedComponents()` | 返回强连通分量。 |

```csharp
using System;
using System.Collections.Generic;
using Inno.Core.Collections;

DependencyGraph<string> graph = new(
    equalityComparer: StringComparer.Ordinal,
    orderingComparer: StringComparer.Ordinal);
graph.AddDependency("Scene", "Texture");
graph.AddDependency("Material", "Texture");
IReadOnlyList<string> order = graph.TopologicalSort();
Console.WriteLine(string.Join(", ", order));
```

当前共享消费者包括 Asset runtime/import dependency、Assembly reload module、Plugin manifest、Script asmdef、Project Settings contributor 和通用 Graph validation。它们统一使用相同的边方向、确定性排序、环路径与反向依赖定义，不再各自维护 DFS/Kahn 变体。

RenderGraph 编译器和 Job scheduler 内部没有改用该容器：前者每帧处理紧凑整数索引并需要资源 hazard 来源诊断，后者直接维护可复用 job slot/dependent counter；在这些高频路径套用带读写锁和通用 key 的持久 Graph 会增加分配与锁竞争。它们只共享算法语义，不共享存储表示。

## IndexedObjectStore&lt;T&gt;

Store 按引用身份存储 class，支持为同一对象定义多个 typed key。名称使用 `Store` 是为了明确表达其职责：它不负责对象创建、复用或生命周期回收，而是“稠密对象集合 + 多索引 + 查询”容器。

### Store API

| 方法/属性 | 说明 |
| --- | --- |
| `count` | 当前对象数。 |
| `DefineKey<TKey>(name, flags, comparer?)` | 创建 typed index，返回 `IndexedObjectKey<TKey>`。 |
| `RemoveKey(key)` | 删除 index；旧 handle 失效。 |
| `TryGetKey<TKey>(name, out key)` | 名称和 TKey 都匹配时返回 handle。 |
| `GetAllKeys()` | lazy fail-fast key name enumerable。 |
| `Add(item)` | 按引用去重添加，返回用于链式 Set 的 `IndexedObjectEntry<T>`。 |
| `Remove(item)` | 删除对象及所有 key 值。 |
| `FindFast(key,value)` | lazy fail-fast 匹配结果。 |
| `Find(key,value)` | 与后续修改隔离的快照。 |
| `First(key,value)` | 首个匹配或 null。 |
| `All()` / `AllFast()` | 全量快照 / lazy fail-fast。 |
| `Query()` | 新建 `IndexedObjectQuery<T>` builder。 |
| `RemoveAll()` | 清对象但保留 keys。 |
| `Clear()` | 清对象和 keys；所有 key handle 失效。 |

### Key flags 与 handle

`IndexedObjectKeyFlags` 是 flags：

- `Unordered`：hash lookup，不维护 key 顺序。
- `Ordered`：使用提供的 comparer 维护顺序；Define 时 comparer 必填。
- `Unique`：每个 key value 最多一个 item。

`IndexedObjectKey<TKey>` 公开 `name` 与 `isValid`。它通过对 owning store 的弱引用验证，不会延长 store 生命周期。`IndexedObjectEntry<T>.isValid` 表示 item 仍在 store；`Set(key,value)` 更新索引并返回自身以便链式调用。

```csharp
using System;
using System.Collections.Generic;
using Inno.Core.Collections;

IndexedObjectStore<Entity> store = new();
IndexedObjectKey<Guid> byId = store.DefineKey<Guid>("id", IndexedObjectKeyFlags.Unique);
IndexedObjectKey<int> byOrder = store.DefineKey<int>(
    "order", IndexedObjectKeyFlags.Ordered, Comparer<int>.Default);
Entity entity = new(Guid.NewGuid(), 10);
store.Add(entity).Set(byId, entity.id).Set(byOrder, entity.order);
Entity? found = store.First(byId, entity.id);
Console.WriteLine(found?.id);

internal sealed record Entity(
    Guid id,
    int order
);
```

## IndexedObjectQuery&lt;T&gt;

| API | 说明 |
| --- | --- |
| `Where(IIndexedObjectQueryCondition<T>)` | 添加自定义 condition。 |
| `Where(Func<T,bool>)` | 包装为 `IndexedObjectPredicateCondition<T>`。 |
| `Find(key,value)` | 添加 `IndexedObjectKeyCondition` 条件。 |
| `OrderBy(orderedKey)` | 按已声明 Ordered 的 key 输出。 |
| `GetFast()` | lazy fail-fast enumerable。 |
| `Get()` | 稳定快照。 |
| `First()` | 首个匹配或 null。 |

普通与 Ordered executor 都会优先选择估计候选数最小的 indexed condition，再验证其他 condition。Ordered 查询在存在可索引条件时只读取候选对象已经维护的 order key，并按 comparer 排序；`First()` 只在候选中寻找最小 order key，不构造完整结果。只有全部条件都是无法提供候选集的 predicate 时才遍历完整有序索引。相同 order key 使用当前 dense storage position 作为确定性 tie-breaker；需要跨删除稳定顺序的调用方应自行维护 Unique + Ordered 顺序键。

## 自定义查询条件

`IIndexedObjectQueryCondition<T>` 需要实现：

- `GetCandidateCount(IndexedObjectStore<T>)`：估计候选数，不知道时返回 `int.MaxValue`。
- `TryGetSingle(...)`：可直接定位唯一结果时返回它。
- `GetSet(...)`：可提供候选集合时返回集合，否则 null。
- `Validate(store,item)`：最终判断。

内置 `IndexedObjectKeyCondition<T,TKey>` 使用 index；`IndexedObjectPredicateCondition<T>` 全扫描并可从 `Func<T,bool>` 隐式转换。

## 快照与 Fast 枚举

- `Get` / `Find` / `All` 返回分离快照，之后修改 Store 不影响结果。
- `GetFast` / `FindFast` / `AllFast` 避免复制，但枚举期间 Store version 改变会抛异常。
- Unique key 的重复值会抛 `InvalidOperationException`，不会静默覆盖另一个对象。
- 更新已有对象的 Unique key 时会先验证新值；若新值冲突，旧索引仍保持有效。
- Storage-order 查询是通用公开语义，Scene 等使用方不需要 friend assembly 或访问 Store 内部句柄；实现仍由 Store 在读锁内结合索引与 dense insertion index 完成。

## 当前源码公开 API 清单

按类型列出当前 public 与可供外部派生实现使用的 protected 声明。签名包含 nullable、默认参数和泛型约束；源码链接可定位实现。表中的“—”表示该声明没有额外 XML 约束，不能据此推断线程安全或不会失败。生命周期与组合顺序见本页前文。

### `Inno.Core.Collections.DependencyGraph`1`

<!-- api: T:Inno.Core.Collections.DependencyGraph`1 e920aee4d52578b0890bef8fe27cf32a501279b9484ab240da358fc141a0d8a3 -->
<!-- api: M:Inno.Core.Collections.DependencyGraph`1.#ctor(System.Collections.Generic.IEqualityComparer{`0},System.Collections.Generic.IComparer{`0}) aaed2b90b37cad2309de5b49eb01818f49118fd8cfae0b837e4d63536e467b4c -->
<!-- api: P:Inno.Core.Collections.DependencyGraph`1.count a859deaaea7482cb6386772413a0fa725d7a590276e74cec0f3dfd9d5206be31 -->
<!-- api: P:Inno.Core.Collections.DependencyGraph`1.version 67b2c380140dda8c292d1652b6de3738acc0b389bc284b5a0e23f74ac64c0190 -->
<!-- api: M:Inno.Core.Collections.DependencyGraph`1.ContainsNode(`0) 744d5e53cab2d395ac558a9101a8e83f9e13ae8ddf91bbaabb3bc702cc27918f -->
<!-- api: M:Inno.Core.Collections.DependencyGraph`1.AddNode(`0) 6be82d165497ec0679a40577ea9d1bccdde909bd12fd7cc9040804cde8375607 -->
<!-- api: M:Inno.Core.Collections.DependencyGraph`1.RemoveNode(`0) a43f5eac9324267e2dd3fbeaa579d33e6894406e0cb5448aa00fb97a48904ae0 -->
<!-- api: M:Inno.Core.Collections.DependencyGraph`1.AddDependency(`0,`0) 1bd841b00ab9ff9f115b999122ce6754ef76aaa920b342965f70afaa851d6443 -->
<!-- api: M:Inno.Core.Collections.DependencyGraph`1.RemoveDependency(`0,`0) 42da340c7d7ce8aa05d50a072a893b94b93c6b6c23aa2d87ef5bf2de53caf97d -->
<!-- api: M:Inno.Core.Collections.DependencyGraph`1.ReplaceDependencies(`0,System.Collections.Generic.IEnumerable{`0}) 32610903c0525b3207fd586e9084f3eb7ac331a460b7b2b9d8fdb743385ce112 -->
<!-- api: M:Inno.Core.Collections.DependencyGraph`1.GetDependencies(`0,System.Boolean) 196c043a4ebd8ebfeae024f873ae6b47959fbb9c652993846645cfd69749a98d -->
<!-- api: M:Inno.Core.Collections.DependencyGraph`1.GetDependents(`0,System.Boolean) 34ac3282e8fd24658a3471c8278905acac68180ebe15c06c7c363e25f959f7c6 -->
<!-- api: M:Inno.Core.Collections.DependencyGraph`1.DependsOn(`0,`0,System.Boolean) 8cd7140c4874723297f6d94540283924da910a5f2e190264e816045fab1caeae -->
<!-- api: M:Inno.Core.Collections.DependencyGraph`1.TopologicalSort 95f4c1d2765b8edde73a7f2ca2d0dbb5f425afddcf63985817f123d692eb53e4 -->
<!-- api: M:Inno.Core.Collections.DependencyGraph`1.TryFindCycle(System.Collections.Generic.IReadOnlyList{`0}@) e5c469390be3264bd2619dfba18c127f866969b10dbcb32500fb869be3078efd -->
<!-- api: M:Inno.Core.Collections.DependencyGraph`1.GetStronglyConnectedComponents 2a74d053a2df410c23d560f7ab1b7b951fea5d117e28b1b0a4006864f306562a -->
<!-- api: M:Inno.Core.Collections.DependencyGraph`1.Clear 05c0e54c313c3205c16f50ab53a53c95fe4b2a7eb0c9bb19abde0c4148ee6fbd -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-0568e8f596adeca9"></a>[`public Inno.Core.Collections.DependencyGraph<TKey>.DependencyGraph(System.Collections.Generic.IEqualityComparer<TKey>? equalityComparer = null, System.Collections.Generic.IComparer<TKey>? orderingComparer = null)`](../../src/foundation/core/Inno.Core.Collections/DependencyGraph/DependencyGraph.cs#L83) | Creates an empty dependency graph.<br>`equalityComparer`：Optional node equality comparer.<br>`orderingComparer`：Optional ordering comparer used to make query results deterministic. When omitted, node insertion order is used. | — | — |
| <a id="api-86bb652d006aa5e3"></a>[`public bool Inno.Core.Collections.DependencyGraph<TKey>.AddDependency(TKey node, TKey dependency)`](../../src/foundation/core/Inno.Core.Collections/DependencyGraph/DependencyGraph.cs#L231) | Adds an edge indicating that one node depends on another.<br>返回：true when the edge was added.<br>`node`：The dependent node.<br>`dependency`：The required dependency. | — | — |
| <a id="api-72fe0bfd9171982f"></a>[`public bool Inno.Core.Collections.DependencyGraph<TKey>.AddNode(TKey node)`](../../src/foundation/core/Inno.Core.Collections/DependencyGraph/DependencyGraph.cs#L164) | Adds a node when it does not already exist.<br>返回：true when a node was added.<br>`node`：The node key. | — | — |
| <a id="api-8084ab8c30f3ee38"></a>[`public void Inno.Core.Collections.DependencyGraph<TKey>.Clear()`](../../src/foundation/core/Inno.Core.Collections/DependencyGraph/DependencyGraph.cs#L572) | Removes every node and edge. | — | — |
| <a id="api-2ea6d0b4de56c184"></a>[`public bool Inno.Core.Collections.DependencyGraph<TKey>.ContainsNode(TKey node)`](../../src/foundation/core/Inno.Core.Collections/DependencyGraph/DependencyGraph.cs#L142) | Determines whether a node exists.<br>返回：true when the node exists.<br>`node`：The node key. | — | — |
| <a id="api-dcc60a5cfd3fd4ae"></a>[`public bool Inno.Core.Collections.DependencyGraph<TKey>.DependsOn(TKey node, TKey dependency, bool recursive = false)`](../../src/foundation/core/Inno.Core.Collections/DependencyGraph/DependencyGraph.cs#L389) | Determines whether one node directly or transitively depends on another.<br>返回：true when the dependency exists.<br>`node`：The dependent node.<br>`dependency`：The dependency to find.<br>`recursive`：Whether transitive edges should be searched. | — | — |
| <a id="api-d64166c0b2c8d4f5"></a>[`public System.Collections.Generic.IReadOnlyList<TKey> Inno.Core.Collections.DependencyGraph<TKey>.GetDependencies(TKey node, bool recursive = false)`](../../src/foundation/core/Inno.Core.Collections/DependencyGraph/DependencyGraph.cs#L350) | Gets direct or transitive dependencies of a node.<br>返回：A stable dependency snapshot, or an empty list when the node is absent.<br>`node`：The node to query.<br>`recursive`：Whether transitive dependencies should be included. | — | — |
| <a id="api-41c2aa3cd57ac6b6"></a>[`public System.Collections.Generic.IReadOnlyList<TKey> Inno.Core.Collections.DependencyGraph<TKey>.GetDependents(TKey node, bool recursive = false)`](../../src/foundation/core/Inno.Core.Collections/DependencyGraph/DependencyGraph.cs#L368) | Gets direct or transitive dependents of a node.<br>返回：A stable dependent snapshot, or an empty list when the node is absent.<br>`node`：The node to query.<br>`recursive`：Whether transitive dependents should be included. | — | — |
| <a id="api-ed96995e9b62aefe"></a>[`public System.Collections.Generic.IReadOnlyList<System.Collections.Generic.IReadOnlyList<TKey>> Inno.Core.Collections.DependencyGraph<TKey>.GetStronglyConnectedComponents()`](../../src/foundation/core/Inno.Core.Collections/DependencyGraph/DependencyGraph.cs#L501) | Gets every strongly connected component in deterministic order.<br>返回：A stable snapshot containing cyclic components and single-node acyclic components. | — | — |
| <a id="api-bad1571da18303f2"></a>[`public bool Inno.Core.Collections.DependencyGraph<TKey>.RemoveDependency(TKey node, TKey dependency)`](../../src/foundation/core/Inno.Core.Collections/DependencyGraph/DependencyGraph.cs#L265) | Removes a dependency edge.<br>返回：true when the edge was removed.<br>`node`：The dependent node.<br>`dependency`：The required dependency. | — | — |
| <a id="api-c1dfd16b621fafb2"></a>[`public bool Inno.Core.Collections.DependencyGraph<TKey>.RemoveNode(TKey node)`](../../src/foundation/core/Inno.Core.Collections/DependencyGraph/DependencyGraph.cs#L190) | Removes a node and every incoming and outgoing edge connected to it.<br>返回：true when the node was removed.<br>`node`：The node key. | — | — |
| <a id="api-4842ae688c8b64b4"></a>[`public void Inno.Core.Collections.DependencyGraph<TKey>.ReplaceDependencies(TKey node, System.Collections.Generic.IEnumerable<TKey> dependencies)`](../../src/foundation/core/Inno.Core.Collections/DependencyGraph/DependencyGraph.cs#L302) | Atomically replaces every direct dependency of a node.<br>`node`：The dependent node.<br>`dependencies`：The complete replacement dependency set. | — | `System.ArgumentNullException`：Thrown when dependencies is null. |
| <a id="api-19c01874a1554d65"></a>[`public System.Collections.Generic.IReadOnlyList<TKey> Inno.Core.Collections.DependencyGraph<TKey>.TopologicalSort()`](../../src/foundation/core/Inno.Core.Collections/DependencyGraph/DependencyGraph.cs#L432) | Returns an order in which every dependency precedes its dependents.<br>返回：A deterministic topological ordering. | — | `System.InvalidOperationException`：Thrown when the graph contains a cycle. The exception message contains a complete cycle. |
| <a id="api-3e6c579e95f0221e"></a>[`public bool Inno.Core.Collections.DependencyGraph<TKey>.TryFindCycle(out System.Collections.Generic.IReadOnlyList<TKey> cycle)`](../../src/foundation/core/Inno.Core.Collections/DependencyGraph/DependencyGraph.cs#L481) | Tries to find one complete cycle in the graph.<br>返回：true when a cycle was found.<br>`cycle`：A closed cycle whose first and last nodes are equal, or an empty list when acyclic. | — | — |
| <a id="api-09794fb3ef202502"></a>[`public int Inno.Core.Collections.DependencyGraph<TKey>.count { get; }`](../../src/foundation/core/Inno.Core.Collections/DependencyGraph/DependencyGraph.cs#L95) | Gets the number of nodes in the graph. | — | — |
| <a id="api-24d407030235a2a9"></a>[`public long Inno.Core.Collections.DependencyGraph<TKey>.version { get; }`](../../src/foundation/core/Inno.Core.Collections/DependencyGraph/DependencyGraph.cs#L117) | Gets the structural version of the graph. | The version changes after every successful structural mutation. | — |
| <a id="api-446588c1dbf12be6"></a>[`public sealed class Inno.Core.Collections.DependencyGraph<TKey> where TKey : notnull`](../../src/foundation/core/Inno.Core.Collections/DependencyGraph/DependencyGraph.cs#L19) | Stores a directed dependency graph and provides deterministic dependency queries. | An edge from node to dependency means that node depends on dependency. The graph accepts cycles; operations that require an acyclic graph, such as Inno.Core.Collections.DependencyGraph`1.TopologicalSort, report them explicitly. | — |

### `Inno.Core.Collections.IIndexedObjectQueryCondition`1`

<!-- api: T:Inno.Core.Collections.IIndexedObjectQueryCondition`1 4c6ea3fb89a86f3a9998c580b58db25e4d50f44a56eb1980d530e79cab64d794 -->
<!-- api: M:Inno.Core.Collections.IIndexedObjectQueryCondition`1.GetCandidateCount(Inno.Core.Collections.IndexedObjectStore{`0}) 09b7af9776db488c67533e9ff0a50cebe7e22af9d37ea23a89847c137ea83f2c -->
<!-- api: M:Inno.Core.Collections.IIndexedObjectQueryCondition`1.TryGetSingle(Inno.Core.Collections.IndexedObjectStore{`0},`0@) 69f5c2c2eaab1a1e6fa0b0bbd13aee3ea96c7bf106060cee9b297100c1a80fb2 -->
<!-- api: M:Inno.Core.Collections.IIndexedObjectQueryCondition`1.GetSet(Inno.Core.Collections.IndexedObjectStore{`0}) afea1d398c0ec350bf927142b18369a9178eb639b2ca451fbd5524d3c389fce9 -->
<!-- api: M:Inno.Core.Collections.IIndexedObjectQueryCondition`1.Validate(Inno.Core.Collections.IndexedObjectStore{`0},`0) ab509149fa2a6ba6e507593994f1180ac6ac8a0a4c60e61b2c9a705cd637c261 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-4df5dea48be3d909"></a>[`int Inno.Core.Collections.IIndexedObjectQueryCondition<T>.GetCandidateCount(Inno.Core.Collections.IndexedObjectStore<T> store)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/QueryCondition/IIndexedObjectQueryCondition.cs#L22) | Returns the estimated match count for this condition.<br>返回：Estimated number of matches; use System.Int32.MaxValue when unknown.<br>`store`：Store being queried. | — | — |
| <a id="api-ff3942920f5c3050"></a>[`System.Collections.Generic.HashSet<T>? Inno.Core.Collections.IIndexedObjectQueryCondition<T>.GetSet(Inno.Core.Collections.IndexedObjectStore<T> store)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/QueryCondition/IIndexedObjectQueryCondition.cs#L50) | Returns a candidate set for this condition if available.<br>返回：Candidate set, or null when not supported.<br>`store`：Store being queried. | — | — |
| <a id="api-63a5c96032430c97"></a>[`bool Inno.Core.Collections.IIndexedObjectQueryCondition<T>.TryGetSingle(Inno.Core.Collections.IndexedObjectStore<T> store, out T item)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/QueryCondition/IIndexedObjectQueryCondition.cs#L36) | Tries to get a single matching item when possible.<br>返回：True when a single item is available.<br>`store`：Store being queried.<br>`item`：The single matched item. | — | — |
| <a id="api-fe5150b80155d2c6"></a>[`bool Inno.Core.Collections.IIndexedObjectQueryCondition<T>.Validate(Inno.Core.Collections.IndexedObjectStore<T> store, T item)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/QueryCondition/IIndexedObjectQueryCondition.cs#L64) | Checks whether the item satisfies this condition.<br>返回：True if the item matches.<br>`store`：Store being queried.<br>`item`：Item to test. | — | — |
| <a id="api-c9582977304b8cc3"></a>[`public interface Inno.Core.Collections.IIndexedObjectQueryCondition<T> where T : class`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/QueryCondition/IIndexedObjectQueryCondition.cs#L11) | Defines one condition evaluated by an object-store query. | — | — |

### `Inno.Core.Collections.IndexedObjectEntry`1`

<!-- api: T:Inno.Core.Collections.IndexedObjectEntry`1 6e4f44889c0879420db4b3b9728e3964d84fae6c3313e043426c3bddc613e173 -->
<!-- api: P:Inno.Core.Collections.IndexedObjectEntry`1.isValid b6f36f8c3d06051663d9966f01e4b5e7f8dd8d971bd926b09e438156ec783ef0 -->
<!-- api: M:Inno.Core.Collections.IndexedObjectEntry`1.Set``1(Inno.Core.Collections.IndexedObjectKey{``0},``0) e4570d7cb26476209b67d9384ac82a9fd2826d907bdb92478558a166d2c67d9e -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-a4014bafd1d19ea3"></a>[`public Inno.Core.Collections.IndexedObjectEntry<T> Inno.Core.Collections.IndexedObjectEntry<T>.Set<TKey>(Inno.Core.Collections.IndexedObjectKey<TKey> key, TKey value) where TKey : notnull`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectEntry.cs#L45) | Sets a key value for the stored item.<br>返回：The same entry for chaining.<br>`key`：Key handle.<br>`value`：Key value. | — | — |
| <a id="api-f7f6c15d53d8dd2f"></a>[`public bool Inno.Core.Collections.IndexedObjectEntry<T>.isValid { get; }`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectEntry.cs#L27) | Returns true if the entry is still valid in its owning store. | — | — |
| <a id="api-4237d4a8c0ccdd99"></a>[`public readonly struct Inno.Core.Collections.IndexedObjectEntry<T> where T : class`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectEntry.cs#L11) | Entry wrapper for setting key values on a stored item. | — | — |

### `Inno.Core.Collections.IndexedObjectKey`1`

<!-- api: T:Inno.Core.Collections.IndexedObjectKey`1 c819304e7c275e94e296d72b238a4507f1570f66d6ae01a7a23b2ae8d08d10f8 -->
<!-- api: F:Inno.Core.Collections.IndexedObjectKey`1.name ce1c8dd97e3ec64f23097c8f5e100a76967eb8170d37d00e5b8795f8b7e15c89 -->
<!-- api: P:Inno.Core.Collections.IndexedObjectKey`1.isValid cec2a9881fc73f810184d814fd782fa9717e3295f0797c4cbb84194f914a0e1c -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-4adcc2a1f3bcb657"></a>[`public readonly string Inno.Core.Collections.IndexedObjectKey<TKey>.name`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectKey.cs#L40) | Key name | — | — |
| <a id="api-eab22119346c4fec"></a>[`public bool Inno.Core.Collections.IndexedObjectKey<TKey>.isValid { get; }`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectKey.cs#L45) | Returns true if the store key is still valid in its owning store. | — | — |
| <a id="api-9543fd3108749fe8"></a>[`public readonly struct Inno.Core.Collections.IndexedObjectKey<TKey>`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectKey.cs#L32) | Opaque handle used to query a key without exposing its implementation. | — | — |

### `Inno.Core.Collections.IndexedObjectKeyCondition`2`

<!-- api: T:Inno.Core.Collections.IndexedObjectKeyCondition`2 a21a8c069f85ba878fecf2b80e3784aecfb48946c6e25fa98f8e391d7f51948b -->
<!-- api: M:Inno.Core.Collections.IndexedObjectKeyCondition`2.#ctor(Inno.Core.Collections.IndexedObjectKey{`1},`1) 5968fa36cffedef81eb0136c515e410d6b81d2694d1280239a4cda269115f10e -->
<!-- api: M:Inno.Core.Collections.IndexedObjectKeyCondition`2.GetCandidateCount(Inno.Core.Collections.IndexedObjectStore{`0}) dc7751c4c949888027656b8a508e18774ff5916ce05549530135b58557928403 -->
<!-- api: M:Inno.Core.Collections.IndexedObjectKeyCondition`2.TryGetSingle(Inno.Core.Collections.IndexedObjectStore{`0},`0@) 3d3822514d0ade2aa2bd1ba53bf738d5d8fc4adc439e14bb8805d0cca3576306 -->
<!-- api: M:Inno.Core.Collections.IndexedObjectKeyCondition`2.GetSet(Inno.Core.Collections.IndexedObjectStore{`0}) 79e98d68cc6ac0399239597f21814558be92ae8f7da3f6c5104fae0a56682c92 -->
<!-- api: M:Inno.Core.Collections.IndexedObjectKeyCondition`2.Validate(Inno.Core.Collections.IndexedObjectStore{`0},`0) 8da00c1d80a1419ab22b467a9f5177702634e2cc0bcc478288748be7ba4cbccf -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-e27f12b9e4bb794b"></a>[`public Inno.Core.Collections.IndexedObjectKeyCondition<T, TKey>.IndexedObjectKeyCondition(Inno.Core.Collections.IndexedObjectKey<TKey> key, TKey value)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/QueryCondition/IndexedObjectKeyCondition.cs#L29) | Creates an indexed-key query condition.<br>`key`：The store key to query.<br>`value`：The required key value. | — | — |
| <a id="api-fe29e493dc792d90"></a>[`public int Inno.Core.Collections.IndexedObjectKeyCondition<T, TKey>.GetCandidateCount(Inno.Core.Collections.IndexedObjectStore<T> store)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/QueryCondition/IndexedObjectKeyCondition.cs#L47) | Gets a candidate count required by the implemented contract.<br>返回：The scalar result calculated from the supplied inputs.<br>`store`：The store consumed by get candidate count; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-aeb5c39649ce1636"></a>[`public System.Collections.Generic.HashSet<T>? Inno.Core.Collections.IndexedObjectKeyCondition<T, TKey>.GetSet(Inno.Core.Collections.IndexedObjectStore<T> store)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/QueryCondition/IndexedObjectKeyCondition.cs#L86) | Gets a set required by the implemented contract.<br>返回：The validated hash sett? that represents the completed operation.<br>`store`：The store consumed by get set; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-d4083443258a33d6"></a>[`public bool Inno.Core.Collections.IndexedObjectKeyCondition<T, TKey>.TryGetSingle(Inno.Core.Collections.IndexedObjectStore<T> store, out T item)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/QueryCondition/IndexedObjectKeyCondition.cs#L62) | Attempts to get single without changing state when the operation cannot complete.<br>返回：true when the operation succeeds or its condition is satisfied; otherwise, false.<br>`store`：The store consumed by try get single; ownership remains with the caller unless explicitly stated otherwise.<br>`item`：The stored item associated with the validated handle. | — | — |
| <a id="api-218048ea0de15cc6"></a>[`public bool Inno.Core.Collections.IndexedObjectKeyCondition<T, TKey>.Validate(Inno.Core.Collections.IndexedObjectStore<T> store, T item)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/QueryCondition/IndexedObjectKeyCondition.cs#L101) | Validates the supplied input and rejects state that cannot satisfy this contract.<br>返回：true when the operation succeeds or its condition is satisfied; otherwise, false.<br>`store`：The store consumed by validate; ownership remains with the caller unless explicitly stated otherwise.<br>`item`：The stored item associated with the validated handle. | — | — |
| <a id="api-4da0307df3e543d3"></a>[`public sealed class Inno.Core.Collections.IndexedObjectKeyCondition<T, TKey> where T : class where TKey : notnull : Inno.Core.Collections.IIndexedObjectQueryCondition<T> where T : class`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/QueryCondition/IndexedObjectKeyCondition.cs#L15) | Matches object-store items associated with one indexed key value. | — | — |

### `Inno.Core.Collections.IndexedObjectKeyFlags`

<!-- api: T:Inno.Core.Collections.IndexedObjectKeyFlags 290b2b8e1dd5d39031f351811a927797ce4f9f9c1a1c6a498c0e9f60b00ba72f -->
<!-- api: F:Inno.Core.Collections.IndexedObjectKeyFlags.Unordered caba9766146107606b167ddc121efdf8af2c758795232f4920d685f1f18afedc -->
<!-- api: F:Inno.Core.Collections.IndexedObjectKeyFlags.Ordered 0fe8012fb66552757d314a92b77bfe8aaed01849f65cb8ea570c7ae4faeadb30 -->
<!-- api: F:Inno.Core.Collections.IndexedObjectKeyFlags.Unique aaa27a1036311df42222f3b6ea1f33631e3ec394f735eef16333b760c9a58f43 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-0efdc181eed56586"></a>[`Inno.Core.Collections.IndexedObjectKeyFlags.Ordered = 2`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectKey.cs#L19) | Maintains key ordering using the supplied comparer. | — | — |
| <a id="api-6984ff7b843a6871"></a>[`Inno.Core.Collections.IndexedObjectKeyFlags.Unique = 4`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectKey.cs#L23) | Allows at most one item for each key value. | — | — |
| <a id="api-7d551bf0f39a901d"></a>[`Inno.Core.Collections.IndexedObjectKeyFlags.Unordered = 1`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectKey.cs#L15) | Uses hash-based lookup without key ordering. | — | — |
| <a id="api-57df28ccadb834dd"></a>[`public enum Inno.Core.Collections.IndexedObjectKeyFlags : int`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectKey.cs#L10) | Defines indexing and cardinality behavior for an object-store key. | — | — |

### `Inno.Core.Collections.IndexedObjectPredicateCondition`1`

<!-- api: T:Inno.Core.Collections.IndexedObjectPredicateCondition`1 b3f31d12a146fc7fb090c3003dd26a36e856491bedec7103c4b1e65fa8f915e2 -->
<!-- api: M:Inno.Core.Collections.IndexedObjectPredicateCondition`1.#ctor(System.Func{`0,System.Boolean}) 10b843e63fbac6c45c35d883f1b5b513ecbbafead232476cb2666ec1c40cd7e9 -->
<!-- api: M:Inno.Core.Collections.IndexedObjectPredicateCondition`1.GetCandidateCount(Inno.Core.Collections.IndexedObjectStore{`0}) 6338862e7b0e25c1cd7430b50ba8404cd1f07f7adf038d06939f55aeded7d1c9 -->
<!-- api: M:Inno.Core.Collections.IndexedObjectPredicateCondition`1.TryGetSingle(Inno.Core.Collections.IndexedObjectStore{`0},`0@) 5b8122c1aef012581bf19181c213fa09208b6b6cb6331f620e450913fa416a05 -->
<!-- api: M:Inno.Core.Collections.IndexedObjectPredicateCondition`1.GetSet(Inno.Core.Collections.IndexedObjectStore{`0}) dac6b3f8a3eb47929b6dbebbfebb3989731db5b19b67b4452a38e85a24645010 -->
<!-- api: M:Inno.Core.Collections.IndexedObjectPredicateCondition`1.Validate(Inno.Core.Collections.IndexedObjectStore{`0},`0) 20562da37d5c7bc48a26445d09ca8e8a485c7bdc31d4a27af7cb01c69ac774f3 -->
<!-- api: M:Inno.Core.Collections.IndexedObjectPredicateCondition`1.op_Implicit(System.Func{`0,System.Boolean})~Inno.Core.Collections.IndexedObjectPredicateCondition{`0} 2b03385c2de494c3cd97a9675d4503d29efb67776c92fbd9f44834a1bb354819 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-637656b57fc5f1b9"></a>[`public Inno.Core.Collections.IndexedObjectPredicateCondition<T>.IndexedObjectPredicateCondition(System.Func<T, bool> predicate)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/QueryCondition/IndexedObjectPredicateCondition.cs#L23) | Creates a predicate query condition.<br>`predicate`：The predicate evaluated for each candidate. | — | — |
| <a id="api-00b38a3100716c39"></a>[`public int Inno.Core.Collections.IndexedObjectPredicateCondition<T>.GetCandidateCount(Inno.Core.Collections.IndexedObjectStore<T> store)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/QueryCondition/IndexedObjectPredicateCondition.cs#L38) | Gets a candidate count required by the implemented contract.<br>返回：The scalar result calculated from the supplied inputs.<br>`store`：The store consumed by get candidate count; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-88e2a68be737e285"></a>[`public System.Collections.Generic.HashSet<T>? Inno.Core.Collections.IndexedObjectPredicateCondition<T>.GetSet(Inno.Core.Collections.IndexedObjectStore<T> store)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/QueryCondition/IndexedObjectPredicateCondition.cs#L71) | Gets a set required by the implemented contract.<br>返回：The validated hash sett? that represents the completed operation.<br>`store`：The store consumed by get set; ownership remains with the caller unless explicitly stated otherwise. | — | — |
| <a id="api-c5f5bcf3549d9f0c"></a>[`public static Inno.Core.Collections.IndexedObjectPredicateCondition<T>.implicit operator Inno.Core.Collections.IndexedObjectPredicateCondition<T>(System.Func<T, bool> predicate)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/QueryCondition/IndexedObjectPredicateCondition.cs#L100) | Creates a query condition from a predicate delegate.<br>返回：The validated indexed object predicate conditiont that represents the completed operation.<br>`predicate`：The predicate to wrap. | — | — |
| <a id="api-52652dda32d4537f"></a>[`public bool Inno.Core.Collections.IndexedObjectPredicateCondition<T>.TryGetSingle(Inno.Core.Collections.IndexedObjectStore<T> store, out T item)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/QueryCondition/IndexedObjectPredicateCondition.cs#L53) | Attempts to get single without changing state when the operation cannot complete.<br>返回：true when the operation succeeds or its condition is satisfied; otherwise, false.<br>`store`：The store consumed by try get single; ownership remains with the caller unless explicitly stated otherwise.<br>`item`：The stored item associated with the validated handle. | — | — |
| <a id="api-90492e242f8e392d"></a>[`public bool Inno.Core.Collections.IndexedObjectPredicateCondition<T>.Validate(Inno.Core.Collections.IndexedObjectStore<T> store, T item)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/QueryCondition/IndexedObjectPredicateCondition.cs#L86) | Validates the supplied input and rejects state that cannot satisfy this contract.<br>返回：true when the operation succeeds or its condition is satisfied; otherwise, false.<br>`store`：The store consumed by validate; ownership remains with the caller unless explicitly stated otherwise.<br>`item`：The stored item associated with the validated handle. | — | — |
| <a id="api-7bfaf1c0045f92e4"></a>[`public sealed class Inno.Core.Collections.IndexedObjectPredicateCondition<T> where T : class : Inno.Core.Collections.IIndexedObjectQueryCondition<T> where T : class`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/QueryCondition/IndexedObjectPredicateCondition.cs#L13) | Matches object-store items by evaluating a predicate. | — | — |

### `Inno.Core.Collections.IndexedObjectQuery`1`

<!-- api: T:Inno.Core.Collections.IndexedObjectQuery`1 02b87c304cc2acab07b0c84557360d8180d5e3273f2dca67e9b9bd075cbf0a6e -->
<!-- api: M:Inno.Core.Collections.IndexedObjectQuery`1.Where(Inno.Core.Collections.IIndexedObjectQueryCondition{`0}) e1bcda67dd39a8449296f2beec0c18b051d294c166bbd9093c4ecafce04635f8 -->
<!-- api: M:Inno.Core.Collections.IndexedObjectQuery`1.Where(System.Func{`0,System.Boolean}) bfbad0f23978f5c36e5a678e2a0e0665c765e339f0cef0c705f0eff52e24b8b9 -->
<!-- api: M:Inno.Core.Collections.IndexedObjectQuery`1.Find``1(Inno.Core.Collections.IndexedObjectKey{``0},``0) d8993dd8c29729564bcc45c601e27a3b79e4936ae57c2a03aca8022349a290f0 -->
<!-- api: M:Inno.Core.Collections.IndexedObjectQuery`1.OrderBy``1(Inno.Core.Collections.IndexedObjectKey{``0}) ef6fcb6ce6f280bdd5b46dbd98041c7f883192d72939c7cf40c4d627b467870e -->
<!-- api: M:Inno.Core.Collections.IndexedObjectQuery`1.GetFast f4e7ffc3f56754c340bebd1637fd42aa3a65190ddf80a07670b1c1484cbc1d6e -->
<!-- api: M:Inno.Core.Collections.IndexedObjectQuery`1.Get ac81a359597a5788e49d8abbd3059cd579031b1730c1fb0cd2785e99ad888e5e -->
<!-- api: M:Inno.Core.Collections.IndexedObjectQuery`1.First ae72959ecbf15f113c4c9c0ed904505595d647cc0471ad7600b5867a68584d66 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-b3435d2b6c261bd8"></a>[`public Inno.Core.Collections.IndexedObjectQuery<T> Inno.Core.Collections.IndexedObjectQuery<T>.Find<TKey>(Inno.Core.Collections.IndexedObjectKey<TKey> key, TKey value) where TKey : notnull`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectQuery.cs#L69) | Adds a key equality condition.<br>返回：The same query instance.<br>`key`：Key handle.<br>`value`：The concrete value read or transformed by this operation. | — | — |
| <a id="api-5de8d553dca57fff"></a>[`public T? Inno.Core.Collections.IndexedObjectQuery<T>.First()`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectQuery.cs#L122) | Executes the query and returns the first matching item or null.<br>返回：The first matching item or null. | — | — |
| <a id="api-611c1caeeeea71c2"></a>[`public System.Collections.Generic.IReadOnlyList<T> Inno.Core.Collections.IndexedObjectQuery<T>.Get()`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectQuery.cs#L114) | Executes the query and returns a stable snapshot.<br>返回：A snapshot list detached from subsequent store mutations. | — | — |
| <a id="api-ab8e2c1c4a6227e7"></a>[`public System.Collections.Generic.IEnumerable<T> Inno.Core.Collections.IndexedObjectQuery<T>.GetFast()`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectQuery.cs#L106) | Executes the query and returns a lazy fail-fast enumerable of results.<br>返回：Lazy fail-fast enumerable of matching items. | Enumeration is fail-fast and throws if the store is modified during iteration. | — |
| <a id="api-56fd4b68e786c392"></a>[`public Inno.Core.Collections.IndexedObjectQuery<T> Inno.Core.Collections.IndexedObjectQuery<T>.OrderBy<TKey>(Inno.Core.Collections.IndexedObjectKey<TKey> key) where TKey : notnull`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectQuery.cs#L86) | Orders results by a key marked with Inno.Core.Collections.IndexedObjectKeyFlags.Ordered.<br>返回：The same query instance.<br>`key`：Key handle. | — | — |
| <a id="api-09ef0a7b6aebd279"></a>[`public Inno.Core.Collections.IndexedObjectQuery<T> Inno.Core.Collections.IndexedObjectQuery<T>.Where(Inno.Core.Collections.IIndexedObjectQueryCondition<T> condition)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectQuery.cs#L34) | Adds a custom condition implementation.<br>返回：The same query instance.<br>`condition`：The condition to append to this query. | — | — |
| <a id="api-0a5afff56b8a18c7"></a>[`public Inno.Core.Collections.IndexedObjectQuery<T> Inno.Core.Collections.IndexedObjectQuery<T>.Where(System.Func<T, bool> predicate)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectQuery.cs#L52) | Adds a predicate condition evaluated against candidate items.<br>返回：The same query instance.<br>`predicate`：Predicate to evaluate. | — | — |
| <a id="api-b478432c85992b55"></a>[`public sealed class Inno.Core.Collections.IndexedObjectQuery<T> where T : class`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectQuery.cs#L12) | Query builder over store keys. | — | — |

### `Inno.Core.Collections.IndexedObjectStore`1`

<!-- api: T:Inno.Core.Collections.IndexedObjectStore`1 20ad0018fa6e44bac273a69eec50310650c0fab61ba5389c250d6858659f8421 -->
<!-- api: M:Inno.Core.Collections.IndexedObjectStore`1.#ctor 292186b973501450736f97a4041a5b1babe4ade501f58e42a752a0bc201cafb0 -->
<!-- api: P:Inno.Core.Collections.IndexedObjectStore`1.count b322fb5362b2a33b7f72c92fc6b0f08d8ab54df314ca4516936545317cd3a9cb -->
<!-- api: M:Inno.Core.Collections.IndexedObjectStore`1.Clear 48191cee3678f6b4a8eb70b3260cb83c9c72b72df65523128163a8ce7a130415 -->
<!-- api: M:Inno.Core.Collections.IndexedObjectStore`1.RemoveAll 75ab225ff3fa0fb38cab61bfff98a4327941ffd3d8843a03e913f19a0b502b81 -->
<!-- api: M:Inno.Core.Collections.IndexedObjectStore`1.Add(`0) 5a966c21da6924d1b30aade22f52a3f316168f5ab0c069a12bde5388eb955a82 -->
<!-- api: M:Inno.Core.Collections.IndexedObjectStore`1.Remove(`0) 756b88b5bc6e080c9c101c6dbd966f4a42e15a4bea16164b0b2dd1032a9ef035 -->
<!-- api: M:Inno.Core.Collections.IndexedObjectStore`1.DefineKey``1(System.String,Inno.Core.Collections.IndexedObjectKeyFlags,System.Collections.Generic.IComparer{``0}) 1611e9253fcdbdc4d2d9e01e9a92d2cb71bb73ccda0af26aa58b73a9faa1bb04 -->
<!-- api: M:Inno.Core.Collections.IndexedObjectStore`1.RemoveKey``1(Inno.Core.Collections.IndexedObjectKey{``0}) d65d78a5f5a78d9702d27657c82cf2d8120319b370cf7b5c41c69a2eb927e750 -->
<!-- api: M:Inno.Core.Collections.IndexedObjectStore`1.TryGetKey``1(System.String,Inno.Core.Collections.IndexedObjectKey{``0}@) 2b53a3dd7726b6352b03736a9e53ce42bda1e2e0c7387a174ecdb931024d253d -->
<!-- api: M:Inno.Core.Collections.IndexedObjectStore`1.GetAllKeys a268d342f31016e6d1adbb44bd176fd22498b46f555c4fbd547af79a0b627b57 -->
<!-- api: M:Inno.Core.Collections.IndexedObjectStore`1.FindFast``1(Inno.Core.Collections.IndexedObjectKey{``0},``0) c70512787047aad418f422f157010936dcc8dc5ee7419537758520ec2a86f92e -->
<!-- api: M:Inno.Core.Collections.IndexedObjectStore`1.Find``1(Inno.Core.Collections.IndexedObjectKey{``0},``0) acec9fd26af1e2ebe117f3c0ad18ced4c02630f6c49513669c8bbdb30861bdbb -->
<!-- api: M:Inno.Core.Collections.IndexedObjectStore`1.First``1(Inno.Core.Collections.IndexedObjectKey{``0},``0) b8907265673ae6c05d79cbad0ab68659aca31f86772f610b9f88e13e7ad18b32 -->
<!-- api: M:Inno.Core.Collections.IndexedObjectStore`1.All c5407e066bc5e7b9b13ae2ee251b2f600103313af79d365df51c112b0a600a80 -->
<!-- api: M:Inno.Core.Collections.IndexedObjectStore`1.AllFast 7b0abb1e8a4e0be5b67d7b431101c98a1cba83133b74fec4cb99bf42ac18998b -->
<!-- api: M:Inno.Core.Collections.IndexedObjectStore`1.Query 2dcc3e767635ff26ac5658062665e7ffa594cc714163565f5fc103612b8d5a58 -->

| 声明 | 行为与结果 | 所有权 / 调用约束 | 失败或限制 |
| --- | --- | --- | --- |
| <a id="api-79c52ce80c1b68a0"></a>[`public Inno.Core.Collections.IndexedObjectStore<T>.IndexedObjectStore()`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectStore.cs#L56) | Creates an empty object store. | — | — |
| <a id="api-7edcbc66988d43a7"></a>[`public Inno.Core.Collections.IndexedObjectEntry<T> Inno.Core.Collections.IndexedObjectStore<T>.Add(T item)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectStore.cs#L151) | Adds an item to the store without indexing.<br>返回：The validated indexed object entryt that represents the completed operation.<br>`item`：The item to add. | — | — |
| <a id="api-34cc295b955b2d4a"></a>[`public System.Collections.Generic.IReadOnlyList<T> Inno.Core.Collections.IndexedObjectStore<T>.All()`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectStore.Queries.cs#L165) | Returns all stored items as a stable snapshot.<br>返回：A snapshot list of all stored items. | The returned list is detached from subsequent store mutations. | — |
| <a id="api-5d9fb9bbeb306553"></a>[`public System.Collections.Generic.IEnumerable<T> Inno.Core.Collections.IndexedObjectStore<T>.AllFast()`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectStore.Queries.cs#L189) | Returns all stored items as a lazy fail-fast enumerable.<br>返回：Lazy fail-fast enumerable of all stored items. | Enumeration throws if the store is modified during iteration. | — |
| <a id="api-7a21f573a6bba318"></a>[`public void Inno.Core.Collections.IndexedObjectStore<T>.Clear()`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectStore.cs#L86) | Clears all stored items and removes all defined keys. | After calling this, all key handles are invalid and must be redefined via Inno.Core.Collections.IndexedObjectStore`1.DefineKey``1(System.String,Inno.Core.Collections.IndexedObjectKeyFlags,System.Collections.Generic.IComparer{``0}). | — |
| <a id="api-f4ec145888f17bd5"></a>[`public Inno.Core.Collections.IndexedObjectKey<TKey> Inno.Core.Collections.IndexedObjectStore<T>.DefineKey<TKey>(string name, Inno.Core.Collections.IndexedObjectKeyFlags flags = Inno.Core.Collections.IndexedObjectKeyFlags.Unordered, System.Collections.Generic.IComparer<TKey>? orderComparer = null) where TKey : notnull`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectStore.Keys.cs#L29) | Defines a query key over stored items.<br>返回：The created key handle.<br>`name`：Unique key name.<br>`flags`：Key behavior flags.<br>`orderComparer`：Optional order comparer. Required when Inno.Core.Collections.IndexedObjectKeyFlags.Ordered is set. | — | — |
| <a id="api-ae51ed62af28e24a"></a>[`public System.Collections.Generic.IReadOnlyList<T> Inno.Core.Collections.IndexedObjectStore<T>.Find<TKey>(Inno.Core.Collections.IndexedObjectKey<TKey> key, TKey value) where TKey : notnull`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectStore.Queries.cs#L56) | Finds items by key and returns a stable snapshot.<br>返回：A stable snapshot list of matching items.<br>`key`：The key handle to query.<br>`value`：The key value to look up. | The returned list is detached from subsequent store mutations. | — |
| <a id="api-9b836b525c526efb"></a>[`public System.Collections.Generic.IEnumerable<T> Inno.Core.Collections.IndexedObjectStore<T>.FindFast<TKey>(Inno.Core.Collections.IndexedObjectKey<TKey> key, TKey value) where TKey : notnull`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectStore.Queries.cs#L29) | Finds items by key and returns a lazy fail-fast enumerable.<br>返回：Lazy fail-fast enumerable of matching items.<br>`key`：The key handle to query.<br>`value`：The key value to look up. | Enumeration throws if the store is modified during iteration. | — |
| <a id="api-de6fbfdb84fc602e"></a>[`public T? Inno.Core.Collections.IndexedObjectStore<T>.First<TKey>(Inno.Core.Collections.IndexedObjectKey<TKey> key, TKey value) where TKey : notnull`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectStore.Queries.cs#L138) | Returns the first item by key or null if none exists.<br>返回：The first matching item or null.<br>`key`：The key handle to query.<br>`value`：The key value to look up. | — | — |
| <a id="api-17f44afdd5942fae"></a>[`public System.Collections.Generic.IEnumerable<string> Inno.Core.Collections.IndexedObjectStore<T>.GetAllKeys()`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectStore.Keys.cs#L162) | Gets all defined key names as a lazy enumerable.<br>返回：Lazy enumerable of key names. | Enumeration is fail-fast and throws if the store is modified during iteration. | — |
| <a id="api-c55927fba21e5ebb"></a>[`public Inno.Core.Collections.IndexedObjectQuery<T> Inno.Core.Collections.IndexedObjectStore<T>.Query()`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectStore.Queries.cs#L205) | Starts a query over stored items.<br>返回：A query builder. | — | — |
| <a id="api-66aeea7790bf00d7"></a>[`public bool Inno.Core.Collections.IndexedObjectStore<T>.Remove(T item)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectStore.cs#L188) | Removes an item from the store and all keys.<br>返回：True if removed.<br>`item`：The item to remove. | — | — |
| <a id="api-a6aa9e40d3663b18"></a>[`public void Inno.Core.Collections.IndexedObjectStore<T>.RemoveAll()`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectStore.cs#L115) | Clears all stored items but keeps defined keys. | Keys remain valid but all indexed values are removed. | — |
| <a id="api-72c90343d2b1bde0"></a>[`public bool Inno.Core.Collections.IndexedObjectStore<T>.RemoveKey<TKey>(Inno.Core.Collections.IndexedObjectKey<TKey> key)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectStore.Keys.cs#L73) | Removes a key previously defined on the store.<br>返回：True if removed.<br>`key`：The key handle to remove. | — | — |
| <a id="api-a563db9f78df9cda"></a>[`public bool Inno.Core.Collections.IndexedObjectStore<T>.TryGetKey<TKey>(string name, out Inno.Core.Collections.IndexedObjectKey<TKey> key) where TKey : notnull`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectStore.Keys.cs#L106) | Gets a previously defined key by name.<br>返回：True if found and type matches.<br>`name`：Key name.<br>`key`：The resolved key handle. | — | — |
| <a id="api-52c6bc070d4779dc"></a>[`public int Inno.Core.Collections.IndexedObjectStore<T>.count { get; }`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectStore.cs#L64) | Number of stored items. | — | — |
| <a id="api-84ed5bf6909ff544"></a>[`public sealed class Inno.Core.Collections.IndexedObjectStore<T> where T : class : Inno.Core.Collections.IIndexedObjectStore`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectStore.cs#L37) | Thread-safe object store with optional query keys over stored items. | — | — |

## 项目依赖

- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
