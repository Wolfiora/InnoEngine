# Contributing to InnoEngine

Use the [InnoEngine documentation repository](https://github.com/Wolfiora/InnoEngine.Docs) to learn the engine and its development setup. Product tutorials and explanations belong there; engine source, XML contracts, extraction and verification tooling remain here.

The [rulebook](.agents/rulebook/README.md) is the single authority for architecture, coding, testing, documentation and delivery requirements for people and agents. [AGENTS.md](AGENTS.md) explains how to select complete rules by changed paths and behavior.

Before implementation, identify the owner, observable behavior and finite acceptance scope. Inspect the reasons in Build `validate plan`, then execute missing or invalid required checks. Prepare an explicit documentation snapshot with `docs index --docs-root <checkout>` and query it with `docs search/read/symbol`; read related contracts and continuations before relying on an API.

For an API change, maintain its source XML and run the applicable engine checks. After a human merge to `master`, independent API export synchronizes eligible C# contracts and actual script projections into the Docs repository's persistent `draft` branch. This does not wait for unrelated full-engine acceptance. Backend, extern, test and fixture API references are excluded; their necessary engine tests remain.

Maintain tutorials and Manual changes separately in Docs, starting from `draft`. API types and members each have their own Markdown: `source: xml` (including an absent marker) is synchronized; `source: <author name>` preserves that symbol's prose. Generated declarations remain authoritative. Incomplete manual explanations produce non-blocking editorial warnings. A human merges Docs `draft` into `main` to publish the validated website. Engine PRs do not require paired Docs metadata.

Keep implementation and acceptance records in [.agents/records](.agents/records/README.md). Write active guidance and contributions in English; preserve historical evidence in its original form. Delivery must distinguish checks actually executed from work still outstanding.

API export first compares complete conservative committed inputs, then evaluates actual owner dependencies only where needed. Immutable extraction reuse retains its original producer revision; it is not a claim of execution at the new commit. A no-change package must not push Docs solely to merge main ancestry. Preserve the existing functional and product validation scope.

## Backend builds in CI

Local and hosted `validate plan` select the affected behavior from the evaluated SDK graph and Git merge base. A test project can declare `InnoValidationSubject` project items to identify the behavior it exercises. MSBuild compiles the full reference closure of each selected entry. In CI, the validation plan selects the required backend projects, product configurations and host tools. An independent managed change can finish on its planning runner. A matching platform ZIP supplies compiler references, runtime assemblies, native libraries and generated resources. Changed inputs schedule a producer for the affected platform. Product consumers still run their selected integration tests.

Branch pushes with an open PR delegate to its merge candidate. The merge queue and the landed `master` diff use the same test selection. In GitHub Actions, manual runs default to affected behavior. Run **Engine CI** with **full** enabled to check all registered platforms. To force fresh backend production, enter the target IDs from `.github/native-targets.json` in **rebuild_backends**, separated by commas. For example, `windows-x64` rebuilds that target. A feature branch retains temporary candidates. A successful default-branch run can promote verified candidates to **Backend Prebuilts YYYY.MM.DD.N**.

Each CI prerelease contains one ZIP per registered target. The file inventory inside the ZIP records the component identities and digests. The annotated Git tag records the release catalog. Identical platform bytes share a download-cache key across versions. Rotation keeps the current prerelease and two rollback versions. An interrupted upload resumes from its draft.

If acquisition fails, inspect the named target, required owner and input digest in the plan and producer log. Correct the source or rerun the responsible producer. The **Engine validation** check reconciles the frozen plan against the actual per-host results.
