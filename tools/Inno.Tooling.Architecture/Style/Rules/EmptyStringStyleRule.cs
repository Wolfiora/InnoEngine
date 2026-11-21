using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

// E-07: an empty string is written 'string.Empty' wherever a constant is not required.
internal sealed class EmptyStringStyleRule : StyleRule
{
    private const string C_ID = "E-07";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Rewrite;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach (LiteralExpressionSyntax literal in EmptyStrings(source.root))
        {
            findings.Add(source.Finding(C_ID, literal.SpanStart, "Write 'string.Empty' for an empty string."));
        }
    }

    internal override IReadOnlyList<StyleEdit> Fix(StyleSource source)
        => EmptyStrings(source.root).Select(static literal => new StyleEdit(literal.SpanStart, literal.Span.End, "string.Empty")).ToArray();

    private static IEnumerable<LiteralExpressionSyntax> EmptyStrings(SyntaxNode root)
    {
        foreach (SyntaxNode node in root.DescendantNodes())
        {
            if (node is not LiteralExpressionSyntax literal || !literal.IsKind(SyntaxKind.StringLiteralExpression))
            {
                continue;
            }
            if (literal.Token.Text != "\"\"") continue;
            if (literal.Ancestors().Any(IsConstantContext)) continue;
            yield return literal;
        }
    }

    // These positions require a compile-time constant, which 'string.Empty' is not.
    private static bool IsConstantContext(SyntaxNode node)
        => node is AttributeSyntax or ParameterSyntax or PatternSyntax or CaseSwitchLabelSyntax or EnumMemberDeclarationSyntax
           || node is FieldDeclarationSyntax field && field.Modifiers.Any(SyntaxKind.ConstKeyword)
           || node is LocalDeclarationStatementSyntax local && local.Modifiers.Any(SyntaxKind.ConstKeyword);
}
