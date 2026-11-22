using System;
using System.Collections.Generic;

namespace Inno.Tooling.Architecture;

internal abstract class StyleRule
{
    internal abstract string id { get; }

    internal abstract StyleRuleKind kind { get; }

    // Whether the rule reports findings under the given rule ID.
    internal virtual bool Covers(string ruleID) => string.Equals(ruleID, id, StringComparison.Ordinal);

    internal abstract void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    );

    // Returns non-overlapping edits against the current text, or an empty list when the rule has no automatic fix.
    internal virtual IReadOnlyList<StyleEdit> Fix(StyleSource source) => [];
}
