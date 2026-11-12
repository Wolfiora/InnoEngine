# Development rules

This directory owns requirements for changing the engine. Product design, mechanisms, usage and contracts belong to [InnoEngine.Docs](https://github.com/Wolfiora/InnoEngine.Docs). Task progress and historical evidence belong to [records](../records/README.md). Humans and agents follow the same requirements.

Run `tests/ci/Test-AgentRules.ps1 -Paths <repository-relative paths> -ShowInstructions` from the engine root to discover complete units. Add explicit units with `-Rules`, behavior tags with `-Behaviors`, and read stages with `-Stage`. [coverage.json](coverage.json) is the sole registry of authorities, sources, applicability and required relationships.

Behavior tags describe the work: for example, `new-boundary`, `public-api`, `ownership`, `serialization`, `reload`, `native`, `documentation`, `rules`, `skills`, `records` and `delivery`. Multiple paths, tags and dependencies may select one unit. Its complete body appears once with all selection reasons.

| Responsibility | Authority |
| --- | --- |
| Architecture, boundaries, reuse and project ownership | [Architecture](architecture.md) |
| Naming, XML, formatting, exceptions and concurrency | [Coding](coding.md) |
| Documentation, API, examples and content ownership | [Docs authority](https://github.com/Wolfiora/InnoEngine.Docs/blob/docs-modernization/.agents/rulebook/documentation.md); load with an explicit `-DocsRoot` |
| Tests, compilation, completion and review | [Validation](validation.md) |
| Commits, PRs and notifications | [Delivery](delivery.md) |
| Editor | [Editor](domains/editor.md) |
| Identity, Missing and reload | [Identity and reload](domains/identity-reload.md) |
| Scripting | [Scripting](domains/scripting.md) |
| Plugins | [Plugins](domains/plugins.md) |
| Rendering and frames | [Rendering](domains/rendering.md) |
| Builds, Native and platforms | [Build and Native](domains/build-native.md) |

Load complete sections. Combining units into a file does not require every task to load that whole file. The checker retains all 241 original rule mappings and supplemental source requirements. Updating a target hash is not semantic preservation.

English translations retain their original Git source identities and register every nonblank source/target line, including headings, examples and nested requirements. Original numbered clauses link to their English targets. The [translation review](../records/decisions/ENGLISH_RULE_PRESERVATION.md) records reviewed obligations and any interpretation. Automated coverage detects omissions and unregistered changes; the implementing owner remains responsible for semantic equivalence. Units not yet translated retain exact source-text verification.

Explicitly approved additional requirements may follow `<!-- approved-addition -->` in an existing unit. Register the approval and the addition's SHA-256 in `approvedAddition`, using LF line endings and trimmed outer whitespace. The loader checks the original body or its reviewed translation separately, checks the addition, then emits both completely. This mechanism permits additions only; it does not authorize deleting or weakening existing rules.
