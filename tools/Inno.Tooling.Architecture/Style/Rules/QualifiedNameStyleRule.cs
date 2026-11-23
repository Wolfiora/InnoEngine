using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

// N-05: code refers to a type by its short name instead of a namespace-qualified name.
// N-06: an alias is PascalCase, differs from the short name of its target and is the only alias of that target.
// The check reads syntax only, so it recognizes a qualified name by its root namespace.
internal sealed class QualifiedNameStyleRule : StyleRule
{
    private const string C_ID = "N-05";
    private const string C_ALIAS_ID = "N-06";

    private static readonly string[] ROOT_NAMESPACES =
    [
        "Inno",
        "Microsoft",
        "System",
        "Xunit"
    ];

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Rewrite;

    internal override bool Covers(string ruleID)
        => string.Equals(ruleID, C_ID, StringComparison.Ordinal) || string.Equals(ruleID, C_ALIAS_ID, StringComparison.Ordinal);

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        HashSet<string> aliases = source.root
            .DescendantNodes()
            .OfType<UsingDirectiveSyntax>()
            .Where(static directive => directive.Alias is not null)
            .Select(static directive => directive.Alias!.Name.Identifier.ValueText)
            .ToHashSet(StringComparer.Ordinal);
        foreach (SyntaxNode node in source.root.DescendantNodes())
        {
            if (node is UsingDirectiveSyntax directive)
            {
                CheckAlias(source, findings, directive);
            }
            else if (IsQualifiedUse(node, aliases))
            {
                findings.Add(
                    source.Finding(C_ID, node.SpanStart, "Import the namespace and use the short name. Found `" + node + "`.")
                );
            }
        }
    }

    private static void CheckAlias(
        StyleSource source,
        ICollection<StyleFinding> findings,
        UsingDirectiveSyntax directive
    ) {
        if (directive.Alias is null)
        {
            return;
        }

        string alias = directive.Alias.Name.Identifier.ValueText;
        string target = ShortName(directive.NamespaceOrType);
        if (!char.IsUpper(alias[0]) || alias.Contains('_', StringComparison.Ordinal))
        {
            findings.Add(source.Finding(C_ALIAS_ID, directive.Alias.SpanStart, "Name an alias in PascalCase. Found `" + alias + "`."));
        }
        else if (string.Equals(alias, target, StringComparison.Ordinal))
        {
            findings.Add(
                source.Finding(
                    C_ALIAS_ID,
                    directive.Alias.SpanStart,
                    "Name an alias after the role of its target, not after the target. Found `" + alias + "`."
                )
            );
        }
        else if (IsRepeatedTarget(directive))
        {
            findings.Add(
                source.Finding(
                    C_ALIAS_ID,
                    directive.Alias.SpanStart,
                    "Declare one alias per target. `" + alias + "` repeats the target of an earlier alias."
                )
            );
        }
    }

    private static bool IsRepeatedTarget(UsingDirectiveSyntax directive)
    {
        if (directive.Parent is null)
        {
            return false;
        }

        string target = directive.NamespaceOrType.ToString();
        return directive.Parent
            .ChildNodes()
            .OfType<UsingDirectiveSyntax>()
            .TakeWhile(other => other != directive)
            .Any(other => other.Alias is not null && string.Equals(other.NamespaceOrType.ToString(), target, StringComparison.Ordinal));
    }

    private static string ShortName(TypeSyntax type)
        => type switch
    {
        QualifiedNameSyntax qualified => qualified.Right.Identifier.ValueText,
        AliasQualifiedNameSyntax qualified => qualified.Name.Identifier.ValueText,
        SimpleNameSyntax simple => simple.Identifier.ValueText,
        _ => string.Empty
    };

    // The outermost dotted name whose leftmost identifier is a root namespace, outside using directives and namespace declarations.
    private static bool IsQualifiedUse(
        SyntaxNode node,
        HashSet<string> aliases
    ) {
        if (
            node is not (QualifiedNameSyntax or MemberAccessExpressionSyntax)
            || node.Parent is QualifiedNameSyntax
            || (node.Parent is MemberAccessExpressionSyntax parent && parent.Expression == node)
        ) {
            return false;
        }

        SyntaxNode current = node;
        while (true)
        {
            switch (current)
            {
                case QualifiedNameSyntax qualified:
                    current = qualified.Left;
                    continue;
                case MemberAccessExpressionSyntax access when access.IsKind(SyntaxKind.SimpleMemberAccessExpression):
                    current = access.Expression;
                    continue;
            }

            break;
        }

        return (
            current is IdentifierNameSyntax root
            && current != node
            && ROOT_NAMESPACES.Contains(root.Identifier.ValueText, StringComparer.Ordinal)
            && !aliases.Contains(root.Identifier.ValueText)
            && !node.Ancestors().Any(static ancestor => ancestor is UsingDirectiveSyntax or BaseNamespaceDeclarationSyntax)
        );
    }
}
