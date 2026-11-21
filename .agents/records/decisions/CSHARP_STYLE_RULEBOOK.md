# C# code style rulebook

[Decisions](README.md) · [Rule authority](../../rulebook/csharp-style.md)

The user reviewed a complete audit of the handwritten C# source, revised the proposed rules on 2026-11-21 and approved one detailed style authority for humans and agents. The [C# code style rulebook](../../rulebook/csharp-style.md) is that authority for names and layout. The [coding rules](../../rulebook/coding.md) keep design, error-handling, resource and change-management requirements.

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

Existing source does not yet conform. Formatting and renaming are separate, later changes: formatting-only changes preserve syntax tokens, and acronym or field renames that touch public members are API changes. `.editorconfig` and the style validator still express the previous static-readonly naming and must follow the rulebook before they are used as gates.
