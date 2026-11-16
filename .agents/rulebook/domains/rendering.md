# Rendering

Load complete rule units. The [registry](../coverage.json) owns routing and provenance.

<!-- rule-unit: rendering -->
<a id="rendering"></a>
## 17. Rendering boundaries
- Rendering's public design must satisfy cross-platform support, API usability, flexible extension and low coupling together. Implementation convenience does not justify sacrificing any of them.
- Only `Inno.Adapter.Rendering.Bgfx`, BGFX build toolchains and the corresponding native tests may reference `Inno.Native.Bgfx`. BGFX handles, View IDs, native pointers and BGFX enums must not appear in other projects' public/protected APIs.
- `Inno.Rendering` remains backend-neutral and must not reference Scene, Assets, Editor or a concrete graphics backend. Upper layers use resource descriptions, capability sets, RenderGraph and command-encoding interfaces.
- General Graph must not reference Rendering or ImGui. Rendering must not reference the Shader authoring layer or Editor Graph in reverse. Shader graphs and source-function modules belong to the built-in authoring layer. Materials store Shader references and parameters, not graphs.
- Author all Shaders through graphs, then pass them through the unified Shader IR, compilation, reflection, validation and artifact-cache chain into runtime. Corresponding providers implement source-language parsing and backend generation. Do not create a second complete source-Shader authoring path.
- Pipelines, Features, Passes, Shader Nodes, GPU resources and compilation artifacts must be capability-aware, generation-scoped and reload-safe. Persistent state stores only Stable IDs and neutral data. Do not retain collectible-ALC Types, delegates or runtime objects long-term.
- Project script extensions may use only backend-neutral Rendering APIs. Isolate extension failures. Activate successful candidates atomically only at frame-safe points, retaining last-good Pipelines, Shaders and GPU resources.
- Rendering Core provides graphics mechanisms only. It must not embed 2D, 2.5D, 3D, PBR, Forward, Deferred, Light, Shadow, Camera, MeshRenderer or any concrete rendering worldview. All concrete rendering models must be composable from scratch through Project scripts or Plugins.
- Shaders, Techniques, Materials and Pipelines compose through open Stable ID contracts. The core must not maintain closed lists of Pass Tags, Render Paths, resource semantics or quality settings.
<!-- /rule-unit: rendering -->

<!-- rule-unit: content-frame -->
<a id="content-frame"></a>
## 27. Content, rendering layers and frame ownership

- `Inno.Content` depends only on necessary Foundation contracts and unifies logical ContentKeys, Pack indexes, read budgets and read-only leases. Shared Player, Runtime Session and AssetDatabase code does not read deployment files or create physical directories. Concrete content sources own files, HTTP, caches and owned streams. Asset consumption reuses `ArtifactLease.OpenRead()`; each opened stream holds an independent pin.
- Content caches validate the complete file set, actual lengths and content hashes. Marker files, mtimes and lengths cannot replace integrity verification. Validate candidates in independent staging before atomic publication. Readers pin a generation; retire old directories only after all readers release them. Distinguish post-commit cleanup failures from commit failures.
- Rendering separates Core graphics mechanisms, Assets runtime assets, Assets.Authoring authoring/compilation, and Runtime model/resource services. Core does not reference Assets or References; Assets does not reference Runtime, Shaders or Pipeline. Runtime and Player publication closures do not reference Authoring, Assets Pipeline or Shader compilation tools. Physical input files belong only to concrete SDK/toolchain adapter boundaries.
- General input uses `Inno.Adapter.Input`'s Core Events implementation. Do not put neutral event snapshots in the SDL project. Separate storage namespaces from physical roots. Document IO uses `IByteDocumentStore`. Host factories provide Session logs, and only their owning Session releases them.
- The primary presentation size must be real and explicitly available: `null` means no primary output. Zero-sized or internal 1×1 resources must not masquerade as an available surface. With no primary output, skip its graph construction and input conversion; independent offscreen requests may still run.
- Rebuild Contributor snapshots only when registration changes, and freeze them at frame start. Clear extension references from private scratch on normal frames, failures and retirement. Graph Validate performs analysis only; final Compile performs complete resource allocation exactly once. Mutation rollback restores both graph and validation state. Do not modify an accepted pass.
- Native recipes declare their actual input closure and tool identities. Share the initial input scan within an operation, retaining necessary revalidation after waiting for a lease and before publication. An unrelated assembly MVID cannot replace a component recipe identity. Unify Task bootstrapping and isolate target properties. Content-identical native deployment must not copy files, replace loaded DLLs or update mtimes.
- Shared compilation of a Task's complete execution closure uses Core IO leases to coordinate cross-process writes and releases MSBuild nodes while waiting. The minimal bootstrap publisher uses operation-private intermediate state. Existing immutable-publication and reader protocols then manage loading and retirement.
<!-- /rule-unit: content-frame -->
