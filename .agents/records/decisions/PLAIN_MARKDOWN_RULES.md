# Plain Markdown rules

[Decisions](README.md) · [Rulebook](../../rulebook/README.md)

On 2026-11-21 the user retired the rule registry. It was built to prove a one-time migration and translation, and it had become the mechanism for every later rule change: a changed sentence needed a registry amendment, a recomputed hash and a decision record, and a new rule file needed two commits.

- Rules are plain Markdown. Each rule file states its own `paths` and `tags` in front matter; there is no central registry and no dependency list.
- A human merges every rule change through a pull request. That review replaces hash locking.
- `coverage.json`, its source identities, line hashes, translation mappings, amendments and approved-addition markers are removed without an archive or a compatibility path. Git history keeps them.
- The rule script selects files and checks front matter, the size limits and the routing examples. It needs no Git history and prints a file list instead of rule bodies.
- One topic per file within 16 KiB. The C# code style became one file per chapter; the Editor and Build and Native domains became directories; sections that former dependencies always loaded together now share a file.
- Paths are whole-path globs instead of unanchored regular expressions. Names such as `State` or `assets` outside their owners no longer select Editor state or identity rules. Editor state now applies to all Editor sources and tests, Editor UI to the presentation and panel directories, and identity rules also to references, extensibility reload and core identity.
- InnoEngine.Docs owns its rule files with the same front matter. The engine script reads them only from an explicit `-DocsRoot`.

Earlier decision records describe changes in registry terms. Their rule text remains in force; the registry mechanics no longer apply.

The same change removed environment-specific guidance: the local output drive, a platform sound path and a personal name used as an example.
