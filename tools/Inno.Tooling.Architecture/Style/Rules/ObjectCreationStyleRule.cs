using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

// E-06: an argument, or a creation inside a larger expression, writes the type: `new T(...)`.
// A field or property initializer, an assignment and a directly returned object keep the target-typed form.
internal sealed class ObjectCreationStyleRule : StyleRule
{
    private const string C_ID = "E-06";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Rewrite;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach (ImplicitObjectCreationExpressionSyntax creation in source.root
            .DescendantNodes()
            .OfType<ImplicitObjectCreationExpressionSyntax>()
        ) {
            if (IsLocalInitializer(creation))
            {
                findings.Add(source.Finding(C_ID, creation.SpanStart, "Declare the local variable as `var x = new T(...)`."));
            }
            else if (IsInsideExpression(creation))
            {
                findings.Add(source.Finding(C_ID, creation.SpanStart, "Write the type of the created object: `new T(...)`."));
            }
        }
    }

    private static bool IsLocalInitializer(ImplicitObjectCreationExpressionSyntax creation)
        => creation.Parent is EqualsValueClauseSyntax
        {
            Parent: VariableDeclaratorSyntax
            {
                Parent: VariableDeclarationSyntax
                {
                    Parent: LocalDeclarationStatementSyntax
                }
            }
        };

    private static bool IsInsideExpression(ImplicitObjectCreationExpressionSyntax creation)
        => creation.Parent is ArgumentSyntax { Parent: not TupleExpressionSyntax }
           or ExpressionElementSyntax
           or ConditionalExpressionSyntax
           or SwitchExpressionArmSyntax
           or YieldStatementSyntax
           or BinaryExpressionSyntax
           or SimpleLambdaExpressionSyntax
           or ParenthesizedLambdaExpressionSyntax
           || creation.Parent is InitializerExpressionSyntax initializer
              && initializer.Kind() is SyntaxKind.CollectionInitializerExpression or SyntaxKind.ArrayInitializerExpression;
}
