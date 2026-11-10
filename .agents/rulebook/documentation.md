# Documentation

Load complete rule units. The [registry](coverage.json) owns routing and provenance. The approved two-repository migration supersedes the old local Wiki ownership and Chinese-language requirements; the remaining quality obligations below are preserved. External rule ownership and validator cutover remain explicit migration work.

<!-- rule-unit: wiki -->
<a id="wiki"></a>
## 11. Documentation maintenance
- Product and API documentation belongs to InnoEngine.Docs under content/en/, entered through its task-oriented home page. The old engine docs/ tree is removed only after its content and callers migrate.
- Organize documentation into About, Getting Started, Manual, Architecture, Extending, Platforms/Backends, API and Contributing. Every category has a useful landing page; assembly navigation is secondary.
- Every actual csproj has one authoritative project entry, identified by source inventory and explicit owner metadata. Do not infer ownership from translated titles or filenames.
- Project entries cover responsibility/boundaries, dependencies/initialization, all public APIs, important protected extension points for derived implementers, common workflows, compilable examples, errors/lifetimes/reload and related navigation.
- API declarations and examples follow current source. Do not describe internal implementation as a stable public contract. Explicitly identify internal mechanisms when explaining them.
- Adding, deleting, renaming or changing public API behavior updates the corresponding project entry and category navigation in the same paired change. New projects receive their authoritative entry and coverage registration.
- Use relative Markdown links between content pages. Project entries provide category and home/related-page navigation. Page moves repair every incoming link.
- Active prose is English under the approved modernization plan. API names, code, source comments and public XML remain English. Use small, complete composition examples instead of copying large source blocks.
- Distinguish currently supported behavior, internal details and future plans. Never present planned APIs as already implemented.
- Before extending documentation, inspect actual public types/members and relevant csproj dependencies. Afterward, reconcile Markdown links, navigation and source signatures.
<!-- /rule-unit: wiki -->

<!-- rule-unit: documentation-standard-0 -->
<a id="documentation-standard-0"></a>
# Documentation maintenance and verification

