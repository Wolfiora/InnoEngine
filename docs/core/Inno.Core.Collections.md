# Inno.Core.Collections

[上一页：Mathematics](Inno.Core.Mathematics.md) · [Core 索引](README.md) · [前往 Assets](../assets/README.md)

Storage 提供两个独立的数据结构：线程安全、方向明确的 `DependencyGraph<TKey>`，以及支持多索引与查询优化的 `IndexedObjectStore<T>`。

## DependencyGraph&lt;TKey&gt;

边 `node -> dependency` 表示“node 依赖 dependency”。Graph 接受环；只有要求 DAG 的操作会显式报告环。

### 构造与状态

```csharp
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
graph.AddDependency("Scene", "Texture");
graph.AddDependency("Material", "Texture");
IReadOnlyList<string> order = graph.TopologicalSort();
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
IndexedObjectStore<Entity> store = new();
IndexedObjectKey<Guid> byId = store.DefineKey<Guid>("id", IndexedObjectKeyFlags.Unique);
IndexedObjectKey<int> byOrder = store.DefineKey<int>(
    "order",
    IndexedObjectKeyFlags.Ordered,
    Comparer<int>.Default);

store.Add(entity)
    .Set(byId, entity.id)
    .Set(byOrder, entity.order);

Entity? found = store.First(byId, entity.id);
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

只列当前源码的 public/protected 表面；内部实现不作为稳定 API。参数、返回、失败和 owner 以英文 XML 为准。

### `Inno.Core.Collections.DependencyGraph<TKey>`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Collections.DependencyGraph<TKey>`](../../src/foundation/core/Inno.Core.Collections/DependencyGraph/DependencyGraph.cs#L19) | Stores a directed dependency graph and provides deterministic dependency queries. |
| [`Inno.Core.Collections.DependencyGraph<TKey>.DependencyGraph(System.Collections.Generic.IEqualityComparer<TKey>? equalityComparer = null, System.Collections.Generic.IComparer<TKey>? orderingComparer = null)`](../../src/foundation/core/Inno.Core.Collections/DependencyGraph/DependencyGraph.cs#L83) | Creates an empty dependency graph. |
| [`System.Collections.Generic.IReadOnlyList<System.Collections.Generic.IReadOnlyList<TKey>> Inno.Core.Collections.DependencyGraph<TKey>.GetStronglyConnectedComponents()`](../../src/foundation/core/Inno.Core.Collections/DependencyGraph/DependencyGraph.cs#L501) | Gets every strongly connected component in deterministic order. |
| [`System.Collections.Generic.IReadOnlyList<TKey> Inno.Core.Collections.DependencyGraph<TKey>.GetDependencies(TKey node, bool recursive = false)`](../../src/foundation/core/Inno.Core.Collections/DependencyGraph/DependencyGraph.cs#L350) | Gets direct or transitive dependencies of a node. |
| [`System.Collections.Generic.IReadOnlyList<TKey> Inno.Core.Collections.DependencyGraph<TKey>.GetDependents(TKey node, bool recursive = false)`](../../src/foundation/core/Inno.Core.Collections/DependencyGraph/DependencyGraph.cs#L368) | Gets direct or transitive dependents of a node. |
| [`System.Collections.Generic.IReadOnlyList<TKey> Inno.Core.Collections.DependencyGraph<TKey>.TopologicalSort()`](../../src/foundation/core/Inno.Core.Collections/DependencyGraph/DependencyGraph.cs#L432) | Returns an order in which every dependency precedes its dependents. |
| [`bool Inno.Core.Collections.DependencyGraph<TKey>.AddDependency(TKey node, TKey dependency)`](../../src/foundation/core/Inno.Core.Collections/DependencyGraph/DependencyGraph.cs#L231) | Adds an edge indicating that one node depends on another. |
| [`bool Inno.Core.Collections.DependencyGraph<TKey>.AddNode(TKey node)`](../../src/foundation/core/Inno.Core.Collections/DependencyGraph/DependencyGraph.cs#L164) | Adds a node when it does not already exist. |
| [`bool Inno.Core.Collections.DependencyGraph<TKey>.ContainsNode(TKey node)`](../../src/foundation/core/Inno.Core.Collections/DependencyGraph/DependencyGraph.cs#L142) | Determines whether a node exists. |
| [`bool Inno.Core.Collections.DependencyGraph<TKey>.DependsOn(TKey node, TKey dependency, bool recursive = false)`](../../src/foundation/core/Inno.Core.Collections/DependencyGraph/DependencyGraph.cs#L389) | Determines whether one node directly or transitively depends on another. |
| [`bool Inno.Core.Collections.DependencyGraph<TKey>.RemoveDependency(TKey node, TKey dependency)`](../../src/foundation/core/Inno.Core.Collections/DependencyGraph/DependencyGraph.cs#L265) | Removes a dependency edge. |
| [`bool Inno.Core.Collections.DependencyGraph<TKey>.RemoveNode(TKey node)`](../../src/foundation/core/Inno.Core.Collections/DependencyGraph/DependencyGraph.cs#L190) | Removes a node and every incoming and outgoing edge connected to it. |
| [`bool Inno.Core.Collections.DependencyGraph<TKey>.TryFindCycle(out System.Collections.Generic.IReadOnlyList<TKey> cycle)`](../../src/foundation/core/Inno.Core.Collections/DependencyGraph/DependencyGraph.cs#L481) | Tries to find one complete cycle in the graph. |
| [`int Inno.Core.Collections.DependencyGraph<TKey>.count`](../../src/foundation/core/Inno.Core.Collections/DependencyGraph/DependencyGraph.cs#L95) | Gets the number of nodes in the graph. |
| [`long Inno.Core.Collections.DependencyGraph<TKey>.version`](../../src/foundation/core/Inno.Core.Collections/DependencyGraph/DependencyGraph.cs#L117) | Gets the structural version of the graph. |
| [`void Inno.Core.Collections.DependencyGraph<TKey>.Clear()`](../../src/foundation/core/Inno.Core.Collections/DependencyGraph/DependencyGraph.cs#L572) | Removes every node and edge. |
| [`void Inno.Core.Collections.DependencyGraph<TKey>.ReplaceDependencies(TKey node, System.Collections.Generic.IEnumerable<TKey> dependencies)`](../../src/foundation/core/Inno.Core.Collections/DependencyGraph/DependencyGraph.cs#L302) | Atomically replaces every direct dependency of a node. |

### `Inno.Core.Collections.IIndexedObjectQueryCondition<T>`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Collections.IIndexedObjectQueryCondition<T>`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/QueryCondition/IIndexedObjectQueryCondition.cs#L11) | Defines one condition evaluated by an object-store query. |
| [`System.Collections.Generic.HashSet<T>? Inno.Core.Collections.IIndexedObjectQueryCondition<T>.GetSet(Inno.Core.Collections.IndexedObjectStore<T> store)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/QueryCondition/IIndexedObjectQueryCondition.cs#L50) | Returns a candidate set for this condition if available. |
| [`bool Inno.Core.Collections.IIndexedObjectQueryCondition<T>.TryGetSingle(Inno.Core.Collections.IndexedObjectStore<T> store, out T item)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/QueryCondition/IIndexedObjectQueryCondition.cs#L36) | Tries to get a single matching item when possible. |
| [`bool Inno.Core.Collections.IIndexedObjectQueryCondition<T>.Validate(Inno.Core.Collections.IndexedObjectStore<T> store, T item)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/QueryCondition/IIndexedObjectQueryCondition.cs#L64) | Checks whether the item satisfies this condition. |
| [`int Inno.Core.Collections.IIndexedObjectQueryCondition<T>.GetCandidateCount(Inno.Core.Collections.IndexedObjectStore<T> store)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/QueryCondition/IIndexedObjectQueryCondition.cs#L22) | Returns the estimated match count for this condition. |

### `Inno.Core.Collections.IndexedObjectEntry<T>`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Collections.IndexedObjectEntry<T>`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectEntry.cs#L11) | Entry wrapper for setting key values on a stored item. |
| [`Inno.Core.Collections.IndexedObjectEntry<T> Inno.Core.Collections.IndexedObjectEntry<T>.Set<TKey>(Inno.Core.Collections.IndexedObjectKey<TKey> key, TKey value)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectEntry.cs#L44) | Sets a key value for the stored item. |
| [`bool Inno.Core.Collections.IndexedObjectEntry<T>.isValid`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectEntry.cs#L27) | Returns true if the entry is still valid in its owning store. |

### `Inno.Core.Collections.IndexedObjectKey<TKey>`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Collections.IndexedObjectKey<TKey>`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectKey.cs#L32) | Opaque handle used to query a key without exposing its implementation. |
| [`bool Inno.Core.Collections.IndexedObjectKey<TKey>.isValid`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectKey.cs#L45) | Returns true if the store key is still valid in its owning store. |
| [`readonly string Inno.Core.Collections.IndexedObjectKey<TKey>.name`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectKey.cs#L40) | Key name |

### `Inno.Core.Collections.IndexedObjectKeyCondition<T, TKey>`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Collections.IndexedObjectKeyCondition<T, TKey>`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/QueryCondition/IndexedObjectKeyCondition.cs#L15) | Matches object-store items associated with one indexed key value. |
| [`Inno.Core.Collections.IndexedObjectKeyCondition<T, TKey>.IndexedObjectKeyCondition(Inno.Core.Collections.IndexedObjectKey<TKey> key, TKey value)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/QueryCondition/IndexedObjectKeyCondition.cs#L29) | Creates an indexed-key query condition. |
| [`System.Collections.Generic.HashSet<T>? Inno.Core.Collections.IndexedObjectKeyCondition<T, TKey>.GetSet(Inno.Core.Collections.IndexedObjectStore<T> store)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/QueryCondition/IndexedObjectKeyCondition.cs#L85) | Gets a set required by the implemented contract. |
| [`bool Inno.Core.Collections.IndexedObjectKeyCondition<T, TKey>.TryGetSingle(Inno.Core.Collections.IndexedObjectStore<T> store, out T item)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/QueryCondition/IndexedObjectKeyCondition.cs#L61) | Attempts to get single without changing state when the operation cannot complete. |
| [`bool Inno.Core.Collections.IndexedObjectKeyCondition<T, TKey>.Validate(Inno.Core.Collections.IndexedObjectStore<T> store, T item)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/QueryCondition/IndexedObjectKeyCondition.cs#L100) | Validates the supplied input and rejects state that cannot satisfy this contract. |
| [`int Inno.Core.Collections.IndexedObjectKeyCondition<T, TKey>.GetCandidateCount(Inno.Core.Collections.IndexedObjectStore<T> store)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/QueryCondition/IndexedObjectKeyCondition.cs#L46) | Gets a candidate count required by the implemented contract. |

### `Inno.Core.Collections.IndexedObjectKeyFlags`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Collections.IndexedObjectKeyFlags`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectKey.cs#L9) | Defines indexing and cardinality behavior for an object-store key. |
| [`Inno.Core.Collections.IndexedObjectKeyFlags.Ordered`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectKey.cs#L19) | Maintains key ordering using the supplied comparer. |
| [`Inno.Core.Collections.IndexedObjectKeyFlags.Unique`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectKey.cs#L23) | Allows at most one item for each key value. |
| [`Inno.Core.Collections.IndexedObjectKeyFlags.Unordered`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectKey.cs#L15) | Uses hash-based lookup without key ordering. |

### `Inno.Core.Collections.IndexedObjectPredicateCondition<T>`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Collections.IndexedObjectPredicateCondition<T>`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/QueryCondition/IndexedObjectPredicateCondition.cs#L13) | Matches object-store items by evaluating a predicate. |
| [`Inno.Core.Collections.IndexedObjectPredicateCondition<T>.IndexedObjectPredicateCondition(System.Func<T, bool> predicate)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/QueryCondition/IndexedObjectPredicateCondition.cs#L23) | Creates a predicate query condition. |
| [`System.Collections.Generic.HashSet<T>? Inno.Core.Collections.IndexedObjectPredicateCondition<T>.GetSet(Inno.Core.Collections.IndexedObjectStore<T> store)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/QueryCondition/IndexedObjectPredicateCondition.cs#L70) | Gets a set required by the implemented contract. |
| [`bool Inno.Core.Collections.IndexedObjectPredicateCondition<T>.TryGetSingle(Inno.Core.Collections.IndexedObjectStore<T> store, out T item)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/QueryCondition/IndexedObjectPredicateCondition.cs#L52) | Attempts to get single without changing state when the operation cannot complete. |
| [`bool Inno.Core.Collections.IndexedObjectPredicateCondition<T>.Validate(Inno.Core.Collections.IndexedObjectStore<T> store, T item)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/QueryCondition/IndexedObjectPredicateCondition.cs#L85) | Validates the supplied input and rejects state that cannot satisfy this contract. |
| [`int Inno.Core.Collections.IndexedObjectPredicateCondition<T>.GetCandidateCount(Inno.Core.Collections.IndexedObjectStore<T> store)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/QueryCondition/IndexedObjectPredicateCondition.cs#L37) | Gets a candidate count required by the implemented contract. |
| [`static Inno.Core.Collections.IndexedObjectPredicateCondition<T>.implicit operator Inno.Core.Collections.IndexedObjectPredicateCondition<T>(System.Func<T, bool> predicate)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/QueryCondition/IndexedObjectPredicateCondition.cs#L100) | Creates a query condition from a predicate delegate. |

### `Inno.Core.Collections.IndexedObjectQuery<T>`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Collections.IndexedObjectQuery<T>`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectQuery.cs#L12) | Query builder over store keys. |
| [`Inno.Core.Collections.IndexedObjectQuery<T> Inno.Core.Collections.IndexedObjectQuery<T>.Find<TKey>(Inno.Core.Collections.IndexedObjectKey<TKey> key, TKey value)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectQuery.cs#L69) | Adds a key equality condition. |
| [`Inno.Core.Collections.IndexedObjectQuery<T> Inno.Core.Collections.IndexedObjectQuery<T>.OrderBy<TKey>(Inno.Core.Collections.IndexedObjectKey<TKey> key)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectQuery.cs#L86) | Orders results by a key marked with . |
| [`Inno.Core.Collections.IndexedObjectQuery<T> Inno.Core.Collections.IndexedObjectQuery<T>.Where(Inno.Core.Collections.IIndexedObjectQueryCondition<T> condition)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectQuery.cs#L34) | Adds a custom condition implementation. |
| [`Inno.Core.Collections.IndexedObjectQuery<T> Inno.Core.Collections.IndexedObjectQuery<T>.Where(System.Func<T, bool> predicate)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectQuery.cs#L52) | Adds a predicate condition evaluated against candidate items. |
| [`System.Collections.Generic.IEnumerable<T> Inno.Core.Collections.IndexedObjectQuery<T>.GetFast()`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectQuery.cs#L106) | Executes the query and returns a lazy fail-fast enumerable of results. |
| [`System.Collections.Generic.IReadOnlyList<T> Inno.Core.Collections.IndexedObjectQuery<T>.Get()`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectQuery.cs#L114) | Executes the query and returns a stable snapshot. |
| [`T? Inno.Core.Collections.IndexedObjectQuery<T>.First()`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectQuery.cs#L122) | Executes the query and returns the first matching item or null. |

### `Inno.Core.Collections.IndexedObjectStore<T>`

| 当前声明 | 行为 |
| --- | --- |
| [`Inno.Core.Collections.IndexedObjectEntry<T> Inno.Core.Collections.IndexedObjectStore<T>.Add(T item)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectStore.cs#L151) | Adds an item to the store without indexing. |
| [`Inno.Core.Collections.IndexedObjectKey<TKey> Inno.Core.Collections.IndexedObjectStore<T>.DefineKey<TKey>(string name, Inno.Core.Collections.IndexedObjectKeyFlags flags = Inno.Core.Collections.IndexedObjectKeyFlags.Unordered, System.Collections.Generic.IComparer<TKey>? orderComparer = null)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectStore.Keys.cs#L29) | Defines a query key over stored items. |
| [`Inno.Core.Collections.IndexedObjectQuery<T> Inno.Core.Collections.IndexedObjectStore<T>.Query()`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectStore.Queries.cs#L205) | Starts a query over stored items. |
| [`Inno.Core.Collections.IndexedObjectStore<T>`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectStore.cs#L37) | Thread-safe object store with optional query keys over stored items. |
| [`Inno.Core.Collections.IndexedObjectStore<T>.IndexedObjectStore()`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectStore.cs#L56) | Creates an empty object store. |
| [`System.Collections.Generic.IEnumerable<T> Inno.Core.Collections.IndexedObjectStore<T>.AllFast()`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectStore.Queries.cs#L189) | Returns all stored items as a lazy fail-fast enumerable. |
| [`System.Collections.Generic.IEnumerable<T> Inno.Core.Collections.IndexedObjectStore<T>.FindFast<TKey>(Inno.Core.Collections.IndexedObjectKey<TKey> key, TKey value)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectStore.Queries.cs#L29) | Finds items by key and returns a lazy fail-fast enumerable. |
| [`System.Collections.Generic.IEnumerable<string> Inno.Core.Collections.IndexedObjectStore<T>.GetAllKeys()`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectStore.Keys.cs#L162) | Gets all defined key names as a lazy enumerable. |
| [`System.Collections.Generic.IReadOnlyList<T> Inno.Core.Collections.IndexedObjectStore<T>.All()`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectStore.Queries.cs#L165) | Returns all stored items as a stable snapshot. |
| [`System.Collections.Generic.IReadOnlyList<T> Inno.Core.Collections.IndexedObjectStore<T>.Find<TKey>(Inno.Core.Collections.IndexedObjectKey<TKey> key, TKey value)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectStore.Queries.cs#L56) | Finds items by key and returns a stable snapshot. |
| [`T? Inno.Core.Collections.IndexedObjectStore<T>.First<TKey>(Inno.Core.Collections.IndexedObjectKey<TKey> key, TKey value)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectStore.Queries.cs#L138) | Returns the first item by key or null if none exists. |
| [`bool Inno.Core.Collections.IndexedObjectStore<T>.Remove(T item)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectStore.cs#L188) | Removes an item from the store and all keys. |
| [`bool Inno.Core.Collections.IndexedObjectStore<T>.RemoveKey<TKey>(Inno.Core.Collections.IndexedObjectKey<TKey> key)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectStore.Keys.cs#L73) | Removes a key previously defined on the store. |
| [`bool Inno.Core.Collections.IndexedObjectStore<T>.TryGetKey<TKey>(string name, out Inno.Core.Collections.IndexedObjectKey<TKey> key)`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectStore.Keys.cs#L106) | Gets a previously defined key by name. |
| [`int Inno.Core.Collections.IndexedObjectStore<T>.count`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectStore.cs#L64) | Number of stored items. |
| [`void Inno.Core.Collections.IndexedObjectStore<T>.Clear()`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectStore.cs#L86) | Clears all stored items and removes all defined keys. |
| [`void Inno.Core.Collections.IndexedObjectStore<T>.RemoveAll()`](../../src/foundation/core/Inno.Core.Collections/IndexedObjectStore/IndexedObjectStore.cs#L115) | Clears all stored items but keeps defined keys. |

## 项目依赖

- [Inno.Extensibility.Catalogs](../extensibility/Inno.Extensibility.Catalogs.md)：公开引用边界由实际签名核对。
