---
name: inno-change
description: Scope an InnoEngine implementation by actual owners, behavior changes, applicable rules and finite acceptance.
---

Confirm the branch, workspace changes and implementing owner; protect other sessions. Run `tests/ci/Test-AgentRules.ps1` from the engine root to select complete rule units by repository-relative paths and behavior tags. The [rulebook](../../rulebook/README.md) is their sole authority.

When task guidance or an API contract is needed, select a prepared documentation snapshot. Use Build `docs search/read/symbol`, then verify relevant source and actual project relationships. Reuse loaded content while its identity and scope remain unchanged. Refresh after relevant edits; a query never creates a Docs candidate or certifies source API correctness.

Identify behavior changes, lifecycle obligations, failure rollback, required callers and finite acceptance. Reuse existing Core mechanisms. Keep task context to owners, rules, documentation/source entry points, decisions and verification reasons. Ordinary local changes do not require another whole-engine audit.

Use `validate plan` for check selection; do not keep a skill-specific test list. A private presentation change still needs relevant behavior and lifecycle coverage; unrelated product obligations need an actual dependency or behavior reason. Maintain API XML in the engine; master-to-draft synchronization owns generated reference updates. Change the Docs Manual only when usage guidance changes, from its `draft` baseline. Update the existing [record](../../records/README.md) with scope, decisions, input identity, valid evidence, blockers and next action; track unrelated improvements separately.
