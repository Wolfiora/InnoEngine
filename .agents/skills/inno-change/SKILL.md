---
name: inno-change
description: Scope an InnoEngine implementation by actual owners, behavior changes, applicable rules and finite acceptance.
---

Confirm the branch, workspace changes and implementing owner; protect other sessions. Run `tests/ci/Test-AgentRules.ps1` from the engine root to select complete rule units by repository-relative paths and behavior tags. The [rulebook](../../rulebook/README.md) is their sole authority.

Establish an explicitly selected documentation snapshot at task start. Use Build `docs search/read/symbol` to find task guidance and necessary contracts, then verify the relevant source and actual project relationships. Reuse loaded rules and documentation while their identities and applicable scope remain unchanged. Refresh after relevant edits; an index does not certify source API correctness.

Identify behavior changes, lifecycle obligations, failure rollback, required callers and finite acceptance. Reuse existing Core mechanisms. Keep task context to owners, rules, documentation/source entry points, decisions and verification reasons. Ordinary local changes do not require another whole-engine audit.

Use `validate plan` for check selection; do not keep a skill-specific test list. A private presentation change still needs relevant behavior and lifecycle coverage; unrelated product obligations need an actual dependency or behavior reason. The engine owns implementation/XML and InnoEngine.Docs owns product prose. Pin paired candidates rather than guessing sibling paths. Update the existing [record](../../records/README.md) with scope, decisions, input identity, valid evidence, blockers and next action; track unrelated improvements separately.
