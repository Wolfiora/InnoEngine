# C# code style rulebook

[Decisions](README.md) · [Rule authority](../../rulebook/csharp-style/README.md)

The user reviewed a complete audit of the handwritten C# source, revised the proposed rules on 2026-11-21 and approved one detailed style authority for humans and agents. The [C# code style rulebook](../../rulebook/csharp-style/README.md) is that authority for names and layout. The [coding rules](../../rulebook/coding.md) keep design, error-handling, resource and change-management requirements.

## Registration

The rulebook is a new authority, not a translation. Its complete text was committed first; the rule registry pins the preface and each chapter to that immutable source revision by section index and hash. Every chapter is one complete unit below the staged-read budget, and `.cs` paths select all of them.

Changing a rule requires committing the new chapter text, then updating that unit's `source` and `sourceHash` in the registry. Updating a hash without a committed source is not a rule change.

## Approved replacements

These clauses of the existing coding rules contradicted the approved rulebook. Each replacement is a substantive amendment, not a translation. Original source hashes and translation mappings remain unchanged in the registry; one amendment hash binds each active line to this decision.

| Unit | Replaced clause | Active clause | Rule |
| --- | --- | --- | --- |
| `naming` | Namespaces match the directory hierarchy by default. | A namespace equals the assembly name of its owning project; directories add no segments. | N-02 |
| `naming` | Constants use `C_` or clearly meaningful readonly static names. | Constants use `C_`; static readonly fields use unprefixed UPPER_SNAKE. | NM-07, NM-08 |
| `csharp-standard-1` | Namespaces follow their owning directory or project convention. | Namespaces follow their owning project. | N-02 |
| `csharp-standard-2` | Example `targetId`. | Example `targetID`. | NM-13 |
| `csharp-standard-2` | Static readonly members use clear semantic names such as `DefaultOptions`. | Static readonly fields use unprefixed UPPER_SNAKE such as `DEFAULT_OPTIONS`. | NM-08 |
| `csharp-standard-4` | Example parameter `commandId` (three lines). | Example parameter `commandID`. | NM-13 |

All other clauses of those units remain in force. Where a general clause and a rulebook rule cover the same subject, the rulebook rule is the detailed form.

## Interpretations

The rulebook resolves these points where the approved revisions allowed more than one reading:

- Conditional-expression continuations align with the condition; call-chain continuations indent four spaces.
- Every empty method, constructor and lambda body is `{}`; property patterns keep `{ }`.
- A multi-line header of `if`, `for`, `foreach`, `while`, `using`, `lock` and `fixed` closes with `) {`.
- Only an `if` without `else` whose statement fits on the same line may omit braces.
- Additional `where` clauses align with the first.
- A multi-line collection expression may open with `[` directly before its first element.
- Two-letter acronyms are lowercase at the start of a camelCase name and uppercase elsewhere.
- Column alignment is recommended from four consecutive declarations of one type.

## Consequences

Formatting and renaming are separate changes: formatting-only changes preserve syntax tokens, and acronym or field renames that touch public members are API changes. `.editorconfig` and the style validator express the naming table of the rulebook.

## Tooling status

`InnoEngine.Build style check|fix ROOT` checks handwritten C# against the rulebook by rule ID and applies the automatic fixes; the architecture validator runs the same engine. A layout fix keeps the token sequence, a reorder keeps the token multiset, and a rewrite is rejected unless the result parses.

The engine currently enforces these rules. A finding is reported under the first ID of its group.

| Reported ID | Also covers |
| --- | --- |
| F-01, F-02, F-03, F-04, F-05 | |
| N-01, N-03, N-04, N-05 | N-06 |
| NM-01 to NM-13 | |
| T-01, T-02, T-03, T-04 | |
| M-02, M-03, M-04, M-05 | |
| O-01 | O-02 |
| B-01, B-04, B-05, B-07 | B-06, B-08 |
| B-03 | T-06 |
| B-09 | M-01 |
| P-01, P-02, P-03 | |
| A-01, A-02 | A-03 |
| W-01, W-03, W-04, W-05 | W-02 |
| I-01, I-02, I-05 | I-03, I-04 |
| S-03, S-04 | |
| E-01, E-03, E-04, E-05, E-06, E-07, E-08, E-09, E-10, E-11, E-12 | |
| D-02, D-05, D-06 | D-04 |
| EX-01 | |

Every other rule is not yet checked by the tool and is enforced in review: N-02, T-05, T-07, T-08, B-02, A-04, S-01, S-02, E-02, all X rules, D-03 and all TS rules. The compiler enforces D-01 through `CS1591`. The checks for N-05, E-04 and E-08 read syntax only: N-05 recognizes a qualified name by its root namespace, E-04 reports a negated `is null` and leaves `==` comparisons to review because an overloaded operator is not visible in syntax, and E-08 knows the members that the same type declaration declares. The naming check reads syntax only: it trusts the names of overrides, interface implementations, native interop declarations and serialized members, and it exempts test methods from the `Async` suffix. `.editorconfig` states the same naming table for editors.

A `SuppressMessage("InnoStyle", "<rule ID>", Justification = "...")` attribute on a member or a type hides the findings of that rule inside the declaration (EX-01). The engine reports a suppression that names no checked rule, names a rule that `style fix` corrects, has no justification, sits on another target or hides no finding. `style check` and the architecture validation list every suppression.

The architecture check of the unified validation runs the style rules and the XML documentation rule on the changed C# sources in the `pull-request` and `fast` profiles, and on every source in the `master` and `full` profiles or when the validator itself changed. Every other architecture invariant stays repository-wide in all profiles. The check carries its changed sources in `changedSources`, so incremental evidence never satisfies a complete run.

These checks read syntax only and report the part of a rule that syntax decides:

| ID | Reported | Left to review |
| --- | --- | --- |
| F-02 | A file that declares no type named like it, a primary type that is not first, and a second top-level type other than an enum, a delegate or a positional record. `Type.Part.cs` is named after `Type`. | Whether the companion is used only by the primary type. |
| M-02 | A block body that is one `return`, call, assignment or `throw`; a constructor with an expression body. | A body that holds a comment or a directive. |
| P-03 | A `CancellationToken` parameter of a declaration that is not named `cancellationToken` or is not the last parameter. | A token that directly precedes a closing `params` parameter, and lambda parameters. |
| W-04 | A line break directly before or after `=`; the fix joins the lines. | |
| I-02 | A declaration typed as an array or `List<T>` (or a `var` local) whose value is `new[] {}`, `new T[] {}`, `Array.Empty<T>()` or a `List<T>` creation without constructor arguments. | A creation without a target type: a call receiver, an inferred generic argument, an anonymous-type member. |
| E-05 | A hand-written `if` that throws `ArgumentNullException` for the tested name, and `?? throw new ArgumentNullException(nameof(x))` on a parameter in a statement of a block body. | Expression bodies and initializers, range checks, custom messages. |
| E-06 | `new()` as an argument, a collection element, an operand, a branch or a local initializer. | |
| E-09 | `string.Format` with a literal format and `string.Concat` with two or more arguments. | A format that is itself a value, and `string.Concat` of one sequence. |
| E-12 | A `using` statement with a declaration that ends its block; the fix writes the declaration. | A `using` statement over an expression, and one whose names collide with a sibling scope. |
| D-06 | `#region`, `#endregion`, `#nullable` and `/* */` block comments. | Comment language, markers, commented-out code and unexplained `#pragma`. |
