---
name: inno-verify
description: Plan and execute bounded InnoEngine validation; diagnose failures and reconcile evidence with current inputs.
---

Load applicable units from the [validation rules](../../rulebook/validation.md) through the rule selector. Use Build `validate plan/run/full/evidence`; inspect owners, reasons, environments, prerequisite artifacts and exclusions. The planner owns the check inventory.

Check evidence identities before executing missing or invalid work. Commits, pushes, report edits and session recovery are not reasons to repeat valid checks. Record the exact invalidating input. Preserve the first failure and retry reason; do not rerun until a run happens to turn green. Unknown owners, empty required discovery, skipped jobs and unavailable environments are not passes. Device, performance, stress and lifecycle checks follow their actual obligations.

Block work whose prerequisites failed; continue independent work when input integrity, environment and cancellation permit. Keep compilation, behavioral contracts, documentation/examples and product requirements distinct. Website rendering and prepared-snapshot queries do not require engine products or Native preparation. Missing reference or example evidence remains blocked.

Ordinary engine verification does not require a paired Docs candidate. Independent API extraction verifies complete source/XML/projection facts; Docs validation checks its selected API version and examples. `API editorial coverage` warnings are neutral and never substitute for or block these checks. Keep a tool crash, corrupt data, missing environment and an editorial reminder distinct.

Write statuses, exact input identities, failure causes, fixes and outstanding evidence to the existing [acceptance record](../../records/acceptance/README.md). Respect an explicitly local acceptance boundary; retain the unexecuted matrix as outstanding work. Historical evidence is not current certification.
