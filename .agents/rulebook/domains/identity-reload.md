# domains/identity-reload

按完整规则单元加载；路由与来源见 [规则登记](../coverage.json)。

<!-- rule-unit: identity-reload -->
<a id="identity-reload"></a>
## 20. Identity、Missing、引用与热重载强制边界
- 跨 UI、callback、queue、frame、Scene、Asset、Scripting 或 Plugin generation 边界定位 live object 时必须经过 `Inno.Core.Identity`。当前 domain 内瞬时解析使用 `Identity.runtimeId`，持久化、History 和跨代恢复只保存 `Identity.persistentId`；runtime ID 绝不进入 Scene、Prefab、Asset metadata、Settings、Plugin manifest 或 History。
- Stable Type ID、Plugin/Feature/Importer 等 semantic ID、Artifact key 和 generation handle 各自保持独立语义，不得与 object Identity 混用。领域二级索引可以把 path/semantic key 映射到 persistent ID 或 immutable record，但不能成为跨 generation live object 的第二权威表。
- Editor ImGui DragDrop 的 native payload 数据必须是源 identity 的 runtime ID；禁止使用随机 token 回查 managed source object。Preview 与 Delivery 都必须通过正确的 `IdentityAllocator` 重新解析并校验 generation；Drop 落盘或记录 History 时转换为 persistent ID。
- Asset、Script Type、Plugin、Importer、Component/System、Graph node、Settings contribution 或 extension 暂时不可用时必须保留 Missing state：原 persistent ID、Stable ID、结构位置、property bytes、依赖和中立 extension state均不得丢失。Missing 与 null/Unassigned 必须在 API 和 UI 中严格区分。
- 同一 identity/type/plugin 恢复后必须通过统一 candidate/recovery transaction 在 owner-thread safe point 原子重建；失败保留原 Missing 与诊断。不得让 Assets、Scene、Scripting、Plugins、Graph 或各 Panel 分别实现互不兼容的 resolver、placeholder 生命周期或恢复事务。
- reload-safe Undo/Redo 只能保存 stable protocol kind、persistent ID、Stable ID、结构值和中立 bytes；不得捕获 runtime object、`Type`、delegate、extension instance 或 ALC。暂时 Missing 只能成为可恢复 barrier，不能丢弃、跳过或移动栈指针；恢复后原操作应自动重新可用。
- 所有可能包含 Asset/Object reference 的序列化必须从 owner 取得完整 `SerializationContext`。除经类型与测试证明完全 context-free 的纯值图外，业务代码禁止直接从 `SerializationContext.empty` 临时拼接 resolver；缺少 required resolver 必须在 composition/startup 失败，不能延迟到 Play、Undo 或 recovery 才爆出。
- Scripting、Plugin、Asset type、Serializer 和 extension generation 必须进入统一 candidate transaction。旧 collectible ALC 的成功退休必须执行 Full GC → `GC.WaitForPendingFinalizers()` → Full GC，并由弱 monitor 确认全部 context 不可达；在此之前 reload 不得报告 Success，也不得开始新的 reload、Play、Build 或 Export。
- unload verification 的 timeout 只能触发明确异常和 `Faulted`，绝不能清空仍 Pending 的 monitor 后继续。失败必须列出 module/domain/scope/generation；Faulted 进程禁止继续 generation transaction，需完整重启 Host。static/event/task/thread/AsyncLocal/GCHandle/native callback/Editor transient state 等旧 generation 强引用必须被测试覆盖。
- 完整规范、目标 API、当前差距和测试矩阵见 `docs/architecture/identity-and-reload.md`；修改 Identity、Scripting、Plugins、Assets、Scene、Editor Interactions/History 或任何 collectible extension 时必须同步核对此页。
<!-- /rule-unit: identity-reload -->

<!-- rule-unit: identity-standard-12 -->
<a id="identity-standard-12"></a>
## 十二、Architecture Tool 强制检查

在可以稳定、低误报地实现后，Architecture Tool 至少应检查：

1. Scene、Asset、Plugin authoring 和 Editor addressable model 必须使用 `IdentityObject` 或声明的 identity-bearing protocol。
2. ImGui Editor identity drag payload 的 unmanaged 数据只能是 Identity runtime ID；禁止随机 Guid token 路由到 managed source object。
3. `runtimeId` 不得出现在 Serialization DTO、History payload model、Settings、Scene/Prefab 或 Asset metadata public schema。
4. reload-safe History payload 不得含 `object`、`Type`、delegate、IdentityObject、Assembly 或 ALC。
5. collectible extension registry 的 active snapshot 外不得保存 extension `Type`、instance 或 delegate。
6. reference-aware serializer 调用不得直接使用 `SerializationContext.empty`；必须从声明的 owner context 进入。
7. Missing placeholder/state 不得公开或内部保存 collectible `Type`/object/delegate。
8. assembly reload Success path 必须经过 completed unload barrier。
9. timeout/failure path不得清空仍 Pending 的 monitor 后回到 Idle。
10. GC barrier Pending 时禁止开始新 compile/reload/Play/Build/Export。
11. Asset/Scene/Plugin/Settings/Graph resolver 必须注册到共享 reference catalog，禁止领域中央 switch。
12. 二级索引只能把领域 key 映射到 identity/immutable record，不能成为跨 generation live object 权威表。

