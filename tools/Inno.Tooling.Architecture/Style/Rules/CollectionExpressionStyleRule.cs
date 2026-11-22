using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

// I-02: an array or a `List<T>` is written as a collection expression where the declaration gives it a target type.
// A creation without a target type (a call receiver, an inferred generic argument, an anonymous-type member)
// has no collection expression form and is not reported.
internal sealed class CollectionExpressionStyleRule : StyleRule
{
    private const string C_ID = "I-02";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Rewrite;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach (EqualsValueClauseSyntax clause in source.root.DescendantNodes().OfType<EqualsValueClauseSyntax>())
        {
            TypeSyntax? declared = clause.Parent switch
            {
                VariableDeclaratorSyntax { Parent: VariableDeclarationSyntax declaration } => declaration.Type,
                PropertyDeclarationSyntax property => property.Type,
                _ => null
            };
            if (declared is null) continue;
            bool isLocal = clause.Parent is VariableDeclaratorSyntax { Parent.Parent: LocalDeclarationStatementSyntax };
            if (IsList(declared) || isLocal && declared.IsVar)
            {
                if (IsPlainListCreation(clause.Value) && (IsList(declared) || clause.Value is ObjectCreationExpressionSyntax))
                {
                    findings.Add(
                        source.Finding(C_ID, clause.Value.SpanStart, "Write the list as a collection expression: `List<T> x = [...]`.")
                    );
                }
            }
            else if (declared is ArrayTypeSyntax && IsArrayCreation(clause.Value))
            {
                findings.Add(source.Finding(C_ID, clause.Value.SpanStart, "Write the array as a collection expression: `T[] x = [...]`."));
            }
        }
    }

    private static bool IsList(TypeSyntax type)
        => type is GenericNameSyntax { Identifier.ValueText: "List", TypeArgumentList.Arguments.Count: 1 };

    // A constructor argument (a capacity or a source) keeps the constructor call.
    private static bool IsPlainListCreation(ExpressionSyntax value)
        => value is BaseObjectCreationExpressionSyntax creation
           && (creation is ImplicitObjectCreationExpressionSyntax || creation is ObjectCreationExpressionSyntax typed && IsList(typed.Type))
           && creation.ArgumentList is null or { Arguments.Count: 0 }
           && creation.Initializer is null or { RawKind: (int)SyntaxKind.CollectionInitializerExpression };

    private static bool IsArrayCreation(ExpressionSyntax value)
        => value is ImplicitArrayCreationExpressionSyntax
           || value is ArrayCreationExpressionSyntax { Initializer: not null }
           || value is InvocationExpressionSyntax
           {
               ArgumentList.Arguments.Count: 0,
               Expression: MemberAccessExpressionSyntax
               {
                   Expression: IdentifierNameSyntax { Identifier.ValueText: "Array" },
                   Name: GenericNameSyntax { Identifier.ValueText: "Empty" }
               }
           };
}
