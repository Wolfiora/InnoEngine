---
paths: ["**/*.cs"]
tags: [naming, members, xml, errors, formatting, identity-naming, csharp-standard, public-api, ownership, documentation]
---
# Coding

## 2. Naming
- Match filenames to their primary type.
- A namespace equals the assembly name of its owning project; directories inside a project do not add namespace segments. `src/composition/editor` uses the project-level namespace rules in section 13.
- Use PascalCase for type names.
- Prefix interfaces with I.
- Use meaningful camelCase for member parameters.
- Prefix private fields with m_.
- Constants use a C_ prefix, such as C_MAX_COUNT. Static readonly fields use UPPER_SNAKE names without a prefix, such as DEFAULT_OPTIONS.
- Keep existing Unity-style API names, such as transform, scene, active and name, consistent with surrounding code.

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

## 6. Error handling
- Validate arguments first: use ArgumentNullException.ThrowIfNull and clear ArgumentException messages.
- Distinguish safe failure from exceptional failure.
- Report explicit exception information at important boundaries.

## 7. Transform and Identity naming clarity
- Align with upper-layer semantics and preserve the existing API naming style.
- Avoid local names that collide with properties and cause shadowing or readability conflicts.
- Keep internal caches consistent after property updates; avoid the unnecessary cost of repeated complete recomputation.

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

# C# development requirements

These requirements apply to handwritten C# source. Generated code and third-party source follow their generation protocols or upstream standards. Repository domain protocols and product-publication policies are supplied by their respective architecture documents.

## 2. Naming

| Element | Rule | Examples |
| --- | --- | --- |
| Types, methods, enum members | PascalCase | GenerationRequest, TryResolve, Borrowed |
| Interfaces | I prefix | IOutputTransaction |
| Properties, parameters, locals | camelCase | targetID, outputDirectory |
| Private fields | m_ prefix | m_generation |
| Constants | C_ prefix | C_MAX_COUNT |
| Static readonly fields | UPPER_SNAKE without a prefix | DEFAULT_OPTIONS |

- External interface implementations, overrides, special CLR members and external protocol signatures follow their contracts.
- The corresponding generator configuration declares generated binding names.
- Names convey meaning, units and ownership. Avoid abbreviations, meaningless helper names and local-variable shadowing.
- Source property names and serialized wire names are declared separately. Naming refactors must not accidentally change data protocols.

## 3. Member organization

Group constants/static members, fields, constructors, public members, protected/internal members and private implementation in that order. Keep members of the same scope/type together and separate responsibilities with blank lines.

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
        string commandID,
        Action command
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(commandID);
        ArgumentNullException.ThrowIfNull(command);
        m_commands.Add(commandID, command);
    }
}
```

## 5. Using directives and compilation

- Explicitly declare ordinary using directives in every file.
- Global usings, implicit imports and MSBuild Using items are prohibited.
- Enable nullable. Restrict unsafe code to actual interop boundaries and define buffer, pointer and resource lifetimes.
- Resolve compilation targets and SDKs from project declarations. Caches do not replace correctness checks.
- Handwritten C# must not use `global::` qualified names. Resolve conflicts with explicit ordinary using directives, clear file-level using type/namespace aliases or local renaming. Keep one alias per target rather than repeating fully qualified names at each use. Generated code, generator output templates and tests verifying that output follow their generation protocol; do not rewrite their qualified names to satisfy handwritten style rules.

## 6. Documentation and comments

- Source comments use English and explain reasons, constraints, ownership or lifetimes.
- Public types/members and externally overridable protected members have complete English XML documentation.
- Summaries describe actual behavior. Param and typeparam tags correspond to every signature parameter. Non-void methods explain returns, including failure/null semantics.
- Exception tags state the conditions for important observable exceptions.
- Overrides and interface implementations may use inheritdoc; document additional constraints locally.
- Template comments that merely restate names are prohibited. Use readable expanded XML tags.
- XML-emitting projects treat CS1572, CS1573 and CS1591 as compilation errors.

## 7. Validation, exceptions and transactions

- Validate arguments first with ArgumentNullException.ThrowIfNull and clear ArgumentException messages.
- Separate safe Try-operation failure from exceptional failure.
- Fully prepare and validate candidates before atomic publication; failure retains last-good state.
- Do not swallow critical exceptions, treat unsupported capabilities as success or use silent fallbacks.
- API changes update current callers, documentation and tests and remove superseded paths. Format changes follow the owning product's current protocol.

## 8. Resources, asynchronous work and concurrency

- Identify the creator, owner, using thread, borrowing period, releaser and final release order.
- Strictly distinguish borrowed from owned resources and define Dispose idempotence and failure semantics.
- Propagate cancellationToken through asynchronous operations. Observe tasks and child-process exits, drain output and avoid stranded background work.
- Callback exceptions must not cross unmanaged boundaries. After unregistration or cancellation, ensure callbacks no longer access released owners.
- An explicit host manages shared process state. Avoid anonymous global state and global service locators.
- Stop new work before retiring generation resources and verifying release. Preserve an explicit fault state on failure.

## 9. Performance

- Define hot-path caches, ownership and invalidation conditions.
- Avoid per-frame complete scans, geometry/texture copies and duplicate command encoding.
- Prefer incremental updates, stable handles, persistent resource caches and pooled buffers with explicit release protocols.
- Do not expose borrowed native memory or expand public APIs for performance.
