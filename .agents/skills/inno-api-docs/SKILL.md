---
name: inno-api-docs
description: Maintain InnoEngine design, usage, API XML and examples against actual public and protected contracts.
---

Select applicable `wiki` and `xml` units through the rule loader with `-DocsRoot <explicit-checkout>`. The [documentation rules](https://github.com/Wolfiora/InnoEngine.Docs/blob/docs-modernization/.agents/rulebook/documentation.md) have one authority in that checkout. Read its additional writing and presentation rules when editing authored content. Engine declarations and XML remain in the engine; narrative content belongs to Docs.

Use Build `docs search/read/symbol` against a prepared snapshot to locate task guidance, the authoritative project entry and exact qualified symbols. Read necessary associated contracts and explicit continuations. Search excerpts and API markers are not complete contracts or matching source verification evidence.

API reference covers public and externally usable protected C# contracts owned by src/ and build/, plus actual ScriptingApiScope projections. Engine API uses only src/ and build/; tools, entries, platforms, backends, extern, tests, fixtures and non-C# APIs are excluded. Preserve useful integration guidance and required Native implementation/deployment tests. Scope (Runtime, Editor, Authoring) and compiler surface are separate identities.

Verify actual public/protected declarations, project references, surfaces and callers. Maintain affected design explanations, mechanisms, tutorials and use contracts in clear English. Distinguish public promises from implementation details. Preserve ownership, threading, lifetime, failure, cancellation and release obligations. Split large pages by coherent tasks or types; retain stable identities, exact overloads and include ownership. Update incoming links and navigation when moving content.

Use the unified validation plan for API and executable example checks. Maintain source XML first; a human master merge triggers independent API export into Docs `draft`. Each type/member Markdown is XML-managed unless `source: manual` explicitly transfers its prose to the author. Never alter generated facts to change a declaration. Preserve manual prose and report contract changes for review. Missing editorial text is a neutral warning; corrupt facts, failed extraction and broken examples remain failures.

Manual and tutorial edits use Docs `draft` or a branch based on it. No paired engine PR metadata, full-engine source certificate or latest-master match is required for website publication. Record API sync, Docs checks, editorial work and deployment separately. Only a human `draft` to `main` merge publishes. Do not duplicate inventories or turn a private color change into an engine-wide rewrite.
