using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

// I-05: the last item of an initializer, a collection, an enum, a switch expression or an anonymous object
// is not followed by a comma.
internal sealed class TrailingCommaStyleRule : StyleRule
{
    private const string C_ID = "I-05";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Rewrite;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach (SyntaxToken comma in TrailingCommas(source.root))
        {
            findings.Add(source.Finding(C_ID, comma.SpanStart, "Remove the comma after the last item."));
        }
    }

    internal override IReadOnlyList<StyleEdit> Fix(StyleSource source)
    {
        List<StyleEdit> edits = [];
        foreach (SyntaxToken comma in TrailingCommas(source.root))
        {
            edits.Add(new StyleEdit(comma.SpanStart, comma.Span.End, string.Empty));
        }

        return edits;
    }

    private static IEnumerable<SyntaxToken> TrailingCommas(SyntaxNode root)
    {
        foreach (SyntaxNode node in root.DescendantNodes())
        {
            SyntaxToken comma = node switch
            {
                InitializerExpressionSyntax initializer => Trailing(initializer.Expressions),
                AnonymousObjectCreationExpressionSyntax anonymous => Trailing(anonymous.Initializers),
                CollectionExpressionSyntax collection => Trailing(collection.Elements),
                EnumDeclarationSyntax declaration => Trailing(declaration.Members),
                SwitchExpressionSyntax expression => Trailing(expression.Arms),
                _ => default
            };
            if (comma.Span.Length > 0) yield return comma;
        }
    }

    private static SyntaxToken Trailing<TNode>(SeparatedSyntaxList<TNode> list)
        where TNode : SyntaxNode
            => list.Count > 0 && list.SeparatorCount == list.Count ? list.GetSeparator(list.Count - 1) : default;
}
