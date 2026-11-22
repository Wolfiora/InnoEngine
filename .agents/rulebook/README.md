# Development rules

This directory owns requirements for changing the engine. Product design, mechanisms, usage and contracts belong to [InnoEngine.Docs](https://github.com/Wolfiora/InnoEngine.Docs). Task progress and historical evidence belong to [records](../records/README.md). Humans and agents follow the same requirements.

## Read the rules for a change

Run `pwsh tests/ci/Test-AgentRules.ps1 -Paths <repository-relative paths>` from the engine root. It lists the rule files that apply to those paths; read each listed file completely. Add `-Tags` for the kind of work and `-Rules` for a specific file. A rule ID is the file path under this directory without `.md`, for example `coding` or `csharp-style/03-naming`.

Tags describe the work: for example, `new-boundary`, `public-api`, `ownership`, `serialization`, `reload`, `native`, `documentation`, `rules`, `skills`, `records` and `delivery`.

| Responsibility | Rules |
| --- | --- |
| Architecture, boundaries, reuse and ownership | [Architecture](architecture.md) |
| Retired compatibility layers and schema versions | [No legacy compatibility](no-legacy.md) |
| Directories, solutions and project files | [Solutions and projects](projects.md) |
| Naming, XML, formatting, exceptions and concurrency | [Coding](coding.md) |
| C# names, layout, line breaks and idioms, with examples | [C# code style](csharp-style/README.md) |
| Documentation, API, examples and content ownership | [Documentation](documentation.md), owned by InnoEngine.Docs |
| Tests, compilation, completion and review | [Validation](validation.md) |
| Commits, PRs and notifications | [Delivery](delivery.md) |
| Editor | [Projects](domains/editor/projects.md), [state](domains/editor/state.md) and [UI](domains/editor/ui.md) |
| Identity, Missing and reload | [Identity and reload](domains/identity-reload.md) |
| Scripting | [Scripting](domains/scripting.md) |
| Plugins | [Plugins](domains/plugins.md) |
| Rendering and frames | [Rendering](domains/rendering.md) |
| Builds, Native and platforms | [Hosts](domains/build-native/hosts.md), [platform products](domains/build-native/platform-products.md) and [backend integration](domains/build-native/backend-integration.md) |

## Add or change a rule

Rules are plain Markdown. Edit a file or add a new one, then open a pull request; a human merges it. Record a lasting decision in [decisions](../records/decisions/README.md) when its reason will matter later.

Every rule file starts with front matter that states when it applies:

```markdown
---
paths: ["src/services/rendering/**", "tests/services/rendering/**"]
tags: [rendering]
---
# Rendering
```

- `paths` lists repository-relative globs that match the whole path. `**/` spans directories and `*` stays inside one name. Matching is case-sensitive.
- `tags` lists the work tags that select the file.
- Declare at least one of the two, each as a one-line list.
- Keep one topic per file and every file within 16 KiB. Split a larger topic into a directory.
- When routing changes, add a path that must select the file and one that must not to the [routing examples](../../tests/ci/Test-AgentRules.examples.json).

Run the script without arguments to check every rule file, the root instruction size and the routing examples.
