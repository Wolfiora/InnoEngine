using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

// N-03: there are no global usings, no `global::` and no usings inside a namespace.
internal sealed class UsingPlacementStyleRule : StyleRule
{
    private const string C_ID = "N-03";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Layout;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach (SyntaxNode node in source.root.DescendantNodes())
        {
            switch (node)
            {
                case UsingDirectiveSyntax directive when directive.GlobalKeyword.IsKind(SyntaxKind.GlobalKeyword):
                    findings.Add(source.Finding(C_ID, directive.SpanStart, "Do not use global using directives."));
                    break;
                case UsingDirectiveSyntax directive when directive.Parent is BaseNamespaceDeclarationSyntax:
                    findings.Add(source.Finding(C_ID, directive.SpanStart, "Place using directives at the top of the file."));
                    break;
                case AliasQualifiedNameSyntax alias when alias.Alias.Identifier.IsKind(SyntaxKind.GlobalKeyword):
                    findings.Add(source.Finding(C_ID, alias.SpanStart, "Do not write `global::`."));
                    break;
            }
        }
    }
}
