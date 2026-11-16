# Editor

Load complete rule units. The [registry](../coverage.json) owns routing and provenance.

<!-- rule-unit: editor-projects -->
<a id="editor-projects"></a>
## 13. Editor projects and reference boundaries
- Business source in each project under `src/composition/editor` uses a namespace exactly matching its csproj/assembly name. Feature directories organize files without adding namespace segments. For example, types under Inno.Editor.Inspection/PropertyDrawing still use namespace Inno.Editor.Inspection.
- Reusable InspectionDrawers, PropertyDrawers, Registries and serialized-property renderers belong to Inno.Editor.Inspection. Business Panels implement specific Drawers in their own projects and must not reference Inno.Editor.Panel.Inspector merely to extend inspection presentation.
- The sole namespace exception is Inno.Editor.ImGui/Widgets: all its types use namespace Inno.Editor.ImGui.ImGuiWidget.
- Inno.Editor.ImGui/Widgets contains only ImGuiWidget.*.cs files. Each widget's presentation, options, results and private state belong together in ImGuiWidget.<Feature>.cs. Do not create separate Widget helper files.
- Organize Editor project directories by independently understandable capabilities, such as Interactions, Documents, Zoom and PropertyDrawing. Do not mechanically scatter one feature's Actions, Menus, DragDrop, Runtime and Presentation into tiny type-role directories. Ambiguous Internal directories are prohibited; C# declarations, not directory names, express accessibility.
- Group Editor csproj ProjectReferences by public API boundary: the first ItemGroup contains only implementation dependencies absent from all public/protected APIs, each with PrivateAssets="compile"; the second contains only dependencies actually exposed through public signatures, base classes or interfaces. Remove unused references.
- Determine public reference visibility from actual API signatures, not merely runtime assembly use. Recheck the groups after changing public types, base classes, parameters, returns or properties.
<!-- /rule-unit: editor-projects -->

<!-- rule-unit: editor-state -->
<a id="editor-state"></a>
## 14. Editor History and Workspace state
- Reversible Editor data changes use EditorInteractions.history. Do not keep another Undo stack in a Panel or treat a simple inverse Action as the general model.
- Feature Modules apply the domain change first, then record it with RecordApplied(name, EditorHistoryChange). History payloads contain only stable protocol kinds, persistent IDs, Stable Type IDs, paths, indexes, scalars and neutral serialized bytes. They must not capture runtime objects, Plugin Types, extension instances or collectible-ALC delegates.
- Every reload-safe protocol declares [EditorHistoryHandler(kind)]. Handler Query only checks current-generation availability; Apply rolls back partial modifications on failure. Its Registry participates in candidate construction and atomic switching with the other Editor Registries.
- RecordValue, delegate-based Execute and derived EditorHistoryOperation are permitted only for Host-only compatibility workflows. These runtime-bound entries are truncated when extension generations change and must not serve EditorScripts or long-lived Scene/Asset records.
- Stable mergeKeys apply only to continuous input for the same logical value. Do not merge boolean toggles, creation, deletion or ordering. Use BeginTransaction for multiple steps, with each child independently atomic.
- Failed Undo/Redo leaves the operation on its original stack without moving pointers or overwriting new state. New operations release the Redo branch. Evicted/cleared operations release their files, objects and Plugin-generation references.
- EditorHistoryOptions automatically spills large payloads to <Project>/Library/Editor/History. Entry, resident-byte and disk-byte budgets all constrain History. Its cache does not enter editor.ini, Asset metadata or Scene serialization.
- Pure workflows such as saving, opening and selection do not enter data Undo by default. Record only the data portion when they actually modify Project data.
- Cross-startup Project semantic state belongs directly to Modules/Panels. Their Attributes require stable, globally unique IDs, and protected Capture(EditorState)/Restore(EditorState) hooks participate in persistence. Extensions use only Get/Set on the parameter object, without exposing JSON implementations in public/protected APIs. Types that do not override Capture do not enter state IO. Do not restore separate Workspace interfaces, readers/writers or second state IDs. Persistent values must be neutral and resolvable again.
- editor.ini is the unified readable Project-level Editor settings document. Standard ImGui sections store layout; each Module/Panel uses [InnoEditor][Module.<id>] or [InnoEditor][Panel.<id>]; Panel switches use [InnoEditor][Panels]. Do not wrap the entire Workspace in Base64 or one opaque payload. Do not persist Undo stacks, dirty Scene content, runtime references or compilation intermediate state.
- The standard Host's embedded resource supplies a new Project's default layout. Core receives a neutral initial document and reuses Core IO atomic writing. Initialize only when the file does not exist; do not overwrite an existing or deliberately empty editor.ini. Templates contain no example Scenes, Asset IDs, Plugin state, Selection, History or temporary popups.
- Editor Selection is transient interaction state for the current session and must not enter editor.ini. Workspace may save independently meaningful navigation positions, open documents and active documents. Startup must not automatically restore Asset, Scene, GameObject, Component or System selection.
- Normal Editor shutdown captures every stateful Module/Panel, then the latest ImGui layout, then forces one atomic write of the complete editor.ini before stopping Modules or unloading Scenes. Throttled runtime saves remain allowed but do not replace shutdown saving.
- All Editor Scene edits use Inno.Editor.Scene.SceneEdits. Ordinary properties store only one property's bytes; Components/Systems store element identity/type/index/state; GameObject deletion stores the minimal subtree; hierarchy changes store only affected placements. Do not serialize or restore an entire Scene for small edits.
- Module/Panel restoration tolerates missing Assets, damaged payloads and script types not yet present in TypeCache. Do not destroy the current editable state before the candidate is fully prepared.
- A stateful Module/Panel must successfully complete protected Restore once before Capture may overwrite its disk section. Extension Registry refresh may reenter during startup/script activation. The restoration coordinator weakly tracks restoring/restored state per instance, prevents reentrant callbacks and does not assume an instance retained in a new snapshot has already been restored.
- Scene Workspace restoration distinguishes an actually missing source file from an Asset Source Index awaiting its first reconciliation. If physical source still exists, retain pending Scene setup and retry. Do not overwrite Project settings with a temporarily empty runtime Scene collection. Editor permits no loaded Scenes; restoration, startup and deletion of the last Scene must not implicitly create an Untitled Scene.
<!-- /rule-unit: editor-state -->

