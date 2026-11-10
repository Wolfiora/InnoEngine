# InnoEngine agent instructions

Architecture, responsibility, dependency direction, low coupling and resource ownership take priority over convenience. Reuse Core IO, Events, Identity and Serialization. Do not introduce parallel managers, test backdoors, retired compatibility layers or implicit fallbacks. Source comments, public XML, active documentation and AI workflow guidance use English. Preserve historical evidence and legal text.

## Start a task

1. Confirm the branch, workspace changes, owner, behavioral scope and finite acceptance. Protect other sessions. Adjacent repositories are read-only unless explicitly authorized; InnoEngine.Docs owns product documentation.
2. Run `tests/ci/Test-AgentRules.ps1 -Paths <repository-relative paths> -ShowInstructions` from the engine root. Absolute paths are also accepted. Add behavior tags with `-Behaviors` or explicit units with `-Rules`; the [rule registry](.agents/rulebook/coverage.json) is the sole routing authority. Do not depend on a client discovering nested instructions.
3. Read all selected complete units, their sources and reasons. If the combination exceeds 16 KiB, read every reported `-Stage` in sequence; never truncate or skip requirements. Root instructions have an 8 KiB hard limit. Use `new-boundary` for new boundaries and `delivery` for delivery work.
4. Establish an explicitly selected Docs snapshot. Build `docs index --docs-root <checkout>` prepares it; `docs search/read/symbol` query the published snapshot. Reuse loaded rules and documentation while their identities and scope remain unchanged. Refresh after relevant edits. Missing or mismatched documentation is visible; never guess sibling paths or silently compile to verify an API.

## Task workflows

- [inno-change](.agents/skills/inno-change/SKILL.md): owners, behavior, applicable rules and bounded scope.
- [inno-verify](.agents/skills/inno-verify/SKILL.md): unified validation, failures and evidence identities.
- [inno-api-docs](.agents/skills/inno-api-docs/SKILL.md): actual symbols, use contracts, project documentation and examples.
- [inno-delivery](.agents/skills/inno-delivery/SKILL.md): semantic review, accurate acceptance and delivery.

Skills describe workflows; they do not replace rules or maintain test lists. Default to one implementer per owner. Parallel work needs independent file responsibility, workspaces and output ownership. Do not duplicate repository scans, builds or acceptance across agents.

## Authority and completion

- [InnoEngine.Docs](https://github.com/Wolfiora/InnoEngine.Docs) owns Manual, tutorials, explanations and reference prose. The engine owns source XML, project/API extraction and query tooling. Use exact snapshot and paired candidate identities.
- The [rulebook](.agents/rulebook/README.md) owns development requirements. Human contributors use the same rules through [CONTRIBUTING](CONTRIBUTING.md).
- [Records](.agents/records/README.md) hold necessary plans, lasting decisions and acceptance. A handoff retains scope, decisions, input identity, valid evidence, blockers and next action. Do not load historical records by default.
- Complete all applicable behavior, callers, references, XML, documentation and validation in the approved task. Record unrelated improvements separately. Required regression, contract and tooling tests may change; unrelated rewrites, weaker assertions, reflection bypasses and test-only production visibility remain prohibited.
- Evidence must match current inputs. Missing, skipped or unavailable checks are not passes. Commits, pushes, report edits and session recovery do not by themselves invalidate evidence. Reconcile the approved plan and report unfinished acceptance and external blockers accurately; continue actionable work.
- Reuse stable build directories by workspace, configuration and target; keep logs separate. Local output uses explicit C-drive owners. Clean only owned temporary artifacts through the existing lease, registry and retirement protocols. Protect other sessions, active readers, useful caches and first failures. Remote delivery follows user authorization and delivery rules.
