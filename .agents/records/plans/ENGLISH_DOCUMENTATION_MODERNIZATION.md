# English documentation and workflow modernization

Status: implementation in progress. This record does not certify completion.

## Approved scope amendment, 2026-11-11

The user requested faster delivery and explicitly excluded backend-native and generated binding API documentation. Product documentation covers engine services, managed adapters, extension contracts, script surfaces and tooling. Native implementation owners retain concise integration boundaries; full ABI/member reference and raw-binding examples are retired. This amendment changes documentation scope only: native correctness, ownership, deployment validation, legal provenance and engine API obligations remain. The existing bound project inventory records the scope, and generation/coverage checks reject native member-reference inputs. Historical evidence and original migration source hashes remain unchanged.

Implemented in Docs commit `3d5991f45fe5f3ba1fa2789d3290e452f78ec876`: 4,327 generated native pages retired, eight actual native owners classified, and 4,524 engine declarations preserved across 812 pages. The [scope report](https://github.com/Wolfiora/InnoEngine.Docs/blob/3d5991f45fe5f3ba1fa2789d3290e452f78ec876/.agents/records/acceptance/ENGINE_ONLY_SCOPE_2026_11_11.md) records 34 passing regressions, source/rendered projection checks, the 28.19-second site build and five query samples (591.1751 ms median). Engine migration inventory marks 72 original ABI fragments as approved retirements awaiting legacy-validator cutover and five native project pages as retained integration overviews. Original hashes remain unchanged. Engine rule loading and negative provenance/routing regressions pass after the explicit amendment; all 241 rule identities and 58 units remain. Product/source execution, complete content coverage and remote deployment are not certified by these checks.

## Authorized baseline and ownership

- Engine baseline: `a4e44769715c0577fb0a4c13b2d14ad998ddd8e1`, branch `ai-pipeline`.
- Engine workspace: `C:/Dev/GameEngineDev/InnoEngine`; initially clean.
- Documentation repository: `Wolfiora/InnoEngine.Docs`, public; default branch `main`, implementation on a migration branch.
- Documentation workspace: `C:/Dev/GameEngineDev/InnoEngine.Docs`; initially an empty Git checkout with the correct remote.
- Adjacent consumer repositories are read-only. Product behavior, generated bindings, and third-party source are outside the modification scope.
- Current documentation and AI workflow become English. Historical records and original evidence retain their bytes and conclusions.

## Scene task migration and search correction, 2026-11-11

Docs commit `f05537abc432759a9e1ff61dab24a91ce1db9420` adds the complete reviewed Scene batch: four original pages mapped to two project entries, seven task/landing pages, architecture and host-state guidance, and 310 exact declarations across 31 type pages. Six example groups compile through actual host/trimmed-script references; their unchanged page and reference inputs remain valid after the query-only tooling update. The [Scene acceptance report](https://github.com/Wolfiora/InnoEngine.Docs/blob/f05537abc432759a9e1ff61dab24a91ce1db9420/.agents/records/acceptance/SCENE_2026_11_11.md) identifies snapshots, source captures, semantic boundaries, actual visual inspection and remaining gaps. Final local source/rendered projection passes for 855 pages and 4,834 bound engine declarations, with 83 entries and 169 still missing. The site built in 21.44 seconds; no full performance or deployment certification is inferred.

Engine commit `18da08eaecb12d89b816166bae8d9480683f6948` corrects the observed natural-language search tie: `instantiate prefab` now returns the Manual task before the method reference. All 37 query regressions pass and the Build CLI compiled with zero warnings/errors. Exact symbols and identifiers retain reference priority; queries do not prepare Native inputs. The original broad validation selection, old Wiki cutover and remote CI/publication requirements remain unresolved. No repeated tests were triggered merely by these ledger/commit updates.

## Required outcomes

1. Inventory all current documentation, rules, project ownership, links, and evidence before migration.
2. Deliver a Godot-inspired English Manual, task navigation, architecture explanations, extension guides, and exact API reference in the Docs repository.
3. Build and deploy the Astro/Starlight site to GitHub Pages using the existing licensed InnoEngine branding.
4. Extract `Inno.Tooling.Documentation`; separate immutable snapshot publication from read-only queries. Keep the Build CLI and provide structured contracts for a future MCP adapter without implementing an MCP server.
5. Bind documentation to engine revisions and API/project manifests. Implement paired PR verification and independently verified publication.
6. Translate active rules and skills without losing original rule identities or requirements.
7. Refine validation obligations, tool input closures, build/preparation reuse, and independent execution dependencies. Preserve required domain, contract, integration, Native, product/device, and performance evidence.
8. Complete engine and Docs CI, scheduled certification, draft release verification, and actual deployment. Preserve user-controlled formal releases and PR merges.
9. Remove the old engine documentation tree and all replaced implementations after their callers migrate.
10. Deliver complete migration/coverage records, meaningful tests, raw performance samples, visual acceptance, real CI/deployment evidence, and explicit external blockers.

## Implementation ledger

| Phase | State | Evidence |
| --- | --- | --- |
| Workspace and baseline confirmation | Complete | Clean `ai-pipeline` at the approved baseline; no additional worktree created |
| Docs repository creation | Complete | Public repository created through authenticated GitHub API; administrator access confirmed |
| Full migration inventory | Captured; review pending | 686 inventoried files, 437 product Markdown pages, 249 baseline projects; individual dispositions still require review |
| English content and website | In progress | Initial task pages and Astro/Starlight build work; Pages Actions configuration exists, but no deployment or complete coverage is claimed |
| Query core and immutable snapshots | In progress | Independent library and Build route implemented; 14 snapshot regressions pass. Old validator/catalog consumers remain to migrate |
| English rules and workflow | In progress | Root instructions, CONTRIBUTING and four skills are English. All 58 complete units are English, including 57 reviewed translations preserving all 241 original rule identities. Cross-repository documentation-rule ownership remains outstanding |
| Incremental validation and CI/CD | In progress | Actual tooling input closures and explicit failure prerequisites implemented; 49 planning regressions and focused execution tests pass. Full CI/CD certification remains outstanding |
| Legacy removal and final acceptance | Pending | Old content remains until verified migration |

## Current decisions

- Reuse the clean engine checkout and empty Docs checkout; do not create another engine worktree.
- Keep query and API extraction implementation in engine tooling. The Docs repository owns prose and presentation.
- Use existing XML/API/project facts, never a second API extractor or a hand-maintained full project graph.
- Prefer exact snapshot queries; preparation/network access is explicit.
- Use staged paired PRs; do not automatically merge either repository.
- Bootstrap website publication may target the approved development branch, clearly labeled development documentation.
- Preserve the original rule source hashes; an English target hash alone cannot prove semantic preservation.

## Validation and artifact policy

Record checks with their input identities. Do not rerun solely for commits, pushes, report edits, or session recovery. Preserve first failures and retry reasons. Keep generated output within declared owners; clean only this task's temporary artifacts and respect active readers. Record cold measurements separately and retain at least five comparable warm samples for measured scenarios.

Cross-repository GitHub App installation/credentials, branch protection, and Pages configuration must be verified when configured. Administrator repository access does not by itself prove that an organization App is installed.

## Local foundation evidence, 2026-11-11

- `Inno.Tooling.Documentation.Tests`: 13 passed, no skips; TRX under `artifacts/acceptance/english-documentation/query-tests/documentation-snapshots-03.trx`.
- First test failure is preserved in `documentation-snapshots.trx`: two assertions incorrectly treated `InvalidDataException` as an `IOException` subclass. The negative fixtures now assert the exact appropriate exception for missing/cyclic includes and path escape; the rejection behavior was retained.
- Build entry compilation passed with zero warnings and errors. Agent-document checks and whole-repository source architecture checks passed. Compiled API, full Wiki coverage, Native and product verification were not represented as passing.
- Rule loader still verifies all 241 original rules and 58 complete units. Root instructions are 4,141 bytes. This initial observation predates the complete translation recorded below.
- Website static/type checks and three Markdown link tests passed. Initial browser inspection verified home navigation and Missing search; the complete visual/accessibility matrix remains outstanding.
- Pages uses custom Actions at `https://wolfiora.github.io/InnoEngine.Docs/`. Configuration is not deployment. No current website certification is claimed.
- The system .NET host lacks the required runtime. The existing user-local SDK at `C:/Users/23842/.dotnet/dotnet.exe` was selected without modifying global settings or installing another SDK.
- `documentation-query-before.json` retains six old-entry observations: first 4,927.50 ms, warm median 1,361.81 ms. Its binary/source correspondence is explicitly unverified, so it is diagnostic evidence rather than a controlled comparison.
- The host Python validator could not run because `python` resolves to the Windows application alias. Rule/skill structural checks and manual workflow review are separate evidence, not a substituted Python pass.

## Remaining work at this checkpoint

The implementation is intentionally incomplete: full English content and API migration; exact source/reference binding and export; source-validation evidence status; explicit remote snapshot preparation; common parser and validator migration; cross-repository rule authority; precise validation dependency refinement; paired PR and App setup; actual CI/Pages/draft-release acceptance; old documentation/query removal; comparable performance samples; full visual review and final migration reconciliation. The previous product documentation remains until its reviewed destinations and callers are migrated.

## Clause preservation and real query follow-up

- Four rule files now contain English delivery, notification, tests, compilation, completion, review, scripting and plugin units. Every nonblank source line and English counterpart is mapped; original numbered rules retain their hashes and identify the translated clause. The review record documents preserved obligations and the compilation wording interpretation. Remaining Chinese rules retain their original full-text verification.
- The complete agent-rule regression script passed, including changed English clauses, missing mappings, changed source mappings, missing or escaping review records and lost original-rule associations. All 241 original rules and 58 units remain registered. Translation hashes detect changes; they do not establish semantic equivalence without the recorded review.
- A real CLI query exposed Markdig inline containers being used as titles. Extracting plain heading text fixed task ranking. Fourteen documentation snapshot tests passed in `documentation-snapshots-06.trx`; a rebuilt Build entry returned the Console presentation task first for `Console presentation`.
- The explicit refresh published 20 pages under snapshot `7a1bfa613d31dea914313074af8889f090c65dcfca044720d05289de072d5f36`. The source API status remains Unverified. This is a representative content slice, not full migration acceptance.
- C-drive free space was approximately 45.9 GB at this checkpoint. Rule-test fixtures were cleaned by their existing guarded ownership cleanup. No adjacent workspace or shared cache was removed.

## Complete English rules and validation dependencies

- All 58 units are English. The 57 translated units preserve original source hashes, all 241 rule identities, every nonblank clause, nested indentation and the separately approved addition. The full rule regression suite passed. Semantic review is recorded in the linked decision; hashes are change detection, not a substitute for that review.
- Active record indexes, current acceptance and the open-item ledger are English. Historical dated reports and evidence remain original; two incoming links were updated for the translated current headings without changing historical conclusions.
- Validation now blocks only explicitly dependent work after a check failure. Independent contexts continue; aggregate failure, cancellation and input integrity remain enforced. Builds explicitly depend on instruction and architecture policy checks.
- Tool source inputs follow the evaluated validation project reference closure. Tool binary inputs follow actual assembly references instead of every DLL beside the CLI. Specific translation review records remain policy inputs; arbitrary records do not become global tool inputs.
- Six focused dependency/environment tests passed, followed by all 49 planning regressions (5 m 33 s). Documentation classification tests initially passed six cases and failed one assertion expecting the base JSON exception instead of its concrete subtype. The planner now reports an actionable InvalidDataException with the original JSON exception; the negative test passed on its explicit retry. First-failure TRX remains retained.
- Project manifest JSON now uses deterministic LF on Windows and Unix. Three project reference export tests passed. The Build entry compiled with zero warnings and errors.
- One overlapping local test/build invocation caused two output-file copy retries while the prior test host held the assembly; the subsequent build succeeded. Test host execution and output mutation must remain sequential for this output directory.
- Local TRX files are under artifacts/acceptance/english-documentation/query-tests. These checks do not certify Native products, Windows/macOS CI, paired PRs or publication.
## Shared catalog and website projection

The immutable query session now exports complete canonical page, project, section, symbol and contract metadata through ExportCatalogAsync and Build docs catalog. Website checks consume this result without reparsing Markdown. The writer validates source-relative/reference links and exact canonical anchors before publication; invalid links preserve the previous generation. Twenty snapshot/catalog tests passed, with four exact link-failure assertions subsequently verified. The Build entry and source architecture check passed. Source API status remains Unverified, and missing project entries remain explicit nulls rather than disappearing from coverage.

## Portable query protocol identity

An attempted measurement found that the CLI could not read a snapshot produced by the site adapter: the MVID-based schema included compiler/commit provenance. The first failure was InvalidDataException: Documentation snapshot integrity or schema mismatch; explicitly rebuild its index. No timing from that failed attempt is a successful query sample.

Protocol identity now hashes normalized authored query/parser/model inputs and their project dependency declaration, plus the explicit format version. Embedded inputs exclude generated assembly metadata and the CLI adapter. This preserves invalidation for actual query-tool edits while avoiding invalidation caused solely by commit metadata, output paths or adapter-specific compilation. All 20 snapshot tests passed after this change. Cross-adapter execution is verified separately below.

## Measured query and remaining extraction migration

The CLI successfully read a snapshot prepared by the independently compiled site adapter after the protocol identity correction. Six process observations (first plus five warm) are retained in DOCUMENTATION_QUERY_OBSERVATIONS_2026_11_11.json. Warm median: 180.9186 ms, including CLI process startup; each result returned five sections and 4,611 UTF-8 bytes, with extending.console-style first. This is a 22-page input, not a full-library benchmark or a controlled old/new comparison.

A bounded existing Wiki extraction for Core Identity evaluated six SDK nodes and exported 42 source symbols. The overall command failed: an incoming old open-item anchor was stale, and the two new documentation tooling projects have no old-engine Wiki pages. The anchor was corrected. Do not create duplicate old-engine pages to hide the ownership migration; the validator still needs its explicit external Docs catalog integration. The first extraction log and facts remain under artifacts/acceptance/english-documentation. Extracted declarations do not turn the failed overall command into passed API evidence.

## API extraction independent of legacy pages

Build docs export-api now invokes the existing evaluated CLR extractor directly with explicit project paths. It does not require a Wiki destination, build selected products, or claim script projection or behavioral certification. Incomplete or unresolved facts cannot replace a previously valid export.

The existing extraction/Wiki regression run passed 42 tests; the new fixture initially failed because it lacked the repository root marker. The fixture was corrected and its focused retry passed (documentation-api-extraction-02.trx). Both results are retained. The Build entry compiled with zero warnings and errors. A real Core Identity capture evaluated six nodes (0.263 s evaluation; 0.482 s capture) and exported 42 symbols. Their identifiers, signature hashes and XML contracts matched the earlier extractor exactly. This removes a legacy page dependency, not the remaining external catalog, source certification or full content migration obligations.

API facts additionally retain Roslyn symbol kind, containing type, base type and direct interfaces. These relationships are derived from the same compilation, not reconstructed from documentation titles. All four reference export tests passed after the addition (documentation-api-extraction-03.trx); the Build entry compiled with no warnings or errors.

## Canonical API declaration comparisons

The canonical catalog now includes a symbol's signature identity and visible C# declarations parsed from its actual section or legacy table row. Site consumers can compare exact nullable, overload and default-value signatures against bound extraction facts without introducing another Markdown parser. Twenty-one documentation regressions passed, including a member-section fixture proving that adjacent examples are not included in a symbol's contract and associated ownership continuations remain present.

The full Core Identity narrative review found three source XML inaccuracies/omissions: omitted Register overrides retain the object's existing persistent ID; count observes weak registry entries rather than proving every object is live; and Register/Unregister expose null/conflict failures. XML now describes these existing behaviors, and RuntimeIdentity explicitly distinguishes equality from liveness. No implementation or public signature changed. The English project, task and type pages preserve these constraints.

The canonical Markdown boundary now rejects executable and presentation HTML in ordinary content, allowing only inert comments and explicit API anchors. Layout remains in the website theme. All 25 snapshot tests passed, including script/image-handler/iframe cases that preserve the previous immutable generation. This prevents imported API prose from introducing an executable HTML surface.

## Large reference search isolation

Search terms now contain each section's own content and owner metadata instead of duplicating every symbol on its type page into every section. Exact XML identifiers retain their direct symbol lookup. The focused regression passed (documentation-section-search.trx), proving that a member-name query does not return unrelated sibling sections. Large SDL reference measurements follow separately; this change does not certify the remaining content migration.


## Native documentation migration checkpoint

Docs commit c55dcb9cb9f0244dabed5de1a329e2a07f0f200e maps 17 baseline SDL3/LibraryLoading pages without removing any of their 6,370 ordinal symbol identities or changing signatures. English shared contracts, original SDL license bytes, exact source XML and member-family pages preserve ownership and ABI limits. The combined 3,357-page site passed all rendered-anchor and bound-declaration comparisons (6,412 symbols including Identity), eight reference-writer tests and fourteen link/projection tests. Source API remains Unverified; other project coverage, platform execution, pairing, publication and legacy cutover are still required.

Five prepared Console search observations had a 667.1395 ms median including CLI startup, returning 4,611 bytes and the correct task first. The first large index and catalog publication took 67.329 seconds for 3,044 parsed plus 313 reused pages. These are current observations, not a controlled pre-migration comparison. No Native preparation ran for reference/site work. The latest measured C-drive free space was 41.69 GiB. A previously rejected cleanup of owned example bin/obj remains unexecuted; no alternate deletion mechanism was attempted.


## Canonical example contexts and publication overhead

The canonical catalog exports fenced code with source positions, content hashes and exact declaration associations. Explicit exampleMode/exampleProjects metadata selects host or real script-projection references. Build docs validate-examples consumes these blocks through the shared library and the existing batched SDK graph; it does not parse Markdown, build products or execute snippets. Missing references/context and empty required discovery remain blocked; independent groups continue. Thirty-one snapshot tests and both example-validator tests pass. The first example test exposed an uncaught InvalidDataException at the group boundary; the correction and first failure are retained. The Build entry compiled in 27.35 seconds with no warnings or errors.

Immutable page publication now confirms an unchanged owner registration without rewriting its timestamp or weakening payload hashing, dependency protection and leases. Seventy-three cache tests passed initially; one new fixture had an invalid workspace/root relationship, and its corrected focused retry passed. All 26 then-current snapshot tests also passed. Same-content 3,497-page confirmation samples are retained in Docs INDEX_PUBLICATION_2026_11_11.json: five-sample median 45,476.5184 ms before and 6,099.7506 ms after (86.59% reduction). The slow first after-sample remains included. These index measurements exclude catalog export and process startup; they do not certify source APIs or the uncompleted cross-platform pipeline.

## Managed authoring command separation

Real example acceptance exposed an unrelated Native producer: scripts and import-sample prepared BGFX Shader tools before opening the authoring workspace. The workspace now owns authoring independently; game/plugin commands explicitly compose their product pipeline after Native preparation. Managed authoring commands select the SDK directly, reject obsolete Native tool-target/support-pack arguments, and do not sweep unrelated engine product caches. Cancellation is propagated through authoring compilation and script-output publication. No product API signature or engine runtime implementation changed.

Four existing CLI boundary test classes moved from the Native product integration owner to Inno.Build.Cli.Tests, preserving assertions. The new managed suite passed 19 tests; focused script ownership/cancellation acceptance passed after adding an unrelated broken product-cache fixture. A real documentation Mover script then compiled through the production scripts --ide path and generated actual Runtime/Editor references without Native preparation. All 11 current documentation example groups passed against these explicit references, including the corrected Panel context (Core plus Editor ImGui). Prepared-reference compilation remains distinct from source certification and device execution.

An additional one-page index update during authoring/build work took 83.106 seconds (80.023 seconds page preparation); retain this observation alongside the controlled-content warm samples rather than treating the 6.1-second median as an upper bound. Host memory had roughly 1.8 GiB free with 15 reusable MSBuild nodes using about 2 GiB; this observation is not proof of causation. Further index IO and bootstrap resource work remains. No unrelated build-server process was terminated.

## Editor and tooling content checkpoints

Docs commit 7e6666e preserves ten Editor baseline files and 895 exact symbols through task guides, explanations and type references. Eleven example groups compiled against their explicit host or real script contexts. The Panel task and API were inspected at desktop, tablet and mobile widths in light/dark modes; member anchors and visible retirement obligations were retained. This is representative review, not full accessibility or engine execution certification.

Docs commit b980236 binds source 417557eb and the refreshed 252-project inventory. One 129-node capture confirmed all previously bound Identity/Editor/Native declarations, XML and source locations, and added 179 tooling declarations. Three legacy project narratives have reviewed English destinations. The 3,527-page website and all 7,486 bound declarations passed projection checks. Only 14 project entries were covered at that checkpoint. Full migration, external validator cutover, legacy removal, source certification, CI pairing and deployment remain unfinished.

Docs commit 41e0ee4 separates captured API source provenance from a new documentation candidate. Nineteen reference-writer/projection tests passed, including unchanged page bytes and timestamps after candidate-only rebinding. Captures do not certify a new engine tree; matching compatibility evidence is still required. A retained-index observation took 27.393 seconds despite parsing no pages, so variable registration IO remains an open performance observation.

## Task heading ranking

The Input task journey exposed a ranking defect: omitting punctuation and the word "and" from a task title placed an assembly overview and a large API index ahead of the task itself. Ranking now compares normalized heading terms and removes the unconditional project-page bonus. Exact symbol, project and alias paths remain; each hit explains its actual match. All 31 existing documentation tests passed. The new fixture first failed because it overwrote its shared ownership heading; retaining that necessary fixture contract made its focused retry pass. Both results are preserved. Actual prepared-site query acceptance follows this committed implementation; no engine or Native preparation is added to reads.

## Input, Storage and Audio content checkpoints

Docs commit bf2a8b6 maps sixteen additional baseline pages across Input, Storage and Audio. Their 744 exact declarations retain original identifiers and signature hashes. Real Runtime-script examples passed for frame input, complete-value storage, audio playback and mixer configuration. The combined site has 3,648 pages and 8,230 bound declarations, all passing canonical source/rendered projection. There are 25 project entries out of 252; this is not complete engine coverage. Required cutover, source certification and remote pipeline acceptance remain open.

Five prepared Input task searches all returned the task first, with a 788.3154 ms median including process startup and a 4,250-byte five-result response. Raw samples remain in Docs INPUT_QUERY_2026_11_11.json. The Storage/Audio site build took 58.65 s including 35.65 s Pagefind work; this is separate from local read-only queries. Desktop/mobile/tablet inspections retained visible use contracts and independently scrolling signatures.

## Parsed-page and query identity separation

The former full query-source identity also keyed parsed pages, so a ranking-only edit reparsed every Markdown file. Page models now have a distinct ownership file and parser identity. A hash-verified older query manifest can supply page payloads only when parser, content and payload identities match. Current catalog validation and index construction still run; ordinary OpenAsync rejects incompatible query or parser generations. No compatibility forwarding API was added.

All 32 existing query tests passed. Both new old-generation cases initially failed because the manually constructed fixture omitted its generation lease file. Adding the fixture's own lease retained the exact invalid-schema assertions; both focused cases passed. They verify zero parsing/no payload rewrite for a query-only change, fresh parsing for a parser change, and a usable current snapshot after preparation. First-failure and corrected TRX files remain in the acceptance owner. CLI compilation and real catalog refresh are recorded with the committed implementation separately.

## MiniAudio, Animation and Text migration

Docs commit 8971f84 adds reviewed destinations for 43 more baseline files and thirteen project entries. All 3,674 corresponding declarations retain exact identities and signatures. The site now has 38 of 252 actual project entries and 11,904 bound declarations across 5,195 pages; generated native pagination is not a completion percentage. Source API status remains Unverified and the old tree remains until its callers and all content have migrated.

Text source XML now describes actual logical-pixel glyph offsets and UTF-8 backend clusters instead of mouse scrolling or generic value placeholders. Commit 4a9edbac changes only XML; executable source was compared unchanged. The actual seven-project extraction and Runtime-script example passed without Native preparation. MiniAudio and Animation examples retained their valid scoped evidence.

Upstream native Setext headings, framed tables and a literal OpenSL pipe caused four rendered-anchor failures, an incorrect task-search ranking and a hidden table cell. The reference writer now preserves their content in portable Markdown, rejects ambiguous columns and verifies actual Astro rendering. Ten writer tests and the focused rendering assertion passed. Eleven projection tests include exact original license bytes, normalization rejection and path boundaries; original MiniAudio license bytes are protected in Git attributes and were compared against the staged blob.

The final local site passed type checks and all bound declaration/rendered-anchor checks. Five prepared Audio query samples had a 1,022.4814 ms median including process startup, returning 4,088 bytes with the correct task first and no SDK/native/network work. Raw sample identities and retained first failures are in Docs acceptance records. Remote pairing, publication, complete migration and Windows/macOS acceptance remain unfinished.

## Assets and deployment content

Docs commit ab243fe3ab83031c51d2df5560565fcba4d3a88b maps seven original files to reviewed English task, architecture and API pages. All 646 baseline declarations retain their signature hashes; five host example groups compile against explicit existing references. Source/render projection passes for 960 pages and 89 project entries. Six new local browser cases pass. Prepared import-settings task queries have a five-sample median of 637.44 ms including process startup, with no SDK/Native/network work. The Docs acceptance record retains exact content identities, first failures and source-certification limits. Full migration and remote delivery remain incomplete.

## Complete test-owner guide migration

Docs commit 34c2a6ccee139522151edf14e3e8a16c575a46ad completes English entries for all 67 actual test projects. This continuation maps 53 original test pages, preserves 368 named scenario examples across 111 source files, and centralizes shared validation instructions without changing tests or classifying support libraries as exempt. The site has 144 project entries and 1,015 pages; source/render checks and twelve additional local Edge cases passed. Full source certification and remote pipeline acceptance remain incomplete.

Seven fixture source files now document their deliberate invalid registrations, collectible replacement values and discovery boundaries in English XML. Only comment lines changed. One bounded union build enabled XML generation and CS1572/CS1573/CS1591 errors: zero warnings/errors, 38.73 seconds, no Native preparation. API export initially rejected two unresolved Editor fixture signatures; restoring its six stale SDK dependency graphs resolved the actual reference closure, after which all fourteen support surfaces exported. First failure and successful preparation logs remain in the owned acceptance directory. The fourteen support project narratives and their exact API pages are the next content batch.

## Test support reference closure

Docs commit 50940c4b97f5e25dadcfd512a404a7137cf61dc3 completes the fourteen support/fixture entries, 94 unchanged declaration identities/signatures, 23 type pages and thirteen API indexes. Three host example groups passed. The final 1,066-page site covers 158 project owners and 5,574 declarations; 94 production/tooling entries remain. Actual prepared queries resolve identical module member identities to separate V1/V2 owners and explicit required-contract continuations. The original testing-category index and every baseline testing page now have reviewed destinations. Native preparation, source certification and remote publication have not been implied by these checks. Registered Docs cleanup retired another 39,261,562 logical bytes while retaining the current snapshot.
