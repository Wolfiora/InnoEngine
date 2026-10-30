# Final ownership review at dd5979c6

Source review at the production candidate. Complete local gates now pass; the final report and exact delivery-head attachment provide acceptance/CI scope.

## Actual authority and retirement

Input flows through the Core Events adapter into one EditorUiSession, the current control or Action request, the owning feature and its existing domain mutation/History protocol. EditorHostResourceStack's List<Action> is a reverse cleanup stack for one Host, not an event dispatcher. Action registrations are generation-owned discovery records, not another event bus. No changed UI owner introduces a listener registry or live-object authority.

The menu template retains neutral IDs, strings, ordering and immutable path structure. Dynamic menu sources build request-local contributions and publish them after successful callbacks. Availability and execution query the current target; shortcut indexes select candidate Action IDs without retaining request targets. The catalog clears its generation indexes when replacing extensions. Logger materializes exception arguments into neutral text synchronously. This keeps a failed extension's exception object out of the long-lived diagnostic record.

GraphCanvasLayout is an owner-thread neutral display projection. SetNodes validates the whole candidate, rejects duplicate IDs before publication and reuses equivalent immutable node/port values. It neither resolves live objects nor owns graph mutation. Reordering remains draw-order state. Controllers/features own document changes and History; custom drawing is frame-scoped. The independent non-Shader public consumer is necessary evidence of reuse rather than a nominal interface.

SceneStore already owns the attachment structure snapshot. Its source-ID projection is EngineObject-to-current-persistent-ID serialization input, never persistent-ID-to-live-object resolution. Core identity rebinding is checked on every capture. A changed identity replaces the read-only projection instead of mutating an already borrowed graph write. Structure invalidation drops the snapshot. Systems have another lifecycle: SceneGraphSourceIds indexes them only for the current write, preventing the structure cache from pinning removed Systems. Subtree and Prefab mappings retain their separate necessary graph-local semantics.

ScenePropertyReferenceScope pins the exact current allocator and Scene runtime identity for one synchronous property operation. Each encountered object is resolved and checked through Core Identity, including domain, exact registration, liveness and Scene ownership. Neutral Guid aliases are validated against that authority and never cache live targets. Missing, destroyed, foreign and colliding targets reject. A constructed but not yet loaded Scene remains valid when its current public ownership contract is valid. Full graph maps remain necessary for graph-local construction and are not silently substituted.

Core Serialization owns capture proof, immutable nodes and canonical output. Scene supplies complete neutral row inputs; arbitrary converters, unsupported metadata, Missing and Prefab paths still use the same authoritative writer. The borrowed proof builder retires after its callback, with current metadata/converter/identity checks. Scratch arrays are borrowed from the BCL and returned cleared. An immutable node's memoized canonical block is bounded at 64 KiB. No snapshot stores source objects, plugin Type, converter, callback or ALC. No persistent revision/schema field is added. Ordinary serialization does not call optional snapshot proof hooks.

Stats uses EditorStatistics as authority. FrameStatisticsSampler is a Panel-owned numeric presentation buffer: at most 120 samples, a 0.5-second interval and a 60-second window. A discontinuity resets it. Console derives a filtered view from existing Diagnostics occurrences and groups; it does not own a second log store or object identity protocol.

The standard Host owns the default layout resource. Editor Core consumes a neutral readable document and Core IO publishes it atomically only when absent, preserving an intentionally empty/custom document. Normal shutdown captures Module/Panel state and current layout before final atomic save. Selection, History, runtime references and compilation intermediates stay transient.

FileByteDocumentStore.Read owns one rename-compatible FileStream and a newly allocated exact output byte array. FileShare.Read|Delete permits the existing atomic publisher protocol while the opened handle pins one complete file. This is a BCL file-sharing correction, with no production OS branch, retry, swallowed cleanup error or changed publisher algorithm.

The negative loaded-package fixture owns its physical image in an existing isolated public SDK child. The child still physically loads the altered same-name/same-MVID DLL, rejects content, and verifies weak Assembly/context collection through full GC/finalizer/full GC. Its success marker follows those assertions. The parent deletes its fixture only after successful child exit. This is a test resource boundary, not a BGCS workaround or weaker loading mode.

## Explicit extension work

- A new Action/menu/toolbar contribution uses existing stable metadata and behavior discovery, without editing a central Panel switch.
- A new graph domain implements its controller/semantic operations and supplies neutral display nodes and ports to Graph.ImGui; the shared presenter does not acquire that domain's type knowledge.
- A new native platform supplies its existing platform/provider contribution. No changed UI or Core file adds a Windows/macOS/backend branch.
- A new serialization provider can decline optional proof and retain exact normal capture. It does not need a Scene reference or private reflection. A provider claiming reuse must provide the complete declared capability.
- A new Panel owns its transient session state and semantic persistence hooks, uses existing Identity/Events/History and supplies failure/retirement tests. It must not create a foundational manager to avoid those contracts.

## Source integrity observations