<!-- rule-unit: editor-layout -->
<a id="editor-layout"></a>
## 24. Editor forms and scrolling

- Each visible region has one scrolling owner. Parent Panel/Modal, layout Child and content Child must not produce scrollbars in the same direction for the same content. A parent hosting a fill-area scrollable Child disables the corresponding scrolling.
- Editor fields with labels on the left and inputs on the right reuse ImGuiWidget.SetupPropertyColumns() and PropertyLabel(), dividing available width 2:3 after deducting any action column. Labels wrap within their own column and row height grows with content.
- Do not patch layouts with fixed label-pixel widths, unconditional scrollbar flags, fictitious content sizes or extra nested scrolling Children. After form/Panel/Popup changes, verify at small window sizes, with long text and at different zoom levels that ScrollMaxX/ScrollMaxY are positive only for actual overflow.
- Editor dropdowns use BeginBoundedCombo/EndBoundedCombo or BeginMenuSelector/EndMenuSelector. Popups open downward from the triggering control's bottom edge. Their width is bounded by the owning window; height is bounded by both remaining window space and a proportion of window height. Do not directly use native Combo automatic flipped placement.
- Align Popup size constraints to pixels at the Widget boundary: floor upper bounds and ceil required content sizes to prevent false overflow when ImGui truncates fractional padding. Regressions cover fractional zoom such as 85%, 90% and 110%, not only integer/half-integer padding. Do not conceal failures with NoScrollbar.
- Custom hit regions check both rectangle containment and whether their owning ImGui window is the currently interactive foreground window. Floating windows, Popups and Modals must block underlying Panel close actions, canvas gestures and game input. Platform events still use Inno.Core.Events; ImGui determines visible-UI hits/focus only. Play Sessions have an independent Input backend receiving events filtered by Game View focus policy, without a second event bus.
- Tree-row disclosure controls, text and overlay buttons each own an independent hit region. Native TreeNodes must not cover custom text using full-row SpanFullWidth/AllowOverlap. Regressions cover prior hover and same-frame move/press, text double-click, arrow expansion and overlay buttons. Clicking must not require an extra hover frame.
<!-- /rule-unit: editor-layout -->

<!-- rule-unit: editor-acceptance -->
<a id="editor-acceptance"></a>
## 32. Editor UI ownership and acceptance

