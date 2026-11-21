using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

// M-04: a field or property initializer writes its type once, and one declaration declares one variable.
internal sealed class FieldInitializerStyleRule : StyleRule
{
    private const string C_ID = "M-04";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Rewrite;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach (ObjectCreationExpressionSyntax creation in Repeated(source.root))
        {
            findings.Add(source.Finding(C_ID, creation.SpanStart, "Write 'new(...)' when the declaration already states the type."));
        }

        foreach (SyntaxNode node in source.root.DescendantNodes())
        {
            if (node is VariableDeclarationSyntax { Variables.Count: > 1, Parent: FieldDeclarationSyntax
                                                                                  or LocalDeclarationStatementSyntax } declaration
            ) {
                findings.Add(source.Finding(C_ID, declaration.SpanStart, "Declare one variable per declaration."));
            }
        }
    }

    internal override IReadOnlyList<StyleEdit> Fix(StyleSource source)
    {
        var edits = new List<StyleEdit>();
        foreach (ObjectCreationExpressionSyntax creation in Repeated(source.root))
        {
            edits.Add(new(creation.NewKeyword.Span.End, creation.Type.Span.End, creation.ArgumentList is null ? "()" : string.Empty));
        }

        foreach (SyntaxNode node in source.root.DescendantNodes())
        {
            if (node is not VariableDeclarationSyntax { Variables.Count: > 1 } declaration) continue;
            if (declaration.Parent is not (FieldDeclarationSyntax or LocalDeclarationStatementSyntax)) continue;
            SyntaxNode statement = declaration.Parent;
            if (!source.IsFirstOnLine(statement.GetFirstToken()) || statement.ToString().Contains('\n')) continue;
            string prefix = source.content[statement.SpanStart..declaration.Type.Span.End];
            string separator = ";\n" + new string(' ', source.Indentation(statement.SpanStart)) + prefix + " ";
            for (int index = 0; index < declaration.Variables.SeparatorCount; index++)
            {
                SyntaxToken comma = declaration.Variables.GetSeparator(index);
                edits.Add(new(comma.SpanStart, declaration.Variables[index + 1].SpanStart, separator));
            }
        }

        return edits;
    }

    private static IEnumerable<ObjectCreationExpressionSyntax> Repeated(SyntaxNode root)
    {
        foreach (SyntaxNode node in root.DescendantNodes())
        {
            (TypeSyntax? type, ExpressionSyntax? value) = node switch
            {
                VariableDeclarationSyntax { Variables.Count: 1, Parent: FieldDeclarationSyntax } declaration
                    => (declaration.Type, declaration.Variables[0].Initializer?.Value),
                PropertyDeclarationSyntax property => (property.Type, property.Initializer?.Value),
                _ => (null, null)
            };
            if (type is null || value is not ObjectCreationExpressionSyntax creation) continue;
            if (Compact(type) != Compact(creation.Type)) continue;
            yield return creation;
        }
    }

    private static string Compact(SyntaxNode node)
        => string.Concat(node.ToString().Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries));
}
