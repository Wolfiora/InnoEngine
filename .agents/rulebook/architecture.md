# Architecture

Load complete rule units. The [registry](coverage.json) owns routing and provenance.

<!-- rule-unit: style -->
<a id="style"></a>
## 1. Project style
- Preserve the repository's consistent readability and extensibility.
- Prioritize clear layering, explicit boundaries and low coupling.
- Write all new comments in English by default, especially public API documentation.
<!-- /rule-unit: style -->

<!-- rule-unit: encapsulation -->
<a id="encapsulation"></a>
## 4. Encapsulation and responsibility
- Expose the minimum necessary public API.
- Prefer composition over inheritance.
- Decouple through registries, factories and abstract interfaces.
<!-- /rule-unit: encapsulation -->

<!-- rule-unit: directories -->
<a id="directories"></a>
## 10. Directory boundaries
- `src/foundation`: Core, extension catalogs, types and generation contracts.
- `src/content`: Assets, References, Scene and Animation.
- `src/services`: backend-neutral domain contracts and runtime services.
- `src/runtime`: Runtime, deployment, generators, Plugins and authoring-time scripting workflows.
- `src/adapters`: neutral SPIs, open Provider/Catalog contracts and Core Events input implementations.
- `backends/<component>`: shared backend runtime, native code, build recipes and component tests; each component has one source owner.
- `platforms/<platform>`: system and SDK integration, explicit targets, thin Editor/Player entries, layouts and Support Packs. A platform must not reference another platform.
- `src/composition`: default engine, Shell, Player and Editor composition entries.
- `entry`: the two lightweight development entries and current-host routing. Other production layers must not reference it. `Inno.Packages.props` pins NuGet versions for the BGCS package family; no source-dependency entry is provided.
- `build`, `tools`, `tests`: neutral unified builds, concrete distribution registration, validation libraries and cross-component contract tests.
- Place new files in directories matching the existing layers and responsibilities wherever possible.
<!-- /rule-unit: directories -->

<!-- rule-unit: no-legacy -->
<a id="no-legacy"></a>
## 15. No legacy compatibility or schema versions
- InnoEngine is a personal engine whose current source and current Project data evolve together. New or changed features do not support old files, schemas, fields, cache directories, namespaces, APIs or serialization layouts. Do not add fallback readers, migrations, compatibility aliases, former IDs, deprecated wrappers or dual writes for them.
- Persistence models, Attributes, Importers, Build Processors, History, Workspaces, Scenes, Prefabs, Asset metadata, Catalogs and script manifests must not introduce legacy-adaptation fields such as `version`, `schemaVersion`, `formatVersion` or `formerVersion`. When a format changes, update the current writer, reader, tests, documentation and Project data directly. Old data may be explicitly invalidated and regenerated.
- Do not reserve migration branches for hypothetical future compatibility. Only an explicit user request to import a particular old format permits a one-time converter with a clear boundary. Conversion logic must not enter normal runtime paths.
- Update all callers when deleting or refactoring an API. Do not retain old overloads, namespace facades, forwarding types or `[Obsolete]` compatibility layers unless the user explicitly requests them.
- These rules permit runtime identities required for current correctness, including TypeCache/Assembly generations, concurrency revisions, change counters, content hashes, artifact fingerprints, MVIDs, job-handle generations and strict magic/headers used only to reject damaged or incorrect input formats. They must not become mechanisms for reading multiple legacy schema generations.
- When reviewing or cleaning implementations named `version`, `legacy`, `migration`, `compatibility`, `former` or `deprecated`, first determine whether they serve only old data/APIs. If so, remove them together with their tests and documentation rather than extending them.
<!-- /rule-unit: no-legacy -->

<!-- rule-unit: architecture -->
<a id="architecture"></a>
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
<!-- /rule-unit: architecture -->

<!-- rule-unit: core-reuse -->
<a id="core-reuse"></a>
## 22. Reuse foundational capabilities
- Before adding events, identity, serialization, mathematics, diagnostics or other foundational capabilities, check for existing contracts in `Inno.Core`. Prefer using or wrapping them directly. Do not build parallel management systems in upper layers such as UI, Canvas or plugins.
- UI and plugin event subscription, dispatch and lifetimes reuse `Inno.Core.Events`. Domains may define their own event types and adapters, but must not replace core event management with another queue, listener registry or event bus.
<!-- /rule-unit: core-reuse -->

<!-- rule-unit: solution -->
<a id="solution"></a>
## 26. Solutions and project files

- Solution Folders represent actual architectural responsibilities. Remove empty groups containing neither valid projects nor Solution Items. Avoid unnecessary single-project wrapper groups for small project collections.
- Authored projects under `entry`, `src`, `backends`, `platforms`, `build`, `tools` and `tests` belong in the Solution, including binding-generation extensions and test fixtures. Exclude `obj`, `artifacts` and independent third-party projects in extern. A project without incoming ProjectReferences is not necessarily unused: check host entries, MSBuild tool calls and generator configuration before removing it.
- Use two-space indentation in `.csproj`, `.props` and `.targets`, blank lines between top-level responsibility blocks, one property per line in long property lists, and `/` in relative ProjectReference paths.
- Merge adjacent reference groups only when their conditions and reference visibility match. Do not reorder Imports, Properties or Targets, or change conditions, metadata or evaluation order. Preserve Editor public/implementation reference boundaries.
- Detailed rules are in `.agents/rulebook/coding.md`. After reorganization, verify valid XML, Solution project identities and configurations, then run affected builds and architecture validation.
- After phases of long builds or acceptance, clean this operation's obsolete tool snapshots, private load directories and test-browser profiles, and check free disk space. Before cleanup, verify the explicit owner, resolved absolute paths, symbolic links and active processes. Retain current artifacts and acceptance logs; do not clean user or SDK global caches. Logical sizes of hardlinked/compressed files do not establish physically reclaimed space.
<!-- /rule-unit: solution -->
