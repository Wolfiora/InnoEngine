using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

// A delimited list with its opening and closing tokens and its items in source order.
internal sealed record StyleList(
    SyntaxToken open,
    SyntaxToken close,
    IReadOnlyList<SyntaxNode> items
) {
    // The argument list of a call, an object creation, a constructor initializer or an attribute, when it has arguments.
    internal static StyleList? Arguments(SyntaxNode node)
    {
        StyleList? list = node switch
        {
            ArgumentListSyntax arguments => new(
                arguments.OpenParenToken,
                arguments.CloseParenToken,
                arguments.Arguments.ToArray<SyntaxNode>()
            ),
            AttributeArgumentListSyntax arguments => new(
                arguments.OpenParenToken,
                arguments.CloseParenToken,
                arguments.Arguments.ToArray<SyntaxNode>()
            ),
            _ => null
        };
        return list is null || list.items.Count == 0 || list.open.IsMissing || list.close.IsMissing ? null : list;
    }

    // The brace or bracket list of an initializer, an anonymous object, a switch expression or a collection
    // expression, when it has items.
    internal static StyleList? Braces(SyntaxNode node)
    {
        StyleList? list = node switch
        {
            InitializerExpressionSyntax initializer when !initializer.IsKind(SyntaxKind.ComplexElementInitializerExpression) => new(
                initializer.OpenBraceToken,
                initializer.CloseBraceToken,
                initializer.Expressions.ToArray<SyntaxNode>()
            ),
            AnonymousObjectCreationExpressionSyntax anonymous => new(
                anonymous.OpenBraceToken,
                anonymous.CloseBraceToken,
                anonymous.Initializers.ToArray<SyntaxNode>()
            ),
            SwitchExpressionSyntax expression => new(
                expression.OpenBraceToken,
                expression.CloseBraceToken,
                expression.Arms.ToArray<SyntaxNode>()
            ),
            CollectionExpressionSyntax collection => new(
                collection.OpenBracketToken,
                collection.CloseBracketToken,
                collection.Elements.ToArray<SyntaxNode>()
            ),
            _ => null
        };
        return list is null || list.items.Count == 0 || list.open.IsMissing || list.close.IsMissing ? null : list;
    }
}