- Domain editing in `features` must remain independent from native UI, Panels and Workbench. Panels compose feature APIs; they do not reference other Panels. Reusable controls belong to the session-owned ImGui adapter, and generic Graph presentation cannot depend on Shader semantics.
- Every interactive host owns one `EditorUiSession`. Transient widget/menu state belongs to its session and stable presentation owner. Native drawing borrows cannot survive their frame, window, context or owner thread. `current` is only a synchronous scoped accessor, never a global state owner.
- Continuous Inspector and viewport edits use one `EditorEditGesture` over `EditorInteractions.history`. Each sample records immediately; Escape rolls back the same transaction. Save, commands and generation changes finish the gesture before their own operation. No feature may delay History publication behind a separate UI-active flag.
- Selection, deferred commands and cross-frame tools resolve identities through Core Identity. Batch operations validate the complete selection, normalize overlapping roots and roll back atomically. Mixed, Missing, read-only and unassigned states remain distinct.
- Final delivery must evaluate architecture, organization, state/lifetime cleanliness, abstraction, reuse, visual consistency, extensibility, interaction, Undo/Redo, menu clarity and component granularity individually. Each evaluation must identify actual evidence, limitations and remaining improvements; directory changes and interface counts alone are insufficient.
- Reassess completion against the original approved plan, including every functional and quantitative acceptance requirement. Separate missing implementation from missing evidence; green CI and focused benchmarks cannot establish untested scale, visual, reload or lifecycle guarantees. Correct stale completion statements and issue ledgers when later review finds a gap.
- UI-neutral name formatting and Action/Menu queries must work without a native context or drawing session. Keep pure label formatting in Editor Core; only drawing and native borrows require an active UI scope.
- Temporary presentation worlds register their borrowed Core allocator through the existing selection coordinator's routing lease. Restore Edit selection before releasing that lease; failed retirement must preserve its owner. The routing table never becomes a second live-object authority.
- Boxed value types are not automatically neutral: a host struct may contain extension objects. Cross-callback targets retain only known neutral scalars strongly; all other non-identity objects use weak references, including boxed structs. Selection, focus and snapshots must not prolong their referenced objects' lifetime.
- The formal script SDK host references the actual exported API owners directly, including reusable presentation libraries when required. It must not use concrete Panels as transitive dependency carriers for asset, shader or graph authoring APIs.
- Session-owned zoom and density requests commit through the presentation preparation phase after selecting the owned context and display metrics, before native frame creation. Repeated preparation during the same drawing frame cannot publish a new metric snapshot. Standalone metrics remain usable without native UI.
- Presentation preparation, drawing and publication form one owned operation. Reentry and retirement during that operation must fail before releasing resources. A callback failure ends only its owned native frame, preserves the original failure and never publishes partial draw data; it must not clear another context's stacks or lower native assertions.
- Native presentation fixtures must create and enter an `EditorUiSession`, explicitly compose their feature/presentation owners, and resolve the exact service under test. Asynchronous shutdown fixtures must retry the public Pending retirement protocol until owned cleanup completes; they cannot rely on continuation timing or remove resource-release assertions.
- Presentation input snapshots expose physical Core modifier semantics. Reverse native shortcut normalization at the presentation boundary using the owned context configuration; do not interpret normalized Control/Super flags as physical keys or add shared-layer OS detection. Native input fixtures explicitly select their conventions and cover both modifier policies and native modes.

- Content-sized modals prepare one coherent frame snapshot and measure it before opening. Explicitly sized resizable windows prepare after current native input establishes their content bounds. Never invoke arbitrary drawing callbacks twice for measurement or rely on the previous message height.
- Shared dialog widgets own text wrapping, progress, bounded body scrolling and action spacing. Domain callers consume those controls and shared metrics; they must not compensate with empty lines, arbitrary padding or a second scroll owner. Escape action behavior is explicit.
- Workflow progress and cancellation are bound to the original owner and operation identity. Late callbacks cannot overwrite a newer request or a cancellation state, and progress presentation cannot weaken publication or generation retirement.

- CI and local acceptance must follow the actual changed owners. Use scoped Editor verification for bounded UI changes; retain full verification for build, platform, backend, SDK, shared runtime or unknown owners. Record omitted checks explicitly and never bypass production validation to shorten CI.
<!-- /rule-unit: editor-acceptance -->

<!-- rule-unit: editor-standard-9 -->
<a id="editor-standard-9"></a>
## Delivery and acceptance

Changes update source, callers, project references, public XML, documentation and applicable tests together. Use real public contracts and replaceable boundaries, without IVT, private reflection or test backdoors.

