using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

// B-07: an empty type ends with a semicolon and has no braces.
internal sealed class EmptyTypeStyleRule : StyleRule
{
    private const string C_ID = "B-07";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Rewrite;

    internal override bool isFixable => true;

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
        List<StyleEdit> edits = [];
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
            edits.Add(new StyleEdit(previous.Span.End, type.CloseBraceToken.Span.End, ";"));
        }

        return edits;
    }
}
