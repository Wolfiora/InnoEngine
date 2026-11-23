using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

// E-04: a negated null test is written `is not null`, not `!(value is null)`.
// A comparison with `==` or `!=` is not reported: whether a type overloads equality is not visible in syntax.
internal sealed class NullPatternStyleRule : StyleRule
{
    private const string C_ID = "E-04";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Rewrite;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach (PrefixUnaryExpressionSyntax negation in source.root.DescendantNodes().OfType<PrefixUnaryExpressionSyntax>())
        {
            if (IsNegatedNullTest(negation))
            {
                findings.Add(source.Finding(C_ID, negation.SpanStart, "Write `is not null` instead of negating `is null`."));
            }
        }
    }

    private static bool IsNegatedNullTest(PrefixUnaryExpressionSyntax negation)
        => negation.IsKind(SyntaxKind.LogicalNotExpression)
           && negation.Operand is ParenthesizedExpressionSyntax
           {
               Expression: IsPatternExpressionSyntax
               {
                   Pattern: ConstantPatternSyntax
                   {
                       Expression: LiteralExpressionSyntax literal
                   }
               }
           }
           && literal.IsKind(SyntaxKind.NullLiteralExpression);
}
