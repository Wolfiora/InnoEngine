namespace Inno.Tooling.Architecture;

internal enum StyleRuleKind
{
    // Changes whitespace and line breaks only. The token sequence is identical before and after.
    Layout,
    // Moves tokens without adding or removing any. The token multiset is identical before and after.
    Reorder,
    // Adds, removes or replaces tokens without changing program meaning.
    Rewrite
}
