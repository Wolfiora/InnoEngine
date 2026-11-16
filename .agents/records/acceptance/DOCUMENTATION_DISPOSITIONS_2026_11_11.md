# Documentation disposition audit — 2026-11-11

The existing [migration register](../plans/english-documentation-baseline.json) remains the single baseline inventory. The Docs repository's `scripts/check-migration.mjs` now checks that register against exact Git blobs, current English destinations, delegated per-source review records, bound native-scope ownership and archived payload bytes. It does not introduce another project graph or migration inventory.

The audit accounts for all 454 original `docs/` files: 364 reviewed narrative/reference destinations, 73 native/generated-reference retirements under the user's engine-only scope, and 17 unchanged historical images. The three platform narratives with unavailable target captures remain explicitly blocked for source certification. `DispositionVerified` is not API semantic certification, completion of physical legacy removal, or evidence of deployment.

The original screenshots had already been archived in `history/visual-evidence`; their bytes were checked again by the audit, not moved into a second archive. An attempted move precondition found the former directory empty before any mutation. The temporary duplicate README was removed. Original historical records remain unchanged.

## Provenance corrections

The earlier `sha256` values remain unchanged. Fourteen represented CRLF checkout bytes and fourteen differed from the declared Git baseline for other reasons. Every product entry now separately records `baselineBlobSha256` and `baselineBlobBytes`, computed from that exact baseline. Differing earlier values are explicitly described as checkout provenance and cannot stand in for baseline evidence.

The ImGuizmo retirement now records the same explicit user scope amendment as the other native retirements. Nine Input/Storage review destinations were encoded as space-joined strings; the Docs records now use actual destination arrays. The audit follows those existing reviews and verifies their exact source match and current destinations, rejecting cycles and missing entries.

## Evidence and limits

Docs artifacts `migration-disposition.json` and `migration-contracts-bound-scope.log` record the full disposition audit and eleven passing tests, including missing/duplicate origins, wrong Git bytes, changed historical payloads, unsupported retirement, missing/escaping/linked destinations, malformed or cyclic delegated reviews and substituted scope inventories. First unsuccessful audits remain in `migration-disposition-first.json` and the implementation record; no source/API evidence was promoted.

Run the audit explicitly from the Docs checkout with `node scripts/check-migration.mjs --engine-root <engine-checkout>`. It reads the selected engine's existing register and Git history. It does not compile engine products, prepare native dependencies, fetch repositories or run during ordinary documentation queries.

Old product files and legacy validator callers still require removal. Source certification, real platform CI, pairing/publication and external App provisioning remain outstanding.
