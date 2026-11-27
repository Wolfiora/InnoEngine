---
name: inno-delivery
description: Review InnoEngine semantic risks and matching evidence, then prepare accurate issues or pull requests and diagnose CI failures.
---

Run `rules select --tags delivery,review,notification` and use the [delivery rules](../../rulebook/delivery.md). Review changed dependency directions, input authority, rollback, concurrency and resource retirement. Directory neatness and test counts do not prove correctness.

Reconcile the approved plan and every required result with current inputs. Distinguish missing implementation, unexecuted acceptance and external blockers. Report final behavior, actual checks, outstanding work and necessary action. Check commit subjects before pushing and issue or PR text before submitting against the delivery rules. Never promote historical evidence to a current pass.

Report API synchronization, Docs validation, manual editorial work and website deployment separately. API export tracks an exact master input; Docs can publish its already selected API version without waiting for a newer unrelated engine CI run. Draft previews never deploy. Only a human Docs `draft` to `main` merge authorizes automatic site deployment. Do not automatically merge either repository's PRs or formally publish an engine release.

Diagnose the first CI failure before retrying; preserve its evidence and the retry reason. Commit identity, pushes, publishing and external communication follow the delivery rules and existing user authorization. Local staged acceptance does not itself authorize a large remote matrix. Clean only owned temporary output through registered ownership and reading protections; retain useful caches, active readers and first-failure diagnostics.
