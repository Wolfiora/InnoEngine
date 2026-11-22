using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Inno.Tooling.Architecture;

// W-03: in a broken call chain every '.' starts its line, indented four spaces from the line on which the
// chain starts, and consecutive calls after the first break each take their own line.
internal sealed class ChainStyleRule : StyleRule
{
    private const string C_ID = "W-03";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Layout;

    internal override bool isFixable => true;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach ((int position, string message, List<StyleEdit> _) in Violations(source))
        {
            findings.Add(source.Finding(C_ID, position, message));
        }
    }

    internal override IReadOnlyList<StyleEdit> Fix(StyleSource source)
    {
        List<StyleEdit> edits = [];
        int fixedEnd = -1;
        foreach ((int position, string _, List<StyleEdit> chainEdits) in Violations(source))
        {
            // A chain nested in the lines of a fixed chain waits for the next round.
            if (position < fixedEnd) continue;
            edits.AddRange(chainEdits);
            fixedEnd = chainEdits.Max(static edit => edit.end);
        }

        return edits;
    }

    // F-04: the outermost call chain of an over-long line breaks before each of its calls on that line. The
    // receiver and, for a static call, the first call keep the statement line.
    internal static (TextSpan span, List<StyleEdit> edits)? BreakOnLine(
        StyleSource source,
        TextLine line
    ) {
        IReadOnlyList<TextSpan> protectedSpans = source.ProtectedSpans();
        (TextSpan span, List<StyleEdit> edits)? best = null;
        IEnumerable<IGrouping<ExpressionSyntax, ExpressionSyntax>> chains = source.root
            .DescendantNodes(TextSpan.FromBounds(line.Start, line.End))
            .Where(static node => node is MemberAccessExpressionSyntax or MemberBindingExpressionSyntax)
            .Cast<ExpressionSyntax>()
            .GroupBy(Top);
        foreach (IGrouping<ExpressionSyntax, ExpressionSyntax> chain in chains)
        {
            ExpressionSyntax top = chain.Key;
            if (best is { } current && top.Span.Length <= current.span.Length) continue;
            if (StyleLayout.IsInInterpolation(top)) continue;
            ExpressionSyntax[] links = chain.OrderBy(static link => Dot(link).SpanStart).ToArray();
            int calls = links.Count(IsCall);
            if (IsCall(links[^1]) && Name(links[^1]) == "ConfigureAwait") calls--;
            if (calls < 2) continue;
            int indent = source.Indentation(top.SpanStart) + StyleLayout.C_INDENT;
            string lineBreak = "\n" + new string(' ', indent);
            int delta = indent - source.Indentation(line.Start);
            bool isFirstCall = true;
            List<StyleEdit> edits = [];
            foreach (ExpressionSyntax link in links)
            {
                if (!IsCall(link)) continue;
                bool keepsLine = isFirstCall && IsStaticReceiver(links[0]);
                isFirstCall = false;
                SyntaxToken lead = Lead(Dot(link));
                SyntaxToken previous = lead.GetPreviousToken();
                if (keepsLine || lead.SpanStart < line.Start || lead.SpanStart >= line.End || source.IsFirstOnLine(lead))
                {
                    continue;
                }
                if (!StyleLayout.IsBlank(source.content, previous.Span.End, lead.SpanStart)) continue;
                edits.Add(new StyleEdit(previous.Span.End, lead.SpanStart, lineBreak));

                // The lines of arguments that continue below keep their offset from the line the call now starts.
                int closeLine = source.LineNumber(link.Parent!.Span.End);
                for (int below = line.LineNumber + 1; below <= closeLine && delta != 0; below++)
                {
                    StyleEdit? shift = StyleLayout.Shift(source, below, delta, protectedSpans);
                    if (shift is not null) edits.Add(shift);
                }
            }

            if (edits.Count > 0) best = (top.Span, edits);
        }

        return best;
    }

    private static IEnumerable<(int position, string message, List<StyleEdit> edits)> Violations(StyleSource source)
    {
        IReadOnlyList<TextSpan> protectedSpans = source.ProtectedSpans();
        IEnumerable<IGrouping<ExpressionSyntax, ExpressionSyntax>> chains = source.root.DescendantNodes()
            .Where(static node => node is MemberAccessExpressionSyntax or MemberBindingExpressionSyntax)
            .Cast<ExpressionSyntax>()
            .GroupBy(Top)
            .OrderBy(static chain => chain.Key.SpanStart);
        foreach (IGrouping<ExpressionSyntax, ExpressionSyntax> chain in chains)
        {
            ExpressionSyntax top = chain.Key;
            int firstLine = source.LineNumber(top.SpanStart);
            int lastLine = source.LineNumber(top.Span.End);
            if (firstLine == lastLine) continue;
            ExpressionSyntax[] links = chain.OrderBy(static link => Dot(link).SpanStart).ToArray();
            int indent = source.Indentation(top.SpanStart) + StyleLayout.C_INDENT;
            string lineBreak = "\n" + new string(' ', indent);
            List<StyleEdit> edits = [];
            string? message = null;
            // The receiver and the first call may share the statement line; every later call starts a line.
            bool isBroken = links.Any(link => source.IsFirstOnLine(Lead(Dot(link))));
            for (int index = 0; index < links.Length; index++)
            {
                SyntaxToken dot = Dot(links[index]);
                SyntaxToken lead = Lead(dot);
                SyntaxToken previous = lead.GetPreviousToken();
                SyntaxToken next = dot.GetNextToken();
                if (source.LineNumber(dot.SpanStart) != source.LineNumber(next.SpanStart))
                {
                    // A '.' that ends its line moves in front of the member it selects.
                    string gap = source.content[dot.Span.End..next.SpanStart];
                    if (!StyleLayout.IsBlank(gap, 0, gap.Length) || source.IsFirstOnLine(lead)) continue;
                    edits.Add(new StyleEdit(previous.Span.End, next.SpanStart, lineBreak + source.content[lead.SpanStart..dot.Span.End]));
                    message ??= "Start the continuation line with '.' instead of ending the line with it.";
                    isBroken = true;
                    continue;
                }

                if (source.IsFirstOnLine(lead)) continue;
                if (!isBroken || !IsCall(links[index])) continue;
                if (index == 0 || !IsCall(links[index - 1]))
                {
                    // The first call keeps the statement line unless its own arguments are broken and calls follow it.
                    ArgumentListSyntax arguments = ((InvocationExpressionSyntax)links[index].Parent!).ArgumentList;
                    bool isSplit = source.LineNumber(arguments.OpenParenToken.SpanStart) != source.LineNumber(
                        arguments.CloseParenToken.SpanStart
                    );
                    if (!isSplit || index + 1 >= links.Length || !IsCall(links[index + 1])) continue;
                }

                if (!StyleLayout.IsBlank(source.content, previous.Span.End, lead.SpanStart)) continue;
                edits.Add(new StyleEdit(previous.Span.End, lead.SpanStart, lineBreak));
                message ??= "Put each call of a broken chain on its own line.";

                // The lines of the arguments keep their offset from the line the call now starts.
                int dotLine = source.LineNumber(dot.SpanStart);
                int delta = indent - source.Indentation(source.text.Lines[dotLine].Start);
                int closeLine = source.LineNumber(links[index].Parent!.Span.End);
                for (int line = dotLine + 1; line <= closeLine && delta != 0; line++)
                {
                    StyleEdit? shift = StyleLayout.Shift(source, line, delta, protectedSpans);
                    if (shift is not null) edits.Add(shift);
                }
            }

            if (edits.Count == 0)
            {
                int[] lines = links
                    .Select(link => Lead(Dot(link)))
                    .Where(source.IsFirstOnLine)
                    .Select(lead => source.LineNumber(lead.SpanStart))
                    .Where(line => line > firstLine)
                    .Distinct()
                    .Order()
                    .ToArray();
                for (int index = 0; index < lines.Length; index++)
                {
                    int delta = indent - source.Indentation(source.text.Lines[lines[index]].Start);
                    int end = index + 1 < lines.Length ? lines[index + 1] - 1 : lastLine;
                    for (int line = lines[index]; line <= end && delta != 0; line++)
                    {
                        StyleEdit? shift = StyleLayout.Shift(source, line, delta, protectedSpans);
                        if (shift is not null) edits.Add(shift);
                    }
                }

                if (edits.Count > 0) message = "Indent the chain four spaces from the line on which it starts.";
            }

            if (message is not null) yield return (edits.Min(static edit => edit.start), message, edits);
        }
    }

    private static string Name(ExpressionSyntax link)
        => link is MemberAccessExpressionSyntax access
           ? access.Name.Identifier.Text
           : ((MemberBindingExpressionSyntax)link).Name.Identifier.Text;

    // A type name, 'this' or 'base' in front of the first '.': the first call reads as one unit with it.
    private static bool IsStaticReceiver(ExpressionSyntax first)
    {
        if (first is not MemberAccessExpressionSyntax access) return false;
        return access.Expression switch
        {
            PredefinedTypeSyntax or ThisExpressionSyntax or BaseExpressionSyntax or GenericNameSyntax => true,
            IdentifierNameSyntax name => char.IsUpper(name.Identifier.Text[0]),
            _ => false
        };
    }

    // The outermost expression of the chain a member access belongs to.
    private static ExpressionSyntax Top(ExpressionSyntax expression)
    {
        while (true)
        {
            ExpressionSyntax? parent = expression.Parent switch
            {
                MemberAccessExpressionSyntax access when access.Expression == expression => access,
                InvocationExpressionSyntax invocation when invocation.Expression == expression => invocation,
                ElementAccessExpressionSyntax element when element.Expression == expression => element,
                ConditionalAccessExpressionSyntax conditional => conditional,
                PostfixUnaryExpressionSyntax postfix when postfix.IsKind(SyntaxKind.SuppressNullableWarningExpression) => postfix,
                _ => null
            };
            if (parent is null) return expression;
            expression = parent;
        }
    }

    private static SyntaxToken Dot(ExpressionSyntax link)
        => link is MemberAccessExpressionSyntax access ? access.OperatorToken : ((MemberBindingExpressionSyntax)link).OperatorToken;

    // The token that starts the link: the '?' of '?.', otherwise the '.' itself.
    private static SyntaxToken Lead(SyntaxToken dot)
    {
        SyntaxToken previous = dot.GetPreviousToken();
        bool isConditional = previous.IsKind(SyntaxKind.QuestionToken)
                             && previous.Parent is ConditionalAccessExpressionSyntax
                             && previous.Span.End == dot.SpanStart;
        return isConditional ? previous : dot;
    }

    private static bool IsCall(ExpressionSyntax link)
        => link.Parent is InvocationExpressionSyntax invocation && invocation.Expression == link;
}
