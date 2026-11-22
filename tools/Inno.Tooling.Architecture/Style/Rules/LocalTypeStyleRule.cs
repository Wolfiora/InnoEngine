using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

// E-01: a local variable states its type, and uses 'var' exactly when its value is 'new T(...)' or 'new T[...]'.
internal sealed class LocalTypeStyleRule : StyleRule
{
    private const string C_ID = "E-01";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Rewrite;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach ((VariableDeclarationSyntax declaration, string message, string? _) in Violations(source.root))
        {
            findings.Add(source.Finding(C_ID, declaration.SpanStart, message));
        }
    }

    internal override IReadOnlyList<StyleEdit> Fix(StyleSource source)
    {
        List<StyleEdit> edits = [];
        foreach ((VariableDeclarationSyntax declaration, string _, string? created) in Violations(source.root))
        {
            if (created is null) continue;
            edits.Add(new StyleEdit(declaration.Type.SpanStart, declaration.Type.Span.End, "var"));
            if (declaration.Variables[0].Initializer!.Value is ImplicitObjectCreationExpressionSyntax creation)
            {
                edits.Add(new StyleEdit(creation.NewKeyword.Span.End, creation.NewKeyword.Span.End, " " + created));
            }
        }

        return edits;
    }

    private static IEnumerable<(VariableDeclarationSyntax declaration, string message, string? created)> Violations(SyntaxNode root)
    {
        foreach (SyntaxNode node in root.DescendantNodes())
        {
            if (node is not VariableDeclarationSyntax { Variables.Count: 1 } declaration) continue;
            if (declaration.Parent is not (LocalDeclarationStatementSyntax or UsingStatementSyntax or ForStatementSyntax))
            {
                continue;
            }
            if (declaration.Parent is LocalDeclarationStatementSyntax local && local.Modifiers.Any(SyntaxKind.ConstKeyword))
            {
                continue;
            }
            ExpressionSyntax? value = declaration.Variables[0].Initializer?.Value;
            if (value is null) continue;
            if (declaration.Type.IsVar)
            {
                // A value built from an anonymous type has no name to state.
                if (value is not (ObjectCreationExpressionSyntax or ArrayCreationExpressionSyntax)
                    && !value.DescendantNodesAndSelf().Any(static part => part is AnonymousObjectCreationExpressionSyntax)
                ) {
                    yield return (declaration, "State the type of the local variable instead of 'var'.", null);
                }

                continue;
            }

            // A nullable or pointer declaration may differ from the created type, so it keeps its written type.
            if (declaration.Type is NullableTypeSyntax or PointerTypeSyntax or RefTypeSyntax or ScopedTypeSyntax)
            {
                continue;
            }
            string written = Compact(declaration.Type);
            switch (value)
            {
                case ImplicitObjectCreationExpressionSyntax:
                    yield return (declaration, "Write 'var' with 'new T(...)' for a local variable.", declaration.Type.ToString());
                    break;
                case ObjectCreationExpressionSyntax creation when Compact(creation.Type) == written:
                    yield return (declaration, "Write 'var' when the value is 'new T(...)'.", written);
                    break;
                case ArrayCreationExpressionSyntax { Initializer: null } array
                    when declaration.Type is ArrayTypeSyntax arrayType
                         && Compact(array.Type.ElementType) == Compact(arrayType.ElementType)
                         && array.Type.RankSpecifiers.Count == arrayType.RankSpecifiers.Count:
                    yield return (declaration, "Write 'var' when the value is 'new T[...]'.", written);
                    break;
            }
        }
    }

    private static string Compact(SyntaxNode node)
        => string.Concat(node.ToString().Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));
}
