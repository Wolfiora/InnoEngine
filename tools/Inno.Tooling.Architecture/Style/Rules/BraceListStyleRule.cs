using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Inno.Tooling.Architecture;

// I-01, I-03 and I-04: a multi-line initializer, anonymous object, 'with' expression, switch expression or
// collection expression has one item per line and its closing delimiter on its own line. Braces, and the
// bracket of a collection that is the value of a declaration or assignment, open on their own line.
internal sealed class BraceListStyleRule : StyleRule
{
    private const string C_ID = "I-01";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Layout;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach ((StyleList list, List<StyleEdit> _) in Violations(source))
        {
            findings.Add(
                source.Finding(
                    C_ID,
                    list.open.SpanStart,
                    "Lay out the initializer or collection: one line when it fits, otherwise one item per line with the delimiters on their own lines."
                )
            );
        }
    }

    internal override IReadOnlyList<StyleEdit> Fix(StyleSource source)
    {
        var edits = new List<StyleEdit>();
        int fixedEnd = -1;
        foreach ((StyleList list, List<StyleEdit> listEdits) in Violations(source).OrderBy(
            static violation => violation.list.open.SpanStart
        )) {
            // A list nested in a fixed list waits for the next round.
            if (list.open.SpanStart < fixedEnd) continue;
            edits.AddRange(listEdits);
            fixedEnd = list.close.Span.End;
        }

        return edits;
    }

    private static bool IsLiteral(SyntaxNode item)
        => item switch
    {
        ExpressionElementSyntax element => IsLiteral(element.Expression),
        LiteralExpressionSyntax => true,
        PrefixUnaryExpressionSyntax unary => IsLiteral(unary.Operand),
        _ => false
    };

    private static IEnumerable<(StyleList list, List<StyleEdit> edits)> Violations(StyleSource source)
    {
        IReadOnlyList<TextSpan> protectedSpans = source.ProtectedSpans();
        foreach (SyntaxNode node in source.root.DescendantNodes())
        {
            StyleList? list = StyleList.Braces(node);
            if (list is null) continue;
            if (source.LineNumber(list.open.SpanStart) == source.LineNumber(list.close.SpanStart))
            {
                // I-01: a one-line initializer does not start a line; it joins 'new T' or expands.
                if (node is not (InitializerExpressionSyntax or AnonymousObjectCreationExpressionSyntax)) continue;
                if (!source.IsFirstOnLine(list.open) || node is AnonymousObjectCreationExpressionSyntax) continue;
                SyntaxToken owner = list.open.GetPreviousToken();
                if (!StyleLayout.IsBlank(source.content, owner.Span.End, list.open.SpanStart)) continue;
                TextLine ownerLine = source.Line(owner.Span.End);
                TextLine braceLine = source.Line(list.open.SpanStart);
                if (owner.Span.End == ownerLine.End
                    && ownerLine.End - ownerLine.Start + 1 + braceLine.End - list.open.SpanStart <= StyleLayout.C_WIDTH
                ) {
                    yield return (list, [new(owner.Span.End, list.open.SpanStart, " ")]);
                    continue;
                }

                List<StyleEdit>? expanded = StyleLayout.Expand(source, list, protectedSpans);
                if (expanded is not null && expanded.Count > 0) yield return (list, expanded);
                continue;
            }

            // A single element that spans several lines hugs the brackets (I-03).
            if (node is CollectionExpressionSyntax && list.items.Count == 1) continue;
            // A numeric matrix or lookup table may keep several elements per line (S-02).
            bool isWide = Enumerable
                .Range(
                    source.LineNumber(list.open.SpanStart),
                    source.LineNumber(list.close.SpanStart) - source.LineNumber(list.open.SpanStart) + 1
                )
                .Any(line => source.text.Lines[line].End - source.text.Lines[line].Start > StyleLayout.C_HARD_WIDTH);
            if (list.items.Count > 1 && !isWide && list.items.All(static item => IsLiteral(item))) continue;
            SyntaxToken previous = list.open.GetPreviousToken();
            bool opensLine = node is not CollectionExpressionSyntax || node.Parent is EqualsValueClauseSyntax or AssignmentExpressionSyntax;
            if (opensLine && !source.IsFirstOnLine(list.open))
            {
                if (!StyleLayout.IsBlank(source.content, previous.Span.End, list.open.SpanStart)) continue;
                int indentation = source.Indentation(previous.SpanStart);
                // After a wrapped '=>' the braces sit at the column of the declaration (M-03).
                SyntaxToken lineStart = StyleLines.FirstToken(source, source.LineNumber(previous.SpanStart));
                if (lineStart.IsKind(SyntaxKind.EqualsGreaterThanToken)
                    && lineStart.Parent is ArrowExpressionClauseSyntax
                ) {
                    indentation -= StyleLayout.C_INDENT;
                }
                yield return (list, [new(previous.Span.End, list.open.SpanStart, "\n" + new string(' ', indentation))]);
                continue;
            }

            if (StyleLayout.IsExpanded(source, list)) continue;
            List<StyleEdit>? edits = StyleLayout.Expand(source, list, protectedSpans);
            if (edits is not null && edits.Count > 0) yield return (list, edits);
        }
    }
}
