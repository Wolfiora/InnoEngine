# Shared documentation execution context

Real Engine CI run [43058546925](https://github.com/Wolfiora/InnoEngine/actions/runs/43058546925) first rejected missing immutable Docs candidate metadata. The PR was updated to the actual paired commit/tree and only failed jobs were retried with that stated reason. Attempt 2 passed candidate selection, pinned SDK setup, explicit source preparation and managed/native graph evaluation on both Windows and macOS.

Both hosts then rejected the same invalid plan: `documentation-examples:managed` depended on a build assigned to a later native context. A Host example shared its owner with a Script example. Build selection already promoted that owner, but example grouping did not follow the shared output context. No product compilation or green aggregate was produced.

The planner now propagates native example context through the actual declared example-owner relationships until stable. Host and Script pages sharing owners use the same prepared references. A page with several owners prepares all of them in the same context; unrelated pages remain managed. It does not infer owners from namespaces or introduce another project/platform table.

Four targeted tests passed, including mixed Host/Script owners, a multi-owner Host example, an independent managed example, both fence syntaxes and script-only preparation. Every required prerequisite precedes its dependent check. The actual repository plan uses default-branch baseline `9d336f486076c8fa9e903c9ce7f1b5b6a414584f`.

The real current-workspace planning probe completed with 252 affected owners and 337 required checks. Raw evidence is under `artifacts/acceptance/english-documentation/shared-context-*` and `query-cutover/shared-example-context.trx`. Planning took approximately 56 seconds including current input hashing and both SDK graphs; it did not compile native products. The local workspace includes an unrelated Editor Assets edit, so this planning probe is not a clean hosted source certificate.

Docs run [43058659021](https://github.com/Wolfiora/InnoEngine.Docs/actions/runs/43058659021) independently passed clean managed bootstrap, npm tests, Astro check/build, canonical projection and all 72 browser journeys. Its completion gate correctly rejected the three still-missing platform API captures and Unverified source evidence. It did not publish a site. These blockers remain open.
