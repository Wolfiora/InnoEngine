---
tags: [architecture, new-boundary, ownership, public-api, encapsulation, core-reuse, style]
---
# Architecture

## 1. Project style
- Preserve the repository's consistent readability and extensibility.
- Prioritize clear layering, explicit boundaries and low coupling.
- Write all new comments in English by default, especially public API documentation.

## 4. Encapsulation and responsibility
- Expose the minimum necessary public API.
- Prefer composition over inheritance.
- Decouple through registries, factories and abstract interfaces.

## 21. Architecture, language frontends and third-party boundaries
- Architectural clarity, dependency direction, responsibility and low coupling take precedence over every feature, performance goal and development convenience. Define the owning layer, public contracts, lifecycle and failure boundary before any feature or extension. Do not trade these for temporary direct connections, leaked implementation names, global singletons or convenient cross-layer calls.
- General source assets in services and asset layers are inert text or byte carriers. Unless a language is explicitly an engine public standard, core importers, runtime services and script contracts must not parse, infer or depend on third-party syntax, or implicitly select semantics from the current backend or a filename.
- Select languages and implementations using stable, explicit, serializable `languageId` and `implementationId` values. Language frontends form immutable generation-scoped snapshots through the existing TypeRegistry/TypeCache system. A frontend receives controlled source resolvers and immutable requests, and produces neutral dependencies, structured diagnostics and frozen artifacts. Lexing, preprocessing, parsing, lowering and native compilation remain in the concrete frontend, adapter or toolchain.
- Explicitly declare any engine-standard language in public contracts and documentation. Otherwise, implementation names, proprietary terminology, capability restrictions and error text from BGFX SC, RML, RmlUi, FreeType, HarfBuzz and similar libraries must not appear in services, scripting or neutral asset contracts. They belong only to concrete implementations and their diagnostics.
- Extensible runtime backends use open stable IDs, provider catalogs and capability descriptions. Closed enums and core-layer switches are not third-party extension mechanisms. Services express neutral capability absence; concrete implementations explain their own limitations.
- Isolate third-party C++ libraries behind handwritten semantic adapters in their owning `backends/<component>/native/Inno.Native.XXX/Native` directory. The same project's BGCS configuration and generation extensions belong in `Bindings/`, and its managed single-file binding in `Generated/Bindings.cs`. CMake intermediate output belongs in the corresponding `backends/<component>/build/Inno.Build.Toolchains.XXX/obj/native`, never `Native/`. Do not modify third-party `extern` source. Adapters expose only narrow backend-neutral C++ facades and hide third-party types with PImpl/opaque handles and neutral DTOs. By default, BGCS Cpp2C generates the C ABI from that facade, then BGCS generates one managed binding file with platform-independent paths from the generated C header. Native and managed generated outputs have separate roots. Handwritten exports/imports and edits to generated code are prohibited. Additional handwritten C ABI is allowed only for an explicitly recorded lowering/shim requirement that BGCS cannot express. Native handles, symbols and third-party types must not cross the adapter boundary. Frame-hot-path callbacks remain native; do not cross into managed code merely to simplify bindings. An explicit process host manages reference counts, thread ownership, session isolation, generations and final release for third-party process-global state. Anonymous globals must not masquerade as session-local state.
- Backend-neutral low-level public APIs for source plugins are permitted and necessary. Minimal public exposure means the smallest stable surface needed for external plugin composition, not only gameplay-level APIs. Plugin-author APIs must explicitly enter script export manifests. Backend SPIs, native bridges, concrete implementations and third-party handles remain unexported.
- Explicitly define ownership and lifetimes on frame hot paths. Do not unconditionally copy all geometry, indices or texture pixels every frame, or encode the same data again for each draw command. Prefer generation-scoped stable handles, incremental resource updates/releases, persistent GPU caches, and pooled or owned buffers with explicit release protocols. Optimization must not expose unsafe borrowed native memory through public layers.
- New systems and boundary changes must also update architecture-validator project classification, solution ownership, dependency rules, native-consumption rules, scripting allowlists and public documentation. They are not complete until repository architecture validation passes.

## 22. Reuse foundational capabilities
- Before adding events, identity, serialization, mathematics, diagnostics or other foundational capabilities, check for existing contracts in `Inno.Core`. Prefer using or wrapping them directly. Do not build parallel management systems in upper layers such as UI, Canvas or plugins.
- UI and plugin event subscription, dispatch and lifetimes reuse `Inno.Core.Events`. Domains may define their own event types and adapters, but must not replace core event management with another queue, listener registry or event bus.

## 1. Design and responsibility

- Prioritize architectural clarity, dependency direction, ownership, encapsulation and low coupling.
- Match filenames to primary types. Directories express independently understandable capabilities; namespaces follow their owning project.
- Public APIs expose the smallest complete contracts needed for external composition. Prefer composition and supply dependencies through constructor parameters or explicit composition entries.
- Check existing repository events, identity, serialization, mathematics and diagnostics first. Reuse them and avoid parallel management systems.
- Interfaces or base classes express extension behavior; immutable discovery metadata is declared once. Replaceable boundaries isolate platforms, storage, compilers and presentation.
- Separate implementation from persistence protocols. Persistent data uses stable identities and neutral values; explicit lifetimes manage runtime objects, delegates and type references.
- Shared mechanisms, concrete backends, platform-system integration, product composition and build distributions each own their source. Shared mechanisms must not select concrete platforms or products.
- A build host describes only the tool execution environment. Requests explicitly select runtime target, CPU, ABI and managed deployment; do not infer them from host facts.
- One backend owns a third-party component's runtime, Native facade, binding configuration and build recipe. Platform products select them through capabilities and frozen descriptions.

## Splitting responsibilities and changing boundaries

Large owners may use partial files for independently understandable responsibilities, while fields, construction and the single lifecycle remain in the primary file. Move members and verify syntax tokens/order first; change behavior/performance separately. Public DTO filenames match their primary type, and closely related enums remain in the owning protocol. Do not create parallel event, identity or Undo managers merely to shorten files.

Public-contract changes update every consumer, reference visibility, script manifest, Stable ID, documentation, success/failure/boundary test and architecture gate. Record ownership, cancellation, thread affinity and final release for read streams, background tasks, snapshots, scratch, native callbacks and device resources. Performance conclusions require measurements; unmeasured values remain unknown.
