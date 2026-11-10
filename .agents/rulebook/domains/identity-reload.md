# Identity and reload

Load complete rule units. The [registry](../coverage.json) owns routing and provenance.

<!-- rule-unit: identity-reload -->
<a id="identity-reload"></a>
## 20. Identity, Missing, references and reload
- Resolve live objects through `Inno.Core.Identity` when crossing UI, callback, queue, frame, Scene, Asset, Scripting or Plugin-generation boundaries. Use `Identity.runtimeId` for transient resolution within the current domain. Persistence, History and cross-generation restoration store only `Identity.persistentId`. Runtime IDs must never enter Scenes, Prefabs, Asset metadata, Settings, Plugin manifests or History.
- Stable Type IDs, semantic IDs such as Plugin/Feature/Importer IDs, Artifact keys and generation handles retain separate meanings and must not be confused with object Identity. Domain secondary indexes may map paths/semantic keys to persistent IDs or immutable records, but cannot become another authority for live objects across generations.
- An Editor ImGui DragDrop native payload contains the source identity's runtime ID. Do not use a random token to look up a managed source object. Preview and Delivery both resolve again through the correct `IdentityAllocator` and verify the generation. Convert to persistent ID when recording a Drop on disk or in History.
- When an Asset, Script Type, Plugin, Importer, Component/System, Graph node, Settings contribution or extension is temporarily unavailable, preserve its Missing state: original persistent ID, Stable ID, structural position, property bytes, dependencies and neutral extension state. APIs and UI must strictly distinguish Missing from null/Unassigned.
- Restore a returning identity/type/plugin atomically through the shared candidate/recovery transaction at an owner-thread safe point. Failure preserves the original Missing state and diagnostics. Assets, Scene, Scripting, Plugins, Graph and individual Panels must not implement incompatible resolvers, placeholder lifetimes or recovery transactions.
- Reload-safe Undo/Redo stores only stable protocol kinds, persistent IDs, Stable IDs, structural values and neutral bytes. It must not capture runtime objects, Types, delegates, extension instances or ALCs. Temporary Missing is a recoverable barrier: do not discard or skip the operation or move the stack pointer. Restoration makes the original operation available again automatically.
- Serialization that may contain Asset/Object references obtains a complete `SerializationContext` from its owner. Except for pure value graphs proven context-free by their types and tests, business code must not assemble temporary resolvers from `SerializationContext.empty`. Missing required resolvers fail at composition/startup, not later during Play, Undo or recovery.
- Scripting, Plugin, Asset type, Serializer and extension generations participate in the shared candidate transaction. Successful retirement of old collectible ALCs requires Full GC → `GC.WaitForPendingFinalizers()` → Full GC and weak monitors confirming every context is unreachable. Before that barrier completes, reload must not report Success or permit another reload, Play, Build or Export.
- An unload-verification timeout causes an explicit exception and Faulted state. Never clear still-Pending monitors and continue. Failures identify module/domain/scope/generation. A Faulted process cannot continue generation transactions and requires a full Host restart. Tests cover old-generation strong references from statics, events, tasks, threads, AsyncLocal, GCHandle, native callbacks and Editor transient state.
- The full specification, target APIs, current gaps and test matrix are in `docs/architecture/identity-and-reload.md`. Reconcile that page when changing Identity, Scripting, Plugins, Assets, Scene, Editor Interactions/History or any collectible extension.
<!-- /rule-unit: identity-reload -->

<!-- rule-unit: identity-standard-12 -->
<a id="identity-standard-12"></a>
## 12. Required architecture checks

Once they can be implemented reliably with low false-positive rates, the Architecture Tool must check at least:

1. Scene, Asset and Plugin authoring, and Editor addressable models use IdentityObject or a declared identity-bearing protocol.
2. Unmanaged ImGui Editor identity-drag payloads contain only Identity runtime IDs; random Guid tokens must not route to managed source objects.
3. Runtime IDs do not appear in Serialization DTOs, History payload models, Settings, Scene/Prefab schemas or public Asset metadata schemas.
4. Reload-safe History payloads contain no object, Type, delegate, IdentityObject, Assembly or ALC.
5. Collectible extension registries retain no extension Type, instance or delegate outside the active snapshot.
6. Reference-aware serializer calls do not directly use SerializationContext.empty; they enter through the declared owner's context.
7. Missing placeholders/states retain no collectible Type, object or delegate, publicly or internally.
8. Assembly-reload Success paths pass through a completed unload barrier.
9. Timeout/failure paths do not clear still-Pending monitors and return to Idle.
10. A Pending GC barrier prevents new compile/reload/Play/Build/Export operations.
11. Asset/Scene/Plugin/Settings/Graph resolvers register with the shared reference catalog; domain-central switches are prohibited.
12. Secondary indexes map domain keys only to identities/immutable records, never becoming live-object authorities across generations.

