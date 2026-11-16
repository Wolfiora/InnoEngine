# Canonical documentation cutover

The engine's 437 remaining product Markdown files and the superseded Architecture documentation query/Wiki implementation have been removed. Product content belongs exclusively to InnoEngine.Docs. This is a local migration checkpoint, not overall completion, source API certification, hosted CI or publication evidence.

## Preservation and ownership

The existing baseline register retains all 454 original product-file identities: 364 reviewed destinations, 73 user-approved native/generated API retirements, and 17 byte-identical historical images. The Docs migration auditor now also rejects surviving original source paths. Twelve positive/negative migration tests and the actual full disposition audit passed. No new product documentation directory or compatibility forwarding entry replaces the retired tree.

127 historical Markdown link destinations now refer to the exact original Git baseline. Labels, conclusions, execution states and raw evidence are preserved. Rule 20.10 retains its source identity and requirements; its English destination now identifies the Docs Identity explanation and linked source evidence. Both the complete-unit translation mapping and original-rule mapping were updated for that reviewed authority relocation. The 241-rule checker passes.

## Regression responsibility after removal

| Retired responsibility | Current implementation and acceptance |
| --- | --- |
| Query-time index refresh, search, sections, symbols, project page guessing | `Inno.Tooling.Documentation`; 50 snapshot/query tests, including explicit preparation, immutable reads, project inventory moves, orphan symbols, duplicate identity, complete continuation, corruption and interrupted publication. |
| Wiki project/signature tables and separate product Markdown parser | Canonical catalog plus Docs projection checks; actual source contracts remain extracted by `DocumentationApiExtractor` and compared through `DocumentationReferences`. No source symbol is inferred from a page title. |
| SDK-selected sources and compile-only contracts | `DocumentationSourceContractTests`: imports, defines, removed files, selected generated inputs, nested enums, nullable/ref/record/generic/inherited declarations, compile-only NuGet references, missing references, invalid language versions and foreign target rejection. Failed extraction preserves previous facts. |
| Wiki executable examples and logical script projections | `DocumentationExampleTests`: owning compile references, unrelated assembly rejection, malformed unused XML, tilde/long fences, nested non-code fences, explicit host/runtime/editor contexts, both script hashes and host leakage rejection. Unclosed fences now fail canonical index publication. |
| Combined Wiki/architecture output selection | `ArchitectureOutputOwnerTests` now calls source reference export/comparison directly and retains evaluated output-owner and public-boundary assertions. |
| Product content boundaries | Docs projection tests: 29 cases passed, including migrated consumer/obsolete-command negatives and ordinary English Editor prose. Actual existing site projection passed before the subsequent parser schema change; refreshed site evidence is separate. |
| Rule, skill and record links | `AgentLinkValidator`, using Markdig link/heading syntax; reference links, explicit anchors, fenced examples, missing paths and missing anchors are tested separately from product content. |

Old `--wiki`, `--wiki-only`, `--wiki-contracts-only` and `--documentation-only` options now fail explicitly. Existing `docs` operations and unified validation own product checks. There is no forwarding compatibility wrapper. SDK and script-reference helpers were retained under their documentation responsibilities.

The former two fixture classes were removed only after ported and existing canonical checks passed. Behavioral changes required by the approved plan are explicit: reads never refresh mutable inputs, pages require one H1, required empty example discovery blocks certification, and missing prerequisites cannot produce an empty green inventory.

## Local evidence and limits

- `query-cutover/legacy-removal.trx`: 38 targeted Architecture tests passed, zero skipped, about 2 minutes.
- `query-cutover/canonical-query-final.trx`: 50 query tests passed, zero skipped, about 8 seconds.
- `cutover-source-policy.log` and `cutover-agent-links.log`: current source policy and agent document validation passed.
- `query-cutover-rule-check.log`: original rule identities and complete translated units verified.
- Docs `migration-cutover.json`: all 454 dispositions and preserved evidence verified; `sourceApiStatus` remains `Unverified`.

Initial failures remain in their original logs. The script fixture initially compiled against runtime assemblies instead of SDK compile references; its correction preserves the real reference boundary. A missing-package assertion was corrected to the actual unresolved public-signature diagnostic. No failed result was overwritten or relabeled.

The unrelated Editor Assets working-tree edit is untouched. No native product build was required. C-drive free space was approximately 34.1 GiB during this checkpoint; task output cleanup remains subject to registered ownership and reader protection.

Remaining work includes refreshed tool API facts/site projection, source certificates and compatibility carry-forward, precise behavioral impact selection, unavailable target captures, live CI, cross-repository App configuration, Pages promotion and draft-release acceptance. These remain actionable work or explicit external prerequisites; this checkpoint does not close #75.
