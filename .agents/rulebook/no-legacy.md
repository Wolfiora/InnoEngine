---
paths: ["src/runtime/plugins/**", "tests/plugins/**"]
tags: [no-legacy, new-boundary]
---
# No legacy compatibility

## 15. No legacy compatibility or schema versions
- InnoEngine is a personal engine whose current source and current Project data evolve together. New or changed features do not support old files, schemas, fields, cache directories, namespaces, APIs or serialization layouts. Do not add fallback readers, migrations, compatibility aliases, former IDs, deprecated wrappers or dual writes for them.
- Persistence models, Attributes, Importers, Build Processors, History, Workspaces, Scenes, Prefabs, Asset metadata, Catalogs and script manifests must not introduce legacy-adaptation fields such as `version`, `schemaVersion`, `formatVersion` or `formerVersion`. When a format changes, update the current writer, reader, tests, documentation and Project data directly. Old data may be explicitly invalidated and regenerated.
- Do not reserve migration branches for hypothetical future compatibility. Only an explicit user request to import a particular old format permits a one-time converter with a clear boundary. Conversion logic must not enter normal runtime paths.
- Update all callers when deleting or refactoring an API. Do not retain old overloads, namespace facades, forwarding types or `[Obsolete]` compatibility layers unless the user explicitly requests them.
- These rules permit runtime identities required for current correctness, including TypeCache/Assembly generations, concurrency revisions, change counters, content hashes, artifact fingerprints, MVIDs, job-handle generations and strict magic/headers used only to reject damaged or incorrect input formats. They must not become mechanisms for reading multiple legacy schema generations.
- When reviewing or cleaning implementations named `version`, `legacy`, `migration`, `compatibility`, `former` or `deprecated`, first determine whether they serve only old data/APIs. If so, remove them together with their tests and documentation rather than extending them.
