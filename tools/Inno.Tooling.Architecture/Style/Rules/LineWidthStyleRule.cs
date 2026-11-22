using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Inno.Tooling.Architecture;

// F-04: no line passes the hard limit, except for an unsplittable string literal. A line that does is
// broken at its outermost argument list (A-02).
internal sealed class LineWidthStyleRule : StyleRule
{
    private const string C_ID = "F-04";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Layout;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach ((TextLine line, StyleList? _) in Violations(source))
        {
            findings.Add(
                source.Finding(
                    C_ID,
                    line.Start + StyleLayout.C_HARD_WIDTH,
                    $"Break the line: it is longer than {StyleLayout.C_HARD_WIDTH} columns."
                )
            );
        }
    }

    internal override IReadOnlyList<StyleEdit> Fix(StyleSource source)
    {
        IReadOnlyList<TextSpan> protectedSpans = source.ProtectedSpans();
        List<StyleEdit> edits = [];
        int fixedEnd = -1;
        foreach ((TextLine line, StyleList? list) in Violations(source))
        {
            // A line that an earlier fix of this round reaches waits for the next round.
            if (line.Start < fixedEnd) continue;
            if (HasCompactBody(source, line)) continue;
            (TextSpan span, List<StyleEdit> edits)? calls = ChainStyleRule.BreakOnLine(source, line);
            if (calls is { } chain && list is not null && list.open.SpanStart < chain.span.Start && list.close.SpanStart >= chain.span.End)
            {
                calls = null;
            }

            List<StyleEdit>? lineEdits = WrapArrow(source, line) ?? BreakOperators(source, line, list, calls?.span) ?? calls?.edits;
            if (lineEdits is null && list is not null) lineEdits = StyleLayout.Expand(source, list, protectedSpans);
            lineEdits ??= ExpandParameter(source, line, protectedSpans);
            if (lineEdits is null || lineEdits.Count == 0) continue;
            edits.AddRange(lineEdits);
            fixedEnd = lineEdits.Max(static edit => edit.end);
        }

        return edits;
    }

    // B-05: an over-long line that holds a control statement and its body is first split by the braces of the body.
    private static bool HasCompactBody(
        StyleSource source,
        TextLine line
    ) {
        foreach (SyntaxNode node in source.root.DescendantNodes(TextSpan.FromBounds(line.Start, line.End)))
        {
            if (node is not StatementSyntax statement || node is BlockSyntax) continue;
            if (statement.Parent is not (StatementSyntax or ElseClauseSyntax) || statement.Parent is BlockSyntax)
            {
                continue;
            }
            if (statement.Parent is ElseClauseSyntax && statement is IfStatementSyntax) continue;
            if (statement.Parent is LabeledStatementSyntax or LocalFunctionStatementSyntax) continue;
            SyntaxToken first = statement.GetFirstToken();
            if (first.SpanStart >= line.Start && first.SpanStart < line.End && !source.IsFirstOnLine(first))
            {
                return true;
            }
        }

        return false;
    }

    // P-01 keeps a single parameter on one line; when nothing else can shorten the line, the list expands.
    private static List<StyleEdit>? ExpandParameter(
        StyleSource source,
        TextLine line,
        IReadOnlyList<TextSpan> protectedSpans
    ) {
        foreach (SyntaxNode node in source.root.DescendantNodes(TextSpan.FromBounds(line.Start, line.End)))
        {
            if (node is not ParameterListSyntax { Parameters.Count: 1 } parameters || node.Parent is LambdaExpressionSyntax)
            {
                continue;
            }
            if (parameters.SpanStart < line.Start || parameters.Span.End > line.End) continue;
            var list = new StyleList(
                parameters.OpenParenToken,
                parameters.CloseParenToken,
                parameters.Parameters.ToArray<SyntaxNode>()
            );
            return StyleLayout.Expand(source, list, protectedSpans);
        }

        return null;
    }

    // M-03: a member whose signature and expression body share an over-long line moves '=>' to the next line.
    private static List<StyleEdit>? WrapArrow(
        StyleSource source,
        TextLine line
    ) {
        foreach (SyntaxNode node in source.root.DescendantNodes(TextSpan.FromBounds(line.Start, line.End)))
        {
            if (node is not ArrowExpressionClauseSyntax clause || clause.Parent is null) continue;
            SyntaxToken arrow = clause.ArrowToken;
            SyntaxToken previous = arrow.GetPreviousToken();
            if (source.LineNumber(arrow.SpanStart) != line.LineNumber || source.IsFirstOnLine(arrow)) continue;
            if (source.LineNumber(clause.Parent.GetFirstToken().SpanStart) != line.LineNumber) continue;
            if (!StyleLayout.IsBlank(source.content, previous.Span.End, arrow.SpanStart)) continue;
            int indentation = source.Indentation(line.Start) + StyleLayout.C_INDENT;
            return [new StyleEdit(previous.Span.End, arrow.SpanStart, "\n" + new string(' ', indentation))];
        }

        return null;
    }

    // W-01: the outermost operator chain of an over-long line breaks before each of its operators, unless
    // an argument list encloses it.
    private static List<StyleEdit>? BreakOperators(
        StyleSource source,
        TextLine line,
        StyleList? list,
        TextSpan? calls
    ) {
        (SyntaxNode chain, SyntaxToken anchor, IReadOnlyList<SyntaxToken> operators)? best = null;
        (SyntaxNode chain, SyntaxToken anchor, IReadOnlyList<SyntaxToken> operators)? outer = null;
        foreach ((SyntaxNode chain, SyntaxToken anchor, IReadOnlyList<SyntaxToken> operators) candidate in StyleOperators.Chains(
            source.root
        )) {
            SyntaxNode chain = candidate.chain;
            if (StyleLayout.IsInInterpolation(chain)) continue;
            if (chain.SpanStart >= line.Start && chain.Span.End <= line.End)
            {
                if (best is not { } current || chain.Span.Length > current.chain.Span.Length) best = candidate;
            }
            else if (candidate.operators.Any(token => IsInside(source, line, token)))
            {
                if (outer is not { } current || chain.Span.Length > current.chain.Span.Length) outer = candidate;
            }
        }

        // A chain that is already broken puts every operator of the over-long line at the start of a line.
        if (outer is { } broken && (best is not { } inner || broken.chain.Span.Contains(inner.chain.Span)))
        {
            // A call chain around the broken operators breaks first.
            if (calls is { } around && around.Contains(broken.chain.Span) && around != broken.chain.Span) return null;
            int column = source.LineNumber(broken.anchor.SpanStart) == line.LineNumber
                         ? source.Column(broken.anchor.SpanStart)
                         : source.Indentation(line.Start);
            return Break(source, broken.operators.Where(token => IsInside(source, line, token)).ToArray(), column);
        }

        if (best is not { } found) return null;
        // An operator inside an argument of a call chain waits until the chain itself is broken.
        if (calls is { } span && span.Contains(found.chain.Span) && span != found.chain.Span) return null;
        if (list is not null)
        {
            bool isOperand = StyleOperators.IsComparison(found.chain) && found.chain.Span.Contains(list.open.Span);
            if (isOperand || IsEnclosed(source, line, found.chain)) return null;
        }

        return Break(source, found.operators, source.Column(found.anchor.SpanStart));
    }

    // True when an argument list or a brace list on the line contains the node: that list breaks first.
    private static bool IsEnclosed(
        StyleSource source,
        TextLine line,
        SyntaxNode node
    ) {
        foreach (SyntaxNode ancestor in node.Ancestors())
        {
            if (ancestor is StatementSyntax or MemberDeclarationSyntax) return false;
            StyleList? list = StyleList.Arguments(ancestor) ?? StyleList.Braces(ancestor);
            if (list is null || list.open.SpanStart > node.SpanStart || list.close.SpanStart < node.Span.End) continue;
            if (list.open.SpanStart >= line.Start && list.close.SpanStart <= line.End) return true;
        }

        return false;
    }

    private static bool IsInside(
        StyleSource source,
        TextLine line,
        SyntaxToken token
    ) => token.SpanStart >= line.Start && token.SpanStart < line.End && !source.IsFirstOnLine(token);

    private static List<StyleEdit>? Break(
        StyleSource source,
        IReadOnlyList<SyntaxToken> operators,
        int column
    ) {
        string lineBreak = "\n" + new string(' ', column);
        List<StyleEdit> edits = [];
        foreach (SyntaxToken token in operators)
        {
            SyntaxToken previous = token.GetPreviousToken();
            if (!StyleLayout.IsBlank(source.content, previous.Span.End, token.SpanStart)) return null;
            edits.Add(new StyleEdit(previous.Span.End, token.SpanStart, lineBreak));
        }

        return edits.Count == 0 ? null : edits;
    }

    // Each over-long line, with the argument list that breaks it or null when the line has none.
    private static IEnumerable<(TextLine line, StyleList? list)> Violations(StyleSource source)
    {
        Dictionary<int, StyleList> lists = [];
        foreach (SyntaxNode node in source.root.DescendantNodes())
        {
            StyleList? list = StyleList.Arguments(node) ?? StyleList.Braces(node);
            if (list is null || (list.items.Count == 1 && node is CollectionExpressionSyntax)) continue;
            if (StyleLayout.IsInInterpolation(node)) continue;
            int line = source.LineNumber(list.open.SpanStart);
            if (line != source.LineNumber(list.close.SpanStart)) continue;
            // Only an outermost list is broken; among those, the longest one.
            if (lists.TryGetValue(line, out StyleList? current))
            {
                if (list.open.SpanStart < current.close.SpanStart) continue;
                if (list.close.SpanStart - list.open.SpanStart <= current.close.SpanStart - current.open.SpanStart)
                {
                    continue;
                }
            }

            lists[line] = list;
        }

        IReadOnlyList<TextSpan> protectedSpans = source.ProtectedSpans();
        foreach (TextLine line in source.text.Lines)
        {
            if (line.End - line.Start <= StyleLayout.C_HARD_WIDTH) continue;
            if (protectedSpans.Any(span => span.Start < line.Start && span.End > line.Start)) continue;
            lists.TryGetValue(line.LineNumber, out StyleList? list);
            if (list is null && IsUnsplittable(source, line) && WrapArrow(source, line) is null) continue;
            yield return (line, list);
        }
    }

    // True when the line fits without its longest literal or comment, which cannot be broken.
    private static bool IsUnsplittable(
        StyleSource source,
        TextLine line
    ) {
        int longest = 0;
        for (int position = line.Start; position < line.End;)
        {
            SyntaxTrivia trivia = source.root.FindTrivia(position);
            if (trivia.Span.Length > 0 && !trivia.IsKind(SyntaxKind.WhitespaceTrivia))
            {
                longest = Math.Max(longest, Math.Min(trivia.Span.End, line.End) - position);
                position = trivia.Span.End;
                continue;
            }

            SyntaxToken token = source.root.FindToken(position);
            if (token.Span.Length == 0 || token.SpanStart < position)
            {
                position++;
                continue;
            }

            SyntaxNode? literal = token.Parent?.AncestorsAndSelf().FirstOrDefault(
                static node => node is InterpolatedStringExpressionSyntax or LiteralExpressionSyntax
            );
            int end = literal is null ? token.Span.End : literal.Span.End;
            if (literal is not null) longest = Math.Max(longest, Math.Min(end, line.End) - position);
            position = Math.Max(end, position + 1);
        }

        return line.End - line.Start - longest <= StyleLayout.C_HARD_WIDTH - 20;
    }
}
