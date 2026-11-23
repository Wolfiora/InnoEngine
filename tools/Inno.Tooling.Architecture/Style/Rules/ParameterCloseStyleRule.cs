using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Inno.Tooling.Architecture;

// P-02: the body, expression body, constructor initializer, base list or 'where' clause that follows the
// closing parenthesis of a multi-line parameter list starts on the line of that parenthesis.
internal sealed class ParameterCloseStyleRule : StyleRule
{
    private const string C_ID = "P-02";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Layout;

    internal override bool isFixable => true;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach ((SyntaxToken close, SyntaxToken next, string _) in Violations(source))
        {
            findings.Add(
                source.Finding(C_ID, close.SpanStart, $"Write '{next.Text}' on the line of the closing parenthesis of the parameter list.")
            );
        }
    }

    internal override IReadOnlyList<StyleEdit> Fix(StyleSource source)
    {
        IReadOnlyList<TextSpan> protectedSpans = source.ProtectedSpans();
        List<StyleEdit> edits = [];
        foreach ((SyntaxToken close, SyntaxToken next, string gap) in Violations(source))
        {
            edits.Add(new StyleEdit(close.Span.End, next.SpanStart, gap));
            // What moved up keeps its own continuation lines relative to the declaration.
            int delta = source.Indentation(close.SpanStart) - source.Indentation(next.SpanStart);
            if (delta == 0 || next.IsKind(SyntaxKind.OpenBraceToken) || next.IsKind(SyntaxKind.SemicolonToken))
            {
                continue;
            }
            SyntaxNode moved = next.Parent!;
            int first = source.LineNumber(next.SpanStart) + 1;
            int last = source.LineNumber(moved.Span.End);
            for (int line = first; line <= last; line++)
            {
                StyleEdit? shift = StyleLayout.Shift(source, line, delta, protectedSpans);
                if (shift is not null) edits.Add(shift);
            }
        }

        return edits;
    }

    private static IEnumerable<(SyntaxToken close, SyntaxToken next, string gap)> Violations(StyleSource source)
    {
        foreach (SyntaxNode node in source.root.DescendantNodes())
        {
            if (node is not ParameterListSyntax parameters || parameters.CloseParenToken.IsMissing) continue;
            SyntaxToken close = parameters.CloseParenToken;
            if (source.LineNumber(parameters.OpenParenToken.SpanStart) == source.LineNumber(close.SpanStart)) continue;
            if (!source.IsFirstOnLine(close)) continue;
            SyntaxToken next = close.GetNextToken();
            string? gap = Gap(parameters, next);
            if (gap is null) continue;
            string actual = source.content[close.Span.End..next.SpanStart];
            if (actual == gap || !StyleLayout.IsBlank(actual, 0, actual.Length)) continue;
            yield return (close, next, gap);
        }
    }

    // The text required between the closing parenthesis and the token that follows it, or null when this rule does not own it.
    private static string? Gap(
        ParameterListSyntax parameters,
        SyntaxToken next
    ) {
        SyntaxNode owner = parameters.Parent!;
        switch (next.Kind())
        {
            case SyntaxKind.SemicolonToken:
                return string.Empty;
            case SyntaxKind.EqualsGreaterThanToken:
            case SyntaxKind.WhereKeyword:
                return " ";
            case SyntaxKind.ColonToken:
                return next.Parent is ConstructorInitializerSyntax or BaseListSyntax ? " " : null;
            case SyntaxKind.OpenBraceToken:
                // A lambda block keeps the brace on its own line (B-04).
                return owner is BaseMethodDeclarationSyntax or LocalFunctionStatementSyntax or TypeDeclarationSyntax ? " " : null;
            default:
                return null;
        }
    }
}
