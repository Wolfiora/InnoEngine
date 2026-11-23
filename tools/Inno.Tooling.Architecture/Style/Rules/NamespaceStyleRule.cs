using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

// N-01: namespaces are file-scoped.
internal sealed class NamespaceStyleRule : StyleRule
{
    private const string C_ID = "N-01";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Layout;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach (NamespaceDeclarationSyntax block in source.root.DescendantNodes().OfType<NamespaceDeclarationSyntax>())
        {
            findings.Add(source.Finding(C_ID, block.SpanStart, "Use a file-scoped namespace."));
        }
    }
}