需要理解 C# 类型与字段的规则使用 Roslyn/metadata analysis；不要用会误伤 `contentVersion`、list index 或 backend handle 的关键词扫描代替语义检查。
<!-- /rule-unit: identity-standard-12 -->

<!-- rule-unit: identity-standard-13 -->
<a id="identity-standard-13"></a>
## 十三、测试矩阵

### 13.1 Identity

- 注册、注销和相同 persistent ID replacement。
- slot 复用后 stale runtime ID 不能解析新对象。
- 不同 allocator 中相同 runtime ID 不能跨 domain 解析。
- duplicate persistent ID candidate 原子失败。
- Registry 索引不意外阻止无 owner object GC。

### 13.2 DragDrop

- native payload 精确包含 runtime ID，不包含 object token。
- Preview 和 Drop 都二次解析；对象在两者之间注销时 Drop Reject。
- reload/session 切换自动取消 active drag。
- Asset、Scene、Plugin/Script entry 使用同一 identity routing contract。
- 跨 Edit/Play/authoring domain 投递被拒绝。

### 13.3 Missing 与恢复

每个支持领域都覆盖：

```text
resolved
  → remove source/type/plugin/provider
  → missing state remains saveable
  → close/reopen or reload
  → reinstall/restore same identity
  → recovered concrete state
```

另外验证：

- nested references 部分恢复。
- expected type mismatch 保持 Missing。
- identity 冲突不产生半恢复状态。
- Graph edge、Scene order、Component/System identity 和 property bytes 不丢失。
- clean document 的自动 Missing/Recovery 不改变 dirty state。
- Player build 对 required Missing closure 明确失败。

### 13.4 Undo/Redo

- 在 target Missing、type Missing、Plugin removed、handler unavailable 时栈指针不移动。
- 恢复后原操作自动重新可用。
- Undo 创建/删除 Missing Component/System/Asset reference。
- redo branch、transaction compensation 和 disk-spilled payload 保持中立。
- Scene/Asset history 始终获得完整 Asset/reference Serialization Context。
- 局部 Scene 属性与元素状态通过所属 Core Identity domain 按需解析当前引用；禁止为每次标量编辑重建整场景的 live object 双向索引。同步作用域固定 Scene owner，拒绝过期、错误 domain、跨 Scene 和类型不匹配的引用；异常与嵌套回调必须恢复原作用域。元素别名只保留经校验的中立 Guid 映射。完整图、Prefab、子树与 reload 候选的冻结局部 ID 映射仍属于各自的当前图事务，不承担当前属性的 live object authority。
- `Remove ScriptComponent → remove Plugin/Asset → Undo → recover → Redo` 全链通过。

### 13.5 GC barrier

- 连续多轮 Script 与 Plugin reload 后每个旧 ALC 弱引用都失效。
- rollback candidate、Plugin uninstall、显式 unload 和 shutdown 的 ALC 也必须失效。
- Asset、Scene、Editor registry 和 History 不保留旧 ALC。
- 故意泄露 static delegate/event、Task、Thread、GCHandle、native callback 时，reload 抛 unload exception 并进入 Faulted。
- Pending 时第二次 reload、Play、Build/Export 被拒绝。
- 成功状态只在 monitor 全部 Completed 后发布。
- shadow generation storage 在成功后清理。
- shutdown 对仍 Pending/Faulted generation 聚合报告，不静默退出。
<!-- /rule-unit: identity-standard-13 -->

<!-- rule-unit: identity-standard-14 -->
<a id="identity-standard-14"></a>
## 十四、验收与变更要求

修改 identity、resolver、Missing 或 reload 时，必须执行上方测试矩阵，并同步领域调用方与退出顺序。测试通过数不能替代跨域生产事务、失败补偿和旧 ALC 不可达证据。

当前执行状态统一维护在 [当前验收](../../records/acceptance/current.md) 与 [当前问题](../../records/open-items.md)。本页不维护第二份阶段进度或历史测试计数。
<!-- /rule-unit: identity-standard-14 -->