The Build entry's unified validate plan orchestrates existing xUnit/VSTest and real product journeys, recording domain, public-contract, integration, Native, product and performance evidence separately. Select contract, workflow, visual and lifecycle assertions through actual dependencies, without rebuilding the same project for multiple profiles. Retain SDK, source identity, TRX and summaries for each execution. Empty discovery or zero isolated-process execution is not success. Product acceptance uses the actual platform-declared renderer and startup expectations; shared entries do not embed backend names as default assertions. Stable --artifacts-path and independent --output select the build owner and run logs respectively. Isolated entries may explicitly select a shorter temporary root with -TemporaryDirectory; otherwise retain OS TEMP/TMP rather than nesting it implicitly in deep result paths. Ordinary and isolated processes use the same temporary owner.

One Settings Apply reuses one History transaction and accepts a new baseline only after every dirty owner succeeds. On failure, attempt rollback first and retain drafts. Show recoverable file/validation failures in the window with complete diagnostics. Lost History integrity prevents further Apply; do not swallow errors and retry. Open settings windows borrow the current generation's editable objects and use the existing admission guard to defer reload, restoring admission on close. Do not create another restoration or History system.

Acceptance orchestration obtains actual Native target, tool target and product properties from the official Editor entry, without globally overriding InnoTestNativeProduct. Tests requiring native deployment declare their own products/targets. Pure contract tests do not automatically prepare a complete product merely because their transitive references include a Native assembly. Ordinary execution, isolated discovery and isolated execution use the same SDK inputs.

Distinguish native-borrow verification from pure value conversion according to actual contracts. Every EditorDrawList operation validates its original window, frame, Context and thread. Color conversion retains existing Session/Context/thread admission. Verify saturation, rounding and nonfinite color behavior against the actual native oracle; ordinary numeric equality does not establish agreement at every boundary.

EditorUiInput Control/Super values mean physical modifiers, not native controls' internal shortcut normalization. The presentation boundary reconstructs them from the current Context's keyboard configuration. List selection reuses those snapshot semantics; platforms still choose primary modifiers through EditorKeyboardPolicy. Native text controls retain that Context's input conventions. Tests explicitly declare their keyboard configuration and cover Control/Super combined with both native modes rather than relying on runner defaults.

Final reports separately assess architecture, organization, state/lifetimes, abstraction, reuse, style, extensibility, interaction, Undo/Redo, menus and granularity, with evidence and limitations for each. Neat directories, test counts or successful startup do not replace visual/input acceptance, repeated reload, resource convergence and paired performance measurements.
<!-- /rule-unit: editor-standard-9 -->

<!-- rule-unit: editor-standard-10 -->
<a id="editor-standard-10"></a>
## Dynamic Modal state and size

- Window size, wrapping and drawing use one snapshot for the current frame, never the previous message's height.
- Preparation only reads and measures. The original domain owner verifies the operation before executing actions. Do not invoke drawing callbacks twice for measurement.
- Unified metrics express padding, item spacing, button height and font scale. Business Modals do not compensate with extra blank lines, fixed oversized heights or scattered numbers.
- Messages and details share one body scrolling owner, with actions remaining reachable. Children do not duplicate outer padding already supplied by the parent window.
- Distinguish declared, content-sized and user-resized dimensions. Partially specified dimensions must also be interpreted correctly.
- Regressions cover same-frame content growth/shrinkage, empty text, changed actions and fractional scale.

### Select CI acceptance by responsibility

Engine CI selects checks from the complete Git change set and actual SDK dependencies, not issue numbers or temporary omission of failures to save time. Isolate Editor domain, presentation, workflow and lifecycle tests by execution prerequisites. Selected products retain actual native smoke and product journeys. Release, NativeAOT restore, Player Support Packs and complete functional matrices belong to explicit full acceptance. Build, Native and reload changes select corresponding deep checks earlier; weekly acceptance adds cold builds, ten warm-cache rounds and long runs. Unknown owners or uninferable shared inputs produce visible diagnostics and conservative expansion. Manual workflows support full acceptance.

Inno.Tooling.Validation is the sole plan owner and does not weaken production-build integrity. Moves cover old and new owners; PRs use actual merge candidates and merge bases; local plans include staged, unstaged and untracked files. Record selection reasons and exclusions. A bounded pass is not a complete product/device matrix pass. Unavailable required environments, missing results and empty required tests fail or block.

Cheap architecture rules remain repository-wide. Prepare compilation metadata and documentation examples from actual dependencies, sharing the batch graph across architecture, tests and documentation. Impact analysis includes project references, imports, source, resources and generated inputs. Text similarity cannot declare SDK/TFM/shared-build changes irrelevant. Log directories, reports, commits/pushes and session recovery do not justify repeated acceptance. Reuse only deterministic evidence with matching inputs, tools, environments and artifacts.
<!-- /rule-unit: editor-standard-10 -->
