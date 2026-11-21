using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

// B-07: an empty method, constructor, accessor, local function or lambda body is '{}' on the line where
// the header ends.
internal sealed class EmptyBodyStyleRule : StyleRule
{
    private const string C_ID = "B-07";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Layout;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach (StyleEdit edit in Fix(source))
        {
            findings.Add(source.Finding(C_ID, edit.end - 1, "Write an empty body as '{}' on the line where the header ends."));
        }
    }

    internal override IReadOnlyList<StyleEdit> Fix(StyleSource source)
    {
        var edits = new List<StyleEdit>();
        foreach (SyntaxNode node in source.root.DescendantNodes())
        {
            if (node is not BlockSyntax { Statements.Count: 0 } block || block.OpenBraceToken.IsMissing || block.CloseBraceToken.IsMissing)
            {
                continue;
            }
            if (block.Parent is not (BaseMethodDeclarationSyntax
                                     or AccessorDeclarationSyntax
                                     or LocalFunctionStatementSyntax
                                     or AnonymousFunctionExpressionSyntax)
            ) {
                continue;
            }
            SyntaxToken previous = block.OpenBraceToken.GetPreviousToken();
            int start = previous.Span.End;
            int end = block.CloseBraceToken.Span.End;
            // A comment or a directive between the header and the closing brace keeps its layout.
            if (!StyleLayout.IsBlank(source.content, start, block.OpenBraceToken.SpanStart)) continue;
            if (!StyleLayout.IsBlank(source.content, block.OpenBraceToken.Span.End, block.CloseBraceToken.SpanStart))
            {
                continue;
            }
            if (source.content[start..end] == " {}") continue;
            edits.Add(new(start, end, " {}"));
        }

        return edits;
    }
}

// B-07: an empty type ends with a semicolon and has no braces.
internal sealed class EmptyTypeStyleRule : StyleRule
{
    private const string C_ID = "B-07";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Rewrite;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach (StyleEdit edit in Fix(source))
        {
            findings.Add(source.Finding(C_ID, edit.end - 1, "End an empty type with a semicolon instead of braces."));
        }
    }

    internal override IReadOnlyList<StyleEdit> Fix(StyleSource source)
    {
        var edits = new List<StyleEdit>();
        foreach (SyntaxNode node in source.root.DescendantNodes())
        {
            if (node is not TypeDeclarationSyntax { Members.Count: 0 } type
                || type.OpenBraceToken.Span.Length == 0
                || type.CloseBraceToken.IsMissing
            ) {
                continue;
            }
            SyntaxToken previous = type.OpenBraceToken.GetPreviousToken();
            if (!StyleLayout.IsBlank(source.content, previous.Span.End, type.OpenBraceToken.SpanStart)) continue;
            if (!StyleLayout.IsBlank(source.content, type.OpenBraceToken.Span.End, type.CloseBraceToken.SpanStart))
            {
                continue;
            }
            if (type.DescendantTrivia().Any(static trivia => trivia.IsDirective)) continue;
            edits.Add(new(previous.Span.End, type.CloseBraceToken.Span.End, ";"));
        }

        return edits;
    }
}
