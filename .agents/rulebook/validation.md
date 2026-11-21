---
paths: ["tests/**", "**/tests/**", "AGENTS.md", "CONTRIBUTING.md", ".agents/**"]
tags: [tests, compilation, completion, review, new-boundary, public-api, ownership, delivery, rules, skills, records]
---
# Validation

## 8. Test boundaries

- Changes to regression, public-contract and validation-tool tests required by the task are permitted. Preserve existing assertions and failure scenarios. Unrelated test rewrites, weaker assertions, production entry points only for tests, InternalsVisibleTo and reflection bypasses are prohibited.
- Select tests from the actual impact graph, behavioral contracts and execution prerequisites. Test filters must not conceal native build dependencies.

## 9. Compilation safety
- Before ending a conversation, do not leave visible compilation errors introduced within the changed target scope, including non-test code.
- Explicitly identify potential compilation risks in the delivery description.

## 10. Tests and acceptance

- Use real public contracts and replaceable public boundaries.
- InternalsVisibleTo, test backdoors, reflection bypasses and widening internal visibility for tests are prohibited.
- Cover real success, failure, cancellation, boundary and lifecycle behavior. Do not write tests that merely repeat the implementation.
- Native calls, AOT, trimming and platform support require actual artifact evidence.
- Report build, test, SDK, host and device status separately. Unexecuted checks are not passes.

## 19. Completion requirements for new systems
- Define clear assembly/domain boundaries and minimal public entry points before implementing UI or platform adapters. Isolate platforms, storage, compilers and presentation behind replaceable contracts. Do not accumulate temporary workflows in Panel, Application or static utility classes.
- Prefer explicit base classes or interfaces for extension discovery. When inheritance or an interface completely identifies an extension and no additional declaration data is required, do not add a parameterless marker Attribute. Attributes carry parameters or explicitly optional semantics that type relationships cannot express, such as stable IDs, ordering, scope and multiple-instance permission. Never make inheritance and a parameterless Attribute two registration conditions that must be synchronized.
- Put a discoverable extension's immutable Stable ID in its protocol Attribute and read it in the Registry before constructing an instance. Do not require abstract/virtual/override instance members to return compile-time constant IDs, or declare an ID both on an Attribute and an instance. Base classes and interfaces express behavior; Attributes carry immutable discovery metadata such as ID, ordering, scope and multiple-instance permission. Only identities genuinely generated from runtime state, and excluded from type discovery and persistent routing, may be instance properties.
- Complete source, callers, project references, solution classification, public XML, documentation, success/failure/boundary tests and necessary build verification together by default. Do not leave placeholders, silent fallbacks, duplicate protocols or compromises deferred to a future refactor.
- Keep public APIs small and complete. Do not ask users to create companion assets, repeat manifests or change unrelated callers for information the engine can reliably derive. Explain each new public API's necessity and stable semantics in the delivery description.
- InternalsVisibleTo, test backdoors, reflection bypasses and widening internal member visibility to simplify tests are strictly prohibited. Test behavior through real public contracts and replaceable public boundaries only.
- Classify new projects and feature files by responsibility; match each filename to its primary type. A file contains closely related contracts or implementation only. Avoid ambiguous helper/internal directories and giant files containing unrelated types.

## 30. Semantic review and delivery

- Do not create or retain Agent.md, AGENTS.md or case variants in the BindGen-CS repository. Engine agent requirements belong only in the engine's own rules.
- Before every delivery, actively assess clear responsibilities, clean code, actual decoupling and extensibility. Inspect execution paths, dependency direction, input authority, failure handling, concurrency protocols and resource lifetimes. Directory names, type names and interface counts do not establish architectural quality.
- Check whether new or changed foundational capabilities can directly reuse existing Core IO, Events, Identity, Serialization and other contracts. Eliminate duplicate protocols, parallel managers and implementations while preserving domain boundaries.
- Verify extensibility through real callers, replaceable boundaries and applicable validation. Explain from actual code which owners must change to add a backend or platform. Unexecuted tests and abstract declarations are not evidence.
- Every final delivery must briefly state this review's findings, actual validation evidence and unresolved or unavailable work. If verification is incomplete, say so; do not claim perfection or that everything passed.
- Before ending execution, reconcile the issue's actual resolution criteria, all approved implementation and acceptance requirements, architectural clarity, foundational reuse and new regressions. When actionable work remains, update the remaining execution plan and continue automatically without requesting repeated approval. Deliver only after all applicable work is complete or external blockers that cannot be resolved independently are explicitly recorded. Stage progress or passing CI does not establish completion of the entire plan.

## 11. Change management

- Formatting-only changes preserve syntax tokens, execution order, contracts and diagnostics.
- Validate naming, responsibility and behavior changes separately from pure formatting.
- Public API, reference-boundary and generation-protocol changes also update consumers, indexes, examples and architecture checks.
- Before completion, ensure the target scope compiles and record validation commands, results and remaining limitations.
