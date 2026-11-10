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
| Query core and immutable snapshots | In progress | Independent library and Build route implemented; 13 snapshot regressions pass. Old validator/catalog consumers remain to migrate |
| English rules and workflow | In progress | Root instructions, CONTRIBUTING and four skills are English. The detailed rulebook and translation-preservation mapping remain outstanding |
| Incremental validation and CI/CD | Pending | No new certification claimed |
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
- Rule loader still verifies all 241 original rules and 58 complete units. Root instructions are 4,141 bytes. Detailed rule translation has not yet started.
- Website static/type checks and three Markdown link tests passed. Initial browser inspection verified home navigation and Missing search; the complete visual/accessibility matrix remains outstanding.
- Pages uses custom Actions at `https://wolfiora.github.io/InnoEngine.Docs/`. Configuration is not deployment. No current website certification is claimed.
- The system .NET host lacks the required runtime. The existing user-local SDK at `C:/Users/23842/.dotnet/dotnet.exe` was selected without modifying global settings or installing another SDK.
- `documentation-query-before.json` retains six old-entry observations: first 4,927.50 ms, warm median 1,361.81 ms. Its binary/source correspondence is explicitly unverified, so it is diagnostic evidence rather than a controlled comparison.
- The host Python validator could not run because `python` resolves to the Windows application alias. Rule/skill structural checks and manual workflow review are separate evidence, not a substituted Python pass.

## Remaining work at this checkpoint

The implementation is intentionally incomplete: full English content and API migration; exact source/reference binding and export; source-validation evidence status; explicit remote snapshot preparation; common parser and validator migration; full rule translation/mapping; precise validation dependency refinement; paired PR and App setup; actual CI/Pages/draft-release acceptance; old documentation/query removal; comparable performance samples; full visual review and final migration reconciliation. The previous product documentation remains until its reviewed destinations and callers are migrated.