Use Roslyn/metadata analysis for rules requiring C# type and field understanding. Keyword scans that misclassify contentVersion, list indexes or backend handles do not replace semantic checks.
<!-- /rule-unit: identity-standard-12 -->

<!-- rule-unit: identity-standard-13 -->
<a id="identity-standard-13"></a>
## 13. Test matrix

### 13.1 Identity

- Registration, unregistration and replacement using the same persistent ID.
- A stale runtime ID cannot resolve a new object after slot reuse.
- Equal runtime IDs from different allocators cannot resolve across domains.
- Duplicate persistent-ID candidates fail atomically.
- Registry indexes do not accidentally prevent garbage collection of objects without owners.

### 13.2 DragDrop

- Native payloads contain exactly the runtime ID, with no object token.
- Preview and Drop both resolve again; Drop rejects an object unregistered between them.
- Reload/session changes automatically cancel active drags.
- Asset, Scene and Plugin/Script entries use the same identity-routing contract.
- Cross-Edit/Play/authoring-domain delivery is rejected.

### 13.3 Missing and recovery

Cover this sequence in every supported domain:

```text
resolved
  → remove source/type/plugin/provider
  → missing state remains saveable
  → close/reopen or reload
  → reinstall/restore same identity
  → recovered concrete state
```

Also verify:

- Partial recovery of nested references.
- Expected-type mismatch remains Missing.
- Identity conflicts cannot produce partially restored state.
- Graph edges, Scene order, Component/System identities and property bytes are preserved.
- Automatic Missing/Recovery does not change a clean document's dirty state.
- Player builds explicitly fail for a required Missing closure.

### 13.4 Undo/Redo

- The stack pointer does not move when a target/type is Missing, a Plugin is removed or a handler is unavailable.
- Restoration automatically makes the original operation available again.
- Undo creates/deletes Missing Component/System/Asset references.
- Redo branches, transaction compensation and disk-spilled payloads remain neutral.
- Scene/Asset History always receives a complete Asset/reference Serialization Context.
- Local Scene properties and element state resolve current references on demand through their owning Core Identity domain. Do not rebuild a whole-Scene bidirectional live-object index for every scalar edit. Synchronous scopes pin the Scene owner and reject stale, wrong-domain, cross-Scene and type-mismatched references. Exceptions and nested callbacks restore the previous scope. Element aliases retain only validated neutral Guid mappings. Frozen local-ID maps for complete graphs, Prefabs, subtrees and reload candidates remain owned by their respective current graph transactions; they do not become live-object authorities for current properties.
- The complete Remove ScriptComponent → remove Plugin/Asset → Undo → recover → Redo sequence passes.

### 13.5 GC barrier

- Every old ALC weak reference becomes invalid after repeated Script and Plugin reloads.
- ALCs also become unreachable after candidate rollback, Plugin uninstall, explicit unload and shutdown.
- Assets, Scenes, Editor registries and History retain no old ALC.
- Deliberate static delegate/event, Task, Thread, GCHandle and native-callback leaks make reload throw an unload exception and enter Faulted.
- While Pending, a second reload, Play and Build/Export are rejected.
- Success is published only after every monitor is Completed.
- Shadow generation storage is cleaned after success.
- Shutdown aggregates and reports still-Pending/Faulted generations instead of silently exiting.
<!-- /rule-unit: identity-standard-13 -->

<!-- rule-unit: identity-standard-14 -->
<a id="identity-standard-14"></a>
## 14. Acceptance and change requirements

Changes to identity, resolvers, Missing or reload must run the preceding matrix and update domain callers and shutdown order together. A count of passing tests does not replace evidence of cross-domain production transactions, failure compensation and old-ALC unreachability.

Maintain current execution status in [current acceptance](../../records/acceptance/current.md) and [open items](../../records/open-items.md). This page does not maintain a second progress ledger or historical test count.
<!-- /rule-unit: identity-standard-14 -->
