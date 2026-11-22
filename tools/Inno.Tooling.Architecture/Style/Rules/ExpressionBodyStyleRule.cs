using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

// M-02: a method, operator, local function or accessor whose body is one expression uses an expression body,
// and a constructor always uses a block body.
internal sealed class ExpressionBodyStyleRule : StyleRule
{
    private const string C_ID = "M-02";

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
                case ConstructorDeclarationSyntax { ExpressionBody: not null } constructor:
                    findings.Add(source.Finding(C_ID, constructor.Identifier.SpanStart, "Give the constructor a block body."));
                    break;
                case MethodDeclarationSyntax
                     or OperatorDeclarationSyntax
                     or ConversionOperatorDeclarationSyntax
                     or LocalFunctionStatementSyntax
                     or AccessorDeclarationSyntax:
                    BlockSyntax? body = node switch
                    {
                        BaseMethodDeclarationSyntax method => method.Body,
                        LocalFunctionStatementSyntax local => local.Body,
                        AccessorDeclarationSyntax accessor => accessor.Body,
                        _ => null
                    };
                    if (body is not null && IsOneExpression(body))
                    {
                        findings.Add(
                            source.Finding(C_ID, body.OpenBraceToken.SpanStart, "Write the single expression as an expression body.")
                        );
                    }

                    break;
            }
        }
    }

    // A comment or a directive inside the body documents a statement, so the block stays.
    private static bool IsOneExpression(BlockSyntax body)
        => body.Statements.Count == 1
           && body.Statements[0] is ReturnStatementSyntax { Expression: not null } or ExpressionStatementSyntax or ThrowStatementSyntax
           && !body.DescendantTrivia().Any(static trivia => trivia.IsKind(SyntaxKind.SingleLineCommentTrivia) || trivia.IsDirective);
}
