using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

// The operator chains whose continuation lines begin with an operator and align with the first operand.
internal static class StyleOperators
{
    // Each chain is one run of operators of the same precedence, with the token its continuation lines align with.
    // An operand of another precedence is a chain of its own, so breaking 'a == b && c' never separates 'a == b'.
    internal static IEnumerable<(SyntaxNode chain, SyntaxToken anchor, IReadOnlyList<SyntaxToken> operators)> Chains(SyntaxNode root)
    {
        foreach (SyntaxNode node in root.DescendantNodes())
        {
            List<SyntaxToken> operators = [];
            switch (node)
            {
                case BinaryExpressionSyntax binary when IsChained(binary) && !IsSameTier(binary, node.Parent):
                    Collect(binary, Tier(binary.Kind()), operators);
                    yield return (node, node.GetFirstToken(), operators);
                    break;
                case BinaryPatternSyntax pattern when !(node.Parent is BinaryPatternSyntax parent && parent.Kind() == pattern.Kind()):
                    Collect(pattern, operators);
                    SyntaxNode owner = node.Parent is IsPatternExpressionSyntax ? node.Parent : node;
                    yield return (node, owner.GetFirstToken(), operators);
                    break;
                case ConditionalExpressionSyntax conditional:
                    operators.Add(conditional.QuestionToken);
                    operators.Add(conditional.ColonToken);
                    yield return (node, node.GetFirstToken(), operators);
                    break;
            }
        }
    }

    // True for a chain of comparisons, whose operands stay together while a looser construct can break.
    internal static bool IsComparison(SyntaxNode chain) => chain is BinaryExpressionSyntax binary && Tier(binary.Kind()) is 6 or 7;

    // 'is' and 'as' stay on the line of their left operand and do not take part in a chain.
    private static bool IsChained(BinaryExpressionSyntax binary)
        => !binary.IsKind(SyntaxKind.IsExpression) && !binary.IsKind(SyntaxKind.AsExpression);

    private static bool IsSameTier(
        BinaryExpressionSyntax binary,
        SyntaxNode? parent
    ) => parent is BinaryExpressionSyntax outer && IsChained(outer) && Tier(outer.Kind()) == Tier(binary.Kind());

    private static int Tier(SyntaxKind kind)
        => kind switch
    {
        SyntaxKind.CoalesceExpression => 0,
        SyntaxKind.LogicalOrExpression => 1,
        SyntaxKind.LogicalAndExpression => 2,
        SyntaxKind.BitwiseOrExpression => 3,
        SyntaxKind.ExclusiveOrExpression => 4,
        SyntaxKind.BitwiseAndExpression => 5,
        SyntaxKind.EqualsExpression or SyntaxKind.NotEqualsExpression => 6,
        SyntaxKind.LessThanExpression
        or SyntaxKind.LessThanOrEqualExpression
        or SyntaxKind.GreaterThanExpression
        or SyntaxKind.GreaterThanOrEqualExpression => 7,
        SyntaxKind.LeftShiftExpression
        or SyntaxKind.RightShiftExpression
        or SyntaxKind.UnsignedRightShiftExpression => 8,
        SyntaxKind.AddExpression or SyntaxKind.SubtractExpression => 9,
        _ => 10
    };

    private static void Collect(
        BinaryPatternSyntax pattern,
        List<SyntaxToken> operators
    ) {
        if (pattern.Left is BinaryPatternSyntax left && left.Kind() == pattern.Kind()) Collect(left, operators);
        operators.Add(pattern.OperatorToken);
        if (pattern.Right is BinaryPatternSyntax right && right.Kind() == pattern.Kind()) Collect(right, operators);
    }

    private static void Collect(
        BinaryExpressionSyntax binary,
        int tier,
        List<SyntaxToken> operators
    ) {
        if (binary.Left is BinaryExpressionSyntax left && IsChained(left) && Tier(left.Kind()) == tier)
        {
            Collect(left, tier, operators);
        }
        operators.Add(binary.OperatorToken);
        if (binary.Right is BinaryExpressionSyntax right && IsChained(right) && Tier(right.Kind()) == tier)
        {
            Collect(right, tier, operators);
        }
    }
}
