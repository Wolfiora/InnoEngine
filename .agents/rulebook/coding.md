# Coding

Load complete rule units. The [registry](coverage.json) owns routing and provenance.

<!-- rule-unit: naming -->
<a id="naming"></a>
## 2. Naming
- Match filenames to their primary type.
- By default, match namespaces to directory hierarchy. `src/composition/editor` uses the project-level namespace rules in section 13.
- Use PascalCase for type names.
- Prefix interfaces with I.
- Use meaningful camelCase for member parameters.
- Prefix private fields with m_.
- Constants use a C_ prefix, such as C_MAX_COUNT, or clearly meaningful readonly static names.
- Keep existing Unity-style API names, such as transform, scene, active and name, consistent with surrounding code.
<!-- /rule-unit: naming -->

<!-- rule-unit: members -->
<a id="members"></a>
## 3. Member organization
- Group class members by access level and role, preferably near the top, in this order:
  1. Constants (const / static)
  2. Fields (m_)
  3. Constructors
  4. Public properties / methods
  5. Protected/internal members
  6. Private methods and utilities
- Keep members of the same scope and type together where possible instead of scattering them through a class.
- Public members come before restricted members.
<!-- /rule-unit: members -->

<!-- rule-unit: xml -->
<a id="xml"></a>
## 5. Comments must be English
- All externally visible public members and protected members overridable by external derived types must have complete English XML documentation:
  - Types and members have a meaningful summary that does not merely repeat their names.
  - Every parameter has a corresponding param tag; every generic parameter has a corresponding typeparam tag.
  - Non-void methods have a returns tag explaining the result and failure/null states.
  - Exception tags describe conditions for important caller-observable exceptions.
  - Overrides and implementations may use inheritdoc, but must document any additional constraints, exceptions or semantics locally.
- Projects emitting XML documentation treat CS1572, CS1573 and CS1591 as compilation errors to prevent invalid parameter tags, missing parameter descriptions and undocumented public members.
- Also document important internal APIs when their behavioral impact warrants it.
- Chinese source comments are prohibited. Avoid leaving temporary comments such as TODO indefinitely.
- Complex logic may have comments written as complete, concise English sentences.
<!-- /rule-unit: xml -->

<!-- rule-unit: errors -->
<a id="errors"></a>
## 6. Error handling
- Validate arguments first: use ArgumentNullException.ThrowIfNull and clear ArgumentException messages.
- Distinguish safe failure from exceptional failure.
- Report explicit exception information at important boundaries.
<!-- /rule-unit: errors -->

<!-- rule-unit: identity-naming -->
<a id="identity-naming"></a>
## 7. Transform and Identity naming clarity
- Align with upper-layer semantics and preserve the existing API naming style.
- Avoid local names that collide with properties and cause shadowing or readability conflicts.
- Keep internal caches consistent after property updates; avoid the unnecessary cost of repeated complete recomputation.
<!-- /rule-unit: identity-naming -->

<!-- rule-unit: formatting -->
<a id="formatting"></a>
## 23. Source readability and formatting
- Use one clear alias for the same type within a file; do not alternate facade aliases for the same call. XML documentation describes current real members. Give each member one meaningful summary and remove descriptions of deleted members.
- Readability is a continuous requirement for all handwritten source in InnoEngine, Canvas and Rendering2D. Generated bindings and third-party extern source are outside manual formatting. Formatting changes must preserve execution order, initialization order, public contracts, serialized data, diagnostics and user experience.
- Make declarations and intent immediately readable. Keep fields, properties, zero/single-parameter functions and simple expressions on one line when they fit a reasonable width. Do not mechanically split names, expression bodies or modifiers solely to meet a line-width rule.
- Every function declaration with more than one parameter, including methods, constructors, local functions, delegates, operators, primary constructors and lambdas, starts a new line after the opening parenthesis, puts each parameter on its own line, and places the closing parenthesis on a new line aligned with the declaration's opening line. An immediately following block begins on that same line as `) {`. Expression bodies, constraints and initializers follow the closing parenthesis according to their syntax. Zero/single-parameter functions need not expand. Apply this to new and changed source; automatic formatting must preserve it.

