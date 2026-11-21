using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

// B-01, B-08 and B-09: the braces of a multi-line body are on their own lines, nothing shares the line of
// an opening brace, and 'else', 'catch', 'finally' and the 'while' of a 'do' loop start a new line.
internal sealed class BraceStyleRule : StyleRule
{
    private const string C_ID = "B-01";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Layout;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach ((int position, string _, string message) in Violations(source))
        {
            findings.Add(source.Finding(C_ID, position, message));
        }
    }

    internal override IReadOnlyList<StyleEdit> Fix(StyleSource source)
    {
        var edits = new List<StyleEdit>();
        foreach ((int position, string indent, string _) in Violations(source))
        {
            int start = position;
            while (start > 0 && source.content[start - 1] == ' ')
            {
                start--;
            }

            edits.Add(new(start, position, "\n" + indent));
        }

        return edits;
    }

    // Each violation is a token that must start a new line, with the indentation that line takes.
    private static IEnumerable<(int position, string indent, string message)> Violations(StyleSource source)
    {
        foreach (SyntaxNode node in source.root.DescendantNodes())
        {
            (SyntaxToken open, SyntaxToken close)? braces = Braces(node);
            if (braces is { } pair && !pair.open.IsMissing && !pair.close.IsMissing
                && source.LineNumber(pair.open.SpanStart) != source.LineNumber(pair.close.SpanStart)
            ) {
                SyntaxToken previous = pair.open.GetPreviousToken();
                int indentation = source.IsFirstOnLine(pair.open)
                                  ? source.Column(pair.open.SpanStart)
                                  : source.Indentation(previous.SpanStart);
                if (!source.IsFirstOnLine(pair.open) && !StyleLayout.ClosesMultiLineHeader(source, previous))
                {
                    yield return (pair.open.SpanStart, new(' ', indentation), "Put the opening brace on its own line.");
                }

                SyntaxToken next = pair.open.GetNextToken();
                if (source.LineNumber(next.SpanStart) == source.LineNumber(pair.open.SpanStart))
                {
                    yield return (next.SpanStart, new(
                        ' ',
                        indentation + StyleLayout.C_INDENT
                    ), "Nothing follows an opening brace on its line.");
                }

                if (!source.IsFirstOnLine(pair.close))
                {
                    yield return (pair.close.SpanStart, new(' ', indentation), "Put the closing brace on its own line.");
                }
            }

            SyntaxToken keyword = node switch
            {
                ElseClauseSyntax clause => clause.ElseKeyword,
                CatchClauseSyntax clause => clause.CatchKeyword,
                FinallyClauseSyntax clause => clause.FinallyKeyword,
                DoStatementSyntax loop => loop.WhileKeyword,
                _ => default
            };
            if (keyword.IsKind(SyntaxKind.None) || keyword.IsMissing || source.IsFirstOnLine(keyword)) continue;
            SyntaxToken before = keyword.GetPreviousToken();
            if (!before.IsKind(SyntaxKind.CloseBraceToken)) continue;
            yield return (keyword.SpanStart, new(' ', source.Indentation(before.SpanStart)), $"Start a new line for '{keyword.Text}'.");
        }
    }

    private static (SyntaxToken open, SyntaxToken close)? Braces(SyntaxNode node)
        => node switch
    {
        BlockSyntax block => (block.OpenBraceToken, block.CloseBraceToken),
        BaseTypeDeclarationSyntax type when !type.OpenBraceToken.IsKind(SyntaxKind.None) => (type.OpenBraceToken, type.CloseBraceToken),
        NamespaceDeclarationSyntax scope => (scope.OpenBraceToken, scope.CloseBraceToken),
        SwitchStatementSyntax statement => (statement.OpenBraceToken, statement.CloseBraceToken),
        SwitchExpressionSyntax expression => (expression.OpenBraceToken, expression.CloseBraceToken),
        AccessorListSyntax accessors => (accessors.OpenBraceToken, accessors.CloseBraceToken),
        _ => null
    };
}
