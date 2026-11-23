using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

// E-05: a null argument is rejected with `ArgumentNullException.ThrowIfNull`, not with `?? throw` or a
// hand-written `if`. A position that cannot hold a statement (an expression body, an initializer) is not reported.
internal sealed class ArgumentGuardStyleRule : StyleRule
{
    private const string C_ID = "E-05";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Rewrite;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach (SyntaxNode node in source.root.DescendantNodes())
        {
            switch (node)
            {
                case BinaryExpressionSyntax coalesce when coalesce.IsKind(SyntaxKind.CoalesceExpression)
                                                          && coalesce.Left is IdentifierNameSyntax name
                                                          && IsNullArgumentThrow(coalesce.Right, name.Identifier.ValueText)
                                                          && IsParameterOfBlockBody(coalesce, name.Identifier.ValueText):
                    findings.Add(
                        source.Finding(
                            C_ID,
                            coalesce.Right.SpanStart,
                            "Validate the parameter first with `ArgumentNullException.ThrowIfNull`."
                        )
                    );
                    break;
                case IfStatementSyntax { Else: null } check when NullTested(check.Condition) is { } tested
                && SingleStatement(check.Statement) is ThrowStatementSyntax { Expression: { } thrown }
                && IsNullArgumentThrow(thrown, tested):
                    findings.Add(
                        source.Finding(
                            C_ID,
                            check.SpanStart,
                            "Write `ArgumentNullException.ThrowIfNull` instead of a hand-written null check."
                        )
                    );
                    break;
            }
        }
    }

    // `throw new ArgumentNullException(nameof(name))` as a throw expression, or the creation itself.
    private static bool IsNullArgumentThrow(
        ExpressionSyntax expression,
        string name
    ) {
        if (expression is ThrowExpressionSyntax thrown) expression = thrown.Expression;
        return expression is ObjectCreationExpressionSyntax
               {
                   Type: IdentifierNameSyntax { Identifier.ValueText: "ArgumentNullException" },
                   ArgumentList.Arguments: { Count: 1 } arguments
               }
               && arguments[0].Expression is InvocationExpressionSyntax
               {
                   Expression: IdentifierNameSyntax { Identifier.ValueText: "nameof" },
                   ArgumentList.Arguments: { Count: 1 } names
               }
               && names[0].Expression is IdentifierNameSyntax named
               && named.Identifier.ValueText == name;
    }

    private static string? NullTested(ExpressionSyntax condition)
        => condition switch
    {
        IsPatternExpressionSyntax
        {
            Expression: IdentifierNameSyntax name,
            Pattern: ConstantPatternSyntax { Expression: LiteralExpressionSyntax { RawKind: (int)SyntaxKind.NullLiteralExpression } }
        } => name.Identifier.ValueText,
        BinaryExpressionSyntax
        {
            RawKind: (int)SyntaxKind.EqualsExpression,
            Left: IdentifierNameSyntax name,
            Right: LiteralExpressionSyntax { RawKind: (int)SyntaxKind.NullLiteralExpression }
        } => name.Identifier.ValueText,
        _ => null
    };

    private static StatementSyntax SingleStatement(StatementSyntax statement)
        => statement is BlockSyntax { Statements.Count: 1 } block ? block.Statements[0] : statement;

    // The value is a parameter of a member with a block body, and the statement is not nested in control flow,
    // so a guard statement at the top of the body can replace the throw expression.
    private static bool IsParameterOfBlockBody(
        SyntaxNode node,
        string name
    ) {
        StatementSyntax? statement = node.FirstAncestorOrSelf<StatementSyntax>();
        if (statement?.Parent is not BlockSyntax { Parent: BaseMethodDeclarationSyntax or LocalFunctionStatementSyntax } body)
        {
            return false;
        }
        if (node
            .Ancestors()
            .TakeWhile(ancestor => ancestor != statement)
            .Any(static ancestor => ancestor is AnonymousFunctionExpressionSyntax)
        ) {
            return false;
        }

        ParameterListSyntax? parameters = body.Parent switch
        {
            BaseMethodDeclarationSyntax method => method.ParameterList,
            LocalFunctionStatementSyntax local => local.ParameterList,
            _ => null
        };
        return parameters is not null && parameters.Parameters.Any(parameter => parameter.Identifier.ValueText == name);
    }
}