- Split long lines and independent concepts by meaning. Indent multiline calls and declarations consistently, with a clear argument or logical group per line. Apply the same principle to call chains, conditions, collection initializers and exception construction. Do not compress multiple executable statements onto one line.
- Use blank lines between responsibilities and processing phases. Keep indentation, braces, spacing and line breaks consistent within a file. Simple automatic properties may be compact; control flow, exception handling and multiple-statement members use readable expanded blocks.
- Comments explain reasons, constraints and lifetimes rather than restating code. XML summary, param, returns and other tags use the expanded form required by existing architecture checks. Wrap long content at complete semantic boundaries. Section 5's English-comment requirement still applies.
- Review readability cleanup file by file and keep it formatting-only. After batch changes, verify syntax tokens, builds and relevant tests. Do not mix renames, logic refactors or behavior fixes into formatting-only commits.

Multiple-parameter example:

```csharp
public void AAA(
    int param1,
    int param2
) {
    // Method body.
}
```
<!-- /rule-unit: formatting -->

<!-- rule-unit: csharp-standard-0 -->
<a id="csharp-standard-0"></a>
# C# development requirements

These requirements apply to handwritten C# source. Generated code and third-party source follow their generation protocols or upstream standards. Repository domain protocols and product-publication policies are supplied by their respective architecture documents.
<!-- /rule-unit: csharp-standard-0 -->

<!-- rule-unit: csharp-standard-1 -->
<a id="csharp-standard-1"></a>
## 1. Design and responsibility

- Prioritize architectural clarity, dependency direction, ownership, encapsulation and low coupling.
- Match filenames to primary types. Directories express independently understandable capabilities; namespaces follow their owning directory or project convention.
- Public APIs expose the smallest complete contracts needed for external composition. Prefer composition and supply dependencies through constructor parameters or explicit composition entries.
- Check existing repository events, identity, serialization, mathematics and diagnostics first. Reuse them and avoid parallel management systems.
- Interfaces or base classes express extension behavior; immutable discovery metadata is declared once. Replaceable boundaries isolate platforms, storage, compilers and presentation.
- Separate implementation from persistence protocols. Persistent data uses stable identities and neutral values; explicit lifetimes manage runtime objects, delegates and type references.
- Shared mechanisms, concrete backends, platform-system integration, product composition and build distributions each own their source. Shared mechanisms must not select concrete platforms or products.
- A build host describes only the tool execution environment. Requests explicitly select runtime target, CPU, ABI and managed deployment; do not infer them from host facts.
- One backend owns a third-party component's runtime, Native facade, binding configuration and build recipe. Platform products select them through capabilities and frozen descriptions.
<!-- /rule-unit: csharp-standard-1 -->

<!-- rule-unit: csharp-standard-2 -->
<a id="csharp-standard-2"></a>
## 2. Naming

| Element | Rule | Examples |
| --- | --- | --- |
| Types, methods, enum members | PascalCase | GenerationRequest, TryResolve, Borrowed |
| Interfaces | I prefix | IOutputTransaction |
| Properties, parameters, locals | camelCase | targetId, outputDirectory |
| Private fields | m_ prefix | m_generation |
| Constants | C_ prefix | C_MAX_COUNT |
| Static readonly members | Clear semantic names | DefaultOptions |

- External interface implementations, overrides, special CLR members and external protocol signatures follow their contracts.
- The corresponding generator configuration declares generated binding names.
- Names convey meaning, units and ownership. Avoid abbreviations, meaningless helper names and local-variable shadowing.
- Source property names and serialized wire names are declared separately. Naming refactors must not accidentally change data protocols.
<!-- /rule-unit: csharp-standard-2 -->

<!-- rule-unit: csharp-standard-3 -->
<a id="csharp-standard-3"></a>
## 3. Member organization

Group constants/static members, fields, constructors, public members, protected/internal members and private implementation in that order. Keep members of the same scope/type together and separate responsibilities with blank lines.
<!-- /rule-unit: csharp-standard-3 -->

