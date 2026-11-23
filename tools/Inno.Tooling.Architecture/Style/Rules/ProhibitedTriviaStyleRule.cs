using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Inno.Tooling.Architecture;

// D-06: `#region`, `#endregion`, `#nullable` inside a file and `/* */` block comments are not written.
internal sealed class ProhibitedTriviaStyleRule : StyleRule
{
    private const string C_ID = "D-06";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Rewrite;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach (SyntaxTrivia trivia in source.root.DescendantTrivia(descendIntoTrivia: true))
        {
            string? message = trivia.Kind() switch
            {
                SyntaxKind.RegionDirectiveTrivia or SyntaxKind.EndRegionDirectiveTrivia => "Remove the region directive.",
                SyntaxKind.NullableDirectiveTrivia => "Remove `#nullable`: the project sets the nullable context.",
                SyntaxKind.MultiLineCommentTrivia => "Write the comment with `//`, not as a block comment.",
                _ => null
            };
            if (message is not null) findings.Add(source.Finding(C_ID, trivia.SpanStart, message));
        }
    }
}