Use the [documentation repository](https://github.com/Wolfiora/InnoEngine.Docs), the [architecture overview](https://github.com/Wolfiora/InnoEngine.Docs/blob/main/content/en/architecture/overview.md) and the engine's documentation/architecture tooling as the content and verification entry points.
<!-- /rule-unit: documentation-standard-0 -->

<!-- rule-unit: documentation-standard-1 -->
<a id="documentation-standard-1"></a>
## One authoritative entry per project

Every actual project has one authoritative project entry and appears in its category navigation. Test-project pages explain verification scope, fixtures, prerequisites and execution rather than presenting test methods as product APIs. Current usage documentation belongs in InnoEngine.Docs; dated plans, execution records and historical acceptance remain in the owning repository's .agents/records/.

Explain responsibility, a quick workflow, initialization and shutdown before API detail, extension, failures and verification. Ownership descriptions identify creation, borrowing/transfer, threads and lifetime termination. Explicitly identify internal implementation details.
<!-- /rule-unit: documentation-standard-1 -->

<!-- rule-unit: documentation-standard-2 -->
<a id="documentation-standard-2"></a>
## API reference and source

Map public types, constructors, members, enum values and protected members usable by external derived types individually. Signatures preserve overloads, nullability, defaults and generic constraints; types also preserve inheritance, interfaces and delegate signatures. Synthesized public record members are in scope.

Each member describes declaration, behavior/results, usage constraints and failure information. Source links locate implementation but do not replace usage documentation. Resolve inheritdoc to the actual inherited contract, retaining local additions. Explain generated constants/members according to their real semantics without modifying generated source to add documentation.

The api comment marker associates a symbol with its signature for verification; it does not prove behavioral quality. Automatically refreshed reference data still requires ownership, threading and context review. An entry with no additional stated constraints does not imply thread safety or infallibility.
<!-- /rule-unit: documentation-standard-2 -->

<!-- rule-unit: documentation-standard-3 -->
<a id="documentation-standard-3"></a>
## Examples

- Executable C# examples use csharp fences and include required using directives, parameters and host prerequisites.
- Borrow Sessions, asset pipelines and build contexts through parameters rather than assuming undeclared globals.
- Script examples use logical namespaces and are checked against current trimmed script references. Do not make invalid examples pass by switching to implementation namespaces.
- Asynchronous examples explain owner-thread resumption and resource release. Ordinary await does not promise resumption on a specified thread.
- Explicitly identify declaration-only or procedural text. Do not convert failing executable examples into ordinary prose to bypass verification.
<!-- /rule-unit: documentation-standard-3 -->

<!-- rule-unit: documentation-standard-4 -->
<a id="documentation-standard-4"></a>
## Content boundaries

Explain capabilities through the engine's own public contracts, without depending on external plugin products, consumer-repository paths or their specific business models. The engine's Plugin mechanism, .iplugin, Source Mounts, Stable IDs and generations are current engine documentation topics. Browser Canvas and general canvas concepts may also be described normally.

Concrete third-party implementation names belong in their backend, Native, integration or toolchain pages. Neutral service pages explain capabilities and contracts. Deleted APIs, project paths and commands must not remain documented as usable.
<!-- /rule-unit: documentation-standard-4 -->

<!-- rule-unit: documentation-standard-5 -->
<a id="documentation-standard-5"></a>
## Large ABI references

Each Native project retains one authoritative entry explaining initialization, borrowed addresses, callbacks, threads and shutdown order first. Complete ABI declarations may be split by type/function in that project's .Api directory and included explicitly with api-include for signature checks. An include resolves to one Markdown page within the selected documentation snapshot; cycles and path escapes fail. Moves repair incoming anchors without copying another API authority. Pagination does not establish individually reviewed function behavior. Missing native semantics must still be checked against the current facade/header and actual consumption.
<!-- /rule-unit: documentation-standard-5 -->

<!-- rule-unit: documentation-standard-6 -->
<a id="documentation-standard-6"></a>
## Verification during the validator migration

The existing Build verify --wiki operation checks the pre-cutover documentation tree and may export --wiki-api <json> and --wiki-examples <json> inventories. --dotnet <absolute-path> and --project-graph <json> select effective MSBuild graph inputs. These callers must migrate to the explicit external documentation snapshot before legacy removal.

Logical script examples explicitly select current ScriptCompiler-generated APIs through --wiki-runtime-reference <dll> and --wiki-editor-reference <dll>. Validators freeze and hash each reference's bytes. Host implementation assemblies must not supply types absent from the script surface. Record generation provenance and current product state together.

Documentation verification covers project entries, signature mappings, relative links, content boundaries and C# examples across all pages. Project examples use their owner's compilation references; architecture guides and category pages default to .NET contracts only. Logical script examples use explicit trimmed references. Complete examples requiring concrete engine composition live with their owning project, and guides link to them without duplicate maintenance.

Complete Host-example acceptance also supplies --dotnet, compiles against actual SDK ReferencePath and the current owning assembly, and records host-sdk status. SDK-free host checks are preliminary diagnostics only. Assemblies elsewhere in the repository do not establish an example's allowed references.

With an SDK selected, API models enumerate actual Compile items and conditional symbols. Selected generated bindings/types, nested types and partial members enter the same inventory; obj or Generated placement is not an exclusion. Other targets' unselected generated files are excluded from the current contract. This extraction does not execute Roslyn source generators, replace post-generation assembly boundary checks or verify every product configuration.

Native project entries distinguish managed initialization from generated ABI surfaces. Generated declarations without XML require review of their owning facades, generation configuration and real consumers, adding pointer-borrowing, release and thread restrictions rather than assuming managed-object lifetimes. Record generated-member counts separately from completed behavioral descriptions. Complete signature markers do not establish complete semantics.

Analyzers understand the project's actual C# language version. Unknown LangVersion values fail rather than guessing Preview. Prepare the selected project when target/configuration/reference output is missing. Do not substitute another configuration or DLLs that happen to exist elsewhere in the repository.

Per-page semantic review remains separate work. Read [current acceptance](../records/acceptance/current.md) and do not mistake page existence or complete markers for full documentation acceptance.
<!-- /rule-unit: documentation-standard-6 -->
