# Current acceptance

[Open work](../open-items.md) · [Implementation ledger](../plans/ENGLISH_DOCUMENTATION_MODERNIZATION.md) · [Historical evidence](README.md)

The English documentation and workflow modernization is **incomplete**. Existing engine/UI certifications remain bound to their original revisions and environments. This page does not promote them to the current working tree.

## Current local evidence

- [Owned cache retirement](OWNED_CACHE_RETIREMENT_2026_11_11.md) records obsolete Docs generations and inactive process payload cleanup, separate logical/physical measurements and the retained unverifiable metadata claim. Metadata-integrity failures now preserve completed retirement results; explicit cleanup still fails visibly. Existing 74 Toolchains regressions passed and both corrected failure fixtures passed without Native preparation.

- Independent documentation-query tooling and the Build CLI route are implemented. All 37 documentation regressions pass locally, including explicit preparation, stable reads, corruption, moves, includes, contracts, ambiguity, continuations, concurrent readers/writers, cancellation behind a protected page lease and natural-task ranking without weakening exact symbol lookup. The [bounded writer comparison](DOCUMENTATION_WRITER_CONCURRENCY.md) retains five before/after samples and their limits. [Deferred keyword preparation and task ranking](DOCUMENTATION_QUERY_PREPARATION.md) remove unrelated work from exact reads and record the still-broad validation selection.
- The current source-project inventory contains 252 projects. After the user-approved exclusion of backend-native/generated binding API documentation and the Scene/Assets migration, the external Docs candidate has 215 authoritative project entries and 9,621 bound engine/support declarations across 1,797 pages, including concise BGFX/ImGui/ImGuizmo integration boundaries. The remaining 37 project entries and complete source certification are unfinished. Native implementation correctness and deployment requirements are unchanged. All 67 actual test owners now have English behavior guides; all fourteen support/fixture libraries now retain their separate public API documentation. These counts describe migrated content, not an overall completion percentage.
- All 58 registered rule units have English bodies; 57 have reviewed translation mappings and one was already English. All 241 original rule identities remain registered. Rule-provenance, routing, complete-unit budgets and negative translation tests pass.
- The Docs repository has a working Astro/Starlight representative slice, local production output and initial home/search inspection. Complete page/API migration, visual acceptance, CI and Pages deployment are outstanding.
- Validation now records explicit prerequisites rather than blocking every context after an unrelated failure. Tool source inputs follow evaluated references; tool binary identities follow actual assembly references. Focused regressions and unresolved validation details are recorded in the implementation ledger.

Local evidence files are retained under artifacts/acceptance/english-documentation/. The ledger records exact filenames, first failures and retry reasons. This summary is not a replacement for those results.

## External setup and unexecuted work

Wolfiora/InnoEngine.Docs exists publicly, uses main and has Pages custom-Actions configuration and a required Documentation validation check. These settings are not a deployed or certified website. Repository-level Actions secrets/variables were empty when checked. Organization Actions-secret metadata access returned HTTP 403; an appropriately installed GitHub App and credentials remain unverified setup requirements.

The first remote migration candidate completed unsuccessfully: [CI run 42934085309](CI_42934085309.md) records the macOS old-Wiki migration failures and Windows runner communication loss. No passing Windows/macOS full matrix, paired-PR publication, actual Pages deployment or draft-release certification is claimed. No formal engine release or automatic merge is authorized by this summary.

## Preserved earlier evidence

The [pre-modernization acceptance summary](https://github.com/Wolfiora/InnoEngine/blob/a4e44769715c0577fb0a4c13b2d14ad998ddd8e1/.agents/records/acceptance/current.md) preserves every original result, count, revision and limitation in its original language.

Relevant original reports remain unchanged:

- [Logo and final Windows acceptance](LOGO_BRANDING_2026_11_07.md): the 2026-11-10 run recorded 329 successful required checks, including one reused result; three other-platform products were not applicable.
- [Editor UI final acceptance](EDITOR_UI_FINAL_ACCEPTANCE_2026_10_30.md): candidate dd5979c6 local UI implementation and applicable acceptance; overlapping profiles must not be added as unique test counts.
- [Exact PR #26 delivery evidence](https://github.com/Wolfiora/InnoEngine/pull/26#issuecomment-6516741338): original delivery commit and CI boundaries.
- [BGCS 4.1.2 consumer acceptance](BGCS412_CONSUMER_ACCEPTANCE_2026_10_25.md), [priority fixes](PRIORITY_FIX_ACCEPTANCE_2026_10_25.md) and [standard NuGet restore](STANDARD_NUGET_RESTORE_ACCEPTANCE_2026_10_26.md): separate original input and product scopes.
- [Previous documentation reorganization](DOCS_AGENT_INFORMATION_ARCHITECTURE_ACCEPTANCE.md): local staged delivery, not the new external website migration.

Manual Mac interaction, physical displays, OS IME, subjective usability and unexecuted device/long-running scenarios remain unverified unless a report explicitly supplies matching evidence. Historical benchmarks retain their original first failures and sample populations; no new performance claim is inferred from them.
