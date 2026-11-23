using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Inno.Tooling.Architecture;

// A-02 and A-03: a broken argument list puts the opening parenthesis at the end of its line, each argument
// on its own line indented four spaces, and the closing parenthesis on its own line. One multi-line lambda,
// initializer, anonymous object, collection expression or multi-line string may instead hug the parentheses.
internal sealed class ArgumentListStyleRule : StyleRule
{
    private const string C_ID = "A-02";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Layout;

    internal override bool isFixable => true;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach (StyleList list in Violations(source))
        {
            findings.Add(
                source.Finding(C_ID, list.open.SpanStart, "Expand the broken argument list: one argument per line and ')' on its own line.")
            );
        }
    }

    internal override IReadOnlyList<StyleEdit> Fix(StyleSource source)
    {
        IReadOnlyList<TextSpan> protectedSpans = source.ProtectedSpans();
        List<StyleEdit> edits = [];
        List<TextSpan> fixedSpans = [];
        foreach (StyleList list in Violations(source).OrderBy(static list => list.open.SpanStart))
        {
            // Nested lists are fixed in a later round, after their container has settled.
            TextSpan span = TextSpan.FromBounds(list.open.SpanStart, list.close.Span.End);
            if (fixedSpans.Any(outer => outer.Contains(span))) continue;
            List<StyleEdit>? listEdits = StyleLayout.Expand(source, list, protectedSpans);
            if (listEdits is null) continue;
            edits.AddRange(listEdits);
            fixedSpans.Add(span);
        }

        return edits;
    }

    private static IEnumerable<StyleList> Violations(StyleSource source)
    {
        foreach (SyntaxNode node in source.root.DescendantNodes())
        {
            StyleList? list = StyleList.Arguments(node);
            if (list is null) continue;
            if (source.LineNumber(list.open.SpanStart) == source.LineNumber(list.close.SpanStart)) continue;
            if (IsHugged(source, list) || StyleLayout.IsExpanded(source, list)) continue;
            yield return list;
        }
    }

    private static bool IsHugged(
        StyleSource source,
        StyleList list
    ) {
        int openLine = source.LineNumber(list.open.SpanStart);
        SyntaxNode? multiLine = null;
        foreach (SyntaxNode argument in list.items)
        {
            if (source.LineNumber(argument.SpanStart) == source.LineNumber(argument.Span.End)) continue;
            if (multiLine is not null) return false;
            multiLine = argument;
        }

        if (multiLine is null || !CanHug(multiLine)) return false;
        // Arguments before the multi-line one share the opening line; the rest follow its closing delimiter.
        int endLine = source.LineNumber(multiLine.Span.End);
        foreach (SyntaxNode argument in list.items)
        {
            int line = source.LineNumber(argument.SpanStart);
            if (argument.SpanStart <= multiLine.SpanStart ? line != openLine : line != endLine) return false;
        }

        return source.LineNumber(list.close.SpanStart) == endLine;
    }

    private static bool CanHug(SyntaxNode argument)
    {
        ExpressionSyntax? expression = argument switch
        {
            ArgumentSyntax value => value.Expression,
            AttributeArgumentSyntax value => value.Expression,
            _ => null
        };
        return expression switch
        {
            LambdaExpressionSyntax or AnonymousMethodExpressionSyntax => true,
            AnonymousObjectCreationExpressionSyntax or CollectionExpressionSyntax => true,
            ObjectCreationExpressionSyntax { Initializer: not null } => true,
            ImplicitObjectCreationExpressionSyntax { Initializer: not null } => true,
            ArrayCreationExpressionSyntax { Initializer: not null } or ImplicitArrayCreationExpressionSyntax => true,
            LiteralExpressionSyntax literal => literal.IsKind(SyntaxKind.StringLiteralExpression) || literal.IsKind(
                SyntaxKind.Utf8StringLiteralExpression
            ),
            InterpolatedStringExpressionSyntax => true,
            _ => false
        };
    }
}
