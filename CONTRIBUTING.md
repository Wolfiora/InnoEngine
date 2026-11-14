# Contributing to InnoEngine

Use the [InnoEngine documentation repository](https://github.com/Wolfiora/InnoEngine.Docs) to learn the engine and its development setup. Product tutorials and explanations belong there; engine source, XML contracts, extraction and verification tooling remain here.

The [rulebook](.agents/rulebook/README.md) is the single authority for architecture, coding, testing, documentation and delivery requirements for people and agents. [AGENTS.md](AGENTS.md) explains how to select complete rules by changed paths and behavior.

Before implementation, identify the owner, observable behavior and finite acceptance scope. Inspect the reasons in Build `validate plan`, then execute missing or invalid required checks. Prepare an explicit documentation snapshot with `docs index --docs-root <checkout>` and query it with `docs search/read/symbol`; read related contracts and continuations before relying on an API.

For an API change, maintain its source XML and run the applicable engine checks. After a human merge to `master`, independent API export synchronizes eligible C# contracts and actual script projections into the Docs repository's persistent `draft` branch. This does not wait for unrelated full-engine acceptance. Backend, extern, test and fixture API references are excluded; their necessary engine tests remain.

Maintain tutorials and Manual changes separately in Docs, starting from `draft`. API types and members each have their own Markdown: `source: xml` (including an absent marker) is synchronized; `source: manual` preserves that symbol's prose. Generated declarations remain authoritative. Incomplete manual explanations produce non-blocking editorial warnings. A human merges Docs `draft` into `main` to publish the validated website. Engine PRs do not require paired Docs metadata.

Keep implementation and acceptance records in [.agents/records](.agents/records/README.md). Write active guidance and contributions in English; preserve historical evidence in its original form. Delivery must distinguish checks actually executed from work still outstanding.