<!-- rule-unit: csharp-standard-4 -->
<a id="csharp-standard-4"></a>
## 4. Declarations and formatting

- Keep zero/single-parameter functions and short expressions on one line when they fit a reasonable width.
- Methods, constructors, local functions, delegates, operators, primary constructors and lambdas with more than one parameter use a line break after the opening parenthesis, one parameter per line, and a closing parenthesis aligned with the declaration's starting line.
- An immediately following function body starts with `) {`; expressions, constraints and initializers follow their required syntax.
- Split long calls, conditions, call chains and collections by meaning. Do not combine multiple executable statements on one line.
- Expand members containing control flow, exception handling or multiple statements into clear blocks.
- Separate responsibilities and execution phases with blank lines; use consistent indentation and brace style throughout a file.
- Automatic formatting must preserve these rules.

```csharp
using System;
using System.Collections.Generic;

internal sealed class CommandSet
{
    private readonly Dictionary<string, Action> m_commands = new(StringComparer.Ordinal);

    internal void Register(
        string commandId,
        Action command
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(commandId);
        ArgumentNullException.ThrowIfNull(command);
        m_commands.Add(commandId, command);
    }
}
```
<!-- /rule-unit: csharp-standard-4 -->

<!-- rule-unit: csharp-standard-5 -->
<a id="csharp-standard-5"></a>
## 5. Using directives and compilation

- Explicitly declare ordinary using directives in every file.
- Global usings, implicit imports and MSBuild Using items are prohibited.
- Enable nullable. Restrict unsafe code to actual interop boundaries and define buffer, pointer and resource lifetimes.
- Resolve compilation targets and SDKs from project declarations. Caches do not replace correctness checks.

<!-- approved-addition -->
- Handwritten C# must not use `global::` qualified names. Resolve conflicts with explicit ordinary using directives, clear file-level using type/namespace aliases or local renaming. Keep one alias per target rather than repeating fully qualified names at each use. Generated code, generator output templates and tests verifying that output follow their generation protocol; do not rewrite their qualified names to satisfy handwritten style rules.
<!-- /rule-unit: csharp-standard-5 -->

<!-- rule-unit: csharp-standard-6 -->
<a id="csharp-standard-6"></a>
## 6. Documentation and comments

- Source comments use English and explain reasons, constraints, ownership or lifetimes.
- Public types/members and externally overridable protected members have complete English XML documentation.
- Summaries describe actual behavior. Param and typeparam tags correspond to every signature parameter. Non-void methods explain returns, including failure/null semantics.
- Exception tags state the conditions for important observable exceptions.
- Overrides and interface implementations may use inheritdoc; document additional constraints locally.
- Template comments that merely restate names are prohibited. Use readable expanded XML tags.
- XML-emitting projects treat CS1572, CS1573 and CS1591 as compilation errors.
<!-- /rule-unit: csharp-standard-6 -->

<!-- rule-unit: csharp-standard-7 -->
<a id="csharp-standard-7"></a>
## 7. Validation, exceptions and transactions

- Validate arguments first with ArgumentNullException.ThrowIfNull and clear ArgumentException messages.
- Separate safe Try-operation failure from exceptional failure.
- Fully prepare and validate candidates before atomic publication; failure retains last-good state.
- Do not swallow critical exceptions, treat unsupported capabilities as success or use silent fallbacks.
- API changes update current callers, documentation and tests and remove superseded paths. Format changes follow the owning product's current protocol.
<!-- /rule-unit: csharp-standard-7 -->

<!-- rule-unit: csharp-standard-8 -->
<a id="csharp-standard-8"></a>
## 8. Resources, asynchronous work and concurrency

- Identify the creator, owner, using thread, borrowing period, releaser and final release order.
- Strictly distinguish borrowed from owned resources and define Dispose idempotence and failure semantics.
- Propagate cancellationToken through asynchronous operations. Observe tasks and child-process exits, drain output and avoid stranded background work.
- Callback exceptions must not cross unmanaged boundaries. After unregistration or cancellation, ensure callbacks no longer access released owners.
- An explicit host manages shared process state. Avoid anonymous global state and global service locators.
- Stop new work before retiring generation resources and verifying release. Preserve an explicit fault state on failure.
<!-- /rule-unit: csharp-standard-8 -->

