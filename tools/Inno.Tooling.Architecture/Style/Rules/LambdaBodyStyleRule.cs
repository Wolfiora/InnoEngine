using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Inno.Tooling.Architecture;

// B-04: an operator or conditional expression body of a lambda that does not fit on the line of the lambda is
// wrapped in parentheses: '(' follows '=>', the expression is indented four spaces and ')' is on its own line.
internal sealed class LambdaBodyStyleRule : StyleRule
{
    private const string C_ID = "B-04";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Rewrite;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach (LambdaExpressionSyntax lambda in Violations(source))
        {
            findings.Add(
                source.Finding(
                    C_ID,
                    lambda.ArrowToken.SpanStart,
                    "Wrap the lambda body in parentheses: it does not fit on the line of the lambda."
                )
            );
        }
    }

    internal override IReadOnlyList<StyleEdit> Fix(StyleSource source)
    {
        IReadOnlyList<TextSpan> protectedSpans = source.ProtectedSpans();
        LambdaExpressionSyntax[] violations = Violations(source).ToArray();
        List<StyleEdit> edits = [];
        foreach (LambdaExpressionSyntax lambda in violations)
        {
            // Innermost lambdas first: an outer body is wrapped once its content has settled.
            if (violations.Any(other => other != lambda && lambda.Span.Contains(other.Span))) continue;
            ExpressionSyntax body = lambda.ExpressionBody!;
            SyntaxToken arrow = lambda.ArrowToken;
            SyntaxToken first = body.GetFirstToken();
            SyntaxToken last = body.GetLastToken();
            if (!StyleLayout.IsBlank(source.content, arrow.Span.End, first.SpanStart)) continue;
            int firstLine = source.LineNumber(first.SpanStart);
            int lastLine = source.LineNumber(last.Span.End);
            if (protectedSpans.Any(span => span.Start < last.Span.End && span.End > first.SpanStart)) continue;
            int indentation = source.Indentation(arrow.SpanStart);
            int inner = indentation + StyleLayout.C_INDENT;
            int column = source.Column(first.SpanStart);
            bool isSameLine = source.LineNumber(arrow.SpanStart) == firstLine;
            edits.Add(new StyleEdit(arrow.Span.End, first.SpanStart, " (\n" + new string(' ', inner)));
            for (int line = firstLine + 1; line <= lastLine; line++)
            {
                // A line aligned with the body follows it; a line indented from its statement moves one level.
                int current = source.Indentation(source.text.Lines[line].Start);
                int delta = isSameLine && current < column ? StyleLayout.C_INDENT : inner - column;
                StyleEdit? shift = StyleLayout.Shift(source, line, delta, protectedSpans);
                if (shift is not null) edits.Add(shift);
            }

            edits.Add(new StyleEdit(last.Span.End, last.Span.End, "\n" + new string(' ', indentation) + ")"));
        }

        return edits;
    }

    private static IEnumerable<LambdaExpressionSyntax> Violations(StyleSource source)
    {
        foreach (SyntaxNode node in source.root.DescendantNodes())
        {
            if (node is not LambdaExpressionSyntax { ExpressionBody: { } body } lambda) continue;
            if (body is not (BinaryExpressionSyntax or IsPatternExpressionSyntax or ConditionalExpressionSyntax))
            {
                continue;
            }
            if (StyleLayout.IsInInterpolation(lambda)) continue;
            int arrowLine = source.LineNumber(lambda.ArrowToken.SpanStart);
            if (arrowLine == source.LineNumber(body.SpanStart) && arrowLine == source.LineNumber(body.Span.End))
            {
                continue;
            }
            yield return lambda;
        }
    }
}
