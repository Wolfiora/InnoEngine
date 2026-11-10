# Contributing to InnoEngine

Use the [InnoEngine documentation repository](https://github.com/Wolfiora/InnoEngine.Docs) to learn the engine and its development setup. Product tutorials and explanations belong there; engine source, XML contracts, extraction and verification tooling remain here.

The [rulebook](.agents/rulebook/README.md) is the single authority for architecture, coding, testing, documentation and delivery requirements for people and agents. [AGENTS.md](AGENTS.md) explains how to select complete rules by changed paths and behavior.

Before implementation, identify the owner, observable behavior and finite acceptance scope. Inspect the reasons in Build `validate plan`, then execute missing or invalid required checks. Prepare an explicit documentation snapshot with `docs index --docs-root <checkout>` and query it with `docs search/read/symbol`; read related contracts and continuations before relying on an API.

For changes spanning engine and documentation, use paired PRs bound to exact inputs. An index or successful website build does not prove source API or executable example validity. Preserve matching engine evidence and publication order.

Keep implementation and acceptance records in [.agents/records](.agents/records/README.md). Write active guidance and contributions in English; preserve historical evidence in its original form. Delivery must distinguish checks actually executed from work still outstanding.