<!-- rule-unit: csharp-standard-9 -->
<a id="csharp-standard-9"></a>
## 9. Performance

- Define hot-path caches, ownership and invalidation conditions.
- Avoid per-frame complete scans, geometry/texture copies and duplicate command encoding.
- Prefer incremental updates, stable handles, persistent resource caches and pooled buffers with explicit release protocols.
- Do not expose borrowed native memory or expand public APIs for performance.
<!-- /rule-unit: csharp-standard-9 -->

<!-- rule-unit: csharp-standard-10 -->
<a id="csharp-standard-10"></a>
## 10. Tests and acceptance

- Use real public contracts and replaceable public boundaries.
- InternalsVisibleTo, test backdoors, reflection bypasses and widening internal visibility for tests are prohibited.
- Cover real success, failure, cancellation, boundary and lifecycle behavior. Do not write tests that merely repeat the implementation.
- Native calls, AOT, trimming and platform support require actual artifact evidence.
- Report build, test, SDK, host and device status separately. Unexecuted checks are not passes.
<!-- /rule-unit: csharp-standard-10 -->

<!-- rule-unit: csharp-standard-11 -->
<a id="csharp-standard-11"></a>
## 11. Change management

- Formatting-only changes preserve syntax tokens, execution order, contracts and diagnostics.
- Validate naming, responsibility and behavior changes separately from pure formatting.
- Public API, reference-boundary and generation-protocol changes also update consumers, indexes, examples and architecture checks.
- Before completion, ensure the target scope compiles and record validation commands, results and remaining limitations.
<!-- /rule-unit: csharp-standard-11 -->

<!-- rule-unit: csharp-standard-12 -->
<a id="csharp-standard-12"></a>
## 12. Solution and MSBuild readability

- Solution Folders express actual responsibility layers. Remove groups without projects or valid Solution Items. Small repositories do not need extra categories wrapping single projects.
- Include all authored source projects in the Solution, including binding-generation extensions and test fixtures; exclude generated artifacts and third-party projects. Before declaring a project without incoming references obsolete, check host entries, MSBuild tool calls and generation configuration.
- Use two-space indentation and LF in `.csproj`, `.props` and `.targets`, with one blank line between top-level responsibility blocks. Do not mix tabs and spaces.
- Keep simple declarations on one line. Put long attribute lists after the element name, one attribute per line. Do not split attribute values in ways that change paths, conditions or commands.
- Use `/` in relative ProjectReference paths. Group implementation dependencies separately from public-signature dependencies according to project rules. Do not mechanically merge groups with different conditions, visibility or responsibilities.
- Remove empty PropertyGroup/ItemGroup elements with no attributes, content or comments. Merge adjacent reference groups only when conditions and visibility match and no intervening evaluation step exists.
- MSBuild evaluates in order. Formatting-only cleanup verifies property values, conditions, metadata, Imports, Targets and effective project-reference order. General XML sorting is not safe project reorganization.
- Preserve actual platform conditions and SDK/toolchain constraints. Formatting platform projects must not move those concerns into neutral business layers.
<!-- /rule-unit: csharp-standard-12 -->

<!-- rule-unit: csharp-standard-13 -->
<a id="csharp-standard-13"></a>
## Splitting responsibilities and changing boundaries

Large owners may use partial files for independently understandable responsibilities, while fields, construction and the single lifecycle remain in the primary file. Move members and verify syntax tokens/order first; change behavior/performance separately. Public DTO filenames match their primary type, and closely related enums remain in the owning protocol. Do not create parallel event, identity or Undo managers merely to shorten files.

Public-contract changes update every consumer, reference visibility, script manifest, Stable ID, documentation, success/failure/boundary test and architecture gate. Record ownership, cancellation, thread affinity and final release for read streams, background tasks, snapshots, scratch, native callbacks and device resources. Performance conclusions require measurements; unmeasured values remain unknown.
<!-- /rule-unit: csharp-standard-13 -->
