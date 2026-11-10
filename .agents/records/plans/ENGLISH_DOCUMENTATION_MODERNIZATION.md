# English documentation and workflow modernization

Status: implementation in progress. This record does not certify completion.

## Authorized baseline and ownership

- Engine baseline: `a4e44769715c0577fb0a4c13b2d14ad998ddd8e1`, branch `ai-pipeline`.
- Engine workspace: `C:/Dev/GameEngineDev/InnoEngine`; initially clean.
- Documentation repository: `Wolfiora/InnoEngine.Docs`, public; default branch `main`, implementation on a migration branch.
- Documentation workspace: `C:/Dev/GameEngineDev/InnoEngine.Docs`; initially an empty Git checkout with the correct remote.
- Adjacent consumer repositories are read-only. Product behavior, generated bindings, and third-party source are outside the modification scope.
- Current documentation and AI workflow become English. Historical records and original evidence retain their bytes and conclusions.

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
