using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

// W-01 and W-02: when an expression breaks at an operator, the operator starts the continuation line.
internal sealed class LeadingOperatorStyleRule : StyleRule
{
    private const string C_ID = "W-01";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Layout;

    internal override bool isFixable => true;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach (StyleEdit edit in Fix(source))
        {
            findings.Add(
                source.Finding(C_ID, edit.start, "Start each continuation line of the broken expression with its operator.")
            );
        }
    }

    internal override IReadOnlyList<StyleEdit> Fix(StyleSource source)
    {
        List<StyleEdit> edits = [];
        var seen = new HashSet<int>();
        foreach ((SyntaxNode chain, SyntaxToken anchor, IReadOnlyList<SyntaxToken> operators) in StyleOperators.Chains(source.root))
        {
            // W-02: a conditional expression that breaks before its ':' starts a line with '?' and with ':'.
            if (chain is ConditionalExpressionSyntax conditional
                && source.LineNumber(conditional.Condition.Span.End) != source.LineNumber(conditional.ColonToken.SpanStart)
            ) {
                string lineBreak = "\n" + new string(' ', source.Column(anchor.SpanStart));
                foreach (SyntaxToken token in operators)
                {
                    SyntaxToken before = token.GetPreviousToken();
                    if (source.IsFirstOnLine(token) || source.IsLastOnLine(token)) continue;
                    if (!StyleLayout.IsBlank(source.content, before.Span.End, token.SpanStart) || !seen.Add(token.SpanStart))
                    {
                        continue;
                    }
                    edits.Add(new StyleEdit(before.Span.End, token.SpanStart, lineBreak));
                }
            }

            foreach (SyntaxToken token in operators)
            {
                SyntaxToken previous = token.GetPreviousToken();
                SyntaxToken next = token.GetNextToken();
                if (source.LineNumber(token.SpanStart) == source.LineNumber(next.SpanStart)) continue;
                if (source.LineNumber(token.SpanStart) != source.LineNumber(previous.Span.End)) continue;
                string gap = source.content[token.Span.End..next.SpanStart];
                if (!StyleLayout.IsBlank(gap, 0, gap.Length) || !seen.Add(token.SpanStart)) continue;
                edits.Add(new StyleEdit(previous.Span.End, next.SpanStart, gap + token.Text + " "));
            }
        }

        return edits;
    }
}