The automated source inventory reports 241 tracked projects, 30 Editor projects, clean engine/companion/BGCS checkouts and no master-to-candidate BGCS/vendor/generated-binding/bindgen/dependency-lock change. All branch commits use Aaron Liao as author and committer, with no co-author trailer. Three import scan hits are intentional negative test text: rejected script global using, rejected implicit import XML, and rejected MSBuild Using XML. They are not production imports.

Four baseline BGCS 4.1.2 package archives retain their pre-implementation SHA-256, and 126 installed archive entries match their package contents. Package container metadata is distinguished from runtime assets. This package check, source audit, actual restored/loaded identity tests and formal generation logs are separate evidence scopes.

Complete local source/product/test, strict lifecycle, visual-content, controlled quantitative review and Wiki results are attached in the final report. Exact delivery-head CI and Ready state are signed in its PR attachment. Physical displays, manual Mac interaction and OS IME operation remain explicitly outside automated simulated-metric claims.

## Necessary public boundary review (#18 subset)

| Boundary | Necessity and ownership | Failure / lifetime contract |
|---|---|---|
| GraphCanvasNode/Port/Layout and pointer input/result/controller | Another graph domain in another assembly supplies neutral geometry and receives intents without referencing Shader | Candidate validation precedes replacement; frame callbacks are not cached; document controller retains mutation/History authority |
| ScriptCompilationInputs and compiler options injection | Host transfers the actual selected SDK's evaluated immutable target/reference/symbol/compiler facts to the runtime compiler and IDE without leaking Build/MSBuild types | Missing/inconsistent inputs, cancellation and failed evaluation do not publish a valid compilation artifact; content identity invalidates products |
| SerializationSnapshot/CaptureSnapshot and per-member/array-row writer input | External owners retain neutral canonical state and optionally certify complete input facts through the same Writer | Ordinary capture remains authoritative; unsupported proof declines reuse; callback borrows retire synchronously; partial errors cannot publish false clean state |
| Converter capture hook, borrowed key builder and metadata capability | Replaceable Converter/metadata providers can explicitly certify complete pure input without a central Scene/plugin allowlist | Exact metadata/converter/current-identity guards remain; mutation, wrong thread, retired builder and ambiguous/contextual effects reject or disable reuse through documented semantics |
| Optional initial layout document on EditorContext | Standard composition supplies its layout while Core remains unaware of concrete standard Panels | Only absent files are initialized; existing empty/custom bytes survive; atomic creation/replacement use Core IO; pure selection/History do not persist |
| Existing presentation and drop/interaction borrows | External controls and plugins compose the same current Session/input/domain contracts | Thread/frame/window/context/generation validation applies; request targets are synchronous borrows; persistent routes use Identity, not retained live objects |

Menu templates, shortcut indexes, Console filtering, Stats sampling, Scene source-ID projections and canonical block caches remain internal. Public visibility was not widened for tests. English XML and current Wiki describe these boundaries; the complete local signature/example gate now passes. This is not a repository-wide semantic-documentation completion claim for #18.

## Final corrected-source execution review

At `5e329bbe`, the uncalled HierarchySelection.DeleteObject route was removed with its unused context parameters/imports. Real deletion remains in the Action/feature path through SceneEdits.DeleteGameObjects and the same History/identity protocol. This is actual dead-path removal, not a forwarding facade or a changed user action.

At `dd5979c6`, both Compiler cache commits now use the existing Core atomic directory transaction. Complete validation and staging remain Compiler responsibilities. Absent destinations use Publish; invalid existing trees use Install instead of destructive best-effort deletion. The existing compilation token reaches both boundaries. Install adds one optional token, no compatibility overload, and restores an evacuated old tree using an uncanceled rollback. No postcommit cancellation undoes a complete cache. Existing DirectoryCleanupException distinguishes committed installation from later cleanup failure, and AggregateException preserves both failures and the old-tree location. Compiler gains only an implementation-only Core.IO reference, not a Build/MSBuild/platform/BGCS dependency. Private staging retirement remains separate.

Public Compiler regression evidence is red before/green after for temporary staged readers. Compiler17 and CoreIO41 pass, including cancellation after old-tree evacuation, persistent rejection with another generation unchanged, exact whole-tree preservation for invalid-cache replacement, and explicit repair. The original handle owner is unknown and is not attributed to BGCS/Roslyn/antivirus. Five independent cold/warm formal requests, all complete corrected-source profiles/companions/six products, controlled quantitative review and full Wiki now pass. Focused tests were not used as a substitute. Exact delivery-head CI remains separately signed in the final attachment.

The Core Install token is necessary because actual asynchronous Compiler operations already own cancellation. It completes one existing transaction boundary rather than exposing Compiler internals or adding a second filesystem policy. Existing two-argument source callers continue to use the same method with its default token, and are compiled again in the complete source/product graph. The unchanged Core filesystem retry budget is not increased.


## Final local verification authority

See ../EDITOR_UI_FINAL_ACCEPTANCE_2026_10_30.md and local-acceptance.json for the complete candidate results, performance-review.json for every initial comparison and graph-tail-summary.json for the predeclared longer tail review. Exact containing-commit CI and Ready state: https://github.com/Wolfiora/InnoEngine/pull/26#issuecomment-6516741338. Physical/manual limits remain unchanged.
