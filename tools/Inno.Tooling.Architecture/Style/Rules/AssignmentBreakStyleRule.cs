using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Inno.Tooling.Architecture;

// W-04: a line is never broken directly before or after `=`. The value starts on the line of the `=`;
// the only exception is the opening bracket of a multi-line collection expression (I-03).
internal sealed class AssignmentBreakStyleRule : StyleRule
{
    private const string C_ID = "W-04";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Layout;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach ((SyntaxToken equals, SyntaxNode _, bool _) in Violations(source))
        {
            findings.Add(source.Finding(C_ID, equals.SpanStart, "Keep `=` and the start of its value on one line."));
        }
    }

    internal override IReadOnlyList<StyleEdit> Fix(StyleSource source)
    {
        IReadOnlyList<TextSpan> protectedSpans = source.ProtectedSpans();
        List<StyleEdit> edits = [];
        int fixedLine = -1;
        foreach ((SyntaxToken equals, SyntaxNode value, bool before) in Violations(source))
        {
            SyntaxToken left = before ? equals.GetPreviousToken() : equals;
            SyntaxToken right = before ? equals : value.GetFirstToken();
            int firstLine = source.LineNumber(right.SpanStart);
            int lastLine = source.LineNumber(value.Span.End);

            // A value that an earlier fix of this round reaches waits for the next round.
            if (source.LineNumber(left.SpanStart) <= fixedLine) continue;
            if (!StyleLayout.IsBlank(source.content, left.Span.End, right.SpanStart)) continue;
            edits.Add(new StyleEdit(left.Span.End, right.SpanStart, " "));

            // The continuation lines were indented from the moved line; they now hang from the statement.
            int delta = source.Indentation(left.SpanStart) - source.Indentation(right.SpanStart);
            bool isDelimited = value.GetLastToken().Kind() is SyntaxKind.CloseParenToken
                               or SyntaxKind.CloseBraceToken
                               or SyntaxKind.CloseBracketToken
                               && source.IsFirstOnLine(value.GetLastToken());
            if (delta != 0 && isDelimited)
            {
                for (int line = firstLine + 1; line <= lastLine; line++)
                {
                    StyleEdit? shift = StyleLayout.Shift(source, line, delta, protectedSpans);
                    if (shift is not null) edits.Add(shift);
                }
            }

            fixedLine = lastLine;
        }

        return edits;
    }

    private static IEnumerable<(SyntaxToken equals, SyntaxNode value, bool before)> Violations(StyleSource source)
    {
        foreach (SyntaxNode node in source.root.DescendantNodes())
        {
            (SyntaxToken equals, SyntaxNode? value) = node switch
            {
                EqualsValueClauseSyntax clause => (clause.EqualsToken, clause.Value),
                AssignmentExpressionSyntax assignment => (assignment.OperatorToken, assignment.Right),
                _ => (default(SyntaxToken), (SyntaxNode?)null)
            };
            if (value is null || equals.IsMissing || value.IsMissing) continue;
            if (source.IsFirstOnLine(equals))
            {
                yield return (equals, value, true);
                continue;
            }
            if (!source.IsLastOnLine(equals)) continue;
            if (value is CollectionExpressionSyntax) continue;
            yield return (equals, value, false);
        }
    }
}
